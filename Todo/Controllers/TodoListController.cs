﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUserStore<IdentityUser> userStore;

        public TodoListController(ApplicationDbContext dbContext, IUserStore<IdentityUser> userStore)
        {
            this.dbContext = dbContext;
            this.userStore = userStore;
        }

        public IActionResult Index()
        {
            var userId = User.Id();
            var todoLists = dbContext.RelevantTodoLists(userId);
            var viewmodel = TodoListIndexViewmodelFactory.Create(todoLists);
            return View(viewmodel);
        }

        public IActionResult Detail(int todoListId, bool hideDoneItems, string orderBy)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            var viewmodel = TodoListDetailViewmodelFactory.Create(todoList, orderBy);
            viewmodel.HideItemsMarkedAsDone = hideDoneItems;
            viewmodel.OrderBy = orderBy;
            viewmodel.newTodoItem = TodoItemCreateFieldsFactory.Create(todoList, todoList.Owner.Id);
            return View(viewmodel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TodoListFields());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoListFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var currentUser = await userStore.FindByIdAsync(User.Id(), CancellationToken.None);

            var todoList = new TodoList(currentUser, fields.Title);

            await dbContext.AddAsync(todoList);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Create", "TodoItem", new {todoList.TodoListId});
        }

        [HttpGet]
        public IActionResult Edit(int todoListId)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            var fields = TodoListEditFieldsFactory.Create(todoList);
            return View(fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoListEditFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var todoList = dbContext.SingleTodoList(fields.TodoListId);

            TodoListEditFieldsFactory.Update(fields, todoList);

            dbContext.Update(todoList);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Detail), new { todoListId = todoList.TodoListId });
        }

        public async Task<ActionResult> CreateTodoItem(int todoListId, string todoListTitle, 
            string title, Importance importance, int rank, string responsiblePartyId,
            bool hideDoneItems, string orderBy)
        {
            // Create Item
            var item = new TodoItem(todoListId, responsiblePartyId, title, importance, rank);

            await dbContext.AddAsync(item);
            await dbContext.SaveChangesAsync();

            // Render partial view for display
            var todoList = dbContext.SingleTodoList(todoListId);
            var viewmodel = TodoListDetailViewmodelFactory.Create(todoList, orderBy);
            viewmodel.HideItemsMarkedAsDone = hideDoneItems;
            viewmodel.OrderBy = orderBy;
            viewmodel.newTodoItem = TodoItemCreateFieldsFactory.Create(todoList, todoList.Owner.Id);
            return PartialView("_DetailPartial", viewmodel);
        }
    }
}