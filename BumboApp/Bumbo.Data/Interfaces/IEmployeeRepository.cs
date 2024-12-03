using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        
        public Employee? GetEmployee(int id);
        public string FindNameFromId(int id);
    }
}

