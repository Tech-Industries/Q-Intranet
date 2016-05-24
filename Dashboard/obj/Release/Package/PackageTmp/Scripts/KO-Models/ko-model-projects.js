function ProjectsViewModel() {
    var self = this;
    var projectsAPI = $("#projectsLink").attr('href');
    var projectAssigneesAPI = $("#projectAssigneesLink").attr('href');
    var projectTasksAPI = $("#projectTasksLink").attr('href');
    var usersAPI = $("#usersLink").attr('href');
    self.Projects = ko.observableArray([]);
    self.SelectedProject = ko.observableArray([]);

    self.Users = ko.observableArray([]);
    self.Todo = ko.observableArray([]);
    self.Active = ko.observableArray([]);
    self.Complete = ko.observableArray([]);
    self.AllTasks = ko.observableArray([]);

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
        if (status == 'All') {
            var load = $.ajax({ type: "GET", cache: false, url: projectsAPI });
        }
        else {
            var load = $.ajax({ type: "GET", cache: false, url: projectsAPI, data: { Status: status } });
        }
        load.success(function (data) {
            var array = $.map(data, function (item) {
                if (item.PercComplete == null) {
                    item.PercComplete = 0.00;
                    item.TasksComplete = 0;
                    if (item.TotalTasks == null) {
                        item.TotalTasks = 0;
                    }
                }
                
                return {
                    ID: item.ID,
                    Name: item.Name,
                    Descripiton: item.Descripiton,
                    Status: item.Status,
                    DateCreated: formatSqlDateTime(item.DateCreated),
                    Priority: item.Priority,
                    TasksComplete: item.TasksComplete,
                    TotalTasks: item.TotalTasks,
                    PercComplete: item.PercComplete,
                    EstTimeRemaining: item.EstTimeRemaining,
                    Assignees: item.Assignees


                };
            });
            self.Projects(array);
        });
    }

    self.addProject = function () {
        var Name = $('#proj-name').val();
        var Description = $('#proj-description').val();
        var Priority = $('#proj-priority').val();
        var Assignees = $('#proj-assignees').val();
        var CreatorID = $('#UID').val();

        if (Name.length > 0 && Description.length > 0 && Priority != 0 && Assignees != null) {
            var sendData = "Name=" + Name + "&Description=" + Description + "&Status=" + "New" + "&DateCreated=" + DateNowFormatted() + "&CreatorID=" + CreatorID + "&Priority=" + Priority;
            var add = $.ajax({ type: "POST", url: projectsAPI, cache: false, data: sendData });
            add.success(function (data) {
                data = data[0];
                Assignees.forEach(function (item) {
                    var aSendData = "ProjID=" + data.ID + "&AssigneeID=" + item;
                    var assign = $.ajax({ type: "POST", url: projectAssigneesAPI, cache: false, data: aSendData });
                });
                //self.Projects.push(data);
                self.LoadProjects();

                $('#proj-name').val("");
                $('#proj-description').val("");
                $('#proj-priority').val("");
                $('#proj-assignees').select2("val", "");
            });
        }
    }

    self.LoadTasks = function () {
        var ProjID = $('#ProjID').val();
        self.SelectedProject([]);
        self.SelectedProject.push($.grep(self.Projects(), function (e) { return e.ID == ProjID })[0]);

        var load = $.ajax({ type: "GET", url: projectTasksAPI, cache: false, data: { ProjID: ProjID } });
        load.done(function (data) {
            var todo = [];
            var active = [];
            var complete = [];
            data.forEach(function (i) {
                if (i.Status == 'To-Do') {
                    todo.push(i);
                }
                else if (i.Status == 'Active') {
                    active.push(i);
                }
                else if (i.Status == 'Complete') {
                    complete.push(i);
                }
            });
            self.AllTasks(data);
            self.Todo(todo);
            self.Active(active);
            self.Complete(complete);


        });
    }

    self.updateTaskStatus = function (taskid, oldStatus, newStatus) {


        var t = $.grep(self.AllTasks(), function (e) { return e.ID == taskid })[0];
        t.Status = newStatus;
        var update = $.ajax({ type: "PUT", url: projectTasksAPI + "/" + taskid, cache: false, data: t });
        update.done(function (data) {
        });
    }

    self.addTask = function () {
        var ProjID = $('#ProjID').val();
        var Name = $('#task-name').val();
        var Description = $('#task-description').val();
        var EstTime = $('#task-esttime').val();
        var AssigneeID = $('#task-assignee').val();
        var CreatorID = $('#UID').val();
        
        if (Name.length > 0 && EstTime.length > 0 && AssigneeID != 0) {
            var sendData = "ProjID=" + ProjID + "&Name=" + Name + "&Description=" + Description + "&DateCreated=" + DateNowFormatted() + "&DateCreated=" + null + "&Status=" + "To-Do" + "&EstTime=" + EstTime + "&CreatorID=" + CreatorID + "&AssigneeID=" + AssigneeID;
            var add = $.ajax({ type: "POST", url: projectTasksAPI, cache: false, data: sendData });
            add.success(function (data) {
                self.Todo.push(data);

                $('#task-name').val("");
                $('#task-description').val("");
                $('#task-esttime').val("");
                $('#task-assignee').val(0);
            });

        }
    }
}