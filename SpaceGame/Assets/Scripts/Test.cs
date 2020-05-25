
using System.Security.Cryptography;

public static class Test
{
    private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

    public static byte[] MakeABunchOfBytes()
    {
        byte[] blob = new byte[64];
        rngCsp.GetBytes(blob);
        return blob;
    }
}
