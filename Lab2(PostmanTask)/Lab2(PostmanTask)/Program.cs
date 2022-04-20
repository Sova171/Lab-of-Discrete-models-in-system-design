using System;
using System.IO;
using System.Linq;
using System.Dynamic;

namespace Lab2_PostmanTask_
{
    class Program
    {
        private static int[,] CoppyArray(int[,] array)
        {
            int[,] copy_array = new int[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < copy_array.GetLength(0); i++)
            {
                for (int j = 0; j < copy_array.GetLength(1); j++)
                {
                    copy_array[i, j] = array[i, j];
                }
            }
            return copy_array;
        }
        private static bool ArrayZero(int[,] mas)
        {
            bool temp = false;
            for (int i = 0; i < mas.GetLength(0); i++)
            {
                for (int j = 0; j < mas.GetLength(0); j++)
                {
                    if (mas[i, j] != 0)
                    {
                        temp = true;
                    }
                }
            }
            return temp;
        }
        private static bool Zero(int top, int[,] mas)
        {
            bool temp = false;
            for (int i = 0; i < mas.GetLength(1); i++)
            {
                if (mas[top, i] != 0)
                {
                    temp = true;
                }
            }
            return temp;
        }
        private static void Result(int[,] mas_postman_origin, int[] result)
        {
            int min_way = 0;
            int[,] mas_postman_origin_copy = CoppyArray(mas_postman_origin);
            int[,] mas_postman = EylerEdge(mas_postman_origin);
            bool flag = false;
            for (int q = 0; q < result.Length; q++)
            {
                if ((q + 1 < result.Length - 1) && mas_postman_origin_copy[result[q], result[q + 1]] != mas_postman[result[q], result[q + 1]])
                {
                    var new_way = AlgorithmDijkstry(mas_postman_origin_copy, result[q]);
                    Console.Write("{0} -> ", new_way[result[q + 1]].Way);                    
                    min_way += new_way[result[q + 1]].Count;
                    flag = true;
                }
                else
                {
                    if (q == result.Length - 1)
                    {
                        Console.Write(result[q] + " -> ");
                    }
                    else
                    {
                        if (flag)
                        {
                            min_way += mas_postman_origin_copy[result[q], result[q + 1]];
                            flag = false;
                        }
                        else
                        {
                            Console.Write(result[q] + " -> ");
                            min_way += mas_postman_origin_copy[result[q], result[q + 1]];
                        }                                                
                    }                    
                }                                            
            }
            Console.WriteLine("end\nmin way = " + min_way);

        }
        private static void PostmanTask(int[,] mas_postman_origin)
        {
            int[,] mas_postman_origin_copy = CoppyArray(mas_postman_origin);
            int[,] mas_postman = EylerEdge(mas_postman_origin);
            int top = 0;
            int[] buble = new int[1] { top };
            int[] result = new int[0];
            while(ArrayZero(mas_postman))
            {
                for (int i = 0; i < mas_postman.Length; i++)
                {
                    if (!Zero(top, mas_postman))
                    {
                        result = Push(buble[buble.Length - 1], result);
                        buble = Pop(buble);
                        top = buble[buble.Length - 1];
                        break;
                    }
                    else
                    {
                        if (mas_postman[top, i] == 0)
                        {
                            continue;
                        }
                    
                        else 
                        {
                            mas_postman[top, i] = 0;
                            mas_postman[i, top] = 0;
                            buble = Push(i, buble);
                            top = i;
                            break;
                        }                                                                
                    }
                }
            }
            
            while (buble.Length != 0)
            {
                result = Push(buble[buble.Length - 1], result);
                buble = Pop(buble);                
            }
            Result(mas_postman_origin_copy, result);
            //Console.WriteLine(result.Length);
        }
        private static int[] Push(int value, int[] mas_push)
        {
            int[] mas_new = new int[mas_push.Length + 1];
            for (int i = 0; i < mas_push.Length; i++)
            {
                mas_new[i] = mas_push[i];
            }
            mas_new[mas_new.Length - 1] = value;
            return mas_new;
        }
        private static int[] Pop(int[] mas_push)
        {
            int[] mas_new = new int[mas_push.Length - 1];
            for (int i = 0; i < mas_new.Length; i++)
            {
                mas_new[i] = mas_push[i];
            }
            return mas_new;
        }
        private static bool Pair(int top, int[,] mas)
        {
            int count_of_edges = 0;
            for (int j = 0; j < mas.GetLength(1); j++)
            {
                if (mas[top, j] == 0)
                {
                    continue;
                }
                else
                {
                    count_of_edges++;
                }
            }               
            return count_of_edges % 2 != 0 ?  true : false;
        }
        private static int[,] EylerEdge(int[,] mas)
        {
            int[] edge = Eyler(mas);
            if (edge.Length == 0)
            {
                return mas;
            }
            else
            {
                for (int j = 0; j < edge.Length; j++)
                {
                    if (Pair(edge[j], mas))
                    {                    
                        var new_edge = AlgorithmDijkstry(mas, edge[j]);
                        for (int i = j+1; i < edge.Length; i++)
                        {                        
                            if (mas[edge[j],edge[i]] == 0 && Pair(edge[i], mas))
                            {
                                mas[edge[j], edge[i]] = new_edge[edge[i]].Count;
                                mas[edge[i], edge[j]] = new_edge[edge[i]].Count;
                                break;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }                
            }
            return mas;
        }
        private static dynamic[] AlgorithmDijkstry(int[,] array, int top = 0)
        {
            dynamic[] mas_component = new dynamic[array.GetLength(0)];
            for (int i = 0; i < mas_component.GetLength(0); i++)
            {
                if (i == top)
                {
                    dynamic anon = new { Count = 0, Way = String.Format("{0}", top), Flag = true };                    
                    mas_component[i] = anon;
                }
                else
                {
                    dynamic anon = new{ Count = int.MaxValue, Way = "", Flag = true };
                    mas_component[i] = anon;  
                }                
            }

            int step = 0;
            while (step != mas_component.Length)
            {
                int temp = int.MaxValue;
                int index = -1;
                for (int i = 0; i < mas_component.Length; i++)
                {
                    if (temp > mas_component[i].Count && mas_component[i].Flag)
                    {
                        temp = mas_component[i].Count;
                        index = i;
                    }
                }
                int current_count = mas_component[index].Count;
                string current_way = mas_component[index].Way;
                for (int i = 0; i < array.GetLength(1); i++)
                {
                    if (array[index, i] == 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (mas_component[i].Count > (array[index, i] + mas_component[index].Count) && mas_component[i].Flag)
                        {
                            mas_component[i] = new { Count = array[index, i] + mas_component[index].Count, Way = String.Format("{0} -> {1}", current_way, i), Flag = true };                       
                        }
                    }
                }
                mas_component[index] = new { Count = current_count, Way = current_way, Flag = false };
                step++ ;                
            }
            return mas_component;
        }
        private static int[] Eyler(int[,] mas)
        {
            int flag = 0;
            int[] mas_of_tops = new int[0];
            for (int i = 0; i < mas.GetLength(0); i++)
            {                
                if (Pair(i, mas))
                {
                    flag++;
                    Array.Resize(ref mas_of_tops, flag);
                    mas_of_tops[flag - 1] = i;
                }
            }
                return mas_of_tops;
        }
        static void Main(string[] args)
        {
            string[] s = File.ReadAllLines("l2-1.txt");
            s = s.Skip(1).ToArray();

            string[,] num = new string[s.Length, s[0].Split(' ').Length];
            for (int i = 0; i < s.Length; i++)
            {
                string[] temp = s[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    num[i, j] = temp[j];
            }

            int[,] mas_postman = new int[num.GetLength(0), num.GetLength(1)];
            for (int i = 0; i < num.GetLength(0); i++)
            {
                for (int j = 0; j < num.GetLength(1); j++)
                {
                    mas_postman[i, j] = Convert.ToInt32(num[i, j]);
                }
            }

            Console.WriteLine("Adjacency matrix");
            for (int i = 0; i < mas_postman.GetLength(0); i++)
            {
                for (int j = 0; j < mas_postman.GetLength(1); j++)
                    Console.Write(mas_postman[i, j] + "\t");
                Console.WriteLine();
            }
            //Console.WriteLine("\nМасив непарних вершин");
            //int[] mas_eyler = Eyler(mas_postman);
            //for (int i = 0; i < mas_eyler.Length; i++)
            //{
            //    Console.Write(mas_eyler[i] + "\t");
            //}
            //Console.WriteLine();

            //var anon = AlgorithmDijkstry(mas_postman, 1);
            //for (int i = 0; i < anon.Length; i++)
            //{
            //    Console.WriteLine("{0}) Way: {1}, Count: {2}", i, anon[i].Way, anon[i].Count);
            //}
            Console.WriteLine("\n");
            //mas_postman = EylerEdge(mas_postman);
            //for (int i = 0; i < mas_postman.GetLength(0); i++)
            //{
            //    for (int j = 0; j < mas_postman.GetLength(1); j++)
            //        Console.Write(mas_postman[i, j] + "\t");
            //    Console.WriteLine();
            //}
            PostmanTask(mas_postman);
            Console.ReadKey();
        }
    }
}
