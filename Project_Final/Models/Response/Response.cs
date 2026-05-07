using Project_Final.ExceptionHandle;
using Project_Final.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Project_Final.Models.Response
{
    public class Response<T> where T : class
    {
        public T? Data { get; set; }
        public Metadata Meta { get; set; }

        public static Response<T> OfSuccessed()
        {
            return OfSuccessed(null);
        }

        public static Response<T> OfSuccessed(T? data)
        {
            var response = new Response<T>();
            response.Data = data;
            var metadata = new Metadata();
            metadata.Code = Metadata.OK_CODE;
            response.Meta = metadata;
            return response;
        }

        public static Response<T> OfSuccessed(T? data, int page, int size, long totalElement)
        {
            var response = new Response<T>();
            response.Data = data;
            var metadata = new Response<T>.Metadata();
            metadata.Code = Metadata.OK_CODE;
            metadata.Size = size;
            metadata.Page = page;
            metadata.TotalElement = totalElement;
            metadata.TotalPage = (int)Math.Ceiling(totalElement / (double)size);
            metadata.HasPreviousPage = page > 1;
            metadata.HasNextPage = page < metadata.TotalPage;
            response.Meta = metadata;
            return response;
        }

        public static Response<T> OfFailed(BussinessErrorCode errorCode)
        {
            return OfFailed(errorCode, null);
        }

        public static Response<T> OfFailed(BussinessErrorCode errorCode, String message)
        {
            Response<T> response = new Response<T>();
            Metadata metadata = new Metadata();
            metadata.Code = Constant.SERVICE_CODE_PREFIX + errorCode.Code;
            metadata.Message = message != null ? message : errorCode.Message;
            response.Meta = metadata;
            return response;
        }

        public static Response<T> OfFailed(BussinessException e)
        {
            return OfFailed(e.ErrorCode, e.Message);
        }

        public static Response<T> OfFailedFieldValidations(FieldValidationException validationException) 
        {
            Response<T> response = new Response<T>();
            Metadata metadata = new Metadata();
            metadata.Code = Constant.SERVICE_CODE_PREFIX + ErrorCode.INVALID_PARAMETER_ERROR.Code;
            metadata.Message = ErrorCode.INVALID_PARAMETER_ERROR.Message;
            Dictionary<string, string> errors = new Dictionary<string, string>();
            foreach (var item in validationException.FieldValidations)
            {
                errors.Add(item.field, item.description);
            }
            metadata.Errors = errors;
            response.Meta = metadata;
            return response;
        }

        public class Metadata
        {
            public static readonly string OK_CODE = Constant.SERVICE_CODE_PREFIX + 200;
            public string? Code { get; set; }
            public string? Message { get; set; }
            public int Page { get; set; }
            public int Size { get; set; }
            public long TotalElement {  get; set; }
            public int TotalPage {  get; set; }
            public bool HasPreviousPage { get; set; }
            public bool HasNextPage { get; set; }
            public Dictionary<string, string>? Errors { get; set; }
        }
    }
}
