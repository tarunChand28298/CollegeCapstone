using ServerApplication.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerApplication.Storage
{
    public interface IStorageUnit
    {
        Task<IEnumerable<SensorDataRecord>> RetrieveSensorDataRecords(int nRecords);
    }
}
