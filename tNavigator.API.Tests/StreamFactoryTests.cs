using System;

namespace tNavigator.API.Tests;

public class StreamFactoryTests
{
    [Fact]
    public void StreamFactoryCanCreateStream()
    {
        var stream = StreamFactory.ASCII2ByteStream("01");
        var buffer = new byte[1];
        var data = stream.Read(buffer);
        Assert.Equal(1, data);
        Assert.Equal(1, buffer[0]);
    }
}
