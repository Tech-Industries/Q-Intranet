function OperationsViewModel() {
    var self = this;
    var usersAPI = $("#usersLink").attr('href');
    var groupsAPI = $("#groupsLink").attr('href');
    var userGroupsAPI = $("#userGroupsLink").attr('href');
    var operationsAPI = $("#OperationsLink").attr('href');

    self.Users = ko.observableArray([]);
    self.UpcomingShipments = ko.observableArray([]);


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

        var load = $.ajax({ type: "GET", url: operationsAPI, cache: false, data: { Type: 'Shipping' } });
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
        var update = $.ajax({ type: "PUT", url: operationsAPI + "/" + DateID, cache: false, data:  u  });
        update.done(function (data) {
        });
    }

    self.AddAdjShipDate = function (ID, newDate, obj) {
        var t = $.grep(self.UpcomingShipments(), function (e) { return e.ID == ID })[0];
        
        var newDateID = 0;
        var sendData = "OrderNo=" + t.OrderNo+ "&AdjShipDate=" + formatShortDate(newDate) + "&OrderLine=" + t.OrderLine+ "&DateDue=" + t.DateDue.split('T')[0];
        var add = $.ajax({ type: "POST", cache: false, url: operationsAPI, data: sendData });
        add.success(function (data) {
            newDateID = data.ID;
            $(obj).parent().parent().parent().children().last().html(newDateID);
        });
        return newDateID;
    }


    self.loadData = function () {
        self.LoadUpcomingShipments();
    }
}