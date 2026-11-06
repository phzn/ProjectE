using System;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001125 RID: 4389
	[CreateAssetMenu(fileName = "WearableAction_New", menuName = "Features/Inventory/Wearable Action Definition")]
	public class WearableActionDefinition : ScriptableObject
	{
		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x060084EF RID: 34031 RVA: 0x00363799 File Offset: 0x00361999
		public string ActionId
		{
			get
			{
				if (!string.IsNullOrEmpty(this.actionId))
				{
					return this.actionId;
				}
				return base.name;
			}
		}

		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x060084F0 RID: 34032 RVA: 0x003637B5 File Offset: 0x003619B5
		public string DisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x060084F1 RID: 34033 RVA: 0x003637BD File Offset: 0x003619BD
		public string DisplayNameWhenActive
		{
			get
			{
				return this.displayNameWhenActive;
			}
		}

		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x060084F2 RID: 34034 RVA: 0x003637C5 File Offset: 0x003619C5
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x060084F3 RID: 34035 RVA: 0x003637CD File Offset: 0x003619CD
		public GenericInput Input
		{
			get
			{
				return this.input;
			}
		}

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x060084F4 RID: 34036 RVA: 0x003637D5 File Offset: 0x003619D5
		public string KeyDisplayLabel
		{
			get
			{
				return this.keyDisplayLabel;
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x060084F5 RID: 34037 RVA: 0x003637DD File Offset: 0x003619DD
		public WearableActionType ActionType
		{
			get
			{
				return this.actionType;
			}
		}

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x060084F6 RID: 34038 RVA: 0x003637E5 File Offset: 0x003619E5
		public WearableActionVisibility Visibility
		{
			get
			{
				return this.visibility;
			}
		}

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x060084F7 RID: 34039 RVA: 0x003637ED File Offset: 0x003619ED
		public string SecondaryEffectId
		{
			get
			{
				return this.secondaryEffectId;
			}
		}

		// Token: 0x060084F8 RID: 34040 RVA: 0x003637F5 File Offset: 0x003619F5
		private void OnValidate()
		{
			if (string.IsNullOrEmpty(this.actionId) && !string.IsNullOrEmpty(base.name))
			{
				this.actionId = base.name;
			}
		}

		// Token: 0x040087C9 RID: 34761
		[Header("Identity")]
		[SerializeField]
		private string actionId;

		// Token: 0x040087CA RID: 34762
		[Tooltip("Default label shown in the HUD. For SecondaryEffectToggle, this is the label when the effect is INACTIVE.")]
		[SerializeField]
		private string displayName = "Wear";

		// Token: 0x040087CB RID: 34763
		[Tooltip("Optional override label used by SecondaryEffectToggle when the effect is ACTIVE (e.g. 'Hide Face' vs 'Show Face').")]
		[SerializeField]
		private string displayNameWhenActive = string.Empty;

		// Token: 0x040087CC RID: 34764
		[TextArea]
		[SerializeField]
		private string description = string.Empty;

		// Token: 0x040087CD RID: 34765
		[Header("Input")]
		[Tooltip("Key bound to this action (Invector GenericInput; KeyCode names: 'Mouse0', 'V', 'B', 'LeftShift', ...).")]
		[SerializeField]
		private GenericInput input = new GenericInput("Mouse0", "Y", "Y");

		// Token: 0x040087CE RID: 34766
		[Tooltip("Label shown in the HUD action list (e.g. 'LMB', 'V').")]
		[SerializeField]
		private string keyDisplayLabel = "LMB";

		// Token: 0x040087CF RID: 34767
		[Header("Behavior")]
		[SerializeField]
		private WearableActionType actionType;

		// Token: 0x040087D0 RID: 34768
		[Tooltip("When this action should be available (HUD listing + input listening).")]
		[SerializeField]
		private WearableActionVisibility visibility;

		// Token: 0x040087D1 RID: 34769
		[Header("Secondary Effect (only for actionType = SecondaryEffectToggle)")]
		[Tooltip("Effect id matching a WearableSecondaryEffect.effectId on the WearableItemSO.")]
		[SerializeField]
		private string secondaryEffectId = string.Empty;
	}
}
