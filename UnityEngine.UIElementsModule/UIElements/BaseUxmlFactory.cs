using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003D0 RID: 976
	public abstract class BaseUxmlFactory<TCreatedType, TTraits> where TCreatedType : new() where TTraits : BaseUxmlTraits, new()
	{
		// Token: 0x06002010 RID: 8208 RVA: 0x000795C0 File Offset: 0x000777C0
		protected BaseUxmlFactory()
		{
			this.m_Traits = new TTraits();
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x000795D8 File Offset: 0x000777D8
		public virtual string uxmlName
		{
			get
			{
				return typeof(TCreatedType).Name;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x000795FC File Offset: 0x000777FC
		public virtual string uxmlNamespace
		{
			get
			{
				return typeof(TCreatedType).Namespace ?? string.Empty;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x00079628 File Offset: 0x00077828
		public virtual string uxmlQualifiedName
		{
			get
			{
				return typeof(TCreatedType).FullName;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x00079649 File Offset: 0x00077849
		public virtual Type uxmlType
		{
			get
			{
				return typeof(TCreatedType);
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002015 RID: 8213 RVA: 0x00079658 File Offset: 0x00077858
		public bool canHaveAnyAttribute
		{
			get
			{
				return this.m_Traits.canHaveAnyAttribute;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002016 RID: 8214 RVA: 0x0007967C File Offset: 0x0007787C
		public virtual IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription
		{
			get
			{
				return this.m_Traits.uxmlAttributesDescription;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002017 RID: 8215 RVA: 0x000796A0 File Offset: 0x000778A0
		public virtual IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				return this.m_Traits.uxmlChildElementsDescription;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x000796C4 File Offset: 0x000778C4
		public virtual string substituteForTypeName
		{
			get
			{
				bool flag = typeof(TCreatedType) == typeof(VisualElement);
				string text;
				if (flag)
				{
					text = string.Empty;
				}
				else
				{
					text = typeof(VisualElement).Name;
				}
				return text;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x0007970C File Offset: 0x0007790C
		public virtual string substituteForTypeNamespace
		{
			get
			{
				bool flag = typeof(TCreatedType) == typeof(VisualElement);
				string text;
				if (flag)
				{
					text = string.Empty;
				}
				else
				{
					text = typeof(VisualElement).Namespace ?? string.Empty;
				}
				return text;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x0007975C File Offset: 0x0007795C
		public virtual string substituteForTypeQualifiedName
		{
			get
			{
				bool flag = typeof(TCreatedType) == typeof(VisualElement);
				string text;
				if (flag)
				{
					text = string.Empty;
				}
				else
				{
					text = typeof(VisualElement).FullName;
				}
				return text;
			}
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x000797A4 File Offset: 0x000779A4
		public virtual bool AcceptsAttributeBag(IUxmlAttributes bag, CreationContext cc)
		{
			return true;
		}

		// Token: 0x04000D40 RID: 3392
		internal TTraits m_Traits;
	}
}
