#include <SoftwareSerial.h>
#include <stdint.h>

#define NEXTPACKET 1
#define RESENDPACKET 2
#define FINAL 3
#define TIMEOUT 10000

int counter = 0;
char confirm;
int timer;
boolean completed;

SoftwareSerial softSerial(8, 9);  // RX, TX 

uint32_t crc_create(const uint8_t *data, short length) {
    uint32_t crc = 0xFFFFFFFF;
    uint32_t polynomial = 0xEDB88320;
    int total_length = length + 11;

    for (size_t i = 11; i < total_length; ++i) {
        crc ^= data[i];
        for (uint8_t j = 0; j < 8; ++j) {
            if (crc & 1) {
                crc = (crc >> 1) ^ polynomial;
            } else {
                crc >>= 1;
            }
        }
    }

    return crc;
}


void setup() {

  Serial.begin(9600); 
  softSerial.begin(9600);
}

byte data[614];

void loop() {
   if(Serial.available()){
    Serial.readBytes(data, 9);
    
    short tmp = (Serial.read() << 8) | (Serial.read());

    data[9] = (byte)((tmp >> 8) & 0xff);
    data[10] = (byte)(tmp & 0xff );

    Serial.readBytes(data + 11, tmp);
    data[11 + tmp + 1] = Serial.read();

    uint32_t crc_number = crc_create(data, tmp);
    data[4] = (crc_number >> 24) & 0xFF;
    data[5] = (crc_number >> 16) & 0xFF;
    data[6] = (crc_number  >> 8)  & 0xFF;
    data[7] = crc_number & 0xFF;


    completed = false;
      while (completed == false){
          softSerial.write(data, 11 + tmp + 1);
          softSerial.flush();

          timer = millis();
          while(softSerial.available() == 0 && millis() - timer > TIMEOUT){
            byte code = softSerial.read();
            if(code == NEXTPACKET){
              completed = true;
              break;
            }else if (code == RESENDPACKET){
              break;
            }
            timer++;
          }
      }
      Serial.write(NEXTPACKET);
    }


}