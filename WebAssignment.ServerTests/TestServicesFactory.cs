using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WebAssignment.Server.Context;
using WebAssignment.Server.Enums;
using WebAssignment.Server.Repositories;

namespace WebAssignment.ServerTests;
internal static class TestServicesFactory
{
    private static readonly ServiceProvider s_serviceProvider = InitializeServiceProvider();

    private static ServiceProvider InitializeServiceProvider()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("configuration.json").Build();
        ServiceCollection services = new();
        LinqToDBForEFTools.Initialize();

        NpgsqlDataSourceBuilder dataSourceBuilder = new(configuration.GetConnectionString("TransactionContext"));

        _ = dataSourceBuilder.MapEnum<TransactionStatus>();

        NpgsqlDataSource dataSource = dataSourceBuilder.Build();

        _ = services.AddDbContextPool<TransactionContext>(options => _ = options.UseNpgsql(dataSource, options => options.EnableRetryOnFailure()));
        _ = services.AddScoped<AssignmentRepositories>();

        return services.BuildServiceProvider();
    }

    public static AssignmentRepositories GetAssignmentRepositories()
    {
        return s_serviceProvider.GetRequiredService<AssignmentRepositories>();
    }
}
