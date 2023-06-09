namespace FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses
{
    public class ShopWorksBaseRequest
    {
        public ShopWorksBaseRequest()
        {
        }
    }

    public class ShopWorksBaseResponse
    {
        public ShopWorksBaseResponse()
        {
            this.IsSuccess = false;
            this.ErrorMessage = string.Empty;
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
