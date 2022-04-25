using System;
using System.IO;
using System.Linq;

namespace Lab4_Algorithm_FordFalkerson_
{
    class Program
    {
        private static void FordFalkerson(int[,] array, int result = 0)
        {
            var constant_edge = NewEdge(array);
            var edge = NewEdge(array);
            bool first_vertex_posible_way = true;
            while (first_vertex_posible_way)
            {
                int index = 0;
                var mas_mark = Mark(array);
                //Шлях
                while (index != array.GetLength(1))
                {
                    bool flag_mas_posible = true;
                    int[] mas_posible_way = new int[0];
                    //Можливі вершини в які можна перейти
                    while (flag_mas_posible)
                    {
                        for (int i = 0; i < edge.GetLength(1); i++)
                        {
                            if (edge[index, i].Potic1 == 0 || mas_mark[i].Vertex_number == 100 || mas_mark[i].Vertex_number != -1)
                            {
                                continue;
                            }
                            else
                            {
                                mas_posible_way = Push(edge[index, i].Potic1, mas_posible_way);
                            }
                        }
                        //Откат
                        if (mas_posible_way.Length == 0)
                        {
                            if (index == 0)
                            {
                                first_vertex_posible_way = false;
                                break;
                            }
                            else
                            {
                                int old_index = index;
                                index = mas_mark[index].Vertex_number;
                                mas_mark[old_index] = new { Count = 0, Vertex_number = 100 };
                            }
                        }
                        else
                        {
                            flag_mas_posible = false;
                        }
                    }
                    if (first_vertex_posible_way == false)
                    {
                        break;
                    }
                    //
                    int value = Max(mas_posible_way);
                    for (int i = 0; i < edge.GetLength(1); i++)
                    {
                        if (edge[index, i].Potic1 == value && mas_mark[i].Vertex_number != 100)
                        {
                            mas_mark[i] = new { Count = value, Vertex_number = index };
                            index = i;
                            break;
                        }
                    }
                    if (index == array.GetLength(1) - 1)
                    {
                        break;
                    }
                }
                //
                int min = mas_mark[0].Count;
                for (int i = 0; i < mas_mark.Length; i++)
                {
                    if (mas_mark[i].Vertex_number != -1 && min > mas_mark[i].Count)
                    {
                        min = mas_mark[i].Count;
                    }
                }
                result += min;
                for (int i = 0; i < mas_mark.Length; i++)
                {
                    if (mas_mark[i].Vertex_number != 100 && mas_mark[i].Vertex_number != -1)
                    {
                        edge[mas_mark[i].Vertex_number, i] = new { Potic1 = edge[mas_mark[i].Vertex_number, i].Potic1 - min, Potic2 = edge[mas_mark[i].Vertex_number, i].Potic2 + min };
                    }
                }
            }


            //Результати
            Console.WriteLine("Ребро \t Величина потоку");
            for (int i = 0; i < constant_edge.GetLength(0); i++)
            {
                for (int j = 0; j < constant_edge.GetLength(1); j++)
                {
                    if (constant_edge[i, j].Potic1 == edge[i, j].Potic1 && constant_edge[i, j].Potic2 == edge[i, j].Potic2)
                    {
                        continue;
                    }
                    else
                    {
                        int potic1 = constant_edge[i, j].Potic1 - edge[i, j].Potic1;
                        int potic2 = constant_edge[i, j].Potic2 - edge[i, j].Potic2;
                        int size_of_the_flow = potic1 > potic2 ? potic1 : potic2;
                        Console.WriteLine("({0}, {1}) \t {2}", i, j, size_of_the_flow);
                    }
                }
            }
            Console.WriteLine("Max flow is: " + result); 
        }
        private static int Max(int[] array)
        {
            int max = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                if (max < array[i])
                {
                    max = array[i];
                }
            }
            return max;
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
        private static dynamic[] Mark(int[,] array)
        {
            dynamic[] mas_mark = new dynamic[array.GetLength(0)];
            for (int i = 0; i < array.GetLength(1); i++)
            {
                if (i == 0)
                {
                    dynamic mark = new { Count = int.MaxValue, Vertex_number = 100 };
                    mas_mark[i] = mark;
                }
                else
                {
                    dynamic mark = new { Count = 0, Vertex_number = -1};
                    mas_mark[i] = mark;
                }

            }
                     

            return mas_mark;
        }
        private static dynamic[,] NewEdge(int[,] array)
        {
            dynamic[,] mas_component = new dynamic[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    dynamic anon = new { Potic1 = array[i, j], Potic2 = 0 };
                    mas_component[i, j] = anon;
                }                
            }
            return mas_component;
        }
        static void Main(string[] args)
        {
            string[] s = File.ReadAllLines("l4-1.txt");
            s = s.Skip(1).ToArray();

            string[,] num = new string[s.Length, s[0].Split(' ').Length];
            for (int i = 0; i < s.Length; i++)
            {
                string[] temp = s[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    num[i, j] = temp[j];
            }

            int[,] mas_ford = new int[num.GetLength(0), num.GetLength(1)];
            for (int i = 0; i < num.GetLength(0); i++)
            {
                for (int j = 0; j < num.GetLength(1); j++)
                {
                    mas_ford[i, j] = Convert.ToInt32(num[i, j]);
                }
            }

            Console.WriteLine("Adjacency matrix");
            for (int i = 0; i < mas_ford.GetLength(0); i++)
            {
                for (int j = 0; j < mas_ford.GetLength(1); j++)
                    Console.Write(mas_ford[i, j] + "\t");
                Console.WriteLine();
            }
            Console.WriteLine("\n\n");
            FordFalkerson(mas_ford);            
            Console.ReadKey();
        }
    }
}
