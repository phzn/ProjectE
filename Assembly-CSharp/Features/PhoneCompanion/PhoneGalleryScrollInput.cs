using System;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E42 RID: 3650
	[DisallowMultipleComponent]
	public sealed class PhoneGalleryScrollInput : MonoBehaviour, IScrollHandler, IEventSystemHandler
	{
		// Token: 0x06006D13 RID: 27923 RVA: 0x002BC194 File Offset: 0x002BA394
		public static void Ensure(ScrollRect scroll)
		{
			if (scroll == null || scroll.viewport == null || scroll.viewport.gameObject == scroll.gameObject)
			{
				return;
			}
			PhoneGalleryScrollInput phoneGalleryScrollInput;
			if (!scroll.viewport.TryGetComponent<PhoneGalleryScrollInput>(out phoneGalleryScrollInput))
			{
				phoneGalleryScrollInput = scroll.viewport.gameObject.AddComponent<PhoneGalleryScrollInput>();
			}
			phoneGalleryScrollInput._scroll = scroll;
		}

		// Token: 0x06006D14 RID: 27924 RVA: 0x002BC1F8 File Offset: 0x002BA3F8
		public void OnScroll(PointerEventData eventData)
		{
			if (this._scroll == null || !this._scroll.isActiveAndEnabled || eventData == null)
			{
				return;
			}
			if (GameplayInput.GetKey(KeyCode.LeftShift) || GameplayInput.GetKey(KeyCode.RightShift))
			{
				return;
			}
			Vector2 scrollDelta = eventData.scrollDelta;
			Vector2 vector = scrollDelta;
			InputSystemUIInputModule inputSystemUIInputModule = eventData.currentInputModule as InputSystemUIInputModule;
			if (inputSystemUIInputModule != null)
			{
				float num = Mathf.Abs(inputSystemUIInputModule.scrollDeltaPerTick);
				if (num <= Mathf.Epsilon)
				{
					return;
				}
				vector /= num;
			}
			try
			{
				eventData.scrollDelta = vector;
				this._scroll.StopMovement();
				this._scroll.OnScroll(eventData);
			}
			finally
			{
				eventData.scrollDelta = scrollDelta;
			}
		}

		// Token: 0x04006FBD RID: 28605
		private ScrollRect _scroll;
	}
}
