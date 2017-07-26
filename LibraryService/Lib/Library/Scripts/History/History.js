$$r(function () {
    getListBooks();
    getListUser();
    getDateOfIssue();
});


function getListBooks() {
    var select = document.getElementById('selectBook');
    var methodName = 'GetListBooks';
    CallLoadModel(methodName, null, function (result) {
        if (result.length != 0) {
            var option;
            result.forEach(function (item, i) {
                option = document.createElement('option');
                option.value = item.Id;
                option.textContent = item.NameBook;
                select.appendChild(option);
            });
        }
    });
}


function getListUser() {
    var select = document.getElementById('selectUser');
    var methodName = 'GetLstUsers';
    CallLoadModel(methodName, null, function (result) {
        if (result.length != 0) {
            var option;
            result.forEach(function (item, i) {
                option = document.createElement('option');
                option.value = item.Id;
                option.textContent = item.FirstName + ' ' + item.LastName;
                select.appendChild(option);
            });
        }
    });
}

function getDateOfIssue() {
    var inputDateOfIssue = document.getElementById('DateOfIssue');
    var methodName = 'GetDateOfIssue';
    CallLoadModel(methodName, null, function (result) {
        inputDateOfIssue.value = result;
        inputDateOfIssue.setAttribute('disabled');
    });
}

function setNewHistory() {
    var methodName = 'AddNewHistory';
    var request = {};
    request.BookId = $('#selectBook').val();
    request.UserId = $('#selectUser').val();
    request.DateOfIssue = $('#DateOfIssue').val();
    request.DateReturn = $('#DateReturn').val();
    if (request.BookId == 0 || request.UserId == 0) {
        return;
    }
    if (request.DateReturn == '') {
        return;
    }
    CallLoadModel(methodName, JSON.stringify(request), function (result) {
        var href = "../";
        window.location.reload(href);
    });
}

function CallLoadModel(methodName, args, onsucces, onerror) {
    $.ajax({
        url: '/History/' + methodName,
        type: "POST",
        async: true,
        cache: false,
        dataType: "json",
        data: "{'args':'" + args + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            onsucces(result);
        },
        error: function (e) {
            onerror(e);
        }
    });
}

