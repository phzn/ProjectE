using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200004C RID: 76
	[DocumentationSorting(DocumentationSortingAttribute.Level.API)]
	public abstract class SignalSourceAsset : ScriptableObject, ISignalSource6D
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600034D RID: 845
		public abstract float SignalDuration { get; }

		// Token: 0x0600034E RID: 846
		public abstract void GetSignal(float timeSinceSignalStart, out Vector3 pos, out Quaternion rot);
	}
}
