using System;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.Slip
{
	// Token: 0x02000C5B RID: 3163
	public sealed class MountedClothSlipPrompt
	{
		// Token: 0x06005B83 RID: 23427 RVA: 0x0023DDF8 File Offset: 0x0023BFF8
		public void Show(Object owner, InteractionHUDController hud, InteractionConfigSO source, string fallbackLabel, bool available)
		{
			if (!available || hud == null)
			{
				this.Hide();
				return;
			}
			if (this._hud == hud && this._owner == owner)
			{
				return;
			}
			this.Hide();
			if (this._config == null)
			{
				this._config = MountedClothSlipPrompt.BuildDropOnlyConfig(source, fallbackLabel);
			}
			this._owner = owner;
			this._hud = hud;
			hud.ShowSupplemental(owner, this._config);
		}

		// Token: 0x06005B84 RID: 23428 RVA: 0x0023DE73 File Offset: 0x0023C073
		public void Hide()
		{
			if (this._hud != null)
			{
				this._hud.HideSupplemental(this._owner);
			}
			this._hud = null;
			this._owner = null;
		}

		// Token: 0x06005B85 RID: 23429 RVA: 0x0023DEA2 File Offset: 0x0023C0A2
		public void Dispose()
		{
			this.Hide();
			if (this._config == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._config);
			}
			else
			{
				Object.DestroyImmediate(this._config);
			}
			this._config = null;
		}

		// Token: 0x06005B86 RID: 23430 RVA: 0x0023DEE0 File Offset: 0x0023C0E0
		public static InteractionConfigSO BuildDropOnlyConfig(InteractionConfigSO source, string fallbackLabel)
		{
			InteractionConfigSO interactionConfigSO = ScriptableObject.CreateInstance<InteractionConfigSO>();
			interactionConfigSO.name = "MountedClothSlip_Runtime";
			interactionConfigSO.hideFlags = HideFlags.HideAndDontSave;
			interactionConfigSO.objectName = ((source != null) ? source.objectName : "Clothing");
			UsableAction usableAction = null;
			if (source != null && source.usableActions != null)
			{
				foreach (UsableAction usableAction2 in source.usableActions)
				{
					if (usableAction2 != null && string.Equals(usableAction2.keyDisplayLabel, "N", StringComparison.OrdinalIgnoreCase))
					{
						usableAction = usableAction2;
						break;
					}
				}
			}
			interactionConfigSO.usableActions.Add(new UsableAction
			{
				actionName = ((usableAction != null) ? usableAction.actionName : fallbackLabel),
				keyDisplayLabel = "N",
				description = ((usableAction != null) ? usableAction.description : string.Empty)
			});
			return interactionConfigSO;
		}

		// Token: 0x04005CB1 RID: 23729
		private InteractionConfigSO _config;

		// Token: 0x04005CB2 RID: 23730
		private InteractionHUDController _hud;

		// Token: 0x04005CB3 RID: 23731
		private Object _owner;
	}
}
