using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var migrationFiles = GetMigrationFiles();

            using var conn = _connectionFactory();
            conn.Open();
            EnsureMigrationsTable(conn);

            foreach (var (fileName, content) in migrationFiles)
            {
                if (!IsMigrationApplied(conn, fileName))
                {
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = content;
                    cmd.ExecuteNonQuery();
                    MarkMigrationApplied(conn, fileName);
                }
            }
        }

        private (string fileName, string content)[] GetMigrationFiles()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames()
                .Where(x => x.Contains("Migrations") && x.EndsWith(".sql"))
                .OrderBy(x => x)
                .ToArray();

            var migrations = new List<(string, string)>();

            foreach (var resourceName in resourceNames)
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    var content = reader.ReadToEnd();
                    // Extract just the filename from the resource name
                    var fileName = resourceName.Split('.').TakeLast(2).First() + ".sql";
                    migrations.Add((fileName, content));
                }
            }

            // Fallback to file system for backward compatibility
            if (migrations.Count == 0 && Directory.Exists(_migrationsPath))
            {
                var fileNames = Directory.GetFiles(_migrationsPath, "*.sql").OrderBy(f => f);
                foreach (var file in fileNames)
                {
                    var fileName = Path.GetFileName(file);
                    var content = File.ReadAllText(file);
                    migrations.Add((fileName, content));
                }
            }

            return migrations.ToArray();
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
