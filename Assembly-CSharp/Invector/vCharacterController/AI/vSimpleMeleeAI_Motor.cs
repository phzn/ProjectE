using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vMelee;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Invector.vCharacterController.AI
{
	// Token: 0x020006DE RID: 1758
	public class vSimpleMeleeAI_Motor : vCharacter
	{
		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002C91 RID: 11409 RVA: 0x000F7028 File Offset: 0x000F5228
		// (set) Token: 0x06002C92 RID: 11410 RVA: 0x000F7030 File Offset: 0x000F5230
		public virtual vSimpleMeleeAI_Motor.AIStates currentState
		{
			get
			{
				return this._currentState;
			}
			set
			{
				this._currentState = value;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002C93 RID: 11411 RVA: 0x000F7039 File Offset: 0x000F5239
		public float lostTargetDistance
		{
			get
			{
				return this.maxDetectDistance + this.distanceToLostTarget;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x000F7048 File Offset: 0x000F5248
		// (set) Token: 0x06002C95 RID: 11413 RVA: 0x000F7050 File Offset: 0x000F5250
		protected virtual Vector3 targetPos
		{
			get
			{
				return this._targetPos;
			}
			set
			{
				this._targetPos = value;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x000F7059 File Offset: 0x000F5259
		// (set) Token: 0x06002C97 RID: 11415 RVA: 0x000F7061 File Offset: 0x000F5261
		protected virtual bool canSeeTarget
		{
			get
			{
				return this._canSeeTarget;
			}
			set
			{
				this._canSeeTarget = value;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002C98 RID: 11416 RVA: 0x000F706A File Offset: 0x000F526A
		// (set) Token: 0x06002C99 RID: 11417 RVA: 0x000F7072 File Offset: 0x000F5272
		protected virtual Vector3 destination
		{
			get
			{
				return this._destination;
			}
			set
			{
				this._destination = value;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002C9A RID: 11418 RVA: 0x000F707B File Offset: 0x000F527B
		// (set) Token: 0x06002C9B RID: 11419 RVA: 0x000F7083 File Offset: 0x000F5283
		protected virtual Vector3 fwd
		{
			get
			{
				return this._fwd;
			}
			set
			{
				this._fwd = value;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002C9C RID: 11420 RVA: 0x000F708C File Offset: 0x000F528C
		// (set) Token: 0x06002C9D RID: 11421 RVA: 0x000F7094 File Offset: 0x000F5294
		protected virtual bool _isGrounded { get; set; }

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002C9E RID: 11422 RVA: 0x000F709D File Offset: 0x000F529D
		// (set) Token: 0x06002C9F RID: 11423 RVA: 0x000F70A5 File Offset: 0x000F52A5
		protected virtual bool isGrounded
		{
			get
			{
				return this._isGrounded;
			}
			set
			{
				this._isGrounded = value;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002CA0 RID: 11424 RVA: 0x000F70AE File Offset: 0x000F52AE
		// (set) Token: 0x06002CA1 RID: 11425 RVA: 0x000F70B6 File Offset: 0x000F52B6
		protected virtual bool isStrafing
		{
			get
			{
				return this._isStrafing;
			}
			set
			{
				this._isStrafing = value;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x000F70BF File Offset: 0x000F52BF
		// (set) Token: 0x06002CA3 RID: 11427 RVA: 0x000F70C7 File Offset: 0x000F52C7
		protected virtual bool inResetAttack { get; set; }

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x000F70D0 File Offset: 0x000F52D0
		// (set) Token: 0x06002CA5 RID: 11429 RVA: 0x000F70D8 File Offset: 0x000F52D8
		protected virtual bool firstAttack { get; set; }

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002CA6 RID: 11430 RVA: 0x000F70E1 File Offset: 0x000F52E1
		// (set) Token: 0x06002CA7 RID: 11431 RVA: 0x000F70E9 File Offset: 0x000F52E9
		protected virtual int attackCount { get; set; }

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002CA8 RID: 11432 RVA: 0x000F70F2 File Offset: 0x000F52F2
		// (set) Token: 0x06002CA9 RID: 11433 RVA: 0x000F70FA File Offset: 0x000F52FA
		protected virtual int _currentWaypoint { get; set; }

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002CAA RID: 11434 RVA: 0x000F7103 File Offset: 0x000F5303
		// (set) Token: 0x06002CAB RID: 11435 RVA: 0x000F710B File Offset: 0x000F530B
		protected virtual int currentWaypoint
		{
			get
			{
				return this._currentWaypoint;
			}
			set
			{
				this._currentWaypoint = value;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002CAC RID: 11436 RVA: 0x000F7114 File Offset: 0x000F5314
		// (set) Token: 0x06002CAD RID: 11437 RVA: 0x000F711C File Offset: 0x000F531C
		protected virtual int _currentPatrolPoint { get; set; }

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002CAE RID: 11438 RVA: 0x000F7125 File Offset: 0x000F5325
		// (set) Token: 0x06002CAF RID: 11439 RVA: 0x000F712D File Offset: 0x000F532D
		protected virtual int currentPatrolPoint
		{
			get
			{
				return this._currentPatrolPoint;
			}
			set
			{
				this._currentPatrolPoint = value;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x000F7136 File Offset: 0x000F5336
		// (set) Token: 0x06002CB1 RID: 11441 RVA: 0x000F713E File Offset: 0x000F533E
		protected virtual float direction
		{
			get
			{
				return this._direction;
			}
			set
			{
				this._direction = value;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002CB2 RID: 11442 RVA: 0x000F7147 File Offset: 0x000F5347
		// (set) Token: 0x06002CB3 RID: 11443 RVA: 0x000F714F File Offset: 0x000F534F
		protected virtual float timer
		{
			get
			{
				return this._timer;
			}
			set
			{
				this._timer = value;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002CB4 RID: 11444 RVA: 0x000F7158 File Offset: 0x000F5358
		// (set) Token: 0x06002CB5 RID: 11445 RVA: 0x000F7160 File Offset: 0x000F5360
		protected virtual float wait
		{
			get
			{
				return this._wait;
			}
			set
			{
				this._wait = value;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002CB6 RID: 11446 RVA: 0x000F7169 File Offset: 0x000F5369
		// (set) Token: 0x06002CB7 RID: 11447 RVA: 0x000F7171 File Offset: 0x000F5371
		protected virtual float fovAngle
		{
			get
			{
				return this._fovAngle;
			}
			set
			{
				this._fovAngle = value;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x000F717A File Offset: 0x000F537A
		// (set) Token: 0x06002CB9 RID: 11449 RVA: 0x000F7182 File Offset: 0x000F5382
		protected virtual float sideMovement { get; set; }

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002CBA RID: 11450 RVA: 0x000F718B File Offset: 0x000F538B
		// (set) Token: 0x06002CBB RID: 11451 RVA: 0x000F7193 File Offset: 0x000F5393
		protected virtual float fwdMovement { get; set; }

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002CBC RID: 11452 RVA: 0x000F719C File Offset: 0x000F539C
		// (set) Token: 0x06002CBD RID: 11453 RVA: 0x000F71A4 File Offset: 0x000F53A4
		protected virtual float strafeSwapeFrequency { get; set; }

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002CBE RID: 11454 RVA: 0x000F71AD File Offset: 0x000F53AD
		// (set) Token: 0x06002CBF RID: 11455 RVA: 0x000F71B5 File Offset: 0x000F53B5
		protected virtual float groundDistance { get; set; }

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002CC0 RID: 11456 RVA: 0x000F71BE File Offset: 0x000F53BE
		// (set) Token: 0x06002CC1 RID: 11457 RVA: 0x000F71C6 File Offset: 0x000F53C6
		protected virtual Vector3 startPosition
		{
			get
			{
				return this._startPosition;
			}
			set
			{
				this._startPosition = value;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002CC2 RID: 11458 RVA: 0x000F71CF File Offset: 0x000F53CF
		// (set) Token: 0x06002CC3 RID: 11459 RVA: 0x000F71D7 File Offset: 0x000F53D7
		protected virtual bool isCrouched
		{
			get
			{
				return this._isCrouched;
			}
			set
			{
				this._isCrouched = value;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x000F71E0 File Offset: 0x000F53E0
		// (set) Token: 0x06002CC5 RID: 11461 RVA: 0x000F71E8 File Offset: 0x000F53E8
		protected virtual bool canAttack
		{
			get
			{
				return this._canAttack;
			}
			set
			{
				this._canAttack = value;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x000F71F1 File Offset: 0x000F53F1
		// (set) Token: 0x06002CC7 RID: 11463 RVA: 0x000F71F9 File Offset: 0x000F53F9
		protected virtual bool tryingBlock
		{
			get
			{
				return this._tryingBlock;
			}
			set
			{
				this._tryingBlock = value;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002CC8 RID: 11464 RVA: 0x000F7202 File Offset: 0x000F5402
		// (set) Token: 0x06002CC9 RID: 11465 RVA: 0x000F720A File Offset: 0x000F540A
		protected virtual bool isRolling
		{
			get
			{
				return this._isRolling;
			}
			set
			{
				this._isRolling = value;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002CCA RID: 11466 RVA: 0x000F7213 File Offset: 0x000F5413
		// (set) Token: 0x06002CCB RID: 11467 RVA: 0x000F721B File Offset: 0x000F541B
		public virtual bool isBlocking
		{
			get
			{
				return this._isBlocking;
			}
			protected set
			{
				this._isBlocking = value;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002CCC RID: 11468 RVA: 0x000F7224 File Offset: 0x000F5424
		// (set) Token: 0x06002CCD RID: 11469 RVA: 0x000F722C File Offset: 0x000F542C
		public bool isAttacking
		{
			get
			{
				return this._isAttacking;
			}
			protected set
			{
				this._isAttacking = value;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002CCE RID: 11470 RVA: 0x000F7238 File Offset: 0x000F5438
		public bool isArmed
		{
			get
			{
				return this.meleeManager != null && (this.meleeManager.rightWeapon != null || (this.meleeManager.leftWeapon != null && this.meleeManager.leftWeapon.meleeType > vMeleeType.OnlyDefense));
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002CCF RID: 11471 RVA: 0x000F7292 File Offset: 0x000F5492
		// (set) Token: 0x06002CD0 RID: 11472 RVA: 0x000F729A File Offset: 0x000F549A
		public virtual bool actions
		{
			get
			{
				return this._actions;
			}
			set
			{
				this._actions = value;
			}
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000F72A4 File Offset: 0x000F54A4
		public override void Init()
		{
			base.Init();
			this.fwd = base.transform.forward;
			this.destination = base.transform.position;
			this.agent = base.GetComponent<NavMeshAgent>();
			this.agentPath = new NavMeshPath();
			this.sphereSensor = base.GetComponentInChildren<vSimpleMeleeAI_SphereSensor>();
			if (this.sphereSensor)
			{
				this.sphereSensor.root = base.transform;
			}
			this.meleeManager = base.GetComponent<vMeleeManager>();
			this.canAttack = true;
			this.attackCount = 0;
			this.sideMovement = (float)this.GetRandonSide();
			this.destination = base.transform.position;
			this._rigidbody = base.GetComponent<Rigidbody>();
			this._rigidbody.useGravity = true;
			this._rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			this.agent.updatePosition = false;
			this.agent.updateRotation = false;
			this.agent.enabled = false;
			this._capsuleCollider = base.GetComponent<CapsuleCollider>();
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			Collider component = base.GetComponent<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Physics.IgnoreCollision(component, componentsInChildren[i]);
			}
			this.healthSlider = base.GetComponentInChildren<v_SpriteHealth>();
			this.head = base.animator.GetBoneTransform(HumanBodyBones.Head);
			this.oldPosition = base.transform.position;
			if (!this.fillHealthOnStart)
			{
				this.currentHealth = this._currentHealth;
			}
			this.startPosition = base.transform.position;
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002CD2 RID: 11474 RVA: 0x000F7421 File Offset: 0x000F5621
		public float distanceToAttack
		{
			get
			{
				if (this.meleeManager)
				{
					return this.meleeManager.GetAttackDistance();
				}
				return 1f;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x000F7444 File Offset: 0x000F5644
		public bool OnCombatArea
		{
			get
			{
				return !(this.currentTarget.transform == null) && (Vector3.Distance(new Vector3(0f, base.transform.position.y, 0f), new Vector3(0f, this.currentTarget.transform.position.y, 0f)) < this.distanceToAttack && this.agressiveAtFirstSight && this.TargetDistance <= this.strafeDistance) && !this.agent.isOnOffMeshLink;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002CD4 RID: 11476 RVA: 0x000F74E0 File Offset: 0x000F56E0
		public bool OnStrafeArea
		{
			get
			{
				if (!this.canSeeTarget)
				{
					this.isStrafing = false;
					return false;
				}
				if (this.currentTarget.transform == null || !this.agressiveAtFirstSight)
				{
					return false;
				}
				bool flag = Vector3.Distance(new Vector3(0f, base.transform.position.y, 0f), new Vector3(0f, this.currentTarget.transform.position.y, 0f)) < 1.5f;
				if (this.isStrafing)
				{
					this.isStrafing = this.TargetDistance < this.strafeDistance + 2f;
				}
				else
				{
					this.isStrafing = this.OnCombatArea;
				}
				return flag && this.isStrafing;
			}
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000F75A4 File Offset: 0x000F57A4
		public bool AgentDone()
		{
			return (!this.agent.enabled && this.agent.updatePosition) || (!this.agent.pathPending && this.AgentStopping() && this.agent.updatePosition);
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000F75E4 File Offset: 0x000F57E4
		public bool AgentStopping()
		{
			return !this.agent.enabled || !this.agent.isOnNavMesh || this.agent.remainingDistance <= this.agent.stoppingDistance;
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000F7620 File Offset: 0x000F5820
		public int GetRandonSide()
		{
			int num = Random.Range(-1, 1);
			if (num < 0)
			{
				num = -1;
			}
			else
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000F7640 File Offset: 0x000F5840
		protected void CheckGroundDistance()
		{
			if (this._capsuleCollider != null)
			{
				float num = 10f;
				if (Physics.Raycast(new Ray(base.transform.position + new Vector3(0f, this._capsuleCollider.height / 2f, 0f), Vector3.down), out this.groundHit, this._capsuleCollider.height * 0.75f, this.groundLayer))
				{
					num = base.transform.position.y - this.groundHit.point.y;
				}
				this.groundDistance = num;
				if (!this.actions && !this.isRolling && this.groundDistance < 0.3f)
				{
					NavMeshHit navMeshHit;
					if (this.currentHealth > 0f && NavMesh.SamplePosition(base.transform.position, out navMeshHit, 5f, -1))
					{
						this._rigidbody.constraints = (RigidbodyConstraints)122;
						this._rigidbody.useGravity = false;
						this.agent.updatePosition = false;
						this.agent.enabled = true;
					}
					if (this.agent.enabled && this.agent.isOnNavMesh && Vector3.Distance(this.agent.nextPosition, base.transform.position) <= 0.1f)
					{
						this.agent.updatePosition = true;
					}
					else if (this.agent.enabled && this.agent.isOnNavMesh && !this.agent.updatePosition)
					{
						this.agent.nextPosition = base.transform.position;
					}
				}
				if (!this.agent.isOnNavMesh && this.groundDistance > 0.3f && !this.ragdolled)
				{
					this._rigidbody.useGravity = true;
					this._rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
					this.agent.enabled = false;
					this.agent.updatePosition = false;
				}
			}
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000F7848 File Offset: 0x000F5A48
		public void CheckAutoCrouch()
		{
			float num = this._capsuleCollider.radius * 0.9f;
			Vector3 vector = base.transform.position + Vector3.up * (this._capsuleCollider.height * 0.5f - this._capsuleCollider.radius);
			RaycastHit raycastHit;
			if (Physics.SphereCast(new Ray(vector, Vector3.up), num, out raycastHit, this.headDetect - this._capsuleCollider.radius * 0.1f, this.autoCrouchLayer))
			{
				this.isCrouched = true;
				return;
			}
			this.isCrouched = false;
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000F78E8 File Offset: 0x000F5AE8
		public bool onFovAngle()
		{
			if (this.currentTarget.transform == null)
			{
				return false;
			}
			Vector3 vector = Quaternion.LookRotation(this.currentTarget.transform.position - base.transform.position, Vector3.up).eulerAngles - base.transform.eulerAngles;
			this.fovAngle = vector.NormalizeAngle().y;
			return this.fovAngle < this.fieldOfView && this.fovAngle > -this.fieldOfView;
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000F7980 File Offset: 0x000F5B80
		public void CheckTarget()
		{
			if (this.currentTarget.transform == null || !this.agressiveAtFirstSight)
			{
				this.canSeeTarget = false;
				this.canSeeTargetIteration = 0;
				return;
			}
			if (this.TargetDistance > this.maxDetectDistance)
			{
				this.canSeeTarget = false;
				this.canSeeTargetIteration = 0;
				return;
			}
			if (this.currentTarget.colliderTarget == null || this.currentTarget.colliderTarget.transform != this.currentTarget.transform)
			{
				this.currentTarget.colliderTarget = this.currentTarget.transform.GetComponent<Collider>();
			}
			if (this.currentTarget.colliderTarget == null)
			{
				this.canSeeTarget = false;
				this.canSeeTargetIteration = 0;
				return;
			}
			Vector3 vector = new Vector3(this.currentTarget.colliderTarget.bounds.center.x, this.currentTarget.colliderTarget.bounds.max.y, this.currentTarget.colliderTarget.bounds.center.z);
			Vector3 vector2 = new Vector3(this.currentTarget.colliderTarget.bounds.center.x, this.currentTarget.colliderTarget.bounds.min.y, this.currentTarget.colliderTarget.bounds.center.z);
			float num = Vector3.Distance(vector, vector2) * 0.15f;
			vector.y -= num;
			vector2.y += num;
			if (!this.onFovAngle() && this.TargetDistance > this.minDetectDistance)
			{
				this.canSeeTarget = false;
				this.canSeeTargetIteration = 0;
				return;
			}
			RaycastHit raycastHit;
			if (this.canSeeTargetIteration == 0 && !Physics.Linecast(this.head.position, vector, out raycastHit, this.obstaclesLayer))
			{
				this.canSeeTarget = true;
				this.canSeeTargetIteration = 0;
				return;
			}
			if (this.canSeeTargetIteration == 1 && !Physics.Linecast(this.head.position, vector2, out raycastHit, this.obstaclesLayer))
			{
				this.canSeeTarget = true;
				this.canSeeTargetIteration = 0;
				return;
			}
			if (this.canSeeTargetIteration == 2 && !Physics.Linecast(this.head.position, this.currentTarget.colliderTarget.bounds.center, out raycastHit, this.obstaclesLayer))
			{
				this.canSeeTarget = true;
				this.canSeeTargetIteration = 0;
				return;
			}
			this.canSeeTargetIteration++;
			if (this.canSeeTargetIteration > 1)
			{
				this.canSeeTargetIteration = 0;
			}
			this.canSeeTarget = false;
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002CDC RID: 11484 RVA: 0x000F7C38 File Offset: 0x000F5E38
		public float TargetDistance
		{
			get
			{
				if (!(this.currentTarget.transform != null))
				{
					return this.maxDetectDistance + 1f;
				}
				if (!this.currentTarget.colliderTarget)
				{
					return Vector3.Distance(base.transform.position, this.currentTarget.transform.position);
				}
				return Vector3.Distance(this._capsuleCollider.bounds.center, this.currentTarget.colliderTarget.bounds.center);
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06002CDD RID: 11485 RVA: 0x000F7CC8 File Offset: 0x000F5EC8
		public Transform headTarget
		{
			get
			{
				if (this.currentTarget.transform != null && this.currentTarget.transform.GetComponent<Animator>() != null)
				{
					return this.currentTarget.transform.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
				}
				return null;
			}
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000F7D1C File Offset: 0x000F5F1C
		protected void RemoveComponents()
		{
			if (!this.removeComponentsAfterDie)
			{
				return;
			}
			if (this._capsuleCollider != null)
			{
				Object.Destroy(this._capsuleCollider);
			}
			if (this._rigidbody != null)
			{
				Object.Destroy(this._rigidbody);
			}
			if (base.animator != null)
			{
				Object.Destroy(base.animator);
			}
			if (this.agent != null)
			{
				Object.Destroy(this.agent);
			}
			MonoBehaviour[] components = base.GetComponents<MonoBehaviour>();
			for (int i = 0; i < components.Length; i++)
			{
				Object.Destroy(components[i]);
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000F7DB4 File Offset: 0x000F5FB4
		public override void TakeDamage(vDamage damage)
		{
			if (this.isRolling || this.currentHealth <= 0f || !base.animator.enabled)
			{
				return;
			}
			if (!damage.ignoreDefense && !this.actions && this.CheckChanceToRoll())
			{
				return;
			}
			base.TakeDamage(damage);
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000F7E04 File Offset: 0x000F6004
		protected override void TriggerDamageReaction(vDamage damage)
		{
			if (!this.isRolling)
			{
				base.TriggerDamageReaction(damage);
			}
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000F7E18 File Offset: 0x000F6018
		protected bool CheckChanceToRoll()
		{
			if (this.isAttacking || this.actions)
			{
				return false;
			}
			float value = Random.value;
			if (value < this.chanceToRoll && value > 0f && this.currentTarget.transform != null)
			{
				base.animator.SetTrigger("ResetState");
				this.sideMovement = (float)this.GetRandonSide();
				Ray ray = new Ray(this.currentTarget.transform.position, this.currentTarget.transform.right * this.sideMovement);
				this.rollDirection = ray.direction;
				base.animator.CrossFadeInFixedTime("Roll", 0.1f);
				return true;
			}
			return false;
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000F7EDB File Offset: 0x000F60DB
		protected IEnumerator CheckChanceToBlock(float chance, float timeToEnter)
		{
			this.tryingBlock = true;
			float value = Random.value;
			if (value < chance && value > 0f && !this.isBlocking)
			{
				if (timeToEnter > 0f)
				{
					yield return new WaitForSeconds(timeToEnter);
				}
				this.isBlocking = !(this.currentTarget.transform == null) && !this.actions && !this.isAttacking;
				base.StartCoroutine(this.ResetBlock());
				this.tryingBlock = false;
			}
			else
			{
				this.tryingBlock = false;
			}
			yield break;
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000F7EF8 File Offset: 0x000F60F8
		protected IEnumerator ResetBlock()
		{
			yield return new WaitForSeconds((this.currentTarget.transform == null) ? 0f : this.raiseShield);
			this.isBlocking = false;
			yield break;
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000F7F07 File Offset: 0x000F6107
		protected virtual void SetAggressive(bool value)
		{
			this.agressiveAtFirstSight = value;
			this.onSetAgressive.Invoke(value);
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000F7F1C File Offset: 0x000F611C
		private int GetDamageResult(int damage, float defenseRate)
		{
			return (int)((float)damage - (float)damage * defenseRate / 100f);
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000F7F2C File Offset: 0x000F612C
		public override void ResetRagdoll()
		{
			this.oldPosition = base.transform.position;
			this.ragdolled = false;
			this._capsuleCollider.isTrigger = false;
			this._rigidbody.isKinematic = false;
			this.agent.updatePosition = false;
			this.agent.enabled = true;
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000F7F81 File Offset: 0x000F6181
		public override void EnableRagdoll()
		{
			this.agent.enabled = false;
			this.agent.updatePosition = false;
			this.ragdolled = true;
			this._rigidbody.isKinematic = true;
			this._capsuleCollider.isTrigger = true;
		}

		// Token: 0x04002B6C RID: 11116
		[vEditorToolbar("Layers", false, "", false, false)]
		[Tooltip("Layers that the character can walk on")]
		public LayerMask groundLayer = 1;

		// Token: 0x04002B6D RID: 11117
		[Tooltip("Distance to became not grounded")]
		[SerializeField]
		protected float groundCheckDistance = 0.5f;

		// Token: 0x04002B6E RID: 11118
		[Tooltip("What objects can make the character auto crouch")]
		public LayerMask autoCrouchLayer = 1;

		// Token: 0x04002B6F RID: 11119
		[Tooltip("[SPHERECAST] ADJUST IN PLAY MODE - White Spherecast put just above the head, this will make the character Auto-Crouch if something hit the sphere.")]
		public float headDetect = 0.95f;

		// Token: 0x04002B70 RID: 11120
		[vEditorToolbar("Locomotion", false, "", false, false)]
		[Tooltip("Use to limit your locomotion animation, if you want to patrol walking set this value to 0.5f")]
		[Range(0f, 1.5f)]
		public float patrolSpeed = 0.5f;

		// Token: 0x04002B71 RID: 11121
		[Tooltip("Use to limit your locomotion animation, if you want to wander walking set this value to 0.5f")]
		[Range(0f, 1.5f)]
		public float wanderSpeed = 0.5f;

		// Token: 0x04002B72 RID: 11122
		[Tooltip("Use to limit your locomotion animation, if you want to chase the target walking set this value to 0.5f")]
		[Range(0f, 1.5f)]
		public float chaseSpeed = 1f;

		// Token: 0x04002B73 RID: 11123
		[Tooltip("Use to limit your locomotion animation, if you want to strafe the target walking set this value to 0.5f")]
		[Range(0f, 1.5f)]
		public float strafeSpeed = 1f;

		// Token: 0x04002B74 RID: 11124
		[Header("--- Strafe ---")]
		[Tooltip("Strafe around the target")]
		public bool strafeSideways = true;

		// Token: 0x04002B75 RID: 11125
		[Tooltip("Strafe a few steps backwards")]
		public bool strafeBackward = true;

		// Token: 0x04002B76 RID: 11126
		[Tooltip("Distance to switch to the strafe locomotion, leave with 0 if you don't want your character to strafe")]
		public float strafeDistance = 3f;

		// Token: 0x04002B77 RID: 11127
		[Tooltip("Min time to change the strafe direction")]
		public float minStrafeSwape = 2f;

		// Token: 0x04002B78 RID: 11128
		[Tooltip("Max time to change the strafe direction")]
		public float maxStrafeSwape = 5f;

		// Token: 0x04002B79 RID: 11129
		[Tooltip("Velocity to rotate the character while strafing")]
		public float strafeRotationSpeed = 5f;

		// Token: 0x04002B7A RID: 11130
		[vEditorToolbar("Detection", false, "", false, false)]
		protected vSimpleMeleeAI_Motor.AIStates _currentState = vSimpleMeleeAI_Motor.AIStates.PatrolWaypoints;

		// Token: 0x04002B7B RID: 11131
		[Header("Who is your Target?")]
		public vSimpleMeleeAI_SphereSensor sphereSensor;

		// Token: 0x04002B7C RID: 11132
		public vTagMask tagsToDetect = new vTagMask { "Player" };

		// Token: 0x04002B7D RID: 11133
		public LayerMask layersToDetect = 1;

		// Token: 0x04002B7E RID: 11134
		public LayerMask obstaclesLayer;

		// Token: 0x04002B7F RID: 11135
		public bool sortTargetFromDistance;

		// Token: 0x04002B80 RID: 11136
		[Range(0f, 360f)]
		public float fieldOfView = 95f;

		// Token: 0x04002B81 RID: 11137
		[Tooltip("Max Distance to detect the Target with FOV")]
		public float maxDetectDistance = 5f;

		// Token: 0x04002B82 RID: 11138
		[Tooltip("Min Distance to noticed the Target without FOV")]
		public float minDetectDistance = 2f;

		// Token: 0x04002B83 RID: 11139
		[Tooltip("Distance to lost the Target")]
		public float distanceToLostTarget = 5f;

		// Token: 0x04002B84 RID: 11140
		[Tooltip("Distance to stop when chasing the Player")]
		public float chaseStopDistance = 1f;

		// Token: 0x04002B85 RID: 11141
		public bool drawAgentPath;

		// Token: 0x04002B86 RID: 11142
		public bool displayGizmos;

		// Token: 0x04002B87 RID: 11143
		[vEditorToolbar("Combat", false, "", false, false)]
		[Tooltip("Check if you want the Enemy to be passive even if you attack him")]
		public bool passiveToDamage;

		// Token: 0x04002B88 RID: 11144
		[Tooltip("Check if you want the Enemy to chase the Target at first sight")]
		public bool agressiveAtFirstSight = true;

		// Token: 0x04002B89 RID: 11145
		[Tooltip("Velocity to rotate the character while attacking")]
		public float attackRotationSpeed = 0.5f;

		// Token: 0x04002B8A RID: 11146
		[Tooltip("Delay to trigger the first attack when close to the target")]
		public float firstAttackDelay;

		// Token: 0x04002B8B RID: 11147
		[Tooltip("Min frequency to attack")]
		public float minTimeToAttack = 4f;

		// Token: 0x04002B8C RID: 11148
		[Tooltip("Max frequency to attack")]
		public float maxTimeToAttack = 6f;

		// Token: 0x04002B8D RID: 11149
		[Tooltip("How many attacks the AI will make on a combo")]
		public int maxAttackCount = 3;

		// Token: 0x04002B8E RID: 11150
		[Tooltip("Randomly attacks based on the maxAttackCount")]
		public bool randomAttackCount = true;

		// Token: 0x04002B8F RID: 11151
		[Range(0f, 1f)]
		public float chanceToRoll = 0.1f;

		// Token: 0x04002B90 RID: 11152
		[Range(0f, 1f)]
		public float chanceToBlockInStrafe = 0.1f;

		// Token: 0x04002B91 RID: 11153
		[Range(0f, 1f)]
		public float chanceToBlockAttack;

		// Token: 0x04002B92 RID: 11154
		[Tooltip("How much time the character will stand up the shield")]
		public float raiseShield = 4f;

		// Token: 0x04002B93 RID: 11155
		[Tooltip("How much time the character will lower the shield")]
		public float lowerShield = 2f;

		// Token: 0x04002B94 RID: 11156
		[vEditorToolbar("Waypoint", false, "", false, false)]
		[Tooltip("Max Distance to change waypoint")]
		[Range(0.5f, 100f)]
		public float distanceToChangeWaypoint = 1f;

		// Token: 0x04002B95 RID: 11157
		[Tooltip("Min Distance to stop when Patrolling through waypoints")]
		[Range(0.5f, 100f)]
		public float patrollingStopDistance = 0.5f;

		// Token: 0x04002B96 RID: 11158
		public vSimpleMeleeAI_Motor.AIPatrolWithOutAreaStyle patrolWithoutAreaStyle;

		// Token: 0x04002B97 RID: 11159
		public vWaypointArea pathArea;

		// Token: 0x04002B98 RID: 11160
		public bool randomWaypoints;

		// Token: 0x04002B99 RID: 11161
		public vFisherYatesRandom randomWaypoint = new vFisherYatesRandom();

		// Token: 0x04002B9A RID: 11162
		public vFisherYatesRandom randomPatrolPoint = new vFisherYatesRandom();

		// Token: 0x04002B9B RID: 11163
		[HideInInspector]
		public CapsuleCollider _capsuleCollider;

		// Token: 0x04002B9C RID: 11164
		[HideInInspector]
		public v_SpriteHealth healthSlider;

		// Token: 0x04002B9D RID: 11165
		[HideInInspector]
		public vMeleeManager meleeManager;

		// Token: 0x04002B9E RID: 11166
		public vSimpleMeleeAI_Motor.OnSetAgressiveEvent onSetAgressive = new vSimpleMeleeAI_Motor.OnSetAgressiveEvent();

		// Token: 0x04002B9F RID: 11167
		[HideInInspector]
		public bool lockMovement;

		// Token: 0x04002BA0 RID: 11168
		public vSimpleMeleeAI_Motor.CharacterTarget currentTarget;

		// Token: 0x04002BA1 RID: 11169
		protected Vector3 _targetPos;

		// Token: 0x04002BA2 RID: 11170
		[SerializeField]
		[vReadOnly(true)]
		protected bool _canSeeTarget;

		// Token: 0x04002BA3 RID: 11171
		protected Vector3 _destination;

		// Token: 0x04002BA4 RID: 11172
		protected Vector3 _fwd;

		// Token: 0x04002BA6 RID: 11174
		protected bool _isStrafing;

		// Token: 0x04002BAC RID: 11180
		protected float _direction;

		// Token: 0x04002BAD RID: 11181
		protected float _timer;

		// Token: 0x04002BAE RID: 11182
		protected float _wait;

		// Token: 0x04002BAF RID: 11183
		protected float _fovAngle;

		// Token: 0x04002BB4 RID: 11188
		protected Vector3 _startPosition;

		// Token: 0x04002BB5 RID: 11189
		protected RaycastHit groundHit;

		// Token: 0x04002BB6 RID: 11190
		protected NavMeshAgent agent;

		// Token: 0x04002BB7 RID: 11191
		protected NavMeshPath agentPath;

		// Token: 0x04002BB8 RID: 11192
		protected Quaternion freeRotation;

		// Token: 0x04002BB9 RID: 11193
		protected Quaternion desiredRotation;

		// Token: 0x04002BBA RID: 11194
		protected Vector3 oldPosition;

		// Token: 0x04002BBB RID: 11195
		protected Vector3 combatMovement;

		// Token: 0x04002BBC RID: 11196
		protected Vector3 rollDirection;

		// Token: 0x04002BBD RID: 11197
		protected Rigidbody _rigidbody;

		// Token: 0x04002BBE RID: 11198
		protected PhysicMaterial frictionPhysics;

		// Token: 0x04002BBF RID: 11199
		protected Transform head;

		// Token: 0x04002BC0 RID: 11200
		protected Collider colliderTarget;

		// Token: 0x04002BC1 RID: 11201
		protected vWaypoint targetWaypoint;

		// Token: 0x04002BC2 RID: 11202
		protected vPoint targetPatrolPoint;

		// Token: 0x04002BC3 RID: 11203
		protected List<vPoint> visitedPatrolPoint = new List<vPoint>();

		// Token: 0x04002BC4 RID: 11204
		protected List<vWaypoint> visitedWaypoint = new List<vWaypoint>();

		// Token: 0x04002BC5 RID: 11205
		protected bool _isCrouched;

		// Token: 0x04002BC6 RID: 11206
		protected bool _canAttack;

		// Token: 0x04002BC7 RID: 11207
		protected bool _tryingBlock;

		// Token: 0x04002BC8 RID: 11208
		protected bool _isRolling;

		// Token: 0x04002BC9 RID: 11209
		protected bool _isBlocking;

		// Token: 0x04002BCA RID: 11210
		protected bool _isAttacking;

		// Token: 0x04002BCB RID: 11211
		protected bool _actions;

		// Token: 0x04002BCC RID: 11212
		private int canSeeTargetIteration;

		// Token: 0x020006DF RID: 1759
		public class OnSetAgressiveEvent : UnityEvent<bool>
		{
		}

		// Token: 0x020006E0 RID: 1760
		public enum AIPatrolWithOutAreaStyle
		{
			// Token: 0x04002BCE RID: 11214
			GoToStartPoint,
			// Token: 0x04002BCF RID: 11215
			Wander,
			// Token: 0x04002BD0 RID: 11216
			Idle
		}

		// Token: 0x020006E1 RID: 1761
		public enum AIStates
		{
			// Token: 0x04002BD2 RID: 11218
			Idle,
			// Token: 0x04002BD3 RID: 11219
			PatrolSubPoints,
			// Token: 0x04002BD4 RID: 11220
			PatrolWaypoints,
			// Token: 0x04002BD5 RID: 11221
			Wander,
			// Token: 0x04002BD6 RID: 11222
			Chase
		}

		// Token: 0x020006E2 RID: 1762
		[Serializable]
		public struct CharacterTarget
		{
			// Token: 0x04002BD7 RID: 11223
			public Transform transform;

			// Token: 0x04002BD8 RID: 11224
			public vIHealthController character;

			// Token: 0x04002BD9 RID: 11225
			public Collider colliderTarget;
		}
	}
}
