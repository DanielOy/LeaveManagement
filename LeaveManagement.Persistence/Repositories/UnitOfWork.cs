using LeaveManagement.Application.Contracts.Persitence;
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

        private ILeaveAllocationRepository _leaveAllocationRepository;
        private ILeaveRequestRepository _leaveRequestRepository;
        private ILeaveTypeRepository _leaveTypeRepository;

        public UnitOfWork(LeaveManagementDbContext dbcontext)
        {
            _dbcontext = dbcontext;
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
            await _dbcontext.SaveChangesAsync();
        }
    }
}
