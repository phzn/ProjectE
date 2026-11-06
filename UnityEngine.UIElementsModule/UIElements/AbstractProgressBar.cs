using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200010F RID: 271
	public abstract class AbstractProgressBar : BindableElement, INotifyValueChanged<float>
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x00023B4F File Offset: 0x00021D4F
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x00023B5C File Offset: 0x00021D5C
		public string title
		{
			get
			{
				return this.m_Title.text;
			}
			set
			{
				this.m_Title.text = value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x00023B6B File Offset: 0x00021D6B
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x00023B73 File Offset: 0x00021D73
		public float lowValue
		{
			get
			{
				return this.m_LowValue;
			}
			set
			{
				this.m_LowValue = value;
				this.SetProgress(this.m_Value);
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x00023B8A File Offset: 0x00021D8A
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x00023B92 File Offset: 0x00021D92
		public float highValue
		{
			get
			{
				return this.m_HighValue;
			}
			set
			{
				this.m_HighValue = value;
				this.SetProgress(this.m_Value);
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00023BAC File Offset: 0x00021DAC
		public AbstractProgressBar()
		{
			base.AddToClassList(AbstractProgressBar.ussClassName);
			VisualElement visualElement = new VisualElement
			{
				name = AbstractProgressBar.ussClassName
			};
			this.m_Background = new VisualElement();
			this.m_Background.AddToClassList(AbstractProgressBar.backgroundUssClassName);
			visualElement.Add(this.m_Background);
			this.m_Progress = new VisualElement();
			this.m_Progress.AddToClassList(AbstractProgressBar.progressUssClassName);
			this.m_Background.Add(this.m_Progress);
			VisualElement visualElement2 = new VisualElement();
			visualElement2.AddToClassList(AbstractProgressBar.titleContainerUssClassName);
			this.m_Background.Add(visualElement2);
			this.m_Title = new Label();
			this.m_Title.AddToClassList(AbstractProgressBar.titleUssClassName);
			visualElement2.Add(this.m_Title);
			visualElement.AddToClassList(AbstractProgressBar.containerUssClassName);
			base.hierarchy.Add(visualElement);
			base.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnGeometryChanged), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00023CB9 File Offset: 0x00021EB9
		private void OnGeometryChanged(GeometryChangedEvent e)
		{
			this.SetProgress(this.value);
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00023CCC File Offset: 0x00021ECC
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x00023CE4 File Offset: 0x00021EE4
		public virtual float value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				bool flag = !EqualityComparer<float>.Default.Equals(this.m_Value, value);
				if (flag)
				{
					bool flag2 = base.panel != null;
					if (flag2)
					{
						using (ChangeEvent<float> pooled = ChangeEvent<float>.GetPooled(this.m_Value, value))
						{
							pooled.target = this;
							this.SetValueWithoutNotify(value);
							this.SendEvent(pooled);
						}
					}
					else
					{
						this.SetValueWithoutNotify(value);
					}
				}
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00023D6C File Offset: 0x00021F6C
		public void SetValueWithoutNotify(float newValue)
		{
			this.m_Value = newValue;
			this.SetProgress(this.value);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00023D84 File Offset: 0x00021F84
		private void SetProgress(float p)
		{
			bool flag = p < this.lowValue;
			float num;
			if (flag)
			{
				num = this.lowValue;
			}
			else
			{
				bool flag2 = p > this.highValue;
				if (flag2)
				{
					num = this.highValue;
				}
				else
				{
					num = p;
				}
			}
			num = this.CalculateProgressWidth(num);
			bool flag3 = num >= 0f;
			if (flag3)
			{
				this.m_Progress.style.right = num;
			}
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00023DF8 File Offset: 0x00021FF8
		private float CalculateProgressWidth(float width)
		{
			bool flag = this.m_Background == null || this.m_Progress == null;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				bool flag2 = float.IsNaN(this.m_Background.layout.width);
				if (flag2)
				{
					num = 0f;
				}
				else
				{
					float num2 = this.m_Background.layout.width - 2f;
					num = num2 - Mathf.Max(num2 * width / this.highValue, 1f);
				}
			}
			return num;
		}

		// Token: 0x04000423 RID: 1059
		public static readonly string ussClassName = "unity-progress-bar";

		// Token: 0x04000424 RID: 1060
		public static readonly string containerUssClassName = AbstractProgressBar.ussClassName + "__container";

		// Token: 0x04000425 RID: 1061
		public static readonly string titleUssClassName = AbstractProgressBar.ussClassName + "__title";

		// Token: 0x04000426 RID: 1062
		public static readonly string titleContainerUssClassName = AbstractProgressBar.ussClassName + "__title-container";

		// Token: 0x04000427 RID: 1063
		public static readonly string progressUssClassName = AbstractProgressBar.ussClassName + "__progress";

		// Token: 0x04000428 RID: 1064
		public static readonly string backgroundUssClassName = AbstractProgressBar.ussClassName + "__background";

		// Token: 0x04000429 RID: 1065
		private readonly VisualElement m_Background;

		// Token: 0x0400042A RID: 1066
		private readonly VisualElement m_Progress;

		// Token: 0x0400042B RID: 1067
		private readonly Label m_Title;

		// Token: 0x0400042C RID: 1068
		private float m_LowValue;

		// Token: 0x0400042D RID: 1069
		private float m_HighValue = 100f;

		// Token: 0x0400042E RID: 1070
		private float m_Value;

		// Token: 0x0400042F RID: 1071
		private const float k_MinVisibleProgress = 1f;

		// Token: 0x02000110 RID: 272
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			// Token: 0x06000947 RID: 2375 RVA: 0x00023F00 File Offset: 0x00022100
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				AbstractProgressBar abstractProgressBar = ve as AbstractProgressBar;
				abstractProgressBar.lowValue = this.m_LowValue.GetValueFromBag(bag, cc);
				abstractProgressBar.highValue = this.m_HighValue.GetValueFromBag(bag, cc);
				string valueFromBag = this.m_Title.GetValueFromBag(bag, cc);
				abstractProgressBar.title = (string.IsNullOrEmpty(valueFromBag) ? string.Empty : valueFromBag);
				abstractProgressBar.value = this.m_Value.GetValueFromBag(bag, cc);
			}

			// Token: 0x04000430 RID: 1072
			private UxmlFloatAttributeDescription m_LowValue = new UxmlFloatAttributeDescription
			{
				name = "low-value",
				defaultValue = 0f
			};

			// Token: 0x04000431 RID: 1073
			private UxmlFloatAttributeDescription m_HighValue = new UxmlFloatAttributeDescription
			{
				name = "high-value",
				defaultValue = 100f
			};

			// Token: 0x04000432 RID: 1074
			private UxmlFloatAttributeDescription m_Value = new UxmlFloatAttributeDescription
			{
				name = "value",
				defaultValue = 0f
			};

			// Token: 0x04000433 RID: 1075
			private UxmlStringAttributeDescription m_Title = new UxmlStringAttributeDescription
			{
				name = "title",
				defaultValue = string.Empty
			};
		}
	}
}
