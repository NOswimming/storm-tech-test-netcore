using Todo.Data.Entities;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public class TodoListEditFieldsFactory
    {
        public static TodoListEditFields Create(TodoList todoList)
        {
            return new TodoListEditFields(todoList.TodoListId, todoList.Title);
        }

        public static void Update(TodoListEditFields src, TodoList dest)
        {
            dest.Title = src.Title;
        }
    }
}