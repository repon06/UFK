using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace ufk
{
    public class Excel
    {
        public const string UID = "Excel.Application";
        object oExcel = null;
        object WorkBooks, WorkBook, WorkSheets, WorkSheet, Range;
        /// <summary>
        /// Открывает ексель файл по заданному пути
        /// </summary>
        /// <param name="Path"></param>
        public void ShowExcelWindow(string path)
        {
            Process.Start(path);
        }
        /// <summary>
        /// Открывает выбранныq ексель файл
        /// </summary>
        /// <param name="Path"></param>
        public void ShowExcelWindow()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();
            string path = open.FileName;
            Process.Start(path);
        }
               
        /// <summary>
        /// КОНСТРУКТОР КЛАССА
        /// </summary>
        public Excel()
        {
            oExcel = Activator.CreateInstance(Type.GetTypeFromProgID(UID));
        }

        /// <summary>
        /// ВИДИМОСТЬ EXCEL - СВОЙСТВО КЛАССА
        /// </summary>
        public bool Visible
        {
            set
            {
                if (false == value)
                    oExcel.GetType().InvokeMember("Visible", BindingFlags.SetProperty,
                        null, oExcel, new object[] { false });
                else
                    oExcel.GetType().InvokeMember("Visible", BindingFlags.SetProperty,
                        null, oExcel, new object[] { true });
            }
        }

        /// <summary>
        /// ОТКРЫТЬ ДОКУМЕНТ
        /// </summary>
        /// <param name="name"></param>
        public void OpenDocument(string name)
        {
            WorkBooks = oExcel.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, oExcel, null);
            WorkBook = WorkBooks.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, WorkBooks, new object[] { name, true });
            WorkSheets = WorkBook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, WorkBook, null);
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { 1 });
        }

        /// <summary>
        /// НОВЫЙ ДОКУМЕНТ
        /// </summary>
        public void NewDocument()
        {
            WorkBooks = oExcel.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, oExcel, null);
            WorkBook = WorkBooks.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, WorkBooks, null);
            WorkSheets = WorkBook.GetType().InvokeMember("Worksheets", BindingFlags.GetProperty, null, WorkBook, null);
            WorkSheet = WorkSheets.GetType().InvokeMember("Item", BindingFlags.GetProperty, null, WorkSheets, new object[] { 1 });
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, WorkSheet, new object[1] { "A1" });
        }

        /// <summary>
        /// ЗАКРЫТЬ ДОКУМЕНТ
        /// </summary>
        public void CloseDocument()
        {
            WorkBook.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, WorkBook, new object[] { true });
        }

        /// <summary>
        /// СОХРАНИТЬ ДОКУМЕНТ
        /// </summary>
        /// <param name="name"></param>
        public void SaveDocument(string name)
        {
            if (File.Exists(name))
                WorkBook.GetType().InvokeMember("Save", BindingFlags.InvokeMethod, null,
                    WorkBook, null);
            else
                try
                {
                    WorkBook.GetType().InvokeMember("SaveAs", BindingFlags.InvokeMethod, null,
                        WorkBook, new object[] { name });
                }
                catch
                {  }
        }  

        /// <summary>
        /// ЗАПИСАТЬ ЗНАЧЕНИЕ В ЯЧЕЙКУ
        /// </summary>
        /// <param name="range"></param>
        /// <param name="value"></param>
        public void SetValue(string range, string value)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            value = value.Replace("=", "-");
            Range.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, Range, new object[] { value });
        }

        /// <summary>
        /// ОБЪЕДЕНИТЬ ЯЧЕЙКИ - Alignment - ВЫРАВНИВАНИЕ В ОБЪЕДИНЕННЫХ ЯЧЕЙКАХ
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Alignment"></param>
        public void SetMerge(string range, int Alignment)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, WorkSheet, new object[] { range });
            object[] args = new object[] { Alignment };
            Range.GetType().InvokeMember("MergeCells", BindingFlags.SetProperty, null, Range, new object[] { true });
            Range.GetType().InvokeMember("HorizontalAlignment", BindingFlags.SetProperty, null, Range, args);
        }

        /// <summary>
        /// УСТАНОВИТЬ ОРИЕНТАЦИЮ СТРАНИЦЫ 1 - КНИЖНЫЙ 2 - АЛЬБОМНЫЙ
        /// </summary>
        /// <param name="Orientation"></param>
        public void SetOrientation(int Orientation)
        {
            //Range.Interior.ColorIndex
            object PageSetup = WorkSheet.GetType().InvokeMember("PageSetup", BindingFlags.GetProperty,
                null, WorkSheet, null);

            PageSetup.GetType().InvokeMember("Orientation", BindingFlags.SetProperty,
                null, PageSetup, new object[] { Orientation });
        }

        /// <summary>
        /// УСТАНОВИТЬ ШИРИНУ СТОЛБЦОВ
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Width"></param>
        public void SetColumnWidth(string range, double Width)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Width };
            Range.GetType().InvokeMember("ColumnWidth", BindingFlags.SetProperty, null, Range, args);
        }

        /// <summary>
        /// УСТАНОВИТЬ ВЫСОТУ СТРОК
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Height"></param>
        public void SetRowHeight(string range, double Height)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            if (Height > 400)
                Height = 409;//Максимальное значение
            object[] args = new object[] { Height };
            Range.GetType().InvokeMember("RowHeight", BindingFlags.SetProperty, null, Range, args);
        }

        /// <summary>
        /// УСТАНОВИТЬ ВИД РАМКИ ВОКРУГ ЯЧЕЙКИ
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Style"></param>
        public void SetBorderStyle(string range, int Style)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty, null, WorkSheet, new object[] { range });
            object[] args = new object[] { Style };
            object[] args1 = new object[] { 1 };
            object Borders = Range.GetType().InvokeMember("Borders", BindingFlags.GetProperty, null, Range, null);
            Borders = Range.GetType().InvokeMember("LineStyle", BindingFlags.SetProperty, null, Borders, args);
        }

        /// <summary>
        /// ЧТЕНИЕ ДАННЫХ ИЗ ВЫБРАННОЙ ЯЧЕЙКИ
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public string GetValue(string range)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            try
            {
                return Range.GetType().InvokeMember("Value", BindingFlags.GetProperty,
                    null, Range, null).ToString();
            }
            catch { return ""; }
        }

        /// <summary>
        /// УСТАНОВИТЬ ВЫРАВНИВАНИЕ В ЯЧЕЙКЕ ПО ВЕРТИКАЛИ
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Alignment"></param>
        public void SetVerticalAlignment(string range, int Alignment)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Alignment };
            Range.GetType().InvokeMember("VerticalAlignment", BindingFlags.SetProperty, null, Range, args);
        }

        /// <summary>
        /// УСТАНОВИТЬ ВЫРАВНИВАНИЕ В ЯЧЕЙКЕ ПО ГОРИЗОНТАЛИ
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Alignment"></param>
        public void SetHorisontalAlignment(string range, int Alignment)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Alignment };
            Range.GetType().InvokeMember("HorizontalAlignment", BindingFlags.SetProperty, null, Range, args);
        }

        /// <summary>
        /// ФОРМАТИРОВАНИЕ УКАЗАННОГО ТЕКСТА В ЯЧЕЙКЕ
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Start"></param>
        /// <param name="Length"></param>
        /// <param name="Color"></param>
        /// <param name="FontStyle"></param>
        /// <param name="FontSize"></param>
        public void SelectText(string range, int Start, int Length, int Color, string FontStyle, int FontSize)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Start, Length };
            object Characters = Range.GetType().InvokeMember("Characters", BindingFlags.GetProperty, null, Range, args);
            object Font = Characters.GetType().InvokeMember("Font", BindingFlags.GetProperty, null, Characters, null);
            Font.GetType().InvokeMember("ColorIndex", BindingFlags.SetProperty, null, Font, new object[] { Color });
            Font.GetType().InvokeMember("FontStyle", BindingFlags.SetProperty, null, Font, new object[] { FontStyle });
            Font.GetType().InvokeMember("Size", BindingFlags.SetProperty, null, Font, new object[] { FontSize });

        }

        /// <summary>
        /// УСТАНОВКА НАПРАВЛЕНИЯ ТЕКСТА
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Orientation"></param>
        public void SetTextOrientation(string range, int Orientation)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Orientation };
            Range.GetType().InvokeMember("Orientation", BindingFlags.SetProperty, null, Range, args);
        }

        /// <summary>
        /// ПЕРЕНОС СЛОВ В ЯЧЕЙКЕ
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Value"></param>
        public void SetWrapText(string range, bool Value)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object[] args = new object[] { Value };
            Range.GetType().InvokeMember("WrapText", BindingFlags.SetProperty, null, Range, args);
        }

        /// <summary>
        /// Установить цвет ячейки
        /// </summary>
        /// <param name="range"></param>
        /// <param name="Value"></param>
        public void SetColor(string range, int sColor)
        {
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });
            object oInterior = Range.GetType().InvokeMember("Interior", BindingFlags.GetProperty, null, Range, null);
            oInterior.GetType().InvokeMember("ColorIndex", BindingFlags.SetProperty, null, oInterior, new object[] { sColor });
            oInterior.GetType().InvokeMember("Pattern", BindingFlags.SetProperty, null, oInterior, new object[] { 1 });
        }

        /// <summary>
        /// ПРИМЕНЕНИЕ ШРИФТА К ЯЧЕЙКЕ
        /// </summary>
        /// <param name="range"></param>
        /// <param name="font"></param>
        public void SetFont(string range, Font font)
        {
            //Range.Font.Name
            Range = WorkSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, WorkSheet, new object[] { range });

            object Font = Range.GetType().InvokeMember("Font", BindingFlags.GetProperty,
                null, Range, null);

            Range.GetType().InvokeMember("Name", BindingFlags.SetProperty, null,
                Font, new object[] { font.Name });

            Range.GetType().InvokeMember("Size", BindingFlags.SetProperty, null,
                Font, new object[] { font.Size });
        }       
    }
}

