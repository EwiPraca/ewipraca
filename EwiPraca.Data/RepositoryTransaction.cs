﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwiPraca.Data.Interfaces
{
    public class RepositoryTransaction : IRepositoryTransaction
    {
        private DbContextTransaction dbContextTransaction;

        public RepositoryTransaction(DbContextTransaction dbContextTransaction)
        {
            this.dbContextTransaction = dbContextTransaction;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
