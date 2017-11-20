using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ufk.Helper
{
    /// <summary>
    /// Массив для 
    /// Изменения размера двухмерного массива
    /// </summary>
    class Array2
    {   // Изменение размера двухмерного массива, где a и b - новые размеры массива
        public static void Resize<T>(ref T[,] arr, int a, int b)
        {   //создаем временный массив
            T[,] tmp = new T[a, b];
            int c = arr.GetLength(0);
            int d = arr.GetLength(1);
            for (int i = 0; i < a; i++)
            {   //переход по элементам
                for (int j = 0; j < b; j++)
                {   //чтобы не выйти за пределы исходного массива, если увеличиваем
                    if (i < c && j < d)
                        tmp[i, j] = arr[i, j]; //перемещаем элементы во временный массив
                    else
                        tmp[i, j] = default(T); //если увеличиваем, то заполняем массив элементами по умолчанию
                }
            }
            arr = tmp; //перемещаем обратно в исходный массив
        }
    }

}
