using System;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010F2 RID: 4338
	public interface IInventorySurfaceConformProvider
	{
		// Token: 0x0600834E RID: 33614
		void ConformWorldInstanceToSurface(GameObject instance, Vector3 surfacePosition, Vector3 surfaceNormal, bool wall);
	}
}
