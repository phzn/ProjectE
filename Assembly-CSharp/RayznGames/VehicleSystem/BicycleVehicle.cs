using System;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200076E RID: 1902
	[RequireComponent(typeof(Rigidbody))]
	public class BicycleVehicle : MonoBehaviour, IVehicle
	{
		// Token: 0x06003158 RID: 12632 RVA: 0x00116003 File Offset: 0x00114203
		public float GetCurrentSpeed()
		{
			return this.currentSpeed;
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003159 RID: 12633 RVA: 0x0011600B File Offset: 0x0011420B
		// (set) Token: 0x0600315A RID: 12634 RVA: 0x00116013 File Offset: 0x00114213
		public float horizontalInput { get; set; }

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x0600315B RID: 12635 RVA: 0x0011601C File Offset: 0x0011421C
		// (set) Token: 0x0600315C RID: 12636 RVA: 0x00116024 File Offset: 0x00114224
		public float verticalInput { get; set; }

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x0600315D RID: 12637 RVA: 0x0011602D File Offset: 0x0011422D
		// (set) Token: 0x0600315E RID: 12638 RVA: 0x00116035 File Offset: 0x00114235
		public bool braking { get; set; }

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x0600315F RID: 12639 RVA: 0x0011603E File Offset: 0x0011423E
		// (set) Token: 0x06003160 RID: 12640 RVA: 0x00116046 File Offset: 0x00114246
		public bool isInControl { get; private set; }

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003161 RID: 12641 RVA: 0x0011604F File Offset: 0x0011424F
		// (set) Token: 0x06003162 RID: 12642 RVA: 0x00116057 File Offset: 0x00114257
		public bool slipFront { get; private set; }

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003163 RID: 12643 RVA: 0x00116060 File Offset: 0x00114260
		// (set) Token: 0x06003164 RID: 12644 RVA: 0x00116068 File Offset: 0x00114268
		public bool slipRear { get; private set; }

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x00116071 File Offset: 0x00114271
		// (set) Token: 0x06003166 RID: 12646 RVA: 0x00116079 File Offset: 0x00114279
		public float currentSteeringAngle { get; private set; }

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06003167 RID: 12647 RVA: 0x00116082 File Offset: 0x00114282
		// (set) Token: 0x06003168 RID: 12648 RVA: 0x0011608A File Offset: 0x0011428A
		public float current_maxSteeringAngle { get; private set; }

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06003169 RID: 12649 RVA: 0x00116093 File Offset: 0x00114293
		// (set) Token: 0x0600316A RID: 12650 RVA: 0x0011609B File Offset: 0x0011429B
		public float currentLeanAngle { get; private set; }

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x001160A4 File Offset: 0x001142A4
		// (set) Token: 0x0600316C RID: 12652 RVA: 0x001160AC File Offset: 0x001142AC
		public float currentSpeed { get; private set; }

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x001160B5 File Offset: 0x001142B5
		public WheelCollider FrontWheel
		{
			get
			{
				return this.frontWheel;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x001160BD File Offset: 0x001142BD
		public WheelCollider RearWheel
		{
			get
			{
				return this.rearWheel;
			}
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x001160C8 File Offset: 0x001142C8
		private void Awake()
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.DisableVisualWheelCollisionProxies();
			bool flag = base.GetComponent<vVehicleController>() != null || base.GetComponent<vBikeController>() != null;
			this.InControl(!flag);
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x0011610F File Offset: 0x0011430F
		private void OnEnable()
		{
			this.wheelPhysicsConfigurationPending = true;
			this.DisableVisualWheelCollisionProxies();
			this.CacheWheelEffects();
			this.ResetWheelEffects();
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x0011612A File Offset: 0x0011432A
		private void OnDisable()
		{
			this.ResetWheelEffects();
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x00116132 File Offset: 0x00114332
		private void DisableVisualWheelCollisionProxies()
		{
			BicycleVehicle.DisableSolidVisualColliders(this.frontWheelTransform);
			BicycleVehicle.DisableSolidVisualColliders(this.rearWheelTransform);
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x0011614C File Offset: 0x0011434C
		private static void DisableSolidVisualColliders(Transform wheelVisual)
		{
			if (wheelVisual == null)
			{
				return;
			}
			foreach (Collider collider in wheelVisual.GetComponentsInChildren<Collider>(true))
			{
				if (!(collider == null) && !collider.isTrigger && !(collider is WheelCollider))
				{
					collider.enabled = false;
				}
			}
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x001161A0 File Offset: 0x001143A0
		private void Start()
		{
			this.CacheWheelEffects();
			this.ResetWheelEffects();
			if (this.frontTrailContact == null)
			{
				Debug.LogWarning("Easy Bike WARNING: \n Front TrailsContact prefab has not been assigned Slip Trails and Smoke will not appear");
			}
			if (this.rearTrailContact == null)
			{
				Debug.LogWarning("Easy Bike WARNING: \n Rear TrailsContact prefab has not been assigned  Slip Trails and Smoke will not appear");
			}
			this.ConfigureWheelPhysics();
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x001161F0 File Offset: 0x001143F0
		private void ConfigureWheelPhysics()
		{
			if (this.frontWheel != null)
			{
				this.frontWheel.ConfigureVehicleSubsteps(5f, 12, 15);
			}
			if (this.rearWheel != null)
			{
				this.rearWheel.ConfigureVehicleSubsteps(5f, 12, 15);
			}
			this.wheelPhysicsConfigurationPending = false;
			BicycleRideDiagnostics.RecordIfAttached(this, "physics.substeps_configured", "speedThreshold=5; belowThreshold=12; aboveThreshold=15");
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x00116258 File Offset: 0x00114458
		private void FixedUpdate()
		{
			if (this.wheelPhysicsConfigurationPending)
			{
				this.ConfigureWheelPhysics();
			}
			if (this.isInControl)
			{
				this.HandleEngine();
				this.HandleSteering();
				this.LeanOnTurnLocal();
				this.UpdateHandles();
			}
			this.UpdateWheels();
			this.EmitTrail();
			this.Speed_O_Meter();
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x001162A5 File Offset: 0x001144A5
		public void InControl(bool state)
		{
			if (!state)
			{
				this.ResetWheelEffects();
			}
			if (this.isInControl != state)
			{
				this.isInControl = state;
				BicycleRideDiagnostics.RecordIfAttached(this, "control.changed", state.ToString());
			}
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x001162D4 File Offset: 0x001144D4
		public void ConstrainRotation(bool state)
		{
			RigidbodyConstraints constraints = this.rb.constraints;
			if (state)
			{
				this.rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			}
			else
			{
				this.rb.constraints = RigidbodyConstraints.None;
			}
			if (constraints != this.rb.constraints)
			{
				BicycleRideDiagnostics.RecordIfAttached(this, "constraints.changed", constraints.ToString() + " -> " + this.rb.constraints.ToString());
			}
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x00116354 File Offset: 0x00114554
		public bool OnGround()
		{
			return (this.frontWheel != null && this.frontWheel.isGrounded) || (this.rearWheel != null && this.rearWheel.isGrounded) || (this.controlContact != null && this.controlContact.GetContact());
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x001163B4 File Offset: 0x001145B4
		private void HandleEngine()
		{
			this.rearWheel.motorTorque = (this.braking ? 0f : (this.verticalInput * this.motorForce));
			float num = (this.braking ? this.brakeForce : 0f);
			this.ApplyBraking(num);
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x00116405 File Offset: 0x00114605
		private void ApplyBraking(float brakeForce)
		{
			this.frontWheel.brakeTorque = brakeForce * this.frontBrakePower;
			this.rearWheel.brakeTorque = brakeForce * this.rearBrakePower;
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x00116430 File Offset: 0x00114630
		private void MaxSteeringReductor()
		{
			float num = this.rb.velocity.magnitude / 30f * this.steerReductorAmmount;
			num = ((num > 1f) ? 1f : num);
			this.current_maxSteeringAngle = Mathf.LerpAngle(this.maxSteeringAngle, this.minSteeringAngle, num);
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x00116488 File Offset: 0x00114688
		private void HandleSteering()
		{
			this.MaxSteeringReductor();
			this.currentSteeringAngle = Mathf.Lerp(this.currentSteeringAngle, this.current_maxSteeringAngle * this.horizontalInput, this.turnSmoothing * 0.1f);
			this.frontWheel.steerAngle = this.currentSteeringAngle;
			this.targetLeanAngle = this.maxLeanAngle * -this.horizontalInput;
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x001164EC File Offset: 0x001146EC
		private void UpdateHandles()
		{
			float num = Mathf.DeltaAngle(this.handle.localEulerAngles.y, this.currentSteeringAngle);
			this.handle.Rotate(Vector3.up, num, Space.Self);
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x00116528 File Offset: 0x00114728
		private void LeanOnTurnLocal()
		{
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			if (this.rb.velocity.magnitude < 2f)
			{
				this.currentLeanAngle = Mathf.LerpAngle(localEulerAngles.z, 0f, 0.05f);
			}
			if (this.currentSteeringAngle < 0.5f && (double)this.currentSteeringAngle > -0.5)
			{
				this.currentLeanAngle = Mathf.LerpAngle(localEulerAngles.z, 0f, this.leanSmoothing * 0.1f);
			}
			else
			{
				this.currentLeanAngle = Mathf.LerpAngle(this.currentLeanAngle, this.targetLeanAngle, this.leanSmoothing * 0.1f);
				this.rb.centerOfMass = new Vector3(this.rb.centerOfMass.x, this.COG.y, this.rb.centerOfMass.z);
			}
			base.transform.localRotation = Quaternion.Euler(localEulerAngles.x, localEulerAngles.y, this.currentLeanAngle);
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x00116639 File Offset: 0x00114839
		private void UpdateWheels()
		{
			this.UpdateFrontWheel();
			this.UpdateSingleWheel(this.rearWheel, this.rearWheelTransform);
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x00116654 File Offset: 0x00114854
		private void UpdateFrontWheel()
		{
			this.GetWheelSpin();
			Vector3 vector;
			Quaternion quaternion;
			this.frontWheel.GetWorldPose(out vector, out quaternion);
			this.frontWheelTransform.position = vector;
			quaternion = Quaternion.Euler(this.frontSpin, 0f, 0f);
			this.frontWheelTransform.localRotation = quaternion;
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x001166A4 File Offset: 0x001148A4
		private void GetWheelSpin()
		{
			float num = this.frontWheel.rpm * 6f;
			this.frontSpin += num * Time.fixedDeltaTime;
			if (this.frontSpin >= 360f)
			{
				this.frontSpin = 0f;
			}
			if (this.frontSpin < 0f)
			{
				this.frontSpin = 360f;
			}
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x00116708 File Offset: 0x00114908
		private void EmitTrail()
		{
			if (this.rb == null || this.rb.isKinematic || !this.isInControl || Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up).sqrMagnitude < 0.5625f)
			{
				this.ResetWheelEffects();
				return;
			}
			bool flag = BicycleVehicle.ReadWheelSlip(this.frontWheel, out this.frontInfo, out this.frontFWD, out this.frontSideCoeff);
			bool flag2 = BicycleVehicle.ReadWheelSlip(this.rearWheel, out this.rearInfo, out this.rearFWD, out this.rearSideCoeff);
			this.slipFront = flag && this.SlipFront();
			this.slipRear = flag2 && this.SlipRear();
			BicycleVehicle.SetWheelEffect(this.frontTrail, this.frontSmoke, this.slipFront, false);
			BicycleVehicle.SetWheelEffect(this.rearTrail, this.rearSmoke, this.slipRear, false);
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x001167F4 File Offset: 0x001149F4
		private void CacheWheelEffects()
		{
			this.frontTrail = ((this.frontTrailContact != null) ? this.frontTrailContact.GetComponentInChildren<TrailRenderer>(true) : null);
			this.frontSmoke = ((this.frontTrailContact != null) ? this.frontTrailContact.GetComponentInChildren<ParticleSystem>(true) : null);
			this.rearTrail = ((this.rearTrailContact != null) ? this.rearTrailContact.GetComponentInChildren<TrailRenderer>(true) : null);
			this.rearSmoke = ((this.rearTrailContact != null) ? this.rearTrailContact.GetComponentInChildren<ParticleSystem>(true) : null);
			BicycleVehicle.DisableAutomaticSmoke(this.frontSmoke);
			BicycleVehicle.DisableAutomaticSmoke(this.rearSmoke);
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x001168A4 File Offset: 0x00114AA4
		private static void DisableAutomaticSmoke(ParticleSystem smoke)
		{
			if (smoke == null)
			{
				return;
			}
			smoke.main.playOnAwake = false;
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x001168CC File Offset: 0x00114ACC
		private void ResetWheelEffects()
		{
			this.slipFront = false;
			this.slipRear = false;
			this.frontInfo = default(WheelHit);
			this.rearInfo = default(WheelHit);
			this.frontFWD = (this.rearFWD = (this.frontSideCoeff = (this.rearSideCoeff = 0f)));
			BicycleVehicle.SetWheelEffect(this.frontTrail, this.frontSmoke, false, true);
			BicycleVehicle.SetWheelEffect(this.rearTrail, this.rearSmoke, false, true);
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x0011694C File Offset: 0x00114B4C
		private static bool ReadWheelSlip(WheelCollider wheel, out WheelHit hit, out float forward, out float sideways)
		{
			hit = default(WheelHit);
			forward = (sideways = 0f);
			if (wheel == null || !wheel.enabled || !wheel.gameObject.activeInHierarchy || !wheel.isGrounded || !wheel.GetGroundHit(out hit))
			{
				hit = default(WheelHit);
				return false;
			}
			forward = BicycleVehicle.NormalizeSlip(hit.forwardSlip, wheel.forwardFriction.extremumSlip);
			sideways = BicycleVehicle.NormalizeSlip(hit.sidewaysSlip, wheel.sidewaysFriction.extremumSlip);
			return true;
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x001169DC File Offset: 0x00114BDC
		private static float NormalizeSlip(float slip, float extremumSlip)
		{
			if (float.IsNaN(slip) || float.IsInfinity(slip) || float.IsNaN(extremumSlip) || float.IsInfinity(extremumSlip) || extremumSlip <= 0.0001f)
			{
				return 0f;
			}
			float num = slip / extremumSlip;
			if (!float.IsNaN(num) && !float.IsInfinity(num))
			{
				return num;
			}
			return 0f;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x00116A34 File Offset: 0x00114C34
		private static void SetWheelEffect(TrailRenderer trail, ParticleSystem smoke, bool emitting, bool clear)
		{
			if (trail != null)
			{
				if (trail.emitting != emitting)
				{
					trail.emitting = emitting;
				}
				if (clear && trail.positionCount > 0)
				{
					trail.Clear();
				}
			}
			if (smoke == null)
			{
				return;
			}
			if (emitting)
			{
				if (!smoke.isPlaying)
				{
					smoke.Play();
					return;
				}
			}
			else if (clear ? (!smoke.isStopped || smoke.IsAlive(true)) : smoke.isPlaying)
			{
				smoke.Stop(true, clear ? ParticleSystemStopBehavior.StopEmittingAndClear : ParticleSystemStopBehavior.StopEmitting);
			}
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x00116AB8 File Offset: 0x00114CB8
		private bool SlipFront()
		{
			return this.frontTrailContact != null && this.frontTrailContact.GetContact() && (this.frontFWD > 1.1f || this.frontFWD < -1.1f || this.frontSideCoeff > 1.1f || this.frontSideCoeff < -1.1f);
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x00116B1C File Offset: 0x00114D1C
		private bool SlipRear()
		{
			return this.rearTrailContact != null && this.rearTrailContact.GetContact() && (this.rearFWD > 1.1f || this.rearFWD < -1.1f || this.rearSideCoeff > 1.1f || this.rearSideCoeff < -1.1f);
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x00116B80 File Offset: 0x00114D80
		private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
		{
			Vector3 vector;
			Quaternion quaternion;
			wheelCollider.GetWorldPose(out vector, out quaternion);
			wheelTransform.rotation = quaternion;
			wheelTransform.position = vector;
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x00116BA8 File Offset: 0x00114DA8
		private void Speed_O_Meter()
		{
			this.currentSpeed = this.rb.velocity.magnitude;
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x00116BD0 File Offset: 0x00114DD0
		[ContextMenu("Set Wheels Springs and Frictions (Unity Default)")]
		public void DefaultSuspensionParameters()
		{
			this.rb = base.GetComponent<Rigidbody>();
			JointSpring jointSpring;
			jointSpring.spring = 23.33f * this.rb.mass;
			jointSpring.damper = 3f * this.rb.mass;
			jointSpring.targetPosition = this.frontWheel.suspensionSpring.targetPosition;
			this.frontWheel.suspensionSpring = jointSpring;
			jointSpring.spring = 23.33f * this.rb.mass;
			jointSpring.damper = 3f * this.rb.mass;
			jointSpring.targetPosition = this.rearWheel.suspensionSpring.targetPosition;
			this.rearWheel.suspensionSpring = jointSpring;
			this.FrictionParameters_Motorbike();
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x00116C98 File Offset: 0x00114E98
		[ContextMenu("Set Wheels Springs and Frictions (Motorbike)")]
		public void MotorbikeSuspensionParameters()
		{
			this.rb = base.GetComponent<Rigidbody>();
			JointSpring jointSpring;
			jointSpring.spring = 750f * this.rb.mass;
			jointSpring.damper = 32.5f * this.rb.mass;
			jointSpring.targetPosition = this.frontWheel.suspensionSpring.targetPosition;
			this.frontWheel.suspensionSpring = jointSpring;
			jointSpring.spring = 350f * this.rb.mass;
			jointSpring.damper = 22.5f * this.rb.mass;
			jointSpring.targetPosition = this.rearWheel.suspensionSpring.targetPosition;
			this.rearWheel.suspensionSpring = jointSpring;
			this.FrictionParameters_Motorbike();
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x00116D60 File Offset: 0x00114F60
		[ContextMenu("Set Wheels Springs and Frictions (Bicycle)")]
		public void BicycleSuspensionParameters()
		{
			this.rb = base.GetComponent<Rigidbody>();
			JointSpring jointSpring;
			jointSpring.spring = 256f * this.rb.mass;
			jointSpring.damper = 16f * this.rb.mass;
			jointSpring.targetPosition = this.frontWheel.suspensionSpring.targetPosition;
			this.frontWheel.suspensionSpring = jointSpring;
			jointSpring.spring = 219f * this.rb.mass;
			jointSpring.damper = 11.7f * this.rb.mass;
			jointSpring.targetPosition = this.rearWheel.suspensionSpring.targetPosition;
			this.rearWheel.suspensionSpring = jointSpring;
			this.FrictionParameters_Bicycle();
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x00116E28 File Offset: 0x00115028
		public void FrictionParameters_Motorbike()
		{
			WheelFrictionCurve wheelFrictionCurve = this.CreateFrictionCurve(0.3f, 1.25f, 0.5f, 1f, 1f);
			WheelFrictionCurve wheelFrictionCurve2 = this.CreateFrictionCurve(0.35f, 1.56f, 0.5f, 1f, 1f);
			this.frontWheel.forwardFriction = wheelFrictionCurve;
			this.frontWheel.sidewaysFriction = wheelFrictionCurve2;
			WheelFrictionCurve wheelFrictionCurve3 = this.CreateFrictionCurve(0.15f, 2.25f, 0.5f, 1f, 1f);
			WheelFrictionCurve wheelFrictionCurve4 = this.CreateFrictionCurve(0.3f, 2.15f, 0.5f, 1f, 1f);
			this.rearWheel.forwardFriction = wheelFrictionCurve3;
			this.rearWheel.sidewaysFriction = wheelFrictionCurve4;
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x00116EE8 File Offset: 0x001150E8
		public void FrictionParameters_Bicycle()
		{
			WheelFrictionCurve wheelFrictionCurve = this.CreateFrictionCurve(0.3f, 1f, 0.5f, 1f, 1f);
			WheelFrictionCurve wheelFrictionCurve2 = this.CreateFrictionCurve(0.35f, 1.25f, 0.5f, 1f, 1f);
			this.frontWheel.forwardFriction = wheelFrictionCurve;
			this.frontWheel.sidewaysFriction = wheelFrictionCurve2;
			WheelFrictionCurve wheelFrictionCurve3 = this.CreateFrictionCurve(0.15f, 2.5f, 0.6f, 1.75f, 1f);
			WheelFrictionCurve wheelFrictionCurve4 = this.CreateFrictionCurve(0.3f, 2.25f, 0.6f, 1.75f, 1f);
			this.rearWheel.forwardFriction = wheelFrictionCurve3;
			this.rearWheel.sidewaysFriction = wheelFrictionCurve4;
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x00116FA8 File Offset: 0x001151A8
		private WheelFrictionCurve CreateFrictionCurve(float exSlip, float exValue, float asSlip, float asValue, float stiffness)
		{
			return new WheelFrictionCurve
			{
				asymptoteSlip = asSlip,
				asymptoteValue = asValue,
				extremumSlip = exSlip,
				extremumValue = exValue,
				stiffness = stiffness
			};
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x00116FE8 File Offset: 0x001151E8
		public void WheelNormalizedSlipInfo()
		{
			this.frontWheel.GetGroundHit(out this.frontInfo);
			this.rearWheel.GetGroundHit(out this.rearInfo);
			this.rearSideCoeff = this.rearInfo.sidewaysSlip / this.rearWheel.sidewaysFriction.extremumSlip;
			this.frontSideCoeff = this.frontInfo.sidewaysSlip / this.frontWheel.sidewaysFriction.extremumSlip;
			this.frontFWD = this.frontInfo.forwardSlip / this.frontWheel.forwardFriction.extremumSlip;
			this.rearFWD = this.rearInfo.forwardSlip / this.rearWheel.forwardFriction.extremumSlip;
			string text = "";
			string text2 = "";
			if ((this.frontFWD < 1.1f && this.frontFWD > -1.1f) || (this.frontSideCoeff < 1.1f && this.frontSideCoeff > -1.1f))
			{
				text = "Grip";
			}
			if (this.frontFWD > 1.1f || this.frontFWD < -1.1f || this.frontSideCoeff > 1.1f || this.frontSideCoeff < -1.1f)
			{
				text = "Slip";
			}
			if (((this.rearFWD < 1.1f) & (this.rearFWD > -1.1f)) || (this.rearSideCoeff < 1.1f && this.rearSideCoeff > -1.1f))
			{
				text2 = "Grip";
			}
			if (this.rearFWD > 1.1f || this.rearFWD < -1.1f || this.rearSideCoeff > 1.1f || this.rearSideCoeff < -1.1f)
			{
				text2 = "Slip";
			}
			Debug.Log("Rear Coeficient = " + this.rearSideCoeff.ToString() + " //  Front Coeficient = " + this.rearSideCoeff.ToString());
			Debug.Log("Rear Grip = " + text2 + "//  Front Grip = " + text);
		}

		// Token: 0x040031E7 RID: 12775
		private Rigidbody rb;

		// Token: 0x040031E8 RID: 12776
		private bool wheelPhysicsConfigurationPending = true;

		// Token: 0x040031E9 RID: 12777
		[Header("Power/Braking")]
		[Space(5f)]
		[Tooltip("The force applied to the rear wheel when verticalinput si provided / is 0 when braking")]
		[SerializeField]
		public float motorForce = 50f;

		// Token: 0x040031EA RID: 12778
		[Tooltip("The maximum braking force applied when braking input is provided")]
		[SerializeField]
		public float brakeForce = 100f;

		// Token: 0x040031EB RID: 12779
		[Tooltip("The braking power used on the front wheel, the moment braking is enabled. (0 = no braking / 1 = brakeforce)")]
		[Range(0f, 1f)]
		public float frontBrakePower = 1f;

		// Token: 0x040031EC RID: 12780
		[Tooltip("The braking power used on the rear wheel, the moment braking is enabled. (0 = no braking / 1 = brakeforce)")]
		[Range(0f, 1f)]
		public float rearBrakePower = 1f;

		// Token: 0x040031ED RID: 12781
		[Tooltip("Adjust this paameter to add an offset to the center of gravity")]
		public Vector3 COG;

		// Token: 0x040031EE RID: 12782
		[Space(20f)]
		[Header("Steering")]
		[Space(5f)]
		[Tooltip("Defines the maximum steering angle for the bike, when horizontal input is provided")]
		[SerializeField]
		private float maxSteeringAngle = 45f;

		// Token: 0x040031EF RID: 12783
		[Tooltip("Defines the highest possisble degrees of Steering at 30m/s (108 km/h) use a low value")]
		[SerializeField]
		private float minSteeringAngle = 5f;

		// Token: 0x040031F0 RID: 12784
		[Tooltip("Sets how current_MaxSteering is reduced based on the speed of the RB, (0 - No effect) (1 - Full)")]
		[Range(0f, 1f)]
		[SerializeField]
		private float steerReductorAmmount;

		// Token: 0x040031F1 RID: 12785
		[Tooltip("Sets the Steering sensitivity [Steering Stiffness] 0 - No turn, 1 - SensitiveTurn)")]
		[Range(0.001f, 1f)]
		[SerializeField]
		private float turnSmoothing;

		// Token: 0x040031F2 RID: 12786
		[Space(20f)]
		[Header("Lean")]
		[Space(5f)]
		[Tooltip("Defines the maximum leaning angle when turning for this bike")]
		[SerializeField]
		private float maxLeanAngle = 45f;

		// Token: 0x040031F3 RID: 12787
		[Tooltip("Sets the Leaning sensitivity (0 - None, 1 - full")]
		[Range(0.001f, 1f)]
		[SerializeField]
		private float leanSmoothing;

		// Token: 0x040031F4 RID: 12788
		private protected float targetLeanAngle;

		// Token: 0x040031F5 RID: 12789
		[Space(20f)]
		[Header("Object References")]
		public Transform handle;

		// Token: 0x040031F6 RID: 12790
		[Space(10f)]
		[SerializeField]
		private WheelCollider frontWheel;

		// Token: 0x040031F7 RID: 12791
		[SerializeField]
		private WheelCollider rearWheel;

		// Token: 0x040031F8 RID: 12792
		[Space(10f)]
		[SerializeField]
		private Transform frontWheelTransform;

		// Token: 0x040031F9 RID: 12793
		[SerializeField]
		private Transform rearWheelTransform;

		// Token: 0x040031FA RID: 12794
		private TrailRenderer frontTrail;

		// Token: 0x040031FB RID: 12795
		private TrailRenderer rearTrail;

		// Token: 0x040031FC RID: 12796
		[Space(10f)]
		[Tooltip("Drop here the Front Trail Prefab")]
		[SerializeField]
		private ContactProvider frontTrailContact;

		// Token: 0x040031FD RID: 12797
		[Tooltip("Drop here the Rear Trail Prefab")]
		[SerializeField]
		private ContactProvider rearTrailContact;

		// Token: 0x040031FE RID: 12798
		private ParticleSystem frontSmoke;

		// Token: 0x040031FF RID: 12799
		private ParticleSystem rearSmoke;

		// Token: 0x04003200 RID: 12800
		private const float MinimumWheelEffectSpeed = 0.75f;

		// Token: 0x04003201 RID: 12801
		[Space(10f)]
		public ContactProvider controlContact;

		// Token: 0x04003206 RID: 12806
		private protected WheelHit frontInfo;

		// Token: 0x04003207 RID: 12807
		private protected WheelHit rearInfo;

		// Token: 0x04003208 RID: 12808
		private protected float frontSpin;

		// Token: 0x04003209 RID: 12809
		private protected float rearSideCoeff;

		// Token: 0x0400320A RID: 12810
		private protected float frontSideCoeff;

		// Token: 0x0400320B RID: 12811
		private float frontFWD;

		// Token: 0x0400320C RID: 12812
		private float rearFWD;
	}
}
