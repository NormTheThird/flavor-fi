function NavigationViewModel() {
    var self = this;

    self.Companies = ko.observableArray([]);
    self.CompanySites = ko.observableArray([]);
    self.Company = ko.observable(new CompanyDataModel());
    self.CompanySite = ko.observable(new CompanySiteDataModel());

    self.GetCompany = function () {
        BaseModel.ServiceCall('/Navigation/GetCompanyAndSites', "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }

                self.Company(response.Company);
                self.CompanySites([]);
                for (i in response.CompanySites) {
                    var site = response.CompanySites[i];
                    if (response.SelectedCompanySiteId === site.Id) {
                        self.CompanySite(site);
                    }
                    self.CompanySites.push(new CompanySiteDataModel(response.CompanySites[i]));
                }
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.GetCompanies = function () {
        BaseModel.ServiceCall('/Navigation/GetCompanies', "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage }
                self.Companies([]);
                for (i in response.Companies) {
                    self.Companies.push(new CompanyDataModel(response.Companies[i]));
                }
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.SelectCompany = function (company) {
        var data = { companyId: company.Id };
        BaseModel.ServiceCall('/Navigation/UpdateCompanyId', "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage }
                self.GetCompany();
                window.location.href = "/Dashboard/Index";
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.SelectCompanySite = function (companySite) {
        self.CompanySite(companySite);
        window.location.href = "/Dashboard/Index";
    };

    self.Load = function () {
        self.GetCompany();
        self.GetCompanies();
    };

    self.Load();
}