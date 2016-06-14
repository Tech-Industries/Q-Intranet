
$(document).on('change', '#frm-doc-up', function () {
    vm.addDelDetDocument();
    vm.loadDelDetDocuments();
});

$(document).on('click', '.btn-sign-review', function () {
    vm.addDelDetReview($(this).attr("UserID"));
    var parent = $(this).parent();

    $(this).parent().parent().removeClass("un-rev");
    $(this).parent().parent().addClass("rev");
    parent.html("<i class='fa fa-check primary' style='color: #1ab394'></i>");
});
var txtBox;
var bit = 0;
$(document).on('click', '.edit-comment', function () {
    var e = $(this);//.parent().parent().parent();
    id = e.attr('commentid');
    userid = e.attr('userid');

    comLabel = $(".comment-item[commentID='" + id + "']").find('.comment-text');
    txtBox = $(".comment-item[commentID='" + id + "']").find('.txt-comment-edit');
    comTime = $(".comment-item[commentID='" + id + "']").find('.comment-time');
    edit = $(".comment-item[commentID='" + id + "']").find('.edit-comment');
    save = $(".comment-item[commentID='" + id + "']").find('.save-comment');
    save.removeClass("hidden");
    edit.addClass("hidden");
    comLabel.addClass("hidden");
    txtBox.removeClass("hidden");
    txtBox.focus();

    txtBox.unbind().on("focusout", function () {
        edit.removeClass("hidden");
        save.addClass("hidden");
        updatedText = txtBox.val();
        comLabel.html(updatedText);
        txtBox.addClass("hidden");
        comLabel.removeClass("hidden");
        updatedTime = DateNowFormatted();
        comTime.html(updatedTime.split(" ")[0] + " @@ " + updatedTime.split(" ")[1]);

        vm.updateComment(id, updatedTime, updatedText);

    });



});



$(document).on('click', '.remove-comment', function () {
    var id = $(this).attr('commentid');
    swal({
        title: "Delete?",
        text: "Are you sure you want to delete this comment?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-primary",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
    function () {
        vm.removeComment(id);
        swal({
            title: "Deleted!",
            type: "success",
            timer: 1000
        });
    });

});

$(document).on('click', '.remove-document', function () {
    var id = $(this).attr('docid');
    var path = $(this).attr('path');
    swal({
        title: "Delete?",
        text: "Are you sure you want to delete this file?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-primary",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
    function () {
        vm.removeDocument(id, path);
        swal({title:"Deleted!",
            type: "success",
            timer: 1000});
    });
});




$(document).on('click', '.btn-complete-del', function () {
    vm.updateDelDet();
    //var parent = $(this).parent();
    //$(this).parent().parent().removeClass("un-rev");
    //$(this).parent().parent().addClass("rev");
    //parent.html("<i class='fa fa-check primary' style='color: #1ab394'></i>");
});

$(document).on('click', '.rev', function () {
});

$(".date-selectable").click(function (item) {
    $(".date-selectable").removeClass("active");
    $(this).addClass("active");
});

$(document).on('click', '.deliverable-type', function (item) {
    $("#del-title").html($(this).children().first().html());
    $(".deliverable-type").removeClass("active");
    $(this).addClass("active");
    $("#DelID").val($(this).attr("delid"));
    var oldFreq = $("#freq").val();
    var newFreq = $(this).attr("freq");
    if (oldFreq != newFreq) {
        $('#freq').val(newFreq).change();
    }

    vm.loadDelDet();
    $('#del-desc').html($(this).attr("desc"));
});

$(document).on('click', "#btn-submit-comment", function () {
    vm.addComment();
});



$("#freq").change(function () {
    var freq = $('#freq').val();

    $('#period').html("<option value='0'>Select a period...</option>")
    $("#period").val("0");
    if (freq == "Weekly") {
        loadWeeks();
        $("#period").val(monYear + "-" + monMonth + "-" + monDay);
        $("#period").removeAttr("disabled");

    }
    else if (freq == "Monthly") {
        $("#period").append("<option value='01'>January</option><option value='02'>February</option><option value='03'>March</option><option value='04'>April</option><option value='05'>May</option><option value='06'>June</option><option value='07'>July</option><option value='08'>August</option><option value='09'>September</option><option value='10'>October</option><option value='11'>November</option><option value='12'>December</option>");
        $("#period").val(curMonth);
        $("#period").removeAttr("disabled");
    }
    else if (freq == "Quarterly") {
        $("#period").append("<option value='Q1'>Q1</option><option value='Q2'>Q2</option><option value='Q3'>Q3</option><option value='Q4'>Q4</option>");
        var q = "";
        if (curMonth <= 3) {
            q = "Q1";
        }
        else if (curMonth <= 6) {
            q = "Q2";
        }
        else if (curMonth <= 9) {
            q = "Q3";
        }
        else {
            q = "Q4";
        }
        $("#period").val(q);
        $("#period").removeAttr("disabled");
    }
    else if (freq == "Semi-Annual") {
        $("#period").append("<option value='S1'>Jan - June</option><option value='S2'>July - Dec</option>")
        var p = 0;
        if (curMonth <= 6) {
            p = "S1";
        }
        else {
            p = "S2";
        }
        $("#period").val(p);
        $("#period").removeAttr("disabled");
    }
    else if (freq == "Annual") {
        $("#period").attr("disabled", "disabled")
    }

});

$("#year").change(function () {
    $('#freq').val() == "Weekly" ? loadWeeks() : vm.loadDelDet();
});

$("#period").change(function () {
    vm.loadDelDet();
});


function loadWeeks() {
    $("#period").html("");
    var ycheck = $('#year').val();
    var date = new Date(ycheck + "-01-07T12:00:00");
    var first = date.getDate() - (date.getDay() - 1);
    var firstDay = new Date(date.setDate(first));
    var i = 1;
    while (firstDay <= new Date(ycheck + "-12-31T12:00:00")) {
        var sday = firstDay.getDate();
        if (sday < 10) {
            sday = "0" + sday;
        }
        var smonth = firstDay.getMonth() + 1;
        if (smonth < 10) {
            smonth = "0" + smonth;
        }
        var syear = firstDay.getFullYear();

        var weekStart = smonth + "/" + sday;
        var dincr = firstDay.getDate() + 4;
        var wincr = firstDay.getDate() + 7;
        var lastDay = new Date(firstDay);
        lastDay = new Date(lastDay.setDate(dincr));
        var lday = lastDay.getDate();
        if (lday < 10) {
            lday = "0" + lday;
        }
        var lmonth = lastDay.getMonth() + 1;
        if (lmonth < 10) {
            lmonth = "0" + lmonth;
        }
        var lyear = lastDay.getFullYear();
        var weekEnd = lmonth + "/" + lday;
        $('#period').append("<option value='" + syear + "-" + smonth + "-" + sday + "'>" + weekStart + " - " + weekEnd + "</option>");
        firstDay = new Date(firstDay.setDate(wincr));
        i += 1;
    }
}

$(document).on("click", "#toggle-add-reviewer", function () {
    $('#add-rev-slide').toggle("slide", {direction: "up"}, 500)
});

$(document).on("click", '#slideup-add-reviewer', function () {
    $('#add-rev-slide').hide("slide", { direction: "up" }, 500);
});

$(document).on("click", "#add-reviewer", function () {
    var DelID = $('#DelID').val();
    var UserID = $('#sel-add-reviewer').val();
    if (DelID != null) {
        if (UserID != 0) {
            vm.addDelReviewer(UserID);
        }
    }
    
});