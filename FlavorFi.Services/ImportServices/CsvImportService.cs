using System;
using System.Collections.Generic;
using System.Text;
using FlavorFi.Common.Models.DatabaseModels;
using FlavorFi.Services.DatabaseServices;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace FlavorFi.Services.ImportServices
{
    public interface ICsvImportService
    {
        List<ShopifyGiftCardReportExportModel> CreateModelListFromCsvFile(string path);
    }

    public class CsvImportService : BaseService, ICsvImportService
    {
        public List<ShopifyGiftCardReportExportModel> CreateModelListFromCsvFile(string path)
        {
            try
            {
                var lst = new List<ShopifyGiftCardReportExportModel>();
                var csvParserOptions = new CsvParserOptions(true, ',');
                var csvParser = new CsvParser<ShopifyGiftCardReportExportModel>(csvParserOptions, new CsvShopifyGiftCardReportMapping());
                var records = csvParser.ReadFromFile(path, Encoding.UTF8);
                foreach (var record in records)
                {
                    lst.Add(new ShopifyGiftCardReportExportModel
                    {
                        OriginalShopifyId = record.Result.OriginalShopifyId,
                        LastFour = record.Result.LastFour,
                        CustomerName = record.Result.CustomerName,
                        Email  = record.Result.Email,
                        OrderName = record.Result.OrderName,
                        CreatedAt = record.Result.CreatedAt,
                        ExpiresOn = record.Result.ExpiresOn,
                        InitialValue = record.Result.InitialValue,
                        Balance = record.Result.Balance,
                        Currency = record.Result.Currency,
                        Expired = record.Result.Expired,
                        Enabled = record.Result.Enabled,
                        DisabledAt = record.Result.DisabledAt
                    });
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class CsvShopifyGiftCardReportMapping : CsvMapping<ShopifyGiftCardReportExportModel>
    {
        public CsvShopifyGiftCardReportMapping() : base()
        {
            MapProperty(0, x => x.OriginalShopifyId);
            MapProperty(1, x => x.LastFour);
            MapProperty(2, x => x.CustomerName);
            MapProperty(3, x => x.Email);
            MapProperty(4, x => x.OrderName);
            MapProperty(5, x => x.CreatedAt, new DateTimeOffestTypeConverter());
            MapProperty(6, x => x.ExpiresOn, new NullableDateTimeOffestTypeConverter());
            MapProperty(7, x => x.InitialValue);
            MapProperty(8, x => x.Balance);
            MapProperty(9, x => x.Currency);
            MapProperty(10, x => x.Expired);
            MapProperty(11, x => x.Enabled);
            MapProperty(12, x => x.DisabledAt, new NullableDateTimeOffestTypeConverter());
        }
    }

    public class DateTimeOffestTypeConverter : ITypeConverter<DateTimeOffset>
    {
        public Type TargetType => typeof(DateTimeOffset);

        public bool TryConvert(string value, out DateTimeOffset result)
        {
            result = DateTimeOffset.Parse(value);
            return true;
        }
    }

    public class NullableDateTimeOffestTypeConverter : ITypeConverter<DateTimeOffset?>
    {
        public Type TargetType => typeof(DateTimeOffset?);

        public bool TryConvert(string value, out DateTimeOffset? result)
        {
            result = string.IsNullOrEmpty(value) ? (DateTimeOffset?)null : DateTimeOffset.Parse(value);
            return true;
        }
    }
}