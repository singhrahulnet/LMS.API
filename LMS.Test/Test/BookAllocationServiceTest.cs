using LMS.Domain;
using LMS.Model;
using LMS.Persistence;
using LMS.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace LMS.Test.Test
{
    [TestClass]
    public class BookAllocationServiceTest
    {
        Random rnd;
        Mock<ITransactionManager> moqTxnMgr;
        [TestInitialize]
        public void Start()
        {
            rnd = new Random();
            moqTxnMgr = new Mock<ITransactionManager>();
            moqTxnMgr.Setup(m => m.Create<IssuedBook>().Get()).Returns(TestData.IssuedBooks);
        }
        [TestCleanup]
        public void Cleanup()
        {
            rnd = null;
            moqTxnMgr = null;
        }
        [TestMethod]
        public void issue_book_to_student_does_issue_book_to_the_student()
        {
            //given
            int randomIssuedBookId = rnd.Next(TestData.IssuedBooks.Count+1, TestData.Books.Count);
            int bookId = TestData.Books.Where(s => s.BookId == randomIssuedBookId).First().BookId;
            var InputBookToBeIssued = TestData.Books.Where(b => b.BookId == bookId).FirstOrDefault();
            int randomStudentId = rnd.Next(1, TestData.Students.Count);
            var InputStudent = TestData.Students.Where(s => s.StudentId == randomStudentId).First();


            moqTxnMgr.Setup(m => m.Create<IssuedBook>().Add(It.IsAny<IssuedBook>())).Verifiable();
            moqTxnMgr.Setup(m => m.Save()).Returns(1);
            var sut = new BookAllocationService(moqTxnMgr.Object);

            //when
            var result = sut.IssueBook(InputStudent, InputBookToBeIssued);

            //then
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsTrue(result);
            moqTxnMgr.Verify(v => v.Create<IssuedBook>().Get(), Times.Once);
            moqTxnMgr.Verify(v => v.Create<IssuedBook>().Add(It.IsAny<IssuedBook>()), Times.Once);
            moqTxnMgr.Verify(v => v.Save(), Times.Once);
        }
        [TestMethod]
        public void extend_return_date_of_book_extends_the_return_date_of_the_book()
        {
            //given
            int randomIssuedBookId = rnd.Next(1,TestData.IssuedBooks.Count);
            int InputBookId = TestData.IssuedBooks.Where(i => i.IssuedBookId == randomIssuedBookId).First().BookId;
            Book InputBook = TestData.Books.Where(b => b.BookId == InputBookId).First();
            moqTxnMgr.Setup(m => m.Create<IssuedBook>().Update(It.IsAny<IssuedBook>())).Verifiable();

            var sut = new BookAllocationService(moqTxnMgr.Object);

            //when
            var result = sut.ExtendReturndate(InputBook, 5);

            //then
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsTrue(result);
            moqTxnMgr.Verify(v => v.Create<IssuedBook>().Get(), Times.Once);
            moqTxnMgr.Verify(v => v.Create<IssuedBook>().Update(It.IsAny<IssuedBook>()), Times.Once);
        }
    }
}
