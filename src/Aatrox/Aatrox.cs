﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Aatrox.Core.Configurations;
using Aatrox.Core.Entities;
using Aatrox.Core.Interfaces;
using Aatrox.Core.Services;
using Aatrox.Data;
using Aatrox.Data.EventArgs;
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
            const string configPath = "credentials.json";

            var cfg = new ConfigurationBuilder()
                .AddJsonFile(configPath, false)
                .Build();

            _configuration = cfg;
            _services = BuildServiceProvider();
            _dbLogger = LogService.GetLogger("Database");

            AatroxDbContext.DatabaseUpdated += DatabaseUpdated;
            using (var db = _services.GetRequiredService<AatroxDbContext>())
            {
                db.Database.Migrate();
            }

            var ds = _services.GetRequiredService<DiscordService>();
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
                    var token = x.GetService<IAatroxConfigurationProvider>().GetConfiguration().Token;
                    return new DiscordClient(new RestDiscordClient(TokenType.Bot, token));
                })
                .AddSingleton<CommandService>()
                .AddSingleton<DiscordService>()
                .AddSingleton<TypeParser<CachedGuild>, CachedGuildParser>()
                .AddSingleton<TypeParser<CachedGuildChannel>, CachedChannelParser>()
                .AddSingleton<TypeParser<CachedUser>, CachedUserParser>()
                .AddSingleton<TypeParser<CachedMember>, CachedMemberParser>()
                .AddSingleton<TypeParser<SkeletonUser>, SkeletonUserParser>()
                .AddSingleton<TypeParser<TimeSpan>, TimeSpanParser>()
                .AddSingleton<TypeParser<Uri>, UriTypeParser>()
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
