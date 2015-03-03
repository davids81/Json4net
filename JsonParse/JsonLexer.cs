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
        PairSeparator,
		Minus,
		Number,
		Object,
		Array,
		True,
		False,
		Null,
		DoubleQuote,
		BackSlash,
		ForewardSlash,
		Backspace,
		FormFeed,
		NewLine,
		CarriageReturn,
		HorizontalTab,
		Hex,
		String,
		PairDelim,
		Comma,
        EOF
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

		public string Text
		{
			get;set;
		}

		

    }

    public class JsonLexer
    {
        private TextWindow m_window;
		private StringBuilder m_builder;


        public JsonLexer(TextWindow Text)
        {
            m_window = Text;
			m_builder = new StringBuilder();
			// advance to the first character
			m_window.Advance();
        }

        public Token Lex()
        {
            char currentChar = m_window.PeekChar();
            switch (currentChar)
            {
                case '{':
                    m_window.Advance();
                    return new Token { SyntaxType = TokenType.BeginObject, Text = "{" };
                case '}':
                    m_window.Advance();
                    return new Token {  SyntaxType = TokenType.EndObject, Text = "}" };
                case '[':
                    m_window.Advance();
                    return new Token { SyntaxType = TokenType.BeginArray, Text = "[" };
                case ']':
                    m_window.Advance();
                    return new Token { SyntaxType = TokenType.EndArray, Text = "]" };
				case '-':
					m_window.Advance();
					return new Token { SyntaxType = TokenType.Minus, Text = "-" };
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
				case '"':
					return ScanStringLiteral();
				case ':':
					m_window.Advance();
					return new Token { SyntaxType = TokenType.PairDelim, Text = ":" };
				case ',':
					m_window.Advance();
					return new Token { SyntaxType = TokenType.Comma, Text = "," };
                case TextWindow.InvalidCharacter:
                    return new Token { SyntaxType = TokenType.EOF, Text = null };
                case 't':
				case 'T':
					StringBuilder trueString = new StringBuilder();
			
					do
					{
						trueString.Append(currentChar);
						m_window.Advance();
						currentChar = m_window.PeekChar();
					} while (char.IsLetter(currentChar));

					if (trueString.Length == 4)
					{
						if (trueString[0] == 't' || trueString[1] == 'T' &&
							trueString[1] == 'r' || trueString[1] == 'R' &&
							trueString[2] == 'u' || trueString[2] == 'U' &&
							trueString[3] == 'e' || trueString[3] == 'E')
						{
							return new Token { SyntaxType = TokenType.True, Text = trueString.ToString() };
						}
					}
				
					throw new ParseException("Invlaid Keyword");
				case 'f':
				case 'F':
					StringBuilder falseString = new StringBuilder();
				
					do
					{
						falseString.Append(currentChar);
						m_window.Advance();
						currentChar = m_window.PeekChar();
					} while (char.IsLetter(currentChar));

					if (falseString.Length == 5)
					{
						if (falseString[0] == 'f' || falseString[1] == 'F' &&
							falseString[1] == 'a' || falseString[1] == 'A' &&
							falseString[2] == 'l' || falseString[2] == 'L' &&
							falseString[3] == 's' || falseString[3] == 'S' &&
							falseString[4] == 'e' || falseString[4] == 'E')
						{
							return new Token { SyntaxType = TokenType.False, Text = falseString.ToString() };
						}
					}
				
					throw new ParseException("Invlaid Keyword");

            }

			return null;
        }

		private Token ScanStringLiteral()
		{
			Token stringToken = new Token();
			stringToken.SyntaxType = TokenType.String;

			m_builder.Clear();
			char c = m_window.PeekChar();
			
			if (c != '"')
			{
				throw new ParseException();
			}

			m_builder.Append(c);
			
			while (true)
			{
				m_window.Advance();
				c = m_window.PeekChar();

				if (c == '"')
				{	
					m_builder.Append(c);
					m_window.Advance();
					stringToken.Text = m_builder.ToString();
					return stringToken;
				}

				if (c == '\n')
				{
					// strings can't span new line characters
					throw new ParseException();
				}

				m_builder.Append(c);
			}
		}

        private Token ScanNumericLiteral()
        {
			m_builder.Clear();

            char c;
            while ((c = m_window.PeekChar()) >= '0' && c <= '9')
            {
                m_builder.Append(c);
                m_window.Advance();
            }

			c = m_window.PeekChar();
		
			if (c == '.')
			{
				m_builder.Append(c);
				m_window.Advance();

				c = m_window.PeekChar();
				if (c >= '0' && c <= '9')
				{
					while ((c = m_window.PeekChar()) >= '0' && c <= '9')
					{
						m_builder.Append(c);
						m_window.Advance();
					}
				}
				else
				{
					// we got a decimal, but we didn't get anything after
					// TODO: don't throw exception - deal with the error and move on later
					// unfortunatly, right now, I don't have a decent way of dealing with it
					throw new ParseException();
				}
			}
			
			if (c == 'e' || c == 'E')
			{
				m_builder.Append(c);
				m_window.Advance();
				c = m_window.PeekChar();

				if ( c == '+' ||  c == '-')
				{
					m_builder.Append(c);
					m_window.Advance();
				}
			}
		
			return new Token() 
			{
				SyntaxType = TokenType.Number,
				Text = m_builder.ToString() 
			};

        }

    }
}


//c = m_window.PeekChar();

//				if (c == '"')
//				{
//					m_builder.Append(c);
//					m_window.Advance();

//					stringToken.Text = m_builder.ToString();
//					stringToken.SyntaxType = TokenType.String;

//					return stringToken;
//				}

//				if (c == '\\')
//				{
//					m_window.Advance();
//					var c2 = m_window.PeekChar();

//					switch (c2)
//					{
//						case '"':
//							stringToken.SyntaxType = TokenType.DoubleQuote;
//							break;
//						case '\\':
//							stringToken.SyntaxType = TokenType.BackSlash;
//							break;
//						case '/':
//							stringToken.SyntaxType = TokenType.ForewardSlash;
//							break;
//						case 'b':
//							stringToken.SyntaxType = TokenType.Backspace;
//							break;
//						case 'f':
//							stringToken.SyntaxType = TokenType.FormFeed;
//							break;
//						case 'n':
//							stringToken.SyntaxType = TokenType.NewLine;
//							break;
//						case 'r':
//							stringToken.SyntaxType = TokenType.CarriageReturn;
//							break;
//						case 't':
//							stringToken.SyntaxType = TokenType.HorizontalTab;
//							break;
//						case 'u':
//							stringToken.SyntaxType = TokenType.Hex;
//							m_window.Advance();

//							for (int i = 0; i < 4; i++)
//							{
//								c = m_window.PeekChar();

//								switch (c)
//								{
//									case 'a':
//									case 'A':
//									case 'b':
//									case 'B':
//									case 'c':
//									case 'C':
//									case 'd':
//									case 'D':
//									case 'e':
//									case 'E':
//									case 'f':
//									case 'F':
//										m_builder.Append(c);
//										m_window.Advance();
//										break;
//									default:
//										throw new ParseException();
//								}

//							}
//							stringToken.Text = m_builder.
//							break;
//					}
//				}