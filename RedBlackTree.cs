using System;


namespace BlackRedTree
{
    class RedBlackTree
    {
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

        public Node root;

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

        #region Insert

        private void InsertFixUp(Node item)
        {
            // x and y are used as variable names for brevity, in a more formal
            // implementation, you should probably change the names

            // maintain red-black tree properties after adding newNode
            while (item != root && item.Parent.Color == Color.Red)
            {
                // Parent node is .Colored red; 
                Node workNode;
                if (item.Parent == item.Parent.Parent.left) // determine traversal path			
                {                                       // is it on the left or right subtree?
                    workNode = item.Parent.Parent.right;            // get uncle
                    if (workNode != null && workNode.Color == Color.Red)
                    {   // uncle is red; change x's Parent and uncle to black
                        item.Parent.Color = Color.Black;
                        workNode.Color = Color.Black;
                        // grandParent must be red. Why? Every red node that is not 
                        // a leaf has only black children 
                        item.Parent.Parent.Color = Color.Red;
                        item = item.Parent.Parent;  // continue loop with grandParent
                    }
                    else
                    {
                        // uncle is black; determine if newNode is greater than Parent
                        if (item == item.Parent.right)
                        {   // yes, newNode is greater than Parent; rotate left
                            // make newNode a left child
                            item = item.Parent;
                            leftRotate(item);
                        }
                        // no, newNode is less than Parent
                        item.Parent.Color = Color.Black; // make Parent black
                        item.Parent.Parent.Color = Color.Red;        // make grandParent black
                        rightRotate(item.Parent.Parent);                    // rotate right
                    }
                }
                else
                {   // newNode's Parent is on the right subtree
                    // this code is the same as above with "left" and "right" swapped
                    workNode = item.Parent.Parent.left;
                    if (workNode != null && workNode.Color == Color.Red)
                    {
                        item.Parent.Color = Color.Black;
                        workNode.Color = Color.Black;
                        item.Parent.Parent.Color = Color.Red;
                        item = item.Parent.Parent;
                    }
                    else
                    {
                        if (item == item.Parent.left)
                        {
                            item = item.Parent;
                            rightRotate(item);
                        }
                        item.Parent.Color = Color.Black;
                        item.Parent.Parent.Color = Color.Red;
                        leftRotate(item.Parent.Parent);
                    }
                }
            }
            root.Color = Color.Black;       // rbTree should always be black
        }

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
            z.Parent = Y;
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

        #endregion

        #region Rotates
        private void rightRotate(Node rotateNode)
        {

            Node workNode = rotateNode.left;
            rotateNode.left = workNode.right;

            if (workNode.right != null)
                workNode.right.Parent = rotateNode;

            if (workNode != null)
                workNode.Parent = rotateNode.Parent;

            if (rotateNode.Parent != null)
            {
                if (rotateNode == rotateNode.Parent.right)
                    rotateNode.Parent.right = workNode;
                else
                    rotateNode.Parent.left = workNode;
            }
            else
                root = workNode;

            workNode.right = rotateNode;
            if (rotateNode != null)
                rotateNode.Parent = workNode;
        }

        private void leftRotate(Node rotateNode)
        {
            Node workNode = rotateNode.right;
            rotateNode.right = workNode.left;

            if (workNode.left != null)
                workNode.left.Parent = rotateNode;

            if (workNode != null)
                workNode.Parent = rotateNode.Parent;

            if (rotateNode.Parent != null)
            {
                if (rotateNode == rotateNode.Parent.left)
                    rotateNode.Parent.left = workNode;
                else
                    rotateNode.Parent.right = workNode;
            }
            else
                root = workNode;

            workNode.left = rotateNode;
            if (rotateNode != null)
                rotateNode.Parent = workNode;
        }
        #endregion

        #region Delete

        private static Node Maximum(Node X)
        {
            while (X.right)
                X = X.right;

            return X ? X : Minimum(X);
        }

        private static Node Minimum(Node X)
        {
            while (X.left)
                X = X.left;

            return X ? X : Maximum(X);
        }

        public void Transplant(Node u, Node v)
        {
            if (u.Parent == null)
                root = v;
            else if (u == u.Parent.left)
                u.Parent.left = v;
            else u.Parent.right = v;
            if (v)
                v.Parent = u.Parent;
        }

        public bool Delete(Key key)
        {
            Node z = Find(key);
            bool bOut = z;
            Delete(z);
            return bOut;
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
            if (z.left == null && z.right == null)
            {
                x = z;
            }
            else if (z.left == null)
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
                if (y.Parent == z)
                    x.Parent = y;
                else
                {
                    Transplant(y, y.right);
                    y.right = z.right;
                    y.right.Parent = y;
                }
                Transplant(z, y);
                y.left = z.left;
                y.left.Parent = y;
                y.Color = z.Color;
            }

            if (y_original_Color == Color.Black)
                DeleteFixUp(x);

            return;
        }

        private void DeleteFixUp(Node X)
        {
            while (X != null && X != root && X.Color == Color.Black)
            {
                if (X == X.Parent.left)
                {
                    Node W = X.Parent.right;
                    if (W.Color == Color.Red)
                    {
                        W.Color = Color.Black;
                        X.Parent.Color = Color.Red;
                        leftRotate(X.Parent);
                        W = X.Parent.right;
                    }
                    if (W.left.Color == Color.Black && W.right.Color == Color.Black)
                    {
                        W.Color = Color.Red;
                        X = X.Parent;
                    }
                    else if (W.right.Color == Color.Black)
                    {
                        W.left.Color = Color.Black;
                        W.Color = Color.Red;
                        rightRotate(W);
                        W = X.Parent.right;
                    }
                    W.Color = X.Parent.Color;
                    X.Parent.Color = Color.Black;
                    W.right.Color = Color.Black;
                    leftRotate(X.Parent);
                    X = root;
                }
                else
                {
                    Node W = X.Parent.left;
                    if (W.Color == Color.Red)
                    {
                        W.Color = Color.Black;
                        X.Parent.Color = Color.Red;
                        rightRotate(X.Parent);
                        W = X.Parent.left;
                    }
                    if (W.right.Color == Color.Black && W.left.Color == Color.Black)
                    {
                        W.Color = Color.Black;
                        X = X.Parent;
                    }
                    else if (W.left.Color == Color.Black)
                    {
                        W.right.Color = Color.Black;
                        W.Color = Color.Red;
                        leftRotate(W);
                        W = X.Parent.left;
                    }
                    W.Color = X.Parent.Color;
                    X.Parent.Color = Color.Black;
                    W.left.Color = Color.Black;
                    rightRotate(X.Parent);
                    X = root;
                }
            }
            if (X)
                X.Color = Color.Black;
        }
        #endregion

        #region Display

        public void DisplayTree()
        {
            Display(root, Console.BufferWidth / 2, Console.BufferWidth / 2, 0, 4);
            /*
            Console.WriteLine("Tree");
            Draw(root);
            Console.WriteLine();
            Console.WriteLine("InCenterrightDisplay"); InCenterrightDisplay(root); Console.WriteLine('\n');
            Console.WriteLine("InCenterleftDisplay"); InCenterleftDisplay(root); Console.WriteLine('\n');
            Console.WriteLine("InOrderDisplay"); InOrderDisplay(root); Console.WriteLine('\n');
            Console.WriteLine("InReverseDisplay"); InReverseDisplay(root); Console.WriteLine('\n');*/

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

        public void Draw(Node n, String prefix = "", bool isleft = false, bool hasright = false)
        {
            if (n != null)
            {
                Console.Write(prefix + (isleft && hasright ? "╠═ " : "╚═ "));
                Console.ForegroundColor = n.Color == Color.Red ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine((string)n);
                Console.ForegroundColor = ConsoleColor.White;
                Draw(n.left, prefix + (isleft ? "║  " : "   "), true, n.right);
                Draw(n.right, prefix + (isleft ? "║  " : "   "), false);
            }
        }

        private void Display(Node node, int offset = 10, int x = 20, int y = 0, int lineoffset = 2)
        {
            Console.SetCursorPosition(x, y + 1);
            Console.ForegroundColor = ConsoleColor.Blue;
            if (node.right != null && node.left != null)
            {
                Console.WriteLine("│");
                Console.SetCursorPosition(x, y + 2);
                Console.WriteLine("┴");
            }
            if (node.right != null && node.left == null)
            {
                Console.WriteLine("│");
                Console.SetCursorPosition(x, y + 2);
                Console.WriteLine("└");
            }
            if (node.right == null && node.left != null)
            {
                Console.WriteLine("│");
                Console.SetCursorPosition(x, y + 2);
                Console.WriteLine("┘");
            }

            for (int i = 0; i < offset / 2 - 1; i++)
            {
                if (node.right == null && node.left != null)
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
                }
                if (node.right != null && node.left == null)
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
                }
                if (node.right != null && node.left != null)
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

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = node.Color == Color.Red ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write(node.data);
            offset /= 2;

            if (node.left != null)
            {
                Display(node.left, offset, x - offset, y + lineoffset, lineoffset);
            }
            if (node.right != null)
            {
                Display(node.right, offset, x + offset, y + lineoffset, lineoffset);
            }
        }
        #endregion
    }
}