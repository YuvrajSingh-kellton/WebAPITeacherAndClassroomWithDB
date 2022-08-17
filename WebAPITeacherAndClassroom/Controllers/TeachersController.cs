using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDBLayer;
using WebAPIDBLayer.Models;
using WebAPITeacherAndClassroom.ApiModels;

namespace WebApiWithRelationShipOneToManyAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly DbContextForRelation _context;
        private readonly IMapper _mapper;

        public TeachersController(DbContextForRelation context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            if (_context.Teachers == null)
            {
                return NotFound();
            }


            var obj = _context.Teachers.Include(t => t.ClassroomsList).ToList();
            return Ok(obj);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            if (_context.Teachers == null)
            {
                return NotFound();
            }
            var teacher = _context.Teachers.Where(t => t.TeacherID == id).Include(cls => cls.ClassroomsList).FirstOrDefault();

            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, BindingTeacherAndClassroom teacher)
        {

            var updateObj = _context.Teachers.Where(t => t.TeacherID == id).Include(c => c.ClassroomsList).FirstOrDefault();
            if (updateObj == null)
            {
                return BadRequest();
            }
            var teacherObj = _mapper.Map<Teacher>(teacher.TeacherModel);

            if (id != teacherObj.TeacherID)
            {
                return BadRequest();
            }
            var classObj = _mapper.Map<List<Classroom>>(teacher.ClassroomModelList);

            updateObj.Address = teacherObj.Address;
            updateObj.TeacherName = teacherObj.TeacherName;

            updateObj.ClassroomsList = classObj;

            _context.Update(updateObj);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(BindingTeacherAndClassroom TCObj)
        {
            if (_context.Teachers == null)
            {
                return Problem("Entity set 'DbContextForRelation.teachers'  is null.");
            }
            var teacherobj = _mapper.Map<Teacher>(TCObj.TeacherModel);
            var classroomList = _mapper.Map<List<Classroom>>(TCObj.ClassroomModelList);
            teacherobj.ClassroomsList = classroomList;
            _context.Teachers.Add(teacherobj);
            await _context.SaveChangesAsync();

            return Ok(teacherobj);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            if (_context.Teachers == null)
            {
                return NotFound();
            }
            var teacherDelete = await _context.Teachers.FindAsync(id);
            if (teacherDelete == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacherDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchTeacher(string Name, string Address, int id)
        {
            var teacherNameAndAddressUpdate = _context.Teachers.Where(t => t.TeacherID == id).FirstOrDefault();
            if (teacherNameAndAddressUpdate == null)
            {
                return BadRequest();
            }
            teacherNameAndAddressUpdate.TeacherName = Name;
            teacherNameAndAddressUpdate.Address = Address;
            _context.Update(teacherNameAndAddressUpdate);
            await _context.SaveChangesAsync();
            return Ok("updated  !");
        }

        private bool TeacherExists(int id)
        {
            return (_context.Teachers?.Any(e => e.TeacherID == id)).GetValueOrDefault();
        }
    }
}