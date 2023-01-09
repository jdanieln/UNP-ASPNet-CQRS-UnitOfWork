namespace PermissionsWebApi.Models
{
    public class PermissionType
    {
        public int Id { get; set; }
        public string Description { get; set; }

        //RelationShip
        public ICollection<Permission> Permissions { get; set; }
    }
}
