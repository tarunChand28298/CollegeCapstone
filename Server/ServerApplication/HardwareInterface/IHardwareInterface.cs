using ServerApplication.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerApplication.HardwareInterface
{
    public interface IHardwareInterface
    {
        public event Action<string> DataRecieved;
        void SetSetpoint(float point);
    }
}
