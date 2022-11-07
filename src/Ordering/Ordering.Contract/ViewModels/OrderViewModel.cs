namespace Ordering.Contract.ViewModels;

public class OrderViewModel
{
    public Guid Id { get; }
    public string Status { get; }

    public OrderViewModel(Guid id, string status)
    {
        Id = id;
        Status = status;
    }
}