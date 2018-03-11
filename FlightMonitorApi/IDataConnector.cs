using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightMonitorApi
{
    public interface IDataConnector
    {
        bool BeforeStart();
    }
}
