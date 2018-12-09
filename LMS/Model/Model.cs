using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Model
{
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }
    }
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
    }
    public class IssuedBook
    {
        [Key]
        public int IssuedBookId { get; set; }
        public DateTime IssueDate { get; set; }
        public bool ReturnDateExtended { get; set; }
        public DateTime ReturnDate { get; set; }
        public int BookId { get; set; }
        public int StudentId { get; set; }
    }
}
