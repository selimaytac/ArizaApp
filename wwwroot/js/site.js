// For CreateNotification.cshtml
$('#FaultType').change(function () {
    if ($(this).val() == "Planlı Çalışma") {
        ChangePlanliCalisma();
    } else {
        changeAriza();
    }
});

$('#FaultType').val() == "Planlı Çalışma" ? ChangePlanliCalisma() : changeAriza();

function ChangePlanliCalisma() {
    $('#AlarmStatus').parent().hide();
    $('#ApprovedBy').parent().hide()
    $('input[name="ApprovedBy"]').val('-');
    $('#AffectedFirms').parent().hide()
    $('input[name="AffectedFirms"]').val('-');
    $('#FailureCase').parent().hide();
    $('#State').parent().hide();
}

function changeAriza() {
    $('#AlarmStatus').parent().show();
    $('#ApprovedBy').parent().show();
    $('#AffectedFirms').parent().show();
    $('#FailureCase').parent().show();
    $('#State').parent().show();
}


$('#State').change(function () {
    if ($(this).val() == "Başladı") {
        $('#EndDate').parent().hide();
    } else {
        $('#EndDate').parent().show();
    }
});