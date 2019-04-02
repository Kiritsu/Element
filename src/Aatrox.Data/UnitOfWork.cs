﻿using Aatrox.Data.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aatrox.Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly AatroxDbContext _context;

        private bool _disposed;

        public IGuildRepository UserRepository { get; }

        internal UnitOfWork(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
            _context = new AatroxDbContext();

            UserRepository = new GuildRepository(_context.Guilds);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _semaphore.Release();
                }
            }
            _disposed = true;
        }
    }
}