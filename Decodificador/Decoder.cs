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
        bin = bin.PadLeft(4 * (int)Math.Ceiling((double)bin.Length / 4), '0');
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
        //Lemos a tabela de 8B6T para conversão de binário para sinais e construímos um dicionário invertido, usando o sinal como chave
        var binFrom8B6T = IO.ReadDictionary<string, string>("Dados/bin-8b6t.csv", 1, 0);

        StringBuilder decodedData = new();

        for (var i = 0; i < signalInput.Length; i += 6)
        {
            string currSignal = signalInput[i..(i + 6)];

            //Para cada 6 sinais, calculamos se há desbalanço.
            var weight = 0;
            foreach (char c in currSignal)
            {
                weight += c switch
                {
                    '+' => 1,
                    '-' => -1,
                    _ => 0
                };
            }

            //Caso algum sinal esteja com desbalanço negativo, significa que ele foi invertido para balancear os níveis.
            //Neste caso invertemos novamente para voltar ao sinal original.
            if (weight == -1)
            {
                StringBuilder invertedStr = new();
                foreach (char c in currSignal)
                {
                    invertedStr.Append(Invert.Signal(c));
                }

                currSignal = invertedStr.ToString();
            }

            //Já tendo o sinal na forma correta, procuramos na tabela de conversão o binário correspondente
            decodedData.Append(binFrom8B6T[currSignal]);
        }

        return BinToHex(decodedData.ToString()).ToUpper();
    }

    public string Decode6B8B(string signalInput)
    {
        return "";
    }

    public string DecodeHdb3(string signalInput)
    {
        var tamString = bitsInput.Length
        var listBits = new char[tamString];
        var lastSignal = '-';
        var cont = 0;
        
        // Criamos uma lista dos bits de entrada para que houvesse a possibilidade de mudar o valor das posições
        // usando o valor do ultimo pulso modificamos de acordo com o metodo AMI e também de acordo com o número de 0's
        // a cada 4 0's existe uma violação, sendo assim mudando o valor dele para demonstrar tal
        // para a decodificação é a mesma coisa, pois faz ao contrario já
        for (var i = 0; i > tamString; i++)
        {
            if (bit == "1")
            {
                lastSignal = Invert.Signal(lastSignal);
                listBits[i] = lastSignal;
                cont = 0;
            } else {
                if (cont == 3){
                    listBits[i] = lastSignal;
                    cont++;
                } else if (cont == 6) {
                    listBits[i-6] = Invert.Signal(lastSignal);
                    listBits[i-7] = Invert.Signal(lastSignal);
                    listBits[i-4] = lastSignal;
                    listBits[i] = lastSignal;
                    cont = 0;
                }
                encodedData.Append = bit;
                cont++;
            }
        }

        return encodedData.ToString();
    }
}