using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ToDoAPI.DATA.EF;
using ToDoAPI.API.Models;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //List<GetToDo>
        public IHttpActionResult GetTodos()
        {
            List<ToDoViewModel> Todo = db.ToDoItems.Include("Category").Select(t => new ToDoViewModel()
            {
                TodoId = t.ToDoId,
                Action = t.Action,
                Done = t.Done,
                Category = new CategoryViewModel
                {
                    CategoryId = (int)t.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }

            }).ToList<ToDoViewModel>();

            if (Todo.Count == 0)
            {
                return NotFound();
            }

            return Ok(Todo);

        }//End List<GetToDo>


        //GetTodo
        public IHttpActionResult GetTodo(int id)
        {
            ToDoViewModel Todo = db.ToDoItems.Include("Category").Where(t => t.ToDoId == id).Select(t => new ToDoViewModel()
            {
                TodoId = t.ToDoId,
                Action = t.Action,
                Done = t.Done,
                Category = new CategoryViewModel
                {
                    CategoryId = (int)t.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }

            }).FirstOrDefault();

            if (Todo == null)
            {
                return NotFound();
            }

            return Ok(Todo);

        }//End GetTodo


        //PostTODO()
        public IHttpActionResult PostToDo(ToDoViewModel Todo)
        {
            if (!ModelState.IsValid)

                return BadRequest("Invalid Data");

            ToDoItem newToDo = new ToDoItem()
            {
                Action = Todo.Action,
                Done = Todo.Done,
                CategoryId = Todo.CategoryId
            };

            db.ToDoItems.Add(newToDo);
            db.SaveChanges();
            return Ok(newToDo);
        }//end PostTODO()


        //PUTToDo
        public IHttpActionResult PutTodo(ToDoViewModel Todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            ToDoItem existingToDo = db.ToDoItems.Where(t => t.ToDoId == Todo.TodoId).FirstOrDefault();

            if (existingToDo != null)
            {
                existingToDo.ToDoId = Todo.TodoId;
                existingToDo.Action = Todo.Action;
                existingToDo.Done = Todo.Done;
                existingToDo.CategoryId = Todo.CategoryId;

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }//End PUTToDo

        public IHttpActionResult DeleteTodo(int id)
        {
            ToDoItem toDo = db.ToDoItems.Where(t => t.ToDoId == id).FirstOrDefault();

            if (toDo != null)
            {
                db.ToDoItems.Remove(toDo);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }//End DeleteTodo

        //Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);    
        }//end Dispose
               
    }//end class
}//end namespace
