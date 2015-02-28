using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParse
{
    public class TextWindow
    {

        private string m_jsonString;
        private char[] m_jsonChars;
        private int m_position;
        
        private const char InvalidCharacter = char.MaxValue;

        public TextWindow(string JsonString)
        {
            m_position = 0;
            m_jsonString = JsonString;
            m_jsonChars = m_jsonString.ToCharArray();
        }

        public char PeekChar()
        {
            if (m_position > m_jsonChars.Length)
            {
                return InvalidCharacter;
            }
            return m_jsonChars[m_position];
        }

		public char PeekChar(int Count)
		{
			if (m_position + Count > m_jsonChars.Length)
			{
				return InvalidCharacter;
			}
			return m_jsonChars[m_position + Count];
		}

        public char NextChar()
        {
            var c = PeekChar();
            if (c != InvalidCharacter)
            {
                 Advance();
            }
           
            return c;
        }

        public void Advance()
        {
            m_position++;
        }

        public void Advance(int Count)
        {
            m_position += Count;
        }
    }
}
