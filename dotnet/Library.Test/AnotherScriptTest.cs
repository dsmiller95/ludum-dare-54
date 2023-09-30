namespace DotnetLibrary.Test;

public class AnotherScriptTest
{
    [Fact]
    public void TimeShouldIncrease()
    {
        var proxy = 10;
        proxy.Should().Be(10);
    }
}