﻿using System;
using System.Linq;
using System.Text;

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Excel;
using ufk.Helper;
using System.Configuration;

namespace ufk
{
    public partial class MainForm : Form
    {
        private readonly int hash = "заплатите за программу!".GetHashCode();
        //private List<Dictionary<string, string>> PAUMENTS = new List<Dictionary<string, string>>();
        private Dictionary<string, List<Dictionary<string, string>>> PAUMENTS = new Dictionary<string, List<Dictionary<string, string>>>();
        public MainForm()
        {
            InitializeComponent();
            loadButton.Enabled = true;
            if (DateTime.Now > DateTime.Parse("30.03.2021"))
                button1.Enabled = false;
            //ConfigurationSettings.AppSettings.Add("test_int", "123");
            //string keyvalue = System.Configuration.ConfigurationManager.AppSettings["keyname"];
        }

        void LoadButtonClick(object sender, EventArgs e)
        {
            print_plat();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            print_plat(true);
        }

        private void print_plat(bool isNew = false)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            var splitter = '#';//символ разделителей

            //если невыясненные
            if (cbNev.CheckState == CheckState.Checked)
            {
                ofd.Multiselect = true;
                ofd.Filter = "Файлы .uf* (*.ufd;*.ufo;*.ufe;*.uff;*.uf*)|*.ufd;*.ufo;*.ufe;*.uff;*.uf*| Файлы xml (*.xml) | *.xml";
            }
            else
            {
                ofd.Multiselect = false;
                //21-11-2017 ofd.Filter = "Файлы tff (*.tff)|*.tff|Файлы zfa (*.zf*)|*.zf*|All files (*.*)|*.*";
                ofd.Filter = "Файлы tff (*.tff)|*.tff";
            }

            if (ofd.ShowDialog() == DialogResult.OK)
                //если не невыясненные, по 1 шт
                if (ofd.FileNames.Length == 1 && cbNev.CheckState == CheckState.Unchecked)
                {
                    ///////////////////////////////// ПЛАТЕЖКИ tff и НЕВЫЯСНЕННЫЕ zfa >	
                    string[] plat = null;
                    var exp = string.Empty; //расширение файла
                    var plat_str = string.Empty;
                    //var na4alo_dannih = 0; // с какой строки в ХМЛ-е начнутся норм данные-начинаем с этой строки читать файл


                    if (ofd.FilterIndex == 1)
                    { //если ТФФ
                        exp = ".tff";
                        //na4alo_dannih = 2;
                    }
                    else
                    { //если ЗФА
                        exp = ".zf*";
                        //na4alo_dannih = 3;
                    }



                    //получаем перечень платежек из файла OLD
                    plat = PaymentReader.GetPaymentLine(ofd.FileName);

                    try
                    {
                        //читаем шаблон XSD
                        string xsd_path = $"{Directory.GetCurrentDirectory()}\\docs\\formulars.xsd";

                        //читаем файл с платежками
                        string fileContent = PaymentReader.ReadPayment(ofd.FileName);

                        // по старому или новому процессу
                        if (isNew)
                            PAUMENTS = new XmlSchemaReader().getPaymentFKValues(fileContent, xsd_path, "http://www.roskazna.ru/eb/domain/Inf_Pay_Doc/formular");
                        else
                        {
                            PaymentFKValues paym = new PaymentFKValues(fileContent); //передаем строки из платежки - и парсим
                            PAUMENTS = paym.getPAUMENTS();
                            if (paym.err.Count > 0)
                            {
                                var errs = paym.err.Aggregate((i, j) => i + "\r\n\r\n" + j);
                                MessageBox.Show($"{errs}. Данные записи не будут обработаны. Исправьте исходный файл.", "Error UFK", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }

                        if (cbNev.CheckState == CheckState.Unchecked)
                            PaymentExcelWriter.SaveXls(ofd.FileName, PAUMENTS);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message};\r\n\r\nTrace: {ex.StackTrace}", "Error UFK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    ///////////////////////////////// < ПЛАТЕЖКИ tff и НЕВЫЯСНЕННЫЕ zfa	
                }//if openfiledial
                else //!!если НЕВЫЯСНЕННЫЕ или >= 1 файла выделили
                if (ofd.FileNames.Length >= 1 && ofd.FileNames.Length <= 520)
                {
                    if (ofd.FilterIndex == 2)//если выбираем zfa
                    {
                        //MessageBox.Show(ofd.FileNames.Length + " ФАЙЛОВ");
                        string[,] inXml = new string[ofd.FileNames.Length, 16];//почему 16 ? 

                        //перебираем все файцы
                        for (int val = 0; val < ofd.FileNames.Length; val++)
                        {
                            string[] tmp = ParceXml.ParcesXml(ofd.FileNames[val].ToString());

                            for (int row = 0; row <= 15; row++)//перебираем все значения одномерного массива - данные 1 файла, вносим в двумернй массив
                                inXml[val, row] = tmp[row];

                            PaymentExcelWriter.SavePosleViyasnXls(ofd.FileNames[0].ToString().Trim().ToLower().Replace(".xml", ".xls"), inXml);
                        }
                    }
                    else//если выбираем UFD
                    if (ofd.FilterIndex == 1  /*|| ofd.FilterIndex==3*/ ) //если выбираем UFD или ZFA!!!
                    {
                        string[][][] inXmlm = new string[ofd.FileNames.Length][][];//почему 16 ?

                        //перебираем все файцы
                        for (int val = 0; val < ofd.FileNames.Length; val++)
                        {
                            inXmlm[val] = new string[5][];
                            inXmlm[val][0] = new string[1];//имя файла
                            inXmlm[val][1] = new string[1];//содержимое файла

                            inXmlm[val][2] = new string[1];//строка UF| с данными платежки - кому кто за что - без дат и сумм
                            inXmlm[val][3] = new string[1];//платежки в файле - строки UFPP|
                            inXmlm[val][4] = new string[1];//коменты к платежкам в файле - строки UFPP_N|

                            inXmlm[val][0][0] = ofd.FileNames[val].ToString();//имя файла в 0;
                            inXmlm[val][1] = PaymentReader.GetPaymentLine(ofd.FileNames[val]);//читаем содержимое файла в 1

                        }

                        string char_plat_na4alo = string.Empty;//начало строки для платежки
                        string char_comm_na4alo = string.Empty;//начало строки для комментариев к платежке
                        string char_posp_na4alo = string.Empty;//начало строки для подписей к платежке

                        if (ofd.FilterIndex == 1)
                        {
                            char_plat_na4alo = "UF|";
                            char_comm_na4alo = "UFPP|";
                            char_posp_na4alo = "UFPP_N|";
                        }

                        string[][][] UFD = new string[inXmlm.Length][][];//???????????????

                        for (int val = 0; val < ofd.FileNames.Length; val++)//перебираем файлы
                        {
                            for (int i = 0; i < inXmlm[val][1].Length; i++)//перебираем содержимое файла по строкам
                                if (inXmlm[val][1][i].StartsWith(char_plat_na4alo)) //если строка начинается с...
                                    inXmlm[val][2][0] = inXmlm[val][1][i];//сохраняем строку с данными платежки - кому кто за что - без дат и сумм


                            inXmlm[val][3] = StringHelper.GetPaymentCount(inXmlm[val][1], char_comm_na4alo);// платежек в файле
                            inXmlm[val][4] = StringHelper.GetPaymentCount(inXmlm[val][1], char_posp_na4alo);// подписей к платежкам в файоле

                            if (inXmlm[val][3].Length == inXmlm[val][4].Length)//определяем размер платежек в фале соотв размеру доп описания к ним?! UFPP==UFPP_N ???
                            {
                                UFD[val] = new string[inXmlm[val][3].Length][]; //[]-кол-во файлов []-кол-во платежек в файле	

                                if (inXmlm[val][2][0] != null)
                                {
                                    //upd 20-11-2017 string[] tmp_uf = ParceSymb2Str(inXmlm[val][2][0], '#');//строка UF| - 1 строка, много значений
                                    string[] tmp_uf = inXmlm[val][2][0].Split(splitter);                     //строка UF| - 1 строка, много значений

                                    for (int xxx = 0; xxx < inXmlm[val][3].Length; xxx++) //перебираем строки с платежками
                                    {
                                        //upd 20-11-2017 sstring[] tmp_ufpp = ParceSymb2Str(inXmlm[val][3][xxx], '#');//строка UFPP| - много строк, много значений
                                        string[] tmp_ufpp = inXmlm[val][3][xxx].Split(splitter);                      //строка UFPP| - много строк, много значений

                                        //upd 20-11-2017 sstring[] tmp_ufpp_n = ParceSymb2Str(inXmlm[val][4][xxx], '#');//строка UFPP_N| - много строк, много значений
                                        string[] tmp_ufpp_n = inXmlm[val][4][xxx].Split(splitter);                      //строка UFPP_N| - много строк, много значений


                                        if (ofd.FileNames[0].ToString().Trim().ToLower().Contains(".ufo"))
                                        {
                                            UFD[val][xxx] = new[] {tmp_ufpp[4],tmp_ufpp[5],tmp_ufpp[13],tmp_uf[18-1],tmp_uf[19-1],
                                    tmp_ufpp[7],tmp_ufpp[8],tmp_ufpp_n[6],"",tmp_ufpp[5],tmp_ufpp[5],
                                    "",tmp_uf[17-1],"","","","",tmp_uf[6],"","","","","",
                                    "",tmp_uf[20-1],"","","","","","","",tmp_uf[1],tmp_uf[2],"",tmp_uf[0],""};
                                        }
                                        else if (ofd.FileNames[0].ToString().Trim().ToLower().Contains(".ufd")
                                                || ofd.FileNames[0].ToString().Trim().ToLower().Contains(".ufe")
                                               || ofd.FileNames[0].ToString().Trim().ToLower().Contains(".uff")
                                              || ofd.FileNames[0].ToString().Trim().ToLower().Contains(".uf")  // !!! для все .UF*
                                                                                                               // || ofd.FileNames[0].ToString().Trim().ToLower().Contains(".zfa")
                                              )
                                        {
                                            UFD[val][xxx] = new[] {tmp_ufpp[4],tmp_ufpp[5],tmp_ufpp[13],tmp_uf[18],tmp_uf[19],
                                    tmp_ufpp[7],tmp_ufpp[8],tmp_ufpp_n[6],"",tmp_ufpp[5],tmp_ufpp[5],
                                    "",tmp_uf[17],"","","","",tmp_uf[6],"","","","","",
                                    "",tmp_uf[20],"","","","","","","",tmp_uf[1],tmp_uf[2],"",tmp_uf[0],""};
                                        }


                                    } //for
                                }//if
                                else
                                    MessageBox.Show("1-Пустая строка");
                            }
                            else
                            {
                                UFD[val] = null;
                                File.Move(inXmlm[val][0][0], inXmlm[val][0][0].Replace(".", "_____.")); //переименование неверного файла
                            }

                        } //for перебираем файлы

                        PaymentExcelWriter.SavePosleViyasnXls(ofd.FileNames[0].ToString().Trim().ToLower().Substring(0, ofd.FileNames[0].ToString().Trim().Length - 3) + "xls", UFD);

                    }//if
                }
                else
                {
                    MessageBox.Show("Ошибочка. Возможно, отменили \r\nили выделили больше 520 файлов!\r\n" + ofd.FileNames.Length + " ФАЙЛОВ", "!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }


    }
}
