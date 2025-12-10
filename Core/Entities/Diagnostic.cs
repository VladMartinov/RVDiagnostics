namespace Core.Entities;

public partial class Diagnostic
{
    public int Id { get; set; }

    public string Port { get; set; } = null!;

    public string Protocol { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual Port PortNavigation { get; set; } = null!;
}
