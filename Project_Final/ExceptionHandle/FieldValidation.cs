namespace Project_Final.ExceptionHandle
{
    public class FieldValidation
    {
        public string field {  get; set; }
        public string description { get; set; }

        public FieldValidation(string field, string description)
        {
            this.field = field;
            this.description = description;
        }
    }
}
