using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x0200024A RID: 586
	public class TooltipManager : MonoBehaviour
	{
		// Token: 0x06000FDD RID: 4061 RVA: 0x00068D60 File Offset: 0x00066F60
		private void Awake()
		{
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			if (component == null)
			{
				Debug.LogError("<b>[Tooltip]</b> Rect Transform is missing from the object.", this);
				return;
			}
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 1f);
			component.offsetMin = new Vector2(0f, 0f);
			component.offsetMax = new Vector2(0f, 0f);
			this.tooltipContent.GetComponent<RectTransform>().pivot = new Vector2(0f, this.tooltipContent.GetComponent<RectTransform>().pivot.y);
			this.tooltipContent.GetComponent<RectTransform>().pivot = new Vector2(this.tooltipContent.GetComponent<RectTransform>().pivot.x, 0f);
			if (this.mainCanvas == null)
			{
				this.mainCanvas = base.gameObject.GetComponentInParent<Canvas>();
			}
			if (this.cameraSource == TooltipManager.CameraSource.Main)
			{
				this.targetCamera = Camera.main;
			}
			this.contentRect = this.tooltipContent.GetComponentInParent<RectTransform>();
			this.tooltipRect = this.tooltipObject.GetComponent<RectTransform>();
			this.contentPos = new Vector3((float)this.vBorderTop, (float)this.hBorderLeft, 0f);
			base.gameObject.transform.SetAsLastSibling();
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00068EC4 File Offset: 0x000670C4
		private void Update()
		{
			if (!this.allowUpdate)
			{
				return;
			}
			if (this.checkDispose && this.currentTooltip != null && !this.currentTooltip.gameObject.activeInHierarchy)
			{
				this.currentTooltip.ProcessExit();
			}
			this.CheckForPosition();
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00068F14 File Offset: 0x00067114
		private void CheckForPosition()
		{
			this.cursorPos = Input.mousePosition;
			this.uiPos = this.tooltipRect.anchoredPosition;
			this.CheckForBounds();
			if (this.mainCanvas.renderMode != RenderMode.ScreenSpaceCamera && this.mainCanvas.renderMode != RenderMode.WorldSpace)
			{
				if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					this.tooltipRect.position = this.cursorPos;
					if (this.transitionMode == TooltipManager.TransitionMode.Damp)
					{
						this.tooltipContent.transform.position = Vector3.SmoothDamp(this.tooltipContent.transform.position, this.cursorPos + this.contentPos, ref this.tooltipVelocity, this.tooltipSmoothness, this.dampSpeed * 1000f, Time.unscaledDeltaTime);
						return;
					}
					this.tooltipContent.transform.position = this.cursorPos + this.contentPos;
				}
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.tooltipRect.parent.GetComponent<RectTransform>(), this.cursorPos, this.targetCamera, out vector);
			this.tooltipRect.localPosition = vector;
			if (this.transitionMode == TooltipManager.TransitionMode.Damp)
			{
				this.tooltipContent.transform.localPosition = Vector3.SmoothDamp(this.tooltipContent.transform.localPosition, this.contentPos, ref this.tooltipVelocity, this.tooltipSmoothness, this.dampSpeed * 1000f, Time.unscaledDeltaTime);
				return;
			}
			this.tooltipContent.transform.localPosition = this.contentPos;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x000690A4 File Offset: 0x000672A4
		private void CheckForBounds()
		{
			if (this.uiPos.x <= (float)this.xLeft)
			{
				this.contentPos = new Vector3((float)this.hBorderLeft, this.contentPos.y, 0f);
				this.contentRect.pivot = new Vector2(0f, this.contentRect.pivot.y);
			}
			else if (this.uiPos.x >= (float)this.xRight)
			{
				this.contentPos = new Vector3((float)this.hBorderRight, this.contentPos.y, 0f);
				this.contentRect.pivot = new Vector2(1f, this.contentRect.pivot.y);
			}
			if (this.uiPos.y <= (float)this.yTop)
			{
				this.contentPos = new Vector3(this.contentPos.x, (float)this.vBorderBottom, 0f);
				this.contentRect.pivot = new Vector2(this.contentRect.pivot.x, 0f);
				return;
			}
			if (this.uiPos.y >= (float)this.yBottom)
			{
				this.contentPos = new Vector3(this.contentPos.x, (float)this.vBorderTop, 0f);
				this.contentRect.pivot = new Vector2(this.contentRect.pivot.x, 1f);
			}
		}

		// Token: 0x040010D1 RID: 4305
		public Canvas mainCanvas;

		// Token: 0x040010D2 RID: 4306
		public GameObject tooltipObject;

		// Token: 0x040010D3 RID: 4307
		public GameObject tooltipContent;

		// Token: 0x040010D4 RID: 4308
		public Camera targetCamera;

		// Token: 0x040010D5 RID: 4309
		[Range(0.01f, 0.5f)]
		public float tooltipSmoothness = 0.1f;

		// Token: 0x040010D6 RID: 4310
		[Range(5f, 10f)]
		public float dampSpeed = 10f;

		// Token: 0x040010D7 RID: 4311
		public float preferredWidth = 375f;

		// Token: 0x040010D8 RID: 4312
		public bool allowUpdate = true;

		// Token: 0x040010D9 RID: 4313
		public bool checkDispose = true;

		// Token: 0x040010DA RID: 4314
		public TooltipManager.CameraSource cameraSource;

		// Token: 0x040010DB RID: 4315
		public TooltipManager.TransitionMode transitionMode;

		// Token: 0x040010DC RID: 4316
		[Range(-50f, 50f)]
		public int vBorderTop = -15;

		// Token: 0x040010DD RID: 4317
		[Range(-50f, 50f)]
		public int vBorderBottom = 10;

		// Token: 0x040010DE RID: 4318
		[Range(-50f, 50f)]
		public int hBorderLeft = 20;

		// Token: 0x040010DF RID: 4319
		[Range(-50f, 50f)]
		public int hBorderRight = -15;

		// Token: 0x040010E0 RID: 4320
		[SerializeField]
		private int xLeft = -400;

		// Token: 0x040010E1 RID: 4321
		[SerializeField]
		private int xRight = 400;

		// Token: 0x040010E2 RID: 4322
		[SerializeField]
		private int yTop = -325;

		// Token: 0x040010E3 RID: 4323
		[SerializeField]
		private int yBottom = 325;

		// Token: 0x040010E4 RID: 4324
		[HideInInspector]
		public LayoutElement contentLE;

		// Token: 0x040010E5 RID: 4325
		[HideInInspector]
		public TooltipContent currentTooltip;

		// Token: 0x040010E6 RID: 4326
		private Vector2 uiPos;

		// Token: 0x040010E7 RID: 4327
		private Vector3 cursorPos;

		// Token: 0x040010E8 RID: 4328
		private Vector3 contentPos = new Vector3(0f, 0f, 0f);

		// Token: 0x040010E9 RID: 4329
		private Vector3 tooltipVelocity = Vector3.zero;

		// Token: 0x040010EA RID: 4330
		private RectTransform contentRect;

		// Token: 0x040010EB RID: 4331
		private RectTransform tooltipRect;

		// Token: 0x0200024B RID: 587
		public enum CameraSource
		{
			// Token: 0x040010ED RID: 4333
			Main,
			// Token: 0x040010EE RID: 4334
			Custom
		}

		// Token: 0x0200024C RID: 588
		public enum TransitionMode
		{
			// Token: 0x040010F0 RID: 4336
			Damp,
			// Token: 0x040010F1 RID: 4337
			Snap
		}
	}
}
