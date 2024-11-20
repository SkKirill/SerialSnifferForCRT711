namespace SerialSnifferForCRT711;

public static class CommandListInfo
{
    public const int ValueStx = 0xF2; // Start Character
    private const int ValueAddr = 0x00; // Адрес устройства (дефолтный 15, но у нас он 0x00)
    private const int ValueCmt = 0x43; // (byte)'C' - Команда
    private const int ValueEtx = 0x03; // End Character  
    public static readonly ByteCommand CommandInitialize30 = new(
        "4.1 Reset (Initialization) Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandInitialize31 = new(
        "4.1 Reset (Initialization) Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x31, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandInitialize33 = new(
        "4.1 Reset (Initialization) Command PM=0x33",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x33, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandInitialize34 = new(
        "4.1 Reset (Initialization) Command PM=0x34",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x34, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandInitialize35 = new(
        "4.1 Reset (Initialization) Command PM=0x35",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x35, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandInitialize37 = new(
        "4.1 Reset (Initialization) Command PM=0x37",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x30, 0x37, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandStatusRequest30 = new(
        "4.2 Status Check Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x31, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandStatusRequest31 = new(
        "4.2 Status Check Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x31, 0x31, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandCardMove30 = new(
        "4.3 Carry Card Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x32, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandCardMove33 = new(
        "4.3 Carry Card Command PM=0x33",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x32, 0x33, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandCardMove39 = new(
        "4.3 Carry Card Command PM=0x39",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x32, 0x39, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandCardEntry30 = new(
        "4.4 Front Entry Enable Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x33, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandCardEntry31 = new(
        "4.4 Front Entry Enable Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x33, 0x31, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandRfidCardType = new(
        "4.5 Check RF Card Type",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x50, 0x31, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl3030 = new(
        "4.6.1 SAM Card Reset (Activation) PM=0x30 Vcc=0x30",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x30, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl3033 = new(
        "4.6.1 SAM Card Reset (Activation) PM=0x30 Vcc=0x33",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x30, 0x33, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl3035 = new(
        "4.6.1 SAM Card Reset (Activation) PM=0x30 Vcc=0x35",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x30, 0x35, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl31 = new(
        "4.6.2 Deactivate SAM Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x52, 0x31, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl32 = new(
        "4.6.3 Inquire SAM Status Command PM=0x32",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x52, 0x32, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl33 = new(
        "4.6.4 SAM APDU operation T=0 protocol PM=0x33 C-APDU=0x00, 0x00, 0x00, 0x00",
        [ValueStx, ValueAddr, 0x00, 0x07, ValueCmt, 0x52, 0x33, 0x00, 0x00, 0x00, 0x00, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl34 = new(
        "4.6.5 SAM APDU operation T=1 protocol PM=0x34 C-APDU=0x00, 0x00, 0x00, 0x00",
        [ValueStx, ValueAddr, 0x00, 0x07, ValueCmt, 0x52, 0x34, 0x00, 0x00, 0x00, 0x00, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl38 = new(
        "4.6.6 SAM Warm Reset PM=0x38",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x52, 0x38, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl39 = new(
        "4.6.7 Auto-Distinguish SAM Card T=0/T=1 Protocol PM=0x39 C-APDU=0x00, 0x00, 0x00, 0x00",
        [ValueStx, ValueAddr, 0x00, 0x07, ValueCmt, 0x52, 0x39, 0x00, 0x00, 0x00, 0x00, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl4030 = new(
        "4.6.8 Select SAM Stand PM=0x40 SAMn=0x30",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x40, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl4031 = new(
        "4.6.8 Select SAM Stand PM=0x40 SAMn=0x31",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x40, 0x31, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl4032 = new(
        "4.6.8 Select SAM Stand PM=0x40 SAMn=0x32",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x40, 0x32, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandSamCardControl4033 = new(
        "4.6.8 Select SAM Stand PM=0x40 SAMn=0x33",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x52, 0x40, 0x33, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandRfidCardControl1356MHz30 = new(
        "4.7.1 Activated contactless IC card PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x30, ValueEtx, 0x00]
    );
    
    public static readonly ByteCommand CommandRfidCardControl1356MHz31 = new(
        "4.7.2 Deactivate RFID card PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x31, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandRfidCardControl1356MHz32 = new(
        "4.7.3 Inquire status of RFID card PM=0x32",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x32, ValueEtx, 0x00]
    );
    /*public static readonly ByteCommand CommandRfidCardControl1356MHz33 = new(
    "4.7.4.1 Download password to EEPROM and 4.7.4.2 Read Key from EEPROM and 4.7.4.4 Upload key from EEPROM" +
    " “C” 60H 33H 00H D0H ks sn pdata and “C” 60H 33H 00H 21H ks sn and “C” 60H 33H 00H 21H ks sn PM=0x33",
    [
        ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x33, 0x00, 0xD0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, ValueEtx, 0x00
    ]
);*/
    /*public static readonly ByteCommand CommandRfidCardControl1356MHz34 = new(
    "4.7.5 Type A RF card communication PM=0x34",
    [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x34, ValueEtx, 0x00]
);*/
    /*public static readonly ByteCommand CommandRfidCardControl1356MHz35 = new(
    "4.7.6 Type B RF card communication PM=0x35",
    [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x35, ValueEtx, 0x00]
);*/
    public static readonly ByteCommand CommandRfidCardControl1356MHz3930 = new(
        "4.7.7 RFID card sleep and wake-up operation PM=0x39 Set=30",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x60, 0x39, 0x30, ValueEtx, 0x00]
    );
    
    public static readonly ByteCommand CommandRfidCardControl1356MHz3931 = new(
        "4.7.7 RFID card sleep and wake-up operation PM=0x39 Set=31",
        [ValueStx, ValueAddr, 0x00, 0x04, ValueCmt, 0x60, 0x39, 0x31, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandBarcodeScan = new(
        "4.8 Barcode scanning",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x70, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandReadMachineConfigInformation = new(
        "4.9 Read CRT-711 configuration",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA3, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandReadMachineVersion30 = new(
        "4.10 Read CRT-711 version information Pm=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA4, 0x30, ValueEtx, 0x00]
    );
    
    public static readonly ByteCommand CommandReadMachineVersion31 = new(
        "4.10 Read CRT-711 version information Pm=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA4, 0x31, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandErrorCardBinCounter30 = new(
        "4.11.1 Read error-card bin counter PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA5, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandErrorCardBinCounter31 = new(
        "4.11.2 Set initial value of error-card bin PM=0x31 Count=0x00, 0x00, 0x00",
        [ValueStx, ValueAddr, 0x00, 0x06, ValueCmt, 0xA5, 0x31, 0x00, 0x00, 0x01, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandEjectCardCounter30 = new(
        "4.12.1 Read eject card counter PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA6, 0x30, ValueEtx, 0x00]
    );
    public static readonly ByteCommand CommandEjectCardCounter31 = new(
        "4.12.2 Set initial value of eject card bin PM=0x31 Count=0x00",
        [ValueStx, ValueAddr, 0x00, 0x06, ValueCmt, 0xA6, 0x31, 0x00, 0x00, 0x01, ValueEtx, 0x00]
    );
}