using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x02000477 RID: 1143
	[vClassHeader("Damage Modifier Controller", true, "icon_v2", false, "", openClose = false, useHelpBox = true, helpBoxText = "Needs a HealthController component")]
	public class vDamageModifierController : vMonoBehaviour
	{
		// Token: 0x06001C13 RID: 7187 RVA: 0x000AFF8C File Offset: 0x000AE18C
		protected virtual void Awake()
		{
			this.Init();
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x000AFF94 File Offset: 0x000AE194
		protected void Init()
		{
			this.GetHealthController();
			if (this.healthController != null)
			{
				this.AddDamageEvent();
				this.InitModifiers();
				this.isInit = true;
			}
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x000AFFB8 File Offset: 0x000AE1B8
		protected virtual void InitModifiers()
		{
			for (int i = 0; i < this.modifiers.Count; i++)
			{
				this.modifiers[i].ResetModifier();
				this.modifiers[i].onBroken.AddListener(delegate(vDamageModifier m)
				{
					this.CheckBrokedModifiers();
				});
			}
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x000B000E File Offset: 0x000AE20E
		protected virtual void AddDamageEvent()
		{
			this.RemoveDamageEvent();
			this.healthController.onStartReceiveDamage.AddListener(new UnityAction<vDamage>(this.ApplyModifiers));
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x000B0033 File Offset: 0x000AE233
		protected virtual void RemoveDamageEvent()
		{
			this.healthController.onStartReceiveDamage.RemoveListener(new UnityAction<vDamage>(this.ApplyModifiers));
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x000B0054 File Offset: 0x000AE254
		protected virtual void GetHealthController()
		{
			switch (this.getHealthMethod)
			{
			case vDamageModifierController.GetHealthControllerMethod.GetComponent:
				this.healthController = base.GetComponent<vIHealthController>();
				return;
			case vDamageModifierController.GetHealthControllerMethod.GetComponentInParent:
				this.healthController = base.GetComponentInParent<vIHealthController>();
				return;
			case vDamageModifierController.GetHealthControllerMethod.GetComponentInChildren:
				this.healthController = base.GetComponentInChildren<vIHealthController>();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x000B00A1 File Offset: 0x000AE2A1
		protected virtual void OnEnable()
		{
			if (this.isInit)
			{
				this.AddDamageEvent();
			}
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x000B00B1 File Offset: 0x000AE2B1
		protected virtual void OnDisable()
		{
			if (this.isInit)
			{
				this.RemoveDamageEvent();
			}
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000B00C1 File Offset: 0x000AE2C1
		protected virtual void CheckBrokedModifiers()
		{
			if (!this.modifiers.Exists((vDamageModifier m) => !m.isBroken))
			{
				this.onAllModifiersIsBroken.Invoke();
			}
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000B00FC File Offset: 0x000AE2FC
		protected virtual void ApplyModifiers(vDamage damage)
		{
			for (int i = 0; i < this.modifiers.Count; i++)
			{
				this.modifiers[i].ApplyModifier(damage);
			}
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000B0134 File Offset: 0x000AE334
		public void ResetAllModifiers()
		{
			for (int i = 0; i < this.modifiers.Count; i++)
			{
				this.modifiers[i].ResetModifier();
			}
		}

		// Token: 0x04001D80 RID: 7552
		[vReadOnly(true)]
		public bool isInit;

		// Token: 0x04001D81 RID: 7553
		[SerializeField]
		protected vDamageModifierController.GetHealthControllerMethod getHealthMethod;

		// Token: 0x04001D82 RID: 7554
		[Tooltip("Modifier List")]
		public List<vDamageModifier> modifiers;

		// Token: 0x04001D83 RID: 7555
		public UnityEvent onAllModifiersIsBroken;

		// Token: 0x04001D84 RID: 7556
		protected vIHealthController healthController;

		// Token: 0x02000478 RID: 1144
		public enum GetHealthControllerMethod
		{
			// Token: 0x04001D86 RID: 7558
			GetComponent,
			// Token: 0x04001D87 RID: 7559
			GetComponentInParent,
			// Token: 0x04001D88 RID: 7560
			GetComponentInChildren
		}
	}
}
