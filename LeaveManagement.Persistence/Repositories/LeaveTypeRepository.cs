using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Domain;

namespace LeaveManagement.Persistence.Repositories
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(LeaveManagementDbContext dbContext) : base(dbContext)
        { }
    }
}
