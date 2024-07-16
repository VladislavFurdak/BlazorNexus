namespace BlazorNexsus.Navigation.Repositories;

public interface IBackPageRepository<T> where T : struct, Enum
{ 
    Task SetBackPage(T pageKey);
    Task<T?> PopBackPage();
    Task<bool> IsBackPageSet();
}