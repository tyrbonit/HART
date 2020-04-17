using System;
using System.Collections;
using System.Text;

namespace HART
{
    /// <summary>
    /// Содержит метода преобразование данных, передаваемых по протоколу HART, в массив байтов и обратно.
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// Преобразовать входные данные в массив байтов.
        /// <para>Поддерживаются следующие типы входных данных:</para>
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
        /// <typeparam name="T">Тип входных данных.</typeparam>
        /// <param name="value">Данные для преобразования.</param>
        /// <returns>Массив байтов.</returns>
        /// <exception cref="ArgumentException">Преобразование типа входных данных не поддерживается.</exception>
        public static byte[] ToByte<T>(T value)
        {
            if (value == null)
                return null;

            switch (value)
            {
                case byte[] ba: return ba;
                case string s: return ToByte(s);
                case float f: return ToByte(f);
                case double d: return ToByte(d);
                case ushort us: return ToByte(us);
                case uint ui: return ToByte(ui);
                case DateTime da: return ToByte(da);
                case BitArray b: return ToByte(b);
                default: throw new ArgumentException($"Преобразование из {typeof(T)} не поддерживается.");
            }
        }

        /// <summary>
        /// Преобразовать массив байтов в тип данных <typeparamref name="T"/>.
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
        /// <param name="value">Массив байтов исходных данных.</param>
        /// <returns>Данные типа <typeparamref name="T"/>.</returns>
        /// /// <exception cref="ArgumentException">Преобразование в заданный тип данных не поддерживается.</exception>
        /// /// <exception cref="ArgumentNullException"><paramref name="value"/> равен <see langword="null"/>.</exception>
        public static T FromByte<T>(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            switch (typeof(T).Name)
            {
                case "String": return (T)(object)FromByteToString(value);
                case "Single": return (T)(object)FromByteToFloat(value);
                case "Double": return (T)(object)FromByteToDouble(value);
                case "UInt16": return (T)(object)FromByteToUInt16(value);
                case "UInt32": return (T)(object)FromByteToUInt32(value);
                case "DateTime": return (T)(object)FromByteToDate(value);
                case "BitArray": return (T)(object)FromByteToBitArray(value);

                default: throw new ArgumentException($"Преобразование в {typeof(T)} не поддерживается.");
            }
        }


        /// <summary>
        /// Преобразовать <see cref="string"/> в массив байтов.
        /// </summary>
        /// <param name="value">Данный для преобразования.</param>
        /// <returns>Массив байтов</returns>
        private static byte[] ToByte(string value) => Encoding.ASCII.GetBytes(value);

        /// <summary>
        /// Преобразовать массив байтов в <see cref="string"/>.
        /// </summary>
        /// <param name="value">Массив байтов для преобразования.</param>
        /// <returns></returns>
        private static string FromByteToString(byte[] value) => Encoding.ASCII.GetString(value);

        /// <summary>
        /// Преобразовать <see cref="float"/> в массив байтов.
        /// </summary>
        /// <param name="value">Данные для преобразования.</param>
        /// <returns>Массив байтов</returns>
        private static byte[] ToByte(float value) => BitConverter.GetBytes(value);

        /// <summary>
        /// Преобразовать массив байтов в <see cref="float"/>.
        /// </summary>
        /// <param name="value">Массив байтов для преобразования.</param>
        /// <returns></returns>
        private static float FromByteToFloat(byte[] value) => BitConverter.ToSingle(value, 0);

        /// <summary>
        /// Преобразовать <see cref="double"/> в массив байтов.
        /// </summary>
        /// <param name="value">Данные для преобразования.</param>
        /// <returns>Массив байтов</returns>
        private static byte[] ToByte(double value) => BitConverter.GetBytes(value);

        /// <summary>
        /// Преобразовать массив байтов в <see cref="double"/>.
        /// </summary>
        /// <param name="value">Массив байтов для преобразования.</param>
        /// <returns></returns>
        private static double FromByteToDouble(byte[] value) => BitConverter.ToDouble(value, 0);

        /// <summary>
        /// Преобразовать <see cref="ushort"/> в массив байтов.
        /// </summary>
        /// <param name="value">Данные для преобразования.</param>
        /// <returns>Массив байтов</returns>
        private static byte[] ToByte(ushort value) => BitConverter.GetBytes(value);

        /// <summary>
        /// Преобразовать массив байтов в <see cref="ushort"/>.
        /// </summary>
        /// <param name="value">Массив байтов для преобразования.</param>
        /// <returns></returns>
        private static ushort FromByteToUInt16(byte[] value) => BitConverter.ToUInt16(value, 0);

        /// <summary>
        /// Преобразовать <see cref="uint"/> в массив байтов.
        /// </summary>
        /// <param name="value">Данные для преобразования.</param>
        /// <returns>Массив байтов</returns>
        private static byte[] ToByte(uint value)
        {
            if (value > 16777215)
                throw new ArgumentOutOfRangeException("Максимально допустимое значение равно 16777215");

            var b = BitConverter.GetBytes(value);
            var result = new byte[3];

            for (var i = 0; i < 3; i++)
                result[i] = b[i];

            return result;
        }

        /// <summary>
        /// Преобразовать массив байтов в <see cref="uint"/>.
        /// </summary>
        /// <param name="value">Массив байтов для преобразования.</param>
        /// <returns></returns>
        private static uint FromByteToUInt32(byte[] value)
        {
            var nByte = new byte[4];
            value.CopyTo(nByte, 0);

            return BitConverter.ToUInt32(nByte, 0);
        }

        /// <summary>
        /// Преобразовать <see cref="DateTime"/> в массив байтов.
        /// </summary>
        /// <param name="value">Данные для преобразования.</param>
        /// <returns>Массив байтов</returns>
        private static byte[] ToByte(DateTime value)
        {
            var bDay = BitConverter.GetBytes(value.Day);
            var bMonth = BitConverter.GetBytes(value.Month);
            var bYear = BitConverter.GetBytes(value.Year - 1900);

            return new byte[] { bDay[0], bMonth[0], bYear[0] };
        }

        /// <summary>
        /// Преобразовать массив байтов в <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value">Массив байтов для преобразования.</param>
        /// <returns></returns>
        private static DateTime FromByteToDate(byte[] value)
        {
            var day = BitConverter.ToInt32(new byte[] { value[0], 0, 0, 0 }, 0);
            var month = BitConverter.ToInt32(new byte[] { value[1], 0, 0, 0 }, 0);
            var year = BitConverter.ToInt32(new byte[] { value[2], 0, 0, 0 }, 0);

            return new DateTime(year + 1900, month, day);
        }

        /// <summary>
        /// Преобразовать <see cref="BitArray"/> в массив байтов.
        /// </summary>
        /// <param name="value">Данные для преобразования.</param>
        /// <returns>Массив байтов</returns>
        private static byte[] ToByte(BitArray value)
        {
            if (value.Count > 8)
                throw new ArgumentOutOfRangeException("Переполнение массива значение. Допускается только 8 элементов в массиве.");

            var result = new byte[1];

            value.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// Преобразовать массив байтов в <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">Массив байтов для преобразования.</param>
        /// <returns></returns>
        private static BitArray FromByteToBitArray(byte[] value)
        {
            if (value.Length > 1)
                throw new ArgumentOutOfRangeException("Переполнение массива значение. Допускается только 1 байт данных.");

            return new BitArray(value);
        }
    }
}
