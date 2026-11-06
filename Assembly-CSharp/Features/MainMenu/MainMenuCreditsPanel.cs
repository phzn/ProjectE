using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x0200105C RID: 4188
	public sealed class MainMenuCreditsPanel : MonoBehaviour
	{
		// Token: 0x06007F1F RID: 32543 RVA: 0x0033F550 File Offset: 0x0033D750
		private void OnEnable()
		{
			this._timer = 0f;
			this._idleTimer = 0f;
			this._userInteracted = false;
			if (this.scrollRect != null)
			{
				this.scrollRect.verticalNormalizedPosition = 1f;
			}
			this._menu = Object.FindObjectOfType<MainMenuController>();
			if (this._menu != null)
			{
				this._menu.EnterCreditsPresentation();
			}
		}

		// Token: 0x06007F20 RID: 32544 RVA: 0x0033F5BC File Offset: 0x0033D7BC
		private void OnDisable()
		{
			if (this._menu != null)
			{
				this._menu.ExitCreditsPresentation();
			}
			this._menu = null;
		}

		// Token: 0x06007F21 RID: 32545 RVA: 0x0033F5E0 File Offset: 0x0033D7E0
		private void Update()
		{
			if (this.scrollRect == null || this.scrollRect.content == null)
			{
				return;
			}
			if (this.UserIsInteracting())
			{
				this._userInteracted = true;
				this._idleTimer = 0f;
				return;
			}
			if (this._userInteracted)
			{
				this._idleTimer += Time.unscaledDeltaTime;
				if (this._idleTimer < this.resumeAfterIdle)
				{
					return;
				}
				this._userInteracted = false;
			}
			this._timer += Time.unscaledDeltaTime;
			if (this._timer < this.startDelay)
			{
				return;
			}
			float num = ((this.scrollRect.viewport != null) ? this.scrollRect.viewport.rect.height : ((RectTransform)this.scrollRect.transform).rect.height);
			float num2 = this.scrollRect.content.rect.height - num;
			if (num2 <= 1f)
			{
				return;
			}
			float num3 = this.scrollSpeed * Time.unscaledDeltaTime / num2;
			this.scrollRect.verticalNormalizedPosition = Mathf.Max(0f, this.scrollRect.verticalNormalizedPosition - num3);
		}

		// Token: 0x06007F22 RID: 32546 RVA: 0x0033F71B File Offset: 0x0033D91B
		private bool UserIsInteracting()
		{
			return (Mathf.Abs(Input.mouseScrollDelta.y) > 0.01f && this.PointerOverScroll()) || (Input.GetMouseButton(0) && this.PointerOverScroll());
		}

		// Token: 0x06007F23 RID: 32547 RVA: 0x0033F750 File Offset: 0x0033D950
		private bool PointerOverScroll()
		{
			return !(EventSystem.current == null) && RectTransformUtility.RectangleContainsScreenPoint((this.scrollRect.viewport != null) ? this.scrollRect.viewport : ((RectTransform)this.scrollRect.transform), Input.mousePosition);
		}

		// Token: 0x040082A0 RID: 33440
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x040082A1 RID: 33441
		[SerializeField]
		[Min(1f)]
		private float scrollSpeed = 34f;

		// Token: 0x040082A2 RID: 33442
		[SerializeField]
		[Min(0f)]
		private float startDelay = 1.6f;

		// Token: 0x040082A3 RID: 33443
		[SerializeField]
		[Min(0f)]
		private float resumeAfterIdle = 3f;

		// Token: 0x040082A4 RID: 33444
		private float _timer;

		// Token: 0x040082A5 RID: 33445
		private float _idleTimer;

		// Token: 0x040082A6 RID: 33446
		private bool _userInteracted;

		// Token: 0x040082A7 RID: 33447
		private MainMenuController _menu;
	}
}
