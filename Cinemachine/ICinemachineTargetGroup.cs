using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200001D RID: 29
	public interface ICinemachineTargetGroup
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600013F RID: 319
		Transform Transform { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000140 RID: 320
		Bounds BoundingBox { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000141 RID: 321
		BoundingSphere Sphere { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000142 RID: 322
		bool IsEmpty { get; }

		// Token: 0x06000143 RID: 323
		Bounds GetViewSpaceBoundingBox(Matrix4x4 observer);

		// Token: 0x06000144 RID: 324
		void GetViewSpaceAngularBounds(Matrix4x4 observer, out Vector2 minAngles, out Vector2 maxAngles, out Vector2 zRange);
	}
}
