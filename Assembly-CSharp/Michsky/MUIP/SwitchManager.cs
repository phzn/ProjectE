using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x0200023F RID: 575
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Button))]
	public class SwitchManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
	{
		// Token: 0x06000F9D RID: 3997 RVA: 0x00067DF8 File Offset: 0x00065FF8
		private void Awake()
		{
			if (this.switchAnimator == null)
			{
				this.switchAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.switchButton == null)
			{
				this.switchButton = base.gameObject.GetComponent<Button>();
				this.switchButton.onClick.AddListener(new UnityAction(this.AnimateSwitch));
				if (this.enableSwitchSounds && this.useClickSound)
				{
					this.switchButton.onClick.AddListener(delegate
					{
						this.soundSource.PlayOneShot(this.clickSound);
					});
				}
			}
			if (this.saveValue)
			{
				this.GetSavedData();
			}
			else
			{
				if (base.gameObject.activeInHierarchy)
				{
					base.StopCoroutine("DisableAnimator");
				}
				if (base.gameObject.activeInHierarchy)
				{
					base.StartCoroutine("DisableAnimator");
				}
				this.switchAnimator.enabled = true;
				if (this.isOn)
				{
					this.switchAnimator.Play("On Instant");
				}
				else
				{
					this.switchAnimator.Play("Off Instant");
				}
			}
			if (this.invokeAtStart && this.isOn)
			{
				this.OnEvents.Invoke();
			}
			else if (this.invokeAtStart && !this.isOn)
			{
				this.OffEvents.Invoke();
			}
			this.isInitialized = true;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00067F41 File Offset: 0x00066141
		private void OnEnable()
		{
			if (this.isInitialized)
			{
				this.UpdateUI();
			}
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00067F54 File Offset: 0x00066154
		private void GetSavedData()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StopCoroutine("DisableAnimator");
				base.StartCoroutine("DisableAnimator");
			}
			this.switchAnimator.enabled = true;
			if (PlayerPrefs.GetString(this.switchTag + "Switch") == "" || !PlayerPrefs.HasKey(this.switchTag + "Switch"))
			{
				if (this.isOn)
				{
					this.switchAnimator.Play("Switch On");
					PlayerPrefs.SetString(this.switchTag + "Switch", "true");
					return;
				}
				this.switchAnimator.Play("Switch Off");
				PlayerPrefs.SetString(this.switchTag + "Switch", "false");
				return;
			}
			else
			{
				if (PlayerPrefs.GetString(this.switchTag + "Switch") == "true")
				{
					this.switchAnimator.Play("Switch On");
					this.isOn = true;
					return;
				}
				if (PlayerPrefs.GetString(this.switchTag + "Switch") == "false")
				{
					this.switchAnimator.Play("Switch Off");
					this.isOn = false;
				}
				return;
			}
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00068098 File Offset: 0x00066298
		public void AnimateSwitch()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StopCoroutine("DisableAnimator");
				base.StartCoroutine("DisableAnimator");
			}
			this.switchAnimator.enabled = true;
			if (this.isOn)
			{
				this.isOn = false;
				this.switchAnimator.Play("Switch Off");
				this.OffEvents.Invoke();
				if (this.saveValue)
				{
					PlayerPrefs.SetString(this.switchTag + "Switch", "false");
				}
			}
			else
			{
				this.isOn = true;
				this.switchAnimator.Play("Switch On");
				this.OnEvents.Invoke();
				if (this.saveValue)
				{
					PlayerPrefs.SetString(this.switchTag + "Switch", "true");
				}
			}
			this.onValueChanged.Invoke(this.isOn);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00068178 File Offset: 0x00066378
		public void SetOn()
		{
			if (this.saveValue)
			{
				PlayerPrefs.SetString(this.switchTag + "Switch", "true");
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StopCoroutine("DisableAnimator");
				base.StartCoroutine("DisableAnimator");
			}
			this.isOn = true;
			this.switchAnimator.enabled = true;
			this.switchAnimator.Play("Switch On");
			this.OnEvents.Invoke();
			this.onValueChanged.Invoke(true);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00068208 File Offset: 0x00066408
		public void SetOff()
		{
			if (this.saveValue)
			{
				PlayerPrefs.SetString(this.switchTag + "Switch", "false");
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StopCoroutine("DisableAnimator");
				base.StartCoroutine("DisableAnimator");
			}
			this.isOn = false;
			this.switchAnimator.enabled = true;
			this.switchAnimator.Play("Switch Off");
			this.OffEvents.Invoke();
			this.onValueChanged.Invoke(false);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00068298 File Offset: 0x00066498
		public void UpdateUI()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StopCoroutine("DisableAnimator");
				base.StartCoroutine("DisableAnimator");
			}
			this.switchAnimator.enabled = true;
			if (this.isOn && this.switchAnimator.gameObject.activeInHierarchy)
			{
				this.switchAnimator.Play("On Instant");
				return;
			}
			if (!this.isOn && this.switchAnimator.gameObject.activeInHierarchy)
			{
				this.switchAnimator.Play("Off Instant");
			}
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0006832A File Offset: 0x0006652A
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableSwitchSounds && this.useHoverSound && this.switchButton.interactable)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0006835A File Offset: 0x0006655A
		private IEnumerator DisableAnimator()
		{
			yield return new WaitForSecondsRealtime(0.5f);
			this.switchAnimator.enabled = false;
			yield break;
		}

		// Token: 0x040010A0 RID: 4256
		[SerializeField]
		public SwitchManager.SwitchEvent onValueChanged = new SwitchManager.SwitchEvent();

		// Token: 0x040010A1 RID: 4257
		public UnityEvent OnEvents = new UnityEvent();

		// Token: 0x040010A2 RID: 4258
		public UnityEvent OffEvents = new UnityEvent();

		// Token: 0x040010A3 RID: 4259
		public bool saveValue = true;

		// Token: 0x040010A4 RID: 4260
		public string switchTag = "Switch";

		// Token: 0x040010A5 RID: 4261
		public bool isOn = true;

		// Token: 0x040010A6 RID: 4262
		public bool invokeAtStart = true;

		// Token: 0x040010A7 RID: 4263
		public bool enableSwitchSounds;

		// Token: 0x040010A8 RID: 4264
		public bool useHoverSound = true;

		// Token: 0x040010A9 RID: 4265
		public bool useClickSound = true;

		// Token: 0x040010AA RID: 4266
		public Animator switchAnimator;

		// Token: 0x040010AB RID: 4267
		public Button switchButton;

		// Token: 0x040010AC RID: 4268
		public AudioSource soundSource;

		// Token: 0x040010AD RID: 4269
		public AudioClip hoverSound;

		// Token: 0x040010AE RID: 4270
		public AudioClip clickSound;

		// Token: 0x040010AF RID: 4271
		private bool isInitialized;

		// Token: 0x02000240 RID: 576
		[Serializable]
		public class SwitchEvent : UnityEvent<bool>
		{
		}
	}
}
