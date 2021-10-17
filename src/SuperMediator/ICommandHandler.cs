using System.Threading.Tasks;

namespace SuperMediator
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task Execute(TCommand command);
    }
}