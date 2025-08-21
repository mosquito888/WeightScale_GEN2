using WeightScaleGen2.BGC.Models.Middleware;

internal class Program
{
    private static async Task Main(string[] args)
    {
        TestKey();
        TestText();
    }

    private static void TestText()
    {
        string privateKey = "DE4EEA5D75456A128A2A2BF6735EC";
        string publicKey = "HkdrOxQ6Yg";

        string textOrg = "BGC.co.th";
        string textEncrypt = "";
        string textDecrypt = "";

        textEncrypt = KeyTools.EncryptKey(textOrg, publicKey);
        textDecrypt = KeyTools.DecryptKey(textEncrypt, publicKey);

        Console.ReadLine();
    }

    private static void TestKey()
    {
        string privateKey = "DE4EEA5D75456A128A2A2BF6735EC";
        string publicKey = "HkdrOxQ6Yg";

        Console.WriteLine($"EncryptKey => {KeyTools.EncryptKey(privateKey, publicKey)}");
        Console.WriteLine($"Decrypt => {KeyTools.DecryptKey(KeyTools.EncryptKey(privateKey, publicKey), publicKey)}");

        Console.ReadLine();
    }
}