using System;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010F3 RID: 4339
	public interface IInventoryPlacementAreaProvider
	{
		// Token: 0x0600834F RID: 33615
		bool IsPlacementAreaValid(Vector3 surfacePosition, Vector3 surfaceNormal, bool wall, Transform player);
	}
}
