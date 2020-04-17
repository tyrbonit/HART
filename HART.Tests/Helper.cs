namespace HART.Tests
{
    internal static class Helper
    {
        public static bool IsByteArrayEqual(this byte[] expected, byte[] actual)
        {
            var result = false;

            if (expected.Length != actual.Length)
                return result;

            for (var i = 0; i < expected.Length; i++)
                result = expected[i] == actual[i];

            return result;
        }
    }
}
