using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000047 RID: 71
	public class MMDebugMenuRadioButton : MMDebugMenuSpriteReplace
	{
		// Token: 0x06000165 RID: 357 RVA: 0x000074D8 File Offset: 0x000056D8
		public override void Initialization()
		{
			base.Initialization();
			this.FindAllRadioButtonsFromTheSameGroup();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000074E8 File Offset: 0x000056E8
		protected virtual void FindAllRadioButtonsFromTheSameGroup()
		{
			this._group = new List<MMDebugMenuRadioButton>();
			foreach (MMDebugMenuRadioButton mmdebugMenuRadioButton in Object.FindObjectsOfType(typeof(MMDebugMenuRadioButton)) as MMDebugMenuRadioButton[])
			{
				if (mmdebugMenuRadioButton.RadioButtonGroupName == this.RadioButtonGroupName && mmdebugMenuRadioButton != this)
				{
					this._group.Add(mmdebugMenuRadioButton);
				}
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007550 File Offset: 0x00005750
		protected override void SpriteOn()
		{
			base.SpriteOn();
			if (this._group.Count >= 1)
			{
				foreach (MMDebugMenuRadioButton mmdebugMenuRadioButton in this._group)
				{
					mmdebugMenuRadioButton.SwitchToOffSprite();
				}
			}
		}

		// Token: 0x040001D9 RID: 473
		public string RadioButtonGroupName;

		// Token: 0x040001DA RID: 474
		protected List<MMDebugMenuRadioButton> _group;
	}
}
