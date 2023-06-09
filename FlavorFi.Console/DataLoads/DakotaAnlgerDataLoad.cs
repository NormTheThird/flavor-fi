using System;
using FlavorFi.Common.Models.ShopifyModels;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Services.ShopifyServices;
using FlavorFi.Common.Enums;
using System.Net;
using HtmlAgilityPack;

namespace FlavorFi.Console.DataLoads
{
    public class DakotaAnlgerDataLoad : BaseDataLoad
    {
        public DataTable InventoryDataTable { get; private set; }
        public DataTable ProductsDataTable { get; private set; }
        public List<ShopifyCustomCollectionModel> CustomCollections { get; private set; }
        public List<ShopifyCreateProductModel> Products { get; private set; }

        public DakotaAnlgerDataLoad(string userToken, Guid companySiteId) : base(userToken, companySiteId) { }

        public void LoadProducts()
        {
            try
            {
                this.ConsoleLog("Uploading dakota angler file", true);
                var baseService = new ShopifyBaseService(this.CompanySiteId);
                var inventoryPath = @"E:\Dropbox\Flavor Mob Share\Dakota Angler\inventory.txt";
                if (!File.Exists(inventoryPath)) this.ConsoleLog("No file exists [" + inventoryPath + "]");
                var productPath = @"E:\Dropbox\Flavor Mob Share\Dakota Angler\products.txt";
                if (!File.Exists(productPath)) this.ConsoleLog("No file exists [" + productPath + "]");
                if (!File.Exists(inventoryPath) || !File.Exists(productPath)) return;

                this.ConsoleLog("Creating inventory datatable from file");
                this.InventoryDataTable = new DataTable();
                var inventorySeperator = ",";
                foreach (string line in File.ReadLines(inventoryPath))
                {
                    try
                    {
                        var modifiedLine = line;
                        if (modifiedLine.StartsWith("SKU")) this.InventoryDataTable = this.CreateDataTable(modifiedLine, inventorySeperator);
                        else
                        {
                            if (modifiedLine.Substring(0, 1).Equals("0"))
                                modifiedLine = modifiedLine.Substring(1, modifiedLine.Length - 1);
                            if (modifiedLine.StartsWith(",")) modifiedLine = "Sku-" + Guid.NewGuid().ToString().Substring(0, 8) + modifiedLine;
                            this.InventoryDataTable.Rows.Add(modifiedLine.Trim(',').Split(inventorySeperator.ToCharArray()));
                        }
                    }
                    catch (Exception ex) { System.Console.WriteLine(ex.Message); return; }
                }

                this.ConsoleLog("Creating products datatable from file");
                this.ProductsDataTable = new DataTable();
                var productsSeperator = "\t";
                foreach (string line in File.ReadLines(productPath))
                {
                    try
                    {
                        var modifiedLine = line;
                        if (modifiedLine.StartsWith("Item ID")) this.ProductsDataTable = this.CreateDataTable(modifiedLine, productsSeperator);
                        else this.ProductsDataTable.Rows.Add(modifiedLine.Trim(',').Split(productsSeperator.ToCharArray()));
                    }
                    catch (Exception ex) { System.Console.WriteLine(ex.Message); return; }
                }

                this.CustomCollections = this.GetCustomCollectionList();
                this.ConsoleLog("Reading " + this.ProductsDataTable.Rows.Count + " records from products file.");
                this.Products = new List<ShopifyCreateProductModel>();
                var rowCount = this.ProductsDataTable.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow row = this.ProductsDataTable.Rows[i];
                    this.CreateModel(row);
                    if (i % 500 == 0 && i != 0)
                        this.ConsoleLog("Created " + i.ToString() + " of " + rowCount.ToString());
                }

                this.ConsoleLog("Inserting " + this.Products.Count.ToString() + " products into shopify");
                foreach (var product in this.Products)
                    this.InsertToShopify(product);

                this.ConsoleLog("Finished uploading dakota angler file", true);
                this.WriteFile("DakotaAngler_LoadProduct");
            }
            catch (Exception ex)
            {
                this.ConsoleLog(ex.Message, true);
                System.Console.ReadKey();
            }
        }

        private List<ShopifyCustomCollectionModel> GetCustomCollectionList()
        {
            try
            {
                this.ConsoleLog("Getting custom collections from shopify.", true);
                var getShopifyRecordsRequest = new GetShopifyRecordsRequest
                {
                    CompanySiteId = this.CompanySiteId,
                    ResourceType = ShopifyResourceType.custom_collections,
                    NumberPerPage = 250
                };
                var getShopifyCustomCollectionResponse = ShopifyCustomCollectionService.GetShopifyCustomCollections(getShopifyRecordsRequest);
                if (!getShopifyCustomCollectionResponse.IsSuccess) throw new ApplicationException(getShopifyCustomCollectionResponse.ErrorMessage);
                this.ConsoleLog(getShopifyCustomCollectionResponse.Collections.Count.ToString() + " custom collections received from shopify", true);
                return getShopifyCustomCollectionResponse.Collections;
            }
            catch (Exception ex) { throw ex; }
        }

        private DataTable CreateDataTable(string headerRow, string split)
        {
            try
            {
                var dt = new DataTable();
                var columns = headerRow.Split(split.ToCharArray());
                foreach (var column in columns)
                    dt.Columns.Add(column, typeof(string));
                return dt;
            }
            catch (Exception ex) { throw ex; }
        }

        private void CreateModel(DataRow row)
        {
            try
            {
                if (string.IsNullOrEmpty(row["GTIN1"].ToString()))
                    return;
                var sku = Convert.ToInt64(row["GTIN1"]).ToString();
                DataRow inventoryRow = this.GetInventory(sku);
                if (inventoryRow == null)
                    return;
               
                var extId = Convert.ToInt64(row["Item ID"]);
                var product = this.Products.FirstOrDefault(p => p.ExternalId.Equals(extId));
                if (product == null)
                {
                    product = new ShopifyCreateProductModel
                    {
                        Id = 0,
                        ExternalId = extId,
                        Title = this.CleanString(Convert.ToString(row["Model Name"])),
                        BodyHtml = "",
                        Vendor = Convert.ToString(row["Brand"]),
                        ProductType = ""
                    };
                    this.AddWebsiteInfo(product);
                    this.Products.Add(product);

                    var costMetafield = product.Metafields.FirstOrDefault(m => m.Key.Equals("cost_price"));
                    if (costMetafield == null)
                    {
                        product.Metafields.Add(new ShopifyCreateMetafieldModel
                        {
                            Namespace = "global",
                            Key = "cost_price",
                            Value = Convert.ToString(inventoryRow.Field<string>("cost")),
                            ValueType = "string"
                        });
                    }

                    var categories = Convert.ToString(row["Category Name"]).Split('>');
                    if (categories.Length > 1)
                    {
                        var categoryMetafield = product.Metafields.FirstOrDefault(m => m.Key.Equals("category"));
                        if (categoryMetafield == null)
                        {
                            product.Metafields.Add(new ShopifyCreateMetafieldModel
                            {
                                Namespace = "global",
                                Key = "category",
                                Value = categories[1].Trim(),
                                ValueType = "string"
                            });
                        }
                    }

                    if (categories.Length > 2)
                    {
                        var subCategoryMetafield = product.Metafields.FirstOrDefault(m => m.Key.Equals("sub_category"));
                        if (subCategoryMetafield == null)
                        {
                            product.Metafields.Add(new ShopifyCreateMetafieldModel
                            {
                                Namespace = "global",
                                Key = "sub_category",
                                Value = categories[2].Trim(),
                                ValueType = "string"
                            });
                        }
                    }
                }

                var partNumber = Convert.ToString(row["MPN"]);
                var option = product.Options.FirstOrDefault(o => o.Name == "Part Number");
                if (option == null)
                {
                    option = new ShopifyOptionModel { Name = "Part Number" };
                    product.Options.Add(option);
                }
                if (!option.Values.Contains(partNumber)) option.Values.Add(partNumber);

                var varId = Convert.ToInt64(row["Variation ID"]);
                var variant = product.Variants.FirstOrDefault(v => v.ExternalId.Equals(varId));
                if (variant == null)
                {
                    variant = new ShopifyCreateProductVariantModel
                    {
                        Id = 0,
                        ExternalId = varId,
                        Size = varId.ToString(),
                        Price = Convert.ToDecimal(row["Price"]),
                        InventoryManagement = "shopify",
                        InventoryPolicy = "deny",
                        InventoryQuantity = Convert.ToInt32(inventoryRow.Field<string>("Onhand New")),
                        Sku = sku,
                        Barcode = sku,
                        Taxable = true,
                    };
                    product.Variants.Add(variant);
                    product.VariantOptions.Add(new ShopifyCreateProductVariantOptionModel { Sku = variant.Sku, Size = varId.ToString() });
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private DataRow GetInventory(string sku)
        {
            try
            {
                var inventory = this.InventoryDataTable.AsEnumerable().FirstOrDefault(r => r.Field<string>("SKU").Equals(sku));
                if (inventory == null) throw new ApplicationException("No inventory found for " + sku);
                return inventory;
            }
            catch (Exception ex)
            {
                //this.ConsoleLog("Unable to get inventory quantity: " + ex.Message);
                return null;
            }
        }

        private void AddWebsiteInfo(ShopifyCreateProductModel product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.Title)) return;
                var part = product.Title + "-" + product.ExternalId.ToString();
                var url = @"http://www.dakotaangler.com/product/dakota-angler-" + part.ToLower() + ".htm";

                var doc = new HtmlDocument();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var response = WebRequest.Create(url).GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream())) { doc.Load(sr); }

                try
                {
                    var descNode = doc.DocumentNode.SelectNodes("//div[@class='seitemdesc']").FirstOrDefault();
                    if (descNode != null) product.BodyHtml = descNode.InnerText.Trim();
                }
                catch (Exception ex)
                {
                    this.ConsoleLog(product.Title + " : " + ex.Message);
                }

                try
                {
                    var imageNodes = doc.DocumentNode.SelectNodes("//ul[@class='seitemimagecarousel-container touchcarousel-container']");
                    if (imageNodes == null)
                    {
                        var imageNode = doc.DocumentNode.SelectNodes("//div[@class='seitemdetailpicture']").FirstOrDefault();
                        if (imageNode != null)
                        {
                            var path = "";
                            if (imageNode.ChildNodes[1].Name.Equals("img"))
                                path = imageNode.ChildNodes[1].Attributes["src"].Value;
                            else
                                path = imageNode.ChildNodes[1].FirstChild.Attributes["src"].Value;
                            var image = this.GetImage(path);
                            if (!string.IsNullOrEmpty(image))
                                product.Images.Add(new ShopifyProductImageModel { Attachment = image });
                        }
                    }
                    else
                    {
                        foreach (var node in imageNodes[0].ChildNodes)
                        {
                            if (node.Name.Equals("li"))
                            {
                                var image = this.GetImage(node.ChildNodes[1].Attributes["src"].Value);
                                if (!string.IsNullOrEmpty(image))
                                    product.Images.Add(new ShopifyProductImageModel { Attachment = image });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.ConsoleLog(product.Title + " : " + ex.Message);
                }

            }
            catch (Exception ex)
            {
                this.ConsoleLog(product.Title + " : " + ex.Message);
            }
        }

        private string GetImage(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return null;
                path = path.Replace("micro", "large");

                var url = @"http://www.dakotaangler.com/synd" + path;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (WebClient client = new WebClient())
                    return Convert.ToBase64String(client.DownloadData(url));
            }
            catch (Exception ex)
            {
                this.ConsoleLog(ex.Message);
                return null;
            }
        }

        private string CleanString(string value)
        {
            try
            {
                var str = value;
                str = str.Replace("'", "");
                str = str.Replace("\"", "");
                str = str.Replace("&", "");
                str = str.Replace(";", "");
                str = str.Replace(":", "");
                str = str.Replace("/", "");
                str = str.Replace("?", "");
                str = str.Replace("!", "");
                str = str.Replace(",", "");
                str = str.Replace(")", "");
                str = str.Replace("(", "");
                str = str.Replace("�", "");
                str = str.Replace(" ", "-");
                str = str.Replace("--", "-");
                return str.Trim();
            }
            catch (Exception ex)
            {
                this.ConsoleLog("Unable to clean string [Value: " + value + "][Error: " + ex.Message);
                return null;
            }
        }

        private void InsertToShopify(ShopifyCreateProductModel product)
        {
            try
            {
                var getProductRequest = new GetShopifyRecordByQueryRequest { CompanySiteId = this.CompanySiteId };
                getProductRequest.SearchFields.Add("title", product.Title);
                var getProductResponse = ShopifyProductService.GetShopifyProductByQuery(getProductRequest);
                if (getProductResponse.IsSuccess && getProductResponse.Product != null)
                {
                    product.Id = getProductResponse.Product.Id;
                }

                var saveShopifyProductRequest = new SaveShopifyProductRequest { Product = product, CompanySiteId = this.CompanySiteId };
                var saveShopifyProductResponse = ShopifyProductService.SaveShopifyProduct(saveShopifyProductRequest);
                if (!saveShopifyProductResponse.IsSuccess)
                    this.ConsoleLog("Error: [Product: " + product.Title + "][Product Error: " + saveShopifyProductResponse.ErrorMessage + "]");
                else
                {
                    this.ConsoleLog("Product uploaded [Product: " + product.Title + "]");
                    product.Id = saveShopifyProductResponse.Product.Id;
                    foreach (var variant in product.Variants)
                    {
                        var _variant = saveShopifyProductResponse.Product.Variants.FirstOrDefault(v => v.Sku.Equals(variant.Sku, StringComparison.CurrentCultureIgnoreCase));
                        if (_variant != null) variant.Id = _variant.Id;
                        var saveShopifyProductVariantRequest = new SaveShopifyProductVariantRequest { ProductId = product.Id, ProductVariant = variant, CompanySiteId = this.CompanySiteId };
                        var saveShopifyProductVariantResponse = ShopifyProductService.SaveShopifyProductVariant(saveShopifyProductVariantRequest);
                        if (saveShopifyProductResponse.IsSuccess) this.ConsoleLog("-- Product variant " + variant.Sku + " saved successfully");
                        else this.ConsoleLog("-- Product variant " + variant.Sku + " error: " + saveShopifyProductVariantResponse.ErrorMessage);
                    }

                    foreach (var metafield in product.Metafields)
                    {
                        var saveShopifyProductMetafieldRequest = new SaveShopifyProductMetafieldRequest { ProductId = product.Id, ProductMetafield = metafield, CompanySiteId = this.CompanySiteId };
                        var saveShopifyProductMetafieldResponse = ShopifyProductService.SaveShopifyProductMetafield(saveShopifyProductMetafieldRequest);
                        if (saveShopifyProductMetafieldResponse.IsSuccess) this.ConsoleLog("-- Product metafield " + metafield.Key + " : " + metafield.Value + " saved successfully");
                        else this.ConsoleLog("-- Product metafield " + metafield.Key + " : " + metafield.Value + " error: " + saveShopifyProductMetafieldResponse.ErrorMessage);

                        if (metafield.Key.Equals("category"))
                            this.SaveCollection(product, metafield.Value);

                        if (metafield.Key.Equals("sub_category"))
                            this.SaveCollection(product, metafield.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private void SaveCollection(ShopifyCreateProductModel product, string category)
        {
            try
            {
                if (string.IsNullOrEmpty(category)) this.ConsoleLog("-- SubCategory value is null or empty");
                var collection = CustomCollections.FirstOrDefault(c => c.Title.Equals(category, StringComparison.CurrentCultureIgnoreCase));
                if (collection == null)
                {
                    var customCollection = new ShopifyCreateCustomCollectionModel { Title = category };
                    var saveCustomCollectionRequest = new SaveShopifyCustomCollectionRequest { Collection = customCollection, CompanySiteId = this.CompanySiteId };
                    var createCollectionResponse = ShopifyCustomCollectionService.SaveShopifyCustomCollection(saveCustomCollectionRequest);
                    if (!createCollectionResponse.IsSuccess) throw new ApplicationException("[CustomCollection: " + category + "][CustomCollection Error: " + createCollectionResponse.ErrorMessage + "]");

                    this.ConsoleLog("-- Category " + category + " was added to custom collection");
                    CustomCollections.Add(createCollectionResponse.Collection);
                    collection = createCollectionResponse.Collection;
                }
                this.SaveCollect(collection.Id, product.Id);
            }
            catch (Exception ex) { this.ConsoleLog("-- Save collection error [Error: " + ex.Message + "]"); }
        }

        private void SaveCollect(long collectionId, long productId)
        {
            try
            {
                var getCollectsRequest = new GetShopifyRecordByQueryRequest { CompanySiteId = this.CompanySiteId };
                getCollectsRequest.SearchFields.Add("product_id", productId.ToString());
                var getCollectsResponse = ShopifyCustomCollectionService.GetShopifyCollects(getCollectsRequest);
                if (!getCollectsResponse.IsSuccess) throw new ApplicationException("-- Get Collects Error [Error: " + getCollectsResponse.ErrorMessage + "]");
                else if (getCollectsResponse.Collects != null)
                    if (getCollectsResponse.Collects.FirstOrDefault(c => c.CollectionId == collectionId) != null) return;

                var saveCollectRequest = new SaveShopifyCollectRequest { CollectionId = collectionId, ProductId = productId, CompanySiteId = this.CompanySiteId };
                var saveCollectResponse = ShopifyCustomCollectionService.SaveShopifyCollect(saveCollectRequest);
                if (!saveCollectResponse.IsSuccess)
                    throw new ApplicationException("[CollectionId: " + collectionId.ToString() + "][ProductId: " + productId.ToString() + "][Error: " + saveCollectResponse.ErrorMessage + "]");
                this.ConsoleLog("-- Product was saved to collection");
            }
            catch (Exception ex) { this.ConsoleLog("-- Save collect error [Error: " + ex.Message + "]"); }
        }
    }
}