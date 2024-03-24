using BankManagementSystem.BackendWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagementSystem.BackendWebAPI
{
    public class AppDBContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
              SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder()
             {
                 DataSource = "HA\\SQL2019",
                 InitialCatalog = "BankingManagementSystem",
                 UserID = "sa",
                 Password = "sasa",
                 TrustServerCertificate = true,
             };


        optionsBuilder.UseSqlServer(sqlBuilder.ConnectionString); 
        }

        public DbSet<StateModel> States { get; set; }
        public DbSet<TownshipModel> Townships { get; set; }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<TransactionHistoryModel> TransactionHistories { get; set; }
    }
}
