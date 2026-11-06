using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200003D RID: 61
	public class MMDebugMenuItemChoices : MonoBehaviour
	{
		// Token: 0x06000139 RID: 313 RVA: 0x00006D80 File Offset: 0x00004F80
		public virtual void TriggerButtonEvent(int index)
		{
			MMDebugMenuButtonEvent.Trigger(this.Choices[index].ButtonEventName, true, MMDebugMenuButtonEvent.EventModes.FromButton);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006D9C File Offset: 0x00004F9C
		public virtual void Select(int index)
		{
			this.Deselect();
			this.Choices[index].ButtonBg.sprite = this.SelectedSprite;
			this.Choices[index].ButtonBg.color = this.AccentColor;
			this.Choices[index].ButtonText.color = this.OffColor;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006E04 File Offset: 0x00005004
		public virtual void Deselect()
		{
			foreach (MMDebugMenuChoiceEntry mmdebugMenuChoiceEntry in this.Choices)
			{
				mmdebugMenuChoiceEntry.ButtonBg.sprite = this.OffSprite;
				mmdebugMenuChoiceEntry.ButtonBg.color = this.OnColor;
				mmdebugMenuChoiceEntry.ButtonText.color = this.OnColor;
			}
		}

		// Token: 0x040001A4 RID: 420
		[Header("Bindings")]
		public Sprite SelectedSprite;

		// Token: 0x040001A5 RID: 421
		public Sprite OffSprite;

		// Token: 0x040001A6 RID: 422
		public Color OnColor = Color.white;

		// Token: 0x040001A7 RID: 423
		public Color OffColor = Color.black;

		// Token: 0x040001A8 RID: 424
		public Color AccentColor = MMColors.ReunoYellow;

		// Token: 0x040001A9 RID: 425
		public List<MMDebugMenuChoiceEntry> Choices;
	}
}
