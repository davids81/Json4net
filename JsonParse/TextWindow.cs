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
        
		private char m_currentChar;

        public const char InvalidCharacter = char.MaxValue;

        public TextWindow(string JsonString)
        {
            m_position = 0;
            m_jsonString = JsonString;
            m_jsonChars = m_jsonString.ToCharArray();
            m_currentChar = m_jsonChars[0];
        }

        public char PeekChar()
        {
            if (IsAtEnd())
            {
                return InvalidCharacter;
            }
			
            //m_currentChar = m_jsonChars[m_position];

            return m_currentChar;
        }

        public bool IsAtEnd()
        {
            return m_position == m_jsonChars.Length - 1;
        }

      

		public char PeekChar(int Count)
		{
			if (IsAtEnd())
			{
				return InvalidCharacter;
			}

			m_currentChar = m_jsonChars[m_position + Count];
			return m_currentChar;
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
            m_currentChar = m_jsonChars[m_position];
            
            while ((/*m_currentChar == '\n' || m_currentChar == '\r' ||*/ char.IsWhiteSpace(m_currentChar)) && !IsAtEnd())
            {
                m_position++;
                m_currentChar = m_jsonChars[m_position];
            }
        }

        public void Advance(int Count)
        {
            m_position += Count;
        }
    }
}
