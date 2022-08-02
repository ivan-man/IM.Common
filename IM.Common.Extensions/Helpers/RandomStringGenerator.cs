namespace IM.Common.Extensions.Helpers;

public static class RandomStringGenerator
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static readonly Random Random = new Random();

    public static string Generate(int length)
    {
        var result = new string(Enumerable.Repeat(Chars, length)
            .Select(x => x[Random.Next(x.Length)]).ToArray());

        return result;
    }
}
