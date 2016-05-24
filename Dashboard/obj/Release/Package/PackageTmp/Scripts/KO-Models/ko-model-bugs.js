function BugsViewModel() {
    var self = this;
    var projectsAPI = $("#projectsLink").attr('href');
    var bugsAPI = $("#bugsLink").attr('href');
    var bugTagsAPI = $("#bugTagsLink").attr('href');
    var usersAPI = $("#usersLink").attr('href');
    var CommentsAPI = $("#CommentsLink").attr('href');

    self.Comments = ko.observableArray([]);
    self.Bugs = ko.observableArray([]);
    self.Tags = ko.observableArray([]);
    self.Projects = ko.observableArray([]);
    self.SelectedBug = ko.observableArray([]);
    self.SelectedBugRaw = ko.observableArray([]);

    self.Users = ko.observableArray([]);
    self.AllBugs = ko.observableArray([]);

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

    self.LoadProjects = function () {
        var status = $('#status').val();
        var load = $.ajax({ type: "GET", cache: false, url: projectsAPI });
        load.success(function (data) {
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Name: item.Name
                };
            });
            self.Projects(array);
        });
    }

    self.LoadBugs = function () {
        var status = $('#status').val();
        var type = $('#type').val();
        console.log(status);
        console.log(type);
        var load = $.ajax({ type: "GET", cache: false, url: bugsAPI, data: { Status: status, Type: type} });
        load.success(function (data) {
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Project: item.Project,
                    Description: item.Description,
                    Type: item.Type,
                    Creator: item.Creator,
                    Assignee: item.Assignee,
                    DateCreated: formatSqlDateTime(item.DateCreated),
                    DateClosed: formatSqlDateTime(item.DateClosed),
                    Status: item.Status,
                    Tags: item.Tags.split(", ")


                };
            });
            self.Bugs(array);
        });
        
    }

    self.LoadBugByID = function (ID) {
        self.SelectedBug($.grep(self.Bugs(), function (e) { return e.ID == ID })[0]);
        $('#Status').val(self.SelectedBug().Status);
        var loadRaw = $.ajax({ type: "GET", cache: false, url: bugsAPI, data: { ID: ID, DataPull: "Bug"} });
        loadRaw.success(function (data) {
            self.SelectedBugRaw(data);
            $('#Assignee').val(self.SelectedBugRaw()[0].AssigneeID);
        });
    }

    self.addBug = function () {
        var Type = $('.type-select.active').html();
        var Project = $("#Project").val();
        var Description = $('#Description').val();
        var UserID = $('#UserID').val();
        var Tags = $('#Tags').val();

        if (Description.length > 0 && Project != 99999 && Tags != null) {
            var sendData = "Project=" + Project + "&Description=" + Description + "&Type=" + Type + "&UserID=" + UserID + "&AssigneeID=0" + "&DateCreated=" + DateNowFormatted() + "&DateClosed=" + "&Status=" + "New" + "&Notes=";
            var add = $.ajax({ type: "POST", url: bugsAPI, cache: false, data: sendData });
            add.success(function (data) {
                data = data[0];

                Tags.forEach(function (item) {
                    var aSendData = "BugID=" + data.ID + "&Tag=" + item;
                    var assign = $.ajax({ type: "POST", url: bugTagsAPI, cache: false, data: aSendData });
                });
                $('#Project').val(99999);
                $('#Description').val("");
                $('#Tags').select2("val", "");
            });
        }
    }

    self.UpdateBug = function () {
        var ID = $('#BugID').val();
        var c = self.SelectedBugRaw()[0];
        c.Status = $('#Status').val();
        if (c.Status == 'Closed' && c.DateClosed == null) {
            c.DateClosed = DateNowFormatted();
        }
        else if(c.Status != 'Closed'){
            c.DateClosed = null;
        }
        if ($('#Assignee').val() != 0) {
            c.AssigneeID = $('#Assignee').val();
        }
        else {
            c.AssigneeID = null;
        }
        $('#SelectedDateClosed').html(c.DateClosed);
        var update = $.ajax({ type: "PUT", url: bugsAPI + "/" + ID, cache: false, data: c });
        update.done(function (data) {
            self.LoadBugs();
        });
    }

    self.LoadComments = function (ID) {
        loadComments(ID, 'Bugs', CommentsAPI, self);
    };

    self.addComment = function () {
        var TypeID = $("#BugID").val();
        var UserID = $("#UserID").val();
        var Comment = $("#txt-new-comment").val();
        addComment(TypeID, UserID, Comment, "Bugs", CommentsAPI, self);

    }
    self.updateComment = function (ID, TimeSubmitted, Comment) {
        updateComment(ID, TimeSubmitted, Comment, self.Comments(), CommentsAPI);
    }
    self.removeComment = function (ID) {
        removeComment(ID, CommentsAPI, self);
    }
    //self.LoadTasks = function () {
    //    var ProjID = $('#ProjID').val();
    //    self.SelectedProject([]);
    //    self.SelectedProject.push($.grep(self.Projects(), function (e) { return e.ID == ProjID })[0]);

    //    var load = $.ajax({ type: "GET", url: projectTasksAPI, cache: false, data: { ProjID: ProjID } });
    //    load.done(function (data) {
    //        var todo = [];
    //        var active = [];
    //        var complete = [];
    //        data.forEach(function (i) {
    //            if (i.Status == 'To-Do') {
    //                todo.push(i);
    //            }
    //            else if (i.Status == 'Active') {
    //                active.push(i);
    //            }
    //            else if (i.Status == 'Complete') {
    //                complete.push(i);
    //            }
    //        });
    //        self.AllTasks(data);
    //        self.Todo(todo);
    //        self.Active(active);
    //        self.Complete(complete);


    //    });
    //}

    //self.updateTaskStatus = function (taskid, oldStatus, newStatus) {


    //    var t = $.grep(self.AllTasks(), function (e) { return e.ID == taskid })[0];
    //    t.Status = newStatus;
    //    var update = $.ajax({ type: "PUT", url: projectTasksAPI + "/" + taskid, cache: false, data: t });
    //    update.done(function (data) {
    //    });
    //}

    //self.addTask = function () {
    //    var ProjID = $('#ProjID').val();
    //    var Name = $('#task-name').val();
    //    var Description = $('#task-description').val();
    //    var EstTime = $('#task-esttime').val();
    //    var AssigneeID = $('#task-assignee').val();
    //    var CreatorID = $('#UID').val();

    //    if (Name.length > 0 && EstTime.length > 0 && AssigneeID != 0) {
    //        var sendData = "ProjID=" + ProjID + "&Name=" + Name + "&Description=" + Description + "&DateCreated=" + DateNowFormatted() + "&DateCreated=" + null + "&Status=" + "To-Do" + "&EstTime=" + EstTime + "&CreatorID=" + CreatorID + "&AssigneeID=" + AssigneeID;
    //        var add = $.ajax({ type: "POST", url: projectTasksAPI, cache: false, data: sendData });
    //        add.success(function (data) {
    //            self.Todo.push(data);

    //            $('#task-name').val("");
    //            $('#task-description').val("");
    //            $('#task-esttime').val("");
    //            $('#task-assignee').val(0);
    //        });

    //    }
    //}
}