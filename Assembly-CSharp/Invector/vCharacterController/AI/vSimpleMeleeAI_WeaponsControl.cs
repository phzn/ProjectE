using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vItemManager;
using Invector.vMelee;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.AI
{
	// Token: 0x020006E8 RID: 1768
	public class vSimpleMeleeAI_WeaponsControl : MonoBehaviour
	{
		// Token: 0x06002D07 RID: 11527 RVA: 0x000F87C8 File Offset: 0x000F69C8
		private IEnumerator Start()
		{
			this.itemCollection = base.GetComponentInChildren<vItemCollection>(true);
			this.ai = base.GetComponent<vSimpleMeleeAI_Controller>();
			this.manager = base.GetComponent<vMeleeManager>();
			yield return new WaitForEndOfFrame();
			if (this.itemCollection && this.ai && this.manager)
			{
				this.ai.onSetAgressive.AddListener(new UnityAction<bool>(this.OnSetAgressive));
				this.leftArm = this.ai.animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
				this.rightArm = this.ai.animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
				int j;
				int i;
				for (i = 0; i < this.itemCollection.items.Count; i = j + 1)
				{
					if (this.itemCollection.items[i].amount > 0 && this.itemCollection.itemListData.items.Find((vItem _item) => _item.id == this.itemCollection.items[i].id && _item.type == vItemType.MeleeWeapon) != null)
					{
						this.AddItem(this.itemCollection.items[i].id, this.itemCollection.items[i].amount);
					}
					j = i;
				}
				if (this.useRightWeapon)
				{
					if (this.randomRightWeapon)
					{
						this.GetRandomWeapon(ref this.rightWeaponItem, vMeleeType.OnlyAttack);
					}
					else
					{
						this.GetItemWeapon(this.rightWeaponID, ref this.rightWeaponItem, vMeleeType.OnlyAttack);
					}
				}
				if (this.useLeftWeapon)
				{
					if (this.randomLeftWeapon)
					{
						this.GetRandomWeapon(ref this.leftWeaponItem, vMeleeType.OnlyDefense);
					}
					else
					{
						this.GetItemWeapon(this.leftWeaponID, ref this.leftWeaponItem, vMeleeType.OnlyDefense);
					}
				}
				if (this.rightArm && this.rightWeaponItem)
				{
					Transform transform = null;
					if (this.customEquipPointR.Count > 0)
					{
						transform = this.customEquipPointR.Find((Transform t) => t.name == this.rightWeaponItem.customHandler);
					}
					if (transform == null)
					{
						transform = this.defaultEquipPointR;
					}
					if (transform)
					{
						this.rightWeapon = Object.Instantiate<GameObject>(this.rightWeaponItem.originalObject);
						this.rightWeapon.transform.parent = transform;
						this.rightWeapon.transform.localPosition = Vector3.zero;
						this.rightWeapon.transform.localEulerAngles = Vector3.zero;
						this.manager.SetRightWeapon(this.rightWeapon);
						this.rightWeapon.SetActive(false);
						if (this.ai.agressiveAtFirstSight)
						{
							base.StartCoroutine(this.EquipItemRoutine(false, this.rightWeaponItem, this.rightWeapon));
						}
					}
				}
				if (this.leftArm && this.leftWeaponItem)
				{
					Transform transform2 = null;
					if (this.customEquipPointL.Count > 0)
					{
						transform2 = this.customEquipPointL.Find((Transform t) => t.name == this.leftWeaponItem.customHandler);
					}
					if (transform2 == null)
					{
						transform2 = this.defaultEquipPointL;
					}
					if (transform2)
					{
						this.leftWeapon = Object.Instantiate<GameObject>(this.leftWeaponItem.originalObject);
						this.leftWeapon.transform.parent = transform2;
						this.leftWeapon.transform.localPosition = Vector3.zero;
						this.leftWeapon.transform.localEulerAngles = Vector3.zero;
						Vector3 localScale = this.leftWeapon.transform.localScale;
						localScale.x *= -1f;
						this.leftWeapon.transform.localScale = localScale;
						this.manager.SetLeftWeapon(this.leftWeapon);
						this.leftWeapon.SetActive(false);
						if (this.ai.agressiveAtFirstSight)
						{
							base.StartCoroutine(this.EquipItemRoutine(true, this.leftWeaponItem, this.leftWeapon));
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000F87D8 File Offset: 0x000F69D8
		public void OnSetAgressive(bool value)
		{
			this.timeToStart = 2f;
			if (value)
			{
				if (this.rightWeapon && !this.rightWeapon.activeSelf && !this.changeRight)
				{
					this.changeRight = true;
					base.StartCoroutine(this.EquipItemRoutine(false, this.rightWeaponItem, this.rightWeapon));
				}
				if (this.leftWeapon && !this.leftWeapon.activeSelf && !this.changeLeft)
				{
					this.changeLeft = true;
					base.StartCoroutine(this.EquipItemRoutine(true, this.leftWeaponItem, this.leftWeapon));
					return;
				}
			}
			else
			{
				if (this.rightWeapon && this.rightWeapon.activeSelf)
				{
					base.StartCoroutine(this.UnequipItemRoutine(false, this.rightWeaponItem, this.rightWeapon));
				}
				if (this.leftWeapon && this.leftWeapon.activeSelf)
				{
					base.StartCoroutine(this.UnequipItemRoutine(true, this.leftWeaponItem, this.leftWeapon));
				}
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000F88F0 File Offset: 0x000F6AF0
		private void GetItemWeapon(int id, ref vItem weaponItem, vMeleeType type)
		{
			if (this.weaponItems.Count > 0)
			{
				weaponItem = this.weaponItems.Find((vItem _item) => _item.id == id && _item.originalObject != null && _item.originalObject.GetComponent<vMeleeWeapon>() != null && (_item.originalObject.GetComponent<vMeleeWeapon>().meleeType == vMeleeType.AttackAndDefense || _item.originalObject.GetComponent<vMeleeWeapon>().meleeType == type));
				this.weaponItems.Remove(weaponItem);
			}
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000F8948 File Offset: 0x000F6B48
		private void GetRandomWeapon(ref vItem weaponItem, vMeleeType type)
		{
			if (this.weaponItems.Count > 0)
			{
				List<vItem> list = this.weaponItems.FindAll((vItem _item) => _item.originalObject != null && _item.originalObject.GetComponent<vMeleeWeapon>() != null && (_item.originalObject.GetComponent<vMeleeWeapon>().meleeType == vMeleeType.AttackAndDefense || _item.originalObject.GetComponent<vMeleeWeapon>().meleeType == type));
				Random.InitState(Random.Range(0, DateTime.Now.Millisecond));
				int num = Random.Range(0, list.Count - 1);
				weaponItem = list[num];
				this.weaponItems.Remove(weaponItem);
			}
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000F89C8 File Offset: 0x000F6BC8
		private IEnumerator EquipItemRoutine(bool flipEquip, vItem item, GameObject weapon)
		{
			this.equipCalls++;
			while (this.inEquip || this.timeToStart > 0f || this.ai.ragdolled)
			{
				this.timeToStart -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			if (!this.inUnequip)
			{
				this.inEquip = true;
				if (weapon != null && item != null)
				{
					this.equipTimer = item.enableDelayTime;
					if (item.type != vItemType.Consumable && !this.ai.isDead && !string.IsNullOrEmpty(item.EnableAnim))
					{
						this.ai.animator.SetBool("FlipEquip", flipEquip);
						this.ai.animator.CrossFade(item.EnableAnim, 0.25f);
					}
					if (weapon != null)
					{
						while (this.equipTimer > 0f)
						{
							this.equipTimer -= Time.deltaTime;
							yield return new WaitForEndOfFrame();
						}
						if (!this.ai.isDead)
						{
							weapon.SetActive(true);
						}
					}
				}
				this.inEquip = false;
				this.equipCalls--;
				if (this.equipCalls == 0)
				{
					this.ai.lockMovement = false;
				}
				if (flipEquip)
				{
					this.changeLeft = false;
				}
				else
				{
					this.changeRight = false;
				}
			}
			yield break;
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000F89EC File Offset: 0x000F6BEC
		private IEnumerator UnequipItemRoutine(bool flipEquip, vItem item, GameObject weapon)
		{
			this.ai.lockMovement = true;
			while (this.inUnequip || this.ai.actions || this.ai.isAttacking || this.ai.ragdolled)
			{
				yield return new WaitForEndOfFrame();
			}
			if (!this.inEquip)
			{
				yield return new WaitForSeconds(1f);
				this.inUnequip = true;
				if (weapon != null && item != null)
				{
					this.unequipTimer = item.enableDelayTime;
					if (item.type != vItemType.Consumable && !string.IsNullOrEmpty(item.DisableAnim))
					{
						this.ai.animator.SetBool("FlipEquip", flipEquip);
						this.ai.animator.CrossFade(item.DisableAnim, 0.25f);
					}
					if (weapon != null)
					{
						while (this.unequipTimer > 0f)
						{
							this.unequipTimer -= Time.deltaTime;
							yield return new WaitForEndOfFrame();
						}
						weapon.SetActive(false);
					}
				}
				this.inUnequip = false;
			}
			this.ai.lockMovement = true;
			yield break;
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000F8A10 File Offset: 0x000F6C10
		public void AddItem(int itemID, int amount)
		{
			if (this.itemCollection.itemListData != null && this.itemCollection.itemListData.items.Count > 0)
			{
				vItem item = this.itemCollection.itemListData.items.Find((vItem t) => t.id.Equals(itemID));
				if (item)
				{
					List<vItem> list = this.weaponItems.FindAll((vItem i) => i.stackable && i.id == item.id && i.amount < i.maxStack);
					if (list.Count == 0)
					{
						vItem vItem = Object.Instantiate<vItem>(item);
						vItem.name = vItem.name.Replace("(Clone)", string.Empty);
						vItem.amount = 0;
						int num = 0;
						while (num < item.maxStack && vItem.amount < vItem.maxStack && amount > 0)
						{
							vItem.amount++;
							amount--;
							num++;
						}
						this.weaponItems.Add(vItem);
						if (amount > 0)
						{
							this.AddItem(item.id, amount);
							return;
						}
					}
					else
					{
						int num2 = this.weaponItems.IndexOf(list[0]);
						int num3 = 0;
						while (num3 < this.weaponItems[num2].maxStack && this.weaponItems[num2].amount < this.weaponItems[num2].maxStack && amount > 0)
						{
							this.weaponItems[num2].amount++;
							amount--;
							num3++;
						}
						if (amount > 0)
						{
							this.AddItem(item.id, amount);
						}
					}
				}
			}
		}

		// Token: 0x04002BEC RID: 11244
		[Header("---- Right Weapon Settings----")]
		public bool useRightWeapon = true;

		// Token: 0x04002BED RID: 11245
		public int rightWeaponID;

		// Token: 0x04002BEE RID: 11246
		public bool randomRightWeapon = true;

		// Token: 0x04002BEF RID: 11247
		[Header("Right Equip Point")]
		public Transform defaultEquipPointR;

		// Token: 0x04002BF0 RID: 11248
		public List<Transform> customEquipPointR;

		// Token: 0x04002BF1 RID: 11249
		[Header("---- Left Weapon Settings----")]
		public bool useLeftWeapon = true;

		// Token: 0x04002BF2 RID: 11250
		public int leftWeaponID;

		// Token: 0x04002BF3 RID: 11251
		public bool randomLeftWeapon = true;

		// Token: 0x04002BF4 RID: 11252
		[Header("Left Equip Point")]
		public Transform defaultEquipPointL;

		// Token: 0x04002BF5 RID: 11253
		public List<Transform> customEquipPointL;

		// Token: 0x04002BF6 RID: 11254
		public vItemCollection itemCollection;

		// Token: 0x04002BF7 RID: 11255
		protected vSimpleMeleeAI_Controller ai;

		// Token: 0x04002BF8 RID: 11256
		protected vMeleeManager manager;

		// Token: 0x04002BF9 RID: 11257
		protected vItem leftWeaponItem;

		// Token: 0x04002BFA RID: 11258
		protected vItem rightWeaponItem;

		// Token: 0x04002BFB RID: 11259
		protected GameObject leftWeapon;

		// Token: 0x04002BFC RID: 11260
		protected GameObject rightWeapon;

		// Token: 0x04002BFD RID: 11261
		protected List<vItem> weaponItems = new List<vItem>();

		// Token: 0x04002BFE RID: 11262
		protected Transform leftArm;

		// Token: 0x04002BFF RID: 11263
		protected Transform rightArm;

		// Token: 0x04002C00 RID: 11264
		protected bool inEquip;

		// Token: 0x04002C01 RID: 11265
		protected bool inUnequip;

		// Token: 0x04002C02 RID: 11266
		protected float equipTimer;

		// Token: 0x04002C03 RID: 11267
		protected float unequipTimer;

		// Token: 0x04002C04 RID: 11268
		protected float timeToStart;

		// Token: 0x04002C05 RID: 11269
		protected int equipCalls;

		// Token: 0x04002C06 RID: 11270
		protected bool changeLeft;

		// Token: 0x04002C07 RID: 11271
		protected bool changeRight;
	}
}
