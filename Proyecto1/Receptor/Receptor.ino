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
    Serial.readBytes(data, 8);
    
    short tmp = (Serial.read() << 8) | (Serial.read());

    Serial.readBytes(data + 10, tmp);

    Serial.write(data, 10 + tmp);

    //chequeo de CRC

    softSerial.write(NEXTPACKET);

    
    } else {
      lcd.setCursor(0, 0);
      lcd.print(" Esperando trama");
    }

  }
  
