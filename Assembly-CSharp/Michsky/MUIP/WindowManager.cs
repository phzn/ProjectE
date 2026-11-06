using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.MUIP
{
	// Token: 0x02000269 RID: 617
	public class WindowManager : MonoBehaviour
	{
		// Token: 0x06001030 RID: 4144 RVA: 0x0006B82E File Offset: 0x00069A2E
		private void Awake()
		{
			if (this.windows.Count == 0)
			{
				return;
			}
			this.InitializeWindows();
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0006B844 File Offset: 0x00069A44
		private void OnEnable()
		{
			if (this.isInitialized && this.nextWindowAnimator == null)
			{
				if (this.currentWindowAnimator != null && this.currentWindowAnimator.gameObject.activeInHierarchy)
				{
					this.currentWindowAnimator.Play(this.windowFadeIn);
				}
				if (this.currentButtonAnimator != null && this.currentButtonAnimator.gameObject.activeInHierarchy)
				{
					this.currentButtonAnimator.Play(this.buttonFadeIn);
					return;
				}
			}
			else if (this.isInitialized && this.nextWindowAnimator != null)
			{
				if (this.nextWindowAnimator.gameObject.activeInHierarchy)
				{
					this.nextWindowAnimator.Play(this.windowFadeIn);
				}
				if (this.nextButtonAnimator != null && this.nextButtonAnimator.gameObject.activeInHierarchy)
				{
					this.nextButtonAnimator.Play(this.buttonFadeIn);
				}
			}
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0006B938 File Offset: 0x00069B38
		public void InitializeWindows()
		{
			if (this.windows[this.currentWindowIndex].firstSelected != null)
			{
				EventSystem.current.firstSelectedGameObject = this.windows[this.currentWindowIndex].firstSelected;
			}
			if (this.windows[this.currentWindowIndex].buttonObject != null)
			{
				this.currentButton = this.windows[this.currentWindowIndex].buttonObject;
				this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
				this.currentButtonAnimator.Play(this.buttonFadeIn);
			}
			this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
			this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
			this.currentWindowAnimator.Play(this.windowFadeIn);
			this.onWindowChange.Invoke(this.currentWindowIndex);
			if (this.altMode)
			{
				this.cachedStateLength = 0.3f;
			}
			else
			{
				this.cachedStateLength = MUIPInternalTools.GetAnimatorClipLength(this.currentWindowAnimator, MUIPInternalTools.windowManagerStateName);
			}
			this.isInitialized = true;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (i != this.currentWindowIndex && this.cullWindows)
				{
					this.windows[i].windowObject.SetActive(false);
				}
				if (this.windows[i].buttonObject != null && this.initializeButtons)
				{
					string tempName = this.windows[i].windowName;
					ButtonManager component = this.windows[i].buttonObject.GetComponent<ButtonManager>();
					if (component != null)
					{
						component.onClick.RemoveAllListeners();
						component.onClick.AddListener(delegate
						{
							this.OpenPanel(tempName);
						});
					}
				}
			}
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0006BB2C File Offset: 0x00069D2C
		public void OpenFirstTab()
		{
			if (this.currentWindowIndex != 0)
			{
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeOut);
				if (this.windows[this.currentWindowIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentWindowIndex].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeOut);
				}
				this.currentWindowIndex = 0;
				this.currentButtonIndex = 0;
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeIn);
				if (this.windows[this.currentWindowIndex].firstSelected != null)
				{
					EventSystem.current.firstSelectedGameObject = this.windows[this.currentWindowIndex].firstSelected;
				}
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentButtonIndex].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeIn);
				}
				this.onWindowChange.Invoke(this.currentWindowIndex);
				return;
			}
			if (this.currentWindowIndex == 0)
			{
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeIn);
				if (this.windows[this.currentWindowIndex].firstSelected != null)
				{
					EventSystem.current.firstSelectedGameObject = this.windows[this.currentWindowIndex].firstSelected;
				}
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentButtonIndex].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeIn);
				}
			}
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0006BDBC File Offset: 0x00069FBC
		public void OpenWindow(string newWindow)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].windowName == newWindow)
				{
					this.newWindowIndex = i;
					break;
				}
			}
			if (this.newWindowIndex != this.currentWindowIndex)
			{
				if (this.cullWindows)
				{
					base.StopCoroutine("DisablePreviousWindow");
				}
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				if (this.windows[this.currentWindowIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentWindowIndex].buttonObject;
				}
				this.currentWindowIndex = this.newWindowIndex;
				this.nextWindow = this.windows[this.currentWindowIndex].windowObject;
				this.nextWindow.SetActive(true);
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.nextWindowAnimator = this.nextWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeOut);
				this.nextWindowAnimator.Play(this.windowFadeIn);
				if (this.cullWindows)
				{
					base.StartCoroutine("DisablePreviousWindow");
				}
				this.currentButtonIndex = this.newWindowIndex;
				if (this.windows[this.currentWindowIndex].firstSelected != null)
				{
					EventSystem.current.firstSelectedGameObject = this.windows[this.currentWindowIndex].firstSelected;
				}
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.nextButton = this.windows[this.currentButtonIndex].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.nextButtonAnimator = this.nextButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeOut);
					this.nextButtonAnimator.Play(this.buttonFadeIn);
				}
				this.onWindowChange.Invoke(this.currentWindowIndex);
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0006BFE0 File Offset: 0x0006A1E0
		public void OpenPanel(string newPanel)
		{
			this.OpenWindow(newPanel);
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0006BFEC File Offset: 0x0006A1EC
		public void OpenWindowByIndex(int windowIndex)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].windowName == this.windows[windowIndex].windowName)
				{
					this.OpenWindow(this.windows[windowIndex].windowName);
					return;
				}
			}
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0006C050 File Offset: 0x0006A250
		public void NextWindow()
		{
			if (this.currentWindowIndex <= this.windows.Count - 2)
			{
				if (this.cullWindows)
				{
					base.StopCoroutine("DisablePreviousWindow");
				}
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindow.gameObject.SetActive(true);
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentButtonIndex].buttonObject;
					this.nextButton = this.windows[this.currentButtonIndex + 1].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeOut);
				}
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeOut);
				this.currentWindowIndex++;
				this.currentButtonIndex++;
				this.nextWindow = this.windows[this.currentWindowIndex].windowObject;
				this.nextWindow.gameObject.SetActive(true);
				this.nextWindowAnimator = this.nextWindow.GetComponent<Animator>();
				this.nextWindowAnimator.Play(this.windowFadeIn);
				if (this.cullWindows)
				{
					base.StartCoroutine("DisablePreviousWindow");
				}
				if (this.windows[this.currentWindowIndex].firstSelected != null)
				{
					EventSystem.current.firstSelectedGameObject = this.windows[this.currentWindowIndex].firstSelected;
				}
				if (this.nextButton != null)
				{
					this.nextButtonAnimator = this.nextButton.GetComponent<Animator>();
					this.nextButtonAnimator.Play(this.buttonFadeIn);
				}
				this.onWindowChange.Invoke(this.currentWindowIndex);
			}
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0006C250 File Offset: 0x0006A450
		public void PrevWindow()
		{
			if (this.currentWindowIndex >= 1)
			{
				if (this.cullWindows)
				{
					base.StopCoroutine("DisablePreviousWindow");
				}
				this.currentWindow = this.windows[this.currentWindowIndex].windowObject;
				this.currentWindow.gameObject.SetActive(true);
				if (this.windows[this.currentButtonIndex].buttonObject != null)
				{
					this.currentButton = this.windows[this.currentButtonIndex].buttonObject;
					this.nextButton = this.windows[this.currentButtonIndex - 1].buttonObject;
					this.currentButtonAnimator = this.currentButton.GetComponent<Animator>();
					this.currentButtonAnimator.Play(this.buttonFadeOut);
				}
				this.currentWindowAnimator = this.currentWindow.GetComponent<Animator>();
				this.currentWindowAnimator.Play(this.windowFadeOut);
				this.currentWindowIndex--;
				this.currentButtonIndex--;
				this.nextWindow = this.windows[this.currentWindowIndex].windowObject;
				this.nextWindow.gameObject.SetActive(true);
				this.nextWindowAnimator = this.nextWindow.GetComponent<Animator>();
				this.nextWindowAnimator.Play(this.windowFadeIn);
				if (this.cullWindows)
				{
					base.StartCoroutine("DisablePreviousWindow");
				}
				if (this.windows[this.currentWindowIndex].firstSelected != null)
				{
					EventSystem.current.firstSelectedGameObject = this.windows[this.currentWindowIndex].firstSelected;
				}
				if (this.nextButton != null)
				{
					this.nextButtonAnimator = this.nextButton.GetComponent<Animator>();
					this.nextButtonAnimator.Play(this.buttonFadeIn);
				}
				this.onWindowChange.Invoke(this.currentWindowIndex);
			}
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0006C443 File Offset: 0x0006A643
		public void ShowCurrentWindow()
		{
			if (this.nextWindowAnimator == null)
			{
				this.currentWindowAnimator.Play(this.windowFadeIn);
				return;
			}
			this.nextWindowAnimator.Play(this.windowFadeIn);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0006C476 File Offset: 0x0006A676
		public void HideCurrentWindow()
		{
			if (this.nextWindowAnimator == null)
			{
				this.currentWindowAnimator.Play(this.windowFadeOut);
				return;
			}
			this.nextWindowAnimator.Play(this.windowFadeOut);
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0006C4A9 File Offset: 0x0006A6A9
		public void ShowCurrentButton()
		{
			if (this.nextButtonAnimator == null)
			{
				this.currentButtonAnimator.Play(this.buttonFadeIn);
				return;
			}
			this.nextButtonAnimator.Play(this.buttonFadeIn);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0006C4DC File Offset: 0x0006A6DC
		public void HideCurrentButton()
		{
			if (this.nextButtonAnimator == null)
			{
				this.currentButtonAnimator.Play(this.buttonFadeOut);
				return;
			}
			this.nextButtonAnimator.Play(this.buttonFadeOut);
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0006C510 File Offset: 0x0006A710
		public void AddNewItem()
		{
			WindowManager.WindowItem windowItem = new WindowManager.WindowItem();
			if (this.windows.Count != 0 && this.windows[this.windows.Count - 1].windowObject != null)
			{
				int num = this.windows.Count - 1;
				GameObject gameObject = Object.Instantiate<GameObject>(this.windows[num].windowObject.transform.parent.GetChild(num).gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.windows[num].windowObject.transform.parent, false);
				gameObject.gameObject.name = "New Window " + num.ToString();
				windowItem.windowName = "New Window " + num.ToString();
				windowItem.windowObject = gameObject;
				if (this.windows[num].buttonObject != null)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.windows[num].buttonObject.transform.parent.GetChild(num).gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
					gameObject2.transform.SetParent(this.windows[num].buttonObject.transform.parent, false);
					gameObject2.gameObject.name = "New Window " + num.ToString();
					windowItem.buttonObject = gameObject2;
				}
			}
			this.windows.Add(windowItem);
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0006C6C6 File Offset: 0x0006A8C6
		private IEnumerator DisablePreviousWindow()
		{
			yield return new WaitForSecondsRealtime(this.cachedStateLength);
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (i != this.currentWindowIndex)
				{
					this.windows[i].windowObject.SetActive(false);
					Debug.Log("Disabled window " + this.windows[i].windowName);
				}
			}
			yield break;
		}

		// Token: 0x040011C0 RID: 4544
		public List<WindowManager.WindowItem> windows = new List<WindowManager.WindowItem>();

		// Token: 0x040011C1 RID: 4545
		public int currentWindowIndex;

		// Token: 0x040011C2 RID: 4546
		private int currentButtonIndex;

		// Token: 0x040011C3 RID: 4547
		private int newWindowIndex;

		// Token: 0x040011C4 RID: 4548
		public bool cullWindows = true;

		// Token: 0x040011C5 RID: 4549
		public bool initializeButtons = true;

		// Token: 0x040011C6 RID: 4550
		private bool isInitialized;

		// Token: 0x040011C7 RID: 4551
		public WindowManager.WindowChangeEvent onWindowChange;

		// Token: 0x040011C8 RID: 4552
		private GameObject currentWindow;

		// Token: 0x040011C9 RID: 4553
		private GameObject nextWindow;

		// Token: 0x040011CA RID: 4554
		private GameObject currentButton;

		// Token: 0x040011CB RID: 4555
		private GameObject nextButton;

		// Token: 0x040011CC RID: 4556
		private Animator currentWindowAnimator;

		// Token: 0x040011CD RID: 4557
		private Animator nextWindowAnimator;

		// Token: 0x040011CE RID: 4558
		private Animator currentButtonAnimator;

		// Token: 0x040011CF RID: 4559
		private Animator nextButtonAnimator;

		// Token: 0x040011D0 RID: 4560
		private string windowFadeIn = "In";

		// Token: 0x040011D1 RID: 4561
		private string windowFadeOut = "Out";

		// Token: 0x040011D2 RID: 4562
		private string buttonFadeIn = "Hover to Pressed";

		// Token: 0x040011D3 RID: 4563
		private string buttonFadeOut = "Pressed to Normal";

		// Token: 0x040011D4 RID: 4564
		private float cachedStateLength;

		// Token: 0x040011D5 RID: 4565
		public bool altMode;

		// Token: 0x0200026A RID: 618
		[Serializable]
		public class WindowChangeEvent : UnityEvent<int>
		{
		}

		// Token: 0x0200026B RID: 619
		[Serializable]
		public class WindowItem
		{
			// Token: 0x040011D6 RID: 4566
			public string windowName = "My Window";

			// Token: 0x040011D7 RID: 4567
			public GameObject windowObject;

			// Token: 0x040011D8 RID: 4568
			public GameObject buttonObject;

			// Token: 0x040011D9 RID: 4569
			public GameObject firstSelected;
		}
	}
}
