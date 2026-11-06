using System;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x020001DD RID: 477
	[RequireComponent(typeof(Animator))]
	public class ContextMenuManager : MonoBehaviour
	{
		// Token: 0x06000DDA RID: 3546 RVA: 0x0005F4EC File Offset: 0x0005D6EC
		private void Awake()
		{
			if (this.mainCanvas == null)
			{
				this.mainCanvas = base.gameObject.GetComponentInParent<Canvas>();
			}
			if (this.contextAnimator == null)
			{
				this.contextAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.cameraSource == ContextMenuManager.CameraSource.Main)
			{
				this.targetCamera = Camera.main;
			}
			this.contextRect = base.gameObject.GetComponent<RectTransform>();
			this.contentRect = this.contextContent.GetComponent<RectTransform>();
			this.contentPos = new Vector3((float)this.vBorderTop, (float)this.hBorderLeft, 0f);
			base.gameObject.transform.SetAsLastSibling();
			this.subMenuBehaviour = ContextMenuManager.SubMenuBehaviour.Click;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0005F5A4 File Offset: 0x0005D7A4
		public void CheckForBound()
		{
			if (this.uiPos.x <= -100f)
			{
				this.horizontalBound = ContextMenuManager.CursorBoundHorizontal.Left;
				this.contentPos = new Vector3((float)this.hBorderLeft, this.contentPos.y, 0f);
				this.contentRect.pivot = new Vector2(0f, this.contentRect.pivot.y);
			}
			else if (this.uiPos.x >= 100f)
			{
				this.horizontalBound = ContextMenuManager.CursorBoundHorizontal.Right;
				this.contentPos = new Vector3((float)this.hBorderRight, this.contentPos.y, 0f);
				this.contentRect.pivot = new Vector2(1f, this.contentRect.pivot.y);
			}
			if (this.uiPos.y <= -75f)
			{
				this.verticalBound = ContextMenuManager.CursorBoundVertical.Bottom;
				this.contentPos = new Vector3(this.contentPos.x, (float)this.vBorderBottom, 0f);
				this.contentRect.pivot = new Vector2(this.contentRect.pivot.x, 0f);
				return;
			}
			if (this.uiPos.y >= 75f)
			{
				this.verticalBound = ContextMenuManager.CursorBoundVertical.Top;
				this.contentPos = new Vector3(this.contentPos.x, (float)this.vBorderTop, 0f);
				this.contentRect.pivot = new Vector2(this.contentRect.pivot.x, 1f);
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0005F734 File Offset: 0x0005D934
		public void SetContextMenuPosition()
		{
			this.cursorPos = Input.mousePosition;
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceCamera || this.mainCanvas.renderMode == RenderMode.WorldSpace)
			{
				this.contextRect.position = this.targetCamera.ScreenToWorldPoint(this.cursorPos);
				this.contextRect.localPosition = new Vector3(this.contextRect.localPosition.x, this.contextRect.localPosition.y, 0f);
				this.contextContent.transform.localPosition = Vector3.SmoothDamp(this.contextContent.transform.localPosition, this.contentPos, ref this.contextVelocity, 0f);
			}
			else if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.contextRect.position = this.cursorPos;
				this.contextContent.transform.position = new Vector3(this.cursorPos.x + this.contentPos.x, this.cursorPos.y + this.contentPos.y, 0f);
			}
			this.uiPos = this.contextRect.anchoredPosition;
			this.CheckForBound();
			if (this.debugMode)
			{
				this.PrintDebug();
			}
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0005F880 File Offset: 0x0005DA80
		public void SetFixedPosition()
		{
			this.cursorPos = Input.mousePosition;
			this.SetContextMenuPosition();
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceCamera || this.mainCanvas.renderMode == RenderMode.WorldSpace)
			{
				this.contextRect.position = this.targetCamera.ScreenToWorldPoint(this.cursorPos);
				this.contextRect.localPosition = new Vector3(this.contextRect.localPosition.x, this.contextRect.localPosition.y, 0f);
				this.contextContent.transform.localPosition = Vector3.SmoothDamp(this.contextContent.transform.localPosition, this.contentPos, ref this.contextVelocity, 0f);
			}
			else if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.contextRect.position = this.cursorPos;
				this.contextContent.transform.position = new Vector3(this.cursorPos.x + this.contentPos.x, this.cursorPos.y + this.contentPos.y, 0f);
			}
			this.uiPos = this.contextRect.anchoredPosition;
			this.CheckForBound();
			if (this.debugMode)
			{
				this.PrintDebug();
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0005F9D4 File Offset: 0x0005DBD4
		private void ProcessContextRect()
		{
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceCamera || this.mainCanvas.renderMode == RenderMode.WorldSpace)
			{
				this.contextRect.position = this.targetCamera.ScreenToWorldPoint(this.cursorPos);
				this.contextRect.localPosition = new Vector3(this.contextRect.localPosition.x, this.contextRect.localPosition.y, 0f);
				this.contextContent.transform.localPosition = Vector3.SmoothDamp(this.contextContent.transform.localPosition, this.contentPos, ref this.contextVelocity, 0f);
				return;
			}
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.contextRect.position = this.cursorPos;
				this.contextContent.transform.position = new Vector3(this.cursorPos.x + this.contentPos.x, this.cursorPos.y + this.contentPos.y, 0f);
			}
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0005FAF0 File Offset: 0x0005DCF0
		private void PrintDebug()
		{
			string[] array = new string[6];
			array[0] = "<b>[Context Menu]</b> UI Pos: ";
			int num = 1;
			Vector2 vector = this.uiPos;
			array[num] = vector.ToString();
			array[2] = ", H: ";
			array[3] = this.horizontalBound.ToString();
			array[4] = ", V: ";
			array[5] = this.verticalBound.ToString();
			Debug.Log(string.Concat(array), this);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0005FB65 File Offset: 0x0005DD65
		public void Open()
		{
			this.contextAnimator.Play("Menu In");
			this.isOn = true;
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0005FB7E File Offset: 0x0005DD7E
		public void Close()
		{
			this.contextAnimator.Play("Menu Out");
			this.isOn = false;
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0005FB97 File Offset: 0x0005DD97
		public void OpenInFixedPosition()
		{
			this.SetFixedPosition();
			this.Open();
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0005FBA5 File Offset: 0x0005DDA5
		public void OpenContextMenu()
		{
			this.Open();
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0005FBAD File Offset: 0x0005DDAD
		public void CloseOnClick()
		{
			this.Close();
		}

		// Token: 0x04000E58 RID: 3672
		public Canvas mainCanvas;

		// Token: 0x04000E59 RID: 3673
		public Camera targetCamera;

		// Token: 0x04000E5A RID: 3674
		public GameObject contextContent;

		// Token: 0x04000E5B RID: 3675
		public Animator contextAnimator;

		// Token: 0x04000E5C RID: 3676
		public GameObject contextButton;

		// Token: 0x04000E5D RID: 3677
		public GameObject contextSeparator;

		// Token: 0x04000E5E RID: 3678
		public GameObject contextSubMenu;

		// Token: 0x04000E5F RID: 3679
		[SerializeField]
		private bool debugMode;

		// Token: 0x04000E60 RID: 3680
		public bool autoSubMenuPosition = true;

		// Token: 0x04000E61 RID: 3681
		public ContextMenuManager.SubMenuBehaviour subMenuBehaviour;

		// Token: 0x04000E62 RID: 3682
		public ContextMenuManager.CameraSource cameraSource;

		// Token: 0x04000E63 RID: 3683
		public ContextMenuManager.CursorBoundHorizontal horizontalBound;

		// Token: 0x04000E64 RID: 3684
		public ContextMenuManager.CursorBoundVertical verticalBound;

		// Token: 0x04000E65 RID: 3685
		[Range(-50f, 50f)]
		public int vBorderTop = -10;

		// Token: 0x04000E66 RID: 3686
		[Range(-50f, 50f)]
		public int vBorderBottom = 10;

		// Token: 0x04000E67 RID: 3687
		[Range(-50f, 50f)]
		public int hBorderLeft = 15;

		// Token: 0x04000E68 RID: 3688
		[Range(-50f, 50f)]
		public int hBorderRight = -15;

		// Token: 0x04000E69 RID: 3689
		private Vector2 uiPos;

		// Token: 0x04000E6A RID: 3690
		private Vector3 cursorPos;

		// Token: 0x04000E6B RID: 3691
		private Vector3 contentPos = new Vector3(0f, 0f, 0f);

		// Token: 0x04000E6C RID: 3692
		private Vector3 contextVelocity = Vector3.zero;

		// Token: 0x04000E6D RID: 3693
		private RectTransform contextRect;

		// Token: 0x04000E6E RID: 3694
		private RectTransform contentRect;

		// Token: 0x04000E6F RID: 3695
		[HideInInspector]
		public bool isOn;

		// Token: 0x020001DE RID: 478
		public enum CameraSource
		{
			// Token: 0x04000E71 RID: 3697
			Main,
			// Token: 0x04000E72 RID: 3698
			Custom
		}

		// Token: 0x020001DF RID: 479
		public enum SubMenuBehaviour
		{
			// Token: 0x04000E74 RID: 3700
			Hover,
			// Token: 0x04000E75 RID: 3701
			Click
		}

		// Token: 0x020001E0 RID: 480
		public enum CursorBoundHorizontal
		{
			// Token: 0x04000E77 RID: 3703
			Left,
			// Token: 0x04000E78 RID: 3704
			Right
		}

		// Token: 0x020001E1 RID: 481
		public enum CursorBoundVertical
		{
			// Token: 0x04000E7A RID: 3706
			Bottom,
			// Token: 0x04000E7B RID: 3707
			Top
		}
	}
}
