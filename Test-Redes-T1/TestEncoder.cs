using Codificador;

namespace Test_Redes_T1;

public class TestEncoder
{
    private static readonly TestResult[] ExpectedNrzi =
    {
        new("5678", "-++--+---+-+----"),
        new("ABCDEF", "++--++-+-+++-++-+-++-+-+"),
        new("2468AC", "--+++----+--++++--++-+++"),
        new("13579B", "---+++-++--++-+-+++-++-+"),
        new("0123456789AB", "-------+++----+--++++--++-+++-+-++++---+--++--+-"),
        new("AABBCCDDEEFF", "++--++--++-+--+-+---+---+--+-++-+-++-+--+-+-+-+-"),
    };

    private static readonly TestResult[] ExpectedMdif =
    {
        new("5678", "+--+-++-+--++-+-+--++--++-+-+-+-"),
        new("ABCDEF", "-+-++-+--+-++--++--+-+-++--+-++--++--+-++--++--+"),
        new("2468AC", "+-+--+-+-++-+-+-+--++-+--+-+-+-++-+--+-++--+-+-+"),
        new("13579B", "+-+-+--+-+-++--+-++-+--+-++--++--+-+-++--+-++--+"),
        new("0123456789AB",
            "+-+-+-+-+-+-+--+-+-++-+-+-+--++-+--+-+-+-++-+--+-++--+-+-++--++--+-+-+-++-+-+--++-+--+-++-+--++-"),
        new("AABBCCDDEEFF",
            "-+-++-+--+-++-+--+-++--++-+--++--++-+-+--++-+-+--++-+--++--+-++--++--+-++--++-+--++--++--++--++-"),
    };

    private static readonly TestResult[] Expected8B6T =
    {
        new("5678", "--++0+---++0"),
        new("ABCDEF", "+-+--++0-+00--++0-"),
        new("2468AC", "+-0+000--+-+-++--+"),
        new("13579B", "-0-++0--+++0-+-00+"),
        new("0123456789AB", "0-+-+0+-0-++00+0--++000-0-0+00+-+--+"),
        new("AABBCCDDEEFF", "+-+-+-0+0--+-+000++0--00++-0+-00-+0-"),
    };

    private static readonly TestResult[] Expected6B8B =
    {
        new("5678", "+++--++---+++-+-+-++++--+++-++--"),
        new("ABCDEF", "++--++----+-+----++--+---+--+---"),
        new("2468AC", "+---+++-+----+--+-++++--++--+---"),
        new("13579B", "-+---++---+--++----+-+-----+--+-"),
        new("0123456789AB", "-++-+++-+--+++--++++-++-+----++-+++-+++-++-+----++---+----++--+-"),
        new("AABBCCDDEEFF", "++--++----++--+--+--+---+---+----++--+-----+-+---++-++---+---+--"),
    };

    private static readonly TestResult[] ExpectedHdb3 =
    {
        new("5678", "0+0-0+-00+-+-000"),
        new("ABCDEF", "+0-0+0-+-+00-+0-+-+0-+-+"),
        new("2468AC", "00+00-000+-0+000-0+0-+00"),
        new("13579B", "000+00-+0-0+0-+-+00-+0-+"),
        new("0123456789AB", "+00+000-00+000-+0-000+0-0+-00+-+-000+00-+0-0+0-+"),
        new("AABBCCDDEEFF", "+0-0+0-0+0-+-0+-+-00+-00+-0+-+0-+-+0-+-0+-+-+-+-"),
    };

    [Test]
    public void TestEncoderNrzi()
    {
        foreach (TestResult testExpected in ExpectedNrzi)
        {
            string testOutput = Encoder.EncodeNrzi(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For nrzi with input {testExpected.Input}:");
        }
    }

    [Test]
    public void TestEncoderMdif()
    {
        foreach (TestResult testExpected in ExpectedMdif)
        {
            string testOutput = Encoder.EncodeMdif(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For mdif with input {testExpected.Input}:");
        }
    }

    [Test]
    public void TestEncoder8B6T()
    {
        foreach (TestResult testExpected in Expected8B6T)
        {
            string testOutput = Encoder.Encode8B6T(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For 8b6t with input {testExpected.Input}:");
        }
    }

    [Test]
    public void TestEncoder6B8B()
    {
        foreach (TestResult testExpected in Expected6B8B)
        {
            string testOutput = Encoder.Encode6B8B(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For 6b8b with input {testExpected.Input}:");
        }
    }

    [Test]
    public void TestEncoderHdb3()
    {
        foreach (TestResult testExpected in ExpectedHdb3)
        {
            string testOutput = Encoder.EncodeHdb3(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For Hdb3 with input {testExpected.Input}:");
        }
    }
}