using LMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Domain
{
    public interface IBookStoreService
    {
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetOverdueBooks();
        bool IssueBook(int studentId, int bookId);
        bool ExtendReturnDate(int bookId, int days);
    }
    public class BookStoreService : IBookStoreService
    {
        IStudentService _studentService;
        IBookService _bookService;
        IBookAllocationService _bookallocationService;
        public BookStoreService(IStudentService studentService, IBookService bookService, IBookAllocationService bookAllocationService)
        {
            _studentService = studentService;
            _bookService = bookService;
            _bookallocationService = bookAllocationService;
        }
        public IEnumerable<Book> GetAllBooks()
        {
            IEnumerable<Book> books = null;
            try
            {
                books = _bookService.GetAllBooks();
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return books;
        }
        public IEnumerable<Book> GetOverdueBooks()
        {
            IEnumerable<Book> overDuebooks = null;
            try
            {
                overDuebooks = _bookService.GetOverdueBooks();
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return overDuebooks;
        }
        public bool IssueBook(int studentId, int bookId)
        {
            //always validate the inputs at entry
            if (studentId == 0 || bookId == 0)
                throw new Exception("no studentId/bookid found");

            bool bookissued = false;

            var student = _studentService.GetStudent(studentId);
            if (student == null) throw new Exception("Student not found");
            var book = _bookService.GetBook(bookId);
            if (book == null) throw new Exception("Book not found");

            try
            {
                return _bookallocationService.IssueBook(student, book);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return bookissued;
        }
        public bool ExtendReturnDate(int bookId, int days)
        {
            if (bookId == 0 || days == 0)
                throw new Exception("no bookid found/extend return day is 0");
            bool extended = false;
            try
            {
                var book = _bookService.GetIssuedBooks().FirstOrDefault(b => b.BookId == bookId);
                if (book == null) throw new Exception("book with ID '" + bookId + "' not found");
                extended = _bookallocationService.ExtendReturndate(book, days);
            }
            catch (Exception ex)
            {
                //shout/catch/throw/log
            }
            return extended;
        }

        #region PRIVATE
        
        Book GetBook(int bookId)
        {
            var book = _bookService.GetBook(bookId);
            if (book == null) throw new Exception("Book not found");
            return book;
        }
        #endregion
    }
}
