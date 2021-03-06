﻿using System;
using System.Threading.Tasks;
using Aatrox.Data;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Prefixes;
using Microsoft.Extensions.DependencyInjection;

namespace Aatrox.Core.Entities
{
    public sealed class AatroxCommandContext : DiscordCommandContext, IAsyncDisposable
    {
        public CachedUser Aatrox => Bot.CurrentUser;
        public CachedMember CurrentMember => Guild.CurrentMember;

        private readonly DatabaseCommandContext _databaseContext;

        public AatroxCommandContext(DiscordBotBase bot, CachedUserMessage message, IPrefix prefix)
            : base(bot, prefix, message)
        {
            _databaseContext = new DatabaseCommandContext(
                this, ServiceProvider.GetRequiredService<AatroxDbContext>());
        }
        
        public DatabaseCommandContext DatabaseContext
        {
            get
            {
                if (!_databaseContext.IsReady)
                {
                    throw new InvalidOperationException(
                        "The database context is not ready. Please make sure it's prepared.");
                }

                return _databaseContext;
            }
        }

        public Task PrepareAsync()
        {
            return _databaseContext.PrepareAsync();
        }

        public Task EndAsync()
        {
            return _databaseContext.Database.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _databaseContext.DisposeAsync();
        }
    }
}
