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
                //Para cada bit da representação em binário, se for 0 invertemos o sinal, se for 1 mantemos o atual.
                //A transição de onda é considerada, mas mapeada diretamente. Desta maneira o código fica mais limpo.
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
            }
        }

        return encodedData.ToString();
    }


    public string Encode8B6T(string hexInput)
    {
        //Lemos a tabela de 8B6T para conversão de bits em sinais e transformamos em um dicionário.
        var _binTo8b6t = IO.ReadDictionary<string, string>("Dados/bin-8b6t.csv");
        hexInput = hexInput.ToLower();
        
        var encodedData = new StringBuilder();
        var weight = 0;
        
        //A cada dois caracteres hexa, transformamos em 8 bits e consultamos o sinal correspondente no dicionário
        for (var i = 0; i < hexInput.Length - 1; i += 2)
        {
            string encodedStr = _binTo8b6t[_hexCharToBin[hexInput[i]] + _hexCharToBin[hexInput[i + 1]]];

            //8b6t tem somente desbalanços positivos. Se ao somar os sinais obtivermos 1, significa que existe desbalanço.
            foreach (char c in encodedStr)
            {
                weight = c switch
                {
                    '+' => weight + 1,
                    '-' => weight - 1,
                    _ => weight
                };
            }

            //Se o peso for 1, significa que os 6 sinais atuais estão em desbalanço. Ao encontrarmos mais 6 sinais em 
            //desbalanço, o peso será 2, sinalizando que podemos inverter estes 6 sinais para balancear o DC novamente.
            if (weight > 1)
            {
                var newEncodedStr = new StringBuilder();
                //Invertemos cada sinal e ao fim zeramos o peso.
                foreach (char c in encodedStr)
                {
                    newEncodedStr.Append(c switch
                    {
                        '+' => '-',
                        '-' => '+',
                        _ => c
                    });
                }

                encodedStr = newEncodedStr.ToString();
                weight = 0;
            }

            encodedData.Append(encodedStr);
        }

        return encodedData.ToString();
    }

    public string Encode6B8B(string hexInput)
    {
        return "";
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