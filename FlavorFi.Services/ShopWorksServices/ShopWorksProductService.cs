using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;

namespace FlavorFi.Services.ShopWorksServices
{
    public class ShopWorksProductService : ShopWorksBaseService
    {
        public List<ShopifyCreateProductModel> Products { get; set; }
        public Dictionary<string, int> ProductInventory { get; set; }
        private Dictionary<int, string> ProductClasses { get; set; }

        public ShopWorksProductService(Guid companyId) : base(companyId) { }

        public GetShopWorksProductsResponse GetShopWorksProducts(GetShopWorksProductsRequest request)

        {
            try
            {
                this.ProductClasses = this.GetShopWorksProductClasses();
                this.ProductInventory = this.GetProductInventory();
                var response = new GetShopWorksProductsResponse();
                var cmdText = GetProductCmdText() + "WHERE sts_AllowUpdate = 1";
                using (var cmd = new OdbcCommand(cmdText, FileMakerProductsConnection))
                {
                    cmd.Connection.Open();
                    this.Products = new List<ShopifyCreateProductModel>();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        while (reader.Read())
                        {
                            //LogReader(reader);
                            CreateProductModel(reader);
                        }

                    response.Products = this.Products;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopWorksProductsResponse { ErrorMessage = ex.Message };
            }
        }

        public GetShopWorksProductResponse GetShopWorksProduct(GetShopWorksProductRequest request)
        {
            try
            {
                this.ProductClasses = this.GetShopWorksProductClasses();
                this.ProductInventory = this.GetProductInventory("WHERE PartNumber = '" + request.PartNumber.Trim() + "'");
                var response = new GetShopWorksProductResponse();
                var cmdText = GetProductCmdText() + "WHERE p.PartNumber = '" + request.PartNumber.Trim() + "' AND p.sts_AllowUpdate = 1";
                using (var cmd = new OdbcCommand(cmdText, this.FileMakerProductsConnection))
                {
                    cmd.Connection.Open();
                    this.Products = new List<ShopifyCreateProductModel>();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        while (reader.Read())
                        {
                            LogReader(reader);
                            CreateProductModel(reader);
                       }

                    response.Products = this.Products;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new GetShopWorksProductResponse { ErrorMessage = ex.Message };
            }
        }



        private void CreateProductModel(OdbcDataReader reader)
        {
            try
            {
                var partNumberArr = ConvertToString(reader["PartNumber"]).Split('_');
                var product = this.Products.FirstOrDefault(p => p.PartNumber.Equals(partNumberArr[0], StringComparison.CurrentCultureIgnoreCase));
                if (product == null)
                {
                    product = new ShopifyCreateProductModel { PartNumber = partNumberArr[0] };
                    this.Products.Add(product);
                }

                product.Title = ConvertToString(reader["Description"]);
                product.BodyHtml = ConvertToString(reader["import_Notes"]);
                product.ProductType = ConvertToString(reader["ProductType"]);
                product.Vendor = ConvertToString(reader["VendorName"]);

                if (reader["FileName"] != DBNull.Value)
                {
                    var imageString = this.ConvertImageToBase64String(ConvertToString(reader["ID_Serial"]) + "_" + ConvertToString(reader["FileName"]));
                    if (!string.IsNullOrEmpty(imageString))
                        product.Images.Add(new ShopifyProductImageModel { Attachment = imageString });
                }

                this.CreateMetafield(product, "shipping_days", ConvertToString(reader["CustomField09"]));
                this.CreateMetafield(product, "gender", ConvertToString(reader["CustomField01"]));
                this.CreateMetafield(product, "sub_category", ConvertToString(reader["CustomField02"]));
                this.CreateMetafield(product, "code_price", ConvertToString(reader["MtxPrice"]));
                this.CreateMetafield(product, "cost_price", ConvertToString(reader["MtxCost01"]));
                this.CreateMetafield(product, "find_code", ConvertToString(reader["FindCode"]));
                this.CreateMetafield(product, "preprint_group", ConvertToString(reader["PreprintGroup"]));

                var productClass = this.ProductClasses.FirstOrDefault(pc => pc.Key == ConvertToInt32(reader["id_ProductClass"]));
                if (productClass.Key > 0) this.CreateMetafield(product, "category", productClass.Value);

                var price = ConvertToDecimal(reader["MtxPrice01"]);
                foreach (var color in GetProductColors(ConvertToString(reader["id_ProductColorBlock"])))
                {
                    if (ConvertToInt32(reader["cn_sts_LimitSize01Button"]) > 0)
                        this.CreateVariant(product, color, "s", price);
                    if (ConvertToInt32(reader["cn_sts_LimitSize02Button"]) > 0)
                        this.CreateVariant(product, color, "m", price);
                    if (ConvertToInt32(reader["cn_sts_LimitSize03Button"]) > 0)
                        this.CreateVariant(product, color, "l", price);
                    if (ConvertToInt32(reader["cn_sts_LimitSize04Button"]) > 0)
                        this.CreateVariant(product, color, "xl", price);
                    if (ConvertToInt32(reader["cn_sts_LimitSize05Button"]) > 0)
                        this.CreateVariant(product, color, "xxl", price);
                    if (ConvertToInt32(reader["cn_sts_LimitSize06Button"]) > 0)
                    {
                        var size = "o/s";
                        if (partNumberArr.Length > 1) size = partNumberArr[1];
                        this.CreateVariant(product, color, size, price);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                Console.WriteLine("Unable to create model for " + Convert.ToString(reader["PartNumber"]) + " Error: " + ex.Message);
            }
        }

        private void CreateVariant(ShopifyCreateProductModel product, string color, string size, decimal price)
        {
            try
            {
                if (string.IsNullOrEmpty(color)) color = "o/c";
                if (string.IsNullOrEmpty(size)) size = "o/s";
                var sku = product.PartNumber.Trim() + "-" + color + "-" + size;
                var variant = product.Variants.FirstOrDefault(v => v.Color.Equals(color, StringComparison.CurrentCultureIgnoreCase)
                                                                && v.Size.Equals(size, StringComparison.CurrentCultureIgnoreCase));
                if (variant == null)
                {
                    variant = new ShopifyCreateProductVariantModel { Color = color, Size = size, Sku = sku};
                    var variantOption = new ShopifyCreateProductVariantOptionModel { Color = color, Size = size, Sku = sku };
                    product.Variants.Add(variant);
                    product.VariantOptions.Add(variantOption);

                    var colorOption = product.Options.FirstOrDefault(o => o.Name.Equals("Color"));
                    if (colorOption == null)
                    {
                        colorOption = new ShopifyOptionModel { Name = "Color" };
                        product.Options.Add(colorOption);
                    }
                    if (!colorOption.Values.Contains(color, StringComparer.CurrentCultureIgnoreCase))
                        colorOption.Values.Add(color);

                    var sizeOption = product.Options.FirstOrDefault(o => o.Name.Equals("Size"));
                    if (sizeOption == null)
                    {
                        sizeOption = new ShopifyOptionModel { Name = "Size" };
                        product.Options.Add(sizeOption);
                    }
                    if (!sizeOption.Values.Contains(size, StringComparer.CurrentCultureIgnoreCase))
                        sizeOption.Values.Add(size);
                }
                variant.Price = Math.Round(price, 2);
                variant.Taxable = true;

                var inventory = this.ProductInventory.FirstOrDefault(i => i.Key.Equals(sku, StringComparison.CurrentCultureIgnoreCase));
                if (string.IsNullOrEmpty(inventory.Key)) variant.InventoryQuantity = 0;
                else variant.InventoryQuantity = inventory.Value;

            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
            }
        }

        private void CreateMetafield(ShopifyCreateProductModel product, string key, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var metafield = new ShopifyCreateMetafieldModel { Namespace = "global", Key = key, Value = value, ValueType = "string" };
                    product.Metafields.Add(metafield);
                }
            }
            catch (Exception) { }
        }

        private Dictionary<string, int> GetProductInventory(string whereClause = "")
        {
            try
            {
                var inventory = new Dictionary<string, int>();
                var cmdText = "Select PartNumber, PartColor, Size01, Size02, Size03, Size04, Size05, Size06 FROM InvLevels " + whereClause;
                using (var cmd = new OdbcCommand(cmdText, this.FileMakerInventoryConnection))
                {
                    cmd.Connection.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            //LogReader(reader);
                            var partNumberArr = ConvertToString(reader["PartNumber"]).Split('_');
                            var partNumber = partNumberArr[0];
                            var color = ConvertToString(reader["PartColor"]);

                            if (string.IsNullOrEmpty(color)) color = "o/c";
                            var sizes = new List<Tuple<int, string>>();
                            if (ConvertToInt32(reader["Size01"]) > 0)
                                sizes.Add(new Tuple<int, string>(ConvertToInt32(reader["Size01"]), "s"));
                            if (ConvertToInt32(reader["Size02"]) > 0)
                                sizes.Add(new Tuple<int, string>(ConvertToInt32(reader["Size02"]), "m"));
                            if (ConvertToInt32(reader["Size03"]) > 0)
                                sizes.Add(new Tuple<int, string>(ConvertToInt32(reader["Size03"]), "l"));
                            if (ConvertToInt32(reader["Size04"]) > 0)
                                sizes.Add(new Tuple<int, string>(ConvertToInt32(reader["Size04"]), "xl"));
                            if (ConvertToInt32(reader["Size05"]) > 0)
                                sizes.Add(new Tuple<int, string>(ConvertToInt32(reader["Size05"]), "xxl"));
                            if (ConvertToInt32(reader["Size06"]) > 0)
                            {
                                var size = "o/s";
                                if (partNumberArr.Length > 1) size = partNumberArr[1];
                                sizes.Add(new Tuple<int, string>(ConvertToInt32(reader["Size06"]), size));
                            }

                            foreach (var size in sizes)
                            {
                                var sku = partNumber + "-" + color + "-" + size.Item2;
                                var _inventory = inventory.FirstOrDefault(i => i.Key.Equals(sku, StringComparison.CurrentCultureIgnoreCase));
                                if (string.IsNullOrEmpty(_inventory.Key)) inventory.Add(sku, size.Item1);
                            }
                        }

                        return inventory;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new Dictionary<string, int>();
            }
        }

        private Dictionary<int, string> GetShopWorksProductClasses()
        {
            try
            {
                var productClasses = new Dictionary<int, string>();
                var cmdText = "SELECT ID_ProductClass, ProductClass FROM ProdClass";
                using (var cmd = new OdbcCommand(cmdText, this.FileMakerCompanyConnection))
                {
                    cmd.Connection.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            //LogReader(reader);
                            if (reader["ID_ProductClass"] != DBNull.Value && reader["ProductClass"] != DBNull.Value)
                                productClasses.Add(Convert.ToInt32(reader["ID_ProductClass"]), Convert.ToString(reader["ProductClass"]));
                        }
                    }

                    return productClasses;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                throw ex;
            }
        }

        private List<string> GetProductColors(string colorBlockArray)
        {
            try
            {
                var idColorArray = colorBlockArray.Replace((char)13, ',').Replace("_", "").Replace(" ", "").Trim(',');
                if (string.IsNullOrEmpty(idColorArray))
                    return new List<string> { "o/c" };
                var colors = new List<string>();
                var cmdText = "SELECT ProductColors FROM ProdCol Where ID_ProductColors in (" + idColorArray + ")";
                using (var cmd = new OdbcCommand(cmdText, this.FileMakerCompanyConnection))
                {
                    cmd.Connection.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            //LogReader(reader);
                            colors.Add(ConvertToString(reader["ProductColors"]));
                        }
                    }

                    if (colors.Count == 0) colors.Add("o/c");
                    return colors;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetProductCmdText()
        {
            return "SELECT p.Description, t.FileName, p.ID_ProductSerial, p.PartNumber, p.ProductType, v.VendorName, " +
                          "p.import_Notes, t.ID_Serial, p.MtxPrice01, p.sts_AllowUpdate, v.CustomField09, p.CustomField01, " +
                          "p.CustomField02, pm.MtxPrice, p.MtxCost01, p.id_ProductClass, p.FindCode, p.PreprintGroup, " +
                          "p.cn_sts_LimitSize01Button, p.cn_sts_LimitSize02Button, p.cn_sts_LimitSize03Button, " +
                          "p.cn_sts_LimitSize04Button, p.cn_sts_LimitSize05Button, p.cn_sts_LimitSize06Button, p.id_ProductColorBlock " +
                   "FROM Prod As p " +
                   "LEFT JOIN Thumb As t on t.id_ProductSerial = p.ID_ProductSerial " +
                   "LEFT JOIN Ven As v on v.ID_Vendor = p.id_Vendor " +
                   "LEFT JOIN PriceMtx pm ON pm.id_Product = p.ID_ProductSerial ";
        }

        private static string ConvertToString(object value)
        {
            try
            {
                if (value == DBNull.Value) return "";
                return Convert.ToString(value).Trim();
            }
            catch (Exception) { return ""; }
        }

        private static decimal ConvertToDecimal(object value)
        {
            try
            {
                if (value == DBNull.Value) return 0.0m;
                return Convert.ToDecimal(value);
            }
            catch (Exception) { return 0.0m; }
        }

        private static int ConvertToInt32(object value)
        {
            try
            {
                if (value == DBNull.Value) return 0;
                return Convert.ToInt32(value);
            }
            catch (Exception) { return 0; }
        }

        private static void LogReader(OdbcDataReader reader)
        {
            try
            {
                var msg = "";
                for (int i = 0; i < reader.FieldCount; i++)
                    msg += "[" + reader.GetName(i) + ": " + Convert.ToString(reader[i]) + "]" + Environment.NewLine;
                Console.WriteLine(msg);
                Console.WriteLine("----------------------------------------------------------------------------------");
            }
            catch (Exception) { }
        }
    }
}