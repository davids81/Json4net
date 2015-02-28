using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParse
{

    public enum TokenType
    {
        BeginObject,
        EndObject,
        BeginArray,
        EndArray,
        PairSeparator
    }

    public class Token
    {
        public Token()
        {

        }

        public TokenType SyntaxType
        {
            get;set;
        }


    }

    public class JsonLexer
    {
        private TextWindow m_window;

        public JsonLexer(TextWindow Text)
        {
            m_window = Text;
        }

        public Token Lex()
        {
            List<char> chars = new List<char>();

            char currentChar = m_window.PeekChar();
            switch (currentChar)
            {
                case '{':
                    m_window.Advance();
                    return new Token { SyntaxType = TokenType.BeginObject };
                case '}':
                    m_window.Advance();
                    return new Token {  SyntaxType = TokenType.EndObject };
                case '[':
                    m_window.Advance();
                    return new Token { SyntaxType = TokenType.BeginArray };
                case ']':
                    m_window.Advance();
                    return new Token { SyntaxType = TokenType.EndArray };
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ScanNumericLiteral();

            }

			throw new NotImplementedException();
        }

        private Token ScanNumericLiteral()
        {
            List<char> numChars = new List<char>();
            char c;
            while ((c = m_window.PeekChar()) >= '0' && c <= '9')
            {
                numChars.Add(c);
                m_window.Advance();
            }

			
			c = m_window.PeekChar();
		
			if (c == '.')
			{
				
				numChars.Add(c);
				m_window.Advance();

				while ((c = m_window.PeekChar()) >= '0' && c <= '9')
				{
					numChars.Add(c);
					m_window.Advance();
				}
			}
			
			bool hasExp = false;

			if (c == 'e' || c == 'E')
			{
				hasExp = true;

				numChars.Add(c);
				m_window.Advance();
				c = m_window.PeekChar();

				if ( c == '+' ||  c == '-')
				{
					numChars.Add(c);
					m_window.Advance();
				}
			}
		
			throw new NotImplementedException();

        }

    }
}
