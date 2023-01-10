using Nest;
using PermissionsWebApi.Configuration;
using PermissionsWebApi.DTOs;
using PermissionsWebApi.Models;

namespace PermissionsWebApi.Application.CommandHandler
{
    public class RemoveCommand
    {
        public int Id { get; set; }
    }
    public class RemovePermissionCommandHandler : ICommandHandler<RemoveCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RemovePermissionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public CommandResult Execute(RemoveCommand command)
        {
            _unitOfWork.Permission.Delete(command.Id);
            _unitOfWork.Commit();
            return new CommandResult { Status = true, Message = "Permission added succesfully" };
        }
    }
}
