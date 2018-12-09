using LMS.Model;
using LMS.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Domain
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBook(int bookId);
        IEnumerable<Book> GetOverdueBooks();
        IEnumerable<IssuedBook> GetIssuedBooks();
    }
    public class BookService : IBookService
    {
        ITransactionManager _mgr;
        public BookService(ITransactionManager mgr)
        {
            _mgr = mgr;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            IEnumerable<Book> books = null;
            try
            {
                books = _mgr.Create<Book>().Get();
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return books;
        }

        public Book GetBook(int bookId)
        {
            if (bookId == 0)
                throw new Exception("no bookId ");
            Book book = null;
            try
            {
                book = _mgr.Create<Book>().GetById(bookId);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }

            return book;
        }

        public IEnumerable<Book> GetOverdueBooks()
        {
            IEnumerable<Book> books = null;
            try
            {
                var issuedBooks = _mgr.Create<IssuedBook>().Get();
                var issuedOverdueBooksId = issuedBooks.Where(i => DateTime.Now.Subtract(i.ReturnDate).Days > 0).Select(o => o.BookId);
                books = _mgr.Create<Book>().Get().Where(b => issuedOverdueBooksId.Contains(b.BookId));
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return books;
        }
        public IEnumerable<IssuedBook> GetIssuedBooks()
        {
            IEnumerable<IssuedBook> books = null;
            try
            {
                books = _mgr.Create<IssuedBook>().Get();

            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return books;
        }
    }
}
