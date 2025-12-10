namespace Core.Entities;

public partial class Car
{
    public int Id { get; set; }

    public string Brand { get; set; } = null!;

    public int Engine { get; set; }

    public int Diagnostic { get; set; }

    public string Model { get; set; } = null!;

    public string? ManufactureInterval { get; set; }

    public virtual Brand BrandNavigation { get; set; } = null!;

    public virtual ICollection<CarDescription> CarDescriptions { get; set; } = new List<CarDescription>();

    public virtual ICollection<CarImage> CarImages { get; set; } = new List<CarImage>();

    public virtual Diagnostic DiagnosticNavigation { get; set; } = null!;

    public virtual Engine EngineNavigation { get; set; } = null!;
}
