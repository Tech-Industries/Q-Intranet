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
    var stagingAuditAPI = operationsAPI + '/' + 'stagingaudit';
    var stagingItemsAPI = operationsAPI + '/' + 'stagingitems';
    var stagingSnapshotAPI = operationsAPI + '/' + 'stagingsnapshot';
    var opportunitiesAPI = operationsAPI + '/' + 'opportunities';
    var utilitiesAPI = $("#UtilityLink").attr('href');

    self.Users = ko.observableArray([]);
    self.UpcomingShipments = ko.observableArray([]);
    self.isLoading = ko.observable(false);

    self.StagingTopLevel = ko.observableArray([]);
    self.StagingDetail = ko.observableArray([]);

    self.Criteria = ko.observableArray([]);
    self.Checklist = ko.observableArray([]);

    self.PastDueSales = ko.observable();
    self.AdjShipDollars = ko.observable();
    self.AdjShipDollarsInt = ko.observable();

    self.Equipment = ko.observableArray([]);

    self.StagingSnapshot = ko.observableArray([]);
    self.Total = ko.observableArray([]);
    self.Missing = ko.observableArray([]);
    self.Incomplete = ko.observableArray([]);
    self.Goal = ko.observableArray([]);
    self.StagingSnapshotCats = ko.observableArray([]);
    self.Opportunities = ko.observableArray([]);

    self.LoadOpportunities = function () {
        var load = $.ajax({ type: "GET", url: opportunitiesAPI, cache: false, data: {} });
        load.done(function (data) {
            self.Opportunities(data);
        });
    }

    self.DeleteOpportunity = function (id) {
        var rem = $.ajax({ type: 'DELETE', cache: false, url: opportunitiesAPI + '/' + id, data: id });
        rem.done(function () {
            self.Opportunities.remove($.grep(self.Opportunities(), function (e) { return e.ID == id })[0]);
        });
    }

    self.AddOpportunity = function () {
        if ($('#PackageName').val().length > 0) {
            sendData = $('#add-opp-form').serialize();
            var add = $.ajax({ type: "POST", cache: false, url: opportunitiesAPI, data: sendData });
            add.success(function (data) {
                self.Opportunities.push(data);
            });
        }
    }

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

    self.LoadUpcomingShipments = function () {
        self.isLoading(true);
        var load = $.ajax({ type: "GET", url: shippingAPI, cache: false });
        load.done(function (data) {
            adjDol = 0;
            pastDol = 0;
            var start = data.length;
            var count = 0;
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
                count += 1;
                if (count == start) {
                    self.isLoading(false);
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

    self.FilterShippingResults = function(filter){
        var today = new Date(Date);
        var range = new Date(Date);
        if (filter == 'This') { range.setDate(range.getDate() + 30); }
        else if (filter = 'Next') { today.add(1, 'month'); range.add(2, 'months'); }
        else if (filter = 'Two') { today.add(2, 'month'); range.add(3, 'months'); }
        console.log(today);
        console.log(range);
    }

    self.UpdateAdjShipDate = function (ID, DateID, newDate) {
        var t = $.grep(self.UpcomingShipments(), function (e) { return e.ID == ID })[0];

        var u = { ID: parseInt(DateID), OrderNo: t.OrderNo, AdjShipDate: formatShortDate(newDate), OrderLine: t.OrderLine };
        var sendData = "OrderNo=" + t.OrderNo + "&AdjShipDate=" + formatShortDate(newDate) + "&OrderLine=" + t.OrderLine;
        var update = $.ajax({ type: "PUT", url: shippingAPI + "/" + DateID, cache: false, data: u });
        update.done(function (data) {
        });
    }

    self.AddAdjShipDate = function (ID, newDate, obj, sale) {
        var t = $.grep(self.UpcomingShipments(), function (e) { return e.ID == ID })[0];

        var newDateID = 0;
        var sendData = "OrderNo=" + t.OrderNo + "&AdjShipDate=" + formatShortDate(newDate) + "&OrderLine=" + t.OrderLine + "&DateDue=" + t.DateDue.split('T')[0];
        var add = $.ajax({ type: "POST", cache: false, url: shippingAPI, data: sendData });
        add.success(function (data) {
            newDateID = data.ID;
            $(obj).parent().parent().parent().children().last().html(newDateID);
        });

        console.log(self.AdjShipDollarsInt());
        sale = parseFloat(sale);
        self.AdjShipDollarsInt(self.AdjShipDollarsInt() + sale);
        console.log(self.AdjShipDollarsInt());
        self.AdjShipDollars(formatMoney(self.AdjShipDollarsInt()));

        return newDateID;
    }

    

    self.LoadStagingTopLevel = function () {
        var load = $.ajax({ type: "GET", url: stagingAPI, cache: false, data: { DaysOut: 14 } });
        load.done(function (data) {
            $.each(data, function (i, item) {
                $('.dtStaging').DataTable().row.add([
                item.ID,
                item.Status,
                item.Job,
                item.Suffix,
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

    self.LoadStagingDetail = function (Job, Suffix) {
        console.log(Suffix);
        if (Suffix.length < 1) {
            Suffix = '';
        }
        var load = $.ajax({ type: "GET", url: stagingDetailAPI, cache: false, data: { Job: Job, Suffix: Suffix } });
        load.done(function (data) {
            $('.dtStagingDetail').DataTable().clear().draw();
            $.each(data, function (i, item) {
                $('.dtStagingDetail').DataTable().row.add([
                item.ID,
                item.Status,
                item.Job,
                item.Suffix,
                item.Seq,
                formatToShortDate(item.DATE_START),
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

    self.LoadStagingChecklist = function (id) {
        self.Checklist([]);
        var load = $.ajax({ type: "GET", url: stagingAuditAPI + '/' + id + '/checklist', cache: false, data: id });
        load.done(function (data) {
            self.Checklist(data);
        });
    }

    self.LoadSelectedStagingItems = function (StagingDetailID) {
        var load = $.ajax({ type: "GET", url: stagingItemsAPI + '/' + StagingDetailID, cache: false, data: { StagingDetailID: StagingDetailID } });
        load.done(function (data) {
            self.Equipment(data);
        });
        load.fail(function () {
            self.Equipment([]);
        });
    }

    self.AddItem = function (StagingDetailID, Type, Description, Location, Consumable) {
        var UserID = parseInt($('#layoutUserID').val());
        var sendData = "StagingDetailID=" + StagingDetailID + "&Type=" + Type + "&Description=" + Description + "&Location=" + Location + "&IssuerID=" + UserID + "&Consumable=" + Consumable;
        var add = $.ajax({ type: "POST", cache: false, url: stagingItemsAPI, data: sendData });
        add.success(function (data) {
            console.log(data);
            self.LoadSelectedStagingItems(StagingDetailID);
        });
        $('#addItem').removeClass('hidden');
    }

    self.RemoveItem = function (ItemID) {
        console.log('please remove');
        var rem = $.ajax({ type: 'DELETE', cache: false, url: stagingItemsAPI + '/' + ItemID, data: ItemID });
        rem.done(function () {
            self.Equipment.remove($.grep(self.Equipment(), function (e) { return e.ID == ItemID })[0]);
        });
    }

    self.StartStagingAudit = function (DetailID) {
        console.log(DetailID);
        var Job = $.grep(self.StagingDetail(), function (e) { return e.ID == DetailID })[0].Job;
        console.log(Job);
        var DateDue = $.grep(self.StagingTopLevel(), function (e) { return e.Job == Job })[0].DateStageDue;
        var sendData = "StagingDetailID=" + DetailID + "&DateDue=" + DateDue;

        console.log(sendData);
        var addAudit = $.ajax({ type: "POST", cache: false, url: stagingAuditAPI, data: sendData });
        addAudit.done(function () {
            self.LoadStagingChecklist(DetailID);
        });
    }

    self.UpdateAuditDetail = function (ID, Status) {
        var sendData = "ID=" + ID + "&Status=" + Status;
        var update = $.ajax({ type: "PUT", cache: false, url: stagingAuditAPI + "/detail/" + ID, data: sendData });
        update.done(function (data) {
            console.log('updated');
        });
    }


    self.ItemReturn = function (ItemID) {
        var u = $.grep(self.Equipment(), function (e) { return e.ID == ItemID })[0];
        var update = $.ajax({ type: "PUT", cache: false, url: stagingItemsAPI + '/' + ItemID, data: u });
        update.done(function (data) {
        });

    }

    self.LoadStagingSnapshot = function () {
        var load = $.ajax({ type: "GET", url: stagingSnapshotAPI, cache: false });
        load.done(function (data) {
            self.StagingSnapshot(data);
        });
    }

    self.LoadStagingSnapshotTrend = function () {
        self.Total([]);
        self.Missing([]);
        self.Incomplete([]);
        var Period = $('#periodSelect').val();
        var load = $.ajax({ type: "GET", url: stagingSnapshotAPI + '/trend', cache: false, data: { Period: Period } });
        load.done(function (data) {
            var total = [];
            var missing = [];
            var incomplete = [];
            for (x = 0; x < data.length; x++) {
                total.push(data[x].Total)
                missing.push(data[x].Missing)
                incomplete.push(data[x].Incomplete);
            }
            self.Total(total);
            self.Missing(missing);
            self.Incomplete(incomplete);
        });
    }

    self.LoadSnapShotCats = function () {
        var Period = $('#periodSelect').val();
        var load = $.ajax({ type: "GET", url: utilitiesAPI+'/calendar', cache: false, data: { Period: Period, Type: 'Daily' } });
        load.done(function (data) {
            var array = [];
            var goal = [];
            for (x = 0; x < data.length; x++) {
                array.push(data[x].Date.split('T')[0])
                goal.push(5);
            }
            self.Goal(goal);
            self.StagingSnapshotCats(array);
        });
    }

    self.LoadStagingMetrics = function () {
        self.LoadSnapShotCats();
        self.LoadStagingSnapshotTrend();
    }

   

}