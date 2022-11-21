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
    $('#AffectedFirms').parent().hide()
    $('#FailureCause').parent().hide();
    $('#State').parent().hide();
    $('#NotifiedBy').parent().find('label').html("Çalışmayı Yapacak Ekip");
    $('#AffectedServices').parent().find('label').html("Çalışma Yapılan Sunucular");
    $('input[name="ApprovedBy"]').val('-');
    $('input[name="AffectedFirms"]').val('-');
    $('input[name="FailureCause"]').val('-');
}

function changeAriza() {
    $('#AlarmStatus').parent().show();
    $('#ApprovedBy').parent().show();
    $('#AffectedFirms').parent().show();
    $('#FailureCause').parent().show();
    $('#State').parent().show();
    $('#NotifiedBy').parent().find('label').html("Problem İle İlgilenen Ekip");
    $('#AffectedServices').parent().find('label').html("Etkilenen Servisler");
}

$('#State').change(function () {
    if ($(this).val() == "Başladı") {
        $('#EndDate').parent().hide();
    } else {
        $('#EndDate').parent().show();
    }
});