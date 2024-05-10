import struct
import serial 
import os, ctypes
import time


NEXTPACKET = 1
RESENDPACKET = 2
FINAL = 3

s = serial.Serial('COM3', 115200)

data = []
count = 0
typing = 1
error = 234
completed = False
size = 600

with open(os.getcwd() + "/lorem1.txt", "a") as f:

    while(True):
        while(s.in_waiting > 0):
            head = s.read(8)
            tmp = s.read() << 8 | s.read(1)
            data = s.read(tmp)

            f.write(data.decode('utf8'))

            s.write(NEXTPACKET.to_bytes())

f.close
