using System;
using System.Runtime.CompilerServices;

namespace Unity.Serialization.Json
{
	// Token: 0x02000048 RID: 72
	internal struct UnsafePackedBinaryWriter : IDisposable
	{
		// Token: 0x06000196 RID: 406 RVA: 0x00006D2A File Offset: 0x00004F2A
		public unsafe UnsafePackedBinaryWriter(UnsafeJsonTokenStream* jsonTokenStream, UnsafePackedBinaryStream* packedBinaryStream)
		{
			this.m_JsonTokenStream = jsonTokenStream;
			this.m_PackedBinaryStream = packedBinaryStream;
			this.JsonTokenNextIndex = 0;
			this.JsonTokenParentIndex = -1;
			this.StripStringEscapeCharacters = true;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000578E File Offset: 0x0000398E
		public void Dispose()
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00006D4F File Offset: 0x00004F4F
		public void Seek(int index, int parent)
		{
			this.JsonTokenNextIndex = index;
			this.JsonTokenParentIndex = parent;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006D60 File Offset: 0x00004F60
		internal unsafe SerializedValueView GetView(int index)
		{
			if (index >= this.m_PackedBinaryStream->TokenNextIndex)
			{
				throw new IndexOutOfRangeException();
			}
			BinaryToken binaryToken = this.m_PackedBinaryStream->Tokens[index];
			HandleData handleData = this.m_PackedBinaryStream->Handles[binaryToken.HandleIndex];
			return new SerializedValueView(this.m_PackedBinaryStream, new Handle
			{
				Index = binaryToken.HandleIndex,
				Version = handleData.DataVersion
			});
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006DEC File Offset: 0x00004FEC
		public unsafe int Write(UnsafeBuffer<char> buffer, int count)
		{
			if (count <= 0)
			{
				return 0;
			}
			count = Math.Min(this.JsonTokenNextIndex + count, this.m_JsonTokenStream->TokenNextIndex) - this.JsonTokenNextIndex;
			int jsonTokenNextIndex = this.JsonTokenNextIndex;
			int tokenNextIndex = this.m_PackedBinaryStream->TokenNextIndex;
			this.m_PackedBinaryStream->EnsureTokenCapacity(this.m_PackedBinaryStream->TokenNextIndex + count);
			int num = this.WriteTokens(buffer, count);
			this.m_PackedBinaryStream->EnsureBufferCapacity(num);
			this.WriteCharacters(jsonTokenNextIndex, tokenNextIndex, buffer, count);
			this.m_PackedBinaryStream->BufferPosition = num;
			return count;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006E78 File Offset: 0x00005078
		private unsafe int WriteTokens(UnsafeBuffer<char> buffer, int count)
		{
			int num = this.m_PackedBinaryStream->BufferPosition;
			int num2 = Math.Min(this.JsonTokenNextIndex + count, this.m_JsonTokenStream->TokenNextIndex);
			while (this.JsonTokenNextIndex < num2)
			{
				Token token = this.m_JsonTokenStream->Tokens[this.JsonTokenNextIndex];
				while (this.JsonTokenParentIndex != -1 && token.Parent < this.JsonTokenParentIndex)
				{
					Token token2 = this.m_JsonTokenStream->Tokens[this.JsonTokenParentIndex];
					this.JsonTokenParentIndex = token2.Parent;
					if (this.m_PackedBinaryStream->TokenParentIndex != -1)
					{
						this.m_PackedBinaryStream->Tokens[this.m_PackedBinaryStream->TokenParentIndex].Length = ((token2.Start == -1) ? (-1) : (this.m_PackedBinaryStream->TokenNextIndex - this.m_PackedBinaryStream->TokenParentIndex));
						this.m_PackedBinaryStream->TokenParentIndex = this.m_PackedBinaryStream->Tokens[this.m_PackedBinaryStream->TokenParentIndex].Parent;
					}
				}
				BinaryToken binaryToken = this.m_PackedBinaryStream->Tokens[this.m_PackedBinaryStream->TokenNextIndex];
				binaryToken.Type = token.Type;
				binaryToken.Position = num;
				binaryToken.Parent = this.m_PackedBinaryStream->TokenParentIndex;
				binaryToken.Length = ((token.Start == -1) ? (-1) : 1);
				binaryToken.LastCharacterIsEscaped = false;
				TokenType type = token.Type;
				if (type - TokenType.Object > 1)
				{
					if (type - TokenType.String <= 2)
					{
						if (token.Parent == -1 || this.m_JsonTokenStream->Tokens[token.Parent].Type == TokenType.Object || token.End == -1)
						{
							this.JsonTokenParentIndex = this.JsonTokenNextIndex;
							this.m_PackedBinaryStream->TokenParentIndex = this.m_PackedBinaryStream->TokenNextIndex;
						}
						if (token.Start != -1)
						{
							num += 4;
						}
						int num3 = ((token.Start != -1) ? token.Start : 0);
						int num4 = ((token.End != -1) ? token.End : buffer.Length);
						if (this.StripStringEscapeCharacters && token.Type == TokenType.String)
						{
							int num5 = 0;
							for (int i = num3; i < num4; i++)
							{
								if (buffer.Buffer[i] == '\\')
								{
									i++;
									if (i < num4)
									{
										num5++;
									}
									else
									{
										binaryToken.LastCharacterIsEscaped = true;
									}
								}
								else
								{
									num5++;
								}
							}
							num += num5 * 2;
						}
						else
						{
							num += (num4 - num3) * 2;
						}
					}
				}
				else
				{
					this.JsonTokenParentIndex = this.JsonTokenNextIndex;
					this.m_PackedBinaryStream->TokenParentIndex = this.m_PackedBinaryStream->TokenNextIndex;
				}
				this.m_PackedBinaryStream->Tokens[this.m_PackedBinaryStream->TokenNextIndex] = binaryToken;
				this.m_PackedBinaryStream->Handles[binaryToken.HandleIndex].DataVersion++;
				this.JsonTokenNextIndex++;
				this.m_PackedBinaryStream->TokenNextIndex++;
			}
			int num6 = this.JsonTokenNextIndex - 1;
			int num7 = this.m_PackedBinaryStream->TokenNextIndex - 1;
			while (num6 >= 0 && num7 >= 0)
			{
				Token token3 = this.m_JsonTokenStream->Tokens[num6];
				ref BinaryToken ptr = this.m_PackedBinaryStream->Tokens[num7];
				this.m_PackedBinaryStream->Tokens[num7].Length = ((token3.Start == -1) ? (-1) : (this.m_PackedBinaryStream->TokenNextIndex - num7));
				num6 = token3.Parent;
				num7 = ptr.Parent;
			}
			if (this.JsonTokenNextIndex >= this.m_JsonTokenStream->TokenNextIndex && !UnsafePackedBinaryWriter.IsObjectKey(this.m_JsonTokenStream->Tokens, this.JsonTokenNextIndex - 1))
			{
				while (this.JsonTokenParentIndex >= 0)
				{
					int jsonTokenParentIndex = this.JsonTokenParentIndex;
					Token token4 = this.m_JsonTokenStream->Tokens[jsonTokenParentIndex];
					if (token4.End == -1)
					{
						break;
					}
					this.JsonTokenParentIndex = token4.Parent;
					if (this.m_PackedBinaryStream->TokenParentIndex != -1)
					{
						this.m_PackedBinaryStream->TokenParentIndex = this.m_PackedBinaryStream->Tokens[this.m_PackedBinaryStream->TokenParentIndex].Parent;
					}
				}
			}
			return num;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007320 File Offset: 0x00005520
		private unsafe void WriteCharacters(int inputTokenStartIndex, int outputTokenStartIndex, UnsafeBuffer<char> buffer, int count)
		{
			for (int i = 0; i < count; i++)
			{
				int num = i + inputTokenStartIndex;
				Token token = this.m_JsonTokenStream->Tokens[num];
				TokenType type = token.Type;
				if (type - TokenType.String <= 2)
				{
					int num2 = i + outputTokenStartIndex;
					int num3 = this.m_PackedBinaryStream->Tokens[num2].Position;
					if (token.Start != -1)
					{
						num3 += 4;
					}
					int num4 = ((token.Start != -1) ? token.Start : 0);
					int num5 = ((token.End != -1) ? token.End : buffer.Length);
					if (token.Type == TokenType.String && this.StripStringEscapeCharacters)
					{
						int j = num4;
						if (j < num5 && token.Start == -1 && this.m_PackedBinaryStream->Tokens[num2 - 1].LastCharacterIsEscaped)
						{
							UnsafePackedBinaryWriter.<WriteCharacters>g__WriteCharacterEscaped|11_0(ref this, ref num3, buffer.Buffer[(IntPtr)(j++) * 2]);
						}
						while (j < num5)
						{
							if (buffer.Buffer[j] == '\\')
							{
								j++;
								if (j < num5)
								{
									UnsafePackedBinaryWriter.<WriteCharacters>g__WriteCharacterEscaped|11_0(ref this, ref num3, buffer.Buffer[j]);
								}
							}
							else
							{
								this.Write<char>(ref num3, buffer.Buffer[j]);
							}
							j++;
						}
					}
					else
					{
						for (int k = num4; k < num5; k++)
						{
							this.Write<ushort>(ref num3, (ushort)buffer.Buffer[k]);
						}
					}
					if (token.End != -1)
					{
						int num6 = num;
						while (token.Start == -1 && token.Parent != -1)
						{
							num6 = token.Parent;
							token = this.m_JsonTokenStream->Tokens[num6];
						}
						int num7 = num6 - num;
						int position = this.m_PackedBinaryStream->Tokens[num2 + num7].Position;
						int num8 = num3 - position;
						num8 -= 4;
						this.Write<int>(ref position, num8 / 2);
					}
				}
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000753E File Offset: 0x0000573E
		private unsafe void Write<[IsUnmanaged] T>(ref int position, T value) where T : struct, ValueType
		{
			*(T*)(this.m_PackedBinaryStream->Buffer + position) = value;
			position += sizeof(T);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007560 File Offset: 0x00005760
		private unsafe static bool IsObjectKey(Token* tokens, int index)
		{
			Token token = tokens[index];
			return (token.Type == TokenType.String || token.Type == TokenType.Primitive) && token.Parent != -1 && tokens[token.Parent].Type == TokenType.Object;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000075BC File Offset: 0x000057BC
		[CompilerGenerated]
		internal static void <WriteCharacters>g__WriteCharacterEscaped|11_0(ref UnsafePackedBinaryWriter writer, ref int p, char c)
		{
			if (c <= '\\')
			{
				if (c == '"')
				{
					writer.Write<char>(ref p, '"');
					return;
				}
				if (c == '0')
				{
					writer.Write<char>(ref p, '\0');
					return;
				}
				if (c == '\\')
				{
					writer.Write<char>(ref p, '\\');
					return;
				}
			}
			else if (c <= 'n')
			{
				if (c == 'b')
				{
					writer.Write<char>(ref p, '\b');
					return;
				}
				if (c == 'n')
				{
					writer.Write<char>(ref p, '\n');
					return;
				}
			}
			else
			{
				if (c == 'r')
				{
					writer.Write<char>(ref p, '\r');
					return;
				}
				if (c == 't')
				{
					writer.Write<char>(ref p, '\t');
					return;
				}
			}
			writer.Write<char>(ref p, '\0');
		}

		// Token: 0x040000BE RID: 190
		private unsafe readonly UnsafeJsonTokenStream* m_JsonTokenStream;

		// Token: 0x040000BF RID: 191
		private unsafe readonly UnsafePackedBinaryStream* m_PackedBinaryStream;

		// Token: 0x040000C0 RID: 192
		public int JsonTokenNextIndex;

		// Token: 0x040000C1 RID: 193
		public int JsonTokenParentIndex;

		// Token: 0x040000C2 RID: 194
		public bool StripStringEscapeCharacters;
	}
}
