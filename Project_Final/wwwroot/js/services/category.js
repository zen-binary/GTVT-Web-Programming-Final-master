import { callApi, onLoadingPage, toastMessage, formatDate, customizeSelectStatus, buildParamSearchUrl } from '../site.js'
import { SERVICE_CODE_SUCCESS, TIME_OUT_API, MESSAGE, SERVICE_CODE_INVALID_PARAMETER, STATUS } from '../utils/constants.js';

const PREFIX_API = '/category'

const pageCate = {
    currentPage: 0,
    totalPage: 0,
    hasPreviousPage: false,
    hasNextPage: false
}

var arrNameCreate = [];

function collectPendingCategoryNames() {
    const val = $('#categoryName input').val()?.trim();
    if (val && !arrNameCreate.includes(val)) {
        arrNameCreate.push(val);
        $('#categoryName input').val('');
    }
    return arrNameCreate.length > 0;
}

export function insertData() {
    collectPendingCategoryNames();
    if (arrNameCreate.length === 0) {
        toastMessage("error", "Vui lòng nhập ít nhất một tên danh mục");
        $('#categoryName input').addClass('is-invalid');
        $('#categoryName #errorName').text('Không được phép để trống');
        return;
    }
    onLoadingPage(true);
    $('#categoryName input').removeClass('is-invalid');
    setTimeout(() => {
        callApi(`${PREFIX_API}`, "POST", { names: arrNameCreate },
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    toastMessage("success", MESSAGE.SUCCESS.CREATE)
                    $('#button-container').empty();
                    arrNameCreate = [];

                    setTimeout(() => {
                        getData()
                    }, 1000)
                } else if (resp?.meta?.code === SERVICE_CODE_INVALID_PARAMETER) {
                    toastMessage("error", resp?.meta?.message)
                    if (resp?.meta?.errors.name) {
                        $('#categoryName input').addClass('is-invalid');
                        $('#categoryName #errorName').text(resp?.meta?.errors.name);
                    }
                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message)
                }
            },
            function (error) {
                toastMessage("error", MESSAGE.ERROR.CREATE)
            },
            function () {
                onLoadingPage(false);
            }
        );
    }, TIME_OUT_API)
}

export function getArrNameCreate() {
    return arrNameCreate;
}

export function setArrNameCreate(data) {
    arrNameCreate = data;
}

export function pushArrNameCreate(data) {
    const name = data?.trim();
    if (name) {
        arrNameCreate.push(name);
    }
}

export function spliceArrNameCreate(start, end) {
    arrNameCreate.splice(start, end);
}

export function getData(param = null) {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API}`, "GET", param,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    var tableBody = $('#tbl-categories tbody');
                    tableBody.empty(); // Xóa dữ liệu cũ trước khi thêm dữ liệu mới

                    resp.data.forEach(function (cate) {
                        var row = $('<tr></tr>');

                        var nameCell = $('<td></td>');
                        nameCell.html('<div class="d-flex px-2"><h6 class="mb-0 text-sm">' + cate.name + '</h6></div>');
                        row.append(nameCell);

                        var statusCell = $('<td class="align-middle text-xs"></td>');
                        statusCell.html(`
                            <span class="badge badge-sm ${cate.status == 'ACTIVE' ? 'bg-gradient-success' : 'bg-gradient-danger'}">
                                ${cate.status == 'ACTIVE' ? STATUS.ACTIVE : STATUS.IN_ACTIVE}
                            </span>
                        `);
                        row.append(statusCell);

                        var actionCell = $('<td class="align-middle text-right text-lg"></td>');
                        actionCell.html(`
                            <div class="d-flex">
                                <div class="me-5"><i class="fas fa-pencil-alt ms-auto text-success cursor-pointer" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit Category" onclick="onEdit(${cate.id})"></i></div>
                                <div><i class="fas fa-trash-alt ms-auto text-danger cursor-pointer" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete Category" onclick="onDelete(${cate.id})"></i></div>
                            </div>
                        `);
                        row.append(actionCell);

                        tableBody.append(row);
                    });

                    pageCate.currentPage = resp?.meta?.page;
                    pageCate.totalPage = resp?.meta?.totalPage;
                    pageCate.hasPreviousPage = resp?.meta?.hasPreviousPage;
                    pageCate.hasNextPage = resp?.meta?.hasNextPage;
                }
            }, function () {

            }, function () {
                onLoadingPage(false);
            }
        )
    }, TIME_OUT_API)
}

export function editData(id) {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API}/${id}`, "GET", null,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    if (resp?.data) {
                        customizeDataModalEdit(resp.data);
                        $('#modalEditCate').modal('show');
                    }
                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message)
                }
            }, function () {

            }, function () {
                onLoadingPage(false);
            }
        )
    }, TIME_OUT_API)
}

export function updateData(data) {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API}/${data.id}`, "PUT", data,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    if (resp?.data) {
                        customizeDataModalEdit(resp.data);
                        toastMessage("success", MESSAGE.SUCCESS.UPDATE)
                        $('#modalEditCate .body-data *').removeClass('is-invalid');
                        getData();
                    }
                } else if (resp?.meta?.code === SERVICE_CODE_INVALID_PARAMETER) {
                    toastMessage("error", resp?.meta?.message)
                    if (resp?.meta?.errors) {
                        const errorMsg = resp.meta.errors
                        Object.keys(errorMsg).forEach(key => {
                            $('#modalEditCate .body-data #' + key).addClass('is-invalid');
                            $('#modalEditCate .body-data .error-' + key).text(errorMsg[key]);
                        })
                    }
                    if (resp?.meta?.errors.CategoryName) {
                        $('#categoryName input').addClass('is-invalid');
                        $('#categoryName #errorName').text(resp?.meta?.errors.CategoryName);
                    }
                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message);
                }
            }, function () {
                toastMessage("error", MESSAGE.ERROR.UPDATE);
            }, function () {
                onLoadingPage(false);
            }
        )
    }, TIME_OUT_API)
}

export function deleteData(id) {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API}/${id}`, "DELETE", null,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    toastMessage("success", MESSAGE.SUCCESS.DELETE);
                    setTimeout(() => {
                        getData();
                    }, 1000)
                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message);
                }
            }, function () {
                toastMessage("error", MESSAGE.ERROR.DELETE);
            }, function () {
                onLoadingPage(false);
            }
        )
    }, TIME_OUT_API)
}

export function handlePageCate(type) {
    // Tạo biến để lưu giá trị page cần chuyển tới
    let newPage = pageCate.currentPage;

    // Xử lý theo loại thao tác
    switch (type) {
        case 'next':
            if (pageCate.hasNextPage) {
                newPage++;
            }
            break;
        case 'pre':
            if (pageCate.hasPreviousPage) {
                newPage--;
            }
            break;
        case 'top':
            newPage = 1;
            break;
        case 'end':
            newPage = pageCate.totalPage;
            break;
        default:
            return;
    }

    // Chỉ gọi API nếu trang mới khác trang hiện tại
    if (newPage !== pageCate.currentPage) {
        getData(buildParamSearchUrl({ page: newPage })); // Gọi API với trang mới
    }
}

function customizeDataModalEdit(data) {
    customizeSelectStatus('#modalEditCate .body-data #status')
    Object.keys(data).forEach(key => {
        function formatData(value) {
            if (['createdAt', 'lastModifiedAt'].includes(key)) {
                return formatDate(value);
            } else {
                return value
            }
        }
        $('#modalEditCate .body-data #' + key).val(formatData(data[key]));
    })
} 