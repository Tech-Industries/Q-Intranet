
function QualityViewModel() {
    var self = this;
    var usersAPI = $("#usersLink").attr('href');
    var groupsAPI = $("#groupsLink").attr('href');
    var userGroupsAPI = $("#userGroupsLink").attr('href');
    var qualityAPI = $("#qualityV1Link").attr('href');

    var CommentsAPI = $("#CommentsLink").attr('href');
    var AssignBtn = null;
    self.Comments = ko.observableArray([]);

    self.isLoading = ko.observable();

    self.Users = ko.observableArray([]);
    self.Names = ko.observableArray([]);
    self.Jobs = ko.observableArray([]);
    self.Suffix = ko.observableArray([]);
    self.Seq = ko.observableArray([]);

    self.InspectionTypes = ko.observableArray([]);

    self.GetFilteredOnboardingTasks = function (part) {

        return $.grep(self.OnboardingTasks(), function (e) { return e[0].OnBoardPart == part.RecordNumber });
    }


    self.loadNames = function () {
        self.Names([]);
        var plantID = 4;
        var load = $.ajax({ type: "GET", url: qualityAPI + '/names', cache: false, data: { plantID } });
        load.done(function (data) {

            data.unshift('');
            self.Names(data);
        });
    }

    self.loadJobs = function () {
        self.Names([]);
        var plantID = 4;
        var load = $.ajax({ type: "GET", url: qualityAPI + '/jobs', cache: false, data: { plantID } });
        load.done(function (data) {
            data.unshift('');
            self.Jobs(data);
        });
    }

    self.loadSuffix = function (job) {
        self.Suffix([]);
        var plantID = 4;
        var load = $.ajax({ type: "GET", url: qualityAPI + '/jobs/' + job + '/suffix', cache: false, data: { plantID, job } });
        load.done(function (data) {
            data.unshift('');
            self.Suffix(data);
        });
    }

    self.loadSeq = function (job, suffix) {
        self.Seq([]);
        var plantID = 4;
        var load = $.ajax({ type: "GET", url: qualityAPI + '/jobs/' + job + '/suffix/' + suffix + '/seq', cache: false, data: { plantID, job, suffix } });
        load.done(function (data) {
            var i = data[0];
            i.Seq = '';
            i.Description = '';
            data.unshift(i);
            self.Seq(data);
        });
    }

    self.loadInspectionTypes = function () {
        self.InspectionTypes([]);
        var plantID = 4;
        var load = $.ajax({ type: "GET", url: qualityAPI + '/inspectiontypes', cache: false, data: { plantID } });
        load.done(function (data) {

            data.unshift('');
            self.InspectionTypes(data);
        });
    }

    self.loadData = function () {
        self.loadNames();
        self.isLoading(false);
        self.loadJobs();
        self.loadInspectionTypes();
    }
}