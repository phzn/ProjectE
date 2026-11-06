using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.MainMenu
{
	// Token: 0x020010A2 RID: 4258
	public sealed class MainMenuUiSoundRelay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler
	{
		// Token: 0x060080F1 RID: 33009 RVA: 0x003495EC File Offset: 0x003477EC
		public void Initialize(MainMenuPresentationDirector presentation)
		{
			this._presentation = presentation;
		}

		// Token: 0x1700148B RID: 5259
		// (get) Token: 0x060080F2 RID: 33010 RVA: 0x003495F5 File Offset: 0x003477F5
		private MainMenuPresentationDirector Presentation
		{
			get
			{
				if (this._presentation == null)
				{
					this._presentation = Object.FindObjectOfType<MainMenuPresentationDirector>();
				}
				return this._presentation;
			}
		}

		// Token: 0x060080F3 RID: 33011 RVA: 0x00349616 File Offset: 0x00347816
		public void OnPointerEnter(PointerEventData eventData)
		{
			MainMenuPresentationDirector presentation = this.Presentation;
			if (presentation == null)
			{
				return;
			}
			presentation.PlayButtonHover();
		}

		// Token: 0x060080F4 RID: 33012 RVA: 0x00349628 File Offset: 0x00347828
		public void OnPointerDown(PointerEventData eventData)
		{
			MainMenuPresentationDirector presentation = this.Presentation;
			if (presentation == null)
			{
				return;
			}
			presentation.PlayButtonClick();
		}

		// Token: 0x04008437 RID: 33847
		private MainMenuPresentationDirector _presentation;
	}
}
