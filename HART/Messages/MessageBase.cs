﻿using System;
using System.Collections;

namespace HART.Messages
{
    /// <summary>
    /// Базовый абстрактный класс описывающий сообщения передаваемые по протоколу HART. 
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// Символ преамбулы.
        /// </summary>
        public const byte PreambleSymbol = 0xFF;

        /// <summary>
        /// Ограничитель.
        /// </summary>
        protected byte Limiter;

        /// <summary>
        /// Адрес устройства.
        /// </summary>
        protected byte[] Address;

        /// <summary>
        /// Данные.
        /// </summary>
        protected byte[] Data;

        /// <summary>
        /// Создать сообщение.
        /// </summary>
        /// <param name="address">Адрес slave-устройства.</param>
        protected MessageBase(byte[] address)
        {
            Address = address;
        }

        /// <summary>
        /// Количество символов в преамбуле.
        /// </summary>
        public int Preamble { get; set; }

        /// <summary>
        /// Формат кадра.
        /// </summary>
        public FrameFormats FrameFormat { get; protected set; }

        /// <summary>
        /// Команда.
        /// </summary>
        public int Command { get; set; }

        /// <summary>
        /// Преобразовать передаваемые данные в массив байтов.
        /// <para>Поддерживаются следующие типы выходных данных:</para>
        /// <list type="bullet">
        /// <item><see cref="string"/></item>
        /// <item><see cref="float"/></item>
        /// <item><see cref="double"/></item>
        /// <item><see cref="ushort"/></item>
        /// <item><see cref="uint"/></item>
        /// <item><see cref="DateTime"/></item>
        /// <item><see cref="BitArray"/></item>
        /// </list>
        /// </summary>
        public void AddDate<T>(T value) => Data = Convert.ToByte(value);

        /// <summary>
        /// Получить данные в формате <typeparamref name="T"/>.
        /// <para>Поддерживаются следующие типы выходных данных:</para>
        /// <list type="bullet">
        /// <item><see cref="string"/></item>
        /// <item><see cref="float"/></item>
        /// <item><see cref="double"/></item>
        /// <item><see cref="ushort"/></item>
        /// <item><see cref="uint"/></item>
        /// <item><see cref="DateTime"/></item>
        /// <item><see cref="BitArray"/></item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">Тип выходных данных.</typeparam>
        public T GetDate<T>()
        {
            switch (typeof(T).Name)
            {
                case "String": return (T)(object)Convert.FromByte<string>(Data);
                case "Single": return (T)(object)Convert.FromByte<float>(Data);
                case "Double": return (T)(object)Convert.FromByte<double>(Data);
                case "UInt16": return (T)(object)Convert.FromByte<ushort>(Data);
                case "UInt32": return (T)(object)Convert.FromByte<uint>(Data);
                case "DateTime": return (T)(object)Convert.FromByte<DateTime>(Data);
                case "BitArray": return (T)(object)Convert.FromByte<BitArray>(Data);

                default: return (T)(object)Data;
            }
        }
    }
}
