using Demo.Web.API.Models;
using Demo.Web.API.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Demo.Web.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseForManipulationDto)validationContext.ObjectInstance;

            if (course.Title == course.Description)
            {
                return new ValidationResult(
                   ErrorMessage,
                    new[] { "CourseForCreationDTO" });
            }
            return ValidationResult.Success;
        }
    }
}