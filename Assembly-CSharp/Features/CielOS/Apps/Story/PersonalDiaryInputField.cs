using System;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x02001433 RID: 5171
	[AddComponentMenu("")]
	public sealed class PersonalDiaryInputField : TMP_InputField
	{
		// Token: 0x06009FBE RID: 40894 RVA: 0x0041E9BA File Offset: 0x0041CBBA
		protected override void OnEnable()
		{
			base.OnEnable();
			this._pageScroll = base.GetComponentInParent<ScrollRect>();
		}

		// Token: 0x06009FBF RID: 40895 RVA: 0x0041E9D0 File Offset: 0x0041CBD0
		public override void OnScroll(PointerEventData eventData)
		{
			if (!this.IsActive() || eventData == null)
			{
				return;
			}
			if (this._pageScroll == null)
			{
				this._pageScroll = base.GetComponentInParent<ScrollRect>();
			}
			if (this._pageScroll != null)
			{
				if (this._pageScroll.isActiveAndEnabled)
				{
					ExecuteEvents.Execute<IScrollHandler>(this._pageScroll.gameObject, eventData, ExecuteEvents.scrollHandler);
				}
				return;
			}
			base.OnScroll(eventData);
		}

		// Token: 0x06009FC0 RID: 40896 RVA: 0x0041EA40 File Offset: 0x0041CC40
		public override void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left && this._pageScroll != null)
			{
				MonoBehaviour[] components = this._pageScroll.GetComponents<MonoBehaviour>();
				for (int i = 0; i < components.Length; i++)
				{
					IControllerScrollSync controllerScrollSync = components[i] as IControllerScrollSync;
					if (controllerScrollSync != null)
					{
						controllerScrollSync.SyncControllerScroll();
					}
				}
				this._pageScroll.StopMovement();
			}
			base.OnPointerDown(eventData);
		}

		// Token: 0x0400A39E RID: 41886
		private ScrollRect _pageScroll;
	}
}
