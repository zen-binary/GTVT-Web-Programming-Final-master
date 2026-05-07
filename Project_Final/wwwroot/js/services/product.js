import { callApi, onLoadingPage, formatNumber, toastMessage, buildParamSearchUrl } from '../site.js';
import { SERVICE_CODE_SUCCESS, TIME_OUT_API, MESSAGE, SERVICE_CODE_INVALID_PARAMETER, PAGE_URL } from '../utils/constants.js';

const PREFIX_API = '/product'

const pageProduct = {
    currentPage: 0,
    totalPage: 0,
    hasPreviousPage: false,
    hasNextPage: false
}

export function getData(param = null) {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(PREFIX_API, 'GET', param,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    var tableBody = $('#tbl-products tbody');
                    tableBody.empty(); // Xóa dữ liệu cũ trước khi thêm dữ liệu mới

                    resp.data.forEach(function (product) {
                        var row = $('<tr></tr>');

                        var nameCell = $('<td></td>');
                        nameCell.html('<div class="d-flex px-2"><h6 class="mb-0 text-sm">' + product.name + '</h6></div>');
                        row.append(nameCell);

                        var cateName = $('<td></td>');
                        cateName.html('<div class="d-flex px-2"><h6 class="mb-0 text-sm">' + product.categoryName + '</h6></div>');
                        row.append(cateName);

                        var quantityCell = $('<td class="align-middle text-end"></td>');
                        quantityCell.html('<span class="text-sm">' + formatNumber(product.quantity) + '</span>');
                        row.append(quantityCell);

                        var priceCell = $('<td class="align-middle text-end"></td>');
                        priceCell.html('<span class="text-sm">' + formatNumber(product.price) + '</span>');
                        row.append(priceCell);

                        var statusCell = $('<td class="align-middle text-xs"></td>');
                        statusCell.html(`
                            <span class="badge badge-sm ${product.status == 'ACTIVE' ? 'bg-gradient-success' : 'bg-gradient-danger'}">
                                ${product.status == 'ACTIVE' ? 'Hoạt động' : 'Ngưng hoạt động'}
                            </span>
                        `);
                        row.append(statusCell);

                        var actionCell = $('<td class="align-middle text-center text-lg"></td>');
                        actionCell.html(`
                             <div class="d-flex">
                                <div class="me-5"><i class="fas fa-pencil-alt ms-auto text-success cursor-pointer" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit Product" onclick="onEdit(${product.id})"></i></div>
                            </div>
                        `);
                        row.append(actionCell);


                        tableBody.append(row);
                    });

                    pageProduct.currentPage = resp?.meta?.page;
                    pageProduct.totalPage = resp?.meta?.totalPage;
                    pageProduct.hasPreviousPage = resp?.meta?.hasPreviousPage;
                    pageProduct.hasNextPage = resp?.meta?.hasNextPage;
                }
            },
            function (xhr, status, error) {
                console.error('API call failed:', status, error);
            }, function () {
                onLoadingPage(false);
            }
        )
    }, TIME_OUT_API)
}


export function insertData(data) {
    onLoadingPage(true);
    $('#form-create input').removeClass('is-invalid');
    setTimeout(() => {
        callApi(`${PREFIX_API}`, "POST", data,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    toastMessage("success", MESSAGE.SUCCESS.CREATE)

                    setTimeout(() => {
                        window.location.href = PAGE_URL.PRODUCT.INDEX
                    }, 1500);
                } else if (resp?.meta?.code === SERVICE_CODE_INVALID_PARAMETER) {
                    toastMessage("error", resp?.meta?.message)
                    if (resp?.meta?.errors) {
                        const errorMsg = resp.meta.errors
                        Object.keys(errorMsg).forEach(key => {
                            $('#form-create #' + key).addClass('is-invalid');
                            $('#form-create #error-' + key).text(errorMsg[key]);
                        })
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

export function editData(id) {
    window.location.href = `${PAGE_URL.PRODUCT.EDIT}/${id}`
}

export function updateData() {
    onLoadingPage(true);
    const data = {
        id: $('#form-edit #id').val(),
        quantity: $('#form-edit #quantity').val(),
        price: $('#form-edit #price').val(),
        categoryId: $('#form-edit #categoryId').val(),
        status: $('#form-edit #status').val(),
    }

    setTimeout(() => {
        callApi(`${PREFIX_API}/${data.id}`, "PUT", data,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    toastMessage("success", MESSAGE.SUCCESS.UPDATE)

                    setTimeout(() => {
                        window.location.href = PAGE_URL.PRODUCT.INDEX
                    }, 1500);
                } else if (resp?.meta?.code === SERVICE_CODE_INVALID_PARAMETER) {
                    toastMessage("error", resp?.meta?.message)

                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message)
                }
            },
            function (error) {
                toastMessage("error", MESSAGE.ERROR.UPDATE)
            },
            function () {
                onLoadingPage(false);
            }
        );
    }, TIME_OUT_API)
}


export function deleteData(id) {
    console.log(id);
}

export function handlePageProduct(type) {
    // Tạo biến để lưu giá trị page cần chuyển tới
    let newPage = pageProduct.currentPage;

    // Xử lý theo loại thao tác
    switch (type) {
        case 'next':
            if (pageProduct.hasNextPage) {
                newPage++;
            }
            break;
        case 'pre':
            if (pageProduct.hasPreviousPage) {
                newPage--;
            }
            break;
        case 'top':
            newPage = 1;
            break;
        case 'end':
            newPage = pageProduct.totalPage;
            break;
        default:
            return;
    }

    // Chỉ gọi API nếu trang mới khác trang hiện tại
    if (newPage !== pageProduct.currentPage) {
        getData(buildParamSearchUrl({ page: newPage })); // Gọi API với trang mới
    }
}