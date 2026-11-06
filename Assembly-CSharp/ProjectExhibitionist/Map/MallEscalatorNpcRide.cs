using System;
using System.Collections;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using UnityEngine;

namespace ProjectExhibitionist.Map
{
	// Token: 0x020009C5 RID: 2501
	[DisallowMultipleComponent]
	public sealed class MallEscalatorNpcRide : MonoBehaviour
	{
		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004666 RID: 18022 RVA: 0x001AA49C File Offset: 0x001A869C
		public bool IsBusy
		{
			get
			{
				return this._busy;
			}
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x001AA4A4 File Offset: 0x001A86A4
		public bool Begin(MallEscalatorNpcRecruiter recruiter, MallEscalatorConveyor conveyor)
		{
			if (this._busy || recruiter == null || conveyor == null || conveyor.WalkSurface == null)
			{
				return false;
			}
			if (!this.CacheAndValidateNpc())
			{
				return false;
			}
			this._recruiter = recruiter;
			this._conveyor = conveyor;
			this._entry = (conveyor.GoesDown ? conveyor.UpperWorldPoint : conveyor.LowerWorldPoint);
			this._exit = (conveyor.GoesDown ? conveyor.LowerWorldPoint : conveyor.UpperWorldPoint);
			this._busy = true;
			this._finishing = false;
			this._wander.ExternalControl = true;
			this._ownsExternalControl = true;
			this._wander.ForceStop();
			this._routine = base.StartCoroutine(this.RunSequence());
			return true;
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x001AA56C File Offset: 0x001A876C
		private bool CacheAndValidateNpc()
		{
			this._fsm = base.GetComponent<NPCStateMachine>();
			this._wander = base.GetComponent<NPCWanderController>();
			this._controller = base.GetComponent<AStarNPCController>();
			this._senses = base.GetComponent<NPCSenses>();
			return this._fsm != null && this._fsm.IsInitialized && (this._fsm.CurrentStateType == NPCStateType.Idle || this._fsm.CurrentStateType == NPCStateType.Wandering) && this._wander != null && this._wander.isActiveAndEnabled && !this._wander.ExternalControl && this._controller != null && this._controller.isActiveAndEnabled && !this._controller.SuppressPOIBehavior && (this._senses == null || (!this._senses.IsAlert && !this._senses.IsDetected));
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x001AA667 File Offset: 0x001A8867
		private IEnumerator RunSequence()
		{
			float deadline = Time.time + 2f;
			while (!this._controller.IsNavigationReady && Time.time < deadline)
			{
				if (!this.CanContinue())
				{
					this.Finish();
					yield break;
				}
				yield return null;
			}
			if (!this.CanContinue())
			{
				this.Finish();
				yield break;
			}
			this._controller.UnlockNavigation(false);
			Vector3 rideDirection = MallEscalatorNpcRide.HorizontalDirection(this._entry, this._exit);
			Vector3 approach = this._entry - rideDirection * 1f;
			this._controller.SetDestination(approach);
			deadline = Time.time + 30f;
			while (Time.time < deadline)
			{
				if (!this.CanContinue())
				{
					this.Finish();
					yield break;
				}
				if (MallEscalatorNpcRide.HorizontalDistance(base.transform.position, approach) <= 0.85f || this._controller.HasReachedDestination())
				{
					break;
				}
				yield return null;
			}
			if (!this.CanContinue() || MallEscalatorNpcRide.HorizontalDistance(base.transform.position, approach) > 2.125f)
			{
				this.Finish();
				yield break;
			}
			this._controller.StopAndLock();
			this._controller.EnterManualMode();
			deadline = Time.time + 2f;
			while (!this._controller.IsManualModeReady && Time.time < deadline)
			{
				if (!this.StillStructurallyValid())
				{
					this.Finish();
					yield break;
				}
				yield return null;
			}
			this._controller.SuppressRootMotionPosition = true;
			this._rootMotionSuppressed = true;
			yield return this.MoveManualTo(this._entry, 1.25f, true);
			if (!this._busy)
			{
				yield break;
			}
			this._controller.SetSpeedOverride(0f);
			Vector3 vector = this._exit - this._entry;
			Vector3 vector2 = vector;
			vector2.y = 0f;
			float horizontalLength = Mathf.Max(0.1f, vector2.magnitude);
			float horizontalSpeed = this._conveyor.Speed * (horizontalLength / Mathf.Max(0.1f, vector.magnitude));
			float travelled = 0f;
			while (travelled < horizontalLength)
			{
				if (!this.StillStructurallyValid())
				{
					this.Finish();
					yield break;
				}
				if (NPCSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					travelled += horizontalSpeed * Time.deltaTime;
					float num = Mathf.Clamp01(travelled / horizontalLength);
					Vector3 vector3 = this._entry + rideDirection * (horizontalLength * num);
					vector3.y = this.SampleRampHeight(vector3, Mathf.Lerp(this._entry.y, this._exit.y, num));
					base.transform.position = vector3;
					this.FaceDirection(rideDirection);
					yield return null;
				}
			}
			Vector3 vector4 = this._exit + rideDirection * 1.15f;
			vector4.y = this._exit.y;
			yield return this.MoveManualTo(vector4, 1.25f, false);
			if (!this._busy)
			{
				yield break;
			}
			this.Finish();
			yield break;
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x001AA676 File Offset: 0x001A8876
		private IEnumerator MoveManualTo(Vector3 target, float speed, bool snapToRamp)
		{
			this._controller.SetSpeedOverride(speed);
			float safetyDeadline = Time.time + 12f;
			while (Time.time < safetyDeadline)
			{
				if (!this.StillStructurallyValid())
				{
					this.Finish();
					yield break;
				}
				if (NPCSimulationPause.IsPaused)
				{
					yield return null;
				}
				else
				{
					Vector3 vector = Vector3.MoveTowards(base.transform.position, target, speed * Time.deltaTime);
					if (snapToRamp)
					{
						vector.y = this.SampleRampHeight(vector, vector.y);
					}
					base.transform.position = vector;
					Vector3 vector2 = target - base.transform.position;
					vector2.y = 0f;
					if (vector2.sqrMagnitude > 0.0004f)
					{
						this.FaceDirection(vector2.normalized);
					}
					if (MallEscalatorNpcRide.HorizontalDistance(base.transform.position, target) <= 0.05f)
					{
						yield break;
					}
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x001AA69C File Offset: 0x001A889C
		private float SampleRampHeight(Vector3 position, float fallback)
		{
			Collider collider = ((this._conveyor != null) ? this._conveyor.WalkSurface : null);
			if (collider == null)
			{
				return fallback;
			}
			Vector3 vector = new Vector3(position.x, collider.bounds.max.y + 1.5f, position.z);
			RaycastHit raycastHit;
			if (collider.Raycast(new Ray(vector, Vector3.down), out raycastHit, collider.bounds.size.y + 4f))
			{
				return raycastHit.point.y;
			}
			return fallback;
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x001AA73C File Offset: 0x001A893C
		private void FaceDirection(Vector3 horizontalDirection)
		{
			if (horizontalDirection.sqrMagnitude < 0.0001f)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(horizontalDirection, Vector3.up);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, 300f * Time.deltaTime);
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x001AA78C File Offset: 0x001A898C
		private static Vector3 HorizontalDirection(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			vector.y = 0f;
			if (vector.sqrMagnitude <= 0.0001f)
			{
				return Vector3.forward;
			}
			return vector.normalized;
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x001AA7C8 File Offset: 0x001A89C8
		private static float HorizontalDistance(Vector3 a, Vector3 b)
		{
			a.y = 0f;
			b.y = 0f;
			return Vector3.Distance(a, b);
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x001AA7EC File Offset: 0x001A89EC
		private bool CanContinue()
		{
			if (!this.StillStructurallyValid())
			{
				return false;
			}
			NPCStateType currentStateType = this._fsm.CurrentStateType;
			return (currentStateType == NPCStateType.Idle || currentStateType == NPCStateType.Wandering || currentStateType == NPCStateType.LookingAt) && (this._senses == null || (!this._senses.IsAlert && !this._senses.IsDetected));
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x001AA854 File Offset: 0x001A8A54
		private bool StillStructurallyValid()
		{
			return this._busy && base.isActiveAndEnabled && this._fsm != null && this._fsm.IsInitialized && this._controller != null && this._controller.isActiveAndEnabled && this._wander != null && this._wander.isActiveAndEnabled && this._conveyor != null;
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x001AA8D0 File Offset: 0x001A8AD0
		private void Finish()
		{
			if (!this._busy || this._finishing)
			{
				return;
			}
			this._finishing = true;
			if (this._routine != null)
			{
				base.StopCoroutine(this._routine);
				this._routine = null;
			}
			if (this._controller != null)
			{
				if (this._rootMotionSuppressed)
				{
					this._controller.SuppressRootMotionPosition = false;
				}
				this._controller.ClearSpeedOverride();
				if (this._controller.IsManualMode)
				{
					this._controller.ExitManualMode(true);
				}
				this._controller.UnlockNavigation(true);
			}
			this._rootMotionSuppressed = false;
			if (this._wander != null && this._ownsExternalControl)
			{
				this._wander.ExternalControl = false;
			}
			this._ownsExternalControl = false;
			bool flag = this._fsm == null || !this._fsm.IsInitialized || this._fsm.CurrentStateType == NPCStateType.Idle || this._fsm.CurrentStateType == NPCStateType.Wandering || this._fsm.CurrentStateType == NPCStateType.LookingAt;
			if (flag && this._controller != null)
			{
				this._controller.ResetToDefault();
			}
			if (flag && this._wander != null)
			{
				this._wander.ResumeWandering();
			}
			MallEscalatorNpcRecruiter recruiter = this._recruiter;
			this._recruiter = null;
			this._conveyor = null;
			this._busy = false;
			this._finishing = false;
			if (recruiter == null)
			{
				return;
			}
			recruiter.NotifyRideFinished(this);
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x001AAA41 File Offset: 0x001A8C41
		private void OnDisable()
		{
			if (this._busy)
			{
				this.Finish();
			}
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x001AAA41 File Offset: 0x001A8C41
		private void OnDestroy()
		{
			if (this._busy)
			{
				this.Finish();
			}
		}

		// Token: 0x040045FB RID: 17915
		private const float ApproachTimeout = 30f;

		// Token: 0x040045FC RID: 17916
		private const float ApproachStandoff = 1f;

		// Token: 0x040045FD RID: 17917
		private const float ApproachReachDistance = 0.85f;

		// Token: 0x040045FE RID: 17918
		private const float BoardingWalkSpeed = 1.25f;

		// Token: 0x040045FF RID: 17919
		private const float LandingOvershoot = 1.15f;

		// Token: 0x04004600 RID: 17920
		private const float ManualPhaseSafetySeconds = 12f;

		// Token: 0x04004601 RID: 17921
		private const float RotationDegreesPerSecond = 300f;

		// Token: 0x04004602 RID: 17922
		private NPCStateMachine _fsm;

		// Token: 0x04004603 RID: 17923
		private NPCWanderController _wander;

		// Token: 0x04004604 RID: 17924
		private AStarNPCController _controller;

		// Token: 0x04004605 RID: 17925
		private NPCSenses _senses;

		// Token: 0x04004606 RID: 17926
		private MallEscalatorNpcRecruiter _recruiter;

		// Token: 0x04004607 RID: 17927
		private MallEscalatorConveyor _conveyor;

		// Token: 0x04004608 RID: 17928
		private Coroutine _routine;

		// Token: 0x04004609 RID: 17929
		private Vector3 _entry;

		// Token: 0x0400460A RID: 17930
		private Vector3 _exit;

		// Token: 0x0400460B RID: 17931
		private bool _busy;

		// Token: 0x0400460C RID: 17932
		private bool _finishing;

		// Token: 0x0400460D RID: 17933
		private bool _ownsExternalControl;

		// Token: 0x0400460E RID: 17934
		private bool _rootMotionSuppressed;
	}
}
