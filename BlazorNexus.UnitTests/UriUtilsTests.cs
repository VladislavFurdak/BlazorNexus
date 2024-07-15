using BlazorNexsus.Navigation;
using FluentAssertions;

namespace UnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CallReplaceRouteParams()
    {
        var dict = new Dictionary<string, string>()
        {
            {"transporttype", "car"},
            {"subcategory", "evse"},
        };
        
       var result = UriUtils.ReplaceRouteParams(@"/route/{transporttype}/{subcategory}", dict);
       
       result.Should().Be("/route/car/evse");
    }
}