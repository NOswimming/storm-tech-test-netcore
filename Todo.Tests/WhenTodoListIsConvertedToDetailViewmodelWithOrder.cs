using Microsoft.AspNetCore.Identity;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Xunit;

namespace Todo.Tests
{
    public class WhenTodoListIsConvertedToDetailViewmodelWithOrder
    {
        private readonly TodoList srcTodoList;
        private readonly TodoListDetailViewmodel resultFields;

        public WhenTodoListIsConvertedToDetailViewmodelWithOrder()
        {
            srcTodoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "shopping")
                    .WithItem("bread", Importance.High, 0)
                    .WithItem("chocolate", Importance.Low, 2)
                    .WithItem("milk", Importance.High, 1)
                    .WithItem("honey", Importance.Medium, 3)
                    .WithItem("magazine", Importance.Low, 4)
                    .WithItem("cheese", Importance.Medium, 5)
                    .Build()
                ;

            resultFields = TodoListDetailViewmodelFactory.Create(srcTodoList, "rank");
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
        public void OrderedByRank()
        {
            var orderedItemTitles = srcTodoList.Items.OrderBy(i => i.Rank).Select(i => i.Title);
            Assert.Equal(orderedItemTitles, resultFields.Items.Select(i => i.Title));
        }
    }
}