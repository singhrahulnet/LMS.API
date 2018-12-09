using LMS.Model;
using LMS.Persistence;
using System;
using System.Linq;

namespace LMS.Domain
{
    public interface IBookAllocationService
    {
        bool IssueBook(Student student, Book book);
        bool ExtendReturndate(IssuedBook book, int days);
    }
    public class BookAllocationService : IBookAllocationService
    {
        ITransactionManager _mgr;
        const int Day2Return = 5;

        public BookAllocationService(ITransactionManager mgr)
        {
            _mgr = mgr;
        }
        public bool IssueBook(Student student, Book book)
        {
            if (student == null || book == null)
                throw new Exception("no student/book found");

            bool assigned = false;
            try
            {
                if (GetIssuedBookDetail(book) == null)
                    return Issue(student, book);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return assigned;
        }
        public bool ExtendReturndate(IssuedBook book, int days)
        {
            if (book == null || days == 0)
                throw new Exception("book not found/days to extendis 0");
            bool extended = false;
            try
            {
                extended = Extend(book, days);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return extended;
        }

        #region PRIVATE
        IssuedBook GetIssuedBookDetail(Book book)
        {
            return _mgr.Create<IssuedBook>().Get().Where(i => i.BookId == book.BookId).FirstOrDefault();
        }
        bool Issue(Student student, Book book)
        {
            var issuedBook = new IssuedBook()
            {
                BookId = book.BookId,
                StudentId = student.StudentId,
                IssueDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(Day2Return)
            };
            _mgr.Create<IssuedBook>().Add(issuedBook);
            return _mgr.Save() == 1 ? true : false;
        }
        bool Extend(IssuedBook book, int days)
        {
            book.ReturnDate = book.ReturnDate.AddDays(days);
            book.ReturnDateExtended = true;
            _mgr.Create<IssuedBook>().Update(book);
            return _mgr.Save() == 1 ? true : false;
        }
        #endregion
    }
}
