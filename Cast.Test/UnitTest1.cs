using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using Casts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Casts.Casts;

namespace Cast.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void reinterpret_test()
        {
            //bytes test
            var raw = new byte[] {1, 5, 255, 3, 4, 10, 15, 2, 3, 5};

            var prim = struct_cast<int>(uint.MaxValue);
            var test = reinterprete_cast<long>(123);

            var mul = pair_cast<int, short>(test);
            var list = sequence_cast<byte[], int>(new[] {1, 526, 5, 8});

            var struc = struct_cast<Vector2>(new Point(10, 20));

            var lim = struct_cast<Vector2>(new Mini(12));

            var r = reinterprete_cast<double>(raw);

            var rf = reinterprete_cast<double>(raw);
            var rI = reinterprete_cast<int>(raw);

            var iR = reinterprete_cast<byte[]>(rI);
            var fR = reinterprete_cast<byte[]>(rf);

            BigInteger f = 16666666666666666666;
            var ri = reinterprete_cast<int>(f);
            var rl = reinterprete_cast<float>(ri);
            var rd = reinterprete_cast<double>(rl);
            var rbi = reinterprete_cast<BigInteger>(rl);
            var L = reinterprete_cast<long>(123);
            var bL = reinterprete_cast<long>(f);

            var c = reinterprete_cast<uint>(uint.MaxValue);
            var cr = reinterprete_cast<int>(c);
            var u = reinterprete_cast<uint>(-1);

            var lambda = new Action<string>((_) => Debug.WriteLine(_));
            var callable = reinterprete_cast<ICallable>(lambda);
            callable.Call("Hello World");

            var func = reinterprete_cast<Action<string>>(callable);
            func("Hello Cast");

            var g = Guid.NewGuid();
            var rGu = reinterprete_cast<byte[]>(g);
            var iGu = reinterprete_cast<Guid>(rGu);

            var sL = reinterprete_cast<byte[]>(new Mini(12));
            var sD = reinterprete_cast<Mini>(sL);

            var same = reinterprete_cast<int>(1234);
            var call = reinterprete_cast<Action<string>>(sD);
            call("Object to Delegate");

            var expr = reinterprete_cast<Expression>(call);

            var strm = reinterprete_cast<MemoryStream>(new FileStream("test.txt", FileMode.Open));

            var rp = new Vector2(10, 10);
            var lp = reinterprete_cast<long>(rp);
            var rep = reinterprete_cast<Vector2>(lp);
            var dis = rp.Distance(++rep);

            var repC = reinterprete_cast<Action>(rep);
            repC();

            var body = reinterprete_cast<byte[]>(call);

            var dtR = reinterprete_cast<ulong>(DateTime.Now);
            var dtN = reinterprete_cast<DateTime>(rGu);
            var pDT = reinterprete_cast<DateTime>(rep*15963583);

            Action act;
            var succes = trycast(pDT, out act);

            var s = new Size();
            s.Height = 125;
            s.Width = 852;

            var castedPoint = reinterprete_cast<Point>(s);
        }
    }

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
            return new { X, Y }.GetHashCode();
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
            double multi = dX * dX + dY * dY;
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

        public static explicit operator byte[] (Vector2 m)
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

    struct Mini
    {
        readonly ushort val;

        public Mini(ushort v)
        {
            val = (ushort) (v & 0xC);
        }

        public override string ToString()
        {
            return val.ToString();
        }

        public static explicit operator Mini(byte[] raw)
        {
            return new Mini(BitConverter.ToUInt16(raw, 0));
        }

        public static explicit operator byte[](Mini m)
        {
            return BitConverter.GetBytes(m.val);
        }

        public static explicit operator Delegate(Mini raw)
        {
            return new Action<string>((_) => Debug.WriteLine(_));
        }
    }
}