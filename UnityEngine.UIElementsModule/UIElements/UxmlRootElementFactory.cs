using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000398 RID: 920
	public class UxmlRootElementFactory : UxmlFactory<VisualElement, UxmlRootElementTraits>
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x00077476 File Offset: 0x00075676
		public override string uxmlName
		{
			get
			{
				return "UXML";
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001EFF RID: 7935 RVA: 0x0007747D File Offset: 0x0007567D
		public override string uxmlQualifiedName
		{
			get
			{
				return this.uxmlNamespace + "." + this.uxmlName;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x00077495 File Offset: 0x00075695
		public override string substituteForTypeName
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x00077495 File Offset: 0x00075695
		public override string substituteForTypeNamespace
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x00077495 File Offset: 0x00075695
		public override string substituteForTypeQualifiedName
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0007749C File Offset: 0x0007569C
		public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
		{
			return null;
		}

		// Token: 0x04000CD1 RID: 3281
		internal const string k_ElementName = "UXML";
	}
}
