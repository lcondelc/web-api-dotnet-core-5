using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Demo.Web.API.Helpers;
using Demo.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.Web.API.Controllers
{
    [Route("api/authorcollections")]
    [ApiController]
    public class AuthorCollectionsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        public AuthorCollectionsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
        }

        [HttpGet("{ids}", Name = "GetAuthorCollections")]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthorCollections(
            [FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids is null)
                return BadRequest();

            var authorEntities = _courseLibraryRepository.GetAuthors(ids);

            if (ids.Count() != authorEntities.Count())
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorEntities));
        }


        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authorCollections)
        {
            var authorEntities = _mapper.Map<IEnumerable<Author>>(authorCollections);

            foreach (var author in authorEntities)
            {
                _courseLibraryRepository.AddAuthor(author);
            }
            _courseLibraryRepository.Save();

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            return CreatedAtRoute("GetAuthorCollections", new { ids = string.Join(",", authorEntities.Select(x => x.Id)) }, authorsToReturn);
        }
    }
}
