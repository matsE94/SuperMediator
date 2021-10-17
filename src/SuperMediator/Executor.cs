using System;
using System.Threading.Tasks;

namespace SuperMediator
{
    public interface IExecutor
    {
        Task ExecuteCommandAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        Task<TResponse> ExecuteQueryAsync<TResponse, TQuery>(TQuery query) 
            where TQuery : class, IQuery<TResponse>;
    }

    public class Executor : IExecutor
    {
        public Executor(Func<Type, object> objectFactory)
        {
            ObjectFactory = objectFactory;
        }

        private Func<Type, object> ObjectFactory { get; }

        public async Task ExecuteCommandAsync<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var type = typeof(ICommandHandler<TCommand>);
            var service = (ICommandHandler<TCommand>)ObjectFactory(type);
            await service.Execute(command);
        }


        public async Task<TResponse> ExecuteQueryAsync<TResponse, TQuery>(TQuery query)
            where TQuery : class, IQuery<TResponse>
        {
            var type = typeof(IQueryHandler<TQuery,TResponse>);
            var service = (IQueryHandler<TQuery, TResponse>)ObjectFactory(type);
            return await service.Execute(query);
        }
    }
}