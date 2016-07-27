function OnboardingViewModel() {
    var self = this;
    var usersAPI = $("#usersLink").attr('href');
    var groupsAPI = $("#groupsLink").attr('href');
    var userGroupsAPI = $("#userGroupsLink").attr('href');
    var onboardingAPI = $("#OnboardingLink").attr('href');

    self.isLoading = ko.observable();

    self.Users = ko.observableArray([]);
    self.OnboardingParts = ko.observableArray([]);
    self.OnboardingTasks = ko.observableArray([]);

    
    self.SelectedPart = ko.observable();
    self.SelectedTask = ko.observable();


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

    var P = function (part, tasks) {
        this.part = part;
        this.tasks = ko.observableArray([]);

    }
    self.Parts = ko.observableArray([]);

    self.SelectTask = function (TaskID, PartID) {
        console.log("Part: " + PartID);
        console.log("Task: " + TaskID);
        var tasks = $.grep(self.OnboardingTasks(), function (e) { return e[0].OnBoardPart == PartID })[0];
        var task = $.grep(tasks, function (e) { return e.RecordNumber == TaskID })[0];
        var part = $.grep(self.OnboardingParts(), function (e) { return e.RecordNumber == PartID})[0];
        console.log(task);
        self.SelectedPart(part);
        self.SelectedTask(task);

    }

    self.LoadOnboardingParts = function () {
        self.isLoading(true)
        var parts = [];
        var tasks = [];
        var outer = 0;
        var inner = 0;
        self.OnboardingTasks([]);

        var load = $.ajax({ type: "GET", cache: false, url: onboardingAPI });
        load.success(function (data) {
            self.OnboardingParts(data);
            outerStart = data.length;
            data.forEach(function (i) {
                i.DateEntered = formatSqlDateTimeToShortDate(i.DateEntered);
                i.DatePODue = formatSqlDateTimeToShortDate(i.DatePODue);

                outer += 1;
                var loadTasks = $.ajax({ type: "GET", url: onboardingAPI + '/' + i.RecordNumber + '/Tasks', cache: false });
                loadTasks.done(function (data1) {
                    innerStart = data1.length;
                    data1.forEach(function (ii) {
                        ii.DueBy = formatSqlDateTimeToShortDate(ii.DueBy);
                        ii.PercentComplete = formatPercent(ii.PercentComplete, 1);
                    });
                    if (data1[10].TaskDescription != 'FAI Populated') {
                        data1.splice(10, 0, { TaskDescription: "FAI Populated", PercentComplete: '0.0%', DueBy: 0, CurrentStatus: '', ActualCompletionHours: 0, StandardCompletionHours: 0 })
                    }
                    self.OnboardingTasks.push(data1);

                    inner += 1;
                    if (inner == outerStart) {
                        self.isLoading(false)
                    }
                });
            });
            self.OnboardingParts(data);
        });

        //var load = $.ajax({ type: "GET", url: onboardingAPI, cache: false });
        //load.done(function (data) {
        //    var tasks = [];
        //    self.OnboardingParts(data);
        //    data.each
        //    for (i = 0; i < data.length; i++) {
        //        var recordNum = data[i].RecordNumber;
        //        var subTasks = [];
        //        var loadTasks = $.ajax({ type: "GET", url: onboardingAPI + '/' + recordNum + '/Tasks', cache: false });
        //        loadTasks.done(function (taskData) {

        //            for (ii = 0; ii < taskData.length; ii++) {
        //                tasks.push(taskData[ii]);
        //            }
        //            var p = $.grep(data, function (e) { return e.RecordNumber == taskData[0].OnBoardPart });
        //            p[0].Tasks = taskData;

        //        });

        //    }

        //    self.OnboardingTasks().push(taskData[ii]);

        //});
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

    self.LoadStagingDetail = function (Job) {
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
        var load = $.ajax({ type: "GET", url: stagingCriteriaAPI, cache: false });
        load.done(function (data) {

            self.Criteria(data);
        });
    }

    self.LoadSelectedStagingItems = function (JobID) {
        var load = $.ajax({ type: "GET", url: stagingItemsAPI, cache: false, data: { JobID: JobID } });
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
        var sendData = "JobID=" + JobID + "&Type=" + Type + "&Description=" + Description + "&Location=" + Location + "&IssuerID=" + UserID + "&Consumable=" + Consumable;
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