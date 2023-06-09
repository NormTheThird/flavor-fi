function DatabaseSyncViewModel(parent) {
    var self = this;
    self.Parent = parent;

    self.DatabaseCustomCollectionsSyncViewModel = ko.observable(new DatabaseEntitySyncViewModel("CustomCollections"));
    self.DatabaseCustomerSyncViewModel = ko.observable(new DatabaseEntitySyncViewModel("Customers"));
    self.DatabaseOrderSyncViewModel = ko.observable(new DatabaseEntitySyncViewModel("Orders"));
    self.DatabaseProductSyncViewModel = ko.observable(new DatabaseEntitySyncViewModel("Products"));
    self.DatabaseUserSyncViewModel = ko.observable(new DatabaseEntitySyncViewModel("Users"));

    self.Load = function () {
        self.DatabaseCustomCollectionsSyncViewModel().Load();
        self.DatabaseCustomerSyncViewModel().Load();
        self.DatabaseOrderSyncViewModel().Load();
        self.DatabaseProductSyncViewModel().Load();
        self.DatabaseUserSyncViewModel().Load();
    };
}

function DatabaseEntitySyncViewModel(entity) {
    var self = this;

    self.EntityStatus = ko.observable(new DatabaseStatusModel());
    self.SessionId = ko.observable("");

    self.GetSyncStatus = function () {
        var data = { Entity: entity };
        BaseModel.ServiceCall('/DataSync/GetDatabaseSyncStatus/', "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.EntityStatus().SyncStatus("To sync " + entity + " select start sync.");
                if (response.SyncRecord !== null) {
                    self.EntityStatus().LastSynced(BaseModel.ToDate(response.SyncRecord.DateStarted));
                    self.EntityStatus().IsSyncing(response.SyncRecord.CurrentlySyncing);
                    if (self.EntityStatus().IsSyncing()) {
                        self.EntityStatus().SyncStatus("Getting sync status");
                        self.SessionId(response.SyncRecord.Id);
                        self.SynchronizeDatabaseEntityStatus();
                    }
                }
            }
            catch (ex) {
                BaseModel.Message(ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.SynchronizeDatabaseEntity = function () {
        self.EntityStatus().IsSyncing(true);
        BaseModel.ServiceCall("/DataSync/SynchronizeDatabase" + entity + "/", "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage }
                self.EntityStatus().SyncStatus(response.Message);
                self.SessionId(response.SessionId);
                self.SynchronizeDatabaseEntityStatus();
            }
            catch (ex) {
                self.EntityStatus().IsSyncing(false);
                self.EntityStatus().SyncStatus(ex);
                BaseModel.Log(ex);
            }
        });
    };

    self.SynchronizeDatabaseEntityStatus = function () {
        var waitingOnLastResponse = false;
        var timerHandle = setInterval(function () {
            if (waitingOnLastResponse === false) {
                waitingOnLastResponse = true;
                var data = { SessionId: self.SessionId() };
                BaseModel.ServiceCall("/DataSync/SynchronizeDatabase" + entity + "Status/", "post", data, true, function (response) {
                    try {
                        if (!response.IsSuccess) { throw response.ErrorMessage; }
                        console.log(response);
                        for (var i in response.Statuses) {
                            var currentStatus = response.Statuses[i];
                            if (currentStatus !== null) {
                                if (currentStatus.RecordsSynced > 0) {
                                    self.EntityStatus().SyncStatus("Syncing " + currentStatus.RecordsSynced + " of " + currentStatus.TotalRecordsToSync + " " + entity);
                                    self.EntityStatus().SyncPercentage((currentStatus.RecordsSynced / currentStatus.TotalRecordsToSync) * 100);
                                }

                                if (currentStatus.IsProcessComplete) {
                                    self.EntityStatus().SyncStatus(currentStatus.Message);
                                    self.EntityStatus().IsSyncing(false);
                                    self.EntityStatus().LastSynced((new Date().getMonth() + 1) + '/' + new Date().getDate() + '/' + new Date().getFullYear());
                                    self.SessionId("");
                                    clearInterval(timerHandle);
                                }
                            }
                        }
                    }
                    catch (ex) {
                        clearInterval(timerHandle);
                        self.EntityStatus().IsSyncing(false);
                        self.EntityStatus().SyncStatus(ex);
                        BaseModel.Log(ex);
                    }
                    finally { waitingOnLastResponse = false; }
                });
            }
        }, 2000);
    };

    self.Load = function () {
        self.GetSyncStatus();
    };
}