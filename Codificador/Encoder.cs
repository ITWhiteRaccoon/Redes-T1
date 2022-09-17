using System.Text;

namespace Codificador;

public class Encoder
{
    enum Signal
    {
        Minus = 0,
        Plus = 1
    }

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
            string hexInput = args[1].ToLower();
            switch (args[0].ToLower())
            {
                case "nrzi":
                    Console.WriteLine(EncodeNrzi(hexInput));
                    break;
                case "mdif":
                    Console.WriteLine(EncodeMdif(hexInput));
                    break;
                case "hdb3":
                    Console.WriteLine(EncodeHdb3(hexInput));
                    break;
                case "8b6t":
                    Console.WriteLine(Encode8B6T(hexInput));
                    break;
                case "6b8b":
                    Console.WriteLine(Encode6B8B(hexInput));
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

    public static string EncodeNrzi(string hexInput)
    {
        StringBuilder encodedData = new();
        char signal = '-';
        foreach (char hex in hexInput.ToLower())
        {
            //Lê cada caractere de entrada como hexa e transforma na representação em binário
            foreach (char bin in HexCharToBin[hex])
            {
                //Para cada bit da representação em binário, se for 1 inverte o sinal atual
                if (bin == '1')
                {
                    signal = signal switch
                    {
                        '-' => '+',
                        '+' => '-'
                    };
                }

                //Adiciona o dado codificado ao fim da string
                encodedData.Append(signal);
            }
        }

        return encodedData.ToString();
    }

    public static string EncodeMdif(string hexInput)
    {
        StringBuilder encodedData = new();
        char lastSignal = '-';
        foreach (char hex in hexInput.ToLower())
        {
            foreach (char bin in HexCharToBin[hex])
            {
                //TODO mudar para escrever o primeiro sinal depois inverter e escrever de novo
                
                switch (bin)
                {
                    case '0' when lastSignal == '-':
                        encodedData.Append("+-");
                        break;
                    case '0' when lastSignal == '+':
                        encodedData.Append("-+");
                        break;
                    case '1' when lastSignal == '-':
                        encodedData.Append("-+");
                        break;
                    case '1' when lastSignal == '+':
                        encodedData.Append("+-");
                        break;
                }

                lastSignal = encodedData[^1];
            }
        }

        return encodedData.ToString();
    }


    public static string Encode8B6T(string hexInput)
    {
        return "";
    }

    public static string Encode6B8B(string hexInput)
    {
        return "";
    }

    public static string EncodeHdb3(string hexInput)
    {
        return "";
    }
}