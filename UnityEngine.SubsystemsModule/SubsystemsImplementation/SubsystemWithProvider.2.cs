using System;

namespace UnityEngine.SubsystemsImplementation
{
	// Token: 0x02000018 RID: 24
	public abstract class SubsystemWithProvider<TSubsystem, TSubsystemDescriptor, TProvider> : SubsystemWithProvider where TSubsystem : SubsystemWithProvider, new() where TSubsystemDescriptor : SubsystemDescriptorWithProvider where TProvider : SubsystemProvider<TSubsystem>
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002DCC File Offset: 0x00000FCC
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public TSubsystemDescriptor subsystemDescriptor { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002DDD File Offset: 0x00000FDD
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00002DE5 File Offset: 0x00000FE5
		protected internal TProvider provider { get; private set; }

		// Token: 0x06000086 RID: 134 RVA: 0x00002DEE File Offset: 0x00000FEE
		protected virtual void OnCreate()
		{
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002DF1 File Offset: 0x00000FF1
		protected override void OnStart()
		{
			this.provider.Start();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002E04 File Offset: 0x00001004
		protected override void OnStop()
		{
			this.provider.Stop();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002E17 File Offset: 0x00001017
		protected override void OnDestroy()
		{
			this.provider.Destroy();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002E2A File Offset: 0x0000102A
		internal sealed override void Initialize(SubsystemDescriptorWithProvider descriptor, SubsystemProvider provider)
		{
			base.providerBase = provider;
			this.provider = (TProvider)((object)provider);
			this.subsystemDescriptor = (TSubsystemDescriptor)((object)descriptor);
			this.OnCreate();
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002E56 File Offset: 0x00001056
		internal sealed override SubsystemDescriptorWithProvider descriptor
		{
			get
			{
				return this.subsystemDescriptor;
			}
		}
	}
}
