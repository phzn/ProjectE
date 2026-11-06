using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200000C RID: 12
	public interface ICameraOverrideStack
	{
		// Token: 0x0600003C RID: 60
		int SetCameraOverride(int overrideId, ICinemachineCamera camA, ICinemachineCamera camB, float weightB, float deltaTime);

		// Token: 0x0600003D RID: 61
		void ReleaseCameraOverride(int overrideId);

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003E RID: 62
		Vector3 DefaultWorldUp { get; }
	}
}
