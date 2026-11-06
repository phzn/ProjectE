using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004C2 RID: 1218
	public sealed class ValueAnimation<T> : IValueAnimationUpdate, IValueAnimation
	{
		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060025FF RID: 9727 RVA: 0x0009F4D4 File Offset: 0x0009D6D4
		// (set) Token: 0x06002600 RID: 9728 RVA: 0x0009F4EC File Offset: 0x0009D6EC
		public int durationMs
		{
			get
			{
				return this.m_DurationMs;
			}
			set
			{
				bool flag = value < 1;
				if (flag)
				{
					value = 1;
				}
				this.m_DurationMs = value;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002601 RID: 9729 RVA: 0x0009F50E File Offset: 0x0009D70E
		// (set) Token: 0x06002602 RID: 9730 RVA: 0x0009F516 File Offset: 0x0009D716
		public Func<float, float> easingCurve { get; set; }

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002603 RID: 9731 RVA: 0x0009F51F File Offset: 0x0009D71F
		// (set) Token: 0x06002604 RID: 9732 RVA: 0x0009F527 File Offset: 0x0009D727
		public bool isRunning { get; private set; }

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002605 RID: 9733 RVA: 0x0009F530 File Offset: 0x0009D730
		// (set) Token: 0x06002606 RID: 9734 RVA: 0x0009F538 File Offset: 0x0009D738
		public Action onAnimationCompleted { get; set; }

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002607 RID: 9735 RVA: 0x0009F541 File Offset: 0x0009D741
		// (set) Token: 0x06002608 RID: 9736 RVA: 0x0009F549 File Offset: 0x0009D749
		public bool autoRecycle { get; set; }

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002609 RID: 9737 RVA: 0x0009F552 File Offset: 0x0009D752
		// (set) Token: 0x0600260A RID: 9738 RVA: 0x0009F55A File Offset: 0x0009D75A
		private bool recycled { get; set; }

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x0600260B RID: 9739 RVA: 0x0009F563 File Offset: 0x0009D763
		// (set) Token: 0x0600260C RID: 9740 RVA: 0x0009F56B File Offset: 0x0009D76B
		private VisualElement owner { get; set; }

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x0600260D RID: 9741 RVA: 0x0009F574 File Offset: 0x0009D774
		// (set) Token: 0x0600260E RID: 9742 RVA: 0x0009F57C File Offset: 0x0009D77C
		public Action<VisualElement, T> valueUpdated { get; set; }

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x0600260F RID: 9743 RVA: 0x0009F585 File Offset: 0x0009D785
		// (set) Token: 0x06002610 RID: 9744 RVA: 0x0009F58D File Offset: 0x0009D78D
		public Func<VisualElement, T> initialValue { get; set; }

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002611 RID: 9745 RVA: 0x0009F596 File Offset: 0x0009D796
		// (set) Token: 0x06002612 RID: 9746 RVA: 0x0009F59E File Offset: 0x0009D79E
		public Func<T, T, float, T> interpolator { get; set; }

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x0009F5A8 File Offset: 0x0009D7A8
		// (set) Token: 0x06002614 RID: 9748 RVA: 0x0009F5F6 File Offset: 0x0009D7F6
		public T from
		{
			get
			{
				bool flag = !this.fromValueSet;
				if (flag)
				{
					bool flag2 = this.initialValue != null;
					if (flag2)
					{
						this.from = this.initialValue(this.owner);
					}
				}
				return this._from;
			}
			set
			{
				this.fromValueSet = true;
				this._from = value;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x0009F607 File Offset: 0x0009D807
		// (set) Token: 0x06002616 RID: 9750 RVA: 0x0009F60F File Offset: 0x0009D80F
		public T to { get; set; }

		// Token: 0x06002617 RID: 9751 RVA: 0x0009F618 File Offset: 0x0009D818
		public ValueAnimation()
		{
			this.SetDefaultValues();
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x0009F630 File Offset: 0x0009D830
		public void Start()
		{
			this.CheckNotRecycled();
			bool flag = this.owner != null;
			if (flag)
			{
				this.m_StartTimeMs = Panel.TimeSinceStartupMs();
				this.Register();
				this.isRunning = true;
			}
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0009F670 File Offset: 0x0009D870
		public void Stop()
		{
			this.CheckNotRecycled();
			bool isRunning = this.isRunning;
			if (isRunning)
			{
				this.Unregister();
				this.isRunning = false;
				Action onAnimationCompleted = this.onAnimationCompleted;
				if (onAnimationCompleted != null)
				{
					onAnimationCompleted();
				}
				bool autoRecycle = this.autoRecycle;
				if (autoRecycle)
				{
					bool flag = !this.recycled;
					if (flag)
					{
						this.Recycle();
					}
				}
			}
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x0009F6D4 File Offset: 0x0009D8D4
		public void Recycle()
		{
			this.CheckNotRecycled();
			bool isRunning = this.isRunning;
			if (isRunning)
			{
				bool flag = !this.autoRecycle;
				if (!flag)
				{
					this.Stop();
					return;
				}
				this.Stop();
			}
			this.SetDefaultValues();
			this.recycled = true;
			ValueAnimation<T>.sObjectPool.Release(this);
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0009F734 File Offset: 0x0009D934
		void IValueAnimationUpdate.Tick(long currentTimeMs)
		{
			this.CheckNotRecycled();
			long num = currentTimeMs - this.m_StartTimeMs;
			float num2 = (float)num / (float)this.durationMs;
			bool flag = false;
			bool flag2 = num2 >= 1f;
			if (flag2)
			{
				num2 = 1f;
				flag = true;
			}
			Func<float, float> easingCurve = this.easingCurve;
			num2 = ((easingCurve != null) ? easingCurve(num2) : num2);
			bool flag3 = this.interpolator != null;
			if (flag3)
			{
				T t = this.interpolator(this.from, this.to, num2);
				Action<VisualElement, T> valueUpdated = this.valueUpdated;
				if (valueUpdated != null)
				{
					valueUpdated(this.owner, t);
				}
			}
			bool flag4 = flag;
			if (flag4)
			{
				this.Stop();
			}
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x0009F7E4 File Offset: 0x0009D9E4
		private void SetDefaultValues()
		{
			this.m_DurationMs = 400;
			this.autoRecycle = true;
			this.owner = null;
			this.m_StartTimeMs = 0L;
			this.onAnimationCompleted = null;
			this.valueUpdated = null;
			this.initialValue = null;
			this.interpolator = null;
			this.to = default(T);
			this.from = default(T);
			this.fromValueSet = false;
			this.easingCurve = new Func<float, float>(Easing.OutQuad);
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x0009F870 File Offset: 0x0009DA70
		private void Unregister()
		{
			bool flag = this.owner != null;
			if (flag)
			{
				this.owner.UnregisterAnimation(this);
			}
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x0009F89C File Offset: 0x0009DA9C
		private void Register()
		{
			bool flag = this.owner != null;
			if (flag)
			{
				this.owner.RegisterAnimation(this);
			}
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x0009F8C8 File Offset: 0x0009DAC8
		internal void SetOwner(VisualElement e)
		{
			bool isRunning = this.isRunning;
			if (isRunning)
			{
				this.Unregister();
			}
			this.owner = e;
			bool isRunning2 = this.isRunning;
			if (isRunning2)
			{
				this.Register();
			}
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x0009F904 File Offset: 0x0009DB04
		private void CheckNotRecycled()
		{
			bool recycled = this.recycled;
			if (recycled)
			{
				throw new InvalidOperationException("Animation object has been recycled. Use KeepAlive() to keep a reference to an animation after it has been stopped.");
			}
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x0009F928 File Offset: 0x0009DB28
		public static ValueAnimation<T> Create(VisualElement e, Func<T, T, float, T> interpolator)
		{
			ValueAnimation<T> valueAnimation = ValueAnimation<T>.sObjectPool.Get();
			valueAnimation.recycled = false;
			valueAnimation.SetOwner(e);
			valueAnimation.interpolator = interpolator;
			return valueAnimation;
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x0009F960 File Offset: 0x0009DB60
		public ValueAnimation<T> Ease(Func<float, float> easing)
		{
			this.easingCurve = easing;
			return this;
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x0009F97C File Offset: 0x0009DB7C
		public ValueAnimation<T> OnCompleted(Action callback)
		{
			this.onAnimationCompleted = callback;
			return this;
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x0009F998 File Offset: 0x0009DB98
		public ValueAnimation<T> KeepAlive()
		{
			this.autoRecycle = false;
			return this;
		}

		// Token: 0x04001240 RID: 4672
		private const int k_DefaultDurationMs = 400;

		// Token: 0x04001241 RID: 4673
		private const int k_DefaultMaxPoolSize = 100;

		// Token: 0x04001242 RID: 4674
		private long m_StartTimeMs;

		// Token: 0x04001243 RID: 4675
		private int m_DurationMs;

		// Token: 0x04001249 RID: 4681
		private static ObjectPool<ValueAnimation<T>> sObjectPool = new ObjectPool<ValueAnimation<T>>(() => new ValueAnimation<T>(), 100);

		// Token: 0x0400124E RID: 4686
		private T _from;

		// Token: 0x0400124F RID: 4687
		private bool fromValueSet = false;
	}
}
