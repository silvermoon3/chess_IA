using System;

namespace processAI1.Board
{
    public static class Util
    {

        public static int round(double x)
        {
            return (int)(Math.Floor(x + 0.5));
        }

        public static int div(int a, int b)
        {
            //TODO ASSERT
            //assert(b > 0);

            int div = a / b;
            if (a < 0 && a != b * div) div--; // fix buggy C semantics

            return div;
        }

        public static int sqrt(int n)
        {
            return (int)Math.Sqrt((double)n);
        }

        public static bool is_square(int n)
        {
            int i = sqrt(n);
            return i * i == n;
        }

        public static double rand_float()
        {
            return (double)(processAI1.RandomNumbers.NextNumber()) / ((double)32767 + 1.0);
        }

        public static int rand_int(int n)
        {
            //TODO ASSERT
            //assert(n > 0);
            return (int)(rand_float() * (double)n);
        }

      
        public static bool string_case_equal(ref string s0, ref string s1)
        {

            if (s0.Length != s1.Length) return false;

            for (int i = 0; i < (int)(s0.Length); i++)
            {
                if (Char.ToLower(s0[i]) != Char.ToLower(s1[i])) return false;
            }

            return true;
        }

        public static bool to_bool(ref string s)
        {
            string t = "true";
            string f = "false";

            if (string_case_equal(ref s,ref t))
            {
                return true;
            }
            else if (string_case_equal(ref s, ref f))
            {
                return false;
            }
            else
            {
                throw new Exception("not a boolean: "+ s);
            }
        }
        public static void Assert(bool b, string exp)
        {
            if (!b)
                throw new Exception("expression suivante est fausse : "+exp);
        }
    }
}
