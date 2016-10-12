using System;
using System.CodeDom;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
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
            var ba = new BitArray(16);
            ba.SetAll(true);
            ba.Parse("0001 0011");

            var baI = reinterpret_cast<ushort>(ba);
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

            var ipL = reinterpret_cast<int>(IPAddress.Parse("127.0.0.1"));
            var ipP = reinterpret_cast<IPAddress>(ipL);

            var ph = new Phone();
            ph.Vorwahl = 497952;
            ph.Number = 6771;

            var PhR = reinterpret_cast<byte[]>(ph);
            var phone = reinterpret_cast<Phone>(PhR);
        }
    }

    public static class Extensions
    {
        public static void Parse(this BitArray ba, string pattern)
        {
            var spl = pattern.Replace(" ", "").ToCharArray();

            for (int i = 0; i < spl.Length; i++)
            {
                ba[i] = bool.Parse(pattern[i].ToString());
            }
        }
    }
}