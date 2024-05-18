namespace EasyBlazor.Navigation.Repositories;

public class CurrentPageRepository<T> : ICurrentPageRepository<T> where T : Enum
{
    private T _currentPage;
    
    public T CurrentPage => _currentPage;

    public void SetCurrentPage(T pageKey)
    {
        _currentPage = pageKey;
    }
        
}