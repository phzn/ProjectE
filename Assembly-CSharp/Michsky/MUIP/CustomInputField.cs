using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.MUIP
{
	// Token: 0x02000212 RID: 530
	[RequireComponent(typeof(TMP_InputField))]
	[RequireComponent(typeof(Animator))]
	public class CustomInputField : MonoBehaviour
	{
		// Token: 0x06000ED3 RID: 3795 RVA: 0x000640FC File Offset: 0x000622FC
		private void Awake()
		{
			this.Initialize();
			this.inputText.onSelect.AddListener(delegate
			{
				this.AnimateIn();
			});
			this.inputText.onEndEdit.AddListener(delegate
			{
				this.HandleEndEdit();
			});
			this.inputText.onValueChanged.AddListener(delegate
			{
				this.UpdateState();
			});
			this.UpdateStateInstant();
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00064169 File Offset: 0x00062369
		private void OnEnable()
		{
			if (this.inputText == null || this.inputFieldAnimator == null)
			{
				this.Initialize();
			}
			this.inputText.ForceLabelUpdate();
			this.UpdateStateInstant();
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x000641A0 File Offset: 0x000623A0
		private void Update()
		{
			if (!this.processSubmit || string.IsNullOrEmpty(this.inputText.text) || EventSystem.current.currentSelectedGameObject != this.inputText.gameObject)
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.Return))
			{
				this.onSubmit.Invoke();
				if (this.clearOnSubmit)
				{
					this.inputText.text = "";
					this.UpdateState();
				}
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00064216 File Offset: 0x00062416
		private void Initialize()
		{
			if (this.inputText == null)
			{
				this.inputText = base.gameObject.GetComponent<TMP_InputField>();
			}
			if (this.inputFieldAnimator == null)
			{
				this.inputFieldAnimator = base.gameObject.GetComponent<Animator>();
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00064258 File Offset: 0x00062458
		public void AnimateIn()
		{
			if (this.inputFieldAnimator.gameObject.activeInHierarchy && !this.isActive)
			{
				base.StopCoroutine("DisableAnimator");
				base.StartCoroutine("DisableAnimator");
				this.isActive = true;
				this.inputFieldAnimator.enabled = true;
				this.inputFieldAnimator.Play(this.inAnim);
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x000642BC File Offset: 0x000624BC
		public void AnimateOut()
		{
			if (this.inputFieldAnimator.gameObject.activeInHierarchy && this.inputText.text.Length == 0 && this.isActive)
			{
				base.StopCoroutine("DisableAnimator");
				base.StartCoroutine("DisableAnimator");
				this.isActive = false;
				this.inputFieldAnimator.enabled = true;
				this.inputFieldAnimator.Play(this.outAnim);
			}
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00064330 File Offset: 0x00062530
		public void UpdateState()
		{
			if (this.inputText.text.Length == 0)
			{
				this.AnimateOut();
				return;
			}
			this.AnimateIn();
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00064354 File Offset: 0x00062554
		public void UpdateStateInstant()
		{
			this.inputFieldAnimator.enabled = true;
			base.StopCoroutine("DisableAnimator");
			base.StartCoroutine("DisableAnimator");
			if (this.inputText.text.Length == 0)
			{
				this.isActive = false;
				this.inputFieldAnimator.Play(this.instaOutAnim);
				return;
			}
			this.isActive = true;
			this.inputFieldAnimator.Play(this.instaInAnim);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000643C8 File Offset: 0x000625C8
		private void HandleEndEdit()
		{
			if (this.setEventSystem && string.IsNullOrEmpty(this.inputText.text) && !EventSystem.current.alreadySelecting && EventSystem.current.currentSelectedGameObject == this.inputText.gameObject)
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			this.AnimateOut();
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00064428 File Offset: 0x00062628
		private IEnumerator DisableAnimator()
		{
			yield return new WaitForSecondsRealtime(this.cachedDuration);
			this.inputFieldAnimator.enabled = false;
			yield break;
		}

		// Token: 0x04000F8E RID: 3982
		[Header("Resources")]
		public TMP_InputField inputText;

		// Token: 0x04000F8F RID: 3983
		[SerializeField]
		private Animator inputFieldAnimator;

		// Token: 0x04000F90 RID: 3984
		[Header("Settings")]
		public bool processSubmit;

		// Token: 0x04000F91 RID: 3985
		public bool clearOnSubmit = true;

		// Token: 0x04000F92 RID: 3986
		[Tooltip("Set the current event system object as null.")]
		[SerializeField]
		private bool setEventSystem;

		// Token: 0x04000F93 RID: 3987
		[Header("Events")]
		public UnityEvent onSubmit = new UnityEvent();

		// Token: 0x04000F94 RID: 3988
		private float cachedDuration = 0.5f;

		// Token: 0x04000F95 RID: 3989
		private string inAnim = "In";

		// Token: 0x04000F96 RID: 3990
		private string outAnim = "Out";

		// Token: 0x04000F97 RID: 3991
		private string instaInAnim = "Instant In";

		// Token: 0x04000F98 RID: 3992
		private string instaOutAnim = "Instant Out";

		// Token: 0x04000F99 RID: 3993
		private bool isActive;
	}
}
