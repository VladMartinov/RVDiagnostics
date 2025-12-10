namespace Core.Entities;

public partial class Brand
{
    public string Name { get; set; } = null!;

    public string? LogoPath { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
