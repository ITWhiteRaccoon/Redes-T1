using System.Text;
using Util;

namespace Codificador;

public class Encoder
{
    private readonly Dictionary<char, string> _hexCharToBin;

    public Encoder()
    {
        _hexCharToBin = IO.ReadDictionary<char, string>("Dados/hex-bin.csv");
    }

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
            var encoder = new Encoder();
            switch (args[0].ToLower())
            {
                case "nrzi":
                    Console.WriteLine(encoder.EncodeNrzi(hexInput));
                    break;
                case "mdif":
                    Console.WriteLine(encoder.EncodeMdif(hexInput));
                    break;
                case "hdb3":
                    Console.WriteLine(encoder.EncodeHdb3(hexInput));
                    break;
                case "8b6t":
                    Console.WriteLine(encoder.Encode8B6T(hexInput));
                    break;
                case "6b8b":
                    Console.WriteLine(encoder.Encode6B8B(hexInput));
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

    public string EncodeNrzi(string hexInput)
    {
        StringBuilder encodedData = new();
        var lastSignal = '-';
        foreach (char hex in hexInput.ToLower())
        {
            //Lê cada caractere de entrada como hexa e transforma na representação em binário
            foreach (char bin in _hexCharToBin[hex])
            {
                //Para cada bit da representação em binário, se for 1 inverte o sinal atual
                if (bin == '1')
                {
                    lastSignal = lastSignal switch
                    {
                        '-' => '+',
                        '+' => '-'
                    };
                }

                //Adiciona o dado codificado ao fim da string
                encodedData.Append(lastSignal);
            }
        }

        return encodedData.ToString();
    }

    public string EncodeMdif(string hexInput)
    {
        StringBuilder encodedData = new();
        var lastSignal = '-';
        foreach (char hex in hexInput.ToLower())
        {
            foreach (char bin in _hexCharToBin[hex])
            {
                //Outro jeito de implementar, invertendo o sinal se for 0 e mantendo se for 1 e escrevendo o segundo sinal invertido
                /*if (bin == '0')
                {
                    lastSignal = lastSignal switch
                    {
                        '-' => '+',
                        '+' => '-'
                    };
                }

                encodedData.Append(lastSignal);
                lastSignal = lastSignal switch
                {
                    '-' => '+',
                    '+' => '-'
                };
                encodedData.Append(lastSignal);*/

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


    public string Encode8B6T(string hexInput)
    {
        var _binTo8b6t = IO.ReadDictionary<string, string>("Dados/bin-8b6t.csv");
        StringBuilder encodedData = new();
        var weight = 0;
        hexInput = hexInput.ToLower();
        for (var i = 0; i < hexInput.Length - 1; i += 2)
        {
            string encodedStr = _binTo8b6t[_hexCharToBin[hexInput[i]] + _hexCharToBin[hexInput[i + 1]]];
            if (weight > 0)
            {
                var newEncodedStr = new StringBuilder();
                foreach (char c in encodedStr)
                {
                    newEncodedStr.Append(c switch
                    {
                        '+' => '-',
                        '-' => '+',
                        _ => c
                    });
                }
            }
            else
            {
                foreach (char c in encodedStr)
                {
                    weight = c switch
                    {
                        '+' => weight + 1,
                        '-' => weight - 1
                    };
                }
            }

            encodedData.Append();
        }

        return encodedData.ToString();
    }

    public string Encode6B8B(string hexInput)
    {
        return "";
    }

    public string EncodeHdb3(string hexInput)
    {
        return "";
    }
}