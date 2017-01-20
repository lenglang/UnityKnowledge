using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace 冒泡排序拓展
{
    class Program {
        static void Sort(int[] sortArray)
        {
            bool swapped = true;
            do
            {
                swapped = false;
                for (int i = 0; i < sortArray.Length - 1; i++)
                {
                    if (sortArray[i] > sortArray[i + 1])
                    {
                        int temp = sortArray[i];
                        sortArray[i] = sortArray[i + 1];
                        sortArray[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }

        static void CommonSort<T>(T[] sortArray, Func<T,T,bool>  compareMethod)
        {
            bool swapped = true;
            do {
                swapped = false;
                for (int i = 0; i < sortArray.Length - 1; i++) {
                    if (compareMethod(sortArray[i],sortArray[i+1])) {
                        T temp = sortArray[i];
                        sortArray[i] = sortArray[i + 1];
                        sortArray[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        static void Main(string[] args) {
            //int[] sortArray = new int[]{123,23,12,3,345,43,53,4};
            //Sort(sortArray);
            //foreach (var temp in sortArray)
            //{
            //    Console.Write(temp+" ");
            //}
            Employee[] employees = new Employee[]
            {
                new Employee("dsf",12), 
                new Employee("435dsf",234), 
                new Employee("234dsf",14), 
                new Employee("ds234f",234), 
                new Employee("dssfdf",90)
            };
            CommonSort<Employee>(employees,Employee.Compare);
            //foreach (Employee em in employees)
            //{
               // Console.WriteLine(em);
            //}
        }
    }
}
