using System;

namespace Unity.Collections
{
	// Token: 0x0200007F RID: 127
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
	public class GenerateTestsForBurstCompatibilityAttribute : Attribute
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00016BB5 File Offset: 0x00014DB5
		// (set) Token: 0x060006D5 RID: 1749 RVA: 0x00016BBD File Offset: 0x00014DBD
		public Type[] GenericTypeArguments { get; set; }

		// Token: 0x040003A6 RID: 934
		public string RequiredUnityDefine;

		// Token: 0x040003A7 RID: 935
		public GenerateTestsForBurstCompatibilityAttribute.BurstCompatibleCompileTarget CompileTarget;

		// Token: 0x02000080 RID: 128
		public enum BurstCompatibleCompileTarget
		{
			// Token: 0x040003A9 RID: 937
			Player,
			// Token: 0x040003AA RID: 938
			Editor,
			// Token: 0x040003AB RID: 939
			PlayerAndEditor
		}
	}
}
