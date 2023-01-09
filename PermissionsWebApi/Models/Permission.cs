﻿namespace PermissionsWebApi.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public DateTime PermissionDate { get; set; }

        //FK
        public int PermissionTypeId { get; set; }
        public PermissionType PermissionType { get; set; }
    }
}
