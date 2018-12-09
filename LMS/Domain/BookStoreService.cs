using LMS.Model;
using System;
using System.Collections.Generic;

namespace LMS.Domain
{
    public interface IBookStoreService
    {
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetOverdueBooks();
        bool IssueBook(int studentId, int bookId);
        bool ExtendReturnDate(int bookId,int days);
    }
    public class BookStoreService : IBookStoreService
    {
        IStudentService _studentService;
        IBookService _bookService;
        IBookAllocationService _bookallocationService;
        public BookStoreService(IStudentService studentService,IBookService bookService, IBookAllocationService bookAllocationService)
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

            bool bookAssigned = false;

            try
            {
                bookAssigned = Issue(studentId, bookId);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return bookAssigned;
        }
        public bool ExtendReturnDate(int bookId, int days)
        {
            if(bookId==0 || days==0)
                throw new Exception("no bookid found/extend return day is 0");
            bool extended = false;
            try
            {
                var book = GetBook(bookId);
                extended= _bookallocationService.ExtendReturndate(book, days);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return extended;
        }

        #region PRIVATE
        bool Issue(int studentId,int bookId)
        {
            var student= _studentService.GetStudent(studentId);
            var book = GetBook(bookId);
            return _bookallocationService.IssueBook(student, book);
        }
        Book GetBook(int bookId)
        {
            return _bookService.GetBook(bookId);
        }
        #endregion
    }
}
