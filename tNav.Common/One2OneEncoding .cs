using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;

namespace tNav.Common;

public class One2OneEncoding : Encoding
{
    // Constructor, if needed, to set up any internal mappings
    public One2OneEncoding() : base(0) // 0 is a placeholder code page
    {
        // Initialize any character-to-byte or byte-to-character maps
    }

    // Override required abstract methods
    public override int GetByteCount(char[] chars, int index, int count) => count;
    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        for (int i = 0; i < charCount; i++)
        {
            // Apply your custom character-to-byte mapping here
            // Example: A direct cast for basic 256-value mappings
            bytes[byteIndex + i] = (byte)chars[charIndex + i];
        }
        return charCount;
    }
    public override int GetCharCount(byte[] bytes, int index, int count) => count;
    
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        for (int i = 0; i < byteCount; i++)
        {
            // Apply your custom byte-to-character mapping here
            // Example: A direct cast for basic 256-value mappings
            chars[charIndex + i] = (char)bytes[byteIndex + i];
        }
        return byteCount;
    }

    public override int GetMaxByteCount(int charCount) => charCount;
    public override int GetMaxCharCount(int byteCount) => byteCount;

    // Optional: override GetDecoder and GetEncoder if custom stateful decoders/encoders are needed
    // public override Decoder GetDecoder() { /* ... */ }
    // public override Encoder GetEncoder() { /* ... */ }
}
