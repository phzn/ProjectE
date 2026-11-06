using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002EE RID: 750
	internal struct RareData : IStyleDataGroup<RareData>, IEquatable<RareData>
	{
		// Token: 0x06001964 RID: 6500 RVA: 0x00062884 File Offset: 0x00060A84
		public RareData Copy()
		{
			return this;
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0006289C File Offset: 0x00060A9C
		public void CopyFrom(ref RareData other)
		{
			this = other;
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x000628AC File Offset: 0x00060AAC
		public static bool operator ==(RareData lhs, RareData rhs)
		{
			return lhs.cursor == rhs.cursor && lhs.textOverflow == rhs.textOverflow && lhs.unityBackgroundImageTintColor == rhs.unityBackgroundImageTintColor && lhs.unityOverflowClipBox == rhs.unityOverflowClipBox && lhs.unitySliceBottom == rhs.unitySliceBottom && lhs.unitySliceLeft == rhs.unitySliceLeft && lhs.unitySliceRight == rhs.unitySliceRight && lhs.unitySliceScale == rhs.unitySliceScale && lhs.unitySliceTop == rhs.unitySliceTop && lhs.unityTextOverflowPosition == rhs.unityTextOverflowPosition;
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0006295C File Offset: 0x00060B5C
		public static bool operator !=(RareData lhs, RareData rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x00062978 File Offset: 0x00060B78
		public bool Equals(RareData other)
		{
			return other == this;
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x00062998 File Offset: 0x00060B98
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RareData && this.Equals((RareData)obj);
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x000629D0 File Offset: 0x00060BD0
		public override int GetHashCode()
		{
			int num = this.cursor.GetHashCode();
			num = (num * 397) ^ (int)this.textOverflow;
			num = (num * 397) ^ this.unityBackgroundImageTintColor.GetHashCode();
			num = (num * 397) ^ (int)this.unityOverflowClipBox;
			num = (num * 397) ^ this.unitySliceBottom;
			num = (num * 397) ^ this.unitySliceLeft;
			num = (num * 397) ^ this.unitySliceRight;
			num = (num * 397) ^ this.unitySliceScale.GetHashCode();
			num = (num * 397) ^ this.unitySliceTop;
			return (num * 397) ^ (int)this.unityTextOverflowPosition;
		}

		// Token: 0x04000AA0 RID: 2720
		public Cursor cursor;

		// Token: 0x04000AA1 RID: 2721
		public TextOverflow textOverflow;

		// Token: 0x04000AA2 RID: 2722
		public Color unityBackgroundImageTintColor;

		// Token: 0x04000AA3 RID: 2723
		public OverflowClipBox unityOverflowClipBox;

		// Token: 0x04000AA4 RID: 2724
		public int unitySliceBottom;

		// Token: 0x04000AA5 RID: 2725
		public int unitySliceLeft;

		// Token: 0x04000AA6 RID: 2726
		public int unitySliceRight;

		// Token: 0x04000AA7 RID: 2727
		public float unitySliceScale;

		// Token: 0x04000AA8 RID: 2728
		public int unitySliceTop;

		// Token: 0x04000AA9 RID: 2729
		public TextOverflowPosition unityTextOverflowPosition;
	}
}
