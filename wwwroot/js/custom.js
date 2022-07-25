const DataTableTurkishUrl = "//cdn.datatables.net/plug-ins/1.10.19/i18n/Turkish.json"

$(document).ready(function () {
    $('#getUsers').DataTable({
        "language": {
            "url": DataTableTurkishUrl,
            "paginate": {
                "previous": "<",
                "next": ">"
            }
        },
        columnDefs: [{
            target: '_all', className: 'text-center'
        }],
        responsive: true
    });
    $('#getFirms').DataTable({
        "language": {
            "url": DataTableTurkishUrl,
            "paginate": {
                "previous": "<",
                "next": ">"
            }
        },
        columnDefs: [{
            target: '_all', className: 'text-center'
        }],
        responsive: true
    });
    $('#getEmails').DataTable({
        "language": {
            "url": DataTableTurkishUrl,
            "paginate": {
                "previous": "<",
                "next": ">"
            }
        },
        columnDefs: [{
            target: '_all', className: 'text-center'
        }],
        responsive: true
    });
});
