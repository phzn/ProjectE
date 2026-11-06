using System;
using System.Reflection;
using Features.Detection;
using UnityEngine;

namespace PoolWaterTools
{
	// Token: 0x0200072C RID: 1836
	[DisallowMultipleComponent]
	public class SwimmingLayerSuppressor : MonoBehaviour
	{
		// Token: 0x06002F98 RID: 12184 RVA: 0x0010D970 File Offset: 0x0010BB70
		private void Awake()
		{
			foreach (Component component in base.transform.root.GetComponentsInChildren<Component>(true))
			{
				if (!(component == null))
				{
					Type type = component.GetType();
					if (type.FullName == "Invector.vCharacterController.vActions.vSwimming")
					{
						this._swimming = component;
						this._isSwimmingProp = type.GetProperty("isSwimming");
						this._inWaterField = type.GetField("inTheWater");
						break;
					}
				}
			}
			if (this._swimming == null)
			{
				Debug.LogWarning("[SwimmingLayerSuppressor] vSwimming not found under player root (from " + base.name + ") - disabling.");
				base.enabled = false;
				return;
			}
			this._resolved = true;
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x0010DA28 File Offset: 0x0010BC28
		private bool IsInWater()
		{
			return (this._isSwimmingProp != null && (bool)this._isSwimmingProp.GetValue(this._swimming, null)) || (this._inWaterField != null && (bool)this._inWaterField.GetValue(this._swimming));
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x0010DA88 File Offset: 0x0010BC88
		private void Update()
		{
			if (!this._resolved)
			{
				return;
			}
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance == null)
			{
				return;
			}
			bool flag = this.IsInWater();
			if (flag && !this._emotionDisabled)
			{
				instance.DisableEmotionLayersForAction();
				this._emotionDisabled = true;
				return;
			}
			if (!flag && this._emotionDisabled)
			{
				instance.RestoreEmotionLayers();
				this._emotionDisabled = false;
			}
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x0010DAE6 File Offset: 0x0010BCE6
		public void RestoreForEncounter()
		{
			if (this._emotionDisabled)
			{
				PlayerEmotionController instance = PlayerEmotionController.Instance;
				if (instance != null)
				{
					instance.RestoreEmotionLayers();
				}
				this._emotionDisabled = false;
			}
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x0010DB07 File Offset: 0x0010BD07
		private void OnDisable()
		{
			this.RestoreForEncounter();
		}

		// Token: 0x0400300A RID: 12298
		private Component _swimming;

		// Token: 0x0400300B RID: 12299
		private PropertyInfo _isSwimmingProp;

		// Token: 0x0400300C RID: 12300
		private FieldInfo _inWaterField;

		// Token: 0x0400300D RID: 12301
		private bool _resolved;

		// Token: 0x0400300E RID: 12302
		private bool _emotionDisabled;
	}
}
