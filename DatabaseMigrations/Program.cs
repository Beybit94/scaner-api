using Common.Configuration;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            var connectionString = GetConnectionStrings();
            var previewOnly = GetBoolParam(args, "preview");
            Migrate(connectionString, previewOnly, true);
        }

        [Obsolete]
        private static void Migrate(string connectionString, bool previewOnly, bool showSql)
        {
            var logFile = File.Open(GetFilePath(connectionString), FileMode.CreateNew, FileAccess.Write);
            var writer = new StreamWriter(logFile);
            var consoleAnnouncer = new CustomConsoleAnnouncer();
            var textAnnouncer = new TextWriterAnnouncer(writer) { ShowElapsedTime = showSql, ShowSql = showSql };

            IAnnouncer announcer = new CompositeAnnouncer(consoleAnnouncer, textAnnouncer);

            var migrationContext = new RunnerContext(announcer);
            var factory = new SqlServer2012ProcessorFactory();
            var assembly = typeof(Program).Assembly;
            Console.WriteLine("Migration: " + connectionString);
            Console.WriteLine();

            try
            {
                using (var processor = factory.Create(connectionString, announcer,
                    new ProcessorOptions { PreviewOnly = previewOnly, Timeout = new TimeSpan(900) }))
                {
                    var runner = new MigrationRunner(assembly, migrationContext, processor);
                    runner.MigrateUp();
                }

            }
            catch (Exception ex)
            {
                writer.Write(ex);
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("..............................................");
                Console.WriteLine("Press Enter key to continue...");
                Console.ReadLine();
            }

            writer.Flush();
            writer.Close();

        }

        private static bool GetBoolParam(string[] args, string paramName)
        {
            return args.Any(s => s.Replace("-", "").ToLowerInvariant().Equals(paramName.ToLowerInvariant()));
        }

        private static string GetFilePath(string connectionString)
        {
            var directoryName = "migrationLogs";
            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);
            var startIndex = connectionString.IndexOf("Initial Catalog=") + 16;
            var fileName = connectionString.Substring(startIndex,
                connectionString.IndexOf(";", startIndex) - startIndex);
            var filePath = Path.Combine(directoryName, fileName + ".log");
            if (File.Exists(filePath)) File.Delete(filePath);
            return filePath;
        }

        private static string GetConnectionStrings()
        {
            return ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString;
        }
    }
}
