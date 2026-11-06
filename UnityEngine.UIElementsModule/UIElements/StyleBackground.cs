using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002F9 RID: 761
	public struct StyleBackground : IStyleValue<Background>, IEquatable<StyleBackground>
	{
		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x000681C4 File Offset: 0x000663C4
		// (set) Token: 0x060019D5 RID: 6613 RVA: 0x000681EF File Offset: 0x000663EF
		public Background value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(Background);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x00068200 File Offset: 0x00066400
		// (set) Token: 0x060019D7 RID: 6615 RVA: 0x00068218 File Offset: 0x00066418
		public StyleKeyword keyword
		{
			get
			{
				return this.m_Keyword;
			}
			set
			{
				this.m_Keyword = value;
			}
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x00068222 File Offset: 0x00066422
		public StyleBackground(Background v)
		{
			this = new StyleBackground(v, StyleKeyword.Undefined);
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x0006822E File Offset: 0x0006642E
		public StyleBackground(Texture2D v)
		{
			this = new StyleBackground(v, StyleKeyword.Undefined);
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0006823A File Offset: 0x0006643A
		public StyleBackground(Sprite v)
		{
			this = new StyleBackground(v, StyleKeyword.Undefined);
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00068246 File Offset: 0x00066446
		public StyleBackground(VectorImage v)
		{
			this = new StyleBackground(v, StyleKeyword.Undefined);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00068254 File Offset: 0x00066454
		public StyleBackground(StyleKeyword keyword)
		{
			this = new StyleBackground(default(Background), keyword);
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00068273 File Offset: 0x00066473
		internal StyleBackground(Texture2D v, StyleKeyword keyword)
		{
			this = new StyleBackground(Background.FromTexture2D(v), keyword);
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00068284 File Offset: 0x00066484
		internal StyleBackground(Sprite v, StyleKeyword keyword)
		{
			this = new StyleBackground(Background.FromSprite(v), keyword);
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00068295 File Offset: 0x00066495
		internal StyleBackground(VectorImage v, StyleKeyword keyword)
		{
			this = new StyleBackground(Background.FromVectorImage(v), keyword);
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x000682A6 File Offset: 0x000664A6
		internal StyleBackground(Background v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x000682B8 File Offset: 0x000664B8
		public static bool operator ==(StyleBackground lhs, StyleBackground rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000682EC File Offset: 0x000664EC
		public static bool operator !=(StyleBackground lhs, StyleBackground rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x00068308 File Offset: 0x00066508
		public static implicit operator StyleBackground(StyleKeyword keyword)
		{
			return new StyleBackground(keyword);
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00068320 File Offset: 0x00066520
		public static implicit operator StyleBackground(Background v)
		{
			return new StyleBackground(v);
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00068338 File Offset: 0x00066538
		public static implicit operator StyleBackground(Texture2D v)
		{
			return new StyleBackground(v);
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00068350 File Offset: 0x00066550
		public bool Equals(StyleBackground other)
		{
			return other == this;
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x00068370 File Offset: 0x00066570
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleBackground)
			{
				StyleBackground styleBackground = (StyleBackground)obj;
				flag = this.Equals(styleBackground);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0006839C File Offset: 0x0006659C
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x000683D0 File Offset: 0x000665D0
		public override string ToString()
		{
			return this.DebugString<Background>();
		}

		// Token: 0x04000AD7 RID: 2775
		private Background m_Value;

		// Token: 0x04000AD8 RID: 2776
		private StyleKeyword m_Keyword;
	}
}
