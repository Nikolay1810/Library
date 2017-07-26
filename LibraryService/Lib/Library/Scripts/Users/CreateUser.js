function createUser() {
    var methodName = 'CreateUser';
    var request = {}
    request.FirstName = $('#FirstName').val();
    request.LastName = $('#LastName').val();
    request.Mail = $('#Mail').val();
    request.PhoneNumber = $('#PhoneNumber').val();
    if (request.FirstName == '') {
        return;
    }
    if (request.LastName == '') {
        return;
    }
    if (request.Mail == '') {
        return;
    }
    if (request.PhoneNumber == '' || request.PhoneNumber == 0) {
        return;
    }
    CallLoadModel(methodName, JSON.stringify(request),
        function (result) {
        if (result != '') {
            var href = '../';
            window.location.reload(href);
        }
    });
}

function CallLoadModel(methodName, args, onsucces, onerror) {
    $.ajax({
        url: '/User/' + methodName,
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