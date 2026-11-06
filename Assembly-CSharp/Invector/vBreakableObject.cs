using System;
using System.Collections;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x020004D9 RID: 1241
	public class vBreakableObject : MonoBehaviour, vIDamageReceiver
	{
		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x000B6BD1 File Offset: 0x000B4DD1
		// (set) Token: 0x06001DE6 RID: 7654 RVA: 0x000B6BD9 File Offset: 0x000B4DD9
		public OnReceiveDamage onStartReceiveDamage
		{
			get
			{
				return this._onStartReceiveDamage;
			}
			protected set
			{
				this._onStartReceiveDamage = value;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x000B6BE2 File Offset: 0x000B4DE2
		// (set) Token: 0x06001DE8 RID: 7656 RVA: 0x000B6BEA File Offset: 0x000B4DEA
		public OnReceiveDamage onReceiveDamage
		{
			get
			{
				return this._onReceiveDamage;
			}
			protected set
			{
				this._onReceiveDamage = value;
			}
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000B6BF3 File Offset: 0x000B4DF3
		protected virtual void Start()
		{
			this._collider = base.GetComponent<Collider>();
			this._rigidBody = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x000B6C10 File Offset: 0x000B4E10
		public void TakeDamage(vDamage damage)
		{
			this.onStartReceiveDamage.Invoke(damage);
			if (!this.isBroken)
			{
				this.isBroken = true;
				base.StartCoroutine(this.BreakObject());
			}
			if (damage.damageValue > 0f)
			{
				this.onReceiveDamage.Invoke(damage);
			}
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x000B6C5E File Offset: 0x000B4E5E
		protected virtual IEnumerator BreakObject()
		{
			if (this._rigidBody)
			{
				Object.Destroy(this._rigidBody);
			}
			if (this._collider)
			{
				Object.Destroy(this._collider);
			}
			yield return new WaitForEndOfFrame();
			this.brokenObject.transform.parent = null;
			this.brokenObject.gameObject.SetActive(true);
			this.OnBroken.Invoke();
			Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x000B6C70 File Offset: 0x000B4E70
		protected virtual void OnTriggerStay(Collider other)
		{
			if (this.breakOnPlayerRoll && other.gameObject.CompareTag("Player"))
			{
				vThirdPersonController component = other.gameObject.GetComponent<vThirdPersonController>();
				if (component && component.isRolling && !this.isBroken)
				{
					this.isBroken = true;
					base.StartCoroutine(this.BreakObject());
				}
			}
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x000B6CD0 File Offset: 0x000B4ED0
		protected virtual void OnCollisionEnter(Collision other)
		{
			if (this.breakOnCollision && this._rigidBody && this._rigidBody.velocity.magnitude > 5f && !this.isBroken)
			{
				this.isBroken = true;
				base.StartCoroutine(this.BreakObject());
			}
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000AD00A File Offset: 0x000AB20A
		Transform vIDamageReceiver.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000B0592 File Offset: 0x000AE792
		GameObject vIDamageReceiver.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x04001EF8 RID: 7928
		public Transform brokenObject;

		// Token: 0x04001EF9 RID: 7929
		[Header("Break Object Settings")]
		[Tooltip("Break objet  OnTrigger with Player rolling")]
		public bool breakOnPlayerRoll = true;

		// Token: 0x04001EFA RID: 7930
		[Tooltip("Break objet  OnCollision with other object")]
		public bool breakOnCollision = true;

		// Token: 0x04001EFB RID: 7931
		[Tooltip("Rigidbody velocity to break OnCollision whit other object")]
		public float maxVelocityToBreak = 5f;

		// Token: 0x04001EFC RID: 7932
		public UnityEvent OnBroken;

		// Token: 0x04001EFD RID: 7933
		[SerializeField]
		protected OnReceiveDamage _onStartReceiveDamage = new OnReceiveDamage();

		// Token: 0x04001EFE RID: 7934
		[SerializeField]
		protected OnReceiveDamage _onReceiveDamage = new OnReceiveDamage();

		// Token: 0x04001EFF RID: 7935
		protected bool isBroken;

		// Token: 0x04001F00 RID: 7936
		protected Collider _collider;

		// Token: 0x04001F01 RID: 7937
		protected Rigidbody _rigidBody;
	}
}
