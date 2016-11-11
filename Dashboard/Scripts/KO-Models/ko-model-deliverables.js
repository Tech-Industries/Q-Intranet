function DeliverablesViewModel() {
    var self = this;
    var usersAPI = $("#usersLink").attr('href');
    var groupsAPI = $("#groupsLink").attr('href');
    var userGroupsAPI = $("#userGroupsLink").attr('href');
    var deliverablesAPI = $("#deliverablesLink").attr('href');
    var ftpAPI = $("#FTPLink").attr('href');
    var deliverableReviewersAPI = $("#deliverableReviewersLink").attr('href');
    var deliverableReviewsAPI = $("#deliverableReviewsLink").attr('href');
    var deliverableDetailAPI = $("#deliverableDetailLink").attr('href');
    var deliverableDocumentsAPI = $("#deliverableDocumentsLink").attr('href');

    var CommentsAPI = $("#CommentsLink").attr('href');



    self.Users = ko.observableArray([]);
    self.WeeklyDels = ko.observableArray([]);
    self.BiWeeklyDels = ko.observableArray([]);
    self.MonthlyDels = ko.observableArray([]);
    self.QuarterlyDels = ko.observableArray([]);
    self.SemiAnnualDels = ko.observableArray([]);
    self.AnnualDels = ko.observableArray([]);
    self.DelUserID = ko.observableArray([]);
    self.DelOwnerID = ko.observableArray([]);
    //DD stands for Deliverable Detail
    self.DDName = ko.observable();
    self.DDDesc = ko.observable();
    self.DDDateDue = ko.observable();
    self.DDDateCompleted = ko.observable();

    self.SelectedDD = ko.observableArray([]);

    self.DelDetID = ko.observable();
    self.DDComplete = ko.observable(false);

    self.DelDetail = ko.observableArray([]);

    self.Comments = ko.observableArray([]);
    self.DDDocuments = ko.observableArray([]);
    self.DDReviewers = ko.observableArray([]);
    self.Owner = ko.observable();
    self.Reviewables = ko.observableArray([]);
    self.UserAccess = ko.observable(false);

    self.CompleteDels = ko.observableArray([]);
    self.IncompleteDels = ko.observableArray([]);
    self.PassedDueComplete = ko.observableArray([]);
    self.PassedDueIncomplete = ko.observableArray([]);
    self.DelStatusChartCats = ko.observableArray([]);

    self.DelsPastDue = ko.observableArray([]);
    self.DelsUpcoming = ko.observableArray([]);

    self.Test = ko.observableArray([]);

    self.isLoading = ko.observable(false);



    self.loadUsers = function () {
        var load = $.ajax({ type: "GET", url: usersAPI, cache: false, data: { ID: 2, type: 'authorized' } });
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

    self.addDeliverable = function () {
        if ($('#Name').val().length > 0 && $('#Description').val().length > 0 && $('#Frequency').val().length > 0 && $('#SelUserID').val().length > 0 && $('#FirstDueDate').val().length > 0) {
            $('#submit-new-deliverable').prop("disabled", true);
            var form = $('#frm-new-del');
            var newDelID;
            var add = $.ajax({ type: 'POST', url: deliverablesAPI, data: form.serialize() });
            add.done(function (data) {
                self.loadUserDeliverables();

                newDelID = data.ID;
                var reviewers = $('#Reviewers').val();
                reviewers.forEach(function (item) {
                    var sendData = "DelID=" + newDelID + "&UserID=" + item;
                    var addReviewer = $.ajax({ type: 'POST', url: deliverableReviewersAPI, data: sendData });
                    addReviewer.done(function (data) { });

                    $('#Name').val('');
                    $('#Description').val('');
                    $('#Frequency').val('');
                    $('#SelUserID').val('');
                    $('#FirstDueDate').val('');
                    $('#Reviewers').select2("val", "");
                    $('#Name').focus();
                });
                $('#submit-new-deliverable').prop("disabled", false);
                swal({ title: "Deliverable Added", type: "success", timer: 1500 })
            });






        }
    }

    self.DateOverride = function (date) {

        console.log(vm.SelectedDD());
        self.SelectedDD().DateCompleted = formatShortDate(date) + 'T00:00:00';
        var sendData = "ID=" + self.SelectedDD().ID+ "&DelID="+self.SelectedDD().DelID+"&DateDue="+self.SelectedDD().DateDue+"&DateCompleted=" + formatShortDate(date);
        console.log(vm.SelectedDD());
        self.DDDateCompleted(formatShortDate(date));
        var load = $.ajax({ type: "GET", url: deliverablesAPI + '/detail/' + self.SelectedDD().ID, cache: false, data: { id: self.SelectedDD().ID} });
        load.done(function (data) {
            data.DateCompleted = formatShortDate(date);
            var update = $.ajax({ type: "PUT", url: deliverablesAPI + '/detail/' + self.SelectedDD().ID, cache: false, data: data  });
            update.done(function (updata) {
                console.log(updata);
                vm.DDDateCompleted(formatSqlDateTime(updata.DateCompleted));
                self.loadDelStatusChart();
            });
            
        });
    }

    self.addDelReviewer = function (UserID) {
        var Name = $('#sel-add-reviewer option:selected').text().split(' ');
        var FirstName = Name[0];
        var LastName = Name[1];
        var item = { 'UserID': parseInt(UserID), 'FirstName': FirstName, 'LastName': LastName, 'ID': 0, 'TimeReviewed': '' }
        self.DDReviewers.push(item);
        var DelID = $('#DelID').val();
        var sendData = "DelID=" + DelID + "&UserID=" + UserID;
        var addReviewer = $.ajax({ type: 'POST', url: deliverableReviewersAPI, data: sendData });
        addReviewer.done(function (data) { });
        $('#sel-add-reviewer').val("0");
    }

    self.updateDelDet = function (comp) {
        var update = ""
        console.log(comp);
        update = $.ajax({ type: "PUT", url: deliverableDetailAPI, cache: false, data: { ID: self.SelectedDD().ID, deldet: self.SelectedDD() } });

        update.done(function (data) {

            self.loadDelDet();
            self.LoadPastDueAndUpcoming();
            self.loadDelStatusChart();
        });
    }


    self.loadUserDeliverables = function () {

        var UserID = $('#Assignee').val();
        self.DelUserID(parseInt(UserID));

        self.WeeklyDels([]);
        self.BiWeeklyDels([]);
        self.MonthlyDels([]);
        self.QuarterlyDels([]);
        self.SemiAnnualDels([]);
        self.AnnualDels([]);

        var load = $.ajax({ type: "GET", url: deliverablesAPI, cache: false, data: { UserID: UserID } });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Name: item.Name,
                    Description: item.Description,
                    Frequency: item.Frequency,
                    UserID: item.UserID,
                    FirstDueDate: item.FirstDueDate
                }
            });
            array.forEach(function (item) {
                if (item.Frequency == "Weekly") {
                    self.WeeklyDels.push(item);
                }
                else if (item.Frequency == "BiWeekly") {
                    self.BiWeeklyDels.push(item);
                }
                else if (item.Frequency == "Monthly") {
                    self.MonthlyDels.push(item);
                }
                else if (item.Frequency == "Quarterly") {
                    self.QuarterlyDels.push(item);
                }
                else if (item.Frequency == "Semi-Annual") {
                    self.SemiAnnualDels.push(item);
                }
                else if (item.Frequency == "Annual") {
                    self.AnnualDels.push(item);
                }
            });

            $(document).find('.deliverable-type').first().click();

            self.LoadPastDueAndUpcoming();
            vm.loadDelStatusChart();
            self.loadReviewables();
        });
    }

    self.LoadPastDueAndUpcoming = function () {

        var UserID = $('#Assignee').val();

        var load0 = $.ajax({ type: "GET", url: deliverablesAPI, cache: false, data: { ID: UserID, DataPull: "DelPastDue" } });
        load0.done(function (data) {
            var array0 = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    DelID: item.DelID,
                    DelDetID: item.DelDetID,
                    Name: item.Name,
                    UserID: item.UserID,
                    DateDue: item.DateDue.split('T')[0],
                    DaysPast: item.DaysPast,
                    Frequency: item.Frequency
                }
            });
            self.DelsPastDue(array0);
        });

        var load1 = $.ajax({ type: "GET", url: deliverablesAPI, cache: false, data: { ID: UserID, DataPull: "DelUpcoming" } });
        load1.done(function (data) {
            var array1 = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    DelID: item.DelID,
                    DelDetID: item.DelDetID,
                    Name: item.Name,
                    UserID: item.UserID,
                    DateDue: item.DateDue.split('T')[0],
                    DaysPast: item.DaysPast,
                    Frequency: item.Frequency
                }
            });
            self.DelsUpcoming(array1);
        });


    }

    self.loadDeliverableByID = function (ID) {

        self.UserAccess(false);
        var load = $.ajax({ type: "GET", url: deliverablesAPI, cache: false, data: { ID: ID, DataPull: "DelDet" } });
        load.done(function (data) {
            data = data[0];
            $('#DelID').val(data.DelID);
            var dateDue = data.DateDue;
            dateDue = dateDue.split('T')[0];
            self.DDDateDue(dateDue);
            self.DelDetID(data.ID);

            self.Owner(data.UserID);
            if (data.DateCompleted == null) {
                self.DDComplete(false);
            }
            else {
                self.DDDateCompleted(formatSqlDateTime(data.DateCompleted));
                self.DDComplete(true);
            }

            var userid = $('#UserID').val();
            self.loadDelDetReviews();
            if ($('#RelPer').val() == 3) { self.UserAccess(true); }
            self.loadComments();
            self.SelectedDD(data);
            self.loadDelDetDocuments();
            $('#del-title').html(data.Name);
            $('#del-title').attr('ownerid', data.UserID);
            self.DelOwnerID(data.UserID);
            $('#del-desc').html(data.Description);
            $('.full-height-scroll').slimscroll({
                height: '100%'
            })
        });
    };

    self.loadDelDet = function () {
        self.UserAccess(false);
        var DelID = $("#DelID").val();
        var Period = $("#period").val();
        var Frequency = $("#freq").val();
        var Year = $("#year").val();
        self.DDDateCompleted("");
        if (Period != 0 || Frequency == "Annual") {
            var load = $.ajax({ type: "GET", url: deliverablesAPI, cache: false, data: { Frequency: Frequency, DelID: DelID, Period: Period, Year: Year } });
            load.done(function (data) {
                console.log(data);
                var dateDue = data.DateDue;
                dateDue = dateDue.split('T')[0];
                self.DDDateDue(dateDue);
                self.DelDetID(data.ID);
                self.Owner(data.UserID);
                $('#del-title').attr('ownerid', data.UserID);
                self.DelOwnerID(data.UserID);
                if (data.DateCompleted == null) {
                    self.DDComplete(false);
                }
                else {
                    self.DDDateCompleted(formatSqlDateTime(data.DateCompleted));
                    self.DDComplete(true);
                }
                self.loadDelDetReviews();
                var userid = $('#UserID').val();
                if ($('#RelPer').val() == 3) { self.UserAccess(true); }
                self.loadComments();
                self.SelectedDD(data);
                self.loadDelDetDocuments();
                $('.full-height-scroll').slimscroll({
                    height: '100%'
                })
            });
            load.fail(function (data) {
                self.DDDateDue('');
            });
        }
    }

    self.loadDelDetReviews = function () {
        var DelDetID = self.DelDetID();
        var DelID = $("#DelID").val();
        var loadReqReviewed = $.ajax({ type: "GET", url: deliverableReviewersAPI, cache: false, data: { DelID: DelID } });
        var exists = false;
        var arr0 = [];
        var arr1 = [];
        var IDSCheck = [];
        loadReqReviewed.done(function (data) {
            arr0 = data;
            var loadHasReviewed = $.ajax({ type: "GET", url: deliverableReviewsAPI, cache: false, data: { DelDetID: DelDetID } });
            loadHasReviewed.done(function (data) {
                arr1 = data;
                arr0.forEach(function (i) {
                    arr1.forEach(function (h) {
                        if (i.UserID == h.UserID) {
                            if (h.TimeReviewed != '') {
                                h.TimeReviewed = formatSqlDateTime(h.TimeReviewed);
                            }
                            exists = true;
                        }
                    });
                    if (exists == false && self.DDDateDue() >= i.DateAdded.split('T')[0]) {
                        var addRev = { ID: i.ID, UserID: i.UserID, FirstName: i.FirstName, LastName: i.LastName, TimeReviewed: "", DateAdded: i.DateAdded.split('T')[0] }
                        arr1.push(addRev);
                    }
                    exists = false;
                });
                self.DDReviewers(arr1);
                arr1.forEach(function (r) {
                    IDSCheck.push(r.UserID);
                });
                IDSCheck.push(parseInt($('#Assignee option:selected').val()));
                var UserID = parseInt($('#layoutUserID').val());
                if ($.inArray(UserID, IDSCheck) > -1) {
                    self.UserAccess(true);

                }
            });
        });
    }

    self.addDelDetReview = function (UserID) {
        var DelDetID = self.DelDetID();
        var TimeReviewed = DateNowFormatted();

        var DDReview = "DelDetID: " + DelDetID + ", UserID: " + UserID + ", TimeReviewed: " + TimeReviewed;
        var sendData = "DelDetID=" + DelDetID + "&UserID=" + UserID + "&TimeReviewed=" + TimeReviewed;
        var addReview = $.ajax({ type: 'POST', url: deliverableReviewsAPI, data: sendData });
        addReview.done(function (item) {
            return TimeReviewed;
        });
        self.loadDelDetReviews();
    }


    self.uploadFile = function () {
        var formData = new FormData($('#doc-upload')[0]);
        console.log(formData);
        $.ajax({
            url: '/Deliverables/UploadFile',
            type: 'Post',
            beforeSend: function () { },
            success: function (result) { },
            error: function (result) { },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    }

    //self.uploadFile = function () {
    //    var files = $('#doc-upload')[0].files[0];
    //    console.log(files);

    //    if (files.length > 0) {
    //        var data = new FormData();
    //        for (var x = 0; x < files.length; x++) {
    //            data.append("file" + x, files[x]);
    //        }
    //        var upload = $.ajax({ type: "POST", url: deliverableDocumentsAPI, cache: false, data: data });
    //        upload.done(function () {
    //            console.log('complete');
    //        });
    //        upload.fail(function () {
    //            console.log('failed');
    //        });
    //    }
    //}

    self.loadDelDetDocuments = function () {
        self.DDDocuments([]);
        var DelDetID = self.DelDetID();
        var load = $.ajax({ type: "GET", url: deliverablesAPI, cache: false, data: { ID: DelDetID, DataPull: "Documents" } });
        load.done(function (data) {
            data.forEach(function (item) {
                item.TimeSubmitted = formatSqlDateTime(item.TimeSubmitted);
            });
            self.DDDocuments(data);
        });
    };



    self.loadReviewables = function () {
        self.Reviewables([]);
        var UserID = $("#Assignee option:selected").val();

        var load = $.ajax({ type: "GET", url: deliverablesAPI, cache: false, data: { ID: UserID, DataPull: "Reviewables" } });
        load.done(function (data) {
            console.log(data);
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Name: item.Name,
                    Description: item.Description,
                    Owner: item.Owner,
                    Frequency: item.Frequency,
                    DelDetID: item.DelDetID,
                    DateDue: item.DateDue.split("T")[0],
                    DateCompleted: item.DateCompleted.split("T")[0]
                }
            });
            console.log(array);
            self.Reviewables(array);

        });
    }



    self.removeDocument = function (id, path) {
        var del = $.ajax({ type: 'DELETE', url: deliverableDocumentsAPI + '/' + id, contentType: 'application/json' });
        del.done(function (data) {
            var remove = $.grep(self.DDDocuments(), function (e) { return e.ID == id })[0];
            self.DDDocuments.remove(remove);
        });
        //var delFile = $.ajax({ type: 'GET', url: ftpAPI, cache: false, data: { path: path, test: 'test' } });
        //delFile.done(function () { console.log("File Deleted") });
    }

    self.removeReviewer = function (id) {

        var del = $.ajax({ type: 'DELETE', url: deliverableReviewersAPI + '/' + id, contentType: 'application/json' });
        del.done(function (data) {
            console.log(data);
            var remove = $.grep(self.DDReviewers(), function (e) { return e.ID == id })[0];
            self.DDReviewers.remove(remove);
        });
    }

    self.addDelDetDocument = function () {
        var formData = new FormData($('#frm-doc-up')[0]);
        //console.log(formData.toString());
        var uploadLink = $('#UploadFileLink').attr('href');
        for (var i = 0, len = document.getElementById('doc-upload').files.length; i < len; i++) {
            formData.append("doc-upload" + (i + 1), document.getElementById('doc-upload').files[i]);
        }
        console.log('test1');
        $.ajax({
            url: uploadLink + '/', //Html.Action("Deliverables", "UploadFile"),
            type: 'Post',
            beforeSend: function () { },
            success: function (result) {
                console.log('test2');
                console.log(result);
                var ftpAPI = $("#FTPLink").attr('href');

                var user = $('#Assignee option:selected').text();
                var freq = $('#freq').val();
                var delNam = $('#del-title').text();
                var dueDate = $('#del-due-date').html();
                var n = result.lastIndexOf('\\');
                var fileName = result.substring(n + 1);
                if (fileName == 'DeliverablesController.cs:line 81') {
                    console.log('test')
                }
                else {
                    console.log('test3');
                    var remoteFile = "/Deliverables/" + user + "/" + freq + "/" + delNam + "/" + dueDate + "/" + fileName;
                    remoteFile = remoteFile.replace(" ", "%20");
                    var data = "remoteFile=" + remoteFile + "&localFile=" + result;
                    $.ajax({
                        method: 'POST', url: ftpAPI, data: data, success: function (res) {
                            var delDetID = $('#DelDetID').val();
                            var title = fileName;
                            var type = "temp";
                            var userID = $('#UserID').val();
                            var loc = "Temp";
                            var path = remoteFile;
                            var time = DateNowFormatted();
                            var sendData = "DelDetID=" + delDetID + "&Title=" + title + "&Type=" + type + "&UserID=" + userID + "&Location=" + loc + "&Path=" + path + "&TimeSubmitted=" + time;
                            var deliverableDocumentsAPI = $("#deliverableDocumentsLink").attr('href');
                            console.log(sendData);
                            $.ajax({
                                method: 'POST',
                                url: deliverableDocumentsAPI,
                                cache: false,
                                data: sendData,
                                success: function () { self.loadDelDetDocuments(); console.log('test4');}
                            });
                        }
                    });
                }
            },
            error: function (result) { },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    }

    self.loadComments = function () {
        var TypeID = self.DelDetID();
        loadComments(TypeID, 'DeliverableDetail', CommentsAPI, self)
    };

    self.addComment = function () {
        var TypeID = self.DelDetID();
        var UserID = $("#UserID").val();
        var Comment = $("#txt-new-comment").val();
        if (Comment.length > 0) {
            addComment(TypeID, UserID, Comment, "DeliverableDetail", CommentsAPI, self);
            $("#txt-new-comment").val('');

        }

    }
    self.updateComment = function (ID, TimeSubmitted, Comment) {
        updateComment(ID, TimeSubmitted, Comment, self.Comments(), CommentsAPI);
    }
    self.removeComment = function (ID) {
        removeComment(ID, CommentsAPI, self);
    }

    self.loadDelStatusChart = function () {

        //self.CompleteDels([]);
        //self.IncompleteDels([]);
        //self.DelStatusChartCats([]);
        var UserID = $("#Assignee option:selected").val();
        var load = $.ajax({ type: "GET", url: deliverablesAPI, cache: false, data: { ID: UserID, DataPull: "DelStatus" } });
        load.done(function (data) {
            var cArray = [];
            var iArray = [];
            var pcArray = [];
            var piArray = [];
            var DelStatusChartCategories = [];

            if (data.length > 0) {
                var array = $.map(data, function (item) {
                    return {
                        ID: item.ID,
                        Month: MonthString(item.Month, 'short'),
                        Year: item.Year,
                        CompleteOnTime: item.CompleteOnTime,
                        Incomplete: item.Incomplete,
                        PassedDueComplete: item.PassedDueComplete,
                        PassedDueIncomplete: item.PassedDueIncomplete

                    }
                });
                for (i = 0; i < array.length; i++) {
                    cArray.push(array[i].CompleteOnTime);
                    iArray.push(array[i].Incomplete);
                    pcArray.push(array[i].PassedDueComplete);
                    piArray.push(array[i].PassedDueIncomplete);
                    DelStatusChartCategories.push(array[i].Month);
                }
            }
            else {
                cArray = [null, null, null];
                iArray = [null, null, null];
                pcArray = [null, null, null];
                piArray = [null, null, null];
            }
            self.CompleteDels(cArray);
            self.IncompleteDels(iArray);
            self.PassedDueComplete(pcArray);
            self.PassedDueIncomplete(piArray);
            self.DelStatusChartCats(DelStatusChartCategories);

        });
    }

    self.loadDirectReportMetrics = function (callback) {
        $('#report-metrics-body').html('');
        var UserID = $("#Assignee option:selected").val();
        var load = $.ajax({ type: "GET", url: usersAPI, cache: false, data: { ID: UserID, type: "directReports" } });
        load.done(function (data) {
            loadMetrics(UserID, $("#Assignee option:selected").text());
            data.forEach(function (item) {
                loadMetrics(item.ID, item.FirstName + " " + item.LastName);
            });

        });

        function loadMetrics(ID, name) {
            var load = $.ajax({ type: "GET", url: deliverablesAPI, cache: false, data: { ID: ID, DataPull: "DelStatus" } });
            load.done(function (data2) {
                console.log(data2);
                if (data2.length > 3) {
                    $('#report-metrics-body').append("<div class='col-sm-12 col-md-6 col-lg-4' style='height: 250px' id='status-" + ID + "'></div>");
                    $("#status-" + ID).highcharts({
                        chart: {
                            type: 'column',
                            options3d: {
                                enabled: true,
                                alpha: 7,
                                beta: 20,
                                depth: 70
                            }
                        },
                        title: {
                            text: ''
                        },
                        subtitle: {
                            text: 'Deliverables Status for <strong>' + name + "</strong>",
                            useHtml: 'true'
                        },
                        xAxis: {
                            type: 'category',
                            categories: self.DelStatusChartCats()
                        },
                        yAxis: {
                            min: 0,
                            max: 100,
                            stackLabels: {
                                enabled: false,
                                style: {
                                    fontWeight: 'bold',
                                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                                }
                            },

                            title: {
                                text: ''
                            }

                        },
                        legend: {
                            enabled: true,
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'middle'
                        },
                        plotOptions: {
                            series: {
                                borderWidth: 0,
                                dataLabels: {
                                    enabled: false,
                                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                                    style: {
                                        textShadow: '0 0 3px black'
                                    }
                                }
                            },
                            column: {
                                stacking: 'percent'
                            }
                        },

                        tooltip: {
                            headerFormat: '<span style="font-size:11px"># of Complete Deliverables</span><br>',
                            pointFormat: '{series.name}: {point.y}<br />',
                            shared: true
                        },

                        series: [
                             {
                                 name: 'Incomplete',
                                 data: [data2[0].Incomplete, data2[1].Incomplete, data2[2].Incomplete, data2[3].Incomplete],
                                 color: 'rgba(149, 165, 166, 0.3)'
                             },
                             {
                                 name: 'Late&Incompl',
                                 data: [data2[0].PassedDueIncomplete, data2[1].PassedDueIncomplete, data2[2].PassedDueIncomplete, data2[3].PassedDueIncomplete],
                                 color: 'rgba(192, 57, 43, 0.3)',
                                 minPointLength: 0,
                             },
                             {
                                 name: 'Late&Compl',
                                 data: [data2[0].PassedDueComplete, data2[1].PassedDueComplete, data2[2].PassedDueComplete, data2[3].PassedDueComplete],
                                 color: '#c0392b',
                                 minPointLength: 0,
                             },
                             {
                                 name: 'Complete',
                                 data: [data2[0].CompleteOnTime, data2[1].CompleteOnTime, data2[2].CompleteOnTime, data2[3].CompleteOnTime],
                                 color: '#344983'
                             }]
                    }).highcharts();
                }

            });
        }
    }
}

//ko.bindingHandlers.chosen = {
//    init: function (element) {
//        ko.bindingHandlers.options.init(element);
//        $(element).chosen({ disable_search_threshold: 10 });
//    },
//    update: function (element, valueAccessor, allBindings) {
//        ko.bindingHandlers.options.update(element, valueAccessor, allBindings);
//        $(element).trigger('chosen:updated');
//    }
//};