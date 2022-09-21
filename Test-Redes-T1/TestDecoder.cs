using Decodificador;

namespace Test_Redes_T1;

public class TestDecoder
{
    private Decoder _decoder;

    private static readonly TestResult[] ExpectedNrzi =
    {
        new("-++--+---+-+----", "5678"),
        new("++--++-+-+++-++-+-++-+-+", "ABCDEF"),
        new("--+++----+--++++--++-+++", "2468AC"),
        new("---+++-++--++-+-+++-++-+", "13579B"),
        new("-------+++----+--++++--++-+++-+-++++---+--++--+-", "0123456789AB"),
        new("++--++--++-+--+-+---+---+--+-++-+-++-+--+-+-+-+-", "AABBCCDDEEFF"),
    };

    private static readonly TestResult[] ExpectedMdif =
    {
        new("+--+-++-+--++-+-+--++--++-+-+-+-", "5678"),
        new("-+-++-+--+-++--++--+-+-++--+-++--++--+-++--++--+", "ABCDEF"),
        new("+-+--+-+-++-+-+-+--++-+--+-+-+-++-+--+-++--+-+-+", "2468AC"),
        new("+-+-+--+-+-++--+-++-+--+-++--++--+-+-++--+-++--+", "13579B"),
        new("+-+-+-+-+-+-+--+-+-++-+-+-+--++-+--+-+-+-++-+--+-++--+-+-++--++--+-+-+-++-+-+--++-+--+-++-+--++-",
            "0123456789AB"),
        new("-+-++-+--+-++-+--+-++--++-+--++--++-+-+--++-+-+--++-+--++--+-++--++--+-++--++-+--++--++--++--++-",
            "AABBCCDDEEFF"),
    };

    private static readonly TestResult[] Expected8B6T =
    {
        new("--++0+---++0", "5678"),
        new("+-+--++0-+00--++0-", "ABCDEF"),
        new("+-0+000--+-+-++--+", "2468AC"),
        new("-0-++0--+++0-+-00+", "13579B"),
        new("0-+-+0+-0-++00+0--++000-0-0+00+-+--+", "0123456789AB"),
        new("+-+-+-0+0--+-+000++0--00++-0+-00-+0-", "AABBCCDDEEFF"),
    };

    private static readonly TestResult[] Expected6B8B =
    {
        new("+++--++---+++-+-+-++++--+++-++--", "56789A"),
        new("++--++----+-+----++--+---+--+---", "ABCDEF"),
        new("+---+++-+----+--+-++++--++--+---", "2468AC"),
        new("-+---++---+--++----+-+-----+--+-", "13579B"),
        new("-++-+++-+--+++--++++-++-+----++-+++-+++-++-+----++---+----++--+-", "0123456789AB"),
        new("++--++----++--+--+--+---+---+----++--+-----+-+---++-++---+---+--", "AABBCCDDEEFF"),
    };

    private static readonly TestResult[] ExpectedHdb3 =
    {
        new("0+0-0+-00+-+-000", "5678"),
        new("+0-0+0-+-+00-+0-+-+0-+-+", "ABCDEF"),
        new("00+00-000+-0+000-0+0-+00", "2468AC"),
        new("000+00-+0-0+0-+-+00-+0-+", "13579B"),
        new("+00+000-00+000-+0-000+0-0+-00+-+-000+00-+0-0+0-+", "0123456789AB"),
        new("+0-0+0-0+0-+-0+-+-00+-00+-0+-+0-+-+0-+-0+-+-+-+-", "AABBCCDDEEFF"),
    };

    [OneTimeSetUp]
    public void SetUp()
    {
        _decoder = new Decoder();
    }

    [Test]
    public void TestDecoderNrzi()
    {
        foreach (TestResult testExpected in ExpectedNrzi)
        {
            string testOutput = _decoder.DecodeNrzi(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For nrzi with input {testExpected.Input}:");
        }
    }

    [Test]
    public void TestDecoderMdif()
    {
        foreach (TestResult testExpected in ExpectedMdif)
        {
            string testOutput = _decoder.DecodeMdif(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For mdif with input {testExpected.Input}:");
        }
    }

    [Test]
    public void TestDecoder8B6T()
    {
        foreach (TestResult testExpected in Expected8B6T)
        {
            string testOutput = _decoder.Decode8B6T(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For 8b6t with input {testExpected.Input}:");
        }
    }

    [Test]
    public void TestDecoder6B8B()
    {
        foreach (TestResult testExpected in Expected6B8B)
        {
            string testOutput = _decoder.Decode6B8B(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For 6b8b with input {testExpected.Input}:");
        }
    }

    [Test]
    public void TestDecoderHdb3()
    {
        foreach (TestResult testExpected in ExpectedHdb3)
        {
            string testOutput = _decoder.DecodeHdb3(testExpected.Input);
            Assert.That(testOutput, Is.EqualTo(testExpected.Output),
                () => $"For Hdb3 with input {testExpected.Input}:");
        }
    }
}