using System;

namespace UnityEngine.SubsystemsImplementation
{
	// Token: 0x02000017 RID: 23
	public abstract class SubsystemWithProvider : ISubsystem
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00002D14 File Offset: 0x00000F14
		public void Start()
		{
			bool running = this.running;
			if (!running)
			{
				this.OnStart();
				this.providerBase.m_Running = true;
				this.running = true;
			}
		}

		// Token: 0x06000076 RID: 118
		protected abstract void OnStart();

		// Token: 0x06000077 RID: 119 RVA: 0x00002D4C File Offset: 0x00000F4C
		public void Stop()
		{
			bool flag = !this.running;
			if (!flag)
			{
				this.OnStop();
				this.providerBase.m_Running = false;
				this.running = false;
			}
		}

		// Token: 0x06000078 RID: 120
		protected abstract void OnStop();

		// Token: 0x06000079 RID: 121 RVA: 0x00002D84 File Offset: 0x00000F84
		public void Destroy()
		{
			this.Stop();
			bool flag = SubsystemManager.RemoveStandaloneSubsystem(this);
			if (flag)
			{
				this.OnDestroy();
			}
		}

		// Token: 0x0600007A RID: 122
		protected abstract void OnDestroy();

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002DAA File Offset: 0x00000FAA
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00002DB2 File Offset: 0x00000FB2
		public bool running { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002DBB File Offset: 0x00000FBB
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00002DC3 File Offset: 0x00000FC3
		internal SubsystemProvider providerBase { get; set; }

		// Token: 0x0600007F RID: 127
		internal abstract void Initialize(SubsystemDescriptorWithProvider descriptor, SubsystemProvider subsystemProvider);

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000080 RID: 128
		internal abstract SubsystemDescriptorWithProvider descriptor { get; }
	}
}
