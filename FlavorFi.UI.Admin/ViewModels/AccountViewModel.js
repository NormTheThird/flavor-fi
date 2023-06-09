function AccountViewModel(page) {
    var self = this;
    self.Page = page;

    self.Content = ko.observable({ name: 'LoginTemplate' });
    self.Email = ko.observable();
    self.Password = ko.observable();
    self.PasswordConfirm = ko.observable();
    self.ResetId = ko.observable(Mob.GetUrlQuery('id'));
    self.Test = ko.observable(true);

    self.Loading = ko.observable({ Page: ko.observable(true), Login: ko.observable(false), RegisterAccount: ko.observable(false) });
    self.IsLoading = ko.computed(function () {
        for (i in self.Loading()) {
            if (self.Loading()[i]()) {
                return true;
            }
        }
        return false;
    })

    self.GetFunction = function () {
        try {
            var task = Mob.GetUrlQuery("f")
            if (task) { task = task.toLowerCase(); }
            else { task = self.Page; }
            switch (task) {
                case 'register':
                    self.ShowRegisterAccount();
                    break;
                case 'reset':
                    self.ShowResetPassword();
                    break;
                default:
                    self.ShowLogin();
                    break;
            }
        }
        catch (ex) {
            Mob.Log(ex);
        }
    }

    self.ShowForgotPassword = function () {
        self.Content({ name: 'ForgotPasswordTemplate' })
    }

    self.ShowRegisterAccount = function () {
        self.Content({ name: 'RegisterAccountTemplate' })
    }

    self.ShowLogin = function () {
        self.Content({ name: 'LoginTemplate' })
    }

    self.ShowResetPassword = function () {
        // Check for ID
        var id = Mob.GetUrlQuery('id');
        if (id && id.length == Mob.Guid.Empty.length) {
            self.Content({ name: 'ResetPasswordTemplate' });
        }
        else {
            self.Content({ name: 'ResetNoIDTemplate' });
        }
    }

    self.Login = function () {
        try {
            self.Loading().Login(true);
            self.BindValidation();
            if (!$('#AccountForm').valid()) { throw "Form Incomplete" }

            var data = { Email: self.Email(), Password: self.Password() };
            Mob.ServiceCall('/Account/Login', "post", data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage }
                    window.location = "/Dashboard";
                }
                catch (ex) {
                    Mob.Message(ex, Mob.MessageLevels.Error);
                    Mob.Log(ex);
                }
                finally {
                    self.Loading().Login(false);
                }
            })
        }
        catch (ex) {
            Mob.Log(ex);
            self.Loading().Login(false);
        }
    }

    self.RegisterAccount = function () {
        try {
            self.Loading().RegisterAccount(true);
            self.BindValidation();
            if (!$('#AccountForm').valid()) { throw "Form Incomplete" }

            var data = { Email: self.Email(), Password: self.Password() };
            Mob.ServiceCall('/Account/Register', "post", data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage }
                    window.location = "/Dashboard";
                }
                catch (ex) {
                    Mob.Message(ex, Mob.MessageLevels.Error);
                    Mob.Log(ex);
                }
                finally {
                    self.Loading().RegisterAccount(false);
                }
            })
        }
        catch (ex) {
            Mob.Log(ex);
            self.Loading().RegisterAccount(false);
        }
    }

    self.ForgotPassword = function () {
        try {
            self.Loading().RegisterAccount(true);

            self.BindValidation();
            if (!$('#AccountForm').valid()) { throw "Form Incomplete" }

            var data = {
                Email: self.Email(),
            };
            Mob.ServiceCall('/Account/ForgotPassword', "post", data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage }
                    self.Content({ name: 'ResetSentTemplate' });
                }
                catch (ex) {
                    Mob.Message(ex, Mob.MessageLevels.Error);
                    Mob.Log(ex);
                }
                finally {
                    self.Loading().RegisterAccount(false);
                }
            })
        }
        catch (ex) {
            Mob.Log(ex);
            self.Loading().RegisterAccount(false);
        }
    }

    self.ResetPassword = function () {
        try {
            self.Loading().RegisterAccount(true);

            self.BindValidation();
            if (!$('#AccountForm').valid()) { throw "Form Incomplete" }

            var data = {
                ResetId: self.ResetId(),
                NewPassword: self.Password()
            };
            Mob.ServiceCall('/Account/ResetPassword', "post", data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage }
                    Mob.Message('Reset successful, Please log in', Mob.MessageLevels.Success);
                    self.ShowLogin();
                }
                catch (ex) {
                    Mob.Message(ex, Mob.MessageLevels.Error);
                    Mob.Log(ex);
                }
                finally {
                    self.Loading().RegisterAccount(false);
                }
            })
        }
        catch (ex) {
            Mob.Log(ex);
            self.Loading().RegisterAccount(false);
        }
    }

    self.BindValidation = function () {
        $('#AccountForm').validate({
            rules: {
                email: 'required',
                password: 'required',
                NewPassword: 'required',
            },
            messages: {
                email: 'A valid email address is required.',
                password: 'A valid password is required.',
                NewPassword: "New password is required.",
            }
        })
    };

    self.Load = function () {
        self.GetFunction();
        self.Loading().Page(false);
    }

    self.Load();
}