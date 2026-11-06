using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200039E RID: 926
	public class UxmlTemplateFactory : UxmlFactory<VisualElement, UxmlTemplateTraits>
	{
		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001F20 RID: 7968 RVA: 0x0007775B File Offset: 0x0007595B
		public override string uxmlName
		{
			get
			{
				return "Template";
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001F21 RID: 7969 RVA: 0x00077762 File Offset: 0x00075962
		public override string uxmlQualifiedName
		{
			get
			{
				return this.uxmlNamespace + "." + this.uxmlName;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001F22 RID: 7970 RVA: 0x000775F2 File Offset: 0x000757F2
		public override string substituteForTypeName
		{
			get
			{
				return typeof(VisualElement).Name;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x00077603 File Offset: 0x00075803
		public override string substituteForTypeNamespace
		{
			get
			{
				return typeof(VisualElement).Namespace ?? string.Empty;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001F24 RID: 7972 RVA: 0x0007761D File Offset: 0x0007581D
		public override string substituteForTypeQualifiedName
		{
			get
			{
				return typeof(VisualElement).FullName;
			}
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x0007777C File Offset: 0x0007597C
		public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
		{
			return null;
		}

		// Token: 0x04000CE0 RID: 3296
		internal const string k_ElementName = "Template";
	}
}
