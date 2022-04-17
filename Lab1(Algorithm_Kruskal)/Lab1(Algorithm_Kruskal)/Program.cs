using System;
using System.IO;
using System.Linq;

namespace Lab1_Algorithm_Kruskal_
{
    class Program
    {
        private static void Kruskal(int[,] mas_kruskal)
        {
            Console.WriteLine("\nEdges and tops of minimum spanning tree");
            int result = 0;
            int[][] component = Component(mas_kruskal);
            while (component[0].Length != mas_kruskal.GetLength(0))
            {
                int min = Min(mas_kruskal);
                bool flag = false;                
                for (int i = 0; i < mas_kruskal.GetLength(0); i++)
                {
                    for (int j = 0; j < mas_kruskal.GetLength(1); j++)
                    {
                        if (mas_kruskal[i, j] == min)
                        {
                            int first_merge = FindComponent(i, component);
                            int second_merge = FindComponent(j, component);
                            if (first_merge != second_merge)
                            {
                                component = Merge(component, first_merge, second_merge);
                                result += mas_kruskal[i, j];
                                mas_kruskal[i, j] = 0;
                                flag = true;
                                Console.WriteLine("Top({0})-- {1} --Top({2})", i+1, min, j+1); 
                            }
                            else
                            {
                                mas_kruskal[i, j] = 0;
                                flag = true;
                            }
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }                
            }
            Console.WriteLine("\nMinimum spanning tree result: " + result);
        }
        private static int Min(int[,] array)
        {
            int min = 1000;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == 0)
                    {
                        continue;
                    }
                    min = array[i, j];
                    break;
                }
            }

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == 0)
                    {
                        continue;
                    }
                    else
                    {
                       if (min > array[i, j])
                        {
                           min = array[i, j];
                        }
                    }
                }
            }
            return min;
            
        }

        private static int FindComponent(int top, int[][] mas_component)
        {
            int number_of_component = -1;
            for (int i = 0; i < mas_component.GetLength(0); i++)
            {
                for (int j = 0; j < mas_component[i].Length; j++)
                {
                    if (mas_component[i][j] == top)
                    {
                        number_of_component = i;
                        break;
                    }
                    
                }
                if (number_of_component != -1)
                {
                    break;
                }
            }
            return number_of_component;
        }

        private static int[][] Component(int[,] array)
        {
            int[][] mas_component = new int[array.GetLength(0)][];
            for (int i = 0; i < mas_component.GetLength(0); i++)
            {
                mas_component[i] = new int[] { i };
            }
            return mas_component;
        }
        private static int[] MergeTwoArray(int[] first_array, int[] second_array)
        {
            first_array = first_array.Concat(second_array).ToArray();
            return first_array;
        }

        private static int[][] Merge(int[][] array, int first_component, int second_component)
        {
            int temp;
            if (first_component > second_component)
            {
                temp = second_component;
                second_component = first_component;
                first_component = temp;
            }
            int[][] mas_component = new int[array.GetLength(0) - 1][];
            int[] merge = MergeTwoArray(array[first_component], array[second_component]);
            for (int i = 0; i < first_component; i++)
            {
                mas_component[i] = array[i];
            }
            mas_component[first_component] = merge;
            for (int i = first_component + 1, j = i; i < mas_component.Length; i++, j++)
            {
                if (i == second_component)
                {
                    j++;
                    //continue;
                }
                mas_component[i] = array[j];
            }
            return mas_component;
        }
        static void Main(string[] args)
        {
            string[] s = File.ReadAllLines("l1_1.txt");
            s = s.Skip(1).ToArray();

            string[,] num = new string[s.Length, s[0].Split(' ').Length];
            for (int i = 0; i < s.Length; i++)
            {
                string[] temp = s[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    num[i, j] = temp[j];
            }

            int[,] mas_kruskal = new int[num.GetLength(0), num.GetLength(1)];
            for (int i = 0;  i < num.GetLength(0); i++)
            {
                for (int j = 0; j < num.GetLength(1); j++)
                {
                    mas_kruskal[i, j] = Convert.ToInt32(num[i,j]);
                }                   
            }

            Console.WriteLine("Adjacency matrix");
            for (int i = 0; i < mas_kruskal.GetLength(0); i++)
            {
                for (int j = 0; j < mas_kruskal.GetLength(1); j++)
                    Console.Write(mas_kruskal[i, j] + " ");
                Console.WriteLine();
            }
            Kruskal(mas_kruskal);
            Console.ReadKey();
        }
    }
}
