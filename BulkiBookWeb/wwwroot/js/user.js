
var userDataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    userDataTable = $("#tblData").DataTable({
        ajax: {
            url: '/admin/user/getall',
            type: 'GET'
        },

        columns: [
            {
                data: 'name',
                width: "15%"
            },
            {
                data: 'email',
                width: "15%"
            },
            {
                data: 'phoneNumber',
                width: "15%"
            },
            {
                data: 'role',
                width: "10%",
                render: function (data) {
                    return `<span class="badge bg-secondary">${data ?? 'No Role'}</span>`;
                }
            },
            {
                data: null,
                width: "20%",
                orderable: false,
                searchable: false,

                render: function (data, type, row) {

                    var today = new Date().getTime();

                    var lockout = row.lockoutEnd
                        ? new Date(row.lockoutEnd).getTime()
                        : 0;

                    var isLocked = lockout > today;

                    return `
                        <div class="d-flex justify-content-end gap-2">

                            <button type="button"
                                onclick="LockUnLock('${row.id}')"
                                class="btn btn-sm btn-outline-${isLocked ? 'danger' : 'success'}">

                                <i class="bi bi-${isLocked ? 'lock' : 'unlock'}"></i>
                                ${isLocked ? 'Lock' : 'UnLock'}

                            </button>

                            <a href="/admin/user/RoleManagement?userId=${encodeURIComponent(row.id)}"
                               class="btn btn-sm btn-outline-secondary">

                                <i class="bi bi-person-circle"></i>
                                Role

                            </a>

                            <a href="/admin/user/ChangePassword?userId=${encodeURIComponent(row.id)}"
                               class="btn btn-sm btn-outline-danger">

                                <i class="bi bi-key"></i>
                                Password

                            </a>

                        </div>
                    `;
                }
            }
        ]
    });
}

function LockUnLock(id) {

    $.ajax({
        type: "POST",
        url: '/admin/user/lockunlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },

        success: function (data) {

            if (data.success) {
                toastr.success(data.message);
                userDataTable.ajax.reload(null, false);
            }
            else {
                toastr.error(data.message);
            }
        },

        error: function (xhr) {
            toastr.error("Something went wrong. Please try again.");
            console.error(xhr.responseText);
        }
    });
}
