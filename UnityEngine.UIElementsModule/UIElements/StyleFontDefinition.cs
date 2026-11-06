using System;
using System.Runtime.InteropServices;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x02000304 RID: 772
	public struct StyleFontDefinition : IStyleValue<FontDefinition>, IEquatable<StyleFontDefinition>
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x000694DC File Offset: 0x000676DC
		// (set) Token: 0x06001A7A RID: 6778 RVA: 0x00069507 File Offset: 0x00067707
		public FontDefinition value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(FontDefinition);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x00069518 File Offset: 0x00067718
		// (set) Token: 0x06001A7C RID: 6780 RVA: 0x00069530 File Offset: 0x00067730
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

		// Token: 0x06001A7D RID: 6781 RVA: 0x0006953A File Offset: 0x0006773A
		public StyleFontDefinition(FontDefinition f)
		{
			this = new StyleFontDefinition(f, StyleKeyword.Undefined);
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00069546 File Offset: 0x00067746
		public StyleFontDefinition(FontAsset f)
		{
			this = new StyleFontDefinition(f, StyleKeyword.Undefined);
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00069552 File Offset: 0x00067752
		public StyleFontDefinition(Font f)
		{
			this = new StyleFontDefinition(f, StyleKeyword.Undefined);
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00069560 File Offset: 0x00067760
		public StyleFontDefinition(StyleKeyword keyword)
		{
			this = new StyleFontDefinition(default(FontDefinition), keyword);
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x0006957F File Offset: 0x0006777F
		internal StyleFontDefinition(object obj, StyleKeyword keyword)
		{
			this = new StyleFontDefinition(FontDefinition.FromObject(obj), keyword);
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00069590 File Offset: 0x00067790
		internal StyleFontDefinition(object obj)
		{
			this = new StyleFontDefinition(FontDefinition.FromObject(obj), StyleKeyword.Undefined);
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000695A1 File Offset: 0x000677A1
		internal StyleFontDefinition(FontAsset f, StyleKeyword keyword)
		{
			this = new StyleFontDefinition(FontDefinition.FromSDFFont(f), keyword);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000695B2 File Offset: 0x000677B2
		internal StyleFontDefinition(Font f, StyleKeyword keyword)
		{
			this = new StyleFontDefinition(FontDefinition.FromFont(f), keyword);
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x000695C4 File Offset: 0x000677C4
		internal StyleFontDefinition(GCHandle gcHandle, StyleKeyword keyword)
		{
			this = new StyleFontDefinition(gcHandle.IsAllocated ? FontDefinition.FromObject(gcHandle.Target) : default(FontDefinition), keyword);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000695FA File Offset: 0x000677FA
		internal StyleFontDefinition(FontDefinition f, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = f;
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x0006960B File Offset: 0x0006780B
		internal StyleFontDefinition(StyleFontDefinition sfd)
		{
			this.m_Keyword = sfd.keyword;
			this.m_Value = sfd.value;
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x00069628 File Offset: 0x00067828
		public static implicit operator StyleFontDefinition(StyleKeyword keyword)
		{
			return new StyleFontDefinition(keyword);
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x00069640 File Offset: 0x00067840
		public static implicit operator StyleFontDefinition(FontDefinition f)
		{
			return new StyleFontDefinition(f);
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x00069658 File Offset: 0x00067858
		public bool Equals(StyleFontDefinition other)
		{
			return this.m_Keyword == other.m_Keyword && this.m_Value.Equals(other.m_Value);
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0006968C File Offset: 0x0006788C
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleFontDefinition)
			{
				StyleFontDefinition styleFontDefinition = (StyleFontDefinition)obj;
				flag = this.Equals(styleFontDefinition);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000696B8 File Offset: 0x000678B8
		public override int GetHashCode()
		{
			return (int)((this.m_Keyword * (StyleKeyword)397) ^ (StyleKeyword)this.m_Value.GetHashCode());
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x000696EC File Offset: 0x000678EC
		public static bool operator ==(StyleFontDefinition left, StyleFontDefinition right)
		{
			return left.Equals(right);
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x00069708 File Offset: 0x00067908
		public static bool operator !=(StyleFontDefinition left, StyleFontDefinition right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000AEE RID: 2798
		private StyleKeyword m_Keyword;

		// Token: 0x04000AEF RID: 2799
		private FontDefinition m_Value;
	}
}
