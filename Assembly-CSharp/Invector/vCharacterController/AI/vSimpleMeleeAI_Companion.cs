using System;
using System.Collections;
using System.Text;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Invector.vCharacterController.AI
{
	// Token: 0x020006CB RID: 1739
	public class vSimpleMeleeAI_Companion : vSimpleMeleeAI_Controller
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x000F4A59 File Offset: 0x000F2C59
		// (set) Token: 0x06002BEA RID: 11242 RVA: 0x000F4A61 File Offset: 0x000F2C61
		public virtual string companionTag
		{
			get
			{
				return this._companionTag;
			}
			set
			{
				this._companionTag = value;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002BEB RID: 11243 RVA: 0x000F4A6A File Offset: 0x000F2C6A
		// (set) Token: 0x06002BEC RID: 11244 RVA: 0x000F4A72 File Offset: 0x000F2C72
		public virtual float companionMaxDistance
		{
			get
			{
				return this._companionMaxDistance;
			}
			set
			{
				this._companionMaxDistance = value;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002BED RID: 11245 RVA: 0x000F4A7B File Offset: 0x000F2C7B
		// (set) Token: 0x06002BEE RID: 11246 RVA: 0x000F4A83 File Offset: 0x000F2C83
		public virtual float followSpeed
		{
			get
			{
				return this._followSpeed;
			}
			set
			{
				this._followSpeed = value;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002BEF RID: 11247 RVA: 0x000F4A8C File Offset: 0x000F2C8C
		// (set) Token: 0x06002BF0 RID: 11248 RVA: 0x000F4A94 File Offset: 0x000F2C94
		public virtual float followStopDistance
		{
			get
			{
				return this._followStopDistance;
			}
			set
			{
				this._followStopDistance = value;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002BF1 RID: 11249 RVA: 0x000F4A9D File Offset: 0x000F2C9D
		// (set) Token: 0x06002BF2 RID: 11250 RVA: 0x000F4AA5 File Offset: 0x000F2CA5
		public virtual float moveToStopDistance
		{
			get
			{
				return this._moveToStopDistance;
			}
			set
			{
				this._moveToStopDistance = value;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x000F4AAE File Offset: 0x000F2CAE
		// (set) Token: 0x06002BF4 RID: 11252 RVA: 0x000F4AB6 File Offset: 0x000F2CB6
		public virtual Transform moveToTarget
		{
			get
			{
				return this._moveToTarget;
			}
			set
			{
				this._moveToTarget = value;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002BF5 RID: 11253 RVA: 0x000F4ABF File Offset: 0x000F2CBF
		// (set) Token: 0x06002BF6 RID: 11254 RVA: 0x000F4AC7 File Offset: 0x000F2CC7
		public virtual vSimpleMeleeAI_Companion.CompanionState companionState
		{
			get
			{
				return this._companionState;
			}
			set
			{
				this._companionState = value;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x000F4AD0 File Offset: 0x000F2CD0
		// (set) Token: 0x06002BF8 RID: 11256 RVA: 0x000F4AD8 File Offset: 0x000F2CD8
		public virtual Transform companion
		{
			get
			{
				return this._companion;
			}
			set
			{
				this._companion = value;
			}
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000F4AE1 File Offset: 0x000F2CE1
		protected void LateUpdate()
		{
			this.CompanionInputs();
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000F4AEC File Offset: 0x000F2CEC
		protected virtual void CompanionInputs()
		{
			if (GameplayInput.GetKeyDown(KeyCode.Alpha1))
			{
				this.companionState = vSimpleMeleeAI_Companion.CompanionState.Stay;
				this.agressiveAtFirstSight = false;
			}
			if (GameplayInput.GetKeyDown(KeyCode.Alpha2))
			{
				this.companionState = vSimpleMeleeAI_Companion.CompanionState.Follow;
				this.agressiveAtFirstSight = false;
			}
			if (GameplayInput.GetKeyDown(KeyCode.Alpha3))
			{
				this.agressiveAtFirstSight = !this.agressiveAtFirstSight;
			}
			if (GameplayInput.GetKeyDown(KeyCode.Alpha4) && this.moveToTarget != null)
			{
				this.SetMoveTo(this.moveToTarget);
				this.companionState = vSimpleMeleeAI_Companion.CompanionState.MoveTo;
				this.agressiveAtFirstSight = false;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002BFB RID: 11259 RVA: 0x000F4B70 File Offset: 0x000F2D70
		protected virtual float companionDistance
		{
			get
			{
				if (!(this.companion != null))
				{
					return 0f;
				}
				return Vector3.Distance(base.transform.position, this.companion.transform.position);
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002BFC RID: 11260 RVA: 0x000F4BA8 File Offset: 0x000F2DA8
		protected virtual bool nearOfCompanion
		{
			get
			{
				return (this.companion != null && this.companion.gameObject.activeSelf && this.companionDistance < this.companionMaxDistance) || this.companion == null || !this.companion.gameObject.activeSelf;
			}
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000F4C08 File Offset: 0x000F2E08
		public virtual void SetMoveTo(Transform _target)
		{
			this.companionState = vSimpleMeleeAI_Companion.CompanionState.MoveTo;
			this.moveToTarget = _target;
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000F4C18 File Offset: 0x000F2E18
		protected override void Start()
		{
			try
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag(this.companionTag);
				if (gameObject != null)
				{
					this.companion = gameObject.transform;
				}
				else
				{
					this.companionState = vSimpleMeleeAI_Companion.CompanionState.None;
					Debug.LogWarning("Cant find the " + this.companionTag);
				}
			}
			catch (UnityException ex)
			{
				this.companionState = vSimpleMeleeAI_Companion.CompanionState.None;
				Debug.LogWarning("AICompanion Cant find the " + this.companionTag);
				Debug.LogWarning("AICompanion " + ex.Message);
			}
			this.Init();
			this.agent.enabled = true;
			base.StartCoroutine(this.CompanionStateRoutine());
			base.StartCoroutine(base.FindTarget());
			base.StartCoroutine(this.DestinationBehaviour());
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000F4CE4 File Offset: 0x000F2EE4
		protected IEnumerator CompanionStateRoutine()
		{
			while (base.enabled)
			{
				yield return new WaitForEndOfFrame();
				StringBuilder debugString = new StringBuilder();
				debugString.AppendLine("----DEBUG----");
				debugString.AppendLine("Agressive : " + this.agressiveAtFirstSight.ToString());
				base.CheckIsOnNavMesh();
				base.CheckAutoCrouch();
				base.SetTarget();
				switch (this.companionState)
				{
				case vSimpleMeleeAI_Companion.CompanionState.None:
					debugString.AppendLine("None : using normal AI routine");
					switch (this.currentState)
					{
					case vSimpleMeleeAI_Motor.AIStates.Idle:
						debugString.AppendLine("idle");
						yield return base.StartCoroutine(base.Idle());
						break;
					case vSimpleMeleeAI_Motor.AIStates.Wander:
						yield return base.StartCoroutine(base.Wander());
						break;
					case vSimpleMeleeAI_Motor.AIStates.Chase:
						yield return base.StartCoroutine(base.Chase());
						break;
					}
					break;
				case vSimpleMeleeAI_Companion.CompanionState.Follow:
					if (this.canSeeTarget && this.nearOfCompanion)
					{
						yield return base.StartCoroutine(base.Chase());
					}
					else
					{
						yield return base.StartCoroutine(this.FollowCompanion());
					}
					debugString.AppendLine((this.canSeeTarget && this.nearOfCompanion) ? "Chase/Follow" : "Follow");
					break;
				case vSimpleMeleeAI_Companion.CompanionState.MoveTo:
					if (this.canSeeTarget)
					{
						yield return base.StartCoroutine(base.Chase());
					}
					else
					{
						yield return base.StartCoroutine(this.MoveTo());
					}
					debugString.AppendLine(this.canSeeTarget ? "Chase/MoveTo" : "MoveTo");
					break;
				case vSimpleMeleeAI_Companion.CompanionState.Stay:
					if (this.canSeeTarget)
					{
						yield return base.StartCoroutine(base.Chase());
					}
					else
					{
						yield return base.StartCoroutine(this.Stay());
					}
					debugString.AppendLine(this.canSeeTarget ? "Chase/Stay" : "Stay");
					break;
				}
				if (this.debugUIText != null && this.debug)
				{
					this.debugUIText.text = debugString.ToString();
				}
				debugString = null;
			}
			yield break;
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000F4CF3 File Offset: 0x000F2EF3
		protected IEnumerator Stay()
		{
			if (this.companion != null)
			{
				this.agent.speed = Mathf.Lerp(this.agent.speed, 0f, 2f * Time.deltaTime);
			}
			else
			{
				yield return base.StartCoroutine(base.Idle());
			}
			yield break;
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x000F4D02 File Offset: 0x000F2F02
		protected override void SetAggressive(bool value)
		{
			if (this.companionState != vSimpleMeleeAI_Companion.CompanionState.Follow)
			{
				base.SetAggressive(value);
			}
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x000F4D14 File Offset: 0x000F2F14
		protected virtual IEnumerator FollowCompanion()
		{
			while (!this.agent.enabled || this.currentHealth <= 0f)
			{
				yield return null;
			}
			if (this.companion != null && this.companion.gameObject.activeSelf)
			{
				this.agent.speed = Mathf.Lerp(this.agent.speed, this.followSpeed, 10f * Time.deltaTime);
				this.agent.stoppingDistance = this.followStopDistance;
				this.UpdateDestination(this.companion.position);
			}
			else
			{
				this.agent.speed = Mathf.Lerp(this.agent.speed, this.moveToSpeed, 10f * Time.deltaTime);
				this.agent.stoppingDistance = this.moveToStopDistance;
				this.UpdateDestination(this.startPosition);
			}
			yield break;
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x000F4D23 File Offset: 0x000F2F23
		protected virtual IEnumerator MoveTo()
		{
			while (!this.agent.enabled || this.currentHealth <= 0f)
			{
				yield return null;
			}
			this.agent.speed = Mathf.Lerp(this.agent.speed, this.moveToSpeed, 2f * Time.deltaTime);
			this.agent.stoppingDistance = this.moveToStopDistance;
			this.UpdateDestination(this.moveToTarget.position);
			if (this.canSeeTarget && this.nearOfCompanion)
			{
				this.currentState = vSimpleMeleeAI_Motor.AIStates.Chase;
			}
			yield break;
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002C04 RID: 11268 RVA: 0x000F4D32 File Offset: 0x000F2F32
		protected override float maxSpeed
		{
			get
			{
				if (this.companionState == vSimpleMeleeAI_Companion.CompanionState.None)
				{
					return base.maxSpeed;
				}
				if (this.companionState == vSimpleMeleeAI_Companion.CompanionState.Follow)
				{
					return this.followSpeed;
				}
				if (this.companionState != vSimpleMeleeAI_Companion.CompanionState.MoveTo)
				{
					return 0f;
				}
				return this.moveToSpeed;
			}
		}

		// Token: 0x04002B21 RID: 11041
		[vEditorToolbar("Companion", false, "", false, false)]
		[SerializeField]
		protected string _companionTag = "Player";

		// Token: 0x04002B22 RID: 11042
		[SerializeField]
		protected float _companionMaxDistance = 10f;

		// Token: 0x04002B23 RID: 11043
		[Range(0f, 1.5f)]
		[SerializeField]
		protected float _followSpeed = 1f;

		// Token: 0x04002B24 RID: 11044
		[SerializeField]
		protected float _followStopDistance = 2f;

		// Token: 0x04002B25 RID: 11045
		[Range(0f, 1.5f)]
		[SerializeField]
		protected float _moveToStopDistance = 0.5f;

		// Token: 0x04002B26 RID: 11046
		[SerializeField]
		protected Transform _moveToTarget;

		// Token: 0x04002B27 RID: 11047
		[SerializeField]
		protected vSimpleMeleeAI_Companion.CompanionState _companionState = vSimpleMeleeAI_Companion.CompanionState.Follow;

		// Token: 0x04002B28 RID: 11048
		[SerializeField]
		protected Transform _companion;

		// Token: 0x04002B29 RID: 11049
		public bool debug = true;

		// Token: 0x04002B2A RID: 11050
		public Text debugUIText;

		// Token: 0x020006CC RID: 1740
		public enum CompanionState
		{
			// Token: 0x04002B2C RID: 11052
			None,
			// Token: 0x04002B2D RID: 11053
			Follow,
			// Token: 0x04002B2E RID: 11054
			MoveTo,
			// Token: 0x04002B2F RID: 11055
			Stay
		}
	}
}
