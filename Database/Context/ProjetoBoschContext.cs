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
        public virtual DbSet<BeneficioEvento> BeneficioEvento { get; set; }
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
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Beneficio>(entity =>
            {
                entity.ToTable("beneficio");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("descricao");

                entity.Property(e => e.DescricaoNormalizada)
                    .HasMaxLength(255)
                    .HasColumnName("descricaoNormalizada")
                    .HasComputedColumnSql("(CONVERT([nvarchar](255),upper(rtrim(ltrim([descricao])))))", false);
            });

            modelBuilder.Entity<BeneficioEvento>(entity =>
            {
                entity.HasKey(e => new { e.BeneficioId, e.EventoId })
                    .HasName("pk_beneficioEvento_id");

                entity.ToTable("beneficioEvento");

                entity.Property(e => e.BeneficioId).HasColumnName("beneficioID");

                entity.Property(e => e.EventoId).HasColumnName("eventoID");

                entity.HasOne(d => d.Beneficio)
                    .WithMany(p => p.BeneficioEvento)
                    .HasForeignKey(d => d.BeneficioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_beneficioEvento_beneficioID");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.BeneficioEvento)
                    .HasForeignKey(d => d.EventoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_fk_beneficioEvento_beneficioID_eventoID");
            });

            modelBuilder.Entity<Colaborador>(entity =>
            {
                entity.ToTable("colaborador");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("cpf");

                entity.Property(e => e.DataDeCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataNascimento)
                    .HasColumnType("date")
                    .HasColumnName("dataNascimento");

                entity.Property(e => e.Edv)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EDV");

                entity.Property(e => e.NomeCompleto)
                    .HasMaxLength(255)
                    .HasColumnName("nomeCompleto");

                entity.Property(e => e.OrigemId).HasColumnName("origemID");

                entity.Property(e => e.Senha)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UnidadeOrganizacionalId).HasColumnName("unidadeOrganizacionalID");

                entity.HasOne(d => d.Origem)
                    .WithMany(p => p.InverseOrigem)
                    .HasForeignKey(d => d.OrigemId)
                    .HasConstraintName("fk_colaborador_origemID");

                entity.HasOne(d => d.UnidadeOrganizacional)
                    .WithMany(p => p.Colaborador)
                    .HasForeignKey(d => d.UnidadeOrganizacionalId)
                    .HasConstraintName("fk_colaborador_unidadeOrganizacionalID");
            });

            modelBuilder.Entity<Direito>(entity =>
            {
                entity.ToTable("direito");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BeneficioId).HasColumnName("beneficioID");

                entity.Property(e => e.ColaboradorId).HasColumnName("colaboradorID");

                entity.Property(e => e.DataRetirada)
                    .HasColumnType("datetime")
                    .HasColumnName("dataRetirada");

                entity.Property(e => e.EventoId).HasColumnName("eventoID");

                entity.Property(e => e.IndicadoId).HasColumnName("indicadoID");

                entity.Property(e => e.RetiradoId).HasColumnName("retiradoID");

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
                entity.ToTable("evento");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataFim)
                    .HasColumnType("datetime")
                    .HasColumnName("dataFim");

                entity.Property(e => e.DataInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("dataInicio");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(255)
                    .HasColumnName("descricao");

                entity.Property(e => e.DescricaoNormalizada)
                    .HasMaxLength(255)
                    .HasColumnName("descricaoNormalizada")
                    .HasComputedColumnSql("(CONVERT([nvarchar](255),upper(rtrim(ltrim([descricao])))))", false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nome");
            });

            modelBuilder.Entity<Permissao>(entity =>
            {
                entity.HasKey(e => new { e.ColaboradorId, e.TipoPermissaoId })
                    .HasName("pk_permissao_colaboradorID_tipoPermissaoID");

                entity.ToTable("permissao");

                entity.Property(e => e.ColaboradorId).HasColumnName("colaboradorID");

                entity.Property(e => e.TipoPermissaoId).HasColumnName("tipoPermissaoID");

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
                entity.ToTable("tipoPermissao");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("descricao");

                entity.Property(e => e.DescricaoNormalizada)
                    .HasMaxLength(255)
                    .HasColumnName("descricaoNormalizada")
                    .HasComputedColumnSql("(CONVERT([nvarchar](255),upper(rtrim(ltrim([descricao])))))", false);
            });

            modelBuilder.Entity<UnidadeOrganizacional>(entity =>
            {
                entity.ToTable("unidadeOrganizacional");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("descricao");

                entity.Property(e => e.DescricaoNormalizada)
                    .HasMaxLength(255)
                    .HasColumnName("descricaoNormalizada")
                    .HasComputedColumnSql("(CONVERT([nvarchar](255),upper(rtrim(ltrim([descricao])))))", false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
