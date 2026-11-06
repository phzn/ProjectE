using System;
using Invector;
using Invector.vMelee;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class DualSwordExample : vMonoBehaviour
{
	// Token: 0x06000A26 RID: 2598 RVA: 0x00046211 File Offset: 0x00044411
	private void Start()
	{
		this.OnEnable();
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x00046219 File Offset: 0x00044419
	private void OnDestroy()
	{
		this.OnDisable();
		if (this.secundaryWeapon)
		{
			Object.Destroy(this.secundaryWeapon.gameObject);
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00046240 File Offset: 0x00044440
	private void OnEnable()
	{
		if (!this.otherSideTransform)
		{
			Animator componentInParent = base.GetComponentInParent<Animator>();
			if (componentInParent)
			{
				Transform[] componentsInChildren = componentInParent.GetBoneTransform(HumanBodyBones.LeftHand).GetComponentsInChildren<Transform>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].gameObject.name.Equals(this.otherSideHandlerName))
					{
						this.otherSideTransform = componentsInChildren[i];
						break;
					}
				}
			}
		}
		if (this.otherSideTransform)
		{
			this.ActiveSecundaryWeapon();
		}
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x000462C0 File Offset: 0x000444C0
	private void ActiveSecundaryWeapon()
	{
		if (this.secundaryWeapon)
		{
			this.secundaryWeapon.gameObject.SetActive(true);
		}
		else
		{
			this.secundaryWeapon = Object.Instantiate<vMeleeWeapon>(this.secundaryWeaponPrefab);
			this.secundaryWeapon.transform.parent = this.otherSideTransform;
			this.secundaryWeapon.transform.localPosition = Vector3.zero;
			this.secundaryWeapon.transform.localEulerAngles = Vector3.zero;
		}
		if (!this.manager)
		{
			this.manager = base.GetComponentInParent<vMeleeManager>();
		}
		if (this.manager)
		{
			this.manager.SetLeftWeapon(this.secundaryWeapon);
		}
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00046375 File Offset: 0x00044575
	private void OnDisable()
	{
		if (this.secundaryWeapon)
		{
			this.secundaryWeapon.gameObject.SetActive(false);
			this.manager.leftWeapon = null;
		}
	}

	// Token: 0x04000959 RID: 2393
	public vMeleeWeapon secundaryWeaponPrefab;

	// Token: 0x0400095A RID: 2394
	public string otherSideHandlerName;

	// Token: 0x0400095B RID: 2395
	[vReadOnly(true)]
	[SerializeField]
	protected vMeleeWeapon secundaryWeapon;

	// Token: 0x0400095C RID: 2396
	[vReadOnly(true)]
	[SerializeField]
	protected Transform otherSideTransform;

	// Token: 0x0400095D RID: 2397
	[vReadOnly(true)]
	[SerializeField]
	protected vMeleeManager manager;
}
