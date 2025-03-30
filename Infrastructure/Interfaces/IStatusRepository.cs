using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Entities;

namespace Infrastructure.Interfaces
{
    public interface IStatusRepository : IBaseRepository<StatusEntity>
    {
    }
}
