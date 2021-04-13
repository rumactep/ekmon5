
from pymodbus.client.sync import ModbusTcpClient as ModbusClient
import pymodbus

def printResponse(response):
    if response.isError():
        print(f'Error message: {response}')
    else:
        print(response.registers)

# Modbus Reader for reading specified modbus registers via read_input_registers function
client = ModbusClient('127.0.0.1')
printResponse(client.read_input_registers(30011, 6, unit = 2))
printResponse(client.read_input_registers(30011, 6, unit = 1))
printResponse(client.read_input_registers(30011, 6, unit = 0))
#printResponse(client.read_input_registers(30011, 6, unit = 3))
