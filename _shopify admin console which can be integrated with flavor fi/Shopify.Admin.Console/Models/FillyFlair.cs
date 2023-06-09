using Newtonsoft.Json.Linq;
using Shopify.Admin.Console.Data;
using Shopify.Admin.Console.Enums;
using Shopify.Admin.Console.RequestAndResponses;
using Shopify.Admin.Console.Services;
using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Shopify.Admin.Console.Models
{
    public class FillyFlair
    {
        public CustomerService CustomerService { get; set; }
        public ProductService ProductService { get; set; }
        public ProductVariantService ProductVariantService { get; set; }
        public CustomCollectionService CustomCollectionService { get; set; }
        public MetaFieldService MetaFieldService { get; set; }
        public CollectService CollectService { get; set; }
        public OrderService OrderService { get; set; }
        public RedirectService RedirectService { get; set; }

        public List<ShopifyCustomCollection> ShopifyCollections { get; set; }
        public List<eav_attribute_option_value> AttributeOptions { get; set; }
        public List<catalog_category_entity_varchar> Categories { get; set; }

        public List<ShopifyCustomer> ShopifyCustomers { get; set; }

        public FillyFlair(string url, string password)
        {
            // Create new data loads
            this.CustomerService = new CustomerService(url, password);
            this.ProductService = new ProductService(url, password);
            this.ProductVariantService = new ProductVariantService(url, password);
            this.CustomCollectionService = new CustomCollectionService(url, password);
            this.MetaFieldService = new MetaFieldService(url, password);
            this.CollectService = new CollectService(url, password);
            this.OrderService = new OrderService(url, password);
            this.RedirectService = new RedirectService(url, password);
        }

        public void LoadOnline()
        {
            //var count = this.ProductService.GetNumberOfProducts();

            //this.UpdateMetaField();

            //this.ListAllProducts();
            //this.ProductDL.DeleteAllProducts();
            //this.UploadNewProducts();

            //this.AddRedirects();
            //dataLoad.ListAllProducts();
            //dataLoad.ListAllCustomers();

            //var count = this.CustomerService.GetActiveCustomerCount();

            //this.UpdateAddCollections();
            //this.UploadNewCustomers();
            //this.UploadNewOrders();

            //this.DeleteOrders();

            this.CreateGiftCards();
        }

        public void LoadStore()
        {
            //var count = this.ProductService.GetNumberOfProducts();

            //this.ListAllProducts();
            //this.ProductDL.DeleteAllProducts();
            //this.UploadNewProducts(true);

            //this.AddRedirects();
            //dataLoad.ListAllProducts();
            //dataLoad.ListAllCustomers();

            //this.UploadNewCustomers();
            //this.UploadNewOrders();

            //this.DeleteOrders();
        }

        /// <summary>
        /// Gets a list of all products
        /// </summary>
        public void ListAllProducts()
        {
            try
            {
                foreach (var product in this.ProductService.GetProducts())
                {
                    var productString = "";
                    productString += "Id: " + product.Id.ToString();
                    productString += " Title: " + product.Title;
                    productString += " ProductType: " + product.ProductType;
                    System.Console.WriteLine(productString);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Uploads the new products to shopify
        /// </summary>
        public void UploadNewProducts(bool _isStore = false)
        {
            using (var context = new FillyFlairEntities())
            {
                this.ShopifyCollections = this.CustomCollectionService.GetCustomCollections();
                this.AttributeOptions = context.eav_attribute_option_value.AsNoTracking().ToList();
                this.Categories = context.catalog_category_entity_varchar.AsNoTracking().ToList();
                var parentProductIds = context.catalog_product_relation.AsNoTracking().GroupBy(p => p.parent_id).Select(g => g.FirstOrDefault().parent_id).ToList();
                var parentProducts = context.catalog_product_entity.Where(e => parentProductIds.Contains(e.entity_id)).ToList();
                System.Console.WriteLine();
                System.Console.WriteLine("Adding " + parentProducts.Count.ToString() + " new parent products: " + DateTime.Now.ToLongTimeString());
                var counter = 0;
                foreach (var parent in parentProducts.OrderBy(p => p.created_at))
                {
                    // Add or update product
                    var product = this.AddOrUpdateProduct(parent, _isStore);
                    if (product == null) continue;

                    // Add or update variants
                    var listOfChild = context.catalog_product_relation.AsNoTracking().Where(r => r.parent_id == parent.entity_id).ToList();
                    foreach (var child in listOfChild)
                    {
                        var childProduct = context.catalog_product_entity.FirstOrDefault(e => e.entity_id == child.child_id);
                        product = this.AddOrUpdateVariants(childProduct, product, parent, _isStore);
                    }

                    try
                    {
                        // Clean up
                        if (product.Variants == null) continue;
                        if (product.Variants.Count() == 0)
                        {
                            if (_isStore) continue;
                            product.Variants = null;
                        }

                        ShopifyProduct updatedProduct = null;
                        if (product.Id == null) updatedProduct = this.ProductService.AddProduct(product);
                        else
                        {
                            updatedProduct = this.ProductService.UpdateProduct(product);
                            foreach (var variant in product.Variants)
                                this.ProductVariantService.UpdateProductVariant(variant);
                        }

                        if (product.Metafields == null) throw new ApplicationException("Metafields are null.");
                        var metaField = product.Metafields.FirstOrDefault(m => m.Key == "mMagentoId");
                        if (metaField == null) throw new ApplicationException("mMagentoId metafield not found for new item. Could not create collect.");
                        long entityId = 0;
                        if (metaField.Value != null) entityId = Convert.ToInt64(metaField.Value);
                        var entity = context.catalog_product_entity.FirstOrDefault(e => e.entity_id == entityId);
                        if (entity == null) throw new ApplicationException("Could not find entity for " + metaField.Value + ". Could not create collect.");
                        this.CreateProductCollects(entity, (long)updatedProduct.Id, product.ProductType);
                        counter++;
                        if (counter % 10 == 0) System.Console.WriteLine(counter.ToString() + " products have been uploaded: " + DateTime.Now.ToLongTimeString());
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error uploading or adding product " + parent.entity_id.ToString());
                        if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                        else System.Console.WriteLine("Error Message: " + ex.Message);

                    }
                }

            }
        }

        public void UpdateAddCollections(bool _isStore = false)
        {
            using (var context = new FillyFlairEntities())
            {
                var parentProductIds = context.catalog_product_relation.AsNoTracking().GroupBy(p => p.parent_id).Select(g => g.FirstOrDefault().parent_id).ToList();
                var parentProducts = context.catalog_product_entity.Where(e => parentProductIds.Contains(e.entity_id)).ToList();
                System.Console.WriteLine();
                System.Console.WriteLine("Adding or updating " + parentProducts.Count.ToString() + " collections: " + DateTime.Now.ToLongTimeString());
                var counter = 0;
                foreach (var parent in parentProducts.OrderBy(p => p.created_at))
                {
                    try
                    {
                        var name = this.GetVarcharAttribute(parent, 71);
                        var products = this.ProductService.GetProductByTitle(name);
                        if (products.Count == 0) continue;
                        var product = products.FirstOrDefault(p => p.Title.Equals(name, StringComparison.CurrentCultureIgnoreCase));
                        if (product == null) continue;
                        if (product.Id == null) continue;
                        this.CreateProductCollects(parent, (long)product.Id, product.ProductType);
                        counter++;
                        if (counter % 10 == 0) System.Console.WriteLine(counter.ToString() + " collections have been updated: " + DateTime.Now.ToLongTimeString());
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error uploading or adding collections " + parent.entity_id.ToString());
                        if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                        else System.Console.WriteLine("Error Message: " + ex.Message);

                    }
                }

            }
        }

        public void UpdateMetaField()
        {
            try
            {
                var product = this.ProductService.GetProduct(10079760591);
                var metaFields = this.MetaFieldService.GetMetaFields(10079760591, MetaFieldResourceType.Products);
                var metafield = metaFields.FirstOrDefault(m => m.Key.Equals("mSizeFit", StringComparison.CurrentCultureIgnoreCase));
                if (metafield == null)
                {
                    metafield = new ShopifyMetaField
                    {
                        //Id = 0,
                        Namespace = "Product",
                        Key = "mSizeFit",
                        ValueType = "string",
                        Description = "MetaField for the mSizeFit of a product."
                    };
                    metaFields.Add(metafield);
                }
                metafield.Value = "Fits true to size";

                // Save to product
                product.Metafields = metaFields;
                this.ProductService.UpdateProduct(product);

                //var metaFields = this.MetaFieldService.GetMetaFields(10079760591, MetaFieldResourceType.Products);
                //var metafield = metaFields.FirstOrDefault(m => m.Key.Equals("mSizeFit", StringComparison.CurrentCultureIgnoreCase));
                //if (metafield == null)
                //{
                //    metafield = new ShopifyMetaField
                //    {
                //        //Id = 0,
                //        Namespace = "Product",
                //        Key = "mSizeFit",
                //        ValueType = "string",
                //        Value = "Fits true to size",
                //        Description = "MetaField for the mSizeFit of a product."
                //    };
                //    this.MetaFieldService.AddMetaField(metafield, 10079760591, MetaFieldResourceType.Products);
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddRedirects()
        {
            using (var context = new FillyFlairEntities())
            {

                var parentProductIds = context.catalog_product_relation.AsNoTracking().GroupBy(p => p.parent_id).Select(g => g.FirstOrDefault().parent_id).ToList();
                var parentProducts = context.catalog_product_entity.Where(e => parentProductIds.Contains(e.entity_id)).ToList();
                System.Console.WriteLine();
                System.Console.WriteLine("Adding " + parentProducts.Count.ToString() + " new redirects: " + DateTime.Now.ToLongTimeString());
                var counter = 0;
                foreach (var parent in parentProducts)
                {
                    try
                    {
                        // Clean up
                        Thread.Sleep(500);
                        var urlKey = this.GetUrlKeyAttribute(parent, 97);
                        if (string.IsNullOrEmpty(urlKey)) continue;
                        this.RedirectService.AddRedirect(urlKey);
                        counter++;
                        if (counter % 100 == 0) System.Console.WriteLine(counter.ToString() + " products have been uploaded: " + DateTime.Now.ToLongTimeString());
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error adding redirect " + parent.entity_id.ToString());
                        System.Console.WriteLine("Error Message: " + ex.Message);
                        if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                    }
                }

            }
        }

        //public void AddFacebookRedirects()
        //{
        //    using (var context = new FillyFlairEntities())
        //    {

        //        var parentProductIds = context.catalog_product_relation.AsNoTracking().GroupBy(p => p.parent_id).Select(g => g.FirstOrDefault().parent_id).ToList();
        //        var parentProducts = context.catalog_product_entity.Where(e => parentProductIds.Contains(e.entity_id)).ToList();
        //        System.Console.WriteLine();
        //        System.Console.WriteLine("Adding " + parentProducts.Count.ToString() + " new redirects: " + DateTime.Now.ToLongTimeString());
        //        var counter = 0;
        //        foreach (var parent in parentProducts)
        //        {
        //            try
        //            {
        //                // Clean up
        //                var urlKey = this.GetUrlKeyAttribute(parent, 97);
        //                if (string.IsNullOrEmpty(urlKey)) continue;
        //                this.RedirectService.AddRedirect(urlKey);
        //                counter++;
        //                if (counter % 100 == 0) System.Console.WriteLine(counter.ToString() + " products have been uploaded: " + DateTime.Now.ToLongTimeString());
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Console.WriteLine("Error adding redirect " + parent.entity_id.ToString());
        //                System.Console.WriteLine("Error Message: " + ex.Message);
        //                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
        //            }
        //        }

        //    }
        //}

        /// <summary>
        /// Uploads the new customers to shopify
        /// </summary>
        public void UploadNewCustomers()
        {
            using (var context = new FillyFlairEntities())
            {
                var magentoCustomers = context.customer_entity.AsNoTracking().OrderByDescending(c => c.updated_at).ToList().Skip(78775);
                System.Console.WriteLine("Importing or updating " + magentoCustomers.Count().ToString() + " customer records from magento.");
                var counter = 0;
                foreach (var magentoCustomer in magentoCustomers)
                {
                    try
                    {
                        var customer = this.AddOrUpdateCustomer(magentoCustomer);
                        if (customer == null) continue;
                        if (customer.Id != null) continue;
                        customer = this.CustomerService.AddCustomer(customer);

                        string url = @"https://fillyflairdev.myshopify.com/admin/customers/" + customer.Id.ToString() + "/account_activation_url.json";
                        var request = WebRequest.Create(url);
                        request.Method = WebRequestMethods.Http.Post;
                        this.SetBasicAuthHeader(request, "5208d2681c1f9859e857f77c1f64ffbf", "60480498225b5edd03ad9b4a24489a57");
                        var authLink = string.Empty;
                        using (var orderResponse = request.GetResponse())
                        {
                            using (var reader = new StreamReader(orderResponse.GetResponseStream()))
                            {
                                JavaScriptSerializer js = new JavaScriptSerializer();
                                var objText = reader.ReadToEnd();
                                dynamic data = JObject.Parse(objText);
                                if (data["account_activation_url"] != null) authLink = data["account_activation_url"].Value;
                            }
                        }

                        Helpers.EmailHelper.Send(customer.FirstName + " " + customer.LastName, customer.Email, authLink);

                        counter++;
                        if (counter % 100 == 0) System.Console.WriteLine("Inserted or updated " + counter.ToString() + " customers.");
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException == null)
                        {
                            System.Console.WriteLine("Error on customer " + magentoCustomer.entity_id.ToString());
                            System.Console.WriteLine("Error Message: " + ex.Message);
                        }
                        else
                        {
                            // Check if invalid email
                            if (ex.InnerException.Message.Equals("email: is invalid", StringComparison.CurrentCultureIgnoreCase)) System.Console.WriteLine("Email invalid " + magentoCustomer.email);
                            else System.Console.WriteLine("Inner Message: " + ex.InnerException.Message);
                        }
                    }
                }

                System.Console.WriteLine();
                System.Console.WriteLine("Number of customers inserted or updated: " + counter.ToString());
            }
        }



        public void CreateGiftCards()
        {
            using (var context = new FillyFlairEntities())
            {
                var request = new SaveGiftCardRequest();
                var magentoCustomers = context.customer_entity.AsNoTracking().ToList();
                var magentoCustomersWithCredit = magentoCustomers.Where(c => c.enterprise_customerbalance.Select(b => b.amount).DefaultIfEmpty(0.0m).Sum() != 0 && c.enterprise_customerbalance.Select(w => w.website_id).FirstOrDefault() == 1).ToList(); 
                System.Console.WriteLine("Createting gift cards for " + magentoCustomersWithCredit.Count().ToString() + " customers " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongTimeString());
                var counter = 0;
                foreach (var magentoCustomer in magentoCustomersWithCredit)
                {
                    var firstNameAttributevalue = this.GetVarcharAttribute(magentoCustomer, 5);
                    var lastNameAttributevalue = this.GetVarcharAttribute(magentoCustomer, 7);
                    var info = magentoCustomer.entity_id.ToString() + " " + firstNameAttributevalue + " " + lastNameAttributevalue + " " + magentoCustomer.email;

                    try
                    {
                        counter++;
                        //var shopifyUser = CustomerService.GetCustomerWithEmail("Roger", "RogerJepsen@gmail.com");
                        var shopifyUser = CustomerService.GetCustomerWithEmail(firstNameAttributevalue, magentoCustomer.email);
                        if (shopifyUser == null) shopifyUser = CustomerService.GetCustomerWithEmail(firstNameAttributevalue, magentoCustomer.email);
                        if (shopifyUser == null) shopifyUser = CustomerService.GetCustomerWithEmail(firstNameAttributevalue, magentoCustomer.email);
                        if (shopifyUser == null) throw new ApplicationException("Could not find customer in shopify");
                        var amount = (decimal)magentoCustomer.enterprise_customerbalance.Select(b => b.amount).DefaultIfEmpty(0.0m).Sum();
                    
                        //var metafield = new ShopifyMetaField()
                        //{
                        //    Namespace = "Customers",
                        //    Key = "mOldCreditAmount",
                        //    Value = amount.ToString("#.##"),
                        //    ValueType = "string",
                        //    Description = "This is the old store credit from magento"
                        //};              
                        //metafield = this.MetaFieldService.AddMetaField(metafield, (long)shopifyUser.Id, MetaFieldResourceType.Customers);
                        //if (metafield.Id == null) throw new ApplicationException("Could not create metafield for customer in shopify");
                        //System.Console.WriteLine("[" + counter.ToString().PadLeft(4, '0') + "] mOldCreditAmount metafield created " + info + " for " + amount.ToString("#.##") + " " + DateTime.Now.ToLongTimeString());

                        request = new SaveGiftCardRequest
                        {
                            InitialValue = amount,
                            UserId = (long)shopifyUser.Id
                        };

                        var response = GiftCardService.SaveGiftCard(request);
                        if (response.IsSuccess) System.Console.WriteLine("[" + counter.ToString().PadLeft(4, '0') + "] Gift Card Created For " + info + " [" + response.GiftCard.Balance.ToString("c") + "][" + response.GiftCard.Code + "] " + DateTime.Now.ToLongTimeString());
                        else System.Console.WriteLine("[" + counter.ToString().PadLeft(4, '0') + "] Error creating gift card for " + info + " Error: " + response.ErrorMessage + " " + DateTime.Now.ToLongTimeString());
                    }
                    catch (Exception ex)
                    {
                        var msg = "[" + counter.ToString().PadLeft(4, '0') + "] Error creating gift card for " + info + " Error: ";
                        if (ex.InnerException != null) System.Console.WriteLine(msg + ex.InnerException.Message);
                        else System.Console.WriteLine(msg + ex.Message);
                    }
                }
                System.Console.WriteLine("Finished " + DateTime.Now.ToLongTimeString());
            }
        }


        private void SetBasicAuthHeader(WebRequest request, String userName, String userPassword)
        {
            string authInfo = userName + ":" + userPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }

        public void UploadNewOrders()
        {
            using (var context = new FillyFlairEntities())
            {
                var orders = context.sales_flat_order.AsNoTracking().Where(o => o.customer_id == 14436).ToList();
                System.Console.WriteLine("Importing or updating " + orders.Count().ToString() + " order records from magento.");
                var counter = 0;
                foreach (var _order in orders)
                {
                    // Add or update product
                    var order = this.AddOrUpdateOrder(_order, context);
                    if (order == null) continue;

                    try
                    {
                        if (order.Id == null) this.OrderService.AddOrder(order);
                        else this.OrderService.UpdateOrder(order);
                        counter++;
                        if (counter % 100 == 0) System.Console.WriteLine(counter.ToString() + " orders have been uploaded: " + DateTime.Now.ToLongTimeString());
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error uploading or adding order " + _order.entity_id.ToString());
                        System.Console.WriteLine("Error Message: " + ex.Message);
                        if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                    }
                }
            }
        }

        #region Products

        private ShopifyProduct AddOrUpdateProduct(catalog_product_entity _entity, bool _isStore = false)
        {
            try
            {
                var now = DateTime.Now;
                var nameAttributeValue = this.GetVarcharAttribute(_entity, 71);
                //var descriptionAttributeValue = this.GetTextAttribute(_entity, 72);
                var descriptionAttributeValue = this.GetTextAttribute(_entity, 73); // Short description is being used in magento
                var actualCost = this.GetDecimalAttribute(_entity, 211);
                var vendorAttributeValue = this.GetVarcharAttribute(_entity, 248);
                var sizeAndFitAttribute = this.GetTextAttribute(_entity, 278);
                var metaKeywordAttribute = this.GetVarcharAttribute(_entity, 84);
                var metaDescriptionAttribute = this.GetTextAttribute(_entity, 83);
                var metaTitleAttribute = this.GetVarcharAttribute(_entity, 82);

                var products = this.ProductService.GetProductByTitle(nameAttributeValue);
                ShopifyProduct product = null;
                if (products.Count > 0)
                {
                    product = products.FirstOrDefault(p => p.Title.Equals(nameAttributeValue, StringComparison.CurrentCultureIgnoreCase));
                    if (product == null)
                    {
                        product = new ShopifyProduct
                        {
                            //Id = _parentId, // not setting as id
                            Title = nameAttributeValue,
                            CreatedAt = _entity.created_at ?? now
                        };
                    }
                }
                else
                {
                    product = new ShopifyProduct
                    {
                        //Id = _parentId, // not setting as id
                        Title = nameAttributeValue,
                        CreatedAt = _entity.created_at ?? now
                    };
                }

                // Create list of metafields
                var metaList = new List<Tuple<string, string, object>>();
                metaList.Add(new Tuple<string, string, object>("mMagentoId", "integer", _entity.entity_id));
                if (!string.IsNullOrEmpty(actualCost)) metaList.Add(new Tuple<string, string, object>("mActualCost", "string", actualCost));
                if (!string.IsNullOrEmpty(sizeAndFitAttribute)) metaList.Add(new Tuple<string, string, object>("mSizeFit", "string", sizeAndFitAttribute));
                if (!string.IsNullOrEmpty(metaTitleAttribute)) metaList.Add(new Tuple<string, string, object>("mMetaTitle", "string", metaTitleAttribute));
                if (!string.IsNullOrEmpty(metaKeywordAttribute)) metaList.Add(new Tuple<string, string, object>("mMetaKeyword", "string", metaKeywordAttribute));
                if (!string.IsNullOrEmpty(metaDescriptionAttribute)) metaList.Add(new Tuple<string, string, object>("mMetaDescription", "string", metaDescriptionAttribute));

                product.BodyHtml = descriptionAttributeValue;
                product.Metafields = this.AddOrUpdateMetaFields(MetaFieldResourceType.Products, metaList, product.Id);
                product.Handle = this.GetUrlKeyAttribute(_entity, 97);
                product.Vendor = vendorAttributeValue;
                product.ProductType = this.GetProductType(_entity).ToString();
                product.Images = this.AddOrUpdateImages(_entity);
                product.Options = new List<ShopifyProductOption>() { this.CreateOption("Size", 1) };
                product.PublishedAt = _isStore ? (DateTime?)null : now;
                product.PublishedScope = "global"; //The sales channels in which the product is visible.
                product.Tags = "";
                product.TemplateSuffix = ""; // The suffix of the liquid template being used. By default, the original template is called product.liquid, without any suffix.
                product.UpdatedAt = _entity.updated_at ?? now;
                return product;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error getting product for " + _entity.entity_id);
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return null;
            }
        }
        private ShopifyProduct AddOrUpdateVariants(catalog_product_entity _entity, ShopifyProduct _product, catalog_product_entity _parentEntity, bool _isStore = false)
        {
            try
            {
                // Get current variants from product and add or update
                var now = DateTime.Now;
                var sizeAttributeValue = this.GetIntAttribute(_entity, 196);
                var nameAttributeValue = this.GetVarcharAttribute(_entity, 71);
                var priceAttributeValue = this.GetDecimalAttribute(_entity, 75);
                var salePriceAttributeValue = this.GetDecimalAttribute(_parentEntity, 76);
                var weightAttributeValue = this.GetDecimalAttribute(_entity, 80);
                var weightTypeAttributeValue = this.GetIntAttribute(_entity, 125);

                var price = Convert.ToDouble(priceAttributeValue);
                var salePrice = string.IsNullOrEmpty(salePriceAttributeValue) ? 0 : Convert.ToDouble(salePriceAttributeValue);

                var inventoryQuantity = this.GetItemQuantity(_entity, _isStore);
                if (_isStore && inventoryQuantity == 0) return _product;

                if (_product.Variants == null) _product.Variants = new List<ShopifyProductVariant>();
                var currantVariants = _product.Variants.ToList();
                var variant = currantVariants.FirstOrDefault(v => v.SKU == _entity.sku);
                if (variant == null)
                {
                    variant = new ShopifyProductVariant
                    {
                        //Id = entity.entity_id, // not setting as id
                        SKU = _entity.sku,
                        CreatedAt = _entity.created_at ?? now,
                    };
                    currantVariants.Add(variant);
                }
                //ImageId = 0,
                //variant.ProductId = (long)product.Id;
                variant.InventoryManagement = "shopify"; //Specifies whether or not Shopify tracks the number of items in stock for this product variant "shopify" or blank
                variant.InventoryPolicy = "deny"; // Specifies whether or not customers are allowed to place an order for a product variant when it's out of stock. "deny", "continue"
                variant.FulfillmentService = "manual"; // Service which is doing the fulfillment. Possible values are "manual" or the handle of a FulfillmentService
                variant.Title = nameAttributeValue;
                variant.Grams = 0;
                //variant.InventoryQuantityAdjustment = 0; // Not needed if uisng old and new inventory fields
                variant.Option1 = sizeAttributeValue;
                //variant.Option2 = ""; // Not used at this time
                //variant.Option3 = ""; // Not used at this time
                variant.Position = currantVariants.Count + 1;
                variant.RequiresShipping = true;
                variant.Taxable = true;
                variant.InventoryQuantity = inventoryQuantity;
                variant.OldInventoryQuantity = inventoryQuantity;
                variant.Price = salePrice != 0 ? salePrice : price;
                variant.Weight = Convert.ToDouble(weightAttributeValue);
                variant.CompareAtPrice = salePrice != 0 ? price : 0;
                variant.Barcode = _entity.sku ?? ""; // We made the barcode same as the sku so each item is different
                variant.WeightUnit = weightTypeAttributeValue;
                variant.Metafields = new List<ShopifyMetaField>();
                variant.UpdatedAt = _entity.updated_at ?? now;
                _product.Variants = currantVariants;

                // Add size to option if it does not exists
                _product.Options = this.AddOrUpdateOptions(_product, "Size", sizeAttributeValue);
                var tags = _product.Options.FirstOrDefault(o => o.Name == "Size").Values.ToList();
                if (tags.Count() > 0) _product.Tags = string.Join(",", tags);
                return _product;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error getting product variant for " + _entity.entity_id);
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return _product;
            }
        }

        private string GetIntAttribute(catalog_product_entity _entity, int _attributeId)
        {
            try
            {
                var intAttribute = _entity.catalog_product_entity_int.FirstOrDefault(_ => _.attribute_id == _attributeId);
                if (intAttribute != null)
                    if (intAttribute.value != null)
                    {
                        var option = this.AttributeOptions.FirstOrDefault(ao => ao.option_id == intAttribute.value);
                        if (option != null) return option.value;
                    }
                return "";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Unable to get attribute for [ProductId: " + _entity.entity_id.ToString() + "][AttributeId: " + _attributeId.ToString() + "]");
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return "";
            }
        }
        private string GetVarcharAttribute(catalog_product_entity _entity, int _attributeId)
        {
            try
            {
                var varcharAttribute = _entity.catalog_product_entity_varchar.FirstOrDefault(_ => _.attribute_id == _attributeId);
                if (varcharAttribute != null)
                    if (varcharAttribute.value != null)
                        return varcharAttribute.value;
                return "";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Unable to get attribute for [ProductId: " + _entity.entity_id.ToString() + "][AttributeId: " + _attributeId.ToString() + "]");
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return "";
            }
        }
        private string GetTextAttribute(catalog_product_entity _entity, int _attributeId)
        {
            try
            {
                var textAttribute = _entity.catalog_product_entity_text.FirstOrDefault(_ => _.attribute_id == _attributeId);
                if (textAttribute != null)
                    if (textAttribute.value != null)
                        return textAttribute.value;
                return "";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Unable to get attribute for [ProductId: " + _entity.entity_id.ToString() + "][AttributeId: " + _attributeId.ToString() + "]");
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return "";
            }
        }
        private string GetDateTimeAttribute(catalog_product_entity _entity, int _attributeId)
        {
            try
            {
                var datetimeAttribute = _entity.catalog_product_entity_varchar.FirstOrDefault(_ => _.attribute_id == _attributeId);
                if (datetimeAttribute != null)
                    if (datetimeAttribute.value != null)
                        return datetimeAttribute.value;
                return "";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Unable to get attribute for [ProductId: " + _entity.entity_id.ToString() + "][AttributeId: " + _attributeId.ToString() + "]");
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return "";
            }
        }
        private string GetDecimalAttribute(catalog_product_entity _entity, int _attributeId)
        {
            try
            {
                var decimalAttribute = _entity.catalog_product_entity_decimal.FirstOrDefault(_ => _.attribute_id == _attributeId);
                if (decimalAttribute != null)
                    if (decimalAttribute.value != null)
                        return decimalAttribute.value.ToString();
                return "";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Unable to get attribute for [ProductId: " + _entity.entity_id.ToString() + "][AttributeId: " + _attributeId.ToString() + "]");
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return "";
            }
        }
        private string GetUrlKeyAttribute(catalog_product_entity _entity, int _attributeId)
        {
            try
            {
                var textAttribute = _entity.catalog_product_entity_url_key.FirstOrDefault(_ => _.attribute_id == _attributeId);
                if (textAttribute != null)
                    if (textAttribute.value != null)
                        return textAttribute.value;
                return "";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Unable to get attribute for [ProductId: " + _entity.entity_id.ToString() + "][AttributeId: " + _attributeId.ToString() + "]");
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Adds or updates options for the product
        /// </summary>
        /// <param name="_product">The product to add or update to</param>
        /// <param name="_option">The option to add or update</param>
        /// <param name="_value">The value for the option</param>
        /// <returns></returns>
        private List<ShopifyProductOption> AddOrUpdateOptions(ShopifyProduct _product, string _option, string _value)
        {
            try
            {
                if (string.IsNullOrEmpty(_value)) return _product.Options.ToList();
                var options = new List<ShopifyProductOption>();
                var option = _product.Options.FirstOrDefault(o => o.Name == _option);
                var optionValues = option.Values.ToList();
                if (!optionValues.Contains(_value))
                {
                    optionValues.Add(_value);
                    option.Values = optionValues;
                }
                foreach (var oldOption in _product.Options)
                {
                    if (oldOption.Name == _option) options.Add(option);
                    else options.Add(oldOption);
                }
                return options;
            }
            catch (Exception)
            {
                return _product.Options.ToList();
            }
        }
        private ShopifyProductOption CreateOption(string _name, int _position)
        {
            return new ShopifyProductOption
            {
                //ProductId = _productId,
                Name = _name,
                Position = _position,
                Values = new List<string>()
            };
        }

        /// <summary>
        /// Gets the item quantity
        /// </summary>
        /// <param name="_entity"></param>
        /// <returns></returns>
        private int GetItemQuantity(catalog_product_entity _entity, bool _isStore = false)
        {
            try
            {
                if (_isStore)
                {
                    using (var context = new FillyFlairEntities())
                    {
                        var store = context.warehouse_stock_movements_index.FirstOrDefault(w => w.stockmovindex_depot_id == 1 && w.stockmovindex_product_id == _entity.entity_id);
                        if (store == null) return 0;
                        return (int)store.stockmovindex_quantity;
                    }
                }
                else
                {
                    var stockItem = _entity.cataloginventory_stock_item.FirstOrDefault();
                    if (stockItem == null) return 0;
                    return (int)stockItem.qty;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error getting item quantity for " + _entity.entity_id.ToString());
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Adds or updates images for the product
        /// </summary>
        /// <param name="_entity">The entity in magento to get the images</param>
        /// <returns></returns>
        private List<ShopifyProductImage> AddOrUpdateImages(catalog_product_entity _entity)
        {
            try
            {
                var imagesValues = _entity.catalog_product_entity_media_gallery.Select(mg => mg.value_id).ToList();
                using (var context = new FillyFlairEntities())
                {
                    var lst = new List<ShopifyProductImage>();
                    var images = context.catalog_product_entity_media_gallery_value.Where(v => imagesValues.Contains(v.value_id) && v.store_id == 0 && v.disabled == 0)
                                                                                   .OrderBy(v => v.position).ToList();
                    foreach (var image in images)
                    {
                        var url = image.catalog_product_entity_media_gallery.value;
                        var src = "http://media.fillyflair.com/media/catalog/product" + url;
                        byte[] data = null;
                        using (WebClient webClient = new WebClient())
                            data = webClient.DownloadData(src);

                        lst.Add(new ShopifyProductImage
                        {
                            //Id = 0,
                            //ProductId = _parentId,
                            Attachment = Convert.ToBase64String(data),
                            Position = (int)image.position,
                            //Metafields = null,
                            //Src = "",
                            //VariantIds = new List<long>(),
                            UpdatedAt = DateTime.Now,
                            CreatedAt = DateTime.Now
                        });
                    }
                    return lst;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error adding or updateing images for " + _entity.entity_id.ToString());
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return new List<ShopifyProductImage>();
            }

        }

        /// <summary>
        /// Adds or updates the metafields for the product
        /// </summary>
        /// <param name="_productId">The product id</param>
        /// <param name="_actualCost">The actual cost</param>
        /// <param name="_magentoId">The old magentoId</param>
        /// <returns></returns>
        private List<ShopifyMetaField> AddOrUpdateMetaFields(MetaFieldResourceType _resourceType, List<Tuple<string, string, object>> _metaList, long? _resourceId)
        {
            List<ShopifyMetaField> currentMetaFields;

            var nameSpace = "";
            if (_resourceType == MetaFieldResourceType.Products) nameSpace = "Product";
            if (_resourceType == MetaFieldResourceType.Customers) nameSpace = "Customer";

            if (_resourceId == null) currentMetaFields = new List<ShopifyMetaField>();
            else currentMetaFields = this.MetaFieldService.GetMetaFields((long)_resourceId, _resourceType);
            foreach (var metaField in _metaList)
            {
                try
                {
                    var newField = currentMetaFields.FirstOrDefault(m => m.Key == metaField.Item1);
                    if (newField == null)
                    {
                        newField = new ShopifyMetaField
                        {
                            //Id = 0,
                            Namespace = nameSpace,
                            Key = metaField.Item1,
                            ValueType = metaField.Item2,
                            Description = "MetaField for the " + metaField.Item1 + " of a product."
                        };
                        currentMetaFields.Add(newField);
                    }
                    newField.Value = metaField.Item3;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Unable to create meta fields [Key: " + metaField.Item1 + "]");
                    if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                    else System.Console.WriteLine("Error Message: " + ex.Message);
                }
            }
            return currentMetaFields;
        }

        /// <summary>
        /// Returns the product type for the product
        /// </summary>
        /// <param name="_entity">The entity in magento to get the information from</param>
        /// <returns></returns>
        private ProductType GetProductType(catalog_product_entity _entity)
        {
            try
            {
                // Get list of custom collections
                var customCollections = new List<ShopifyCustomCollection>();
                var categories = _entity.catalog_category_product.ToList();
                var productTypes = new List<ProductType>();
                foreach (var catagory in categories)
                {
                    var entity = catagory.catalog_category_entity.catalog_category_entity_varchar.FirstOrDefault(c => c.attribute_id == 41);
                    if (entity == null) continue;
                    if (string.IsNullOrEmpty(entity.value)) continue;

                    // Accessories ProductType
                    if (entity.value.Equals("Accessories", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("All Accessories", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Belts", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Bracelets", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Earrings", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Hats", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Necklaces", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Other Accessories", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Other Jewelry", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Purses & Clutches", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Scarves", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);
                    if (entity.value.Equals("Sunglasses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Accessories);

                    // Activewear ProductType
                    if (entity.value.Equals("Activewear", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Activewear);

                    // Basics ProductType
                    if (entity.value.Equals("Basics", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Basics);
                    if (entity.value.Equals("Undergarments", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Basics);

                    // Bottoms ProductType
                    if (entity.value.Equals("All Bottoms", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Bottoms);
                    if (entity.value.Equals("Bottoms", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Bottoms);
                    if (entity.value.Equals("Denim", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Bottoms);
                    if (entity.value.Equals("Jeans", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Bottoms);
                    if (entity.value.Equals("Leggings", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Bottoms);
                    if (entity.value.Equals("Pants", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Bottoms);
                    if (entity.value.Equals("Shorts", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Bottoms);
                    if (entity.value.Equals("Skirts", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Bottoms);

                    // Dresses ProductType
                    if (entity.value.Equals("All Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Bridesmaids Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Casual Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Dresses on Sale", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Floral Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("High Low", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Lace Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Long Sleeve Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Maxi Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Mini Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Print Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Short Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);
                    if (entity.value.Equals("Strapless Dresses", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Dresses);

                    // Gift Cards ProductType
                    if (entity.value.Equals("Gift Card", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.GiftCards);

                    // Kids ProductType
                    if (entity.value.Equals("Kids", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Kids);

                    // Outerwear ProductType
                    if (entity.value.Equals("Jackets", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Outerwear);
                    if (entity.value.Equals("Outerwear", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Outerwear);
                    if (entity.value.Equals("Vests", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Outerwear);

                    // Tops ProductType
                    if (entity.value.Equals("3/4 and Long Sleeves", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Tops);
                    if (entity.value.Equals("Cardigans", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Tops);
                    if (entity.value.Equals("Dolmans", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Tops);
                    if (entity.value.Equals("Graphic Tees", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Tops);
                    if (entity.value.Equals("Sweatshirts & Outerwear", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Tops);
                    //if (entity.value.Equals("Tanks and Tees", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Tops);
                    if (entity.value.Equals("Tees", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Tops);
                    if (entity.value.Equals("Tops", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Tops);
                    if (entity.value.Equals("Tunics", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Tops);

                    // Shoes ProductType
                    if (entity.value.Equals("All Shoes", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Shoes);
                    if (entity.value.Equals("Booties", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Shoes);
                    if (entity.value.Equals("Boots", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Shoes);
                    if (entity.value.Equals("Flats", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Shoes);
                    if (entity.value.Equals("Heels", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Shoes);
                    if (entity.value.Equals("Sandals", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Shoes);
                    if (entity.value.Equals("Shoes", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Shoes);
                    if (entity.value.Equals("Wedges", StringComparison.CurrentCultureIgnoreCase)) productTypes.Add(ProductType.Shoes);
                }

                //Basics - would override tops & accessories
                if (productTypes.Contains(ProductType.Basics)) return ProductType.Basics;

                //Accessories - I can't think of an example except maybe basics that would be in this category.
                if (productTypes.Contains(ProductType.Accessories)) return ProductType.Accessories;

                //GiftCards - would override anything else
                if (productTypes.Contains(ProductType.GiftCards)) return ProductType.GiftCards;

                //Kids - would override anything else it would be in
                if (productTypes.Contains(ProductType.Kids)) return ProductType.Kids;

                //Shoes - There shouldn't be anything else these are in so shoes would override.
                if (productTypes.Contains(ProductType.Shoes)) return ProductType.Shoes;

                //Activewear - would override tops & bottoms
                if (productTypes.Contains(ProductType.Activewear)) return ProductType.Activewear;

                //Bottoms - would override anything except activewear
                if (productTypes.Contains(ProductType.Bottoms)) return ProductType.Bottoms;

                //Outerwear - would override tops
                if (productTypes.Contains(ProductType.Outerwear)) return ProductType.Outerwear;

                //Dresses - On this one there could be some that are tops and dresses, so I would say dresses would override tops
                if (productTypes.Contains(ProductType.Dresses)) return ProductType.Dresses;

                //Tops - Basics, Activewear, Dresses, and Outerwear would override tops.Otherwise, it should just be a top
                if (productTypes.Contains(ProductType.Tops)) return ProductType.Tops;
                return ProductType.Unknown;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error getting product type for " + _entity.entity_id);
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return ProductType.Unknown;
            }

        }

        /// <summary>
        /// Creates the collect for the product id and the collection id.
        /// </summary>
        /// <param name="_entity">The magento product entity to populate the collection</param>
        /// <param name="_productId">The product id to add the collections to.</param>
        public void CreateProductCollects(catalog_product_entity _entity, long _productId, string _productType)
        {
            try
            {
                // Get list of custom collections
                var customCollections = new List<ShopifyCustomCollection>();
                var categories = _entity.catalog_category_product.ToList();
                var categoryList = new List<string>();
                foreach (var catagory in categories)
                {
                    var entity = catagory.catalog_category_entity.catalog_category_entity_varchar.FirstOrDefault(c => c.attribute_id == 41);
                    if (entity == null) continue;
                    if (string.IsNullOrEmpty(entity.value)) continue;

                    var productType = ProductType.Unknown;
                    if (!string.IsNullOrEmpty(_productType)) productType = (ProductType)Enum.Parse(typeof(ProductType), _productType);

                    customCollections = this.UpdateCustomCollection(customCollections, entity.value.Trim(), productType);
                    categoryList.Add(entity.value.ToUpper().Trim());
                }

                // Create the collect
                var collects = this.CollectService.GetCollects(_productId);
                foreach (var collection in customCollections)
                {
                    var collect = collects.FirstOrDefault(c => c.CollectionId == collection.Id);
                    if (collect == null)
                    {
                        var newCollect = this.CollectService.AddCollect(new ShopifyCollect { ProductId = _productId, CollectionId = (long)collection.Id });
                        collects.Add(newCollect);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Unable to create custom collect for [EntityId: " + _entity.entity_id.ToString() + "][ProductId: " + _productId.ToString() + "]");
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
            }
        }
        private List<ShopifyCustomCollection> UpdateCustomCollection(List<ShopifyCustomCollection> _currentCollection, string _value, ProductType _productType)
        {
            // Accessories Collection
            //if (_value.Equals("Accessories", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Other Accessories");
            //if (_value.Equals("All Accessories", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Other Accessories");
            if (_value.Equals("Belts", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Other Accessories");
            if (_value.Equals("Bracelets", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Bracelets");
            if (_value.Equals("Earrings", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Earrings");
            if (_value.Equals("Hats", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Hats");
            if (_value.Equals("Necklaces", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Necklaces");
            if (_value.Equals("Other Accessories", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Other Accessories");
            if (_value.Equals("Other Jewelry", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Other Accessories");
            if (_value.Equals("Purses & Clutches", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Purses & Clutches");
            if (_value.Equals("Scarves", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Scarves");
            if (_value.Equals("Sunglasses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Sunglasses");

            // Activewear Collection
            if (_value.Equals("Activewear", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Activewear");

            // Basics Collection
            if (_value.Equals("Basics", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Basics");
            if (_value.Equals("Undergarments", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Basics");

            // Bottoms Collection
            if (_value.Equals("All Bottoms", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Bottoms");
            if (_value.Equals("Bottoms", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Bottoms");
            if (_value.Equals("Denim", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Jeans");
            if (_value.Equals("Jeans", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Jeans");
            if (_value.Equals("Leggings", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Leggings");
            if (_value.Equals("Pants", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Bottoms");
            if (_value.Equals("Shorts", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Shorts");
            if (_value.Equals("Skirts", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Skirts");

            // Dresses Collection
            if (_value.Equals("All Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Dresses");
            if (_value.Equals("Bridesmaids Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Bridesmaid Dresses");
            if (_value.Equals("Casual Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Casual Dresses");
            if (_value.Equals("Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Dresses");
            if (_value.Equals("Dresses on Sale", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Dresses Sale");
            if (_value.Equals("Floral Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Floral Dresses");
            if (_value.Equals("High Low", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "High Low Dresses");
            if (_value.Equals("Lace Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Lace Dresses");
            if (_value.Equals("Long Sleeve Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Long Sleeve Dresses");
            if (_value.Equals("Maxi Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Maxi Dresses");
            if (_value.Equals("Mini Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Short Dresses");
            if (_value.Equals("Print Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Print Dresses");
            if (_value.Equals("Short Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Short Dresses");
            if (_value.Equals("Strapless Dresses", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Strapless Dresses");

            // Filly Flair Exclusives Collection
            if (_value.Equals("Exclusive", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Filly Flair Exclusives");
            //if (_value.Equals("Filly Flair", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Filly Flair Exclusives");
            if (_value.Equals("Filly Flair Exclusive", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Filly Flair Exclusives");
            //if (_value.Equals("FIlly Flair Web", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Filly Flair Exclusives");

            // Gift Cards Collection
            if (_value.Equals("Gift Card", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Gift Cards");

            // Kids Collection
            if (_value.Equals("Kids", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Kids");

            // New Collection
            if (_value.Equals("Best Sellers", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Best Sellers", "New");
            if (_value.Equals("Just In", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "New");
            if (_value.Equals("New", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "New");
            if (_value.Equals("Whats New", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "New");

            // Outerwear Collection
            if (_value.Equals("Jackets", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Jackets");
            if (_value.Equals("Outerwear", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Outerwear");
            if (_value.Equals("Vests", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Vests");

            // Plus Collection
            if (_value.Equals("Plus", StringComparison.CurrentCultureIgnoreCase))
            {
                if (_productType == ProductType.Dresses)
                    if (_currentCollection.Any(c => c.Title.Contains("Sale"))) return this.AddCustomCollection(_currentCollection, "Plus Dresses", "Plus Sale");
                    else return this.AddCustomCollection(_currentCollection, "Plus Dresses");
                if (_productType == ProductType.Tops)
                    if (_currentCollection.Any(c => c.Title.Contains("Sale"))) return this.AddCustomCollection(_currentCollection, "Plus Tops", "Plus Sale");
                    else return this.AddCustomCollection(_currentCollection, "Plus Tops");
                if (_productType == ProductType.Bottoms)
                    if (_currentCollection.Any(c => c.Title.Contains("Sale"))) return this.AddCustomCollection(_currentCollection, "Plus Bottoms", "Plus Sale");
                    else return this.AddCustomCollection(_currentCollection, "Plus Bottoms");
                return this.AddCustomCollection(_currentCollection, "Plus");
            }

            // Tops Collection
            if (_value.Equals("3/4 and Long Sleeves", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "3/4 & Long Sleeves");
            if (_value.Equals("Cardigans", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Cardigans");
            if (_value.Equals("Dolmans", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Dolmans");
            if (_value.Equals("Graphic Tees", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Graphic Tees");
            if (_value.Equals("Sweatshirts & Outerwear", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Sweatshirts");
            if (_value.Equals("Floral", StringComparison.CurrentCultureIgnoreCase))
                return this.AddCustomCollection(_currentCollection, "Floral Tops");
            if (_value.Equals("Tees", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Tees");
            if (_value.Equals("Tops", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Tops");
            if (_value.Equals("Tunics", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Tunics");

            // Sale Collection
            //if (_value.Equals("All Clothing On Sale", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Sale");
            //if (_value.Equals("Blowout Sale", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Sale");
            //if (_value.Equals("Last Chance Sale", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Sale");
            //if (_value.Equals("On Sale", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Sale");
            //if (_value.Equals("Sale", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Sale");
            if (_value.Equals("Sale", StringComparison.CurrentCultureIgnoreCase) || _value.Equals("On Sale", StringComparison.CurrentCultureIgnoreCase) ||
                _value.Equals("Last Chance Sale", StringComparison.CurrentCultureIgnoreCase) || _value.Equals("Blowout Sale", StringComparison.CurrentCultureIgnoreCase) ||
                _value.Equals("All Clothing On Sale", StringComparison.CurrentCultureIgnoreCase))
            {
                if (_productType == ProductType.Dresses)
                    if (_currentCollection.Any(c => c.Title.Contains("Plus"))) return this.AddCustomCollection(_currentCollection, "Dresses Sale", "Plus Sale", "Sale");
                    else return this.AddCustomCollection(_currentCollection, "Dresses Sale", "Sale");
                if (_productType == ProductType.Tops)
                    if (_currentCollection.Any(c => c.Title.Contains("Plus"))) return this.AddCustomCollection(_currentCollection, "Tops Sale", "Plus Sale", "Sale");
                    else return this.AddCustomCollection(_currentCollection, "Tops Sale", "Sale");
                if (_productType == ProductType.Bottoms)
                    if (_currentCollection.Any(c => c.Title.Contains("Plus"))) return this.AddCustomCollection(_currentCollection, "Bottoms Sale", "Plus Sale", "Sale");
                    else return this.AddCustomCollection(_currentCollection, "Bottoms Sale", "Sale");
                if (_productType == ProductType.Shoes) return this.AddCustomCollection(_currentCollection, "Shoes Sale", "Sale");
                if (_productType == ProductType.Accessories) return this.AddCustomCollection(_currentCollection, "Accessories Sale", "Sale");
                if (_productType == ProductType.Kids) return this.AddCustomCollection(_currentCollection, "Kids Sale", "Sale");
                return this.AddCustomCollection(_currentCollection, "Sale");
            }

            // Shoes Collection
            if (_value.Equals("All Shoes", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Shoes");
            if (_value.Equals("Booties", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Booties");
            if (_value.Equals("Boots", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Boots");
            if (_value.Equals("Flats", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Casual Shoes");
            if (_value.Equals("Heels", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Heels");
            if (_value.Equals("Sandals", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Sandals");
            if (_value.Equals("Shoes", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Shoes");
            if (_value.Equals("Wedges", StringComparison.CurrentCultureIgnoreCase)) return this.AddCustomCollection(_currentCollection, "Wedges");

            return _currentCollection;
        }
        private List<ShopifyCustomCollection> AddCustomCollection(List<ShopifyCustomCollection> _currentCollection, string _collectionName, string _secondCollectionName = null, string _thirdCollectionName = null)
        {
            try
            {
                // Add new collection name to list
                var now = DateTime.Now;
                if (this.ShopifyCollections == null) this.ShopifyCollections = this.CustomCollectionService.GetCustomCollections();
                var newValue = _currentCollection.FirstOrDefault(c => c.Title.Equals(_collectionName, StringComparison.CurrentCultureIgnoreCase));
                if (newValue == null)
                {
                    var newCollection = this.ShopifyCollections.FirstOrDefault(c => c.Title.Equals(_collectionName, StringComparison.CurrentCultureIgnoreCase));
                    if (newCollection == null)
                    {
                        newCollection = new ShopifyCustomCollection
                        {
                            Id = 0,
                            Title = _collectionName,
                            Handle = "_" + _collectionName,
                            Published = true,
                            PublishedScope = "global",
                            PublishedAt = now,
                            UpdatedAt = now
                        };
                        newCollection = this.CustomCollectionService.AddCustomCollection(newCollection);
                        this.ShopifyCollections.Add(newCollection);
                    }
                    _currentCollection.Add(newCollection);
                }

                // Add second collection name to list
                if (!string.IsNullOrEmpty(_secondCollectionName))
                {
                    var secondValue = _currentCollection.FirstOrDefault(c => c.Title.Equals(_secondCollectionName, StringComparison.CurrentCultureIgnoreCase));
                    if (secondValue == null)
                    {
                        var newCollection = this.ShopifyCollections.FirstOrDefault(c => c.Title.Equals(_secondCollectionName, StringComparison.CurrentCultureIgnoreCase));
                        if (newCollection == null)
                        {
                            newCollection = new ShopifyCustomCollection
                            {
                                Id = 0,
                                Title = _secondCollectionName,
                                Handle = _secondCollectionName,
                                Published = true,
                                PublishedScope = "global",
                                PublishedAt = now,
                                UpdatedAt = now
                            };
                            newCollection = this.CustomCollectionService.AddCustomCollection(newCollection);
                            this.ShopifyCollections.Add(newCollection);
                        }
                        _currentCollection.Add(newCollection);
                    }
                }

                // Add third collection name to list
                if (!string.IsNullOrEmpty(_thirdCollectionName))
                {
                    var thirdValue = _currentCollection.FirstOrDefault(c => c.Title.Equals(_thirdCollectionName, StringComparison.CurrentCultureIgnoreCase));
                    if (thirdValue == null)
                    {
                        var newCollection = this.ShopifyCollections.FirstOrDefault(c => c.Title.Equals(_thirdCollectionName, StringComparison.CurrentCultureIgnoreCase));
                        if (newCollection == null)
                        {
                            newCollection = new ShopifyCustomCollection
                            {
                                Id = 0,
                                Title = _thirdCollectionName,
                                Handle = _thirdCollectionName,
                                Published = true,
                                PublishedScope = "global",
                                PublishedAt = now,
                                UpdatedAt = now
                            };
                            newCollection = this.CustomCollectionService.AddCustomCollection(newCollection);
                            this.ShopifyCollections.Add(newCollection);
                        }
                        _currentCollection.Add(newCollection);
                    }
                }

                // return list
                return _currentCollection;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Cannot add values to custom collection list [" + _collectionName + "]");
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
                return _currentCollection;
            }
        }

        #endregion

        #region Customers

        /// <summary>
        /// Gets a list of all customers
        /// </summary>
        public void ListAllCustomers()
        {
            try
            {
                foreach (var cusomer in this.CustomerService.GetCustomers())
                {
                    var customerString = "";
                    customerString += "Id: " + cusomer.Id.ToString();
                    customerString += " FirstName: " + cusomer.FirstName;
                    customerString += " LastName: " + cusomer.LastName;
                    customerString += " Email: " + cusomer.Email;
                    System.Console.WriteLine(customerString);
                    System.Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private ShopifyCustomer AddOrUpdateCustomer(customer_entity _entity)
        {
            try
            {
                var now = DateTime.Now;
                var firstNameAttributevalue = this.GetVarcharAttribute(_entity, 5);
                var lastNameAttributevalue = this.GetVarcharAttribute(_entity, 7);

                var rewardPoints = 0;
                var rewards = _entity.enterprise_reward.ToList().FirstOrDefault();
                if (rewards != null) rewardPoints = (int)rewards.points_balance;

                decimal? storeCreditAmount = 0.0m;
                var storeCredit = _entity.giftvoucher_credit;
                if (storeCredit != null)
                    storeCreditAmount = storeCredit.Select(s => s.balance).DefaultIfEmpty(0.0m).Sum();

                decimal? giftCardCreditAmount = 0;
                using (var context = new FillyFlairEntities())
                {
                    var otherGiftCardCredit = context.giftvouchers.Where(g => g.recipient_email.Equals(_entity.email, StringComparison.CurrentCultureIgnoreCase));
                    if (otherGiftCardCredit != null)
                        storeCreditAmount = otherGiftCardCredit.Select(s => s.balance).DefaultIfEmpty(0.0m).Sum();
                    var giftCardCredit = context.giftvouchers.Where(g => g.customer_id == _entity.entity_id);
                    if (giftCardCredit != null)
                        storeCreditAmount += giftCardCredit.Select(s => s.balance).DefaultIfEmpty(0.0m).Sum();
                }

                var customer = this.CustomerService.GetCustomerWithEmail(firstNameAttributevalue, _entity.email);
                if (customer == null)
                {
                    customer = new ShopifyCustomer
                    {
                        //Id = 0,
                        Email = _entity.email.Trim(),
                        CreatedAt = now
                    };
                }
                customer.FirstName = firstNameAttributevalue;
                customer.LastName = lastNameAttributevalue;
                customer.Addresses = this.AddOrUpdateAddresses(_entity.customer_address_entity, null);
                customer.DefaultAddress = customer.Addresses == null ? null : customer.Addresses.FirstOrDefault();

                // Create list of metafields
                var metaList = new List<Tuple<string, string, object>>();
                if (rewardPoints > 0) metaList.Add(new Tuple<string, string, object>("mRewardPoints", "integer", rewardPoints));
                if (storeCreditAmount > 0) metaList.Add(new Tuple<string, string, object>("mStoreCreditAmount", "string", storeCreditAmount.ToString()));
                if (giftCardCreditAmount > 0) metaList.Add(new Tuple<string, string, object>("mGiftCardAmount", "string", giftCardCreditAmount.ToString()));
                customer.Metafields = this.AddOrUpdateMetaFields(MetaFieldResourceType.Customers, metaList, _entity.entity_id);

                customer.Note = "";
                customer.TaxExempt = false;
                customer.MultipassIdentifier = "";
                customer.AcceptsMarketing = false;
                customer.LastOrderId = 0;
                customer.LastOrderName = "";
                customer.OrdersCount = 0;
                customer.Tags = "";
                customer.TotalSpent = 0;
                customer.VerifiedEmail = false;
                customer.UpdatedAt = now;
                return customer;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        private string GetVarcharAttribute(customer_entity _entity, int _attributeId)
        {
            try
            {
                var varcharAttribute = _entity.customer_entity_varchar.FirstOrDefault(_ => _.attribute_id == _attributeId);
                if (varcharAttribute != null)
                    if (varcharAttribute.value != null)
                        return varcharAttribute.value;
                return "";
            }
            catch (Exception)
            {
                System.Console.WriteLine("Unable to get attribute for [CustomerId: " + _entity.entity_id.ToString() + "][AttributeId: " + _attributeId.ToString() + "]");
                return "";
            }
        }
        private List<ShopifyAddress> AddOrUpdateAddresses(IEnumerable<customer_address_entity> _entity, IEnumerable<ShopifyAddress> _currentAddresses)
        {
            try
            {
                if (_entity == null) return null;
                var addressEntitys = _entity.ToList();
                var currentAddresses = new List<ShopifyAddress>();
                if (_currentAddresses != null) currentAddresses = _currentAddresses.ToList();
                foreach (var entity in addressEntitys)
                {
                    var address = this.AddOrUpdateAddress(entity);
                    if (address == null) continue;
                    currentAddresses.Add(address);
                }
                return currentAddresses;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
                return _currentAddresses == null ? null : _currentAddresses.ToList();
            }
        }
        private ShopifyAddress AddOrUpdateAddress(customer_address_entity _entity)
        {
            try
            {
                if (_entity == null) return null;
                var address = new ShopifyAddress
                {
                    //Id = 0 
                };
                address.FirstName = this.GetVarcharAttribute(_entity, 20);
                address.LastName = this.GetVarcharAttribute(_entity, 22);
                address.Address1 = this.GetTextAttribute(_entity, 25);
                address.Address2 = ""; // Not used in magento
                address.City = this.GetVarcharAttribute(_entity, 26);
                address.Company = this.GetVarcharAttribute(_entity, 24);
                address.Name = "";
                address.Country = this.GetVarcharAttribute(_entity, 27);
                address.CountryCode = "";
                address.CountryName = "";
                address.Default = false;
                address.Phone = this.GetVarcharAttribute(_entity, 31);
                address.Province = this.GetVarcharAttribute(_entity, 28);  // used for the state
                address.ProvinceCode = "";
                address.Zip = this.GetVarcharAttribute(_entity, 30);
                return address;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        private string GetTextAttribute(customer_address_entity _entity, int _attributeId)
        {
            try
            {
                var textAttribute = _entity.customer_address_entity_text.FirstOrDefault(_ => _.attribute_id == _attributeId);
                if (textAttribute != null)
                    if (textAttribute.value != null)
                        return textAttribute.value;
                return "";
            }
            catch (Exception)
            {
                System.Console.WriteLine("Unable to get attribute for [ProductId: " + _entity.entity_id.ToString() + "][AttributeId: " + _attributeId.ToString() + "]");
                return "";
            }
        }
        private string GetVarcharAttribute(customer_address_entity _entity, int _attributeId)
        {
            try
            {
                var varcharAttribute = _entity.customer_address_entity_varchar.FirstOrDefault(_ => _.attribute_id == _attributeId);
                if (varcharAttribute != null)
                    if (varcharAttribute.value != null)
                        return varcharAttribute.value;
                return "";
            }
            catch (Exception)
            {
                System.Console.WriteLine("Unable to get attribute for [CustomerId: " + _entity.entity_id.ToString() + "][AttributeId: " + _attributeId.ToString() + "]");
                return "";
            }
        }

        #endregion

        #region Orders

        private void DeleteOrders()
        {
            var orders = this.OrderService.GetOrders();
            System.Console.WriteLine("Deleting " + orders.Count.ToString() + " orders");
            foreach (var order in orders)
                this.OrderService.DeleteOrder((long)order.Id);
            orders = this.OrderService.GetOrders();
            System.Console.WriteLine(orders.Count.ToString() + " Orders remaining");
        }

        private ShopifyOrder AddOrUpdateOrder(sales_flat_order _order, FillyFlairEntities _context)
        {
            _order.customer_email = "WilliamRNorman@Gmail.com";

            try
            {
                var now = DateTime.Now;
                var order = new ShopifyOrder()
                {
                    //Id = 0,
                    //LocationId = 0,
                    //UserId = 0,
                    //Number = 0,
                    OrderNumber = Convert.ToInt32(_order.entity_id),
                    //LandingSite = "",
                    Name = "",
                    //OrderStatusUrl = "",
                    ProcessingMethod = "",
                    ReferringSite = "",
                    Tags = "",
                    CancelReason = "",
                    CartToken = "",
                    Token = "",
                    FinancialStatus = "paid",
                    FulfillmentStatus = "fulfilled",
                    Note = "Test note about the customer.",
                    TotalLineItemsPrice = 0,
                    TotalDiscounts = 0,
                    TotalTax = 0,
                    TotalWeight = 0,
                    TaxesIncluded = false,
                    BuyerAcceptsMarketing = false,
                    ClientDetails = new ShopifyClientDetails(),
                    TaxLines = new List<ShopifyTaxLine>(),
                    ShippingLines = new List<ShopifyShippingLine>(),
                    DiscountCodes = new List<ShopifyDiscountCode>(),
                    Fulfillments = new List<ShopifyFulfillment>(),
                    NoteAttributes = new List<ShopifyNoteAttribute>(),
                    ProcessedAt = null,
                    CancelledAt = null,
                    ClosedAt = null,
                    CreatedAt = _order.created_at ?? now
                    //PaymentDetails = new ShopifyPaymentDetails(), // Depricated to be removed
                    //Transactions = new List<ShopifyTransaction>(), // Depricated to be removed
                    //ContactEmail = "", // Depricated to be removed
                    //TotalPriceUsd = 0, // Depricated to be removed
                };
                order.Email = _order.customer_email;
                order.Customer = this.CustomerService.GetCustomerWithEmail(_order.customer_firstname, _order.customer_email);
                order.BrowserIp = _order.remote_ip;
                order.Currency = _order.store_currency_code;
                order.SourceName = _order.store_name;
                order.SubtotalPrice = _order.subtotal == null ? 0 : Convert.ToDouble(_order.subtotal);
                order.TotalPrice = _order.total_paid == null ? 0 : Convert.ToDouble(_order.total_paid);
                order.ShippingAddress = this.AddOrUpdateAddress(_context.customer_address_entity.FirstOrDefault(c => c.entity_id == _order.shipping_address_id));
                order.BillingAddress = this.AddOrUpdateAddress(_context.customer_address_entity.FirstOrDefault(c => c.entity_id == _order.billing_address_id));
                order.LineItems = this.AddOrUpdateLineItems(_order.sales_flat_order_item);
                order.UpdatedAt = _order.updated_at ?? now;
                return order;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message + "[OrderId: " + _order.entity_id.ToString() + "]");
                return null;
            }
        }
        private List<ShopifyLineItem> AddOrUpdateLineItems(IEnumerable<sales_flat_order_item> _items)
        {
            try
            {
                var items = new List<ShopifyLineItem>();
                if (_items == null) throw new ApplicationException("No line items exists. ");
                foreach (var item in _items.ToList())
                {
                    var newItem = this.AddOrUpdateLineItem(item);
                    if (newItem != null) items.Add(newItem);
                }
                if (items.Count == 0) throw new ApplicationException("No line items exists. ");
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private ShopifyLineItem AddOrUpdateLineItem(sales_flat_order_item _item)
        {
            try
            {
                ShopifyProduct product = null;
                var products = this.ProductService.GetProductByTitle(_item.name);
                if (products.Count == 0) return null;
                if (products.Count > 1) product = products.FirstOrDefault(p => p.Title.Equals(_item.name, StringComparison.CurrentCultureIgnoreCase));
                var variant = product.Variants.FirstOrDefault(v => v.SKU.Equals(_item.sku, StringComparison.CurrentCultureIgnoreCase));
                if (variant == null) return null;

                var item = new ShopifyLineItem
                {
                    //Id = 0,
                    ProductId = product.Id,
                    VariantId = variant.Id,
                    Name = variant.Title,
                    Title = product.Title,
                    VariantTitle = variant.Title,
                    FulfillmentService = variant.FulfillmentService,
                    FulfillmentStatus = "Fulfilled",
                    SKU = variant.SKU,
                    Vendor = product.Vendor,
                    FulfillableQuantity = 0,
                    Quantity = Convert.ToInt32(_item.qty_ordered),
                    Grams = 0,
                    Price = Convert.ToDouble(_item.price),
                    TotalDiscount = 0,
                    RequiresShipping = variant.RequiresShipping,
                    GiftCard = false,
                    Taxable = false,
                    TaxLines = new List<ShopifyTaxLine>(),
                    Properties = new List<ShopifyLineItemProperty>()
                };
                return item;
            }
            catch (Exception ex)
            {
                return new ShopifyLineItem();
            }
        }

        #endregion

    }
}
