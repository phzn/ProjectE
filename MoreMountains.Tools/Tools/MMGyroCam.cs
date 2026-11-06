using System;
using Cinemachine;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000024 RID: 36
	[AddComponentMenu("More Mountains/Tools/Cinemachine/MMGyroCam")]
	[Serializable]
	public class MMGyroCam
	{
		// Token: 0x040000E9 RID: 233
		public CinemachineVirtualCamera Cam;

		// Token: 0x040000EA RID: 234
		public Transform LookAt;

		// Token: 0x040000EB RID: 235
		public Transform RotationCenter;

		// Token: 0x040000EC RID: 236
		public Vector2 MinRotation = new Vector2(-2f, -2f);

		// Token: 0x040000ED RID: 237
		public Vector2 MaxRotation = new Vector2(2f, 2f);

		// Token: 0x040000EE RID: 238
		public Transform AnimatedPosition;

		// Token: 0x040000EF RID: 239
		[MMReadOnly]
		public Vector3 InitialAngles;

		// Token: 0x040000F0 RID: 240
		[MMReadOnly]
		public Vector3 InitialPosition;
	}
}
