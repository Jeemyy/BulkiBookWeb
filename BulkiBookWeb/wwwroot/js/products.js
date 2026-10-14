new DataTable('#tblData', {
    ajax: '/admin/product/getall',
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
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-small btn-outline-success">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="/admin/product/delete?id=${data}" class="btn btn-small btn-outline-danger">
                                <i class="bi bi-pencil-trash"></i> Delete
                            </a>
                        </div> `
            }
        }
    ]
} ); 
