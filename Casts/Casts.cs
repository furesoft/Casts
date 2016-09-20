using System;
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

        public static event Action<Exception> OnError;

        static void RaiseOnError(Exception ex)
        {
            OnError?.Invoke(ex);
        }

        static Casts()
        {
            Providers.Add(new SizeProvider());
            Providers.Add(new PointProvider());
            Providers.Add(new GuidProvider());
            Providers.Add(new MethodBodyProvider());
            Providers.Add(new DateTimeProvider());
            Providers.Add(new BigIntegerProvider());
            Providers.Add(new ColorProvider());
            Providers.Add(new RectangleProvider());
            Providers.Add(new IPAddressProvider());
            Providers.Add(new BitArrayProvider());
        }

        #region Multiple Cast
#if RELEASE
        [DebuggerStepThrough]
#endif
        public static Tuple<T, U> pair_cast<T, U>(object o)
        {
            var f = reinterprete_cast<T>(o);
            var s = reinterprete_cast<U>(o);

            return new Tuple<T, U>(f, s);
        }

#if RELEASE
        [DebuggerStepThrough]
#endif
        public static IEnumerable<Out> sequence_cast<Out, In>(IEnumerable<In> src)
        {
            foreach (var r in src)
            {
                yield return reinterprete_cast<Out>(r);
            }
        }
        #endregion

#if RELEASE
        [DebuggerStepThrough]
#endif
        public static T struct_cast<T>(object obj)
            where T : struct
        {
            try
            {
                int size = Marshal.SizeOf(obj);
                
                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(obj, ptr, true);

                var res = (T) Marshal.PtrToStructure(ptr, typeof (T));
                Marshal.FreeHGlobal(ptr);
                
                return res;
            }
            catch (Exception ex)
            {
                RaiseOnError(ex);
            }

            return default(T);
        }

#if RELEASE
        [DebuggerStepThrough]
#endif
        static object reinterprete_cast(object val, Type T)
        {
            byte[] bytes = GetBytes(val);
            
            //primtive
            var t = Type.GetTypeCode(T);

            if (bytes != null)
            {
                if (t != TypeCode.String)
                {
                    if (bytes.Length <= 8)
                    {
                        var b = new List<byte>();
                        b.AddRange(bytes);
                        b.AddRange(new byte[] {0, 0, 0, 0, 0, 0, 0, 0});

                        bytes = b.ToArray();
                    }
                }

                foreach (var castProvider in Providers)
                {
                    var ret = castProvider.FromBinary(bytes, T);
                    if (ret != null) return ret;
                }

                var primitive = PrimitiveCast(val, T, t, bytes);
                if(primitive != null) return primitive;
            }
            
            RaiseOnError(new InvalidCastException($"Cant cast from '{val.GetType().Name}' to '{T.Name}'"));
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
        public static T reinterprete_cast<T>(object v)
        {
            return (T) reinterprete_cast(v, typeof (T));
        }

        public static bool trycast<T>(object o, out T value)
        {
            try
            {
                value = reinterprete_cast<T>(o);
                return value != null;
            }
            catch (Exception)
            {
                value = default(T);
            }
            return false;
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