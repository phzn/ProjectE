using System;
using UnityEngine;
using UnityEngine.UI;

namespace Invector
{
	// Token: 0x020004DE RID: 1246
	public class vDisplayWeaponStandalone : MonoBehaviour
	{
		// Token: 0x06001E08 RID: 7688 RVA: 0x000B6FFD File Offset: 0x000B51FD
		protected virtual void Start()
		{
			this.RemoveWeaponIcon();
			this.RemoveWeaponText();
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x000B700B File Offset: 0x000B520B
		public virtual void SetWeaponIcon(Sprite icon)
		{
			if (!this.weaponIcon)
			{
				return;
			}
			this.weaponIcon.sprite = icon;
			if (!this.weaponIcon.gameObject.activeSelf)
			{
				this.weaponIcon.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x000B704A File Offset: 0x000B524A
		public virtual void SetWeaponText(string text)
		{
			if (!this.weaponText)
			{
				return;
			}
			this.weaponText.text = text;
			if (!this.weaponText.gameObject.activeSelf)
			{
				this.weaponText.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x000B708C File Offset: 0x000B528C
		public virtual void RemoveWeaponIcon()
		{
			if (!this.weaponIcon)
			{
				return;
			}
			this.weaponIcon.sprite = this.defaultIcon;
			if (this.weaponIcon.gameObject.activeSelf && this.weaponIcon.sprite == null)
			{
				this.weaponIcon.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x000B70EE File Offset: 0x000B52EE
		public virtual void RemoveWeaponText()
		{
			if (!this.weaponText)
			{
				return;
			}
			this.weaponText.text = this.defaultText;
		}

		// Token: 0x04001F0C RID: 7948
		[Header("Weapon Display source")]
		public Image weaponIcon;

		// Token: 0x04001F0D RID: 7949
		public Text weaponText;

		// Token: 0x04001F0E RID: 7950
		[Header("Weapon unarmed sources")]
		public Sprite defaultIcon;

		// Token: 0x04001F0F RID: 7951
		public string defaultText;
	}
}
