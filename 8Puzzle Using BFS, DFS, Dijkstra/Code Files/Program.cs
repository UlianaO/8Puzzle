using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;


namespace _8Puzzle
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("**********************************************************************************************");
                Console.WriteLine("Choose puzzle example to run(1 or 2): ");

                string choice = Console.ReadLine();
                string filePath;
                List<string> lines;
                List<int> lint;


                switch (choice)
                {
                    case "1":
                        filePath = "..//..//..//Example1.txt";
                        break;

                    case "2":
                        filePath = "..//..//..//Example2.txt";
                        break;

                    default:
                        Console.WriteLine("Entered wrong character");
                        filePath = null;
                        return;
                        break;

                }

                lines = File.ReadAllLines(filePath).ToList();
                int[] array = new int[9];
                lint = new List<int>();
                foreach (string line in lines)
                {
                    string[] items = line.Split(' ');
                    for (int i = 0; i < items.Length; i++)
                    {
                        lint.Add(int.Parse(items[i]));
                    }
                    array = lint.ToArray();
                }

                Node root = new Node(array);
                Search search = new Search();
                List<Node> path_copy;

                Console.WriteLine("Choose algorithm to perform: 1-BFS, 2-DFS, 3-Dijkstra");
                string alg_choice = Console.ReadLine();

                switch (alg_choice)
                {
                    case "1":
                        path_copy = search.BFS(root);
                        break;
                    case "2":
                        path_copy = search.DepthFirstTraversal(root);
                        break;
                    case "3":
                        path_copy = search.dijkstra(root);
                        break;
                    default:
                        path_copy = search.dijkstra(root);
                        break;
                }

                if (path_copy.Count > 0)
                {
                    for (int i = path_copy.Count - 1; i >= 0; i--)
                    {
                        path_copy[i].PrintPuzzle();
                        Console.WriteLine("Cost of the puzzle above is {0} ", path_copy[i].costSumPair.Item1);
                    }
                }
                Console.WriteLine("**********************************************************************************************");
            }

            Console.Read();
        }
    }
}