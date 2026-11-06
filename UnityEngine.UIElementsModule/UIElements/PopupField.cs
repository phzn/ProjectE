using System;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x02000109 RID: 265
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class PopupField<T> : BasePopupField<T, T>
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00023528 File Offset: 0x00021728
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x00023540 File Offset: 0x00021740
		public virtual Func<T, string> formatSelectedValueCallback
		{
			get
			{
				return this.m_FormatSelectedValueCallback;
			}
			set
			{
				this.m_FormatSelectedValueCallback = value;
				base.textElement.text = this.GetValueToDisplay();
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x0002355C File Offset: 0x0002175C
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x00023574 File Offset: 0x00021774
		public virtual Func<T, string> formatListItemCallback
		{
			get
			{
				return this.m_FormatListItemCallback;
			}
			set
			{
				this.m_FormatListItemCallback = value;
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00023580 File Offset: 0x00021780
		internal override string GetValueToDisplay()
		{
			bool flag = this.m_FormatSelectedValueCallback != null;
			string text;
			if (flag)
			{
				text = this.m_FormatSelectedValueCallback(this.value);
			}
			else
			{
				bool flag2 = this.value != null;
				if (flag2)
				{
					T value = this.value;
					text = UIElementsUtility.ParseMenuName(value.ToString());
				}
				else
				{
					text = string.Empty;
				}
			}
			return text;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x000235E8 File Offset: 0x000217E8
		internal override string GetListItemToDisplay(T value)
		{
			bool flag = this.m_FormatListItemCallback != null;
			string text;
			if (flag)
			{
				text = this.m_FormatListItemCallback(value);
			}
			else
			{
				text = ((value != null && this.m_Choices.Contains(value)) ? value.ToString() : string.Empty);
			}
			return text;
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00023640 File Offset: 0x00021840
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x00023658 File Offset: 0x00021858
		public override T value
		{
			get
			{
				return base.value;
			}
			set
			{
				List<T> choices = this.m_Choices;
				this.m_Index = ((choices != null) ? choices.IndexOf(value) : (-1));
				base.value = value;
			}
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0002367C File Offset: 0x0002187C
		public override void SetValueWithoutNotify(T newValue)
		{
			List<T> choices = this.m_Choices;
			this.m_Index = ((choices != null) ? choices.IndexOf(newValue) : (-1));
			base.SetValueWithoutNotify(newValue);
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x000236A0 File Offset: 0x000218A0
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x000236B8 File Offset: 0x000218B8
		public int index
		{
			get
			{
				return this.m_Index;
			}
			set
			{
				bool flag = value != this.m_Index;
				if (flag)
				{
					this.m_Index = value;
					bool flag2 = this.m_Index >= 0 && this.m_Index < this.m_Choices.Count;
					if (flag2)
					{
						this.value = this.m_Choices[this.m_Index];
					}
					else
					{
						this.value = default(T);
					}
				}
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0002372C File Offset: 0x0002192C
		public PopupField()
			: this(null)
		{
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00023737 File Offset: 0x00021937
		public PopupField(string label = null)
			: base(label)
		{
			base.AddToClassList(PopupField<T>.ussClassName);
			base.labelElement.AddToClassList(PopupField<T>.labelUssClassName);
			base.visualInput.AddToClassList(PopupField<T>.inputUssClassName);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00023777 File Offset: 0x00021977
		public PopupField(List<T> choices, T defaultValue, Func<T, string> formatSelectedValueCallback = null, Func<T, string> formatListItemCallback = null)
			: this(null, choices, defaultValue, formatSelectedValueCallback, formatListItemCallback)
		{
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00023788 File Offset: 0x00021988
		public PopupField(string label, List<T> choices, T defaultValue, Func<T, string> formatSelectedValueCallback = null, Func<T, string> formatListItemCallback = null)
			: this(label)
		{
			bool flag = defaultValue == null;
			if (flag)
			{
				throw new ArgumentNullException("defaultValue");
			}
			this.choices = choices;
			bool flag2 = !this.m_Choices.Contains(defaultValue);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Default value {0} is not present in the list of possible values", defaultValue));
			}
			this.SetValueWithoutNotify(defaultValue);
			this.formatListItemCallback = formatListItemCallback;
			this.formatSelectedValueCallback = formatSelectedValueCallback;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00023801 File Offset: 0x00021A01
		public PopupField(List<T> choices, int defaultIndex, Func<T, string> formatSelectedValueCallback = null, Func<T, string> formatListItemCallback = null)
			: this(null, choices, defaultIndex, formatSelectedValueCallback, formatListItemCallback)
		{
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00023811 File Offset: 0x00021A11
		public PopupField(string label, List<T> choices, int defaultIndex, Func<T, string> formatSelectedValueCallback = null, Func<T, string> formatListItemCallback = null)
			: this(label)
		{
			this.choices = choices;
			this.SetIndexWithoutNotify(defaultIndex);
			this.formatListItemCallback = formatListItemCallback;
			this.formatSelectedValueCallback = formatSelectedValueCallback;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00023840 File Offset: 0x00021A40
		internal override void AddMenuItems(IGenericMenu menu)
		{
			bool flag = menu == null;
			if (flag)
			{
				throw new ArgumentNullException("menu");
			}
			bool flag2 = this.m_Choices == null;
			if (!flag2)
			{
				using (List<T>.Enumerator enumerator = this.m_Choices.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						T item = enumerator.Current;
						bool flag3 = EqualityComparer<T>.Default.Equals(item, this.value) && !base.showMixedValue;
						menu.AddItem(this.GetListItemToDisplay(item), flag3, delegate
						{
							this.ChangeValueFromMenu(item);
						});
					}
				}
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00023910 File Offset: 0x00021B10
		internal void SetIndexWithoutNotify(int index)
		{
			this.m_Index = index;
			bool flag = this.m_Index >= 0 && this.m_Index < this.m_Choices.Count;
			if (flag)
			{
				this.SetValueWithoutNotify(this.m_Choices[this.m_Index]);
			}
			else
			{
				this.SetValueWithoutNotify(default(T));
			}
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00023972 File Offset: 0x00021B72
		private void ChangeValueFromMenu(T menuItem)
		{
			this.value = menuItem;
		}

		// Token: 0x04000415 RID: 1045
		internal const int kPopupFieldDefaultIndex = -1;

		// Token: 0x04000416 RID: 1046
		private int m_Index = -1;

		// Token: 0x04000417 RID: 1047
		public new static readonly string ussClassName = "unity-popup-field";

		// Token: 0x04000418 RID: 1048
		public new static readonly string labelUssClassName = PopupField<T>.ussClassName + "__label";

		// Token: 0x04000419 RID: 1049
		public new static readonly string inputUssClassName = PopupField<T>.ussClassName + "__input";
	}
}
