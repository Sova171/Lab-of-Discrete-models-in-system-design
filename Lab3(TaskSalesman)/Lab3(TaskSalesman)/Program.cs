using System;
using System.IO;
using System.Linq;

namespace Lab3_TaskSalesman_
{
    class Program
    {
        private static int FindValue(int[] mas, int value)
        {
            int index_value = 1000;
            for (int i = 0; i < mas.Length; i++)
            {
                if (i == value)
                {
                    index_value = mas[i];
                    break;
                }
            }
            return index_value;
        }
        private static int FindIndex(int[] mas, int value)
        {
            int index_value = 1000;
            for (int i = 0; i < mas.Length; i++)
            {
                if (mas[i] == value)
                {
                    index_value = i;
                    break;
                }
            }
            return index_value;
        }
        private static int[,] Result(int[,] result, int[,] array, int[] index_koef, int limit, int[] mas_i, int[] mas_j)
        {            
            int[,] array_with_edge = CoppyArray(array);
            int[,] array_without_edge = CoppyArray(array);

            //з дугою
            //
            int index_str = FindValue(mas_i, index_koef[0]);
            int index_row = FindValue(mas_j, index_koef[1]);
            int value_str = FindIndex(mas_i, index_row);
            int value_row = FindIndex(mas_j, index_str);
            if (value_str != 1000 && value_row != 1000)
            {
                array_with_edge[value_str, value_row] = 1000;
            }
            
            //
            int[] new_mas_str = DeleteValueFromArray(mas_i, index_koef[0]);
            int[] new_mas_row = DeleteValueFromArray(mas_j, index_koef[1]);
            array_with_edge = Delete_str(array_with_edge, index_koef[0]);
            array_with_edge = Delete_row(array_with_edge, index_koef[1]);
            int limit_of_array_with_edge = Limit(array_with_edge) + limit;
            array_with_edge = NewArray(array_with_edge);

            //без дуги
            array_without_edge[index_koef[0], index_koef[1]] = 1000;
            int limit_array_without_edge = Limit(array_without_edge) + limit;
            array_without_edge = NewArray(array_without_edge);

            //порівняння
            if (limit_of_array_with_edge <= limit_array_without_edge)
            {
                if (array_with_edge.GetLength(0) == 2)
                {
                    for (int i = 0; i < array_with_edge.GetLength(0); i++)
                    {
                        for (int j = 0; j < array_with_edge.GetLength(1); j++)
                        {
                            if (array_with_edge[i, j] == 0)
                            {
                                int[] edge_array = new int[2];
                                edge_array[0] = new_mas_str[i];
                                edge_array[1] = new_mas_row[j];
                                result = PushResult(result, edge_array);
                            }
                        }
                    }
                    return result;
                }
                int[] index_koef_array_with_edge = MaxZeroKoef(array_with_edge);
                int[] new_index = new int[2];
                new_index[0] = new_mas_str[index_koef_array_with_edge[0]];
                new_index[1] = new_mas_row[index_koef_array_with_edge[1]];
                int[,] new_result_with_edge = PushResult(result, new_index);
                result = Result(new_result_with_edge, array_with_edge, index_koef_array_with_edge, limit_of_array_with_edge, new_mas_str, new_mas_row);
                return result;
            }
            else
            {
                int[] index_koef_array_without_edge = MaxZeroKoef(array_without_edge);              
                //int[,] new_result_without_edge = PushResult(result, index_koef_array_without_edge); 
                result = Result(result, array_without_edge, index_koef_array_without_edge, limit_array_without_edge, mas_i, mas_j);
                return result;
            }
        }

        private static int[,] NewArray(int[,] array)
        {
            //віднімаємо від кожного елемента масива мінімальний елемент кожного відповідного рядка
            int[] mas_minstr = MinStr(array);
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] < 1000)
                    {
                        array[i, j] -= mas_minstr[i];
                    }
                }
            }
            //віднімаємо від кожного елемента масива мінімальний елемент кожного відповідного рядка
            int[] mas_minrow = MinRow(array);
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[j, i] < 1000)
                    {
                        array[j, i] -= mas_minrow[i];
                    }
                }
            }
            return array;
        }             
       
       
        private static int Limit(int[,] array)
        {
            //віднімаємо від кожного елемента масива мінімальний елемент кожного відповідного рядка
            int[] mas_minstr = MinStr(array);
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] < 1000)
                    {
                        array[i, j] -= mas_minstr[i];
                    }
                }
            }
            //віднімаємо від кожного елемента масива мінімальний елемент кожного відповідного рядка
            int[] mas_minrow = MinRow(array);
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[j, i] < 1000)
                    {
                        array[j, i] -= mas_minrow[i];
                    }
                }
            }
            //нижня межа
            int limit = 0;
            for (int i = 0; i < mas_minrow.Length; i++)
            {
                limit += mas_minrow[i];
                limit += mas_minstr[i];
            }
            return limit;
        }
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
        private static int[] DeleteValueFromArray(int[] array, int value)
        {
            int[] new_array = new int[array.Length - 1];
            for (int i = 0, j = 0; i < new_array.Length; i++, j++)
            {
                if (j == value)
                {
                    j++;
                }
                new_array[i] = array[j];
            }
            return new_array;
        }
        //2 метода для допомоги при запису почакових індексів першого масиву
        private static int[] IndexMasRow(int[,] array)
        {
            int[] index_j = new int[array.GetLength(1)];
            for (int i = 0; i < array.GetLength(1); i++)
            {
                index_j[i] = i;
            }
            return index_j;
        }
        private static int[] IndexMasStr(int[,] array)
        {
            int[] index_i = new int[array.GetLength(0)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                index_i[i] = i;
            }
            return index_i;
        }
        // Метод для запису дуг
        private static int[,] PushResult(int[,] array, int[] mas_push)
        {
            int[,] mas_new = new int[array.GetLength(0) + 1, mas_push.Length];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    mas_new[i, j] = array[i, j];
                }                
            }

            for (int i = 0; i < mas_push.Length; i++)
            {
                mas_new[mas_new.GetLength(0) - 1, i] = mas_push[i];
            }  
            
            return mas_new;
        }
        private static int[] MaxZeroKoef(int[,] array)
        {
            int koef = 0;
            int[] index_koef = new int[2];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == 0)
                    {
                        int zero_koef = ZeroKoef(i, j, array);
                        if (koef <= zero_koef)
                        {
                            koef = zero_koef;
                            index_koef[0] = i;
                            index_koef[1] = j;
                        }
                    }
                }
            }

            return index_koef;
        }
        private static int ZeroKoef(int index_str, int index_row, int[,] array)
        {
            int koef = 0;
            int min_str = 1000;
            for (int j = 0; j < array.GetLength(1); j++)
            {                                
                if (min_str > array[index_str, j] && j != index_row)
                {
                    min_str = array[index_str, j];
                }                        
            }
            koef += min_str;

            int min_row = 1000;
            for (int j = 0; j < array.GetLength(0); j++)
            {
                if (min_row > array[j, index_row] && j != index_str)
                {
                    min_row = array[j, index_row];
                }
            }
            koef += min_row;

            return koef;
        }
        private static int[,] NoZeroArray(int[,] array)
        {           
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == 0)
                    {
                        array[i, j] = 1000;
                    }                   
                }
            }
            return array;
        }
        private static void TaskSalesman(int[,] array)
        {            
            int limit = Limit(array);
            array = NewArray(array);

            //шукаємо максимальний коефіцієнт нульвих елементів масиву
            int[] mas_i = IndexMasStr(array);
            int[] mas_j = IndexMasRow(array);
            int[] index_koef = MaxZeroKoef(array);
            int[,] result = new int[0, 2];
            result = PushResult(result, index_koef);
            int[,] final_result = Result(result, array, index_koef, limit, mas_i, mas_j);
            
            //for (int i = 0; i < final_result.GetLength(0); i++)
            //{
            //    Console.WriteLine("{0} {1}", final_result[i, 0], final_result[i, 1]);
            //}

            int start = final_result[0, 0];
            Console.Write(final_result[0, 0]);
            int second_value = final_result[0, 1];                       
            while (start != second_value)
            {
                for (int i = 0; i < final_result.GetLength(0); i++)
                {
                   if (second_value == final_result[i, 0])
                   {
                        Console.Write(" -> " + second_value);
                        second_value = final_result[i, 1];
                        break;
                   }                                      
                }
            } 
            Console.Write(" -> " + second_value);
        }
        private static int[] MinStr(int[,] array)
        {
            int[] min_str = new int[array.GetLength(0)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                int min = array[i, 0];
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (min > array[i, j])
                    {
                        min = array[i, j];
                    }
                }
                min_str[i] = min;
            }
            return min_str;
        }
        private static int[] MinRow(int[,] array)
        {
            int[] min_row = new int[array.GetLength(1)];
            for (int i = 0; i < array.GetLength(1); i++)
            {
                int min = array[0, i];
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (min > array[j, i])
                    {
                        min = array[j, i];
                    }
                }
                min_row[i] = min;
            }
            return min_row;
        }
        private static int[,] Delete_str(int[,] array, int idx)
        {
            int[,] arrOut = new int[array.GetLength(0) - 1, array.GetLength(1)];
            bool check = true;
            for (int i = 0; i < arrOut.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (i != idx && check)
                    {
                        arrOut[i, j] = array[i, j];
                    }
                    if (i == idx)
                    {
                        check = false;
                    }
                    if (check == false)
                    {
                        arrOut[i, j] = array[i + 1, j];
                    }
                }
            }

            return arrOut;
        }
        private static int[,] Delete_row(int[,] array, int idx)
        {
            int[,] arrOut = new int[array.GetLength(0), array.GetLength(1) - 1];

            int temp = arrOut.GetLength(0);
            for (int i = 0; i < arrOut.GetLength(0); i++)
            {
                bool check = true;
                for (int j = 0; j < arrOut.GetLength(1); j++)
                {
                    if (j != idx && check)
                    {
                        arrOut[i, j] = array[i, j];
                    }
                    if (j == idx)
                    {
                        check = false;
                    }
                    if (check == false)
                    {
                        arrOut[i, j] = array[i, j + 1];
                    }
                }
            }

            return arrOut;
        }

        static void Main(string[] args)
        {
            string[] s = File.ReadAllLines("l3-5.txt");
            s = s.Skip(1).ToArray();

            string[,] num = new string[s.Length, s[0].Split(' ').Length];
            for (int i = 0; i < s.Length; i++)
            {
                string[] temp = s[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    num[i, j] = temp[j];
            }

            int[,] mas_salesman = new int[num.GetLength(0), num.GetLength(1)];
            for (int i = 0; i < num.GetLength(0); i++)
            {
                for (int j = 0; j < num.GetLength(1); j++)
                {
                    mas_salesman[i, j] = Convert.ToInt32(num[i, j]);
                }
            }

            mas_salesman = NoZeroArray(mas_salesman);
            Console.WriteLine("Adjacency matrix");
            for (int i = 0; i < mas_salesman.GetLength(0); i++)
            {
                for (int j = 0; j < mas_salesman.GetLength(1); j++)
                    Console.Write(mas_salesman[i, j] + "\t");
                Console.WriteLine();
            }

            TaskSalesman(mas_salesman);
           
            Console.ReadKey();
        }
    }
}
