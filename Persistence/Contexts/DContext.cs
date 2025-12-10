using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public partial class DContext : DbContext
{
    public DContext()
    {
    }

    public DContext(DbContextOptions<DContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; } = null!;

    public virtual DbSet<Car> Cars { get; set; } = null!;

    public virtual DbSet<CarDescription> CarDescriptions { get; set; } = null!;

    public virtual DbSet<CarImage> CarImages { get; set; } = null!;

    public virtual DbSet<Diagnostic> Diagnostics { get; set; } = null!;

    public virtual DbSet<DiagnosticTest> DiagnosticTests { get; set; } = null!;

    public virtual DbSet<Engine> Engines { get; set; } = null!;

    public virtual DbSet<Port> Ports { get; set; } = null!;

    public virtual DbSet<Test> Tests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("brands");

            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LogoPath).HasColumnName("logo_path");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.ToTable("cars");

            entity.HasIndex(e => e.Brand, "cars_brand_index");

            entity.HasIndex(e => e.Diagnostic, "cars_diagnostic_index");

            entity.HasIndex(e => e.Engine, "cars_engine_index");

            entity.HasIndex(e => e.Id, "cars_id_uindex").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand).HasColumnName("brand");
            entity.Property(e => e.Diagnostic).HasColumnName("diagnostic");
            entity.Property(e => e.Engine).HasColumnName("engine");
            entity.Property(e => e.ManufactureInterval).HasColumnName("manufacture_interval");
            entity.Property(e => e.Model).HasColumnName("model");

            entity.HasOne(d => d.BrandNavigation).WithMany(p => p.Cars)
                .HasForeignKey(d => d.Brand)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.DiagnosticNavigation).WithMany(p => p.Cars)
                .HasForeignKey(d => d.Diagnostic)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.EngineNavigation).WithMany(p => p.Cars)
                .HasForeignKey(d => d.Engine)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CarDescription>(entity =>
        {
            entity.HasKey(e => new { e.Car, e.Language });

            entity.ToTable("car_descriptions");

            entity.HasIndex(e => new { e.Car, e.Language }, "car_descriptions_car_language_uindex").IsUnique();

            entity.Property(e => e.Car).HasColumnName("car");
            entity.Property(e => e.Language).HasColumnName("language");
            entity.Property(e => e.Description).HasColumnName("description");

            entity.HasOne(d => d.CarNavigation).WithMany(p => p.CarDescriptions).HasForeignKey(d => d.Car);
        });

        modelBuilder.Entity<CarImage>(entity =>
        {
            entity.HasKey(e => new { e.Car, e.Path });

            entity.ToTable("car_images");

            entity.HasIndex(e => new { e.Car, e.Path }, "car_images_car_path_uindex").IsUnique();

            entity.Property(e => e.Car).HasColumnName("car");
            entity.Property(e => e.Path).HasColumnName("path");

            entity.HasOne(d => d.CarNavigation).WithMany(p => p.CarImages).HasForeignKey(d => d.Car);
        });

        modelBuilder.Entity<Diagnostic>(entity =>
        {
            entity.ToTable("diagnostics");

            entity.HasIndex(e => e.Id, "diagnostics_id_uindex").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Port).HasColumnName("port");
            entity.Property(e => e.Protocol).HasColumnName("protocol");

            entity.HasOne(d => d.PortNavigation).WithMany(p => p.Diagnostics)
                .HasForeignKey(d => d.Port)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DiagnosticTest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("diagnostic_tests");

            entity.HasIndex(e => e.Diagnostic, "diagnostic_tests_diagnostic_index");

            entity.HasIndex(e => new { e.Test, e.Diagnostic }, "diagnostic_tests_test_diagnostic_uindex").IsUnique();

            entity.HasIndex(e => e.Test, "diagnostic_tests_test_index");

            entity.Property(e => e.Diagnostic).HasColumnName("diagnostic");
            entity.Property(e => e.Test).HasColumnName("test");

            entity.HasOne(d => d.DiagnosticNavigation).WithMany().HasForeignKey(d => d.Diagnostic);

            entity.HasOne(d => d.TestNavigation).WithMany()
                .HasForeignKey(d => d.Test)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Engine>(entity =>
        {
            entity.ToTable("engines");

            entity.HasIndex(e => e.Ecu, "engines_ecu_uindex").IsUnique();

            entity.HasIndex(e => e.Id, "engines_id_uindex").IsUnique();

            entity.HasIndex(e => e.Name, "engines_name_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConstructionInterval).HasColumnName("construction_interval");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Ecu).HasColumnName("ecu");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Port>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("ports");

            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ImagePath).HasColumnName("image_path");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("tests");

            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
