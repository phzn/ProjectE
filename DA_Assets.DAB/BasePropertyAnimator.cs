using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.DAB
{
	// Token: 0x02000004 RID: 4
	public abstract class BasePropertyAnimator<T> : BasePropertyAnimatorBase
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002397 File Offset: 0x00000597
		protected Graphic _targetGraphic
		{
			get
			{
				if (this.__targetGraphic == null)
				{
					this.__targetGraphic = base.GetComponent<Graphic>();
				}
				return this.__targetGraphic;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000023B9 File Offset: 0x000005B9
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000023C1 File Offset: 0x000005C1
		public AnimatedProperty<T> Properties
		{
			get
			{
				return this._properties;
			}
			set
			{
				this._properties = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000023CA File Offset: 0x000005CA
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000023D2 File Offset: 0x000005D2
		public EventAnimations Animations
		{
			get
			{
				return this._animations;
			}
			set
			{
				this._animations = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000023DB File Offset: 0x000005DB
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000023E3 File Offset: 0x000005E3
		private DAPointerEvent _pointerEvent
		{
			get
			{
				return this.__pointerEvent;
			}
			set
			{
				if (this._debugMode)
				{
					Debug.Log(string.Format("[{0}] PointerEvent changed from {1} to {2}", base.name, this.__pointerEvent, value));
				}
				this.__pointerEvent = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000241A File Offset: 0x0000061A
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002422 File Offset: 0x00000622
		private DAState _state
		{
			get
			{
				return this.__state;
			}
			set
			{
				if (this._debugMode)
				{
					Debug.Log(string.Format("[{0}] State changed from {1} to {2}", base.name, this.__state, value));
				}
				this.__state = value;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000245C File Offset: 0x0000065C
		private void Start()
		{
			this.SetIdleValue();
			if (this._animations.Looped.Enabled)
			{
				this.StartLoopAnimation();
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000248A File Offset: 0x0000068A
		protected void StartLoopAnimation()
		{
			this.StartAnimation(this._properties.Looped, this._animations.Looped);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024A8 File Offset: 0x000006A8
		private void Update()
		{
			if (this._state == DAState.Idle || this._state == DAState.Wait)
			{
				return;
			}
			if (this._state == DAState.Forward)
			{
				this._currentTime += Time.deltaTime;
			}
			else if (this._state == DAState.Backward || this._state == DAState.Interrupt)
			{
				this._currentTime -= Time.deltaTime;
			}
			this._progress = Mathf.Clamp01(this._currentTime / this._targetAnimation.Duration);
			if (this._targetAnimation.Curve != null)
			{
				this._curveValue = this._targetAnimation.Curve.Evaluate(this._progress);
			}
			this.ApplyValue(this._curveValue, this._idleValue, this._targetValue);
			if (this._progress < 1f)
			{
				if (this._progress <= 0f)
				{
					if (this._pendingAnimation != null)
					{
						this.StartAnimation(this._pendingValue, this._pendingAnimation.Value);
						this._pendingAnimation = null;
						return;
					}
					if (this._animations.Looped.Enabled)
					{
						this.StartLoopAnimation();
						return;
					}
					this._state = DAState.Idle;
				}
				return;
			}
			if (this._pointerEvent == DAPointerEvent.OnNonInteractable)
			{
				this._state = DAState.Wait;
				return;
			}
			if (this._pointerEvent == DAPointerEvent.OnPointerEnter)
			{
				this._state = DAState.Wait;
				return;
			}
			if (this._targetAnimation.PingPong)
			{
				this._state = DAState.Backward;
				return;
			}
			if (this._animations.Looped.Enabled && this._pointerEvent != DAPointerEvent.OnNonInteractable)
			{
				this.StartLoopAnimation();
				return;
			}
			this._state = DAState.Idle;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002638 File Offset: 0x00000838
		[return: TupleElementNames(new string[] { "Value", "Animation" })]
		private ValueTuple<T, AnimationItem> GetAnimationDataByPointerEvent(DAPointerEvent pointerEvent)
		{
			switch (pointerEvent)
			{
			case DAPointerEvent.OnPointerClick:
				return new ValueTuple<T, AnimationItem>(this._properties.Pressed, this._animations.Pressed);
			case DAPointerEvent.OnPointerEnter:
			case DAPointerEvent.OnPointerExit:
				return new ValueTuple<T, AnimationItem>(this._properties.Highlighted, this._animations.Highlighted);
			case DAPointerEvent.OnInteractable:
			case DAPointerEvent.OnNonInteractable:
				return new ValueTuple<T, AnimationItem>(this._properties.Disabled, this._animations.Disabled);
			default:
				return default(ValueTuple<T, AnimationItem>);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026C0 File Offset: 0x000008C0
		public override void SendPointerEvent(DAPointerEvent pointerEvent)
		{
			if (this._pointerEvent == DAPointerEvent.OnNonInteractable && pointerEvent != DAPointerEvent.OnInteractable)
			{
				if (this._debugMode)
				{
					Debug.Log("return 1");
				}
				return;
			}
			ValueTuple<T, AnimationItem> animationDataByPointerEvent = this.GetAnimationDataByPointerEvent(pointerEvent);
			if (!animationDataByPointerEvent.Item2.Enabled)
			{
				if (this._debugMode)
				{
					Debug.Log("return 2");
				}
				return;
			}
			if (pointerEvent == DAPointerEvent.OnPointerEnter && this._pointerEvent == DAPointerEvent.OnPointerEnter && this._state != DAState.Wait)
			{
				if (this._debugMode)
				{
					Debug.Log("return 3");
				}
				return;
			}
			if (this._debugMode)
			{
				Debug.Log(string.Format("[{0}] Event {1} received. Previous: {2}", base.name, pointerEvent, this._pointerEvent));
			}
			switch (pointerEvent)
			{
			case DAPointerEvent.OnPointerClick:
			case DAPointerEvent.OnPointerEnter:
			case DAPointerEvent.OnNonInteractable:
				if (this._progress >= 1f && animationDataByPointerEvent.Item1.Equals(this._targetValue))
				{
					if (this._debugMode)
					{
						Debug.Log("break 1");
					}
				}
				else
				{
					if (this._debugMode)
					{
						Debug.Log(pointerEvent);
					}
					if (this._state == DAState.Forward || this._state == DAState.Backward || this._state == DAState.Wait)
					{
						this._state = DAState.Interrupt;
						this._targetAnimation = this.CreateTransitionItem(this._curveValue);
						this._currentTime = this._targetAnimation.Duration;
						this._pendingAnimation = new AnimationItem?(animationDataByPointerEvent.Item2);
						this._pendingValue = animationDataByPointerEvent.Item1;
					}
					else if (this._state == DAState.Idle)
					{
						this.StartAnimation(animationDataByPointerEvent.Item1, animationDataByPointerEvent.Item2);
					}
				}
				break;
			case DAPointerEvent.OnPointerExit:
				this._state = DAState.Backward;
				break;
			case DAPointerEvent.OnInteractable:
				this._state = DAState.Forward;
				break;
			}
			this._pointerEvent = pointerEvent;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002884 File Offset: 0x00000A84
		private AnimationItem CreateTransitionItem(float currentCurveValue)
		{
			return new AnimationItem
			{
				Curve = AnimationCurve.Linear(0f, 0f, 1f, currentCurveValue),
				Duration = 0.1f,
				PingPong = false
			};
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000028CA File Offset: 0x00000ACA
		private void StartAnimation(T targetValue, AnimationItem animation)
		{
			this._state = DAState.Forward;
			this._targetValue = targetValue;
			this._targetAnimation = animation;
			this._currentTime = 0f;
			this.OnStartAnimation();
		}

		// Token: 0x0600001A RID: 26
		protected abstract void ApplyValue(float curveValue, T from, T to);

		// Token: 0x04000005 RID: 5
		private Graphic __targetGraphic;

		// Token: 0x04000006 RID: 6
		[SerializeField]
		protected AnimatedProperty<T> _properties;

		// Token: 0x04000007 RID: 7
		[Space]
		[SerializeField]
		protected EventAnimations _animations;

		// Token: 0x04000008 RID: 8
		[Space]
		[SerializeField]
		[HideInInspector]
		private DAPointerEvent __pointerEvent;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		[HideInInspector]
		private DAState __state;

		// Token: 0x0400000A RID: 10
		[Space]
		[SerializeField]
		protected bool _debugMode;

		// Token: 0x0400000B RID: 11
		protected T _idleValue;

		// Token: 0x0400000C RID: 12
		protected T _targetValue;

		// Token: 0x0400000D RID: 13
		protected T _pendingValue;

		// Token: 0x0400000E RID: 14
		protected AnimationItem _targetAnimation;

		// Token: 0x0400000F RID: 15
		protected AnimationItem? _pendingAnimation;

		// Token: 0x04000010 RID: 16
		protected float _progress;

		// Token: 0x04000011 RID: 17
		protected float _curveValue;

		// Token: 0x04000012 RID: 18
		protected float _currentTime;
	}
}
