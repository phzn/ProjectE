using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200077D RID: 1917
	[RequireComponent(typeof(Rigidbody))]
	public class UnicycleVehicle : MonoBehaviour, IVehicle
	{
		// Token: 0x06003236 RID: 12854 RVA: 0x0011CAE7 File Offset: 0x0011ACE7
		public float GetCurrentSpeed()
		{
			return this.currentSpeed;
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06003237 RID: 12855 RVA: 0x0011CAEF File Offset: 0x0011ACEF
		// (set) Token: 0x06003238 RID: 12856 RVA: 0x0011CAF7 File Offset: 0x0011ACF7
		public float horizontalInput { get; set; }

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06003239 RID: 12857 RVA: 0x0011CB00 File Offset: 0x0011AD00
		// (set) Token: 0x0600323A RID: 12858 RVA: 0x0011CB08 File Offset: 0x0011AD08
		public float verticalInput { get; set; }

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x0011CB11 File Offset: 0x0011AD11
		// (set) Token: 0x0600323C RID: 12860 RVA: 0x0011CB19 File Offset: 0x0011AD19
		public bool braking { get; set; }

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x0600323D RID: 12861 RVA: 0x0011CB22 File Offset: 0x0011AD22
		// (set) Token: 0x0600323E RID: 12862 RVA: 0x0011CB2A File Offset: 0x0011AD2A
		public bool isInControl { get; private set; }

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x0011CB33 File Offset: 0x0011AD33
		// (set) Token: 0x06003240 RID: 12864 RVA: 0x0011CB3B File Offset: 0x0011AD3B
		public bool slip { get; private set; }

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x0011CB44 File Offset: 0x0011AD44
		// (set) Token: 0x06003242 RID: 12866 RVA: 0x0011CB4C File Offset: 0x0011AD4C
		public float currentSpeed { get; private set; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x0011CB55 File Offset: 0x0011AD55
		// (set) Token: 0x06003244 RID: 12868 RVA: 0x0011CB5D File Offset: 0x0011AD5D
		public float currentSteeringAngle { get; private set; }

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06003245 RID: 12869 RVA: 0x0011CB66 File Offset: 0x0011AD66
		// (set) Token: 0x06003246 RID: 12870 RVA: 0x0011CB6E File Offset: 0x0011AD6E
		public float current_maxSteeringAngle { get; private set; }

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06003247 RID: 12871 RVA: 0x0011CB77 File Offset: 0x0011AD77
		// (set) Token: 0x06003248 RID: 12872 RVA: 0x0011CB7F File Offset: 0x0011AD7F
		public float currentLeanAngle { get; private set; }

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06003249 RID: 12873 RVA: 0x0011CB88 File Offset: 0x0011AD88
		// (set) Token: 0x0600324A RID: 12874 RVA: 0x0011CB90 File Offset: 0x0011AD90
		public Vector3 COG
		{
			get
			{
				return this.centerOfMassOffset;
			}
			set
			{
				this.centerOfMassOffset = value;
			}
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x0011CB99 File Offset: 0x0011AD99
		private void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.rb.centerOfMass = this.centerOfMassOffset;
			this.InControl(true);
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x0011CBC0 File Offset: 0x0011ADC0
		private void Start()
		{
			if (this.wheel != null)
			{
				this.wheel.ConfigureVehicleSubsteps(5f, 12, 15);
			}
			if (this.wheelTransform != null)
			{
				this.wheelRestRotation = this.wheelTransform.localRotation;
			}
			if (this.leanPivot != null)
			{
				this.leanPivotRestRotation = this.leanPivot.localRotation;
			}
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x0011CC30 File Offset: 0x0011AE30
		private void FixedUpdate()
		{
			if (this.isInControl)
			{
				this.HandleEngine();
				this.HandleSteering();
				this.ClampSpeed();
			}
			this.UpdateVisualLean();
			this.UpdateWheel();
			this.currentSpeed = ((this.rb != null && !this.rb.isKinematic) ? this.rb.velocity.magnitude : 0f);
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x0011CC9E File Offset: 0x0011AE9E
		public void InControl(bool state)
		{
			if (this.isInControl == state)
			{
				return;
			}
			this.isInControl = state;
			if (state && this.rb != null)
			{
				this.ConstrainRotation(true);
			}
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x0011CCC9 File Offset: 0x0011AEC9
		public void ConstrainRotation(bool state)
		{
			if (this.rb == null)
			{
				return;
			}
			this.rb.constraints = (RigidbodyConstraints)80;
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x0011CCE7 File Offset: 0x0011AEE7
		public bool OnGround()
		{
			if (!(this.wheel != null))
			{
				return !(this.controlContact != null) || this.controlContact.GetContact();
			}
			return this.wheel.isGrounded;
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x0011CD20 File Offset: 0x0011AF20
		private void HandleEngine()
		{
			if (this.wheel == null)
			{
				return;
			}
			this.wheel.motorTorque = (this.braking ? 0f : (this.verticalInput * this.motorForce * this.motorForceScale));
			this.wheel.brakeTorque = (this.braking ? this.brakeForce : 0f);
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x0011CD8C File Offset: 0x0011AF8C
		private void ClampSpeed()
		{
			if (this.rb == null || this.rb.isKinematic)
			{
				return;
			}
			Vector3 velocity = this.rb.velocity;
			Vector3 vector = new Vector3(velocity.x, 0f, velocity.z);
			if (vector.sqrMagnitude < 0.01f)
			{
				return;
			}
			float num = ((Vector3.Dot(vector.normalized, base.transform.forward) >= 0f) ? this.maxSpeed : this.maxReverseSpeed);
			if (num <= 0f)
			{
				return;
			}
			if (vector.magnitude > num)
			{
				vector = vector.normalized * num;
				this.rb.velocity = new Vector3(vector.x, velocity.y, vector.z);
			}
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x0011CE60 File Offset: 0x0011B060
		private void HandleSteering()
		{
			if (this.rb == null || this.rb.isKinematic)
			{
				return;
			}
			float num = Mathf.Clamp01(this.rb.velocity.magnitude / Mathf.Max(0.01f, this.turnReductionSpeed));
			this.current_maxSteeringAngle = Mathf.Lerp(this.maxTurnRate, this.minTurnRate, num);
			Vector3 vector = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
			float num2 = ((vector.sqrMagnitude > 0.1f && Vector3.Dot(vector.normalized, base.transform.forward) < 0f) ? (-1f) : 1f);
			float num3 = (this.invertSteering ? (-this.horizontalInput) : this.horizontalInput) * this.current_maxSteeringAngle * num2;
			this.currentSteeringAngle = Mathf.Lerp(this.currentSteeringAngle, num3, this.steerResponsiveness * Time.fixedDeltaTime);
			Vector3 angularVelocity = this.rb.angularVelocity;
			angularVelocity.y = this.currentSteeringAngle * 0.017453292f;
			this.rb.angularVelocity = angularVelocity;
			if (this.wheel != null)
			{
				this.wheel.steerAngle = Mathf.Clamp(this.currentSteeringAngle * 0.3f, -30f, 30f);
			}
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x0011CFD8 File Offset: 0x0011B1D8
		private void UpdateVisualLean()
		{
			if (this.leanPivot == null)
			{
				return;
			}
			float num = (this.invertSteering ? (-this.horizontalInput) : this.horizontalInput);
			float num2 = Mathf.Clamp01(this.rb.velocity.magnitude / Mathf.Max(0.01f, this.turnReductionSpeed));
			float num3 = (this.invertLean ? 1f : (-1f));
			float num4 = num * this.maxLeanAngle * num2 * num3;
			this.currentLeanAngle = Mathf.Lerp(this.currentLeanAngle, num4, this.leanResponsiveness * Time.fixedDeltaTime);
			this.leanPivot.localRotation = this.leanPivotRestRotation * Quaternion.Euler(0f, 0f, this.currentLeanAngle);
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x0011D0A0 File Offset: 0x0011B2A0
		private void UpdateWheel()
		{
			if (this.wheel == null || this.wheelTransform == null)
			{
				return;
			}
			float num = this.wheel.rpm * 6f;
			this.wheelSpin = (this.wheelSpin + num * Time.fixedDeltaTime) % 360f;
			Vector3 vector;
			Quaternion quaternion;
			this.wheel.GetWorldPose(out vector, out quaternion);
			this.wheelTransform.position = vector;
			this.wheelTransform.localRotation = this.wheelRestRotation * Quaternion.Euler(0f, 0f, this.wheelSpin);
		}

		// Token: 0x040032FD RID: 13053
		private Rigidbody rb;

		// Token: 0x040032FE RID: 13054
		[Header("Power / Braking")]
		[Tooltip("Motor torque applied to the wheel when verticalInput != 0")]
		[SerializeField]
		public float motorForce = 80f;

		// Token: 0x040032FF RID: 13055
		[Tooltip("Brake torque applied when braking")]
		[SerializeField]
		private float brakeForce = 150f;

		// Token: 0x04003300 RID: 13056
		[Tooltip("Top speed cap forward in m/s (0 = uncapped)")]
		[SerializeField]
		private float maxSpeed = 4f;

		// Token: 0x04003301 RID: 13057
		[Tooltip("Top speed cap in reverse in m/s")]
		[SerializeField]
		private float maxReverseSpeed = 2f;

		// Token: 0x04003302 RID: 13058
		[Tooltip("Multiplier on motorForce — reduce to go slower without changing motorForce")]
		[Range(0.01f, 1f)]
		[SerializeField]
		private float motorForceScale = 0.3f;

		// Token: 0x04003303 RID: 13059
		[Header("Steering")]
		[Tooltip("Maximum yaw rotation speed in deg/s")]
		[SerializeField]
		private float maxTurnRate = 90f;

		// Token: 0x04003304 RID: 13060
		[Tooltip("Turn rate at max speed (allows sharper turns at low speed)")]
		[SerializeField]
		private float minTurnRate = 30f;

		// Token: 0x04003305 RID: 13061
		[Tooltip("Speed at which turn rate reaches its minimum")]
		[SerializeField]
		private float turnReductionSpeed = 8f;

		// Token: 0x04003306 RID: 13062
		[Tooltip("How quickly steering reaches the target (higher = snappier)")]
		[Range(1f, 20f)]
		[SerializeField]
		private float steerResponsiveness = 8f;

		// Token: 0x04003307 RID: 13063
		[Tooltip("Flip this if steering feels inverted")]
		[SerializeField]
		private bool invertSteering;

		// Token: 0x04003308 RID: 13064
		[Header("Visual Lean (optional)")]
		[Tooltip("Optional child transform to rotate for visual lean. Leave empty to disable lean.")]
		[SerializeField]
		private Transform leanPivot;

		// Token: 0x04003309 RID: 13065
		[Tooltip("Max visual lean angle in degrees")]
		[SerializeField]
		private float maxLeanAngle = 25f;

		// Token: 0x0400330A RID: 13066
		[Tooltip("How quickly the lean reaches the target (higher = snappier)")]
		[Range(1f, 20f)]
		[SerializeField]
		private float leanResponsiveness = 6f;

		// Token: 0x0400330B RID: 13067
		[Tooltip("Flip this if the lean appears reversed")]
		[SerializeField]
		private bool invertLean;

		// Token: 0x0400330C RID: 13068
		[Header("References")]
		[SerializeField]
		private WheelCollider wheel;

		// Token: 0x0400330D RID: 13069
		[SerializeField]
		private Transform wheelTransform;

		// Token: 0x0400330E RID: 13070
		[Tooltip("Adjust local center of gravity (lower = more stable)")]
		[SerializeField]
		private Vector3 centerOfMassOffset = new Vector3(0f, -0.2f, 0f);

		// Token: 0x0400330F RID: 13071
		public ContactProvider controlContact;

		// Token: 0x04003314 RID: 13076
		private float wheelSpin;

		// Token: 0x04003315 RID: 13077
		private Quaternion wheelRestRotation = Quaternion.identity;

		// Token: 0x04003316 RID: 13078
		private Quaternion leanPivotRestRotation = Quaternion.identity;
	}
}
