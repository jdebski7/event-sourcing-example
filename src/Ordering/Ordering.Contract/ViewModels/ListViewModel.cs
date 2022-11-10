namespace Ordering.Contract.ViewModels;

public class ListViewModel<T> where T : class
{
    public IEnumerable<T> Items { get; }

    public ListViewModel(IEnumerable<T> items)
    {
        Items = items;
    }
}