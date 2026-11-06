using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.UIElements
{
	// Token: 0x020002B4 RID: 692
	internal struct TextureId
	{
		// Token: 0x0600140F RID: 5135 RVA: 0x00047720 File Offset: 0x00045920
		public TextureId(int index)
		{
			this.m_Index = index + 1;
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0004772C File Offset: 0x0004592C
		public int index
		{
			get
			{
				return this.m_Index - 1;
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x00047738 File Offset: 0x00045938
		public bool IsValid()
		{
			return this.m_Index > 0;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x00047754 File Offset: 0x00045954
		public float ConvertToGpu()
		{
			return (float)this.index;
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00047770 File Offset: 0x00045970
		public override bool Equals(object obj)
		{
			bool flag = !(obj is TextureId);
			return !flag && (TextureId)obj == this;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x000477A8 File Offset: 0x000459A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(TextureId other)
		{
			return this.m_Index == other.m_Index;
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x000477C8 File Offset: 0x000459C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return this.m_Index.GetHashCode();
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x000477E8 File Offset: 0x000459E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(TextureId left, TextureId right)
		{
			return left.m_Index == right.m_Index;
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00047808 File Offset: 0x00045A08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(TextureId left, TextureId right)
		{
			return !(left == right);
		}

		// Token: 0x04000952 RID: 2386
		private readonly int m_Index;

		// Token: 0x04000953 RID: 2387
		public static readonly TextureId invalid = new TextureId(-1);
	}
}
