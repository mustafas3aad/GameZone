namespace GameZone.Attributes
{
    public class MaxFileSizeAttribute: ValidationAttribute
    {
        private readonly int maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }

        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
            
            var file = value as IFormFile;
            if (file != null)
            {
                if(file.Length > maxFileSize)
                {
                    return new ValidationResult(errorMessage: $"Maximam Allowed size is {maxFileSize} bytes");
                }
            }
            return ValidationResult.Success;
        }
    }
}
