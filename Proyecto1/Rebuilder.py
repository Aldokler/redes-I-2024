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

with open("C:/Users/britn/Documents/REDES/Proyecto1/TheBible.txt", "a") as f:

    while(s.available() == 0):
        head = s.read(8)
        int tmp = s.read() << 8 | s.read(1)
        data = s.read(tmp)

        f.write(data.decode('utf8'))

        s.write(NEXTPACKET.to_bytes())

f.close