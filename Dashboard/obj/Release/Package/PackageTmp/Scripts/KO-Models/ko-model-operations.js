﻿function OperationsViewModel() {
    var self = this;
    var usersAPI = $("#usersLink").attr('href');
    var groupsAPI = $("#groupsLink").attr('href');
    var userGroupsAPI = $("#userGroupsLink").attr('href');
    var operationsAPI = $("#OperationsLink").attr('href');
    var shippingAPI = operationsAPI + '/' + 'shipping';
    var stagingAPI = operationsAPI + '/' + 'staging';
    var stagingDetailAPI = operationsAPI + '/' + 'stagingdetail';
    var stagingCriteriaAPI = operationsAPI + '/' + 'stagingcriteria';

    self.Users = ko.observableArray([]);
    self.UpcomingShipments = ko.observableArray([]);


    self.StagingTopLevel = ko.observableArray([]);
    self.StagingDetail = ko.observableArray([]);

    self.Criteria = ko.observableArray([]);

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

        var load = $.ajax({ type: "GET", url: shippingAPI, cache: false});
        load.done(function (data) {
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
                formatToShortDate(item.DateDue.split('T')[0]),
                formatToShortDate(item.AdjShipDate.split('T')[0]),
                item.DateID
                ]).draw();
            });
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

    self.AddAdjShipDate = function (ID, newDate, obj) {
        var t = $.grep(self.UpcomingShipments(), function (e) { return e.ID == ID })[0];
        
        var newDateID = 0;
        var sendData = "OrderNo=" + t.OrderNo+ "&AdjShipDate=" + formatShortDate(newDate) + "&OrderLine=" + t.OrderLine+ "&DateDue=" + t.DateDue.split('T')[0];
        var add = $.ajax({ type: "POST", cache: false, url: shippingAPI, data: sendData });
        add.success(function (data) {
            newDateID = data.ID;
            $(obj).parent().parent().parent().children().last().html(newDateID);
        });
        return newDateID;
    }


    self.LoadStagingTopLevel = function () {
        var load = $.ajax({ type: "GET", url: stagingAPI, cache: false });
        load.done(function (data) {
            $.each(data, function (i, item) {
                $('.dtStaging').DataTable().row.add([
                item.ID,
                '0',
                item.Job,
                formatToShortDate(item.DateStart.split('T')[0]),
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


}