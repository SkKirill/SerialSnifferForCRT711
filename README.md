# CRT 711

#doubt  - сомнение в чем-то или несостыковка с мануалом выявленная в ходе проверки
## Команда инициализации CRT

> Документация 4.1

> Пример команд:  `0xF2 'ADDR' 0x00 0x03 0x43 0x30 'PM' 0x03 'BCC'`
> >  **PM**=`0x30` -> `0xF2  0x00  0x00  0x03  0x43  0x30  0x30  0x03  0xB1`
> >  **PM**=`0x31` -> `0xF2  0x00  0x00  0x03  0x43  0x30  0x31  0x03  0xB0`

<details><summary>Примечание</summary>
 
- Это ***первая команда после включения питания***, в противном случае другая команда не может быть выполнена.
	- #doubt ***Другие команды выполнялись***
- После ее выполнения в первый раз ***устройство автоматически проверит и измерит скорость передачи данных*** по хосту.
- Если CRT-711 инициализирован в режиме отключения, карта не будет принята этой командой.
- CRT-711 находится в состоянии запрета и предварительно очищает все коды ошибок, а затем возвращает информацию о версии программного обеспечения.
- ***Отключает ввод карт с передней панели.***
</details>
<details><summary> Pm (параметр команды)</summary>

- Если в CRT-711 нет карточки, двигатель слегка повернется, чтобы очистить карточку в устройстве для укладки.
- Если в CRT-711 есть карточки, порядок их удаления показан ниже:

| PM     | Действие                                                                      |
| ------ | ----------------------------------------------------------------------------- |
| `0x30` | Переместит карточку в положение для удержания лицевой стороной.               |
| `0x31` | Опустит карту в ячейку для карточек с ошибками.                               |
| `0x33` | Если карта находится внутри CRT-711, не перемещает карту.                     |
| `0x34` | Аналогично **pm=`0x30`**, и счетчик ячеек для карточек с ошибками заработает. |
| `0x35` | Аналогично **pm=`0x31`**, и будет работать ячейка для карточек ошибок.        |
| `0x37` | Аналогично **pm=`0x33`**, и будет работать ячейка для карточек ошибок.        |
</details>
<details><summary> CRT возвращает text package</summary>

- `CM` , `PM` -> в нашем случае это `0x30  0x31` или `0x30  0x30`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA** -> `CRT-711-V10` -> версия программного обеспечения 
	- #doubt  ***у нас возвращает*** ->`CRT-711-V1.0`
</details>

---
## Получение статуса с CRT

> Документация 4.2

> Пример команд:  `0xF2 'ADDR' 0x00 0x03 0x43 0x31 'PM' 0x03 'BCC'`
> >  **PM**=`0x30` -> `0xF2  0x00  0x00  0x03  0x43  0x31  0x30  0x03  0xB0`
> >  **PM**=`0x31` -> `0xF2  0x00  0x00  0x03  0x43  0x31  0x31  0x03  0xB1`

<details><summary> Pm (параметр команды)</summary>

| PM     | Действие                                                                                                    |
| ------ | ----------------------------------------------------------------------------------------------------------- |
| `0x30` | Сообщает текущее состояние о наличии карточки в аппарате.                                                   |
| `0x31` | Сообщает текущее состояние о наличии карточки в аппарате, а также возвращает информацию о датчике (4 байта) |
</details>
<details><summary> CRT возвращает text package</summary>

- **`CM` , `PM`** -> в нашем случае это `0x31 0x31` или `0x31 0x30`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA**  или **Sensor(4 byte)**

Расположение датчика указано на чертеже внешнего вида.

<table>
	<thead>
        <tr>
            <th>Sensor / Датчик</th>
            <th>Status</th>
        </tr>
    </thead>
	<tbody>
		<tr>
	        <td rowspan="2">S1</td>
	        <td>30H - Нет карты</td>
	    </tr>
	    <tr>
	        <td>31H - Есть карта</td>
	    </tr>
		<tr>
	        <td rowspan="2">S2</td>
	        <td>30H - Нет карты</td>
	    </tr>
	    <tr>
	        <td>31H - Есть карта</td>
	    </tr>
	    <tr>
	        <td rowspan="2">S3</td>
	        <td>30H - Нет карты</td>
	    </tr>
	    <tr>
	        <td>31H - Есть карта</td>
	    </tr>
	    <tr>
	        <td rowspan="2">S4</td>
	        <td>30H - Нет карты</td>
	    </tr>
	    <tr>
	        <td>31H - Есть карта</td>
	    </tr>
	   </tbody>
</table>
</details>

---
## Передвижение / перенос карты

#doubt: параметр `SET` или `DATA` передаваемые CRT скорее всего список команд для выполнения их последовательно.

> Документация 4.3

> Пример команд:  `0xF2 'ADDR' 0x00 'LENL' 0x43 0x31 'PM' 'SET' 0x03 'BCC'`
> >  **PM**=`0x30` -> `0xF2  0x00  0x00  0x03  0x43  0x32  0x30  0x03  0xB3`
> >  **PM**=`0x33` -> `0xF2  0x00  0x00  0x03  0x43  0x32  0x33  0x03  0xB0`
> >  **PM**=`0x39` -> `0xF2  0x00  0x00  0x03  0x43  0x32  0x39  0x03  0xBA

<details><summary> Pm (параметр команды)</summary>

| PM     | Действие                                                                                                                                                                       |
| ------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `0x30` | Переместит карту в положение для удержания карты на лицевой стороне                                                                                                            |
| `0x33` | Уберёт карту в ячейку для карт c ошибкой (если на ячейке для карт ошибкой установлен датчик, то при заполнении ячейки для карт с ошибкой устройство выдаст код ошибки `0xA1`). |
| `0x39` | Переместит карту на лицевую сторону, не удерживая ее в таком положении                                                                                                         |
</details>
<details><summary> CRT возвращает text package</summary>

- **`CM` , `PM`** -> в нашем случае это `0x32 0x30` или `0x32 0x33` или `0x32 0x39`
- [st0](#st0), [st1](#st1), [st2](#st2)
</details>

---
## Активация ввода карты с передней панели

#doubt: параметр `SET` или `DATA` - не выяснил зачем, указано как необязательный, при отсутствии берется дефолтное значение параметра автомотически `0x30`

> Документация 4.4

> Пример команд:  `0xF2 0x08 0x00 'LENL' 0x43 0x31 'PM' 'SET' 0x03 'BCC'`
> >  **PM**=`0x30` -> `0xF2  0x00  0x00  0x03  0x43  0x33  0x30  0x03  0xB2`
> >  **PM**=`0x31` -> `0xF2  0x00  0x00  0x03  0x43  0x33  0x31  0x03  0xB3`

<details><summary> Pm (параметр команды)</summary>

| PM     | Действие                                |
| ------ | --------------------------------------- |
| `0x30` | Активирует ввод карты с передней панели |
| `0x31` | Отключает ввод карты с передней панели  |
</details>
<details><summary> CRT возвращает text package </summary>
- **`CM` , `PM`** -> в нашем случае это `0x33 0x30` или `0x33 0x31`
- [st0](#st0), [st1](#st1), [st2](#st2)
- </details>
<details><summary> Примечание </summary>
- При выполнении команды сброса(инициализации) *документация 4.1* CRT-711 **отключит ввод карты с передней панели**
</details>

---
## Проверка типа карты RF

> Документация 4.5

> Команда:  `0xF2  'ADDR'  0x00  0x03  0x43  0x50  0x31  0x03  0xD0`

<details><summary> Примечание </summary>
- Проверит тип RFID-карты, поднесите карты в положение RFID-карты, выполниться автомотическая проверка типа карты и вернет информацию о карте.
</details>
<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0x50  0x31`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA**  или **Card_type (2 byte)**

| Card_type 1 byte | Card_type 2 byte | Тип                        |
| ---------------- | ---------------- | -------------------------- |
| 0                | 0                | Неизвестный тип RFID-карты |
| 1                | 0                | Mifare one S50Card         |
| 1                | 1                | Mifare one S70Card         |
| 1                | 2                | Mifare one UL Card         |
| 2                | 0                | Type A CPU Card            |
| 3                | 0                | Type B CPU Card            |
</details>

---
## Получение статуса карты RFID

> Документация 4.7.3

> Команда:  `0xF2  'ADDR'  0x00  0x03  0x43  0x60  0x32  0x03  0xE3`

<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0x60  0x32`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA**  или **sti, stj (2 byte)**

| sti | stj | Тип                |
| --- | --- | ------------------ |
| 0   | 0   | Не , RFID-карты    |
| 1   | 0   | Mifare one S50Card |
| 1   | 1   | Mifare one S70Card |
| 1   | 2   | Mifare one UL Card |
| 2   | 0   | Type A CPU Card    |
| 3   | 0   | Type B CPU Card    |
</details>

---
## Считывание ключа из EEPROM из RF модуля

> Документация 4.7.4.2

> Команда:  `0xF2  'ADDR'  0x00  'LENL'  0x43  0x60  0x33  'ks'  'sn'  0x03 'BCC'`

<details><summary> Pm (Параметры команды)</summary>

- `ks` (1 byte): key select (Key A=`0x00`, Key B=`0x01`)
	- #doubt **Это тот же ключ что и устанавливают при загрузке пароля**
- `sn` (1 byte): sector number (`sn`=`0x00`-`0x0F`)
	- #doubt **Это сектор с расположением данного ключа**
</details>
<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0x60  0x33`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA**  или **rdata (8 byte)**
- [Sw1+Sw2](#Sw1+Sw2)
</details>

---
## Чтение QR кода

> Документация 4.8

> Команда:  `0xF2  'ADDR'  0x00  0x03  0x43  0x70  0x30  0x03  0xF1`

<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0x70  0x30`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA**  -> данные QR кода

> При ошибке сканирования вернет `0x70`
</details>

---
## Чтение конфигурации CRT-711

> Документация 4.9

> Команда:  `0xF2  'ADDR'  0x00  0x03  0x43  0xA3  0x30  0x03  0x22`

<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0x70  0x30`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA**  или **Rev** -> конфигурация
</details>

---
## Получение версии CRT-711 

> Документация 4.10

> Пример команд:  `0xF2 'ADDR' 0x00 0x03 0x43 0xA4 'PM' 0x03 'BCC'`
> >  **PM**=`0x30` -> `0xF2  0x00  0x00  0x03  0x43  0xA4  0x30  0x03  0x25`
> >  **PM**=`0x31` -> `0xF2  0x00  0x00  0x03  0x43  0xA4  0x31  0x03  0x24`

<details><summary> Pm (параметр команды)</summary>

| PM     | Действие                                                 |
| ------ | -------------------------------------------------------- |
| `0x30` | Считывание информации о программном обеспечении машины   |
| `0x31` | Считывание информации о программном обеспечении IC-карты |
</details>
<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0xA4  0x30` или `0xA4  0x31`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA**  -> версия
</details>

---
## Количество карт с ошибками в ячейке для карт с ошибками

> Документация 4.11.1

> Команда:  `0xF2  'ADDR'  0x00  0x03  0x43  0xA5  0x30  0x03  0x24`

<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0xA5  0x30`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA**  или **Count (3 byte)**
</details>

---
## Задать значение количества карт с ошибками ячейке для карт с ошибками

> Документация 4.11.2

> Команда:  `0xF2  'ADDR'  0x00  'LENL'  0x43  0xA5  0x31 'Count(3byte)' 0x03  'BCC'`

<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0x70  0x30`
- [st0](#st0), [st1](#st1), [st2](#st2)
</details>

---
## Счетчик считывания и извлечения карт

> Документация 4.12.1

> Команда:  `0xF2  'ADDR'  0x00  0x03  0x43  0xA6  0x30  0x03  0x27`

<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0xA6  0x30`
- [st0](#st0), [st1](#st1), [st2](#st2)
- **DATA**  или **Count (3 byte)**
</details>

---
## Задать значение cчетчику считывания и извлечения карт

> Документация 4.12.2

> Команда:  `0xF2  'ADDR'  0x00  'LENL'  0x43  0xA6  0x31 'Count(3byte)' 0x03  'BCC'`

<details><summary> CRT возвращает text package</summary>
- **`CM` , `PM`** -> в нашем случае это `0xA6  0x31`
- [st0](#st0), [st1](#st1), [st2](#st2)
</details>

---
## Возвращаемые типы

### st0

| st0 | Значение                         |
| --- | -------------------------------- |
| 0   | В аппарате нет карты             |
| 1   | Одна карта в безеле              |
| 2   | Одна карта на позиции RFID-карты |
### st1

| st1 | Значение                             |
| --- | ------------------------------------ |
| 0   | Нет карт в ячейке для карт с ошибкой |
| 1   | Ячейка для карт с ошибкой заполнена  |
### st2

| st2 | Значение                               |
| --- | -------------------------------------- |
| 0   | Зарезервировано для функции расширения |
| 1   | Зарезервировано для функции расширения |
### Sw1+Sw2

| Sw1    | Sw2    |        |
| ------ | ------ | ------ |
| `0x90` | `0x00` | Успех  |
| `0x6F` | `0x00` | Провал |

