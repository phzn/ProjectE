using System;

namespace UnityEngine.SubsystemsImplementation.Extensions
{
	// Token: 0x02000019 RID: 25
	public static class SubsystemDescriptorExtensions
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00002E6C File Offset: 0x0000106C
		public static SubsystemProxy<TSubsystem, TProvider> CreateProxy<TSubsystem, TProvider>(this SubsystemDescriptorWithProvider<TSubsystem, TProvider> descriptor) where TSubsystem : SubsystemWithProvider, new() where TProvider : SubsystemProvider<TSubsystem>
		{
			TProvider tprovider = descriptor.CreateProvider();
			return (tprovider != null) ? new SubsystemProxy<TSubsystem, TProvider>(tprovider) : null;
		}
	}
}
