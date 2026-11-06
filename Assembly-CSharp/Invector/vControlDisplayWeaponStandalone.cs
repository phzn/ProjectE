using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004DD RID: 1245
	public class vControlDisplayWeaponStandalone : MonoBehaviour
	{
		// Token: 0x06001DFF RID: 7679 RVA: 0x000B6F21 File Offset: 0x000B5121
		public virtual void SetLeftWeaponIcon(Sprite icon)
		{
			if (!this.leftDisplay)
			{
				return;
			}
			this.leftDisplay.SetWeaponIcon(icon);
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000B6F3D File Offset: 0x000B513D
		public virtual void SetLeftWeaponText(string text)
		{
			if (!this.leftDisplay)
			{
				return;
			}
			this.leftDisplay.SetWeaponText(text);
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000B6F59 File Offset: 0x000B5159
		public virtual void RemoveLeftWeaponIcon()
		{
			if (!this.leftDisplay)
			{
				return;
			}
			this.leftDisplay.RemoveWeaponIcon();
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000B6F74 File Offset: 0x000B5174
		public virtual void RemoveLeftWeaponText()
		{
			if (!this.leftDisplay)
			{
				return;
			}
			this.leftDisplay.RemoveWeaponText();
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x000B6F8F File Offset: 0x000B518F
		public virtual void SetRightWeaponIcon(Sprite icon)
		{
			if (!this.rightDisplay)
			{
				return;
			}
			this.rightDisplay.SetWeaponIcon(icon);
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x000B6FAB File Offset: 0x000B51AB
		public virtual void SetRightWeaponText(string text)
		{
			if (!this.rightDisplay)
			{
				return;
			}
			this.rightDisplay.SetWeaponText(text);
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000B6FC7 File Offset: 0x000B51C7
		public virtual void RemoveRightWeaponIcon()
		{
			if (!this.rightDisplay)
			{
				return;
			}
			this.rightDisplay.RemoveWeaponIcon();
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x000B6FE2 File Offset: 0x000B51E2
		public virtual void RemoveRightWeaponText()
		{
			if (!this.rightDisplay)
			{
				return;
			}
			this.rightDisplay.RemoveWeaponText();
		}

		// Token: 0x04001F0A RID: 7946
		[SerializeField]
		protected vDisplayWeaponStandalone leftDisplay;

		// Token: 0x04001F0B RID: 7947
		[SerializeField]
		protected vDisplayWeaponStandalone rightDisplay;
	}
}
