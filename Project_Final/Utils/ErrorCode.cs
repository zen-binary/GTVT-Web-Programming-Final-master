using Project_Final.ExceptionHandle;

namespace Project_Final.Utils
{
    public class ErrorCode
    {
        public static BussinessErrorCode INVALID_PARAMETER_ERROR = new BussinessErrorCode { Code = 4000, Message = "Invalid parameters", HttpStatus = 400 };
        public static BussinessErrorCode INVALID_ORDER_QUANTITY = new BussinessErrorCode { Code = 4001, Message = "Order quantity can't greate than product quantity", HttpStatus = 400 };
        public static BussinessErrorCode INVALID_CATEGORY_ID = new BussinessErrorCode { Code = 4002, Message = "CategoryId invalid", HttpStatus = 400 };
        public static BussinessErrorCode DUPLICATE_CATEGORY = new BussinessErrorCode { Code = 4003, Message = "Duplicate category", HttpStatus = 400 };
        public static BussinessErrorCode UNAUTHORIZE = new BussinessErrorCode { Code = 4001, Message = "Unauthorize", HttpStatus = 401 };
        public static BussinessErrorCode NOT_FOUND_DATA = new BussinessErrorCode { Code = 4041, Message = "Not found data", HttpStatus = 404 };
        public static BussinessErrorCode INTERNAL_SERVER_ERROR = new BussinessErrorCode { Code = 5000, Message = "Internal server errror", HttpStatus = 500 };
    }
}
