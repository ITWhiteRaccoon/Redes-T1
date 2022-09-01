namespace Redes_T1_Codificador;

public class Program
{
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

    public static void Codificador() { }
}
