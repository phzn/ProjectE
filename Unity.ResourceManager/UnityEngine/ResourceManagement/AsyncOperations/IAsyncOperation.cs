using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x02000057 RID: 87
	internal interface IAsyncOperation
	{
		// Token: 0x06000218 RID: 536
		object GetResultAsObject();

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000219 RID: 537
		Type ResultType { get; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600021A RID: 538
		int Version { get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600021B RID: 539
		string DebugName { get; }

		// Token: 0x0600021C RID: 540
		void DecrementReferenceCount();

		// Token: 0x0600021D RID: 541
		void IncrementReferenceCount();

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600021E RID: 542
		int ReferenceCount { get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600021F RID: 543
		float PercentComplete { get; }

		// Token: 0x06000220 RID: 544
		DownloadStatus GetDownloadStatus(HashSet<object> visited);

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000221 RID: 545
		AsyncOperationStatus Status { get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000222 RID: 546
		Exception OperationException { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000223 RID: 547
		bool IsDone { get; }

		// Token: 0x17000082 RID: 130
		// (set) Token: 0x06000224 RID: 548
		Action<IAsyncOperation> OnDestroy { set; }

		// Token: 0x06000225 RID: 549
		void GetDependencies(List<AsyncOperationHandle> deps);

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000226 RID: 550
		bool IsRunning { get; }

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000227 RID: 551
		// (remove) Token: 0x06000228 RID: 552
		event Action<AsyncOperationHandle> CompletedTypeless;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000229 RID: 553
		// (remove) Token: 0x0600022A RID: 554
		event Action<AsyncOperationHandle> Destroyed;

		// Token: 0x0600022B RID: 555
		void InvokeCompletionEvent();

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600022C RID: 556
		Task<object> Task { get; }

		// Token: 0x0600022D RID: 557
		void Start(ResourceManager rm, AsyncOperationHandle dependency, DelegateList<float> updateCallbacks);

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600022E RID: 558
		AsyncOperationHandle Handle { get; }

		// Token: 0x0600022F RID: 559
		void WaitForCompletion();
	}
}
