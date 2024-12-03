using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        public string FindNameFromId(int id);
    }
}
