using LMS.Domain;
using LMS.Model;
using LMS.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Test
{
    [TestClass]
    public class BookStoreServiceTest
    {
        Mock<IStudentService> moqStudentService;
        Mock<IBookService> moqBookService;
        Mock<IBookAllocationService> moqBookAllocationService;
        Random rnd;
        [TestInitialize]
        public void Start()
        {
            rnd = new Random();
            moqStudentService = new Mock<IStudentService>();
            moqBookService = new Mock<IBookService>();
            moqBookAllocationService = new Mock<IBookAllocationService>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            rnd = null;
            moqStudentService = null;
            moqBookService = null;
            moqBookAllocationService = null;
        }

        [TestMethod]
        public void get_books_returns_all_available_books()
        {
            //given
            moqBookService.Setup(m => m.GetAllBooks()).Returns(TestData.Books);

            var sut = new BookStoreService(moqStudentService.Object, moqBookService.Object, moqBookAllocationService.Object);

            //when
            var result = sut.GetAllBooks();

            //then

            Assert.IsInstanceOfType(result, typeof(IEnumerable<Book>));
            Assert.AreEqual(TestData.Books.Count, result.Count());
            moqBookService.Verify(v => v.GetAllBooks(), Times.Once);
        }
        [TestMethod]
        public void get_overdue_books_returns_all_books_which_are_overdue()
        {
            //given
            var overDueBooksId = TestData.IssuedBooks.Where(i => DateTime.Now.Subtract(i.ReturnDate).Days > 0).Select(s => s.BookId).ToList();
            var overDueBooks = TestData.Books.Where(b => overDueBooksId.Contains(b.BookId));

            moqBookService.Setup(m => m.GetOverdueBooks()).Returns(overDueBooks);
            var sut = new BookStoreService(moqStudentService.Object, moqBookService.Object, moqBookAllocationService.Object);

            //when
            var result = sut.GetOverdueBooks();

            //then
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Book>));
            moqBookService.Verify(v => v.GetOverdueBooks(), Times.Once);

        }
        [TestMethod]
        public void issue_book_issues_book_to_student()
        {
            //given
            int randomStudentId = rnd.Next(1, TestData.Students.Count);
            var student = TestData.Students.Where(s => s.StudentId == randomStudentId).First();
            int randomBookId = rnd.Next(TestData.IssuedBooks.Count + 1, TestData.Books.Count);
            var book = TestData.Books.Where(s => s.BookId == randomBookId).First();
            moqStudentService.Setup(m => m.GetStudent(It.IsAny<int>())).Returns(student);
            moqBookService.Setup(m => m.GetBook(It.IsAny<int>())).Returns(book);
            moqBookAllocationService.Setup(m => m.IssueBook(It.IsAny<Student>(), It.IsAny<Book>())).Returns(true);

            var sut = new BookStoreService(moqStudentService.Object, moqBookService.Object, moqBookAllocationService.Object);

            //when
            var result = sut.IssueBook(student.StudentId, book.BookId);

            //then
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsTrue(result);
            moqStudentService.Verify(v => v.GetStudent(It.IsAny<int>()), Times.Once);
            moqBookService.Verify(v => v.GetBook(It.IsAny<int>()), Times.Once);
            moqBookAllocationService.Verify(v => v.IssueBook(It.IsAny<Student>(), It.IsAny<Book>()), Times.Once);
        }
        [TestMethod]
        public void issue_book_does_not_issue_book_to_student()
        {
            //given
            int randomStudentId = rnd.Next(1, TestData.Students.Count);
            var student = TestData.Students.Where(s => s.StudentId == randomStudentId).First();
            int randomBookId = rnd.Next(1, TestData.IssuedBooks.Count);
            var book = TestData.Books.Where(s => s.BookId == randomBookId).First();
            moqStudentService.Setup(m => m.GetStudent(It.IsAny<int>())).Returns(student);
            moqBookService.Setup(m => m.GetBook(It.IsAny<int>())).Returns(book);
            var sut = new BookStoreService(moqStudentService.Object, moqBookService.Object, moqBookAllocationService.Object);

            //when
            var result = sut.IssueBook(student.StudentId, book.BookId);

            //then
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsFalse(result);
            moqStudentService.Verify(v => v.GetStudent(It.IsAny<int>()), Times.Once);
            moqBookService.Verify(v => v.GetBook(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void extend_date_extends_book_return_date()
        {
            //given
            int randomBookId = rnd.Next(1, TestData.IssuedBooks.Count);
            var issuedBook = TestData.IssuedBooks.Where(s => s.BookId == randomBookId);
            moqBookService.Setup(m => m.GetIssuedBooks()).Returns(issuedBook);
            moqBookAllocationService.Setup(m => m.ExtendReturndate(It.IsAny<IssuedBook>(), It.IsAny<int>())).Returns(true);
            var sut = new BookStoreService(moqStudentService.Object, moqBookService.Object, moqBookAllocationService.Object);

            //when
            var result = sut.ExtendReturnDate(issuedBook.First().BookId, rnd.Next(1, 10));

            //then
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsTrue(result);
            moqBookService.Verify(v => v.GetIssuedBooks(), Times.Once);
            moqBookAllocationService.Verify(v => v.ExtendReturndate(It.IsAny<IssuedBook>(), It.IsAny<int>()), Times.Once);

        }
        [TestMethod]
        public void extend_date_does_not_extends_book_return_date()
        {
            //given
            int randomBookId = rnd.Next(1, TestData.IssuedBooks.Count);
            var issuedBook = TestData.IssuedBooks.Where(s => s.BookId == randomBookId);
            moqBookService.Setup(m => m.GetIssuedBooks()).Returns(issuedBook);

            moqBookAllocationService.Setup(m => m.ExtendReturndate(It.IsAny<IssuedBook>(), It.IsAny<int>())).Returns(false);
            var sut = new BookStoreService(moqStudentService.Object, moqBookService.Object, moqBookAllocationService.Object);

            //when
            var result = sut.ExtendReturnDate(issuedBook.First().BookId, rnd.Next(1, 10));

            //then
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsFalse(result);
            moqBookService.Verify(v => v.GetIssuedBooks(), Times.Once);
            moqBookAllocationService.Verify(v => v.ExtendReturndate(It.IsAny<IssuedBook>(), It.IsAny<int>()), Times.Once);
        }
    }
}
