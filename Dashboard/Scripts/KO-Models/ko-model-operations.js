function OperationsViewModel() {
    var self = this;
    var usersAPI = $("#usersLink").attr('href');
    var groupsAPI = $("#groupsLink").attr('href');
    var userGroupsAPI = $("#userGroupsLink").attr('href');
    var operationsAPI = $("#OperationsLink").attr('href');
    var shippingAPI = operationsAPI + '/' + 'shipping';
    var stagingAPI = operationsAPI + '/' + 'staging';
    var stagingDetailAPI = operationsAPI + '/' + 'stagingdetail';
    var stagingCriteriaAPI = operationsAPI + '/' + 'stagingcriteria';
    var stagingItemsAPI = operationsAPI + '/' + 'stagingitems';

    self.Users = ko.observableArray([]);
    self.UpcomingShipments = ko.observableArray([]);


    self.StagingTopLevel = ko.observableArray([]);
    self.StagingDetail = ko.observableArray([]);

    self.Criteria = ko.observableArray([]);

    self.PastDueSales = ko.observable();
    self.AdjShipDollars = ko.observable();
    self.AdjShipDollarsInt = ko.observable();

    self.Materials = ko.observableArray();
    self.Equipment = ko.observableArray();

    self.loadUsers = function () {
        var load = $.ajax({ type: "GET", url: usersAPI, cache: false, data: {} });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    UserName: item.UserName,
                    FirstName: item.FirstName,
                    LastName: item.LastName,
                    Email: item.Email
                }
            });
            self.Users(array);
        });
    }

    self.LoadOnboardingParts = function () {

        var load = $.ajax({ type: "GET", url: shippingAPI, cache: false});
        load.done(function (data) {
            adjDol = 0;
            pastDol = 0;
            $.each(data, function (i, item) {
                $('.dtShippingPlan').DataTable().row.add([
                item.ID,
                item.OrderNo,
                item.OrderLine,
                item.Customer,
                item.CustomerName,
                item.Part,
                item.Description,
                item.QtySched,
                formatMoney(item.Price),
                formatMoney(item.Sales),
                item.Sales,
                formatToShortDate(item.DateDue.split('T')[0]),
                formatToShortDate(item.AdjShipDate.split('T')[0]),
                item.DateID
                ]).draw();
                
                if (item.AdjShipDate != '1900-01-01T00:00:00') {
                    adjDol += item.Sales;
                    
                }
                if (item.DateDue.split('T')[0] < GetDate()) {
                    pastDol += item.Sales;
                }
                
            });
            self.AdjShipDollarsInt(adjDol);
            self.AdjShipDollars(formatMoney(adjDol));
            self.PastDueSales(formatMoney(pastDol));
            self.UpcomingShipments(data);
        });
        load.fail(function () {
            console.log('test');
        });
    }

    self.UpdateAdjShipDate = function (ID, DateID, newDate) {
        var t = $.grep(self.UpcomingShipments(), function (e) { return e.ID == ID })[0];

        var u = { ID: parseInt(DateID), OrderNo: t.OrderNo, AdjShipDate: formatShortDate(newDate), OrderLine: t.OrderLine, DateDue: t.DateDue.split('T')[0] };
        var sendData = "OrderNo=" + t.OrderNo + "&AdjShipDate=" + formatShortDate(newDate) + "&OrderLine=" + t.OrderLine + "&DateDue=" + t.DateDue.split('T')[0];
        var update = $.ajax({ type: "PUT", url: shippingAPI + "/" + DateID, cache: false, data: u });
        update.done(function (data) {
        });
    }

    self.AddAdjShipDate = function (ID, newDate, obj, sale) {
        var t = $.grep(self.UpcomingShipments(), function (e) { return e.ID == ID })[0];
        
        var newDateID = 0;
        var sendData = "OrderNo=" + t.OrderNo+ "&AdjShipDate=" + formatShortDate(newDate) + "&OrderLine=" + t.OrderLine+ "&DateDue=" + t.DateDue.split('T')[0];
        var add = $.ajax({ type: "POST", cache: false, url: shippingAPI, data: sendData });
        add.success(function (data) {
            newDateID = data.ID;
            $(obj).parent().parent().parent().children().last().html(newDateID);
        });

        console.log(self.AdjShipDollarsInt());
        sale = parseFloat(sale);
        self.AdjShipDollarsInt(self.AdjShipDollarsInt() +  sale);
        console.log(self.AdjShipDollarsInt());
        self.AdjShipDollars(formatMoney(self.AdjShipDollarsInt()));

        return newDateID;
    }


    self.LoadStagingTopLevel = function () {
        var load = $.ajax({ type: "GET", url: stagingAPI, cache: false, data: {DaysOut: 14} });
        load.done(function (data) {
            $.each(data, function (i, item) {
                $('.dtStaging').DataTable().row.add([
                item.ID,
                '0',
                item.Job,
                formatToShortDate(item.DateStart.split('T')[0]),
                formatToShortDate(item.DateStageDue.split('T')[0]),
                item.Steps,
                item.PartNum,
                item.Description
                ]).draw();
            });
            self.StagingTopLevel(data);
        });
        load.fail(function () {
            console.log('test');
        });
    }

    self.LoadStagingDetail= function (Job) {
        var load = $.ajax({ type: "GET", url: stagingDetailAPI, cache: false, data: { Job: Job } });
        load.done(function (data) {
            $('.dtStagingDetail').DataTable().clear().draw();
            $.each(data, function (i, item) {
                $('.dtStagingDetail').DataTable().row.add([
                item.ID,
                '0',
                item.Job,
                item.Suffix,
                item.Seq,
                formatToShortDate(item.DATE_START.split('T')[0]),
                item.PARTNUM,
                item.DESCRIPTION,
                item.WC
                ]).draw();
            });
            
            self.StagingDetail(data);
        });
        load.fail(function () {
            console.log('test');
        });
    }

    self.LoadStagingCriteria = function (job) {
        var load = $.ajax({ type: "GET", url: stagingCriteriaAPI, cache: false});
        load.done(function (data) {
            
            self.Criteria(data);
        });
    }

    self.LoadSelectedStagingItems = function (JobID) {
        var load = $.ajax({ type: "GET", url: stagingItemsAPI, cache: false, data: {JobID: JobID} });
        load.done(function (data) {
            self.Materials($.grep(data, function (e) { return e.Type == 'Material' }));
            self.Equipment($.grep(data, function (e) { return e.Type == 'Equipment' }));
        });
        load.fail(function () {
            self.Materials([]);
            self.Equipment([]);
        });
    }

    self.AddItem = function (JobID, Type, Description, Location, Consumable) {
        var UserID = parseInt($('#layoutUserID').val());
        var sendData = "JobID=" + JobID + "&Type=" + Type + "&Description=" + Description + "&Location=" + Location + "&IssuerID=" + UserID+ "&Consumable=" + Consumable;
        var add = $.ajax({ type: "POST", cache: false, url: stagingItemsAPI, data: sendData });
        add.success(function (data) {
            console.log(data);
            self.LoadSelectedStagingItems();
        });
        $('#addItem').removeClass('hidden');
    }

    self.RemoveItem = function (ID) {

    }

}