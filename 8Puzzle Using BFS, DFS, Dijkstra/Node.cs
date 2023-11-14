using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8Puzzle
{
    class Node
    {
        //get the children
        public List<Node> children = new List<Node>();
        public Node parent;
        public Node child;
        public int[] puzzle = new int[9];
        public int x = 0;
        public int col = 3;
        public int row = 3;
        public int[] goal_state = new int[9];
        public Tuple<int, int> costSumPair = new Tuple<int, int>(1, 1);
 

        public Node(int[] p)
        {
            SetPuzzle(p);
        }

        public void setCostSumPair(int c, int s)
        {
            this.costSumPair = new Tuple<int, int>(c, s);
        }
        public void SetPuzzle(int[] p)
        {
            for (int i = 0; i < puzzle.Length; i++)
            {
                this.puzzle[i] = p[i];
            }
            
        }

        //find 0 on the board, applies all 4 moves
        public void ExpandMove()
        {
            //goes throught the puzzle, finds 0(empty slot)
            for (int i = 0; i < puzzle.Length; i++)
            {
                if (puzzle[i] == 0)
                    x = i;
            }

           
            //exchanging neighbors with an empty tile
            MoveLeft(puzzle, x);
            MoveUp(puzzle, x);
            MoveRight(puzzle, x);
            MoveDown(puzzle, x);

        //    return listAllNeighbors;
        }

        public Node createNode(int[] p)
        {
            Node child = new Node(p);
            children.Add(child);
            child.parent = this;
          //////////////////////////////////////////////////////////////////////////////  child.parent.costSumPair


            return child;
        }
        public int getCost()
        {
            //compare the parent and the child
            //record what was moved

            /*
{   parent
    1, 2, 6,                                     1 2 6
    3, 4, 5,  ----> 7 displaced tiles  ----->    0 4 5   --------> tileMoved = cost = 3.
    0, 7, 8                                      3 7 8
}; */
            int cost = 0;
            if (this.parent != null)
            {
                for (int i = 0; i < this.puzzle.Length && i < this.parent.puzzle.Length; i++)
                {
                    if (this.parent.puzzle[i] != this.puzzle[i])
                    {
                        if (this.parent.puzzle[i] >= this.puzzle[i])
                            cost = this.parent.puzzle[i];
                        else
                            cost = this.puzzle[i];
                    }
                }
            }

            return cost;
        }

        public int getNumOfDisplacedTiles()
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

            for (int i = 0; i < goal.Length && i < this.puzzle.Length; i++)
            {
                if (this.puzzle[i] != goal[i])
                {
                    numDisplacedTiles++;
                }

            }
            return numDisplacedTiles;
        }

        //to expand the node, 4 ways to move the tile
        public Node MoveRight(int[] p, int tile)
        {
            //can not move to the right from the most right tile --> use tile % columns for the constraint
            if (tile % col < col - 1)
            {
                //copy the puzzle before moving
                int[] current_p = new int[9];
                CopyPuzzle(current_p, p);

                //exchanging
                int temp = current_p[tile + 1];
                current_p[tile + 1] = current_p[tile];
                current_p[tile] = temp;

                //creating a node-result from moving the tile
                Node child = createNode(current_p);

                return child; 

            }
            else
                return null;
        }

        public void CopyPuzzle(int[] a, int[] b)
        {
            for (int i = 0; i < b.Length; i++)
            {
                a[i] = b[i];
            }
        }
      /*  public Node createPair(Node ch, int c, int dt)
        {
            int cost = getCost(parent, child);
            int displacedTiled = getNumOfDisplacedTiles(child);
        } */

        public Node MoveLeft(int[] p, int tile)
        {
            if (tile % col > 0)
            {
                int[] current_p = new int[9];
                CopyPuzzle(current_p, p);

                int temp = current_p[tile - 1];
                current_p[tile - 1] = current_p[tile];
                current_p[tile] = temp;

                //creating a node-result from moving the tile
                Node child = createNode(current_p);
                return child;
            }
            else
                return null;
        }
        public Node MoveUp(int[] p, int tile)
        {
            if (tile - col >= 0)
            {
                int[] current_p = new int[9];
                CopyPuzzle(current_p, p);

                int temp = current_p[tile - 3];
                current_p[tile - 3] = current_p[tile];
                current_p[tile] = temp;

                //creating a node-result from moving the tile
                Node child = createNode(current_p);

                return child;
            }
            else
                return null;
        }
        public Node MoveDown(int[] p, int tile)
        {
            if (tile + col < puzzle.Length)
            {
                int[] current_p = new int[9];
                CopyPuzzle(current_p, p);

                int temp = current_p[tile + 3];
                current_p[tile + 3] = current_p[tile];
                current_p[tile] = temp;

                //creating a node-result from moving the tile
                Node child = createNode(current_p);
                return child;
            }
            else
                return null;
        }

        public void PrintPuzzle()
        {
            Console.WriteLine();
            int k = 0;

            for (int i = 0; i < col; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < col; j++)
                {
                    Console.Write(puzzle[k] + " | ");
                    k++;
                }
                Console.WriteLine();
            }
          //  Console.WriteLine("Cost of this puzzle is ", this.costSumPair.Item1);
        }
        //calculate the number of displaced tiles
        //go through each current p[i] && compare with goal, put each tile that does not match into the list

        public bool ComparePuzzle(int[] p)
        {
            bool same = true;
            for (int i = 0; i < puzzle.Length; i++)
            {
                if (puzzle[i] != p[i])
                {
                    same = false;
                }
            }

            return same;
        }


        public bool checkIfGoal()
        {
            bool isGoal = true;

            int[] goal =
            {
                1, 2, 3,
                8, 0, 4,
                7, 6, 5,
            };

            for (int i = 0; i < goal.Length && i < puzzle.Length; i++)
            {
                if (puzzle[i] != goal[i])
                    isGoal = false;
            }

            return isGoal;
        }
    }
}
