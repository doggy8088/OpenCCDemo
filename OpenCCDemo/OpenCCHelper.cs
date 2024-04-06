using System.Runtime.InteropServices;
using System.Text;

public static class OpenCCHelper
{
    [DllImport(@"C:\Tools\opencc\bin\opencc.dll", EntryPoint = "opencc_open")]
    private static extern IntPtr opencc_open(string configFileName);

    [DllImport(@"C:\Tools\opencc\bin\opencc.dll", EntryPoint = "opencc_convert_utf8")]
    private static extern IntPtr opencc_convert_utf8(IntPtr opencc, IntPtr input, long length);

    public static string ConvertFromSimplifiedToTraditional(this string text, string config = "s2t")
    {
        return OpenCC(text, config: config);
    }

    public static string ConvertFromSimplifiedToTraditionalTaiwan(this string text, string config = "s2twp")
    {
        return OpenCC(text, config: config);
    }

    public static string ConvertFromTraditionalTaiwanToSimplified(this string text, string config = "tw2sp")
    {
        return OpenCC(text, config: config);
    }

    public static string ConvertFromTraditionalToSimplified(this string text, string config = "t2s")
    {
        return OpenCC(text, config: config);
    }

    public static string OpenCC(this string text, string config)
    {
        var configFile = @"C:\Tools\OpenCC\share\opencc\" + config + ".json";
        if (!File.Exists(configFile))
        {
            throw new FileNotFoundException("設定檔找不到", configFile);
        }

        IntPtr opencc = opencc_open(configFile);
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
