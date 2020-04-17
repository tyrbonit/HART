using HART.Messages;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Linq;

namespace HART.Connectors
{
    /// <summary>
    /// Осуществляет возможность подключения к slave-устройству по серийному порту.
    /// </summary>
    public class SerialPortAdapter : IConnector
    {
        private readonly SerialPort _serialPort;
        private readonly Queue<byte[]> ReceivedData = new Queue<byte[]>();
        private readonly List<byte> _newData = new List<byte>();

        private bool _isPreambleSought = false;
        private bool _isPreambleFound = false;
        private bool _isAddressFound = false;
        private bool _AddressIsLong = false;
        private bool _isCommandFound = false;
        private bool _commandIsLong = false;
        private bool _isCounterFound = false;
        private bool _isDataFound = false;
        private int _numberOfPreambleBytes = 0;
        private int _numberOfDataBytes = 0;
        private int _numberOfAddressBytes = 5;

        /// <summary>
        /// Открыт ли порт.
        /// </summary>
        public bool IsConnected => _serialPort.IsOpen;

        /// <summary>
        /// Время ожидания в милисекундах для завершения операции чтения.
        /// </summary>
        public int ReadTimeout
        {
            get => _serialPort.ReadTimeout;
            set => _serialPort.ReadTimeout = value;
        }

        /// <summary>
        /// Время ожидания в милисекундах для завершения операции чтения.
        /// </summary>
        public int WriteTimeout
        {
            get => _serialPort.ReadTimeout;
            set => _serialPort.ReadTimeout = value;
        }

        /// <summary>
        /// Оповещение о том, что сформированно новое сообщение от slave-устройства.
        /// </summary>
        public event Action DataReceived;

        /// <summary>
        /// Создать серийный порт для подключения.
        /// </summary>
        /// <param name="portNumber">Номер порта (цифра после COM, например 1, 2, … Чтобы получилось название порта).</param>
        /// <param name="baudRate">Скорость передачи в бодах.</param>
        /// <param name="parity">Протокол контроля чётности.</param>
        /// <param name="dataBits">Число битов данных.</param>
        /// <param name="stopBits">Число стоповых битов в байте.</param>
        public SerialPortAdapter(int portNumber, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _serialPort = new SerialPort($"COM{portNumber}", baudRate, parity, dataBits, stopBits)
            {
                NewLine = Environment.NewLine
            };

            _serialPort.DataReceived += DataReceivedHandler;

            ReadTimeout = 1000;
            WriteTimeout = 1000;
        }



        /// <summary>
        /// Создать серийный порт для подключения.
        /// </summary>
        /// <param name="portNumber">Номер порта (цифра после COM, например 1, 2, … Чтобы получилось название порта).</param>
        public SerialPortAdapter(int portNumber) : this(portNumber, 1200, Parity.Odd, 8, StopBits.One)
        {
        }

        /// <summary>
        /// Проверить доступен ли указааный порт ввода/вывода.
        /// </summary>
        /// <param name="portNumber">Номер порта ввода/вывода.</param>
        /// <returns><see langword="true"/>, если порт доступен.</returns>        
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Ожидание>")]
        public static bool IsPortAccessible(int portNumber)
        {
            var result = false;
            var portName = $"COM{portNumber}";

            if (SerialPort.GetPortNames().Contains(portName))
            {
                var port = new SerialPort(portName);
                try
                {
                    port.Open();

                    if (port.IsOpen)
                    {
                        result = true;
                        port.Close();
                    }
                }
                catch (Exception)
                {
                    port.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// Открыть соединение.
        /// </summary>
        public void Connect() => _serialPort.Open();

        /// <summary>
        /// Закрыть соединение.
        /// </summary>
        public void Disconnect() => _serialPort.Close();

        /// <summary>
        /// Отправить запрос.
        /// </summary>
        /// <param name="buffer">Массив передаваемых данных.</param>
        public void Request(byte[] buffer) => _serialPort.Write(buffer, 0, buffer.Length);

        /// <summary>
        /// Получить ответ.
        /// </summary>
        /// <returns>Массив полученных байтов.</returns>
        public byte[] Response() => ReceivedData.Dequeue();

        /// <summary>
        /// Обработчик события <see cref="SerialPort.DataReceived"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            var count = _serialPort.BytesToRead;
            var buffer = new byte[count];
            _serialPort.Read(buffer, 0, count);

            for (var i = 0; i < count; i++)
            {
                var value = buffer[i];

                if (!_isPreambleFound)
                {
                    if (!_isPreambleSought)
                    {
                        if (value == MessageBase.PreambleSymbol)
                        {
                            _numberOfPreambleBytes++;
                            _isPreambleSought = true;
                            _newData.Add(value);
                            continue;
                        }
                        else continue;
                    }
                    else
                    {
                        if (value == MessageBase.PreambleSymbol)
                        {
                            _numberOfPreambleBytes++;
                            _newData.Add(value);
                            continue;
                        }
                        else
                        {
                            var isLimiter = value == 0x01 || value == 0x06 || value == 0x81 || value == 0x86;
                            if (_numberOfPreambleBytes >= 2 && isLimiter)
                            {
                                _isPreambleSought = false;
                                _isPreambleFound = true;
                                _AddressIsLong = value == 0x86 || value == 0x81;
                                _newData.Add(value);
                                continue;
                            }
                            else
                            {
                                _isPreambleSought = false;
                                _isPreambleFound = false;
                                _newData.Clear();
                                ResetStatusMessage();
                                continue;
                            }
                        }
                    }
                }

                if (!_isAddressFound)
                {
                    if (_AddressIsLong)
                    {
                        if (_numberOfAddressBytes > 0)
                        {
                            _numberOfAddressBytes--;
                            _newData.Add(value);
                            continue;
                        }
                        else
                        {
                            _isAddressFound = true;
                        }
                    }
                    else
                    {
                        _isAddressFound = true;
                        _newData.Add(value);
                        continue;
                    }
                }

                if (!_isCommandFound)
                {
                    if (!_commandIsLong)
                    {
                        _commandIsLong = value == 254;
                        _isCommandFound = !_commandIsLong;
                        _newData.Add(value);
                        continue;
                    }
                    else
                    {
                        _isCommandFound = true;
                        _newData.Add(value);
                        continue;
                    }
                }

                if (!_isCounterFound)
                {
                    _isCounterFound = true;
                    _numberOfDataBytes = value;
                    _newData.Add(value);
                    continue;
                }

                if (!_isDataFound)
                {
                    if (_numberOfDataBytes > 0)
                    {
                        _numberOfDataBytes--;
                        _newData.Add(value);
                        continue;
                    }
                    else
                    {
                        _isDataFound = true;
                    }
                }

                _newData.Add(value);
                ReceivedData.Enqueue(_newData.ToArray());
                DataReceived?.Invoke();
                ResetStatusMessage();
            }
        }

        /// <summary>
        /// Сбросить флаги, отвечающие за фомирование нового сообщения.
        /// </summary>
        private void ResetStatusMessage()
        {
            _isPreambleSought = false;
            _isPreambleFound = false;
            _isAddressFound = false;
            _AddressIsLong = false;
            _isCommandFound = false;
            _commandIsLong = false;
            _isCounterFound = false;
            _isDataFound = false;

            _numberOfPreambleBytes = 0;
            _numberOfDataBytes = 0;
            _numberOfAddressBytes = 5;

            _newData.Clear();
        }
    }
}
