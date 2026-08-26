using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Migrations
{
    public class Program
    {
        private const string CompanyCnpjPlaceholder = "COMPANY_CNPJ";

        static void Main(string[] args)
        {
            Console.WriteLine("Execução Migration");

            if (args.Length < 1)
                throw new Exception("Necessário argumento 0: Nome da connectionstring.");

            if (args.Length < 2)
                throw new Exception("Necessário argumento 1: CNPJ do tenant.");

            Console.WriteLine("Working Dir.: " + AppContext.BaseDirectory);

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string? connection = configuration.GetConnectionString(args[0]);

            if (string.IsNullOrEmpty(connection))
                throw new Exception("Connectionstring não encontrada.");

            string cnpj = NormalizeCnpj(args[1]);
            connection = connection.Replace(CompanyCnpjPlaceholder, cnpj);

            IServiceProvider serviceProvider = CreateServices(connection);

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
                Console.WriteLine("Migration executado com sucesso.");
            }
        }

        private static string NormalizeCnpj(string cnpj)
        {
            string normalizedCnpj = new(cnpj.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(normalizedCnpj))
                throw new Exception("CNPJ do tenant inválido.");

            return normalizedCnpj;
        }

        private static IServiceProvider CreateServices(string connection)
        {
            return new ServiceCollection().AddFluentMigratorCore().
                ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalCommandTimeout(TimeSpan.FromMinutes(5))
                .WithGlobalConnectionString(connection)
                .ScanIn(typeof(Program).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider();
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp();
        }
    }
}