using System;

namespace UnityEngine.SubsystemsImplementation
{
	// Token: 0x02000015 RID: 21
	public abstract class SubsystemProvider<TSubsystem> : SubsystemProvider where TSubsystem : SubsystemWithProvider, new()
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00002CC1 File Offset: 0x00000EC1
		protected internal virtual bool TryInitialize()
		{
			return true;
		}

		// Token: 0x0600006C RID: 108
		public abstract void Start();

		// Token: 0x0600006D RID: 109
		public abstract void Stop();

		// Token: 0x0600006E RID: 110
		public abstract void Destroy();
	}
}
