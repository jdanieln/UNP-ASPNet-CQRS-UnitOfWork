using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CommandHandler;
using Application.Commands;
using Application.QueryHandler;
using Domain;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionsWebApi.Kafka;

namespace PermissionsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly ICommandHandler<PermissionDTO> _permissionCommandHandler;
        private readonly ICommandHandler<RemoveByIdCommand> _removeCommandHandler;
        private readonly IQueryHandler<Permission, QueryByIdCommand> _permissionQueryHandler;
        private readonly IKafkaProducerHandler _kafkaProducerHandler;
        public PermissionsController(
            ICommandHandler<PermissionDTO> permissionCommandHandler, 
            ICommandHandler<RemoveByIdCommand> removeCommandHandler, 
            IQueryHandler<Permission, QueryByIdCommand> permissionQueryHandler,
            IKafkaProducerHandler kafkaProducerHandler
            )
        {
            _permissionCommandHandler = permissionCommandHandler;
            _removeCommandHandler = removeCommandHandler;
            _permissionQueryHandler = permissionQueryHandler;
            _kafkaProducerHandler = kafkaProducerHandler;
        }

        // GET: api/Permissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> GetPermission()
        {
            _kafkaProducerHandler.WriteMessage("GET");
            var permissions = await _permissionQueryHandler.GetAll();
            return Ok(permissions);
        }

        // GET: api/Permissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> GetPermission(int id)
        {
            _kafkaProducerHandler.WriteMessage("GET");
            var permission = await _permissionQueryHandler.GetOne(new QueryByIdCommand()
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
            _kafkaProducerHandler.WriteMessage("PUT");
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
            _kafkaProducerHandler.WriteMessage("POST");
            _permissionCommandHandler.Execute(permission);
            return CreatedAtAction("GetPermission", new { id = permission.Id }, permission);
        }

        // DELETE: api/Permissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            _kafkaProducerHandler.WriteMessage("DELETE");
            _removeCommandHandler.Execute(new RemoveByIdCommand()
            {
                Id = id
            });
            return NoContent();
        }

    }
}
