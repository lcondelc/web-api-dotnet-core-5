using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Demo.Web.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Demo.Web.API.Controllers
{
    [Route("api/author/{authorId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
                return NotFound();

            var courses = _courseLibraryRepository.GetCourses(authorId);

            return Ok(_mapper.Map<IEnumerable<CourseDto>>(courses));
        }

        [HttpGet]
        [Route("{courseId}", Name = "GetCoursesForAuthor")]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId, Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
                return NotFound();

            var course = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (course is null)
                return NotFound();

            return Ok(_mapper.Map<CourseDto>(course));
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseForCreationDto course)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
                return NotFound();

            var newCourse = _mapper.Map<Course>(course);
            _courseLibraryRepository.AddCourse(authorId, newCourse);
            _courseLibraryRepository.Save();

            var courseToReturn = _mapper.Map<CourseDto>(newCourse);
            return CreatedAtRoute("GetCoursesForAuthor", new { authorId = authorId, courseId = newCourse.Id }, courseToReturn);
        }

        [HttpPut("{courseId}")]
        public ActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, [FromBody] CourseForUpdateDto course)
        {
            var authorCourse = _courseLibraryRepository.GetCourse(authorId, courseId);
            if (authorCourse is null)
                return NotFound();

            _mapper.Map(course, authorCourse);

            _courseLibraryRepository.UpdateCourse(authorCourse);
            _courseLibraryRepository.Save();

            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdateCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> course)
        {
            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthorFromRepo is null)
                return NotFound();

            var courseToPath = _mapper.Map<CourseForUpdateDto>(courseForAuthorFromRepo);

            course.ApplyTo(courseToPath);

            if (!TryValidateModel(courseToPath))
                return ValidationProblem(ModelState);

            _mapper.Map(courseToPath, courseForAuthorFromRepo);

            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);
            _courseLibraryRepository.Save();

            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public ActionResult SeleteCourseForAuthor(Guid authorId, Guid courseId)
        {
            var authorCourse = _courseLibraryRepository.GetCourse(authorId, courseId);
            if (authorCourse is null)
                return NotFound();

            _courseLibraryRepository.DeleteCourse(authorCourse);
            _courseLibraryRepository.Save();

            return NoContent();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}