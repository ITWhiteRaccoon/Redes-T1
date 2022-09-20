namespace Util;

public class IO
{
    public static Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(string filename, int keyPosition = 0,
        int valuePosition = 1) where TKey : notnull
    {
        var dict = new Dictionary<TKey, TValue>();
        var file = File.ReadLines(filename);
        foreach (string line in file)
        {
            string[] splitLine = line.Split(',');
            dict[(TKey)Convert.ChangeType(splitLine[keyPosition], typeof(TKey))] =
                (TValue)Convert.ChangeType(splitLine[valuePosition], typeof(TValue));
        }

        return dict;
    }
}