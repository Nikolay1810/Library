$$r(function () {
    CreateSelectAuthor();
});


function CreateSelectAuthor() {
    var divAuthor = document.getElementById('selectAuthor');
    var methodName = 'GetAuthorList';
    CallLoadModel(methodName, null, function (result) {
        if (result != null) {
            var divBlock = document.createElement('div');
            divBlock.style.display = 'block';
            var label = document.createElement('lable');
            label.textContent = 'Choose the author';
            label.style.display = 'inline-block';
            label.className = 'control-label col-md-2';
            label.style.fontWeight = 'bold';
            label.style.marginRight = '15px';

            var select = document.createElement('select');
            select.className = 'form-control';
            select.style.display = 'inline-block';
            select.style.width = '18%';
            select.id = 'selectedAuthorId';

            var option = document.createElement('option');
            option.value = 0;
            select.appendChild(option);
            for (var i = 0; i < result.length; i++) {
                option = document.createElement('option');
                option.value = result[i].Id;
                option.textContent = result[i].FirstName + ' ' + result[i].LastName;
                select.appendChild(option);
            }
            divBlock.appendChild(label);
            divBlock.appendChild(select);
            divAuthor.appendChild(divBlock);
        }
    });
}

function addAuthor() {
    var buttonAuthor = document.getElementById('addAuthorButton');
    var divLineAuthor = document.createElement('div');
    divLineAuthor.style.display = 'block';
    divLineAuthor.style.padding = '25px';

    var labelName = document.createElement('label');
    labelName.textContent = 'First name';
    labelName.style.marginLeft = '5px';
    labelName.style.display = 'inline-block';


    var inputName = document.createElement('input');
    inputName.type = 'text';
    inputName.className = 'form-control';
    inputName.style.marginLeft = '5px';
    inputName.style.display = 'inline-block';
    inputName.name = "FirstName";




    var labelLastName = document.createElement('label');
    labelLastName.textContent = 'Last name';
    labelLastName.style.marginLeft = '10px';
    labelLastName.style.display = 'inline-block';


    var inputLastName = document.createElement('input');
    inputLastName.type = 'text';
    inputLastName.className = 'form-control';
    inputLastName.style.marginLeft = '5px';
    inputLastName.style.display = 'inline-block';
    inputLastName.name = "LastName";


    var deleteButton = document.createElement('input');
    deleteButton.style.marginLeft = '10px';
    deleteButton.style.display = 'inline-block';
    deleteButton.className = 'btn btn-default';

    deleteButton.type = 'button';
    deleteButton.value = 'Remove';
    deleteButton.setAttribute('onclick', 'removeAuthorLine(this)');
    divLineAuthor.appendChild(labelName);
    divLineAuthor.appendChild(inputName);
    divLineAuthor.appendChild(labelLastName);
    divLineAuthor.appendChild(inputLastName);
    divLineAuthor.appendChild(deleteButton);
    $(divLineAuthor).insertBefore($(buttonAuthor));
    
}

function removeAuthorLine(onClickElement) {
    $(onClickElement).parent().remove();
}

function addNewBooks() {
    var request = {};
    var request2 = {};
    var methodName = "AddNewBooks";

    request.NameBook = $('#NameBook').val();
    request.Quantity = $('#Quantity').val();
    request.YearPublish = $('#YearPublish').val();
    request.AuthorId = $('#selectedAuthorId').val();

    request2 = [];
    
    var authors = document.querySelectorAll('div[id = author]>div');
    for (var i = 0; i < authors.length; i++) {

        var Author = {};

        if (authors[i].childNodes[1].name == 'FirstName' && authors[i].childNodes[1].value != '') {
            Author.FirstName = authors[i].childNodes[1].value;
        }
        else {
            return;
        }
        if (authors[i].childNodes[3].name == 'LastName' && authors[i].childNodes[3].value != '') {
            Author.LastName = authors[i].childNodes[3].value;
        }
        else {
            return;
        }
        
        request2.push(Author);
        
    }
    if (request.NameBook == "") {
        return;
    }
    if (request.Quantity == "") {
        return;
    }
    if (request.YearPublish == "") {
        return;
    }

    CallLoadModel(methodName, JSON.stringify(request)+'&'+ JSON.stringify(request2), function (result) {
        window.location.href = "/";
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
