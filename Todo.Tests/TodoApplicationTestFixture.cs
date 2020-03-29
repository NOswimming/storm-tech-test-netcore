using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Todo.Data;

namespace Todo.Tests
{
    public class TodoApplicationTestFixture
    {
        public ServiceProvider ServiceProvider { get; }
        public ApplicationDbContext ApplicationDbContext => GetRequiredService<ApplicationDbContext>();
        public TodoApplicationTestFixture()
        {
            ServiceProvider = BuildServiceCollection().BuildServiceProvider();
        }

        protected IServiceCollection BuildServiceCollection()
        {
            return new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(nameof(TodoApplicationTestFixture)));
        }

        public T GetRequiredService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}