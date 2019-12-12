﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Aatrox.Core.Abstractions;
using Aatrox.Core.Configurations;
using Aatrox.Core.Interfaces;
using Aatrox.Core.Services;
using Aatrox.Data;
using Aatrox.Data.EventArgs;
using Aatrox.Services;
using Aatrox.TypeParsers;
using Disqord;
using Disqord.Rest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace Aatrox
{
    public class Aatrox
    {
        private IConfiguration _configuration;
        private IServiceProvider _services;
        private LogService _dbLogger;

        private async Task InitializeAsync()
        {
            var configPath = Environment.GetEnvironmentVariable("AATROX_CONFIG_PATH") ?? "credentials.json";

            var cfg = new ConfigurationBuilder()
                .AddJsonFile(configPath, false)
                .Build();

            _configuration = cfg;
            _services = BuildServiceProvider();
            _dbLogger = LogService.GetLogger("Database");

            try
            {
                AatroxDbContext.DatabaseUpdated += DatabaseUpdated;
                using var db = _services.GetRequiredService<AatroxDbContext>();
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                _dbLogger.Error("Database migration failed. Exiting.", ex);
                return;
            }

            var ds = _services.GetRequiredService<IDiscordService>();
            ds.AddTypeParser(CachedChannelParser.Instance);
            ds.AddTypeParser(CachedGuildParser.Instance);
            ds.AddTypeParser(CachedUserParser.Instance);
            ds.AddTypeParser(CachedMemberParser.Instance);
            ds.AddTypeParser(SkeletonUserParser.Instance);
            ds.AddTypeParser(TimeSpanParser.Instance);
            ds.AddTypeParser(UriTypeParser.Instance);
            await ds.SetupAsync(Assembly.GetEntryAssembly());

            await Task.Delay(Timeout.Infinite);
        }

        private IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()
                .AddSingleton(x => new LogService("Aatrox"))
                .Configure<AatroxConfiguration>(x => _configuration.GetSection("Aatrox").Bind(x))
                .AddSingleton<IAatroxConfigurationProvider, AatroxConfigurationProvider>()
                .Configure<DatabaseConfiguration>(x => _configuration.GetSection("Database").Bind(x))
                .AddSingleton<IDatabaseConfigurationProvider, DatabaseConfigurationProvider>()
                .AddSingleton<ConnectionStringProvider>()
                .AddDbContext<AatroxDbContext>(ServiceLifetime.Transient)
                .AddSingleton(x =>
                {
                    var token = x.GetRequiredService<IAatroxConfigurationProvider>().GetConfiguration().Token;
                    return new DiscordClient(new RestDiscordClient(TokenType.Bot, token));
                })
                .AddSingleton<ICommandService, CommandService>()
                .AddSingleton<IDiscordService, DiscordService>()
                .AddSingleton<IPaginatorService, PaginatorService>()
                .BuildServiceProvider();
        }

        private Task DatabaseUpdated(DatabaseActionEventArgs arg)
        {
            if (arg.IsErrored)
            {
                _dbLogger.Error(arg.Path, arg.Exception);
            }
            else
            {
                _dbLogger.Debug(arg.Path);
            }

            return Task.CompletedTask;
        }

        private static async Task Main()
        {
            await new Aatrox().InitializeAsync();
        }
    }
}
