using System;


namespace _3kauto.Helpers
{
    class Led
    {

        // https://github.com/bruhautomation/ESP-MQTT-JSON-Digital-LEDs
        // Recommended by Adafruit NeoPixel "Best Practices" to help protect LEDs from current onrush...
        // https://github.com/jasoncoon/esp8266-fastled-webserver
        // https://github.com/atuline/FastLED-Demos
        // https://www.tweaking4all.com/hardware/arduino/adruino-led-strip-effects/
        // выполн кода без delay - в FastLed -> EVERY_N_MILLISECONDS (200) {...выполнять код через кажд 200 сек}
        //возможно надо возвращать не двумерный массив, а одномерный-с коорд нужных пикс


        //LEDS_SATURATION — максимальная цветовая насыщенность
        //LEDS_BRIGHTNESS — максимальная яркость(на 60 светодиодах примерное потребление получилось 800 мА при яркости 255, 400 мА при 128 и 200 мА при 64)
        //hueStep — шаг изменения цвета за один цикл
        //UPDATE_DELAY — скорость «бега» цвета по ленте
        const int led_count = 150;//общее кол-во светодиодов в ленте
        const int led_segment_lenght = 25;//кол-во светодиодов в 1 отрезке - длина
        const int led_segment_count = 6;//led_count / led_segment_lenght;// 6;//кол-во отрезков
        static int[,] led_srip; //Массив индексов светодиодов c# int[,] led_srip = new int[4, 5]; c++ int led_srip[6][25];

        public Led()
        {
            InitializeLed();

            //Зажечь 1 ряд - вернуть номера нужных элементов
            var row0 = GetLedRow(0);
            var row24 = GetLedRow(24);
            var col0 = GetLedCol(0);
            var col5 = GetLedCol(5);
            int[] diag0 = GetLedDiagonal(0);
            int[] diag1 = GetLedDiagonal(1);
            int[] diag4 = GetLedDiagonal(4);

            int[] diag4l = GetLedDiagonal(4, false);

            WriteArray(diag0, "диагональ 0");
            WriteArray(diag1, "диагональ 1");
            WriteArray(diag4, "диагональ 4");
            WriteArray(diag4l, "диагональ 4L");

            WriteArray(led_srip,"strip");
            WriteArray(row0);
            WriteArray(row24);

            WriteArray(col0);
            WriteArray(col5);



        }

        /// <summary>
        /// установить цвет пикселя
        /// </summary>
        /// <param name="Pixel"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        void setPixel(int Pixel, byte red, byte green, byte blue)
        {
        # ifdef ADAFRUIT_NEOPIXEL_H 
            // NeoPixel
            strip.setPixelColor(Pixel, strip.Color(red, green, blue));
        #endif
        # ifndef ADAFRUIT_NEOPIXEL_H 
            // FastLED
            leds[Pixel].r = red;
            leds[Pixel].g = green;
            leds[Pixel].b = blue;
        #endif
        }

        /// <summary>
        /// поставить по всей ленте 1 цвет
        /// помогает при обнулении черным
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        void setAll(byte red, byte green, byte blue)
        {
            for (int i = 0; i < NUM_LEDS; i++)
            {
                setPixel(i, red, green, blue);
            }
            showStrip();
        }

        /// <summary>
        /// поставить нужный цвет в массиве
        /// </summary>
        /// <param name="ledArr"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        void setArrayLed(int[] ledArr, byte red, byte green, byte blue) {
            for (int i = 0; i < sizeof(ledArr); i++) {
                setPixel(i, red, green, blue);
            }
            showStrip();
        }

        /// <summary>
        /// поставить нужный цвет в массиве с задержкой
        /// </summary>
        /// <param name="ledArr"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        void setArrayLed(int[] ledArr, byte red, byte green, byte blue, uint8_t wait)
        {
            EVERY_N_MILLISECONDS(wait)
            for (int i = 0; i < sizeof(ledArr); i++)
            {
                setPixel(i, red, green, blue);
                //delay(wait);
                showStrip();
            }
        }

        /// <summary>
        /// диагональ
        /// +добавить - прав/лев
        /// </summary>
        /// <param name="r"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private int[] GetLedDiagonal(int r, bool right = true)
        {
            int[] led_ = new int[led_segment_lenght];
            int r_ = r;
            for (int w = 0; w < led_segment_lenght/*led_srip.GetLength(1)*/; w++)
            {
                if (r_ >= led_segment_count)
                    r_ = 0;//обнуляем, если дошли до конца строки-на нов строку
                if (r_ < 0)
                    r_ = led_segment_count - 1;//обнуляем, если дошли до конца строки-на нов строку

                led_[w] = led_srip[r_, w];

                if (right)
                    r_++;
                else
                    r_--;
            }
            //Console.WriteLine();

            return led_;
        }

        private void WriteArray(object o, string text = null, bool debug = false)
        {
            if (!string.IsNullOrEmpty(text))
                Console.WriteLine($"{text}: ");

            if (debug)
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

                if (debug)
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
