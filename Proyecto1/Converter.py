import struct
import serial
import os, ctypes
import time


NEXTPACKET = 1
RESENDPACKET = 2
FINAL = 3

s = serial.Serial('COM2', 9600)

data = []
count = 0
typing = 1
error = 234
completed = False
size = 600
start = 1
end = 2

with open("C:/Users/britn/Documents/REDES/Proyecto1/TheBible.txt", "rb") as f:

    while (byte := f.read(600)):
        trama = bytearray()
        trama.extend(start.to_bytes(1, 'big'))
        trama.extend(count.to_bytes(2, 'big'))
        trama.extend(typing.to_bytes(1, 'big'))
        trama.extend(error.to_bytes(4, 'big'))
        trama.extend(typing.to_bytes(1, 'big'))
        trama.extend(size.to_bytes(2, 'big'))
        trama.extend(byte)
        trama.extend(end.to_bytes(1, 'big'))

        while (completed == False):
            s.write(trama)
            s.flush()

        while(s.available() == 0):
          code = s.read()
          if(int.from_bytes(code, "big") == NEXTPACKET):
            completed = True
            break

        count+=1
f.close