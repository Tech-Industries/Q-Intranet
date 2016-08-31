function OnboardingViewModel() {
    var self = this;
    var usersAPI = $("#usersLink").attr('href');
    var groupsAPI = $("#groupsLink").attr('href');
    var userGroupsAPI = $("#userGroupsLink").attr('href');
    var onboardingAPI = $("#OnboardingLink").attr('href');

    var CommentsAPI = $("#CommentsLink").attr('href');
    var AssignBtn = null;
    self.Comments = ko.observableArray([]);

    self.isLoading = ko.observable();

    self.Users = ko.observableArray([]);
    self.OnboardingParts = ko.observableArray([]);
    self.UnfilteredParts = ko.observableArray([]);
    self.OnboardingTasks = ko.observableArray([]);

    self.Customers = ko.observable([]);
    self.Projects = ko.observable([]);
    self.PartTypes = ko.observable([]);
    self.PartClasses = ko.observable([]);
    self.PartNums = ko.observable([]);



    self.SelectedPart = ko.observable();
    self.SelectedTask = ko.observable();

    self.SelectedTaskPure = ko.observable();

    self.AvailJobs = ko.observableArray([]);

    self.ArchivedParts = ko.observableArray([]);


    self.loadUsers = function () {
        var load = $.ajax({ type: "GET", url: usersAPI, cache: false, data: { type: 'Onboarding' } });
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
            console.log(data);
        });
    }

    var P = function (part, tasks) {
        this.part = part;
        this.tasks = ko.observableArray([]);

    }
    self.Parts = ko.observableArray([]);

    self.SelectTask = function (TaskID, PartID) {
        var tasks = $.grep(self.OnboardingTasks(), function (e) { return e[0].OnBoardPart == PartID })[0];
        var task = $.grep(tasks, function (e) { return e.RecordNumber == TaskID })[0];
        var part = $.grep(self.OnboardingParts(), function (e) { return e.RecordNumber == PartID })[0];
        self.SelectedPart(part);
        self.SelectedTask(task);

        $('#selectedDelegate').val(task.DelegatedTo);

    }

    self.SelectPart = function (PartID) {
        var part = $.grep(self.OnboardingParts(), function (e) { return e.RecordNumber == PartID })[0];
        self.SelectedPart(part);
    }

    self.UpdateTasksForPart = function () {
        var today = new Date();
        var Year = today.getFullYear();
        var Month = checkZero(today.getMonth() + 1);
        var Day = checkZero(today.getDate());
        var startDate = Year + '-' + Month + '-' + Day + 'T00:00:00';
        var rangeDate = addDays(Year, Month, Day, 10);

        var part = self.SelectedPart();
        var loadTasks = $.ajax({ type: "GET", url: onboardingAPI + '/' + part.RecordNumber + '/Tasks', cache: false });
        loadTasks.done(function (data1) {
            innerStart = data1.length;
            data1.forEach(function (ii) {
                ii.CurrentStatus = self.checkStatus(ii, startDate, rangeDate);
                user = $.grep(self.Users(), function (e) { return e.ID == ii.DelegatedTo })[0]
                ii.DelegatedToName = user.FirstName + ' ' + user.LastName;
                if (ii.DueBy != null) {
                    ii.DueBy = formatSqlDateTimeToFullDate(ii.DueBy);
                }
            });
            var tasks = $.grep(self.OnboardingTasks(), function (e) { return e[0].OnBoardPart == part.RecordNumber })[0];
            self.OnboardingTasks.remove(tasks);
            self.OnboardingTasks.push(data1);
        });


    }

    self.SaveChanges = function () {
        var update = self.SelectedTask();
        update.CompletedBy = $('#layoutUserID').val();

        var save = $.ajax({ type: "PUT", cache: false, url: onboardingAPI + '/Tasks/' + update.RecordNumber, data: update });
        save.done(function (data) {

            self.UpdateTasksForPart();
        });
    }

    self.LoadOnboardingParts = function () {
        var UserID = $('#layoutUserID').val();
        self.isLoading(true)
        var parts = [];
        var tasks = [];
        var outer = 0;
        var inner = 0;
        var today = new Date();

        var Year = today.getFullYear();
        var Month = checkZero(today.getMonth() + 1);
        var Day = checkZero(today.getDate());
        var startDate = Year + '-' + Month + '-' + Day + 'T00:00:00';
        var rangeDate = addDays(Year, Month, Day, 10);

        self.OnboardingTasks([]);
        var loadAuthParts = $.ajax({ type: "GET", cache: false, url: onboardingAPI + '/Authorized/' + UserID, error: function (data) { self.isLoading(false); } });
        loadAuthParts.done(function (authParts) {

            var load = $.ajax({ type: "GET", cache: false, url: onboardingAPI });
            load.success(function (data) {

                if ($('#RelPer').val() == 18) {
                    parts = data;
                }
                else {
                    for (x = 0; x < authParts.length; x++) {
                        parts.push($.grep(data, function (e) { return e.RecordNumber == authParts[x].OnBoardPart })[0])
                    }
                }

                self.OnboardingParts(parts);
                self.UnfilteredParts(parts);
                outerStart = parts.length;
                parts.forEach(function (i) {
                    self.Customers().add(i.Customer);
                    self.Projects().add(i.PackageName);
                    self.PartTypes().add(i.PartType);
                    self.PartClasses().add(i.OnBoardClass);
                    self.PartNums().add(i.PartNumber);
                    i.DateEntered = formatSqlDateTimeToFullDate(i.DateEntered);
                    i.DatePODue = formatSqlDateTimeToFullDate(i.DatePODue);
                    i.DatePOReceived = formatSqlDateTimeToFullDate(i.DatePOReceived);

                    outer += 1;
                    var loadTasks = $.ajax({ type: "GET", url: onboardingAPI + '/' + i.RecordNumber + '/Tasks', cache: false });
                    loadTasks.done(function (data1) {
                        innerStart = data1.length;
                        data1.forEach(function (ii) {
                            ii.CurrentStatus = self.checkStatus(ii, startDate, rangeDate);
                            user = $.grep(self.Users(), function (e) { return e.ID == ii.DelegatedTo })[0]
                            ii.DelegatedToName = user.FirstName+' '+user.LastName;
                            if (ii.DueBy != null) {
                                ii.DueBy = formatSqlDateTimeToFullDate(ii.DueBy);
                            }
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
            });
        });
    }

    self.DeletePart = function (PartID) {
        console.log(PartID);
        var update = $.grep(self.OnboardingParts(), function (e) { return e.RecordNumber == PartID })[0];
        var updatePart = $.ajax({ type: "DELETE", cache: false, url: onboardingAPI + '/' + update.RecordNumber, data: { id: PartID } });
        self.OnboardingParts.remove(update);
    }

    self.ArchivePart = function (PartID) {
        var update = $.grep(self.OnboardingParts(), function (e) { return e.RecordNumber == PartID })[0];
        update.Archived = true;
        self.OnboardingParts.remove(update);
        self.ArchivedParts.push(update);
        var updatePart = $.ajax({ type: "PUT", cache: false, url: onboardingAPI + '/' + update.RecordNumber, data: update });
        updatePart.done(function (data) {

        });
    }

    self.FilterParts = function (Customer, Project, PartType, PartClass, PartNum) {
        var parts = self.UnfilteredParts();
        if (Customer != null && Customer != '') {
            parts = $.grep(parts, function (e) { return e.Customer == Customer });
        }

        if (Project != null && Project != '') {
            parts = $.grep(parts, function (e) { return e.PackageName == Project });
        }

        if (PartType != null && PartType != '') {
            parts = $.grep(parts, function (e) { return e.PartType == PartType });
        }

        if (PartClass != null && PartClass != '') {
            parts = $.grep(parts, function (e) { return e.OnBoardClass == PartClass });
        }

        if (PartNum != null && PartNum != '') {
            parts = $.grep(parts, function (e) { return e.PartNumber == PartNum });
        }


        self.OnboardingParts(parts);
    }

    self.ClearFilters = function () {
        self.OnboardingParts(self.UnfilteredParts())
    }

    self.LoadAvailJobs = function (PartID) {
        self.AvailJobs([]);
        var load = $.ajax({ type: "GET", url: onboardingAPI + '/' + PartID + '/AvailJobs', cache: false });
        load.done(function (data) {
            self.AvailJobs(data);
        });
    }

    self.AssignJob = function (PartID, Job) {
        console.log(PartID + ' ' + Job);

        var update = $.grep(self.OnboardingParts(), function (e) { return e.RecordNumber == PartID })[0];
        update.FirstArticleJobNumber = Job;

        var updatePart = $.ajax({ type: "PUT", cache: false, url: onboardingAPI + '/' + update.RecordNumber, data: update });
        updatePart.done(function (data) {

        });
    }

    self.checkStatus = function (ii, startDate, rangeDate) {

        if (ii.PercentComplete == 100) {
            if (ii.CompletedOn > ii.DueBy) {
                return 'Completed Late';
            }
            else {
                return 'Complete';
            }
        }
        else if (ii.PercentComplete > 0 && ii.PercentComplete < 100) {
            return 'In Progress';
        }
        else if (ii.PercentComplete == 0) {

            if (ii.DueBy > rangeDate) {
                return 'Future';
            }
            else if (ii.DueBy >= startDate && ii.DueBy < rangeDate) {
                return 'Upcoming';
            }
            else if (ii.DueBy < startDate) {
                return 'Late';
            }
        }

    }

    //self.LoadOnboardingParts = function () {
    //       var UserID = $('#layoutUserID').val();
    //       self.isLoading(true)
    //       var parts = [];
    //       var tasks = [];
    //       var outer = 0;
    //       var inner = 0;
    //       self.OnboardingTasks([]);

    //           var load = $.ajax({ type: "GET", cache: false, url: onboardingAPI });
    //           load.success(function (data) {
    //               self.OnboardingParts(data);
    //               outerStart = data.length;
    //               data.forEach(function (i) {
    //                   i.DateEntered = formatSqlDateTimeToFullDate(i.DateEntered);
    //                   i.DatePODue = formatSqlDateTimeToFullDate(i.DatePODue);

    //                   outer += 1;
    //                   var loadTasks = $.ajax({ type: "GET", url: onboardingAPI + '/' + i.RecordNumber + '/Tasks', cache: false });
    //                   loadTasks.done(function (data1) {
    //                       innerStart = data1.length;
    //                       data1.forEach(function (ii) {
    //                           ii.DueBy = formatSqlDateTimeToFullDate(ii.DueBy);
    //                           ii.PercentComplete = formatPercent(ii.PercentComplete, 1);
    //                       });
    //                       if (data1[10].TaskDescription != 'FAI Populated') {
    //                           data1.splice(10, 0, { TaskDescription: "FAI Populated", PercentComplete: '0.0%', DueBy: 0, CurrentStatus: '', ActualCompletionHours: 0, StandardCompletionHours: 0 })
    //                       }
    //                       self.OnboardingTasks.push(data1);

    //                       inner += 1;
    //                       if (inner == outerStart) {
    //                           self.isLoading(false)
    //                       }
    //                   });
    //               });
    //               self.OnboardingParts(data);
    //           });
    //           load.fail(function () {
    //               console.log('test');
    //           });
    //       }


    self.loadComments = function () {
        var TypeID = self.SelectedTask().RecordNumber;
        console.log(TypeID);
        loadComments(TypeID, 'OnboardingTasks', CommentsAPI, self)
    };

    self.addComment = function () {
        var TypeID = self.SelectedTask().RecordNumber;
        var UserID = $('#layoutUserID').val();
        var Comment = $("#txt-new-comment").val();
        if (Comment.length > 0) {
            addComment(TypeID, UserID, Comment, "OnboardingTasks", CommentsAPI, self);
            $("#txt-new-comment").val('');

        }

    }
    self.updateComment = function (ID, TimeSubmitted, Comment) {
        updateComment(ID, TimeSubmitted, Comment, self.Comments(), CommentsAPI);
    }
    self.removeComment = function (ID) {
        removeComment(ID, CommentsAPI, self);
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