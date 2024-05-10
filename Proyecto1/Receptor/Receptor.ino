#include <SoftwareSerial.h>
#include <LiquidCrystal.h>
#include <stdint.h>

#define NEXTPACKET 1
#define RESENDPACKET 2
#define FINAL 3

SoftwareSerial softSerial(8, 9);  // RX, TX 

const int rs = 12, en = 11, d4 = 5, d5 = 4, d6 = 3, d7 = 2;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

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
byte oTildada[] = {
  B00010,
  B00100,
  B00000,
  B01110,
  B10001,
  B10001,
  B10001,
  B01110,
};

void setup() {
  lcd.begin(16, 2);
  lcd.createChar(0, oTildada);

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
  if (errores == 0){
    tasaErrores = 0.0;
  }

  char numberToDisplay[10];
  dtostrf(tasaErrores, 5, 2, numberToDisplay);

  lcd.setCursor(0, 0);
  lcd.print("Transmisi");
  lcd.write(byte(0));
  lcd.print("n     ");
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
  if(softSerial.available()){
    lcd.setCursor(0, 0);
    //lcd.print("Recibiendo trama");
    lcd.print("Transmisi");
    lcd.write(byte(0));
    lcd.print("n     ");
    lcd.setCursor(0, 1);
    lcd.print("curso... ");

    data[0] = softSerial.read();
    short id = (softSerial.read() << 8) | (softSerial.read());
    lcd.write(id);
    cantidadTramas = id;

    data[1] = (byte)((id >> 8) & 0xff);
    data[2] = (byte)(id & 0xff );
    
    softSerial.readBytes(data + 3, 6);
    
    short tmp = (softSerial.read() << 8) | (softSerial.read());

    data[9] = (byte)((tmp >> 8) & 0xff);
    data[10] = (byte)(tmp & 0xff );

    softSerial.readBytes(data + 11, tmp);
    data[11 + tmp + 1] = softSerial.read();

    //chequeo de CRC
    uint32_t new_crc_number = crc_create(data, tmp);

    uint32_t crc_number_recieved = 0;
    for (int i = 4; i < 8; i++) {
      crc_number_recieved = (crc_number_recieved << 8) | data[i];
    }

    Serial.print("crc_number_recieved");
    Serial.print(crc_number_recieved);
    Serial.print("new_crc_number");
    Serial.print(new_crc_number);

    if(new_crc_number == crc_number_recieved){

    }
    else{

    }

    lcd.setCursor(0, 0);
    lcd.print("Estado de enlace");
    lcd.setCursor(0, 1);
    //If dió error:
    //errores++;
    //lcd.print("ruidoso...");

    //Else
    lcd.write(byte(0));
    lcd.print("ptimo...");

    Serial.write(data, 11 + tmp + 1);
    

    softSerial.write(NEXTPACKET);
    
    } else {
      lcd.setCursor(0, 0);
      lcd.print(" Esperando trama");
      
      lcd.setCursor(0, 1);
      lcd.print("                ");
    }

  }