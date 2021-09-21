using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ToDoAPI.API.Models;
using ToDoAPI.DATA.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //List<categoryVM>
        public IHttpActionResult GetCategories()
        {
            List<CategoryViewModel> cats = db.Categories.Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Description = c.Description,
                Name = c.Name

            }).ToList<CategoryViewModel>();

            if (cats.Count == 0)
            {
                return NotFound();
            }
            return Ok(cats);

        }//End List<categoryVM>

        //GetCategory
        public IHttpActionResult GetCategory(int id)
        {
            CategoryViewModel cat = db.Categories.Where(c => c.CategoryId == id).Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description

            }).FirstOrDefault();

            if (cat == null)
            {
                return NotFound();
            }

            return NotFound();
        }//End GetCategory

        //PostCategory
        public IHttpActionResult PostCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            db.Categories.Add(new Category()
            {
                Name = cat.Name,
                Description = cat.Description
            });

            db.SaveChanges();
            return Ok(cat);
        }//end PostCategory

        //Put
        public IHttpActionResult PutCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            Category existingCat = db.Categories.Where(c => c.CategoryId == cat.CategoryId).FirstOrDefault();

            if (existingCat != null)
            {
                existingCat.Name = cat.Name;
                existingCat.Description = cat.Description;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }//end Put

        //Delete
        public IHttpActionResult DeleteCategory(int id)
        {
            Category cat = db.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//End Delete

        //Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);    
        }//Dispose 

    }
}
