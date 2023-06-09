function PushNotificationViewModel(parent) {
    var self = this;
    self.Parent = parent;

    self.Content = ko.observable({ name: 'NotificationTableTemplate' });
    self.Notification = ko.observable(new PushNotificationDataModel());
    self.Status = ko.observable("");

    self.AllNotifications = ko.observableArray();

    self.NotificationPaging = ko.observable(new PaginationVM(self.AllNotifications));

    self.Loading = ko.observable({ Page: ko.observable(true), GetPushNotifications: ko.observable(false), SaveNotification: ko.observable(false), GetNotificationStatus: ko.observable(false) });
    self.IsLoading = ko.computed(function () {
        for (i in self.Loading()) {
            if (self.Loading()[i]()) { return true }
        }
        return false;
    })

    self.SendNotification = function (notification) {
        console.log(notification);
        if (notification instanceof PushNotificationDataModel) {
            self.Notification(new PushNotificationDataModel(ko.toJS(notification)));
        }
        else {
            self.Notification(new PushNotificationDataModel())
        }
        self.Content({ name: 'NotificationEditTemplate', });
    }

    self.ShowTable = function () {
        self.Content({ name: 'NotificationTableTemplate', });
    }

    self.GetPushNotifications = function () {
        try {
            self.Loading().GetPushNotifications(true);
            var data = {};

            Mob.PromiseCall('/Mobile/GetPushNotifications', 'get', data).then(function (response) {
                self.AllNotifications(response.PushNotifications.map(function (notification) {
                    return new PushNotificationDataModel(notification);
                }))
                self.Loading().GetPushNotifications(false);
            }).catch(function (response) {
                console.error(response);
                self.Loading().GetPushNotifications(false);
            })
        } catch (ex) {
            self.Loading().GetPushNotifications(false);
            Mob.Log(ex);
        }
    }

    self.SaveNotification = function () {
        try {
            self.Loading().SaveNotification(true);
            var data = ko.toJS(self.Notification);
            Mob.PromiseCall('/Mobile/SendPushNotificationAsync', 'post', data).then(function (response) {
                setTimeout(self.GetNotificationStatus, 2500);
            }).catch(function (response) {
                console.error(response);
            })
        } catch (ex) {
            self.Loading().SaveNotification(false);
            Mob.Log(ex);
        }
    }

    self.GetNotificationStatus = function () {
        try {
            var data = {};
            Mob.PromiseCall('/Mobile/GetSendPushNotificationStatus', 'post', data).then(function (response) {
                console.log(response);
                if (!response.IsProcessComplete) {
                    self.Status(response.Message);
                    self.Content({ name: 'NotificationTableTemplate', });
                    self.GetPushNotifications();
                    self.Loading().SaveNotification(false);
                }
            }).catch(function (response) {
                console.error(response);
            })

        } catch (ex) {
            self.Loading().GetNotificationStatus(false);
            Mob.Log(ex);
        }
    }

    self.Load = function () {
        self.GetPushNotifications();
        self.ShowTable();
        //self.GetNotificationStatus();
        setTimeout(self.ShowTemplate, 5000)
        self.Loading().Page(false);
    }

    self.Load();
}