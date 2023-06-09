using FlavorFi.Services.ShopWorksServices;
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;

namespace FlavorFi.ShopWorks.Service.Services
{
    public class BaseService
    {
        public Guid CompanySiteId { get; private set; }
        public ShopWorksBaseService ShopWorksBaseService { get; private set; }
        public string Log { get; set; }

        public BaseService()
        {
            this.ShopWorksBaseService = new ShopWorksBaseService();
            this.CompanySiteId = Guid.Parse("7866C5D7-425F-43A5-A1D5-CAEC13CC10E7");
        }

        public void ConsoleLog(string message, bool addDate = false)
        {
            if (addDate) message += " [" + DateTime.Now + "]";
            Console.WriteLine(message);
            this.Log += message + Environment.NewLine;
        }

        public void GetDatabaseSchema(string database, OdbcConnection connection)
        {
            this.ConsoleLog(database + " Schema");
            using (var cmd = new OdbcCommand("", connection))
            {
                cmd.Connection.Open();
                cmd.Connection.GetSchema("Tables").AsEnumerable().Select(r => r.Field<string>(2)).ToList().ForEach(r => this.ConsoleLog(Convert.ToString(r)));
            }
            this.ConsoleLog("");
        }

        public void GetTableSchema(string table, OdbcConnection connection)
        {
            this.ConsoleLog(table + " Schema");
            using (var cmd = new OdbcCommand("SELECT * FROM " + table, connection))
            {
                cmd.Connection.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    var tableSchema = reader.GetSchemaTable();
                    foreach (DataRow row in tableSchema.Rows)
                        ConsoleLog(Convert.ToString(row["ColumnName"]));
                }
            }
            this.ConsoleLog("");
        }

        public void WriteFile()
        {
            var path = @"C:\Logs\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
            if (!File.Exists(path))
                File.WriteAllText(path, this.Log);
        }
    }
}
