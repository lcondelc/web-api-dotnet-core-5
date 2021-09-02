using Demo.Web.API.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Demo.Web.API.Models
{
    public class CourseForUpdateDto : CourseForManipulationDto
    {
        [Required(ErrorMessage = "The Description field is required.")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
