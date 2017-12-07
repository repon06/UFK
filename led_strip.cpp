        private const int led_count = 150;//общее кол-во светодиодов в ленте
        private const int led_segment_lenght = 25;//кол-во светодиодов в 1 отрезке - длина
        private const int led_segment_count = 6;//led_count / led_segment_lenght;// 6;//кол-во отрезков
        private static int[,] led_srip; //c# int[,] myArr = new int[4, 5]; c++ int led_srip[6][25];

        private static void InitializeLed()
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

                    Console.Write($"{item} ");
                    if (item <= led_count)
                        led_srip[h, w] = item;//led_srip[h][w] = item;
                    else
                        throw new Exception("led strip lengh < this item");
                }
                Console.WriteLine();
            }
        }
