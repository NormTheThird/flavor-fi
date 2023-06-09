using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using FlavorFi.Services.ShopWorksServices;
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;

namespace FlavorFi.ShopWorks.Services
{
    public class BaseService
    {
        public CompanySiteModel CompanySite { get; private set; }
        public ShopWorksBaseService ShopWorksBaseService { get; private set; }
        public string Log { get; set; }

        public BaseService(Guid companySiteId)
        {
            try
            {
                var companySiteResponse = CompanyService.GetCompanySite(new GetCompanySiteRequest { CompanySiteId = companySiteId });
                if (!companySiteResponse.IsSuccess) throw new ApplicationException(companySiteResponse.ErrorMessage);

                this.CompanySite = companySiteResponse.CompanySite;
                this.ShopWorksBaseService = new ShopWorksBaseService(this.CompanySite.CompanyId);
            }
            catch (Exception ex) { throw ex; }

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

        public void WriteFile(string logName)
        {
            var path = @"C:\Logs\" + logName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".txt";
            if (!Directory.Exists(@"C:\Logs\"))
                Directory.CreateDirectory(@"C:\Logs\");
            if (!File.Exists(path))
                File.WriteAllText(path, this.Log);
        }
    }
}