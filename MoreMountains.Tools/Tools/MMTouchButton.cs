using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x0200018D RID: 397
	[RequireComponent(typeof(Rect))]
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("More Mountains/Tools/Controls/MMTouchButton")]
	public class MMTouchButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, ISubmitHandler
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0002AE9F File Offset: 0x0002909F
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x0002AEA7 File Offset: 0x000290A7
		public virtual bool ReturnToInitialSpriteAutomatically { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x0002AEB0 File Offset: 0x000290B0
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x0002AEB8 File Offset: 0x000290B8
		public virtual MMTouchButton.ButtonStates CurrentState { get; protected set; }

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000956 RID: 2390 RVA: 0x0002AEC4 File Offset: 0x000290C4
		// (remove) Token: 0x06000957 RID: 2391 RVA: 0x0002AEFC File Offset: 0x000290FC
		public event Action<PointerEventData.FramePressState, PointerEventData> ButtonStateChange;

		// Token: 0x06000958 RID: 2392 RVA: 0x0002AF31 File Offset: 0x00029131
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002AF3C File Offset: 0x0002913C
		protected virtual void Initialization()
		{
			this.ReturnToInitialSpriteAutomatically = true;
			this._selectable = base.GetComponent<Selectable>();
			this._image = base.GetComponent<Image>();
			if (this._image != null)
			{
				this._initialColor = this._image.color;
				this._initialSprite = this._image.sprite;
			}
			this._animator = base.GetComponent<Animator>();
			if (this.Animator != null)
			{
				this._animator = this.Animator;
			}
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			if (this._canvasGroup != null)
			{
				this._initialOpacity = this.IdleOpacity;
				this._canvasGroup.alpha = this._initialOpacity;
				this._initialOpacity = this._canvasGroup.alpha;
			}
			this.ResetButton();
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002B00C File Offset: 0x0002920C
		protected virtual void Update()
		{
			switch (this.CurrentState)
			{
			case MMTouchButton.ButtonStates.Off:
				this.SetOpacity(this.IdleOpacity);
				if (this._image != null && this.ReturnToInitialSpriteAutomatically)
				{
					this._image.color = this._initialColor;
					this._image.sprite = this._initialSprite;
				}
				if (this._selectable != null)
				{
					this._selectable.interactable = true;
					if (EventSystem.current.currentSelectedGameObject == base.gameObject)
					{
						if (this._image != null && this.HighlightedChangeColor)
						{
							this._image.color = this.HighlightedColor;
						}
						if (this.HighlightedSprite != null)
						{
							this._image.sprite = this.HighlightedSprite;
						}
					}
				}
				break;
			case MMTouchButton.ButtonStates.ButtonPressed:
				this.SetOpacity(this.PressedOpacity);
				this.OnPointerPressed();
				if (this._image != null)
				{
					if (this.PressedSprite != null)
					{
						this._image.sprite = this.PressedSprite;
					}
					if (this.PressedChangeColor)
					{
						this._image.color = this.PressedColor;
					}
				}
				break;
			case MMTouchButton.ButtonStates.Disabled:
				this.SetOpacity(this.DisabledOpacity);
				if (this._image != null)
				{
					if (this.DisabledSprite != null)
					{
						this._image.sprite = this.DisabledSprite;
					}
					if (this.DisabledChangeColor)
					{
						this._image.color = this.DisabledColor;
					}
				}
				if (this._selectable != null)
				{
					this._selectable.interactable = false;
				}
				break;
			}
			this.UpdateAnimatorStates();
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002B1D4 File Offset: 0x000293D4
		protected virtual void LateUpdate()
		{
			if (this.CurrentState == MMTouchButton.ButtonStates.ButtonUp)
			{
				this.CurrentState = MMTouchButton.ButtonStates.Off;
			}
			if (this.CurrentState == MMTouchButton.ButtonStates.ButtonDown)
			{
				this.CurrentState = MMTouchButton.ButtonStates.ButtonPressed;
			}
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002B1F6 File Offset: 0x000293F6
		public virtual void InvokeButtonStateChange(PointerEventData.FramePressState newState, PointerEventData data)
		{
			Action<PointerEventData.FramePressState, PointerEventData> buttonStateChange = this.ButtonStateChange;
			if (buttonStateChange == null)
			{
				return;
			}
			buttonStateChange(newState, data);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0002B20C File Offset: 0x0002940C
		public virtual void OnPointerDown(PointerEventData data)
		{
			if (!this.Interactable)
			{
				return;
			}
			if (Time.time - this._lastClickTimestamp < this.BufferDuration)
			{
				return;
			}
			if (this.CurrentState != MMTouchButton.ButtonStates.Off)
			{
				return;
			}
			this.CurrentState = MMTouchButton.ButtonStates.ButtonDown;
			this._lastClickTimestamp = Time.time;
			this.InvokeButtonStateChange(PointerEventData.FramePressState.Pressed, data);
			if (Time.timeScale != 0f && this.PressedFirstTimeDelay > 0f)
			{
				base.Invoke("InvokePressedFirstTime", this.PressedFirstTimeDelay);
				return;
			}
			this.ButtonPressedFirstTime.Invoke();
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0002B290 File Offset: 0x00029490
		protected virtual void InvokePressedFirstTime()
		{
			if (this.ButtonPressedFirstTime != null)
			{
				this.ButtonPressedFirstTime.Invoke();
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0002B2A8 File Offset: 0x000294A8
		public virtual void OnPointerUp(PointerEventData data)
		{
			if (!this.Interactable)
			{
				return;
			}
			if (this.CurrentState != MMTouchButton.ButtonStates.ButtonPressed && this.CurrentState != MMTouchButton.ButtonStates.ButtonDown)
			{
				return;
			}
			this.CurrentState = MMTouchButton.ButtonStates.ButtonUp;
			this.InvokeButtonStateChange(PointerEventData.FramePressState.Released, data);
			if (Time.timeScale != 0f && this.ReleasedDelay > 0f)
			{
				base.Invoke("InvokeReleased", this.ReleasedDelay);
				return;
			}
			this.ButtonReleased.Invoke();
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0002B316 File Offset: 0x00029516
		protected virtual void InvokeReleased()
		{
			if (this.ButtonReleased != null)
			{
				this.ButtonReleased.Invoke();
			}
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0002B32B File Offset: 0x0002952B
		public virtual void OnPointerPressed()
		{
			if (!this.Interactable)
			{
				return;
			}
			this.CurrentState = MMTouchButton.ButtonStates.ButtonPressed;
			if (this.ButtonPressed != null)
			{
				this.ButtonPressed.Invoke();
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0002B350 File Offset: 0x00029550
		protected virtual void ResetButton()
		{
			this.SetOpacity(this._initialOpacity);
			this.CurrentState = MMTouchButton.ButtonStates.Off;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0002B365 File Offset: 0x00029565
		public virtual void OnPointerEnter(PointerEventData data)
		{
			if (!this.Interactable)
			{
				return;
			}
			if (!this.MouseMode)
			{
				this.OnPointerDown(data);
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0002B37F File Offset: 0x0002957F
		public virtual void OnPointerExit(PointerEventData data)
		{
			if (!this.Interactable)
			{
				return;
			}
			if (!this.MouseMode)
			{
				this.OnPointerUp(data);
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002B399 File Offset: 0x00029599
		protected virtual void OnEnable()
		{
			this.ResetButton();
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0002B3A4 File Offset: 0x000295A4
		private void OnDisable()
		{
			bool flag = this.CurrentState != MMTouchButton.ButtonStates.Off && this.CurrentState != MMTouchButton.ButtonStates.Disabled && this.CurrentState != MMTouchButton.ButtonStates.ButtonUp;
			this.DisableButton();
			this.CurrentState = MMTouchButton.ButtonStates.Off;
			if (flag)
			{
				this.InvokeButtonStateChange(PointerEventData.FramePressState.Released, null);
				UnityEvent buttonReleased = this.ButtonReleased;
				if (buttonReleased == null)
				{
					return;
				}
				buttonReleased.Invoke();
			}
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0002B3F8 File Offset: 0x000295F8
		public virtual void DisableButton()
		{
			this.CurrentState = MMTouchButton.ButtonStates.Disabled;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0002B401 File Offset: 0x00029601
		public virtual void EnableButton()
		{
			if (this.CurrentState == MMTouchButton.ButtonStates.Disabled)
			{
				this.CurrentState = MMTouchButton.ButtonStates.Off;
			}
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0002B413 File Offset: 0x00029613
		protected virtual void SetOpacity(float newOpacity)
		{
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = newOpacity;
			}
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0002B430 File Offset: 0x00029630
		protected virtual void UpdateAnimatorStates()
		{
			if (this._animator == null)
			{
				return;
			}
			if (this.DisabledAnimationParameterName != null)
			{
				this._animator.SetBool(this.DisabledAnimationParameterName, this.CurrentState == MMTouchButton.ButtonStates.Disabled);
			}
			if (this.PressedAnimationParameterName != null)
			{
				this._animator.SetBool(this.PressedAnimationParameterName, this.CurrentState == MMTouchButton.ButtonStates.ButtonPressed);
			}
			if (this.IdleAnimationParameterName != null)
			{
				this._animator.SetBool(this.IdleAnimationParameterName, this.CurrentState == MMTouchButton.ButtonStates.Off);
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0002B4B2 File Offset: 0x000296B2
		public virtual void OnSubmit(BaseEventData eventData)
		{
			if (this.ButtonPressedFirstTime != null)
			{
				this.ButtonPressedFirstTime.Invoke();
			}
			if (this.ButtonReleased != null)
			{
				this.ButtonReleased.Invoke();
			}
		}

		// Token: 0x04000AE5 RID: 2789
		[Header("Interaction")]
		public bool Interactable = true;

		// Token: 0x04000AE6 RID: 2790
		[Header("Binding")]
		[Tooltip("The method(s) to call when the button gets pressed down")]
		public UnityEvent ButtonPressedFirstTime;

		// Token: 0x04000AE7 RID: 2791
		[Tooltip("The method(s) to call when the button gets released")]
		public UnityEvent ButtonReleased;

		// Token: 0x04000AE8 RID: 2792
		[Tooltip("The method(s) to call while the button is being pressed")]
		public UnityEvent ButtonPressed;

		// Token: 0x04000AE9 RID: 2793
		[Header("Sprite Swap")]
		[MMInformation("Here you can define, for disabled and pressed states, if you want a different sprite, and a different color.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the sprite to use on the button when it's in the disabled state")]
		public Sprite DisabledSprite;

		// Token: 0x04000AEA RID: 2794
		[Tooltip("whether or not to change color when the button is disabled")]
		public bool DisabledChangeColor;

		// Token: 0x04000AEB RID: 2795
		[Tooltip("the color to use when the button is disabled")]
		[MMCondition("DisabledChangeColor", true)]
		public Color DisabledColor = Color.white;

		// Token: 0x04000AEC RID: 2796
		[Tooltip("the sprite to use on the button when it's in the pressed state")]
		public Sprite PressedSprite;

		// Token: 0x04000AED RID: 2797
		[Tooltip("whether or not to change the button color on press")]
		public bool PressedChangeColor;

		// Token: 0x04000AEE RID: 2798
		[Tooltip("the color to use when the button is pressed")]
		[MMCondition("PressedChangeColor", true)]
		public Color PressedColor = Color.white;

		// Token: 0x04000AEF RID: 2799
		[Tooltip("the sprite to use on the button when it's in the highlighted state")]
		public Sprite HighlightedSprite;

		// Token: 0x04000AF0 RID: 2800
		[Tooltip("whether or not to change color when highlighting the button")]
		public bool HighlightedChangeColor;

		// Token: 0x04000AF1 RID: 2801
		[Tooltip("the color to use when the button is highlighted")]
		[MMCondition("HighlightedChangeColor", true)]
		public Color HighlightedColor = Color.white;

		// Token: 0x04000AF2 RID: 2802
		[Header("Opacity")]
		[MMInformation("Here you can set different opacities for the button when it's pressed, idle, or disabled. Useful for visual feedback.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the opacity to apply to the canvas group when the button is pressed")]
		public float PressedOpacity = 1f;

		// Token: 0x04000AF3 RID: 2803
		[Tooltip("the new opacity to apply to the canvas group when the button is idle")]
		public float IdleOpacity = 1f;

		// Token: 0x04000AF4 RID: 2804
		[Tooltip("the new opacity to apply to the canvas group when the button is disabled")]
		public float DisabledOpacity = 1f;

		// Token: 0x04000AF5 RID: 2805
		[Header("Delays")]
		[MMInformation("Specify here the delays to apply when the button is pressed initially, and when it gets released. Usually you'll keep them at 0.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("the delay to apply to events when the button gets pressed for the first time")]
		public float PressedFirstTimeDelay;

		// Token: 0x04000AF6 RID: 2806
		[Tooltip("the delay to apply to events when the button gets released")]
		public float ReleasedDelay;

		// Token: 0x04000AF7 RID: 2807
		[Header("Buffer")]
		[Tooltip("the duration (in seconds) after a press during which the button can't be pressed again")]
		public float BufferDuration;

		// Token: 0x04000AF8 RID: 2808
		[Header("Animation")]
		[MMInformation("Here you can bind an animator, and specify animation parameter names for the various states.", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("an animator you can bind to this button to have its states updated to reflect the button's states")]
		public Animator Animator;

		// Token: 0x04000AF9 RID: 2809
		[Tooltip("the name of the animation parameter to turn true when the button is idle")]
		public string IdleAnimationParameterName = "Idle";

		// Token: 0x04000AFA RID: 2810
		[Tooltip("the name of the animation parameter to turn true when the button is disabled")]
		public string DisabledAnimationParameterName = "Disabled";

		// Token: 0x04000AFB RID: 2811
		[Tooltip("the name of the animation parameter to turn true when the button is pressed")]
		public string PressedAnimationParameterName = "Pressed";

		// Token: 0x04000AFC RID: 2812
		[Header("Mouse Mode")]
		[MMInformation("If you set this to true, you'll need to actually press the button for it to be triggered, otherwise a simple hover will trigger it (better to leave it unchecked if you're going for touch input).", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("If you set this to true, you'll need to actually press the button for it to be triggered, otherwise a simple hover will trigger it (better for touch input).")]
		public bool MouseMode;

		// Token: 0x04000B00 RID: 2816
		protected bool _zonePressed;

		// Token: 0x04000B01 RID: 2817
		protected CanvasGroup _canvasGroup;

		// Token: 0x04000B02 RID: 2818
		protected float _initialOpacity;

		// Token: 0x04000B03 RID: 2819
		protected Animator _animator;

		// Token: 0x04000B04 RID: 2820
		protected Image _image;

		// Token: 0x04000B05 RID: 2821
		protected Sprite _initialSprite;

		// Token: 0x04000B06 RID: 2822
		protected Color _initialColor;

		// Token: 0x04000B07 RID: 2823
		protected float _lastClickTimestamp;

		// Token: 0x04000B08 RID: 2824
		protected Selectable _selectable;

		// Token: 0x02000476 RID: 1142
		public enum ButtonStates
		{
			// Token: 0x040020D9 RID: 8409
			Off,
			// Token: 0x040020DA RID: 8410
			ButtonDown,
			// Token: 0x040020DB RID: 8411
			ButtonPressed,
			// Token: 0x040020DC RID: 8412
			ButtonUp,
			// Token: 0x040020DD RID: 8413
			Disabled
		}
	}
}
