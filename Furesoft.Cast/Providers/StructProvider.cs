using System;
using System.Runtime.InteropServices;

namespace Casts.Providers
{
    public class StructProvider : ICastProvider
    {
        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.IsValueType)
            {
                int size = Marshal.SizeOf(to);

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.Copy(raw, 0, ptr, size);

                var res = Marshal.PtrToStructure(ptr, to);
                Marshal.FreeHGlobal(ptr);

                return res;
            }
            return null;
        }

        public override byte[] ToBinary(object obj)
        {
            if (obj.GetType()?.BaseType?.Name == nameof(ValueType))
            {
                int size = Marshal.SizeOf(obj);
                byte[] arr = new byte[size];

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(obj, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
                Marshal.FreeHGlobal(ptr);

                return arr;
            }
            return null;
        }
    }
}