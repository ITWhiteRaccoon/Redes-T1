using System.Text;

namespace Decodificador;

public class Program
{
    private static readonly Dictionary<string, char> BinToHexChar = new()
    {
        { "0000", '0' },
        { "0001", '1' },
        { "0010", '2' },
        { "0011", '3' },
        { "0100", '4' },
        { "0101", '5' },
        { "0110", '6' },
        { "0111", '7' },
        { "1000", '8' },
        { "1001", '9' },
        { "1010", 'a' },
        { "1011", 'b' },
        { "1100", 'c' },
        { "1101", 'd' },
        { "1110", 'e' },
        { "1111", 'f' }
    };

    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("erro");
            return;
        }

        switch (args[0])
        {
            case "nrzi":
                Console.WriteLine(nrziDecode(args[1]));
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

    public static string nrziDecode(string binInput)
    {
        var decodedData = new StringBuilder();
        var digit = 0;
        var lastSignal = '-';
        for (var i = 0; i <= binInput.Length - 4; i += 4)
        {
            var hex = new StringBuilder();
            foreach (var c in binInput[i..(i + 4)])
            {
                digit = c != lastSignal ? 1 : 0;

                lastSignal = c;
                hex.Append(digit);
            }

            hex.Append(' ');
            decodedData.Append(hex);
        }

        return decodedData.ToString();
    }
}
