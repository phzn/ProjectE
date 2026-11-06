using System;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x0200003B RID: 59
	public interface IResourceProvider
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000189 RID: 393
		string ProviderId { get; }

		// Token: 0x0600018A RID: 394
		Type GetDefaultType(IResourceLocation location);

		// Token: 0x0600018B RID: 395
		bool CanProvide(Type type, IResourceLocation location);

		// Token: 0x0600018C RID: 396
		void Provide(ProvideHandle provideHandle);

		// Token: 0x0600018D RID: 397
		void Release(IResourceLocation location, object asset);

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600018E RID: 398
		ProviderBehaviourFlags BehaviourFlags { get; }
	}
}
