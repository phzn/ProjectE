using System;

namespace UnityEngine.Bindings
{
	// Token: 0x02000026 RID: 38
	[VisibleToOtherModules]
	[AttributeUsage(AttributeTargets.Class)]
	internal class MarshalUnityObjectAs : Attribute, IBindingsAttribute
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000025EB File Offset: 0x000007EB
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000025F3 File Offset: 0x000007F3
		public Type MarshalAsType { get; set; }

		// Token: 0x06000079 RID: 121 RVA: 0x000025FC File Offset: 0x000007FC
		public MarshalUnityObjectAs(Type marshalAsType)
		{
			this.MarshalAsType = marshalAsType;
		}
	}
}
