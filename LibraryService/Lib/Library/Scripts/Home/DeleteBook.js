function deleteBook(bookId, elementOnClick) {
    var request = {};
    request.Id = bookId;
    var methodName = 'DeleteBook';
    CallLoadModel(methodName, JSON.stringify(request),
       function (result) {
         $(elementOnClick).parent().parent().remove();

       });
}



function CallLoadModel(methodName, args, onsucces, onerror) {
    $.ajax({
        url: '/Home/' + methodName,
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
