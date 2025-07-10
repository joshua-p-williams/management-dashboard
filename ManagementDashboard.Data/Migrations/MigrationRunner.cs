using System;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace ManagementDashboard.Data.Migrations
{
    public class MigrationRunner
    {
        private readonly string _migrationsPath;
        private readonly Func<IDbConnection> _connectionFactory;

        public MigrationRunner(string migrationsPath, Func<IDbConnection> connectionFactory)
        {
            _migrationsPath = migrationsPath;
            _connectionFactory = connectionFactory;
        }

        public void RunMigrations()
        {
            var migrationFiles = Directory.GetFiles(_migrationsPath, "*.sql").OrderBy(f => f);
            using var conn = _connectionFactory();
            conn.Open();
            EnsureMigrationsTable(conn);
            foreach (var file in migrationFiles)
            {
                var migrationName = Path.GetFileName(file);
                if (!IsMigrationApplied(conn, migrationName))
                {
                    var sql = File.ReadAllText(file);
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    MarkMigrationApplied(conn, migrationName);
                }
            }
        }

        private void EnsureMigrationsTable(IDbConnection conn)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS _Migrations (Id INTEGER PRIMARY KEY AUTOINCREMENT, MigrationName TEXT NOT NULL UNIQUE, AppliedOnUtc DATETIME NOT NULL);";
            cmd.ExecuteNonQuery();
        }

        private bool IsMigrationApplied(IDbConnection conn, string migrationName)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(1) FROM _Migrations WHERE MigrationName = @name";
            var param = cmd.CreateParameter();
            param.ParameterName = "@name";
            param.Value = migrationName;
            cmd.Parameters.Add(param);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private void MarkMigrationApplied(IDbConnection conn, string migrationName)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO _Migrations (MigrationName, AppliedOnUtc) VALUES (@name, @utc)";
            var p1 = cmd.CreateParameter();
            p1.ParameterName = "@name";
            p1.Value = migrationName;
            var p2 = cmd.CreateParameter();
            p2.ParameterName = "@utc";
            p2.Value = DateTime.Now;
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.ExecuteNonQuery();
        }
    }
}
