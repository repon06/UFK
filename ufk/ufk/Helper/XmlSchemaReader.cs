using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using System.IO;

namespace ufk.Helper
{
    class XmlSchemaReader
    {

        private readonly char[] spliter = { '|' };

        /**
         * complexTypeName - <complexType name="tBDPL">
         */
        public Dictionary<string, List<string>> getListElement(string path, string target_namespace, string complexTypeNames)
        {
            var listComplexTypeNames = complexTypeNames.Split(',').Select(val => val.Trim()).ToList();
            Dictionary<string, List<string>> listComplexType = new Dictionary<string, List<string>>();

            //path = @"D:\temp\UFK-master_\UFK-master\ufk\ufk\docs\formulars.xsd"; //"http://www.roskazna.ru/eb/domain/Inf_Pay_Doc/formular"
            //String common_xsd_path = @"D:\temp\UFK-master_\UFK-master\ufk\ufk\docs\common.xsd"; //"http://www.roskazna.ru/eb/domain/common"
            //String baseTypes_xsd_path = @"D:\temp\UFK-master_\UFK-master\ufk\ufk\docs\baseTypes.xsd";//"http://www.roskazna.ru/eb/domain/common/base"
            //String appliedTypes_xsd_path = @"D:\temp\UFK-master_\UFK-master\ufk\ufk\docs\appliedTypes.xsd"; //"http://www.roskazna.ru/eb/domain/common/applied"

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);
            schemaSet.Add(target_namespace, path);
            //schemaSet.Add("http://www.roskazna.ru/eb/domain/common", common_xsd_path);
            //schemaSet.Add("http://www.roskazna.ru/eb/domain/common/base", baseTypes_xsd_path);
            //schemaSet.Add("http://www.roskazna.ru/eb/domain/common/applied", appliedTypes_xsd_path);
            //schemaSet.Add("http://www.roskazna.ru/eb/domain/Inf_Pay_Doc/formular", path);
            schemaSet.Compile();

            // Retrieve the compiled XmlSchema object from the XmlSchemaSet
            // by iterating over the Schemas property.
            XmlSchema customerSchema = null;
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                customerSchema = schema;
            }

            // перебираю наименования шаблонов BDPLST, BDPL
            foreach (string complexTypeName in listComplexTypeNames)
                foreach (object item in customerSchema.Items)
                {
                    XmlSchemaElement schemaElement = item as XmlSchemaElement;
                    XmlSchemaComplexType complexType = item as XmlSchemaComplexType;
                    if (schemaElement != null)
                    {
                        Console.Out.WriteLine("Schema Element: {0}", schemaElement.Name);
                    }
                    else if (complexType != null)
                    {

                        if (complexType.Name == complexTypeName)
                        {
                            Console.Out.WriteLine("Complex Type: {0}", complexType.Name);

                            listComplexType.Add(complexType.Name, OutputElements(complexType.Particle));
                            if (complexType.AttributeUses.Count > 0)
                            {
                                IDictionaryEnumerator enumerator = complexType.AttributeUses.GetEnumerator();

                                while (enumerator.MoveNext())
                                {
                                    XmlSchemaAttribute attribute = (XmlSchemaAttribute)enumerator.Value;
                                    Console.Out.WriteLine("      Attribute/Type: {0}", attribute.Name);
                                }
                            }


                        }
                    }
                }
            return listComplexType;
        }

        private List<String> OutputElements(XmlSchemaParticle particle)
        {
            List<String> outList = new List<string>();
            XmlSchemaSequence sequence = particle as XmlSchemaSequence;
            XmlSchemaChoice choice = particle as XmlSchemaChoice;
            XmlSchemaAll all = particle as XmlSchemaAll;

            if (sequence != null)
            {
                Console.Out.WriteLine("  Sequence");

                for (int i = 0; i < sequence.Items.Count; i++)
                {
                    XmlSchemaElement childElement = sequence.Items[i] as XmlSchemaElement;
                    XmlSchemaSequence innerSequence = sequence.Items[i] as XmlSchemaSequence;
                    XmlSchemaChoice innerChoice = sequence.Items[i] as XmlSchemaChoice;
                    XmlSchemaAll innerAll = sequence.Items[i] as XmlSchemaAll;

                    if (childElement != null)
                    {
                        Console.Out.WriteLine("    Element/Type: {0}:{1}", childElement.Name, childElement.SchemaTypeName.Name);
                        outList.Add(childElement.Name);
                    }
                    else OutputElements(sequence.Items[i] as XmlSchemaParticle);
                }
                return outList;
            }
            else if (choice != null) // хз - надо ли?!
            {
                Console.Out.WriteLine("  Choice");
                for (int i = 0; i < choice.Items.Count; i++)
                {
                    XmlSchemaElement childElement = choice.Items[i] as XmlSchemaElement;
                    XmlSchemaSequence innerSequence = choice.Items[i] as XmlSchemaSequence;
                    XmlSchemaChoice innerChoice = choice.Items[i] as XmlSchemaChoice;
                    XmlSchemaAll innerAll = choice.Items[i] as XmlSchemaAll;

                    if (childElement != null)
                    {
                        Console.Out.WriteLine("    Element/Type: {0}:{1}", childElement.Name, childElement.SchemaTypeName.Name);
                    }
                    else OutputElements(choice.Items[i] as XmlSchemaParticle);
                }

                Console.Out.WriteLine();
            }
            else if (all != null) // хз - надо ли?!
            {
                Console.Out.WriteLine("  All");
                for (int i = 0; i < all.Items.Count; i++)
                {
                    XmlSchemaElement childElement = all.Items[i] as XmlSchemaElement;
                    XmlSchemaSequence innerSequence = all.Items[i] as XmlSchemaSequence;
                    XmlSchemaChoice innerChoice = all.Items[i] as XmlSchemaChoice;
                    XmlSchemaAll innerAll = all.Items[i] as XmlSchemaAll;

                    if (childElement != null)
                    {
                        Console.Out.WriteLine("    Element/Type: {0}:{1}", childElement.Name, childElement.SchemaTypeName.Name);
                    }
                    else OutputElements(all.Items[i] as XmlSchemaParticle);
                }
                Console.Out.WriteLine();
            }
            return null;
        }


        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }


        public List<Dictionary<string, string>> getPaymentFKValues(string fileContent, string filePath, string target_namesapce, Dictionary<string, string> fileContentRegex)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            List<Dictionary<string, string>> PAUMENTS = new List<Dictionary<string, string>>(); //{ set; get; }//генерим список из распарс ДИКШН PAUMENTS_DIC
            Dictionary<string, List<string>> tmpl_xsd = new Dictionary<string, List<string>>(comparer);


            var fk_match = new Regex(fileContentRegex["fk"]).Match(fileContent);
            var bd_match = new Regex(fileContentRegex["bd"]).Match(fileContent);//сколько платежей в платежке на какую общ сумму (BD||..|||0|4|61976.39|)
            //var payment_matches_all = new Regex(fileContentRegex["BDPL"] + fileContentRegex["BDPLST"]).Matches(fileContent);
            var payment_matches = new Regex(fileContentRegex["BDPL"]).Matches(fileContent);

            if (payment_matches.Count == 0)
                Console.WriteLine("Изменился формат/требования к форматам текстовых файлов УФК\r\nhttps://moufk.roskazna.gov.ru/gis/trebovaniy-k-formatam/");

            string fk = fk_match.Value;
            string bd = bd_match.Value;
            List<string> payment_pd_pdst = payment_matches.Cast<Match>().Select(match => match.Value.Replace("\r\n", string.Empty).Trim()).ToList();//выборка по 2 строки одного платежа // тут ноль из ЭДВ_64725530_230121.TFF

            //List<Payment> payments_all = payment_pd_pdst.Cast<string>().Select(paym => new Payment(paym, new Regex(fileContentRegex["BDPL"]), new Regex(fileContentRegex["BDPLST"]))).ToList(); // тут ноль из ЭДВ_64725530_230121.TFF
            List<string> payments_bdpd = payment_pd_pdst.Cast<string>().Select(paym => Payment.getPayment(paym, new Regex(fileContentRegex["BDPL"]))).ToList();
            //List<string> payments_bdpdst = payment_pd_pdst.Cast<string>().Select(paym => Payment.getPayment(paym, new Regex(fileContentRegex["BDPLST"]))).ToList();

            //пробуем пройти по строке-разбить - присвоить
            foreach (string payment in payments_bdpd)
            {
                List<string> listValuesPayment = payment.Split(spliter).ToList();
                string templateFieldName = "t" + listValuesPayment[0];
                listValuesPayment.Remove(listValuesPayment[0]);
                Console.WriteLine(templateFieldName);

                // смотрим, если шаблон с таким именем еще не загружен - загр, сохр
                if (!tmpl_xsd.Keys.Contains(templateFieldName))
                {
                    var xsd_template = getListElement(filePath, target_namesapce, templateFieldName);
                    List<string> tBDPL = xsd_template.Where(list => list.Key == templateFieldName).Select(value => value.Value).First();
                    tmpl_xsd.Add(templateFieldName, tBDPL);
                }

                Console.WriteLine($"платежка {templateFieldName}, Items: " + listValuesPayment.Count);
                Console.WriteLine($"шаблон {templateFieldName}, Items: " + tmpl_xsd[templateFieldName].Count);

                //объединяем список значений и ключей из шабл
                Dictionary<string, string> paym_dic = tmpl_xsd[templateFieldName].Zip(listValuesPayment, (k, v) => new { k, v })
                                                        .ToDictionary(x => x.k, x => x.v, comparer);

                PAUMENTS.Add(paym_dic);
            }

            return PAUMENTS;
        }

    }

}
