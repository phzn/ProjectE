using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001244 RID: 4676
	public enum CameraTargetMode
	{
		// Token: 0x04009597 RID: 38295
		[Tooltip("Offset from target, camera always looks at LookAt bone. Classic orbital shot.")]
		Orbital,
		// Token: 0x04009598 RID: 38296
		[Tooltip("Camera follows target position with offset but uses explicit rotation. Good for 'over the shoulder' or 'from behind' shots where you don't want auto-aim.")]
		FollowOnly,
		// Token: 0x04009599 RID: 38297
		[Tooltip("Camera placed at fixed world offset from target with explicit rotation. No Follow/LookAt. Good for static cinematic angles that just need initial positioning.")]
		Static,
		// Token: 0x0400959A RID: 38298
		[Tooltip("Camera continuously orbits around the target at a configurable radius, speed, and height. Always looks at LookAt bone.")]
		OrbitAround,
		// Token: 0x0400959B RID: 38299
		[Tooltip("Camera moves along a cyclic path of waypoints (relative to target). Always looks at LookAt bone. Good for showcase/reveal sequences.")]
		Dolly,
		// Token: 0x0400959C RID: 38300
		[Tooltip("Camera oscillates smoothly between two offsets (ping-pong). Always looks at LookAt bone. Good for subtle breathing/sway motion.")]
		PingPong,
		// Token: 0x0400959D RID: 38301
		[Tooltip("Vertical crane shot: camera sweeps from StartOffset to EndOffset over CraneDuration, then holds. Always looks at LookAt bone. Great for dramatic reveals.")]
		Crane
	}
}
