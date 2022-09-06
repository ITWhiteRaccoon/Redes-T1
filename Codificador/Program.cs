using System.Text;

namespace Codificador;

public class Program
{
    private static readonly Dictionary<char, string> HexCharToBin = new()
    {
        { '0', "0000" },
        { '1', "0001" },
        { '2', "0010" },
        { '3', "0011" },
        { '4', "0100" },
        { '5', "0101" },
        { '6', "0110" },
        { '7', "0111" },
        { '8', "1000" },
        { '9', "1001" },
        { 'a', "1010" },
        { 'b', "1011" },
        { 'c', "1100" },
        { 'd', "1101" },
        { 'e', "1110" },
        { 'f', "1111" }
    };

    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("erro");
            return;
        }

        try
        {
            switch (args[0])
            {
                case "nrzi":
                    Console.WriteLine(nrziEncode(args[1]));
                    break;
                case "mdif":
                case "hdb3":
                case "8b6t":
                case "6b8b":
                    Console.WriteLine($"Selected {args[0]} with data {args[1]}");
                    break;
                default:
                    Console.WriteLine("erro");
                    break;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("erro");
        }
    }

    public static string nrziEncode(string hexInput)
    {
        var encodedData = new StringBuilder();
        var signal = '-';
        foreach (var c in hexInput)
        {
            foreach (var bin in HexCharToBin[c])
            {
                if (bin == '1')
                {
                    signal = signal switch
                    {
                        '-' => '+',
                        '+' => '-'
                    };
                }

                encodedData.Append(signal);
            }
        }

        return encodedData.ToString();
    }
}
