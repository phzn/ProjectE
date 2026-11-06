using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002EC RID: 748
	internal struct InheritedData : IStyleDataGroup<InheritedData>, IEquatable<InheritedData>
	{
		// Token: 0x06001956 RID: 6486 RVA: 0x00061F68 File Offset: 0x00060168
		public InheritedData Copy()
		{
			return this;
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x00061F80 File Offset: 0x00060180
		public void CopyFrom(ref InheritedData other)
		{
			this = other;
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00061F90 File Offset: 0x00060190
		public static bool operator ==(InheritedData lhs, InheritedData rhs)
		{
			return lhs.color == rhs.color && lhs.fontSize == rhs.fontSize && lhs.letterSpacing == rhs.letterSpacing && lhs.textShadow == rhs.textShadow && lhs.unityFont == rhs.unityFont && lhs.unityFontDefinition == rhs.unityFontDefinition && lhs.unityFontStyleAndWeight == rhs.unityFontStyleAndWeight && lhs.unityParagraphSpacing == rhs.unityParagraphSpacing && lhs.unityTextAlign == rhs.unityTextAlign && lhs.unityTextOutlineColor == rhs.unityTextOutlineColor && lhs.unityTextOutlineWidth == rhs.unityTextOutlineWidth && lhs.visibility == rhs.visibility && lhs.whiteSpace == rhs.whiteSpace && lhs.wordSpacing == rhs.wordSpacing;
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x000620A4 File Offset: 0x000602A4
		public static bool operator !=(InheritedData lhs, InheritedData rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x000620C0 File Offset: 0x000602C0
		public bool Equals(InheritedData other)
		{
			return other == this;
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x000620E0 File Offset: 0x000602E0
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is InheritedData && this.Equals((InheritedData)obj);
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00062118 File Offset: 0x00060318
		public override int GetHashCode()
		{
			int num = this.color.GetHashCode();
			num = (num * 397) ^ this.fontSize.GetHashCode();
			num = (num * 397) ^ this.letterSpacing.GetHashCode();
			num = (num * 397) ^ this.textShadow.GetHashCode();
			num = (num * 397) ^ ((this.unityFont == null) ? 0 : this.unityFont.GetHashCode());
			num = (num * 397) ^ this.unityFontDefinition.GetHashCode();
			num = (num * 397) ^ (int)this.unityFontStyleAndWeight;
			num = (num * 397) ^ this.unityParagraphSpacing.GetHashCode();
			num = (num * 397) ^ (int)this.unityTextAlign;
			num = (num * 397) ^ this.unityTextOutlineColor.GetHashCode();
			num = (num * 397) ^ this.unityTextOutlineWidth.GetHashCode();
			num = (num * 397) ^ (int)this.visibility;
			num = (num * 397) ^ (int)this.whiteSpace;
			return (num * 397) ^ this.wordSpacing.GetHashCode();
		}

		// Token: 0x04000A71 RID: 2673
		public Color color;

		// Token: 0x04000A72 RID: 2674
		public Length fontSize;

		// Token: 0x04000A73 RID: 2675
		public Length letterSpacing;

		// Token: 0x04000A74 RID: 2676
		public TextShadow textShadow;

		// Token: 0x04000A75 RID: 2677
		public Font unityFont;

		// Token: 0x04000A76 RID: 2678
		public FontDefinition unityFontDefinition;

		// Token: 0x04000A77 RID: 2679
		public FontStyle unityFontStyleAndWeight;

		// Token: 0x04000A78 RID: 2680
		public Length unityParagraphSpacing;

		// Token: 0x04000A79 RID: 2681
		public TextAnchor unityTextAlign;

		// Token: 0x04000A7A RID: 2682
		public Color unityTextOutlineColor;

		// Token: 0x04000A7B RID: 2683
		public float unityTextOutlineWidth;

		// Token: 0x04000A7C RID: 2684
		public Visibility visibility;

		// Token: 0x04000A7D RID: 2685
		public WhiteSpace whiteSpace;

		// Token: 0x04000A7E RID: 2686
		public Length wordSpacing;
	}
}
