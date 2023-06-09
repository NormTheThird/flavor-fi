function BaseDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Id = ko.observable(data.Id || BaseModel.Guid.Empty);
    self.CompanyId = ko.observable(data.CompanyId || BaseModel.Guid.Empty);
    self.DateCreated = ko.observable(BaseModel.ToDate(data.DateCreated) || false);
    self.TimeCreated = ko.observable(BaseModel.ToTime(data.DateCreated) || false);
}

function BaseSiteDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new BaseDataModel(data));

    self.CompanySiteId = ko.observable(data.CompanySiteId || BaseModel.Guid.Empty);
}

function AccountDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new ActiveSiteDataModel(data));

    self.AddressId = ko.observable(data.AddressId || BaseModel.Guid.Empty);
    self.Address = ko.observable(new AddressDataModel(data.Address));
    self.AWSProfileImageId = ko.observable(data.AWSProfileImageId || BaseModel.Guid.Empty);
    self.FirstName = ko.observable(data.FirstName || "");
    self.LastName = ko.observable(data.LastName || "");
    self.Email = ko.observable(data.Email || "");
    self.PhoneNumber = ko.observable(data.PhoneNumber || "");
    self.AltPhoneNumber = ko.observable(data.AltPhoneNumber || "");
    self.AllowedOrigin = ko.observable(data.AllowedOrigin || "");
    self.RefreshTokenLifeTimeMinutes = ko.observable(data.RefreshTokenLifeTimeMinutes || "");
    self.IsCompanyAdmin = ko.observable(data.IsCompanyAdmin || "");
    self.DateOfBirth = ko.observable(BaseModel.ToDate(data.DateOfBirth) || false);

    self.ProfileImage = ko.computed(function () {
        if (self.AWSProfileImageId() === BaseModel.Guid.Empty) { return ""; }
        return BaseModel.AmazonS3Path("profile") + self.AWSProfileImageId();
    });
}

function AccountRefreshTokenDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Id = ko.observable(data.Id || BaseModel.Guid.Empty);
    self.RefreshToken = ko.observable(data.RefreshToken || "");
    self.DateIssued = ko.observable(BaseModel.ToDate(data.DateIssued) || false);
    self.DateExpired = ko.observable(BaseModel.ToDate(data.DateExpired) || false);
}

function ActiveDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new BaseDataModel(data));

    self.IsActive = ko.observable(data.IsActive || false);
}

function ActiveSiteDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new BaseSiteDataModel(data));

    self.IsActive = ko.observable(data.IsActive || false);
}

function AddressDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new BaseDataModel(data));

    self.Address1 = ko.observable(data.Address1 || "");
    self.Address2 = ko.observable(data.Address2 || "");
    self.City = ko.observable(data.City || "");
    self.State = ko.observable(data.State || "");
    self.ZipCode = ko.observable(data.ZipCode || "");
    self.Latitude = ko.observable(data.Latitude || 0);
    self.Longitude = ko.observable(data.Longitude || 0);

    self.FullAddress = ko.computed(function () {
        var retval = (self.Address1() + " " + self.Address2()).trim();
        if (retval !== "")
            retval += ", ";
        retval += self.City() + ", " + self.State() + " " + self.ZipCode();
        if (self.City() === "")
            retval = self.State() + " " + self.ZipCode();
        return retval.trim();
    });

    self.CityStateZipDisplay = ko.computed(function () {
        var retval = self.City() + ", " + self.State() + " " + self.ZipCode();
        if (self.City() === "")
            retval = self.State() + " " + self.ZipCode();
        return retval.trim();
    });
}

function AdminAccountListDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new ActiveDataModel(data));

    self.AWSProfileImageId = ko.observable(data.AWSProfileImageId || BaseModel.Guid.Empty);
    self.FullName = ko.observable(data.FullName || "");
    self.CompanyName = ko.observable(data.CompanyName || "");
    self.Email = ko.observable(data.Email || "");
    self.PhoneNumber = ko.observable(data.PhoneNumber || "");
    self.AltPhoneNumber = ko.observable(data.AltPhoneNumber || "");
    self.IsCompanyAdmin = ko.observable(data.IsCompanyAdmin || "");
}

function CompanyDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new ActiveDataModel(data));

    self.Name = ko.observable(data.Name || "");
    self.Address1 = ko.observable(data.Address1 || "");
    self.Address2 = ko.observable(data.Address2 || "");
    self.City = ko.observable(data.City || "");
    self.State = ko.observable(data.State || "");
    self.ZipCode = ko.observable(data.ZipCode || "");

    self.AddressDisplay = ko.computed(function () {
        return self.Address1() + " " + self.Address2();
    });

    self.CityStateZipDisplay = ko.computed(function () {
        var retval = self.City() + ", " + self.State() + " " + self.ZipCode();
        if (self.City() === "")
            retval = self.State() + " " + self.ZipCode();
        return retval.trim();
    });
}

function CompanySiteDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Id = ko.observable(data.Id || BaseModel.Guid.Empty);
    self.CompanyId = ko.observable(data.CompanyId || BaseModel.Guid.Empty);
    self.Name = ko.observable(data.Name || "");
    self.ShopifyUrl = ko.observable(data.ShopifyUrl || "");
    self.ShopifyWebhookUrl = ko.observable(data.ShopifyWebhookUrl || "");
    self.ShopifyDomain = ko.observable(data.ShopifyDomain || "");
    self.ShopifyApiPublicKey = ko.observable(data.ShopifyApiPublicKey || "");
    self.ShopifyApiSecretKey = ko.observable(data.ShopifyApiSecretKey || "");
    self.ShopifySharedSecret = ko.observable(data.ShopifySharedSecret || "");
    self.IsActive = ko.observable(data.IsActive || false);
    self.DateCreated = ko.observable(BaseModel.ToDate(data.DateCreated) || false);
}

function DatabaseStatusModel(data) {
    var self = this;
    if (!data) data = {};

    self.SyncStatus = ko.observable(data.SyncStatus || "");
    self.SyncPercentage = ko.observable(data.SyncPercentage || 0);
    self.IsSyncing = ko.observable(data.IsSyncing || false);
    self.LastSynced = ko.observable(data.DateCreated || "Never");
}

function ErrorDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new BaseDataModel(data));

    self.Source = ko.observable(data.Source || "");
    self.ExceptionType = ko.observable(data.ExceptionType || "");
    self.ExceptionMessage = ko.observable(data.ExceptionMessage || "");
    self.InnerExceptionMessage = ko.observable(data.InnerExceptionMessage || "");
    self.StackTrace = ko.observable(data.StackTrace || "");
    self.IsReviewed = ko.observable(data.IsReviewed || false);
    self.ReviewedBy = ko.observable(data.ReviewedBy || "");
    self.DateReviewed = ko.observable(BaseModel.ToDate(data.DateReviewed) || "");
}

function GiftCardTotalsDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.GiftCardTotalAmount = ko.observable(data.GiftCardTotalAmount || 0);
    self.GiftCardAmountFromReturns = ko.observable(data.GiftCardAmountFromReturns || 0);
    self.RemainingTotalBalance = ko.observable(data.RemainingTotalBalance || 0);
    self.RemainingReturnBalance = ko.observable(data.RemainingReturnBalance || 0);
}

function GiftCardMonthlyTotalsDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Month = ko.observable(data.Month || 0);
    self.GiftCardTotalAmount = ko.observable(data.GiftCardTotalAmount || 0);
    self.GiftCardAmountFromReturns = ko.observable(data.GiftCardAmountFromReturns || 0);
}

function ModuleDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new ActiveDataModel(data));

    self.Name = ko.observable(data.Name || "");
    self.DisplayName = ko.observable(data.DisplayName || "");
    self.Description = ko.observable(data.Description || "");
}

function NewAccountDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Email = ko.observable(data.Email || "");
    self.Password = ko.observable(data.Username || "");
}

function ShopifyWebhookActivityLogDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new BaseDataModel(data));

    self.Topic = ko.observable(data.Topic || "");
    self.Activity = ko.observable(data.Activity || "");
    self.RecordId = ko.observable(data.RecordId || 0);
}

function OrdersForPickupDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.ShopifyOrderId = ko.observable(data.ShopifyOrderId || BaseModel.Guid.Empty);
    self.OrderNumber = ko.observable(data.OrderNumber || 0);
    self.OrderDate = ko.observable(BaseModel.ToDate(data.OrderDate) || false);
    self.FullName = ko.observable(data.FullName || "");
}

function OrderItemDataModel(data) {
    var self = this;
    if (data == null) data = {
        "Id": 0,
        "Name": "",
        "Image": "",
        "Sku": "",
        "Quantity": 0,
        "CheckedQuantity": 0,
        "Price": 0
    };
    self.Id = ko.observable(data.Id || BaseModel.Guid.Empty);
    self.Name = ko.observable(data.Name);
    self.Image = ko.observable(data.Image);
    self.Sku = ko.observable(data.Sku);
    self.Quantity = ko.observable(data.Quantity);
    self.CheckedQuantity = ko.observable(0);
    self.Price = ko.observable(data.Price);

    self.QuantityDisplay = ko.computed(function () {
        return self.CheckedQuantity() + "/" + self.Quantity();
    });

    self.PriceDisplay = ko.computed(function () {
        var num = self.Price();
        var p = num.toFixed(2).split(".");
        return "$" + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
            return num === "-" ? acc : num + (i && !(i % 3) ? "," : "") + acc;
        }, "") + "." + p[1];
    });
}

function PushNotificationDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Id = ko.observable(data.Id || BaseModel.Guid.Empty);
    self.CreatedByName = ko.observable(data.CreatedByName || "");
    self.Title = ko.observable(data.Title || "");
    self.Message = ko.observable(data.Message || "");
    self.Note = ko.observable(data.Note || "");
    self.DateCreated = ko.observable(BaseModel.ToDate(data.DateCreated));
    self.Devices = ko.observableArray(data.Devices ? data.Devices.map(function (device) {
        return new PushNotificationDeviceDataModel(device);
    }) : []);
}

function PushNotificationDeviceDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Id = ko.observable(data.Id || BaseModel.Guid.Empty);
    self.PushNotifcationId = ko.observable(data.PushNotifcationId || BaseModel.Guid.Empty);
    self.Device = ko.observable(data.Device || "");
    self.Payload = ko.observable(data.Payload || "");
    self.Response = ko.observable(data.Response || "");
    self.IsSuccess = ko.observable(data.IsSuccess || false);
    self.IsError = ko.observable(data.IsError || false);
    self.DateCreated = ko.observable(data.DateCreated || false);
}

function WebhookDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Id = ko.observable(data.Id || 0);
    self.Url = ko.observable(data.Url || "");
    self.Event = ko.observable(data.Event || "");
    self.Topic = ko.observable(data.Topic || "");
    self.Format = ko.observable(data.Format || "");
    self.IsActive = ko.observable(data.IsActive || false);
    self.CreatedAt = ko.observable(data.CreatedAt || "");
    self.UpdatedAt = ko.observable(data.UpdatedAt || "");
}




function ShopifyBaseDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Id = ko.observable(data.Id || BaseModel.Guid.Empty);
    self.UpdatedAt = ko.observable(data.UpdatedAt || false);
    self.CreatedAt = ko.observable(BaseModel.ToDate(data.CreatedAt) || false);
}

function ShopifyAddressDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new BaseDataModel(data));

    self.FirstName = ko.observable(data.FirstName || "");
    self.LastName = ko.observable(data.LastName || "");
    self.Address1 = ko.observable(data.Address1 || "");
    self.Address2 = ko.observable(data.Address2 || "");
    self.City = ko.observable(data.City || "");
    self.Province = ko.observable(data.Province || "");
    self.ProvinceCode = ko.observable(data.ProvinceCode || "");
    self.ZipCode = ko.observable(data.ZipCode || "");
    self.Country = ko.observable(data.Country || "");
    self.CountryCode = ko.observable(data.CountryCode || "");
    self.CountryName = ko.observable(data.CountryName || "");
    self.Latitude = ko.observable(data.Latitude || 0);
    self.Longitude = ko.observable(data.Longitude || 0);

    self.CityStateZipDisplay = ko.computed(function () {
        var retval = self.City() + ", " + self.Province() + " " + self.ZipCode();
        if (self.City() === "")
            retval = self.Province() + " " + self.ZipCode();
        return retval.trim();
    });
}

function ShopifyCustomerBaseDataModel(data) {
    var self = this;
    if (!data) data = {};
    $.extend(self, new ShopifyBaseDataModel(data));

    self.FirstName = ko.observable(data.FirstName || "");
    self.LastName = ko.observable(data.LastName || "");
    self.Email = ko.observable(data.Email || "");
    self.PhoneNumber = ko.observable(data.PhoneNumber || "");

    self.DisplayName = ko.computed(function () {
        return self.FirstName() + ' ' + self.LastName();
    });
}

function ShopifyOrderItemDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.Id = ko.observable(data.Id || BaseModel.Guid.Empty);
    self.Name = ko.observable(data.Name);
    self.Image = ko.observable(data.Image);
    self.Sku = ko.observable(data.Sku);
    self.NumberOf = ko.observable(data.NumberOf);
    self.Quantity = ko.observable(data.Quantity);
    self.Price = ko.observable(data.Price);
    self.IsFulfilled = ko.observable(false);

    self.QuantityDisplay = ko.computed(function () {
        return self.NumberOf() + "/" + self.Quantity();
    });

    self.PriceDisplay = ko.computed(function () {
        var num = self.Price();
        var p = num.toFixed(2).split(".");
        return "$" + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
            return num === "-" ? acc : num + (i && !(i % 3) ? "," : "") + acc;
        }, "") + "." + p[1];
    });
}

function ShopifyQualityCheckOrderDataModel(data) {
    var self = this;
    if (!data) data = {};

    self.OrderNumber = ko.observable(data.OrderNumber);
    self.OrderItems = ko.observableArray($.map(data.OrderItems, function (item) {
        return new ShopifyOrderItemDataModel(item);
    }));
    self.Customer = ko.observable(new ShopifyCustomerBaseDataModel(data.Customer));
    self.ShippingAddress = ko.observable(new ShopifyAddressDataModel(data.ShippingAddress));
    self.IsTrendsetter = ko.observable(data.IsTrendsetter || false);
}