using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json.Unsafe
{
	// Token: 0x0200008E RID: 142
	internal readonly struct UnsafeArrayView : IEnumerable<UnsafeValueView>, IEnumerable
	{
		// Token: 0x0600037C RID: 892 RVA: 0x0000F0D6 File Offset: 0x0000D2D6
		internal unsafe UnsafeArrayView(UnsafePackedBinaryStream* stream, int tokenIndex)
		{
			this.m_Stream = stream;
			this.m_TokenIndex = tokenIndex;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000F0E8 File Offset: 0x0000D2E8
		public unsafe int Count()
		{
			int num = 0;
			int length = this.m_Stream->GetToken(this.m_TokenIndex).Length;
			if (length == 1)
			{
				return 0;
			}
			int i = this.m_Stream->GetFirstChildIndex(this.m_TokenIndex);
			int num2 = this.m_TokenIndex + length;
			while (i < num2)
			{
				i += this.m_Stream->GetToken(i).Length;
				num++;
			}
			return num;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000F14E File Offset: 0x0000D34E
		public IEnumerator<UnsafeValueView> GetEnumerator()
		{
			return new UnsafeArrayView.Enumerator(this.m_Stream, this.m_TokenIndex);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000F166 File Offset: 0x0000D366
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000F16E File Offset: 0x0000D36E
		public unsafe SerializedArrayView AsSafe()
		{
			return new SerializedArrayView(this.m_Stream, this.m_Stream->GetHandle(this.m_TokenIndex));
		}

		// Token: 0x040001C4 RID: 452
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001C5 RID: 453
		private readonly int m_TokenIndex;

		// Token: 0x0200008F RID: 143
		private struct Enumerator : IEnumerator<UnsafeValueView>, IEnumerator, IDisposable
		{
			// Token: 0x06000381 RID: 897 RVA: 0x0000F18C File Offset: 0x0000D38C
			internal unsafe Enumerator(UnsafePackedBinaryStream* stream, int index)
			{
				this.m_Stream = stream;
				this.m_Start = index;
				this.m_End = index + this.m_Stream->GetToken(index).Length;
				this.m_Position = index;
			}

			// Token: 0x06000382 RID: 898 RVA: 0x0000F1BC File Offset: 0x0000D3BC
			public unsafe bool MoveNext()
			{
				if (this.m_Position >= this.m_End)
				{
					throw new InvalidOperationException();
				}
				int length = this.m_Stream->GetToken(this.m_Position).Length;
				if (this.m_Position == this.m_Start)
				{
					if (length == 1)
					{
						return false;
					}
					this.m_Position = this.m_Stream->GetFirstChildIndex(this.m_Position);
					return true;
				}
				else
				{
					if (this.m_Position + length < this.m_End)
					{
						this.m_Position += length;
						return true;
					}
					return false;
				}
			}

			// Token: 0x17000069 RID: 105
			// (get) Token: 0x06000383 RID: 899 RVA: 0x0000F242 File Offset: 0x0000D442
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x1700006A RID: 106
			// (get) Token: 0x06000384 RID: 900 RVA: 0x0000F24F File Offset: 0x0000D44F
			public UnsafeValueView Current
			{
				get
				{
					return new UnsafeValueView(this.m_Stream, this.m_Position);
				}
			}

			// Token: 0x06000385 RID: 901 RVA: 0x0000F262 File Offset: 0x0000D462
			public void Reset()
			{
				this.m_Position = this.m_Start;
			}

			// Token: 0x06000386 RID: 902 RVA: 0x0000578E File Offset: 0x0000398E
			public void Dispose()
			{
			}

			// Token: 0x040001C6 RID: 454
			private unsafe readonly UnsafePackedBinaryStream* m_Stream;

			// Token: 0x040001C7 RID: 455
			private readonly int m_Start;

			// Token: 0x040001C8 RID: 456
			private readonly int m_End;

			// Token: 0x040001C9 RID: 457
			private int m_Position;
		}
	}
}
