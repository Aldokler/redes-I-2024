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
int cantidadTramas = 0;
int errores = 0;

void ceFinit() {
  float e = errores;
  float cT = cantidadTramas;
  float tasaErrores = e*100/cT;

  char numberToDisplay[10];
  dtostrf(tasaErrores, 5, 2, numberToDisplay);

  lcd.setCursor(0, 0);
  lcd.print("Transmisión     ");
  lcd.setCursor(0, 1);
  lcd.print("     Finalizada!");

  delay(3000);

  while(true){
    lcd.setCursor(0, 0);
    lcd.print("Tasa de error   ");
    lcd.setCursor(0, 1);
    lcd.print("estimada: ");
    lcd.print(numberToDisplay);
    lcd.print("%");

  }
}

void loop() {
  ceFinit()
  if(softSerial.available() > 1){
    lcd.setCursor(0, 0);
    //lcd.print("Recibiendo trama");
    lcd.print("Transmisión en  ");
    lcd.setCursor(0, 1);
    lcd.print("curso...");

    data[0] = Serial.read();
    short id = (Serial.read() << 8) | (Serial.read());
    cantidadTramas = id;

    data[1] = (byte)((id >> 8) & 0xff);
    data[2] = (byte)(id & 0xff );
    
    Serial.readBytes(data + 3, 6);
    
    short tmp = (Serial.read() << 8) | (Serial.read());

    data[9] = (byte)((tmp >> 8) & 0xff);
    data[10] = (byte)(tmp & 0xff );

    Serial.readBytes(data + 11, tmp);
    data[11 + tmp + 1] = Serial.read();

    //chequeo de CRC

    //If dió error:
    errores++;

    Serial.write(data, 11 + tmp + 1);
    

    softSerial.write(NEXTPACKET);
    
    } else {
      lcd.setCursor(0, 0);
      lcd.print(" Esperando trama");
      
      lcd.setCursor(0, 1);
      lcd.print("                ");
    }

  }
  
