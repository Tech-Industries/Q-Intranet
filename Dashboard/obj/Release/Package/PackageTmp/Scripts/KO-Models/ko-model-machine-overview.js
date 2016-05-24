function MachineOverviewViewModel() {
    var self = this;
    var machineStateAPI = $("#machineStateLink").attr('href');
    self.LatheStates = ko.observableArray([]);
    self.MillStates = ko.observableArray([]);
    self.MachineDetails = ko.observableArray([]);
    self.CurrentState = ko.observable();
    self.TrailWeeks = ko.observable();
    self.SpindleUtilization = ko.observable();
    self.SUPercent = ko.observable();
    self.TrendChartCats = ko.observableArray([]);


    self.loadMachineStates = function () {
        var location = $('#plant-select').val();
        var load = $.ajax({ type: "GET", cache: false, url: machineStateAPI, data: { location: location } });
        load.success(function (data) {
            var array = $.map(data, function (item) {
                return {
                    Machine: item.RESOURCE_ID,
                    Description: item.Description,
                    Mode: item.RESOURCE_STATE,
                    Type: item.Type


                };
            });
            lathes = [];
            mills = [];
            $.each(array, function (i, v) {
                if (v.Type == "Lathe") { lathes.push(v) }
                else if (v.Type == "Mill") { mills.push(v) }
                else {
                    mills.push(v)
                }
            });
            self.LatheStates(lathes);
            self.MillStates(mills);
        });
    }

    self.loadMachineDetail = function (machine) {

        var location = $('#plant-select').val();
        var load = $.ajax({ type: "GET", cache: false, url: machineStateAPI, data: { location: location, machine: machine } });
        load.success(function (data) {
            var TWeeks = new Array();
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Machine: item.Machine,
                    Description: item.Description,
                    Mode: item.Mode,
                    CurrentMode: item.CurrentMode,
                    TotalTimeInMode: item.TotalTimeInMode,
                    StatePercentage: item.StatePercentage,
                    Perc1: item.Perc1,
                    Perc2: item.Perc2,
                    Perc3: item.Perc3,
                    Perc4: item.Perc4,
                    Date1: item.Date1,
                    Date2: item.Date2,
                    Date3: item.Date3,
                    Date4: item.Date4


                }
            });
            $.each(array, function (i, v) {
                try { v.Date1 = formatSqlDateTimeToShortDate(v.Date1); } catch (Exception) { }
                try { v.Date2 = formatSqlDateTimeToShortDate(v.Date2); } catch (Exception) { }
                try { v.Date3 = formatSqlDateTimeToShortDate(v.Date3); } catch (Exception) { }
                try { v.Date4 = formatSqlDateTimeToShortDate(v.Date4); } catch (Exception) { }
                if (v.CurrentMode != null) {
                    if (v.CurrentMode != self.CurrentState()) {
                        self.CurrentState(v.CurrentMode);
                    }
                }
                if (v.Mode == "IN CYCLE") {
                    self.SpindleUtilization(v.StatePercentage);
                    self.SUPercent(formatPercent(v.StatePercentage));
                }
                Tweeks = new Array();
                TCats = new Array();
                TWeeks.push(0);
                TWeeks.push(v.Perc1);
                TWeeks.push(v.Perc2);
                TWeeks.push(v.Perc3);
                TWeeks.push(v.Perc4);
                TCats.push('0');
                TCats.push(v.Date1);
                TCats.push(v.Date2);
                TCats.push(v.Date3);
                TCats.push(v.Date4);
            });
            if (array.length > 0) {
                self.TrendChartCats(TCats);
                self.MachineDetails(array);
                self.TrailWeeks(TWeeks);

            }
            else {
                self.MachineDetails([]);
                self.CurrentState("UNAVAILABLE");
                self.SpindleUtilization(0);
                self.SUPercent("N/A");
            }
        });
    }

}