function COGModel(data) {
    var self = this;
    if (data == null) data = {
        "OrderId": "",
        "OrderDetailId": "",
        "OriginalOrderId": "",
        "ProductId": "",
        "OriginalProductId": "",
        "Id": "",
        "Name": "",
        "Sku": "",
        "Vendor": "",
        "Variant": "",
        "Quantity": "",
        "CostOfGood": "",
        "SalePrice": "",
        "Markup": "",
        "DateSold": "",
    }

    self.OrderId = ko.observable(data.OrderId);
    self.OrderDetailId = ko.observable(data.OrderDetailId);
    self.OriginalOrderId = ko.observable(data.OriginalOrderId);
    self.ProductId = ko.observable(data.ProductId);
    self.OriginalProductId = ko.observable(data.OriginalProductId);
    self.Id = ko.observable(data.Id);
    self.Name = ko.observable(data.Name);
    self.Sku = ko.observable(data.Sku);
    self.Vendor = ko.observable(data.Vendor);
    self.Variant = ko.observable(data.Variant);
    self.Quantity = ko.observable(data.Quantity);
    self.CostOfGood = ko.observable(parseFloat((data.CostOfGood.match(/\d+\.?/g) || [0]).join("")));
    self.SalePrice = ko.observable(data.SalePrice);
    self.Markup = ko.computed(function () {
        return (self.SalePrice() || 0) - (self.CostOfGood() || 0);
    })

    self.DateSold = ko.observable(BaseModel.ToDate(data.DateSold));
    self.ShowDetails = ko.observable(false);
    self.Expand = function () {
        self.ShowDetails(!self.ShowDetails());
    }
}

function GiftCardModel(data) {
    var self = this;

    if (data == null) data = {};

    self.Id = ko.observable(data.Id);
    self.LastFour = ko.observable(data.LastFour.toUpperCase());
    self.IssuedBy = ko.observable(data.IssuedBy);
    self.Customer = ko.observable(data.Customer);
    self.DateCreated = ko.observable(BaseModel.ToDate(data.DateCreated));

    self.Note = ko.observable(data.Note);
    self.Balance = ko.observable(data.Balance);
    self.InitialValue = ko.observable(data.InitialValue);

    self.NoteVisible = ko.observable(false);
    self.ShowNote = function () {
        self.NoteVisible(!self.NoteVisible());
    }
}

function PurchaseHistoryDataModel(data) {
    var self = this;
    if (data == null) data = {};

    self.Id = ko.observable(data.Id);
    self.Amount = ko.observable(data.Amount);
    self.DateCreated = ko.observable(Base.ToDate(data.DateCreated));
    self.OrderNumber = ko.observable(data.OrderNumber);
    self.TransactionDate = ko.observable(Base.ToDate(data.TransactionDate));
    self.UsedBy = ko.observable(data.UsedBy);
}

function CardHistoryDataModel(data) {
    var self = this;

    if (data == null) data = {};

    self.Id = ko.observable(data.Id);
    self.Details = ko.observable(data.Details);
    self.DateCreated = ko.observable(Base.ToDate(data.DateCreated));
}

function SalesReport(data) {
    var self = this;

    if (!data) { data = {}; }
    self.GrossSales = ko.observable(data.GrossSales);
    self.Discounts = ko.observable(data.Discounts);
    self.Returns = ko.observable(data.Returns);
    self.SalesTax = ko.observable(data.SalesTax);

}

function CogsReportViewModel(parent) {
    var self = this;
    self.Parent = parent;

    self.Status = ko.observable("");
    self.StartDate = ko.observable();
    self.EndDate = ko.observable();

    self.ReportData = ko.observableArray();

    self.Vendors = ko.observableArray();
    self.SelectedVendor = ko.observable();

    self.TotalDiscounts = ko.observable();
    self.TotalRefunds = ko.observable();

    self.SortMethod = ko.observable();
    self.Sort = ko.observable();

    self.RefinedReport = ko.computed(function () {
        vendorFiltered = self.ReportData().filter(function (row) {
            if (!self.SelectedVendor()) return true;
            return (row.Vendor() == self.SelectedVendor());
        })

        return vendorFiltered.sort(function (a, b) {
            if (!self.SortMethod()) return 0;

            if (self.SortMethod() == self.Sort().ItemName.Asc || self.SortMethod() == self.Sort().ItemName.Desc) {
                var val = a.Name().localeCompare(b.Name());
                return self.Sort().ItemName.Desc == self.SortMethod() ? val * -1 : val;
            }

            if (self.SortMethod() == self.Sort().Sku.Asc || self.SortMethod() == self.Sort().Sku.Desc) {
                var val = a.Sku().localeCompare(b.Sku());
                return self.Sort().Sku.Desc == self.SortMethod() ? val * -1 : val;
            }

            if (self.SortMethod() == self.Sort().Vendor.Asc || self.SortMethod() == self.Sort().Vendor.Desc) {
                var val = a.Vendor().localeCompare(b.Vendor());
                return self.Sort().Vendor.Desc == self.SortMethod() ? val * -1 : val;
            }

            if (self.SortMethod() == self.Sort().Variant.Asc || self.SortMethod() == self.Sort().Variant.Desc) {
                var val = a.Variant().localeCompare(b.Variant());
                return self.Sort().Variant.Desc == self.SortMethod() ? val * -1 : val;
            }

            if (self.SortMethod() == self.Sort().CostOfGood.Asc || self.SortMethod() == self.Sort().CostOfGood.Desc) {
                var val = a.CostOfGood() - (b.CostOfGood());
                return self.Sort().CostOfGood.Desc == self.SortMethod() ? val * -1 : val;
            }

            if (self.SortMethod() == self.Sort().SalePrice.Asc || self.SortMethod() == self.Sort().SalePrice.Desc) {
                var val = a.SalePrice() - (b.SalePrice());
                return self.Sort().SalePrice.Desc == self.SortMethod() ? val * -1 : val;
            }

            if (self.SortMethod() == self.Sort().Markup.Asc || self.SortMethod() == self.Sort().Markup.Desc) {
                var val = a.Markup() - (b.Markup());
                return self.Sort().Markup.Desc == self.SortMethod() ? val * -1 : val;
            }

            if (self.SortMethod() == self.Sort().DateSold.Asc || self.SortMethod() == self.Sort().DateSold.Desc) {
                var val = a.DateSold() - (b.DateSold());
                return self.Sort().DateSold.Desc == self.SortMethod() ? val * -1 : val;
            }


        })
    })

    self.ReportPaging = ko.observable(new PaginationVM(self.RefinedReport));

    self.TotalCOGs = ko.computed(function () {
        var total = 0;
        for (i in self.RefinedReport()) {
            total += self.RefinedReport()[i].CostOfGood();
        }
        return total;
    })

    self.TotalSales = ko.computed(function () {
        var total = 0;
        for (i in self.RefinedReport()) {
            total += self.RefinedReport()[i].SalePrice() * self.RefinedReport()[i].Quantity();   
        }
        return total;
    })

    self.TotalMarkup = ko.computed(function () {
        var total = 0;
        for (i in self.RefinedReport()) {
            total += self.RefinedReport()[i].Markup();
        }
        return total;
    })

    self.Loading = ko.observable({ Page: ko.observable(true), GetReportData: ko.observable(false), ExportReport: ko.observable(false) });
    self.IsLoading = ko.computed(function () {
        for (i in self.Loading()) {
            if (self.Loading()[i]()) { return true }
        }
        return false;
    })

    self.ChangeSort = function (vm, evt) {
        var sort = self.Sort()[evt.currentTarget.dataset.sort];
        self.SortMethod(self.SortMethod() == sort.Asc ? sort.Desc : sort.Asc);
    }

    self.SetSort = function () {
        var headers = ["ItemName", "Sku", "Vendor", "Variant", "CostOfGood", "SalePrice", "Markup", "DateSold"];
        var out = [];
        for (i in headers) {
            out[headers[i]] = { Asc: BaseModel.Guid.New(), Desc: BaseModel.Guid.New() };
        }
        self.Sort(out);
    }

    self.ThisMonth = function () {
        var start = new Date();
        var end = new Date();
        start.setDate(1);
        self.StartDate(start);
        self.EndDate(end);
    };

    self.ThisYear = function () {
        var start = new Date();
        var end = new Date();
        start.setDate(1);
        start.setMonth(0);
        self.StartDate(start);
        self.EndDate(end);
    };

    self.LastMonth = function () {
        var start = new Date();
        var end = new Date();
        start.setDate(1);
        start.setMonth(start.getMonth() - 1);
        end.setDate(0);
        self.StartDate(start);
        self.EndDate(end);
    };

    self.GetReportData = function () {
        try {
            self.Loading().GetReportData(true);

            if (!self.StartDate()) { self.StartDate(new Date()) }
            if (!self.EndDate()) { self.EndDate(new Date()) }

            var data = {     
                CompanySiteId: BaseModel.Cookies("current_site_id"),
                StartDate: self.StartDate().toDateString(),
                EndDate: self.EndDate().toDateString()
            };

            BaseModel.ServiceCall('/Report/RunCOGReport', 'post', data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage };
                    // Uncomment if you need to test for visibility
                    //response.CostOfGoods.push({})
                    self.ReportData(response.CostOfGoods.map(function (good) {
                        return new COGModel(good);
                    }))
                    self.Vendors(response.Vendors);
                    self.TotalDiscounts(response.Discounts);
                    self.TotalRefunds(response.Refunds);

                    if (!response.CostOfGoods.length) {
                        self.Status("No Data found for this range.");
                    }

                } catch (ex) {
                    BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                    BaseModel.Log(ex);
                }
                finally {
                    self.Loading().GetReportData(false);
                }
            })
        } catch (ex) {
            self.Loading().GetReportData(false);
            BaseModel.Log(ex);
        }
    }

    self.ExportReport = function () {
        try {
            self.Loading().ExportReport(true);

            if (!self.StartDate()) { self.StartDate(new Date()) }
            if (!self.EndDate()) { self.EndDate(new Date()) }

            var data = {
                StartDate: self.StartDate().toDateString(),
                EndDate: self.EndDate().toDateString()
            };

            BaseModel.ServiceCall('/Report/ExportCOGReport', 'post', data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage };
                    BaseModel.SaveByteArray([new Uint8Array(response.Report)], "COG Report.csv");
                    BaseModel.Message('Your report is ready', BaseModel.MessageLevels.Success);

                } catch (ex) {
                    BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                    BaseModel.Log(ex);
                }
                finally {
                    self.Loading().ExportReport(false);
                }
            })
        } catch (ex) {
            self.Loading().ExportReport(false);
            BaseModel.Log(ex);
        }
    }

    self.Load = function () {
        self.SetSort();
        self.Loading().Page(false);
    }

    self.Load();
}

var GiftCardsReportViewModel2 = function () {
    var self = this;

    self.StartDate = ko.observable();
    self.EndDate = ko.observable();
    self.Status = ko.observable();
    self.PerPage = ko.observable();
    self.Page = ko.observable();
    self.GiftCards = ko.observableArray();
    self.GiftCardProducts = ko.observableArray();
    self.SelectedNote = ko.observable("");
    self.SearchText = ko.observable("");
    self.SortMethod = ko.observable(0);

    self.FilteredLength = ko.observable(0);

    self.IssuerFilter = ko.observable();
    self.CardTypeFilter = ko.observable();

    self.Loading = ko.observable(true);
    self.LoadingReport = ko.observable(false);

    self.IsLoading = ko.computed(function () {
        if (self.Loading()) return true;
        if (self.LoadingReport()) return true;
        return false;
    })

    self.FilteredCards = ko.computed(function () {
        if (self.IsLoading()) return [];

        var typeFiltered = [];
        var type = self.IssuerFilter();
        if (type == "internal") {
            for (i in self.GiftCards()) {
                let card = self.GiftCards()[i];
                if (card.IssuedBy())
                    typeFiltered.push(card);
            }
        }
        else if (type == "external") {
            for (i in self.GiftCards()) {
                let card = self.GiftCards()[i];
                if (!card.IssuedBy())
                    typeFiltered.push(card);
            }
        }
        else {
            for (i in self.GiftCards()) {
                let card = self.GiftCards()[i];
                typeFiltered.push(card);
            }
        }

        var out = []

        if (self.SearchText()) {
            for (i in typeFiltered) {
                let card = typeFiltered[i];
                let searchString = card.Customer() + " " + card.LastFour() + " " + card.IssuedBy();
                var expr = new RegExp(self.SearchText(), "i");

                if (searchString.match(expr)) {
                    out.push(card);
                }
            }
        }
        else {
            out = typeFiltered;
        }

        self.FilteredLength(out.length);
        console.log(self.FilteredLength());
        return out;
    })


    self.SortedCards = ko.computed(function () {
        var out = self.FilteredCards();

        if (self.SortMethod() == 0) {
            out.sort(function (a, b) {
                if (a.LastFour().toUpperCase() > b.LastFour().toUpperCase()) return 1;
                if (a.LastFour().toUpperCase() < b.LastFour().toUpperCase()) return -1;
                return 0;
            })
        }

        if (self.SortMethod() == 1) {
            out.sort(function (a, b) {
                if (a.LastFour().toUpperCase() > b.LastFour().toUpperCase()) return -1;
                if (a.LastFour().toUpperCase() < b.LastFour().toUpperCase()) return 1;
                return 0;
            })
        }

        if (self.SortMethod() == 2) {
            out.sort(function (a, b) {
                if (a.Balance() > b.Balance()) return 1;
                if (a.Balance() < b.Balance()) return -1;
                return 0;
            })
        }

        if (self.SortMethod() == 3) {
            out.sort(function (a, b) {
                if (a.Balance() > b.Balance()) return -1;
                if (a.Balance() < b.Balance()) return 1;
                return 0;
            })
        }

        if (self.SortMethod() == 4) {
            out.sort(function (a, b) {
                if (a.IssuedBy().toUpperCase() > b.IssuedBy().toUpperCase()) return 1;
                if (a.IssuedBy().toUpperCase() < b.IssuedBy().toUpperCase()) return -1;
                return 0;
            })
        }

        if (self.SortMethod() == 5) {
            out.sort(function (a, b) {
                if (a.IssuedBy().toUpperCase() > b.IssuedBy().toUpperCase()) return -1;
                if (a.IssuedBy().toUpperCase() < b.IssuedBy().toUpperCase()) return 1;
                return 0;
            })
        }

        if (self.SortMethod() == 6) {
            out.sort(function (a, b) {
                if (a.Customer().toUpperCase() > b.Customer().toUpperCase()) return 1;
                if (a.Customer().toUpperCase() < b.Customer().toUpperCase()) return -1;
                return 0;
            })
        }

        if (self.SortMethod() == 7) {
            out.sort(function (a, b) {
                if (a.Customer().toUpperCase() > b.Customer().toUpperCase()) return -1;
                if (a.Customer().toUpperCase() < b.Customer().toUpperCase()) return 1;
                return 0;
            })
        }

        if (self.SortMethod() == 8) {
            out.sort(function (a, b) {
                if (a.DateCreated() > b.DateCreated()) return 1;
                if (a.DateCreated() < b.DateCreated()) return -1;
                return 0;
            })
        }

        if (self.SortMethod() == 8) {
            out.sort(function (a, b) {
                if (a.DateCreated() > b.DateCreated()) return -1;
                if (a.DateCreated() < b.DateCreated()) return 1;
                return 0;
            })
        }

        return out;
    })

    self.PaginationVM = ko.observable(new PaginationViewModel({
        Page: self.Page,
        PerPage: self.PerPage,
        ArrayLength: self.FilteredLength
    }));

    self.Visible = ko.computed(function () {
        if (self.IsLoading()) return [];
        var perPage = parseInt(self.PerPage());
        var first = (self.Page() * perPage) - perPage;
        return self.SortedCards().slice(first, first + perPage);
    })

    self.GiftCardTotalBalance = ko.computed(function () {
        if (self.IsLoading()) return 0;
        var total = 0;
        for (i in self.SortedCards()) {
            let card = self.SortedCards()[i];
            total += +card.InitialValue();

        }
        return total;
    })

    self.GiftCardTotalUsed = ko.computed(function () {
        if (self.IsLoading()) return 0;
        var total = 0;
        for (i in self.SortedCards()) {
            let card = self.SortedCards()[i];
            total += +card.Balance();
        }
        return self.GiftCardTotalBalance() - total;
    })

    self.GiftCardTotalRemaining = ko.computed(function () {
        if (self.IsLoading()) return 0;
        return self.GiftCardTotalBalance() - self.GiftCardTotalUsed()
    })

    self.Load = function () {
        $('.wait-container').show();
        self.LoadGiftCards();
        self.Loading(false);
    }

    self.ShowNote = function (item) {
        self.SelectedNote(item.Note());
    }

    self.HideNote = function () {
        self.SelectedNote("");
    }

    self.LoadGiftCards = function () {
        self.Status("Loading Gift Cards");
        self.LoadingReport(true);
        var data = {};
        Base.ServiceCall("/Report/RunGiftCardReport", "get", data, null, function (response) {
            if (response.IsSuccess) {
                for (i in response.GiftCardProducts) {
                    self.GiftCardProducts.push(response.GiftCardProducts[i]);
                }
                self.GiftCards.removeAll();
                for (i in response.GiftCards) {
                    self.GiftCards.push(new GiftCardModel(response.GiftCards[i]));
                }
                self.LoadPagination();
            }
            else {
                Base.Message("Error retrieving report.", "error");
            }
            self.LoadingReport(false);
        })
    }

    self.RunReport = function () {
        self.LoadGiftCards();
    }

    self.ExportReport = function () {
        self.LoadingReport(true);
        var data = {};
        self.Status("Exporting Gift Cards");
        Base.ServiceCall("/Report/ExportGiftCardReport", "post", data, true, function (response) {
            if (response.IsSuccess) {
                Base.SaveByteArray([new Uint8Array(response.Report)], "Gift Card Report.csv");
                swal("Your report is ready", "", "success");
            } else {
                swal("Error exporting report", response.ErrorMessage, "error");
            }
            self.LoadingReport(false);
        });
    };

    self.ChangeSort = function (initial) {
        return function () {
            if (self.SortMethod() == initial) self.SortMethod(initial + 1);
            else self.SortMethod(initial);
        }
    }

    self.SortByGCNumber = self.ChangeSort(0);
    self.SortByBalance = self.ChangeSort(2);
    self.SortByIssuer = self.ChangeSort(4);
    self.SortByCustomer = self.ChangeSort(6);
    self.SortByCreationDate = self.ChangeSort(8);
}

function GiftCardsReportViewModel(parent) {
    var self = this;
    self.Parent = parent;

    self.StartDate = ko.observable();
    self.EndDate = ko.observable();

    self.Status = ko.observable();
    self.IssuerFilter = ko.observable();
    self.SearchText = ko.observable();

    self.Sort = ko.observable();
    self.SortMethod = ko.observable();

    self.ReportData = ko.observableArray();
    self.GiftCardTotalAmountUsed = ko.observable();
    self.GiftCardTotalLiability = ko.observable();
    self.RefinedReport = ko.computed(function () {
        var issuer = self.ReportData().filter(function (card) {
            var filter = self.IssuerFilter();
            if (filter == 'all') return true;
            if (filter == 'internal') {
                return card.IssuedBy();
            }
            else {
                return (!card.IssuedBy());
            }
        });
        return issuer.filter(function (card) {
            var searchExpr = new RegExp(self.SearchText(), "i");
            var searchString = card.LastFour() + " " + card.Customer() + " " + card.IssuedBy();
            return searchString.match(searchExpr);
        })
    })

    self.ReportPaging = ko.observable(new PaginationVM(self.RefinedReport));

    self.GiftCardTotalAmountIssued = ko.computed(function () {
        var input = self.RefinedReport();
        var balance = 0;
        for (i in input) {
            var card = input[i];
            balance += card.InitialValue();
        }
        return balance;
    })

    //self.GiftCardTotalAmountIssued = ko.computed(function () {
    //    var input = self.RefinedReport();
    //    var balance = 0;
    //    for (i in input) {
    //        var card = input[i];
    //        balance += card.Balance();
    //    }
    //    return balance;
    //})

    self.Loading = ko.observable({ Page: ko.observable(true), GetReportData: ko.observable(false), ExportReport: ko.observable(false) });
    self.IsLoading = ko.computed(function () {
        for (i in self.Loading()) {
            if (self.Loading()[i]()) { return true }
        }
        return false;
    })

    self.ChangeSort = function (vm, evt) {
        var sort = self.Sort()[evt.currentTarget.dataset.sort];
        self.SortMethod(self.SortMethod() == sort.Asc ? sort.Desc : sort.Asc);
    }

    self.SetSort = function () {
        var headers = ["GCNumber", "Balance", "IssuedBy", "Customer", "CreationDate"];
        var out = [];
        for (i in headers) {
            out[headers[i]] = { Asc: BaseModel.Guid.New(), Desc: BaseModel.Guid.New() };
        }
        self.Sort(out);
    }

    self.ThisMonth = function () {
        var start = new Date();
        var end = new Date();
        start.setDate(1);
        self.StartDate(start);
        self.EndDate(end);
    };

    self.ThisYear = function () {
        var start = new Date();
        var end = new Date();
        start.setDate(1);
        start.setMonth(0);
        self.StartDate(start);
        self.EndDate(end);
    };

    self.LastMonth = function () {
        var start = new Date();
        var end = new Date();
        start.setDate(1);
        start.setMonth(start.getMonth() - 1);
        end.setDate(0);
        self.StartDate(start);
        self.EndDate(end);
    };

    self.GetReportData = function () {
        try {
            self.Loading().GetReportData(true);

            if (!self.StartDate()) { self.StartDate(new Date()) }
            if (!self.EndDate()) { self.EndDate(new Date()) }

            var data = {
                CompanySiteId: BaseModel.Cookies("current_site_id"),
                StartDate: self.StartDate().toDateString(),
                EndDate: self.EndDate().toDateString()
            };

            BaseModel.ServiceCall('/Report/RunGiftCardReport', 'get', data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage };

                    self.ReportData(response.GiftCards.map(function (giftCard) {
                        return new GiftCardModel(giftCard);
                    }));
                    self.GiftCardTotalAmountUsed(response.TotalAmountUsed);
                    self.GiftCardTotalLiability(response.TotalLiability);

                } catch (ex) {
                    BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                    BaseModel.Log(ex);
                }
                finally {
                    self.Loading().GetReportData(false);
                }
            })
        } catch (ex) {
            self.Loading().GetReportData(false);
            BaseModel.Log(ex);
        }
    }

    self.ExportReport = function () {
        try {
            self.Loading().ExportReport(true);

            if (!self.StartDate()) { self.StartDate(new Date()) }
            if (!self.EndDate()) { self.EndDate(new Date()) }

            var data = {
                StartDate: self.StartDate().toDateString(),
                EndDate: self.EndDate().toDateString()
            };

            BaseModel.ServiceCall('/Report/ExportGiftCardReport', 'post', data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage };
                    BaseModel.SaveByteArray([new Uint8Array(response.Report)], "Gift Card Report.csv");
                    BaseModel.Message('Your report is ready', BaseModel.MessageLevels.Success);

                } catch (ex) {
                    BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                    BaseModel.Log(ex);
                }
                finally {
                    self.Loading().ExportReport(false);
                }
            })
        } catch (ex) {
            self.Loading().ExportReport(false);
            BaseModel.Log(ex);
        }
    }

    self.Load = function () {
        self.SetSort();
        //self.GetReportData();
        self.Loading().Page(false);
    }

    self.Load();
}

function SalesReportViewModel(parent) {
    var self = this;
    self.Parent = parent;

    self.Status = ko.observable("");
    self.StartDate = ko.observable();
    self.EndDate = ko.observable();

    self.SalesReport = ko.observable(new SalesReport());

    self.Loading = ko.observable({ Page: ko.observable(true), GetReportData: ko.observable(false) });
    self.IsLoading = ko.computed(function () {
        for (i in self.Loading()) {
            if (self.Loading()[i]()) { return true }
        }
        return false;
    })

    self.ThisMonth = function () {
        var start = new Date();
        var end = new Date();
        start.setDate(1);
        self.StartDate(start);
        self.EndDate(end);
    };

    self.ThisYear = function () {
        var start = new Date();
        var end = new Date();
        start.setDate(1);
        start.setMonth(0);
        self.StartDate(start);
        self.EndDate(end);
    };

    self.LastMonth = function () {
        var start = new Date();
        var end = new Date();
        start.setDate(1);
        start.setMonth(start.getMonth() - 1);
        end.setDate(0);
        self.StartDate(start);
        self.EndDate(end);
    };

    self.GetReportData = function () {
        try {
            self.Loading().GetReportData(true);

            if (!self.StartDate()) { self.StartDate(new Date); }
            if (!self.EndDate()) { self.EndDate(new Date); }

            var data = {
                CompanySiteId: BaseModel.Cookies("current_site_id"),
                StartDate: self.StartDate().toDateString(),
                EndDate: self.EndDate().toDateString(),
            };
            BaseModel.ServiceCall('/Report/GetSalesReport', 'get', data, true, function (response) {
                try {
                    if (!response.IsSuccess) { throw response.ErrorMessage };
                    self.SalesReport(new SalesReport(response));

                } catch (ex) {
                    BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                    BaseModel.Log(ex);
                }
                finally {
                    self.Loading().GetReportData(false);
                }
            })
        } catch (ex) {
            self.Loading().GetReportData(false);
            BaseModel.Log(ex);
        }
    }

    self.Load = function () {
        self.Loading().Page(false);
    }

    self.Load();
}
