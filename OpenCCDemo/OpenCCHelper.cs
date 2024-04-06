using System.Runtime.InteropServices;
using System.Text;

public static class OpenCCHelper
{
    [DllImport(@"C:\Tools\opencc\bin\opencc.dll", EntryPoint = "opencc_open")]
    private static extern IntPtr opencc_open(string configFileName);

    [DllImport(@"C:\Tools\opencc\bin\opencc.dll", EntryPoint = "opencc_convert_utf8")]
    private static extern IntPtr opencc_convert_utf8(IntPtr opencc, IntPtr input, long length);

    public static string ConvertS2TWP(this string text)
    {
        IntPtr opencc = opencc_open(@"C:\Tools\OpenCC\share\opencc\s2twp.json");
        try
        {
            int len = Encoding.UTF8.GetByteCount(text);
            byte[] buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(text, 0, text.Length, buffer, 0);
            IntPtr inStr = Marshal.AllocHGlobal(buffer.Length);
            try
            {
                Marshal.Copy(buffer, 0, inStr, buffer.Length);
                IntPtr outStr = opencc_convert_utf8(opencc, inStr, -1);
                try
                {
                    int outLen = 0;
                    while (Marshal.ReadByte(outStr, outLen) != 0) ++outLen;
                    byte[] outBuffer = new byte[outLen];
                    Marshal.Copy(outStr, outBuffer, 0, outBuffer.Length);
                    return Encoding.UTF8.GetString(outBuffer);
                }
                finally
                {
                    Marshal.FreeHGlobal(outStr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(inStr);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(opencc);
        }
    }

    public static string ConvertTW2SP(this string text)
    {
        IntPtr opencc = opencc_open(@"C:\Tools\OpenCC\share\opencc\tw2sp.json");
        try
        {
            int len = Encoding.UTF8.GetByteCount(text);
            byte[] buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(text, 0, text.Length, buffer, 0);
            IntPtr inStr = Marshal.AllocHGlobal(buffer.Length);
            try
            {
                Marshal.Copy(buffer, 0, inStr, buffer.Length);
                IntPtr outStr = opencc_convert_utf8(opencc, inStr, -1);
                try
                {
                    int outLen = 0;
                    while (Marshal.ReadByte(outStr, outLen) != 0) ++outLen;
                    byte[] outBuffer = new byte[outLen];
                    Marshal.Copy(outStr, outBuffer, 0, outBuffer.Length);
                    return Encoding.UTF8.GetString(outBuffer);
                }
                finally
                {
                    Marshal.FreeHGlobal(outStr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(inStr);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(opencc);
        }
    }
}
