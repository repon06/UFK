using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ufk.Helper
{
    class PaymentExcelWriter
    {

        /// <summary>
        /// Калядину выгрузки Платежные поручения
        /// 
        /// UPD 01.04.2014
        /// Обновили в УФК структуру TFF файлов
        /// добавили реквизит какой-то...
        /// у меня в массиве, начиная с 23 записи - сдвигать вперед!
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Zn"></param>
        /// na4alo_dannih - c с какой строки начинать искать данные - т.е. скока пропустить -служ инф
        /// exp - расширение
        public static void SaveXlsKalad(string filename, string[,] Zn, int na4alo_dannih, string exp)
        {
            Console.WriteLine("saveXls");

            try
            {

                FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
                ExcelWriter writer = new ExcelWriter(stream);
                writer.BeginWrite();

                int ii = 0;

                writer.WriteCell(ii, 0, "Номер документа");
                writer.WriteCell(ii, 1, "Дата документа");
                writer.WriteCell(ii, 2, "Сумма документа");
                writer.WriteCell(ii, 3, "ИНН плательщика");
                writer.WriteCell(ii, 4, "КПП плательщика");
                writer.WriteCell(ii, 5, "ИНН получателя");
                writer.WriteCell(ii, 6, "КПП получателя");
                writer.WriteCell(ii, 7, "Код бюджетной классификации");
                writer.WriteCell(ii, 8, "Вид платежа");
                writer.WriteCell(ii, 9, "Поступило в банк плательщика");
                writer.WriteCell(ii, 10, "Дата списания со счета плательщика");
                writer.WriteCell(ii, 11, "Вид операции");
                writer.WriteCell(ii, 12, "Наименование плательщика");
                writer.WriteCell(ii, 13, "Расчетный счет плательщика");
                writer.WriteCell(ii, 14, "БИК банка плательщика");
                writer.WriteCell(ii, 15, "Наименование банка плательщика");
                writer.WriteCell(ii, 16, "Коррсчет банка плательщика");
                writer.WriteCell(ii, 17, "Наименование получателя");
                writer.WriteCell(ii, 18, "Расчетный счет получателя");
                writer.WriteCell(ii, 19, "БИК банка получателя");
                writer.WriteCell(ii, 20, "Наименование банка получателя");
                writer.WriteCell(ii, 21, "Коррсчет банка получателя");
                writer.WriteCell(ii, 22, "Очередность платежа");
                writer.WriteCell(ii, 23, "Статус составителя расчетного документа");
                writer.WriteCell(ii, 24, "Назначение платежа");
                writer.WriteCell(ii, 25, "Показатель основания платежа");
                writer.WriteCell(ii, 26, "Код ОКАТО");
                writer.WriteCell(ii, 27, "Показатель налогового периода");
                writer.WriteCell(ii, 28, "Показатель номера документа");
                writer.WriteCell(ii, 29, "Показатель даты документа");
                writer.WriteCell(ii, 30, "Тип платежа");
                writer.WriteCell(ii, 31, "Номер частичного платежа");
                writer.WriteCell(ii, 32, "Номер платежного документа");
                writer.WriteCell(ii, 33, "Дата платежного документа");
                writer.WriteCell(ii, 34, "Содержание операции");
                writer.WriteCell(ii, 35, "Глобальный идентификатор");
                writer.WriteCell(ii, 36, "Срок платежа");

                ii = 1;//запоминать перепрыгиваем строку c заголовками

                for (int i = na4alo_dannih; i < Zn.GetLength(0); i++)
                {

                    if (exp == ".tff")
                    {
                        if (i % 2 == 0 && StringHelper.kbk_kaladyn.Contains(StringHelper.GetNotNull(Zn[i, 26]))) //если четные и содержит кбк отдела аренды (Калядин)
                        {
                            try
                            {
                                writer.WriteCell(ii, 0, StringHelper.GetNotNull(Zn[i, 0]));
                                writer.WriteCell(ii, 1, StringHelper.GetNotNull(Zn[i, 1]));
                                writer.WriteCell(ii, 2, Convert.ToDouble(StringHelper.GetNotNull(Zn[i, 2]).Replace(".", ",")));
                                writer.WriteCell(ii, 3, StringHelper.GetNotNull(Zn[i, 7]));
                                writer.WriteCell(ii, 4, StringHelper.GetNotNull(Zn[i, 8]));
                                writer.WriteCell(ii, 5, StringHelper.GetNotNull(Zn[i, 14]));
                                writer.WriteCell(ii, 6, StringHelper.GetNotNull(Zn[i, 15]));
                                writer.WriteCell(ii, 7, StringHelper.GetNotNull(Zn[i, 26]));
                                writer.WriteCell(ii, 8, StringHelper.GetNotNull(Zn[i, 3]));
                                writer.WriteCell(ii, 9, StringHelper.GetNotNull(Zn[i, 4], Zn[i, 1]));
                                writer.WriteCell(ii, 10, StringHelper.GetNotNull(Zn[i, 5], Zn[i, 1]));
                                writer.WriteCell(ii, 11, StringHelper.GetNotNull(Zn[i, 6]));
                                writer.WriteCell(ii, 12, StringHelper.GetNotNull(Zn[i, 9]));
                                writer.WriteCell(ii, 13, StringHelper.GetNotNull(Zn[i, 10]));
                                writer.WriteCell(ii, 14, StringHelper.GetNotNull(Zn[i, 11]));
                                writer.WriteCell(ii, 15, StringHelper.GetNotNull(Zn[i, 12]));
                                writer.WriteCell(ii, 16, StringHelper.GetNotNull(Zn[i, 13]));
                                writer.WriteCell(ii, 17, StringHelper.GetNotNull(Zn[i, 16]));
                                writer.WriteCell(ii, 18, StringHelper.GetNotNull(Zn[i, 17]));
                                writer.WriteCell(ii, 19, StringHelper.GetNotNull(Zn[i, 18]));
                                writer.WriteCell(ii, 20, StringHelper.GetNotNull(Zn[i, 19]));
                                writer.WriteCell(ii, 21);
                                writer.WriteCell(ii, 22);
                                writer.WriteCell(ii, 23, StringHelper.GetNotNull(Zn[i, 25]));
                                writer.WriteCell(ii, 24, StringHelper.GetNotNull(Zn[i, 24]));
                                writer.WriteCell(ii, 25, StringHelper.GetNotNull(Zn[i, 28]));
                                writer.WriteCell(ii, 26, StringHelper.GetNotNull(Zn[i, 27]));
                                writer.WriteCell(ii, 27, StringHelper.GetNotNull(Zn[i, 29]));
                                writer.WriteCell(ii, 28, StringHelper.GetNotNull(Zn[i, 30]));
                                writer.WriteCell(ii, 29, StringHelper.GetNotNull(Zn[i, 31]));
                                writer.WriteCell(ii, 30, StringHelper.GetNotNull(Zn[i, 32]));
                                writer.WriteCell(ii, 31);
                                writer.WriteCell(ii, 32);
                                writer.WriteCell(ii, 33);
                                writer.WriteCell(ii, 34);
                                writer.WriteCell(ii, 35, StringHelper.GetNotNull(Zn[i, 40]));
                                ii++;
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("Ошбика в строке " + i + "\r\n" + exc.Message + "\r\n" + exc.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else //// "ZFA"
                    {
                        if (i % 2 == 0 && StringHelper.kbk_kaladyn.Contains(StringHelper.GetNotNull(Zn[i, 25]))) //если четные и содержит кбк отдела аренды (Калядин)
                        {
                            try
                            {
                                writer.WriteCell(ii, 0, StringHelper.GetNotNull(Zn[i, 0]));// 0, "Номер документа
                                writer.WriteCell(ii, 1, StringHelper.GetNotNull(Zn[i, 1]));// 1, "Дата документа
                                writer.WriteCell(ii, 2, Convert.ToDouble(StringHelper.GetNotNull(Zn[i, 2]).Replace(".", ",")));// 2, "Сумма документа
                                writer.WriteCell(ii, 3, StringHelper.GetNotNull(Zn[i, 7]));// 3, "ИНН плательщика
                                writer.WriteCell(ii, 4, StringHelper.GetNotNull(Zn[i, 8]));// 4, "КПП плательщика
                                writer.WriteCell(ii, 5, StringHelper.GetNotNull(Zn[i, 14]));// 5, "ИНН получателя
                                writer.WriteCell(ii, 6, StringHelper.GetNotNull(Zn[i, 15]));// 6, "КПП получателя
                                writer.WriteCell(ii, 7, StringHelper.GetNotNull(Zn[i, 25]));// 7, "Код бюджетной классификации
                                writer.WriteCell(ii, 8);// 8, "Вид платежа StringHelper.GetNotNull(Zn[i,3])
                                writer.WriteCell(ii, 9, StringHelper.GetNotNull(Zn[i, 4], Zn[i, 5]));// 9, "Поступило в банк плательщика
                                writer.WriteCell(ii, 10, StringHelper.GetNotNull(Zn[i, 5], Zn[i, 5]));// 10, "Дата списания со счета плательщика
                                writer.WriteCell(ii, 11, StringHelper.GetNotNull(Zn[i, 6]));// 11, "Вид операции
                                writer.WriteCell(ii, 12, StringHelper.GetNotNull(Zn[i, 9]));// 12, "Наименование плательщика
                                writer.WriteCell(ii, 13, StringHelper.GetNotNull(Zn[i, 10]));// 13, "Расчетный счет плательщика
                                writer.WriteCell(ii, 14, StringHelper.GetNotNull(Zn[i, 11]));// 14, "БИК банка плательщика
                                writer.WriteCell(ii, 15, StringHelper.GetNotNull(Zn[i, 12]));// 15, "Наименование банка плательщика
                                writer.WriteCell(ii, 16, StringHelper.GetNotNull(Zn[i, 13]));// 16, "Коррсчет банка плательщика
                                writer.WriteCell(ii, 17, StringHelper.GetNotNull(Zn[i, 16]));// 17, "Наименование получателя
                                writer.WriteCell(ii, 18, StringHelper.GetNotNull(Zn[i, 17]));// 18, "Расчетный счет получателя
                                writer.WriteCell(ii, 19, StringHelper.GetNotNull(Zn[i, 18]));// 19, "БИК банка получателя
                                writer.WriteCell(ii, 20, StringHelper.GetNotNull(Zn[i, 19]));// 20, "Наименование банка получателя
                                writer.WriteCell(ii, 21);   // 21, "Коррсчет банка получателя
                                writer.WriteCell(ii, 22);   // 22, "Очередность платежа
                                writer.WriteCell(ii, 23);// 23, "Статус составителя расчетного документа , StringHelper.GetNotNull(Zn[i,25])
                                writer.WriteCell(ii, 24, StringHelper.GetNotNull(Zn[i, 23]));// 24, "Назначение платежа
                                writer.WriteCell(ii, 25);// 25, "Показатель основания платежа , StringHelper.GetNotNull(Zn[i,28])
                                writer.WriteCell(ii, 26);// 26, "Код ОКАТО, StringHelper.GetNotNull(Zn[i,27])
                                writer.WriteCell(ii, 27);// 27, "Показатель налогового периода , StringHelper.GetNotNull(Zn[i,29])
                                writer.WriteCell(ii, 28);// 28, "Показатель номера документа , StringHelper.GetNotNull(Zn[i,30])
                                writer.WriteCell(ii, 29);// 29, "Показатель даты документа , StringHelper.GetNotNull(Zn[i,31])
                                writer.WriteCell(ii, 30);// 30, "Тип платежа , StringHelper.GetNotNull(Zn[i,32])
                                writer.WriteCell(ii, 31);
                                writer.WriteCell(ii, 32);
                                writer.WriteCell(ii, 33);
                                writer.WriteCell(ii, 34);
                                writer.WriteCell(ii, 35, StringHelper.GetNotNull(Zn[i, 39]));// 35, "Глобальный идентификатор
                                ii++;
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("Ошбика в строке " + i + "\r\n" + exc.Message + "\r\n" + exc.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                    }


                } //for


                writer.EndWrite();
                stream.Close();

                //ПЕРЕСОХРАНЯЕМ!            
                Excel xlstmp = new Excel();
                xlstmp.OpenDocument(filename);
                xlstmp.SaveDocument(filename);
                xlstmp.CloseDocument();

                MessageBox.Show((ii - 1) + " сконвертированы в файл " + filename + " сохранен!", "save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception strStream)
            {
                MessageBox.Show("Что-то с файлом " + filename + "\r\n" + strStream.Message + "\r\n" + strStream.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Гамаюновой выгрузки Платежные поручения
        /// 
        /// UPD 01.04.2014
        /// Обновили в УФК структуру TFF файлов
        /// добавили реквизит какой-то...
        /// у меня в массиве, начиная с 23 записи - сдвигать вперед!
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Zn"></param>
        /// na4alo_dannih - c с какой строки начинать искать данные - т.е. скока пропустить -служ инф
        /// exp - расширение
        public static void SaveXlsGam(string filename, string[,] Zn, int na4alo_dannih, string exp)
        {

            try
            {

                FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
                ExcelWriter writer = new ExcelWriter(stream);
                writer.BeginWrite();


                //            int cnt=0;//длина строки,чтобы прибавлять сл строчку

                int ii = 0;

                writer.WriteCell(ii, 0, "Номер документа");
                writer.WriteCell(ii, 1, "Дата документа");
                writer.WriteCell(ii, 2, "Сумма документа");
                writer.WriteCell(ii, 3, "ИНН плательщика");
                writer.WriteCell(ii, 4, "КПП плательщика");
                writer.WriteCell(ii, 5, "ИНН получателя");
                writer.WriteCell(ii, 6, "КПП получателя");
                writer.WriteCell(ii, 7, "Дата списания со счета плательщика");
                writer.WriteCell(ii, 8, "Наименование плательщика");
                writer.WriteCell(ii, 9, "Назначение платежа");
                writer.WriteCell(ii, 10, "Код бюджетной классификации");
                writer.WriteCell(ii, 11, "Срок платежа");

                ii = 1;//запоминать строку-перепрыгиваем же

                for (int i = 2; i < Zn.GetLength(0); i++)
                {

                    /* убрал и заменил 24,08,2015            	if (i%2==0 && ( StringHelper.GetNotNull(Zn[i,26])!="04611105012040000120" && StringHelper.GetNotNull(Zn[i,26])!="04611105024040000120" && StringHelper.GetNotNull(Zn[i,26])!="04611406012040000430" && StringHelper.GetNotNull(Zn[i,26])!="04611406024040000430"

                                                  ))//если четные */
                    if (i % 2 == 0 && !StringHelper.kbk_kaladyn.Contains(StringHelper.GetNotNull(Zn[i, 26])))
                    {
                        try
                        {
                            writer.WriteCell(ii, 0, StringHelper.GetNotNull(Zn[i, 0]));
                            writer.WriteCell(ii, 1, StringHelper.GetNotNull(Zn[i, 1]));
                            writer.WriteCell(ii, 2, Convert.ToDouble(StringHelper.GetNotNull(Zn[i, 2]).Replace(".", ",")));
                            //writer.WriteCell(ii, 2, StringHelper.GetNotNull(Zn[i,2]).Replace(".",","));
                            writer.WriteCell(ii, 3, StringHelper.GetNotNull(Zn[i, 7]));
                            writer.WriteCell(ii, 4, StringHelper.GetNotNull(Zn[i, 8]));
                            writer.WriteCell(ii, 5, StringHelper.GetNotNull(Zn[i, 14]));
                            writer.WriteCell(ii, 6, StringHelper.GetNotNull(Zn[i, 15]));
                            writer.WriteCell(ii, 7, StringHelper.GetNotNull(Zn[i, 5]));
                            writer.WriteCell(ii, 8, StringHelper.GetNotNull(Zn[i, 9]));
                            writer.WriteCell(ii, 9, StringHelper.GetNotNull(Zn[i, 24]));
                            writer.WriteCell(ii, 10, StringHelper.GetNotNull(Zn[i, 26]));
                            writer.WriteCell(ii, 11);

                            ii++;
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("Ошбика в строке " + i + "\r\n" + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }


                writer.EndWrite();
                stream.Close();

                //ПЕРЕСОХРАНЯЕМ!            
                Excel xlstmp = new Excel();
                xlstmp.OpenDocument(filename);
                xlstmp.SaveDocument(filename);
                xlstmp.CloseDocument();


                MessageBox.Show((ii - 1) + " сконвертированы в файл " + filename + " сохранен!", "save", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception strStream)
            {
                MessageBox.Show("Что-то с файлом " + filename + "\r\n" + strStream.Message + "\r\n" + strStream.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// СОхранять невыясненные
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Zn"></param>
        ///  <param name="na4alo_dannih">откуда начинать считывать данные - номер строки</param>
        public void saveNeviyasnXls(string filename, string[,] Zn, int na4alo_dannih)
        {
            //			Console.WriteLine("saveXls");

            //Microsoft.Office.Interop.Excel.Application xls=new ApplicationClass();
            //xls.Visible=true;
            try
            {

                FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
                ExcelWriter writer = new ExcelWriter(stream);
                writer.BeginWrite();

                //            int cnt=0;//длина строки,чтобы прибавлять сл строчку

                int ii = 0;

                writer.WriteCell(ii, 0, "Номер документа");
                writer.WriteCell(ii, 1, "Дата документа");
                writer.WriteCell(ii, 2, "Сумма документа");
                writer.WriteCell(ii, 3, "ИНН плательщика");
                writer.WriteCell(ii, 4, "КПП плательщика");
                writer.WriteCell(ii, 5, "ИНН получателя");
                writer.WriteCell(ii, 6, "КПП получателя");
                writer.WriteCell(ii, 7, "Код бюджетной классификации");
                writer.WriteCell(ii, 8, "Вид платежа");
                writer.WriteCell(ii, 9, "Поступило в банк плательщика");
                writer.WriteCell(ii, 10, "Дата списания со счета плательщика");
                writer.WriteCell(ii, 11, "Вид операции");
                writer.WriteCell(ii, 12, "Наименование плательщика");
                writer.WriteCell(ii, 13, "Расчетный счет плательщика");
                writer.WriteCell(ii, 14, "БИК банка плательщика");
                writer.WriteCell(ii, 15, "Наименование банка плательщика");
                writer.WriteCell(ii, 16, "Коррсчет банка плательщика");
                writer.WriteCell(ii, 17, "Наименование получателя");
                writer.WriteCell(ii, 18, "Расчетный счет получателя");
                writer.WriteCell(ii, 19, "БИК банка получателя");
                writer.WriteCell(ii, 20, "Наименование банка получателя");
                writer.WriteCell(ii, 21, "Коррсчет банка получателя");
                writer.WriteCell(ii, 22, "Очередность платежа");
                writer.WriteCell(ii, 23, "Статус составителя расчетного документа");
                writer.WriteCell(ii, 24, "Назначение платежа");
                writer.WriteCell(ii, 25, "Показатель основания платежа");
                writer.WriteCell(ii, 26, "Код ОКАТО");
                writer.WriteCell(ii, 27, "Показатель налогового периода");
                writer.WriteCell(ii, 28, "Показатель номера документа");
                writer.WriteCell(ii, 29, "Показатель даты документа");
                writer.WriteCell(ii, 30, "Тип платежа");
                writer.WriteCell(ii, 31, "Номер частичного платежа");
                writer.WriteCell(ii, 32, "Номер платежного документа");
                writer.WriteCell(ii, 33, "Дата платежного документа");
                writer.WriteCell(ii, 34, "Содержание операции");
                writer.WriteCell(ii, 35, "Глобальный идентификатор");
                writer.WriteCell(ii, 36, "Срок платежа");


                ii = 1;//запоминать строку-перепрыгиваем же

                for (int i = na4alo_dannih; i < Zn.GetLength(0); i++)
                {
                    //if (i%2==0 &&   - только четные строки 	12011105010040000120
                    if ((i % 2 == 0))
                    {

                        try
                        {

                            writer.WriteCell(ii, 0, StringHelper.GetNotNull(Zn[i - 1, 20]));
                            writer.WriteCell(ii, 1, StringHelper.GetNotNull(Zn[i - 1, 21]));
                            writer.WriteCell(ii, 2, Convert.ToDouble(StringHelper.GetNotNull(Zn[i - 1, 28]).Replace(".", ",")));
                            //writer.WriteCell(ii, 2, StringHelper.GetNotNull(Zn[i,2]).Replace(".",","));
                            writer.WriteCell(ii, 3, StringHelper.GetNotNull(Zn[i - 1, 12]));
                            writer.WriteCell(ii, 4, StringHelper.GetNotNull(Zn[i - 1, 13]));
                            writer.WriteCell(ii, 5, StringHelper.GetNotNull(Zn[i, 14]));
                            writer.WriteCell(ii, 6, StringHelper.GetNotNull(Zn[i, 15]));
                            writer.WriteCell(ii, 7, StringHelper.GetNotNull(Zn[i, 25]));
                            writer.WriteCell(ii, 8, StringHelper.GetNotNull(Zn[i, 3]));
                            writer.WriteCell(ii, 9, StringHelper.GetNotNull(Zn[i, 4], Zn[i, 1]));
                            writer.WriteCell(ii, 10, StringHelper.GetNotNull(Zn[i, 5], Zn[i, 1]));
                            writer.WriteCell(ii, 11, StringHelper.GetNotNull(Zn[i, 6]));
                            writer.WriteCell(ii, 12, StringHelper.GetNotNull(Zn[i, 9]));
                            writer.WriteCell(ii, 13, StringHelper.GetNotNull(Zn[i, 10]));
                            writer.WriteCell(ii, 14, StringHelper.GetNotNull(Zn[i, 11]));
                            writer.WriteCell(ii, 15, StringHelper.GetNotNull(Zn[i, 12]));
                            writer.WriteCell(ii, 16, StringHelper.GetNotNull(Zn[i, 13]));

                            writer.WriteCell(ii, 17, StringHelper.GetNotNull(Zn[i, 16]));
                            writer.WriteCell(ii, 18, StringHelper.GetNotNull(Zn[i, 17]));
                            writer.WriteCell(ii, 19, StringHelper.GetNotNull(Zn[i, 18]));
                            writer.WriteCell(ii, 20, StringHelper.GetNotNull(Zn[i, 19]));
                            writer.WriteCell(ii, 21);
                            writer.WriteCell(ii, 22);
                            writer.WriteCell(ii, 23, StringHelper.GetNotNull(Zn[i, 24]));
                            writer.WriteCell(ii, 24, StringHelper.GetNotNull(Zn[i, 23]));

                            writer.WriteCell(ii, 25, StringHelper.GetNotNull(Zn[i, 27]));
                            writer.WriteCell(ii, 26, StringHelper.GetNotNull(Zn[i, 26]));
                            writer.WriteCell(ii, 27, StringHelper.GetNotNull(Zn[i, 28]));
                            writer.WriteCell(ii, 28, StringHelper.GetNotNull(Zn[i, 29]));
                            writer.WriteCell(ii, 29, StringHelper.GetNotNull(Zn[i, 30]));
                            writer.WriteCell(ii, 30, StringHelper.GetNotNull(Zn[i, 31]));
                            writer.WriteCell(ii, 31);
                            writer.WriteCell(ii, 32);
                            writer.WriteCell(ii, 33);
                            writer.WriteCell(ii, 34);
                            writer.WriteCell(ii, 35, StringHelper.GetNotNull(Zn[i, 39]));

                            ii++;
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("Ошбика в строке " + i + " , " + Zn[i, 0] + "\r\n" + exc.Message + "\r\n" + exc.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }


                writer.EndWrite();
                stream.Close();

                //ПЕРЕСОХРАНЯЕМ!            
                Excel xlstmp = new Excel();
                xlstmp.OpenDocument(filename);
                xlstmp.SaveDocument(filename);
                xlstmp.CloseDocument();


                MessageBox.Show((ii - 1) + " сконвертированы в файл " + filename + " сохранен!", "save", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception strStream)
            {
                MessageBox.Show("Что-то с файлом " + filename + "\r\n" + strStream.Message + "\r\n" + strStream.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //////////////////////
        /// 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Zn"></param>
        public static void SavePosleViyasnXls(string filename, string[,] Zn)
        {

            try
            {

                FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
                ExcelWriter writer = new ExcelWriter(stream);
                writer.BeginWrite();

                //            int cnt=0;//длина строки,чтобы прибавлять сл строчку

                writer.WriteCell(0, 0, "Номер документа");
                writer.WriteCell(0, 1, "Дата документа");
                writer.WriteCell(0, 2, "Сумма документа");
                writer.WriteCell(0, 3, "ИНН плательщика");
                writer.WriteCell(0, 4, "КПП плательщика");
                writer.WriteCell(0, 5, "ИНН получателя");
                writer.WriteCell(0, 6, "КПП получателя");
                writer.WriteCell(0, 7, "Код бюджетной классификации");
                writer.WriteCell(0, 8, "Вид платежа");
                writer.WriteCell(0, 9, "Поступило в банк плательщика");
                writer.WriteCell(0, 10, "Дата списания со счета плательщика");
                writer.WriteCell(0, 11, "Вид операции");
                writer.WriteCell(0, 12, "Наименование плательщика");
                writer.WriteCell(0, 13, "Расчетный счет плательщика");
                writer.WriteCell(0, 14, "БИК банка плательщика");
                writer.WriteCell(0, 15, "Наименование банка плательщика");
                writer.WriteCell(0, 16, "Коррсчет банка плательщика");
                writer.WriteCell(0, 17, "Наименование получателя");
                writer.WriteCell(0, 18, "Расчетный счет получателя");
                writer.WriteCell(0, 19, "БИК банка получателя");
                writer.WriteCell(0, 20, "Наименование банка получателя");
                writer.WriteCell(0, 21, "Коррсчет банка получателя");
                writer.WriteCell(0, 22, "Очередность платежа");
                writer.WriteCell(0, 23, "Статус составителя расчетного документа");
                writer.WriteCell(0, 24, "Назначение платежа");
                writer.WriteCell(0, 25, "Показатель основания платежа");
                writer.WriteCell(0, 26, "Код ОКАТО");
                writer.WriteCell(0, 27, "Показатель налогового периода");
                writer.WriteCell(0, 28, "Показатель номера документа");
                writer.WriteCell(0, 29, "Показатель даты документа");
                writer.WriteCell(0, 30, "Тип платежа");
                writer.WriteCell(0, 31, "Номер частичного платежа");
                writer.WriteCell(0, 32, "Номер платежного документа");
                writer.WriteCell(0, 33, "Дата платежного документа");
                writer.WriteCell(0, 34, "Содержание операции");
                writer.WriteCell(0, 35, "Глобальный идентификатор");
                writer.WriteCell(0, 36, "Срок платежа");

                for (int i = 0; i < Zn.GetLength(0); i++)
                {

                    try
                    {
                        writer.WriteCell(i + 1, 0, StringHelper.GetNotNull(Zn[i, 3]));
                        writer.WriteCell(i + 1, 1, StringHelper.GetNotNull(Zn[i, 4].ToString()));//Дата документа
                        writer.WriteCell(i + 1, 2, Convert.ToDouble(StringHelper.GetNotNull(Zn[i, 14]).Replace(".", ",")));
                        writer.WriteCell(i + 1, 3, StringHelper.GetNotNull(Zn[i, 2]));
                        writer.WriteCell(i + 1, 4, StringHelper.GetNotNull(Zn[i, 6]));
                        writer.WriteCell(i + 1, 5, StringHelper.GetNotNull(Zn[i, 10]));
                        writer.WriteCell(i + 1, 6, StringHelper.GetNotNull(Zn[i, 11]));
                        writer.WriteCell(i + 1, 7, StringHelper.GetNotNull(Zn[i, 12]));
                        writer.WriteCell(i + 1, 8, "0");//вид платежа
                        writer.WriteCell(i + 1, 9, StringHelper.GetNotNull(Zn[i, 4], Zn[i, 1]));//Поступило в банк плательщика
                        writer.WriteCell(i + 1, 10, StringHelper.GetNotNull(Zn[i, 4], Zn[i, 1]));//Дата списания со счета плательщика
                        writer.WriteCell(i + 1, 11, "01");//Вид операции
                        writer.WriteCell(i + 1, 12, StringHelper.GetNotNull(Zn[i, 1]));
                        writer.WriteCell(i + 1, 13);
                        writer.WriteCell(i + 1, 14);
                        writer.WriteCell(i + 1, 15);
                        writer.WriteCell(i + 1, 16);

                        writer.WriteCell(i + 1, 17, StringHelper.GetNotNull(Zn[i, 9]));
                        writer.WriteCell(i + 1, 18);
                        writer.WriteCell(i + 1, 19, "046311001");//БИК банка получателя
                        writer.WriteCell(i + 1, 20, "ГРКЦ ГУ БАНКА РОССИИ ПО САРАТОВСКОЙ ОБЛ. Г. САРАТОВ");//"Наименование банка получателя"
                        writer.WriteCell(i + 1, 21);
                        writer.WriteCell(i + 1, 22);
                        writer.WriteCell(i + 1, 23);
                        writer.WriteCell(i + 1, 24, StringHelper.GetNotNull(Zn[i, 8]));//Назначение платежа

                        writer.WriteCell(i + 1, 25);
                        writer.WriteCell(i + 1, 26, StringHelper.GetNotNull(Zn[i, 15]));
                        writer.WriteCell(i + 1, 27, "0");//Показатель налогового периода
                        writer.WriteCell(i + 1, 28, "0");//Показатель номера документа
                        writer.WriteCell(i + 1, 29, StringHelper.GetNotNull(Zn[i, 4]));//Показатель даты документа
                        writer.WriteCell(i + 1, 30, StringHelper.GetNotNull(Zn[i, 7].Replace("Платежное поручение", "ПЛ")));//Тип платежа
                        writer.WriteCell(i + 1, 31);
                        writer.WriteCell(i + 1, 32);
                        writer.WriteCell(i + 1, 33);
                        writer.WriteCell(i + 1, 34);
                        writer.WriteCell(i + 1, 35, StringHelper.GetNotNull(Zn[i, 13]));
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Ошбика" + "\r\n" + exc.Message + "\r\n" + exc.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }


                writer.EndWrite();
                stream.Close();

                //ПЕРЕСОХРАНЯЕМ!            
                Excel xlstmp = new Excel();
                xlstmp.OpenDocument(filename);
                xlstmp.SaveDocument(filename);
                xlstmp.CloseDocument();


                MessageBox.Show(filename + " сохранен!", "save", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception strStream)
            {
                MessageBox.Show("Что-то с файлом " + filename + "\r\n" + strStream.Message + "\r\n" + strStream.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        /// <summary>
        /// файлы UFD после выяснения
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Zn"></param>
        public static void SavePosleViyasnXls(string filename, string[][][] Zn)
        {
            try
            {

                FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
                ExcelWriter writer = new ExcelWriter(stream);
                writer.BeginWrite();

                writer.WriteCell(0, 0, "Номер документа");
                writer.WriteCell(0, 1, "Дата документа");
                writer.WriteCell(0, 2, "Сумма документа");
                writer.WriteCell(0, 3, "ИНН плательщика");
                writer.WriteCell(0, 4, "КПП плательщика");
                writer.WriteCell(0, 5, "ИНН получателя");
                writer.WriteCell(0, 6, "КПП получателя");
                writer.WriteCell(0, 7, "Код бюджетной классификации");
                writer.WriteCell(0, 8, "Вид платежа");
                writer.WriteCell(0, 9, "Поступило в банк плательщика");
                writer.WriteCell(0, 10, "Дата списания со счета плательщика");
                writer.WriteCell(0, 11, "Вид операции");
                writer.WriteCell(0, 12, "Наименование плательщика");
                writer.WriteCell(0, 13, "Расчетный счет плательщика");
                writer.WriteCell(0, 14, "БИК банка плательщика");
                writer.WriteCell(0, 15, "Наименование банка плательщика");
                writer.WriteCell(0, 16, "Коррсчет банка плательщика");
                writer.WriteCell(0, 17, "Наименование получателя");
                writer.WriteCell(0, 18, "Расчетный счет получателя");
                writer.WriteCell(0, 19, "БИК банка получателя");
                writer.WriteCell(0, 20, "Наименование банка получателя");
                writer.WriteCell(0, 21, "Коррсчет банка получателя");
                writer.WriteCell(0, 22, "Очередность платежа");
                writer.WriteCell(0, 23, "Статус составителя расчетного документа");
                writer.WriteCell(0, 24, "Назначение платежа");
                writer.WriteCell(0, 25, "Показатель основания платежа");
                writer.WriteCell(0, 26, "Код ОКАТО");
                writer.WriteCell(0, 27, "Показатель налогового периода");
                writer.WriteCell(0, 28, "Показатель номера документа");
                writer.WriteCell(0, 29, "Показатель даты документа");
                writer.WriteCell(0, 30, "Тип платежа");
                writer.WriteCell(0, 31, "Номер частичного платежа");
                writer.WriteCell(0, 32, "Номер платежного документа");
                writer.WriteCell(0, 33, "Дата платежного документа");
                writer.WriteCell(0, 34, "Содержание операции");
                writer.WriteCell(0, 35, "Глобальный идентификатор");
                writer.WriteCell(0, 36, "Срок платежа");
                int row = 1;//строка с которой забивать данные, 0 - заголовой
                            //MessageBox.Show(Zn[0].Length + "\r\n" + Zn[0][0].Length);

                for (int i = 0; i < Zn.Length; i++)//перебираем все платежки
                    if (Zn[i] != null)//если текущий файл обработали  и не переименовали
                        for (int j = 0; j < Zn[i].Length; j++)//перебираем в каждой платежке записи
                        {
                            try
                            {
                                //Convert.ToDouble(StringHelper.GetNotNull(Zn[i,2]).Replace(".",","))
                                writer.WriteCell(row, 0, StringHelper.GetNotNull(Zn[i][j][0]));
                                writer.WriteCell(row, 1, StringHelper.GetNotNull(Zn[i][j][1]));
                                writer.WriteCell(row, 2, Convert.ToDouble(StringHelper.GetNotNull(Zn[i][j][2]).Replace(".", ",")));
                                writer.WriteCell(row, 3, StringHelper.GetNotNull(Zn[i][j][3]));
                                writer.WriteCell(row, 4, StringHelper.GetNotNull(Zn[i][j][4]));
                                writer.WriteCell(row, 5, StringHelper.GetNotNull(Zn[i][j][5]));
                                writer.WriteCell(row, 6, StringHelper.GetNotNull(Zn[i][j][6]));
                                writer.WriteCell(row, 7, StringHelper.GetNotNull(Zn[i][j][7]));
                                writer.WriteCell(row, 8, StringHelper.GetNotNull(Zn[i][j][8]));
                                writer.WriteCell(row, 9, StringHelper.GetNotNull(Zn[i][j][9], Zn[i][j][1]));
                                writer.WriteCell(row, 10, StringHelper.GetNotNull(Zn[i][j][10], Zn[i][j][1]));
                                writer.WriteCell(row, 11, StringHelper.GetNotNull(Zn[i][j][11]));
                                writer.WriteCell(row, 12, StringHelper.GetNotNull(Zn[i][j][12]));
                                writer.WriteCell(row, 13, StringHelper.GetNotNull(Zn[i][j][13]));
                                writer.WriteCell(row, 14, StringHelper.GetNotNull(Zn[i][j][14]));
                                writer.WriteCell(row, 15, StringHelper.GetNotNull(Zn[i][j][15]));
                                writer.WriteCell(row, 16, StringHelper.GetNotNull(Zn[i][j][16]));
                                writer.WriteCell(row, 17, StringHelper.GetNotNull(Zn[i][j][17]));
                                writer.WriteCell(row, 18, StringHelper.GetNotNull(Zn[i][j][18]));
                                writer.WriteCell(row, 19, StringHelper.GetNotNull(Zn[i][j][19]));
                                writer.WriteCell(row, 20, StringHelper.GetNotNull(Zn[i][j][20]));
                                writer.WriteCell(row, 21, StringHelper.GetNotNull(Zn[i][j][21]));
                                writer.WriteCell(row, 22, StringHelper.GetNotNull(Zn[i][j][22]));
                                writer.WriteCell(row, 23, StringHelper.GetNotNull(Zn[i][j][23]));
                                writer.WriteCell(row, 24, StringHelper.GetNotNull(Zn[i][j][24]));
                                writer.WriteCell(row, 25, StringHelper.GetNotNull(Zn[i][j][25]));
                                writer.WriteCell(row, 26, StringHelper.GetNotNull(Zn[i][j][26]));
                                writer.WriteCell(row, 27, StringHelper.GetNotNull(Zn[i][j][27]));
                                writer.WriteCell(row, 28, StringHelper.GetNotNull(Zn[i][j][28]));
                                writer.WriteCell(row, 29, StringHelper.GetNotNull(Zn[i][j][29]));
                                writer.WriteCell(row, 30, StringHelper.GetNotNull(Zn[i][j][30]));
                                writer.WriteCell(row, 31, StringHelper.GetNotNull(Zn[i][j][31]));
                                writer.WriteCell(row, 32, StringHelper.GetNotNull(Zn[i][j][32]));
                                writer.WriteCell(row, 33, StringHelper.GetNotNull(Zn[i][j][33]));
                                writer.WriteCell(row, 34, StringHelper.GetNotNull(Zn[i][j][34]));
                                writer.WriteCell(row, 35, StringHelper.GetNotNull(Zn[i][j][35]));
                                writer.WriteCell(row, 36, StringHelper.GetNotNull(Zn[i][j][36]));


                                row++;
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("Ошбика" + "\r\n" + exc.Message + "\r\n" + exc.Source + "\r\n" + exc.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }


                writer.EndWrite();
                stream.Close();

                //ПЕРЕСОХРАНЯЕМ!            
                Excel xlstmp = new Excel();
                xlstmp.OpenDocument(filename);
                xlstmp.SaveDocument(filename);
                xlstmp.CloseDocument();


                MessageBox.Show(filename + " сохранен!", "save", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception strStream)
            {
                MessageBox.Show("Что-то с файлом " + filename + "\r\n" + strStream.Message + "\r\n" + strStream.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
