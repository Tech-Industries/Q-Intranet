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

    self.PreviousScores = ko.observableArray([]);

    self.checkOwnerID = function () {
        self.OwnerID(parseInt($('#areaSelect option:selected').attr('ownerid')));
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
        if (parseInt($('#UID').val()) == self.OwnerID || $('#RelPer').val() == "9") {
            var del = $.ajax({ type: 'DELETE', url: UCFActionsAPI + '/' + ID, contentType: 'application/json' });
            del.always(function () {
                var remove = $.grep(self.Actions(), function (e) { return e.ID == ID })[0];
                self.Actions.remove(remove);
            });
        }
    }

    self.updateActionStatus = function (ID, Status, DateComplete) {
        console.log(parseInt($('#UID').val() + ' - ' + self.OwnerID));
        if (parseInt($('#UID').val()) == self.OwnerID || $('#RelPer').val() == "9") {
            var t = $.grep(self.Actions(), function (e) { return e.ID == ID })[0];
            t.Status = Status;
            t.DateComplete = DateComplete;
            var update = $.ajax({ type: "PUT", url: UCFActionsAPI + "/" + ID, cache: false, data: t });
            update.done(function (data) {
            });
        }
    }
    self.loadPreviousScores = function () {

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

    self.loadPlants = function () {
        var load = $.ajax({ type: "GET", cache: false, url: PlantsAPI });
        load.success(function (data) {
            self.Plants(data);
        });

    }

    self.loadAreas = function (ID) {
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
        var Area = $('#Area option:selected').text();
        var load = $.ajax({ type: "GET", cache: false, url: UCFCategoriesAPI, data: { Area: Area } });
        load.success(function (data) {
            self.UCFCategories(data);
        });
    }
    self.LoadChallenges = function (ID) {
        var load = $.ajax({ type: "GET", url: UCFChallengesAPI, cache: false, data: { ID: ID } })
        load.success(function (data2) {
            self.UCFChallenges(data2);
        });
    }

    self.LoadCriteria = function (ID) {
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

    self.loadPlantRollup = function () {

        var ID = $('#plantSelect option:selected').attr('plantid');
        var Year = parseInt($('#yearSelect').val());
        var Month = parseInt($('#monthSelect').val());
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
        var ID = $('#plantSelect option:selected').attr('plantid');
        var Year = parseInt($('#yearSelect').val());
        var Month = parseInt($('#monthSelect').val());
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