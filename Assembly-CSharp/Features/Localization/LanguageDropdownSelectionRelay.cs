using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Localization
{
	// Token: 0x020010CC RID: 4300
	internal sealed class LanguageDropdownSelectionRelay : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
	{
		// Token: 0x0600825C RID: 33372 RVA: 0x00354236 File Offset: 0x00352436
		internal void Initialize(LanguageDropdown owner, int index, int versionAtOpen)
		{
			this._owner = owner;
			this._index = index;
			this._versionAtOpen = versionAtOpen;
		}

		// Token: 0x0600825D RID: 33373 RVA: 0x0035424D File Offset: 0x0035244D
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.Activate();
			}
		}

		// Token: 0x0600825E RID: 33374 RVA: 0x0035425D File Offset: 0x0035245D
		public void OnSubmit(BaseEventData eventData)
		{
			this.Activate();
		}

		// Token: 0x0600825F RID: 33375 RVA: 0x00354265 File Offset: 0x00352465
		private void Activate()
		{
			if (this._owner != null)
			{
				this._owner.NotifyItemActivated(this._index, this._versionAtOpen);
			}
		}

		// Token: 0x04008585 RID: 34181
		private LanguageDropdown _owner;

		// Token: 0x04008586 RID: 34182
		private int _index;

		// Token: 0x04008587 RID: 34183
		private int _versionAtOpen;
	}
}
