using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000353 RID: 851
	[Serializable]
	internal struct StyleSelectorPart
	{
		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06001C87 RID: 7303 RVA: 0x0006ED88 File Offset: 0x0006CF88
		// (set) Token: 0x06001C88 RID: 7304 RVA: 0x0006EDA0 File Offset: 0x0006CFA0
		public string value
		{
			get
			{
				return this.m_Value;
			}
			internal set
			{
				this.m_Value = value;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x0006EDAC File Offset: 0x0006CFAC
		// (set) Token: 0x06001C8A RID: 7306 RVA: 0x0006EDC4 File Offset: 0x0006CFC4
		public StyleSelectorType type
		{
			get
			{
				return this.m_Type;
			}
			internal set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x0006EDD0 File Offset: 0x0006CFD0
		public override string ToString()
		{
			return UnityString.Format("[StyleSelectorPart: value={0}, type={1}]", new object[] { this.value, this.type });
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x0006EE0C File Offset: 0x0006D00C
		public static StyleSelectorPart CreateClass(string className)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.Class,
				m_Value = className
			};
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0006EE38 File Offset: 0x0006D038
		public static StyleSelectorPart CreatePseudoClass(string className)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.PseudoClass,
				m_Value = className
			};
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x0006EE64 File Offset: 0x0006D064
		public static StyleSelectorPart CreateId(string Id)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.ID,
				m_Value = Id
			};
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x0006EE90 File Offset: 0x0006D090
		public static StyleSelectorPart CreateType(Type t)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.Type,
				m_Value = t.Name
			};
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x0006EEC0 File Offset: 0x0006D0C0
		public static StyleSelectorPart CreateType(string typeName)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.Type,
				m_Value = typeName
			};
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x0006EEEC File Offset: 0x0006D0EC
		public static StyleSelectorPart CreatePredicate(object predicate)
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.Predicate,
				tempData = predicate
			};
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x0006EF18 File Offset: 0x0006D118
		public static StyleSelectorPart CreateWildCard()
		{
			return new StyleSelectorPart
			{
				m_Type = StyleSelectorType.Wildcard
			};
		}

		// Token: 0x04000BD0 RID: 3024
		[SerializeField]
		private string m_Value;

		// Token: 0x04000BD1 RID: 3025
		[SerializeField]
		private StyleSelectorType m_Type;

		// Token: 0x04000BD2 RID: 3026
		internal object tempData;
	}
}
