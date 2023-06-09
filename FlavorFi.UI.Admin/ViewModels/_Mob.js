var Mob = {
    ServiceCall: function (url, type, data, async, callback) {
        if (callback == undefined || url == undefined) {
            var badcall = { IsSuccess: false, ErrorMessage: "Call must provide callback and url!", IsTokenBad: false };
            callback(badcall);
        }
        if (type == undefined || type.length == 0) { type = "get"; }
        if (async == undefined || async.length == 0) { async = true; }
        if (data == undefined || data.length == 0) { data = {}; }

        var RootUrl = "";

        $.ajax({
            type: type,
            url: RootUrl + url,
            data: data,
            cache: false,
            async: async,
            success: function (response) {
                if (response.IsTokenBad) { window.location = "/Account"; };
                callback(response);
            },
            error: function (xhr2, status2, error2) {
                var message = status2 + " / " + error2;
                if (xhr2 != undefined && xhr2.responseText != undefined) {
                    var s = xhr2.responseText.indexOf("<title>");
                    var e = xhr2.responseText.indexOf("</title>");
                    if (s > 0 && e > s) {
                        var l = e - s;
                        message = xhr2.responseText.substr(s + 7, l - 7);
                    }
                }
                var response = { IsSuccess: false, ErrorMessage: "Error: " + message, IsTokenBad: false };

                callback(response);
            }
        });
    },
    Message: function (message, level, autoDismiss) {
        $.notify(message, level);
    },
    MessageLevels: {
        Success: 'success',
        Info: 'info',
        Warning: 'warn',
        Error: 'error'
    },
    Log: function (exception) {
        console.error(exception);
    },
    Cookies: function (cookie, newVal) {
        var cookies = document.cookie.split(";");
        var allCookies = {};
        for (i in cookies) {
            var cookieParts = cookies[i].split('=');
            var key = cookieParts.shift();
            var value = cookieParts.join("=");
            if (key && value) allCookies[key.toLowerCase().trim()] = value.trim();
        }

        if (cookie) {
            if (newVal) {
                var newCookie = cookie + "=" + newVal + "; path=/";
                document.cookie = newCookie;
                return allCookies[cookie.toLowerCase()] = newVal;
            }
            else {
                return allCookies[cookie.toLowerCase()];
            }
        }
        return allCookies;
    },
    GetUrlQuery: function (query) {
        var args = window.location.href.split('?')
        var params = {};
        if (args.length <= 1) { return null; }
        var url = args[1].split('#')[0].split("&");
        for (i in url) {
            param = url[i].split("=");
            params[param[0].toLowerCase()] = param[1];
        }
        if (query) {
            return params[query];
        }
        return params;
    },
    Guid: {
        Empty: '00000000-0000-0000-0000-000000000000',
        Test: '00000000-0000-0000-0000-000000000000',
        New: function () {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
        }
    },
    ToDate: function (date) {
        try {
            if (date) {
                if (date instanceof Date) { return date }
                return new Date(parseInt((date.match(/\d+/) || 0)));
            }
        }
        catch (ex) {
            console.error('Date parse Failed', date);
        }
        //return new Date();
    },
    States: [
        { Name: "Alabama", ShortName: "AL" },
        { Name: "Alaska", ShortName: "AK" },
        { Name: "Arizona", ShortName: "AZ" },
        { Name: "Arkansas", ShortName: "AR" },
        { Name: "California", ShortName: "CA" },
        { Name: "Colorado", ShortName: "CO" },
        { Name: "Connecticut", ShortName: "CT" },
        { Name: "Delaware", ShortName: "DE" },
        { Name: "District of Columbia", ShortName: "DC" },
        { Name: "Florida", ShortName: "FL" },
        { Name: "Georgia", ShortName: "GA" },
        { Name: "Guam", ShortName: "GU" },
        { Name: "Hawaii", ShortName: "HI" },
        { Name: "Idaho", ShortName: "ID" },
        { Name: "Illinois", ShortName: "IL" },
        { Name: "Indiana", ShortName: "IN" },
        { Name: "Iowa", ShortName: "IA" },
        { Name: "Kansas", ShortName: "KS" },
        { Name: "Kentucky", ShortName: "KY" },
        { Name: "Louisiana", ShortName: "LA" },
        { Name: "Maine", ShortName: "ME" },
        { Name: "Maryland", ShortName: "MD" },
        { Name: "Massachusetts", ShortName: "MA" },
        { Name: "Michigan", ShortName: "MI" },
        { Name: "Minnesota", ShortName: "MN" },
        { Name: "Mississippi", ShortName: "MS" },
        { Name: "Missouri", ShortName: "MO" },
        { Name: "Montana", ShortName: "MT" },
        { Name: "Nebraska", ShortName: "NE" },
        { Name: "Nevada", ShortName: "NV" },
        { Name: "New Hampshire", ShortName: "NH" },
        { Name: "New Jersey", ShortName: "NJ" },
        { Name: "New Mexico", ShortName: "NM" },
        { Name: "New York", ShortName: "NY" },
        { Name: "North Carolina", ShortName: "NC" },
        { Name: "North Dakota", ShortName: "ND" },
        { Name: "Ohio", ShortName: "OH" },
        { Name: "Oklahoma", ShortName: "OK" },
        { Name: "Oregon", ShortName: "OR" },
        { Name: "Pennsylvania", ShortName: "PA" },
        { Name: "Rhode Island", ShortName: "RI" },
        { Name: "South Carolina", ShortName: "SC" },
        { Name: "South Dakota", ShortName: "SD" },
        { Name: "Tennessee", ShortName: "TN" },
        { Name: "Texas", ShortName: "TX" },
        { Name: "Utah", ShortName: "UT" },
        { Name: "Vermont", ShortName: "VT" },
        { Name: "Virginia", ShortName: "VA" },
        { Name: "Washington", ShortName: "WA" },
        { Name: "West Virginia", ShortName: "WV" },
        { Name: "Wisconsin", ShortName: "WI" },
        { Name: "Wyoming", ShortName: "WY" },
    ],
    PromiseCall: function (url, type, data) {
        return new Promise(function (resolve, reject) {
            Mob.ServiceCall(url, type, data, true, function (response) {
                if (response.IsSuccess) { resolve(response); }
                else { reject(response); }
            })
        })
    }
}