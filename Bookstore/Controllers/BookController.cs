
using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {

        public ActionResult Show(int pageNumber)
        {

            List<GetBooksOfPage_Result> books = null;
            using (BookstoreEntities dbContext=new BookstoreEntities())
            {
                int bookPerPage = 5;
                books = dbContext.GetBooksOfPage(bookPerPage, pageNumber).ToList();
                ViewBag.MaxPageNumber = dbContext.Books.Count()/bookPerPage + 1;
            }


            return View(books);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Book targetBook = null;
            using (BookstoreEntities dbContext=new BookstoreEntities())
            {
                targetBook = dbContext.Books.SingleOrDefault(b=>b.Id==id);
            }
            return View(targetBook);

        }


        [HttpPost]
        public ActionResult Edit(Book book) {
            using (BookstoreEntities dbContext=new BookstoreEntities())
            {
                if (this.Request.Files!=null&&this.Request.Files.Count>0&&this.Request.Files[0].ContentLength>0&&this.Request.Files[0].ContentLength<1024*100)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/Covers/" + fileName;
                    book.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));
                    //this.Request.Files[0].SaveAs(filePhysicalPath);
                    //string textInFile=System.IO.File>ReadAllText(filePhysicalPath);
                    //book.Description=textInFile;上传文件

                }




                dbContext.Books.Attach(book);
                dbContext.Entry(book).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Show", new { pageNumber = 1 });
        }
    }
}