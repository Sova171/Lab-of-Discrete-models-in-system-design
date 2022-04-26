using System;
using System.IO;
using System.Linq;

namespace Lab5_Izomorfizm_
{
    class Program
    {
        private static void IzomorfizmOfGraphs(int[,] graf1, int[,] graf2)
        {
            //Перевірка чи графи мають однакову к-сть вершин та к-сть не нульових ребер
            if (graf1.GetLength(0) != graf2.GetLength(0) || graf1.GetLength(1) != graf2.GetLength(1) || CountOfEdges(graf1) != CountOfEdges(graf2))
            {
                Console.WriteLine("\nGraphs are NOT isomorphic");
            }

            //Перевірка чи є однакові рядки з однаковими значеннями
            int[] result_of_str = CountSameValueOfStr(graf1, graf2);
            int[] result_of_row = CountSameValueOfRow(graf1, graf2);
            bool flag = false;
            for (int i = 0; i < result_of_str.Length; i++)
            {
                if (result_of_str[i] == 0 || result_of_row[i] == 0)
                {
                    flag = true;
                    break;
                }
            }

            if (flag)
            {
                Console.WriteLine("\nGraphs are NOT isomorphic");
            }
            else
            {
                Console.WriteLine("\nGraphs are isomorphic");
            }
            
        }
        private static int[][] ComponentOfStr(int[,] array)
        {
            int[][] mas_component = new int[array.GetLength(0)][];
            for (int i = 0; i < mas_component.GetLength(0); i++)
            {
                mas_component[i] = new int[array.GetLength(1)];
            }

            for (int j = 0; j < mas_component.Length; j++)
            {
                for (int g = 0; g < mas_component[j].Length; g++)
                {
                    mas_component[j][g] = array[j, g];
                }
            }
            return mas_component;
        }
        private static int[][] ComponentOfRow(int[,] array)
        {
            int[][] mas_component = new int[array.GetLength(0)][];
            for (int i = 0; i < mas_component.GetLength(0); i++)
            {
                mas_component[i] = new int[array.GetLength(1)];
            }

            for (int j = 0; j < mas_component.Length; j++)
            {
                for (int g = 0; g < mas_component[j].Length; g++)
                {
                    mas_component[j][g] = array[g, j];
                }
            }
            return mas_component;
        }
        private static int[] CountSameValueOfStr(int[,] graf1, int[,] graf2)
        {
            int[] result = new int[graf1.GetLength(0)];
            int[][] mas_str1 = ComponentOfStr(graf1);
            int[][] mas_str2 = ComponentOfStr(graf2);
            for (int i = 0; i < mas_str1.Length; i++)
            {
                mas_str1[i] = BubbleSort(mas_str1[i]);
                mas_str2[i] = BubbleSort(mas_str2[i]);
            }

            for (int i = 0; i < mas_str1.Length; i++)
            {
                
                int count = 0;
                for (int j = 0; j < mas_str1.Length; j++)
                {
                    bool flag = true;
                    for (int g = 0; g < mas_str1[j].Length; g++)
                    {
                        if (mas_str1[i][g] != mas_str2[j][g])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        count++;
                    }
                }
                result[i] = count;
            }
            return result;
        }
        private static int[] CountSameValueOfRow(int[,] graf1, int[,] graf2)
        {
            int[] result = new int[graf1.GetLength(0)];
            int[][] mas_str1 = ComponentOfRow(graf1);
            int[][] mas_str2 = ComponentOfRow(graf2);
            for (int i = 0; i < mas_str1.Length; i++)
            {
                mas_str1[i] = BubbleSort(mas_str1[i]);
                mas_str2[i] = BubbleSort(mas_str2[i]);
            }

            for (int i = 0; i < mas_str1.Length; i++)
            {
                int count = 0;
                for (int j = 0; j < mas_str1.Length; j++)
                {
                    bool flag = true;
                    for (int g = 0; g < mas_str1[j].Length; g++)
                    {
                        if (mas_str1[i][g] != mas_str2[j][g])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        count++;
                    }
                }
                result[i] = count;
            }
            return result;
        }       

        //метод обміну елементів
        static void Swap(ref int e1, ref int e2)
        {
            var temp = e1;
            e1 = e2;
            e2 = temp;
        }

        //сортування бульбашкою
        static int[] BubbleSort(int[] array)
        {
            var len = array.Length;
            for (var i = 1; i < len; i++)
            {
                for (var j = 0; j < len - i; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        Swap(ref array[j], ref array[j + 1]);
                    }
                }
            }

            return array;
        }
        private static int CountOfEdges(int[,] graf1)
        {
            int count = 0;
            for (int i = 0; i < graf1.GetLength(0); i++)
            {
                for (int j = 0; j < graf1.GetLength(1); j++)
                {
                    if (graf1[i, j] == 0)
                    {
                        continue;
                    }
                    else
                    {
                        count++;
                    }
                }
            }
            return count;
        }
       
        static void Main(string[] args)
        {
            //Initialize graf1
            string[] s = File.ReadAllLines("l5-1.txt");

            string[,] num = new string[s.Length, s[0].Split(' ').Length];
            for (int i = 0; i < s.Length; i++)
            {
                string[] temp = s[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    num[i, j] = temp[j];
            }

            int[,] mas_graf1 = new int[num.GetLength(0), num.GetLength(1)];
            for (int i = 0; i < num.GetLength(0); i++)
            {
                for (int j = 0; j < num.GetLength(1); j++)
                {
                    mas_graf1[i, j] = Convert.ToInt32(num[i, j]);
                }
            }
            //Print graf1
            Console.WriteLine("\nGraf1 matrix");
            for (int i = 0; i < mas_graf1.GetLength(0); i++)
            {
                for (int j = 0; j < mas_graf1.GetLength(1); j++)
                    Console.Write(mas_graf1[i, j] + "\t");
                Console.WriteLine();
            }



            //Initialize graf2
            string[] s1 = File.ReadAllLines("l5-2.txt");

            string[,] num1 = new string[s1.Length, s1[0].Split(' ').Length];
            for (int i = 0; i < s1.Length; i++)
            {
                string[] temp = s1[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    num1[i, j] = temp[j];
            }

            int[,] mas_graf2 = new int[num1.GetLength(0), num1.GetLength(1)];
            for (int i = 0; i < num1.GetLength(0); i++)
            {
                for (int j = 0; j < num1.GetLength(1); j++)
                {
                    mas_graf2[i, j] = Convert.ToInt32(num1[i, j]);
                }
            }

            //Print graf2            
            Console.WriteLine("\nGraf2 matrix");
            for (int i = 0; i < mas_graf2.GetLength(0); i++)
            {
                for (int j = 0; j < mas_graf2.GetLength(1); j++)
                    Console.Write(mas_graf2[i, j] + "\t");
                Console.WriteLine();
            }            
            IzomorfizmOfGraphs(mas_graf1, mas_graf2);
            Console.ReadKey();
        }
    }
}
