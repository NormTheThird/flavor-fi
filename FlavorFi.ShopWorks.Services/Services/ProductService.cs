using FlavorFi.Common.Enums;
using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses;
using FlavorFi.Common.Extensions;
using FlavorFi.Services.ShopifyServices;
using FlavorFi.Services.ShopWorksServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavorFi.ShopWorks.Services
{
    public class ProductService : BaseService
    {
        private List<ShopifyCustomCollectionModel> CustomCollections { get; set; }
        private ShopWorksProductService ShopWorksProductService { get; set; }

        public ProductService(Guid companySiteId) : base(companySiteId)
        {
            this.ShopWorksProductService = new ShopWorksProductService(this.CompanySite.CompanyId);
        }

        public void SyncProducts()
        {
            try
            {
                this.CustomCollections = this.GetCustomCollectionList();
                this.ConsoleLog("Getting products from shopworks", true);
                var getShopWorksProductsResponse = this.ShopWorksProductService.GetShopWorksProducts(new GetShopWorksProductsRequest());
                if (!getShopWorksProductsResponse.IsSuccess) throw new ApplicationException("[Get Products Error: " + getShopWorksProductsResponse.ErrorMessage + "]");

                this.ConsoleLog("Adding / updating " + getShopWorksProductsResponse.Products.Count + " products from shopworks", true);
                foreach (var product in getShopWorksProductsResponse.Products)
                    this.AddOrEditProduct(product);
                this.ConsoleLog("Finished syncing products from shopworks", true);
                this.WriteFile("ShopWorks_SyncProducts");
            }
            catch (Exception ex)
            {
                this.ConsoleLog(ex.Message);
                this.WriteFile("ShopWorks_SyncProducts");
                throw ex;
            }
        }

        public void SyncProduct(string partNumber)
        {
            try
            {
                this.CustomCollections = this.GetCustomCollectionList();
                if (string.IsNullOrEmpty(partNumber)) throw new ApplicationException("Part number is empty");
                this.ConsoleLog("Getting product " + partNumber + " from shopworks", true);
                var getShopWorksProductResponse = this.ShopWorksProductService.GetShopWorksProduct(new GetShopWorksProductRequest { PartNumber = partNumber });
                if (!getShopWorksProductResponse.IsSuccess) throw new ApplicationException("[Get Product Error: " + getShopWorksProductResponse.ErrorMessage + "]");

                foreach (var product in getShopWorksProductResponse.Products)
                    this.AddOrEditProduct(product);
                this.ConsoleLog("Finished syncing product from shopworks", true);
                this.WriteFile("ShopWorks_SyncProduct");
            }
            catch (Exception ex)
            {
                this.ConsoleLog(ex.Message);
                throw ex;
            }
        }


        private List<ShopifyCustomCollectionModel> GetCustomCollectionList()
        {
            try
            {
                this.ConsoleLog("Getting custom collections from shopify.", true);
                var getShopifyRecordsRequest = new GetShopifyRecordsRequest
                {
                    CompanySiteId = this.CompanySite.Id,
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

        private void AddOrEditProduct(ShopifyCreateProductModel product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.Title)) this.LogProduct(product, "Product title does not exist");
                else
                {
                    var getProductRequest = new GetShopifyRecordByQueryRequest { CompanySiteId = this.CompanySite.Id };
                    getProductRequest.SearchFields.Add("title", product.Title);
                    var getProductResponse = ShopifyProductService.GetShopifyProductByQuery(getProductRequest);
                    if (getProductResponse.IsSuccess && getProductResponse.Product != null)
                    {
                        product.Id = getProductResponse.Product.Id;
                        var currentTags = new List<string>();
                        foreach (var tag in product.Tags.Split(',').ToList())
                            if (!currentTags.Contains(tag.Trim(), StringComparer.CurrentCultureIgnoreCase))
                                if (!string.IsNullOrEmpty(tag))
                                    currentTags.Add(tag);                        
                        foreach (var tag in getProductResponse.Product.Tags.Split(',').ToList())
                            if (!currentTags.Contains(tag.Trim(), StringComparer.CurrentCultureIgnoreCase))
                                if (!string.IsNullOrEmpty(tag))
                                    currentTags.Add(tag);
                        foreach (var option in product.Options)
                            foreach (var value in option.Values)
                                if (!currentTags.Contains(value.Trim(), StringComparer.CurrentCultureIgnoreCase))
                                    if (!string.IsNullOrEmpty(value.Trim()))
                                        currentTags.Add(value.Trim());
                        foreach (var metafield in product.Metafields)
                        {
                            if (metafield.Key.Contains("price") || metafield.Key.Contains("days")) continue;
                            else if (!currentTags.Contains(metafield.Value.Trim(), StringComparer.CurrentCultureIgnoreCase))
                                if (!string.IsNullOrEmpty(metafield.Value.Trim()))
                                    currentTags.Add(metafield.Value.Trim());
                        }
                        product.Tags = string.Join(",", currentTags.Select(t => t)).Trim().Trim(',');
                    }

                    var saveShopifyProductRequest = new SaveShopifyProductRequest { Product = product, CompanySiteId = this.CompanySite.Id };
                    var saveShopifyProductResponse = ShopifyProductService.SaveShopifyProduct(saveShopifyProductRequest);
                    if (!saveShopifyProductResponse.IsSuccess) this.LogProduct(product, saveShopifyProductResponse.ErrorMessage, true);
                    else
                    {
                        product.Id = saveShopifyProductResponse.Product.Id;
                        this.LogProduct(product, "Success");
                        foreach (var variant in product.Variants)
                        {
                            var _variant = saveShopifyProductResponse.Product.Variants.FirstOrDefault(v => v.Sku.Equals(variant.Sku, StringComparison.CurrentCultureIgnoreCase));
                            if (_variant != null) variant.Id = _variant.Id;
                            var saveShopifyProductVariantRequest = new SaveShopifyProductVariantRequest { ProductId = product.Id, ProductVariant = variant, CompanySiteId = this.CompanySite.Id };
                            var saveShopifyProductVariantResponse = ShopifyProductService.SaveShopifyProductVariant(saveShopifyProductVariantRequest);
                            if (saveShopifyProductVariantResponse.IsSuccess) this.ConsoleLog("-- Product variant " + variant.Sku + " saved successfully");
                            else this.ConsoleLog("-- Product variant " + variant.Sku + " error: " + saveShopifyProductVariantResponse.ErrorMessage);
                        }

                        foreach (var metafield in product.Metafields)
                        {
                            var saveShopifyProductMetafieldRequest = new SaveShopifyProductMetafieldRequest { ProductId = product.Id, ProductMetafield = metafield, CompanySiteId = this.CompanySite.Id };
                            var saveShopifyProductMetafieldResponse = ShopifyProductService.SaveShopifyProductMetafield(saveShopifyProductMetafieldRequest);
                            if (saveShopifyProductMetafieldResponse.IsSuccess) this.ConsoleLog("-- Product metafield " + metafield.Key + " : " + metafield.Value + " saved successfully");
                            else this.ConsoleLog("-- Product metafield " + metafield.Key + " : " + metafield.Value + " error: " + saveShopifyProductMetafieldResponse.ErrorMessage);

                            if (metafield.Key.Equals("sub_category")) this.SaveCollection(product, metafield.Value);
                            if (metafield.Key.Equals("category"))
                            {
                                // Equipment Any item that has EQUIPMENT in the PRODUCT CLASS field
                                // Footwear Any item that has FOOTWEAR in the PRODUCT CLASS field
                                // Apparel Any item that has APPAREL in the PRODUCT CLASS field
                                this.SaveCollection(product, metafield.Value);
                            }

                            // Team Stores Any item that has a value in the PREPRINT GROUP field(these will be the individual web stores we set up for high schools, etc)
                            if (metafield.Key.Equals("preprint_group"))
                                this.SaveCollection(product, metafield.Value);

                            // Brands This will be taken from the information in the PART NUMBER field in PRODUCT file or the information in the CUSTOM field #10 IN VENDOR FILE
                            // NOT SURE WHAT THIS MEANS

                            // Clearance if FIND CODE field in product file = CLEARANCE
                            if (metafield.Key.Equals("find_code"))
                                this.SaveCollection(product, metafield.Value.Trim());

                            if (metafield.Key.Equals("gender"))
                            {
                                // Men Any item that has ADULT in the GENDER field
                                if (metafield.Value.Trim().Equals("Adult", StringComparison.CurrentCultureIgnoreCase))
                                    this.SaveCollection(product, "Men");
                                // Ladies Any item that has LADIES in the GENDER field
                                if (metafield.Value.Trim().Equals("Ladies", StringComparison.CurrentCultureIgnoreCase))
                                    this.SaveCollection(product, "Ladies");
                                // Youth Any item that has YOUTH in the GENDER field
                                if (metafield.Value.Trim().Equals("Youth", StringComparison.CurrentCultureIgnoreCase))
                                    this.SaveCollection(product, "Youth");
                            }

                        }
                    }
                }
            }
            catch (Exception ex) { this.ConsoleLog("[AddOrEditProduct][Error: " + ex.Message + "]"); }
        }

        private void SaveCollection(ShopifyCreateProductModel product, string category)
        {
            try
            {
                if (string.IsNullOrEmpty(category)) this.ConsoleLog("-- Category value is null or empty");
                var collection = CustomCollections.FirstOrDefault(c => c.Title.Equals(category, StringComparison.CurrentCultureIgnoreCase));
                if (collection == null)
                {
                    var customCollection = new ShopifyCreateCustomCollectionModel { Title = category };
                    var saveCustomCollectionRequest = new SaveShopifyCustomCollectionRequest { Collection = customCollection, CompanySiteId = this.CompanySite.Id };
                    var createCollectionResponse = ShopifyCustomCollectionService.SaveShopifyCustomCollection(saveCustomCollectionRequest);
                    if (!createCollectionResponse.IsSuccess) throw new ApplicationException("[CustomCollection: " + category + "][CustomCollection Error: " + createCollectionResponse.ErrorMessage + "]");

                    this.ConsoleLog("-- Category " + category + " was added to custom collection");
                    CustomCollections.Add(createCollectionResponse.Collection);
                    collection = createCollectionResponse.Collection;
                }
                this.SaveCollect(collection.Id, product.Id, category);
            }
            catch (Exception ex) { this.ConsoleLog("-- Save collection error [Error: " + ex.Message + "]"); }
        }

        private void SaveCollect(long collectionId, long productId, string category)
        {
            try
            {
                var getCollectsRequest = new GetShopifyRecordByQueryRequest { CompanySiteId = this.CompanySite.Id };
                getCollectsRequest.SearchFields.Add("product_id", productId.ToString());
                var getCollectsResponse = ShopifyCustomCollectionService.GetShopifyCollects(getCollectsRequest);
                if (!getCollectsResponse.IsSuccess) throw new ApplicationException("-- Get Collects Error [Error: " + getCollectsResponse.ErrorMessage + "]");
                else if (getCollectsResponse.Collects != null)
                    if (getCollectsResponse.Collects.FirstOrDefault(c => c.CollectionId == collectionId) != null) return;

                var saveCollectRequest = new SaveShopifyCollectRequest { CollectionId = collectionId, ProductId = productId, CompanySiteId = this.CompanySite.Id };
                var saveCollectResponse = ShopifyCustomCollectionService.SaveShopifyCollect(saveCollectRequest);
                if (!saveCollectResponse.IsSuccess)
                    throw new ApplicationException("[CollectionId: " + collectionId.ToString() + "][ProductId: " + productId.ToString() + "][Error: " + saveCollectResponse.ErrorMessage + "]");
                this.ConsoleLog("-- Product was saved to collection " + category);
            }
            catch (Exception ex) { this.ConsoleLog("-- Save collect error [Error: " + ex.Message + "]"); }
        }

        private void LogProduct(ShopifyCreateProductModel product, string message, bool isError = false)
        {
            try
            {
                var msg = "";
                if (!string.IsNullOrEmpty(product.PartNumber)) msg += "[Part Number: " + product.PartNumber + "]";
                if (!string.IsNullOrEmpty(product.Title)) msg += "[Title: " + product.Title + "]";
                if (isError) this.ConsoleLog(msg + "[Product Error: " + message + "]");
                else this.ConsoleLog(msg + "[" + message + "]");
            }
            catch (Exception) { }
        }
    }
}