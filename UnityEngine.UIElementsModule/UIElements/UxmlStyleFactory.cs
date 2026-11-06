using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200039B RID: 923
	public class UxmlStyleFactory : UxmlFactory<VisualElement, UxmlStyleTraits>
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x000775D3 File Offset: 0x000757D3
		public override string uxmlName
		{
			get
			{
				return "Style";
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x000775DA File Offset: 0x000757DA
		public override string uxmlQualifiedName
		{
			get
			{
				return this.uxmlNamespace + "." + this.uxmlName;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x000775F2 File Offset: 0x000757F2
		public override string substituteForTypeName
		{
			get
			{
				return typeof(VisualElement).Name;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x00077603 File Offset: 0x00075803
		public override string substituteForTypeNamespace
		{
			get
			{
				return typeof(VisualElement).Namespace ?? string.Empty;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x0007761D File Offset: 0x0007581D
		public override string substituteForTypeQualifiedName
		{
			get
			{
				return typeof(VisualElement).FullName;
			}
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x00077630 File Offset: 0x00075830
		public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
		{
			return null;
		}

		// Token: 0x04000CD8 RID: 3288
		internal const string k_ElementName = "Style";
	}
}
