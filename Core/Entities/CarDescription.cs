namespace Core.Entities;

public partial class CarDescription
{
    public int Car { get; set; }

    public string Language { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual Car CarNavigation { get; set; } = null!;
}
