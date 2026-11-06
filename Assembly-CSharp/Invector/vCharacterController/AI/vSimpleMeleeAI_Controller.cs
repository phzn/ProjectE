using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vEventSystems;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.AI
{
	// Token: 0x020006D1 RID: 1745
	[vClassHeader("Simple Melee AI", "This is a Simple Melee AI that comes with the MeleeCombat package as a bonus, if you want a more advanced AI check our AI Template")]
	public class vSimpleMeleeAI_Controller : vSimpleMeleeAI_Animator, vIMeleeFighter, vIAttackReceiver, vIAttackListener
	{
		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06002C1F RID: 11295 RVA: 0x000F53CD File Offset: 0x000F35CD
		// (set) Token: 0x06002C20 RID: 11296 RVA: 0x000F53D5 File Offset: 0x000F35D5
		protected virtual float moveToSpeed
		{
			get
			{
				return this._moveToSpeed;
			}
			set
			{
				this._moveToSpeed = value;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002C21 RID: 11297 RVA: 0x000F53DE File Offset: 0x000F35DE
		// (set) Token: 0x06002C22 RID: 11298 RVA: 0x000F53E6 File Offset: 0x000F35E6
		protected virtual Vector3 moveToDestination
		{
			get
			{
				return this._moveToDestination;
			}
			set
			{
				this._moveToDestination = value;
			}
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000F53F0 File Offset: 0x000F35F0
		protected override void Start()
		{
			base.Start();
			this.ignorePatrolTimer = -1f;
			this.moveToDestination = base.transform.position;
			this.Init();
			base.StartCoroutine(this.StateRoutine());
			base.StartCoroutine(this.FindTarget());
			base.StartCoroutine(this.DestinationBehaviour());
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000F544C File Offset: 0x000F364C
		protected void FixedUpdate()
		{
			this.ControlLocomotion();
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000F5454 File Offset: 0x000F3654
		public virtual void SetCurrentTarget(Transform target)
		{
			if (target != this.currentTarget.transform)
			{
				this.currentTarget.transform = target;
				this.currentTarget.colliderTarget = target.GetComponent<Collider>();
				this.currentTarget.character = target.GetComponent<vIHealthController>();
			}
			this.AddTagsToDetect(target.gameObject.tag);
			this.sphereSensor.AddTarget(target);
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000F54BF File Offset: 0x000F36BF
		public virtual void RemoveCurrentTarget()
		{
			if (this.currentTarget.transform)
			{
				this.currentTarget.transform = null;
				this.currentTarget.colliderTarget = null;
				this.currentTarget.character = null;
			}
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000F54F7 File Offset: 0x000F36F7
		public virtual void AddTagsToDetect(string tag)
		{
			if (!this.tagsToDetect.Contains(tag))
			{
				this.tagsToDetect.Add(tag);
			}
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x000F5513 File Offset: 0x000F3713
		public virtual void RemoveTagToDetect(string tag)
		{
			if (this.tagsToDetect.Contains(tag))
			{
				this.tagsToDetect.Remove(tag);
			}
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000F5530 File Offset: 0x000F3730
		protected void SetTarget()
		{
			if (this.currentHealth > 0f && this.sphereSensor != null)
			{
				if (this.currentTarget.transform == null || this.sortTargetFromDistance)
				{
					this.sphereSensor.CheckTargetsAround(this.fieldOfView, this.minDetectDistance, this.maxDetectDistance, this.tagsToDetect, this.layersToDetect, this.sortTargetFromDistance);
					vIHealthController targetvCharacter = this.sphereSensor.GetTargetvCharacter();
					if (targetvCharacter != null && targetvCharacter.currentHealth > 0f)
					{
						this.currentTarget.transform = targetvCharacter.transform;
						this.currentTarget.character = targetvCharacter;
					}
				}
				if (!this.CheckTargetIsAlive() || base.TargetDistance > base.lostTargetDistance)
				{
					this.currentTarget.transform = null;
					return;
				}
			}
			else if (this.currentHealth <= 0f)
			{
				this.destination = base.transform.position;
				this.currentTarget.transform = null;
			}
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x000F562E File Offset: 0x000F382E
		private bool CheckTargetIsAlive()
		{
			return !(this.currentTarget.transform == null) && this.currentTarget.character != null && this.currentTarget.character.currentHealth > 0f;
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000F566C File Offset: 0x000F386C
		protected IEnumerator FindTarget()
		{
			for (;;)
			{
				yield return new WaitForSeconds(this.findTargetIteration);
				if (this.currentHealth > 0f)
				{
					this.SetTarget();
					base.CheckTarget();
				}
			}
			yield break;
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000F567C File Offset: 0x000F387C
		private void ControlLocomotion()
		{
			if ((base.AgentDone() && this.agent.updatePosition) || this.lockMovement)
			{
				this.agent.speed = 0f;
				this.combatMovement = Vector3.zero;
			}
			if (this.agent.isOnOffMeshLink)
			{
				float magnitude = this.agent.desiredVelocity.magnitude;
				base.UpdateAnimator(base.AgentDone() ? 0f : magnitude, this.direction);
				return;
			}
			Vector3 vector = (this.agent.enabled ? (this.agent.updatePosition ? this.agent.desiredVelocity : (this.agent.nextPosition - base.transform.position)) : (this.destination - base.transform.position));
			if (base.OnStrafeArea)
			{
				Vector3 normalized = base.transform.InverseTransformDirection(vector).normalized;
				this.combatMovement = Vector3.Lerp(this.combatMovement, normalized, 2f * Time.deltaTime);
				base.UpdateAnimator(base.AgentDone() ? 0f : this.combatMovement.z, this.combatMovement.x);
				return;
			}
			float magnitude2 = vector.magnitude;
			this.combatMovement = Vector3.zero;
			base.UpdateAnimator(base.AgentDone() ? 0f : magnitude2, 0f);
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x000F57F0 File Offset: 0x000F39F0
		private Vector3 AgentDirection()
		{
			Vector3 vector = (base.AgentDone() ? ((this.currentTarget.transform != null && base.OnStrafeArea && this.canSeeTarget) ? (new Vector3(this.destination.x, base.transform.position.y, this.destination.z) - base.transform.position) : base.transform.forward) : this.agent.desiredVelocity);
			this.fwd = Vector3.Lerp(this.fwd, vector, 20f * Time.deltaTime);
			return this.fwd;
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x000F58A1 File Offset: 0x000F3AA1
		protected virtual IEnumerator DestinationBehaviour()
		{
			for (;;)
			{
				yield return new WaitForSeconds(this.destinationRoutineIteration);
				base.CheckGroundDistance();
				if (this.agent.updatePosition)
				{
					this.UpdateDestination(this.destination);
				}
			}
			yield break;
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x000F58B0 File Offset: 0x000F3AB0
		protected virtual void UpdateDestination(Vector3 position)
		{
			if (this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(position);
			}
			if (this.agent.enabled && this.agent.hasPath && this.drawAgentPath)
			{
				Debug.DrawLine(base.transform.position, position, Color.red, 0.5f);
				Vector3 vector = base.transform.position;
				for (int i = 0; i < this.agent.path.corners.Length; i++)
				{
					Vector3 vector2 = this.agent.path.corners[i];
					Debug.DrawLine(vector, vector2, Color.green, 0.5f);
					vector = vector2;
				}
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000F596A File Offset: 0x000F3B6A
		protected void CheckIsOnNavMesh()
		{
			if (!this.agent.isOnNavMesh && this.agent.enabled && !this.ragdolled)
			{
				Debug.LogWarning("Missing NavMesh Bake, character will die - Please Bake your navmesh again!");
				this.currentHealth = 0f;
			}
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000F59A3 File Offset: 0x000F3BA3
		protected IEnumerator StateRoutine()
		{
			while (base.enabled)
			{
				this.CheckIsOnNavMesh();
				base.CheckAutoCrouch();
				yield return new WaitForSeconds(this.stateRoutineIteration);
				if (!this.lockMovement)
				{
					switch (this.currentState)
					{
					case vSimpleMeleeAI_Motor.AIStates.Idle:
						if (this.currentState != this.oldState)
						{
							this.onIdle.Invoke();
							this.oldState = this.currentState;
						}
						yield return base.StartCoroutine(this.Idle());
						break;
					case vSimpleMeleeAI_Motor.AIStates.PatrolSubPoints:
						if (this.currentState != this.oldState)
						{
							this.onPatrol.Invoke();
							this.oldState = this.currentState;
						}
						yield return base.StartCoroutine(this.PatrolSubPoints());
						break;
					case vSimpleMeleeAI_Motor.AIStates.PatrolWaypoints:
						if (this.currentState != this.oldState)
						{
							this.onPatrol.Invoke();
							this.oldState = this.currentState;
						}
						yield return base.StartCoroutine(this.PatrolWaypoints());
						break;
					case vSimpleMeleeAI_Motor.AIStates.Wander:
						if (this.currentState != this.oldState)
						{
							this.onPatrol.Invoke();
							this.oldState = this.currentState;
						}
						yield return base.StartCoroutine(this.Wander());
						break;
					case vSimpleMeleeAI_Motor.AIStates.Chase:
						if (this.currentState != this.oldState)
						{
							this.onChase.Invoke();
							this.oldState = this.currentState;
						}
						yield return base.StartCoroutine(this.Chase());
						break;
					}
				}
			}
			yield break;
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x000F59B2 File Offset: 0x000F3BB2
		protected IEnumerator Idle()
		{
			while (this.currentHealth <= 0f)
			{
				yield return null;
			}
			if (this.canSeeTarget)
			{
				this.currentState = vSimpleMeleeAI_Motor.AIStates.Chase;
			}
			else
			{
				this.agent.speed = Mathf.Lerp(this.agent.speed, 0f, this.smoothSpeed * Time.deltaTime);
			}
			yield break;
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x000F59C1 File Offset: 0x000F3BC1
		protected IEnumerator Chase()
		{
			while (this.currentHealth <= 0f)
			{
				yield return null;
			}
			this.agent.speed = Mathf.Lerp(this.agent.speed, this.chaseSpeed, this.smoothSpeed * Time.deltaTime);
			this.agent.stoppingDistance = this.chaseStopDistance;
			if (!this.isBlocking && !this.tryingBlock)
			{
				base.StartCoroutine(base.CheckChanceToBlock(this.chanceToBlockInStrafe, this.lowerShield));
			}
			if (this.currentTarget.transform == null || !this.agressiveAtFirstSight)
			{
				this.currentState = vSimpleMeleeAI_Motor.AIStates.PatrolWaypoints;
			}
			if (base.TargetDistance <= base.distanceToAttack && this.meleeManager != null && this.canAttack && !this.actions)
			{
				this.canAttack = false;
				yield return base.StartCoroutine(this.MeleeAttackRotine());
			}
			if (this.attackCount <= 0 && !this.inResetAttack && !base.isAttacking)
			{
				base.StartCoroutine(this.ResetAttackCount());
				yield return null;
			}
			if (base.OnStrafeArea && this.strafeSideways)
			{
				if (this.strafeSwapeFrequency <= 0f)
				{
					this.sideMovement = (float)base.GetRandonSide();
					this.strafeSwapeFrequency = Random.Range(this.minStrafeSwape, this.maxStrafeSwape);
				}
				else
				{
					this.strafeSwapeFrequency -= Time.deltaTime;
				}
				this.fwdMovement = (float)((base.TargetDistance < base.distanceToAttack) ? (this.strafeBackward ? (-1) : 0) : ((base.TargetDistance > base.distanceToAttack) ? 1 : 0));
				Vector3 vector = base.transform.right * this.sideMovement + base.transform.forward * this.fwdMovement;
				Ray ray = new Ray(new Vector3(base.transform.position.x, (this.currentTarget.transform != null) ? this.currentTarget.transform.position.y : base.transform.position.y, base.transform.position.z), vector);
				if (base.TargetDistance < this.strafeDistance - 0.5f)
				{
					this.destination = (base.OnStrafeArea ? ray.GetPoint(this.agent.stoppingDistance + 0.5f) : this.currentTarget.transform.position);
				}
				else if (this.currentTarget.transform != null)
				{
					this.destination = this.currentTarget.transform.position;
				}
			}
			else if (!base.OnStrafeArea && this.currentTarget.transform != null)
			{
				this.destination = this.currentTarget.transform.position;
			}
			else
			{
				this.fwdMovement = (float)((base.TargetDistance < base.distanceToAttack) ? (this.strafeBackward ? (-1) : 0) : ((base.TargetDistance > base.distanceToAttack) ? 1 : 0));
				Ray ray2 = new Ray(base.transform.position, base.transform.forward * this.fwdMovement);
				if (base.TargetDistance < this.strafeDistance - 0.5f)
				{
					this.destination = ((this.fwdMovement != 0f) ? ray2.GetPoint(this.agent.stoppingDistance + ((this.fwdMovement > 0f) ? base.TargetDistance : 1f)) : base.transform.position);
				}
				else if (this.currentTarget.transform != null)
				{
					this.destination = this.currentTarget.transform.position;
				}
			}
			yield break;
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x000F59D0 File Offset: 0x000F3BD0
		protected IEnumerator PatrolSubPoints()
		{
			while (!this.agent.enabled)
			{
				yield return null;
			}
			if (this.targetWaypoint)
			{
				if (this.targetPatrolPoint == null || !this.targetPatrolPoint.isValid)
				{
					this.targetPatrolPoint = this.GetPatrolPoint(this.targetWaypoint);
				}
				else
				{
					this.agent.speed = Mathf.Lerp(this.agent.speed, (this.agent.hasPath && this.targetPatrolPoint.isValid) ? this.patrolSpeed : 0f, this.smoothSpeed * Time.deltaTime);
					this.agent.stoppingDistance = this.patrollingStopDistance;
					this.destination = (this.targetPatrolPoint.isValid ? this.targetPatrolPoint.position : base.transform.position);
					if (Vector3.Distance(base.transform.position, this.destination) < this.targetPatrolPoint.areaRadius && this.targetPatrolPoint.CanEnter(base.transform) && !this.targetPatrolPoint.IsOnWay(base.transform))
					{
						this.targetPatrolPoint.Enter(base.transform);
						this.wait = Time.time + this.targetPatrolPoint.timeToStay;
						this.visitedPatrolPoint.Add(this.targetPatrolPoint);
					}
					else if (Vector3.Distance(base.transform.position, this.destination) < this.targetPatrolPoint.areaRadius && (!this.targetPatrolPoint.CanEnter(base.transform) || !this.targetPatrolPoint.isValid))
					{
						this.targetPatrolPoint = this.GetPatrolPoint(this.targetWaypoint);
					}
					if (this.targetPatrolPoint != null && this.targetPatrolPoint.IsOnWay(base.transform) && Vector3.Distance(base.transform.position, this.destination) < this.distanceToChangeWaypoint && (this.wait < Time.time || !this.targetPatrolPoint.isValid))
					{
						this.wait = 0f;
						if (this.visitedPatrolPoint.Count == this.pathArea.GetValidSubPoints(this.targetWaypoint, false).Count)
						{
							this.currentState = vSimpleMeleeAI_Motor.AIStates.PatrolWaypoints;
							this.targetWaypoint.Exit(base.transform);
							this.targetPatrolPoint.Exit(base.transform);
							this.targetWaypoint = null;
							this.targetPatrolPoint = null;
							this.visitedPatrolPoint.Clear();
						}
						else
						{
							this.targetPatrolPoint.Exit(base.transform);
							this.targetPatrolPoint = this.GetPatrolPoint(this.targetWaypoint);
						}
					}
				}
			}
			if (this.canSeeTarget)
			{
				this.currentState = vSimpleMeleeAI_Motor.AIStates.Chase;
			}
			yield break;
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x000F59DF File Offset: 0x000F3BDF
		protected IEnumerator PatrolWaypoints()
		{
			while (!this.agent.enabled)
			{
				yield return null;
			}
			if (this.pathArea != null && this.pathArea.waypoints.Count > 0)
			{
				if (this.targetWaypoint == null || !this.targetWaypoint.isValid)
				{
					this.targetWaypoint = this.GetWaypoint();
				}
				else
				{
					this.agent.speed = Mathf.Lerp(this.agent.speed, (this.agent.hasPath && this.targetWaypoint.isValid) ? this.patrolSpeed : 0f, this.smoothSpeed * Time.deltaTime);
					this.agent.stoppingDistance = this.patrollingStopDistance;
					this.destination = this.targetWaypoint.position;
					if (Vector3.Distance(base.transform.position, this.destination) < this.targetWaypoint.areaRadius && this.targetWaypoint.CanEnter(base.transform) && !this.targetWaypoint.IsOnWay(base.transform))
					{
						this.targetWaypoint.Enter(base.transform);
						this.wait = Time.time + this.targetWaypoint.timeToStay;
					}
					else if (Vector3.Distance(base.transform.position, this.destination) < this.targetWaypoint.areaRadius && (!this.targetWaypoint.CanEnter(base.transform) || !this.targetWaypoint.isValid))
					{
						this.targetWaypoint = this.GetWaypoint();
					}
					if (this.targetWaypoint != null && this.targetWaypoint.IsOnWay(base.transform) && Vector3.Distance(base.transform.position, this.destination) < this.distanceToChangeWaypoint && (this.wait < Time.time || !this.targetWaypoint.isValid))
					{
						this.wait = 0f;
						if (this.targetWaypoint.subPoints.Count > 0)
						{
							this.currentState = vSimpleMeleeAI_Motor.AIStates.PatrolSubPoints;
						}
						else
						{
							this.targetWaypoint.Exit(base.transform);
							this.visitedPatrolPoint.Clear();
							this.targetWaypoint = this.GetWaypoint();
						}
					}
				}
			}
			else if (this.ignorePatrolTimer < Time.time)
			{
				switch (this.patrolWithoutAreaStyle)
				{
				case vSimpleMeleeAI_Motor.AIPatrolWithOutAreaStyle.GoToStartPoint:
					yield return base.StartCoroutine(this.GoToStartingPoint());
					break;
				case vSimpleMeleeAI_Motor.AIPatrolWithOutAreaStyle.Wander:
					this.currentState = vSimpleMeleeAI_Motor.AIStates.Wander;
					break;
				case vSimpleMeleeAI_Motor.AIPatrolWithOutAreaStyle.Idle:
					this.currentState = vSimpleMeleeAI_Motor.AIStates.Idle;
					break;
				}
			}
			else if (this.ignorePatrolTimer > Time.time)
			{
				yield return base.StartCoroutine(this.GoToDestionation());
			}
			if (this.canSeeTarget)
			{
				this.currentState = vSimpleMeleeAI_Motor.AIStates.Chase;
			}
			yield break;
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x000F59EE File Offset: 0x000F3BEE
		protected virtual IEnumerator GoToDestionation()
		{
			yield return null;
			this.agent.speed = Mathf.Lerp(this.agent.speed, this.moveToSpeed, this.smoothSpeed * Time.deltaTime);
			this.agent.stoppingDistance = this.patrollingStopDistance;
			this.destination = this.moveToDestination;
			yield break;
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x000F59FD File Offset: 0x000F3BFD
		protected virtual IEnumerator GoToStartingPoint()
		{
			yield return null;
			this.agent.speed = Mathf.Lerp(this.agent.speed, this.patrolSpeed, this.smoothSpeed * Time.deltaTime);
			this.agent.stoppingDistance = this.patrollingStopDistance;
			this.destination = this.startPosition;
			yield break;
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x000F5A0C File Offset: 0x000F3C0C
		protected virtual IEnumerator Wander()
		{
			this.agent.speed = Mathf.Lerp(this.agent.speed, this.wanderSpeed, this.smoothSpeed * Time.deltaTime);
			do
			{
				yield return null;
				this.destination = base.transform.position + Quaternion.AngleAxis((float)Random.Range(-120, 120), base.transform.up) * base.transform.forward * (this.patrollingStopDistance + 4f);
			}
			while (this.agent.enabled && this.agent.isOnNavMesh && this.agent.remainingDistance <= this.patrollingStopDistance);
			if (this.canSeeTarget)
			{
				this.currentState = vSimpleMeleeAI_Motor.AIStates.Chase;
			}
			yield break;
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x000F5A1C File Offset: 0x000F3C1C
		private vWaypoint GetWaypoint()
		{
			List<vWaypoint> validPoints = this.pathArea.GetValidPoints(false);
			if (this.randomWaypoints)
			{
				this.currentWaypoint = this.randomWaypoint.Next(validPoints.Count);
			}
			else
			{
				int currentWaypoint = this.currentWaypoint;
				this.currentWaypoint = currentWaypoint + 1;
			}
			if (this.currentWaypoint >= validPoints.Count)
			{
				this.currentWaypoint = 0;
			}
			if (validPoints.Count == 0)
			{
				this.agent.isStopped = true;
				return null;
			}
			if (this.visitedWaypoint.Count == validPoints.Count)
			{
				this.visitedWaypoint.Clear();
			}
			if (this.visitedWaypoint.Contains(validPoints[this.currentWaypoint]))
			{
				return null;
			}
			this.agent.isStopped = false;
			return validPoints[this.currentWaypoint];
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x000F5AE4 File Offset: 0x000F3CE4
		private vPoint GetPatrolPoint(vWaypoint waypoint)
		{
			List<vPoint> validSubPoints = this.pathArea.GetValidSubPoints(waypoint, false);
			if (waypoint.randomPatrolPoint)
			{
				this.currentPatrolPoint = this.randomPatrolPoint.Next(validSubPoints.Count);
			}
			else
			{
				int currentPatrolPoint = this.currentPatrolPoint;
				this.currentPatrolPoint = currentPatrolPoint + 1;
			}
			if (this.currentPatrolPoint >= validSubPoints.Count)
			{
				this.currentPatrolPoint = 0;
			}
			if (validSubPoints.Count == 0)
			{
				this.agent.isStopped = true;
				return null;
			}
			if (this.visitedPatrolPoint.Contains(validSubPoints[this.currentPatrolPoint]))
			{
				return null;
			}
			this.agent.isStopped = false;
			return validSubPoints[this.currentPatrolPoint];
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x000F5B8E File Offset: 0x000F3D8E
		protected IEnumerator MeleeAttackRotine()
		{
			if (!base.isAttacking && !this.actions && this.attackCount > 0 && !this.lockMovement && !this.isRolling)
			{
				this.sideMovement = (float)base.GetRandonSide();
				this.agent.stoppingDistance = base.distanceToAttack;
				int attackCount = this.attackCount;
				this.attackCount = attackCount - 1;
				base.MeleeAttack();
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x000F5B9D File Offset: 0x000F3D9D
		public void FinishAttack()
		{
			this.canAttack = true;
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000F5BA6 File Offset: 0x000F3DA6
		private IEnumerator ResetAttackCount()
		{
			this.inResetAttack = true;
			this.canAttack = false;
			float num;
			if (this.firstAttack)
			{
				this.firstAttack = false;
				num = this.firstAttackDelay;
			}
			else
			{
				num = Random.Range(this.minTimeToAttack, this.maxTimeToAttack);
			}
			yield return new WaitForSeconds(num);
			this.attackCount = (this.randomAttackCount ? Random.Range(1, this.maxAttackCount + 1) : this.maxAttackCount);
			this.canAttack = true;
			this.inResetAttack = false;
			yield break;
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x000F5BB5 File Offset: 0x000F3DB5
		public void OnEnableAttack()
		{
			base.isAttacking = true;
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x000F5BBE File Offset: 0x000F3DBE
		public void OnDisableAttack()
		{
			base.isAttacking = false;
			this.canAttack = true;
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x000F5BCE File Offset: 0x000F3DCE
		public void ResetAttackTriggers()
		{
			base.animator.ResetTrigger("WeakAttack");
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x000F5BE0 File Offset: 0x000F3DE0
		public void BreakAttack(int breakAtkID)
		{
			this.ResetAttackCount();
			this.ResetAttackTriggers();
			this.OnRecoil(breakAtkID);
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x000F5BF6 File Offset: 0x000F3DF6
		public void OnRecoil(int recoilID)
		{
			base.TriggerRecoil(recoilID);
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x000F5C00 File Offset: 0x000F3E00
		public void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker)
		{
			base.StartCoroutine(base.CheckChanceToBlock(this.chanceToBlockAttack, 0f));
			Vector3 vector = ((attacker != null && attacker.character != null) ? attacker.character.transform.position : damage.hitPosition);
			if (!damage.ignoreDefense && this.isBlocking && this.meleeManager != null && this.meleeManager.CanBlockAttack(vector))
			{
				int num = ((this.meleeManager != null) ? this.meleeManager.GetDefenseRate() : 0);
				if (num > 0)
				{
					damage.ReduceDamage((float)num);
				}
				if (attacker != null && this.meleeManager != null && this.meleeManager.CanBreakAttack())
				{
					attacker.OnRecoil(this.meleeManager.GetDefenseRecoilID());
				}
				this.meleeManager.OnDefense();
			}
			if (!this.passiveToDamage && damage.sender != null)
			{
				this.SetCurrentTarget(damage.sender);
				this.currentState = vSimpleMeleeAI_Motor.AIStates.Chase;
			}
			damage.hitReaction = !this.isBlocking;
			if (!this.passiveToDamage)
			{
				this.SetAggressive(true);
			}
			this.TakeDamage(damage);
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x000F5D29 File Offset: 0x000F3F29
		public virtual void MoveTo(Vector3 position, float moveToSpeed = 1f, float ignorePatrolTimer = 2f)
		{
			this.moveToDestination = position;
			this.currentState = vSimpleMeleeAI_Motor.AIStates.PatrolWaypoints;
			this.moveToSpeed = moveToSpeed;
			this.ignorePatrolTimer = Time.time + ignorePatrolTimer;
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002C45 RID: 11333 RVA: 0x000F5D4D File Offset: 0x000F3F4D
		public vICharacter character
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000AD00A File Offset: 0x000AB20A
		Transform vIMeleeFighter.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x000B0592 File Offset: 0x000AE792
		GameObject vIMeleeFighter.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x04002B3D RID: 11069
		[vEditorToolbar("Iterations", false, "", false, false)]
		public float stateRoutineIteration = 0.15f;

		// Token: 0x04002B3E RID: 11070
		public float destinationRoutineIteration = 0.25f;

		// Token: 0x04002B3F RID: 11071
		public float findTargetIteration = 0.25f;

		// Token: 0x04002B40 RID: 11072
		public float smoothSpeed = 5f;

		// Token: 0x04002B41 RID: 11073
		[vEditorToolbar("Events", false, "", false, false)]
		[Header("--- On Change State Events ---")]
		public UnityEvent onIdle;

		// Token: 0x04002B42 RID: 11074
		public UnityEvent onChase;

		// Token: 0x04002B43 RID: 11075
		public UnityEvent onPatrol;

		// Token: 0x04002B44 RID: 11076
		protected vSimpleMeleeAI_Motor.AIStates oldState;

		// Token: 0x04002B45 RID: 11077
		protected float ignorePatrolTimer;

		// Token: 0x04002B46 RID: 11078
		protected float _moveToSpeed;

		// Token: 0x04002B47 RID: 11079
		protected Vector3 _moveToDestination;
	}
}
