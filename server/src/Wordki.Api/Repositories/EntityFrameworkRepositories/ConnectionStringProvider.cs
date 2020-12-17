﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Wordki.Api.Repositories.EntityFrameworkRepositories
{
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; }
    }

    public class SimpleConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString { get; }

        public SimpleConnectionStringProvider(IOptions<DatabaseConfig> databaseConfig)
        {
            var config = databaseConfig.Value;
            ConnectionString = $"Host={config.Server};Port={config.Port};Database={config.Database};User Id={config.User};Password={config.Password}";
        }
    }

    public class HerokuConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string PostgressTag = "DATABASE_URL";
        private readonly ILogger<IConnectionStringProvider> logger;

        public string ConnectionString { get; }

        public HerokuConnectionStringProvider(ILogger<IConnectionStringProvider> logger){
            this.logger = logger;
        }

        public HerokuConnectionStringProvider(IConfiguration configuration)
        {
            var value = configuration.GetValue<string>(PostgressTag);
            logger.LogInformation(value);
            value = value.Remove(0, "postgres://".Length);
            var counter = value.IndexOf(':');
            var user = value.Substring(0, counter);
            value = value.Remove(0, counter + 1);

            counter = value.IndexOf('@');
            var password = value.Substring(0, counter);
            value = value.Remove(0, counter + 1);

            counter = value.IndexOf(':');
            var host = value.Substring(0, counter);
            value = value.Remove(0, counter + 1);

            counter = value.IndexOf('/');
            var port = value.Substring(0, counter);
            value = value.Remove(0, counter + 1);

            var database = value;

            ConnectionString = $"Host={host};Port={port};Database={database};User Id={user};Password={password}";
            logger.LogInformation(ConnectionString);
        }
    }
}
