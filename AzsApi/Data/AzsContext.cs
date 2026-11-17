using AzsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AzsApi.Data;

public class AzsContext : DbContext
{
    public AzsContext(DbContextOptions<AzsContext> options) : base(options)
    {
    }

    public DbSet<Firma> Firmy { get; set; }
    public DbSet<Azs> AzsStancii { get; set; }
    public DbSet<Toplivo> Topliva { get; set; }
    public DbSet<FirmaToplivo> FirmyToplivo { get; set; }
    public DbSet<Klient> Klienty { get; set; }
    public DbSet<Prodazha> Prodazhi { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Firma>(entity =>
        {
            entity.ToTable("firma");
            entity.HasKey(e => e.NazvanieFirmy);
            entity.Property(e => e.NazvanieFirmy).HasColumnName("nazvanie_firmy").HasMaxLength(100);
            entity.Property(e => e.YurAdres).HasColumnName("yur_adres").HasMaxLength(200);
            entity.Property(e => e.Telefon).HasColumnName("telefon").HasMaxLength(30);
        });

        modelBuilder.Entity<Azs>(entity =>
        {
            entity.ToTable("azs");
            entity.HasKey(e => e.KodAzs);
            entity.Property(e => e.KodAzs).HasColumnName("kod_azs");
            entity.Property(e => e.NazvanieFirmy).HasColumnName("nazvanie_firmy").HasMaxLength(100);
            entity.Property(e => e.AdresAzs).HasColumnName("adres_azs").HasMaxLength(200);

            entity.HasOne(e => e.Firma)
                .WithMany(f => f.AzsList)
                .HasForeignKey(e => e.NazvanieFirmy)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Toplivo>(entity =>
        {
            entity.ToTable("toplivo");
            entity.HasKey(e => e.KodTopliva);
            entity.Property(e => e.KodTopliva).HasColumnName("kod_topliva");
            entity.Property(e => e.VidTopliva).HasColumnName("vid_topliva").HasMaxLength(50);
            entity.Property(e => e.EdinicaIzmereniya).HasColumnName("edinica_izmereniya").HasMaxLength(20);
            entity.Property(e => e.CenaRub).HasColumnName("cena_rub").HasPrecision(10, 2);
        });

        modelBuilder.Entity<FirmaToplivo>(entity =>
        {
            entity.ToTable("firma_toplivo");
            entity.HasKey(e => new { e.NazvanieFirmy, e.KodTopliva });
            entity.Property(e => e.NazvanieFirmy).HasColumnName("nazvanie_firmy").HasMaxLength(100);
            entity.Property(e => e.KodTopliva).HasColumnName("kod_topliva");

            entity.HasOne(e => e.Firma)
                .WithMany(f => f.FirmyToplivo)
                .HasForeignKey(e => e.NazvanieFirmy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Toplivo)
                .WithMany(t => t.FirmyToplivo)
                .HasForeignKey(e => e.KodTopliva)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Klient>(entity =>
        {
            entity.ToTable("klient");
            entity.HasKey(e => e.KartSchet);
            entity.Property(e => e.KartSchet).HasColumnName("kart_schet").HasMaxLength(30);
            entity.Property(e => e.Fio).HasColumnName("fio").HasMaxLength(150);
            entity.Property(e => e.Adres).HasColumnName("adres").HasMaxLength(200);
            entity.Property(e => e.Telefon).HasColumnName("telefon").HasMaxLength(30);
        });

        modelBuilder.Entity<Prodazha>(entity =>
        {
            entity.ToTable("prodazha");
            entity.HasKey(e => e.KodProdazhi);
            entity.Property(e => e.KodProdazhi).HasColumnName("kod_prodazhi");
            entity.Property(e => e.DataProdazhi).HasColumnName("data_prodazhi");
            entity.Property(e => e.KartSchet).HasColumnName("kart_schet").HasMaxLength(30);
            entity.Property(e => e.KodAzs).HasColumnName("kod_azs");
            entity.Property(e => e.KodTopliva).HasColumnName("kod_topliva");
            entity.Property(e => e.Kolichestvo).HasColumnName("kolichestvo").HasPrecision(10, 2);

            entity.HasOne(e => e.Klient)
                .WithMany(k => k.Prodazhi)
                .HasForeignKey(e => e.KartSchet)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Azs)
                .WithMany(a => a.Prodazhi)
                .HasForeignKey(e => e.KodAzs)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Toplivo)
                .WithMany(t => t.Prodazhi)
                .HasForeignKey(e => e.KodTopliva)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
