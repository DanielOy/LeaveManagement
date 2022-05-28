﻿using LeaveManagement.Application.DTOs.LeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LeaveTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<LeaveTypesController>
        [HttpGet]
        public async Task<ActionResult<List<LeaveTypeDto>>> Get()
        {
            var request = new GetLeaveTypeListRequest();
            var leaveTypes = await _mediator.Send(request);
            return Ok(leaveTypes);
        }

        // GET api/<LeaveTypesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveTypeDto>> Get(int id)
        {
            var request = new GetLeaveTypeDetailRequest() { Id = id };
            var leaveType = await _mediator.Send(request);
            return Ok(leaveType);
        }

        // POST api/<LeaveTypesController>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<int>> Post([FromBody] CreateLeaveTypeDto leaveType)
        {
            var command = new CreateLeaveTypeCommand() { CreateLeaveTypeDto = leaveType };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT api/<LeaveTypesController>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<int>> Put([FromBody] LeaveTypeDto leaveType)
        {
            var command = new UpdateLeaveTypeCommand() { LeaveTypeDto = leaveType };
            await _mediator.Send(command);
            return Ok(leaveType.Id);
        }

        // DELETE api/<LeaveTypesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var command = new DeleteLeaveTypeCommand() { Id = id };
            await _mediator.Send(command);
            return Ok(id);
        }
    }
}
