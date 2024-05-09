#include <SoftwareSerial.h>
#include <LiquidCrystal.h>

#define NEXTPACKET 1
#define RESENDPACKET 2
#define FINAL 3

SoftwareSerial softSerial(8, 9);  // RX, TX 

const int rs = 12, en = 11, d4 = 5, d5 = 4, d6 = 3, d7 = 2;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

void setup() {
  lcd.begin(16, 2);

  Serial.begin(115200); 
  softSerial.begin(9600);
}

byte data[609];

void loop() {
  if(softSerial.available() > 1){
    lcd.setCursor(0, 0);
    lcd.print("Recibiendo trama");


    short id = (Serial.read() << 8) | (Serial.read());

    //Do something with id

    data[0] = (byte)((id >> 8) & 0xff);
    data[1] = (byte)(id & 0xff );
    
    Serial.readBytes(data + 2, 6);
    
    short tmp = (Serial.read() << 8) | (Serial.read());

    data[8] = (byte)((tmp >> 8) & 0xff);
    data[9] = (byte)(tmp & 0xff );

    Serial.readBytes(data + 10, tmp);

    //chequeo de CRC

    Serial.write(data, 10 + tmp);

    softSerial.write(NEXTPACKET);

    
    } else {
      lcd.setCursor(0, 0);
      lcd.print(" Esperando trama");
    }

  }
  
