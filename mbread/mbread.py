
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
client = ModbusClient('192.168.15.55')
client = ModbusClient('127.0.0.1')
printResponse(client.read_input_registers(4000, 2, unit = 1))
printResponse(client.read_input_registers(4003, 2, unit = 1))
printResponse(client.read_input_registers(4006, 2, unit = 1))
printResponse(client.read_input_registers(4009, 2, unit = 1))

printResponse(client.read_input_registers(4064, 1, unit = 1))
printResponse(client.read_input_registers(4065, 1, unit = 1))
printResponse(client.read_input_registers(4066, 1, unit = 1))
printResponse(client.read_input_registers(4067, 1, unit = 1))

client.close()