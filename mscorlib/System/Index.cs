using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000145 RID: 325
	public readonly struct Index : IEquatable<Index>
	{
		// Token: 0x06000C07 RID: 3079 RVA: 0x00031DA2 File Offset: 0x0002FFA2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Index(int value, bool fromEnd = false)
		{
			if (value < 0)
			{
				ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
			}
			if (fromEnd)
			{
				this._value = ~value;
				return;
			}
			this._value = value;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00031DC0 File Offset: 0x0002FFC0
		private Index(int value)
		{
			this._value = value;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x00031DC9 File Offset: 0x0002FFC9
		public static Index Start
		{
			get
			{
				return new Index(0);
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00031DD1 File Offset: 0x0002FFD1
		public static Index End
		{
			get
			{
				return new Index(-1);
			}
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00031DD9 File Offset: 0x0002FFD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Index FromStart(int value)
		{
			if (value < 0)
			{
				ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
			}
			return new Index(value);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00031DEA File Offset: 0x0002FFEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Index FromEnd(int value)
		{
			if (value < 0)
			{
				ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
			}
			return new Index(~value);
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00031DFC File Offset: 0x0002FFFC
		public int Value
		{
			get
			{
				if (this._value < 0)
				{
					return ~this._value;
				}
				return this._value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00031E15 File Offset: 0x00030015
		public bool IsFromEnd
		{
			get
			{
				return this._value < 0;
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00031E20 File Offset: 0x00030020
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetOffset(int length)
		{
			int num;
			if (this.IsFromEnd)
			{
				num = length - ~this._value;
			}
			else
			{
				num = this._value;
			}
			return num;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00031E49 File Offset: 0x00030049
		public override bool Equals(object value)
		{
			return value is Index && this._value == ((Index)value)._value;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00031E68 File Offset: 0x00030068
		public bool Equals(Index other)
		{
			return this._value == other._value;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00031E78 File Offset: 0x00030078
		public override int GetHashCode()
		{
			return this._value;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00031E80 File Offset: 0x00030080
		public static implicit operator Index(int value)
		{
			return Index.FromStart(value);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00031E88 File Offset: 0x00030088
		public override string ToString()
		{
			if (this.IsFromEnd)
			{
				return this.ToStringFromEnd();
			}
			return ((uint)this.Value).ToString();
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00031EB4 File Offset: 0x000300B4
		private unsafe string ToStringFromEnd()
		{
			Span<char> span = new Span<char>(stackalloc byte[(UIntPtr)22], 11);
			int num;
			((uint)this.Value).TryFormat(span.Slice(1), out num, default(ReadOnlySpan<char>), null);
			*span[0] = '^';
			return new string(span.Slice(0, num + 1));
		}

		// Token: 0x04001251 RID: 4689
		private readonly int _value;
	}
}
