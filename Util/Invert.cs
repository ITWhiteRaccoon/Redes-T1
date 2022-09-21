namespace Util;

public class Invert
{
    public static string BitsWithMask(string bits, uint bitMask)
    {
        return Convert.ToString(Convert.ToUInt16(bits, 2) ^ bitMask, 2);
    }

    public static char Bit(char bit)
    {
        return bit switch
        {
            '0' => '1',
            '1' => '0',
            _ => bit
        };
    }

    public static char Signal(char signal)
    {
        return signal switch
        {
            '+' => '-',
            '-' => '+',
            _ => signal
        };
    }
}