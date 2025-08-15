using System;
using System.Net.Sockets;
using NModbus;

public class ModbusService : IModbusService, IDisposable
{
    private TcpClient _client;
    private IModbusMaster _master;
    private readonly IModbusFactory _factory = new ModbusFactory();
    private byte _unitId;

    public bool IsConnected => _client?.Connected ?? false;

    public void Connect(string ipAddress, int port, byte unitId)
    {
        Disconnect();

        _client = new TcpClient();
        var ar = _client.BeginConnect(ipAddress, port, null, null);
        if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3)))
        {
            _client.Close();
            _client = null;
            throw new Exception("Falha ao conectar: Connection Timed Out.");
        }

        _client.EndConnect(ar);
        _client.ReceiveTimeout = 3000;
        _client.SendTimeout = 3000;

        // TCP:
        _master = _factory.CreateMaster(_client);
        _unitId = unitId;
    }

    public void Disconnect()
    {
        _master?.Dispose();
        _master = null;
        _client?.Close();
        _client = null;
    }

    public float ReadValue(ushort address)
    {
        if (!IsConnected || _master == null)
            throw new InvalidOperationException("Cliente MODBUS não está conectado.");

        var regs = _master.ReadHoldingRegisters(_unitId, address, 2);
        return ConvertRegistersToFloat(regs);
    }

    private static float ConvertRegistersToFloat(ushort[] regs)
    {
        if (regs == null || regs.Length != 2)
            throw new ArgumentException("São necessários 2 registradores (32 bits).");

        uint raw = ((uint)regs[0] << 16) | regs[1];
        var bytes = BitConverter.GetBytes(raw);
        return BitConverter.ToSingle(bytes, 0);
    }

    public void Dispose() => Disconnect();
}
