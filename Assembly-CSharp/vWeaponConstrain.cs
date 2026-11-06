using System;
using Invector;
using UnityEngine;

// Token: 0x02000134 RID: 308
[vClassHeader(" Weapon Constrain ", "Weapon Constrain Helper: call true OnEquip and false OnDrop by CollectableStandalone events to avoid handler movement on 2018.x", iconName = "meleeIcon")]
public class vWeaponConstrain : vMonoBehaviour
{
	// Token: 0x06000A2C RID: 2604 RVA: 0x000463A1 File Offset: 0x000445A1
	protected virtual void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000463AF File Offset: 0x000445AF
	public virtual void Inv_Weapon_FreezeAll(bool status)
	{
		if (status)
		{
			this.m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			return;
		}
		this.m_Rigidbody.constraints = RigidbodyConstraints.None;
	}

	// Token: 0x0400095E RID: 2398
	private Rigidbody m_Rigidbody;
}
