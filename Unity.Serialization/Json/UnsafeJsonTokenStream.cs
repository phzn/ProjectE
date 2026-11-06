using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000044 RID: 68
	internal struct UnsafeJsonTokenStream : IDisposable
	{
		// Token: 0x0600016F RID: 367 RVA: 0x000062E0 File Offset: 0x000044E0
		public unsafe UnsafeJsonTokenStream(int initialTokenCapacity, Allocator label)
		{
			this.m_Label = label;
			this.Tokens = (Token*)UnsafeUtility.Malloc((long)(initialTokenCapacity * sizeof(Token)), 4, label);
			this.Discard = (int*)UnsafeUtility.Malloc((long)(initialTokenCapacity * 4), 4, label);
			this.TokenCapacity = initialTokenCapacity;
			this.TokenNextIndex = 0;
			this.TokenParentIndex = -1;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006330 File Offset: 0x00004530
		public unsafe void Dispose()
		{
			UnsafeUtility.Free((void*)this.Tokens, this.m_Label);
			UnsafeUtility.Free((void*)this.Discard, this.m_Label);
			this.Tokens = null;
			this.Discard = null;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006364 File Offset: 0x00004564
		public unsafe void Add(Token token)
		{
			if (this.TokenNextIndex >= this.TokenCapacity)
			{
				this.Grow();
			}
			ref Token tokens = ref *this.Tokens;
			int tokenNextIndex = this.TokenNextIndex;
			this.TokenNextIndex = tokenNextIndex + 1;
			*((ref tokens) + (IntPtr)tokenNextIndex * (IntPtr)sizeof(Token)) = token;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000063AB File Offset: 0x000045AB
		public void Reset()
		{
			this.TokenNextIndex = 0;
			this.TokenParentIndex = -1;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000063BC File Offset: 0x000045BC
		public unsafe int DiscardCompleted(int depth = 128)
		{
			if (this.TokenNextIndex == 0)
			{
				return 0;
			}
			int* ptr;
			int i;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)depth) * 4];
				i = -1;
			}
			Token token;
			for (int num = this.TokenNextIndex - 1; num != -1; num = token.Parent)
			{
				token = this.Tokens[num];
				if (token.Start != -1)
				{
					if (token.End == -1 && (token.Type == TokenType.Primitive || token.Type == TokenType.String))
					{
						Token token2 = token;
						int num2 = num;
						int num3 = 1;
						while (token2.End == -1 && num2 < this.TokenNextIndex - 1)
						{
							num3++;
							token2 = this.Tokens[(IntPtr)(++num2) * (IntPtr)sizeof(Token)];
						}
						if (i + num3 >= depth)
						{
							return -1;
						}
						for (int j = num3 - 1; j >= 0; j--)
						{
							ptr[(IntPtr)(++i) * 4] = num + j;
						}
					}
					else
					{
						if (i + 1 >= depth)
						{
							return -1;
						}
						ptr[(IntPtr)(++i) * 4] = num;
					}
				}
			}
			this.TokenNextIndex = i + 1;
			int num4 = 0;
			while (i >= 0)
			{
				int num = ptr[i];
				if (this.TokenParentIndex == num)
				{
					this.TokenParentIndex = num4;
				}
				Token token3 = this.Tokens[num];
				int num5 = num4 - 1;
				if (token3.Start != -1)
				{
					while (num5 != -1 && this.Tokens[num5].Start == -1)
					{
						num5--;
					}
				}
				token3.Parent = num5;
				this.Tokens[num4] = token3;
				this.Discard[num] = num4;
				num4++;
				i--;
			}
			return 0;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006578 File Offset: 0x00004778
		private void Grow()
		{
			int num = this.TokenCapacity * 2;
			this.Tokens = NativeArrayUtility.Resize<Token>(this.Tokens, this.TokenCapacity, num, 4, this.m_Label);
			this.Discard = NativeArrayUtility.Resize<int>(this.Discard, this.TokenCapacity, num, 4, this.m_Label);
			this.TokenCapacity = num;
		}

		// Token: 0x040000A2 RID: 162
		public const int PartialTokenStart = -1;

		// Token: 0x040000A3 RID: 163
		public const int PartialTokenEnd = -1;

		// Token: 0x040000A4 RID: 164
		public const int DefaultDiscardDepthLimit = 128;

		// Token: 0x040000A5 RID: 165
		private readonly Allocator m_Label;

		// Token: 0x040000A6 RID: 166
		public unsafe Token* Tokens;

		// Token: 0x040000A7 RID: 167
		public int TokenCapacity;

		// Token: 0x040000A8 RID: 168
		public int TokenNextIndex;

		// Token: 0x040000A9 RID: 169
		public int TokenParentIndex;

		// Token: 0x040000AA RID: 170
		public unsafe int* Discard;
	}
}
