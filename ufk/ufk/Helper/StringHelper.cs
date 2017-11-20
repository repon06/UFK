using System;
using System.Text.RegularExpressions;

namespace ufk.Helper
{
    class StringHelper
    {

        /// <summary>
        /// кбк коды по аренде земли - все остальные выгружать ГАмаюновой
        /// </summary>
        public static string[] kbk_kaladyn = { "04611105012040000120", "04611105024040000120", "04611406012040000430", "04611406024040000430" };

        /// <summary>
        /// возвращает кол-во знаков разделителей
        /// </summary>
        /// <returns></returns>
        public static int GetCountSplitter(string str, char splitter)
        {
            try
            {
                //!можно и так! int count = str.ToArray().Where(i => i == splitter).Count();
                int countZn = new Regex(splitter.ToString()).Matches(str).Count;
                return countZn;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// чтобы не нулевые
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetNotNull(string str)
        {
            if (str != null)
                if (str == string.Empty || str.Trim() == "")
                    return "0";
                else
                    return str.Trim();
            else
                return "0";
        }

        /// <summary>
        /// чтобы не нулевые
        /// если 1 значение нулевое, то подставл 2-е знач)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static string GetNotNull(string str, string str2)
        {
            if (str != null)
                if (str == string.Empty || str.Trim() == "")
                    return str2;
                else
                    return str.Trim();
            else
                return str2;
        }


        /// <summary>
        /// определяем сколько платежек располагается в файле .UFD
        /// "UFPP|"
        /// </summary>
        /// <param name="str4file"></param>
        public static string[] GetPaymentCount(string[] str4file, string whoFindStr)
        {
            int cnt = 0;

            //определяем кол-во платежек - UFPP, либо сопроводит информац - UFPP_N
            for (int q = 0; q < str4file.Length; q++)
                if (str4file[q].StartsWith(whoFindStr))
                    cnt++;

            int cntStr = 0;//номер строки в новом массиве			
            string[] str = new string[cnt];
            for (int q = 0; q < str4file.Length; q++)
            {
                if (str4file[q].StartsWith(whoFindStr))
                {
                    str[cntStr] = str4file[q];
                    cntStr++;
                }
            }

            return str;
        }

        /// <summary>
        /// возвращает массив индексов разделителей
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] GetZnIndexs(string str, int CountZn)
        {
            int[] ZnIndexs = new int[CountZn];
            int y = 0;

            for (int x = 0; x < str.Length; x++)
            {
                if (str.Substring(x, 1) == "#")
                {
                    ZnIndexs[y] = x;
                    y++;
                }
            }
            return ZnIndexs;
        }

        /*

 20-11-2017

 /// <summary>
 /// парсинг строки s по символам symb
 /// пример строки: UFPP_N|1|Комитет по управлению имуществом города Саратова|6450003860|645001001|63401000000||04611105012040000120|||0.00|||
 /// </summary>
 /// <param name="s"></param>
 /// <returns></returns>
 public string[] ParceSymb2Str_(string s, char splitter)
 {

     //string[,] Zn=new string[0,0];//сами данные - между разделителями
     string[] ZnN = new string[0];//сами данные - между разделителями

     string plat_str = s.Replace('|', splitter).Trim();//очищенная ттекущая строка
     int countZnIndex = GetCountSplitter(plat_str, splitter);//определяем кол-во разделителей

     //изменяем размерность массива
     if (ZnN.Length < s.Length || ZnN.Length < countZnIndex)
         Array.Resize(ref ZnN, countZnIndex);

     //MessageBox.Show( countZnIndex.ToString(), "ZnN.Length="+ZnN.Length.ToString() );

     int[] ZnIndexs = getZnIndexs(plat_str, countZnIndex);//массив расположения разделителей
                                                          //int i=0;			
     for (int q = 0; q + 1 < ZnIndexs.Length; q++)
         ZnN[q] = plat_str.Substring(ZnIndexs[q] + 1, ZnIndexs[q + 1] - ZnIndexs[q] - 1);

     ZnIndexs = null;
     return ZnN;
 }
 */


    }
}
