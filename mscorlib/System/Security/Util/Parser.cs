using System;
using System.IO;
using System.Text;

namespace System.Security.Util
{
	// Token: 0x020003EE RID: 1006
	internal sealed class Parser
	{
		// Token: 0x06002983 RID: 10627 RVA: 0x00096874 File Offset: 0x00094A74
		internal SecurityElement GetTopElement()
		{
			return this._doc.GetRootElement();
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x00096884 File Offset: 0x00094A84
		private void GetRequiredSizes(TokenizerStream stream, ref int index)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num = 1;
			SecurityElementType securityElementType = SecurityElementType.Regular;
			string text = null;
			bool flag5 = false;
			bool flag6 = false;
			int num2 = 0;
			for (;;)
			{
				short num3 = stream.GetNextToken();
				while (num3 != -1)
				{
					switch (num3 & 255)
					{
					case 0:
						flag4 = true;
						flag6 = false;
						num3 = stream.GetNextToken();
						if (num3 == 2)
						{
							stream.TagLastToken(17408);
							for (;;)
							{
								num3 = stream.GetNextToken();
								if (num3 != 3)
								{
									break;
								}
								stream.ThrowAwayNextString();
								stream.TagLastToken(20480);
							}
							if (num3 == -1)
							{
								goto Block_9;
							}
							if (num3 != 1)
							{
								goto Block_10;
							}
							flag4 = false;
							index++;
							flag6 = false;
							num--;
							flag = true;
							goto IL_03B9;
						}
						else if (num3 == 3)
						{
							flag3 = true;
							stream.TagLastToken(16640);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							if (securityElementType != SecurityElementType.Regular)
							{
								goto Block_12;
							}
							flag = true;
							num++;
							goto IL_03B9;
						}
						else
						{
							if (num3 == 6)
							{
								num2 = 1;
								do
								{
									num3 = stream.GetNextToken();
									switch (num3)
									{
									case 0:
										num2++;
										break;
									case 1:
										num2--;
										break;
									case 3:
										stream.ThrowAwayNextString();
										stream.TagLastToken(20480);
										break;
									}
								}
								while (num2 > 0);
								flag4 = false;
								flag6 = false;
								flag = true;
								goto IL_03B9;
							}
							if (num3 != 5)
							{
								goto IL_02B3;
							}
							num3 = stream.GetNextToken();
							if (num3 != 3)
							{
								goto Block_17;
							}
							flag3 = true;
							securityElementType = SecurityElementType.Format;
							stream.TagLastToken(16640);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							num2 = 1;
							num++;
							flag = true;
							goto IL_03B9;
						}
						break;
					case 1:
						if (flag4)
						{
							flag4 = false;
							goto IL_03C4;
						}
						goto IL_02E0;
					case 2:
						num3 = stream.GetNextToken();
						if (num3 == 1)
						{
							stream.TagLastToken(17408);
							index++;
							num--;
							flag6 = false;
							flag = true;
							goto IL_03B9;
						}
						goto IL_0329;
					case 3:
						if (flag4)
						{
							if (securityElementType == SecurityElementType.Comment)
							{
								stream.ThrowAwayNextString();
								stream.TagLastToken(20480);
								goto IL_03B9;
							}
							if (text == null)
							{
								text = stream.GetNextString();
								goto IL_03B9;
							}
							if (!flag5)
							{
								goto Block_5;
							}
							stream.TagLastToken(16896);
							index += SecurityDocument.EncodedStringSize(text) + SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							text = null;
							flag5 = false;
							goto IL_03B9;
						}
						else
						{
							if (flag6)
							{
								stream.TagLastToken(25344);
								index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + SecurityDocument.EncodedStringSize(" ");
								goto IL_03B9;
							}
							stream.TagLastToken(17152);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							flag6 = true;
							goto IL_03B9;
						}
						break;
					case 4:
						flag5 = true;
						goto IL_03B9;
					case 5:
						if (!flag4 || securityElementType != SecurityElementType.Format || num2 != 1)
						{
							goto IL_0397;
						}
						num3 = stream.GetNextToken();
						if (num3 == 1)
						{
							stream.TagLastToken(17408);
							index++;
							num--;
							flag6 = false;
							flag = true;
							goto IL_03B9;
						}
						goto IL_037C;
					}
					goto Block_1;
					IL_03C4:
					num3 = stream.GetNextToken();
					continue;
					IL_03B9:
					if (flag)
					{
						flag = false;
						flag2 = false;
						break;
					}
					flag2 = true;
					goto IL_03C4;
				}
				if (flag2)
				{
					index++;
					num--;
					flag6 = false;
				}
				else if (num3 == -1 && (num != 1 || !flag3))
				{
					goto IL_03F5;
				}
				if (num <= 1)
				{
					return;
				}
			}
			Block_1:
			goto IL_03A8;
			Block_5:
			throw new XmlSyntaxException(this._t.LineNo);
			Block_9:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Unexpected end of file."));
			Block_10:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Expected > character."));
			Block_12:
			throw new XmlSyntaxException(this._t.LineNo);
			Block_17:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_02B3:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Expected / character or string."));
			IL_02E0:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Unexpected > character."));
			IL_0329:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Expected > character."));
			IL_037C:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Expected > character."));
			IL_0397:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_03A8:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_03F5:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Unexpected end of file."));
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x00096CAC File Offset: 0x00094EAC
		private int DetermineFormat(TokenizerStream stream)
		{
			if (stream.GetNextToken() == 0 && stream.GetNextToken() == 5)
			{
				this._t.GetTokens(stream, -1, true);
				stream.GoToPosition(2);
				bool flag = false;
				bool flag2 = false;
				short num = stream.GetNextToken();
				while (num != -1 && num != 1)
				{
					if (num != 3)
					{
						if (num != 4)
						{
							throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Unexpected end of file."));
						}
						flag = true;
					}
					else
					{
						if (flag && flag2)
						{
							this._t.ChangeFormat(Encoding.GetEncoding(stream.GetNextString()));
							return 0;
						}
						if (!flag)
						{
							if (string.Compare(stream.GetNextString(), "encoding", StringComparison.Ordinal) == 0)
							{
								flag2 = true;
							}
						}
						else
						{
							flag = false;
							flag2 = false;
							stream.ThrowAwayNextString();
						}
					}
					num = stream.GetNextToken();
				}
				return 0;
			}
			return 2;
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x00096D70 File Offset: 0x00094F70
		private void ParseContents()
		{
			TokenizerStream tokenizerStream = new TokenizerStream();
			this._t.GetTokens(tokenizerStream, 2, false);
			tokenizerStream.Reset();
			int num = this.DetermineFormat(tokenizerStream);
			tokenizerStream.GoToPosition(num);
			this._t.GetTokens(tokenizerStream, -1, false);
			tokenizerStream.Reset();
			int num2 = 0;
			this.GetRequiredSizes(tokenizerStream, ref num2);
			this._doc = new SecurityDocument(num2);
			int num3 = 0;
			tokenizerStream.Reset();
			for (short num4 = tokenizerStream.GetNextFullToken(); num4 != -1; num4 = tokenizerStream.GetNextFullToken())
			{
				if ((num4 & 16384) == 16384)
				{
					short num5 = (short)((int)num4 & 65280);
					if (num5 <= 17152)
					{
						if (num5 == 16640)
						{
							this._doc.AddToken(1, ref num3);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num3);
							goto IL_019D;
						}
						if (num5 == 16896)
						{
							this._doc.AddToken(2, ref num3);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num3);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num3);
							goto IL_019D;
						}
						if (num5 == 17152)
						{
							this._doc.AddToken(3, ref num3);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num3);
							goto IL_019D;
						}
					}
					else
					{
						if (num5 == 17408)
						{
							this._doc.AddToken(4, ref num3);
							goto IL_019D;
						}
						if (num5 == 20480)
						{
							tokenizerStream.ThrowAwayNextString();
							goto IL_019D;
						}
						if (num5 == 25344)
						{
							this._doc.AppendString(" ", ref num3);
							this._doc.AppendString(tokenizerStream.GetNextString(), ref num3);
							goto IL_019D;
						}
					}
					throw new XmlSyntaxException();
				}
				IL_019D:;
			}
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x00096F28 File Offset: 0x00095128
		private Parser(Tokenizer t)
		{
			this._t = t;
			this._doc = null;
			try
			{
				this.ParseContents();
			}
			finally
			{
				this._t.Recycle();
			}
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x00096F70 File Offset: 0x00095170
		internal Parser(string input)
			: this(new Tokenizer(input))
		{
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x00096F7E File Offset: 0x0009517E
		internal Parser(string input, string[] searchStrings, string[] replaceStrings)
			: this(new Tokenizer(input, searchStrings, replaceStrings))
		{
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x00096F8E File Offset: 0x0009518E
		internal Parser(byte[] array, Tokenizer.ByteTokenEncoding encoding)
			: this(new Tokenizer(array, encoding, 0))
		{
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x00096F9E File Offset: 0x0009519E
		internal Parser(byte[] array, Tokenizer.ByteTokenEncoding encoding, int startIndex)
			: this(new Tokenizer(array, encoding, startIndex))
		{
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x00096FAE File Offset: 0x000951AE
		internal Parser(StreamReader input)
			: this(new Tokenizer(input))
		{
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x00096FBC File Offset: 0x000951BC
		internal Parser(char[] array)
			: this(new Tokenizer(array))
		{
		}

		// Token: 0x04001EFB RID: 7931
		private SecurityDocument _doc;

		// Token: 0x04001EFC RID: 7932
		private Tokenizer _t;

		// Token: 0x04001EFD RID: 7933
		private const short c_flag = 16384;

		// Token: 0x04001EFE RID: 7934
		private const short c_elementtag = 16640;

		// Token: 0x04001EFF RID: 7935
		private const short c_attributetag = 16896;

		// Token: 0x04001F00 RID: 7936
		private const short c_texttag = 17152;

		// Token: 0x04001F01 RID: 7937
		private const short c_additionaltexttag = 25344;

		// Token: 0x04001F02 RID: 7938
		private const short c_childrentag = 17408;

		// Token: 0x04001F03 RID: 7939
		private const short c_wastedstringtag = 20480;
	}
}
