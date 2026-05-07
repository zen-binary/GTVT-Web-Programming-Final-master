using Project_Final.Utils;

namespace Project_Final.ExceptionHandle
{
    public class FieldValidationException : Exception
    {
        public List<FieldValidation> FieldValidations { get; }

        public FieldValidationException(List<FieldValidation> fieldValidations) : base(ErrorCode.INVALID_PARAMETER_ERROR.Message)
        {
            FieldValidations = fieldValidations;
        }
    }
}
