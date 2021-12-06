using System;

namespace BlackRedTree
{
    public class Key
    {
        private int[] f = { 0, 0, 0 };

        public Key(int k1, int k2, int k3)
        {
            f = new int[] { k1, k2, k3 };
        }

        public Key(string str)
        {
            f[0] = f[1] = f[2] = 0; // save for TryParse
            string[] arr = str.Split('.');
            int.TryParse(arr[0], out f[0]);
            int.TryParse(arr[1], out f[1]);
            int.TryParse(arr[2], out f[2]);
        }

        public static implicit operator Key(string str)
        {
            return new Key(str);
        }

        public static implicit operator string(Key p)
        {
            return p.f[0].ToString("00") + '.' + p.f[1].ToString("00") + '.' + p.f[2].ToString("00");
        }

        private static int Check(Key a, Key b)
        {
            if (a.f[0] < b.f[0])
                return -1;
            else if (a.f[0] > b.f[0])
                return 1;

            if (a.f[1] < b.f[1])
                return -1;
            else if (a.f[1] > b.f[1])
                return 1;

            if (a.f[2] < b.f[2])
                return -1;
            else if (a.f[2] > b.f[2])
                return 1;

            return 0; // a = b
        }

        public static bool operator !=(Key a, Key b)
        {
            return Check(a, b) != 0;
        }

        public static bool operator ==(Key a, Key b)
        {
            return Check(a, b) == 0;
        }

        public static bool operator <(Key a, Key b)
        {
            return Check(a, b) == -1;
        }

        public static bool operator >(Key a, Key b)
        {
            return Check(a, b) == 1;
        }
    }
}
