using FluentMigrator.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations
{
    [Obsolete]
    public class CustomConsoleAnnouncer : IAnnouncer
    {
        private int _errorMessageCount = 0;
        public void Heading(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.Write(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " [" + message + "] ");
            Console.ResetColor();
        }

        public void ElapsedTime(TimeSpan timeSpan)
        {
            Console.WriteLine("Elapsed time: " + Math.Round(timeSpan.TotalMinutes, 3) + " minutes");
        }

        public void Error(string message)
        {
            if (_errorMessageCount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Exception: " + message);
                Console.ResetColor();
            }

            _errorMessageCount++;
        }

        public void Say(string message)
        {
        }

        public void Sql(string sql)
        {
        }

        public void Error(Exception exception)
        {
        }

        public void Write(string message, bool isNotSql = true)
        {
        }

        public void Emphasize(string message)
        {
            throw new NotImplementedException();
        }
    }
}
