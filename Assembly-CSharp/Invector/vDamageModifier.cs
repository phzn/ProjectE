using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector
{
	// Token: 0x02000474 RID: 1140
	[Serializable]
	public class vDamageModifier
	{
		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x000AFDD6 File Offset: 0x000ADFD6
		public bool isBroken
		{
			get
			{
				return this.destructible && this.resistance <= 0f;
			}
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000AFDF4 File Offset: 0x000ADFF4
		public virtual void ApplyModifier(vDamage damage)
		{
			if (damage.damageValue > 0f && this.CanFilterDamage(damage.damageType) && !this.isBroken)
			{
				float num;
				if (this.percentage)
				{
					num = damage.damageValue / 100f * (float)this.value;
				}
				else
				{
					num = (float)this.value;
				}
				if (this.destructible)
				{
					this.resistance -= damage.damageValue;
					this.onChangeResistance.Invoke(Mathf.Max(this.resistance, 0f));
					if (this.resistance <= 0f)
					{
						this.onBroken.Invoke(this);
					}
				}
				if (!this.destructible || this.resistance > 0f)
				{
					damage.damageValue -= num;
				}
			}
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x000AFED0 File Offset: 0x000AE0D0
		protected virtual bool CanFilterDamage(string damageType)
		{
			switch (this.filterMethod)
			{
			case vDamageModifier.FilterMethod.ApplyToAll:
				return true;
			case vDamageModifier.FilterMethod.ApplyToAllInList:
				return this.damageTypes.Contains(damageType);
			case vDamageModifier.FilterMethod.ApplyToAllOutList:
				return !this.damageTypes.Contains(damageType);
			default:
				return true;
			}
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x000AFF18 File Offset: 0x000AE118
		public virtual void ResetModifier()
		{
			if (this.destructible)
			{
				this.resistance = this.maxResistance;
				this.onChangeResistance.Invoke(Mathf.Max(this.resistance, 0f));
			}
		}

		// Token: 0x04001D72 RID: 7538
		public string name = "MyModifier";

		// Token: 0x04001D73 RID: 7539
		public vDamageModifier.FilterMethod filterMethod;

		// Token: 0x04001D74 RID: 7540
		[Tooltip("List of Damage type that this can modify, keep empty if the filter will be applied to all types of damage")]
		public List<string> damageTypes = new List<string>();

		// Token: 0x04001D75 RID: 7541
		[Tooltip("Modifier value")]
		public int value;

		// Token: 0x04001D76 RID: 7542
		[Tooltip("true: Reduce a percentage of damage value\nfalse: Reduce da damage value directly")]
		public bool percentage;

		// Token: 0x04001D77 RID: 7543
		[Tooltip("The Filter will receive all damage and decrease your self resistance")]
		public bool destructible = true;

		// Token: 0x04001D78 RID: 7544
		public float resistance = 100f;

		// Token: 0x04001D79 RID: 7545
		public float maxResistance = 100f;

		// Token: 0x04001D7A RID: 7546
		public Slider.SliderEvent onChangeResistance;

		// Token: 0x04001D7B RID: 7547
		public vDamageModifier.DamageModifierEvent onBroken;

		// Token: 0x02000475 RID: 1141
		public enum FilterMethod
		{
			// Token: 0x04001D7D RID: 7549
			ApplyToAll,
			// Token: 0x04001D7E RID: 7550
			ApplyToAllInList,
			// Token: 0x04001D7F RID: 7551
			ApplyToAllOutList
		}

		// Token: 0x02000476 RID: 1142
		[Serializable]
		public class DamageModifierEvent : UnityEvent<vDamageModifier>
		{
		}
	}
}
