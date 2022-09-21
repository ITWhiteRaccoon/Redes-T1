using System.Text;
using Util;

namespace Decodificador;

public class Decoder
{
    private readonly Dictionary<string, char> _binToHexChar;

    public Decoder()
    {
        _binToHexChar = IO.ReadDictionary<string, char>("Dados/hex-bin.csv", 1, 0);
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
            string signalInput = args[1].ToLower();
            Decoder decoder = new();
            Console.WriteLine(args[0].ToLower() switch
            {
                "nrzi" => decoder.DecodeNrzi(signalInput),
                "mdif" => decoder.DecodeMdif(signalInput),
                "hdb3" => decoder.DecodeHdb3(signalInput),
                "8b6t" => decoder.Decode8B6T(signalInput),
                "6b8b" => decoder.Decode6B8B(signalInput),
                _ => "erro"
            });
        }
        catch (Exception)
        {
            Console.WriteLine("erro");
        }
    }

    public string BinToHex(string bin)
    {
        //Transforma o binário em hexadecimal, adicionando zeros à esquerda para completar os 4 bits
        StringBuilder hex = new();
        bin = bin.PadLeft((int)Math.Ceiling((double)(bin.Length / 4)), '0');
        for (var i = 0; i < bin.Length; i += 4)
        {
            hex.Append(_binToHexChar[bin[i..(i + 4)]]);
        }

        return hex.ToString();
    }

    public string DecodeNrzi(string signalInput)
    {
        StringBuilder decodedDataBin = new();
        var lastSignal = '-';
        foreach (char c in signalInput)
        {
            //Para cada sinal lido, se diferente do anterior quer dizer que o bit é 1, se igual então 0
            int digit = c != lastSignal ? 1 : 0;
            //Guarda o sinal lido como último usado
            lastSignal = c;
            //Adiciona o bit ao final dos dados decodificados
            decodedDataBin.Append(digit);
        }

        //O dado foi lido bit a bit. Para retornar, transforma na representação em hexa
        return BinToHex(decodedDataBin.ToString()).ToUpper();
    }

    public string DecodeMdif(string signalInput)
    {
        StringBuilder decodedDataBin = new();
        var lastSignal = '-';
        for (var i = 0; i < signalInput.Length; i += 2)
        {
            //Para cada sinal lido, se for diferente do anterior, quer dizer que o bit é 0. O segundo sinal é ignorado,
            //pois representa a transição de onde que serve apenas para sincronização.
            int digit = signalInput[i] != lastSignal ? 0 : 1;
            lastSignal = signalInput[i + 1];

            decodedDataBin.Append(digit);
        }

        return BinToHex(decodedDataBin.ToString()).ToUpper();
    }

    public string Decode8B6T(string signalInput)
    {
        var binFrom8B6T = IO.ReadDictionary<string, string>("Dados/bin-8b6t.csv", 1, 0);

        StringBuilder decodedDataBin = new();
        var weight = 0;

        for (var i = 0; i < signalInput.Length; i += 6)
        {
            string currSignal = signalInput[i..(i + 6)];

            var newWeight = 0;
            foreach (char c in currSignal)
            {
                newWeight = c switch
                {
                    '+' => newWeight + 1,
                    '-' => newWeight - 1,
                    _ => newWeight
                };
            }

            if (weight == 1 && newWeight == -1)
            {
                StringBuilder invertedStr = new();
                foreach (var c in currSignal)
                {
                    invertedStr.Append(c switch
                    {
                        '+' => '-',
                        '-' => '+',
                        _ => c
                    });
                }

                currSignal = invertedStr.ToString();
                newWeight = 0;
            }

            weight = newWeight;

            decodedDataBin.Append(binFrom8B6T[currSignal]);
        }

        return BinToHex(decodedDataBin.ToString()).ToUpper();
    }

    public string Decode6B8B(string signalInput)
    {
        return "";
    }

    public string DecodeHdb3(string signalInput)
    {
        return "";
    }
}