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
            Encoder encoder = new();
            Console.WriteLine(args[0].ToLower() switch
            {
                "nrzi" => encoder.EncodeNrzi(hexInput),
                "mdif" => encoder.EncodeMdif(hexInput),
                "hdb3" => encoder.EncodeHdb3(hexInput),
                "8b6t" => encoder.Encode8B6T(hexInput),
                "6b8b" => encoder.Encode6B8B(hexInput),
                _ => "erro"
            });
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
            //Lemos cada caractere de entrada como hexa e transformamos na representação em binário
            foreach (char bin in _hexCharToBin[hex])
            {
                //Para cada bit da representação em binário, se for 1 invertemos o sinal atual
                if (bin == '1')
                {
                    lastSignal = Invert.Signal(lastSignal);
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
                //Para cada bit da representação em binário, se for 0 invertemos o sinal, se for 1 mantemos o atual.
                //A transição de onda é considerada, mas mapeada diretamente. Desta maneira o código fica mais simples.
                encodedData.Append(bin switch
                {
                    '0' when lastSignal == '-' => "+-",
                    '0' when lastSignal == '+' => "-+",
                    '1' when lastSignal == '-' => "-+",
                    '1' when lastSignal == '+' => "+-"
                });

                lastSignal = encodedData[^1];

                //Outro jeito de implementar, invertemos o sinal se for 0 e mantemos se for 1. Então escrevemos o
                //segundo sinal invertido representando a transição da onda.
                /*if (bin == '0')
                {
                    lastSignal = Invert.Signal(lastSignal);
                }

                encodedData.Append(lastSignal);

                lastSignal = Invert.Signal(lastSignal);
                encodedData.Append(lastSignal);*/
            }
        }

        return encodedData.ToString();
    }


    public string Encode8B6T(string hexInput)
    {
        //Lemos a tabela de 8B6T para conversão de bits em sinais e transformamos em um dicionário.
        var binTo8B6T = IO.ReadDictionary<string, string>("Dados/bin-8b6t.csv");
        hexInput = hexInput.ToLower();

        StringBuilder encodedData = new();
        var weight = 0;

        //A cada dois caracteres hexa, transformamos em 8 bits e consultamos o sinal correspondente no dicionário
        for (var i = 0; i < hexInput.Length - 1; i += 2)
        {
            string encodedStr = binTo8B6T[_hexCharToBin[hexInput[i]] + _hexCharToBin[hexInput[i + 1]]];

            //8b6t tem somente desbalanços positivos. Se ao somar os sinais obtivermos 1, significa que existe desbalanço.
            foreach (char c in encodedStr)
            {
                weight += c switch
                {
                    '+' => 1,
                    '-' => -1,
                    _ => 0
                };
            }

            //Se o peso for 1, significa que os 6 sinais atuais estão em desbalanço. Ao encontrarmos mais 6 sinais em 
            //desbalanço, o peso será 2, sinalizando que podemos inverter estes 6 sinais para balancear o DC novamente.
            if (weight == 2)
            {
                StringBuilder invertedStr = new();
                //Invertemos cada sinal e ao fim zeramos o peso.
                foreach (char c in encodedStr)
                {
                    invertedStr.Append(Invert.Signal(c));
                }

                encodedStr = invertedStr.ToString();
                weight = 0;
            }

            encodedData.Append(encodedStr);
        }

        return encodedData.ToString();
    }

    public string Encode6B8B(string hexInput)
    {
        StringBuilder encodedData = new();
        StringBuilder binData = new();
        foreach (char c in hexInput.ToLower())
        {
            binData.Append(_hexCharToBin[c]);
        }

        string bin = binData.ToString().PadLeft(6 * (int)Math.Ceiling((double)binData.Length / 6), '0');

        for (var i = 0; i < bin.Length; i += 6)
        {
            string currBits = bin[i..(i + 6)];

            var disparity = 0;
            foreach (char c in currBits)
            {
                disparity += c switch
                {
                    '0' => -1,
                    '1' => 1,
                    _ => 0
                };
            }

            encodedData.Append(disparity switch
            {
                0 => "10" + currBits,
                2 when currBits != "001111" => "00" + currBits,
                -2 when currBits != "110000" => "11" + currBits,
                2 or -2 => "01" + Invert.BitsWithMask(currBits, 0b000100),
                6 or -6 => "01" + Invert.BitsWithMask(currBits, 0b011001),
                4 or -4 => currBits.IndexOf('0') switch
                {
                    0 or 1 => "01" + Invert.BitsWithMask(currBits, 0b000011),
                    2 or 3 => "01" + Invert.BitsWithMask(currBits, 0b100001),
                    4 or 5 => "01" + Invert.BitsWithMask(currBits, 0b110000)
                }
            });
        }

        return encodedData.ToString();
    }

    public string EncodeHdb3(string bitsInput)
    {
        StringBuilder encodedData = new();
        var lastBit = '';
        
        foreach (char bit in bitsInput)
        {
            lastBit == "1"
            if (lastBit == "1")
            {
                
            } else if (lastBit == "1")
        }

        return encodedData.ToString();
    }
}