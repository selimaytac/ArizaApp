const DataTableTurkishUrl = "//cdn.datatables.net/plug-ins/1.10.19/i18n/Turkish.json"
// Every table can customizable.
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
        dom: 'Bfrtip',
        buttons: [
            'copy', 'excel', 'pdf', 'print'
        ],
        columnDefs: [{
            target: '_all', className: 'text-center'
        }],
        responsive: true
    });
    
    $('#getNotifications').DataTable({
        "language": {
            "url": DataTableTurkishUrl,
            "paginate": {
                "previous": "<",
                "next": ">"
            },
        },
        dom: 'Bfrtip',
        buttons: [
            'copy', 'excel', 'pdf', 'print'
        ],
        columnDefs: [{
            target: '_all', className: 'text-center'
        }],
        responsive: true,
        scrollX:        true,
        scrollCollapse: true,
        paging:         true,
        columnDefs: [
            { width: '20%', targets: 0 }
        ],
        fixedColumns: true
    }).columns.adjust().draw();
    
    $('#getDepartments').DataTable({
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
