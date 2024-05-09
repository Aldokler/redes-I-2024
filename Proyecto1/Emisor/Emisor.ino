#include <SoftwareSerial.h>

#define NEXTPACKET 1
#define RESENDPACKET 2
#define FINAL 3
#define TIMEOUT 10000

int counter = 0;
char confirm;
int timer;
boolean completed;

SoftwareSerial softSerial(8, 9);  // RX, TX 

void setup() {

  Serial.begin(9600); 
  softSerial.begin(9600);
}

byte data[609];

void loop() {
   if(Serial.available() > 1){
    Serial.readBytes(data, 8);
    
    short tmp = (Serial.read() << 8) | (Serial.read());

    data[8] = (byte)(tmp & 0xff);
    data[9] = (byte)((tmp >> 8) & 0xff);

    Serial.readBytes(data + 10, tmp);

    completed = false;
      while (completed == false){
        softSerial.write(data, 10 + tmp);
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
