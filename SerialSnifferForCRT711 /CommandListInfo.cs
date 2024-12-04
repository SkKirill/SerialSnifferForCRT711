namespace SerialSnifferForCRT711;

public static class CommandListInfo
{
    public const int ValueStx = 0xF2; // Start Character
    private const int ValueAddr = 0x00; // Адрес устройства (дефолтный 15, но у нас он 0x00)
    private const int ValueCmt = 0x43; // (byte)'C' - Команда
    public const int ValueEtx = 0x03; // End Character

    /// <summary>
    /// Команды инициализации работы с CRT
    /// </summary>
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
    
    /// <summary>
    /// Команды получения статуса с CRT
    /// </summary>
    public static readonly ByteCommand CommandStatusRequest = new(
        "4.2 Status Check Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x31, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandStatusRequestWithSensor = new(
        "4.2 Status Check + Status Sensor Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x31, 0x31, ValueEtx, 0x00]
    );

    /// <summary>
    /// Команды передвижения карты внутри CRT
    /// </summary>
    public static readonly ByteCommand CommandCardMoveToFrontWithHolding = new(
        "4.3 Carry Card Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x32, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandCardMoveToErrorBin = new(
        "4.3 Carry Card Command PM=0x33",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x32, 0x33, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandCardMoveToFrontWithoutHolding = new(
        "4.3 Carry Card Command PM=0x39",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x32, 0x39, ValueEtx, 0x00]
    );

    /// <summary>
    /// Команды включения и выключения приема карты
    /// </summary>
    public static readonly ByteCommand CommandCardEntryEnable = new(
        "4.4 Front Entry Enable Command PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x33, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandCardEntryDisable = new(
        "4.4 Front Entry Disable Command PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x33, 0x31, ValueEtx, 0x00]
    );

    /// <summary>
    /// Команда получения типа карты который находится в положении RFID карты
    /// </summary>
    public static readonly ByteCommand CommandRfidCardCheckType = new(
        "4.5 Check RF Card Type",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x50, 0x31, ValueEtx, 0x00]
    );

    /// <summary>
    /// Команда активации прибора считывания информации с RFID карты
    /// </summary>
    public static readonly ByteCommand CommandRfidCardActivation = new(
        "4.7.1 Activated contactless IC card",
        [ValueStx, ValueAddr, 0x00, 0x05, ValueCmt, 0x60, 0x30, (byte)'A', (byte)'B', ValueEtx, 0x00]
    );

    /// <summary>
    /// Команда выключения устройства считывания информации с RFID карты
    /// </summary>
    public static readonly ByteCommand CommandRfidCardDeactivation = new(
        "4.7.2 Deactivate RFID card PM=0x31",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x31, ValueEtx, 0x00]
    );

    /// <summary>
    /// Команда получения информации с устройства считывания информации с RFID карты
    /// </summary>
    public static readonly ByteCommand CommandRfidCardStatus = new(
        "4.7.3 Inquire status of RFID card PM=0x32",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0x60, 0x32, ValueEtx, 0x00]
    );

    /// <summary>
    /// Команды считывания конфигурации и версии устройства считывания информации с RFID карты
    /// </summary>
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

    /// <summary>
    /// Команды считывания и указания новых значений в счетчики карт
    /// </summary>
    public static readonly ByteCommand CommandErrorCardBinCounterRead = new(
        "4.11.1 Read error-card bin counter PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA5, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandErrorCardBinCounterSet = new(
        "4.11.2 Set initial value of error-card bin PM=0x31 Count=0x30, 0x30, 0x30",
        [ValueStx, ValueAddr, 0x00, 0x06, ValueCmt, 0xA5, 0x31, 0x30, 0x30, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandEjectCardCounterRead = new(
        "4.12.1 Read eject card counter PM=0x30",
        [ValueStx, ValueAddr, 0x00, 0x03, ValueCmt, 0xA6, 0x30, ValueEtx, 0x00]
    );

    public static readonly ByteCommand CommandEjectCardCounterSet = new(
        "4.12.2 Set initial value of eject card bin PM=0x31 Count=0x30, 0x30, 0x30",
        [ValueStx, ValueAddr, 0x00, 0x06, ValueCmt, 0xA6, 0x31, 0x30, 0x30, 0x30, ValueEtx, 0x00]
    );
}