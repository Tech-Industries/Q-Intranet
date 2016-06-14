function PrevMaintViewModel() {
    var self = this;
    var prevmaintAPI = $("#PrevMaintLink").attr('href');

    self.loadScorecard = function (year, month, day) {

        PlantID = $('#selPlant').val();
        var load = $.ajax({ type: "GET", url: prevmaintAPI, cache: false, data: { PlantID: PlantID } });
        load.done(function (data) {
            if (data.length > 0) {

                $('.dtPrevMaint').DataTable().clear();
                $('.dtPrevMaintSamll').DataTable().clear();
                //insert data 
                $.each(data, function (i, item) {
                    $('.dtPrevMaint').DataTable().row.add([

                      item.Status,
                      item.Status,
                      item.MachineID,
                      item.Description,
                      item.MonthlyLastPmDate,
                      item.MonthlyNextPmDue,
                      item.QuarterlyLastPmDate,
                      item.QuarterlyNextPmDue,
                      item.AnnualLastPmDate,
                      item.AnnualNextPmDue,
                      formatMoney(item.TotalCost),
                      formatDecimal(item.SumDowntime, 1)
                    ]).draw();
                });
                $.each(data, function (i, item) {
                    $('.dtPrevMaintSamll').DataTable().row.add([

                      item.Status,
                      item.Status,
                      item.MachineID,
                      item.Description,
                      item.AnnualLastPmDate,
                      item.AnnualNextPmDue,
                      formatMoney(item.TotalCost),
                      formatDecimal(item.SumDowntime, 1)
                    ]).draw();
                });

            }
            else {

                $('.dtPrevMaint').dataTable().fnClearTable();
                $('.dtPrevMaintSamll').dataTable().fnClearTable();
            }

        });


    }
}