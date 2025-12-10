namespace Core.Entities;

public partial class CarImage
{
    public int Car { get; set; }

    public string Path { get; set; } = null!;

    public virtual Car CarNavigation { get; set; } = null!;
}
