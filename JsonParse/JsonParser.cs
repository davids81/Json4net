using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParse
{
    public class JsonParser
    {

		private JsonLexer m_lexer;

		public JsonParser(JsonLexer Lexer)
		{
			m_lexer = Lexer;
		}

		public void Parse()
		{
			SyntaxTree jsonObject = ParseObject();
		}

		private bool EatToken(TokenType Type)
		{
			Token aToken = m_lexer.Lex();
			if (aToken.SyntaxType == Type)
			{
				return true;
			}
			return false;
		}

		private SyntaxTree ParseObject()
		{
			SyntaxTree tree = new SyntaxTree();

			EatToken(TokenType.BeginObject);
		    ParseMembers();
			EatToken(TokenType.EndObject);

			return tree;
		}

		private void ParseMembers()
		{
			while (ParseMember());
		}

		private bool ParseMember()
		{
			ParsePair();
			return true;
		}

		private void ParsePair()
		{
			ParseString();
			EatToken(TokenType.PairDelim);
			ParseValue();
			return;
		}

		private bool ParseString()
		{
			Token stringToken = m_lexer.Lex();
			return true;
		}

		private void ParseArray()
		{
			EatToken(TokenType.BeginArray);
			ParseElements();
			EatToken(TokenType.EndArray);
		}

		private void ParseElements()
		{
			ParseValue();

			while (true)
			{
				if (EatToken(TokenType.Comma))
				{
					ParseValue();
				}
				else
				{
					break;
				}
			}
		}

		private void ParseValue()
		{
			Token next = m_lexer.Lex();

			switch(next.SyntaxType)
			{
				case TokenType.BeginObject:
					ParseObject();
					break;
				case TokenType.String:
					Token stringTok = m_lexer.Lex();
					break;
				case TokenType.Number:
					Token numTok = m_lexer.Lex();
					break;
				case TokenType.BeginArray:
					ParseArray();
					break;
				case TokenType.True:

					break;
				case TokenType.False:

					break;
				case TokenType.Null:

					break;
			}
		}
    }
}
