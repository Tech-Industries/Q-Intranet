function UsersViewModel() {
    var self = this;
    var usersAPI = $("#usersLink").attr('href');
    var groupsAPI = $("#groupsLink").attr('href');
    var userGroupsAPI = $("#userGroupsLink").attr('href');

    self.Users = ko.observableArray([]);
    self.Groups = ko.observableArray([]);
    self.AUserGroups = ko.observableArray([]);
    self.UAUserGroups = ko.observableArray([]);
    self.SelectedUserID = ko.observable();
    self.SelectedUserInfo = ko.observableArray([]);


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

    self.loadUser = function (ID) {
        var load = $.ajax({ type: "GET", url: usersAPI, cache: false, data: {ID: ID} });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    UserName: item.UserName,
                    FirstName: item.FirstName,
                    LastName: item.LastName,
                    Email: item.Email,
                    SupervisorID: item.SupervisorID
                }
            });
            self.SelectedUserInfo(array);
            console.log(array[0]);
        });
    }

    self.removeUser = function (id) {
        var del = $.ajax({ type: 'DELETE', url: usersAPI + '/' + id, contentType: 'application/json' });
        del.done(function (data) {
            self.Users.remove(function (item) { return item.ID === id });
            var load = $.ajax({ type: 'GET', url: userGroupsAPI, contenntType: 'application/json', data: { userId: id }});
            load.done(function (data){
                
                data.forEach(function (d) {
                   self.removeUserGroup(d.ID);
                });
            });
        });
    };

    self.addUser = function () {

        if ($('#FirstName').val().length > 0 && $('#LastName').val().length > 0 && $('#UserName').val().length > 0 && $('#Email').val().length > 0)

            var form = $('#frmAddUser');
        var add = $.ajax({ type: 'POST', url: usersAPI, data: form.serialize() });
        add.done(function (data) {
            self.Users.push(data);

        });

        $('#FirstName').val('');
        $('#LastName').val('');
        $('#UserName').val('');
        $('#Email').val('');
        $('#SupervisorID').val('');
        $('#FirstName').focus();

    }

    self.loadGroups = function () {
        var load = $.ajax({ type: "GET", url: groupsAPI, cache: false, data: {} });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Category: item.Category,
                    Access: item.Access,
                    Description: item.Description,
                }
            });
            self.Groups(array);
        });
    }

    self.removeGroup = function (id) {
        var del = $.ajax({ type: 'DELETE', url: groupsAPI + '/' + id, contentType: 'application/json' });
        del.done(function (data) {
            self.Groups.remove(function (item) { return item.ID === id });
        });
    };

    self.addGroup = function () {

        if ($('#Category').val().length > 0 && $('#Access').val().length > 0 && $('#Description').val().length > 0) {

            var form = $('#frmAddGroup');
            var add = $.ajax({ type: 'POST', url: groupsAPI, data: form.serialize() });
            add.done(function (data) {
                self.Groups.push(data);
                self.UAUserGroups.push(data);

            });

            $('#Category').val('');
            $('#Access').val('');
            $('#Description').val('');
            $('#Category').focus();
        }


       
    }

    self.loadAUserGroups = function (ID) {
        console.log(ID);
        self.AUserGroups([]);
        var load = $.ajax({ type: "GET", url: groupsAPI, cache: false, data: {ID: ID, ua: 'assigned' } });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Category: item.Category,
                    Access: item.Access,
                    Description: item.Description
                }
            });
            self.AUserGroups(array);
        });
    }

    self.loadUAUserGroups = function (ID) {
        self.UAUserGroups([]);
        var load = $.ajax({ type: "GET", url: groupsAPI, cache: false, data: { ID: ID, ua: 'unassgined' } });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Category: item.Category,
                    Access: item.Access,
                    Description: item.Description
                }
            });
            self.UAUserGroups(array);
        });
    }

    self.addUserGroup = function () {
        if ($('#UserID').val().length > 0) {

            var form = $('#frmAddUserGroup');
            var add = $.ajax({ type: 'POST', url: userGroupsAPI, data: form.serialize() });
            add.done(function (data) {
                var GroupID = $('#GroupID').val();
                var load = $.ajax({ type: "GET", url: groupsAPI, cache: false, data: { ID: GroupID } });
                load.done(function (data) {
                    var array = $.map(data, function (item) {
                        return {
                            ID: item.ID,
                            Category: item.Category,
                            Access: item.Access,
                            Description: item.Description
                        }
                    });
                    self.AUserGroups.push(array[0]);
                    self.UAUserGroups.remove(function (item) { return item.Category === array[0].Category && item.Access === array[0].Access});
                });

            });
            
        }
    }

    self.removeUserGroup = function (ID) {
        var del = $.ajax({ type: 'DELETE', url: userGroupsAPI + '/' + ID, contentType: 'application/json' });
        del.done(function (deldata) {

        });
    }
    self.removeFromAUserGroups = function (groupID) {

        var userID = $('#UserID').val();
        var fetch = $.ajax({ type: 'GET', url: userGroupsAPI, contentType: 'application/json', data: { userID: userID, groupID: groupID } });
        fetch.done(function (data) {
            console.log(data);
            var ID = data[0].ID;
            var del = $.ajax({ type: 'DELETE', url: userGroupsAPI + '/' + ID, contentType: 'application/json' });
            del.done(function (deldata) {
                console.log(groupID);
                var append;
                self.AUserGroups.remove(function (item) { if (item.ID === groupID) { append = item } return item.ID === groupID });
                self.UAUserGroups.push(append);
            });
         
        });
    }

    self.loadData = function () {
        self.loadUsers();
        self.loadGroups();
    }

    self.loadUserDetail = function (ID) {
        self.SelectedUserID(ID);
        self.loadAUserGroups(ID);
        self.loadUAUserGroups(ID);
        self.loadUser(ID);
    }
    
}