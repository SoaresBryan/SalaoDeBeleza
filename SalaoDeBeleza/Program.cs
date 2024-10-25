using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SalaoDeBeleza
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== Sistema de Gestão de Salão de Beleza ===");
                    Console.WriteLine("1. Cadastrar Profissional");
                    Console.WriteLine("2. Cadastrar Cliente");
                    Console.WriteLine("3. Agendar Serviço");
                    Console.WriteLine("4. Listar Serviços Agendados");
                    Console.WriteLine("0. Sair");
                    Console.Write("Selecione uma opção: ");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            CadastrarProfissional(context);
                            break;
                        case "2":
                            CadastrarCliente(context);
                            break;
                        case "3":
                            AgendarServico(context);
                            break;
                        case "4":
                            ListarServicosAgendados(context);
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar novamente.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

        static void CadastrarProfissional(AppDbContext context)
        {
            Console.WriteLine("\n=== Cadastro de Profissional ===");
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("CPF: ");
            string cpf = Console.ReadLine();
            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();
            Console.Write("Endereço: ");
            string endereco = Console.ReadLine();

            // Validação da data de nascimento com TryParseExact
            Console.Write("Data de Nascimento (yyyy-MM-dd): ");
            DateTime dataNascimento;
            while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dataNascimento))
            {
                Console.WriteLine("Data inválida! Por favor, insira a data no formato yyyy-MM-dd.");
                Console.Write("Data de Nascimento (yyyy-MM-dd): ");
            }

            Console.Write("Função: ");
            string funcao = Console.ReadLine();

            var profissional = new Profissional
            {
                Nome = nome,
                CPF = cpf,
                Telefone = telefone,
                Endereco = endereco,
                DataNascimento = dataNascimento,
                Funcao = funcao
            };

            context.Profissionais.Add(profissional);
            context.SaveChanges();
            Console.WriteLine("Profissional cadastrado com sucesso! Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        static void CadastrarCliente(AppDbContext context)
        {
            Console.WriteLine("\n=== Cadastro de Cliente ===");
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("CPF: ");
            string cpf = Console.ReadLine();
            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();
            Console.Write("Endereço: ");
            string endereco = Console.ReadLine();

            // Validação da data de nascimento com TryParseExact
            Console.Write("Data de Nascimento (yyyy-MM-dd): ");
            DateTime dataNascimento;
            while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dataNascimento))
            {
                Console.WriteLine("Data inválida! Por favor, insira a data no formato yyyy-MM-dd.");
                Console.Write("Data de Nascimento (yyyy-MM-dd): ");
            }

            var cliente = new Cliente
            {
                Nome = nome,
                CPF = cpf,
                Telefone = telefone,
                Endereco = endereco,
                DataNascimento = dataNascimento
            };

            context.Clientes.Add(cliente);
            context.SaveChanges();
            Console.WriteLine("Cliente cadastrado com sucesso! Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        static void AgendarServico(AppDbContext context)
        {
            Console.WriteLine("\n=== Agendamento de Serviço ===");

            // Listar Clientes para mostrar os IDs
            ListarClientes(context);

            // Selecionar Cliente
            Console.Write("ID do Cliente: ");
            int idCliente = int.Parse(Console.ReadLine());
            var cliente = context.Clientes.Find(idCliente);
            if (cliente == null)
            {
                Console.WriteLine("Cliente não encontrado! Pressione qualquer tecla para voltar.");
                Console.ReadKey();
                return;
            }

            // Listar Profissionais para mostrar os IDs
            ListarProfissionais(context);

            // Selecionar Profissional
            Console.Write("ID do Profissional: ");
            int idProfissional = int.Parse(Console.ReadLine());
            var profissional = context.Profissionais.Find(idProfissional);
            if (profissional == null)
            {
                Console.WriteLine("Profissional não encontrado! Pressione qualquer tecla para voltar.");
                Console.ReadKey();
                return;
            }

            // Listar Serviços para mostrar os IDs
            ListarServicos(context);

            // Selecionar Serviço
            Console.Write("ID do Serviço: ");
            int idServico = int.Parse(Console.ReadLine());
            var servico = context.Servicos.Find(idServico);
            if (servico == null)
            {
                Console.WriteLine("Serviço não encontrado! Pressione qualquer tecla para voltar.");
                Console.ReadKey();
                return;
            }

            // Validação da data do agendamento com TryParseExact
            Console.Write("Data do Agendamento (yyyy-MM-dd): ");
            DateTime dataAgendamento;
            while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dataAgendamento))
            {
                Console.WriteLine("Data inválida! Por favor, insira a data no formato yyyy-MM-dd.");
                Console.Write("Data do Agendamento (yyyy-MM-dd): ");
            }

            Console.Write("Prioridade (baixa/média/alta): ");
            string prioridade = Console.ReadLine();

            var agendamento = new Agendamento
            {
                IdCliente = cliente.Id,
                IdProfissional = profissional.Id,
                IdServico = servico.Id,
                DataAgendamento = dataAgendamento,
                Prioridade = prioridade
            };

            context.Agendamentos.Add(agendamento);
            context.SaveChanges();
            Console.WriteLine("Serviço agendado com sucesso! Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        static void ListarServicosAgendados(AppDbContext context)
        {
            Console.WriteLine("\n=== Lista de Serviços Agendados ===");

            var agendamentos = context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .Include(a => a.Servico)
                .ToList();

            foreach (var agendamento in agendamentos)
            {
                Console.WriteLine($"Agendamento ID: {agendamento.Id}");
                Console.WriteLine($"Cliente: {agendamento.Cliente.Nome}");
                Console.WriteLine($"Profissional: {agendamento.Profissional.Nome}");
                Console.WriteLine($"Serviço: {agendamento.Servico.NomeServico} - R$ {agendamento.Servico.PrecoServico}");
                Console.WriteLine($"Data: {agendamento.DataAgendamento:yyyy-MM-dd}");
                Console.WriteLine($"Prioridade: {agendamento.Prioridade}");
                Console.WriteLine("----------------------------------");
            }
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        static void ListarClientes(AppDbContext context)
        {
            Console.WriteLine("\n=== Lista de Clientes ===");
            var clientes = context.Clientes.ToList();
            foreach (var cliente in clientes)
            {
                Console.WriteLine($"ID: {cliente.Id} | Nome: {cliente.Nome} | CPF: {cliente.CPF}");
            }
            Console.WriteLine("----------------------------------");
        }

        static void ListarProfissionais(AppDbContext context)
        {
            Console.WriteLine("\n=== Lista de Profissionais ===");
            var profissionais = context.Profissionais.ToList();
            foreach (var profissional in profissionais)
            {
                Console.WriteLine($"ID: {profissional.Id} | Nome: {profissional.Nome} | Função: {profissional.Funcao}");
            }
            Console.WriteLine("----------------------------------");
        }

        static void ListarServicos(AppDbContext context)
        {
            Console.WriteLine("\n=== Lista de Serviços ===");
            var servicos = context.Servicos.ToList();
            foreach (var servico in servicos)
            {
                Console.WriteLine($"ID: {servico.Id} | Serviço: {servico.NomeServico} | Preço: R$ {servico.PrecoServico}");
            }
            Console.WriteLine("----------------------------------");
        }
    }
}
