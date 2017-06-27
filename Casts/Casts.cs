using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Casts.Providers;

// ReSharper disable All

namespace Casts
{
    public static class Casts
    {
        public static List<ICastProvider> Providers = new List<ICastProvider>();

        static Casts()
        {
            Providers.Add(new StructProvider());
            Providers.Add(new GuidProvider());
            Providers.Add(new DateTimeProvider());
            Providers.Add(new BigIntegerProvider());
            Providers.Add(new IPAddressProvider());
            Providers.Add(new BitArrayProvider());
        }

#if RELEASE
        [DebuggerStepThrough]
#endif
        static object reinterpret_cast(object val, Type T)
        {
            byte[] bytes = GetBytes(val);
            
            //primtive
            var t = Type.GetTypeCode(T);
            
            if (bytes != null)
            {
                
                if (t != TypeCode.String && T.Name != nameof(BitArray))
                {
                    if (bytes.Length <= 8)
                    {
                        var b = new List<byte>();
                        b.AddRange(bytes);
                        b.AddRange(new byte[] {0, 0, 0, 0, 0, 0, 0, 0});

                        bytes = b.ToArray();
                    }
                }

                var primitive = PrimitiveCast(val, T, t, bytes);
                if (primitive != null) return primitive;

                foreach (var castProvider in Providers)
                {
                    var ret = castProvider.FromBinary(bytes, T);
                    if (ret != null) return ret;
                }
            }
            
            throw (new InvalidCastException($"Cant cast from '{val.GetType().Name}' to '{T.Name}'"));
            return null;
        }

        public static TimeSpan Time(Action a)
        {
            var sw = new Stopwatch();
            sw.Start();
            a();
            sw.Stop();

            return sw.Elapsed;
        }

        static object PrimitiveCast(object val, Type T, TypeCode t, byte[] bytes)
        {
            switch (t)
            {
                case TypeCode.Char:
                    return BitConverter.ToChar(bytes, 0);
                case TypeCode.Int32:
                    return BitConverter.ToInt32(bytes, 0);
                case TypeCode.Single:
                    return BitConverter.ToSingle(bytes, 0);
                case TypeCode.Boolean:
                    return BitConverter.ToBoolean(bytes, 0);
                case TypeCode.Byte:
                    return bytes[0];
                case TypeCode.Int16:
                    return BitConverter.ToInt16(bytes, 0);
                case TypeCode.UInt16:
                    return BitConverter.ToUInt16(bytes, 0);
                case TypeCode.UInt32:
                    return BitConverter.ToUInt32(bytes, 0);
                case TypeCode.Int64:
                    return BitConverter.ToInt64(bytes, 0);
                case TypeCode.UInt64:
                    return BitConverter.ToUInt64(bytes, 0);
                case TypeCode.Double:
                    return BitConverter.ToDouble(bytes, 0);
                case TypeCode.DateTime:
                    return DateTime.FromBinary(BitConverter.ToInt64(bytes, 0));
                case TypeCode.String:
                    return Encoding.Default.GetString(bytes);
                case TypeCode.Object:
                    if (T.Name == typeof (byte[]).Name)
                    {
                        return GetBytes(val);
                    }
                    else
                    {
                        var m = T.GetMethods();
                        var s =
                            m.Where(
                                _ =>
                                    _.ReturnType.Name == T.Name &&
                                    _.GetParameters().FirstOrDefault()?.ParameterType.Name == typeof (byte[]).Name);

                        if (s.Any())
                        {
                            return s.FirstOrDefault().Invoke(val, new object[] {bytes});
                        }
                    }
                    break;
            }
            return null;
        }

#if RELEASE
        [DebuggerStepThrough]
#endif
        public static T reinterpret_cast<T>(object v)
        {
            return (T) reinterpret_cast(v, typeof (T));
        }

        static byte[] GetBytes(object val)
        {
            var t = Type.GetTypeCode(val.GetType());
            var typ = val.GetType();

            switch (t)
            {
                case TypeCode.Char:
                    return BitConverter.GetBytes((char)val);
                case TypeCode.Int32:
                    return BitConverter.GetBytes((int)val);
                case TypeCode.Single:
                    return BitConverter.GetBytes((float)val);
                case TypeCode.Boolean:
                    return BitConverter.GetBytes((bool)val);
                case TypeCode.SByte:
                    return BitConverter.GetBytes((sbyte)val);
                case TypeCode.Byte:
                    return BitConverter.GetBytes((byte)val);
                case TypeCode.Int16:
                    return BitConverter.GetBytes((short)val);
                case TypeCode.UInt16:
                    return BitConverter.GetBytes((ushort)val);
                case TypeCode.UInt32:
                    return BitConverter.GetBytes((uint)val);
                case TypeCode.Int64:
                    return BitConverter.GetBytes((long)val);
                case TypeCode.UInt64:
                    return BitConverter.GetBytes((ulong)val);
                case TypeCode.Double:
                    return BitConverter.GetBytes((double)val);
                case TypeCode.DateTime:
                    return BitConverter.GetBytes(((DateTime)val).ToBinary());
                case TypeCode.String:
                    return Encoding.Default.GetBytes((string)val);
                default:
                    if (val is byte[])
                    {
                        return (byte[]) val;
                    }
                    else
                    {
                        var m = typ.GetMethods();
                        var s =
                            m.Where(
                                _ =>
                                    _.ReturnType.Name == typeof (byte[]).Name &&
                                    _.GetParameters().FirstOrDefault()?.ParameterType.Name == val.GetType().Name);

                        if (s.Any())
                        {
                            return (byte[]) s.FirstOrDefault().Invoke(val, new object[] {val});
                        }
                        else
                        {
                            foreach (var castProvider in Providers)
                            {
                                var ret = castProvider.ToBinary(val);
                                if (ret != null) return ret;
                            }
                        }
                    }
                    break;
            }

            return null;
        }
    }
}