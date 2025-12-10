namespace Core.Entities;

public partial class Engine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ConstructionInterval { get; set; }

    public string Ecu { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
