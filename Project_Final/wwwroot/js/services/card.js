import { callApi, onLoadingPage, toastMessage, formatNumber } from '../site.js'
import { SERVICE_CODE_SUCCESS, TIME_OUT_API, MESSAGE, PAGE_URL } from '../utils/constants.js';

const PREFIX_API_CARD = '/card'
const PREFIX_API_PRODUCT = '/product'
const PREFIX_API_BILL = '/bill'

var idSelected = "";
var cardOrderDetails = [];

export function selectedCard(id) {
    if (idSelected == id) {
        return;
    }
    getCardInfo(id);
    idSelected = id;

    $('#card-' + id).tab('show');
}

export function createCard() {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API_CARD}/create`, "POST", 
            {
                "receiverName": null,
                "receiverAddress": null,
                "phoneNumber": null,
                "cardOrderDetails": [],
            },
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    toastMessage("success", MESSAGE.SUCCESS.CREATE)
                    setTimeout(() => {
                        window.location.reload();
                    }, 1500)
                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message);
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

export function getCardInfo(id) {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API_CARD}/${id}`, "GET", null,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    cardOrderDetails = resp?.data?.cardOrderDetails || [];
                    viewDataInfo(resp?.data);
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


export function updateData(msg = null) {
    onLoadingPage(true);
    const data = {
        id: $('#card-info #id').val(),
        phoneNumber: $('#card-info #phoneNumber').val(),
        receiverName: $('#card-info #receiverName').val(),
        receiverAddress: $('#card-info #receiverAddress').val(),
        createdAt: $('#card-info #createdAt').val(),
        cardOrderDetails: cardOrderDetails,
    }

    setTimeout(() => {
        callApi(`${PREFIX_API_CARD}/${data.id}`, "PUT", data,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    toastMessage("success", msg ? msg : MESSAGE.SUCCESS.UPDATE)
                    setTimeout(() => {
                        getCardInfo(data.id);
                    }, 1000);
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

export function getProduct() {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API_PRODUCT}`, "GET", 'status=ACTIVE',
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    var tableBody = $('#tbl-products tbody');
                    tableBody.empty(); // Xóa dữ liệu cũ trước khi thêm dữ liệu mới

                    resp.data.forEach(function (product) {
                        var row = $('<tr></tr>');

                        var nameCell = $('<td></td>');
                        nameCell.html(`
                          <div class="px-2">
                            <h6 class="mb-0 text-sm ${product.status === 'ACTIVE' ? '' : 'text-danger'}">${product.name}</h6>
                            <div class="d-flex justify-content-between text-sm">
                                <span>${formatNumber(product.quantity)}</span>
                                <span>${formatNumber(product.price)}</span>
                            </div>
                          </div>
                        `);
                        row.append(nameCell);

                        var actionCell = $('<td class="align-middle text-center text-lg"></td>');
                        actionCell.html(`
                            <i class="fas fa-shopping-cart text-sm ms-auto text-success cursor-pointer" 
                               data-bs-toggle="tooltip" data-bs-placement="top" title="Add to Cart"
                               data-product='${JSON.stringify(product).replace(/'/g, "&apos;")}' 
                               onclick="onAddToCard(this)"></i>
                        `);
                        row.append(actionCell);


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

export function addToCard(productStr) {
    var product = JSON.parse($(productStr).attr('data-product'));
    const containsId = cardOrderDetails.some(card => card.productId === product.id);

    if (containsId) {
        let index = cardOrderDetails.findIndex(card => card.productId === product.id);
        let quantity = cardOrderDetails[index].quantity;
        cardOrderDetails[index].quantity = quantity += 1;
    } else {
        cardOrderDetails.push({
            productId: product.id,
            productName: product.name,
            price: product.price,
            quantity: 1,
        });
    }

    updateData('Thêm vào giỏ hàng thành công');
}

export function deleteCard() {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API_CARD}/${idSelected}`, "DELETE", null,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    toastMessage("success", MESSAGE.SUCCESS.DELETE)
                    setTimeout(() => {
                        window.location.reload();
                    }, 1500)
                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message);
                }
            },
            function (error) {
                toastMessage("error", MESSAGE.ERROR.DELETE)
            },
            function () {
                onLoadingPage(false);
            }
        );
    }, TIME_OUT_API)
}

export function deleteProductInCard(id) {
    let index = cardOrderDetails.findIndex(card => card.productId === id);
    if (index >= 0) {
        cardOrderDetails.splice(index, 1);
        updateData('Xóa thành công');
    }
}

export function editQuantity(id, quantity) {
    let index = cardOrderDetails.findIndex(card => card.productId === id);
    if (index >= 0) {
        cardOrderDetails[index].quantity = quantity;
        updateData();
    }
}

export function createBill() {
    onLoadingPage(true);
    setTimeout(() => {
        callApi(`${PREFIX_API_BILL}/create/${idSelected}`, "POST", null,
            function (resp) {
                if (resp?.meta?.code === SERVICE_CODE_SUCCESS) {
                    toastMessage("success", "Thanh toán thành công")
                    setTimeout(() => {
                        window.location.href = PAGE_URL.BILL.INDEX
                    }, 1500)
                } else if (resp?.meta?.message) {
                    toastMessage("error", resp?.meta?.message);
                }
            },
            function (error) {
                toastMessage("error", "Thanh toán thất bại")
            },
            function () {
                onLoadingPage(false);
            }
        );
    }, TIME_OUT_API)
}

function viewDataInfo(data) {
    $('#card-info #receiverName').val(data?.receiverName);
    $('#card-info #receiverAddress').val(data?.receiverAddress);
    $('#card-info #phoneNumber').val(data?.phoneNumber);
    $('#card-info #id').val(data?.id);
    $('#card-info #createdAt').val(data?.createdAt);

    var tableBody = $('#tbl-products-in-card tbody');
    tableBody.empty(); // Xóa dữ liệu cũ trước khi thêm dữ liệu mới
    cardOrderDetails?.forEach(function (product) {
        var row = $('<tr></tr>');

        var nameCell = $('<td></td>');
        nameCell.html('<div class="d-flex px-2"><h6 class="mb-0 text-sm">' + product.productName + '</h6></div>');
        row.append(nameCell);

        var quantityCell = $('<td class="align-middle text-end"></td>');
        quantityCell.html(`<input class="form-control"type="number" min="0" onchange="onEditQuantity(${product.productId}, this)" value="${product.quantity}"/>`);
        row.append(quantityCell);

        var priceCell = $('<td class="align-middle text-end"></td>');
        priceCell.html('<span class="text-sm">' + formatNumber(product.price) + '</span>');
        row.append(priceCell);


        var actionCell = $('<td class="align-middle text-center text-lg"></td>');
        actionCell.html(`
                         <i class="fas fa-trash-alt ms-auto text-danger cursor-pointer" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete Product" onclick="onDeleteProd(${product.productId})"></i></div>
                        `);
        row.append(actionCell);


        tableBody.append(row);
    });
}