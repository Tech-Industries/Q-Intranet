function UltraViewModel() {
    var self = this;
    var usersAPI = $("#usersLink").attr('href');
    var UCFCategoriesAPI = $("#UCFCategoriesLink").attr('href');
    var UCFChallengesAPI = $("#UCFChallengesLink").attr('href');
    var UCFCriteriaAPI = $("#UCFCriteriaLink").attr('href');
    var UCFAreasAPI = $("#UCFAreasLink").attr('href');
    var UCFAuditsAPI = $("#UCFAuditsLink").attr('href');
    var UCFAuditDetailAPI = $("#UCFAuditDetailLink").attr('href');
    var UCFGoalsAPI = $("#UCFGoalsLink").attr('href');
    var UCFActionsAPI = $("#UCFActionsLink").attr('href');
    var PlantsAPI = $("#PlantsLink").attr('href');


    self.Plants = ko.observableArray([]);
    self.Areas = ko.observableArray([]);
    self.UCFCategories = ko.observableArray([]);
    self.UCFChallenges = ko.observableArray([]);
    self.UCFCriteria = ko.observableArray([]);

    self.Score = ko.observable(0);

    self.Users = ko.observableArray([]);

    self.OwnerID = ko.observable();



    //Metrics
    self.Goals = ko.observableArray([]);
    self.AreaGoals = ko.observableArray([]);
    self.PlantTrendCats = ko.observableArray([]);
    self.PlantScores = ko.observableArray([]);

    self.AreaScores = ko.observableArray([]);

    self.Actions = ko.observableArray([]);
    self.ActionsHistory = ko.observableArray([]);

    self.PreviousScores = ko.observableArray([]);

    self.AuditHistory = ko.observableArray([]);
    self.LastAudit = ko.observable();
    var test = 0;

    self.checkOwnerID = function () {
        self.OwnerID($('#areaSelect option:selected').attr('ownerid'));
    }

    self.addAction = function () {
        if ($('#Description').val().length > 0 && $('#Owner').val().length > 0 && $('#DateStart').val().length > 0 && $('#DateDue').val().length > 0) {

            var AreaID = $('#areaSelect').val();
            var Description = $('#Description').val();
            var Owner = $('#Owner').val();
            var DateCreated = GetDate();
            var DateStart = formatShortDate($('#DateStart').val());
            var DateDue = formatShortDate($('#DateDue').val());
            var Status = 'Open';
            var OwnerID = $('#areaSelect option:selected').attr('ownerid');

            var sendData = "AreaID=" + AreaID + "&Owner=" + Owner + "&DateCreated=" + DateCreated + "&DateStart=" + DateStart + "&DateDue=" + DateDue + "&DateComplete=null" + "&Status=" + Status + "&Description=" + Description + "&OwnerID=" + OwnerID;

            var add = $.ajax({ type: "POST", cache: false, url: UCFActionsAPI, data: sendData });
            add.success(function (data) {
                swal("Action Submitted", "This action has be succesfully submit.", "success");
                $('#Description').val('');
                $('#Owner').val('');
                $('#DateStart').val('');
                $('#DateDue').val('');
                self.LoadActions();
            });
        }
        else {
            swal("Error!", "Please fill out all criteria.", "error");
        }
    }

    self.removeAction = function (ID) {
        if (parseInt($('#UID').val()) == self.OwnerID() || $('#RelPer').val() == "Admin") {
            var del = $.ajax({ type: 'DELETE', url: UCFActionsAPI + '/' + ID, contentType: 'application/json' });
            del.always(function () {
                var remove = $.grep(self.Actions(), function (e) { return e.ID == ID })[0];
                self.Actions.remove(remove);
            });
        }
    }

    self.updateActionStatus = function (ID, Status, DateComplete) {
        console.log(parseInt($('#UID').val() + ' - ' + self.OwnerID()));
        if (parseInt($('#UID').val()) == self.OwnerID() || $('#RelPer').val() == "Admin") {
            var t = $.grep(self.Actions(), function (e) { return e.ID == ID })[0];
            t.Status = Status;
            t.DateComplete = DateComplete;
            var update = $.ajax({ type: "PUT", url: UCFActionsAPI + "/" + ID, cache: false, data: t });
            update.done(function (data) {
            });
        }
    }

    self.loadTopLevelHistory = function () {
        test += 1;
        console.log(test);
        var AreaID = $('#areaSelect').val();
        var load = $.ajax({ type: "GET", url: UCFAuditsAPI, cache: false, data: { ID: AreaID, Type: 'TopLevelHistory' } });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    AreaID: item.AreaID,
                    AreaName: item.AreaName,
                    AuditID: item.AuditID,
                    Auditor: item.Auditor,
                    DateCompleted: formatSqlDateTime(item.DateCompleted),
                    PlantID: item.PlantID,
                    PlantName: item.PlantName,
                    Score: item.Score
                }
            });
            self.AuditHistory(array);
            if (self.AuditHistory().length > 0) {

                $('.dtUcfHistory').DataTable().clear();


                //insert data 
                $.each(array, function (i, item) {
                    $('.dtUcfHistory').DataTable().row.add([
                      item.AuditID,
                      item.PlantName,
                      item.AreaName,
                      item.Auditor,
                      item.DateCompleted,
                      item.Score,
                    ]).draw();
                });
                $('.dtUcfHistory tbody tr').addClass('audit-selectable');
            }
            else {

                $('.dtUcfHistory').dataTable().fnClearTable();
                //swal(
                //{
                //    title: "No Records Found!",
                //    text: "It looks like there were no sales recorded on " + self.SelectedDay() + "",
                //    showCancelButton: false,
                //    closeOnConfirm: true,
                //    animation: "slide-from-top"
                //},
                //function () { $("#salesDetailModal").modal('hide'); });
            }
            self.LastAudit(formatSqlDateTime(data[0].DateCompleted));
        });
        load.fail(function (data) {
        });
    };

    self.loadSelectedAudit = function (AuditID) {
        test += 1;
        console.log(test);
        var load = $.ajax({ type: "GET", url: UCFAuditsAPI, cache: false, data: { ID: AuditID, Type: 'Detail' } });
        load.done(function (data) {
            var category = '';
            var categories = [];
            var catSpan = [];
            var catCount = 0;

            var challenge = '';
            var challenges = [];
            var challSpan = [];
            var challCount = 0;

            var score = 0;

            for (i = 0; i < data.length; i++) {
                if (data[i].Category != category) {
                    console.log(data[i].Category);
                    categories.push(data[i].Category);
                    category = data[i].Category;
                    if (catCount > 0) {
                        catSpan.push(catCount);
                    }
                    catCount = 0;

                }
                if (data[i].Challenge != challenge) {
                    challenges.push(data[i].Challenge);
                    challenge = data[i].Challenge;

                    if (challCount > 0) {

                        challSpan.push(challCount);
                    }
                    challCount = 0;
                }


                catCount += 1;
                challCount += 1;
                if (data[i].Selected == true) {
                    score += data[i].Score;
                }
            }
            catSpan.push(catCount);
            challSpan.push(challCount);

            $('#ucf-detai-body').html('');
            var challs = 0;
            var prevChalls = 0;
            var n = 0;

            var ii = 0;
            var iii = 0;
            var old = 0;

            $('#ucf-detai-body').append("<div class='row'><div class='col-md-4'><h3>Date Completed: " + formatSqlDateTime(data[0].DateCompleted) + "</h3></div><div class='col-md-4'><h3>Plant: " + data[0].PlantName + ' - ' + data[0].AreaName + "</h3></div><div class='col-md-2'><h3>Auditor: " + data[0].Auditor + "</h3></div><div class='col-md-2'><h3>Score: " + score + "</h3></div></div>")

            for (i = 0; i < categories.length; i++) {
                $('#ucf-detai-body').append('<div class="row"><div class="col-md-8 col-md-offset-2 col-sm-8 col-sm-offset-2" id="category-' + i + '"><h1 class="">' + categories[i] + '</h1></div></div>')
                challs += catSpan[i];
                console.log('-------Start---------');
                while (prevChalls != challs) {
                    prevChalls += challSpan[ii];
                    $('#category-' + i).append('<table class="table table-condensed table-striped table-bordered table-hover dtUcfDetail"><tbody id="challenge-' + ii + '"> <tr><td  class="" rowspan="' + (challSpan[ii] + 1) + '" style="width:50%"><h3>' + challenges[ii] + '</h3></td></tr></tbody></table>');
                    n = prevChalls;
                    for (old; old < prevChalls; old++) {
                        console.log(old + ' / ' + prevChalls + ' - ' + ii);
                        var fa = '';
                        if (data[old].Selected == true) {
                            fa = 'fa-check';
                        }
                        $('#challenge-' + ii).append('<tr><td style="width:40%">' + data[old].Criteria + '</td><td style="width:5%">' + data[old].Score + '</td><td style="width:5%"><i class="fa ' + fa + '"></i></td></tr>');
                        iii++;
                    }

                    ii++
                }
                console.log('-------End---------');
                $('#ucf-detai-body').append('');
            }
        });
    };

    self.loadPreviousScores = function () {
        test += 1;
        console.log(test);

        var AreaID = $('#Area').val();
        var load = $.ajax({ type: "GET", url: UCFAuditDetailAPI, cache: false, data: { ID: AreaID, Type: 'PreviousScores' } });
        load.done(function (data) {
            self.PreviousScores(data);
            for (i = 0; i < data.length; i++) {
                if (data[i].Selected == false) { data[i].Score = 0; }

                $('#prev-score-' + data[i].CriteriaID).html(data[i].Score);
            }
        });
        load.fail(function (data) {
            $('.prev-score').html('');
        });
    };

    self.loadSelectedAudits = function (month) {
        test += 1;
        console.log(test);
        var m = getMonthNum(month);
        console.log(m);
    };

    self.loadUsers = function () {
        test += 1;
        console.log(test);
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

    self.loadPlants = function () {
        var load = $.ajax({ type: "GET", cache: false, url: PlantsAPI });
        load.success(function (data) {
            self.Plants(data);
        });

    }

    self.loadAreas = function (ID) {
        test += 1;
        console.log(test);
        self.Areas([]);

        var load = $.ajax({ type: "GET", cache: false, url: UCFAreasAPI, data: { ID: ID } });
        load.success(function (data) {
            self.Areas(data);
            $('#areaSelect').change();
        });
    }

    self.addArea = function (PlantID, Name) {
        var sendData = "PlantID=" + PlantID + "&Name=" + Name;
        var pushData = { PlantID: PlantID, Name: Name };
        var load = $.ajax({ type: "POST", cache: false, url: UCFAreasAPI, data: sendData });
        load.success(function (data) {
            self.loadAreas(PlantID);
        });
    }

    self.LoadCategories = function () {
        test += 1;
        console.log(test);
        var Area = $('#Area option:selected').text();
        var load = $.ajax({ type: "GET", cache: false, url: UCFCategoriesAPI, data: { Area: Area } });
        load.success(function (data) {
            self.UCFCategories(data);
        });
    }
    self.LoadChallenges = function (ID) {
        test += 1;
        console.log(test);
        var load = $.ajax({ type: "GET", url: UCFChallengesAPI, cache: false, data: { ID: ID } })
        load.success(function (data2) {
            self.UCFChallenges(data2);
        });
    }

    self.LoadCriteria = function (ID) {
        test += 1;
        console.log(test);
        var load = $.ajax({ type: "GET", url: UCFCriteriaAPI, cache: false, data: { ID: ID } })
        load.success(function (data2) {
            self.UCFCriteria(data2);
        });
    }

    self.UpdateScore = function (action, amt) {
        if (action == 'reduce') {
            self.Score(self.Score() - amt);
        }
        else {
            self.Score(self.Score() + amt);
        }

    }

    self.LoadActions = function () {
        test += 1;
        console.log(test);

        var ID = $('#areaSelect').val();
        console.log(ID);
        if (ID != null && ID != '') {
            var load = $.ajax({ type: "GET", url: UCFActionsAPI, cache: false, data: { ID: ID } });
            load.done(function (data) {
                for (i = 0; i < data.length; i++) {
                    data[i].DateDue = data[i].DateDue.split('T')[0];
                }
                self.Actions(data);

                vm.checkOwnerID();
            });
        }
    }

    self.LoadActionsHistory = function () {

        var ID = $('#areaSelect').val();
        console.log(ID);
        if (ID != null && ID != '') {
            var load = $.ajax({ type: "GET", url: UCFActionsAPI, cache: false, data: { ID: ID, All: true } });
            load.done(function (data) {
                for (i = 0; i < data.length; i++) {
                    data[i].DateDue = data[i].DateDue.split('T')[0];
                    if (data[i].DateComplete != null) {
                        data[i].DateComplete = data[i].DateComplete.split('T')[0];
                    }
                }
                self.ActionsHistory(data);

                vm.checkOwnerID();
            });
        }
    }

    self.loadPlantRollup = function () {
        test += 1;
        console.log(test);

        var ID = $('#plantSelect option:selected').attr('plantid');
        var Year = $("#periodSelect").val().split('-')[0];
        var Month = $("#periodSelect").val().split('-')[1];
        Range = 5;


        var load = $.ajax({ type: "GET", url: UCFAuditsAPI, cache: false, data: { ID: ID, Year: Year, Month: Month, Range: Range } });
        load.done(function (data) {
            var plantscores = [];

            check = Month - Range;
            if (check <= 0) {
                check += 12;
            }
            incr = 0;
            i = 0;


            while (incr <= Range) {
                if (i < data.length) {
                    if (data[i].Month == check) {
                        plantscores.push(data[i].Score);
                        i++;
                    }
                    else {
                        plantscores.push(null);
                    }
                }

                incr += 1;
                check += 1;
                if (check > 12) {
                    check -= 12;
                }

            }
            while (plantscores.length < Range + 1) {
                plantscores.push(null);
            }

            self.PlantScores(plantscores);

            self.PlantTrendCats(getMonthNamesInRange(Month, Range + 1));


        });

        var loadGoals = $.ajax({ type: "GET", url: UCFGoalsAPI, cache: false, data: { PlantID: ID, Year: Year, Month: Month, Range: Range } });
        loadGoals.done(function (data) {
            var goals = [];
            for (i = 0; i < data.length; i++) {
                goals.push(data[i].Amount);
            }
            self.Goals(goals);
        });
    }

    self.loadAreaTrend = function () {
        test += 1;
        console.log(test);
        var ID = $('#plantSelect option:selected').attr('plantid');
        var Year = $("#periodSelect").val().split('-')[0];
        var Month = $("#periodSelect").val().split('-')[1];
        Range = 5;
        var AreaID = $('#areaSelect option:selected').val();
        if (AreaID != null && AreaID != '') {
            var load = $.ajax({ type: "GET", url: UCFAuditsAPI, cache: false, data: { PlantID: ID, AreaID: AreaID, Year: Year, Month: Month, Range: Range } });
            load.done(function (data) {
                var areascores = [];
                check = Month - Range;
                if (check <= 0) {
                    check += 12;
                }
                incr = 0;
                i = 0;
                while (incr <= Range) {
                    if (i < data.length) {
                        if (data[i].Month == check) {
                            areascores.push(data[i].Score);
                            i++;
                        }
                        else {
                            areascores.push(null);
                        }
                    }
                    incr += 1;
                    check += 1;
                    if (check > 12) {
                        check -= 12;
                    }

                }
                while (areascores.length < Range + 1) {
                    areascores.push(null);
                }

                self.AreaScores(areascores)

                var loadGoals = $.ajax({ type: "GET", url: UCFGoalsAPI, cache: false, data: { PlantID: ID, Year: Year, Month: Month, Range: Range } });
                loadGoals.done(function (data) {
                    var goals = [];
                    for (i = 0; i < data.length; i++) {
                        goals.push(data[i].Amount);
                    }
                    self.AreaGoals(goals);
                });

            });
        }
    }

    self.SubmitAudit = function () {
        var plantid = $('#Plant').val();
        var areaid = $('#Area').val();
        if (plantid == 0 || areaid == 0 || score == 0) {
        }
        else {
            var sendData = "UserID=" + $('#UID').val() + "&DateCompleted=" + DateNowFormatted() + "&AreaID=" + areaid;
            var addAudit = $.ajax({ type: "POST", url: UCFAuditsAPI, cache: false, data: sendData });
            addAudit.done(function (data) {
                AuditID = data.ID;
                $('.scoring').each(function () {
                    thisScore = $(this).text();
                    //thisScore = parseFloat(Math.round(thisScore * 100) / 100).toFixed(2);
                    if ($(this).hasClass('selected')) {
                        detSendData = "AuditID=" + AuditID + "&CriteriaID=" + $(this).attr('criteriaid') + "&Score=" + thisScore + "&Selected=True";

                        $(this).removeClass('selected');
                    }
                    else {
                        detSendData = "AuditID=" + AuditID + "&CriteriaID=" + $(this).attr('criteriaid') + '&Score=' + thisScore + "&Selected=False";
                    }
                    var addAuditDetail = $.ajax({ type: "POST", url: UCFAuditDetailAPI, cache: false, data: detSendData });
                    addAudit.fail(function (res) {
                    });

                    addAuditDetail.done(function () {
                    });

                });
            });
            $('#Plant').val(0);
            $('#Area').val(0);
            score = 0;
            $('#dis-score').html(score);


            swal({
                title: "Submitted!",
                type: "success",
                timer: 1000
            });
        }
    }

    self.LoadAuditForm = function () {
        test += 1;
        console.log(test);
        self.UCFChallenges([]);
        self.UCFCriteria([]);

        var Area = $('#Area option:selected').text();
        var load = $.ajax({ type: "GET", cache: false, url: UCFCategoriesAPI, data: { Area: Area } });
        load.success(function (data) {
            self.UCFCategories(data);
            data.forEach(function (i) {
                var loadChallenges = $.ajax({ type: "GET", url: UCFChallengesAPI, cache: false, data: { ID: i.ID } });
                load.done(function (data) {
                    loadChallenges.success(function (data1) {

                        data1.forEach(function (ii) {
                            self.UCFChallenges.push(ii);
                            var loadCriteria = $.ajax({ type: "GET", url: UCFCriteriaAPI, cache: false, data: { ID: ii.ID } })
                            loadCriteria.success(function (data2) {
                                data2.forEach(function (iii) {
                                    self.UCFCriteria.push(iii);
                                    self.loadPreviousScores();
                                });
                            });
                        });
                    });
                });
            });
        });

    }

    self.loadData = function () {
        self.loadPlantRollup();
        self.loadAreaTrend();
        self.LoadActions();
    }
}