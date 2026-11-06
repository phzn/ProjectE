using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E4D RID: 3661
	[AddComponentMenu("")]
	public sealed class PhoneScreenRaycaster : GraphicRaycaster
	{
		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x06006D5C RID: 27996 RVA: 0x002BEF84 File Offset: 0x002BD184
		private Canvas ScreenCanvas
		{
			get
			{
				if (!(this._screenCanvas != null))
				{
					return this._screenCanvas = base.GetComponent<Canvas>();
				}
				return this._screenCanvas;
			}
		}

		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x06006D5D RID: 27997 RVA: 0x002BEFB5 File Offset: 0x002BD1B5
		public override int sortOrderPriority
		{
			get
			{
				if (this.ScreenCanvas.renderMode != RenderMode.WorldSpace)
				{
					return base.sortOrderPriority;
				}
				return this.ScreenCanvas.sortingOrder;
			}
		}

		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06006D5E RID: 27998 RVA: 0x002BEFD7 File Offset: 0x002BD1D7
		public override int renderOrderPriority
		{
			get
			{
				if (this.ScreenCanvas.renderMode != RenderMode.WorldSpace)
				{
					return base.renderOrderPriority;
				}
				return this.ScreenCanvas.rootCanvas.renderOrder;
			}
		}

		// Token: 0x06006D5F RID: 27999 RVA: 0x002BEFFE File Offset: 0x002BD1FE
		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if (this._owner == null || !this._owner.IsScreenInteractionActive)
			{
				return;
			}
			base.Raycast(eventData, resultAppendList);
		}

		// Token: 0x06006D60 RID: 28000 RVA: 0x002BF024 File Offset: 0x002BD224
		public static void EnsureInstalled(GameObject phoneRoot, SmartphoneController owner)
		{
			if (phoneRoot == null || owner == null)
			{
				return;
			}
			foreach (Canvas canvas in phoneRoot.GetComponentsInChildren<Canvas>(true))
			{
				if (canvas.renderMode == RenderMode.WorldSpace)
				{
					PhoneScreenRaycaster phoneScreenRaycaster = canvas.GetComponent<PhoneScreenRaycaster>();
					foreach (GraphicRaycaster graphicRaycaster in canvas.GetComponents<GraphicRaycaster>())
					{
						if (!(graphicRaycaster.GetType() != typeof(GraphicRaycaster)))
						{
							if (phoneScreenRaycaster == null)
							{
								bool enabled = graphicRaycaster.enabled;
								graphicRaycaster.enabled = false;
								phoneScreenRaycaster = canvas.gameObject.AddComponent<PhoneScreenRaycaster>();
								phoneScreenRaycaster.ignoreReversedGraphics = graphicRaycaster.ignoreReversedGraphics;
								phoneScreenRaycaster.blockingObjects = graphicRaycaster.blockingObjects;
								phoneScreenRaycaster.blockingMask = graphicRaycaster.blockingMask;
								phoneScreenRaycaster.enabled = enabled;
							}
							else
							{
								graphicRaycaster.enabled = false;
							}
						}
					}
					if (phoneScreenRaycaster != null)
					{
						phoneScreenRaycaster._owner = owner;
						phoneScreenRaycaster._screenCanvas = canvas;
					}
				}
			}
		}

		// Token: 0x04007009 RID: 28681
		private SmartphoneController _owner;

		// Token: 0x0400700A RID: 28682
		private Canvas _screenCanvas;
	}
}
