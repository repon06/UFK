using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace ufk.Helper
{
    /// <summary>
    /// последовательные записи 1 платежки
    /// </summary>
    class Payment
    {
        private readonly char[] spliter = { '|' };
        private readonly string delete_chars = "(0)";

        public string bdpd { get; set; }
        public string bdpdst { get; set; }

        public static string getPayment(string paymentContent, Regex regex)
        {
            Match match = regex.Match(paymentContent);
            return match.Value;
        }

        public Payment() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentContent"></param>
        /// <param name="bd_regex">@"(BDPD\|.*(\n|$))"</param>
        /// <param name="bdpd_regex">@"(BDPDST\|.*(\n|$))"</param>
        public Payment(string paymentContent, string bd_regex, string bdpd_regex)
        {
            var bdpd_regex_pt = new Regex(bd_regex);
            var bdpdst_regex_pt = new Regex(bdpd_regex);

            var bdpd_match = bdpd_regex_pt.Match(paymentContent);
            var bdpdst_match = bdpdst_regex_pt.Match(paymentContent);

            bdpd = bdpd_match.Value;
            bdpdst = bdpdst_match.Value;
        }

        public Payment(string paymentContent, Regex bdpd_regex_pt, Regex bdpdst_regex_pt)
        {
            var bdpd_match = bdpd_regex_pt.Match(paymentContent);
            var bdpdst_match = bdpdst_regex_pt.Match(paymentContent);

            bdpd = bdpd_match.Value;
            bdpdst = bdpdst_match.Value;
        }

        public Dictionary<string, string> ParsePayment(string str, PaymentType type, PaymentFKTemplate templates, bool debug = false)
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

            /// изменили кол-во параметров в шаблоне от 01.06.2020
            /// добавили: Код вида дохода	KOD_ INCOME
            if (values.Length == template.Length)
            {
                if (debug)
                {
                    Console.WriteLine($"tmpl.FK: {templates.fk}");
                    Console.WriteLine($"tmpl.BD: {templates.bd}");
                    Console.WriteLine($"tmpl.BDPD: {templates.bdpd}");
                    Console.WriteLine($"tmpl.BDPDST: {templates.bdpdst}");

                    foreach (string v in values)
                        Console.WriteLine($"values: '{v.Trim()}'");

                    foreach (string t in template)
                        Console.WriteLine($"fk_template: '{t.Trim().Replace(delete_chars, string.Empty)}'");
                }

                Dictionary<string, string> dic = template.Zip(values, (s, i) => new { s, i })
                    .ToDictionary(item => item.s.Replace(delete_chars, string.Empty), item => StringHelper.GetNotNull(item.i.Trim())); //значение триммим и ставим '0', если нет знач.

                return dic;
            }
            else
                throw new Exception("Различается кол-во параметров в платежке и в шаблоне!");
        }


    }
}
