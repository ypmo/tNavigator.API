using System;

namespace tNav.Common.Tests;

public class StreamFactoryTests
{
    [Fact]
    public void StreamFactoryCanCreateStream()
    {
        var stream = StreamFactory.ASCII2ByteStream("e4");
        var buffer = new byte[2];
        var data = stream.Read(buffer);
        Assert.Equal(1, data);
        Assert.Equal(228, buffer[0]);
    }
}
