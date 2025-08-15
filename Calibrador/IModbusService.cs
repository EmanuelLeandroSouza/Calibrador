using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calibrador
{
    public interface IModbusService
    {
        bool IsConnected { get; }
        void Connect(string ipAddress, int port, byte unitId);
        void Disconnect();
        float ReadValue(ushort address);
    }
}
