using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UDPCasts
{
    public static class ByteFactory
    {
        private static Regex hexRegex1Byte = new Regex("(?:0x)(?<content>[0123456789abcdef]{2})");

        private static Regex hexRegexnByte = new Regex("(?:(0x|^))(?<content>[0123456789abcdef]{2,})");

        public static byte[] ArrayFromHexString(string hexString)
        {
            Match match = hexRegexnByte.Match(hexString.ToLower());

            if (match.Success)
            {
                List<byte> bytes = new List<byte>();
                List<char> content = match.Groups["content"].Value.ToCharArray().ToList();

                if ((content.Count % 2) != 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                while (content.Any())
                {
                    bytes.Add(ByteFactory.FromChar(content[0], content[1]));
                    content.RemoveRange(0, 2);
                }

                return bytes.ToArray();
            }
            else
            {
                throw new ArgumentOutOfRangeException("hexString", "Must be in format \"0xAB\"");
            }
        }

        public static byte FromChar(char upperNybble, char lowerNybble)
        {
            return FromHexString(string.Format("0x{0}{1}", upperNybble, lowerNybble));
        }

        public static byte FromHexString(string hexString)
        {
            Match match = hexRegex1Byte.Match(hexString.ToLower());

            if (match.Success)
            {
                return Convert.ToByte(match.Groups["content"].Value, 16);
            }
            else
            {
                throw new ArgumentOutOfRangeException("hexString", "Must be in format \"0xAB\"");
            }
        }
    }
}