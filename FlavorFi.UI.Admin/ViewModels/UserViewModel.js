function UserDataModel(data) {
    var self = this;
    if (data == null) data = {
        "Id": Mob.Guid.Empty,
        "CompanyId": Mob.Guid.Empty,
        "FirstName": "",
        "LastName": "",
        "Email": "",
        "Username": "",
        "PhoneNumber": "",
        "AltPhoneNumber": "",
        "IsActive": false,
        "IsCompanyAdmin": false
    }

    self.Id = ko.observable(data.Id);
    self.CompanyId = ko.observable(data.CompanyId);
    self.FirstName = ko.observable(data.FirstName);
    self.LastName = ko.observable(data.LastName);
    self.Email = ko.observable(data.Email)
    self.Username = ko.observable(data.Username);;
    self.PhoneNumber = ko.observable(data.PhoneNumber);
    self.AltPhoneNumber = ko.observable(data.AltPhoneNumber);
    self.IsActive = ko.observable(data.IsActive);
    self.IsCompanyAdmin = ko.observable(data.IsCompanyAdmin);
};

function UserViewModel(parent) {
    var self = this;
    self.Parent = parent;

    self.Users = ko.observableArray();
    self.User = ko.observable(new UserDataModel());


    self.Loading = ko.observable({ Page: ko.observable(true), GetUsers: ko.observable(false), SaveUser: ko.observable(false), SendPassLink: ko.observable(false) });
    self.IsLoading = ko.computed(function () {
        for (i in self.Loading()) {
            if (self.Loading()[i]()) { return true }
        }
        return false;
    })

    self.EditUser = function (user) {
        $('#add-user').collapse('show');
        self.BindValidation();
        if (user instanceof UserDataModel) {
            self.User(new UserDataModel(ko.toJS(user)));
        }
        else {
            self.User(new UserDataModel());
        }
    }

    self.ClosePanel = function () {
        $('#add-user').collapse('hide');
    }

    self.GetUsers = function () {
        try {
            self.Loading().GetUsers(true);
            var data = {};
            Mob.ServiceCall('/Management/GetUsers', 'get', data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage };
                    self.Users(response.Users.map(function (user) {
                        return new UserDataModel(user);
                    }));
                } catch (ex) {
                    Mob.Message("Error: " + ex, Mob.MessageLevels.Error);
                    Mob.Log(ex);
                }
                finally {
                    self.Loading().GetUsers(false);
                }
            })
        } catch (ex) {
            self.Loading().GetUsers(false);
            Mob.Log(ex);
        }
    }

    self.SaveUser = function () {
        try {
            self.Loading().SaveUser(true);
            if (!$('#UserForm').valid()) { throw "Please complete required fields" };
            var data = {
                User: ko.toJS(self.User)
            };
            Mob.ServiceCall('/Management/SaveUser', 'post', data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage };
                    Mob.Message('Saved user!', Mob.MessageLevels.Success);
                    self.GetUsers();
                    self.ClosePanel();

                } catch (ex) {
                    Mob.Message("Error: " + ex, Mob.MessageLevels.Error);
                    Mob.Log(ex);
                }
                finally {
                    self.Loading().SaveUser(false);
                }
            })
        } catch (ex) {
            self.Loading().SaveUser(false);
            Mob.Log(ex);
        }
    }
    self.BindValidation = function () {
        $('#UserForm').validate({
            rules: {
                "FirstName": "required",
                "LastName": "required",
                "Email": "required",
                "PhoneNumber": "required",
            },
            messages: {
                "FirstName": "First Name is required.",
                "LastName": "Last Name is required.",
                "Email": "Email is required.",
                "PhoneNumber": "Phone Number is required.",
            }
        })
    }

    self.SendPassLink = function () {
        try {
            self.Loading().SendPassLink(true);
            var data = {
                Email: self.User().Email()
            };
            Mob.ServiceCall('/Management/SendPasswordResetLink', 'post', data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage };
                    Mob.Message("A password reset link has been sent", Mob.MessageLevels.Success);
                    self.ClosePanel();

                } catch (ex) {
                    Mob.Message("Error: " + ex, Mob.MessageLevels.Error);
                    Mob.Log(ex);
                }
                finally {
                    self.Loading().SendPassLink(false);
                }
            })
        } catch (ex) {
            self.Loading().SendPassLink(false);
            Mob.Log(ex);
        }
    }

    self.Load = function () {
        self.GetUsers();
        self.Loading().Page(false);
    }

    self.Load();
}
