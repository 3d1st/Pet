using System;

namespace LinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] source = {1, 2, 3, 4, 5};
            
            var list = new SimpleLinkedList<int>(source);

            Console.Write("Source:\t\t");
            Console.WriteLine(list.ToString());
            
            list.Reverse();
            Console.Write("Reversed:\t");
            Console.WriteLine(list.ToString());
            Console.ReadKey();
        }
    }
}