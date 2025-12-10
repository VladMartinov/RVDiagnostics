namespace Core.Entities;

public partial class DiagnosticTest
{
    public int Diagnostic { get; set; }

    public string Test { get; set; } = null!;

    public virtual Diagnostic DiagnosticNavigation { get; set; } = null!;

    public virtual Test TestNavigation { get; set; } = null!;
}
