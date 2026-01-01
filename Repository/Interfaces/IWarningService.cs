using Domins.Dtos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IWarningService
    {
        public Task SendWarning(Warningdto dto);
    }
}

