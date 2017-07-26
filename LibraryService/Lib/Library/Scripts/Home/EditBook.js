$$r(function () {
    var pageUrl = document.URL;
    var bookId = _GetParametrFromUrl(pageUrl, 'Id');
    editBook(bookId);
    getAutherForBook(bookId);
    
});



function editBook(bookId) {
    var request = {};
    request.Id = bookId;
    var methodName = 'GetBookById';
    CallLoadModel(methodName, JSON.stringify(request),
       function (result) {
           if(result != null && result != ""){
               $('#bookId').val(result.Id);
               $('#NameBook').val(result.NameBook);
               $('#Quantity').val(result.Quantity);
               $('#YearPublish').val(result.YearPublish);
           }
       });
}

function getAutherForBook(bookId) {
    var request = {};
    request.Id = bookId;
    var methodName = 'GetAutherForBook';
    CallLoadModel(methodName, JSON.stringify(request),
       function (result) {
           if (result != null && result != "") {
               var buttonAuthor = document.getElementById('addAuthorButton');

               result.forEach(function (item, i) {
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
                   inputName.value = item.FirstName;




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
                   inputLastName.value = item.LastName;


                   var deleteButton = document.createElement('input');
                   deleteButton.style.marginLeft = '10px';
                   deleteButton.style.display = 'inline-block';
                   deleteButton.className = 'btn btn-default';

                   deleteButton.id = item.Id;

                   deleteButton.type = 'button';
                   deleteButton.value = 'Remove';
                   deleteButton.setAttribute('onclick', 'removeAuthorLine(this)');
                   divLineAuthor.appendChild(labelName);
                   divLineAuthor.appendChild(inputName);
                   divLineAuthor.appendChild(labelLastName);
                   divLineAuthor.appendChild(inputLastName);
                   divLineAuthor.appendChild(deleteButton);
                   $(divLineAuthor).insertBefore($(buttonAuthor));

               });
           }
       });
}

function removeAuthorLine(onClickElement) {
    if (onClickElement.id != '' && onClickElement.id != undefined) {
        var request = {}
        request.Id = onClickElement.id;
        var methodName = 'DeleteAuthorFromBook';
        CallLoadModel(methodName, JSON.stringify(request), function (result) {
            $(onClickElement).parent().remove();
        });
    }
    else {
        $(onClickElement).parent().remove();
    }
}


function updateBooks() {
    var request = {};
    var request2 = {};
    var methodName = "UpdateBook";

    request.Id = $('#bookId').val();
    request.NameBook = $('#NameBook').val();
    request.Quantity = $('#Quantity').val();
    request.YearPublish = $('#YearPublish').val();

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
        if (authors[i].childNodes[4].id != '' && authors[i].childNodes[4].id != undefined && authors[i].childNodes[4].id != null) {
            Author.Id = authors[i].childNodes[4].id;
        }
        else {
            Author.Id = 0;
        }
        request2.push(Author);
        
    }
    if (request.Id == "") {
        return;
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
    CallLoadModel(methodName, JSON.stringify(request) + '&' + JSON.stringify(request2), function (result) {
        var href = "../";
        window.location.reload(href);

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


function _GetParametrFromUrl (locationString, name) {
    name = name.replace(/[\[]/, '\\\[').replace(/[\]]/, '\\\]');
    var regexS = '[\\?&]' + name + '=([^&#]*)';
    var regex = new RegExp(regexS);
    var results = regex.exec(locationString);
    if (results == null)
        return '';
    else
        return decodeURIComponent(results[1].replace(/\+/g, ' '));
}