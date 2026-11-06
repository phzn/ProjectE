using System;
using UnityEngine;

namespace Invector.vMelee
{
	// Token: 0x0200050A RID: 1290
	[vClassHeader("HitBox", true, "icon_v2", false, "", openClose = false)]
	public class vHitBox : vMonoBehaviour
	{
		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x000BA995 File Offset: 0x000B8B95
		public Collider trigger
		{
			get
			{
				this._trigger = base.gameObject.GetComponent<Collider>();
				if (!this._trigger)
				{
					this._trigger = base.gameObject.AddComponent<BoxCollider>();
				}
				return this._trigger;
			}
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x000BA9CC File Offset: 0x000B8BCC
		private void OnDrawGizmos()
		{
			Color color = (((this.triggerType & vHitBoxType.Damage) != (vHitBoxType)0 && (this.triggerType & vHitBoxType.Recoil) == (vHitBoxType)0) ? Color.green : (((this.triggerType & vHitBoxType.Damage) != (vHitBoxType)0 && (this.triggerType & vHitBoxType.Recoil) != (vHitBoxType)0) ? Color.yellow : (((this.triggerType & vHitBoxType.Recoil) != (vHitBoxType)0 && (this.triggerType & vHitBoxType.Damage) == (vHitBoxType)0) ? Color.red : Color.black)));
			color.a = 0.6f;
			Gizmos.color = color;
			if (!Application.isPlaying && this.trigger && !this.trigger.enabled)
			{
				this.trigger.enabled = true;
			}
			if (this.trigger && this.trigger.enabled && this.trigger as BoxCollider)
			{
				BoxCollider boxCollider = this.trigger as BoxCollider;
				Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.lossyScale);
				Gizmos.DrawCube(boxCollider.center, Vector3.Scale(Vector3.one, boxCollider.size));
			}
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x000BAAF0 File Offset: 0x000B8CF0
		private void Start()
		{
			if (this.trigger)
			{
				this.trigger.isTrigger = true;
				this.trigger.enabled = false;
			}
			int num = LayerMask.NameToLayer("Ignore Raycast");
			base.transform.gameObject.layer = num;
			this.canHit = (this.triggerType & vHitBoxType.Damage) != (vHitBoxType)0 || (this.triggerType & vHitBoxType.Recoil) > (vHitBoxType)0;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x000BAB5C File Offset: 0x000B8D5C
		private void OnTriggerEnter(Collider other)
		{
			if (this.TriggerCondictions(other) && this.attackObject != null)
			{
				this.attackObject.OnHit(this, other);
			}
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x000BAB84 File Offset: 0x000B8D84
		private bool TriggerCondictions(Collider other)
		{
			return this.canHit && this.attackObject != null && (this.attackObject.meleeManager == null || other.gameObject != this.attackObject.meleeManager.gameObject);
		}

		// Token: 0x04001FFA RID: 8186
		[HideInInspector]
		public vMeleeAttackObject attackObject;

		// Token: 0x04001FFB RID: 8187
		public int damagePercentage = 100;

		// Token: 0x04001FFC RID: 8188
		[vEnumFlag]
		public vHitBoxType triggerType = vHitBoxType.Damage | vHitBoxType.Recoil;

		// Token: 0x04001FFD RID: 8189
		protected bool canHit;

		// Token: 0x04001FFE RID: 8190
		protected Collider _trigger;
	}
}
