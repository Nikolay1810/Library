$$r(function () {
    
    getAllBook();
});


function getAllBook() {
    var request = {};
    var Id = $('#selectShow').val();
    var methodName;
    var args;
    if (Id != '0') {
        methodName = 'GetOtherBooks';
        args = Id;
    }
    else {
        methodName = 'GetAllBooks';
        args = null;
    }
    var tbody = document.getElementById('contentBooks');
    CallLoadModel(methodName, args, function (result) {
        $('#contentBooks').children().remove();

        if (result.length != 0) {
            result.forEach(function (item, i) {
                var tr = document.createElement('tr');
                var tdName = document.createElement('td');
                tdName.textContent = item.NameBook;

                var tdQuantity = document.createElement('td');
                tdQuantity.textContent = item.Quantity;

                var tdYear = document.createElement('td');
                tdYear.textContent = item.YearPublish;

                var tdEdit = document.createElement('td');
                var tdDelete = document.createElement('td');

                var aEdit = document.createElement('a');
                aEdit.href = '../Home/Edit?&Id=' + item.Id;
                aEdit.style.cursor = 'pointer';
                aEdit.textContent = 'Edit';

                var aDelete = document.createElement('a');
                aDelete.style.cursor = 'pointer';
                aDelete.setAttribute('onclick', 'deleteBook(' + item.Id + ', this)');
                aDelete.textContent = 'Delete';

                tdEdit.appendChild(aEdit);
                tdDelete.appendChild(aDelete);

                tr.appendChild(tdName);
                tr.appendChild(tdQuantity);
                tr.appendChild(tdYear);
                tr.appendChild(tdEdit);
                tr.appendChild(tdDelete);
               
                tbody.appendChild(tr);
            });
        }
    });

}

function sendMail() {
    var methodName = "SendMail";

    var messegeDiv = document.createElement('div');
    var home = document.getElementById('Home')
    messegeDiv.id = 'animateMessage';
    messegeDiv.style.width = '0px';
    messegeDiv.style.height = '0px';
    messegeDiv.style.backgroundColor = 'blue';
    messegeDiv.style.position = 'absolute';
    messegeDiv.style.color = 'white';
    messegeDiv.style.left = '40%';
    messegeDiv.style.top = '40%';
    messegeDiv.textContent = 'Successfully';
    messegeDiv.style.textAlign = 'center';
    messegeDiv.style.lineHeight = '40px';
    messegeDiv.style.borderRadius = '30%';
    messegeDiv.style.fontSize = '24px';
    messegeDiv.style.opacity = '0.5';

    CallLoadModel(methodName, null, function (result) {

        home.appendChild(messegeDiv);
        $('#animateMessage').animate({ width: '150px', height: '40px' }, 700);
        setTimeout(function () {
            $('#animateMessage').animate({ width: '0px', height: '0px' }, 700);
        }, 700);

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