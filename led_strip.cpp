// https://www.tweaking4all.com/hardware/arduino/adruino-led-strip-effects/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

{
    class Led
    {

        private const int led_count = 150;//общее кол-во светодиодов в ленте
        private const int led_segment_lenght = 25;//кол-во светодиодов в 1 отрезке - длина
        private const int led_segment_count = 6;//led_count / led_segment_lenght;// 6;//кол-во отрезков
        private static int[,] led_srip; //Массив индексов светодиодов c# int[,] led_srip = new int[4, 5]; c++ int led_srip[6][25];

        public Led()
        {
            InitializeLed();

            //Зажечь 1 ряд - вернуть номера нужных элементов
            var row0 = GetLedRow(0);
            var row24 = GetLedRow(24);
            var col0 = GetLedCol(0);
            var col5 = GetLedCol(5);
            int[] diag0 = GetLedDiagonal(0);

            WriteArray(diag0);

            WriteArray(led_srip);
            WriteArray(row0);
            WriteArray(row24);

            WriteArray(col0);
            WriteArray(col5);


            //LEDS_SATURATION — максимальная цветовая насыщенность
            //LEDS_BRIGHTNESS — максимальная яркость(на 60 светодиодах примерное потребление получилось 800 мА при яркости 255, 400 мА при 128 и 200 мА при 64)
            //hueStep — шаг изменения цвета за один цикл
            //UPDATE_DELAY — скорость «бега» цвета по ленте
        }

        /// <summary>
        /// диагональ
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private int[] GetLedDiagonal(int r)
        {
            int[] led_ = new int[led_segment_lenght];
            int r_ = 0;
            for (int w = 0; w < led_segment_lenght/*led_srip.GetLength(1)*/; w++)
            {
                if (r_ >= led_segment_count)
                    r_ = 0;//обнуляем, если дошли до конца строки-на нов строку

                //if (w == 0)
                led_[w] = led_srip[r_, w];

                // Console.Write($"{} ");

                r_++;
            }
            //Console.WriteLine();

            return led_;
        }

        private void WriteArray(object o)
        {
            Console.WriteLine(o.GetType());
            if (o.GetType() == typeof(int[]))
            {
                foreach (int index in (int[])o)
                    Console.Write($"{index} ");
                Console.WriteLine();
            }
            else if (o.GetType() == typeof(int[,]))
            {
                var arr = (int[,])o;

                Console.WriteLine($"arr size: {arr.GetLength(0)};{arr.GetLength(1)}");

                if (arr.Rank == 2)//размерность=2 - двумерный
                    for (int h = 0; h < arr.GetLength(0); h++)
                    {
                        for (int w = 0; w < arr.GetLength(1); w++)
                        {
                            Console.Write($"{arr[h, w]} ");
                        }
                        Console.WriteLine();
                    }
            }
        }

        /// <summary>
        /// Зажечь 1 ряд - вернуть номера нужных элементов строки
        /// </summary>
        /// <param name="row_num">номер строки</param>
        /// <returns></returns>
        private int[] GetLedRow(int row_num)
        {
            int[] row_ = new int[led_segment_count];//по кол-ву отрезков
            if (row_num < led_segment_lenght)
                for (int h = 0; h < led_segment_count; h++)
                    row_[h] = led_srip[h, row_num];
            return row_;
        }

        /// <summary>
        /// вернуть номера нужных элементов столбца 
        /// </summary>
        /// <param name="col_num">Номер столбца/отрезка</param>
        /// <returns></returns>
        private int[] GetLedCol(int col_num)
        {
            int[] col_ = new int[led_segment_lenght];//по кол-ву отрезков
            if (col_num < led_segment_count)
                for (int w = 0; w < led_segment_lenght; w++)
                    col_[w] = led_srip[col_num, w];
            return col_;
        }

        private static void InitializeLed(bool debug = false)
        {
            led_srip = new int[led_segment_count, led_segment_lenght];
            int item = 0;
            for (int h = 0; h < led_segment_count; h++)
            {
                for (int w = 0; w < led_segment_lenght; w++)
                {
                    //для четных строк - элементы попорядку
                    if (h % 2 == 0)
                    {
                        item = (h == 0) ? w : led_segment_lenght * h + w + 1;
                    }
                    else //для HEчетных строк - элементы в обратн порядке
                    {
                        int max_in_segment = led_segment_lenght * (h + 1);//25-50-75-100-125-150
                        item = max_in_segment - (w);
                    }

                    if (debug)
                        Console.Write($"{item} ");

                    if (item <= led_count)
                        led_srip[h, w] = item;//led_srip[h][w] = item;
                    else
                        throw new Exception("led strip lengh < this item");
                }

                if (debug)
                    Console.WriteLine();
            }
        }


    }
}
