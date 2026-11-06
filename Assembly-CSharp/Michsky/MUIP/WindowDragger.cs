using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.MUIP
{
	// Token: 0x02000268 RID: 616
	public class WindowDragger : UIBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06001029 RID: 4137 RVA: 0x0006B5B0 File Offset: 0x000697B0
		public new void Start()
		{
			if (this.dragArea == null)
			{
				try
				{
					Canvas canvas = (Canvas)Object.FindObjectsOfType(typeof(Canvas))[0];
					this.dragArea = canvas.GetComponent<RectTransform>();
				}
				catch
				{
					Debug.LogError("<b>[Movable Window]</b> Drag Area has not been assigned.");
				}
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x0006B610 File Offset: 0x00069810
		private RectTransform DragObjectInternal
		{
			get
			{
				if (this.dragObject == null)
				{
					return base.transform as RectTransform;
				}
				return this.dragObject;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x0006B634 File Offset: 0x00069834
		private RectTransform DragAreaInternal
		{
			get
			{
				if (this.dragArea == null)
				{
					RectTransform rectTransform = base.transform as RectTransform;
					while (rectTransform.parent != null && rectTransform.parent is RectTransform)
					{
						rectTransform = rectTransform.parent as RectTransform;
					}
					return rectTransform;
				}
				return this.dragArea;
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0006B68C File Offset: 0x0006988C
		public void OnBeginDrag(PointerEventData data)
		{
			this.originalPanelLocalPosition = this.DragObjectInternal.localPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.DragAreaInternal, data.position, data.pressEventCamera, out this.originalLocalPointerPosition);
			base.gameObject.transform.SetAsLastSibling();
			if (this.topOnDrag)
			{
				this.dragObject.transform.SetAsLastSibling();
			}
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0006B6F0 File Offset: 0x000698F0
		public void OnDrag(PointerEventData data)
		{
			Vector2 vector;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.DragAreaInternal, data.position, data.pressEventCamera, out vector))
			{
				Vector3 vector2 = vector - this.originalLocalPointerPosition;
				this.DragObjectInternal.localPosition = this.originalPanelLocalPosition + vector2;
			}
			this.ClampToArea();
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0006B748 File Offset: 0x00069948
		private void ClampToArea()
		{
			Vector3 localPosition = this.DragObjectInternal.localPosition;
			Vector3 vector = this.DragAreaInternal.rect.min - this.DragObjectInternal.rect.min;
			Vector3 vector2 = this.DragAreaInternal.rect.max - this.DragObjectInternal.rect.max;
			localPosition.x = Mathf.Clamp(this.DragObjectInternal.localPosition.x, vector.x, vector2.x);
			localPosition.y = Mathf.Clamp(this.DragObjectInternal.localPosition.y, vector.y, vector2.y);
			this.DragObjectInternal.localPosition = localPosition;
		}

		// Token: 0x040011BB RID: 4539
		[Header("Resources")]
		public RectTransform dragArea;

		// Token: 0x040011BC RID: 4540
		public RectTransform dragObject;

		// Token: 0x040011BD RID: 4541
		[Header("Settings")]
		public bool topOnDrag = true;

		// Token: 0x040011BE RID: 4542
		private Vector2 originalLocalPointerPosition;

		// Token: 0x040011BF RID: 4543
		private Vector3 originalPanelLocalPosition;
	}
}
