var OrderPickupViewModel = function () {
    var self = this;
    self.OrdersForPickup = ko.observableArray();
    self.OrderForPickup = ko.observable(new OrdersForPickupDataModel());

    self.GetOrdersForPickup = function () {
        BaseModel.ServiceCall("/Standalone/GetShopifyOrdersForPickup", "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.OrdersForPickup(response.OrdersForPickup.map(function (order) {
                    return new OrdersForPickupDataModel(order);
                }));
                self.OrderForPickup();
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.SetOrderForPickup = function (order) {
        self.OrderForPickup(order);
    };

    self.MarkAsPickedUp = function () {
        var data = { ShopifyOrderId: self.OrderForPickup().ShopifyOrderId() };
        BaseModel.ServiceCall("/Standalone/SaveShopifyOrderAsPickedup", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                BaseModel.Message("Order " + self.OrderForPickup().OrderNumber() + " has been marked as pickedup", BaseModel.MessageLevels.Success);
                self.GetOrdersForPickup();
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            }
        });
    };

    self.Load = function () {
        self.GetOrdersForPickup();
    };
};

var QualityCheckViewModel = function () {
    var self = this;
    self.ItemsVisible = ko.observable(false);
    self.InputSelected = ko.observable(false);
    self.ItemSelected = ko.observable(false);
    self.GroupItems = ko.observable(false);
    self.OrderNumber = ko.observable("");
    self.ItemNumber = ko.observable("");
    self.Order = ko.observable(new ShopifyQualityCheckOrderDataModel());

    self.OrderHeader = ko.computed(function () {
        if (self.OrderNumber() === "") return "Quality Check";
        else return "Quality Check for order number " + self.OrderNumber();
    });

    self.GetOrder = function () {
        var data = { OrderNumber: self.OrderNumber() };
        BaseModel.ServiceCall("/Standalone/GetShopifyOrder", "post", data, true, function (response) {
            if (response.IsSuccess) {
                self.Order(new ShopifyQualityCheckOrderDataModel(response));
                if (self.Order().OrderItems().length > 0) {
                    self.ItemsVisible(true);
                    self.ItemSelected(true);
                }
                else {
                    swal("Error No Items!", "No items found for order number " + self.OrderNumber() + "!", "error", 3000);
                    self.OrderNumber("");
                }
            } else {
                swal("Error Order Number " + self.OrderNumber(), response.ErrorMessage, "error", 3000);
                self.OrderNumber("");
                self.Order(new ShopifyQualityCheckOrderDataModel());
                self.ItemsVisible(false);
            }
        });
    };

    self.GetItem = function () {
        var found = false;
        $.each(self.Order().OrderItems(), function (index, item) {
            try {
                console.log("Scanned Value: [" + self.ItemNumber().trim().toUpperCase() + "] | Sku: [" + item.Sku().trim().toUpperCase() + "] | [ Fulfilled: " + item.IsFulfilled() + "]");
                if (self.ItemNumber().trim() === "") {
                    console.log("Empty Item Number");
                    self.ItemNumber(document.getElementById('exampleInputAmount').value);
                }

                if (self.GroupItems()) {
                    if (self.ItemNumber().trim() == '') {
                        console.log("Empty: " + self.ItemNumber());
                        self.ItemNumber(document.getElementById('exampleInputAmount').value);
                    }

                    if (self.ItemNumber().trim().toUpperCase() == value.Sku().trim().toUpperCase()) {

                        console.log("Match: " + self.ItemNumber());
                        if (value.CheckedQuantity() == value.Quantity())
                            swal("Error!", "This item " + self.ItemNumber() + " has already been fulfilled.", "error", 3000);
                        else {
                            value.CheckedQuantity(value.CheckedQuantity() + 1);
                            self.ItemSelected(true);
                        }
                        self.ItemNumber('');
                        found = true;
                        return false;
                    }
                }
                else {
                    if (self.ItemNumber().trim().toUpperCase() === item.Sku().trim().toUpperCase()) {
                        if (item.Quantity() === 1) {
                            if (item.IsFulfilled()) { swal("Error!", "This item " + self.ItemNumber() + " has already been fulfilled.", "error", 3000); }
                            else { item.IsFulfilled(true); }
                        }
                        else if (!item.IsFulfilled()) {
                            item.IsFulfilled(true);
                        }
                        else {
                            var fullfilled = true;
                            $.each(self.Order().OrderItems(), function (index, multipleItem) {
                                if (multipleItem.Sku().trim().toUpperCase() === item.Sku().trim().toUpperCase()) {
                                    console.log("Item: [" + item.Sku().trim().toUpperCase() + "] | MultipleItem: [" + multipleItem.Sku().trim().toUpperCase() + "] | [ Fulfilled: " + multipleItem.IsFulfilled() + "]");
                                    if (!multipleItem.IsFulfilled()) { fullfilled = false; }
                                }
                            });
                            if (fullfilled) { swal("Error!", "This item " + self.ItemNumber() + " has already been fulfilled.", "error", 3000); }
                            else { return true; }
                        }

                        found = true;
                        self.ItemNumber('');
                        return false;
                    }
                }
            } catch (ex) {
                console.log(ex);
            }
        });

        if (!found) {
            swal("Error!", "This item " + self.ItemNumber() + " is not part of this order.", "error", 3000);
            self.ItemNumber('');
        }
        else {
            var finished = true;
            $.each(self.Order().OrderItems(), function (index, item) {
                if (!item.IsFulfilled()) finished = false;
            });
            if (finished) {
                self.InputSelected(true);
                swal({
                    title: "Congrats!",
                    text: "Order has been verified and is now ready to ship.",
                    type: "success",
                    showCancelButton: false,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Ok",
                    closeOnConfirm: true
                },
                    function () {
                        self.Clear();
                    });
            }
        }

    };

    self.Clear = function () {
        self.ItemsVisible(false);
        self.OrderNumber('');
        self.ItemNumber('');
        self.InputSelected(true);
    };

    self.Scan = function (data, e) {
        if (e.keyCode === 32) return;
        if (e.keyCode === 13 || e.keyCode === 9) {
            self.GetOrder();
            return;
        }
        else self.OrderNumber(self.OrderNumber() + String.fromCharCode(e.keyCode));
    };

    self.ScanItem = function (data, e) {
        if (e.keyCode === 32) return;
        if (e.keyCode === 13 || e.keyCode === 9) {
            self.GetItem();
            return;
        }
        else self.ItemNumber(self.ItemNumber().trim() + String.fromCharCode(e.keyCode));
    };

    self.Load = function () {
        self.Clear();
    };
};