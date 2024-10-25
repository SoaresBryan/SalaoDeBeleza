using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SalaoDeBeleza
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BancoDeBeleza;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração das chaves estrangeiras e relacionamentos
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Agendamentos)
                .HasForeignKey(a => a.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Profissional)
                .WithMany(p => p.Agendamentos)
                .HasForeignKey(a => a.IdProfissional)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Servico)
                .WithMany(s => s.Agendamentos)
                .HasForeignKey(a => a.IdServico)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public class Cliente
        {
            public int Id { get; set; } // Id_cliente
            public string Nome { get; set; }
            public string CPF { get; set; }
            public string Telefone { get; set; }
            public string Endereco { get; set; }
            public DateTime DataNascimento { get; set; }

            public ICollection<Agendamento> Agendamentos { get; set; }
        }

        public class Profissional
        {
            public int Id { get; set; } // Id_profissional
            public string Nome { get; set; }
            public string CPF { get; set; }
            public string Telefone { get; set; }
            public string Endereco { get; set; }
            public DateTime DataNascimento { get; set; }
            public string Funcao { get; set; }

            public ICollection<Agendamento> Agendamentos { get; set; }
        }

        public class Servico
        {
            public int Id { get; set; } // Id_servico
            public string NomeServico { get; set; } // Descrição do Serviço
            public decimal PrecoServico { get; set; } // Valor do Serviço

            public ICollection<Agendamento> Agendamentos { get; set; }
        }

        public class Agendamento
        {
            public int Id { get; set; } // Id_agendamento
            public int IdCliente { get; set; } // FK para Cliente
            public int IdProfissional { get; set; } // FK para Profissional
            public int IdServico { get; set; } // FK para Servico
            public DateTime DataAgendamento { get; set; }
            public string Prioridade { get; set; }

            public Cliente Cliente { get; set; }
            public Profissional Profissional { get; set; }
            public Servico Servico { get; set; }
        }
    }
}
