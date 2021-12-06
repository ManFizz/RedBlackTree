using System;

namespace BlackRedTree
{
    class RedBlackTree
    {
        public Node root;

        ~RedBlackTree()
        {
            RecDestruct(root);
        }

        private void RecDestruct(Node x)
        {
            if (!x) return;

            RecDestruct(x.left);
            RecDestruct(x.right);
            Delete(x);
        }

        #region Insert

        public void Insert(Key itemKey)
        {
            Node z = new Node(itemKey);
            if (root == null)
            {
                root = z;
                root.Color = Color.Black;
                return;
            }

            Node Y = null;
            Node X = root;
            while (X != null)
            {
                Y = X;
                if (z.data < X.data)
                    X = X.left;
                else if (z.data == X.data)
                {
                    X.AddEqual();
                    return;
                }
                else
                    X = X.right;
            }
            z.parent = Y;
            if (Y == null)
                root = z;
            else if (z.data < Y.data)
                Y.left = z;
            else
                Y.right = z;
            z.left = null;
            z.right = null;
            z.Color = Color.Red;
            InsertFixUp(z);
        }

        private void InsertFixUp(Node z)
        {
            while (z != root && z.parent.Color == Color.Red)
            {
                Node y;
                if (z.parent == z.parent.parent.left)	
                {
                    y = z.parent.parent.right;
                    if (y != null && y.Color == Color.Red)
                    {
                        z.parent.Color = Color.Black;
                        y.Color = Color.Black;
                        z.parent.parent.Color = Color.Red;
                        z = z.parent.parent;
                    }
                    else
                    {
                        if (z == z.parent.right)
                        {
                            z = z.parent;
                            leftRotate(z);
                        }
                        z.parent.Color = Color.Black;
                        z.parent.parent.Color = Color.Red;
                        rightRotate(z.parent.parent);
                    }
                }
                else
                {
                    y = z.parent.parent.left;
                    if (y != null && y.Color == Color.Red)
                    {
                        z.parent.Color = Color.Black;
                        y.Color = Color.Black;
                        z.parent.parent.Color = Color.Red;
                        z = z.parent.parent;
                    }
                    else
                    {
                        if (z == z.parent.left)
                        {
                            z = z.parent;
                            rightRotate(z);
                        }
                        z.parent.Color = Color.Black;
                        z.parent.parent.Color = Color.Red;
                        leftRotate(z.parent.parent);
                    }
                }
            }
            root.Color = Color.Black;
        }

        #endregion

        #region Rotates

        private void leftRotate(Node X)
        {
            Node Y = X.right;
            X.right = Y.left;

            if (Y.left != null)
                Y.left.parent = X;

            if (Y != null)
                Y.parent = X.parent;

            if (X.parent == null)
                root = Y;
            else if (X == X.parent.left)
                X.parent.left = Y;
            else
                X.parent.right = Y;

            Y.left = X;
            if (X != null)
                X.parent = Y;

        }

        private void rightRotate(Node Y)
        {
            Node X = Y.left;
            Y.left = X.right;
            if (X.right != null)
                X.right.parent = Y;

            if (X != null)
                X.parent = Y.parent;

            if (Y.parent == null)
                root = X;
            else if (Y == Y.parent.right)
                Y.parent.right = X;
            else
                Y.parent.left = X;

            X.right = Y;//put Y on X's right
            if (Y != null)
            {
                Y.parent = X;
            }
        }
        #endregion

        #region Delete

        public bool Delete(Key key)
        {
            Node z = Find(key);
            bool bOut = z;
            Delete(z);
            return bOut;
        }


        private Node Minimum(Node X)
        {
            Node Y = null;
            while (X) 
            {
                Y = X;
                X = X.left;
            }
            return Y;
        }

        private void Transplant(Node u, Node v)
        {
            if (!u.parent)
                root = v;
            else if (u == u.parent.left)
                u.parent.left = v;
            else
                u.parent.right = v;
            if(v)   
                v.parent = u.parent;
        }

        public void Delete(Node z)
        {
            if (z.HasEqual())
            {
                z.DeleteEqual();
                return;
            }

            Node y = z;
            Node x;
            Color y_original_Color = y.Color;
            if (z.left == null)
            {
                x = z.right;
                Transplant(z, z.right);
            }
            else if (z.right == null)
            {
                x = z.left;
                Transplant(z, z.left);
            }
            else
            {
                y = Minimum(z.right); //Без проверок возможна бесконечная рекурсия
                y_original_Color = y.Color;
                x = y.right;
                if (y.parent == z)
                    x.parent = y;
                else
                {
                    Transplant(y, y.right);
                    y.right = z.right;
                    y.right.parent = y;
                }
                Transplant(z, y);
                y.left = z.left;
                y.left.parent = y;
                y.Color = z.Color;
            }

            if (y_original_Color == Color.Black && x)
                DeleteFixUp(x);

            return;
        }

        private void DeleteFixUp(Node X)
        {
            while (X != root && X.Color == Color.Black)
            {
                if (X == X.parent.left)
                {
                    Node Y = X.parent.right;
                    if (Y.Color == Color.Red)
                    {
                        Y.Color = Color.Black;
                        X.parent.Color = Color.Red;
                        leftRotate(X.parent);
                        Y = X.parent.right;
                    }
                    if (Y.left.Color == Color.Black && Y.right.Color == Color.Black)
                    {
                        Y.Color = Color.Red;
                        X = X.parent;
                    }
                    else
                    {
                        if (Y.right.Color == Color.Black)
                        {
                            Y.left.Color = Color.Black;
                            Y.Color = Color.Red;
                            rightRotate(Y);
                            Y = X.parent.right;
                        }
                        Y.Color = X.parent.Color;
                        X.parent.Color = Color.Black;
                        Y.right.Color = Color.Black;
                        leftRotate(X.parent);
                        X = root;
                    }
                }
                else
                {
                    Node Y = X.parent.left;
                    if (Y.Color == Color.Red)
                    {
                        Y.Color = Color.Black;
                        X.parent.Color = Color.Red;
                        rightRotate(X.parent);
                        Y = X.parent.left;
                    }
                    if (Y.right.Color == Color.Black && Y.left.Color == Color.Black)
                    {
                        Y.Color = Color.Black;
                        X = X.parent;
                    }
                    else
                    {
                        if (Y.left.Color == Color.Black)
                        {
                            Y.right.Color = Color.Black;
                            Y.Color = Color.Red;
                            leftRotate(Y);
                            Y = X.parent.left;
                        }
                        Y.Color = X.parent.Color;
                        X.parent.Color = Color.Black;
                        Y.left.Color = Color.Black;
                        rightRotate(X.parent);
                        X = root;
                    }
                }
            }
            X.Color = Color.Black;
        }

        #endregion

        #region Display

        public void DisplayTree(bool all = false)
        {
            int k = Display(root, Console.BufferWidth / 2, Console.BufferWidth / 2, 0, 4);
            Console.SetCursorPosition(0, k-1);
            if (all)
            {
                Console.WriteLine("InCenterrightDisplay"); InCenterrightDisplay(root); Console.WriteLine('\n');
                Console.WriteLine("InCenterleftDisplay"); InCenterleftDisplay(root); Console.WriteLine('\n');
                Console.WriteLine("InOrderDisplay"); InOrderDisplay(root); Console.WriteLine('\n');
                Console.WriteLine("InReverseDisplay"); InReverseDisplay(root); Console.WriteLine('\n');
            }
        }

        private void InCenterrightDisplay(Node current) //Центрированный обход 
        {
            if (!current)
                return;

            InCenterrightDisplay(current.right);
            Console.Write("(" + current + ") ");
            InCenterrightDisplay(current.left);
        }

        private void InCenterleftDisplay(Node current) //Центрированный обход 
        {
            if (!current)
                return;

            InCenterleftDisplay(current.left);
            Console.Write("(" + current + ") ");
            InCenterleftDisplay(current.right);
        }

        private void InOrderDisplay(Node current) //Прямой обход
        {
            if (!current)
                return;

            Console.Write("(" + current + ") ");
            InOrderDisplay(current.left);
            InOrderDisplay(current.right);
        }

        private void InReverseDisplay(Node current) //Обратный обход
        {
            if (!current)
                return;

            InReverseDisplay(current.left);
            InReverseDisplay(current.right);
            Console.Write("(" + current + ") ");
        }

        private int Display(Node node, int offset = 10, int x = 20, int y = 0, int lineoffset = 2)
        {
            if (!node)
                return y;

            Console.SetCursorPosition(x, y + 1); //For change Color
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            if (node.right || node.left)
            {
                Console.WriteLine("│");
                Console.SetCursorPosition(x, y + 2);
                if (node.right && node.left)
                    Console.WriteLine("┴");

                if (node.right && !node.left)
                    Console.WriteLine("└");

                if (!node.right && node.left)
                    Console.WriteLine("┘");

                for (int i = 0; i < offset / 2 - 1; i++)
                {
                    if (!node.right && node.left)
                    {
                        Console.SetCursorPosition(x - i - 1, y + 2);
                        Console.WriteLine("─");
                        if (i == offset / 2 - 2)
                        {
                            Console.SetCursorPosition(x - offset / 2 + 1, y + 2);
                            Console.WriteLine("┌");
                            Console.SetCursorPosition(x - offset / 2 + 1, y + 3);
                            Console.WriteLine("│");
                        }
                        continue;
                    }

                    if (node.right && !node.left)
                    {
                        Console.SetCursorPosition(x + i + 1, y + 2);
                        Console.WriteLine("─");
                        if (i == offset / 2 - 2)
                        {
                            Console.SetCursorPosition(x + offset / 2, y + 2);
                            Console.WriteLine("┐");
                            Console.SetCursorPosition(x + offset / 2, y + 3);
                            Console.WriteLine("│");
                        }
                        continue;
                    }

                    Console.SetCursorPosition(x - i - 1, y + 2);
                    Console.WriteLine("─");
                    if (i == offset / 2 - 2)
                    {
                        Console.SetCursorPosition(x - offset / 2 + 1, y + 2);
                        Console.WriteLine("┌");
                        Console.SetCursorPosition(x - offset / 2 + 1, y + 3);
                        Console.WriteLine("│");

                    }
                    Console.SetCursorPosition(x + i + 1, y + 2);
                    Console.WriteLine("─");
                    if (i == offset / 2 - 2)
                    {
                        Console.SetCursorPosition(x + offset / 2, y + 2);
                        Console.WriteLine("┐");
                        Console.SetCursorPosition(x + offset / 2, y + 3);
                        Console.WriteLine("│");

                    }
                }
            }

            Console.SetCursorPosition(x-3, y);
            Console.ForegroundColor = node.Color == Color.Red ? ConsoleColor.DarkRed : ConsoleColor.White;
            Console.Write(node.data);

            offset /= 2;
            int k = Display(node.left, offset, x - offset, y + lineoffset, lineoffset);
            int c = Display(node.right, offset, x + offset, y + lineoffset, lineoffset);
            return k > c ? k : c;
        }
        #endregion

        public Node Find(Key key)
        {
            Node temp = root;
            while (temp)
            {
                if (key < temp.data)
                    temp = temp.left;

                if (key > temp.data)
                    temp = temp.right;

                if (key == temp.data)
                    return temp;
            }

            return null;
        }

    }
}