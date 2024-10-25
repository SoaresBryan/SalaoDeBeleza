using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Construtor sem parâmetros para suporte a migrações
    public AppDbContext() { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Profissional> Profissionais { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SeuNomeDoBanco;Trusted_Connection=True;");
        }
    }
}



public class Cliente
{
    public int ClienteId { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
}

public class Profissional
{
    public int ProfissionalId { get; set; }
    public string Nome { get; set; }
    public string Especialidade { get; set; }
}

public class Servico
{
    public int ServicoId { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
}

public class Agendamento
{
    public int AgendamentoId { get; set; }
    public int ClienteId { get; set; }
    public int ProfissionalId { get; set; }
    public int ServicoId { get; set; }
    public DateTime DataAgendamento { get; set; }

    public Cliente Cliente { get; set; }
    public Profissional Profissional { get; set; }
    public Servico Servico { get; set; }
}
