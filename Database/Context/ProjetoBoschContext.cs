using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BoschCartaoDigitalBackEnd.Models.v1.ProjetoBoschContext;

#nullable disable

namespace BoschCartaoDigitalBackEnd.Database.Context
{
    public partial class ProjetoBoschContext : DbContext
    {
        public ProjetoBoschContext()
        {
        }

        public ProjetoBoschContext(DbContextOptions<ProjetoBoschContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Beneficio> Beneficio { get; set; }
        public virtual DbSet<Colaborador> Colaborador { get; set; }
        public virtual DbSet<Direito> Direito { get; set; }
        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<Permissao> Permissao { get; set; }
        public virtual DbSet<TipoPermissao> TipoPermissao { get; set; }
        public virtual DbSet<UnidadeOrganizacional> UnidadeOrganizacional { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:ProjetoBoschContext");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Beneficio>(entity =>
            {
                entity.Property(e => e.DescricaNormalizada).HasComputedColumnSql("(CONVERT([nvarchar](255),upper(rtrim(ltrim([descricao])))))", false);
            });

            modelBuilder.Entity<Colaborador>(entity =>
            {
                entity.Property(e => e.Cpf).IsUnicode(false);

                entity.Property(e => e.Edv)
                    .IsUnicode(false)
                    .HasComputedColumnSql("(CONVERT([varchar](10),upper(rtrim(ltrim([id]+(55555))))))", false);

                entity.HasOne(d => d.UnidadeOrganizacional)
                    .WithMany(p => p.Colaborador)
                    .HasForeignKey(d => d.UnidadeOrganizacionalId)
                    .HasConstraintName("fk_colaborador_unidadeOrganizacionalID");
            });

            modelBuilder.Entity<Direito>(entity =>
            {
                entity.HasOne(d => d.Beneficio)
                    .WithMany(p => p.Direito)
                    .HasForeignKey(d => d.BeneficioId)
                    .HasConstraintName("fk_direito_beneficioID");

                entity.HasOne(d => d.Colaborador)
                    .WithMany(p => p.DireitoColaborador)
                    .HasForeignKey(d => d.ColaboradorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_direito_colaboradorID");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.Direito)
                    .HasForeignKey(d => d.EventoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_direito_eventoID");

                entity.HasOne(d => d.Indicado)
                    .WithMany(p => p.DireitoIndicado)
                    .HasForeignKey(d => d.IndicadoId)
                    .HasConstraintName("fk_direito_indicadoID");

                entity.HasOne(d => d.Retirado)
                    .WithMany(p => p.DireitoRetirado)
                    .HasForeignKey(d => d.RetiradoId)
                    .HasConstraintName("fk_direito_retiradoID");
            });

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.Property(e => e.DescricaNormalizada).HasComputedColumnSql("(CONVERT([nvarchar](255),upper(rtrim(ltrim([descricao])))))", false);
            });

            modelBuilder.Entity<Permissao>(entity =>
            {
                entity.HasKey(e => new { e.ColaboradorId, e.TipoPermissaoId })
                    .HasName("pk_permissao_colaboradorID_tipoPermissaoID");

                entity.HasOne(d => d.Colaborador)
                    .WithMany(p => p.Permissao)
                    .HasForeignKey(d => d.ColaboradorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_permissao_colaboradorID");

                entity.HasOne(d => d.TipoPermissao)
                    .WithMany(p => p.Permissao)
                    .HasForeignKey(d => d.TipoPermissaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_permissao_tipoPermissaoID");
            });

            modelBuilder.Entity<TipoPermissao>(entity =>
            {
                entity.Property(e => e.DescricaNormalizada).HasComputedColumnSql("(CONVERT([nvarchar](255),upper(rtrim(ltrim([descricao])))))", false);
            });

            modelBuilder.Entity<UnidadeOrganizacional>(entity =>
            {
                entity.Property(e => e.DescricaNormalizada).HasComputedColumnSql("(CONVERT([nvarchar](255),upper(rtrim(ltrim([descricao])))))", false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
