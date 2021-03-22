using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ufk.Helper
{
    class PaymentReader
    {

        public static string ReadPayment(string path)
        {
            var value = System.IO.File.ReadAllText(path, Encoding.GetEncoding(1251));//.OpenText(path);
            return value;//file.ReadToEnd();
        }

        /// <summary>
        /// чтение платежек из файла - выясненые
        /// по строкам.
        /// возвращ набор строк
        /// 
        /// </summary>
        public static string[] GetPaymentLine(string FileName)
        //public string[] getPlatUfk(System.IO.StreamReader file)	
        {
            int max_lines = 10000;
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(FileName, Encoding.GetEncoding(1251));

            /*OpenFileDialog ofd=new OpenFileDialog();*/
            string[] line = new string[max_lines];
            int counter = 0;
            if (counter > max_lines)
                throw new Exception("Кол-во платежек превышает " + max_lines);

            while ((line[counter] = file.ReadLine()) != null) //.Replace("|","#")
            {
                //MessageBox.Show (line);
                counter++;
            }
            file.Close();
            Array.Resize(ref line, counter);
            if (line.Length > 0)
                return line;
            else
                return null;

        }


        /// <summary>
        /// Невыясненые платежки
        /// </summary>
        /// 09.10.2013 11:20:04 Калядин Максим Андреевич > Денис Быстров > 
        /// Привет. Надо чтобы так же загружались в программу, чтобы были реестром. 
        /// а не по одному платежу.
        /// <param name="file"></param>
        /// <returns></returns>
        public static string[] GetNeviyasnUfk(System.IO.StreamReader file)
        {
            /*OpenFileDialog ofd=new OpenFileDialog();*/
            string[] line = new string[2500];
            int counter = 0;

            while ((line[counter] = file.ReadLine()) != null) //.Replace("|","#")
            {
                //MessageBox.Show (line);
                counter++;
            }
            file.Close();

            Array.Resize(ref line, counter);


            if (line.Length > 0)
                return line;
            else
                return null;
        }

    }
}
