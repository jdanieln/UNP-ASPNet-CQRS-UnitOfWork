using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionsWebApi.Application;
using PermissionsWebApi.Application.CommandHandler;
using PermissionsWebApi.Application.QueryHandler;
using PermissionsWebApi.Configuration;
using PermissionsWebApi.Data;
using PermissionsWebApi.DTOs;
using PermissionsWebApi.Models;

namespace PermissionsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly ICommandHandler<PermissionDTO> _permissionCommandHandler;
        private readonly ICommandHandler<RemoveCommand> _removeCommandHandler;
        private readonly IQueryHandler<Permission, QueryCommand> _permissionQueryHandler;

        public PermissionsController(ICommandHandler<PermissionDTO> permissionCommandHandler, ICommandHandler<RemoveCommand> removeCommandHandler, IQueryHandler<Permission, QueryCommand> permissionQueryHandler)
        {
            _permissionCommandHandler = permissionCommandHandler;
            _removeCommandHandler = removeCommandHandler;
            _permissionQueryHandler = permissionQueryHandler;
        }

        // GET: api/Permissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> GetPermission()
        {
            var permissions = await _permissionQueryHandler.GetAll();
            return Ok(permissions);
        }

        // GET: api/Permissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> GetPermission(int id)
        {
            var permission = await _permissionQueryHandler.GetOne(new QueryCommand()
            {
                Id = id
            });

            if (permission == null)
            {
                return NotFound();
            }

            return Ok(permission);
        }

        // PUT: api/Permissions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermission(int id, PermissionDTO permission)
        {
            if (id != permission.Id)
            {
                return BadRequest();
            }

            _permissionCommandHandler.Execute(permission);
            return NoContent();
        }

        // POST: api/Permissions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostPermission(PermissionDTO permission)
        {
            _permissionCommandHandler.Execute(permission);
            return CreatedAtAction("GetPermission", new { id = permission.Id }, permission);
        }

        // DELETE: api/Permissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            _removeCommandHandler.Execute(new RemoveCommand()
            {
                Id = id
            });
            return NoContent();
        }

    }
}
