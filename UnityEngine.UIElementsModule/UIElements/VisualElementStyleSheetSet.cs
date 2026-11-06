using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200040C RID: 1036
	public struct VisualElementStyleSheetSet : IEquatable<VisualElementStyleSheetSet>
	{
		// Token: 0x0600211B RID: 8475 RVA: 0x0007D43C File Offset: 0x0007B63C
		internal VisualElementStyleSheetSet(VisualElement element)
		{
			this.m_Element = element;
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x0007D448 File Offset: 0x0007B648
		public void Add(StyleSheet styleSheet)
		{
			bool flag = styleSheet == null;
			if (flag)
			{
				throw new ArgumentNullException("styleSheet");
			}
			bool flag2 = this.m_Element.styleSheetList == null;
			if (flag2)
			{
				this.m_Element.styleSheetList = new List<StyleSheet>();
			}
			else
			{
				bool flag3 = this.m_Element.styleSheetList.Contains(styleSheet);
				if (flag3)
				{
					return;
				}
			}
			this.m_Element.styleSheetList.Add(styleSheet);
			this.m_Element.IncrementVersion(VersionChangeType.StyleSheet);
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x0007D4CC File Offset: 0x0007B6CC
		public void Clear()
		{
			bool flag = this.m_Element.styleSheetList == null;
			if (!flag)
			{
				this.m_Element.styleSheetList = null;
				this.m_Element.IncrementVersion(VersionChangeType.StyleSheet);
			}
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x0007D508 File Offset: 0x0007B708
		public bool Remove(StyleSheet styleSheet)
		{
			bool flag = styleSheet == null;
			if (flag)
			{
				throw new ArgumentNullException("styleSheet");
			}
			bool flag2 = this.m_Element.styleSheetList != null && this.m_Element.styleSheetList.Remove(styleSheet);
			bool flag4;
			if (flag2)
			{
				bool flag3 = this.m_Element.styleSheetList.Count == 0;
				if (flag3)
				{
					this.m_Element.styleSheetList = null;
				}
				this.m_Element.IncrementVersion(VersionChangeType.StyleSheet);
				flag4 = true;
			}
			else
			{
				flag4 = false;
			}
			return flag4;
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x0007D590 File Offset: 0x0007B790
		internal void Swap(StyleSheet old, StyleSheet @new)
		{
			bool flag = old == null;
			if (flag)
			{
				throw new ArgumentNullException("old");
			}
			bool flag2 = @new == null;
			if (flag2)
			{
				throw new ArgumentNullException("new");
			}
			bool flag3 = this.m_Element.styleSheetList == null;
			if (!flag3)
			{
				int num = this.m_Element.styleSheetList.IndexOf(old);
				bool flag4 = num >= 0;
				if (flag4)
				{
					this.m_Element.IncrementVersion(VersionChangeType.StyleSheet);
					this.m_Element.styleSheetList[num] = @new;
				}
			}
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x0007D624 File Offset: 0x0007B824
		public bool Contains(StyleSheet styleSheet)
		{
			bool flag = styleSheet == null;
			if (flag)
			{
				throw new ArgumentNullException("styleSheet");
			}
			bool flag2 = this.m_Element.styleSheetList != null;
			return flag2 && this.m_Element.styleSheetList.Contains(styleSheet);
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002121 RID: 8481 RVA: 0x0007D674 File Offset: 0x0007B874
		public int count
		{
			get
			{
				bool flag = this.m_Element.styleSheetList == null;
				int num;
				if (flag)
				{
					num = 0;
				}
				else
				{
					num = this.m_Element.styleSheetList.Count;
				}
				return num;
			}
		}

		// Token: 0x170007B1 RID: 1969
		public StyleSheet this[int index]
		{
			get
			{
				bool flag = this.m_Element.styleSheetList == null;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.m_Element.styleSheetList[index];
			}
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x0007D6EC File Offset: 0x0007B8EC
		public bool Equals(VisualElementStyleSheetSet other)
		{
			return object.Equals(this.m_Element, other.m_Element);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x0007D710 File Offset: 0x0007B910
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is VisualElementStyleSheetSet && this.Equals((VisualElementStyleSheetSet)obj);
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x0007D748 File Offset: 0x0007B948
		public override int GetHashCode()
		{
			return (this.m_Element != null) ? this.m_Element.GetHashCode() : 0;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x0007D770 File Offset: 0x0007B970
		public static bool operator ==(VisualElementStyleSheetSet left, VisualElementStyleSheetSet right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x0007D78C File Offset: 0x0007B98C
		public static bool operator !=(VisualElementStyleSheetSet left, VisualElementStyleSheetSet right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E00 RID: 3584
		private readonly VisualElement m_Element;
	}
}
