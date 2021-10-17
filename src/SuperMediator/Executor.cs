using System;
using System.Threading.Tasks;

namespace SuperMediator
{
    public interface IExecutor
    {
        Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResponse> ExecuteAsync<TResponse>(IQuery<TResponse> query);
    }

    public class Executor : IExecutor
    {
        public Executor(Func<Type, object> objectFactory)
        {
            ObjectFactory = objectFactory;
        }

        private Func<Type, object> ObjectFactory { get; }

        public async Task ExecuteAsync<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var type = typeof(ICommandHandler<TCommand>);
            var service = (ICommandHandler<TCommand>)ObjectFactory(type);
            await service.Execute(command);
        }

        public async Task<TResponse> ExecuteAsync<TResponse>(IQuery<TResponse> query)
        {
            var type = typeof(IQueryHandler<IQuery<TResponse>,TResponse>);
            var service = (IQueryHandler<IQuery<TResponse>,TResponse>)ObjectFactory(type);
            return await service.Execute(query);
        }
    }
}