using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000043 RID: 67
	public class MMDebugMenuDebugTab : MonoBehaviour
	{
		// Token: 0x06000152 RID: 338 RVA: 0x000071FC File Offset: 0x000053FC
		protected virtual void Awake()
		{
			MMDebug.MMDebugLogEvent.Register(new MMDebug.MMDebugLogEvent.Delegate(this.OnMMDebugLogEvent));
			this.DebugText.text = "";
			this._rectTransform = base.gameObject.GetComponent<RectTransform>();
			this.CommandPrompt.onEndEdit.AddListener(delegate(string val)
			{
				this.CommandPrompt.text = "";
				if (val != "")
				{
					MMDebug.DebugLogCommand(val);
				}
			});
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007258 File Offset: 0x00005458
		protected virtual void Update()
		{
			this.TouchScreenVisible = TouchScreenKeyboard.visible;
			if (this.TouchScreenVisible)
			{
				this._rectTransform.MMSetBottom(650f);
				return;
			}
			this._rectTransform.MMSetBottom(0f);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000728E File Offset: 0x0000548E
		protected virtual void LateUpdate()
		{
			if (this._touchScreenVisibleLastFrame != this.TouchScreenVisible)
			{
				base.StartCoroutine(this.ScrollToLogBottomCo());
			}
			this._touchScreenVisibleLastFrame = this.TouchScreenVisible;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000072B7 File Offset: 0x000054B7
		protected virtual void OnEnable()
		{
			base.StartCoroutine(this.ScrollToLogBottomCo());
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000072C6 File Offset: 0x000054C6
		protected virtual void OnMMDebugLogEvent(MMDebug.DebugLogItem item)
		{
			this.DebugText.text = MMDebug.LogHistoryText;
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.ScrollToLogBottomCo());
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000072F2 File Offset: 0x000054F2
		protected virtual IEnumerator ScrollToLogBottomCo()
		{
			yield return new WaitForEndOfFrame();
			this.DebugScrollRect.normalizedPosition = Vector2.zero;
			this.CommandPrompt.ActivateInputField();
			this.CommandPrompt.Select();
			yield break;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007301 File Offset: 0x00005501
		public virtual void OnDestroy()
		{
			MMDebug.MMDebugLogEvent.Unregister(new MMDebug.MMDebugLogEvent.Delegate(this.OnMMDebugLogEvent));
		}

		// Token: 0x040001C2 RID: 450
		public ScrollRect DebugScrollRect;

		// Token: 0x040001C3 RID: 451
		public Text DebugText;

		// Token: 0x040001C4 RID: 452
		public InputField CommandPrompt;

		// Token: 0x040001C5 RID: 453
		public Text CommandPromptCharacter;

		// Token: 0x040001C6 RID: 454
		public bool TouchScreenVisible;

		// Token: 0x040001C7 RID: 455
		protected TouchScreenKeyboard _touchScreenKeyboard;

		// Token: 0x040001C8 RID: 456
		protected RectTransform _rectTransform;

		// Token: 0x040001C9 RID: 457
		protected float _mobileMenuOffset = -1000f;

		// Token: 0x040001CA RID: 458
		protected bool _touchScreenVisibleLastFrame;
	}
}
