using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000220 RID: 544
	[RequireComponent(typeof(CanvasGroup))]
	public class ModalWindowManager : MonoBehaviour
	{
		// Token: 0x06000F16 RID: 3862 RVA: 0x000650B8 File Offset: 0x000632B8
		private void Awake()
		{
			this.isOn = false;
			if (this.mwAnimator == null)
			{
				this.mwAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.closeOnCancel)
			{
				this.onCancel.AddListener(new UnityAction(this.CloseWindow));
			}
			if (this.closeOnConfirm)
			{
				this.onConfirm.AddListener(new UnityAction(this.CloseWindow));
			}
			if (this.confirmButton != null)
			{
				this.confirmButton.onClick.AddListener(new UnityAction(this.onConfirm.Invoke));
			}
			if (this.cancelButton != null)
			{
				this.cancelButton.onClick.AddListener(new UnityAction(this.onCancel.Invoke));
			}
			if (this.startBehaviour == ModalWindowManager.StartBehaviour.Disable)
			{
				this.isOn = false;
				base.gameObject.SetActive(false);
			}
			else if (this.startBehaviour == ModalWindowManager.StartBehaviour.Enable)
			{
				this.isOn = false;
				this.OpenWindow();
			}
			this.cachedStateLength = MUIPInternalTools.GetAnimatorClipLength(this.mwAnimator, MUIPInternalTools.modalWindowStateName);
			this.UpdateUI();
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x000651D7 File Offset: 0x000633D7
		private void OnEnable()
		{
			if (this.onEnableBehaviour == ModalWindowManager.OnEnableBehaviour.Restore && this.isOn)
			{
				this.isOn = false;
				this.Open();
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x000651F7 File Offset: 0x000633F7
		private void OnDisable()
		{
			if (this.onEnableBehaviour == ModalWindowManager.OnEnableBehaviour.None)
			{
				this.isOn = false;
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00065208 File Offset: 0x00063408
		public void UpdateUI()
		{
			if (this.useCustomContent)
			{
				return;
			}
			if (this.windowIcon != null)
			{
				this.windowIcon.sprite = this.icon;
			}
			if (this.windowTitle != null)
			{
				this.windowTitle.text = this.titleText;
			}
			if (this.windowDescription != null)
			{
				this.windowDescription.text = this.descriptionText;
			}
			if (this.showCancelButton && this.cancelButton != null)
			{
				this.cancelButton.gameObject.SetActive(true);
			}
			else if (this.cancelButton != null)
			{
				this.cancelButton.gameObject.SetActive(false);
			}
			if (this.showConfirmButton && this.confirmButton != null)
			{
				this.confirmButton.gameObject.SetActive(true);
				return;
			}
			if (this.confirmButton != null)
			{
				this.confirmButton.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0006530C File Offset: 0x0006350C
		public void Open()
		{
			if (this.isOn)
			{
				return;
			}
			this.isOn = true;
			base.gameObject.SetActive(true);
			this.onOpen.Invoke();
			base.StopCoroutine("DisableObject");
			this.mwAnimator.Play("Fade-in");
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0006535B File Offset: 0x0006355B
		public void Close()
		{
			if (!this.isOn)
			{
				return;
			}
			this.isOn = false;
			this.onClose.Invoke();
			this.mwAnimator.Play("Fade-out");
			base.StartCoroutine("DisableObject");
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00065394 File Offset: 0x00063594
		public void OpenWindow()
		{
			this.Open();
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0006539C File Offset: 0x0006359C
		public void CloseWindow()
		{
			this.Close();
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x000653A4 File Offset: 0x000635A4
		public void AnimateWindow()
		{
			if (!this.isOn)
			{
				base.StopCoroutine("DisableObject");
				this.isOn = true;
				base.gameObject.SetActive(true);
				this.mwAnimator.Play("Fade-in");
				return;
			}
			this.isOn = false;
			this.mwAnimator.Play("Fade-out");
			base.StartCoroutine("DisableObject");
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0006540B File Offset: 0x0006360B
		private IEnumerator DisableObject()
		{
			yield return new WaitForSecondsRealtime(this.cachedStateLength);
			if (this.closeBehaviour == ModalWindowManager.CloseBehaviour.Disable)
			{
				base.gameObject.SetActive(false);
			}
			else if (this.closeBehaviour == ModalWindowManager.CloseBehaviour.Destroy)
			{
				Object.Destroy(base.gameObject);
			}
			yield break;
		}

		// Token: 0x04000FD8 RID: 4056
		public Image windowIcon;

		// Token: 0x04000FD9 RID: 4057
		public TextMeshProUGUI windowTitle;

		// Token: 0x04000FDA RID: 4058
		public TextMeshProUGUI windowDescription;

		// Token: 0x04000FDB RID: 4059
		public ButtonManager confirmButton;

		// Token: 0x04000FDC RID: 4060
		public ButtonManager cancelButton;

		// Token: 0x04000FDD RID: 4061
		public Animator mwAnimator;

		// Token: 0x04000FDE RID: 4062
		public Sprite icon;

		// Token: 0x04000FDF RID: 4063
		public string titleText = "Title";

		// Token: 0x04000FE0 RID: 4064
		[TextArea(1, 4)]
		public string descriptionText = "Description here";

		// Token: 0x04000FE1 RID: 4065
		public UnityEvent onOpen = new UnityEvent();

		// Token: 0x04000FE2 RID: 4066
		public UnityEvent onClose = new UnityEvent();

		// Token: 0x04000FE3 RID: 4067
		public UnityEvent onConfirm = new UnityEvent();

		// Token: 0x04000FE4 RID: 4068
		public UnityEvent onCancel = new UnityEvent();

		// Token: 0x04000FE5 RID: 4069
		public bool useCustomContent;

		// Token: 0x04000FE6 RID: 4070
		public bool isOn;

		// Token: 0x04000FE7 RID: 4071
		public bool closeOnCancel = true;

		// Token: 0x04000FE8 RID: 4072
		public bool closeOnConfirm = true;

		// Token: 0x04000FE9 RID: 4073
		public bool showCancelButton = true;

		// Token: 0x04000FEA RID: 4074
		public bool showConfirmButton = true;

		// Token: 0x04000FEB RID: 4075
		public ModalWindowManager.StartBehaviour startBehaviour = ModalWindowManager.StartBehaviour.Disable;

		// Token: 0x04000FEC RID: 4076
		public ModalWindowManager.CloseBehaviour closeBehaviour = ModalWindowManager.CloseBehaviour.Disable;

		// Token: 0x04000FED RID: 4077
		public ModalWindowManager.OnEnableBehaviour onEnableBehaviour;

		// Token: 0x04000FEE RID: 4078
		private float cachedStateLength;

		// Token: 0x02000221 RID: 545
		public enum StartBehaviour
		{
			// Token: 0x04000FF0 RID: 4080
			None,
			// Token: 0x04000FF1 RID: 4081
			Disable,
			// Token: 0x04000FF2 RID: 4082
			Enable
		}

		// Token: 0x02000222 RID: 546
		public enum CloseBehaviour
		{
			// Token: 0x04000FF4 RID: 4084
			None,
			// Token: 0x04000FF5 RID: 4085
			Disable,
			// Token: 0x04000FF6 RID: 4086
			Destroy
		}

		// Token: 0x02000223 RID: 547
		public enum OnEnableBehaviour
		{
			// Token: 0x04000FF8 RID: 4088
			None,
			// Token: 0x04000FF9 RID: 4089
			Restore
		}
	}
}
