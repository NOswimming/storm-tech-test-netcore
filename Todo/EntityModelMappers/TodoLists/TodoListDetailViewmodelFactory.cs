using System;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, string orderBy)
        {
            var items = todoList.Items
                .Select(TodoItemSummaryViewmodelFactory.Create)
                .OrderBy(GetOrderByFunc(orderBy))
                .ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items);
        }

        private static Func<TodoItemSummaryViewmodel, object> GetOrderByFunc(string orderBy)
        {
            switch(orderBy?.ToLowerInvariant())
            {
                case "rank":
                    return new Func<TodoItemSummaryViewmodel, object>(i => i.Rank);
                case "importance": 
                default:
                    return new Func<TodoItemSummaryViewmodel, object>(i => i.Importance);
            }
        }
    }
}