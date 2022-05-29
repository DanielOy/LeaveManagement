using LeaveManagement.Application.Constants;
using LeaveManagement.Application.Contracts.Persitence;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LeaveManagementDbContext _dbcontext;
        private readonly IHttpContextAccessor _httpContext;

        private ILeaveAllocationRepository _leaveAllocationRepository;
        private ILeaveRequestRepository _leaveRequestRepository;
        private ILeaveTypeRepository _leaveTypeRepository;

        public UnitOfWork(LeaveManagementDbContext dbcontext, IHttpContextAccessor httpContext)
        {
            _dbcontext = dbcontext;
            _httpContext = httpContext;
        }

        public ILeaveAllocationRepository LeaveAllocationRepository => _leaveAllocationRepository ??= new LeaveAllocationRepository(_dbcontext);

        public ILeaveRequestRepository LeaveRequestRepository => _leaveRequestRepository ??= new LeaveRequestRepository(_dbcontext);

        public ILeaveTypeRepository LeaveTypeRepository => _leaveTypeRepository ??= new LeaveTypeRepository(_dbcontext);

        public void Dispose()
        {
            _dbcontext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            string userName = _httpContext.HttpContext.User.FindFirst(CustomClaimTypes.Uid)?.Value;
            await _dbcontext.SaveChangesAsync(userName);
        }
    }
}
