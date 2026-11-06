using System;

namespace UnityEngine.Bindings
{
	// Token: 0x0200002C RID: 44
	[VisibleToOtherModules]
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	internal class SpanAttribute : Attribute, IBindingsMarshalAsSpan
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00002663 File Offset: 0x00000863
		public bool IsReadOnly { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600008B RID: 139 RVA: 0x0000266B File Offset: 0x0000086B
		public string SizeParameter { get; }

		// Token: 0x0600008C RID: 140 RVA: 0x00002673 File Offset: 0x00000873
		public SpanAttribute(string sizeParameter, bool isReadOnly = false)
		{
			this.SizeParameter = sizeParameter;
			this.IsReadOnly = isReadOnly;
		}
	}
}
