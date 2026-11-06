using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000242 RID: 578
	[RequireComponent(typeof(Toggle))]
	[RequireComponent(typeof(Animator))]
	public class CustomToggle : MonoBehaviour
	{
		// Token: 0x06000FAF RID: 4015 RVA: 0x00068450 File Offset: 0x00066650
		private void Awake()
		{
			if (this.toggleObject == null)
			{
				this.toggleObject = base.gameObject.GetComponent<Toggle>();
			}
			if (this.toggleAnimator == null)
			{
				this.toggleAnimator = this.toggleObject.GetComponent<Animator>();
			}
			if (this.invokeOnAwake)
			{
				this.toggleObject.onValueChanged.Invoke(this.toggleObject.isOn);
			}
			this.toggleObject.onValueChanged.AddListener(new UnityAction<bool>(this.UpdateState));
			this.UpdateState();
			this.isInitialized = true;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x000684E7 File Offset: 0x000666E7
		private void OnEnable()
		{
			if (!this.isInitialized)
			{
				return;
			}
			this.UpdateState();
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x000684F8 File Offset: 0x000666F8
		public void UpdateState()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			base.StopCoroutine("DisableAnimator");
			base.StartCoroutine("DisableAnimator");
			this.toggleAnimator.enabled = true;
			if (this.toggleObject.isOn)
			{
				this.toggleAnimator.Play("On Instant");
				return;
			}
			this.toggleAnimator.Play("Off Instant");
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00068568 File Offset: 0x00066768
		public void UpdateState(bool value)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			base.StopCoroutine("DisableAnimator");
			base.StartCoroutine("DisableAnimator");
			this.toggleAnimator.enabled = true;
			if (this.toggleObject.isOn)
			{
				this.toggleAnimator.Play("Toggle On");
				return;
			}
			this.toggleAnimator.Play("Toggle Off");
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x000685D6 File Offset: 0x000667D6
		private IEnumerator DisableAnimator()
		{
			yield return new WaitForSecondsRealtime(0.6f);
			this.toggleAnimator.enabled = false;
			yield break;
		}

		// Token: 0x040010B3 RID: 4275
		[HideInInspector]
		public Toggle toggleObject;

		// Token: 0x040010B4 RID: 4276
		[HideInInspector]
		public Animator toggleAnimator;

		// Token: 0x040010B5 RID: 4277
		[Header("Settings")]
		public bool invokeOnAwake;

		// Token: 0x040010B6 RID: 4278
		private bool isInitialized;
	}
}
