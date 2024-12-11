namespace GameZone.Attributes
{
    public class AllowedExtensionAttribute:ValidationAttribute
    {
        private readonly string allowedExtensions;

        public AllowedExtensionAttribute(string allowedExtensions)
        {
            this.allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
            
            var file = value as IFormFile;
            if (file != null) 
            {
                var extension = Path.GetExtension(file.FileName);
                var isAllowed = allowedExtensions.Split(separator: ',').Contains(extension, StringComparer.OrdinalIgnoreCase);
                if (!isAllowed)
                {
                     return new ValidationResult(errorMessage: $"Only {allowedExtensions} are allowed!");
                }
            }
            return ValidationResult.Success;
        }
    }
}
