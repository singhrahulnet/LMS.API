using LMS.Model;
using LMS.Persistence;
using System;

namespace LMS.Domain
{
    public interface IStudentService
    {
        Student GetStudent(int studentId);
    }
    public class StudentService : IStudentService
    {
        ITransactionManager _mgr;

        public StudentService(ITransactionManager mgr)
        {
            _mgr = mgr;
        }
        public Student GetStudent(int studentId)
        {
            if(studentId==0)
                throw new Exception("no studentId found");

            var student = _mgr.Create<Student>().GetById(studentId);
            if (student == null)
                throw new Exception("Student with ID '" + studentId + "' not found");
            return student;
        }
    }
}
