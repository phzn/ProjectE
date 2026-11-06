using System;

namespace UnityEngine
{
	// Token: 0x0200000E RID: 14
	public class SubsystemDescriptor<TSubsystem> : SubsystemDescriptor where TSubsystem : Subsystem
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000021C7 File Offset: 0x000003C7
		internal override ISubsystem CreateImpl()
		{
			return this.Create();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000021D4 File Offset: 0x000003D4
		public TSubsystem Create()
		{
			TSubsystem tsubsystem = SubsystemManager.FindDeprecatedSubsystemByDescriptor(this) as TSubsystem;
			bool flag = tsubsystem != null;
			TSubsystem tsubsystem2;
			if (flag)
			{
				tsubsystem2 = tsubsystem;
			}
			else
			{
				tsubsystem = Activator.CreateInstance(base.subsystemImplementationType) as TSubsystem;
				tsubsystem.m_SubsystemDescriptor = this;
				SubsystemManager.AddDeprecatedSubsystem(tsubsystem);
				tsubsystem2 = tsubsystem;
			}
			return tsubsystem2;
		}
	}
}
