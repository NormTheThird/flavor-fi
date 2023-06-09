function CompanyViewModel() {
    var self = this;
    self.CurrentPage = ko.observable("");

    self.Company = ko.observable(new CompanyDataModel());
    self.CompanySite = ko.observable(new CompanySiteDataModel());
    self.CompanySites = ko.observableArray([]);
    self.CompanySiteLocations = ko.observableArray([]);

    self.CancelCompanyEdit = function () {
        self.GetCompany();
        $("#company-edit").hide();
    };

    self.GetCompany = function () {
        BaseModel.ServiceCall('/Company/GetCompany', "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.Company(new CompanyDataModel(response.Company));
                self.GetCompanySites();
                $("#company-edit").hide();
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.GetCompanySites = function () {
        var data = { CompanyId: self.Company().Id };
        BaseModel.ServiceCall('/Company/GetCompanySites', "post", data, true, function (response) {
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

    self.GetCompanySiteLocations = function () {
        var data = { CompanyId: self.Company().Id };
        BaseModel.ServiceCall('/Company/GetCompanySiteLocations', "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.CompanySiteLocations(ko.observableArray(response.Locations ? response.Locations.map(function (location) { return location; }) : []));
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.EditCompany = function () {
        $("#company-edit").show();
    };

    self.EditCompanySite = function (site) {
        self.GetCompanySiteLocations();
        self.CompanySite(site);
        $("#company-sites-list").hide();
        $("#company-sites-edit").show();

    };

    self.SaveCompany = function () {
        var data = { Company: self.Company() };
        BaseModel.ServiceCall('/Company/SaveCompany', 'post', data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                BaseModel.Message("Company information has been updated!", BaseModel.MessageLevels.Success);
                $("#company-edit").hide();
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.SaveCompanySite = function () {
        self.CancelCompanySiteEdit()
    };

    self.CancelCompanySiteEdit = function () {
        self.GetCompany();
        $("#company-sites-edit").hide();
        $("#company-sites-list").show();
    };

    self.BindValidation = function () {
        $('#CompanyForm').validate({
            rules: { Name: 'required' },
            messages: { Name: 'Name is required.' }
        });
    };

    self.Load = function () {
        self.GetCompany();
    };

    self.Load();
}