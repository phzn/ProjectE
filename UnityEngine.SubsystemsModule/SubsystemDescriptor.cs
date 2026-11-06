using System;

namespace UnityEngine
{
	// Token: 0x0200000D RID: 13
	public abstract class SubsystemDescriptor : ISubsystemDescriptor
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000219D File Offset: 0x0000039D
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000021A5 File Offset: 0x000003A5
		public string id { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000021AE File Offset: 0x000003AE
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000021B6 File Offset: 0x000003B6
		public Type subsystemImplementationType { get; set; }

		// Token: 0x0600002C RID: 44 RVA: 0x000021BF File Offset: 0x000003BF
		ISubsystem ISubsystemDescriptor.Create()
		{
			return this.CreateImpl();
		}

		// Token: 0x0600002D RID: 45
		internal abstract ISubsystem CreateImpl();
	}
}
