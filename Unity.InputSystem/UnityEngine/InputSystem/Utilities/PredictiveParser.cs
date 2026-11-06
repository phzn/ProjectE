using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000144 RID: 324
	internal struct PredictiveParser
	{
		// Token: 0x060011B3 RID: 4531 RVA: 0x00054720 File Offset: 0x00052920
		public unsafe void ExpectSingleChar(ReadOnlySpan<char> str, char c)
		{
			if (*str[this.m_Position] != (ushort)c)
			{
				throw new InvalidOperationException(string.Format("Expected a '{0}' character at position {1} in : {2}", c, this.m_Position, str.ToString()));
			}
			this.m_Position++;
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0005477C File Offset: 0x0005297C
		public unsafe int ExpectInt(ReadOnlySpan<char> str)
		{
			int num = this.m_Position;
			int num2 = 1;
			if (*str[num] == 45)
			{
				num2 = -1;
				num++;
			}
			int num3 = 0;
			for (;;)
			{
				char c = (char)(*str[num]);
				if (c < '0' || c > '9')
				{
					break;
				}
				num3 *= 10;
				num3 += (int)(c - '0');
				num++;
			}
			if (this.m_Position == num)
			{
				throw new InvalidOperationException(string.Format("Expected an int at position {0} in {1}", this.m_Position, str.ToString()));
			}
			this.m_Position = num;
			return num3 * num2;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00054808 File Offset: 0x00052A08
		public unsafe ReadOnlySpan<char> ExpectString(ReadOnlySpan<char> str)
		{
			int position = this.m_Position;
			if (*str[position] != 34)
			{
				throw new InvalidOperationException(string.Format("Expected a '\"' character at position {0} in {1}", this.m_Position, str.ToString()));
			}
			this.m_Position++;
			for (;;)
			{
				char c = (char)(*str[this.m_Position]);
				c |= ' ';
				if (c < 'a' || c > 'z')
				{
					break;
				}
				this.m_Position++;
			}
			if (*str[this.m_Position] != 34)
			{
				throw new InvalidOperationException(string.Format("Expected a closing '\"' character at position {0} in string: {1}", this.m_Position, str.ToString()));
			}
			if (this.m_Position - position == 1)
			{
				return ReadOnlySpan<char>.Empty;
			}
			ReadOnlySpan<char> readOnlySpan = str.Slice(position + 1, this.m_Position - position - 1);
			this.m_Position++;
			return readOnlySpan;
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x000548FB File Offset: 0x00052AFB
		public unsafe bool AcceptSingleChar(ReadOnlySpan<char> str, char c)
		{
			if (*str[this.m_Position] != (ushort)c)
			{
				return false;
			}
			this.m_Position++;
			return true;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00054920 File Offset: 0x00052B20
		public unsafe bool AcceptString(ReadOnlySpan<char> input, out ReadOnlySpan<char> output)
		{
			output = default(ReadOnlySpan<char>);
			int position = this.m_Position;
			int num = position;
			if (*input[num] != 34)
			{
				return false;
			}
			num++;
			for (;;)
			{
				char c = (char)(*input[num]);
				c |= ' ';
				if (c < 'a' || c > 'z')
				{
					break;
				}
				num++;
			}
			if (*input[num] != 34)
			{
				return false;
			}
			if (this.m_Position - position == 1)
			{
				output = ReadOnlySpan<char>.Empty;
			}
			else
			{
				output = input.Slice(position + 1, num - position - 1);
			}
			this.m_Position = num + 1;
			return true;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x000549B8 File Offset: 0x00052BB8
		public unsafe void AcceptInt(ReadOnlySpan<char> str)
		{
			if (*str[this.m_Position] == 45)
			{
				this.m_Position++;
			}
			for (;;)
			{
				char c = (char)(*str[this.m_Position]);
				if (c < '0' || c > '9')
				{
					break;
				}
				this.m_Position++;
			}
		}

		// Token: 0x04000728 RID: 1832
		private int m_Position;
	}
}
