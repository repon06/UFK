using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ufk.Helper
{
    /// <summary>
    /// Парсит по шаблону платежку (из файла) из Федерального казначейства
    /// http://moufk.roskazna.ru/gis/trebovaniy-k-formatam/
    /// Требования к форматам файлов, используемых при информационном взаимодействии
    /// 17 августа 2017
    /// </summary>
    class FkPaymentHelper
    {
        readonly string templatePath = $"{Directory.GetCurrentDirectory()}\\templateplat.txt";
        //readonly string str_template;
        readonly PaymentFKTemplate templates;
        private readonly char[] spliter = { '|' };
        private readonly string delete_chars = "(0)";

        public FkPaymentHelper()
        {
            /*str_template = ReadTemplate(templatePath);*/
            templates = ReadTemplates(templatePath);

            Console.WriteLine($"tmpl.FK: {templates.fk}");
            Console.WriteLine($"tmpl.BD: {templates.bd}");
            Console.WriteLine($"tmpl.BDPD: {templates.bdpd}");
            Console.WriteLine($"tmpl.BDPDST: {templates.bdpdst}");
        }

        /// <summary>
        /// читаем файл шаблона - выводим текст
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ReadTemplate(string path)
        {
            var file = System.IO.File.OpenText(path);
            return file.ReadToEnd();
        }

        /// <summary>
        /// читаем файл шаблона - выводим PaymentFKTemplate
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private PaymentFKTemplate ReadTemplates(string path)
        {
            string[] stringSeparators = new string[] { "\r\n" };
            var reader = System.IO.File.OpenText(path);
            var text = reader.ReadToEnd();
            string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

            if (lines.Count() == 4)
                return new PaymentFKTemplate(lines[0], lines[1], lines[2], lines[3]);
            else
                throw new Exception("В файле шаблона не 4 строки!");
        }

        /*
        /// <summary>
        /// распознать 1 строку FK
        /// </summary>
        /// <param name="fk_str"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public Dictionary<string, string> ParseFK(string fk_str, bool debug = false)
        {
            var values = fk_str.Split(spliter);
            var fk_template = templates.fk.Split(spliter);

            if (values.Length == fk_template.Length)
            {
                if (debug)
                {
                    foreach (var v in values)
                        Console.WriteLine($"values: '{v.Trim()}'");

                    foreach (var t in fk_template)
                        Console.WriteLine($"fk_template: '{t.Trim().Replace(delete_chars, string.Empty)}'");
                }

                Dictionary<string, string> dic = fk_template.Zip(values, (s, i) => new { s, i }).ToDictionary(item => item.s.Replace(delete_chars, string.Empty), item => item.i);

                if (debug)
                {
                    Console.WriteLine($"dictionary: {dic["NUM_VER"]}");
                    Console.WriteLine($"dictionary: {dic["FORMER"]}");
                }

                return dic;
            }
            else
                throw new Exception("Различается кол-во параметров в платежке и в шаблоне!");
        }

        public Dictionary<string, string> ParseBDPD(string str, bool debug = false)
        {
            var values = str.Split(spliter);
            var template = templates.bdpd.Split(spliter);

            if (values.Length == template.Length)
            {
                if (debug)
                {
                    foreach (var v in values)
                        Console.WriteLine($"values: '{v.Trim()}'");

                    foreach (var t in template)
                        Console.WriteLine($"fk_template: '{t.Trim().Replace(delete_chars, string.Empty)}'");
                }

                Dictionary<string, string> dic = template.Zip(values, (s, i) => new { s, i }).ToDictionary(item => item.s.Replace(delete_chars, string.Empty), item => item.i);

                return dic;
            }
            else
                throw new Exception("Различается кол-во параметров в платежке и в шаблоне!");
        }
        */

        public Dictionary<string, string> ParsePayment(string str, PaymentType type, bool debug = false)
        {
            var values = str.Split(spliter);
            string[] template;// = templates.bdpdst.Split(spliter);

            switch (type)
            {
                case PaymentType.bd:
                    template = templates.bd.Split(spliter);
                    break;
                case PaymentType.fk:
                    template = templates.fk.Split(spliter);
                    break;
                case PaymentType.bdpd:
                    template = templates.bdpd.Split(spliter);
                    break;
                case PaymentType.bdpdst:
                    template = templates.bdpdst.Split(spliter);
                    break;
                default:
                    template = null;
                    break;
            }

            if (values.Length == template.Length)
            {
                if (debug)
                {
                    foreach (var v in values)
                        Console.WriteLine($"values: '{v.Trim()}'");

                    foreach (var t in template)
                        Console.WriteLine($"fk_template: '{t.Trim().Replace(delete_chars, string.Empty)}'");
                }

                Dictionary<string, string> dic = template.Zip(values, (s, i) => new { s, i }).ToDictionary(item => item.s.Replace(delete_chars, string.Empty), item => item.i);

                return dic;
            }
            else
                throw new Exception("Различается кол-во параметров в платежке и в шаблоне!");
        }

        private void xxx()
        {

            string valueStr = "BDPDST       |04611105012040000120|20         |            |         |63701000|2997.15|          |0      |          |         |";
            string valueStr_ = "BDPDST|04611105012040000120|20|||63701000|2997.15||0|||";
            var splitStr = valueStr.Split(spliter);
            Console.WriteLine($"str count: {splitStr.Length}");
            foreach (var i in splitStr)
                Console.WriteLine($"str: '{i.Trim()}'");

            Console.WriteLine();

            string tmplStr = "BDPDST(+BDPD)|KBK(0)              |TYPE_KBK(0)|ADD_KLASS(0)|NUM_BO(0)|ОКАТО(0)|SUM    |SUM_NDS(0)|DIR_SUM|MES_FIN(0)|REZERV(0)|";
            var splitTmpl = tmplStr.Split(spliter);
            Console.WriteLine($"tmpl count: {splitTmpl.Length}");
            foreach (var j in splitTmpl)
                Console.WriteLine($"tmpl: '{j.Trim()}'");

        }

    }


}
