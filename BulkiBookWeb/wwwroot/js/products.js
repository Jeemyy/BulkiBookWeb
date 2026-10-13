new DataTable('#tblData', {
    ajax: '/product/getall',
    columns: [
        { data: 'title', width: "25%" },
        { data: 'isbn', width: "20%" },
        { data: 'author', width: "15%" },
        {
            data: 'price', width: "5%", render: function (data)
            { return "$" + data.toFixed(2); }
        },
        {
            data: 'category.name', width: "10%", render: function (data)
            { return '<span class="badge bg-secondary">' + data + '</span>' }
        },
        {
            data: 'id', render: function (data)
            {
                return ` <div class="d-flex justify-content-end gap-2">
                            <a href="/product/edit?id=${data}" class="btn btn-small btn-outline-success">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onclick="Delete('/product/delete/${data}')" class="btn btn-small btn-outline-danger">
                                <i class="bi bi-pencil-trash"></i> Delete
                            </a>
                        </div> `
            }
        }
    ]
} );

function Delete(url) {
    Swal.fire({
  title: "Are you sure?",
  text: "You won't be able to revert this!",
  icon: "warning",
  showCancelButton: true,
  confirmButtonColor: "#3085d6",
  cancelButtonColor: "#d33",
  confirmButtonText: "Yes, delete it!"
}).then((result) => {
  if (result.isConfirmed) 
    $.ajax({
    url : url,
    type: "DELETE",
    success: function (data) {
            Swal.fire({
        title: "Deleted!",
        text: "Your file has been deleted.",
        icon: "success"
      });

    }
    })
});
}