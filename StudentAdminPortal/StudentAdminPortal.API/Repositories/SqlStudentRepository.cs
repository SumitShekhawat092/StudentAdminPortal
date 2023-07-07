using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext _context;

        public SqlStudentRepository(StudentAdminContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Student.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }
        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await _context.Student
                .Include(nameof(Gender))
                .Include(nameof(Address))
                .FirstOrDefaultAsync(x => x.Id == studentId);
        }

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await _context.Gender.ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid studentId)
        {
            return await _context.Student.AnyAsync(x => x.Id == studentId);
        }

        public async Task<Student> UpdateStudentAsync(Guid studentId, Student request)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            { 
                existingStudent.FirstName= request.FirstName;
                existingStudent.LastName= request.LastName;
                existingStudent.DateOfBirth= request.DateOfBirth;
                existingStudent.Email= request.Email;
                existingStudent.Mobile= request.Mobile;
                existingStudent.GenderId= request.GenderId;
                existingStudent.Address.PhysicalAddress= request.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress= request.Address.PostalAddress;

                await _context.SaveChangesAsync();
                return existingStudent;
            }

            return null;
        }

        public async Task<Student> DeleteStudentAsync(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);
            if (student != null)
            { 
                _context.Student.Remove(student);
                await _context.SaveChangesAsync();
                return student;
            }
            return null;
        }

        public async Task<Student> AddStudentAsync(Student request)
        {
            var newStudent = await _context.Student.AddAsync(request);
            await _context.SaveChangesAsync();
            return newStudent.Entity;
        }
    }
}
