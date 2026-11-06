using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000003 RID: 3
	[RequireComponent(typeof(Rigidbody))]
	public class VehicleAI : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public static void CollectActiveVehicles(List<VehicleAI> results)
		{
			results.Clear();
			foreach (VehicleAI vehicleAI in VehicleAI.OriginInstances)
			{
				if (vehicleAI != null && vehicleAI.gameObject.activeInHierarchy)
				{
					results.Add(vehicleAI);
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002130 File Offset: 0x00000330
		public static void ApplyWorldOriginShiftToAll(Vector3 delta)
		{
			foreach (VehicleAI vehicleAI in VehicleAI.OriginInstances)
			{
				if (vehicleAI != null)
				{
					vehicleAI.ApplyWorldOriginShift(delta);
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000005 RID: 5 RVA: 0x0000218C File Offset: 0x0000038C
		// (remove) Token: 0x06000006 RID: 6 RVA: 0x000021C4 File Offset: 0x000003C4
		public event Action<VehicleAI> OnApproachingEnd;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000007 RID: 7 RVA: 0x000021FC File Offset: 0x000003FC
		// (remove) Token: 0x06000008 RID: 8 RVA: 0x00002234 File Offset: 0x00000434
		public event Action<VehicleAI> OnEndReached;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000009 RID: 9 RVA: 0x0000226C File Offset: 0x0000046C
		// (remove) Token: 0x0600000A RID: 10 RVA: 0x000022A4 File Offset: 0x000004A4
		public event Action<VehicleAI, TrackData> OnTransitioned;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600000B RID: 11 RVA: 0x000022DC File Offset: 0x000004DC
		// (remove) Token: 0x0600000C RID: 12 RVA: 0x00002314 File Offset: 0x00000514
		public event Action<VehicleAI> OnStuck;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002349 File Offset: 0x00000549
		public float CurrentSpeed
		{
			get
			{
				return this._currentSpeed;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002351 File Offset: 0x00000551
		public TrackData CurrentTrack
		{
			get
			{
				return this._track;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002359 File Offset: 0x00000559
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002361 File Offset: 0x00000561
		public uint SpawnGeneration { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000236A File Offset: 0x0000056A
		public bool IsBlocked
		{
			get
			{
				return this._blocked || this._interactionStopOwners.Count > 0;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002384 File Offset: 0x00000584
		public float PathT
		{
			get
			{
				if (this._state != VehicleAI.State.Blending)
				{
					return this._pathT;
				}
				return this._blendT;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000239C File Offset: 0x0000059C
		public Vector3 TravelDirection
		{
			get
			{
				return this._travelDirection;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000023A4 File Offset: 0x000005A4
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000023AC File Offset: 0x000005AC
		public bool HasPassenger
		{
			get
			{
				return this._hasPassenger;
			}
			set
			{
				if (this._hasPassenger != value && VspDiag.Carry)
				{
					Debug.Log(string.Format("[CarryDiag] HasPassenger '{0}': {1} → {2} frame={3} pos={4}", new object[]
					{
						base.name,
						this._hasPassenger,
						value,
						Time.frameCount,
						base.transform.position
					}), this);
				}
				if (this._hasPassenger != value)
				{
					VehicleAI._passengerCarCount = Mathf.Max(0, VehicleAI._passengerCarCount + (value ? 1 : (-1)));
				}
				this._hasPassenger = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002447 File Offset: 0x00000647
		public static bool PassengerCarActive
		{
			get
			{
				return VehicleAI._passengerCarCount > 0;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002451 File Offset: 0x00000651
		public Vector3 ExitPosition
		{
			get
			{
				if (!(this._track != null))
				{
					return base.transform.position;
				}
				return this._track.GetWorldPosition(1f);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000247D File Offset: 0x0000067D
		public Vector3 ExitTangent
		{
			get
			{
				if (!(this._track != null))
				{
					return base.transform.forward;
				}
				return this._track.GetWorldTangent(1f);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000024A9 File Offset: 0x000006A9
		public void ApplyWorldOriginShift(Vector3 delta)
		{
			this._blendP0 += delta;
			this._blendP1 += delta;
			this._diagLastPos += delta;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024E4 File Offset: 0x000006E4
		private void Awake()
		{
			VehicleAI.OriginInstances.Add(this);
			this._rb = base.GetComponent<Rigidbody>();
			this._rb.isKinematic = true;
			this._rb.useGravity = false;
			this._rb.interpolation = RigidbodyInterpolation.None;
			this._bodyCollider = base.GetComponent<Collider>();
			if (this._bodyCollider == null)
			{
				BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
				boxCollider.size = new Vector3(1.8f, 0.8f, 3.8f);
				this._bodyCollider = boxCollider;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002574 File Offset: 0x00000774
		private void OnDestroy()
		{
			VehicleAI.OriginInstances.Remove(this);
			this.HasPassenger = false;
			this.UnregisterFromTrack();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000258F File Offset: 0x0000078F
		private void OnDisable()
		{
			this._interactionStopOwners.Clear();
			this.UnregisterFromTrack();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000025A4 File Offset: 0x000007A4
		public void Initialize(TrackData track, float startT)
		{
			uint spawnGeneration = this.SpawnGeneration;
			this.SpawnGeneration = spawnGeneration + 1U;
			VehicleAI.OriginInstances.Add(this);
			this.IsAbandoned = false;
			this._blocked = false;
			this._interactionStopOwners.Clear();
			this._speedZoneLimit = -1f;
			this._stuckTimer = 0f;
			this._currentSpeed = 0f;
			this.HasPassenger = false;
			this.PassengerRoot = null;
			this._courtesyStopped = false;
			this._courtesyStopTimer = 0f;
			this._courtesyCooldownTimer = 0f;
			this._courtesyCheckTimer = 0f;
			this._diagHasLastPose = false;
			this._diagSpinAccumDeg = 0f;
			this._diagLowSpeedTime = 0f;
			this._diagStallReported = false;
			this.AttachToTrack(track, startT, true);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000266A File Offset: 0x0000086A
		public void SetInitialSpeed(float speed)
		{
			this._currentSpeed = Mathf.Clamp(speed, 0f, this.EffectiveSpeedLimit);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002684 File Offset: 0x00000884
		private void AttachToTrack(TrackData track, float startT, bool snapImmediately)
		{
			this._state = VehicleAI.State.Following;
			this._track = track;
			this._pathLength = track.GetLength();
			this._pathT = startT;
			this._lookedAhead = false;
			this._nextTrack = null;
			this._curveSpeedLimit = float.MaxValue;
			this._curvatureTimer = 0f;
			this._fullSim = true;
			this._lodTimer = 0f;
			this._staggerCurvatureCadence = true;
			this._staggerLodCadence = true;
			this._deadEndAhead = false;
			this.RegisterOnTrack(track);
			if (snapImmediately)
			{
				this.SnapToPath(true);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002710 File Offset: 0x00000910
		private void RegisterOnTrack(TrackData track)
		{
			if (this._registeredTrack == track)
			{
				return;
			}
			this.UnregisterFromTrack();
			if (track == null)
			{
				return;
			}
			List<VehicleAI> list;
			if (!VehicleAI.s_vehiclesByTrack.TryGetValue(track, out list))
			{
				list = (VehicleAI.s_vehiclesByTrack[track] = new List<VehicleAI>(4));
			}
			list.Add(this);
			this._registeredTrack = track;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000276C File Offset: 0x0000096C
		private void UnregisterFromTrack()
		{
			if (this._registeredTrack == null)
			{
				return;
			}
			List<VehicleAI> list;
			if (VehicleAI.s_vehiclesByTrack.TryGetValue(this._registeredTrack, out list))
			{
				list.Remove(this);
				if (list.Count == 0)
				{
					VehicleAI.s_vehiclesByTrack.Remove(this._registeredTrack);
				}
			}
			this._registeredTrack = null;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027C4 File Offset: 0x000009C4
		[return: TupleElementNames(new string[] { "gap", "leaderSpeed", "isSoft" })]
		private ValueTuple<float, float, bool> SenseLeaderOnSpline()
		{
			if (this._state != VehicleAI.State.Following || this._registeredTrack == null)
			{
				return new ValueTuple<float, float, bool>(float.MaxValue, 0f, false);
			}
			List<VehicleAI> list;
			if (!VehicleAI.s_vehiclesByTrack.TryGetValue(this._registeredTrack, out list) || list.Count < 2)
			{
				return new ValueTuple<float, float, bool>(float.MaxValue, 0f, false);
			}
			VehicleAI vehicleAI = null;
			float num = float.MaxValue;
			for (int i = 0; i < list.Count; i++)
			{
				VehicleAI vehicleAI2 = list[i];
				if (!(vehicleAI2 == null) && !(vehicleAI2 == this) && vehicleAI2._pathT > this._pathT && vehicleAI2._pathT < num)
				{
					num = vehicleAI2._pathT;
					vehicleAI = vehicleAI2;
				}
			}
			if (vehicleAI != null)
			{
				return new ValueTuple<float, float, bool>(Mathf.Max((num - this._pathT) * this._pathLength - this.vehicleLength, 0.01f), vehicleAI._currentSpeed, false);
			}
			List<VehicleAI> list2;
			if (this._nextTrack != null && VehicleAI.s_vehiclesByTrack.TryGetValue(this._nextTrack, out list2))
			{
				VehicleAI vehicleAI3 = null;
				float num2 = float.MaxValue;
				for (int j = 0; j < list2.Count; j++)
				{
					VehicleAI vehicleAI4 = list2[j];
					if (!(vehicleAI4 == null) && !(vehicleAI4 == this) && vehicleAI4._pathT < num2)
					{
						num2 = vehicleAI4._pathT;
						vehicleAI3 = vehicleAI4;
					}
				}
				if (vehicleAI3 != null)
				{
					return new ValueTuple<float, float, bool>(Mathf.Max((1f - this._pathT) * this._pathLength + num2 * this._nextTrackLength - this.vehicleLength, 0.01f), vehicleAI3._currentSpeed, false);
				}
			}
			return new ValueTuple<float, float, bool>(float.MaxValue, 0f, false);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000298E File Offset: 0x00000B8E
		public void Stop()
		{
			this._blocked = true;
			this._currentSpeed = 0f;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000029A2 File Offset: 0x00000BA2
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000029AA File Offset: 0x00000BAA
		public bool IsAbandoned { get; private set; }

		// Token: 0x06000026 RID: 38 RVA: 0x000029B3 File Offset: 0x00000BB3
		public void SetAbandoned(bool value)
		{
			this.IsAbandoned = value;
			if (value)
			{
				this._blocked = true;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000029C6 File Offset: 0x00000BC6
		public void SetBlocked(bool value)
		{
			if (this.IsAbandoned && !value)
			{
				return;
			}
			this._blocked = value;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000029DB File Offset: 0x00000BDB
		public void SetInteractionBlocked(Object owner, bool value)
		{
			if (owner == null)
			{
				return;
			}
			if (value && owner != null)
			{
				this._interactionStopOwners.Add(owner);
				return;
			}
			this._interactionStopOwners.Remove(owner);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A08 File Offset: 0x00000C08
		public void MarkDeadEndAhead()
		{
			this._deadEndAhead = true;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A11 File Offset: 0x00000C11
		public void SetNextTrack(TrackData next)
		{
			this._nextTrack = next;
			this._nextTrackLength = ((next != null) ? next.GetLength() : 0f);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A36 File Offset: 0x00000C36
		public void SetSpeedZoneLimit(float limit)
		{
			this._speedZoneLimit = limit;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A3F File Offset: 0x00000C3F
		public void ClearSpeedZoneLimit()
		{
			this._speedZoneLimit = -1f;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002A4C File Offset: 0x00000C4C
		private float EffectiveSpeedLimit
		{
			get
			{
				float num = Mathf.Min(this.cruiseSpeed, (this._track != null) ? this._track.speedLimit : this.cruiseSpeed);
				if (this._speedZoneLimit > 0f)
				{
					num = Mathf.Min(num, this._speedZoneLimit);
				}
				if (this._curveSpeedLimit > 0f)
				{
					num = Mathf.Min(num, this._curveSpeedLimit);
				}
				return Mathf.Max(num, 0.1f);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002AC8 File Offset: 0x00000CC8
		private void FixedUpdate()
		{
			if (VehicleSimulationPause.IsPaused)
			{
				return;
			}
			if (this._track == null)
			{
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.UpdateCurvatureSpeed(fixedDeltaTime);
			this.UpdateSimulationLod(fixedDeltaTime);
			ValueTuple<float, float, bool> valueTuple = ((this._fullSim || this.HasPassenger) ? this.SenseAhead() : this.SenseLeaderOnSpline());
			this.UpdateCourtesyStop(fixedDeltaTime);
			float num;
			if (this.IsBlocked || this._courtesyStopped)
			{
				num = -this.comfortDeceleration * 2f;
			}
			else if (valueTuple.Item3 && valueTuple.Item1 < 3.4028235E+38f)
			{
				float num2 = ((this.detectionProfile != null) ? this.detectionProfile.detectionRange : 15f);
				float num3 = this.EffectiveSpeedLimit * Mathf.Clamp01((valueTuple.Item1 - 2.5f) / Mathf.Max(num2 - 2.5f, 0.1f));
				float num4 = ((valueTuple.Item1 <= 2.5f) ? (this.comfortDeceleration * 3f) : this.comfortDeceleration);
				num = ((num3 < this._currentSpeed) ? Mathf.Max((num3 - this._currentSpeed) / fixedDeltaTime, -num4) : this.ComputeIDM(valueTuple.Item1, valueTuple.Item2));
			}
			else
			{
				num = this.ComputeIDM(valueTuple.Item1, valueTuple.Item2);
			}
			float num5 = this.EffectiveSpeedLimit;
			if (this._deadEndAhead && this._state == VehicleAI.State.Following && this._pathLength > 0f)
			{
				float num6 = Mathf.Max((1f - this._pathT) * this._pathLength, 0.05f);
				num5 = Mathf.Min(num5, Mathf.Sqrt(2f * this.comfortDeceleration * num6) + 0.3f);
			}
			float num7 = Mathf.Clamp(this._currentSpeed + num * fixedDeltaTime, 0f, num5);
			float num8 = this.comfortDeceleration * 3f * fixedDeltaTime;
			this._currentSpeed = Mathf.Max(num7, this._currentSpeed - num8);
			if (!this.IsBlocked && !this._courtesyStopped)
			{
				if (this._currentSpeed < 0.05f)
				{
					this._stuckTimer += fixedDeltaTime;
					if (this._stuckTimer >= this.stuckTimeout)
					{
						Action<VehicleAI> onStuck = this.OnStuck;
						if (onStuck != null)
						{
							onStuck(this);
						}
					}
				}
				else
				{
					this._stuckTimer = 0f;
				}
			}
			else
			{
				this._stuckTimer = 0f;
			}
			if (VspDiag.Sense && !this._fullSim && !this.HasPassenger && this.simLodPlayer != null)
			{
				this._diagLodWarnTimer -= fixedDeltaTime;
				if (this._diagLodWarnTimer <= 0f && (base.transform.position - this.simLodPlayer.position).sqrMagnitude < 225f)
				{
					this._diagLodWarnTimer = 1f;
					Debug.LogError(string.Format("[SenseDiag] '{0}' LOD says FAR (SenseAhead skipped) but player is {1:F1}m away! fullSimRadius={2} lodTimer={3:F2} frame={4}", new object[]
					{
						base.name,
						(base.transform.position - this.simLodPlayer.position).magnitude,
						this.fullSimRadius,
						this._lodTimer,
						Time.frameCount
					}), this);
				}
			}
			if (this._currentSpeed > 0.01f)
			{
				if (this._state == VehicleAI.State.Blending)
				{
					this.AdvanceAlongBlend(this._currentSpeed, fixedDeltaTime);
				}
				else
				{
					this.AdvanceAlongPath(this._currentSpeed, fixedDeltaTime);
				}
			}
			if (VspDiag.Curve)
			{
				this.CurveDiagDetectors(fixedDeltaTime);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002E5C File Offset: 0x0000105C
		private void CurveDiagDetectors(float dt)
		{
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			if (!this._diagHasLastPose)
			{
				this._diagLastPos = position;
				this._diagLastRot = rotation;
				this._diagHasLastPose = true;
				return;
			}
			float num = (position - this._diagLastPos).magnitude / dt;
			float num2 = Quaternion.Angle(this._diagLastRot, rotation) / dt;
			this._diagLastPos = position;
			this._diagLastRot = rotation;
			this._diagSpinWarnTimer -= dt;
			if (this._state == VehicleAI.State.Blending)
			{
				if (this._currentSpeed < 0.3f)
				{
					this._diagLowSpeedTime += dt;
					if (this._diagLowSpeedTime > 1f && !this._diagStallReported)
					{
						this._diagStallReported = true;
						Debug.LogError(string.Format("[CurveDiag] BLEND STALL '{0}': speed={1:F2} for >{2:F1}s blendT={3:F2} blendLen={4:F2} curveLimit={5} blocked={6} track='{7}' frame={8}", new object[]
						{
							base.name,
							this._currentSpeed,
							this._diagLowSpeedTime,
							this._blendT,
							this._blendLength,
							VehicleAI.FmtLimit(this._curveSpeedLimit),
							this._blocked,
							(this._track != null) ? this._track.name : "null",
							Time.frameCount
						}), this);
					}
				}
				else
				{
					this._diagLowSpeedTime = 0f;
				}
			}
			else
			{
				this._diagLowSpeedTime = 0f;
				this._diagStallReported = false;
			}
			if (num2 > 200f && this._diagSpinWarnTimer <= 0f)
			{
				this._diagSpinWarnTimer = 1f;
				Debug.LogError(string.Format("[CurveDiag] TROTTOLA '{0}': angVel={1:F0}°/s posVel={2:F2}m/s state={3} blendT={4:F2} speed={5:F2} track='{6}' frame={7}", new object[]
				{
					base.name,
					num2,
					num,
					this._state,
					this._blendT,
					this._currentSpeed,
					(this._track != null) ? this._track.name : "null",
					Time.frameCount
				}), this);
			}
			if (num < 0.05f)
			{
				this._diagSpinAccumDeg += num2 * dt;
				if (this._diagSpinAccumDeg > 90f && this._diagSpinWarnTimer <= 0f)
				{
					this._diagSpinWarnTimer = 1f;
					Debug.LogError(string.Format("[CurveDiag] ROTATE-ON-SPOT '{0}': {1:F0}° accumulated while stationary, state={2} speed={3:F2} blendT={4:F2} curveLimit={5} blocked={6} frame={7}", new object[]
					{
						base.name,
						this._diagSpinAccumDeg,
						this._state,
						this._currentSpeed,
						this._blendT,
						VehicleAI.FmtLimit(this._curveSpeedLimit),
						this._blocked,
						Time.frameCount
					}), this);
					this._diagSpinAccumDeg = 0f;
					return;
				}
			}
			else
			{
				this._diagSpinAccumDeg = 0f;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003181 File Offset: 0x00001381
		private static string FmtLimit(float v)
		{
			if (v < 1.7014117E+38f)
			{
				return v.ToString("F2");
			}
			return "inf";
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000031A0 File Offset: 0x000013A0
		private void UpdateCourtesyStop(float dt)
		{
			if (this._courtesyCooldownTimer > 0f)
			{
				this._courtesyCooldownTimer -= dt;
			}
			if (this._courtesyStopped)
			{
				this._courtesyStopTimer -= dt;
				if (this._courtesyStopTimer <= 0f || this.HasPassenger || VehicleAI.PassengerCarActive)
				{
					this._courtesyStopped = false;
					this._courtesyCooldownTimer = 5f;
				}
				return;
			}
			if (this.HasPassenger || VehicleAI.PassengerCarActive || this.hasIntersectionPriority)
			{
				return;
			}
			if (this._courtesyCooldownTimer > 0f || this.simLodPlayer == null)
			{
				return;
			}
			this._courtesyCheckTimer -= dt;
			if (this._courtesyCheckTimer > 0f)
			{
				return;
			}
			this._courtesyCheckTimer = 0.15f;
			Vector3 position = this.simLodPlayer.position;
			float num = this.vehicleLength * 0.5f + 1f + 1f;
			if ((base.transform.position - position).sqrMagnitude > num * num)
			{
				return;
			}
			if (this._bodyCollider == null)
			{
				return;
			}
			Vector3 vector = this._bodyCollider.ClosestPointOnBounds(position);
			vector.y = position.y;
			if ((vector - position).sqrMagnitude <= 1f)
			{
				this._courtesyStopped = true;
				this._courtesyStopTimer = 2f;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003300 File Offset: 0x00001500
		private void UpdateSimulationLod(float dt)
		{
			if (this.simLodPlayer == null || this.fullSimRadius <= 0f)
			{
				this._fullSim = true;
				return;
			}
			this._lodTimer -= dt;
			if (this._lodTimer > 0f)
			{
				return;
			}
			this._lodTimer = this.ScheduleNextCadence(0.5f, ref this._staggerLodCadence, 2246822507U);
			float sqrMagnitude = (base.transform.position - this.simLodPlayer.position).sqrMagnitude;
			this._fullSim = sqrMagnitude <= this.fullSimRadius * this.fullSimRadius;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000033A5 File Offset: 0x000015A5
		private float ScheduleNextCadence(float interval, ref bool staggerFirstRepeat, uint salt)
		{
			if (!staggerFirstRepeat)
			{
				return interval;
			}
			staggerFirstRepeat = false;
			return this.CadencePhase(interval, salt);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000033B8 File Offset: 0x000015B8
		private float CadencePhase(float interval, uint salt)
		{
			int num = base.GetInstanceID() ^ (int)salt;
			int num2 = (num ^ (int)((uint)num >> 16)) * 2146121005;
			return ((num2 ^ (int)((uint)num2 >> 15)) & 65535) * (interval / 65536f);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000033E4 File Offset: 0x000015E4
		private void UpdateCurvatureSpeed(float dt)
		{
			if (this._state != VehicleAI.State.Following || this._track == null)
			{
				return;
			}
			this._curvatureTimer -= dt;
			if (this._curvatureTimer > 0f)
			{
				return;
			}
			this._curvatureTimer = this.ScheduleNextCadence(0.2f, ref this._staggerCurvatureCadence, 2654435769U);
			float num = this.OffsetT(this._pathT, 5f);
			float num2 = this.EstimateCurvature(this._pathT);
			float num3 = this.EstimateCurvature(num);
			float num4 = Mathf.Max(num2, num3);
			if (num4 <= 0.0001f)
			{
				this._curveSpeedLimit = float.MaxValue;
				return;
			}
			float num5 = Mathf.Sqrt(this.maxLateralAccel / num4);
			this._curveSpeedLimit = ((float.IsNaN(num5) || float.IsInfinity(num5)) ? float.MaxValue : num5);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000034B0 File Offset: 0x000016B0
		private float EstimateCurvature(float t)
		{
			float num = this.OffsetT(t, 2f);
			Vector3 worldTangent = this._track.GetWorldTangent(t);
			Vector3 worldTangent2 = this._track.GetWorldTangent(num);
			if (worldTangent.sqrMagnitude <= 1E-06f || worldTangent2.sqrMagnitude <= 1E-06f)
			{
				return 0f;
			}
			float num2 = Vector3.Angle(worldTangent, worldTangent2) * 0.017453292f;
			if (float.IsNaN(num2))
			{
				return 0f;
			}
			Vector3 worldPosition = this._track.GetWorldPosition(t);
			Vector3 worldPosition2 = this._track.GetWorldPosition(num);
			float num3 = Vector3.Distance(worldPosition, worldPosition2);
			if (num3 < 0.001f)
			{
				return 0f;
			}
			return num2 / num3;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003558 File Offset: 0x00001758
		private float OffsetT(float t, float meters)
		{
			if (this._pathLength < 0.001f)
			{
				return t;
			}
			return Mathf.Clamp01(t + meters / this._pathLength);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003578 File Offset: 0x00001778
		private float ComputeIDM(float gap, float leaderSpeed)
		{
			float currentSpeed = this._currentSpeed;
			float effectiveSpeedLimit = this.EffectiveSpeedLimit;
			float num = 1f - Mathf.Pow(currentSpeed / effectiveSpeedLimit, 4f);
			float num2 = ((this.detectionProfile != null) ? this.detectionProfile.detectionRange : 15f);
			if (gap >= num2)
			{
				return this.maxAcceleration * num;
			}
			float num3 = currentSpeed - leaderSpeed;
			float num4 = Mathf.Pow((this.minGap + Mathf.Max(0f, currentSpeed * this.timeHeadway + currentSpeed * num3 / (2f * Mathf.Sqrt(this.maxAcceleration * this.comfortDeceleration)))) / Mathf.Max(gap, 0.01f), 2f);
			return this.maxAcceleration * (num - num4);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003634 File Offset: 0x00001834
		private bool IsParallelOncoming(VehicleAI other, Vector3 hitPoint)
		{
			if (Vector3.Dot(this._travelDirection, other.TravelDirection) > -0.6f)
			{
				return false;
			}
			Vector3 vector = other.transform.position - base.transform.position;
			Vector3 normalized = Vector3.Cross(this._travelDirection, Vector3.up).normalized;
			float num = Mathf.Abs(Vector3.Dot(vector, normalized));
			float num2 = Vector3.Dot(vector, this._travelDirection);
			return num > 1f || num2 < 0f;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000036BC File Offset: 0x000018BC
		[return: TupleElementNames(new string[] { "gap", "leaderSpeed", "isSoftObstacle" })]
		private ValueTuple<float, float, bool> SenseAhead()
		{
			if (this.detectionProfile == null)
			{
				return new ValueTuple<float, float, bool>(float.MaxValue, 0f, false);
			}
			float num = this.vehicleLength * 0.5f;
			Vector3 vector = base.transform.position + Vector3.up * 0.5f;
			int num2 = Physics.SphereCastNonAlloc(vector, this.detectionProfile.detectionRadius, this._travelDirection, VehicleAI._hitBuffer, this.detectionProfile.detectionRange + num, this.detectionProfile.detectionLayers, QueryTriggerInteraction.Ignore);
			float num3 = float.MaxValue;
			float num4 = 0f;
			bool flag = false;
			int num5 = Physics.OverlapSphereNonAlloc(vector, this.detectionProfile.detectionRadius, VehicleAI._originOverlapBuffer, this.detectionProfile.detectionLayers, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num5; i++)
			{
				Collider collider = VehicleAI._originOverlapBuffer[i];
				if (!(collider == null) && !collider.transform.IsChildOf(base.transform) && (!(this.PassengerRoot != null) || !collider.transform.IsChildOf(this.PassengerRoot)) && Vector3.Dot(collider.bounds.center - vector, this._travelDirection) >= 0f && this.detectionProfile.IsBlockingTag(collider.gameObject.tag))
				{
					num3 = 0f;
					flag = true;
					break;
				}
			}
			for (int j = 0; j < num2; j++)
			{
				Collider collider2 = VehicleAI._hitBuffer[j].collider;
				if (!(collider2 == null) && !collider2.transform.IsChildOf(base.transform) && (!(this.PassengerRoot != null) || !collider2.transform.IsChildOf(this.PassengerRoot)))
				{
					VehicleAI componentInParent = collider2.GetComponentInParent<VehicleAI>();
					if (componentInParent != null && componentInParent != this)
					{
						if ((!this.hasIntersectionPriority || componentInParent.CurrentSpeed <= 0.5f) && (!this.ignoreOtherVehicles || !this.IsParallelOncoming(componentInParent, VehicleAI._hitBuffer[j].point)))
						{
							float num6 = Mathf.Max(0f, VehicleAI._hitBuffer[j].distance - num);
							if (num6 < num3)
							{
								num3 = num6;
								num4 = componentInParent.CurrentSpeed;
								flag = false;
							}
						}
					}
					else if (this.detectionProfile.IsBlockingTag(collider2.gameObject.tag) && Vector3.Dot(collider2.bounds.center - vector, this._travelDirection) >= 0f)
					{
						float num7 = Mathf.Max(0f, VehicleAI._hitBuffer[j].distance - num);
						if (num7 < num3)
						{
							num3 = num7;
							num4 = 0f;
							flag = true;
						}
					}
				}
			}
			if (VspDiag.Sense && this.simLodPlayer != null)
			{
				this._diagSenseLogTimer -= Time.fixedDeltaTime;
				float num8 = Vector3.Distance(base.transform.position, this.simLodPlayer.position);
				if (this._diagSenseLogTimer <= 0f && num8 <= 15f)
				{
					this._diagSenseLogTimer = 0.5f;
					this.SenseDiagDump(vector, num2, num3, num4, num8);
				}
			}
			return new ValueTuple<float, float, bool>(num3, num4, flag);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003A30 File Offset: 0x00001C30
		private void SenseDiagDump(Vector3 origin, int hitCount, float chosenGap, float leaderSpeed, float playerDist)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			stringBuilder.Append(string.Format("[SenseDiag] '{0}' speed={1:F2} limit={2} fullSim={3} playerDist={4:F1} ", new object[]
			{
				base.name,
				this._currentSpeed,
				VehicleAI.FmtLimit(this.EffectiveSpeedLimit),
				this._fullSim,
				playerDist
			}));
			stringBuilder.Append(string.Format("origin={0:F2} dir={1:F2} radius={2} range={3} ", new object[]
			{
				origin,
				this._travelDirection,
				this.detectionProfile.detectionRadius,
				this.detectionProfile.detectionRange
			}));
			stringBuilder.Append(string.Format("layers={0} queriesHitTriggers={1} vehicleLength={2:F2} ", this.detectionProfile.detectionLayers.value, Physics.queriesHitTriggers, this.vehicleLength));
			stringBuilder.Append(string.Format("chosenGap={0} leaderSpeed={1:F2} hits={2} | ", (chosenGap >= 1.7014117E+38f) ? "none" : chosenGap.ToString("F2"), leaderSpeed, hitCount));
			for (int i = 0; i < hitCount; i++)
			{
				Collider collider = VehicleAI._hitBuffer[i].collider;
				if (collider == null)
				{
					stringBuilder.Append(string.Format("[{0}] null; ", i));
				}
				else
				{
					stringBuilder.Append(string.Format("[{0}] '{1}' tag={2} layer={3} trig={4} dist={5:F2} self={6}; ", new object[]
					{
						i,
						collider.name,
						collider.tag,
						LayerMask.LayerToName(collider.gameObject.layer),
						collider.isTrigger,
						VehicleAI._hitBuffer[i].distance,
						collider.transform.IsChildOf(base.transform)
					}));
				}
			}
			Collider[] array = Physics.OverlapSphere(origin, this.detectionProfile.detectionRadius, this.detectionProfile.detectionLayers, QueryTriggerInteraction.Ignore);
			bool flag = false;
			foreach (Collider collider2 in array)
			{
				if (collider2 != null && collider2.CompareTag("Player"))
				{
					flag = true;
					break;
				}
			}
			stringBuilder.Append(string.Format("| playerOverlapsCastOrigin={0}", flag));
			if (flag)
			{
				Debug.LogError(stringBuilder.ToString() + "  ← SphereCast CANNOT see the player (origin sphere already overlaps it)!", this);
				return;
			}
			Debug.Log(stringBuilder.ToString(), this);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003CCC File Offset: 0x00001ECC
		private void AdvanceAlongPath(float speed, float dt)
		{
			if (this._pathLength < 0.001f)
			{
				return;
			}
			this._pathT += speed * dt / this._pathLength;
			if (!this._lookedAhead && this._pathT >= 0.85f)
			{
				this._lookedAhead = true;
				if (!this._track.isLoop)
				{
					Action<VehicleAI> onApproachingEnd = this.OnApproachingEnd;
					if (onApproachingEnd != null)
					{
						onApproachingEnd(this);
					}
				}
			}
			if (this._pathT >= 1f)
			{
				if (this._track.isLoop)
				{
					this._pathT -= 1f;
				}
				else
				{
					if (this._nextTrack != null)
					{
						this.BeginTransition(this._nextTrack);
						return;
					}
					this._pathT = 1f;
					this._currentSpeed = 0f;
					Action<VehicleAI> onEndReached = this.OnEndReached;
					if (onEndReached == null)
					{
						return;
					}
					onEndReached(this);
					return;
				}
			}
			this.SnapToPath(false);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003DB4 File Offset: 0x00001FB4
		private void BeginTransition(TrackData next)
		{
			TrackData track = this._track;
			Vector3 position = base.transform.position;
			Vector3 vector = ((this._travelDirection.sqrMagnitude > 1E-06f) ? this._travelDirection.normalized : track.GetWorldTangent(1f).normalized);
			Vector3 worldPosition = next.GetWorldPosition(0f);
			Vector3 normalized = next.GetWorldTangent(0f).normalized;
			float num = Vector3.Distance(position, worldPosition);
			float num2 = Vector3.Dot(vector, normalized);
			if (num < 0.05f && num2 > 0.995f)
			{
				if (VspDiag.Curve)
				{
					Debug.Log(string.Format("[CurveDiag] Transition DIRECT '{0}': '{1}' → '{2}' gap={3:F3} dot={4:F3} speed={5:F2} frame={6}", new object[]
					{
						base.name,
						track.name,
						next.name,
						num,
						num2,
						this._currentSpeed,
						Time.frameCount
					}));
				}
				this.AttachToTrack(next, 0f, true);
				Action<VehicleAI, TrackData> onTransitioned = this.OnTransitioned;
				if (onTransitioned == null)
				{
					return;
				}
				onTransitioned(this, track);
				return;
			}
			else
			{
				float num3 = Mathf.Max(num, 0.01f);
				this._blendP0 = position;
				this._blendP1 = worldPosition;
				this._blendM0 = vector * num3;
				this._blendM1 = normalized * num3;
				this._blendT = 0f;
				this._blendLength = this.ApproxBlendLength(8);
				this._state = VehicleAI.State.Blending;
				this._nextTrack = next;
				this._track = next;
				this._pathLength = next.GetLength();
				this._lookedAhead = false;
				if (VspDiag.Curve)
				{
					this._diagBlendStartTime = Time.time;
					this._diagBlendLogTimer = 0f;
					Debug.Log(string.Format("[CurveDiag] Transition BLEND '{0}': '{1}' → '{2}' gap={3:F2} dot={4:F3} blendLen={5:F2} speed={6:F2} curveLimit(frozen)={7} frame={8}", new object[]
					{
						base.name,
						track.name,
						next.name,
						num,
						num2,
						this._blendLength,
						this._currentSpeed,
						VehicleAI.FmtLimit(this._curveSpeedLimit),
						Time.frameCount
					}));
				}
				Action<VehicleAI, TrackData> onTransitioned2 = this.OnTransitioned;
				if (onTransitioned2 == null)
				{
					return;
				}
				onTransitioned2(this, track);
				return;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00004004 File Offset: 0x00002204
		private void AdvanceAlongBlend(float speed, float dt)
		{
			if (this._blendLength < 0.001f)
			{
				this.FinishBlend();
				return;
			}
			this._blendT += speed * dt / this._blendLength;
			if (this._blendT >= 1f)
			{
				this.FinishBlend();
				return;
			}
			Vector3 vector = this.HermitePoint(this._blendT);
			Vector3 vector2 = this.HermiteTangent(this._blendT);
			if (VspDiag.Curve)
			{
				this._diagBlendLogTimer -= dt;
				if (this._diagBlendLogTimer <= 0f)
				{
					this._diagBlendLogTimer = 0.25f;
					float num = Vector3.Dot(vector2, this._travelDirection);
					Debug.Log(string.Format("[CurveDiag] blend '{0}' t={1:F2} speed={2:F2} limit={3} curveLimit={4} pos={5:F1} tan·travel={6:F2}{7} frame={8}", new object[]
					{
						base.name,
						this._blendT,
						speed,
						VehicleAI.FmtLimit(this.EffectiveSpeedLimit),
						VehicleAI.FmtLimit(this._curveSpeedLimit),
						vector,
						num,
						(num < 0f) ? " ←TANGENT FLIP (loop in Hermite!)" : "",
						Time.frameCount
					}));
				}
			}
			this.ApplyPose(vector, vector2, false);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000413C File Offset: 0x0000233C
		private void FinishBlend()
		{
			if (VspDiag.Curve)
			{
				Debug.Log(string.Format("[CurveDiag] Blend END '{0}' → '{1}' duration={2:F2}s speed={3:F2} frame={4}", new object[]
				{
					base.name,
					(this._nextTrack != null) ? this._nextTrack.name : ((this._track != null) ? this._track.name : "null"),
					Time.time - this._diagBlendStartTime,
					this._currentSpeed,
					Time.frameCount
				}));
			}
			TrackData trackData = ((this._nextTrack != null) ? this._nextTrack : this._track);
			this.AttachToTrack(trackData, 0f, false);
			Vector3 worldPosition = this._track.GetWorldPosition(0f);
			Vector3 worldTangent = this._track.GetWorldTangent(0f);
			this.ApplyPose(worldPosition, worldTangent, false);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004234 File Offset: 0x00002434
		private Vector3 HermitePoint(float s)
		{
			float num = s * s;
			float num2 = num * s;
			float num3 = 2f * num2 - 3f * num + 1f;
			float num4 = num2 - 2f * num + s;
			float num5 = -2f * num2 + 3f * num;
			float num6 = num2 - num;
			return num3 * this._blendP0 + num4 * this._blendM0 + num5 * this._blendP1 + num6 * this._blendM1;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000042C0 File Offset: 0x000024C0
		private Vector3 HermiteTangent(float s)
		{
			float num = s * s;
			float num2 = 6f * num - 6f * s;
			float num3 = 3f * num - 4f * s + 1f;
			float num4 = -6f * num + 6f * s;
			float num5 = 3f * num - 2f * s;
			Vector3 vector = num2 * this._blendP0 + num3 * this._blendM0 + num4 * this._blendP1 + num5 * this._blendM1;
			if (vector.sqrMagnitude <= 1E-06f)
			{
				return this._travelDirection;
			}
			return vector.normalized;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004374 File Offset: 0x00002574
		private float ApproxBlendLength(int samples)
		{
			float num = 0f;
			Vector3 vector = this.HermitePoint(0f);
			for (int i = 1; i <= samples; i++)
			{
				Vector3 vector2 = this.HermitePoint((float)i / (float)samples);
				num += Vector3.Distance(vector, vector2);
				vector = vector2;
			}
			return num;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000043B8 File Offset: 0x000025B8
		private void SnapToPath(bool hardSnapRotation)
		{
			if (this._track == null)
			{
				return;
			}
			SplineContainer splineContainer = this._track.SplineContainer;
			float3 @float = splineContainer.Spline.EvaluatePosition(this._pathT);
			Vector3 vector = splineContainer.transform.TransformPoint(@float);
			float3 float2 = splineContainer.Spline.EvaluateTangent(this._pathT);
			Vector3 vector2 = splineContainer.transform.TransformDirection(math.normalize(float2));
			this.ApplyPose(vector, vector2, hardSnapRotation);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004434 File Offset: 0x00002634
		private void ApplyPose(Vector3 worldPos, Vector3 worldTangent, bool hardSnapRotation)
		{
			this._rb.MovePosition(worldPos);
			if (worldTangent.sqrMagnitude <= 0.001f)
			{
				return;
			}
			this._travelDirection = worldTangent.normalized;
			Quaternion quaternion = Quaternion.LookRotation(this._travelDirection, Vector3.up);
			if (this.rotationOffset != Vector3.zero)
			{
				quaternion *= Quaternion.Euler(this.rotationOffset);
			}
			if (hardSnapRotation)
			{
				this._rb.MoveRotation(quaternion);
				return;
			}
			float num = Mathf.Max(this._currentSpeed, 1f);
			float num2 = this.turnSpeed * 60f * Mathf.Clamp(num / Mathf.Max(this.cruiseSpeed, 0.1f), 0.3f, 1.5f) * Time.fixedDeltaTime;
			Quaternion quaternion2 = Quaternion.RotateTowards(this._rb.rotation, quaternion, num2);
			this._rb.MoveRotation(quaternion2);
		}

		// Token: 0x04000001 RID: 1
		private static readonly HashSet<VehicleAI> OriginInstances = new HashSet<VehicleAI>();

		// Token: 0x04000002 RID: 2
		[HideInInspector]
		public float cruiseSpeed;

		// Token: 0x04000003 RID: 3
		[HideInInspector]
		public float turnSpeed;

		// Token: 0x04000004 RID: 4
		[HideInInspector]
		public float vehicleLength;

		// Token: 0x04000005 RID: 5
		[HideInInspector]
		public DetectionProfile detectionProfile;

		// Token: 0x04000006 RID: 6
		[HideInInspector]
		public Vector3 rotationOffset;

		// Token: 0x04000007 RID: 7
		[HideInInspector]
		public Transform simLodPlayer;

		// Token: 0x04000008 RID: 8
		[HideInInspector]
		public float fullSimRadius = -1f;

		// Token: 0x04000009 RID: 9
		[Header("IDM — Intelligent Driver Model")]
		[Range(0.5f, 8f)]
		public float maxAcceleration = 2.5f;

		// Token: 0x0400000A RID: 10
		[Range(0.5f, 8f)]
		public float comfortDeceleration = 3.5f;

		// Token: 0x0400000B RID: 11
		[Range(0.2f, 5f)]
		public float minGap = 1.5f;

		// Token: 0x0400000C RID: 12
		[Range(0.5f, 3f)]
		public float timeHeadway = 1.2f;

		// Token: 0x0400000D RID: 13
		[Header("Cornering")]
		[Tooltip("Maximum comfortable lateral acceleration in corners (m/s²). Lower = slower, wider cornering. Curve speed limit = sqrt(maxLateralAccel / curvature).")]
		[Range(0.5f, 12f)]
		public float maxLateralAccel = 3f;

		// Token: 0x0400000E RID: 14
		[Header("Stuck Detection")]
		[Tooltip("Seconds a vehicle can stay stationary (without being in a zone) before it is despawned to clear jams.")]
		[Range(5f, 60f)]
		public float stuckTimeout = 15f;

		// Token: 0x0400000F RID: 15
		[HideInInspector]
		public bool ignoreOtherVehicles;

		// Token: 0x04000010 RID: 16
		[HideInInspector]
		public bool hasIntersectionPriority;

		// Token: 0x04000016 RID: 22
		private bool _hasPassenger;

		// Token: 0x04000017 RID: 23
		private static int _passengerCarCount;

		// Token: 0x04000018 RID: 24
		[HideInInspector]
		public Transform PassengerRoot;

		// Token: 0x04000019 RID: 25
		private VehicleAI.State _state;

		// Token: 0x0400001A RID: 26
		private TrackData _track;

		// Token: 0x0400001B RID: 27
		private float _pathT;

		// Token: 0x0400001C RID: 28
		private float _pathLength;

		// Token: 0x0400001D RID: 29
		private float _currentSpeed;

		// Token: 0x0400001E RID: 30
		private bool _blocked;

		// Token: 0x0400001F RID: 31
		private readonly HashSet<Object> _interactionStopOwners = new HashSet<Object>();

		// Token: 0x04000020 RID: 32
		private bool _deadEndAhead;

		// Token: 0x04000021 RID: 33
		private float _nextTrackLength;

		// Token: 0x04000022 RID: 34
		private float _speedZoneLimit = -1f;

		// Token: 0x04000023 RID: 35
		private bool _lookedAhead;

		// Token: 0x04000024 RID: 36
		private TrackData _nextTrack;

		// Token: 0x04000025 RID: 37
		private float _stuckTimer;

		// Token: 0x04000026 RID: 38
		private Rigidbody _rb;

		// Token: 0x04000027 RID: 39
		private Collider _bodyCollider;

		// Token: 0x04000028 RID: 40
		private Vector3 _travelDirection = Vector3.forward;

		// Token: 0x04000029 RID: 41
		private float _curveSpeedLimit = float.MaxValue;

		// Token: 0x0400002A RID: 42
		private float _curvatureTimer;

		// Token: 0x0400002B RID: 43
		private bool _staggerCurvatureCadence = true;

		// Token: 0x0400002C RID: 44
		private const float CurvatureRefreshInterval = 0.2f;

		// Token: 0x0400002D RID: 45
		private bool _fullSim = true;

		// Token: 0x0400002E RID: 46
		private float _lodTimer;

		// Token: 0x0400002F RID: 47
		private bool _staggerLodCadence = true;

		// Token: 0x04000030 RID: 48
		private const float LodRefreshInterval = 0.5f;

		// Token: 0x04000031 RID: 49
		private bool _courtesyStopped;

		// Token: 0x04000032 RID: 50
		private float _courtesyStopTimer;

		// Token: 0x04000033 RID: 51
		private float _courtesyCooldownTimer;

		// Token: 0x04000034 RID: 52
		private float _courtesyCheckTimer;

		// Token: 0x04000035 RID: 53
		private const float CourtesySideRange = 1f;

		// Token: 0x04000036 RID: 54
		private const float CourtesyStopDuration = 2f;

		// Token: 0x04000037 RID: 55
		private const float CourtesyCooldown = 5f;

		// Token: 0x04000038 RID: 56
		private const float CourtesyCheckInterval = 0.15f;

		// Token: 0x04000039 RID: 57
		private const float SoftStopGap = 2.5f;

		// Token: 0x0400003A RID: 58
		private Vector3 _blendP0;

		// Token: 0x0400003B RID: 59
		private Vector3 _blendP1;

		// Token: 0x0400003C RID: 60
		private Vector3 _blendM0;

		// Token: 0x0400003D RID: 61
		private Vector3 _blendM1;

		// Token: 0x0400003E RID: 62
		private float _blendLength;

		// Token: 0x0400003F RID: 63
		private float _blendT;

		// Token: 0x04000040 RID: 64
		private float _diagSenseLogTimer;

		// Token: 0x04000041 RID: 65
		private float _diagLodWarnTimer;

		// Token: 0x04000042 RID: 66
		private float _diagBlendStartTime;

		// Token: 0x04000043 RID: 67
		private float _diagBlendLogTimer;

		// Token: 0x04000044 RID: 68
		private float _diagLowSpeedTime;

		// Token: 0x04000045 RID: 69
		private bool _diagStallReported;

		// Token: 0x04000046 RID: 70
		private float _diagSpinWarnTimer;

		// Token: 0x04000047 RID: 71
		private float _diagSpinAccumDeg;

		// Token: 0x04000048 RID: 72
		private Vector3 _diagLastPos;

		// Token: 0x04000049 RID: 73
		private Quaternion _diagLastRot = Quaternion.identity;

		// Token: 0x0400004A RID: 74
		private bool _diagHasLastPose;

		// Token: 0x0400004B RID: 75
		private static readonly RaycastHit[] _hitBuffer = new RaycastHit[32];

		// Token: 0x0400004C RID: 76
		private static readonly Collider[] _originOverlapBuffer = new Collider[32];

		// Token: 0x0400004D RID: 77
		private const float LookAheadThreshold = 0.85f;

		// Token: 0x0400004E RID: 78
		private const float DirectTransitionDist = 0.05f;

		// Token: 0x0400004F RID: 79
		private const float DirectTransitionDot = 0.995f;

		// Token: 0x04000050 RID: 80
		private static readonly Dictionary<TrackData, List<VehicleAI>> s_vehiclesByTrack = new Dictionary<TrackData, List<VehicleAI>>();

		// Token: 0x04000051 RID: 81
		private TrackData _registeredTrack;

		// Token: 0x0200001A RID: 26
		private enum State
		{
			// Token: 0x040000DA RID: 218
			Following,
			// Token: 0x040000DB RID: 219
			Blending
		}
	}
}
