namespace EasyBlazor.Navigation.Repositories;

public class BackpageRepository<T> : IBackpageRepository<T> where  T : Enum
{
    private T? _backPage;

    public bool IsBackPageSet => _backPage != null;

    public void SetBackPage(T pageKey)
    {
        _backPage = pageKey;
    }

    public T? PopBackPage()
    {
        var backPage = _backPage;
        _backPage = default;
        return backPage;
    }
}