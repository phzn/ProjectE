using System;
using Pathfinding.Serialization;

namespace Pathfinding
{
	// Token: 0x020000DC RID: 220
	public interface IGraphInternals
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000753 RID: 1875
		// (set) Token: 0x06000754 RID: 1876
		string SerializedEditorSettings { get; set; }

		// Token: 0x06000755 RID: 1877
		void OnDestroy();

		// Token: 0x06000756 RID: 1878
		void DisposeUnmanagedData();

		// Token: 0x06000757 RID: 1879
		void DestroyAllNodes();

		// Token: 0x06000758 RID: 1880
		IGraphUpdatePromise ScanInternal(bool async);

		// Token: 0x06000759 RID: 1881
		void SerializeExtraInfo(GraphSerializationContext ctx);

		// Token: 0x0600075A RID: 1882
		void DeserializeExtraInfo(GraphSerializationContext ctx);

		// Token: 0x0600075B RID: 1883
		void PostDeserialization(GraphSerializationContext ctx);
	}
}
