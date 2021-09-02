using Demo.Web.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Demo.Web.API.Models.Base
{
    [CourseTitleMustBeDifferentFromDescriptionAttribute(ErrorMessage = "The provided description should be different from the title")]

    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "The Title field is required.")]
        [MaxLength(100, ErrorMessage = "The field Title must be a string type with a maximum length of '100'.")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "The field Description must be a string type with a maximum length of '500'.")]

        public virtual string Description { get; set; }
    }
}
