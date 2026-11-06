using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000B1 RID: 177
	[AttributeUsage(AttributeTargets.Field)]
	public class HLSLArray : Attribute
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x0001BD7F File Offset: 0x00019F7F
		public HLSLArray(int arraySize, Type elementType)
		{
			this.arraySize = arraySize;
			this.elementType = elementType;
		}

		// Token: 0x040003E1 RID: 993
		public int arraySize;

		// Token: 0x040003E2 RID: 994
		public Type elementType;
	}
}
