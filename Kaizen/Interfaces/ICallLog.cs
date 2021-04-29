using Kaizen.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kaizen.Interfaces
{
    public interface ICallLog
    {
        IEnumerable<CallLogModel> GetCallLogs();
    }
}
