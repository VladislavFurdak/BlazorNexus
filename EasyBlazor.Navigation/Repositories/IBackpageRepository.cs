namespace EasyBlazor.Navigation.Repositories;

public interface IBackpageRepository<T>  where T : Enum
{
    void SetBackPage(T pageKey);
    T? PopBackPage();

    bool IsBackPageSet { get; }
}