using System;
using System.Collections.Generic;
using Features.Audio;
using Features.Detection;
using Features.NPC;
using Features.RenderingPrecision;
using Invector.vCharacterController.vActions;
using UnityEngine;

// Token: 0x020000B7 RID: 183
[DisallowMultipleComponent]
public class NPCDiveController : MonoBehaviour
{
	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x00032F2C File Offset: 0x0003112C
	public NPCDiveController.DiveState State
	{
		get
		{
			return this._state;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x0600073D RID: 1853 RVA: 0x00032F34 File Offset: 0x00031134
	public bool IsBusy
	{
		get
		{
			return this._state > NPCDiveController.DiveState.Inactive;
		}
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00032F40 File Offset: 0x00031140
	public void Initialize(AStarNPCController controller, NPCWanderController wander)
	{
		this._controller = controller;
		this._wander = wander;
		this._animator = ((controller != null) ? controller.animator : base.GetComponentInChildren<Animator>());
		this._senses = base.GetComponentInChildren<NPCSenses>();
		this._initialized = this._controller != null && this._animator != null;
		this._state = NPCDiveController.DiveState.Inactive;
		CapsuleCollider componentInChildren = base.GetComponentInChildren<CapsuleCollider>();
		if (componentInChildren != null)
		{
			this._capsuleHeight = componentInChildren.height;
		}
		this._waterAudio = base.GetComponent<WaterSwimmerAudio>();
		if (this._waterAudio == null)
		{
			this._waterAudio = base.gameObject.AddComponent<WaterSwimmerAudio>();
		}
		this._waterAudio.SetUnderwaterLoopEnabled(false);
		this._waterAudio.SetSplashVolume(0.5f);
		this.ResolveWaterFXFromPlayer();
		if (!this._initialized)
		{
			Debug.LogWarning(string.Format("[NPCDiveController] Init incomplete on {0} (controller={1}, animator={2})", base.name, this._controller, this._animator));
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00033040 File Offset: 0x00031240
	private void ResolveWaterFXFromPlayer()
	{
		if (this.splashEffect != null && this.waterRingEffect != null)
		{
			return;
		}
		vSwimming vSwimming = Object.FindObjectOfType<vSwimming>();
		if (vSwimming == null)
		{
			return;
		}
		if (this.splashEffect == null)
		{
			this.splashEffect = vSwimming.impactEffect;
		}
		if (this.waterRingEffect == null)
		{
			this.waterRingEffect = vSwimming.waterRingEffect;
		}
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x000330AE File Offset: 0x000312AE
	public void AssignWaterFX(GameObject splash, GameObject ring, AudioClip sound)
	{
		if (splash != null)
		{
			this.splashEffect = splash;
		}
		if (ring != null)
		{
			this.waterRingEffect = ring;
		}
		if (sound != null)
		{
			this.splashSound = sound;
		}
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x000330E0 File Offset: 0x000312E0
	public void AssignWaterAudioClips(AudioClip splash, AudioClip underwater, AudioClip exit)
	{
		if (this._waterAudio == null)
		{
			this._waterAudio = base.GetComponent<WaterSwimmerAudio>();
		}
		if (this._waterAudio != null)
		{
			this._waterAudio.AssignClips(splash, underwater, exit);
		}
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00033118 File Offset: 0x00031318
	private float ComputeSurfaceY(float waterY)
	{
		return waterY - (this._capsuleHeight * 0.5f + this.heightOffset);
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00033130 File Offset: 0x00031330
	private float ResolveWaterlineY(float fallback)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(this.waterTag);
		if (array != null && array.Length != 0)
		{
			return array[0].transform.position.y;
		}
		return fallback;
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00033164 File Offset: 0x00031364
	public bool BeginDive(PoolDivePoint point)
	{
		if (!this._initialized || this.IsBusy || point == null)
		{
			return false;
		}
		PoolDiveLayout layout = point.Layout;
		if (layout == null)
		{
			Debug.LogWarning("[NPCDiveController] " + base.name + ": dive point has no PoolDiveLayout.");
			return false;
		}
		this._activePoint = point;
		this._diveStandPoint = point.DiveStandPosition;
		this._swimPath = layout.BuildSwimRoute(this._diveStandPoint);
		this._swimIndex = 0;
		this._activeClimbPoint = layout.PickClimbPoint(this._diveStandPoint);
		this._climbStandPoint = ((this._activeClimbPoint != null) ? this._activeClimbPoint.StandPosition : this._diveStandPoint);
		this.waterlineY = this.ResolveWaterlineY(layout.WaterlineY);
		this._surfaceY = this.ComputeSurfaceY(this.waterlineY);
		this._splashedThisDive = false;
		this._wander.ExternalControl = true;
		this._controller.SetDestination(this._diveStandPoint);
		this.SetState(NPCDiveController.DiveState.Approaching);
		return true;
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x00033270 File Offset: 0x00031470
	public void BeginInWater(Vector3 swimTarget, Vector3 climbStandPoint, float waterline)
	{
		if (!this._initialized || this.IsBusy)
		{
			return;
		}
		this._activePoint = null;
		this._swimPath = new List<Vector3> { swimTarget };
		this._swimIndex = 0;
		this._climbStandPoint = climbStandPoint;
		this.waterlineY = this.ResolveWaterlineY(waterline);
		this._surfaceY = this.ComputeSurfaceY(this.waterlineY);
		this._splashedThisDive = false;
		this._wander.ExternalControl = true;
		this._controller.EnterManualMode();
		this.SetState(NPCDiveController.DiveState.EnteringManualToSwim);
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x000332F9 File Offset: 0x000314F9
	private void Awake()
	{
		WorldOrigin.Shifted += this.OnWorldOriginShifted;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0003330C File Offset: 0x0003150C
	private void OnDestroy()
	{
		WorldOrigin.Shifted -= this.OnWorldOriginShifted;
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x0003331F File Offset: 0x0003151F
	private void OnEnable()
	{
		NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
		NPCSimulationPause.RegisterParticipant(this);
		this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00033344 File Offset: 0x00031544
	private void Update()
	{
		NPCRagdollRecovery component = base.GetComponent<NPCRagdollRecovery>();
		if (component != null && component.IsRecovering)
		{
			return;
		}
		if (NPCSimulationPause.IsPaused)
		{
			this.BeginSimulationPauseIfNeeded();
			return;
		}
		this.ResumeSimulationIfNeeded();
		float frameTimeLoss = NPCSimulationPause.FrameTimeLoss;
		if (frameTimeLoss > 0f)
		{
			this._nextAutoDiveCheck += frameTimeLoss;
		}
		if (!this._initialized)
		{
			return;
		}
		if (this._state == NPCDiveController.DiveState.Inactive)
		{
			if (this.autoDive)
			{
				this.TickAutoDive();
			}
			return;
		}
		switch (this._state)
		{
		case NPCDiveController.DiveState.Approaching:
			this.TickApproaching();
			return;
		case NPCDiveController.DiveState.EnteringManual:
			this.TickEnteringManual();
			return;
		case NPCDiveController.DiveState.EnteringManualToSwim:
			this.TickEnteringManualToSwim();
			return;
		case NPCDiveController.DiveState.Diving:
			this.TickDiving();
			return;
		case NPCDiveController.DiveState.Swimming:
			this.TickSwimming();
			return;
		case NPCDiveController.DiveState.Climbing:
			this.TickClimbing();
			return;
		case NPCDiveController.DiveState.WalkingOut:
			this.TickWalkingOut();
			return;
		case NPCDiveController.DiveState.Exiting:
			this.TickExiting();
			return;
		default:
			return;
		}
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00033424 File Offset: 0x00031624
	private void TickAutoDive()
	{
		if (this._wander == null || this._wander.ExternalControl)
		{
			return;
		}
		if (this._wander.CurrentState != NPCWanderController.WanderState.Idle)
		{
			return;
		}
		if (Time.time < this._nextAutoDiveCheck)
		{
			return;
		}
		this._nextAutoDiveCheck = Time.time + this.autoDiveCheckInterval;
		if (Random.value > this.autoDiveChance)
		{
			return;
		}
		PoolDivePoint poolDivePoint = PoolDivePoint.FindNearestFree(base.transform.position);
		if (poolDivePoint == null)
		{
			return;
		}
		if ((poolDivePoint.DiveStandPosition - base.transform.position).sqrMagnitude > this.autoDiveMaxDistance * this.autoDiveMaxDistance)
		{
			return;
		}
		if (poolDivePoint.TryReserve(this))
		{
			this.BeginDive(poolDivePoint);
		}
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x000334E4 File Offset: 0x000316E4
	private void TickApproaching()
	{
		if (this._controller.HasReachedDestination())
		{
			this._controller.EnterManualMode();
			this.SetState(NPCDiveController.DiveState.EnteringManual);
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x0600074C RID: 1868 RVA: 0x00033505 File Offset: 0x00031705
	private Vector3 CurrentSwimTarget
	{
		get
		{
			if (this._swimPath == null || this._swimIndex >= this._swimPath.Count)
			{
				return this._climbStandPoint;
			}
			return this._swimPath[this._swimIndex];
		}
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0003353A File Offset: 0x0003173A
	private void TickEnteringManual()
	{
		if (this._controller.IsManualModeReady)
		{
			this.FaceTowards(this.CurrentSwimTarget, true);
			this.CrossFade(this.diveState, 0.1f);
			this.SetState(NPCDiveController.DiveState.Diving);
		}
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x00033570 File Offset: 0x00031770
	private void TickDiving()
	{
		if (!this._splashedThisDive && base.transform.position.y + this._capsuleHeight * 0.5f <= this.waterlineY)
		{
			this.PlaySplash(new Vector3(base.transform.position.x, this.waterlineY, base.transform.position.z));
		}
		if (this.NormalizedTimeInState(this.diveState) >= this.diveHandoffNormalizedTime)
		{
			this.EnterSwimming();
		}
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x000335F8 File Offset: 0x000317F8
	private void PlaySplash(Vector3 atSurface)
	{
		this._splashedThisDive = true;
		if (this.splashEffect != null)
		{
			Object.Instantiate<GameObject>(this.splashEffect, atSurface, Quaternion.identity);
		}
		if (this._waterAudio != null)
		{
			this._waterAudio.PlaySplashAt(atSurface);
			return;
		}
		if (this.splashSound != null)
		{
			AudioSource.PlayClipAtPoint(this.splashSound, atSurface, this.splashVolume);
		}
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x00033668 File Offset: 0x00031868
	private void SpawnWaterRing()
	{
		if (this.waterRingEffect == null)
		{
			return;
		}
		this._waterRingTimer += NPCSimulationPause.SimulationDeltaTime;
		if (this._waterRingTimer < this.waterRingFrequencySwim)
		{
			return;
		}
		this._waterRingTimer = 0f;
		Vector3 vector = new Vector3(base.transform.position.x, this.waterlineY, base.transform.position.z);
		Object.Instantiate<GameObject>(this.waterRingEffect, vector, Quaternion.identity);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x000336EF File Offset: 0x000318EF
	private void TickEnteringManualToSwim()
	{
		if (this._controller.IsManualModeReady)
		{
			this.EnterSwimming();
		}
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x00033704 File Offset: 0x00031904
	private void EnterSwimming()
	{
		Vector3 position = base.transform.position;
		position.y = this._surfaceY;
		base.transform.position = position;
		if (!this._splashedThisDive)
		{
			this.PlaySplash(new Vector3(base.transform.position.x, this.waterlineY, base.transform.position.z));
		}
		this._swimIndex = 0;
		this._swimmingToClimb = false;
		this._waterRingTimer = 0f;
		this.SuspendSenses();
		this.SetWaterState(3);
		this.CrossFade(this.swimForwardState, 0.15f);
		this._controller.SuppressRootMotionPosition = true;
		if (this._waterAudio != null)
		{
			this._waterAudio.EnterWater();
		}
		this.SetState(NPCDiveController.DiveState.Swimming);
		if (this.debugLog)
		{
			Debug.Log(string.Format("[NPCDiveController] EnterSwimming: pos={0} swimPathCount={1} first={2}", base.transform.position, (this._swimPath != null) ? this._swimPath.Count : 0, this.CurrentSwimTarget));
		}
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00033820 File Offset: 0x00031A20
	private Vector3 ClimbApproachAtSurface()
	{
		Vector3 vector = ((this._activeClimbPoint != null) ? this._activeClimbPoint.SwimApproachPosition : this._climbStandPoint);
		return new Vector3(vector.x, this._surfaceY, vector.z);
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x00033868 File Offset: 0x00031A68
	private void TickSwimming()
	{
		Vector3 vector = (this._swimmingToClimb ? this.ClimbApproachAtSurface() : this.CurrentSwimTarget);
		Vector3 vector2 = base.transform.position;
		Vector3 vector3 = new Vector3(vector.x, this._surfaceY, vector.z);
		Vector3 vector4 = new Vector3(vector2.x, this._surfaceY, vector2.z);
		Vector3 vector5 = vector3 - vector4;
		this.FaceTowards(vector, false);
		this.SpawnWaterRing();
		vector2 = Vector3.MoveTowards(vector4, vector3, this.swimSpeed * NPCSimulationPause.SimulationDeltaTime);
		vector2.y = this._surfaceY;
		base.transform.position = vector2;
		if (vector5.sqrMagnitude > this.swimArrivalRadius * this.swimArrivalRadius)
		{
			return;
		}
		if (this._swimmingToClimb)
		{
			this.BeginClimb();
			return;
		}
		this._swimIndex++;
		if (this._swimPath == null || this._swimIndex >= this._swimPath.Count)
		{
			this._swimmingToClimb = true;
			if (this.debugLog)
			{
				Debug.Log("[NPCDiveController] swim waypoints done → swimming to climb approach");
				return;
			}
		}
		else if (this.debugLog)
		{
			Debug.Log(string.Format("[NPCDiveController] reached waypoint {0}/{1}", this._swimIndex, this._swimPath.Count));
		}
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x000339AC File Offset: 0x00031BAC
	private void BeginClimb()
	{
		this._climbTarget = ((this._activeClimbPoint != null) ? this._activeClimbPoint.MatchTarget : null);
		this._controller.SuppressRootMotionPosition = true;
		this.SetWaterState(0);
		this._climbStartPos = base.transform.position;
		this._finishXZ = (this._finishY = (this._finishRot = false));
		this.CrossFade(this.climbState, 0.1f);
		this.SetState(NPCDiveController.DiveState.Climbing);
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00033A30 File Offset: 0x00031C30
	private void TickClimbing()
	{
		if (this._climbTarget == null)
		{
			if (this.NormalizedTimeInState(this.climbState) >= this.climbExitNormalizedTime)
			{
				this.BeginWalkOut();
			}
			return;
		}
		float num = Mathf.Clamp01(this.NormalizedTimeInState(this.climbState));
		Vector3 vector = this._climbTarget.position;
		Transform transform = ((this._animator != null) ? this._animator.GetBoneTransform(HumanBodyBones.RightHand) : null);
		if (transform != null)
		{
			vector = this._climbTarget.position - base.transform.rotation * base.transform.InverseTransformPoint(transform.position);
		}
		Vector3 vector2 = this._climbTarget.InverseTransformPoint(vector);
		vector2.x = this._climbTarget.InverseTransformPoint(base.transform.position).x;
		vector = this._climbTarget.TransformPoint(vector2);
		Vector3 position = base.transform.position;
		float num2 = NPCDiveController.ClimbXZCurve.Evaluate(num);
		float num3 = NPCDiveController.ClimbYCurve.Evaluate(num);
		if (num2 < 1f)
		{
			position.x = Mathf.Lerp(position.x, vector.x, num2);
			position.z = Mathf.Lerp(position.z, vector.z, num2);
			this._finishXZ = true;
		}
		else if (this._finishXZ)
		{
			this._finishXZ = false;
			position.x = vector.x;
			position.z = vector.z;
		}
		if (num3 < 1f)
		{
			position.y = Mathf.Lerp(position.y, vector.y, num3);
			this._finishY = true;
		}
		else if (this._finishY)
		{
			this._finishY = false;
			position.y = vector.y;
		}
		base.transform.position = position;
		Quaternion quaternion = Quaternion.Euler(base.transform.eulerAngles.x, this._climbTarget.eulerAngles.y, base.transform.eulerAngles.z);
		float num4 = NPCDiveController.ClimbRotCurve.Evaluate(num);
		if (num4 < 1f)
		{
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, quaternion, num4);
			this._finishRot = true;
		}
		else if (this._finishRot)
		{
			this._finishRot = false;
			base.transform.rotation = quaternion;
		}
		if (num >= this.climbExitNormalizedTime)
		{
			this.BeginWalkOut();
		}
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00033CA8 File Offset: 0x00031EA8
	private void BeginWalkOut()
	{
		this._controller.SuppressRootMotionPosition = false;
		this.SetWaterState(0);
		if (this._waterAudio != null)
		{
			this._waterAudio.ExitWater();
		}
		this.CrossFade(this.locomotionState, 0.2f);
		this.SetState(NPCDiveController.DiveState.WalkingOut);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00033CFC File Offset: 0x00031EFC
	private void TickWalkingOut()
	{
		Vector3 climbStandPoint = this._climbStandPoint;
		Vector3 vector = climbStandPoint - base.transform.position;
		vector.y = 0f;
		if (vector.sqrMagnitude <= 0.09f)
		{
			this.SetState(NPCDiveController.DiveState.Exiting);
			return;
		}
		this.FaceTowards(climbStandPoint, false);
		float num = this.walkOutSpeed * NPCSimulationPause.SimulationDeltaTime;
		Vector3 vector2 = Vector3.MoveTowards(base.transform.position, new Vector3(climbStandPoint.x, base.transform.position.y, climbStandPoint.z), num);
		base.transform.position = vector2;
		this._controller.SetSpeedOverride(this.walkOutSpeed);
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00033DAC File Offset: 0x00031FAC
	private void TickExiting()
	{
		this.SetWaterState(0);
		this._controller.ClearSpeedOverride();
		this._controller.SuppressRootMotionPosition = false;
		this._controller.ExitManualMode(false);
		if (this._activePoint != null)
		{
			this._activePoint.Release(this);
			this._activePoint = null;
		}
		this._wander.ExternalControl = false;
		this.ResumeSenses();
		this.SetState(NPCDiveController.DiveState.Inactive);
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00033E20 File Offset: 0x00032020
	private void SuspendSenses()
	{
		if (this._senses == null || this._sensesSuspended)
		{
			return;
		}
		this._senses.CurrentAttention = 0f;
		this._senses.IsAlert = false;
		this._senses.IsDetected = false;
		this._senses.HasLineOfSight = false;
		this._senses.HasAudioContact = false;
		this._senses.IsPassiveReacting = false;
		this._senses.CurrentDetectionSource = DetectionSource.None;
		this._senses.enabled = false;
		this._sensesSuspended = true;
		if (this.debugLog)
		{
			Debug.Log("[NPCDiveController] " + base.name + ": senses suspended (in water).");
		}
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00033ED4 File Offset: 0x000320D4
	private void ResumeSenses()
	{
		if (this._senses == null || !this._sensesSuspended)
		{
			return;
		}
		this._senses.enabled = true;
		this._sensesSuspended = false;
		if (this.debugLog)
		{
			Debug.Log("[NPCDiveController] " + base.name + ": senses resumed (out of water).");
		}
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00033F2D File Offset: 0x0003212D
	private void OnDisable()
	{
		NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
		this.ResumeSenses();
		if (this._waterAudio != null)
		{
			this._waterAudio.ExitWater();
		}
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x00033F60 File Offset: 0x00032160
	public void CancelForWaterRecovery()
	{
		if (this._activePoint != null)
		{
			this._activePoint.Release(this);
			this._activePoint = null;
		}
		if (this._controller != null)
		{
			this._controller.SuppressRootMotionPosition = false;
			this._controller.ClearSpeedOverride();
			this._controller.ExitManualMode(true);
		}
		if (this._wander != null)
		{
			this._wander.ExternalControl = false;
		}
		this.ResumeSenses();
		if (this._waterAudio != null)
		{
			this._waterAudio.ExitWater();
		}
		this.SetWaterState(0);
		this.SetState(NPCDiveController.DiveState.Inactive);
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00034008 File Offset: 0x00032208
	private void OnWorldOriginShifted(Vector3 delta)
	{
		if (!this.IsBusy)
		{
			return;
		}
		this._diveStandPoint += delta;
		this._climbStandPoint += delta;
		this._climbStartPos += delta;
		this._surfaceY += delta.y;
		this.waterlineY += delta.y;
		if (this._swimPath != null)
		{
			for (int i = 0; i < this._swimPath.Count; i++)
			{
				List<Vector3> swimPath = this._swimPath;
				int num = i;
				swimPath[num] += delta;
			}
		}
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x000340B5 File Offset: 0x000322B5
	private void HandleSimulationPauseChanged(bool paused)
	{
		if (paused)
		{
			this.BeginSimulationPauseIfNeeded();
			return;
		}
		this.ResumeSimulationIfNeeded();
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x000340C7 File Offset: 0x000322C7
	private void BeginSimulationPauseIfNeeded()
	{
		if (this._simulationPauseActive)
		{
			return;
		}
		this._simulationPauseActive = true;
		this._simulationPauseStartedAt = Time.time;
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x000340E4 File Offset: 0x000322E4
	private void ResumeSimulationIfNeeded()
	{
		if (!this._simulationPauseActive)
		{
			return;
		}
		this._nextAutoDiveCheck += Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
		this._simulationPauseActive = false;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0003411C File Offset: 0x0003231C
	private void FaceTowards(Vector3 worldPoint, bool instant)
	{
		Vector3 vector = worldPoint - base.transform.position;
		vector.y = 0f;
		if (vector.sqrMagnitude < 0.0001f)
		{
			return;
		}
		Quaternion quaternion = Quaternion.LookRotation(vector);
		base.transform.rotation = (instant ? quaternion : Quaternion.RotateTowards(base.transform.rotation, quaternion, this.swimTurnSpeed * NPCSimulationPause.SimulationDeltaTime));
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0003418B File Offset: 0x0003238B
	private void CrossFade(string stateName, float dur)
	{
		if (this._animator != null && !string.IsNullOrEmpty(stateName))
		{
			this._animator.CrossFadeInFixedTime(stateName, dur, 0);
		}
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x000341B4 File Offset: 0x000323B4
	private float NormalizedTimeInState(string stateName)
	{
		if (this._animator == null)
		{
			return 0f;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(0);
		if (!currentAnimatorStateInfo.IsName(stateName))
		{
			return 0f;
		}
		return currentAnimatorStateInfo.normalizedTime;
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x000341F9 File Offset: 0x000323F9
	private void SetWaterState(int value)
	{
		if (this._animator != null && !string.IsNullOrEmpty(this.waterStateParam))
		{
			this._animator.SetInteger(this.waterStateParam, value);
		}
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00034228 File Offset: 0x00032428
	private void SetState(NPCDiveController.DiveState s)
	{
		if (this.debugLog)
		{
			Debug.Log(string.Format("[NPCDiveController] {0}: {1} → {2}", base.name, this._state, s));
		}
		this._state = s;
	}

	// Token: 0x04000617 RID: 1559
	[Header("State (read-only)")]
	[SerializeField]
	private NPCDiveController.DiveState _state;

	// Token: 0x04000618 RID: 1560
	[Header("Water")]
	[Tooltip("Y della superficie del collider acqua (tag 'Water'). Auto-rilevata dal collider se presente; altrimenti override per-dive dal PoolDivePoint, altrimenti questo default.")]
	[SerializeField]
	private float waterlineY = -5.66f;

	// Token: 0x04000619 RID: 1561
	[Tooltip("Offset extra sotto la superficie (come vSwimming.heightOffset del player). Più alto = galleggia più in basso.")]
	[SerializeField]
	private float heightOffset = 0.55f;

	// Token: 0x0400061A RID: 1562
	[Tooltip("Tag del collider acqua da cui leggere la quota della superficie (come il player).")]
	[SerializeField]
	private string waterTag = "Water";

	// Token: 0x0400061B RID: 1563
	[Header("Swim")]
	[SerializeField]
	private float swimSpeed = 0.9f;

	// Token: 0x0400061C RID: 1564
	[SerializeField]
	private float swimTurnSpeed = 240f;

	// Token: 0x0400061D RID: 1565
	[SerializeField]
	private float swimArrivalRadius = 0.6f;

	// Token: 0x0400061E RID: 1566
	[Tooltip("Velocità di camminata (m/s) dal bordo allo stand point dopo il climb (senza A*).")]
	[SerializeField]
	private float walkOutSpeed = 1.2f;

	// Token: 0x0400061F RID: 1567
	[Header("Handoff thresholds (normalized clip time)")]
	[SerializeField]
	private float diveHandoffNormalizedTime = 0.85f;

	// Token: 0x04000620 RID: 1568
	[SerializeField]
	private float climbExitNormalizedTime = 0.98f;

	// Token: 0x04000621 RID: 1569
	[Header("Autonomous trigger")]
	[Tooltip("If true, the NPC will occasionally dive on its own when idle near a free PoolDivePoint.")]
	[SerializeField]
	private bool autoDive = true;

	// Token: 0x04000622 RID: 1570
	[Tooltip("Average seconds between dive attempts while idle.")]
	[SerializeField]
	private float autoDiveCheckInterval = 8f;

	// Token: 0x04000623 RID: 1571
	[Tooltip("Probability [0..1] a given check actually starts a dive.")]
	[SerializeField]
	private float autoDiveChance = 0.35f;

	// Token: 0x04000624 RID: 1572
	[Tooltip("Only consider dive points within this distance of the NPC.")]
	[SerializeField]
	private float autoDiveMaxDistance = 25f;

	// Token: 0x04000625 RID: 1573
	private float _nextAutoDiveCheck;

	// Token: 0x04000626 RID: 1574
	[Header("Effetti acqua (riusa i prefab/audio del player — assegnabili a mano o auto da vSwimming)")]
	[Tooltip("Splash al tuffo (Invector impactWaterFX).")]
	[SerializeField]
	private GameObject splashEffect;

	// Token: 0x04000627 RID: 1575
	[Tooltip("Anello d'acqua periodico mentre nuota (Invector impactWaterRing).")]
	[SerializeField]
	private GameObject waterRingEffect;

	// Token: 0x04000628 RID: 1576
	[Tooltip("Suono del tuffo (Invector waterSplash).")]
	[SerializeField]
	private AudioClip splashSound;

	// Token: 0x04000629 RID: 1577
	[SerializeField]
	[Range(0f, 1f)]
	private float splashVolume = 1f;

	// Token: 0x0400062A RID: 1578
	[Tooltip("Frequenza (s) di spawn degli anelli d'acqua mentre nuota.")]
	[SerializeField]
	private float waterRingFrequencySwim = 0.3f;

	// Token: 0x0400062B RID: 1579
	[Header("Animator state names (on Pool_Master_Controller)")]
	[SerializeField]
	private string diveState = "PoolDive";

	// Token: 0x0400062C RID: 1580
	[SerializeField]
	private string swimForwardState = "SwimForward";

	// Token: 0x0400062D RID: 1581
	[SerializeField]
	private string climbState = "PoolClimb";

	// Token: 0x0400062E RID: 1582
	[SerializeField]
	private string locomotionState = "Normal Movement";

	// Token: 0x0400062F RID: 1583
	[SerializeField]
	private string waterStateParam = "WaterState";

	// Token: 0x04000630 RID: 1584
	[Header("Debug")]
	public bool debugLog;

	// Token: 0x04000631 RID: 1585
	private AStarNPCController _controller;

	// Token: 0x04000632 RID: 1586
	private NPCWanderController _wander;

	// Token: 0x04000633 RID: 1587
	private Animator _animator;

	// Token: 0x04000634 RID: 1588
	private NPCSenses _senses;

	// Token: 0x04000635 RID: 1589
	private bool _sensesSuspended;

	// Token: 0x04000636 RID: 1590
	private Vector3 _diveStandPoint;

	// Token: 0x04000637 RID: 1591
	private List<Vector3> _swimPath;

	// Token: 0x04000638 RID: 1592
	private int _swimIndex;

	// Token: 0x04000639 RID: 1593
	private PoolClimbPoint _activeClimbPoint;

	// Token: 0x0400063A RID: 1594
	private Vector3 _climbStandPoint;

	// Token: 0x0400063B RID: 1595
	private bool _swimmingToClimb;

	// Token: 0x0400063C RID: 1596
	private float _surfaceY;

	// Token: 0x0400063D RID: 1597
	private Vector3 _climbStartPos;

	// Token: 0x0400063E RID: 1598
	private PoolDivePoint _activePoint;

	// Token: 0x0400063F RID: 1599
	private float _capsuleHeight = 1.8f;

	// Token: 0x04000640 RID: 1600
	private bool _splashedThisDive;

	// Token: 0x04000641 RID: 1601
	private float _waterRingTimer;

	// Token: 0x04000642 RID: 1602
	private AudioSource _audio;

	// Token: 0x04000643 RID: 1603
	private WaterSwimmerAudio _waterAudio;

	// Token: 0x04000644 RID: 1604
	private bool _initialized;

	// Token: 0x04000645 RID: 1605
	private bool _simulationPauseActive;

	// Token: 0x04000646 RID: 1606
	private float _simulationPauseStartedAt;

	// Token: 0x04000647 RID: 1607
	private static readonly AnimationCurve ClimbXZCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.6026f, 1f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x04000648 RID: 1608
	private static readonly AnimationCurve ClimbYCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.4523f, 1.0017544f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x04000649 RID: 1609
	private static readonly AnimationCurve ClimbRotCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, 1f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x0400064A RID: 1610
	private Transform _climbTarget;

	// Token: 0x0400064B RID: 1611
	private bool _finishXZ;

	// Token: 0x0400064C RID: 1612
	private bool _finishY;

	// Token: 0x0400064D RID: 1613
	private bool _finishRot;

	// Token: 0x020000B8 RID: 184
	public enum DiveState
	{
		// Token: 0x0400064F RID: 1615
		Inactive,
		// Token: 0x04000650 RID: 1616
		Approaching,
		// Token: 0x04000651 RID: 1617
		EnteringManual,
		// Token: 0x04000652 RID: 1618
		EnteringManualToSwim,
		// Token: 0x04000653 RID: 1619
		Diving,
		// Token: 0x04000654 RID: 1620
		Swimming,
		// Token: 0x04000655 RID: 1621
		Climbing,
		// Token: 0x04000656 RID: 1622
		WalkingOut,
		// Token: 0x04000657 RID: 1623
		Exiting
	}
}
