// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

import { API_BASE_URL, STATUS } from './utils/constants.js'

export function onLoadingPage(value = false) {
    $('#app-loading').css({
        display: value ? 'block' : 'none'
    })
}

export function formatNumber(number) {
    return new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    }).format(number);
}

export function callApi(url, method, data, successCallback, errorCallback, completeCallback) {
    $.ajax({
        url: API_BASE_URL + url,
        type: method,
        data: method === 'GET' ? data : JSON.stringify(data), // Only send body data for non-GET requests
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            if (typeof successCallback === 'function') {
                successCallback(response);
            }
        },
        error: function (xhr, status, error) {
            if (typeof errorCallback === 'function') {
                errorCallback(xhr, status, error);
            }
        },
        complete: function (xhr, status) {
            if (typeof completeCallback === 'function') {
                completeCallback(xhr, status);
            }
        }
    });
}


const Toast = Swal.mixin({
    toast: true,
    position: "top-end",
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.onmouseenter = Swal.stopTimer;
        toast.onmouseleave = Swal.resumeTimer;
    }
});

export function toastMessage(type, messsage) {
    Toast.fire({
        icon: type,
        title: messsage
    });
}

export function formatDate(value) {
    const date = new Date(value);

    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${hours}:${minutes}:${seconds} ${day}/${month}/${year}`;
}

export function customizeSelectStatus(element) {
    const statuses = [
        { value: 'ACTIVE', text: STATUS.ACTIVE },
        { value: 'INACTIVE', text: STATUS.IN_ACTIVE },
    ];

    $(element).empty();
    statuses.forEach(status => {
        $(element).append(
            $('<option></option>').val(status.value).text(status.text)
        );
    })
}

export function buildParamSearchUrl(params) {
    if (!params) {
        return '';
    }

    const queryString = Object.keys(params)
        .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(params[key])}`)
        .join('&');

    return queryString;
}
