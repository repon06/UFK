
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text;
using System.Xml;
using System.IO;

//using System.Xml.Linq;//Linq;


namespace ufk
{
    /// <summary>
    /// Description of ParceXml.
    /// </summary>
    public class ParceXml
    {

        /// <param name="path">path = @"C:\Users\LordRP\Desktop\1.xml";//путь до файла</param>
        public static string[] ParcesXml(string path)
        {

            string[] x = new string[16];
            //int i=0;

            XDocument doc = XDocument.Load(path);
            XElement items = doc.Root;
            // ea650f4e-aae5-4240-b3dc-1b015796cfec.XML
            //D:\работа\2013\c#_xml\Neviyasnennie\НЕВЫЯСНЕННЫЕ\068eb7a7-e11e-4152-a32e-0c758d5d215b.XML


            IEnumerable<string> TofkName = from item in items.Descendants("Header") select (string)item.Element("TofkName");
            x[0] = TofkName.ToArray()[0];//есть
            IEnumerable<string> PayerName = from item in items.Descendants("Header") select (string)item.Element("PayerName");
            x[1] = PayerName.ToArray()[0]; ;//есть
            IEnumerable<string> PayerInn = from item in items.Descendants("Header") select (string)item.Element("PayerInn");
            x[2] = PayerInn.ToArray()[0]; ;//есть
            IEnumerable<string> PayerKpp = from item in items.Descendants("Header") select (string)item.Element("PayerKpp");
            x[6] = PayerKpp.ToArray()[0];
            IEnumerable<string> DocName = from item in items.Descendants("Line") select (string)item.Element("DocName");
            x[7] = DocName.ToArray()[0];
            IEnumerable<string> DocNumber = from item in items.Descendants("Line") select (string)item.Element("DocNumber");
            x[3] = DocNumber.ToArray()[0]; ;//есть
            IEnumerable<string> DocDate = from item in items.Descendants("Line") select (string)item.Element("DocDate");
            x[4] = DocDate.ToArray()[0].Trim().Substring(0, 10);
            x[4] = DateTime.ParseExact(x[4], "yyyy-MM-dd", null).ToString("d.MM.yyyy");
            IEnumerable<string> FinOrgName = from item in items.Descendants("Header") select (string)item.Element("FinOrgName");
            x[5] = FinOrgName.ToArray()[0]; ;//есть
            IEnumerable<string> PayerPassport = from item in items.Descendants("Header") select (string)item.Element("PayerPassport");
            x[8] = PayerPassport.ToArray()[0];
            IEnumerable<string> ReceiverName = from item in items.Descendants("Line") select (string)item.Element("ReceiverName");
            x[9] = ReceiverName.ToArray()[0];
            IEnumerable<string> ReceiverInn = from item in items.Descendants("Line") select (string)item.Element("ReceiverInn");
            x[10] = ReceiverInn.ToArray()[1];
            IEnumerable<string> ReceiverKpp = from item in items.Descendants("Line") select (string)item.Element("ReceiverKpp");
            x[11] = ReceiverKpp.ToArray()[1];

            //  MessageBox.Show( "ИНН "+x[10], "КПП "+x[11]);

            IEnumerable<string> Bcc = from item in items.Descendants("Line") select (string)item.Element("Bcc");
            x[12] = Bcc.ToArray()[1];
            IEnumerable<string> EpdGuidTff = from item in items.Descendants("Line") select (string)item.Element("EpdGuidTff");
            x[13] = EpdGuidTff.ToArray()[0];
            IEnumerable<string> Amount = from item in items.Descendants("Line") select (string)item.Element("Amount");
            x[14] = Amount.ToArray()[0];
            IEnumerable<string> OkatoCode = from item in items.Descendants("Line") select (string)item.Element("OkatoCode");
            x[15] = OkatoCode.ToArray()[0];

            return x;

        }
    }
}
