using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vlsiasm
{
    class Parser
    {
        public static bool TryParseHex(string input, ref int result)
        {
            result = 0;
            int sign = 1;
            foreach (char c in input)
            {
                if (c >= '0' && c <= '9')
                    result = result * 16 + c - '0';
                else if (c >= 'a' && c <= 'f')
                    result = result * 16 + c - 'a' + 10;
                else if (c >= 'A' && c <= 'F')
                    result = result * 16 + c - 'A' + 10;
                else if (c == '-')
                    sign = -1;
                else
                    return false;
            }
            result *= sign;
            return true;
        }
        public static bool TryParseRegister(string input, ref int result)
        {
            result = 0;
            if (input.Length >= 2 && input.Length <= 3 && (input[0] == 'r' || input[0] == 'R'))
            {
                if (input[1] >= '0' && input[1] <= '9')
                {
                    result = input[1] - '0';
                }
                else
                {
                    return false;
                }
                if (input.Length == 3)
                {
                    if (input[2] >= '0' && input[2] <= '9')
                    {
                        result = result * 10 + input[2] - '0';
                    }
                    else
                    {
                        return false;
                    }
                }
                if (result < 0 || result > 31)
                    return false;
                return true;
            }
            return false;
        }
    }
}
