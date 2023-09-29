namespace DotnetLibrary.Test;

public class AnotherScriptTest
{
    [Fact]
    public void TimeShouldIncrease()
    {
        var proxy = new AnotherScript();
        proxy.Time.Should().Be(0);
        proxy._Process(1);
        proxy.Time.Should().Be(1);
    }
}