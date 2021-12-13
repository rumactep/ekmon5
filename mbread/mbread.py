
from pymodbus.client.sync import ModbusTcpClient as ModbusClient
import pymodbus
import struct

def printResponse(response):
    if response.isError():
        print(f'Error message: {response}')
    else:
        if len(response.registers) == 2: # perhaps it is float
            print(ushort2float((response.registers[0], response.registers[1])))
        else :
            print(response.registers)

def ushort2float(ff) :
    ps = struct.pack("HH", *ff) # "HH" for ushort ushort, "hh" for short short
    uf = struct.unpack("f", ps)[0]
    return [uf]


# Modbus Reader for reading specified modbus registers via read_input_registers function
#client = ModbusClient('127.0.0.1')
#client = ModbusClient('192.168.15.55')
#client = ModbusClient('192.168.8.156')
client = ModbusClient('127.0.0.1')
unitid = 4
printResponse(client.read_input_registers(1, 1, unit = unitid))
printResponse(client.read_input_registers(2, 1, unit = unitid))
printResponse(client.read_input_registers(3, 1, unit = unitid))

printResponse(client.read_input_registers(4, 2, unit = unitid))
printResponse(client.read_input_registers(5, 2, unit = unitid))

printResponse(client.read_input_registers(6, 2, unit = unitid))
printResponse(client.read_input_registers(7, 2, unit = unitid))

client.close()
