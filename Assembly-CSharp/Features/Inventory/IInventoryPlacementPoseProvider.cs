using System;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010EC RID: 4332
	public interface IInventoryPlacementPoseProvider
	{
		// Token: 0x06008345 RID: 33605
		Vector3 AdjustPlacementPosition(Vector3 surfacePosition, Vector3 surfaceNormal, bool wall);

		// Token: 0x06008346 RID: 33606
		Quaternion AdjustPlacementRotation(Quaternion surfaceRotation, bool wall);
	}
}
