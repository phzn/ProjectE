using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Rendering
{
	// Token: 0x020000AF RID: 175
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
	public class GenerateHLSL : Attribute
	{
		// Token: 0x06000554 RID: 1364 RVA: 0x0001BC9C File Offset: 0x00019E9C
		public GenerateHLSL(PackingRules rules = PackingRules.Exact, bool needAccessors = true, bool needSetters = false, bool needParamDebug = false, int paramDefinesStart = 1, bool omitStructDeclaration = false, bool containsPackedFields = false, bool generateCBuffer = false, int constantRegister = -1, [CallerFilePath] string sourcePath = null)
		{
			this.sourcePath = sourcePath;
			this.packingRules = rules;
			this.needAccessors = needAccessors;
			this.needSetters = needSetters;
			this.needParamDebug = needParamDebug;
			this.paramDefinesStart = paramDefinesStart;
			this.omitStructDeclaration = omitStructDeclaration;
			this.containsPackedFields = containsPackedFields;
			this.generateCBuffer = generateCBuffer;
			this.constantRegister = constantRegister;
		}

		// Token: 0x040003D1 RID: 977
		public PackingRules packingRules;

		// Token: 0x040003D2 RID: 978
		public bool containsPackedFields;

		// Token: 0x040003D3 RID: 979
		public bool needAccessors;

		// Token: 0x040003D4 RID: 980
		public bool needSetters;

		// Token: 0x040003D5 RID: 981
		public bool needParamDebug;

		// Token: 0x040003D6 RID: 982
		public int paramDefinesStart;

		// Token: 0x040003D7 RID: 983
		public bool omitStructDeclaration;

		// Token: 0x040003D8 RID: 984
		public bool generateCBuffer;

		// Token: 0x040003D9 RID: 985
		public int constantRegister;

		// Token: 0x040003DA RID: 986
		public string sourcePath;
	}
}
