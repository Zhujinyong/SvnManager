using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Centa.SvnLog.Infrastructure.Interfaces;

namespace Centa.SvnLog.Infrastructure
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IDapperContext Context { get; private set; }

        public IDbTransaction Transaction { get; private set; }

        public UnitOfWork(IDapperContext context)
        {
            Context = context;
        }

        public IDbTransaction BeginTransaction()
        {
            if (Transaction != null)
            {
                throw new NullReferenceException("Not finished previous transaction");
            }
            Transaction = Context.Connection.BeginTransaction();
            return Transaction;
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
            else
            {
                throw new NullReferenceException("Tryed commit not opened transaction");
            }
        }

        private bool HasActiveTransaction
        {
            get
            {
                return Transaction != null;
            }
        }

        private void Rollback()
        {
            Transaction.Rollback();
            Transaction = null;
        }

        public void RunTransaction(Action<IDbTransaction> action)
        {
            BeginTransaction();
            try
            {
                action(Transaction);
                Commit();
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                {
                    Rollback();
                }
                throw ex;
            }
        }

        public int Excute(string sql)
        {
            return Context.Connection.Execute(sql, transaction: Transaction);
        }

        public IEnumerable<dynamic> Query(string sql)
        {
            return Context.Connection.Query<dynamic>(sql);
        }

        /// <summary>
        /// 设置上下文连接字符串
        /// </summary>
        /// <param name="connectionString"></param>
        public void SetContext(string connectionString)
        {
            IDapperContext context = new DapperContext(connectionString);
            Context.Connection.Dispose();
            Context = context;
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
            }
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}