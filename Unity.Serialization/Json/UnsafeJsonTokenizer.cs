using System;
using Unity.Collections;

namespace Unity.Serialization.Json
{
	// Token: 0x02000041 RID: 65
	internal struct UnsafeJsonTokenizer : IDisposable
	{
		// Token: 0x06000161 RID: 353 RVA: 0x00005740 File Offset: 0x00003940
		public UnsafeJsonTokenizer(Allocator allocator)
		{
			this.PrevChar = 0;
			this.ResultCode = 0;
			this.m_TokenStream = null;
			this.m_CommentType = JsonCommentType.Unknown;
			this.m_CharBuffer = null;
			this.m_CharBufferLength = 0;
			this.m_CharBufferPosition = 0;
			this.m_IsEnd = false;
			this.m_NextStringCharIsEscaped = false;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000578E File Offset: 0x0000398E
		public void Dispose()
		{
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005790 File Offset: 0x00003990
		public void Reset()
		{
			this.PrevChar = 0;
			this.ResultCode = 0;
			this.m_TokenStream = null;
			this.m_CommentType = JsonCommentType.Unknown;
			this.m_CharBuffer = null;
			this.m_CharBufferLength = 0;
			this.m_CharBufferPosition = 0;
			this.m_IsEnd = false;
			this.m_NextStringCharIsEscaped = false;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000057E0 File Offset: 0x000039E0
		public unsafe int Write(UnsafeJsonTokenStream* stream, ushort* charBuffer, int charBufferStart, int charBufferLength, bool end)
		{
			this.m_TokenStream = stream;
			this.m_CharBuffer = charBuffer;
			this.m_CharBufferPosition = charBufferStart;
			this.m_CharBufferLength = charBufferLength;
			this.m_IsEnd = end;
			if (this.m_TokenStream->TokenNextIndex > 0 && this.m_TokenStream->Tokens[this.m_TokenStream->TokenNextIndex - 1].End == -1)
			{
				switch (this.m_TokenStream->Tokens[this.m_TokenStream->TokenNextIndex - 1].Type)
				{
				case TokenType.String:
				{
					int num = this.ParseString(this.m_TokenStream->TokenNextIndex - 1, -1);
					if (num != 0)
					{
						this.ResultCode = num;
						return this.m_CharBufferPosition - charBufferStart;
					}
					this.m_CharBufferPosition++;
					break;
				}
				case TokenType.Primitive:
				{
					int num2 = this.ParsePrimitive(this.m_TokenStream->TokenNextIndex - 1, -1);
					if (num2 != 0)
					{
						this.ResultCode = num2;
						return this.m_CharBufferPosition - charBufferStart;
					}
					this.m_CharBufferPosition++;
					break;
				}
				case TokenType.Comment:
				{
					int num3 = this.ParseComment(this.m_TokenStream->TokenNextIndex - 1, -1);
					if (num3 != 0)
					{
						this.ResultCode = num3;
						return this.m_CharBufferPosition - charBufferStart;
					}
					this.m_CharBufferPosition++;
					break;
				}
				}
			}
			while (this.m_CharBufferPosition < this.m_CharBufferLength)
			{
				ushort num4 = this.m_CharBuffer[this.m_CharBufferPosition];
				if (num4 <= 47)
				{
					if (num4 <= 13)
					{
						if (num4 != 0 && num4 - 9 > 1 && num4 != 13)
						{
							goto IL_045F;
						}
					}
					else if (num4 != 32 && num4 != 44)
					{
						if (num4 != 47)
						{
							goto IL_045F;
						}
						this.m_CharBufferPosition++;
						this.PrevChar = 0;
						this.m_CommentType = JsonCommentType.Unknown;
						if (this.ParseComment(this.m_TokenStream->TokenParentIndex, this.m_CharBufferPosition + 1) == -1)
						{
							this.ResultCode = -1;
							return this.m_CharBufferPosition - charBufferStart;
						}
					}
				}
				else
				{
					Token token2;
					if (num4 <= 91)
					{
						if (num4 == 58 || num4 == 61)
						{
							goto IL_05CE;
						}
						if (num4 != 91)
						{
							goto IL_045F;
						}
					}
					else
					{
						if (num4 != 93)
						{
							if (num4 == 123)
							{
								goto IL_01F1;
							}
							if (num4 != 125)
							{
								goto IL_045F;
							}
						}
						TokenType tokenType = ((num4 == 125) ? TokenType.Object : TokenType.Array);
						if (this.m_TokenStream->TokenNextIndex <= 0)
						{
							this.ResultCode = -1;
							return this.m_CharBufferPosition - charBufferStart;
						}
						int num5 = this.m_TokenStream->TokenNextIndex - 1;
						Token token;
						for (;;)
						{
							token = this.m_TokenStream->Tokens[num5];
							if (token.Start != -1 && token.End == -1 && token.Type != TokenType.String && token.Type != TokenType.Primitive)
							{
								goto Block_28;
							}
							if (token.Parent == -1)
							{
								goto Block_30;
							}
							num5 = token.Parent;
						}
						IL_0375:
						Token token3;
						if (this.m_TokenStream->TokenParentIndex == -1)
						{
							token2 = default(Token);
							token3 = token2;
						}
						else
						{
							token3 = this.m_TokenStream->Tokens[this.m_TokenStream->TokenParentIndex];
						}
						Token token4 = token3;
						if (this.m_TokenStream->TokenParentIndex != -1 && token4.Type != TokenType.Object && token4.Type != TokenType.Array)
						{
							this.m_TokenStream->TokenParentIndex = this.m_TokenStream->Tokens[this.m_TokenStream->TokenParentIndex].Parent;
							goto IL_05CE;
						}
						goto IL_05CE;
						Block_30:
						if (token.Type != tokenType || this.m_TokenStream->TokenParentIndex == -1)
						{
							this.ResultCode = -1;
							return this.m_CharBufferPosition - charBufferStart;
						}
						goto IL_0375;
						Block_28:
						if (token.Type != tokenType)
						{
							this.ResultCode = -1;
							return this.m_CharBufferPosition - charBufferStart;
						}
						this.m_TokenStream->TokenParentIndex = token.Parent;
						token.End = this.m_CharBufferPosition + 1;
						this.m_TokenStream->Tokens[num5] = token;
						goto IL_0375;
					}
					IL_01F1:
					ref UnsafeJsonTokenStream tokenStream = ref *this.m_TokenStream;
					token2 = new Token
					{
						Type = ((num4 == 123) ? TokenType.Object : TokenType.Array),
						Parent = this.m_TokenStream->TokenParentIndex,
						Start = this.m_CharBufferPosition,
						End = -1
					};
					tokenStream.Add(token2);
					this.m_TokenStream->TokenParentIndex = this.m_TokenStream->TokenNextIndex - 1;
				}
				IL_05CE:
				this.m_CharBufferPosition++;
				continue;
				IL_045F:
				int num6;
				if (num4 == 34)
				{
					this.m_CharBufferPosition++;
					this.PrevChar = 0;
					num6 = this.ParseString(this.m_TokenStream->TokenParentIndex, this.m_CharBufferPosition);
					if (num6 == -1)
					{
						this.ResultCode = -1;
						return this.m_CharBufferPosition - charBufferStart;
					}
				}
				else
				{
					int charBufferPosition = this.m_CharBufferPosition;
					num6 = this.ParsePrimitive(this.m_TokenStream->TokenParentIndex, charBufferPosition);
					if (num6 == -1)
					{
						this.ResultCode = -1;
						return this.m_CharBufferPosition - charBufferStart;
					}
				}
				if (this.m_TokenStream->TokenParentIndex == -1 || this.m_TokenStream->Tokens[this.m_TokenStream->TokenParentIndex].Type == TokenType.Object)
				{
					this.m_TokenStream->TokenParentIndex = this.m_TokenStream->TokenNextIndex - 1;
				}
				else if (this.m_TokenStream->TokenParentIndex != -1 && this.m_TokenStream->Tokens[this.m_TokenStream->TokenParentIndex].Type != TokenType.Object && this.m_TokenStream->Tokens[this.m_TokenStream->TokenParentIndex].Type != TokenType.Array)
				{
					this.m_TokenStream->TokenParentIndex = this.m_TokenStream->Tokens[this.m_TokenStream->TokenParentIndex].Parent;
				}
				if (num6 == -2)
				{
					this.ResultCode = -2;
					return this.m_CharBufferPosition - charBufferStart;
				}
				goto IL_05CE;
			}
			this.ResultCode = 0;
			return this.m_CharBufferPosition - charBufferStart;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005DEC File Offset: 0x00003FEC
		private unsafe int ParseString(int parent, int start)
		{
			while (this.m_CharBufferPosition < this.m_CharBufferLength)
			{
				ushort num = this.m_CharBuffer[this.m_CharBufferPosition];
				if (num == 92)
				{
					this.m_NextStringCharIsEscaped = !this.m_NextStringCharIsEscaped;
				}
				else
				{
					if (num == 34 && !this.m_NextStringCharIsEscaped)
					{
						this.m_NextStringCharIsEscaped = false;
						ref UnsafeJsonTokenStream tokenStream = ref *this.m_TokenStream;
						Token token = new Token
						{
							Type = TokenType.String,
							Parent = parent,
							Start = start,
							End = this.m_CharBufferPosition
						};
						tokenStream.Add(token);
						break;
					}
					this.m_NextStringCharIsEscaped = false;
				}
				this.PrevChar = num;
				this.m_CharBufferPosition++;
			}
			if (this.m_CharBufferPosition >= this.m_CharBufferLength)
			{
				ref UnsafeJsonTokenStream tokenStream2 = ref *this.m_TokenStream;
				Token token = new Token
				{
					Type = TokenType.String,
					Parent = parent,
					Start = start,
					End = (this.m_IsEnd ? this.m_CharBufferPosition : (-1))
				};
				tokenStream2.Add(token);
				return -2;
			}
			return 0;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005EF8 File Offset: 0x000040F8
		private unsafe int ParsePrimitive(int parent, int start)
		{
			while (this.m_CharBufferPosition < this.m_CharBufferLength)
			{
				ushort num = this.m_CharBuffer[this.m_CharBufferPosition];
				if (num == 32 || num == 9 || num == 13 || num == 10 || num == 0 || num == 44 || num == 93 || num == 125 || num == 58 || num == 61)
				{
					ref UnsafeJsonTokenStream tokenStream = ref *this.m_TokenStream;
					Token token = new Token
					{
						Type = TokenType.Primitive,
						Parent = parent,
						Start = start,
						End = this.m_CharBufferPosition
					};
					tokenStream.Add(token);
					this.m_CharBufferPosition--;
					break;
				}
				if (num < 32 || num >= 127)
				{
					return -1;
				}
				this.m_CharBufferPosition++;
			}
			if (this.m_CharBufferPosition >= this.m_CharBufferLength)
			{
				ref UnsafeJsonTokenStream tokenStream2 = ref *this.m_TokenStream;
				Token token = new Token
				{
					Type = TokenType.Primitive,
					Parent = parent,
					Start = start,
					End = (this.m_IsEnd ? this.m_CharBufferPosition : (-1))
				};
				tokenStream2.Add(token);
				return -2;
			}
			return 0;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006018 File Offset: 0x00004218
		private unsafe int ParseComment(int parent, int start)
		{
			while (this.m_CharBufferPosition < this.m_CharBufferLength)
			{
				ushort num = this.m_CharBuffer[this.m_CharBufferPosition];
				switch (this.m_CommentType)
				{
				case JsonCommentType.Unknown:
				{
					char c = (char)num;
					if (c != '*')
					{
						if (c != '/')
						{
							return -1;
						}
						this.m_CommentType = JsonCommentType.SingleLine;
					}
					else
					{
						this.m_CommentType = JsonCommentType.MultiLine;
					}
					break;
				}
				case JsonCommentType.SingleLine:
				{
					char c = (char)num;
					if (c == '\0' || c == '\n')
					{
						ref UnsafeJsonTokenStream tokenStream = ref *this.m_TokenStream;
						Token token = new Token
						{
							Type = TokenType.Comment,
							Parent = parent,
							Start = start,
							End = this.m_CharBufferPosition - 1
						};
						tokenStream.Add(token);
						return 0;
					}
					goto IL_00F1;
				}
				case JsonCommentType.MultiLine:
					if (num == 47 && this.PrevChar == 42)
					{
						ref UnsafeJsonTokenStream tokenStream2 = ref *this.m_TokenStream;
						Token token = new Token
						{
							Type = TokenType.Comment,
							Parent = parent,
							Start = start,
							End = this.m_CharBufferPosition - 1
						};
						tokenStream2.Add(token);
						return 0;
					}
					goto IL_00F1;
				default:
					goto IL_00F1;
				}
				IL_00F8:
				this.m_CharBufferPosition++;
				continue;
				IL_00F1:
				this.PrevChar = num;
				goto IL_00F8;
			}
			if (this.m_CharBufferPosition >= this.m_CharBufferLength)
			{
				ref UnsafeJsonTokenStream tokenStream3 = ref *this.m_TokenStream;
				Token token = new Token
				{
					Type = TokenType.Comment,
					Parent = parent,
					Start = start,
					End = (this.m_IsEnd ? this.m_CharBufferPosition : (-1))
				};
				tokenStream3.Add(token);
			}
			return -2;
		}

		// Token: 0x04000090 RID: 144
		public ushort PrevChar;

		// Token: 0x04000091 RID: 145
		public int ResultCode;

		// Token: 0x04000092 RID: 146
		private unsafe UnsafeJsonTokenStream* m_TokenStream;

		// Token: 0x04000093 RID: 147
		private JsonCommentType m_CommentType;

		// Token: 0x04000094 RID: 148
		private unsafe ushort* m_CharBuffer;

		// Token: 0x04000095 RID: 149
		private int m_CharBufferLength;

		// Token: 0x04000096 RID: 150
		private int m_CharBufferPosition;

		// Token: 0x04000097 RID: 151
		private bool m_IsEnd;

		// Token: 0x04000098 RID: 152
		private bool m_NextStringCharIsEscaped;
	}
}
