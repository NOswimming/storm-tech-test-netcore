using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Todo.Services;
using Xunit;

namespace Todo.Tests
{
    public class WhenRelevantTodoListsRetrieved
    {
        private readonly TodoApplicationTestFixture testFixture;


        private readonly IdentityUser aliceIdentityUser;
        private readonly IQueryable<TodoList> resultTodoLists;

        public WhenRelevantTodoListsRetrieved()
        {
            // Create test fixture
            testFixture = new TodoApplicationTestFixture();

            // Add data to test fixture.

            // Create todo list for test user.
            aliceIdentityUser = new IdentityUser("alice@example.com");

            var aliceShoppingTodoList = new TestTodoListBuilder(aliceIdentityUser, "shopping")
                    .WithItem("bread", Importance.High)
                    .WithItem("chocolate", Importance.Low)
                    .WithItem("milk", Importance.High)
                    .WithItem("honey", Importance.Medium)
                    .WithItem("magazine", Importance.Low)
                    .WithItem("cheese", Importance.Medium)
                    .Build();

            // Create todo list for other user
            var bobIdentityUser = new IdentityUser("bob@example.com");

            var bobShoppingTodoList = new TestTodoListBuilder(bobIdentityUser, "shopping")
                    .WithItem("eggs", Importance.High)
                    .WithItem("sweets", Importance.Low)
                    .WithItem("apples", Importance.High)
                    .WithItem("pasta", Importance.Medium)
                    .Build();

            var bobChoresTodoList = new TestTodoListBuilder(bobIdentityUser, "chores")
                    .WithItem("clean bathroom", Importance.High)
                    .WithItem("gardening", Importance.Low)
                    .WithItem("hoovering", Importance.Medium)
                    .WithItem("dusting", Importance.Medium)
                    .Build();

            // Assign the garding to alice
            var gardeningItem = bobChoresTodoList.Items.Single(i => i.Title == "gardening");
            gardeningItem.ResponsiblePartyId = aliceIdentityUser.Id;


            testFixture.ApplicationDbContext.TodoLists.AddRange(new[]
            {
                aliceShoppingTodoList,
                bobShoppingTodoList,
                bobChoresTodoList
            });
            testFixture.ApplicationDbContext.SaveChanges();


            // Get results to test
            resultTodoLists = testFixture.ApplicationDbContext.RelevantTodoLists(aliceIdentityUser.Id);
        }

        [Fact]
        public void IncludesAllListsWithUserAsOwner()
        {
            var ownedLists = testFixture.ApplicationDbContext.TodoLists
                .Where(l => l.Owner == aliceIdentityUser);

            Assert.All(ownedLists, ol =>
            {
                Assert.Contains(ol, resultTodoLists);
            });
        }

        [Fact]
        public void IncludesAllListsWithUserAsResponsiblePartyForItem()
        {
            var responsibleItemLists = testFixture.ApplicationDbContext.TodoItems
                .Where(i => i.ResponsibleParty == aliceIdentityUser)
                .Select(i => i.TodoList)
                .Distinct();

            Assert.All(responsibleItemLists, ril =>
            {
                Assert.Contains(ril, resultTodoLists);
            });
        }

        [Fact]
        public void ExcludesAllListsWhereUserIsNotOwnerOrResponsiblePartyForItem()
        {
            var excludedLists = testFixture.ApplicationDbContext.TodoLists
                .Where(l => l.Owner != aliceIdentityUser && l.Items.All(i => i.ResponsiblePartyId != aliceIdentityUser.Id));

            Assert.All(excludedLists, el =>
            {
                Assert.DoesNotContain(el, resultTodoLists);
            });
        }
    }
}