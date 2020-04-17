﻿using HART.Connectors;
using HART.Messages;

using System;
using System.Collections.Generic;

namespace HART
{
    /// <summary>
    /// Создает и управляет протоколом HART для общения с подключенными slave-устройствами.
    /// </summary>
    public class HartProtocol
    {
        private readonly IConnector _connector;

        /// <summary>
        /// Полученные сообщения.
        /// </summary>
        public Queue<Response> Messages = new Queue<Response>();

        /// <summary>
        /// Открыто ли соединение со slave-устройством.
        /// </summary>
        public bool IsConnected => _connector.IsConnected;

        /// <summary>
        /// Оповещение о том, что получено новое сообщение от slave-устройства.
        /// </summary>
        public event Action NewMessage;

        /// <summary>
        /// Инициализировать соединение со slave-устройством по HART-протоколу.
        /// </summary>
        /// <param name="connector">Интерфейс обмена данными между устройствами.</param>
        public HartProtocol(IConnector connector)
        {
            _connector = connector;
            _connector.DataReceived += GetNewMessage;
        }

        /// <summary>
        /// Открыть соединение со slave-устройством.
        /// </summary>
        public void Connect() => _connector.Connect();

        /// <summary>
        /// Закрыть соединение со slave-устройством.
        /// </summary>
        public void Disconnect() => _connector.Disconnect();

        /// <summary>
        /// Отправить запрос.
        /// </summary>
        /// <param name="message">Сообщение-запрос.</param>
        public void Request(Request message) => _connector.Request(message.Serialize());

        /// <summary>
        /// Получить новое сообщение.
        /// </summary>
        private void GetNewMessage()
        {
            Messages.Enqueue(Response.Deserialize(_connector.Response()));
            NewMessage?.Invoke();
        }
    }
}
