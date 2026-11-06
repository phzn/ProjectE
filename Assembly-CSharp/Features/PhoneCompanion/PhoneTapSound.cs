using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E51 RID: 3665
	public class PhoneTapSound : MonoBehaviour
	{
		// Token: 0x06006D7E RID: 28030 RVA: 0x002BFAB8 File Offset: 0x002BDCB8
		public void Configure(AudioClip clip, float volume, Canvas phoneCanvas)
		{
			this._clip = clip;
			this._volume = Mathf.Clamp01(volume);
			this._phoneCanvas = phoneCanvas;
			if (this._audio == null)
			{
				this._audio = base.gameObject.AddComponent<AudioSource>();
				this._audio.playOnAwake = false;
				this._audio.spatialBlend = 0f;
				this._audio.ignoreListenerPause = true;
			}
		}

		// Token: 0x06006D7F RID: 28031 RVA: 0x002BFB26 File Offset: 0x002BDD26
		public void SetActive(bool active)
		{
			this._active = active;
		}

		// Token: 0x06006D80 RID: 28032 RVA: 0x002BFB30 File Offset: 0x002BDD30
		private void Update()
		{
			if (!this._active || this._clip == null)
			{
				return;
			}
			if (!GameplayInput.GetMouseButtonDown(0))
			{
				return;
			}
			EventSystem current = EventSystem.current;
			if (current == null)
			{
				return;
			}
			if (this._pointerData == null)
			{
				this._pointerData = new PointerEventData(current);
			}
			this._pointerData.position = GameplayInput.mousePosition;
			this._hits.Clear();
			current.RaycastAll(this._pointerData, this._hits);
			foreach (RaycastResult raycastResult in this._hits)
			{
				if (!(raycastResult.gameObject == null) && (!(this._phoneCanvas != null) || raycastResult.gameObject.transform.IsChildOf(this._phoneCanvas.transform)))
				{
					this._audio.PlayOneShot(this._clip, this._volume);
					break;
				}
			}
		}

		// Token: 0x0400701D RID: 28701
		private AudioClip _clip;

		// Token: 0x0400701E RID: 28702
		private float _volume = 1f;

		// Token: 0x0400701F RID: 28703
		private Canvas _phoneCanvas;

		// Token: 0x04007020 RID: 28704
		private AudioSource _audio;

		// Token: 0x04007021 RID: 28705
		private bool _active;

		// Token: 0x04007022 RID: 28706
		private readonly List<RaycastResult> _hits = new List<RaycastResult>();

		// Token: 0x04007023 RID: 28707
		private PointerEventData _pointerData;
	}
}
