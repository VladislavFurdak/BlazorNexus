using BlazorNexsus.Navigation.Utils;
using FluentAssertions;

namespace UnitTests.Tests;

public class UriUtilsTests
{
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