using System;

namespace BlackRedTree
{
    public class List
    {
        public List prev;
        public Key val;

        public List(Key data)
        {
            val = data;
            prev = null;
        }

        public List(List link, Key data)
        {
            val = data;
            prev = link;
        }

        public List Remove()
        {
            if (!prev)
                return null;

            prev = prev.prev;
            return this;
        }

        public static implicit operator bool(List p)
        {
            return p != null;
        }

        ~List()
        {
            List p = this;
            while (p)
                p.Remove();
        }
    }
}
