using System;
using System.Collections.Generic;
using Invector.vEventSystems;
using Invector.vItemManager;
using UnityEngine;

namespace Invector.vMelee
{
	// Token: 0x02000513 RID: 1299
	public class vMeleeManager : vMonoBehaviour, IWeaponEquipmentListener
	{
		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001F71 RID: 8049 RVA: 0x000BB4BC File Offset: 0x000B96BC
		// (set) Token: 0x06001F72 RID: 8050 RVA: 0x000BB4C4 File Offset: 0x000B96C4
		public virtual vIMeleeFighter fighter { get; set; }

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x000BB4CD File Offset: 0x000B96CD
		// (set) Token: 0x06001F74 RID: 8052 RVA: 0x000BB4D5 File Offset: 0x000B96D5
		protected virtual int damageMultiplier { get; set; }

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001F75 RID: 8053 RVA: 0x000BB4DE File Offset: 0x000B96DE
		// (set) Token: 0x06001F76 RID: 8054 RVA: 0x000BB4E6 File Offset: 0x000B96E6
		protected virtual int currentRecoilID { get; set; }

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001F77 RID: 8055 RVA: 0x000BB4EF File Offset: 0x000B96EF
		// (set) Token: 0x06001F78 RID: 8056 RVA: 0x000BB4F7 File Offset: 0x000B96F7
		protected virtual int currentReactionID { get; set; }

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001F79 RID: 8057 RVA: 0x000BB500 File Offset: 0x000B9700
		// (set) Token: 0x06001F7A RID: 8058 RVA: 0x000BB508 File Offset: 0x000B9708
		protected virtual bool ignoreDefense { get; set; }

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001F7B RID: 8059 RVA: 0x000BB511 File Offset: 0x000B9711
		// (set) Token: 0x06001F7C RID: 8060 RVA: 0x000BB519 File Offset: 0x000B9719
		protected virtual bool activeRagdoll { get; set; }

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x000BB522 File Offset: 0x000B9722
		// (set) Token: 0x06001F7E RID: 8062 RVA: 0x000BB52A File Offset: 0x000B972A
		protected virtual float senselessTime { get; set; }

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001F7F RID: 8063 RVA: 0x000BB533 File Offset: 0x000B9733
		// (set) Token: 0x06001F80 RID: 8064 RVA: 0x000BB53B File Offset: 0x000B973B
		protected virtual bool inRecoil { get; set; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x000BB544 File Offset: 0x000B9744
		// (set) Token: 0x06001F82 RID: 8066 RVA: 0x000BB54C File Offset: 0x000B974C
		protected virtual string attackName { get; set; }

		// Token: 0x06001F83 RID: 8067 RVA: 0x000BB555 File Offset: 0x000B9755
		protected virtual void Start()
		{
			this.Init();
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x000BB560 File Offset: 0x000B9760
		public virtual void Init()
		{
			this.fighter = base.gameObject.GetMeleeFighter();
			using (List<vBodyMember>.Enumerator enumerator = this.Members.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					vBodyMember member = enumerator.Current;
					if (member.attackObject == null)
					{
						vMeleeAttackObject[] componentsInChildren = base.GetComponentsInChildren<vMeleeAttackObject>();
						if (componentsInChildren.Length != 0)
						{
							member.attackObject = Array.Find<vMeleeAttackObject>(componentsInChildren, (vMeleeAttackObject a) => a.attackObjectName.Equals(member.bodyPart));
						}
						if (member.attackObject == null)
						{
							Debug.LogWarning("Can't find the attack Object " + member.bodyPart);
							continue;
						}
					}
					member.attackObject.damage.damageValue = this.defaultDamage.damageValue;
					if (member.bodyPart == HumanBodyBones.LeftLowerArm.ToString())
					{
						vMeleeWeapon componentInChildren = member.attackObject.GetComponentInChildren<vMeleeWeapon>(true);
						this.leftWeapon = componentInChildren;
					}
					if (member.bodyPart == HumanBodyBones.RightLowerArm.ToString())
					{
						vMeleeWeapon componentInChildren2 = member.attackObject.GetComponentInChildren<vMeleeWeapon>(true);
						this.rightWeapon = componentInChildren2;
					}
					member.attackObject.meleeManager = this;
					member.SetActiveDamage(false);
				}
			}
			if (this.leftWeapon != null)
			{
				this.leftWeapon.SetActiveDamage(false);
				this.leftWeapon.meleeManager = this;
			}
			if (this.rightWeapon != null)
			{
				this.rightWeapon.meleeManager = this;
				this.rightWeapon.SetActiveDamage(false);
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x000BB74C File Offset: 0x000B994C
		public virtual void SetActiveAttack(List<string> bodyParts, vAttackType type, bool active = true, int damageMultiplier = 0, int recoilID = 0, int reactionID = 0, bool ignoreDefense = false, bool activeRagdoll = false, float senselessTime = 0f, string attackName = "")
		{
			for (int i = 0; i < bodyParts.Count; i++)
			{
				string text = bodyParts[i];
				this.SetActiveAttack(text, type, active, damageMultiplier, recoilID, reactionID, ignoreDefense, activeRagdoll, senselessTime, attackName);
			}
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x000BB78C File Offset: 0x000B998C
		public virtual void SetActiveAttack(string bodyPart, vAttackType type, bool active = true, int damageMultiplier = 0, int recoilID = 0, int reactionID = 0, bool ignoreDefense = false, bool activeRagdoll = false, float senselessTime = 0f, string attackName = "")
		{
			this.damageMultiplier = damageMultiplier;
			this.currentRecoilID = recoilID;
			this.currentReactionID = reactionID;
			this.ignoreDefense = ignoreDefense;
			this.activeRagdoll = activeRagdoll;
			this.attackName = attackName;
			this.senselessTime = senselessTime;
			if (type == vAttackType.Unarmed)
			{
				vBodyMember vBodyMember = this.Members.Find((vBodyMember member) => member.bodyPart == bodyPart);
				if (vBodyMember != null)
				{
					vBodyMember.SetActiveDamage(active);
					return;
				}
			}
			else
			{
				if (bodyPart == "RightLowerArm" && this.rightWeapon != null)
				{
					this.rightWeapon.meleeManager = this;
					this.rightWeapon.SetActiveDamage(active);
					return;
				}
				if (bodyPart == "LeftLowerArm" && this.leftWeapon != null)
				{
					this.leftWeapon.meleeManager = this;
					this.leftWeapon.SetActiveDamage(active);
				}
			}
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x000BB878 File Offset: 0x000B9A78
		public virtual void OnDamageHit(ref vHitInfo hitInfo)
		{
			vDamage vDamage = new vDamage(hitInfo.attackObject.damage);
			vDamage.sender = base.transform;
			vDamage.reaction_id = this.currentReactionID;
			vDamage.recoil_id = this.currentRecoilID;
			if (this.activeRagdoll)
			{
				vDamage.activeRagdoll = this.activeRagdoll;
			}
			if (this.attackName != string.Empty)
			{
				vDamage.damageType = this.attackName;
			}
			if (this.ignoreDefense)
			{
				vDamage.ignoreDefense = this.ignoreDefense;
			}
			if (this.senselessTime != 0f)
			{
				vDamage.senselessTime = this.senselessTime;
			}
			vDamage.damageValue *= (float)((this.damageMultiplier > 1) ? this.damageMultiplier : 1);
			hitInfo.targetIsBlocking = !hitInfo.attackObject.ApplyDamage(hitInfo.hitBox, hitInfo.targetCollider, vDamage);
			this.onDamageHit.Invoke(hitInfo);
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x000BB96C File Offset: 0x000B9B6C
		public virtual void OnRecoilHit(vHitInfo hitInfo)
		{
			if (this.hitProperties.useRecoil && this.InRecoilRange(hitInfo) && !this.inRecoil)
			{
				this.inRecoil = true;
				int currentRecoilID = this.currentRecoilID;
				if (this.fighter != null)
				{
					this.fighter.OnRecoil(currentRecoilID);
				}
				this.onRecoilHit.Invoke(hitInfo);
				base.Invoke("ResetRecoil", 1f);
			}
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x000BB9D8 File Offset: 0x000B9BD8
		public virtual void OnDefense()
		{
			if (this.leftWeapon != null && this.leftWeapon.meleeType != vMeleeType.OnlyAttack && this.leftWeapon.gameObject.activeInHierarchy)
			{
				this.leftWeapon.OnDefense();
			}
			if (this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyAttack && this.rightWeapon.gameObject.activeInHierarchy)
			{
				this.rightWeapon.OnDefense();
			}
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x000BBA57 File Offset: 0x000B9C57
		public virtual int GetAttackID()
		{
			if (this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyDefense && this.rightWeapon.gameObject.activeInHierarchy)
			{
				return this.rightWeapon.attackID;
			}
			return 0;
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000BBA94 File Offset: 0x000B9C94
		public virtual float GetAttackStaminaCost()
		{
			if (this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyDefense && this.rightWeapon.gameObject.activeInHierarchy)
			{
				return this.rightWeapon.staminaCost;
			}
			return this.defaultStaminaCost;
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x000BBAE0 File Offset: 0x000B9CE0
		public virtual float GetAttackStaminaRecoveryDelay()
		{
			if (this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyDefense && this.rightWeapon.gameObject.activeInHierarchy)
			{
				return this.rightWeapon.staminaRecoveryDelay;
			}
			return this.defaultStaminaRecoveryDelay;
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x000BBB2C File Offset: 0x000B9D2C
		public virtual float GetAttackDistance()
		{
			if (this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyDefense && this.rightWeapon.gameObject.activeInHierarchy)
			{
				return this.rightWeapon.distanceToAttack;
			}
			return this.defaultAttackDistance;
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x000BBB78 File Offset: 0x000B9D78
		public virtual int GetDefenseID()
		{
			if (this.leftWeapon != null && this.leftWeapon.meleeType != vMeleeType.OnlyAttack && this.leftWeapon.gameObject.activeInHierarchy)
			{
				base.GetComponent<Animator>().SetBool("FlipAnimation", false);
				return this.leftWeapon.defenseID;
			}
			if (this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyAttack && this.rightWeapon.gameObject.activeInHierarchy)
			{
				base.GetComponent<Animator>().SetBool("FlipAnimation", true);
				return this.rightWeapon.defenseID;
			}
			return 0;
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x000BBC1C File Offset: 0x000B9E1C
		public virtual int GetDefenseRate()
		{
			if (this.leftWeapon != null && this.leftWeapon.meleeType != vMeleeType.OnlyAttack && this.leftWeapon.gameObject.activeInHierarchy)
			{
				return this.leftWeapon.defenseRate;
			}
			if (this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyAttack && this.rightWeapon.gameObject.activeInHierarchy)
			{
				return this.rightWeapon.defenseRate;
			}
			return this.defaultDefenseRate;
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x000BBCA3 File Offset: 0x000B9EA3
		public virtual int GetMoveSetID()
		{
			if (this.rightWeapon != null && this.rightWeapon.gameObject.activeInHierarchy)
			{
				return this.rightWeapon.movesetID;
			}
			return 0;
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x000BBCD4 File Offset: 0x000B9ED4
		public virtual bool CanBreakAttack()
		{
			if (this.leftWeapon != null && this.leftWeapon.meleeType != vMeleeType.OnlyAttack && this.leftWeapon.gameObject.activeInHierarchy)
			{
				return this.leftWeapon.breakAttack;
			}
			return this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyAttack && this.rightWeapon.gameObject.activeInHierarchy && this.rightWeapon.breakAttack;
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x000BBD58 File Offset: 0x000B9F58
		public virtual bool CanBlockAttack(Vector3 attackPoint)
		{
			if (this.leftWeapon != null && this.leftWeapon.meleeType != vMeleeType.OnlyAttack && this.leftWeapon.gameObject.activeInHierarchy)
			{
				return Math.Abs(base.transform.HitAngle(attackPoint, true)) <= this.leftWeapon.defenseRange;
			}
			if (this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyAttack && this.rightWeapon.gameObject.activeInHierarchy)
			{
				return Math.Abs(base.transform.HitAngle(attackPoint, true)) <= this.rightWeapon.defenseRange;
			}
			return Math.Abs(base.transform.HitAngle(attackPoint, true)) <= this.defaultDefenseRange;
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x000BBE24 File Offset: 0x000BA024
		public virtual int GetDefenseRecoilID()
		{
			if (this.leftWeapon != null && this.leftWeapon.meleeType != vMeleeType.OnlyAttack && this.leftWeapon.gameObject.activeInHierarchy)
			{
				return this.leftWeapon.recoilID;
			}
			if (this.rightWeapon != null && this.rightWeapon.meleeType != vMeleeType.OnlyAttack && this.rightWeapon.gameObject.activeInHierarchy)
			{
				return this.rightWeapon.recoilID;
			}
			return 0;
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x000BBEA8 File Offset: 0x000BA0A8
		protected virtual bool InRecoilRange(vHitInfo hitInfo)
		{
			Vector3 vector = new Vector3(base.transform.position.x, hitInfo.hitPoint.y, base.transform.position.z);
			return (Quaternion.LookRotation(hitInfo.hitPoint - vector).eulerAngles - base.transform.eulerAngles).NormalizeAngle().y <= this.hitProperties.recoilRange;
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x000BBF2C File Offset: 0x000BA12C
		public virtual void SetLeftWeapon(GameObject weaponObject)
		{
			if (weaponObject)
			{
				this.leftWeapon = weaponObject.GetComponent<vMeleeWeapon>();
				this.SetLeftWeapon(this.leftWeapon);
				return;
			}
			if (this.leftWeapon)
			{
				this.onUnequipWeapon.Invoke(this.leftWeapon, true);
			}
			this.leftWeapon = null;
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x000BBF80 File Offset: 0x000BA180
		public virtual void SetRightWeapon(GameObject weaponObject)
		{
			if (weaponObject)
			{
				this.rightWeapon = weaponObject.GetComponent<vMeleeWeapon>();
				this.SetRightWeapon(this.rightWeapon);
				return;
			}
			if (this.rightWeapon)
			{
				this.onUnequipWeapon.Invoke(this.rightWeapon, false);
			}
			this.rightWeapon = null;
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x000BBFD4 File Offset: 0x000BA1D4
		public virtual void SetLeftWeapon(vMeleeWeapon weapon)
		{
			if (weapon)
			{
				this.leftWeapon = weapon;
				this.leftWeapon.IsEquipped = true;
				this.leftWeapon.SetActiveDamage(false);
				this.leftWeapon.meleeManager = this;
				this.onEquipWeapon.Invoke(weapon, true);
				return;
			}
			if (this.leftWeapon)
			{
				this.onUnequipWeapon.Invoke(this.leftWeapon, true);
			}
			this.leftWeapon = null;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000BC048 File Offset: 0x000BA248
		public virtual void SetRightWeapon(vMeleeWeapon weapon)
		{
			if (weapon)
			{
				this.rightWeapon = weapon;
				this.rightWeapon.IsEquipped = true;
				this.rightWeapon.meleeManager = this;
				this.rightWeapon.SetActiveDamage(false);
				this.onEquipWeapon.Invoke(weapon, false);
				return;
			}
			if (this.rightWeapon)
			{
				this.onUnequipWeapon.Invoke(this.rightWeapon, false);
			}
			this.rightWeapon = null;
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x000BC0BC File Offset: 0x000BA2BC
		public virtual vMeleeWeapon CurrentActiveAttackWeapon
		{
			get
			{
				vMeleeWeapon currentAttackWeapon = this.CurrentAttackWeapon;
				if (currentAttackWeapon && currentAttackWeapon.IsEquipped)
				{
					return currentAttackWeapon;
				}
				return null;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x000BC0E4 File Offset: 0x000BA2E4
		public virtual vMeleeWeapon CurrentActiveDefenseWeapon
		{
			get
			{
				vMeleeWeapon currentDefenseWeapon = this.CurrentDefenseWeapon;
				if (currentDefenseWeapon && currentDefenseWeapon.IsEquipped)
				{
					return currentDefenseWeapon;
				}
				return null;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x000BC10C File Offset: 0x000BA30C
		public virtual vMeleeWeapon CurrentAttackWeapon
		{
			get
			{
				if (this.rightWeapon && (this.rightWeapon.meleeType == vMeleeType.OnlyAttack || this.rightWeapon.meleeType == vMeleeType.AttackAndDefense))
				{
					return this.rightWeapon;
				}
				if (this.leftWeapon && (this.leftWeapon.meleeType == vMeleeType.OnlyAttack || this.leftWeapon.meleeType == vMeleeType.AttackAndDefense))
				{
					return this.leftWeapon;
				}
				return null;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x000BC17C File Offset: 0x000BA37C
		public virtual vMeleeWeapon CurrentDefenseWeapon
		{
			get
			{
				if (this.rightWeapon && (this.rightWeapon.meleeType == vMeleeType.OnlyDefense || this.rightWeapon.meleeType == vMeleeType.AttackAndDefense))
				{
					return this.rightWeapon;
				}
				if (this.leftWeapon && (this.leftWeapon.meleeType == vMeleeType.OnlyDefense || this.leftWeapon.meleeType == vMeleeType.AttackAndDefense))
				{
					return this.leftWeapon;
				}
				return null;
			}
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x000BC1E8 File Offset: 0x000BA3E8
		protected virtual void ResetRecoil()
		{
			this.inRecoil = false;
		}

		// Token: 0x0400202E RID: 8238
		public List<vBodyMember> Members = new List<vBodyMember>();

		// Token: 0x0400202F RID: 8239
		public vDamage defaultDamage = new vDamage(10);

		// Token: 0x04002030 RID: 8240
		public HitProperties hitProperties;

		// Token: 0x04002031 RID: 8241
		public vMeleeWeapon leftWeapon;

		// Token: 0x04002032 RID: 8242
		public vMeleeWeapon rightWeapon;

		// Token: 0x04002033 RID: 8243
		public vOnHitEvent onDamageHit;

		// Token: 0x04002034 RID: 8244
		public vOnHitEvent onRecoilHit;

		// Token: 0x04002035 RID: 8245
		public OnEquipWeaponEvent onEquipWeapon;

		// Token: 0x04002036 RID: 8246
		public OnEquipWeaponEvent onUnequipWeapon;

		// Token: 0x04002037 RID: 8247
		[Tooltip("NPC ONLY- Ideal distance for the attack")]
		public float defaultAttackDistance = 1f;

		// Token: 0x04002038 RID: 8248
		[Tooltip("Default cost for stamina when attack")]
		public float defaultStaminaCost = 20f;

		// Token: 0x04002039 RID: 8249
		[Tooltip("Default recovery delay for stamina when attack")]
		public float defaultStaminaRecoveryDelay = 1f;

		// Token: 0x0400203A RID: 8250
		[Range(0f, 100f)]
		public int defaultDefenseRate = 100;

		// Token: 0x0400203B RID: 8251
		[Range(0f, 180f)]
		public float defaultDefenseRange = 90f;
	}
}
