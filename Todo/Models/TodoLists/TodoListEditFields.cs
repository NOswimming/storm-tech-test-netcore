using System.ComponentModel.DataAnnotations;

namespace Todo.Models.TodoLists
{
    public class TodoListEditFields
    {
        public int TodoListId { get; set; }
        public string Title { get; set; }

        public TodoListEditFields() { }
        public TodoListEditFields(int todoListId, string title)
        {
            TodoListId = todoListId;
            Title = title;
        }
    }
}