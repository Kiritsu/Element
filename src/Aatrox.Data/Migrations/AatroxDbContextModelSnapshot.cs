﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Aatrox.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Aatrox.Data.Migrations
{
    [DbContext(typeof(AatroxDbContext))]
    partial class AatroxDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Aatrox.Data.Entities.GuildEntity", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnName("snowflake_id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<string>>("Prefixes")
                        .HasColumnName("prefixes")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("guild_entity");
                });

            modelBuilder.Entity("Aatrox.Data.Entities.LeagueUserEntity", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnName("snowflake_id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<List<long>>("Channels")
                        .HasColumnName("channels")
                        .HasColumnType("bigint[]");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("CurrentGameInfo")
                        .HasColumnName("send_current_game_info")
                        .HasColumnType("boolean");

                    b.Property<string>("Region")
                        .HasColumnName("region")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnName("username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("league_user_entity");
                });

            modelBuilder.Entity("Aatrox.Data.Entities.OsuUserEntity", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnName("snowflake_id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<List<long>>("Channels")
                        .HasColumnName("channels")
                        .HasColumnType("bigint[]");

                    b.Property<int>("CountryRankMin")
                        .HasColumnName("country_rank_min")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PpMin")
                        .HasColumnName("pp_min")
                        .HasColumnType("integer");

                    b.Property<bool>("SendNewBestScore")
                        .HasColumnName("send_new_best_score")
                        .HasColumnType("boolean");

                    b.Property<bool>("SendRecentScore")
                        .HasColumnName("send_recent_score")
                        .HasColumnType("boolean");

                    b.Property<string>("Username")
                        .HasColumnName("username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("osu_user_entity");
                });

            modelBuilder.Entity("Aatrox.Data.Entities.UserEntity", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnName("snowflake_id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Language")
                        .HasColumnName("language")
                        .HasColumnType("integer");

                    b.Property<bool>("Premium")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("user_entity");
                });

            modelBuilder.Entity("Aatrox.Data.Entities.LeagueUserEntity", b =>
                {
                    b.HasOne("Aatrox.Data.Entities.UserEntity", "User")
                        .WithOne("LeagueProfile")
                        .HasForeignKey("Aatrox.Data.Entities.LeagueUserEntity", "Id")
                        .HasConstraintName("fkey_league_user_entity_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Aatrox.Data.Entities.OsuUserEntity", b =>
                {
                    b.HasOne("Aatrox.Data.Entities.UserEntity", "User")
                        .WithOne("OsuProfile")
                        .HasForeignKey("Aatrox.Data.Entities.OsuUserEntity", "Id")
                        .HasConstraintName("fkey_osu_user_entity_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
