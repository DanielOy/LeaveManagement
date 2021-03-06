using AutoMapper;
using LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.Responses;
using LeaveManagement.Domain;
using MediatR;
using LeaveManagement.Application.Contracts.Infrastructure;
using LeaveManagement.Application.Models;
using LeaveManagement.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using LeaveManagement.Application.Constants;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaveRequestCommandHandler(IMapper mapper, IEmailSender emailSender, IHttpContextAccessor httpContext,  IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContext = httpContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestDtoValidator(_unitOfWork.LeaveRequestRepository);
            var validationResult = await validator.ValidateAsync(request.CreateLeaveRequestDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var userId = _httpContext.HttpContext.User.Claims.FirstOrDefault(q => q.Type == CustomClaimTypes.Uid)?.Value;
            var allocation = await _unitOfWork.LeaveAllocationRepository.GetUserAllocations(userId, request.CreateLeaveRequestDto.LeaveTypeId);

            if (allocation == null)
                throw new Exception("You don't have any allocations for this leave type.");

            int daysRequested = (int)(request.CreateLeaveRequestDto.EndDate - request.CreateLeaveRequestDto.StartDate).TotalDays;

            if (daysRequested > allocation.NumberOfDays)
                throw new Exception("You don't have enough days for this request.");

            var leaveRequest = _mapper.Map<LeaveRequest>(request.CreateLeaveRequestDto);
            leaveRequest.RequestingEmployeeId = userId;
            leaveRequest = await _unitOfWork.LeaveRequestRepository.Add(leaveRequest);

            await _unitOfWork.Save();

            var emailAddress = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;

            var email = new Email
            {
                To = emailAddress,
                Body = $"Your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} " +
                $"has been submitted successfully",
                Subject = "Leave Request Submitted"
            };

            try
            {
                await _emailSender.SendEmail(email);
            }
            catch { }


            return leaveRequest.Id;
        }
    }
}
