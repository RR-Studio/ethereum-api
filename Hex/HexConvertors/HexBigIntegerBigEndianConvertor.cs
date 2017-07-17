using System.Numerics;

namespace Stock.Ethereum.Api
{
    public class HexBigIntegerBigEndianConvertor: IHexConvertor<BigInteger>
    {  

        public string ConvertToHex(BigInteger newValue)
        {
            return newValue.ToHex(false);
        }

        public BigInteger ConvertFromHex(string hex)
        {
            return hex.HexToBigInteger(false);
        }

    }
}