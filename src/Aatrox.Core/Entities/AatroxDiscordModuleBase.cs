﻿using System.Threading.Tasks;
using Aatrox.Core.Helpers;
using Aatrox.Core.Services;
using Disqord;
using Disqord.Rest;
using Qmmands;

namespace Aatrox.Core.Entities
{
    public class AatroxDiscordModuleBase : ModuleBase<AatroxDiscordCommandContext>
    {
        public DatabaseContext DbContext => Context.DatabaseContext;

        public Task<RestUserMessage> RespondLocalizedAsync(string key, params object[] parameters)
        {
            var localization = InternationalizationService.GetLocalization(key, DbContext.User.Language, parameters);

            return Context.Channel.SendMessageAsync(localization);
        }

        public Task<RestUserMessage> RespondEmbedLocalizedAsync(string key, params object[] parameters)
        {
            var localization = InternationalizationService.GetLocalization(key, DbContext.User.Language, parameters);
            var embed = EmbedHelper.New(Context, localization);

            return Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        public Task<RestUserMessage> RespondAsync(string message)
        {
            return Context.Channel.SendMessageAsync(message);
        }

        public Task<RestUserMessage> RespondEmbedAsync(string message)
        {
            var embed = EmbedHelper.New(Context, message);

            return Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        public Task<RestUserMessage> RespondAsync(string message, LocalEmbed embed)
        {
            return Context.Channel.SendMessageAsync(message, embed: embed);
        }

        public Task<RestUserMessage> RespondAsync(LocalEmbed embed)
        {
            return Context.Channel.SendMessageAsync(embed: embed);
        }
    }
}