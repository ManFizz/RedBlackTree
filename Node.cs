using System;

namespace BlackRedTree
{
    public class Node
    {
        public static implicit operator string(Node node)
        {
            int k = 0;
            List p = node.List;
            while (p)
            {
                k++;
                p = p.prev;
            }
            return node.data + (k != 0 ? " [" + (1 + k).ToString() + ']' : "");
        }

        public int GetLevel()
        {
            int k = 0;
            Node n = this;
            while (n)
            {
                if (n.Color == Color.Black)
                    k++;
                n = n.parent;
            }
            return k;
        }

        //Dynamic list
        public List List = null;

        public void AddEqual()
        {
            List = new List(List, data);
        }

        public void DeleteEqual()
        {
            List = List.Remove();
        }

        public bool HasEqual()
        {
            return List;
        }

        //Black Red Tree
        public Color Color;

        //Node AVL
        public Node left;
        public Node right;
        public Node parent;
        public Key data;

        //Initialization
        public Node(Key data) 
        { 
            this.data = data;
        }

        public Node(Color Color) 
        { 
            this.Color = Color; 
        }

        public Node(Key data, Color Color) 
        { 
            this.data = data; this.Color = Color; 
        }

        public Node(Node parent, Color Color) 
        {
            this.parent = parent;
            this.Color = Color;
        }

        //Func
        public static implicit operator bool(Node x) { return x != null; }
        public int Height() { return this.parent ? this.parent.Height() + 1 : 1; }

    }
}
