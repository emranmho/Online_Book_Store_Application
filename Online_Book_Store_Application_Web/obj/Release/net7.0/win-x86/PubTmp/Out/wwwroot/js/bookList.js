var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Book/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "priceAfterDiscount", "width": "15%" },
            { "data": "bookAuthor.name", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": function (row) {
                    return checkAvailability(row.isAvailable);
                },
                "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `

                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Book/Upsert?id=${data}" class="btn btn-primary mx-2">Edit</a>
                            <a onClick=Delete('/Admin/Book/DeletePost/${data}') class="btn btn-danger mx-2">Delete</a>
                        </div>                           

                           `
                },
                "width": "15%"
            },
            ]
    });
};
function checkAvailability(isAvailable) {
    return isAvailable ? "Yes" : "No";
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
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);                        
                    } else {
                        toastr.error(data.message);
                    }
                }
                })
        }
    })
}
