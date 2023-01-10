﻿using PermissionsWebApi.Application;
using PermissionsWebApi.Configuration;
using PermissionsWebApi.DTOs;
using PermissionsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CommandHandler
{
    public class AddPermissionCommandHandler: ICommandHandler<PermissionDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddPermissionCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public CommandResult Execute(PermissionDTO permission)
        {
            var newPermission = new Permission()
            {
                Id = permission.Id,
                EmployeeForename = permission.EmployeeForename,
                EmployeeSurname = permission.EmployeeSurname,
                PermissionDate = permission.PermissionDate,
                PermissionTypeId = permission.PermissionTypeId
            };
             _unitOfWork.Permission.Add(newPermission);
             _unitOfWork.Commit();

            return new CommandResult { Status = true, Message = "Permission added succesfully" };
        }
    }
}
