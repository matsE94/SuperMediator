using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SuperMediator.Test
{
    public class SuperMediatorTests
    {
        class MyCommand : ICommand
        {
        }

        class MyCommandHandler : ICommandHandler<MyCommand>
        {
            public Task Execute(MyCommand command)
            {
                // execute some code here
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task ExecuteAsync_WithCommand_ShouldNotThrowAny()
        {
            // arrange
            var services = new ServiceCollection();
            services.AddTransient<ICommandHandler<MyCommand>, MyCommandHandler>();
            var provider = services.BuildServiceProvider();
            object ObjectFactory(Type type) => provider.GetRequiredService(type);
            
            var executor = new Executor(ObjectFactory);

            // act + assert
            await executor.Invoking(async x => await x.ExecuteAsync(new MyCommand()))
                .Should()
                .NotThrowAsync();
        }
        
        
        class MyQuery : IQuery<int>
        {
            public string Name { get; set; }
        }

        class MyQueryHandler : IQueryHandler<MyQuery,int>
        {
            public async Task<int> Execute(MyQuery query)
            {
                // execute some code here
                if (query.Name is "Batman")
                {
                    return 42;
                }
                
                return 0;
            }
        }
        
        [Fact]
        public async Task ExecuteAsync_WithQuery_ShouldNotReturnNull()
        {
            // arrange
            var services = new ServiceCollection();
            services.AddTransient<IQueryHandler<MyQuery,int>, MyQueryHandler>();
            var provider = services.BuildServiceProvider();
            object ObjectFactory(Type type) => provider.GetRequiredService(type);
            
            var executor = new Executor(ObjectFactory);

            // act + assert
            await executor.Invoking(async x => await x.ExecuteAsync(new MyCommand()))
                .Should()
                .NotThrowAsync();
        }
    }
}