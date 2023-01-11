using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Configuration;
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
    public class PermissionTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKafkaProducerHandler _kafkaProducerHandler;

        public PermissionTypesController(IUnitOfWork unitOfWork, IKafkaProducerHandler kafkaProducerHandler)
        {
            _unitOfWork = unitOfWork;
            _kafkaProducerHandler = kafkaProducerHandler;
        }

        // GET: api/PermissionTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionType>>> GetPermissionType()
        {
            _kafkaProducerHandler.WriteMessage("GET");
            var permissionTypes = await _unitOfWork.PermissionType.All();
            return Ok(permissionTypes);
        }

        // GET: api/PermissionTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionType>> GetPermissionType(int id)
        {
            _kafkaProducerHandler.WriteMessage("GET");
            var permissionType = await _unitOfWork.PermissionType.GetById(id);

            if (permissionType == null)
            {
                return NotFound();
            }

            return Ok(permissionType);
        }

        // PUT: api/PermissionTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermissionType(int id, PermissionTypeDTO permissionType)
        {
            _kafkaProducerHandler.WriteMessage("PUT");
            if (id != permissionType.Id)
            {
                return BadRequest();
            }
            var newPermissionType = new PermissionType()
            {
                Id = permissionType.Id,
                Description = permissionType.Description
            };
            _unitOfWork.PermissionType.Add(newPermissionType);
            _unitOfWork.Commit();
            return NoContent();
        }

        // POST: api/PermissionTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PermissionType>> PostPermissionType(PermissionTypeDTO permissionType)
        {
            _kafkaProducerHandler.WriteMessage("POST");
            var newPermissionType = new PermissionType()
            {
                Id = permissionType.Id,
                Description = permissionType.Description
            };
            _unitOfWork.PermissionType.Add(newPermissionType);
            _unitOfWork.Commit();

            return CreatedAtAction("GetPermissionType", new { id = permissionType.Id }, permissionType);
        }

        // DELETE: api/PermissionTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermissionType(int id)
        {
            _kafkaProducerHandler.WriteMessage("DELETE");
            var permissionType = await _unitOfWork.PermissionType.GetById(id);
            if (permissionType == null)
            {
                return NotFound();
            }

            _unitOfWork.Permission.Delete(id);
            _unitOfWork.Commit();

            return NoContent();
        }
    }
}
