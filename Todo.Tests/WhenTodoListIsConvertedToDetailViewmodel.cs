using Microsoft.AspNetCore.Identity;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Xunit;

namespace Todo.Tests
{
    public class WhenTodoListIsConvertedToDetailViewmodel
    {
        private readonly TodoList srcTodoList;
        private readonly TodoListDetailViewmodel resultFields;

        public WhenTodoListIsConvertedToDetailViewmodel()
        {
            srcTodoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "shopping")
                    .WithItem("bread", Importance.High)
                    .WithItem("chocolate", Importance.Low)
                    .WithItem("milk", Importance.High)
                    .WithItem("honey", Importance.Medium)
                    .WithItem("magazine", Importance.Low)
                    .WithItem("cheese", Importance.Medium)
                    .Build()
                ;

            resultFields = TodoListDetailViewmodelFactory.Create(srcTodoList);
        }

        [Fact]
        public void EqualTodoListId()
        {
            Assert.Equal(srcTodoList.TodoListId, resultFields.TodoListId);
        }

        [Fact]
        public void EqualTitle()
        {
            Assert.Equal(srcTodoList.Title, resultFields.Title);
        }

        [Fact]
        public void OrderedByImportance()
        {
            var orderedItemTitles = srcTodoList.Items.OrderBy(i => i.Importance).Select(i => i.Title);
            Assert.Equal(orderedItemTitles, resultFields.Items.Select(i => i.Title));
        }
    }
}