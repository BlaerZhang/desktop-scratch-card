using UnityEngine;

public static class StringExtensions
{
    /// <summary>
    /// Turn int into K/M format string
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string ToKMB(this int num)
    {
        if (num >= 1000000)
            return (num / 1000000f).ToString("0.0") + "M";
        if (num >= 1000)
            return (num / 1000f).ToString("0.0") + "K";
        return num.ToString();
    }
}
