using LeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Persitence.Contract
{
    public interface ILeaveRequestRepository:IGenericRepository<LeaveRequest>
    {

    }
}
