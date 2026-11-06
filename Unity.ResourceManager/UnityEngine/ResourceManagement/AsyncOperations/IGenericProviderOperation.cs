using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x0200005E RID: 94
	internal interface IGenericProviderOperation
	{
		// Token: 0x060002D1 RID: 721
		void Init(ResourceManager rm, IResourceProvider provider, IResourceLocation location, AsyncOperationHandle<IList<AsyncOperationHandle>> depOp);

		// Token: 0x060002D2 RID: 722
		void Init(ResourceManager rm, IResourceProvider provider, IResourceLocation location, AsyncOperationHandle<IList<AsyncOperationHandle>> depOp, bool releaseDependenciesOnFailure);

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002D3 RID: 723
		int ProvideHandleVersion { get; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002D4 RID: 724
		IResourceLocation Location { get; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002D5 RID: 725
		int DependencyCount { get; }

		// Token: 0x060002D6 RID: 726
		void GetDependencies(IList<object> dstList);

		// Token: 0x060002D7 RID: 727
		TDepObject GetDependency<TDepObject>(int index);

		// Token: 0x060002D8 RID: 728
		void SetProgressCallback(Func<float> callback);

		// Token: 0x060002D9 RID: 729
		void ProviderCompleted<T>(T result, bool status, Exception e);

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002DA RID: 730
		Type RequestedType { get; }

		// Token: 0x060002DB RID: 731
		void SetDownloadProgressCallback(Func<DownloadStatus> callback);

		// Token: 0x060002DC RID: 732
		void SetWaitForCompletionCallback(Func<bool> callback);
	}
}
