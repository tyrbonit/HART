using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace HART.Tests
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        public void To_From_Converter_Test()
        {
            #region string
            var string1 = "@!$dsf";
            var string2 = "Hello world.";
            var string3 = "12qwe4326";

            var bString1 = Convert.ToByte(string1);
            var bString2 = Convert.ToByte(string2);
            var bString3 = Convert.ToByte(string3);

            var sString1 = Convert.FromByte<string>(bString1);
            var sString2 = Convert.FromByte<string>(bString2);
            var sString3 = Convert.FromByte<string>(bString3);

            Assert.IsTrue(string1 == sString1);
            Assert.IsTrue(string2 == sString2);
            Assert.IsTrue(string3 == sString3);
            #endregion

            #region float
            var float1 = 2345.23456F;
            var float2 = -0.342342342342342342342F;
            var float3 = float.MaxValue;
            var float4 = float.MinValue;
            var float5 = float.PositiveInfinity;
            var float6 = float.NegativeInfinity;
            var float7 = float.NaN;
            var float8 = float.Epsilon; ;

            var bFloat1 = Convert.ToByte(float1);
            var bFloat2 = Convert.ToByte(float2);
            var bFloat3 = Convert.ToByte(float3);
            var bFloat4 = Convert.ToByte(float4);
            var bFloat5 = Convert.ToByte(float5);
            var bFloat6 = Convert.ToByte(float6);
            var bFloat7 = Convert.ToByte(float7);
            var bFloat8 = Convert.ToByte(float8);

            var fFloat1 = Convert.FromByte<float>(bFloat1);
            var fFloat2 = Convert.FromByte<float>(bFloat2);
            var fFloat3 = Convert.FromByte<float>(bFloat3);
            var fFloat4 = Convert.FromByte<float>(bFloat4);
            var fFloat5 = Convert.FromByte<float>(bFloat5);
            var fFloat6 = Convert.FromByte<float>(bFloat6);
            var fFloat7 = Convert.FromByte<float>(bFloat7);
            var fFloat8 = Convert.FromByte<float>(bFloat8);

            Assert.IsTrue(float1 == fFloat1);
            Assert.IsTrue(float2 == fFloat2);
            Assert.IsTrue(float3 == fFloat3);
            Assert.IsTrue(float4 == fFloat4);
            Assert.IsTrue(float5 == fFloat5);
            Assert.IsTrue(float6 == fFloat6);
            Assert.IsTrue(float.IsNaN(float7) == float.IsNaN(fFloat7));
            Assert.IsTrue(float8 == fFloat8);
            #endregion

            #region double
            var double1 = 2345.23456;
            var double2 = -0.342342342342342342342;
            var double3 = double.MaxValue;
            var double4 = double.MinValue;
            var double5 = double.PositiveInfinity;
            var double6 = double.NegativeInfinity;
            var double7 = double.NaN;
            var double8 = double.Epsilon; ;

            var bDouble1 = Convert.ToByte(double1);
            var bDouble2 = Convert.ToByte(double2);
            var bDouble3 = Convert.ToByte(double3);
            var bDouble4 = Convert.ToByte(double4);
            var bDouble5 = Convert.ToByte(double5);
            var bDouble6 = Convert.ToByte(double6);
            var bDouble7 = Convert.ToByte(double7);
            var bDouble8 = Convert.ToByte(double8);

            var dDouble1 = Convert.FromByte<double>(bDouble1);
            var dDouble2 = Convert.FromByte<double>(bDouble2);
            var dDouble3 = Convert.FromByte<double>(bDouble3);
            var dDouble4 = Convert.FromByte<double>(bDouble4);
            var dDouble5 = Convert.FromByte<double>(bDouble5);
            var dDouble6 = Convert.FromByte<double>(bDouble6);
            var dDouble7 = Convert.FromByte<double>(bDouble7);
            var dDouble8 = Convert.FromByte<double>(bDouble8);

            Assert.IsTrue(double1 == dDouble1);
            Assert.IsTrue(double2 == dDouble2);
            Assert.IsTrue(double3 == dDouble3);
            Assert.IsTrue(double4 == dDouble4);
            Assert.IsTrue(double5 == dDouble5);
            Assert.IsTrue(double6 == dDouble6);
            Assert.IsTrue(double.IsNaN(double7) == double.IsNaN(dDouble7));
            Assert.IsTrue(double8 == dDouble8);
            #endregion

            #region ushort
            ushort ushort1 = 2345;
            var ushort2 = ushort.MaxValue;
            var ushort3 = ushort.MinValue;

            var bUshort1 = Convert.ToByte(ushort1);
            var bshort2 = Convert.ToByte(ushort2);
            var bUshort3 = Convert.ToByte(ushort3);

            var uUshort1 = Convert.FromByte<ushort>(bUshort1);
            var uUshort2 = Convert.FromByte<ushort>(bshort2);
            var uUshort3 = Convert.FromByte<ushort>(bUshort3);

            Assert.IsTrue(ushort1 == uUshort1);
            Assert.IsTrue(ushort2 == uUshort2);
            Assert.IsTrue(ushort3 == uUshort3);
            #endregion

            #region uint
            uint uint1 = 2345;
            uint uint2 = 16777215;
            var uint3 = uint.MinValue;

            var bUint1 = Convert.ToByte(uint1);
            var bUint2 = Convert.ToByte(uint2);
            var bUint3 = Convert.ToByte(uint3);

            var uUint1 = Convert.FromByte<uint>(bUshort1);
            var uUint2 = Convert.FromByte<uint>(bshort2);
            var uUint3 = Convert.FromByte<uint>(bUshort3);

            Assert.IsTrue(ushort1 == uUshort1);
            Assert.IsTrue(ushort2 == uUshort2);
            Assert.IsTrue(ushort3 == uUshort3);
            #endregion

            #region dateTime
            var date = new DateTime(2020, 4, 10);

            var bDate = Convert.ToByte(date);

            var sDate = Convert.FromByte<DateTime>(bDate);

            Assert.IsTrue(sDate == date);
            #endregion

            #region bitArray

            var bitArray = new BitArray(8);
            bitArray.Set(0, false);
            bitArray.Set(1, false);
            bitArray.Set(2, false);
            bitArray.Set(3, false);
            bitArray.Set(4, false);
            bitArray.Set(5, false);
            bitArray.Set(6, false);
            bitArray.Set(7, true);

            var bBitArray = Convert.ToByte(bitArray);
            var sBitArray = Convert.FromByte<BitArray>(bBitArray);

            Assert.IsTrue(bitArray.Count == sBitArray.Count);
            for (int i = 0; i < bitArray.Count; i++)
                Assert.IsTrue(bitArray[i] == sBitArray[i]);

            #endregion

            #region Exception

            Assert.ThrowsException<ArgumentException>(() => Convert.ToByte(200L));
            Assert.ThrowsException<ArgumentException>(() => Convert.FromByte<long>(new byte[1]));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Convert.ToByte(uint.MaxValue));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Convert.ToByte(new BitArray(9)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Convert.FromByte<BitArray>(new byte[2]));
                 
            Assert.ThrowsException<ArgumentNullException>(() => Convert.FromByte<long>(null));

            #endregion
        }
    }
}
