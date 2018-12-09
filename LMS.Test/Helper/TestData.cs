using Bogus;
using LMS.Model;
using System;
using System.Collections.Generic;

namespace LMS.Test.Helper
{
    class TestData
    {
        static int bookCount = 10, studentCount = 5;
        static Random rnd = new Random();

        public static List<Book> Books
        {
            get
            {
                var Id = 1;
                var books = new Faker<Book>()
                    .RuleFor(i => i.BookId, f => Id++)
                    .RuleFor(n => n.Name, f => f.Commerce.ProductName()
                         );
                return books.Generate(bookCount);
            }

        }
        public static List<Student> Students
        {
            get
            {
                var Id = 1;
                var students = new Faker<Student>()
                    .RuleFor(i => i.StudentId, f => Id++)
                    .RuleFor(n => n.Name, f => f.Person.FirstName + ' ' + f.Person.LastName
                    );
                return students.Generate(studentCount);
            }
        }
        public static List<IssuedBook> IssuedBooks
        {
            get
            {
                int days2Return = 10;
                DateTime issueDate = DateTime.Now.AddDays(-10);

                var issuedBookNotOverdue = new IssuedBook()
                {
                    IssuedBookId=1,
                    BookId = 1,
                    StudentId = 1,
                    IssueDate = issueDate,
                    ReturnDate = issueDate.AddDays(days2Return),
                    ReturnDateExtended = false
                };
                issueDate = DateTime.Now.AddDays(-15);
                var issuedBookOverdue = new IssuedBook()
                {
                    IssuedBookId=2,
                    BookId = 2,
                    StudentId = 1,
                    IssueDate = issueDate,
                    ReturnDate = issueDate.AddDays(days2Return),
                    ReturnDateExtended = false,
                };
                issueDate = DateTime.Now.AddDays(-15);
                var issuedBookReturnDateExtended = new IssuedBook()
                {
                    IssuedBookId=3,
                    BookId = 3,
                    StudentId = 2,
                    IssueDate = issueDate,
                    ReturnDate = issueDate.AddDays(2 * days2Return),
                    ReturnDateExtended = true,
                };
                return new List<IssuedBook>() { issuedBookNotOverdue, issuedBookOverdue, issuedBookReturnDateExtended };
            }
        }
    }
}
