using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cast.Test
{
    public struct Vector2
    {
        public int X { get; set; }
        public int Y { get; set; }
        public static Vector2 Empty => new Vector2();

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return new {X, Y}.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{X}:{Y}]";
        }

        public override bool Equals(object obj)
        {
            var tmp = (Vector2) obj;
            return tmp.X == X && tmp.Y == Y;
        }

        public double Distance(Vector2 p2)
        {
            double dX = X - p2.X;
            double dY = Y - p2.Y;
            double multi = dX*dX + dY*dY;
            double rad = Math.Round(Math.Sqrt(multi), 3);

            return rad;
        }

        public static Vector2 operator ++(Vector2 p)
        {
            p.X++;
            p.Y++;
            return p;
        }

        public static Vector2 operator *(Vector2 v, int mul)
        {
            v.X *= mul;
            v.Y *= mul;

            return v;
        }

        public static bool operator <(Vector2 a, Vector2 b)
        {
            return a.X < b.X && a.Y < b.Y;
        }

        public static bool operator >(Vector2 a, Vector2 b)
        {
            return a.X > b.X && a.Y > b.Y;
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            a.X -= b.X;
            a.Y -= b.Y;

            return a;
        }

        public static explicit operator Vector2(byte[] raw)
        {
            var x = BitConverter.ToInt32(raw, 0);
            var y = BitConverter.ToInt32(raw, 4);

            return new Vector2 {X = x, Y = y};
        }

        public static explicit operator byte[](Vector2 m)
        {
            var buf = new List<byte>();
            buf.AddRange(BitConverter.GetBytes(m.X));
            buf.AddRange(BitConverter.GetBytes(m.Y));
            return buf.ToArray();
        }

        public static explicit operator Delegate(Vector2 raw)
        {
            return new Action(() => Debug.WriteLine(raw));
        }
    }
}