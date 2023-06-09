function DashboardViewModel() {
    var self = this;

    self.GiftCardTotalWidgetModel = ko.observable(new GiftCardTotalWidgetModel());

    self.Load = function () {
        self.GiftCardTotalWidgetModel().Load();
    };
}

function GiftCardTotalWidgetModel() {
    var self = this;

    self.CurrentYear = ko.observable("");
    self.GiftCardTotals = ko.observable(new GiftCardTotalsDataModel());
    self.GiftCardMonthlyTotals = ko.observableArray([]);
    self.LatestUploadDate = ko.observable("");

    self.Loading = ko.observable({
        Page: ko.observable(true),
        GetTotalCounts: ko.observable(false),
        GetGridData: ko.observable(false),
        GetLatestsUploadDate: ko.observable(false)
    });
    self.IsLoading = ko.computed(function () {
        var loading = self.Loading();
        for (var i in loading) {
            if (loading.hasOwnProperty(i)) {
                if (loading[i]()) {
                    $("#gift-card-totals").addClass("ld-loading");
                    return;
                }
            }
        }
        $("#gift-card-totals").removeClass("ld-loading");
    });

    self.GetLatestsUploadDate = function () {
        self.Loading().GetLatestsUploadDate(true);
        BaseModel.ServiceCall("/Dashboard/GetLatestsUploadDate", "post", null, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.LatestUploadDate(BaseModel.ToDate(response.LatestUploadDate));
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            } finally {
                self.Loading().GetLatestsUploadDate(false);
            }
        });
    };

    self.GetTotalCounts = function () {
        self.Loading().GetTotalCounts(true);
        data = { Year: self.CurrentYear() };
        BaseModel.ServiceCall("/Dashboard/GetShopifyGiftCardTotals", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.GiftCardTotals(new GiftCardTotalsDataModel(response.ShopifyGiftCardTotals));
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            } finally {
                self.Loading().GetTotalCounts(false);
            }
        });
    };

    self.GetGridData = function () {
        self.Loading().GetGridData(true);
        data = { Year: self.CurrentYear() };
        BaseModel.ServiceCall("/Dashboard/GetShopifyGiftCardMonthlyTotals", "post", data, true, function (response) {
            try {
                if (!response.IsSuccess) { throw response.ErrorMessage; }
                self.GiftCardMonthlyTotals(response.ShopifyGiftCardMonthlyTotals.map(function (monthlyTotal) {
                    return new GiftCardMonthlyTotalsDataModel(monthlyTotal);
                }));
                self.LoadChart();
            } catch (ex) {
                BaseModel.Message("Error: " + ex, BaseModel.MessageLevels.Error);
                BaseModel.Log(ex);
            } finally {
                self.Loading().GetGridData(false);
            }
        });
    };

    self.LoadChart = function () {
        var data1 = { "label": "Gift Cards From Returns", "data": [] };
        var data2 = { "label": "Total Gift Cards", "data": [] };

        self.GiftCardMonthlyTotals().map(function (monthlyTotal) {
            data1.data.push([monthlyTotal.Month(), monthlyTotal.GiftCardAmountFromReturns()]);
            data2.data.push([monthlyTotal.Month(), monthlyTotal.GiftCardTotalAmount()]);
        });
        var chartUsersOptions = {
            series: { splines: { show: true, tension: 0.4, lineWidth: 1, fill: 1 } },
            grid: { tickColor: "#404652", borderWidth: 0, borderColor: '#404652', color: '#ffffff' },
            legend: { show: true, noColumns: 2, container: $('#legendholder') },
            xaxis: {
                mode: "time",
                ticks: [[0, "Jan"], [1, "Feb"], [2, "Mar"], [3, "Apr"], [4, "May"], [5, "Jun"],
                        [6, "Jul"], [7, "Aug"], [8, "Sep"], [9, "Oct"], [10, "Nov"], [11, "Dec"]]
            },
            yaxis: { ticks: 4 },
            colors: ["#de4284", "#9B2E5C"]
        };

        $.plot($("#GiftCardTotalsChart"), [data2, data1], chartUsersOptions);
    };

    self.GetLastYear = function () {
        self.LoadData("2018");
    };

    self.GetThisYear = function () {
        self.LoadData("2019");
    };

    self.LoadData = function (year) {
        self.CurrentYear(year);
        self.GetLatestsUploadDate();
        self.GetTotalCounts();
        self.GetGridData();
    };

    self.Load = function () {
        self.LoadData("2019");
        self.Loading().Page(false);
    };
}