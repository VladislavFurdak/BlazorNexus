using Microsoft.AspNetCore.Components;

namespace UnitTests.Dummy;
 
public class NavigationManagerMock : NavigationManager
{
    public Action<string> ResultUri = (string uri) => { };

    public NavigationManagerMock(string baseUri)
    { 
        Initialize(baseUri, baseUri);
    }
    
    protected override void NavigateToCore(string uri, NavigationOptions options)
    {
        Uri = BaseUri.TrimEnd('/') + uri;
        ResultUri(uri);
    }
}