using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000048 RID: 72
	public class MMDebugMenuSpriteReplace : MonoBehaviour
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000075BC File Offset: 0x000057BC
		public virtual bool CurrentValue
		{
			get
			{
				return this._image.sprite == this.OnSprite;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000075D4 File Offset: 0x000057D4
		protected virtual void Awake()
		{
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000075D8 File Offset: 0x000057D8
		public virtual void Initialization()
		{
			this._image = base.gameObject.GetComponent<Image>();
			this._mmTouchButton = base.gameObject.GetComponent<MMTouchButton>();
			if (this._mmTouchButton != null)
			{
				this._mmTouchButton.ReturnToInitialSpriteAutomatically = false;
			}
			if (this._image == null)
			{
				return;
			}
			if (this.OnSprite == null || this.OffSprite == null)
			{
				return;
			}
			if (this.StartsOn)
			{
				this._image.sprite = this.OnSprite;
				return;
			}
			this._image.sprite = this.OffSprite;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007678 File Offset: 0x00005878
		public virtual void Swap()
		{
			if (this._image.sprite != this.OnSprite)
			{
				this.SwitchToOnSprite();
				return;
			}
			this.SwitchToOffSprite();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000769F File Offset: 0x0000589F
		public virtual void SwitchToOffSprite()
		{
			if (this._image == null)
			{
				return;
			}
			if (this.OffSprite == null)
			{
				return;
			}
			this.SpriteOff();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000076C5 File Offset: 0x000058C5
		protected virtual void SpriteOff()
		{
			this._image.sprite = this.OffSprite;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000076D8 File Offset: 0x000058D8
		public virtual void SwitchToOnSprite()
		{
			if (this._image == null)
			{
				return;
			}
			if (this.OnSprite == null)
			{
				return;
			}
			this.SpriteOn();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000076FE File Offset: 0x000058FE
		protected virtual void SpriteOn()
		{
			this._image.sprite = this.OnSprite;
		}

		// Token: 0x040001DB RID: 475
		public Sprite OnSprite;

		// Token: 0x040001DC RID: 476
		public Sprite OffSprite;

		// Token: 0x040001DD RID: 477
		public bool StartsOn = true;

		// Token: 0x040001DE RID: 478
		protected Image _image;

		// Token: 0x040001DF RID: 479
		protected MMTouchButton _mmTouchButton;
	}
}
