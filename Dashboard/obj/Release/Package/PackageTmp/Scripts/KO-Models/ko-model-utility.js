function UtilityViewModel() {
    var self = this;
    var utilityAPI = $("#UtilityLink").attr('href');
    var usersAPI = $("#usersLink").attr('href');

    self.Employees = ko.observableArray([]);
    self.isLoading = ko.observable(false);

    self.LoadADInfo = function () {
        self.isLoading(true);
        var load = $.ajax({ type: "GET", cache: false, url: utilityAPI+'/adinfo' });
        load.success(function (data) {
            self.Employees(data);
            $.each(data, function (i, item) {
                $('.dtEmployees').DataTable().row.add([
                //"<div><img src='data:image/jpg;charset=utf-8;base64, " + item.thumbnailPhoto + "'></div>",
                item.FirstName,
                item.LastName,
                item.Title,
                item.Email,
                item.OfficeNumber,
                item.Mobile
                ]).draw();
            });
        });
        load.always(function (data) {
            self.isLoading(false);
        });
    }

}