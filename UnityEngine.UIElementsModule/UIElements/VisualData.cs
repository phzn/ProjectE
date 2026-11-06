using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002F1 RID: 753
	internal struct VisualData : IStyleDataGroup<VisualData>, IEquatable<VisualData>
	{
		// Token: 0x06001979 RID: 6521 RVA: 0x00062E68 File Offset: 0x00061068
		public VisualData Copy()
		{
			return this;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00062E80 File Offset: 0x00061080
		public void CopyFrom(ref VisualData other)
		{
			this = other;
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00062E90 File Offset: 0x00061090
		public static bool operator ==(VisualData lhs, VisualData rhs)
		{
			return lhs.backgroundColor == rhs.backgroundColor && lhs.backgroundImage == rhs.backgroundImage && lhs.backgroundPositionX == rhs.backgroundPositionX && lhs.backgroundPositionY == rhs.backgroundPositionY && lhs.backgroundRepeat == rhs.backgroundRepeat && lhs.backgroundSize == rhs.backgroundSize && lhs.borderBottomColor == rhs.borderBottomColor && lhs.borderBottomLeftRadius == rhs.borderBottomLeftRadius && lhs.borderBottomRightRadius == rhs.borderBottomRightRadius && lhs.borderLeftColor == rhs.borderLeftColor && lhs.borderRightColor == rhs.borderRightColor && lhs.borderTopColor == rhs.borderTopColor && lhs.borderTopLeftRadius == rhs.borderTopLeftRadius && lhs.borderTopRightRadius == rhs.borderTopRightRadius && lhs.opacity == rhs.opacity && lhs.overflow == rhs.overflow;
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00062FE4 File Offset: 0x000611E4
		public static bool operator !=(VisualData lhs, VisualData rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00063000 File Offset: 0x00061200
		public bool Equals(VisualData other)
		{
			return other == this;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x00063020 File Offset: 0x00061220
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is VisualData && this.Equals((VisualData)obj);
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00063058 File Offset: 0x00061258
		public override int GetHashCode()
		{
			int num = this.backgroundColor.GetHashCode();
			num = (num * 397) ^ this.backgroundImage.GetHashCode();
			num = (num * 397) ^ this.backgroundPositionX.GetHashCode();
			num = (num * 397) ^ this.backgroundPositionY.GetHashCode();
			num = (num * 397) ^ this.backgroundRepeat.GetHashCode();
			num = (num * 397) ^ this.backgroundSize.GetHashCode();
			num = (num * 397) ^ this.borderBottomColor.GetHashCode();
			num = (num * 397) ^ this.borderBottomLeftRadius.GetHashCode();
			num = (num * 397) ^ this.borderBottomRightRadius.GetHashCode();
			num = (num * 397) ^ this.borderLeftColor.GetHashCode();
			num = (num * 397) ^ this.borderRightColor.GetHashCode();
			num = (num * 397) ^ this.borderTopColor.GetHashCode();
			num = (num * 397) ^ this.borderTopLeftRadius.GetHashCode();
			num = (num * 397) ^ this.borderTopRightRadius.GetHashCode();
			num = (num * 397) ^ this.opacity.GetHashCode();
			return (num * 397) ^ (int)this.overflow;
		}

		// Token: 0x04000AB2 RID: 2738
		public Color backgroundColor;

		// Token: 0x04000AB3 RID: 2739
		public Background backgroundImage;

		// Token: 0x04000AB4 RID: 2740
		public BackgroundPosition backgroundPositionX;

		// Token: 0x04000AB5 RID: 2741
		public BackgroundPosition backgroundPositionY;

		// Token: 0x04000AB6 RID: 2742
		public BackgroundRepeat backgroundRepeat;

		// Token: 0x04000AB7 RID: 2743
		public BackgroundSize backgroundSize;

		// Token: 0x04000AB8 RID: 2744
		public Color borderBottomColor;

		// Token: 0x04000AB9 RID: 2745
		public Length borderBottomLeftRadius;

		// Token: 0x04000ABA RID: 2746
		public Length borderBottomRightRadius;

		// Token: 0x04000ABB RID: 2747
		public Color borderLeftColor;

		// Token: 0x04000ABC RID: 2748
		public Color borderRightColor;

		// Token: 0x04000ABD RID: 2749
		public Color borderTopColor;

		// Token: 0x04000ABE RID: 2750
		public Length borderTopLeftRadius;

		// Token: 0x04000ABF RID: 2751
		public Length borderTopRightRadius;

		// Token: 0x04000AC0 RID: 2752
		public float opacity;

		// Token: 0x04000AC1 RID: 2753
		public OverflowInternal overflow;
	}
}
