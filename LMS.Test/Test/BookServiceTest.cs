using LMS.Domain;
using LMS.Helper;
using LMS.Model;
using LMS.Persistence;
using LMS.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Test.Test
{
    [TestClass]
    public class BookServiceTest
    {
        Random rnd;
        Mock<ITransactionManager> moqTxnMgr;
        [TestInitialize]
        public void Start()
        {
            rnd = new Random();
            moqTxnMgr = new Mock<ITransactionManager>();
        }
        [TestCleanup]
        public void Cleanup()
        {
            rnd = null;
            moqTxnMgr = null;
        }
        [TestMethod]
        public void get_all_books_returns_all_books()
        {
            //given
            moqTxnMgr.Setup(m => m.Create<Book>().Get()).Returns(TestData.Books);
            var sut = new BookService(moqTxnMgr.Object);

            //when
            var result = sut.GetAllBooks();

            //then
            Assert.IsInstanceOfType(result,typeof( IEnumerable<Book>));
            Assert.AreEqual(TestData.Books.Count, result.Count());
            moqTxnMgr.Verify(v => v.Create<Book>().Get(), Times.Once);
        }
        [TestMethod]
        public void get_book_of_supplied_bookId_returns_the_book_with_the_given_id()
        {
            //given
            int randomBookId = rnd.Next(1, TestData.Books.Count);
            Book book = TestData.Books.Where(s => s.BookId == randomBookId).First();
            int bookId = book.BookId;
            moqTxnMgr.Setup(m => m.Create<Book>().GetById(It.IsAny<int>())).Returns(book);
            var sut = new BookService(moqTxnMgr.Object);

            //when
            var result = sut.GetBook(bookId);

            //then
            Assert.IsInstanceOfType(result, typeof(Book));
            Assert.IsNotNull(result);
            moqTxnMgr.Verify(v => v.Create<Book>().GetById(It.IsAny<int>()), Times.Once);
        }
        [TestMethod]
        public void get_overdue_books_returns_the_book_with_the_return_date_passed()
        {
            //given
            var issuedBooks = TestData.IssuedBooks;
            moqTxnMgr.Setup(m => m.Create<IssuedBook>().Get()).Returns(issuedBooks);
            moqTxnMgr.Setup(m => m.Create<Book>().Get()).Returns(TestData.Books);

            var sut = new BookService(moqTxnMgr.Object);

            //when
            var result = sut.GetOverdueBooks();

            //then
            if(result!=null)
            {
                Assert.IsInstanceOfType(result, typeof(IEnumerable<Book>));
            }
            moqTxnMgr.Verify(v => v.Create<Book>().Get(), Times.Once);
            moqTxnMgr.Verify(v => v.Create<IssuedBook>().Get(), Times.Once);
        }
    }
}
