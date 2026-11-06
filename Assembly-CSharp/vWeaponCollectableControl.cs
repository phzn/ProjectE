using System;
using Invector.vCharacterController.vActions;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000136 RID: 310
public class vWeaponCollectableControl : MonoBehaviour
{
	// Token: 0x06000A30 RID: 2608 RVA: 0x000463D0 File Offset: 0x000445D0
	public virtual void Start()
	{
		vCollectableStandalone component = base.GetComponent<vCollectableStandalone>();
		if (component)
		{
			component.OnEquip.AddListener(new UnityAction(this.OnEquip));
			component.OnDrop.AddListener(new UnityAction(this.OnDrop));
		}
		if (this._rigidbody == null)
		{
			this._rigidbody = base.GetComponentInParent<Rigidbody>();
		}
		if (this._physicsCollider == null)
		{
			this._physicsCollider = base.GetComponentInParent<Collider>();
		}
		if (this._selfCollider == null)
		{
			this._selfCollider = base.GetComponentInChildren<Collider>();
		}
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0004646C File Offset: 0x0004466C
	public virtual void OnEquip()
	{
		if (this._rigidbody != null)
		{
			this._rigidbody.isKinematic = true;
		}
		if (this._physicsCollider != null)
		{
			this._physicsCollider.enabled = false;
		}
		if (this._selfCollider != null)
		{
			this._selfCollider.enabled = false;
		}
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x000464C8 File Offset: 0x000446C8
	public virtual void OnDrop()
	{
		if (this._rigidbody != null)
		{
			this._rigidbody.isKinematic = false;
		}
		if (this._physicsCollider != null)
		{
			this._physicsCollider.enabled = true;
		}
		if (this._selfCollider != null)
		{
			this._selfCollider.enabled = true;
		}
	}

	// Token: 0x04000962 RID: 2402
	[vHelpBox("This component is used for NO INVENTORY weapons", vHelpBoxAttribute.MessageType.Info)]
	public Rigidbody _rigidbody;

	// Token: 0x04000963 RID: 2403
	public Collider _physicsCollider;

	// Token: 0x04000964 RID: 2404
	public Collider _selfCollider;
}
