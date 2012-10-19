using System;
using System.Linq;

namespace Dongle.System
{
    /// <summary>
    /// Funções úteis relacionadas a números
    /// </summary>
    public static class NumericExtensions
    {
        /// <summary>
        /// Converte um bool para 0 ou 1
        /// </summary>
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        /// <summary>
        /// Converte qualquer coisa para 0 ou 1
        /// </summary>
        public static int ToInt(this object value)
        {
            if (value == null)
            {
                return 0;
            }
            var strValue = value.ToString().Trim().ToLower();
            if (strValue == "" || strValue == "0" || strValue == "false" || strValue == "não" || strValue == "nao" || strValue == "no" || strValue == "n")
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// Obtém o hexa de um numero (QWORD)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hexMinLength">Tamanho mínimo da string (completa com zeros a esquerda).</param>
        /// <returns></returns>
        public static string ToHex(this long value, int hexMinLength = 8)
        {
            string result;
            if (value <= int.MaxValue && value >= int.MinValue)
            {
                result = Convert.ToString((int)value, 16).ToUpper();
            }
            else
            {
                result = Convert.ToString(value, 16).ToUpper();    
            }
            if(result.Length < hexMinLength)
            {
                result = result.PadLeft(hexMinLength, '0');
            }
            return result;
        }

        private static readonly char[] HexSymbolTable = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        /// <summary>
        /// Converte um long para uma string em hexa. Mais rápido que o padrao do .Net, e converte em DWORD.
        /// </summary>
        public static string ToHexFast(this long value)
        {
            if (value <= int.MinValue || value >= int.MaxValue)
            {
                return ToHex(value);
            }
            const int size = 8;
            var buffer = new char[size];
            var place = size;
            var q = value;
            if (value < 0)
            {
                q += 4294967296; //ou 2^32
            }
            do
            {
                var r = q%16;
                if (r < 0)
                {
                    r = 16 + r;
                }
                buffer[--place] = HexSymbolTable[r];
                q /= 16;
            }
            while (q != 0);            
            var padChar = (value < 0) ? 'F' : '0';
            while (place > 0)
            {
                buffer[--place] = padChar;                
            }         
            return new string(buffer);
        }
       
        /// <summary>
        /// Converte um int[] para texto
        /// </summary>
        public static string IntToString(params int[] values)
        {
            return values.Aggregate("", (current, value) => current + Convert.ToChar(value));
        }

        /// <summary>
        /// Compõe as condições especificadas em um binário (1, 2, 4, 8).
        /// Exemplo: true, true, false, true => 1 + 2 + 0 + 8 => 11
        /// </summary>
        public static long BinaryComposition(params bool[] conditions)
        {
            var result = 0L;
            for (var i = 0; i < conditions.Length; i++)
            {
                if (conditions[i])
                {
                    result = result | (long)Math.Pow(2, i);
                }
            }
            return result;
        }

        /// <summary>
        /// Função de eficácia duvidosa proveniente da GBWCD2
        /// </summary>
        public static long FromMadUnsigned(this long value)
        {
            if (value <= 4294967296L)
                return value;
            return value - 4294967296;
        }

        /// <summary>
        /// Converte um long para ULong, somando 2^32 ao número
        /// </summary>
        public static long ToUnsigned(this long value)
        {
            if (value < 0)
                return value + 4294967296;
            return value; 
        }

        /// <summary>
        /// Bitwise Xor
        /// </summary>
        public static long Except(this long value, long except)
        {
            if (except != 0 && (value & except) != 0)
            {
                value = value ^ except;
            }
            return value;
        }

        /// <summary>
        /// Converte um double para decimal
        /// </summary>
        public static decimal ToDecimal(this double value)
        {
            return Convert.ToDecimal(value);
        }

        /// <summary>
        /// Converte um double para decimal
        /// </summary>
        public static decimal ToInt32(this double value)
        {
            return Convert.ToInt32(value);
        }
    }
}
