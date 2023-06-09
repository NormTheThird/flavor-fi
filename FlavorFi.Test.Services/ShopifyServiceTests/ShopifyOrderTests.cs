using System;
using FlavorFi.Common.Enums;
using FlavorFi.Common.RequestAndResponses.ShopifyRequestAndResponses;
using NUnit.Framework;

namespace FlavorFi.Test.Services.ShopifyServiceTests
{
    //[TestFixture]
    //public class ShopifyOrderTests
    //{
    //    public IShopifyOrderService ShopifyOrderService { get; set; }
    //    public GetShopifyRecordRequest ShopifyRecordRequest = new GetShopifyRecordRequest
    //    {
    //        CompanySiteId = Guid.Parse("97948564-E3E5-4A8D-A20E-676832D0636E"),
    //        ResourceType = ShopifyResourceType.orders,
    //        UserToken = null
    //    };

    //    public GetShopifyRecordsRequest ShopifyRecordsRequest = new GetShopifyRecordsRequest
    //    {
    //        CompanySiteId = Guid.Parse("97948564-E3E5-4A8D-A20E-676832D0636E"),
    //        ResourceType = ShopifyResourceType.orders,
    //        UserToken = null,
    //        NumberPerPage = 250
    //    };

    //    public GetShopifyRecordsPerPageRequest ShopifyRecordsPerPageRequest = new GetShopifyRecordsPerPageRequest
    //    {
    //        CompanySiteId = Guid.Parse("97948564-E3E5-4A8D-A20E-676832D0636E"),
    //        ResourceType = ShopifyResourceType.orders,
    //        UserToken = null,
    //        NumberPerPage = 250
    //    };

    //    public ShopifyOrderTests()
    //    {
    //         this.ShopifyOrderService = new ShopifyOrderService();   
    //    }

    //    [TestCase(516806213678)]
    //    public void Should_Get_Shopify_Order(long recordId)
    //    {
    //        var request = ShopifyRecordRequest;
    //        request.RecordId = recordId;
    //        var retval = this.ShopifyOrderService.GetShopifyOrder(request);
    //        Assert.IsTrue(retval.IsSuccess);
    //        Assert.AreEqual(request.RecordId, retval.Order.Id);
    //    }

    //    [Test]
    //    [Ignore("To many records so it takes to long")]
    //    public void Should_Get_Shopify_Orders()
    //    {
    //        var request = ShopifyRecordsRequest;
    //        var retval = this.ShopifyOrderService.GetShopifyOrders(request);
    //        Assert.IsTrue(retval.IsSuccess);
    //        Assert.IsTrue(retval.Orders.Count > 0);
    //    }

    //    [Test]
    //    public void Should_Get_Shopify_Orders_Per_Page()
    //    {
    //        var request = ShopifyRecordsPerPageRequest;
    //        request.PageNumber = 1;
    //        var retval = this.ShopifyOrderService.GetShopifyOrdersPerPage(request);
    //        Assert.IsTrue(retval.IsSuccess);
    //        Assert.IsTrue(retval.Orders.Count > 0);
    //    }
    //}
}
