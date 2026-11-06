using System;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C48 RID: 3144
	[CreateAssetMenu(menuName = "Stadium/Parachute flight profile")]
	public sealed class StadiumParachuteProfile : ScriptableObject
	{
		// Token: 0x06005AC8 RID: 23240 RVA: 0x00239020 File Offset: 0x00237220
		public Vector2 SpeedForInput(float vertical)
		{
			vertical = Mathf.Clamp(vertical, -1f, 1f);
			if (vertical >= 0f)
			{
				return Vector2.Lerp(new Vector2(this.cruiseSpeed, this.sinkSpeed), new Vector2(this.forwardSpeed, this.forwardSink), vertical);
			}
			return Vector2.Lerp(new Vector2(this.cruiseSpeed, this.sinkSpeed), new Vector2(this.brakeSpeed, this.brakeSink), -vertical);
		}

		// Token: 0x04005BBF RID: 23487
		[Header("Flight • metres / second")]
		[Min(0.1f)]
		[Tooltip("Neutral input. The default is a leisurely glide.")]
		public float cruiseSpeed = 4.4f;

		// Token: 0x04005BC0 RID: 23488
		[Min(0.1f)]
		public float sinkSpeed = 0.85f;

		// Token: 0x04005BC1 RID: 23489
		[Min(0.1f)]
		public float forwardSpeed = 6f;

		// Token: 0x04005BC2 RID: 23490
		[Min(0.1f)]
		public float forwardSink = 1.25f;

		// Token: 0x04005BC3 RID: 23491
		[Min(0.1f)]
		public float brakeSpeed = 1.6f;

		// Token: 0x04005BC4 RID: 23492
		[Min(0.1f)]
		public float brakeSink = 0.55f;

		// Token: 0x04005BC5 RID: 23493
		[Min(0.1f)]
		public float acceleration = 2.4f;

		// Token: 0x04005BC6 RID: 23494
		[Min(0.1f)]
		public float verticalAcceleration = 5f;

		// Token: 0x04005BC7 RID: 23495
		[Header("Steering • degrees / second")]
		[Range(5f, 100f)]
		public float turnSpeed = 36f;

		// Token: 0x04005BC8 RID: 23496
		[Header("Steering • degrees / second")]
		[Range(5f, 100f)]
		public float brakingTurnSpeed = 25f;

		// Token: 0x04005BC9 RID: 23497
		[Min(1f)]
		public float turnAcceleration = 85f;

		// Token: 0x04005BCA RID: 23498
		[Range(0.02f, 1f)]
		public float inputSmoothing = 0.22f;

		// Token: 0x04005BCB RID: 23499
		[Header("Deployment and soft landing")]
		[Min(0.2f)]
		public float openingDuration = 0.72f;

		// Token: 0x04005BCC RID: 23500
		[Min(0.5f)]
		public float minimumClearance = 1.65f;

		// Token: 0x04005BCD RID: 23501
		[Min(0.3f)]
		[Tooltip("Drop beneath the departure floor; excludes ordinary jumps in place.")]
		public float departureDrop = 1.2f;

		// Token: 0x04005BCE RID: 23502
		[Min(0.1f)]
		public float flareHeight = 1.6f;

		// Token: 0x04005BCF RID: 23503
		[Range(0.2f, 1.2f)]
		public float landingDuration = 0.6f;

		// Token: 0x04005BD0 RID: 23504
		[Header("Presentation")]
		public RuntimeAnimatorController animationController;

		// Token: 0x04005BD1 RID: 23505
		[Range(0f, 25f)]
		public float canopyBank = 11f;

		// Token: 0x04005BD2 RID: 23506
		[Range(0f, 15f)]
		public float canopyBrakePitch = 5f;

		// Token: 0x04005BD3 RID: 23507
		[Range(0f, 3f)]
		public float canopySway = 0.65f;

		// Token: 0x04005BD4 RID: 23508
		[Range(0f, 1f)]
		public float openingVolume = 0.85f;

		// Token: 0x04005BD5 RID: 23509
		[Range(0f, 1f)]
		public float windVolume = 0.4f;

		// Token: 0x04005BD6 RID: 23510
		[Header("Third person")]
		[Range(2f, 10f)]
		public float cameraDistance = 5.6f;

		// Token: 0x04005BD7 RID: 23511
		[Range(0.5f, 5f)]
		[Tooltip("Closest third-person flight zoom. Mouse wheel / the usual controller camera modifier.")]
		public float cameraMinDistance = 1.35f;

		// Token: 0x04005BD8 RID: 23512
		[Range(5f, 15f)]
		public float cameraMaxDistance = 10f;

		// Token: 0x04005BD9 RID: 23513
		[Range(1f, 4f)]
		public float cameraHeight = 2f;

		// Token: 0x04005BDA RID: 23514
		[Range(40f, 90f)]
		public float cameraFov = 62f;

		// Token: 0x04005BDB RID: 23515
		[Header("First person • stable horizon, independent look")]
		[Range(10f, 89f)]
		public float lookYawRange = 85f;

		// Token: 0x04005BDC RID: 23516
		public Vector2 lookPitchRange = new Vector2(-65f, 75f);

		// Token: 0x04005BDD RID: 23517
		public Vector3 eyeOffset = new Vector3(0f, 0.015f, 0.035f);
	}
}
