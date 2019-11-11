﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatrox.Core.Entities;
using Qmmands;

namespace Aatrox.Modules
{
    [Name("Settings"), Group("Settings")]
    public sealed class SettingsCommands : AatroxModuleBase
    {
        [Name("Prefix"), Group("Prefix")]
        public sealed class PrefixCommands : AatroxModuleBase
        {
            [Command("List")]
            [Description("List the different prefixes for that guild.")]
            public Task ListAsync()
            {
                if (DbContext.Guild.Prefixes.Count <= 0)
                {
                    return RespondEmbedLocalizedAsync("no_custom_prefix");
                }

                return RespondEmbedAsync(string.Join(", ", DbContext.Guild.Prefixes.Select(x => $"`{x}`")));
            }

            [Command("Add")]
            [Description("Adds a prefix for that guild.")]
            public async Task AddAsync([Description("Prefix to add"), Remainder] string prefix)
            {
                if (DbContext.Guild.Prefixes.Select(x => x.ToLowerInvariant()).Contains(prefix.ToLowerInvariant()))
                {
                    await RespondEmbedLocalizedAsync("prefix_already_added");
                    return;
                }

                DbContext.Guild.Prefixes.Add(prefix);
                await DbContext.UpdateGuildAsync();
                await RespondEmbedLocalizedAsync("prefix_added");
            }

            [Command("Remove", "Delete")]
            [Description("Removes a prefix from that guild.")]
            public async Task RemoveAsync([Description("Prefix to remove"), Remainder] string prefix)
            {
                if (!DbContext.Guild.Prefixes.Select(x => x.ToLowerInvariant()).Contains(prefix.ToLowerInvariant()))
                {
                    await RespondEmbedLocalizedAsync("unknown_prefix");
                    return;
                }

                DbContext.Guild.Prefixes.RemoveAll(x => x.Equals(prefix, StringComparison.OrdinalIgnoreCase));
                await DbContext.UpdateGuildAsync();
                await RespondEmbedLocalizedAsync("prefix_removed");
            }
        }
    }
}
