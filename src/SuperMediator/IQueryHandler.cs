using System.Threading.Tasks;

namespace SuperMediator
{
    public interface IQueryHandler<TQuery,TResponse> where TQuery : IQuery<TResponse>
    {
        Task<TResponse> Execute(TQuery query);
    }
}