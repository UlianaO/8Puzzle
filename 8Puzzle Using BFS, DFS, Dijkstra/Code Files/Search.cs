using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8Puzzle
{
    class Search
    {
        public Search()
        {

        }


        bool goalFound = false;

        //find which tile was swapped

        public List <Node> dijkstra(Node c)
        {
            bool goalIsFound = false;
            int cost = 0;   
            PriorityQueue <Node, int> pq = new PriorityQueue<Node, int>();

            List<Node> visited = new List<Node>();
            List <Node> path = new List<Node> ();
            pq.Enqueue(c, c.costSumPair.Item1); //cost from c to c is 0
            visited.Add(c);

            int costi = c.getCost();
            int dti = c.getNumOfDisplacedTiles();
            int sumi = costi + dti;
            c.setCostSumPair(costi, sumi);

            if (c.checkIfGoal())
            {
                goalIsFound = true;
                tracePath(path, c);
            }
            int dt = 0, sum = 0;

            while (pq.Count > 0 && !goalIsFound)
            {
                Node current = pq.Dequeue();
                current.ExpandMove();

                if(current.checkIfGoal())
                {
                    goalIsFound = true;
                    tracePath(path, current);
                }
                
                //for each child
                for(int i = 0; i < current.children.Count; i++)
                {
                    Node current_child = current.children[i];

                    if (current_child.checkIfGoal())
                    {
                        goalIsFound = true;
                  
                        tracePath(path, current_child);

                    }
                    //if not in visited and not in pq.
                    if (!checkIfContains(visited, current_child))
                    {
                        
                       cost =  current_child.getCost();
                       dt = dt + current_child.getNumOfDisplacedTiles();
                       sum = sum + cost + dt;
                        
                        current_child.setCostSumPair(cost, sum);
                        pq.Enqueue(current_child, current_child.costSumPair.Item2);
                        visited.Add(current_child);

                    }
                }
               
            }

            return path;
        }

        public bool checkIfContainPQ(PriorityQueue<Node, int> pq, Node c)
        {
            PriorityQueue<Node, int> pq_copy = pq;
            bool result = false;
            Node current;
            //if pq part Node contains Node c return true
            //iterate through a queue looking for node C
            if (pq_copy.Count != 0)
            {
                while (pq_copy.Count > 0)
                {
                    current = pq_copy.Dequeue();
                    if (current.Equals(c)) ;
                    result = true;
                }
            }
            return result;
        }
        public int getCost(Node p)
        {
            //compare the parent and the child
            //record what was moved

            /*
{   parent
    1, 2, 6,                                     1 2 6
    3, 4, 5,  ----> 7 displaced tiles  ----->    0 4 5   --------> tileMoved = cost = 3.
    0, 7, 8                                      3 7 8
}; */
            int cost = -1;
            if (p.parent != null)
            {
                for (int i = 0; i < p.puzzle.Length && i < p.parent.puzzle.Length; i++)
                {
                    if (p.parent.puzzle[i] != p.puzzle[i])
                    {
                        if (p.parent.puzzle[i] >= p.puzzle[i])
                            cost = p.parent.puzzle[i];
                        else
                            cost = p.puzzle[i];
                    }

                }
            }

            return cost;
        }

        public int getNumOfDisplacedTiles(Node p)
        {
            /*
            {
                1, 2, 6,
                3, 4, 5,  ----> 7 displaced tiles
                0, 7, 8
            }; */
            int numDisplacedTiles = 0;

            int[] goal =
           {
                1, 2, 3,
                8, 0, 4,
                7, 6, 5,
            };

            for (int i = 0; i < goal.Length && i < p.puzzle.Length; i++)
            {
                if (p.puzzle[i] != goal[i])
                {
                    numDisplacedTiles++;
                }

            }
            return numDisplacedTiles;
        }
        public List <Node> DFS1(Node c)
        {
            List <Node> visited = new List <Node>();
            List<Node> path;

            path = DFS2(c, visited);
            return path;
            
        }

        public List <Node> DFS2(Node c, List <Node> visited)
        {
            List<Node> path = new List <Node>();
            visited.Add (c);
            c.ExpandMove();
          
            for (int i = 0; i < c.children.Count; i++)
            {
                if(c.children[i].checkIfGoal())
                {
                    goalFound = true;
                    tracePath(path, c.children[i]);
                    break;
                }

                if (!goalFound)
                {
                    if (!visited.Contains(c.children[i]))
                        DFS2(c.children[i], visited);
                }
            }
            return path;
        }

        public List <Node> Dfs (Node c)
        {
            List <Node> visited = new List<Node>();    
            Stack <Node> stack = new Stack<Node>();
            List <Node> path = new List<Node>();
            List <Node> fully_explored = new List<Node>();
         //   visited.Add(c);

            stack.Push(c);

            while (stack.Count != 0)
            {
                Node k = stack.Pop();
                visited.Add(k);
                k.PrintPuzzle();
                k.ExpandMove();
                //Console.WriteLine("next->" + c);
                for (int i = 0; i < k.children.Count; i++)
                {
                    Node current_child = k.children[i];
                    if (current_child.checkIfGoal())
                    {
                        goalFound = true;
                        tracePath(path, current_child);
                    }
                    if (!checkIfContains(visited, current_child) && !checkIfContainsStack(stack, current_child))
                    {
                       
                        stack.Push(current_child);
                    }
                }
            }
            return path;
        }

        public bool checkIfContainsStack(Stack <Node> st, Node c)
        {
            Stack <Node> copy = new Stack<Node> ();
            copy = st;
            bool contains = false;

            for (int i = 0; i < copy.Count; i++)
            {
                if (c == copy.Pop())
                {
                    contains = true;
                }
            }
            return contains;
        }
        public List<Node> DepthFirstTraversal(Node c)
        {
            List<Node> visited = new List<Node>();
            Stack<Node> stack = new Stack<Node>();
            bool goalFound = false;
            List<Node> path = new List<Node>();

            //if the first one is the goal state
            if (c.checkIfGoal() == true)
            {
                tracePath(path, c);
                return path;
            }
            //Start by putting any one of the graph's vertices on top of a stack.
            stack.Push(c);
            int cost = 0;
            while (stack.Count != 0)
            {
                Node current = stack.Pop();
                //Take the top item of the stack and add it to the visited list.
                visited.Add(current);
                current.ExpandMove();

                for (int i = 0; i < current.children.Count; i++)
                {

                    Node child = current.children[i];
                    if (!goalFound)
                    {
                        if (child.checkIfGoal())
                        {
                            path.Add(child);
                            goalFound = true;
                            while (child.parent != null) //if parent is null, we have the root node
                            {
                                cost++;
                                child = child.parent;
                                path.Add(child);
                                //Console.WriteLine("Cost of puzzle above is 1");
                            }
                            Console.WriteLine("Cost of DFS traversal is {0}", cost);
                        }

                        if (!checkIfContains(visited, child) && !stack.Contains(child) ) //if not visited, push on to the stack
                        {

                            stack.Push(child);
                        }
                    }
                }

            }
            return path;

        }

        public List<Node> BFS(Node root)
        {
            int cost = 0;
            List<Node> path = new List<Node>();
            List <Node> explored = new List<Node>(); 
            LinkedList <Node> qu = new LinkedList<Node>();

            if (root.checkIfGoal())
            {
                tracePath(path, root);
                return path;
            }
   
            qu.AddLast(root);
            bool goalFound = false;

            while (qu.Any()  && !goalFound)
            {
                Node current = qu.First();
                current.setCostSumPair(1, 1);
                explored.Add(current);
                qu.RemoveFirst();

                current.ExpandMove();
                

                for (int i = 0; i < current.children.Count; i++)

                {
                    Node current_child = current.children[i];
                    if(current_child.checkIfGoal())
                    {
                        path.Add(current_child);
                        goalFound = true;
                        while (current_child.parent != null) //if parent is null, we have the root node
                        {
                            cost++;
                            current_child = current_child.parent;
                            path.Add(current_child);
                            //Console.WriteLine("Cost of puzzle above is 1");
                        }
                        Console.WriteLine("Cost of BFS traversal is {0}", cost);
                        // Console.WriteLine(cost);
                       // tracePath2(path, current_child);
                    }

                    if(!checkIfContainsLL(qu, current_child) && !checkIfContains(explored, current_child))
                    {
                        qu.AddLast(current_child);
                    }
                     
                }
            }
            return path;
        }

        public void tracePath(List <Node> path, Node n)
        {
            int steps = 0;
            int cost = 0;
           // Console.WriteLine("Tracing shortest path: ");
            Node current = n;
            cost = current.costSumPair.Item1;
            path.Add(current);
            List<Node> repeating = new List<Node>();    

            while(current.parent != null) //if parent is null, we have the root node
            {
                cost = cost + current.costSumPair.Item1;
                current = current.parent;
               // cost = cost + current.costSumPair.Item1;
                if (!path.Contains(current))
                {
                   // Console.WriteLine(current.costSumPair.Item1);
                   // cost = cost + current.costSumPair.Item1;
                    //current = current.parent;
                    path.Add(current);
              
                }
            }

            Console.WriteLine("Goal found in {0} steps and the shortest path cost was {1}", path.Count-1, cost);
            
        }
       
        public static bool checkIfContainsLL(LinkedList<Node> list, Node c)
        {

            bool contains = false;

                if(list.Contains(c))
                {
                    contains = true;
                }

            return contains;
        }

        public static bool checkIfContains(List <Node> list, Node c)
        {
            bool contains = false;

            for(int i = 0; i < list.Count;i++)
            {
                if(list[i].ComparePuzzle(c.puzzle))
                {
                    contains = true;
                }
            }
            return contains;
        }
    }
}
