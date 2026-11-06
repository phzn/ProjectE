using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x0200036F RID: 879
	[AddComponentMenu("")]
	public class Barbarian : MonoBehaviour
	{
		// Token: 0x06001836 RID: 6198 RVA: 0x00066E80 File Offset: 0x00065080
		protected virtual void Awake()
		{
			this._initialPosition = base.transform.position;
			this._initialLookAtTarget = base.transform.position + base.transform.forward * 10f;
			this._lookAtTarget = this._initialLookAtTarget;
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00066ED5 File Offset: 0x000650D5
		protected virtual void Update()
		{
			this.HandleInput();
			this.LookAtTarget();
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x00066EE4 File Offset: 0x000650E4
		protected virtual void LookAtTarget()
		{
			Vector3 vector = this._lookAtTarget - this._initialPosition;
			base.transform.LookAt(this._lookAtTarget + vector * 5f);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00066F24 File Offset: 0x00065124
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame())
			{
				this.Attack();
			}
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x00066F34 File Offset: 0x00065134
		protected virtual void Attack()
		{
			if (Time.time - this._lastAttackStartedAt >= this.CooldownDuration + this.AttackDuration)
			{
				this.AcquireTargets();
				base.StartCoroutine(this.AttackCoroutine());
				this._lastAttackStartedAt = Time.time;
				return;
			}
			MMFeedbacks deniedFeedback = this.DeniedFeedback;
			if (deniedFeedback == null)
			{
				return;
			}
			deniedFeedback.PlayFeedbacks();
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00066F8C File Offset: 0x0006518C
		protected virtual void AcquireTargets()
		{
			this._targets = new List<Vector3>();
			foreach (Collider collider in Physics.OverlapSphere(base.transform.position, 5f))
			{
				Vector3 position = collider.transform.position;
				Vector3 vector = base.transform.position - position;
				if (collider.GetComponent<BarbarianEnemy>() != null)
				{
					this._targets.Add(position + vector * this.AttackPositionOffset);
				}
			}
			this._targets.MMShuffle<Vector3>();
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0006701E File Offset: 0x0006521E
		protected virtual IEnumerator AttackCoroutine()
		{
			float intervalDuration = this.AttackDuration / (float)this._targets.Count;
			MMFeedbacks attackFeedback = this.AttackFeedback;
			if (attackFeedback != null)
			{
				attackFeedback.PlayFeedbacks();
			}
			int enemyCounter = 0;
			foreach (Vector3 vector in this._targets)
			{
				MMFeedbacks individualAttackFeedback = this.IndividualAttackFeedback;
				if (individualAttackFeedback != null)
				{
					individualAttackFeedback.PlayFeedbacks();
				}
				MMTween.MoveTransform(this, base.transform, base.transform.position, vector, null, 0f, intervalDuration, this.AttackCurve, false);
				this._lookAtTarget = vector;
				yield return MMCoroutine.WaitFor(intervalDuration - (float)enemyCounter * this.IntervalDecrement);
				int num = enemyCounter;
				enemyCounter = num + 1;
			}
			List<Vector3>.Enumerator enumerator = default(List<Vector3>.Enumerator);
			MMTween.MoveTransform(this, base.transform, base.transform.position, this._initialPosition, null, 0f, intervalDuration, this.AttackCurve, false);
			this._lookAtTarget = this._initialLookAtTarget;
			yield break;
			yield break;
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00067030 File Offset: 0x00065230
		protected virtual void OnTriggerEnter(Collider other)
		{
			this._enemy = other.GetComponent<BarbarianEnemy>();
			if (this._enemy != null)
			{
				int num = Random.Range(50, 250);
				this._enemy.TakeDamage(num);
			}
		}

		// Token: 0x04001C0F RID: 7183
		[Header("Cooldown")]
		[Tooltip("a duration, in seconds, between two attacks, during which attacks are prevented")]
		public float CooldownDuration = 0.1f;

		// Token: 0x04001C10 RID: 7184
		[Header("Feedbacks")]
		[Tooltip("a feedback to call when the attack starts")]
		public MMFeedbacks AttackFeedback;

		// Token: 0x04001C11 RID: 7185
		[Tooltip("a feedback to call when each individual attack phase starts")]
		public MMFeedbacks IndividualAttackFeedback;

		// Token: 0x04001C12 RID: 7186
		[Tooltip("a feedback to call when trying to attack while in cooldown")]
		public MMFeedbacks DeniedFeedback;

		// Token: 0x04001C13 RID: 7187
		[Header("Attack settings")]
		public MMTween.MMTweenCurve AttackCurve = MMTween.MMTweenCurve.EaseInOutOverhead;

		// Token: 0x04001C14 RID: 7188
		public float AttackDuration = 2.5f;

		// Token: 0x04001C15 RID: 7189
		public float AttackPositionOffset = 0.3f;

		// Token: 0x04001C16 RID: 7190
		public float IntervalDecrement = 0.1f;

		// Token: 0x04001C17 RID: 7191
		protected List<Vector3> _targets;

		// Token: 0x04001C18 RID: 7192
		protected float _lastAttackStartedAt = -100f;

		// Token: 0x04001C19 RID: 7193
		protected Vector3 _initialPosition;

		// Token: 0x04001C1A RID: 7194
		protected Vector3 _initialLookAtTarget;

		// Token: 0x04001C1B RID: 7195
		protected Vector3 _lookAtTarget;

		// Token: 0x04001C1C RID: 7196
		protected BarbarianEnemy _enemy;
	}
}
