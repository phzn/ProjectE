using System;

namespace Unity.Entities
{
	// Token: 0x02000195 RID: 405
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class BurstMonoInteropMethodAttribute : Attribute
	{
		// Token: 0x06000A0F RID: 2575 RVA: 0x00032159 File Offset: 0x00030359
		public BurstMonoInteropMethodAttribute(bool makePublic = false)
		{
			this.MakePublic = makePublic;
		}

		// Token: 0x040004C1 RID: 1217
		public bool MakePublic;
	}
}
