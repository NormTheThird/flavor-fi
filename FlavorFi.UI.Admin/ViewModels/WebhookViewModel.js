function WebhookActivityLogsViewModel(parent) {
    var self = this;
    self.Parent = parent;

    self.ShopifyWebhookActivityLogs = ko.observableArray();

    self.GetShopifyWebhookActivityLogs = function () {
        BaseModel.ServiceCall('/Webhook/GetShopifyWebhookActivityLogs/', "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.ShopifyWebhookActivityLogs(response.ShopifyWebhookActivityLogs.map(function (log) {
                    return new ShopifyWebhookActivityLogDataModel(log);
                }));
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.Load = function () {
        self.GetShopifyWebhookActivityLogs();
    };
}