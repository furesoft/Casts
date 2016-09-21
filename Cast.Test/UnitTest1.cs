using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Casts.Casts;

namespace Cast.Test
{
    [TestClass]
    public class UnitTest1
    {
        public static void Main()
        {
            new UnitTest1().Test();    
        }

        [TestMethod]
        public void Test()
        {
            //time test
            var timeL = Time(() => { var l = (long) 123; });
            var timeI = Time(() => { var l = (int) 123L; });

            var timeRL = Time(() => { reinterpret_cast<long>(123); });
            var timeRI = Time(() => { reinterpret_cast<int>(123L); });

            //bitarray test
            var ba = new BitArray(32);
            ba.SetAll(true);

            var baI = reinterpret_cast<int>(ba);
            var Iba = reinterpret_cast<BitArray>((short) 255);
            
            //bytes test
            var raw = new byte[] {1, 5, 255, 3, 4, 10, 15, 2, 3, 5};

            var prim = struct_cast<int>(uint.MaxValue);
            var test = reinterpret_cast<long>(123);

            var mul = pair_cast<int, short>(test);
            var list = sequence_cast<byte[], int>(new[] {1, 526, 5, 8});

            var struc = struct_cast<Vector2>(new Point(10, 20));

            var lim = struct_cast<Vector2>(new Mini(12));

            var r = reinterpret_cast<double>(raw);

            var rf = reinterpret_cast<double>(raw);
            var rI = reinterpret_cast<int>(raw);

            var iR = reinterpret_cast<byte[]>(rI);
            var fR = reinterpret_cast<byte[]>(rf);

            BigInteger f = 16666666666666666666;
            var ri = reinterpret_cast<int>(f);
            var rl = reinterpret_cast<float>(ri);
            var rd = reinterpret_cast<double>(rl);
            var rbi = reinterpret_cast<BigInteger>(rl);
            var L = reinterpret_cast<long>(123);
            var bL = reinterpret_cast<long>(f);

            var c = reinterpret_cast<uint>(uint.MaxValue);
            var cr = reinterpret_cast<int>(c);
            var u = reinterpret_cast<uint>(-1);


            var g = Guid.NewGuid();
            var rGu = reinterpret_cast<byte[]>(g);
            var iGu = reinterpret_cast<Guid>(rGu);

            var sL = reinterpret_cast<byte[]>(new Mini(12));
            var sD = reinterpret_cast<Mini>(sL);

            var same = reinterpret_cast<int>(1234);
            var call = reinterpret_cast<Action>(sD);
            call();

            var rp = new Vector2(10, 10);
            var lp = reinterpret_cast<long>(rp);
            var rep = reinterpret_cast<Vector2>(lp);
            var dis = rp.Distance(++rep);

            var repC = reinterpret_cast<Action>(rep);
            repC();

            var body = reinterpret_cast<byte[]>(call);

            var dtR = reinterpret_cast<ulong>(DateTime.Now);
            var dtN = reinterpret_cast<DateTime>(rGu);
            var pDT = reinterpret_cast<DateTime>(rep*15963583);

            Action act;
            var succes = trycast(rep, out act);

            var s = new Size();
            s.Height = 125;
            s.Width = 852;

            var castedPoint = reinterpret_cast<Point>(s);
            var ip = reinterpret_cast<IPAddress>("127.0.0.1");

            var ipL = reinterpret_cast<int>(ip);
            var ipP = reinterpret_cast<IPAddress>(ipL);
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
            return new Action(() => Debug.WriteLine(raw.val));
        }
    }
}