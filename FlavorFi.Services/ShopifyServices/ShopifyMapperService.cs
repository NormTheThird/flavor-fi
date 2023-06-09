using FlavorFi.Common.Enums;
using FlavorFi.Common.Models.ShopifyModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace FlavorFi.Services.ShopifyServices
{
    public interface IShopifyMapperService
    {
        List<T> MapToList<T>(JToken _source, ShopifyResourceType _resourceType);
        T Map<T>(JToken _source);
    }

    public class ShopifyMapperService : IShopifyMapperService
    {
        public List<T> MapToList<T>(JToken _source, ShopifyResourceType _resourceType)
        {
            try
            {
                var lst = new List<T>();
                foreach (var item in _source[_resourceType.ToString()])
                {
                    var model = Map<T>(item);
                    if (model != null) lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Map<T>(JToken _source)
        {
            try
            {
                if (typeof(T) == typeof(ShopifyCustomerModel))
                    return (T)(Object)MapToShopifyCustomerModel(_source);

                else if (typeof(T) == typeof(ShopifyMetafieldModel))
                    return (T)(Object)MapToShopifyMetafieldModel(_source);

                else if (typeof(T) == typeof(ShopifyOrderModel))
                    return (T)(Object)MapToShopifyOrderModel(_source);

                else if (typeof(T) == typeof(ShopifyOrderTransactionModel))
                    return (T)(Object)MapToShopifyOrderTransactionModel(_source);

                else if (typeof(T) == typeof(ShopifyProductModel))
                    return (T)(Object)MapToShopifyProductModel(_source["product"]);

                else if (typeof(T) == typeof(ShopifyWebhookModel))
                {
                    var model = MapToObject<ShopifyWebhookModel>(_source);
                    model.IsActive = true;
                    return (T)(object)model;
                }
                else
                    return (T)(Object)MapToObject<T>(_source);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ShopifyAddressModel MapToShopifyAddressModel(JToken _address)
        {
            try
            {
                if (_address == null) return null;
                return new ShopifyAddressModel
                {
                    Id = ConvertToLong(_address["id"]),
                    CustomerId = ConvertToLong(_address["customer_id"]),
                    FirstName = ConvertToString(_address["first_name"]),
                    LastName = ConvertToString(_address["last_name"]),
                    Company = ConvertToString(_address["company"]),
                    Address1 = ConvertToString(_address["address1"]),
                    Address2 = ConvertToString(_address["address2"]),
                    City = ConvertToString(_address["city"]),
                    Province = ConvertToString(_address["province"]),
                    Country = ConvertToString(_address["country"]),
                    ZipCode = ConvertToString(_address["zip"]),
                    PhoneNumber = ConvertToString(_address["phone"]),
                    Name = ConvertToString(_address["name"]),
                    ProvinceCode = ConvertToString(_address["province_code"]),
                    CountryCode = ConvertToString(_address["country_code"]),
                    CountryName = ConvertToString(_address["country_name"]),
                    Latitude = ConvertToDecimal(_address["latitude"]),
                    Longitude = ConvertToDecimal(_address["longitude"]),
                    IsDefault = ConvertToBool(_address["default"]),
                    CreatedAt = ConvertToDateTimeOffset(_address["created_at"]),
                    UpdatedAt = ConvertToDateTimeOffset(_address["updated_at"]),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ShopifyClientDetailModel MapToShopifyClientDetailModel(JToken _client)
        {
            try
            {
                if (_client == null) return null;
                var ClientDetail = new ShopifyClientDetailModel
                {
                    BrowserIp = ConvertToString(_client["browser_ip"]),
                    AcceptLanguage = ConvertToString(_client["accept_language"]),
                    UserAgent = ConvertToString(_client["user_agent"]),
                    SessionHash = ConvertToString(_client["session_hash"]),
                    BrowserWidth = ConvertToInt(_client["browser_width"]),
                    BrowserHeight = ConvertToInt(_client["browser_height"]),
                };

                return ClientDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ShopifyCustomerModel MapToShopifyCustomerModel(JToken _customer)
        {
            try
            {
                if (_customer == null) return null;
                var customer = new ShopifyCustomerModel
                {
                    Id = ConvertToLong(_customer["id"]),
                    Email = ConvertToString(_customer["email"]),
                    AcceptsMarketing = ConvertToBool(_customer["accepts_marketing"]),
                    CreatedAt = ConvertToDateTimeOffset(_customer["created_at"]),
                    UpdatedAt = ConvertToDateTimeOffset(_customer["updated_at"]),
                    FirstName = ConvertToString(_customer["first_name"]),
                    LastName = ConvertToString(_customer["last_name"]),
                    OrderCount = ConvertToInt(_customer["orders_count"]),
                    State = ConvertToString(_customer["state"]),
                    TotalSpent = ConvertToDecimal(_customer["total_spent"]),
                    LastOrderId = ConvertToLong(_customer["last_order_id"]),
                    Note = ConvertToString(_customer["note"]),
                    HasVerifiedEmail = ConvertToBool(_customer["verified_email"]),
                    MultipassIdentifier = ConvertToString(_customer["multipass_identifier"]),
                    IsTaxExempt = ConvertToBool(_customer["tax_exempt"]),
                    PhoneNumber = ConvertToString(_customer["phone"]),
                    Tags = ConvertToString(_customer["tags"]),
                    LastOrderName = ConvertToString(_customer["last_order_name"]),
                    Addresses = new List<ShopifyAddressModel>(),
                };

                if (_customer["default_address"] != null)
                    customer.DefaultAddress = MapToShopifyAddressModel(_customer["default_address"]);

                if (_customer["addresses"] != null)
                {
                    foreach (var _address in _customer["addresses"])
                    {
                        var address = MapToShopifyAddressModel(_address);
                        if (address != null) customer.Addresses.Add(address);
                    }
                }
                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ShopifyMetafieldModel MapToShopifyMetafieldModel(JToken _metafield)
        {
            try
            {
                return new ShopifyMetafieldModel
                {
                    Id = ConvertToLong(_metafield["id"]),
                    OwnerId = ConvertToLong(_metafield["owner_id"]),
                    Namespace = ConvertToString(_metafield["namespace"]),
                    Key = ConvertToString(_metafield["key"]),
                    Value = ConvertToString(_metafield["value"]),
                    ValueType = ConvertToString(_metafield["value_type"]),
                    Description = ConvertToString(_metafield["description"]),
                    OwnerResource = ConvertToString(_metafield["owner_resource"]),
                    CreatedAt = ConvertToDateTimeOffset(_metafield["created_at"]),
                    UpdatedAt = ConvertToDateTimeOffset(_metafield["updated_at"]),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ShopifyOrderModel MapToShopifyOrderModel(JToken _order)
        {
            try
            {
                var order = new ShopifyOrderModel
                {
                    Id = ConvertToLong(_order["id"]),
                    Email = ConvertToString(_order["email"]),
                    ClosedAt = ConvertToNullableDateTimeOffset(_order["closed_at"]),
                    CreatedAt = ConvertToDateTimeOffset(_order["created_at"]),
                    UpdatedAt = ConvertToDateTimeOffset(_order["updated_at"]),
                    Number = ConvertToLong(_order["number"]),
                    Note = ConvertToString(_order["note"]),
                    Token = ConvertToString(_order["token"]),
                    Gateway = ConvertToString(_order["gateway"]),
                    Test = ConvertToString(_order["test"]),
                    TotalPrice = ConvertToDecimal(_order["total_price"]),
                    SubtotalPrice = ConvertToDecimal(_order["subtotal_price"]),
                    TotalWeight = ConvertToDecimal(_order["total_weight"]),
                    TotalTax = ConvertToDecimal(_order["total_tax"]),
                    TaxesIncluded = ConvertToBool(_order["taxes_included"]),
                    Currency = ConvertToString(_order["currency"]),
                    FinancialStatus = ConvertToString(_order["financial_status"]),
                    Confirmed = ConvertToBool(_order["confirmed"]),
                    TotalDiscounts = ConvertToDecimal(_order["total_discounts"]),
                    TotalLineItemsPrice = ConvertToDecimal(_order["total_line_items_price"]),
                    CartToken = ConvertToString(_order["cart_token"]),
                    BuyerAcceptsMarketing = ConvertToBool(_order["buyer_accepts_marketing"]),
                    Name = ConvertToString(_order["name"]),
                    ReferringSite = ConvertToString(_order["referring_site"]),
                    LandingSite = ConvertToString(_order["landing_site"]),
                    CancelledAt = ConvertToNullableDateTimeOffset(_order["cancelled_at"]),
                    CancelReason = ConvertToString(_order["cancel_reason"]),
                    TotalPriceUsd = ConvertToDecimal(_order["total_price_usd"]),
                    CheckoutToken = ConvertToString(_order["checkout_token"]),
                    Reference = ConvertToString(_order["reference"]),
                    UserId = ConvertToLong(_order["user_id"]),
                    LocationId = ConvertToLong(_order["location_id"]),
                    SourceIdentifier = ConvertToString(_order["source_identifier"]),
                    SourceUrl = ConvertToString(_order["source_url"]),
                    ProcessedAt = ConvertToNullableDateTimeOffset(_order["processed_at"]),
                    DeviceId = ConvertToString(_order["device_id"]),
                    PhoneNumber = ConvertToString(_order["phone"]),
                    CustomerLocale = ConvertToString(_order["customer_locale"]),
                    AppId = ConvertToLong(_order["app_id"]),
                    BrowserIP = ConvertToString(_order["browser_ip"]),
                    LandingSiteRef = ConvertToString(_order["landing_site_ref"]),
                    OrderNumber = ConvertToLong(_order["order_number"]),
                    DiscountCodes = new List<ShopifyOrderDiscountModel>(),
                    NoteAttributes = MapToDictionary(_order["note_attributes"]),
                    PaymentGatewayNames = MapToListOfString(_order["payment_gateway_names"]),
                    ProcessingMethod = ConvertToString(_order["processing_method"]),
                    CheckoutId = ConvertToLong(_order["checkout_id"]),
                    SourceName = ConvertToString(_order["source_name"]),
                    FulfillmentStatus = ConvertToString(_order["fulfillment_status"]),
                    TaxLines = new List<ShopifyTaxLineModel>(),
                    Tags = ConvertToString(_order["tags"]),
                    ContactEmail = ConvertToString(_order["contact_email"]),
                    OrderStatusUrl = ConvertToString(_order["order_status_url"]),
                    LineItems = new List<ShopifyOrderLineItemModel>(),
                    ShippingLines = new List<ShopifyOrderShippingLineModel>(),
                    BillingAddress = MapToShopifyAddressModel(_order["billing_address"]),
                    ShippingAddress = MapToShopifyAddressModel(_order["shipping_address"]),
                    Fulfillments = new List<ShopifyOrderFulfillmentModel>(),
                    ClientDetails = MapToShopifyClientDetailModel(_order["client_details"]),
                    Refunds = new List<ShopifyRefundModel>(),
                    PaymentDetails = MapToShopifyPaymentDetailModel(_order["payment_details"]),
                    Customer = MapToShopifyCustomerModel(_order["customer"])
                };

                if (_order["discount_codes"] != null)
                {
                    foreach (var _discount in _order["discount_codes"])
                    {
                        var discount = MapToShopifyOrderDiscountModel(_discount);
                        if (discount != null) order.DiscountCodes.Add(discount);
                    }
                }

                if (_order["tax_lines"] != null)
                {
                    foreach (var _tax in _order["tax_lines"])
                    {
                        var tax = MapToShopifyTaxLineModel(_tax);
                        if (tax != null) order.TaxLines.Add(tax);
                    }
                }

                if (_order["line_items"] != null)
                {
                    foreach (var _line in _order["line_items"])
                    {
                        var line = MapToShopifyOrderLineItemModel(_line);
                        if (line != null) order.LineItems.Add(line);
                    }
                }

                if (_order["shipping_lines"] != null)
                    foreach (var _shipping in _order["shipping_lines"])
                        order.ShippingLines.Add(MapToObject<ShopifyOrderShippingLineModel>(_shipping));

                if (_order["fulfillments"] != null)
                {
                    foreach (var _fulfillment in _order["fulfillments"])
                    {
                        var fulfillment = MapToShopifyOrderFulfillmentModel(_fulfillment);
                        if (fulfillment != null) order.Fulfillments.Add(fulfillment);
                    }
                }

                if (_order["refunds"] != null)
                {
                    foreach (var _refund in _order["refunds"])
                    {
                        var refund = MapToShopifyRefundModel(_refund);
                        if (refund != null) order.Refunds.Add(refund);
                    }
                }

                return order;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ShopifyOrderLineItemModel MapToShopifyOrderLineItemModel(JToken _item)
        {
            try
            {
                return new ShopifyOrderLineItemModel
                {
                    Id = ConvertToLong(_item["id"]),
                    ProductId = ConvertToLong(_item["product_id"]),
                    VariantId = ConvertToLong(_item["variant_id"]),
                    Sku = ConvertToString(_item["sku"]),
                    Name = ConvertToString(_item["name"]),
                    Title = ConvertToString(_item["title"]),
                    VariantTitle = ConvertToString(_item["variant_title"]),
                    VariantInventoryManagement = ConvertToString(_item["variant_inventory_management"]),
                    Vendor = ConvertToString(_item["vendor"]),
                    FulfillmentService = ConvertToString(_item["fulfillment_service"]),
                    FulfillmentStatus = ConvertToString(_item["fulfillment_status"]),
                    FulfillmentQuantity = ConvertToInt(_item["fulfillable_quantity"]),
                    Quantity = ConvertToInt(_item["quantity"]),
                    Grams = ConvertToInt(_item["grams"]),
                    Price = ConvertToDecimal(_item["price"]),
                    PreTaxPrice = ConvertToDecimal(_item["pre_tax_price"]),
                    TotalDiscount = ConvertToDecimal(_item["total_discount"]),
                    RequiresShipping = ConvertToBool(_item["requires_shipping"]),
                    IsTaxable = ConvertToBool(_item["taxable"]),
                    IsGiftCard = ConvertToBool(_item["gift_card"]),
                    ProductExists = ConvertToBool(_item["product_exists"])
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyOrderTransactionModel MapToShopifyOrderTransactionModel(JToken _transaction)
        {
            try
            {
                var transaction = new ShopifyOrderTransactionModel
                {
                    Id = ConvertToLong(_transaction["id"]),
                    OrderId = ConvertToLong(_transaction["order_id"]),
                    GiftCardId = ConvertToLong(_transaction["receipt"]["gift_card_id"]),
                    LocationId = ConvertToLong(_transaction["location_id"]),
                    UserId = ConvertToLong(_transaction["user_id"]),
                    ParentId = ConvertToLong(_transaction["parent_id"]),
                    DeviceId = ConvertToLong(_transaction["device_id"]),
                    Kind = ConvertToString(_transaction["kind"]),
                    Gateway = ConvertToString(_transaction["gateway"]),
                    Status = ConvertToString(_transaction["status"]),
                    Message = ConvertToString(_transaction["message"]),
                    Currency = ConvertToString(_transaction["currency"]),
                    Authorization = ConvertToString(_transaction["authorization"]),
                    ErrorCode = ConvertToString(_transaction["error_code"]),
                    SourceName = ConvertToString(_transaction["source_name"]),
                    Amount = ConvertToDecimal(_transaction["amount"]),
                    Test = ConvertToBool(_transaction["test"]),
                    CreatedAt = ConvertToDateTimeOffset(_transaction["created_at"]),
                };

                return transaction;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyOrderDiscountModel MapToShopifyOrderDiscountModel(JToken _discount)
        {
            try
            {
                return new ShopifyOrderDiscountModel
                {
                    Amount = ConvertToDecimal(_discount["amount"]),
                    Code = ConvertToString(_discount["code"]),
                    Type = ConvertToString(_discount["type"]),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyOrderFulfillmentModel MapToShopifyOrderFulfillmentModel(JToken _fulfillment)
        {
            try
            {
                return new ShopifyOrderFulfillmentModel
                {
                    Id = ConvertToLong(_fulfillment["id"]),
                    OrderId = ConvertToLong(_fulfillment["order_id"]),
                    Status = ConvertToString(_fulfillment["status"]),
                    TrackingCompany = ConvertToString(_fulfillment["tracking_company"]),
                    TrackingNumber = ConvertToString(_fulfillment["tracking_number"]),
                    CreatedAt = ConvertToDateTimeOffset(_fulfillment["created_at"]),
                    UpdatedAt = ConvertToDateTimeOffset(_fulfillment["updated_at"]),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyPaymentDetailModel MapToShopifyPaymentDetailModel(JToken _payment)
        {
            try
            {
                if (_payment == null)
                    return null;
                return new ShopifyPaymentDetailModel
                {
                    AVSResultCode = ConvertToString(_payment["avs_result_code"]),
                    CreditCardBin = ConvertToString(_payment["credit_card_bin"]),
                    CVVResultCode = ConvertToString(_payment["cvv_result_code"]),
                    CreditCardNumber = ConvertToString(_payment["credit_card_number"]),
                    CreditCardCompany = ConvertToString(_payment["credit_card_company"]),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ShopifyProductModel MapToShopifyProductModel(JToken _product)
        {
            try
            {
                var product = new ShopifyProductModel
                {
                    Id = ConvertToLong(_product["id"]),
                    Title = ConvertToString(_product["title"]),
                    BodyHtml = ConvertToString(_product["body_html"]),
                    Vendor = ConvertToString(_product["vendor"]),
                    ProductType = ConvertToString(_product["product_type"]),
                    Handle = ConvertToString(_product["handle"]),
                    TemplateSuffix = ConvertToString(_product["template_suffix"]),
                    PublishedScope = ConvertToString(_product["published_scope"]),
                    Tags = ConvertToString(_product["tags"]),
                    CreatedAt = ConvertToDateTimeOffset(_product["created_at"]),
                    UpdatedAt = ConvertToDateTimeOffset(_product["updated_at"]),
                    PublishedAt = ConvertToDateTimeOffset(_product["published_at"]),
                    Image = MapToShopifyProductImageModel(_product["image"])
                };

                foreach (var _variant in _product["variants"])
                {
                    var variant = MapToShopifyProductVariantModel(_variant);
                    if (variant != null) product.Variants.Add(variant);
                }

                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyProductVariantModel MapToShopifyProductVariantModel(JToken _product)
        {
            try
            {
                return new ShopifyProductVariantModel
                {
                    Id = ConvertToLong(_product["id"]),
                    ProductId = ConvertToLong(_product["product_id"]),
                    ImageId = ConvertToLong(_product["image_id"]),
                    InventoryItemId = ConvertToLong(_product["inventory_item_id"]),
                    Title = ConvertToString(_product["title"]),
                    Sku = ConvertToString(_product["sku"]),
                    InventoryPolicy = ConvertToString(_product["inventory_policy"]),
                    InventoryManagement = ConvertToString(_product["inventory_management"]),
                    FulfillmentService = ConvertToString(_product["fulfillment_service"]),
                    Option1 = ConvertToString(_product["option1"]),
                    Option2 = ConvertToString(_product["option2"]),
                    Option3 = ConvertToString(_product["option3"]),
                    Barcode = ConvertToString(_product["barcode"]),
                    WeightUnit = ConvertToString(_product["weight_unit"]),
                    Position = ConvertToInt(_product["position"]),
                    Grams = ConvertToInt(_product["grams"]),
                    InventoryQuantity = ConvertToInt(_product["inventory_quantity"]),
                    OldInventoryQuantity = ConvertToInt(_product["old_inventory_quantity"]),
                    Price = ConvertToDecimal(_product["price"]),
                    CompareAtPrice = ConvertToDecimal(_product["compare_at_price"]),
                    Weight = ConvertToDecimal(_product["weight"]),
                    Taxable = ConvertToBool(_product["taxable"]),
                    RequiresShipping = ConvertToBool(_product["requires_shipping"]),
                    CreatedAt = ConvertToDateTimeOffset(_product["created_at"]),
                    UpdatedAt = ConvertToDateTimeOffset(_product["updated_at"])
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyProductImageModel MapToShopifyProductImageModel(JToken _image)
        {
            try
            {
                return new ShopifyProductImageModel
                {
                    Id = ConvertToLong(_image["id"]),
                    ProductId = ConvertToLong(_image["product_id"]),
                    Src = ConvertToString(_image["src"]),
                    CreatedAt = ConvertToDateTimeOffset(_image["created_at"]),
                    UpdatedAt = ConvertToDateTimeOffset(_image["updated_at"])
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyRefundModel MapToShopifyRefundModel(JToken _refund)
        {
            try
            {
                var refund = new ShopifyRefundModel
                {
                    Id = ConvertToLong(_refund["id"]),
                    UserId = ConvertToLong(_refund["user_id"]),
                    Note = ConvertToString(_refund["note"]),
                    Restock = ConvertToBool(_refund["restock"]),
                    ProcessedAt = ConvertToDateTimeOffset(_refund["processed_at"]),
                    CreatedAt = ConvertToDateTimeOffset(_refund["created_at"]),
                };

                foreach (var _line in _refund["refund_line_items"])
                {
                    var line = MapToShopifyRefundLineItemModel(_line);
                    if (line != null) refund.LineItems.Add(line);
                }

                foreach (var _transaction in _refund["transactions"])
                {
                    var transaction = MapToShopifyTransactionModel(_transaction);
                    if (transaction != null) refund.Transactions.Add(transaction);
                }

                return refund;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyRefundLineItemModel MapToShopifyRefundLineItemModel(JToken _refundItem)
        {
            try
            {
                return new ShopifyRefundLineItemModel
                {
                    Id = ConvertToLong(_refundItem["id"]),
                    LineItemId = ConvertToLong(_refundItem["line_item_id"]),
                    Quantity = ConvertToInt(_refundItem["quantity"]),
                    LineItem = MapToShopifyOrderLineItemModel(_refundItem["line_item"])
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyTaxLineModel MapToShopifyTaxLineModel(JToken _taxLine)
        {
            try
            {
                return new ShopifyTaxLineModel
                {
                    Price = ConvertToDecimal(_taxLine["price"]),
                    Rate = ConvertToDecimal(_taxLine["rate"]),
                    Title = ConvertToString(_taxLine["title"])
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ShopifyTransactionModel MapToShopifyTransactionModel(JToken _transaction)
        {
            try
            {
                return new ShopifyTransactionModel
                {
                    Id = ConvertToLong(_transaction["id"]),
                    Amount = ConvertToString(_transaction["amount"]),
                    AuthorizationCode = ConvertToString(_transaction["authorization"]),
                    DeviceId = ConvertToLong(_transaction["device_id"]),
                    Gateway = ConvertToString(_transaction["gateway"]),
                    SourceName = ConvertToString(_transaction["source_name"]),
                    PaymentDetails = MapToShopifyPaymentDetailModel(_transaction["payment_details"]),
                    Kind = ConvertToString(_transaction["kind"]),
                    OtherId = ConvertToLong(_transaction["order_id"]),
                    ErrorCode = ConvertToString(_transaction["error_code"]),
                    Status = ConvertToString(_transaction["status"]),
                    Test = ConvertToBool(_transaction["test"]),
                    UserId = ConvertToLong(_transaction["user_id"]),
                    Currency = ConvertToString(_transaction["currency"]),
                    CreatedAt = ConvertToDateTimeOffset(_transaction["created_at"]),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Maps the shopify object to the appropriate model object
        /// </summary>
        /// <typeparam name="T">The model type to map too.</typeparam>
        /// <param name="item">The JToken object</param>
        /// <returns>T</returns>
        private static T MapToObject<T>(JToken item)
        {
            try
            {
                var model = Activator.CreateInstance(typeof(T));
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    var customAttributes = property.GetCustomAttributes(true);
                    foreach (var attribute in customAttributes)
                    {
                        if (attribute.GetType() == typeof(JsonPropertyAttribute))
                        {
                            var jsonAttirbute = (JsonPropertyAttribute)attribute;
                            if (property.PropertyType == typeof(string))
                                property.SetValue(model, ConvertToString(item[jsonAttirbute.PropertyName]));
                            else if (property.PropertyType == typeof(bool))
                                property.SetValue(model, ConvertToBool(item[jsonAttirbute.PropertyName]));
                            else if (property.PropertyType == typeof(long))
                                property.SetValue(model, ConvertToLong(item[jsonAttirbute.PropertyName]));
                            else if (property.PropertyType == typeof(int))
                                property.SetValue(model, ConvertToInt(item[jsonAttirbute.PropertyName]));
                            else if (property.PropertyType == typeof(DateTimeOffset))
                                property.SetValue(model, ConvertToDateTimeOffset(item[jsonAttirbute.PropertyName]));
                            continue;
                        }
                    }
                }
                return (T)model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Converts the jtoken field array to a dictionary
        /// </summary>
        /// <param name="_values">the jtoken field array</param>
        /// <returns>Dictionary<string, string></returns>
        private static Dictionary<string, string> MapToDictionary(JToken _values)
        {
            try
            {
                var dictionary = new Dictionary<string, string>();
                foreach (var value in _values) dictionary.Add(ConvertToString(value[0]), ConvertToString(value[1]));
                return dictionary;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Converts the jtoken field array to a list of string
        /// </summary>
        /// <param name="_values">the jtoken field array</param>
        /// <returns>List<string></returns>
        private static List<string> MapToListOfString(JToken _values)
        {
            try
            {
                var lst = new List<string>();
                foreach (var value in _values) lst.Add(ConvertToString(value[0]));
                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Converts the jtoken field to an int
        /// </summary>
        /// <param name="_int">the jtoken field of type int</param>
        /// <returns>int</returns>
        private static int ConvertToInt(JToken _int)
        {
            try
            {
                if (_int == null) return 0;
                return Convert.ToInt32(_int);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        ///     Converts the jtoken field to a long
        /// </summary>
        /// <param name="_long">the jtoken field of type long</param>
        /// <returns>Int64</returns>
        private static Int64 ConvertToLong(JToken _long)
        {
            try
            {
                if (_long == null) return 0;
                return Convert.ToInt64(_long);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        ///     Converts the jtoken field to a bool
        /// </summary>
        /// <param name="_bool">the jtoken field of type bool</param>
        /// <returns>bool</returns>
        private static bool ConvertToBool(JToken _bool)
        {
            try
            {
                if (_bool == null) return false;
                return Convert.ToBoolean(_bool);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Converts the jtoken field to a string
        /// </summary>
        /// <param name="_string">the jtoken field of type string</param>
        /// <returns>string</returns>
        private static string ConvertToString(JToken _string)
        {
            try
            {
                if (_string == null) return "";
                return Convert.ToString(_string);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        ///     Converts the jtoken field to a decimal
        /// </summary>
        /// <param name="_decimal">the jtoken field of type decimal</param>
        /// <returns>decimal</returns>
        private static decimal ConvertToDecimal(JToken _decimal)
        {
            try
            {
                if (_decimal == null) return 0.0m;
                return Convert.ToDecimal(_decimal);
            }
            catch (Exception)
            {
                return 0.0m;
            }
        }

        /// <summary>
        ///     Converts the jtoken field to a datetime offset
        /// </summary>
        /// <param name="_dateTime">the jtoken field to convert</param>
        /// <returns>DateTimeOFfset</returns>
        private static DateTimeOffset ConvertToDateTimeOffset(JToken _dateTime)
        {
            try
            {
                if (_dateTime == null) return DateTimeOffset.Parse("1/1/1980");
                return DateTimeOffset.Parse(_dateTime.ToString());
            }
            catch (Exception)
            {
                return DateTimeOffset.Parse("1/1/1980");
            }
        }

        /// <summary>
        ///     Converts the jtoken field to a nullable datetime offset
        /// </summary>
        /// <param name="_dateTime">the jtoken field to convert</param>
        /// <returns>DateTimeOffset?</returns>
        private static DateTimeOffset? ConvertToNullableDateTimeOffset(JToken _dateTime)
        {
            try
            {
                if (_dateTime == null) return null;
                return DateTimeOffset.Parse(_dateTime.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}