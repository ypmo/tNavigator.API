using System;
using System.Text;

namespace tNav.Common.Tests;

public class EncoderTests
{
    private readonly Custom256ByteEncoding _encoding = new();
   // Helper method to assume the simple 1:1 mapping for testing purposes
    // (You will need to adjust this based on your *actual* map)
    private char GetExpectedCharForByte(byte b)
    {
        // If your map is simple 1:1 for the first 256 Unicode chars:
        return (char)b;
        // If your map is complex, you must hardcode the expected mapping here
    }

    [Fact]
    public void GetBytes_MapsAll256CharsToCorrectBytes()
    {
        for (int i = 0; i < 256; i++)
        {
            char expectedChar = GetExpectedCharForByte((byte)i);
            string inputString = expectedChar.ToString();
            
            byte[] bytes = _encoding.GetBytes(inputString);

            Assert.Single(bytes);
            Assert.Equal((byte)i, bytes[0]);
        }
    }

    [Fact]
    public void GetChars_MapsAll256BytesToCorrectChars()
    {
        for (int i = 0; i < 256; i++)
        {
            byte[] inputBytes = new byte[] { (byte)i };
            
            char[] chars = _encoding.GetChars(inputBytes);

            Assert.Single(chars);
            Assert.Equal(GetExpectedCharForByte((byte)i), chars[0]);
        }
    }

    [Fact]
    public void GetString_ConvertsByteArrayToStringCorrectly()
    {
        byte[] inputBytes = new byte[] { 65, 66, 67, 255 }; // A, B, C, and the last byte
        // Assuming byte 255 maps to char '\u00ff' (if 1:1 map)
        string expectedString = "ABC" + GetExpectedCharForByte(255); 

        string resultString = _encoding.GetString(inputBytes);

        Assert.Equal(expectedString, resultString);
    }

    [Fact]
    public void GetBytes_ConvertsStringToByteArrayCorrectly()
    {
        string inputString = "Hello";
        // Assuming 'H','e','l','l','o' map to 72, 101, 108, 108, 111 (ASCII/1:1 map)
        byte[] expectedBytes = new byte[] { 72, 101, 108, 108, 111 };

        byte[] resultBytes = _encoding.GetBytes(inputString);

        Assert.Equal(expectedBytes, resultBytes);
    }
    
    [Fact]
    public void UnsupportedChar_UsesFallbackBehavior()
    {
        // Test behavior for a character outside the 256-char range (e.g., '€' U+20AC)
        string input = "Test€";

        // The default fallback in C# usually throws an exception or uses '?'
        // You should configure and test the specific fallback you intend to use.

        // Example using default Exception fallback:
        var bytes = _encoding.GetBytes(input);
        Assert.Throws<EncoderFallbackException>(() => _encoding.GetBytes(input));
    }
}
