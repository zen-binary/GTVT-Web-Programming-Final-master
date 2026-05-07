// constants.js

export const API_BASE_URL = '/admin/api';
export const WEB_BASE_URL = '/Admin/';
export const SERVICE_CODE_PREFIX = 'UTC-';
export const SERVICE_CODE_SUCCESS = SERVICE_CODE_PREFIX + '200';
export const SERVICE_CODE_INVALID_PARAMETER = SERVICE_CODE_PREFIX + '4000';
export const TIME_OUT_API = 1000
export const TIME_OUT_MESSAGE = 3000
export const MESSAGE = {
    SUCCESS: {
        CREATE: 'Thêm mới thành công.',
        UPDATE: 'Chỉnh sửa thành công.',
        DELETE: 'Xóa thành công.',
    },
    ERROR: {
        CREATE: 'Thêm mới thất bại.',
        UPDATE: 'Chỉnh sửa thất bại.',
        DELETE: 'Xóa thất bại.',
    }
}

export const STATUS = {
    ACTIVE: 'Hoạt động',
    IN_ACTIVE: 'Ngưng hoạt động',
}

export const PAGE_URL = {
    CATEGORY: {
        INDEX: "/Admin/Category",
    },
    PRODUCT: {
        INDEX: "/Admin/Product",
        EDIT: "/Admin/Product/Edit",
    },
    CARD: {
        INDEX: "/Admin/Card",
    },
    BILL: {
        INDEX: "/Admin/Bill",
        DETAIL: "/Admin/Bill/Detail",
    },
}