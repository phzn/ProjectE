using System;
using System.Runtime.CompilerServices;

namespace MagicaCloth2
{
	// Token: 0x0200015D RID: 349
	[Serializable]
	public struct VertexAttribute : IEquatable<VertexAttribute>
	{
		// Token: 0x06000678 RID: 1656 RVA: 0x000396A5 File Offset: 0x000378A5
		public VertexAttribute(byte initialValue = 0)
		{
			this.Value = initialValue;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x000396AE File Offset: 0x000378AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			this.Value = 0;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000396B7 File Offset: 0x000378B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFlag(byte flag, bool sw)
		{
			if (sw)
			{
				this.Value |= flag;
				return;
			}
			this.Value &= ~flag;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000396DC File Offset: 0x000378DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFlag(VertexAttribute attr, bool sw)
		{
			if (sw)
			{
				this.Value |= attr.Value;
				return;
			}
			this.Value &= ~attr.Value;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0003970B File Offset: 0x0003790B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSet(byte flag)
		{
			return (this.Value & flag) > 0;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00039718 File Offset: 0x00037918
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsInvalid()
		{
			return !this.IsSet(3);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00039724 File Offset: 0x00037924
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsFixed()
		{
			return this.IsSet(1);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0003972D File Offset: 0x0003792D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsMove()
		{
			return this.IsSet(2);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00039736 File Offset: 0x00037936
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsDontMove()
		{
			return !this.IsSet(2);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00039742 File Offset: 0x00037942
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsMotion()
		{
			return !this.IsSet(8);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0003974E File Offset: 0x0003794E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsDisableCollision()
		{
			return this.IsSet(16);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00039758 File Offset: 0x00037958
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VertexAttribute JoinAttribute(VertexAttribute attr1, VertexAttribute attr2)
		{
			if (attr1.Value >= attr2.Value)
			{
				return attr2;
			}
			return attr1;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0003976B File Offset: 0x0003796B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(VertexAttribute other)
		{
			return this.Value == other.Value;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0003977C File Offset: 0x0003797C
		public override bool Equals(object obj)
		{
			if (obj is VertexAttribute)
			{
				VertexAttribute vertexAttribute = (VertexAttribute)obj;
				return this.Equals(vertexAttribute);
			}
			return false;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x000397A1 File Offset: 0x000379A1
		public override int GetHashCode()
		{
			return (int)this.Value;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0003976B File Offset: 0x0003796B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(VertexAttribute lhs, VertexAttribute rhs)
		{
			return lhs.Value == rhs.Value;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000397A9 File Offset: 0x000379A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(VertexAttribute lhs, VertexAttribute rhs)
		{
			return lhs.Value != rhs.Value;
		}

		// Token: 0x04000A84 RID: 2692
		public const byte Flag_Fixed = 1;

		// Token: 0x04000A85 RID: 2693
		public const byte Flag_Move = 2;

		// Token: 0x04000A86 RID: 2694
		public const byte Flag_InvalidMotion = 8;

		// Token: 0x04000A87 RID: 2695
		public const byte Flag_DisableCollision = 16;

		// Token: 0x04000A88 RID: 2696
		public const byte Flag_Triangle = 128;

		// Token: 0x04000A89 RID: 2697
		public static readonly VertexAttribute Invalid = default(VertexAttribute);

		// Token: 0x04000A8A RID: 2698
		public static readonly VertexAttribute Fixed = new VertexAttribute(1);

		// Token: 0x04000A8B RID: 2699
		public static readonly VertexAttribute Move = new VertexAttribute(2);

		// Token: 0x04000A8C RID: 2700
		public static readonly VertexAttribute DisableCollision = new VertexAttribute(16);

		// Token: 0x04000A8D RID: 2701
		public byte Value;
	}
}
