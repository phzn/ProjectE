using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000166 RID: 358
	[MMRequiresConstantRepaint]
	[AddComponentMenu("More Mountains/Tools/GUI/MMProgressBar")]
	public class MMProgressBar : MMMonoBehaviour
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0002774B File Offset: 0x0002594B
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x00027753 File Offset: 0x00025953
		public virtual bool Bumping { get; protected set; }

		// Token: 0x06000869 RID: 2153 RVA: 0x0002775C File Offset: 0x0002595C
		public virtual void UpdateBar01(float normalizedValue)
		{
			this.UpdateBar(Mathf.Clamp01(normalizedValue), 0f, 1f);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00027774 File Offset: 0x00025974
		public virtual void UpdateBar(float currentValue, float minValue, float maxValue)
		{
			if (!this._initialized)
			{
				this.Initialization();
			}
			if (this.StoreBarColorOnPlay)
			{
				this.StoreInitialColor();
			}
			if (!base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(true);
			}
			this._newPercent = MMMaths.Remap(currentValue, minValue, maxValue, this.MinimumBarFillValue, this.MaximumBarFillValue);
			this._actualUpdate = this.BarTarget != this._newPercent;
			if (!this._actualUpdate)
			{
				return;
			}
			if (this.CurrentState != MMProgressBar.MMProgressBarStates.Idle)
			{
				if ((this.CurrentState == MMProgressBar.MMProgressBarStates.Decreasing || this.CurrentState == MMProgressBar.MMProgressBarStates.InDecreasingDelay) && this._newPercent >= this.BarTarget)
				{
					base.StopCoroutine(this._coroutine);
					this.SetBar01(this.BarTarget);
				}
				if ((this.CurrentState == MMProgressBar.MMProgressBarStates.Increasing || this.CurrentState == MMProgressBar.MMProgressBarStates.InIncreasingDelay) && this._newPercent <= this.BarTarget)
				{
					base.StopCoroutine(this._coroutine);
					this.SetBar01(this.BarTarget);
				}
			}
			this._percentLastTimeBarWasUpdated = this.BarProgress;
			this._delayedBarDecreasingProgress = this.DelayedBarDecreasingProgress;
			this._delayedBarIncreasingProgress = this.DelayedBarIncreasingProgress;
			this.BarTarget = this._newPercent;
			if (this._newPercent != this._percentLastTimeBarWasUpdated && !this.Bumping)
			{
				this.Bump();
			}
			this.DetermineDeltaTime();
			this._lastUpdateTimestamp = this._time;
			this.DetermineDirection();
			if (this._direction < 0)
			{
				UnityEvent onBarMovementDecreasingStart = this.OnBarMovementDecreasingStart;
				if (onBarMovementDecreasingStart != null)
				{
					onBarMovementDecreasingStart.Invoke();
				}
			}
			else
			{
				UnityEvent onBarMovementIncreasingStart = this.OnBarMovementIncreasingStart;
				if (onBarMovementIncreasingStart != null)
				{
					onBarMovementIncreasingStart.Invoke();
				}
			}
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
			}
			this._coroutineShouldRun = true;
			if (base.gameObject.activeInHierarchy)
			{
				this._coroutine = base.StartCoroutine(this.UpdateBarsCo());
			}
			else
			{
				this.SetBar(currentValue, minValue, maxValue);
			}
			this.UpdateText();
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00027944 File Offset: 0x00025B44
		public virtual void SetBar(float currentValue, float minValue, float maxValue)
		{
			float num = MMMaths.Remap(currentValue, minValue, maxValue, 0f, 1f);
			this.SetBar01(num);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0002796C File Offset: 0x00025B6C
		public virtual void SetBar01(float newPercent)
		{
			if (!this._initialized)
			{
				this.Initialization();
			}
			newPercent = MMMaths.Remap(newPercent, 0f, 1f, this.MinimumBarFillValue, this.MaximumBarFillValue);
			this.BarProgress = newPercent;
			this.DelayedBarDecreasingProgress = newPercent;
			this.DelayedBarIncreasingProgress = newPercent;
			this.BarTarget = newPercent;
			this._percentLastTimeBarWasUpdated = newPercent;
			this._delayedBarDecreasingProgress = this.DelayedBarDecreasingProgress;
			this._delayedBarIncreasingProgress = this.DelayedBarIncreasingProgress;
			this.SetBarInternal(newPercent, this.ForegroundBar, this._foregroundImage, this._initialBarSize);
			this.SetBarInternal(newPercent, this.DelayedBarDecreasing, this._delayedDecreasingImage, this._initialBarSize);
			this.SetBarInternal(newPercent, this.DelayedBarIncreasing, this._delayedIncreasingImage, this._initialBarSize);
			this.UpdateText();
			this._coroutineShouldRun = false;
			this.CurrentState = MMProgressBar.MMProgressBarStates.Idle;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00027A3F File Offset: 0x00025C3F
		protected virtual void Start()
		{
			if (!this._initialized)
			{
				this.Initialization();
			}
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00027A4F File Offset: 0x00025C4F
		protected virtual void OnEnable()
		{
			if (!this._initialized)
			{
				return;
			}
			this.StoreInitialColor();
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00027A60 File Offset: 0x00025C60
		public virtual void Initialization()
		{
			this.BarTarget = -1f;
			this._isForegroundBarNotNull = this.ForegroundBar != null;
			this._isDelayedBarDecreasingNotNull = this.DelayedBarDecreasing != null;
			this._isDelayedBarIncreasingNotNull = this.DelayedBarIncreasing != null;
			this._isPercentageTextNotNull = this.PercentageText != null;
			this._isPercentageTextMeshProNotNull = this.PercentageTextMeshPro != null;
			this._initialScale = base.transform.localScale;
			if (this._isForegroundBarNotNull)
			{
				this._foregroundImage = this.ForegroundBar.GetComponent<Image>();
				this._isForegroundImageNotNull = this._foregroundImage != null;
				this._initialBarSize = this._foregroundImage.rectTransform.sizeDelta;
			}
			if (this._isDelayedBarDecreasingNotNull)
			{
				this._delayedDecreasingImage = this.DelayedBarDecreasing.GetComponent<Image>();
			}
			if (this._isDelayedBarIncreasingNotNull)
			{
				this._delayedIncreasingImage = this.DelayedBarIncreasing.GetComponent<Image>();
			}
			this._initialized = true;
			this.StoreInitialColor();
			this._percentLastTimeBarWasUpdated = this.BarProgress;
			if (this.SetInitialFillValueOnStart)
			{
				this.SetBar01(this.InitialFillValue);
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00027B83 File Offset: 0x00025D83
		protected virtual void StoreInitialColor()
		{
			if (!this.Bumping && this._isForegroundImageNotNull)
			{
				this._initialColor = this._foregroundImage.color;
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00027BA6 File Offset: 0x00025DA6
		protected virtual void DebugUpdateBar()
		{
			this.UpdateBar01(this.DebugNewTargetValue);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00027BB4 File Offset: 0x00025DB4
		protected virtual void DebugSetBar()
		{
			this.SetBar01(this.DebugNewTargetValue);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00027BC4 File Offset: 0x00025DC4
		public virtual void Plus10Percent()
		{
			float num = this.BarTarget + 0.1f;
			num = Mathf.Clamp(num, 0f, 1f);
			this.UpdateBar01(num);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00027BF8 File Offset: 0x00025DF8
		public virtual void Minus10Percent()
		{
			float num = this.BarTarget - 0.1f;
			num = Mathf.Clamp(num, 0f, 1f);
			this.UpdateBar01(num);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00027C2C File Offset: 0x00025E2C
		public virtual void Plus20Percent()
		{
			float num = this.BarTarget + 0.2f;
			num = Mathf.Clamp(num, 0f, 1f);
			this.UpdateBar01(num);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00027C60 File Offset: 0x00025E60
		public virtual void Minus20Percent()
		{
			float num = this.BarTarget - 0.2f;
			num = Mathf.Clamp(num, 0f, 1f);
			this.UpdateBar01(num);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00027C94 File Offset: 0x00025E94
		protected virtual void UpdateText()
		{
			if (this._isPercentageTextMeshProNotNull || this._isPercentageTextNotNull)
			{
				this.ComputeUpdatedText();
			}
			if (this._isPercentageTextNotNull)
			{
				this.PercentageText.text = this._updatedText;
			}
			if (this._isPercentageTextMeshProNotNull)
			{
				this.PercentageTextMeshPro.text = this._updatedText;
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00027CEC File Offset: 0x00025EEC
		protected virtual void ComputeUpdatedText()
		{
			this._updatedText = this.TextPrefix + (this.BarTarget * this.TextValueMultiplier).ToString(this.TextFormat);
			if (this.DisplayTotal)
			{
				this._updatedText = this._updatedText + this.TotalSeparator + this.TextValueMultiplier.ToString(this.TextFormat);
			}
			this._updatedText += this.TextSuffix;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00027D6C File Offset: 0x00025F6C
		protected virtual IEnumerator UpdateBarsCo()
		{
			while (this._coroutineShouldRun)
			{
				this.DetermineDeltaTime();
				this.DetermineDirection();
				this.UpdateBars();
				yield return null;
			}
			this.CurrentState = MMProgressBar.MMProgressBarStates.Idle;
			yield break;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00027D7B File Offset: 0x00025F7B
		protected virtual void DetermineDeltaTime()
		{
			this._deltaTime = ((this.TimeScale == MMProgressBar.TimeScales.Time) ? Time.deltaTime : Time.unscaledDeltaTime);
			this._time = ((this.TimeScale == MMProgressBar.TimeScales.Time) ? Time.time : Time.unscaledTime);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00027DB3 File Offset: 0x00025FB3
		protected virtual void DetermineDirection()
		{
			this._direction = ((this._newPercent > this._percentLastTimeBarWasUpdated) ? 1 : (-1));
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00027DD0 File Offset: 0x00025FD0
		protected virtual void UpdateBars()
		{
			float num = 0f;
			float num3;
			if (this._direction < 0)
			{
				float num2 = this.ComputeNewFill(this.LerpForegroundBar, this.LerpForegroundBarSpeedDecreasing, this.LerpForegroundBarDurationDecreasing, this.LerpForegroundBarCurveDecreasing, 0f, this._percentLastTimeBarWasUpdated, out num3);
				this.SetBarInternal(num2, this.ForegroundBar, this._foregroundImage, this._initialBarSize);
				this.SetBarInternal(num2, this.DelayedBarIncreasing, this._delayedIncreasingImage, this._initialBarSize);
				this.BarProgress = num2;
				this.DelayedBarIncreasingProgress = num2;
				this.CurrentState = MMProgressBar.MMProgressBarStates.Decreasing;
				if (this._time - this._lastUpdateTimestamp > this.DecreasingDelay)
				{
					float num4 = this.ComputeNewFill(this.LerpDecreasingDelayedBar, this.LerpDecreasingDelayedBarSpeed, this.LerpDecreasingDelayedBarDuration, this.LerpDecreasingDelayedBarCurve, this.DecreasingDelay, this._delayedBarDecreasingProgress, out num);
					this.SetBarInternal(num4, this.DelayedBarDecreasing, this._delayedDecreasingImage, this._initialBarSize);
					this.DelayedBarDecreasingProgress = num4;
					this.CurrentState = MMProgressBar.MMProgressBarStates.InDecreasingDelay;
				}
			}
			else
			{
				float num2 = this.ComputeNewFill(this.LerpForegroundBar, this.LerpForegroundBarSpeedIncreasing, this.LerpForegroundBarDurationIncreasing, this.LerpForegroundBarCurveIncreasing, 0f, this._delayedBarIncreasingProgress, out num3);
				this.SetBarInternal(num2, this.DelayedBarIncreasing, this._delayedIncreasingImage, this._initialBarSize);
				this.DelayedBarIncreasingProgress = num2;
				this.CurrentState = MMProgressBar.MMProgressBarStates.Increasing;
				if (this.DelayedBarIncreasing == null)
				{
					num2 = this.ComputeNewFill(this.LerpForegroundBar, this.LerpForegroundBarSpeedIncreasing, this.LerpForegroundBarDurationIncreasing, this.LerpForegroundBarCurveIncreasing, 0f, this._percentLastTimeBarWasUpdated, out num);
					this.SetBarInternal(num2, this.DelayedBarDecreasing, this._delayedDecreasingImage, this._initialBarSize);
					this.SetBarInternal(num2, this.ForegroundBar, this._foregroundImage, this._initialBarSize);
					this.BarProgress = num2;
					this.DelayedBarDecreasingProgress = num2;
					this.CurrentState = MMProgressBar.MMProgressBarStates.InDecreasingDelay;
				}
				else if (this._time - this._lastUpdateTimestamp > this.IncreasingDelay)
				{
					float num4 = this.ComputeNewFill(this.LerpIncreasingDelayedBar, this.LerpForegroundBarSpeedIncreasing, this.LerpForegroundBarDurationIncreasing, this.LerpForegroundBarCurveIncreasing, this.IncreasingDelay, this._delayedBarDecreasingProgress, out num);
					this.SetBarInternal(num4, this.DelayedBarDecreasing, this._delayedDecreasingImage, this._initialBarSize);
					this.SetBarInternal(num4, this.ForegroundBar, this._foregroundImage, this._initialBarSize);
					this.BarProgress = num4;
					this.DelayedBarDecreasingProgress = num4;
					this.CurrentState = MMProgressBar.MMProgressBarStates.InDecreasingDelay;
				}
			}
			if (num3 >= 1f && num >= 1f)
			{
				this._coroutineShouldRun = false;
				if (this._direction > 0)
				{
					UnityEvent onBarMovementIncreasingStop = this.OnBarMovementIncreasingStop;
					if (onBarMovementIncreasingStop == null)
					{
						return;
					}
					onBarMovementIncreasingStop.Invoke();
					return;
				}
				else
				{
					UnityEvent onBarMovementDecreasingStop = this.OnBarMovementDecreasingStop;
					if (onBarMovementDecreasingStop == null)
					{
						return;
					}
					onBarMovementDecreasingStop.Invoke();
				}
			}
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00028078 File Offset: 0x00026278
		protected virtual float ComputeNewFill(bool lerpBar, float barSpeed, float barDuration, AnimationCurve barCurve, float delay, float lastPercent, out float t)
		{
			t = 0f;
			float num5;
			if (lerpBar)
			{
				float num = this._time - this._lastUpdateTimestamp - delay;
				float num2 = barSpeed;
				if (num2 == 0f)
				{
					num2 = 1f;
				}
				float num3 = ((this.BarFillMode == MMProgressBar.BarFillModes.FixedDuration) ? barDuration : (Mathf.Abs(this._newPercent - lastPercent) / num2));
				float num4 = MMMaths.Remap(num, 0f, num3, 0f, 1f);
				num4 = Mathf.Clamp(num4, 0f, 1f);
				t = num4;
				if (t < 1f)
				{
					num4 = barCurve.Evaluate(num4);
					num5 = Mathf.LerpUnclamped(lastPercent, this._newPercent, num4);
				}
				else
				{
					num5 = this._newPercent;
				}
			}
			else
			{
				num5 = this._newPercent;
			}
			return Mathf.Clamp(num5, 0f, 1f);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00028150 File Offset: 0x00026350
		protected virtual void SetBarInternal(float newAmount, Transform bar, Image image, Vector2 initialSize)
		{
			if (bar == null)
			{
				return;
			}
			switch (this.FillMode)
			{
			case MMProgressBar.FillModes.LocalScale:
				this._targetLocalScale = Vector3.one;
				switch (this.BarDirection)
				{
				case MMProgressBar.BarDirections.LeftToRight:
					this._targetLocalScale.x = newAmount;
					break;
				case MMProgressBar.BarDirections.RightToLeft:
					this._targetLocalScale.x = 1f - newAmount;
					break;
				case MMProgressBar.BarDirections.UpToDown:
					this._targetLocalScale.y = 1f - newAmount;
					break;
				case MMProgressBar.BarDirections.DownToUp:
					this._targetLocalScale.y = newAmount;
					break;
				}
				bar.localScale = this._targetLocalScale;
				return;
			case MMProgressBar.FillModes.FillAmount:
				if (image == null)
				{
					return;
				}
				image.fillAmount = newAmount;
				return;
			case MMProgressBar.FillModes.Width:
			{
				if (image == null)
				{
					return;
				}
				float num = MMMaths.Remap(newAmount, 0f, 1f, 0f, initialSize.x);
				image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
				return;
			}
			case MMProgressBar.FillModes.Height:
			{
				if (image == null)
				{
					return;
				}
				float num2 = MMMaths.Remap(newAmount, 0f, 1f, 0f, initialSize.y);
				image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
				return;
			}
			case MMProgressBar.FillModes.Anchor:
				if (image == null)
				{
					return;
				}
				switch (this.BarDirection)
				{
				case MMProgressBar.BarDirections.LeftToRight:
					this._anchorVector.x = 0f;
					this._anchorVector.y = 0f;
					image.rectTransform.anchorMin = this._anchorVector;
					this._anchorVector.x = newAmount;
					this._anchorVector.y = 1f;
					image.rectTransform.anchorMax = this._anchorVector;
					return;
				case MMProgressBar.BarDirections.RightToLeft:
					this._anchorVector.x = newAmount;
					this._anchorVector.y = 0f;
					image.rectTransform.anchorMin = this._anchorVector;
					this._anchorVector.x = 1f;
					this._anchorVector.y = 1f;
					image.rectTransform.anchorMax = this._anchorVector;
					return;
				case MMProgressBar.BarDirections.UpToDown:
					this._anchorVector.x = 0f;
					this._anchorVector.y = newAmount;
					image.rectTransform.anchorMin = this._anchorVector;
					this._anchorVector.x = 1f;
					this._anchorVector.y = 1f;
					image.rectTransform.anchorMax = this._anchorVector;
					return;
				case MMProgressBar.BarDirections.DownToUp:
					this._anchorVector.x = 0f;
					this._anchorVector.y = 0f;
					image.rectTransform.anchorMin = this._anchorVector;
					this._anchorVector.x = 1f;
					this._anchorVector.y = newAmount;
					image.rectTransform.anchorMax = this._anchorVector;
					return;
				default:
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00028424 File Offset: 0x00026624
		public virtual void Bump()
		{
			float num = this._newPercent - this._percentLastTimeBarWasUpdated;
			float num2 = this.BumpIntensityMultiplier.Evaluate(num);
			bool flag = false;
			if (!this._initialized)
			{
				return;
			}
			this.DetermineDirection();
			if (this.BumpOnIncrease && this._direction > 0)
			{
				flag = true;
			}
			if (this.BumpOnDecrease && this._direction < 0)
			{
				flag = true;
			}
			if (this.BumpScaleOnChange)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.BumpCoroutine(num2));
			}
			UnityEvent onBump = this.OnBump;
			if (onBump != null)
			{
				onBump.Invoke();
			}
			UnityEvent<float> onBumpIntensity = this.OnBumpIntensity;
			if (onBumpIntensity == null)
			{
				return;
			}
			onBumpIntensity.Invoke(this.ApplyBumpIntensityMultiplier ? num2 : 1f);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x000284DD File Offset: 0x000266DD
		protected virtual IEnumerator BumpCoroutine(float intensityMultiplier)
		{
			float journey = 0f;
			this.Bumping = true;
			while (journey <= this.BumpDuration)
			{
				journey += this._deltaTime;
				float num = Mathf.Clamp01(journey / this.BumpDuration);
				float num2 = this.BumpScaleAnimationCurve.Evaluate(num);
				if (this.ApplyBumpIntensityMultiplier)
				{
					float num3 = Mathf.Abs(1f - num2) * intensityMultiplier;
					num2 = 1f + num3;
				}
				float num4 = this.BumpColorAnimationCurve.Evaluate(num);
				base.transform.localScale = num2 * this._initialScale;
				if (this.ChangeColorWhenBumping && this._isForegroundImageNotNull)
				{
					this._foregroundImage.color = Color.Lerp(this._initialColor, this.BumpColor, num4);
				}
				yield return null;
			}
			if (this.ChangeColorWhenBumping && this._isForegroundImageNotNull)
			{
				this._foregroundImage.color = this._initialColor;
			}
			this.Bumping = false;
			yield return null;
			yield break;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000284F3 File Offset: 0x000266F3
		public virtual void ShowBar()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00028501 File Offset: 0x00026701
		public virtual void HideBar(float delay)
		{
			if (delay <= 0f)
			{
				base.gameObject.SetActive(false);
				return;
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.HideBarCo(delay));
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00028533 File Offset: 0x00026733
		protected virtual IEnumerator HideBarCo(float delay)
		{
			yield return MMCoroutine.WaitFor(delay);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x040009D3 RID: 2515
		[MMInspectorGroup("Bindings", true, 10, false)]
		[Tooltip("optional - the ID of the player associated to this bar")]
		public string PlayerID;

		// Token: 0x040009D4 RID: 2516
		[Tooltip("the main, foreground bar")]
		public Transform ForegroundBar;

		// Token: 0x040009D5 RID: 2517
		[Tooltip("the delayed bar that will show when moving from a value to a new, lower value")]
		[FormerlySerializedAs("DelayedBar")]
		public Transform DelayedBarDecreasing;

		// Token: 0x040009D6 RID: 2518
		[Tooltip("the delayed bar that will show when moving from a value to a new, higher value")]
		public Transform DelayedBarIncreasing;

		// Token: 0x040009D7 RID: 2519
		[MMInspectorGroup("Fill Settings", true, 11, false)]
		[FormerlySerializedAs("StartValue")]
		[Range(0f, 1f)]
		[Tooltip("the local scale or fillamount value to reach when the value associated to the bar is at 0%")]
		public float MinimumBarFillValue;

		// Token: 0x040009D8 RID: 2520
		[FormerlySerializedAs("EndValue")]
		[Range(0f, 1f)]
		[Tooltip("the local scale or fillamount value to reach when the bar is full")]
		public float MaximumBarFillValue = 1f;

		// Token: 0x040009D9 RID: 2521
		[Tooltip("whether or not to initialize the value of the bar on start")]
		public bool SetInitialFillValueOnStart;

		// Token: 0x040009DA RID: 2522
		[MMCondition("SetInitialFillValueOnStart", true)]
		[Range(0f, 1f)]
		[Tooltip("the initial value of the bar")]
		public float InitialFillValue;

		// Token: 0x040009DB RID: 2523
		[Tooltip("the direction this bar moves to")]
		public MMProgressBar.BarDirections BarDirection;

		// Token: 0x040009DC RID: 2524
		[Tooltip("the foreground bar's fill mode")]
		public MMProgressBar.FillModes FillMode;

		// Token: 0x040009DD RID: 2525
		[Tooltip("defines whether the bar will work on scaled or unscaled time (whether or not it'll keep moving if time is slowed down for example)")]
		public MMProgressBar.TimeScales TimeScale;

		// Token: 0x040009DE RID: 2526
		[Tooltip("the selected fill animation mode")]
		public MMProgressBar.BarFillModes BarFillMode;

		// Token: 0x040009DF RID: 2527
		[MMInspectorGroup("Foreground Bar Settings", true, 12, false)]
		[Tooltip("whether or not the foreground bar should lerp")]
		public bool LerpForegroundBar = true;

		// Token: 0x040009E0 RID: 2528
		[Tooltip("the speed at which to lerp the foreground bar")]
		[MMCondition("LerpForegroundBar", true)]
		public float LerpForegroundBarSpeedDecreasing = 15f;

		// Token: 0x040009E1 RID: 2529
		[Tooltip("the speed at which to lerp the foreground bar if value is increasing")]
		[FormerlySerializedAs("LerpForegroundBarSpeed")]
		[MMCondition("LerpForegroundBar", true)]
		public float LerpForegroundBarSpeedIncreasing = 15f;

		// Token: 0x040009E2 RID: 2530
		[Tooltip("the speed at which to lerp the foreground bar if speed is decreasing")]
		[MMCondition("LerpForegroundBar", true)]
		public float LerpForegroundBarDurationDecreasing = 0.2f;

		// Token: 0x040009E3 RID: 2531
		[Tooltip("the duration each update of the foreground bar should take (only if in fixed duration bar fill mode)")]
		[MMCondition("LerpForegroundBar", true)]
		public float LerpForegroundBarDurationIncreasing = 0.2f;

		// Token: 0x040009E4 RID: 2532
		[Tooltip("the curve to use when animating the foreground bar fill decreasing")]
		[MMCondition("LerpForegroundBar", true)]
		public AnimationCurve LerpForegroundBarCurveDecreasing = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040009E5 RID: 2533
		[Tooltip("the curve to use when animating the foreground bar fill increasing")]
		[MMCondition("LerpForegroundBar", true)]
		public AnimationCurve LerpForegroundBarCurveIncreasing = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040009E6 RID: 2534
		[MMInspectorGroup("Delayed Bar Decreasing", true, 13, false)]
		[Tooltip("the delay before the delayed bar moves (in seconds)")]
		[FormerlySerializedAs("Delay")]
		public float DecreasingDelay = 1f;

		// Token: 0x040009E7 RID: 2535
		[Tooltip("whether or not the delayed bar's animation should lerp")]
		[FormerlySerializedAs("LerpDelayedBar")]
		public bool LerpDecreasingDelayedBar = true;

		// Token: 0x040009E8 RID: 2536
		[Tooltip("the speed at which to lerp the delayed bar")]
		[FormerlySerializedAs("LerpDelayedBarSpeed")]
		[MMCondition("LerpDecreasingDelayedBar", true)]
		public float LerpDecreasingDelayedBarSpeed = 15f;

		// Token: 0x040009E9 RID: 2537
		[Tooltip("the duration each update of the foreground bar should take (only if in fixed duration bar fill mode)")]
		[FormerlySerializedAs("LerpDelayedBarDuration")]
		[MMCondition("LerpDecreasingDelayedBar", true)]
		public float LerpDecreasingDelayedBarDuration = 0.2f;

		// Token: 0x040009EA RID: 2538
		[Tooltip("the curve to use when animating the delayed bar fill")]
		[FormerlySerializedAs("LerpDelayedBarCurve")]
		[MMCondition("LerpDecreasingDelayedBar", true)]
		public AnimationCurve LerpDecreasingDelayedBarCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040009EB RID: 2539
		[MMInspectorGroup("Delayed Bar Increasing", true, 18, false)]
		[Tooltip("the delay before the delayed bar moves (in seconds)")]
		public float IncreasingDelay = 1f;

		// Token: 0x040009EC RID: 2540
		[Tooltip("whether or not the delayed bar's animation should lerp")]
		public bool LerpIncreasingDelayedBar = true;

		// Token: 0x040009ED RID: 2541
		[Tooltip("the speed at which to lerp the delayed bar")]
		[MMCondition("LerpIncreasingDelayedBar", true)]
		public float LerpIncreasingDelayedBarSpeed = 15f;

		// Token: 0x040009EE RID: 2542
		[Tooltip("the duration each update of the foreground bar should take (only if in fixed duration bar fill mode)")]
		[MMCondition("LerpIncreasingDelayedBar", true)]
		public float LerpIncreasingDelayedBarDuration = 0.2f;

		// Token: 0x040009EF RID: 2543
		[Tooltip("the curve to use when animating the delayed bar fill")]
		[MMCondition("LerpIncreasingDelayedBar", true)]
		public AnimationCurve LerpIncreasingDelayedBarCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040009F0 RID: 2544
		[MMInspectorGroup("Bump", true, 14, false)]
		[Tooltip("whether or not the bar should 'bump' when changing value")]
		public bool BumpScaleOnChange = true;

		// Token: 0x040009F1 RID: 2545
		[Tooltip("whether or not the bar should bump when its value increases")]
		public bool BumpOnIncrease;

		// Token: 0x040009F2 RID: 2546
		[Tooltip("whether or not the bar should bump when its value decreases")]
		public bool BumpOnDecrease;

		// Token: 0x040009F3 RID: 2547
		[Tooltip("the duration of the bump animation")]
		public float BumpDuration = 0.2f;

		// Token: 0x040009F4 RID: 2548
		[Tooltip("whether or not the bar should flash when bumping")]
		public bool ChangeColorWhenBumping = true;

		// Token: 0x040009F5 RID: 2549
		[Tooltip("whether or not to store the initial bar color before a bump")]
		public bool StoreBarColorOnPlay = true;

		// Token: 0x040009F6 RID: 2550
		[Tooltip("the color to apply to the bar when bumping")]
		[MMCondition("ChangeColorWhenBumping", true)]
		public Color BumpColor = Color.white;

		// Token: 0x040009F7 RID: 2551
		[Tooltip("the curve to map the bump animation on")]
		[FormerlySerializedAs("BumpAnimationCurve")]
		public AnimationCurve BumpScaleAnimationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(1f, 1f),
			new Keyframe(0.3f, 1.05f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040009F8 RID: 2552
		[Tooltip("the curve to map the bump animation color animation on")]
		public AnimationCurve BumpColorAnimationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040009F9 RID: 2553
		[Tooltip("if this is true, the BumpIntensityMultiplier curve will be evaluated to apply a multiplier to the bump intensity")]
		public bool ApplyBumpIntensityMultiplier;

		// Token: 0x040009FA RID: 2554
		[Tooltip("the curve to map the bump's intensity on. x is the normalized delta of the bump (from -1:-100% to 1:100%), y is the associated multiplier")]
		[MMCondition("ApplyBumpIntensityMultiplier", true)]
		public AnimationCurve BumpIntensityMultiplier = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(-1f, 1f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040009FC RID: 2556
		[MMInspectorGroup("Events", true, 16, false)]
		[Tooltip("an event to trigger every time the bar bumps")]
		public UnityEvent OnBump;

		// Token: 0x040009FD RID: 2557
		[Tooltip("an event to trigger every time the bar bumps, with its bump intensity (based on BumpDeltaMultiplier) in parameter")]
		public UnityEvent<float> OnBumpIntensity;

		// Token: 0x040009FE RID: 2558
		[Tooltip("an event to trigger every time the bar starts decreasing")]
		public UnityEvent OnBarMovementDecreasingStart;

		// Token: 0x040009FF RID: 2559
		[Tooltip("an event to trigger every time the bar stops decreasing")]
		public UnityEvent OnBarMovementDecreasingStop;

		// Token: 0x04000A00 RID: 2560
		[Tooltip("an event to trigger every time the bar starts increasing")]
		public UnityEvent OnBarMovementIncreasingStart;

		// Token: 0x04000A01 RID: 2561
		[Tooltip("an event to trigger every time the bar stops increasing")]
		public UnityEvent OnBarMovementIncreasingStop;

		// Token: 0x04000A02 RID: 2562
		[MMInspectorGroup("Text", true, 20, false)]
		[Tooltip("a Text object to update with the bar's value")]
		public Text PercentageText;

		// Token: 0x04000A03 RID: 2563
		[Tooltip("a TMPro text object to update with the bar's value")]
		public TMP_Text PercentageTextMeshPro;

		// Token: 0x04000A04 RID: 2564
		[Tooltip("a prefix to always add to the bar's value display")]
		public string TextPrefix;

		// Token: 0x04000A05 RID: 2565
		[Tooltip("a suffix to always add to the bar's value display")]
		public string TextSuffix;

		// Token: 0x04000A06 RID: 2566
		[Tooltip("a value multiplier to always apply to the bar's value when displaying it")]
		public float TextValueMultiplier = 1f;

		// Token: 0x04000A07 RID: 2567
		[Tooltip("the format in which the text should display")]
		public string TextFormat = "{000}";

		// Token: 0x04000A08 RID: 2568
		[Tooltip("whether or not to display the total after the current value")]
		public bool DisplayTotal;

		// Token: 0x04000A09 RID: 2569
		[Tooltip("if DisplayTotal is true, the separator to put between the current value and the total")]
		[MMCondition("DisplayTotal", true)]
		public string TotalSeparator = " / ";

		// Token: 0x04000A0A RID: 2570
		[MMInspectorGroup("Debug", true, 15, false)]
		[Tooltip("the value the bar will move to if you press the DebugSet button")]
		[Range(0f, 1f)]
		public float DebugNewTargetValue;

		// Token: 0x04000A0B RID: 2571
		[MMInspectorButton("DebugUpdateBar")]
		public bool DebugUpdateBarButton;

		// Token: 0x04000A0C RID: 2572
		[MMInspectorButton("DebugSetBar")]
		public bool DebugSetBarButton;

		// Token: 0x04000A0D RID: 2573
		[MMInspectorButton("Bump")]
		public bool TestBumpButton;

		// Token: 0x04000A0E RID: 2574
		[MMInspectorButton("Plus10Percent")]
		public bool Plus10PercentButton;

		// Token: 0x04000A0F RID: 2575
		[MMInspectorButton("Minus10Percent")]
		public bool Minus10PercentButton;

		// Token: 0x04000A10 RID: 2576
		[MMInspectorGroup("Debug Read Only", true, 19, false)]
		[Tooltip("the current progress of the bar, ideally read only")]
		[Range(0f, 1f)]
		public float BarProgress;

		// Token: 0x04000A11 RID: 2577
		[Tooltip("the value towards which the bar is currently interpolating, ideally read only")]
		[Range(0f, 1f)]
		public float BarTarget;

		// Token: 0x04000A12 RID: 2578
		[Tooltip("the current progress of the delayed bar increasing")]
		[Range(0f, 1f)]
		public float DelayedBarIncreasingProgress;

		// Token: 0x04000A13 RID: 2579
		[Tooltip("the current progress of the delayed bar decreasing")]
		[Range(0f, 1f)]
		public float DelayedBarDecreasingProgress;

		// Token: 0x04000A14 RID: 2580
		protected bool _initialized;

		// Token: 0x04000A15 RID: 2581
		protected Vector2 _initialBarSize;

		// Token: 0x04000A16 RID: 2582
		protected Color _initialColor;

		// Token: 0x04000A17 RID: 2583
		protected Vector3 _initialScale;

		// Token: 0x04000A18 RID: 2584
		protected Image _foregroundImage;

		// Token: 0x04000A19 RID: 2585
		protected Image _delayedDecreasingImage;

		// Token: 0x04000A1A RID: 2586
		protected Image _delayedIncreasingImage;

		// Token: 0x04000A1B RID: 2587
		protected Vector3 _targetLocalScale = Vector3.one;

		// Token: 0x04000A1C RID: 2588
		protected float _newPercent;

		// Token: 0x04000A1D RID: 2589
		protected float _percentLastTimeBarWasUpdated;

		// Token: 0x04000A1E RID: 2590
		protected float _lastUpdateTimestamp;

		// Token: 0x04000A1F RID: 2591
		protected float _time;

		// Token: 0x04000A20 RID: 2592
		protected float _deltaTime;

		// Token: 0x04000A21 RID: 2593
		protected int _direction;

		// Token: 0x04000A22 RID: 2594
		protected Coroutine _coroutine;

		// Token: 0x04000A23 RID: 2595
		protected bool _coroutineShouldRun;

		// Token: 0x04000A24 RID: 2596
		protected bool _isDelayedBarIncreasingNotNull;

		// Token: 0x04000A25 RID: 2597
		protected bool _isDelayedBarDecreasingNotNull;

		// Token: 0x04000A26 RID: 2598
		protected bool _actualUpdate;

		// Token: 0x04000A27 RID: 2599
		protected Vector2 _anchorVector;

		// Token: 0x04000A28 RID: 2600
		protected float _delayedBarDecreasingProgress;

		// Token: 0x04000A29 RID: 2601
		protected float _delayedBarIncreasingProgress;

		// Token: 0x04000A2A RID: 2602
		protected MMProgressBar.MMProgressBarStates CurrentState;

		// Token: 0x04000A2B RID: 2603
		protected string _updatedText;

		// Token: 0x04000A2C RID: 2604
		protected string _totalText;

		// Token: 0x04000A2D RID: 2605
		protected bool _isForegroundBarNotNull;

		// Token: 0x04000A2E RID: 2606
		protected bool _isForegroundImageNotNull;

		// Token: 0x04000A2F RID: 2607
		protected bool _isPercentageTextNotNull;

		// Token: 0x04000A30 RID: 2608
		protected bool _isPercentageTextMeshProNotNull;

		// Token: 0x02000468 RID: 1128
		public enum MMProgressBarStates
		{
			// Token: 0x040020A0 RID: 8352
			Idle,
			// Token: 0x040020A1 RID: 8353
			Decreasing,
			// Token: 0x040020A2 RID: 8354
			Increasing,
			// Token: 0x040020A3 RID: 8355
			InDecreasingDelay,
			// Token: 0x040020A4 RID: 8356
			InIncreasingDelay
		}

		// Token: 0x02000469 RID: 1129
		public enum FillModes
		{
			// Token: 0x040020A6 RID: 8358
			LocalScale,
			// Token: 0x040020A7 RID: 8359
			FillAmount,
			// Token: 0x040020A8 RID: 8360
			Width,
			// Token: 0x040020A9 RID: 8361
			Height,
			// Token: 0x040020AA RID: 8362
			Anchor
		}

		// Token: 0x0200046A RID: 1130
		public enum BarDirections
		{
			// Token: 0x040020AC RID: 8364
			LeftToRight,
			// Token: 0x040020AD RID: 8365
			RightToLeft,
			// Token: 0x040020AE RID: 8366
			UpToDown,
			// Token: 0x040020AF RID: 8367
			DownToUp
		}

		// Token: 0x0200046B RID: 1131
		public enum TimeScales
		{
			// Token: 0x040020B1 RID: 8369
			UnscaledTime,
			// Token: 0x040020B2 RID: 8370
			Time
		}

		// Token: 0x0200046C RID: 1132
		public enum BarFillModes
		{
			// Token: 0x040020B4 RID: 8372
			SpeedBased,
			// Token: 0x040020B5 RID: 8373
			FixedDuration
		}
	}
}
