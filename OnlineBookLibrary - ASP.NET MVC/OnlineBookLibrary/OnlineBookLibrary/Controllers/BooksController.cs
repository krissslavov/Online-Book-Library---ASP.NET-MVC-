using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OBL.Data.Context;
using OBL.Data.Entities;
using PagedList;




namespace OnlineBookLibrary.Controllers
{
    public class BooksController : Controller
    {
        private BookCatalogDbContext db = new BookCatalogDbContext();
        private static string lastSearch = null;
        private static IQueryable<Book> Search(string search, string searchValue, IQueryable<Book> books)
        {

            if (!String.IsNullOrEmpty(search))
            {
                switch (searchValue)
                {
                    case "Genre":
                        lastSearch = "Genre";
                        return books.Where(x => x.Genre.GenreName.Contains(search));

                    case "Author":
                        lastSearch = "Author";
                        return books.Where(x => x.Author.UserName.Contains(search));

                    case "Title":
                        lastSearch = "Title";
                        return books.Where(x => x.Title.Contains(search));



                }
            }
            return books.AsQueryable();

        }


        // GET: Books
        public ActionResult Index(int? page, string search, string sortOrder, string searchValue)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;

            List<string> valueList = new List<string>() { "Title", "Author", "Genre"};
            ViewBag.SearchValue = new SelectList(valueList, "", "", searchValue);
            IQueryable<Book> books = db.Books.AsQueryable();

            ViewBag.Search = search;
            books = Search(search, searchValue, books);

            ViewBag.CurrentSortParm = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AuthorSortParm = sortOrder == "author_asc" ? "author_desc" : "author_asc";
            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(x => x.Title);
                    break;
                case "author_asc":
                    books = books.OrderBy(x => x.Author.UserName);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(x => x.Author.UserName);
                    break;
                default:
                    books = books.OrderBy(x => x.Title);
                    break;
            }

            return View(books.ToPagedList(pageNumber, pageSize));
        }



        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "FirstName");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,Title,ReleaseDate,AuthorId,GenreId,Description")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "FirstName", book.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", book.GenreId);
            return View(book);
        }

        // GET: Books/Edit/5
       // [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "FirstName", book.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", book.GenreId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
       // [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,Title,ReleaseDate,AuthorId,GenreId,Description")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "FirstName", book.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", book.GenreId);
            return View(book);
        }

        // GET: Books/Delete/5
       [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
