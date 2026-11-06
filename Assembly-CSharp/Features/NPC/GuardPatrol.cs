using System;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EC5 RID: 3781
	[DisallowMultipleComponent]
	public class GuardPatrol : MonoBehaviour
	{
		// Token: 0x06007133 RID: 28979 RVA: 0x002DF45D File Offset: 0x002DD65D
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInChildren<Animator>();
			}
			this.CacheSpeedParam();
		}

		// Token: 0x06007134 RID: 28980 RVA: 0x002DF480 File Offset: 0x002DD680
		private void OnEnable()
		{
			this._simulationPauseActive = false;
			this._startTime = Time.time + this.initialDelay;
			this._state = GuardPatrol.State.Idle;
			this._idleUntil = this._startTime;
			this._started = false;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			NPCSimulationPause.RegisterParticipant(this);
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x06007135 RID: 28981 RVA: 0x002DF4E2 File Offset: 0x002DD6E2
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			this._currentAnimSpeed = 0f;
			this.ApplySpeed(0f);
		}

		// Token: 0x06007136 RID: 28982 RVA: 0x002DF50C File Offset: 0x002DD70C
		private void Update()
		{
			if (NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			float frameTimeLoss = NPCSimulationPause.FrameTimeLoss;
			if (frameTimeLoss > 0f)
			{
				this._startTime += frameTimeLoss;
				this._idleUntil += frameTimeLoss;
			}
			if (this.waypoints == null || this.waypoints.Length < 2)
			{
				return;
			}
			if (Time.time < this._startTime)
			{
				return;
			}
			if (!this._started)
			{
				this._started = true;
				this.AdvanceTarget();
				this._state = GuardPatrol.State.Moving;
			}
			float num = Mathf.Min(NPCSimulationPause.SimulationDeltaTime, 0.1f);
			GuardPatrol.State state = this._state;
			if (state != GuardPatrol.State.Idle)
			{
				if (state == GuardPatrol.State.Moving)
				{
					this.TickMoving(num);
				}
			}
			else
			{
				this.TickIdle(num);
			}
			this.UpdateAnimatorSpeed(num);
			if (this._state == GuardPatrol.State.Moving && this._currentAnimSpeed > 0f && this._moveTarget != null)
			{
				Vector3 vector = new Vector3(this._moveTarget.position.x, base.transform.position.y, this._moveTarget.position.z);
				base.transform.position = Vector3.MoveTowards(base.transform.position, vector, this._currentAnimSpeed * num);
			}
		}

		// Token: 0x06007137 RID: 28983 RVA: 0x002DF649 File Offset: 0x002DD849
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x06007138 RID: 28984 RVA: 0x002DF65B File Offset: 0x002DD85B
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x06007139 RID: 28985 RVA: 0x002DF678 File Offset: 0x002DD878
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			float num = Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
			this._startTime += num;
			this._idleUntil += num;
			this._simulationPauseActive = false;
		}

		// Token: 0x0600713A RID: 28986 RVA: 0x002DF6C8 File Offset: 0x002DD8C8
		private void TickMoving(float dt)
		{
			Transform transform = this.CurrentWaypoint();
			this._moveTarget = transform;
			if (transform == null)
			{
				this._state = GuardPatrol.State.Idle;
				this._idleUntil = Time.time + 1f;
				this._targetAnimSpeed = 0f;
				return;
			}
			Vector3 vector = transform.position - base.transform.position;
			vector.y = 0f;
			if (vector.magnitude <= this.arriveDistance)
			{
				float num = Random.Range(this.idleTimeRange.x, this.idleTimeRange.y);
				this._idleUntil = Time.time + num;
				this._state = GuardPatrol.State.Idle;
				this._targetAnimSpeed = 0f;
				this._moveTarget = null;
				if (this.debugLog)
				{
					Debug.Log(string.Format("[GuardPatrol] {0}: arrivato a {1}, idle {2:F1}s", base.name, transform.name, num));
				}
				return;
			}
			this._targetAnimSpeed = this.moveSpeed;
			if (vector.sqrMagnitude > 0.0001f)
			{
				Quaternion quaternion = Quaternion.LookRotation(vector.normalized, Vector3.up);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, this.rotationSpeed * dt);
			}
		}

		// Token: 0x0600713B RID: 28987 RVA: 0x002DF800 File Offset: 0x002DDA00
		private void TickIdle(float dt)
		{
			this._targetAnimSpeed = 0f;
			this._moveTarget = null;
			if (this.faceWaypointForwardOnIdle)
			{
				Transform transform = this.CurrentWaypoint();
				if (transform != null)
				{
					Vector3 forward = transform.forward;
					forward.y = 0f;
					if (forward.sqrMagnitude > 0.0001f)
					{
						Quaternion quaternion = Quaternion.LookRotation(forward.normalized, Vector3.up);
						base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, this.rotationSpeed * dt);
					}
				}
			}
			if (Time.time < this._idleUntil)
			{
				return;
			}
			this.AdvanceTarget();
			this._state = GuardPatrol.State.Moving;
		}

		// Token: 0x0600713C RID: 28988 RVA: 0x002DF8AC File Offset: 0x002DDAAC
		private void UpdateAnimatorSpeed(float dt)
		{
			this._currentAnimSpeed = Mathf.Lerp(this._currentAnimSpeed, this._targetAnimSpeed, 1f - Mathf.Exp(-this.speedDamp * dt));
			if (this._targetAnimSpeed <= 0f && this._currentAnimSpeed < 0.01f)
			{
				this._currentAnimSpeed = 0f;
			}
			this.ApplySpeed(this._currentAnimSpeed);
		}

		// Token: 0x0600713D RID: 28989 RVA: 0x002DF915 File Offset: 0x002DDB15
		private void ApplySpeed(float worldSpeed)
		{
			if (!this._hasSpeedParam)
			{
				return;
			}
			this.animator.SetFloat(this._speedHash, worldSpeed);
		}

		// Token: 0x0600713E RID: 28990 RVA: 0x002DF934 File Offset: 0x002DDB34
		private void AdvanceTarget()
		{
			int num = this.waypoints.Length;
			if (num == 0)
			{
				this._currentIndex = -1;
				return;
			}
			switch (this.mode)
			{
			case GuardPatrol.PatrolMode.PingPong:
				if (this._currentIndex < 0)
				{
					this._currentIndex = 0;
				}
				else
				{
					if (this._currentIndex >= num - 1)
					{
						this._direction = -1;
					}
					else if (this._currentIndex <= 0)
					{
						this._direction = 1;
					}
					this._currentIndex = Mathf.Clamp(this._currentIndex + this._direction, 0, num - 1);
				}
				break;
			case GuardPatrol.PatrolMode.Loop:
				this._currentIndex = (this._currentIndex + 1) % num;
				break;
			case GuardPatrol.PatrolMode.Random:
				if (num == 1)
				{
					this._currentIndex = 0;
				}
				else
				{
					int num2;
					do
					{
						num2 = Random.Range(0, num);
					}
					while (num2 == this._currentIndex);
					this._currentIndex = num2;
				}
				break;
			}
			if (this.debugLog)
			{
				string text = "[GuardPatrol] {0}: prossimo waypoint #{1} ({2})";
				object name = base.name;
				object obj = this._currentIndex;
				Transform transform = this.CurrentWaypoint();
				Debug.Log(string.Format(text, name, obj, (transform != null) ? transform.name : null));
			}
		}

		// Token: 0x0600713F RID: 28991 RVA: 0x002DFA38 File Offset: 0x002DDC38
		private Transform CurrentWaypoint()
		{
			if (this.waypoints == null || this._currentIndex < 0 || this._currentIndex >= this.waypoints.Length)
			{
				return null;
			}
			return this.waypoints[this._currentIndex];
		}

		// Token: 0x06007140 RID: 28992 RVA: 0x002DFA6C File Offset: 0x002DDC6C
		private void CacheSpeedParam()
		{
			this._hasSpeedParam = false;
			if (this.animator == null || string.IsNullOrEmpty(this.speedParam))
			{
				return;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in this.animator.parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Float && animatorControllerParameter.name == this.speedParam)
				{
					this._speedHash = animatorControllerParameter.nameHash;
					this._hasSpeedParam = true;
					return;
				}
			}
			if (this.debugLog)
			{
				Debug.LogWarning(string.Concat(new string[] { "[GuardPatrol] ", base.name, ": animator float param '", this.speedParam, "' non trovato" }));
			}
		}

		// Token: 0x06007141 RID: 28993 RVA: 0x002DFB2C File Offset: 0x002DDD2C
		private void OnDrawGizmosSelected()
		{
			if (!this.drawGizmos || this.waypoints == null)
			{
				return;
			}
			Gizmos.color = Color.cyan;
			for (int i = 0; i < this.waypoints.Length; i++)
			{
				if (!(this.waypoints[i] == null))
				{
					Gizmos.DrawWireSphere(this.waypoints[i].position, 0.25f);
					int num = ((this.mode == GuardPatrol.PatrolMode.Loop) ? ((i + 1) % this.waypoints.Length) : (i + 1));
					if (num < this.waypoints.Length && this.waypoints[num] != null)
					{
						Gizmos.DrawLine(this.waypoints[i].position, this.waypoints[num].position);
					}
				}
			}
		}

		// Token: 0x040073FD RID: 29693
		[Header("Waypoints")]
		[Tooltip("Punti di pattuglia (POI) in ordine. Posizionali in scena come empty GameObject. Servono almeno 2 waypoint perché la guardia si muova.")]
		public Transform[] waypoints;

		// Token: 0x040073FE RID: 29694
		[Tooltip("Ordine di visita dei waypoint.")]
		public GuardPatrol.PatrolMode mode;

		// Token: 0x040073FF RID: 29695
		[Header("Movement")]
		[Tooltip("Velocità di camminata (m/s).")]
		[Min(0.1f)]
		public float moveSpeed = 1.3f;

		// Token: 0x04007400 RID: 29696
		[Tooltip("Velocità di rotazione verso la direzione di marcia (deg/s).")]
		[Min(1f)]
		public float rotationSpeed = 180f;

		// Token: 0x04007401 RID: 29697
		[Tooltip("Distanza (m) entro cui il waypoint è considerato raggiunto.")]
		[Min(0.01f)]
		public float arriveDistance = 0.15f;

		// Token: 0x04007402 RID: 29698
		[Header("Idle at Waypoint")]
		[Tooltip("Range (s) di sosta al waypoint prima di ripartire. Pesca uniformemente in [x,y].")]
		public Vector2 idleTimeRange = new Vector2(2f, 5f);

		// Token: 0x04007403 RID: 29699
		[Tooltip("Se true, alla sosta la guardia ruota verso il forward del waypoint (se assegnato). Utile per far guardare la guardia in una direzione precisa durante la pausa.")]
		public bool faceWaypointForwardOnIdle;

		// Token: 0x04007404 RID: 29700
		[Header("Startup")]
		[Tooltip("Ritardo iniziale (s) prima di iniziare a pattugliare.")]
		[Min(0f)]
		public float initialDelay = 0.5f;

		// Token: 0x04007405 RID: 29701
		[Header("Animator")]
		[Tooltip("Animator da pilotare per l'animazione di camminata. Auto-trovato se vuoto.")]
		public Animator animator;

		// Token: 0x04007406 RID: 29702
		[Tooltip("Nome del parametro float che guida il blend di locomozione (Idle/Walk/Run).")]
		public string speedParam = "Speed";

		// Token: 0x04007407 RID: 29703
		[Tooltip("Smoothing del parametro Speed dell'Animator (più alto = transizione idle/walk più rapida).")]
		[Min(1f)]
		public float speedDamp = 10f;

		// Token: 0x04007408 RID: 29704
		[Header("Debug")]
		public bool debugLog;

		// Token: 0x04007409 RID: 29705
		public bool drawGizmos = true;

		// Token: 0x0400740A RID: 29706
		private GuardPatrol.State _state;

		// Token: 0x0400740B RID: 29707
		private int _currentIndex = -1;

		// Token: 0x0400740C RID: 29708
		private int _direction = 1;

		// Token: 0x0400740D RID: 29709
		private float _idleUntil;

		// Token: 0x0400740E RID: 29710
		private float _startTime;

		// Token: 0x0400740F RID: 29711
		private int _speedHash;

		// Token: 0x04007410 RID: 29712
		private bool _hasSpeedParam;

		// Token: 0x04007411 RID: 29713
		private float _currentAnimSpeed;

		// Token: 0x04007412 RID: 29714
		private float _targetAnimSpeed;

		// Token: 0x04007413 RID: 29715
		private bool _started;

		// Token: 0x04007414 RID: 29716
		private Transform _moveTarget;

		// Token: 0x04007415 RID: 29717
		private bool _simulationPauseActive;

		// Token: 0x04007416 RID: 29718
		private float _simulationPauseStartedAt;

		// Token: 0x04007417 RID: 29719
		private const float MaxStepDeltaTime = 0.1f;

		// Token: 0x02000EC6 RID: 3782
		public enum PatrolMode
		{
			// Token: 0x04007419 RID: 29721
			PingPong,
			// Token: 0x0400741A RID: 29722
			Loop,
			// Token: 0x0400741B RID: 29723
			Random
		}

		// Token: 0x02000EC7 RID: 3783
		private enum State
		{
			// Token: 0x0400741D RID: 29725
			Idle,
			// Token: 0x0400741E RID: 29726
			Moving
		}
	}
}
