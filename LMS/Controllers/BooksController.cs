using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Domain;
using LMS.Model;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBookStoreService _bookStoreService;

        public BooksController(IBookStoreService bookStoreService)
        {
            _bookStoreService = bookStoreService;
        }
        /// <summary>
        /// Retrieve all the books.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Book>))]
        [ProducesResponseType(400)]
        public IActionResult GetBooks()
        {
            try
            {
                var books = _bookStoreService.GetAllBooks();
                if (books == null) return NotFound();
                return Ok(books.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);//shout/catch/throw/log
            }
        }

        /// <summary>
        /// Retrieve all the overdue books.
        /// </summary>
        [HttpGet]
        [Route("overduebooks")]
        [ProducesResponseType(200, Type = typeof(List<Book>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetOverDueBooks()
        {
            try
            {
                var overDueBooks = _bookStoreService.GetOverdueBooks();
                if (overDueBooks == null) return NotFound();
                return Ok(overDueBooks.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);//shout/catch/throw/log
            }
        }

        /// <summary>
        /// Issue the book to the student.
        /// </summary>
        /// <param name="studentId">The studentId of the desired student</param>
        /// <param name="bookId">The bookId of the book to be issued</param>
        /// <returns>A boolean status</returns>
        [HttpPost]
        [Route("issue/studentId/{studentId}/bookId/{bookId}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult AssignBook(int studentId, int bookId)
        {
            if (!ModelState.IsValid || studentId == 0 || bookId == 0)
                return BadRequest(ModelState);
            try
            {
                var assigned = _bookStoreService.IssueBook(studentId, bookId);
                return Ok(assigned);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);//shout/catch/throw/log
            }
        }

        /// <summary>
        /// Extend the book return date.
        /// </summary>
        /// <param name="bookId">The bookId of the desired book to extend return date</param>
        /// <param name="days">The number of days to extend the return date of the issued book</param>
        /// <returns>A boolean status</returns>
        [HttpPut]
        [Route("extend/bookId/{bookId}/days/{days}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult ExtendBook(int bookId, int days)
        {
            if (!ModelState.IsValid || bookId == 0 || days == 0)
                return BadRequest(ModelState);
            try
            {
                var extended= _bookStoreService.ExtendReturnDate(bookId,days);
                return Ok(extended);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);//shout/catch/throw/log
            }
        }
    }
}