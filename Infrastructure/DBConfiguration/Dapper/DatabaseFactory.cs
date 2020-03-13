﻿using Infrastructure.Interfaces.DBConfiguration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;

namespace Infrastructure.DBConfiguration.Dapper
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private IOptions<DataSettings> dataSettings;
        protected string ConnectionString => !string.IsNullOrEmpty(dataSettings.Value.DefaultConnection) ?
                                                    dataSettings.Value.DefaultConnection :
                                                    DBConfiguration.DatabaseConnection.ConnectionConfiguration
                                                                                      .GetConnectionString("DefaultConnection");

        public IDbConnection GetDbConnection => new SqlConnection(ConnectionString);

        public DatabaseFactory(IOptions<DataSettings> dataSettings)
        {
            this.dataSettings = dataSettings;

        }
    }
}
