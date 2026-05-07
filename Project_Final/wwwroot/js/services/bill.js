import { callApi, onLoadingPage, toastMessage, formatNumber, buildParamSearchUrl } from '../site.js'
import { SERVICE_CODE_SUCCESS, TIME_OUT_API, MESSAGE, PAGE_URL } from '../utils/constants.js';

const PREFIX_API_BILL = '/bill'

const pageBill = {
    currentPage: 0,
    totalPage: 0,
    hasPreviousPage: false,
    hasNextPage: false
}
export function getData(param = null) {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API_BILL}`, "GET", param,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    var tableBody = $('#tbl-bills tbody');
                    tableBody.empty(); // Xóa dữ liệu cũ trước khi thêm dữ liệu mới

                    resp?.data?.forEach(function (bill) {
                        var row = $(`<tr ondblclick="getDetails(${bill.id})"></tr>`);

                        var idCell = $('<td></td>');
                        idCell.html(`<div class="d-flex px-2"><h6 class="mb-0 text-sm"><a href="${PAGE_URL.BILL.DETAIL}/${bill.id}">${bill.id}</a></h6></div>`);
                        row.append(idCell);

                        var receiverNameCell = $('<td></td>');
                        receiverNameCell.html(`<div class="d-flex px-2"><h6 class="mb-0 text-sm">${bill?.receiverName || ''}</h6></div>`);
                        row.append(receiverNameCell);

                        var receiverPhoneCell = $('<td></td>');
                        receiverPhoneCell.html(`<div class="d-flex px-2"><h6 class="mb-0 text-sm">${bill?.receiverPhoneNumber || ''}</h6></div>`);
                        row.append(receiverPhoneCell);

                        var receiverAddressCell = $('<td></td>');
                        receiverAddressCell.html(`<div class="d-flex px-2"><h6 class="mb-0 text-sm">${bill?.receiverAddress || ''}</h6></div>`);
                        row.append(receiverAddressCell);

                        var totalAmountCell = $('<td class="text-end"></td>');
                        totalAmountCell.html(`<div class="px-2"><h6 class="mb-0 text-sm">${formatNumber(bill?.totalAmount || 0)}</h6></div>`);
                        row.append(totalAmountCell);

                        tableBody.append(row);
                    });

                    pageBill.currentPage = resp?.meta?.page;
                    pageBill.totalPage = resp?.meta?.totalPage;
                    pageBill.hasPreviousPage = resp?.meta?.hasPreviousPage;
                    pageBill.hasNextPage = resp?.meta?.hasNextPage;
                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message)
                }
            },
            function (error) {
                toastMessage("error", error?.errors?.meta?.message)
            },
            function () {
                onLoadingPage(false);
            }
        );
    }, TIME_OUT_API)
}


export function handlePageBill(type) {
    // Tạo biến để lưu giá trị page cần chuyển tới
    let newPage = pageBill.currentPage;

    // Xử lý theo loại thao tác
    switch (type) {
        case 'next':
            if (pageBill.hasNextPage) {
                newPage++;
            }
            break;
        case 'pre':
            if (pageBill.hasPreviousPage) {
                newPage--;
            }
            break;
        case 'top':
            newPage = 1;
            break;
        case 'end':
            newPage = pageBill.totalPage;
            break;
        default:
            return;
    }

    // Chỉ gọi API nếu trang mới khác trang hiện tại
    if (newPage !== pageBill.currentPage) {
        getData(buildParamSearchUrl({ page: newPage })); // Gọi API với trang mới
    }
}

export function getDetails(id) {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API_BILL}/details/${id}`, "GET", null,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    var tableBody = $('#tbl-bill-details tbody');
                    tableBody.empty(); // Xóa dữ liệu cũ trước khi thêm dữ liệu mới

                    resp?.data?.forEach(function (detail) {
                        var row = $(`<tr></tr>`);

                        var idCell = $('<td></td>');
                        idCell.html(`<div class="d-flex px-2"><h6 class="mb-0 text-sm">${detail.productName}</h6></div>`);
                        row.append(idCell);

                        var receiverNameCell = $('<td class="text-end"></td>');
                        receiverNameCell.html(`<div class="px-2"><h6 class="mb-0 text-sm text-end">${formatNumber(detail.quantity)}</h6></div>`);
                        row.append(receiverNameCell);

                        var receiverPhoneCell = $('<td class="text-end"></td>');
                        receiverPhoneCell.html(`<div class="px-2"><h6 class="mb-0 text-sm">${formatNumber(detail.price)}</h6></div>`);
                        row.append(receiverPhoneCell);

                        var receiverAddressCell = $('<td class="text-end"></td>');
                        receiverAddressCell.html(`<div class="px-2"><h6 class="mb-0 text-sm">${formatNumber(detail.price * detail.quantity)}</h6></div>`);
                        row.append(receiverAddressCell);

                        tableBody.append(row);
                    });
                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message)
                }
            },
            function (error) {
                toastMessage("error", error?.errors?.meta?.message)
            },
            function () {
                onLoadingPage(false);
            }
        );
    }, TIME_OUT_API)
}