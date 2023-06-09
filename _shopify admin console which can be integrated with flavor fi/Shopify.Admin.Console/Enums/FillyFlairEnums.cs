using System.ComponentModel;

namespace Shopify.Admin.Console.Enums
{
    //public enum ProductType
    //{
    //    Unknown,
    //    Accessories,
    //    Activewear,
    //    Basics,
    //    Bottoms,
    //    BridesmaidDress,
    //    CasualDress,
    //    FloralDress,
    //    HighLowDress,
    //    LaceDress,
    //    LongSleeveDress,
    //    MaxiDress,
    //    MiniDress,
    //    PrintDress,
    //    ShortDress,
    //    StraplessDress,
    //    GiftCards,
    //    Kids,
    //    Outerwear,
    //    Tops,
    //    Shoes
    //}

    public enum ProductType
    {
        Unknown,
        Accessories,
        Activewear,
        Basics,
        Bottoms,
        Dresses,
        GiftCards,
        Kids,
        Outerwear,
        Tops,
        Shoes
    }

    public enum MetaFieldResourceType
    {
        [Description("Blogs")]
        Blogs,
        [Description("customers")]
        Customers,
        [Description("Custom Collections")]
        CustomCollections,
        [Description("Orders")]
        Orders,
        [Description("Pages")]
        Pages,
        [Description("products")]
        Products,
        [Description("Product Variants")]
        ProductVariants        
    }
}