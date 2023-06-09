function SecurityViewModel() {
    var self = this;
    self.CurrentPage = ko.observable("");

    self.FirstName = ko.observable("");
    self.LastName = ko.observable("");
    self.Email = ko.observable("");
    self.Password = ko.observable("");
    self.ConfirmPassword = ko.observable("");
    self.ResetId = ko.observable("");
    self.RegistrationCode = ko.observable("");
    self.ErrorMessage = ko.observable("");
    self.Agrees = ko.observable(false);
    self.RememberMe = ko.observable(false);
    self.IsResetPassword = ko.observable(false);

    self.EnterSubmit = function (vm, evt) {
        if (evt.key === 'Enter') { self.Login(); }
    };
    self.EnterRegister = function (vm, evt) {
        if (evt.key === 'Enter') { self.RegisterAccount(); }
    };

    self.ShowRegisterAccount = function () {
        self.Show("register");
    };

    self.RegisterAccount = function () {
        self.ErrorMessage("");
        if (!self.Agrees()) {
            self.ErrorMessage("* Please agree to the terms to sign up.");
            return;
        }

        $("#security-login").addClass("ld-loading");
        var data = {
            FirstName: self.FirstName(), LastName: self.LastName(), Email: self.Email(),
            Password: self.Password(), RegistrationCode: self.RegistrationCode()
        };
        BaseModel.ServiceCall('/Security/Register', "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) {
                    self.ErrorMessage(response.ErrorMessage);
                    return;
                }

                window.location = "/Dashboard";
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
            finally { $("#security-login").removeClass("ld-loading"); }
        });

    };

    self.ShowLogin = function () {
        self.Show("login");
    };

    self.ValidateAccount = function () {
        self.ErrorMessage("");
        $("#security-login").addClass("ld-loading");
        var data = { Email: self.Email(), Password: self.Password(), RememberMe: self.RememberMe() };
        BaseModel.ServiceCall("/Security/ValidateAccount", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) {
                    self.ErrorMessage(response.ErrorMessage);
                    $("#security-login").removeClass("ld-loading");
                    return;
                }

                window.location = "/Dashboard";
            } catch (ex) {
                window.location = "/Security/Index";
                Base.Log(ex);
                $("#security-login").removeClass("ld-loading");
            }
        });
    };

    self.ShowForgotPassword = function () {
        self.IsResetPassword(false);
        self.Show("forgot-password");
    };

    self.ForgotPassword = function () {
        self.ErrorMessage("");
        var data = { Email: self.Email(), BaseUrl: window.location.origin };
        BaseModel.ServiceCall('/Security/ForgotPassword', "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.IsResetPassword(true);
            }
            catch (ex) {
                self.ErrorMessage(ex);
                BaseModel.Log(ex);
            }
        });
    };

    self.ShowResetPassword = function () {
        self.Show("reset-password");
    };

    self.ResetPassword = function () {
        self.ErrorMessage("");
        if (self.Password() !== self.ConfirmPassword()) {
            self.ErrorMessage("* Passwords do not match!");
            return;
        }

        var data = { ResetId: self.ResetId(), NewPassword: self.Password() };
        BaseModel.ServiceCall('/Security/ResetPassword', "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.Show("login");
            }
            catch (ex) {
                self.ErrorMessage(ex);
                BaseModel.Log(ex);
            }
        });
    };

    self.Show = function (page) {
        self.ErrorMessage("");
        $("#security-" + self.CurrentPage()).hide();
        $("#security-" + page).show();
        self.CurrentPage(page);
    };

    self.Load = function (resetId) {
        if (resetId !== "") {
            self.ResetId(resetId);
            self.Show("reset-password");
        }
        else {
            self.Show("login");
        }
    };
}