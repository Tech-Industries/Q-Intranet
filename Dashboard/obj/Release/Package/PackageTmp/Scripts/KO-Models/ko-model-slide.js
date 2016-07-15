function SlideViewModel() {
    var self = this;
    var financeAPI = $("#financeLink").attr('href');
    var qualityAPI = $("#qualityLink").attr('href');
    var goalsAPI = $("#goalsLink").attr('href');
    var slideAPI = $("#SlideLink").attr('href');
    var pageAPI = $("#PageLink").attr('href');
    var UCFAuditsAPI = $("#UCFAuditsLink").attr('href');
    var flashAPI = $("#FlashLink").attr('href');

    self.ChartSalesMTD = ko.observableArray([]);
    self.ChartScrapMTD = ko.observableArray([]);
    self.ChartCWOMTD = ko.observableArray([]);
    self.SalesGoals = ko.observableArray([]);
    self.ScrapGoals = ko.observableArray([]);
    self.Goals = ko.observableArray([]);
    self.ChartCategories = ko.observableArray([]);

    self.SalesToday = ko.observable();
    self.SalesMTD = ko.observable();
    self.SalesMTDI = ko.observable();
    self.MarginsMTD = ko.observable();
    self.MarginsMTDPercOfSales = ko.observable();
    self.SalesGoalM = ko.observable();
    self.ScrapMTD = ko.observable();
    self.ScrapMTDPercOfSales = ko.observable();
    self.ScrapMTDI = ko.observable();
    self.OTMTD = ko.observable();
    self.CWOMTD = ko.observable();
    self.SalesYTD = ko.observable();


    self.SalesTodayPerc = ko.observable();
    self.SalesMTDPerc = ko.observable();
    self.SalesYTDPerc = ko.observable();

    self.SalesTodayPerc = 10;
    self.SalesMTDPerc = 50;
    self.SalesYTDPerc = 100;

    self.SelectedDay = ko.observable();
    self.SelectedDaySales = ko.observableArray([]);
    self.SelectedDaySalesAccum = ko.observable();
    self.SelectedDayMargAccum = ko.observable();
    self.SelectedDayMargPerc = ko.observable();
    self.SelectedDayScrap = ko.observableArray([]);
    self.SelectedDayScrapAccum = ko.observable();
    self.Incentives = ko.observableArray([]);
    self.isLoading = ko.observable(false);

    self.MeetingInfo = ko.observableArray([]);
    self.Test = ko.observableArray([]);
    
    self.FacilityScore = ko.observable();
    self.AreaScores = ko.observableArray([]);

    self.DeliveryRates = ko.observableArray([]);
    self.RollingTwelveLabels = ko.observableArray([]);

    var nameContain = "All";
    var year = "";
    var month = "";
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();


    self.loadOnTimeDeliveryTrend = function () {
        var ID = $('#plantSelect option:selected').attr('plantid');
        Year = $("#yearSelect").val();
        Month = $("#monthSelect").val();
        Range = 11;
        var load = $.ajax({ type: "GET", url: flashAPI, cache: false, data: { PlantID: ID, Year: Year, Month: Month, Range: Range } });
        load.done(function (data) {
            var deliveryRates = [];
            check = Month - Range;
            if (check <= 0) {
                check += 12;
            }
            incr = 0;
            i = 0;
            while (incr <= Range) {
                if (i < data.length) {
                    if (data[i].Month == check) {
                        deliveryRates.push(data[i].OnTimePercent);
                        i++;
                    }
                    else {
                        deliveryRates.push(null);
                    }
                }
                incr += 1;
                check += 1;
                if (check > 12) {
                    check -= 12;
                }

            }
            while (deliveryRates.length < Range + 1) {
                deliveryRates.push(null);
            }
            self.DeliveryRates(deliveryRates);

            self.RollingTwelveLabels(getMonthNamesInRange(Month, Range + 1));
        });
    }


    self.loadUCFSlide = function () {
        PlantID = $('#plantSelect option:selected').attr('plantid');
        Year = $('#yearSelect').val();
        Month = $('#monthSelect').val();

        var load = $.ajax({ type: "GET", url: UCFAuditsAPI, cache: false, data: { PlantID: 3, Year: Year, Month: '05' } });
        load.done(function (data) {
            if (data.length > 0) {
                var t = $.grep(data, function (e) { return e.AreaName == 'Facility' })[0];
            }
        });
    }

    self.CheckRefreshFlag = function () {

        var load = $.ajax({ type: "GET", url: pageAPI, cache: false, data: { Name: 'SlideShow' }});
        load.done(function (data) {
            if (data[0].Refresh == true) {
                location.reload();
            }
            else {
            }

        });
    }

    self.loadMeeting = function () {
        plantid = 1;
        var load = $.ajax({ type: "GET", url: slideAPI, cache: false, data: { PlantID: plantid, Type: 'Meeting' } });
        load.done(function (data) {
            d = data[0];
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    What: item.Meeting,
                    Where: item.Location,
                    When: getLongDate(item.Time),
                    ShortDate: formatSqlDateTimeToShortDate(item.Time)


                };
            });
            self.MeetingInfo(array[0]);
            
        });
    }

    self.loadIncentives = function () {
        plantid = 1;
        year = $("#yearSelect").val();
        month = $("#monthSelect").val();
        var days = getDaysInMonth(parseInt(month), parseInt(year));
        var currYear = new Date().getFullYear();
        var currMonth = new Date().getMonth() + 1;
        var currDay = new Date().getDate();
        var curr = false;
        if (parseInt(year) == currYear && parseInt(month) == currMonth) {
            curr = true;
        }

        var load = $.ajax({ type: "GET", url: slideAPI, cache: false, data: { ID: plantid, Year: year, Month: month } });
        load.done(function (data) {
            if (curr) {
                currGoal = (data[0].CWOGoal / days) * currDay;
                lowerGoal = (data[0].CWOGoal / days) * (currDay - 2);
                if (data[0].Cwo >= currGoal) {
                    data[0].CwoStatus = 'green';
                }
                if (data[0].Cwo >= lowerGoal) {
                    data[0].CwoStatus = 'yellow';
                }
                if (data[0].Cwo < lowerGoal) {
                    data[0].CwoStatus = 'red';
                }

            }
            else {
                if (data[0].Cwo >= data[0].CWOGoal) {
                    data[0].CwoStatus = 'green';
                }
                else if (data[0].Cwo < data[0].CWOGoal) {
                    data[0].CwoStatus = 'red';
                }
            }

            if (data[0].ScrapAsPercent > 1.5) {
                data[0].ScrapStatus = 'red';
            }
            else if (data[0].ScrapAsPercent <= 1.5) {
                data[0].ScrapStatus = 'green';
            }


            if (data[0].UCFScore == null) {
                data[0].UCFStatus = 'yellow';
                data[0].UCFScore = 'N/A';
            }
            else if (data[0].UCFScore >= data[0].UCFGoal) {
                data[0].UCFStatus = 'green';
            }
            else if (data[0].UCFScore >= data[0].UCFGoal - 5) {
                data[0].UCFStatus = 'yellow';
            }
            else if (data[0].UCFScore < data[0].UCFGoal - 5) {
                data[0].UCFStatus = 'red';
            }

            if (data[0].PDSales > 30000) {
                data[0].PDStatus = 'red';
            }
            else if (data[0].PDSales > 0) {
                data[0].PDStatus = 'yellow';
            }
            else {
                data[0].PDStatus = 'green';
            }

            data[0].ScrapAsPercent = formatPercent(data[0].ScrapAsPercent);
            data[0].Cwo = formatMoney(data[0].Cwo).split('.')[0];
            data[0].CWOGoal = formatMoney(data[0].CWOGoal).split('.')[0];
            data[0].PDSales = formatMoney(data[0].PDSales).split('.')[0];
            self.Incentives(data[0]);
        });
    }

    self.loadSales = function () {
        nameContain = $("#plantSelect").val();
        year = $("#yearSelect").val();
        month = $("#monthSelect").val();
        if (nameContain == null) {
            nameContain = "All";
        }
        var load = $.ajax({ type: "GET", url: financeAPI, cache: false, data: { fType: "Sales", nameContain: nameContain, year: year, month: month } });
        load.done(function (data) {
            self.isLoading(true);
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Comapny: item.Comapny,
                    Year: item.Year,
                    Month: item.Month,
                    Day: item.Day,
                    Sales: item.Sales,
                    MarginAmt: item.MarginAmt


                };
            });



            var daysInMonth = new Date(year, month, 0).getDate();
            var len = array.length;
            var newData = [];
            var newCat = [];
            var accum = 0;
            var margAccum = 0;
            var dayIncr = 0;

            for (var i = 0; i < len;) {
                var day = array[i].Day;

                dayIncr++;
                while (day > dayIncr) {
                    newData.push(Math.round(accum * 100) / 100);
                    newCat.push(year + '-' + month + '-' + dayIncr.toString());

                    dayIncr++;
                }

                accum += array[i].Sales;
                margAccum += array[i].MarginAmt;
                newData.push(Math.round(accum * 100) / 100);
                newCat.push(year + '-' + month + '-' + day);
                i++;
            }
            while (dayIncr < daysInMonth) {
                dayIncr += 1;
                if (mm == month && dayIncr < dd - 1) {
                    newData.push(Math.round(accum * 100) / 100);
                }
                else if (mm == month && dayIncr > dd - 1) {
                    newData.push(null);
                }
                if (mm != month) {
                    newData.push(Math.round(accum * 100) / 100);
                }



                newCat.push(year + '-' + month + '-' + dayIncr.toString());
            }
            self.ChartSalesMTD(newData);
            self.ChartCategories(newCat);
            self.SalesMTD(formatMoney(accum));
            self.SalesMTDI(accum);
            self.MarginsMTD(formatMoney(margAccum));
            self.MarginsMTDPercOfSales(formatPercent((margAccum / self.SalesMTDI()) * 100));
        });
        load.fail(function () { console.log("fail"); });
        load.always(function () { self.isLoading(false); });


    };

    self.loadScrap = function () {
        nameContain = $("#plantSelect").val();
        year = $("#yearSelect").val();
        month = $("#monthSelect").val();
        if (nameContain == null) {
            nameContain = "All";
        }
        var load = $.ajax({ type: "GET", url: qualityAPI, cache: false, data: { fType: 'MTDChart', nameContain: nameContain, year: year, month: month } });
        load.done(function (data) {
            self.isLoading(true);
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Amount: item.Amount,
                    Plant: item.Plant,
                    Year: item.year,
                    Month: item.Month,
                    Day: item.Day


                };
            });



            var daysInMonth = new Date(year, month, 0).getDate();
            var len = array.length;
            var newData = [];
            var accum = 0;
            var dayIncr = 0;

            for (var i = 0; i < len;) {
                var day = array[i].Day;

                dayIncr++;
                while (day > dayIncr) {
                    newData.push(Math.round(accum * 100) / 100);

                    dayIncr++;
                }

                accum += array[i].Amount;
                newData.push(Math.round(accum * 100) / 100);
                i++;
            }
            while (dayIncr < daysInMonth) {
                dayIncr += 1;
                if (mm == month && dayIncr < dd - 1) {
                    newData.push(Math.round(accum * 100) / 100);
                }
                else if (mm == month && dayIncr > dd - 1) {
                    newData.push(null);
                }
                if (mm != month) {
                    newData.push(Math.round(accum * 100) / 100);
                }
            }
            self.ChartScrapMTD(newData);
            self.ScrapMTD(formatMoney(accum));
            self.ScrapMTDPercOfSales(formatPercent((accum / self.SalesMTDI()) * 100));
            self.ScrapMTDI(accum);
        });
        load.fail(function () { console.log("fail"); });
        load.always(function () { self.isLoading(false); });


    };

    self.loadSalesGoals = function () {
        nameContain = $("#plantSelect").val();
        month = $("#monthSelect").val();
        year = $("#yearSelect").val();
        var scrapMod = 0;
        if (nameContain == 'BEST') {
            scrapMod = .015;
        }
        else {
            scrapMod = .0125;
        }
        var daysInMonth = new Date(year, month, 0).getDate();;
        if (nameContain == null) {
            nameContain = 'All';
        }
        var load = $.ajax({ type: "GET", url: goalsAPI, cache: false, data: { fType: "Sales", nameContain: nameContain, year: year, month: month } });
        load.done(function (data) {
            self.isLoading(true);
            self.Goals([]);
            var array = $.map(data, function (item) {
                return {
                    Amount: item.Amount
                };

            });
            self.Goals(array);
            var len = array.length;
            if (len > 0) {
                var dailyAdd = (array[0].Amount / daysInMonth);
                var accum = 0, output = [], scrapOutput = [];

                for (var i = 0; i < daysInMonth; i++) {
                    accum += dailyAdd;
                    output.push(Math.round(accum * 100) / 100);
                    scrapOutput.push((Math.round(accum * scrapMod) * 100) / 100);
                }
                self.SalesGoals(output);
                self.ScrapGoals(scrapOutput);
            }
            else {
                self.SalesGoals([]);
                self.ScrapGoals([]);
            }
            self.SalesGoalM(formatMoney(accum));

        });
        load.fail(function () { console.log("fail"); });
        load.always(function () { self.isLoading(false); });

    }

    self.loadSelectDaySales = function (year, month, day) {


        if (day < 10) { day = "0" + day; }
        nameContain = $("#plantSelect").val();
        var load = $.ajax({ type: "GET", url: financeAPI, cache: false, data: { fType: "SelectedDay", nameContain: nameContain, year: year, month: month, day: day } });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    OnTime: item.OnTime,
                    Blank: "",
                    PlantName: item.PlantName,
                    Customer: item.Customer,
                    PartNum: item.PartNum,
                    Description: item.Description,
                    DateDue: item.DateDue,
                    QtyShipped: item.QtyShipped,
                    Extension: item.Extension,
                    Cost: item.Cost,
                    MarginAmt: item.MarginAmt,
                    MarginPercent: item.MarginPercent

                };
            });
            var accum = 0;
            var margAccum = 0;
            var tempArray = [];
            $.each(array, function (index, val) {
                accum += val.Extension;
                margAccum += val.MarginAmt;

            });


            self.SelectedDaySalesAccum(formatMoney(accum));
            self.SelectedDayMargAccum(formatMoney(margAccum));
            self.SelectedDayMargPerc(formatPercent((margAccum / accum) * 100));
            self.SelectedDaySales(tempArray);
            self.SelectedDay(year + "-" + month + "-" + day);



            if (self.SelectedDaySalesAccum() != '$0.00') {

                $('.dtSalesDetail').DataTable().clear();
                $('.dtSalesDetailSmall').DataTable().clear();

                //insert data 
                $.each(array, function (i, item) {
                    $('.dtSalesDetail').DataTable().row.add([

                      item.Blank,
                      item.OnTime,
                      item.PlantName,
                      item.Customer,
                      item.PartNum,
                      item.Description,
                      formatShortDate(item.DateDue),
                      item.QtyShipped,
                      formatMoney(item.Extension),
                      formatMoney(item.Cost),
                      formatMoney(item.MarginAmt),
                      formatPercent(item.MarginPercent)
                    ]).draw();
                });
                $.each(array, function (i, item) {
                    $('.dtSalesDetailSmall').DataTable().row.add([

                      item.Blank,
                      item.OnTime,
                      item.Customer,
                      item.PartNum,
                      formatMoney(item.Extension),
                      formatPercent(item.MarginPercent),
                      item.QtyShipped,
                    ]).draw();
                });

            }
            else {

                $('.dtSalesDetail').dataTable().fnClearTable();
                swal(
                {
                    title: "No Records Found!",
                    text: "It looks like there were no sales recorded on " + self.SelectedDay() + "",
                    showCancelButton: false,
                    closeOnConfirm: true,
                    animation: "slide-from-top"
                },
                function () { $("#salesDetailModal").modal('hide'); });
            }
            $("#salesDetailModal").modal('show');

        });


    }

    self.loadSelectDayScrap = function (year, month, day) {


        if (day < 10) { day = "0" + day; }
        nameContain = $("#plantSelect").val();
        var load = $.ajax({ type: "GET", url: qualityAPI, cache: false, data: { fType: "SelectedDay", nameContain: nameContain, year: year, month: month, day: day } });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    PlantCode: item.PlantCode,
                    ControlNumber: item.ControlNumber,
                    Job: item.Job,
                    Suffix: item.Suffix,
                    PartNum: item.PartNum,
                    Description: item.Description,
                    QtyDisposed: item.QtyDisposed,
                    DisposedValue: item.DisposedValue,

                };
            });
            var accum = 0;
            var margAccum = 0;
            var tempArray = [];
            $.each(array, function (index, val) {
                accum += val.DisposedValue;
            });


            self.SelectedDayScrapAccum(formatMoney(accum));
            self.SelectedDayScrap(tempArray);
            self.SelectedDay(year + "-" + month + "-" + day);



            if (self.SelectedDayScrapAccum() != '$0.00') {

                $('.dtScrapDetail').DataTable().clear();

                //insert data 
                $.each(array, function (i, item) {
                    $('.dtScrapDetail').DataTable().row.add([

                      item.PlantCode,
                      item.ControlNumber,
                      item.Job,
                      item.Suffix,
                      item.PartNum,
                      item.Description,
                      item.QtyDisposed,
                      formatMoney(item.DisposedValue)

                    ]).draw();
                });

            }
            else {

                $('.dtScrapDetail').dataTable().fnClearTable();
                swal(
                {
                    title: "No Records Found!",
                    text: "It looks like there were no scrap records on " + self.SelectedDay() + "",
                    showCancelButton: false,
                    closeOnConfirm: true,
                    animation: "slide-from-top"
                },
                function () { $("#scrapDetailModal").modal('hide'); });
            }
            $("#scrapDetailModal").modal('show');

        });


    }

    self.loadCWO = function () {
        nameContain = $("#plantSelect").val();
        year = $("#yearSelect").val();
        month = $("#monthSelect").val();
        if (nameContain == null) {
            nameContain = "All";
        }
        var load = $.ajax({ type: "GET", url: financeAPI, cache: false, data: { fType: "CWO", nameContain: nameContain, year: year, month: month } });
        load.done(function (data) {
            self.isLoading(true);
            var array = $.map(data, function (item) {
                return {
                    ID: item.ID,
                    Comapny: item.Comapny,
                    Year: item.Year,
                    Month: item.Month,
                    Day: item.Day,
                    Amount: item.Amount


                };
            });



            var daysInMonth = new Date(year, month, 0).getDate();
            var len = array.length;
            var newData = [];
            var accum = 0;
            var dayIncr = 0;

            for (var i = 0; i < len;) {
                var day = array[i].Day;

                dayIncr++;
                while (day > dayIncr) {
                    newData.push(Math.round(accum * 100) / 100);

                    dayIncr++;
                }

                accum += array[i].Amount;
                newData.push(Math.round(accum * 100) / 100);
                i++;
            }
            while (dayIncr < daysInMonth) {
                dayIncr += 1;
                if (mm == month && dayIncr < dd - 1) {
                    newData.push(Math.round(accum * 100) / 100);
                }
                else if (mm == month && dayIncr > dd - 1) {
                    newData.push(null);
                }
                if (mm != month) {
                    newData.push(Math.round(accum * 100) / 100);
                }



            }
            self.ChartCWOMTD(newData);
            self.CWOMTD(formatMoney(accum));
        });
        load.fail(function () { console.log("fail"); });
        load.always(function () { self.isLoading(false); });


    };

    self.loadScrapMTD = function () {

        year = $("#yearSelect").val();
        month = $("#monthSelect").val();
        nameContain = $("#plantSelect").val();
        var load = $.ajax({ type: "GET", url: qualityAPI, cache: false, data: { fType: 'Snapshot', nameContain: nameContain, year: year, month: month } });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    Amount: item.Amount
                };
            });
            if (array.length != 1) {
                self.ScrapMTD("$0.00");
            }
            else {
                self.ScrapMTD(formatMoney(array[0].Amount));
            }


        });


    }

    self.loadOTMTD = function () {

        year = $("#yearSelect").val();
        month = $("#monthSelect").val();
        nameContain = $("#plantSelect").val();
        var load = $.ajax({ type: "GET", url: financeAPI, cache: false, data: { fType: "Overtime", nameContain: nameContain, year: year, month: month } });
        load.done(function (data) {
            var array = $.map(data, function (item) {
                return {
                    Amount: item.Amount
                };
            });
            if (array.length != 1) {
                self.OTMTD("$0.00");
            }
            else {
                self.OTMTD(formatMoney(array[0].Amount));
            }


        });


    }

    self.loadData = function () {

        self.loadSales();
        self.loadCWO();
        self.loadSalesGoals();
        self.loadScrap();
        self.loadOTMTD();
        self.loadIncentives();
        self.loadMeeting();
        self.loadUCFSlide();
        self.loadOnTimeDeliveryTrend();
        //$('.dtSalesDetail').dataTable().fnClearTable();
    }
}