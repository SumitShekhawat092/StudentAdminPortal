using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        public readonly IMapper _mapper;
        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var student = await _studentRepository.GetStudentsAsync();
            return Ok(_mapper.Map<List<StudentDTO>>(student));
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}"), ActionName("GetStudentAsync")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            // fatch
            var student = await _studentRepository.GetStudentAsync(studentId);

            if (student == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<StudentDTO>(student));
        }

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await _studentRepository.ExistsAsync(studentId))
            {
                //update details here

                var updateStudent = await _studentRepository.UpdateStudentAsync(studentId, _mapper.Map<Student>(request));
                if (updateStudent == null)
                {
                    return Ok();
                }
                return Ok(_mapper.Map<StudentDTO>(updateStudent));

            }
            return NotFound();
        }
        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await _studentRepository.ExistsAsync(studentId))
            {
                //delete record here
                var deletedStudent = await _studentRepository.DeleteStudentAsync(studentId);

                return Ok(_mapper.Map<StudentDTO>(deletedStudent));
            }
            return NotFound();
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
            var newStudent = await _studentRepository.AddStudentAsync(_mapper.Map<Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = newStudent.Id}, _mapper.Map<StudentDTO>(newStudent));
        }
    }
}
