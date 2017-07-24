using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Stock.Ethereum.Api
{
    public class EthereumUtils
    {
        public static string DecimalToHex(decimal vl)
        {
            return (new HexBigInteger(new BigInteger(vl))).HexValue;
        }


        public static string ULongToHex(ulong vl)
        {
            return (new HexBigInteger(new BigInteger(vl))).HexValue;
        }

        public static string BigintToHex(BigInteger vl)
        {
            return (new HexBigInteger(vl)).HexValue;
        }



        public static ulong HexToULong(string hex)
        {
            HexBigInteger vl = new HexBigInteger(hex);

            return (ulong)vl.Value;
        }

        public static BigInteger HexToBigInteger(string hex)
        {
            HexBigInteger vl = new HexBigInteger(hex);

            return vl.Value;
        }
    }
}
