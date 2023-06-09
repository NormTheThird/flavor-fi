// Custom Bindings

ko.bindingHandlers.fadeVisible = {
    init: function (_elem, _value) {
        var value = _value();
        $(_elem).toggle(ko.unwrap(value));
    },
    update: function (_elem, _value) {
        var value = _value();
        ko.unwrap(value) ? $(_elem).slideDown() : $(_elem).slideUp();
    }
}

ko.bindingHandlers.delayVisible = {
    init: function (_elem, _value) {
        var value = _value();
        $(_elem).toggle(ko.unwrap(value));
    },
    update: function (_elem, _value) {
        var value = _value();
        ko.unwrap(value) ? $(_elem).show(1000) : $(_elem).hide(1000);
    }
}

// Template Animators
ko.bindingHandlers.fadeTemplate = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        return ko.bindingHandlers['template']['init'](element, valueAccessor, allBindings);
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var value = ko.unwrap(valueAccessor());
        $(element).fadeOut(function () {
            ko.bindingHandlers['template']['update'](element, valueAccessor, allBindings, viewModel, bindingContext);
            $(this).fadeIn();
        });
    }
};

ko.bindingHandlers.slideTemplate = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        return ko.bindingHandlers['template']['init'](element, valueAccessor, allBindings);
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var value = ko.unwrap(valueAccessor());
        $(element).slideUp(function () {
            ko.bindingHandlers['template']['update'](element, valueAccessor, allBindings, viewModel, bindingContext);
            $(this).slideDown();
        });
    }
};


function ShowTemplate(elem) {
    console.log("Show template", elem)
    //for (i in elem) {
    //    if (elem[i].nodeType == 1) {
    //        $(elem[i]).slideDown();
    //    }
    //}
}
function HideTemplate(elem) {
    console.log("Hide template", elem)
    for (i in elem) {
        if (elem[i].nodeType == 1) {
            $(elem[i]).slideUp();
        }
    }
}


// Computed Obserable Extenders (magic)
function PhoneMask() {
    return ko.computed({
        read: function () {
            var phoneNumber = this() ? this() : "";
            if (phoneNumber.length < 1) return;
            var out = "";
            out = out + "(" + phoneNumber.substring(0, 3);
            if (phoneNumber.length > 3) out = out + ") " + phoneNumber.substring(3, 6);
            if (phoneNumber.length > 6) out = out + "-" + phoneNumber.substring(6, 10);
            return out;
        },
        write: function (_phoneNumber) {
            var phoneNumber = _phoneNumber.replace(/[^\.\d]/g, "");
            this(phoneNumber);
            this.notifySubscribers();
        },
        owner: this,
    }).extend({ notify: 'always' });
}

function ToAmount() {
    return ko.computed({
        read: function () {
            var num = ko.unwrap(this);
            return this() ? "$" + num.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",") : "$0.00";
        },
        write: function (_amount) {
            var amount = (_amount.match(/\d+\.?/g) || [0]).join("");
            this(parseFloat(amount));
            this.notifySubscribers();
        },
        owner: this,
    }).extend({ notify: 'always' });
}

function ToShortAmount() {
    return ko.computed({
        read: function () {
            var num = ko.unwrap(this);
            return this() ? "$" + num.toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ",") : "$0";
        },
        write: function (_amount) {
            this(_amount);
            this.notifySubscribers();
        },
        owner: this,
    }).extend({ notify: 'always' });
}

function OnlyPositive() {
    return ko.computed({
        read: function () {
            return this();
        },
        write: function (_amount) {
            this(Math.abs(_amount));
            this.notifySubscribers();
        },
        owner: this,
    }).extend({ notify: 'always' });
}

function ToShortDate() {
    return ko.computed({
        read: function () {
            if (this() instanceof Date) {
                return this().toLocaleDateString('en-US', { month: 'numeric', day: 'numeric', year: 'numeric' });
            }
        },
        write: function (date) {
            if (date) {
                this(new Date(date));
            }
            this.notifySubscribers();
        },
        owner: this,
    }).extend({ notify: 'always' });
}

function ToDaysAgo() {
    return ko.computed({
        read: function () {
            if (this() instanceof Date) {
                var today = new Date();
                return Math.floor((today - this()) / 1000 / 60 / 60 / 24) + " days ago.";
            }
        },
        write: function (date) {
            if (date) {
                this(new Date(date));
            }
            this.notifySubscribers();
        },
        owner: this,
    }).extend({ notify: 'always' });
}

ko.subscribable.fn.PhoneMask = PhoneMask;
ko.subscribable.fn.ToAmount = ToAmount;
ko.subscribable.fn.ToShortAmount = ToShortAmount;
ko.subscribable.fn.OnlyPositive = OnlyPositive;
ko.subscribable.fn.ToShortDate = ToShortDate;
ko.subscribable.fn.ToDaysAgo = ToDaysAgo;

function MainViewModel() {
    var self = this;
    self.NavigationVM = ko.observable(new NavigationViewModel(self));
    self.CurrentVM = ko.observable();
}