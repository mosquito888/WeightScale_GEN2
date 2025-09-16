$(document).ready(function () {

    // DataTable
    $(".datatable").DataTable();

    // Select2
    $(".select2").select2({
        theme: "bootstrap-5",
        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
        placeholder: '-- Please Select One --',
        allowClear: true,
        minimumInputLength: 3
    });
})

function OnlyNumberKey(evt) {
    // Only ASCII character in that range allowed
    var ASCIICode = (evt.which) ? evt.which : evt.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57)) {
        return false;
    } else {
        return true;
    }
}

function validateDateFromTo(dateFromVal, dateToVal) {
    if (dateFromVal.val() == '' || dateFromVal.val() == undefined) {
        alertMessage("Please input DateFrom");
        return false;
    }
    if (dateToVal.val() == '' || dateToVal.val() == undefined) {
        alertMessage("Please input DateTo");
        return false;
    }
    var D_datefrom = dateFromVal.val().split("/");
    var D_dateto = dateToVal.val().split("/");
    var cus_Date_from = Date.parse(D_datefrom[1] + "/" + D_datefrom[0] + "/" + D_datefrom[2]);
    var cus_Date_to = Date.parse(D_dateto[1] + "/" + D_dateto[0] + "/" + D_dateto[2]);
    if (cus_Date_from > cus_Date_to) {
        alertMessage("Incorrect date from less then date to");
        return false;
    }
    return true;
}

function MoneyOperator(firstVal, secondVal, operator) {

    if (operator == '+') {
        var Total = parseFloat(firstVal.replace(/,/g, '')) + parseFloat(secondVal.replace(/,/g, ''));
    }
    if (operator == '-') {
        var Total = parseFloat(firstVal.replace(/,/g, '')) - parseFloat(secondVal.replace(/,/g, ''));
    }
    var result = moneyFormatVal(Total.toString());
    return result;
}

function moneyFormatVal(Val) {

    if (Val != null) {
        var toReturn = parseFloat(Val.replace(/,/g, ""))
            .toFixed(2)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",");

        if (isNaN(parseFloat(Val.replace(/,/g, "")))) {
            var toReturn = 0.00;
        }
        Val = toReturn;
    } else {
        var toReturn = 0.00;
    }
    return toReturn;
}

function validateTextFromTo(idDateFrom, idDateTo) {
    var objFrom = document.getElementById(idDateFrom);
    var objTo = document.getElementById(idDateTo);
    if (objFrom.value == '') {
        alertMessage("Please input Days From");
        return false;
    }
    if (objTo.value == '') {
        alertMessage("Please input Days To");
        return false;
    }
    if (Number(objFrom.value) > Number(objTo.value)) {
        alertMessage("Incorrect Days From");
        return false;
    }
    return true;
}

// #region Modal
function modalGET(caption, controller, action, isFull) {
    var url = root + controller + '/' + action;
    $.get(url, function (result) {

        $('#modalDialog > .modal-dialog > .modal-content > .modal-body').html(result);
        showModal(caption, isFull);
    });
}

function modalGET_Id(caption, controller, action, id, isFull) {
    var url = root + controller + '/' + action + '/' + id;
    $.get(url, function (result) {
        $('#modalDialog > .modal-dialog > .modal-content > .modal-body').html(result);
        showModal(caption, isFull);
    });
}

function modalGET_Args(caption, controller, action, args, isFull) {
    var url = root + controller + '/' + action + '?' + args;
    $.get(url, function (result) {
        $('#modalDialog > .modal-dialog > .modal-content > .modal-body').html(result);
        showModal(caption, isFull);
    });
}

function modalPOST(caption, controller, action, data, isFull, med) {
    var url = root + controller + '/' + action;
    $.post(url, data, function (result) {
        $('#modalDialog > .modal-dialog > .modal-content > .modal-body').html(result);
        showModal(caption, isFull, med);
    });
}

function modalPOSTLv2(caption, controller, action, data, isFull, med) {
    var url = root + controller + '/' + action;
    $.post(url, data, function (result) {
        $('#modalDialogLv2 > .modal-dialog > .modal-content > .modal-body').html(result);
        showModalLv2(caption, isFull, med);
    });
}

function modalPOSTLv3(caption, controller, action, data, isFull, med) {
    var url = root + controller + '/' + action;
    $.post(url, data, function (result) {
        $('#modalDialogLv3 > .modal-dialog > .modal-content > .modal-body').html(result);
        showModalLv3(caption, isFull, med);
    });
}

function showModal(caption, isFull, med) {
    $('#modalDialog > .modal-dialog').removeClass('modal-20');
    $('#modalDialog > .modal-dialog').removeClass('modal-30');
    $('#modalDialog > .modal-dialog').removeClass('modal-40');
    $('#modalDialog > .modal-dialog').removeClass('modal-50');
    $('#modalDialog > .modal-dialog').removeClass('modal-55');
    $('#modalDialog > .modal-dialog').removeClass('modal-60');
    $('#modalDialog > .modal-dialog').removeClass('modal-70');
    $('#modalDialog > .modal-dialog').removeClass('modal-80');
    $('#modalDialog > .modal-dialog').removeClass('modal-100');
    if (isFull === true) {
        $('#modalDialog > .modal-dialog').addClass('modal-100');
    } else {
        var x = med;
        switch (x) {
            case (20):
                $('#modalDialog > .modal-dialog').addClass('modal-20');
                break;
            case (30):
                $('#modalDialog > .modal-dialog').addClass('modal-30');
                break;
            case (40):
                $('#modalDialog > .modal-dialog').addClass('modal-40');
                break;
            case (50):
                $('#modalDialog > .modal-dialog').addClass('modal-50');
                break;
            case (55):
                $('#modalDialog > .modal-dialog').addClass('modal-55');
                break;
            case (60):
                $('#modalDialog > .modal-dialog').addClass('modal-60');
                break;
            case (70):
                $('#modalDialog > .modal-dialog').addClass('modal-70');
                break;
            case (80):
                $('#modalDialog > .modal-dialog').addClass('modal-80');
                break;
            case (90):
                $('#modalDialog > .modal-dialog').addClass('modal-90');
                break;
            case (100):
                $('#modalDialog > .modal-dialog').addClass('modal-100');
                break;
            default:
                $('#modalDialog > .modal-dialog').addClass('modal-50');
                break;
        }
    }
    $('#modalDialog > .modal-dialog > .modal-content > .modal-header > .modal-title').text(caption);
    $('#modalDialog').modal('show');
}

function showModalLv2(caption, isFull, med) {
    $('#modalDialogLv2 > .modal-dialog').removeClass('modal-20');
    $('#modalDialogLv2 > .modal-dialog').removeClass('modal-30');
    $('#modalDialogLv2 > .modal-dialog').removeClass('modal-40');
    $('#modalDialogLv2 > .modal-dialog').removeClass('modal-50');
    $('#modalDialogLv2 > .modal-dialog').removeClass('modal-55');
    $('#modalDialogLv2 > .modal-dialog').removeClass('modal-60');
    $('#modalDialogLv2 > .modal-dialog').removeClass('modal-70');
    $('#modalDialogLv2 > .modal-dialog').removeClass('modal-80');
    $('#modalDialogLv2 > .modal-dialog').removeClass('modal-100');
    if (isFull === true) {
        $('#modalDialogLv2 > .modal-dialog').addClass('modal-100');
    } else {
        var x = med;
        switch (x) {
            case (20):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-20');
                break;
            case (30):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-30');
                break;
            case (40):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-40');
                break;
            case (50):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-50');
                break;
            case (55):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-55');
                break;
            case (60):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-60');
                break;
            case (70):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-70');
                break;
            case (80):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-80');
                break;
            case (90):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-90');
                break;
            case (100):
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-100');
                break;
            default:
                $('#modalDialogLv2 > .modal-dialog').addClass('modal-50');
                break;
        }
    }
    $('#modalDialogLv2 > .modal-dialog > .modal-content > .modal-header > .modal-title').text(caption);
    $('#modalDialogLv2').modal('show');
}

function showModalLv3(caption, isFull, med) {
    $('#modalDialogLv3 > .modal-dialog').removeClass('modal-20');
    $('#modalDialogLv3 > .modal-dialog').removeClass('modal-30');
    $('#modalDialogLv3 > .modal-dialog').removeClass('modal-40');
    $('#modalDialogLv3 > .modal-dialog').removeClass('modal-50');
    $('#modalDialogLv3 > .modal-dialog').removeClass('modal-55');
    $('#modalDialogLv3 > .modal-dialog').removeClass('modal-60');
    $('#modalDialogLv3 > .modal-dialog').removeClass('modal-70');
    $('#modalDialogLv3 > .modal-dialog').removeClass('modal-80');
    $('#modalDialogLv3 > .modal-dialog').removeClass('modal-100');
    if (isFull === true) {
        $('#modalDialogLv3 > .modal-dialog').addClass('modal-100');
    } else {
        var x = med;
        switch (x) {
            case (20):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-20');
                break;
            case (30):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-30');
                break;
            case (40):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-40');
                break;
            case (50):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-50');
                break;
            case (55):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-55');
                break;
            case (60):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-60');
                break;
            case (70):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-70');
                break;
            case (80):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-80');
                break;
            case (90):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-90');
                break;
            case (100):
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-100');
                break;
            default:
                $('#modalDialogLv3 > .modal-dialog').addClass('modal-50');
                break;
        }
    }
    $('#modalDialogLv3 > .modal-dialog > .modal-content > .modal-header > .modal-title').text(caption);
    $('#modalDialogLv3').modal('show');
}

function clearModal() {
    $('#modalDialog > .modal-dialog > .modal-content > .modal-body').html('');
    $('#modalDialog > .modal-dialog > .modal-content > .modal-header > .modal-title').text('');
    $('#modalDialog').modal('hide');
}

function clearModalLv2() {
    $('#modalDialogLv2 > .modal-dialog > .modal-content > .modal-body').html('');
    $('#modalDialogLv2 > .modal-dialog > .modal-content > .modal-header > .modal-title').text('');
    $('#modalDialogLv2').modal('hide');
}

function clearModalLv3() {
    $('#modalDialogLv3 > .modal-dialog > .modal-content > .modal-body').html('');
    $('#modalDialogLv3 > .modal-dialog > .modal-content > .modal-header > .modal-title').text('');
    $('#modalDialogLv3').modal('hide');
}
// #endregion Modal