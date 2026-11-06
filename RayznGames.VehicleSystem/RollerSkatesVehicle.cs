using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200001A RID: 26
	[RequireComponent(typeof(Rigidbody))]
	public class RollerSkatesVehicle : MonoBehaviour, IVehicle
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00007478 File Offset: 0x00005678
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00007480 File Offset: 0x00005680
		public float horizontalInput { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00007489 File Offset: 0x00005689
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00007491 File Offset: 0x00005691
		public float verticalInput { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000749A File Offset: 0x0000569A
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x000074A2 File Offset: 0x000056A2
		public bool braking { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000074AB File Offset: 0x000056AB
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000074B3 File Offset: 0x000056B3
		public bool isInControl { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000074BC File Offset: 0x000056BC
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x000074C4 File Offset: 0x000056C4
		public float currentSpeed { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000074CD File Offset: 0x000056CD
		// (set) Token: 0x060000EB RID: 235 RVA: 0x000074D5 File Offset: 0x000056D5
		public bool slipping { get; private set; }

		// Token: 0x060000EC RID: 236 RVA: 0x000074DE File Offset: 0x000056DE
		private void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.rb.constraints = (RigidbodyConstraints)80;
			this.InControl(true);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007500 File Offset: 0x00005700
		private void Start()
		{
			this.SetupTrails();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00007508 File Offset: 0x00005708
		private void SetupTrails()
		{
			if (this.leftTrailContact != null)
			{
				this.leftTrail = this.leftTrailContact.GetComponentInChildren<TrailRenderer>();
				this.leftSmoke = this.leftTrailContact.GetComponentInChildren<ParticleSystem>();
			}
			if (this.rightTrailContact != null)
			{
				this.rightTrail = this.rightTrailContact.GetComponentInChildren<TrailRenderer>();
				this.rightSmoke = this.rightTrailContact.GetComponentInChildren<ParticleSystem>();
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007575 File Offset: 0x00005775
		private void FixedUpdate()
		{
			this.CheckGround();
			if (this.isInControl)
			{
				this.HandleMovement();
				this.HandleSteering();
			}
			this.ApplyFriction();
			this.UpdateSpeed();
			this.EmitTrails();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000075A4 File Offset: 0x000057A4
		private void CheckGround()
		{
			RaycastHit raycastHit;
			if (this.groundContact != null)
			{
				this.isGrounded = this.groundContact.GetContact();
			}
			else if (Physics.Raycast(base.transform.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, this.groundCheckDistance + 0.1f, this.groundLayer))
			{
				this.isGrounded = true;
				this.groundNormal = raycastHit.normal;
				this.slopeAngle = Vector3.Angle(this.groundNormal, Vector3.up);
			}
			else
			{
				this.isGrounded = false;
				this.groundNormal = Vector3.up;
				this.slopeAngle = 0f;
			}
			if (!this.isGrounded)
			{
				this.rb.AddForce(Physics.gravity * (this.airGravityMultiplier - 1f), ForceMode.Acceleration);
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000768C File Offset: 0x0000588C
		private void HandleMovement()
		{
			if (!this.isGrounded)
			{
				return;
			}
			Vector3 forward = base.transform.forward;
			if (this.slopeAngle > this.maxSlopeAngle && this.verticalInput > 0f)
			{
				this.verticalInput = 0f;
			}
			if (this.verticalInput > 0.1f && !this.braking)
			{
				Vector3 normalized = Vector3.ProjectOnPlane(forward, this.groundNormal).normalized;
				float num = this.currentSpeed / this.maxSpeed;
				float num2 = 1f - num * 0.5f;
				this.rb.AddForce(normalized * this.verticalInput * this.accelerationForce * num2, ForceMode.Acceleration);
			}
			if (this.braking && this.currentSpeed > 0.1f)
			{
				Vector3 vector = -this.rb.velocity.normalized;
				this.rb.AddForce(vector * this.brakeForce, ForceMode.Acceleration);
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007790 File Offset: 0x00005990
		private void HandleSteering()
		{
			if (!this.isGrounded || this.currentSpeed < 0.5f)
			{
				return;
			}
			Vector3 forward = base.transform.forward;
			float num = Mathf.Clamp01(this.currentSpeed / this.maxSpeed);
			float num2 = Mathf.Lerp(this.maxTurnRate, this.minTurnRateAtSpeed, num);
			this.currentTurnRate = Mathf.Lerp(this.currentTurnRate, this.horizontalInput * num2, this.steeringSmoothness);
			if (Mathf.Abs(this.currentTurnRate) > 0.1f)
			{
				base.transform.Rotate(Vector3.up, this.currentTurnRate * Time.fixedDeltaTime);
				Vector3 velocity = this.rb.velocity;
				Vector3 vector = Vector3.Project(velocity, forward);
				Vector3 vector2 = velocity - vector;
				float num3 = Mathf.Lerp(0.1f, this.driftFactor, Mathf.Abs(this.horizontalInput));
				this.rb.velocity = vector + vector2 * num3;
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00007884 File Offset: 0x00005A84
		private void ApplyFriction()
		{
			if (!this.isGrounded)
			{
				return;
			}
			Vector3 forward = base.transform.forward;
			if (!this.braking && Mathf.Abs(this.verticalInput) < 0.1f)
			{
				this.rb.velocity *= 1f - this.rollingResistance * Time.fixedDeltaTime;
			}
			Vector3 velocity = this.rb.velocity;
			Vector3 vector = Vector3.Project(velocity, forward);
			Vector3 vector2 = velocity - vector;
			if (!this.braking)
			{
				this.rb.velocity = vector + vector2 * this.groundFriction;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007928 File Offset: 0x00005B28
		private void UpdateSpeed()
		{
			Vector3 forward = base.transform.forward;
			this.currentSpeed = Vector3.Dot(this.rb.velocity, forward);
			this.currentSpeed = Mathf.Max(0f, this.currentSpeed);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007970 File Offset: 0x00005B70
		private void EmitTrails()
		{
			bool flag = this.isGrounded && ((this.braking && this.currentSpeed > 2f) || (Mathf.Abs(this.horizontalInput) > 0.5f && this.currentSpeed > 5f));
			this.slipping = flag;
			if (this.leftTrail != null)
			{
				TrailRenderer trailRenderer = this.leftTrail;
				bool flag2;
				if (flag)
				{
					ContactProvider contactProvider = this.leftTrailContact;
					flag2 = contactProvider == null || contactProvider.GetContact();
				}
				else
				{
					flag2 = false;
				}
				trailRenderer.emitting = flag2;
			}
			if (this.rightTrail != null)
			{
				TrailRenderer trailRenderer2 = this.rightTrail;
				bool flag3;
				if (flag)
				{
					ContactProvider contactProvider2 = this.rightTrailContact;
					flag3 = contactProvider2 == null || contactProvider2.GetContact();
				}
				else
				{
					flag3 = false;
				}
				trailRenderer2.emitting = flag3;
			}
			if (flag)
			{
				if (this.leftSmoke != null && !this.leftSmoke.isPlaying)
				{
					this.leftSmoke.Play();
				}
				if (this.rightSmoke != null && !this.rightSmoke.isPlaying)
				{
					this.rightSmoke.Play();
					return;
				}
			}
			else
			{
				if (this.leftSmoke != null && this.leftSmoke.isPlaying)
				{
					this.leftSmoke.Stop();
				}
				if (this.rightSmoke != null && this.rightSmoke.isPlaying)
				{
					this.rightSmoke.Stop();
				}
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007AC8 File Offset: 0x00005CC8
		public void InControl(bool state)
		{
			if (this.isInControl != state)
			{
				this.isInControl = state;
				if (!state)
				{
					this.horizontalInput = 0f;
					this.verticalInput = 0f;
				}
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007AF3 File Offset: 0x00005CF3
		public void ConstrainRotation(bool state)
		{
			if (state)
			{
				this.rb.constraints = (RigidbodyConstraints)80;
				return;
			}
			this.rb.constraints = RigidbodyConstraints.None;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007B12 File Offset: 0x00005D12
		public bool OnGround()
		{
			return this.isGrounded;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00007B1C File Offset: 0x00005D1C
		public float GetVelocityMagnitude()
		{
			return this.rb.velocity.magnitude;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007B3C File Offset: 0x00005D3C
		public bool IsDrifting()
		{
			if (!this.isGrounded || this.currentSpeed < 2f)
			{
				return false;
			}
			Vector3 forward = base.transform.forward;
			Vector3 velocity = this.rb.velocity;
			return (velocity - Vector3.Project(velocity, forward)).magnitude > 1f;
		}

		// Token: 0x040000FA RID: 250
		private Rigidbody rb;

		// Token: 0x040000FB RID: 251
		[Header("Movement")]
		[Tooltip("Maximum speed in m/s")]
		[SerializeField]
		private float maxSpeed = 12f;

		// Token: 0x040000FC RID: 252
		[Tooltip("Acceleration force when pushing")]
		[SerializeField]
		private float accelerationForce = 15f;

		// Token: 0x040000FD RID: 253
		[Tooltip("How quickly the skater decelerates when not pushing (rolling resistance)")]
		[SerializeField]
		private float rollingResistance = 0.5f;

		// Token: 0x040000FE RID: 254
		[Tooltip("Braking force when actively braking")]
		[SerializeField]
		private float brakeForce = 25f;

		// Token: 0x040000FF RID: 255
		[Header("Steering")]
		[Tooltip("Maximum turn rate in degrees per second")]
		[SerializeField]
		private float maxTurnRate = 120f;

		// Token: 0x04000100 RID: 256
		[Tooltip("Turn rate is reduced at high speeds. This is the minimum turn rate at max speed.")]
		[SerializeField]
		private float minTurnRateAtSpeed = 40f;

		// Token: 0x04000101 RID: 257
		[Tooltip("How smoothly steering responds (higher = more responsive)")]
		[Range(0.1f, 1f)]
		[SerializeField]
		private float steeringSmoothness = 0.5f;

		// Token: 0x04000102 RID: 258
		[Header("Physics")]
		[Tooltip("How much the skater drifts sideways when turning at speed")]
		[Range(0f, 1f)]
		[SerializeField]
		private float driftFactor = 0.3f;

		// Token: 0x04000103 RID: 259
		[Tooltip("Friction coefficient when grounded")]
		[SerializeField]
		private float groundFriction = 0.98f;

		// Token: 0x04000104 RID: 260
		[Tooltip("Gravity multiplier when airborne")]
		[SerializeField]
		private float airGravityMultiplier = 2.5f;

		// Token: 0x04000105 RID: 261
		[Tooltip("Maximum slope angle the skater can climb (degrees)")]
		[SerializeField]
		private float maxSlopeAngle = 25f;

		// Token: 0x04000106 RID: 262
		[Header("Ground Detection")]
		[SerializeField]
		private ContactProvider groundContact;

		// Token: 0x04000107 RID: 263
		[Tooltip("Layer mask for ground detection")]
		[SerializeField]
		private LayerMask groundLayer = -1;

		// Token: 0x04000108 RID: 264
		[Tooltip("Distance to check for ground")]
		[SerializeField]
		private float groundCheckDistance = 0.3f;

		// Token: 0x04000109 RID: 265
		[Header("Visual Effects")]
		[Tooltip("Trail renderer for left skate")]
		[SerializeField]
		private ContactProvider leftTrailContact;

		// Token: 0x0400010A RID: 266
		[Tooltip("Trail renderer for right skate")]
		[SerializeField]
		private ContactProvider rightTrailContact;

		// Token: 0x0400010B RID: 267
		private TrailRenderer leftTrail;

		// Token: 0x0400010C RID: 268
		private TrailRenderer rightTrail;

		// Token: 0x0400010D RID: 269
		private ParticleSystem leftSmoke;

		// Token: 0x0400010E RID: 270
		private ParticleSystem rightSmoke;

		// Token: 0x0400010F RID: 271
		private float currentTurnRate;

		// Token: 0x04000110 RID: 272
		private Vector3 moveDirection;

		// Token: 0x04000111 RID: 273
		private bool isGrounded;

		// Token: 0x04000112 RID: 274
		private Vector3 groundNormal = Vector3.up;

		// Token: 0x04000113 RID: 275
		private float slopeAngle;
	}
}
