namespace tNav.Common.Tests;

public class StringDecoderTests
{

    private readonly One2OneEncoding _encoding = new();
    [Fact]
    public void UnsupportedChar_UsesFallbackBehavior()
    {
          var decoded ="Test[]".OneToOneToUTF8();
          Assert.Equal("Test[]",decoded);
    }
}
