using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003A1 RID: 929
	public class UxmlAttributeOverridesFactory : UxmlFactory<VisualElement, UxmlAttributeOverridesTraits>
	{
		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x000778AF File Offset: 0x00075AAF
		public override string uxmlName
		{
			get
			{
				return "AttributeOverrides";
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001F32 RID: 7986 RVA: 0x000778B6 File Offset: 0x00075AB6
		public override string uxmlQualifiedName
		{
			get
			{
				return this.uxmlNamespace + "." + this.uxmlName;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001F33 RID: 7987 RVA: 0x000775F2 File Offset: 0x000757F2
		public override string substituteForTypeName
		{
			get
			{
				return typeof(VisualElement).Name;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001F34 RID: 7988 RVA: 0x00077603 File Offset: 0x00075803
		public override string substituteForTypeNamespace
		{
			get
			{
				return typeof(VisualElement).Namespace ?? string.Empty;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001F35 RID: 7989 RVA: 0x0007761D File Offset: 0x0007581D
		public override string substituteForTypeQualifiedName
		{
			get
			{
				return typeof(VisualElement).FullName;
			}
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000778D0 File Offset: 0x00075AD0
		public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
		{
			return null;
		}

		// Token: 0x04000CE8 RID: 3304
		internal const string k_ElementName = "AttributeOverrides";
	}
}
