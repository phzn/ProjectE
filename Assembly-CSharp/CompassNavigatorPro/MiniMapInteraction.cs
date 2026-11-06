using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CompassNavigatorPro
{
	// Token: 0x020002A9 RID: 681
	public class MiniMapInteraction : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x0600134E RID: 4942 RVA: 0x0007CFD7 File Offset: 0x0007B1D7
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.compass != null)
			{
				this.compass.BubbleEvent<Vector2>(this.compass.OnMiniMapMouseEnter, eventData.position);
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0007D003 File Offset: 0x0007B203
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.compass != null)
			{
				this.compass.BubbleEvent<Vector2>(this.compass.OnMiniMapMouseExit, eventData.position);
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0007D030 File Offset: 0x0007B230
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.compass != null && !this.isDragging && this.dragEndFrame != Time.frameCount && this.compass.miniMapIconEvents)
			{
				Vector3 worldPositionFromPointerEvent = this.compass.GetWorldPositionFromPointerEvent(eventData.position);
				this.compass.BubbleEvent<Vector3, int>(this.compass.OnMiniMapMouseClick, worldPositionFromPointerEvent, (int)eventData.button);
			}
			this.isDragging = false;
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0007D0A4 File Offset: 0x0007B2A4
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.isDragging = true;
			if (this.compass != null && this.compass.currentMiniMapAllowsUserDrag)
			{
				this.dragStartWorldPosition = this.compass.GetWorldPositionFromPointerEvent(eventData.position);
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0007D0F4 File Offset: 0x0007B2F4
		public void OnDrag(PointerEventData eventData)
		{
			if (this.isDragging && this.compass != null && this.compass.currentMiniMapAllowsUserDrag)
			{
				Vector3 worldPositionFromPointerEvent = this.compass.GetWorldPositionFromPointerEvent(eventData.position);
				this.dampDir = worldPositionFromPointerEvent - this.dragStartWorldPosition;
				this.UpdateOffset();
			}
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0007D14E File Offset: 0x0007B34E
		private void UpdateOffset()
		{
			if (this.compass == null)
			{
				return;
			}
			this.compass.miniMapFollowOffset -= this.dampDir;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0007D17C File Offset: 0x0007B37C
		public void OnEndDrag(PointerEventData eventData)
		{
			this.isDragging = false;
			this.dragEndFrame = Time.frameCount;
			if (this.compass != null)
			{
				if (this.compass.miniMapFullScreenState && this.compass.miniMapFullScreenAutoResetDrag)
				{
					this.compass.ResetDragOffset();
					return;
				}
				if (!this.compass.miniMapFullScreenState && this.compass.miniMapAutoResetDrag)
				{
					this.compass.ResetDragOffset();
				}
			}
		}

		// Token: 0x040014B6 RID: 5302
		[NonSerialized]
		public CompassPro compass;

		// Token: 0x040014B7 RID: 5303
		private bool isDragging;

		// Token: 0x040014B8 RID: 5304
		private Vector3 dragStartWorldPosition;

		// Token: 0x040014B9 RID: 5305
		private Vector3 dampDir;

		// Token: 0x040014BA RID: 5306
		private int dragEndFrame;
	}
}
