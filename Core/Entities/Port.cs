namespace Core.Entities;

public partial class Port
{
    public string Name { get; set; } = null!;

    public string? ImagePath { get; set; }

    public virtual ICollection<Diagnostic> Diagnostics { get; set; } = new List<Diagnostic>();
}
