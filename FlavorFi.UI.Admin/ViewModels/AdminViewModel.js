function AccountAdminViewModel(parent) {
    var self = this;
    self.Parent = parent;
    self.CurrentPage = ko.observable("");

    self.Account = ko.observable(new AccountDataModel());
    self.AccountRefreshToken = ko.observable(new AccountRefreshTokenDataModel());
    self.NewAccount = ko.observable(new NewAccountDataModel());
    self.Accounts = ko.observableArray();
    self.Password = ko.observable("");

    self.SendEmail = ko.computed(function () {
        try {
            return "mailto:" + self.Account().Email();
        }
        catch (ex) {
            console.log(ex);
            return "mailto:" + self.Account().Email;
        }
    });

    self.GetAdminAccounts = function () {
        BaseModel.ServiceCall("/Admin/GetAdminAccounts", "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.Accounts(response.Accounts.map(function (account) {
                    return new AdminAccountListDataModel(account);
                }));
                self.Show("list");
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.GetAccount = function (id) {
        var data = { AccountId: id };
        BaseModel.ServiceCall("/Admin/GetAccount", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.Account(new AccountDataModel(response.Account));
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.GetAccountActivity = function (id) {
        var data = { AccountId: id };
        BaseModel.ServiceCall("/AccountAdmin/GetAccountActivity", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.Activity(response.AccountActivity.map(function (activity) {
                    return new AccountActivityDataModel(activity);
                }));
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.EditAccount = function (account) {
        self.GetAccount(account.Id);
        //self.GetAccountActivity(account.Id);
        self.Show("edit");
    };

    self.GoBackToAccounts = function () {
        self.Show("list");
    };

    self.SaveAccount = function () {
        var data = { Account: ko.toJS(self.Account()) };
        BaseModel.ServiceCall("/AccountAdmin/SaveAccount", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                BaseModel.Message("Account saved successfully.", BaseModel.MessageLevels.Success);
                self.GetAccount();
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.RemoveImage = function () {
        self.Account().AWSProfileImageId(BaseModel.Guid.Empty);
    };

    self.SaveImage = function (image) {
        AWS.config.region = BaseModel.AmazonS3Config.region;
        AWS.config.accessKeyId = BaseModel.AmazonS3Config.accessKeyId;
        AWS.config.secretAccessKey = BaseModel.AmazonS3Config.secretAccessKey;
        var s3 = new AWS.S3();
        var imageKey = image.upload.uuid;

        s3.upload({
            Key: imageKey,
            Body: image,
            Bucket: BaseModel.AmazonS3Bucket("profile"),
            ContentType: image.type
        }, function (err) {
            if (err) { console.error(err); }

            var data = { AccountId: self.Account().Id(), AWSProfileImageId: imageKey };
            BaseModel.ServiceCall("/AccountAdmin/SaveAccountProfileImage", "post", data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage; }
                    BaseModel.Message("Your profile image saved successfully.", BaseModel.MessageLevels.Success);
                    self.GetAccount();
                } catch (ex) {
                    BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                    BaseModel.Log(ex);
                }
            });
        });
    };

    self.SaveNewAccount = function () {
        var data = {
            Email: self.NewAccount().Email(),
            Password: self.NewAccount().Password(),
            IsArtist: self.NewAccount().IsArtist()
        };
        BaseModel.ServiceCall("/AccountAdmin/SaveNewAccount", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.NewAccount(new NewAccountDataModel());
                self.GetAccounts();
                BaseModel.Message("New account has been created", BaseModel.MessageLevels.Success);
                self.Password("");
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.ChangeAccountStatus = function (account) {
        var data = { AccountId: account.Id };
        BaseModel.ServiceCall("/Admin/ChangeAccountStatus", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                ko.utils.arrayFirst(self.Accounts(), function (acct) {
                    if (acct.Id() === account.Id()) {
                        acct.IsActive(!acct.IsActive());
                    }
                });
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.DeleteAccount = function (account) {
        var data = { AccountId: account.Id };
        BaseModel.ServiceCall("/AccountAdmin/DeleteAccount", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.GetAccounts();
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.SaveNewPassword = function () {
        if (self.Password() === "") {
            BaseModel.Message("Please enter a password", BaseModel.MessageLevels.Error);
            return;
        }

        var data = { Email: self.Email(), BaseUrl: window.location.origin };
        BaseModel.ServiceCall("/Admin/SaveNewPassword", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                BaseModel.Message("Password has been successfully changed", BaseModel.MessageLevels.Success);
                self.Password("");
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.ResetPassword = function () {
        var data = { Email: self.Account().Email };
        BaseModel.ServiceCall("/Admin/PasswordReset", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage };
                BaseModel.Message("The password has been reset and a link has been emailed.", BaseModel.MessageLevels.Success);
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.Show = function (page) {
        $("#accountadmin-" + self.CurrentPage()).hide();
        $("#accountadmin-" + page).show();
        self.CurrentPage(page);
    };

    self.Load = function () {
        self.GetAdminAccounts();
    };
}

function CompanyAdminViewModel(parent) {
    var self = this;
    self.Parent = parent;
    self.CurrentPage = ko.observable("");

    self.Companies = ko.observableArray([]);
    self.Company = ko.observable(new CompanyDataModel());
    self.CompanySites = ko.observableArray([]);

    self.CancelCompanyEdit = function () {
        self.GetCompany();
        self.Show("list");
    };

    self.GetCompanies = function () {
        BaseModel.ServiceCall('/Admin/GetCompanies', "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.Companies(response.Companies.map(function (company) {
                    return new CompanyDataModel(company);
                }));
                self.Show("list");
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.GetCompany = function () {
        BaseModel.ServiceCall('/Admin/GetCompany', "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.Company(new CompanyDataModel(response.Company));
                self.GetCompanySites();
                self.Show("list");
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.GetCompanySites = function () {
        var data = { CompanyId: self.Company().Id };
        BaseModel.ServiceCall('/Admin/GetCompanySites', "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.CompanySites(ko.observableArray(response.CompanySites ? response.CompanySites.map(function (site) {
                    return new CompanySiteDataModel(site);
                }) : []));
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.EditCompany = function () {
        self.Show("edit");
    };

    self.SaveCompany = function () {
        var data = { Company: self.Company() };
        BaseModel.ServiceCall('/Admin/SaveCompany', 'post', data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.Show("list");
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.ChangeCompanyStatus = function (account) {
        var data = { AccountId: account.Id };
        BaseModel.ServiceCall("/Admin/ChangeCompanyStatus", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                ko.utils.arrayFirst(self.Accounts(), function (acct) {
                    if (acct.Id() === account.Id()) {
                        acct.IsActive(!acct.IsActive());
                    }
                });
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.Show = function (page) {
        $("#companyadmin-" + self.CurrentPage()).hide();
        $("#companyadmin-" + page).show();
        self.CurrentPage(page);
    };

    self.Load = function () {
        self.GetCompanies();
    };
}

function ModuleAdminViewModel(parent) {
    var self = this;
    self.Parent = parent;
    self.CurrentPage = ko.observable("");

    self.Modules = ko.observableArray([]);
    self.Module = ko.observable(new ModuleDataModel());

    self.Show = function (page) {
        $("#moduleadmin-" + self.CurrentPage()).hide();
        $("#moduleadmin-" + page).show();
        self.CurrentPage(page);
    };

    self.GetModules = function () {
        BaseModel.ServiceCall('/Admin/GetModules', "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.Modules(response.Modules.map(function (module) {
                    return new ModuleDataModel(module);
                }));
                self.Show("list");
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.ChangeModuleStatus = function (module) {
        var data = { ModuleId: module.Id };
        BaseModel.ServiceCall("/Admin/ChangeModuleStatus", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                ko.utils.arrayFirst(self.Modules(), function (mod) {
                    if (mod.Id() === module.Id()) {
                        mod.IsActive(!mod.IsActive());
                    }
                });
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.Load = function () {
        self.GetModules();
    };
}

function WebhooksViewModel(parent) {
    var self = this;
    self.Parent = parent;

    self.ShopifyWebhooks = ko.observableArray();
    self.ShopifyWebhook = ko.observable();

    self.GetShopifyWebhooks = function () {
        BaseModel.ServiceCall('/Webhook/GetShopifyWebhooks/', "get", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.ShopifyWebhooks([]);
                for (i in response.Webhooks) {
                    self.ShopifyWebhooks.push(new WebhookDataModel(response.Webhooks[i]));
                }
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.ActivateShopifyWebhook = function (webhook) {
        var data = { Webhook: webhook };
        BaseModel.ServiceCall('/Webhook/ActivateShopifyWebhook/', "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                ko.utils.arrayFirst(self.ShopifyWebhooks(), function (_webhook) {
                    if (_webhook.Topic() === webhook.Topic()) {
                        _webhook.IsActive(!_webhook.IsActive());
                        _webhook.Id(response.NewWebhookId);
                    }
                });
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.DeleteShopifyWebhook = function (webhook) {
        var data = { WebhookId: webhook.Id };
        BaseModel.ServiceCall('/Webhook/DeleteShopifyWebhook/', "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                ko.utils.arrayFirst(self.ShopifyWebhooks(), function (_webhook) {
                    if (_webhook.Topic() === webhook.Topic()) {
                        _webhook.IsActive(!_webhook.IsActive());
                        _webhook.Id(0);
                    }
                });
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.Load = function () {
        self.GetShopifyWebhooks();
    };
}