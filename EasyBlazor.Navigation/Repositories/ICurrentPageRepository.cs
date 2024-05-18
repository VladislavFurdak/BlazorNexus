namespace EasyBlazor.Navigation.Repositories;

public interface ICurrentPageRepository<T> where T : Enum
{ 
    T CurrentPage { get; }
    void SetCurrentPage(T pageKey);
}