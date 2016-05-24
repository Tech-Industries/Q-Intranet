//   _____ _       _           _    __      __                              _                                     
//  / ____| |     | |         | |   \ \    / /                             | |       /\                             
// | |  __| | ___ | |__   __ _| |    \ \  / /_ _ _ __ ___    __ _ _ __   __| |      /  \   _ __ _ __ __ _ _   _ ___ 
// | | |_ | |/ _ \| '_ \ / _` | |     \ \/ / _` | '__/ __|  / _` | '_ \ / _` |     / /\ \ | '__| '__/ _` | | | / __|
// | |__| | | (_) | |_) | (_| | |      \  / (_| | |  \__ \ | (_| | | | | (_| |    / ____ \| |  | | | (_| | |_| \__ \
//  \_____|_|\___/|_.__/ \__,_|_|       \/ \__,_|_|  |___/  \__,_|_| |_|\__,_|   /_/    \_\_|  |_|  \__,_|\__, |___/
//                                                                                                     __/ |    
//                                                                                                    |___/ 
var monthShort = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
var monthLong = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

//            _____ _____     _    _      _                     
//      /\   |  __ \_   _|   | |  | |    | |                    
//     /  \  | |__) || |     | |__| | ___| |_ __   ___ _ __ ___ 
//    / /\ \ |  ___/ | |     |  __  |/ _ \ | '_ \ / _ \ '__/ __|
//   / ____ \| |    _| |_    | |  | |  __/ | |_) |  __/ |  \__ \
//  /_/    \_\_|   |_____|   |_|  |_|\___|_| .__/ \___|_|  |___/
//                                         | |                  
//                                         |_|   

function setCookies() {
    var homeLink = $("#setCookiesLink").attr("href");
    var plantName = $("#plantSelect").val();
    var month = $("#monthSelect").val();
    var year = $("#yearSelect").val();
    var load = $.ajax({ type: "GET", url: homeLink, cache: false, data: { plantName: plantName, month: month, year: year } });
    load.done(function () {
    });
    load.fail(function () {
    });
    load.always(function () {
    });

}

$(".change-bind").change(function () {
    setCookies();
});


//   _____ _                           _     __  __             _             _       _                 
//  / ____| |                         | |   |  \/  |           (_)           | |     | |                
// | |    | |__  _ __ ___  _ __   __ _| |   | \  / | __ _ _ __  _ _ __  _   _| | __ _| |_ ___  _ __ ___ 
// | |    | '_ \| '__/ _ \| '_ \ / _` | |   | |\/| |/ _` | '_ \| | '_ \| | | | |/ _` | __/ _ \| '__/ __|
// | |____| | | | | | (_) | | | | (_| | |   | |  | | (_| | | | | | |_) | |_| | | (_| | || (_) | |  \__ \
//  \_____|_| |_|_|  \___/|_| |_|\__,_|_|   |_|  |_|\__,_|_| |_|_| .__/ \__,_|_|\__,_|\__\___/|_|  |___/
//                                                              | |                                    
//                                                              |_|            

function incrDay(year, month, day, norp) {
    year = parseInt(year);
    month = parseInt(month);
    day = parseInt(day);
    if (norp == 'next') {
        var dim = getDaysInMonth(month, year);
        
        day += 1;
        if (day > dim) {
            day -= dim;
            month += 1;
            if (month > 12) {
                month -= 12;
                year += 1;
            }

        }
        day = checkZero(day);
        month = checkZero(month);
        return year + ' ' + month + ' ' + day;
    }
    else if (norp == 'prev') {

        day -= 1;
        if (day < 1) {
            month -= 1;
            if (month < 1) {
                month += 12;
                year -= 1;
            }

            var dim = getDaysInMonth(month, year);
            day = dim;

        }
        day = checkZero(day);
        month = checkZero(month);
        return year + ' ' + month + ' ' + day;
    }
}

//  _   _                 _                   ______                         _   _   _             
// | \ | |               | |                 |  ____|                       | | | | (_)            
// |  \| |_   _ _ __ ___ | |__   ___ _ __    | |__ ___  _ __ _ __ ___   __ _| |_| |_ _ _ __   __ _ 
// | . ` | | | | '_ ` _ \| '_ \ / _ \ '__|   |  __/ _ \| '__| '_ ` _ \ / _` | __| __| | '_ \ / _` |
// | |\  | |_| | | | | | | |_) |  __/ |      | | | (_) | |  | | | | | | (_| | |_| |_| | | | | (_| |
// |_| \_|\__,_|_| |_| |_|_.__/ \___|_|      |_|  \___/|_|  |_| |_| |_|\__,_|\__|\__|_|_| |_|\__, |
//                                                                                            __/ |
//                                                                                           |___/

function checkZero(i) {
    if (i < 10) {
        i = '0' + i;
    }
    return i;
}

function formatDecimal(amount, precision) {
    if (amount == null) {
        return '0.0';
    }
    else {
        return parseFloat(amount, 10).toFixed(precision).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
    }
}

function formatMoney(amount) {
    if (amount == null) {
        return '$0.00';
    }
    else {
        return "$" + parseFloat(amount, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
    }
}

function formatMoneyNoDec(amount) {
    if (amount == null) {
        return '$0';
    }
    else {
        return "$" + parseFloat(amount, 10).toFixed().replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
    }
}

function formatPercent(amount) {
    
    if (amount == null) {
        return '0.00%';
    }
    else {
        return parseFloat(amount, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString() + "%";
    }
}


//  _____        _           ______                         _   _   _             
// |  __ \      | |         |  ____|                       | | | | (_)            
// | |  | | __ _| |_ ___    | |__ ___  _ __ _ __ ___   __ _| |_| |_ _ _ __   __ _ 
// | |  | |/ _` | __/ _ \   |  __/ _ \| '__| '_ ` _ \ / _` | __| __| | '_ \ / _` |
// | |__| | (_| | ||  __/   | | | (_) | |  | | | | | | (_| | |_| |_| | | | | (_| |
// |_____/ \__,_|\__\___|   |_|  \___/|_|  |_| |_| |_|\__,_|\__|\__|_|_| |_|\__, |
//                                                                           __/ |
//                                                                          |___/ 
function formatShortDate(date) {
    var y = date.substring(0, 4);
    var m = date.substring(4, 6);
    var d = date.substring(6, 8);
    return m + "/" + d + "/" + y;
}

function getDaysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}

function addYearsToDate(date, years) {

    var s = date.split('T');
    var d = s[0].split('-');
    var formDate = d[1] + "/" + d[2] + "/" + d[0]

    if (/^\d{2}\/\d{2}\/\d{4}$/i.test(formDate)) {

        var parts = formDate.split("/");


        var month = parts[0] && parseInt(parts[0], 10);
        var day = parts[1] && parseInt(parts[1], 10);
        var year = parts[2] && parseInt(parts[2], 10);
        var duration = parseInt(years, 10);

        if (day <= 31 && day >= 1 && month <= 12 && month >= 1) {

            var expiryDate = new Date(year, month - 1, day);
            expiryDate.setFullYear(expiryDate.getFullYear() + duration);

            var day = ('0' + expiryDate.getDate()).slice(-2);
            var month = ('0' + (expiryDate.getMonth() + 1)).slice(-2);
            var year = expiryDate.getFullYear();

            return (month + "/" + day + "/" + year);

        } else {
            // display error message
        }
    }
    else {
        return 'fail';
    }
}

function formatSqlDateTime(ts) {
    if (ts == null) {
        return null;
    }
    else {
        return ts.split("T")[0] + " @ " + ts.split("T")[1]
    }
}

function formatSqlDateTimeToShortDate(ts) {
    var date = ts.split("T")[0];
    var y = date.substring(0, 4);
    var m = date.substring(5, 7);
    var d = date.substring(8, 10);
    return m + "/" + d + "/" + y;

}


function DateNowFormatted() {
    var d = new Date;
    return d.getFullYear() + "-" + ('0' + (d.getMonth() + 1)).slice(-2) + "-" + ('0' + d.getDate()).slice(-2) + " " + ('0' + (d.getHours() + 1)).slice(-2) + ":" + ('0' + (d.getMinutes() + 1)).slice(-2) + ":" + ('0' + (d.getSeconds() + 1)).slice(-2);
}

function GetDate() {
    var d = new Date;
    return d.getFullYear() + "-" + ('0' + (d.getMonth() + 1)).slice(-2) + "-" + ('0' + d.getDate()).slice(-2);
}

function formatShortDate(date) {
    return date.split('/')[2] + "-" + date.split('/')[0] + "-" + date.split('/')[1];
}
function formatShortDateNoSlash(date) {
    return date.substring(0, 4) + "-" + date.substring(4, 6) + "-" + date.substring(6, 8);
}

function MonthString(month, length) {
    if (length == 'short') {
        return monthShort[month - 1];
    }
    else if (length == 'long') {
        return monthLong[month - 1];
    }
}

//  _____        _           _____      _        _                 _ 
// |  __ \      | |         |  __ \    | |      (_)               | |
// | |  | | __ _| |_ ___    | |__) |___| |_ _ __ _  _____   ____ _| |
// | |  | |/ _` | __/ _ \   |  _  // _ \ __| '__| |/ _ \ \ / / _` | |
// | |__| | (_| | ||  __/   | | \ \  __/ |_| |  | |  __/\ V / (_| | |
// |_____/ \__,_|\__\___|   |_|  \_\___|\__|_|  |_|\___| \_/ \__,_|_|

function getWeekNumber(datedue) {
    // Copy date so don't modify original
    dateParts = datedue.split('-');
    d = new Date(dateParts[0], dateParts[1], dateParts[2]);
    d.setHours(0, 0, 0);
    // Set to nearest Thursday: current date + 4 - current day number
    // Make Sunday's day number 7
    d.setDate(d.getDate() + 4 - (d.getDay() || 7));
    // Get first day of year
    var yearStart = new Date(d.getFullYear(), 0, 1);
    // Calculate full weeks to nearest Thursday
    var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7);
    // Return array of year and week number
    return weekNo;
}

function getMonday(datedue) {
    dateParts = datedue.split('-');
    var curr = new Date(datedue); // get current date
    var first = curr.getDate() - (curr.getDay()-1); // First day is the day of the month - the day of the week
    var last = first + 6; // last day is the first day + 6

    var firstday = new Date(curr.setDate(first));
    var lastday = new Date(curr.setDate(last)).toUTCString();
    year = firstday.getFullYear();
    month = firstday.getMonth()+1;
    if (month < 10) { month = "0" + month.toString() }
    day = firstday.getDate();
    if (day < 10) { day = "0" + day.toString() }
    return year.toString()+"-"+month.toString()+"-"+day.toString();
}

function findPeriodVal(Frequency, DateDue) {
    var value;
    console.log(DateDue);
    if (Frequency == 'Weekly') {
        value = getMonday(DateDue);

    }
    else if (Frequency == 'Monthly') {
        value = DateDue.split('-')[1].toString();
       

    }
    else if (Frequency == 'Quarterly') {
        month = DateDue.split('-')[1];
        if (month <= 3) {
            value = "Q1";
        }
        else if (mont <= 6) {
            value = "Q2";
        }
        else if (mont <= 9) {
            value = "Q3";
        }
        else if (mont <= 12) {
            value = "Q4";
        }

    }
    else if (Frequency == 'Semi-Annual') {
        month = DateDue.split('-')[1];
        if (month <= 6) {
            value = "S1";
        }
        else if (mont <= 12) {
            value = "S2";
        }
    }
    else if (Frequency == 'Annual') {

    }

    return value;

}

function getMonthNamesInRange(Month, Range) {

    var toReturn = [];

    for (i = 1; i <= Range; i += 1) {
        Month -= 1;
        if (Month < 0) { Month += 12; }
        toReturn.push(monthLong[Month]);
    }
    return toReturn.reverse();
}

//   _____                                     _       
//  / ____|                                   | |      
// | |     ___  _ __ ___  _ __ ___   ___ _ __ | |_ ___ 
// | |    / _ \| '_ ` _ \| '_ ` _ \ / _ \ '_ \| __/ __|
// | |___| (_) | | | | | | | | | | |  __/ | | | |_\__ \
//  \_____\___/|_| |_| |_|_| |_| |_|\___|_| |_|\__|___/

function loadComments(TypeID, Type, API, self) {
    self.Comments([]);
    var load = $.ajax({ type: "GET", url: API, cache: false, data: { ID: TypeID, Type: Type } });
    load.done(function (data) {
        data.forEach(function (item) {
            item.TimeSubmitted = formatSqlDateTime(item.TimeSubmitted);
        });
        self.Comments(data)
    });
};

function addComment(TypeID, UserID, Comment, Type, API, self) {
    var TimeSubmitted = DateNowFormatted();
    var DDComment = "TypeID: " + TypeID + ", UserID: " + UserID + ", TimeSubmitted: " + TimeSubmitted + ", CommentText: " + Comment + ", Type: "+ Type;
    var sendData = "TypeID=" + TypeID + "&UserID=" + UserID + "&TimeSubmitted=" + TimeSubmitted + "&CommentText=" + Comment + "&Type=" + Type;    
    var addComment = $.ajax({type: 'POST', url: API, data: sendData });
    addComment.done(function (item) {
        item.FirstName = $("#user-full-name").html().split(" ")[0];
        item.LastName = $("#user-full-name").html().split(" ")[1];
        item.TimeSubmitted = formatSqlDateTime(item.TimeSubmitted);
        self.Comments.push(item);
    });
    

}
function updateComment(ID, TimeSubmitted, Comment, AllComments, API) {

    var c = $.grep(AllComments, function (e) { return e.ID == ID })[0];
    c.TimeSubmitted = TimeSubmitted;
    c.CommentText = Comment;
    var update = $.ajax({ type: "PUT", url: API + "/" + ID, cache: false, data: c });
    update.done(function (data) {
    });
}
function removeComment(ID, API, self) {
    var del = $.ajax({ type: 'DELETE', url: API + '/' + ID, contentType: 'application/json' });
    del.done(function (data) {
        
    });
    var remove = $.grep(self.Comments(), function (e) { return e.ID == ID })[0];
    self.Comments.remove(remove);
}



//  _______        _       ______                         _   _   _             
// |__   __|      | |     |  ____|                       | | | | (_)            
//    | | _____  _| |_    | |__ ___  _ __ _ __ ___   __ _| |_| |_ _ _ __   __ _ 
//    | |/ _ \ \/ / __|   |  __/ _ \| '__| '_ ` _ \ / _` | __| __| | '_ \ / _` |
//    | |  __/>  <| |_    | | | (_) | |  | | | | | | (_| | |_| |_| | | | | (_| |
//    |_|\___/_/\_\\__|   |_|  \___/|_|  |_| |_| |_|\__,_|\__|\__|_|_| |_|\__, |
//                                                                        __/ |
//                                                                       |___/ 


function firstLetterCap(str) {
    str = str.toLowerCase().replace(/\b[a-z]/g, function (letter) {
        return letter.toUpperCase();
    });
    return str;
}