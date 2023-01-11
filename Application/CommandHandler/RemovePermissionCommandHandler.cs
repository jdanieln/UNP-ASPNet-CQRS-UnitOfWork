using Application.Commands;
using Domain;
using Domain.Configuration;

namespace Application.CommandHandler
{
    public class RemovePermissionCommandHandler : ICommandHandler<RemoveByIdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RemovePermissionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public CommandResult Execute(RemoveByIdCommand command)
        {
            _unitOfWork.Permission.Delete(command.Id);
            _unitOfWork.Commit();
            return new CommandResult { Status = true, Message = "Permission added succesfully" };
        }
    }
}
