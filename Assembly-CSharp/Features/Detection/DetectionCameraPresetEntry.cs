using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001246 RID: 4678
	[Serializable]
	public class DetectionCameraPresetEntry
	{
		// Token: 0x040095A1 RID: 38305
		[Tooltip("Display label for this preset (editor only)")]
		public string Label = "Default";

		// Token: 0x040095A2 RID: 38306
		[Tooltip("How this camera relates to its target")]
		public CameraTargetMode TargetMode;

		// Token: 0x040095A3 RID: 38307
		[Tooltip("Which transform to use as position/follow reference for this preset.\nDefault = phase default (NPC during NPC phase, Player during Emote phase).\nPlayer = always Ciel. NPC = always the detected NPC.")]
		public PresetReferenceTarget ReferenceTarget;

		// Token: 0x040095A4 RID: 38308
		[Header("Transform (relative to target)")]
		[Tooltip("Position offset from the target transform. Used by: Orbital, FollowOnly, Static, PingPong (as start offset).")]
		public Vector3 PositionOffset = new Vector3(0f, 1.5f, -2f);

		// Token: 0x040095A5 RID: 38309
		[Tooltip("Euler rotation of the camera. For Orbital/OrbitAround/Dolly/PingPong/Crane this is ignored (camera auto-aims at LookAt). For FollowOnly/Static this is applied directly.")]
		public Vector3 Rotation = Vector3.zero;

		// Token: 0x040095A6 RID: 38310
		[Header("Lens")]
		[Tooltip("Field of view for this preset")]
		[Range(10f, 120f)]
		public float FieldOfView = 40f;

		// Token: 0x040095A7 RID: 38311
		[Header("Transition")]
		[Tooltip("Blend duration when switching TO this preset (seconds)")]
		[Range(0.1f, 3f)]
		public float BlendDuration = 0.5f;

		// Token: 0x040095A8 RID: 38312
		[Header("LookAt / Follow")]
		[Tooltip("Name of the bone to use as LookAt target (e.g. 'Head', 'Spine1'). If empty, uses the target's root transform. Used by: Orbital, OrbitAround, Dolly, PingPong, Crane.")]
		public string LookAtBoneName = "Head";

		// Token: 0x040095A9 RID: 38313
		[Tooltip("Optional bone name used as Follow target / position origin (e.g. 'Hips', 'Spine'). If empty, uses the resolved target's root transform. Applies to all modes: offsets and orbit/dolly/etc. are computed from this bone.")]
		public string FollowBoneName = "";

		// Token: 0x040095AA RID: 38314
		[Tooltip("Damping for the Transposer body (lower = snappier). Applies to: FollowOnly (non-Orbital modes).")]
		[Range(0f, 5f)]
		public float FollowDamping = 0.5f;

		// Token: 0x040095AB RID: 38315
		[Header("Body / Transposer (Orbital)")]
		[Tooltip("Transposer BindingMode (stored as int). 0=LockToTargetOnAssign, 1=LockToTargetWithWorldUp, 2=LockToTargetNoRoll, 3=LockToTarget, 4=WorldSpace, 5=SimpleFollowWithWorldUp. Captured automatically from the VCam.")]
		public int BodyBindingMode = 4;

		// Token: 0x040095AC RID: 38316
		[Tooltip("Transposer Follow Offset — the offset in binding-mode space. For Orbital mode this is used instead of PositionOffset.")]
		public Vector3 BodyFollowOffset = new Vector3(0f, 1.5f, -2f);

		// Token: 0x040095AD RID: 38317
		[Tooltip("Transposer X Damping")]
		[Range(0f, 20f)]
		public float BodyXDamping = 0.5f;

		// Token: 0x040095AE RID: 38318
		[Tooltip("Transposer Y Damping")]
		[Range(0f, 20f)]
		public float BodyYDamping = 0.5f;

		// Token: 0x040095AF RID: 38319
		[Tooltip("Transposer Z Damping")]
		[Range(0f, 20f)]
		public float BodyZDamping = 0.5f;

		// Token: 0x040095B0 RID: 38320
		[Tooltip("Transposer Yaw Damping (only relevant for some binding modes)")]
		[Range(0f, 20f)]
		public float BodyYawDamping;

		// Token: 0x040095B1 RID: 38321
		[Tooltip("Transposer Pitch Damping")]
		[Range(0f, 20f)]
		public float BodyPitchDamping;

		// Token: 0x040095B2 RID: 38322
		[Tooltip("Transposer Roll Damping")]
		[Range(0f, 20f)]
		public float BodyRollDamping;

		// Token: 0x040095B3 RID: 38323
		[Header("Aim / Composer")]
		[Tooltip("Offset from the LookAt target's pivot (e.g. shift aim up from Head bone). Used by: Orbital and any mode with a Composer.")]
		public Vector3 AimTrackedObjectOffset = Vector3.zero;

		// Token: 0x040095B4 RID: 38324
		[Tooltip("Horizontal screen position of the LookAt target (0=left, 0.5=center, 1=right)")]
		[Range(0f, 1f)]
		public float AimScreenX = 0.5f;

		// Token: 0x040095B5 RID: 38325
		[Tooltip("Vertical screen position of the LookAt target (0=bottom, 0.5=center, 1=top)")]
		[Range(0f, 1f)]
		public float AimScreenY = 0.5f;

		// Token: 0x040095B6 RID: 38326
		[Tooltip("Width of the dead zone (no re-aim while target stays inside)")]
		[Range(0f, 1f)]
		public float AimDeadZoneWidth;

		// Token: 0x040095B7 RID: 38327
		[Tooltip("Height of the dead zone")]
		[Range(0f, 1f)]
		public float AimDeadZoneHeight;

		// Token: 0x040095B8 RID: 38328
		[Tooltip("Width of the soft zone (camera re-aims softly)")]
		[Range(0f, 1f)]
		public float AimSoftZoneWidth = 0.8f;

		// Token: 0x040095B9 RID: 38329
		[Tooltip("Height of the soft zone")]
		[Range(0f, 1f)]
		public float AimSoftZoneHeight = 0.8f;

		// Token: 0x040095BA RID: 38330
		[Tooltip("Horizontal bias within the soft zone")]
		[Range(-0.5f, 0.5f)]
		public float AimBiasX;

		// Token: 0x040095BB RID: 38331
		[Tooltip("Vertical bias within the soft zone")]
		[Range(-0.5f, 0.5f)]
		public float AimBiasY;

		// Token: 0x040095BC RID: 38332
		[Tooltip("Horizontal damping for the Composer aim (lower = snappier)")]
		[Range(0f, 20f)]
		public float AimHorizontalDamping = 0.5f;

		// Token: 0x040095BD RID: 38333
		[Tooltip("Vertical damping for the Composer aim")]
		[Range(0f, 20f)]
		public float AimVerticalDamping = 0.5f;

		// Token: 0x040095BE RID: 38334
		[Header("OrbitAround Settings")]
		[Tooltip("Distance from the target")]
		[Range(1f, 15f)]
		public float OrbitRadius = 3f;

		// Token: 0x040095BF RID: 38335
		[Tooltip("Rotation speed in degrees per second")]
		[Range(5f, 180f)]
		public float OrbitSpeed = 30f;

		// Token: 0x040095C0 RID: 38336
		[Tooltip("Height offset above the target")]
		[Range(-2f, 5f)]
		public float OrbitHeight = 1.5f;

		// Token: 0x040095C1 RID: 38337
		[Tooltip("If true, orbits clockwise when viewed from above")]
		public bool OrbitClockwise = true;

		// Token: 0x040095C2 RID: 38338
		[Tooltip("Starting angle in degrees (0 = in front of target, 90 = to the right)")]
		[Range(0f, 360f)]
		public float OrbitStartAngle;

		// Token: 0x040095C3 RID: 38339
		[Header("Dolly Path Settings")]
		[Tooltip("Waypoints that define the camera path (relative to target). Camera cycles through them continuously.")]
		public List<DollyWaypoint> DollyWaypoints = new List<DollyWaypoint>();

		// Token: 0x040095C4 RID: 38340
		[Tooltip("If true, the path reverses at the end instead of jumping to start")]
		public bool DollyPingPong;

		// Token: 0x040095C5 RID: 38341
		[Header("PingPong Settings")]
		[Tooltip("Second offset — camera oscillates between PositionOffset and this")]
		public Vector3 PingPongEndOffset = new Vector3(0f, 1.5f, 2f);

		// Token: 0x040095C6 RID: 38342
		[Tooltip("Time for one full cycle (A→B→A) in seconds")]
		[Range(1f, 20f)]
		public float PingPongCycleDuration = 6f;

		// Token: 0x040095C7 RID: 38343
		[Header("Crane Settings")]
		[Tooltip("Where the crane shot ends (PositionOffset is the start)")]
		public Vector3 CraneEndOffset = new Vector3(0f, 4f, -3f);

		// Token: 0x040095C8 RID: 38344
		[Tooltip("Duration of the crane sweep in seconds")]
		[Range(1f, 10f)]
		public float CraneDuration = 3f;

		// Token: 0x040095C9 RID: 38345
		[Tooltip("Easing curve for the crane movement (default: smooth ease-in-out)")]
		public AnimationCurve CraneEasing = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040095CA RID: 38346
		[Header("Collision Avoidance")]
		[Tooltip("If true, camera avoids obstacles between itself and the target. Uses raycasting (similar to Invector/Cinemachine collider approach).")]
		public bool EnableCollisionAvoidance;

		// Token: 0x040095CB RID: 38347
		[Tooltip("Layers that the camera should avoid colliding with")]
		public LayerMask CollisionLayers = -1;

		// Token: 0x040095CC RID: 38348
		[Tooltip("Radius of the spherecast used for collision detection")]
		[Range(0.1f, 1f)]
		public float CollisionRadius = 0.3f;

		// Token: 0x040095CD RID: 38349
		[Tooltip("Minimum allowed distance from the target when pulled forward by collision")]
		[Range(0.2f, 2f)]
		public float MinDistanceFromTarget = 0.5f;

		// Token: 0x040095CE RID: 38350
		[Tooltip("How quickly the camera returns to its ideal position after obstacle is cleared")]
		[Range(0.1f, 2f)]
		public float CollisionDamping = 0.3f;
	}
}
