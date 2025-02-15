var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/home/ticket/getall' },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "status", "width": "15%"},
            {
                data: 'id',
                "render": function (data) {

                    return `<div class="w-75 btn-group" role="group">
                     <a href="/home/ticket/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                    </div>`
                },
                "width": "25%"
            },
            {
                data: 'id',
                "render": function (data, index, row) {
                    if (row.isAdmin) {
                        return `<div class="w-75 btn-group" role="group">
                         <a onClick=Delete('/home/ticket/delete?id=${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                        </div>`
                    }
                    return ''
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}