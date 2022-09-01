// For CreateNotification.cshtml
$('#FaultType').change(function () {
    if ($(this).val() == "Planlı Çalışma") {
        $('#AlarmStatus').parent().hide();
        $('#ApprovedBy').parent().hide();
        $('#AffectedFirms').parent().hide();
        $('#FailureCase').parent().hide();
        $('#State').parent().hide();
    } else {
        $('#AlarmStatus').parent().show();
        $('#ApprovedBy').parent().show();
        $('#AffectedFirms').parent().show();
        $('#FailureCase').parent().show();
        $('#State').parent().show();
    }
});

$('#State').change(function () {
    if ($(this).val() == "Başladı") {
        $('#EndDate').parent().hide();
    } else {
        $('#EndDate').parent().show();
    }
});