using System;

namespace UnityEngine.SubsystemsImplementation
{
	// Token: 0x02000013 RID: 19
	public class SubsystemDescriptorWithProvider<TSubsystem, TProvider> : SubsystemDescriptorWithProvider where TSubsystem : SubsystemWithProvider, new() where TProvider : SubsystemProvider<TSubsystem>
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00002AFE File Offset: 0x00000CFE
		internal override ISubsystem CreateImpl()
		{
			return this.Create();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002B0C File Offset: 0x00000D0C
		public TSubsystem Create()
		{
			TSubsystem tsubsystem = SubsystemManager.FindStandaloneSubsystemByDescriptor(this) as TSubsystem;
			bool flag = tsubsystem != null;
			TSubsystem tsubsystem2;
			if (flag)
			{
				tsubsystem2 = tsubsystem;
			}
			else
			{
				TProvider tprovider = this.CreateProvider();
				bool flag2 = tprovider == null;
				if (flag2)
				{
					tsubsystem2 = default(TSubsystem);
				}
				else
				{
					tsubsystem = ((base.subsystemTypeOverride != null) ? ((TSubsystem)((object)Activator.CreateInstance(base.subsystemTypeOverride))) : new TSubsystem());
					tsubsystem.Initialize(this, tprovider);
					SubsystemManager.AddStandaloneSubsystem(tsubsystem);
					tsubsystem2 = tsubsystem;
				}
			}
			return tsubsystem2;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002BAC File Offset: 0x00000DAC
		internal sealed override void ThrowIfInvalid()
		{
			bool flag = base.providerType == null;
			if (flag)
			{
				throw new InvalidOperationException("Invalid descriptor - must supply a valid providerType field!");
			}
			bool flag2 = !base.providerType.IsSubclassOf(typeof(TProvider));
			if (flag2)
			{
				throw new InvalidOperationException(string.Format("Can't create provider - providerType '{0}' is not a subclass of '{1}'!", base.providerType.ToString(), typeof(TProvider).ToString()));
			}
			bool flag3 = base.subsystemTypeOverride != null && !base.subsystemTypeOverride.IsSubclassOf(typeof(TSubsystem));
			if (flag3)
			{
				throw new InvalidOperationException(string.Format("Can't create provider - subsystemTypeOverride '{0}' is not a subclass of '{1}'!", base.subsystemTypeOverride.ToString(), typeof(TSubsystem).ToString()));
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002C74 File Offset: 0x00000E74
		internal TProvider CreateProvider()
		{
			TProvider tprovider = (TProvider)((object)Activator.CreateInstance(base.providerType));
			return tprovider.TryInitialize() ? tprovider : default(TProvider);
		}
	}
}
