using System;
using System.IO;

#pragma warning disable CA1416 //Проверка совместимости платформы

namespace BlackRedTree
{
    public enum Color
    {
        Red = 0,
        Black
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            RedBlackTree tree = new RedBlackTree();
            for (int i = 15; i > 0; i--)
                tree.Insert(i+".00.00");

            do
            {
                tree.DisplayTree();
                Console.ReadLine();
                Console.Clear();
                tree.Delete(tree.root);
            } while (tree.root != tree.sentinel);

            for (int i = 1; i <= 15; i++)
                tree.Insert(i + ".00.00");

            do
            {
                tree.DisplayTree();
                Console.ReadLine();
                Console.Clear();
                tree.Delete(tree.root);
            } while (tree.root != tree.sentinel);
        }
    }
}
