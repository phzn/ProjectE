using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace UnityEngine.UI
{
	// Token: 0x02000028 RID: 40
	public static class LayoutUtility
	{
		// Token: 0x060002CD RID: 717 RVA: 0x0000F2E1 File Offset: 0x0000D4E1
		public static float GetMinSize(RectTransform rect, int axis)
		{
			if (axis != 0)
			{
				return LayoutUtility.GetMinHeight(rect);
			}
			return LayoutUtility.GetMinWidth(rect);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000F2F3 File Offset: 0x0000D4F3
		public static float GetPreferredSize(RectTransform rect, int axis)
		{
			if (axis != 0)
			{
				return LayoutUtility.GetPreferredHeight(rect);
			}
			return LayoutUtility.GetPreferredWidth(rect);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000F305 File Offset: 0x0000D505
		public static float GetFlexibleSize(RectTransform rect, int axis)
		{
			if (axis != 0)
			{
				return LayoutUtility.GetFlexibleHeight(rect);
			}
			return LayoutUtility.GetFlexibleWidth(rect);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000F317 File Offset: 0x0000D517
		public static float GetMinWidth(RectTransform rect)
		{
			return LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.minWidth, 0f);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000F344 File Offset: 0x0000D544
		public static float GetPreferredWidth(RectTransform rect)
		{
			return Mathf.Max(LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.minWidth, 0f), LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.preferredWidth, 0f));
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F3AA File Offset: 0x0000D5AA
		public static float GetFlexibleWidth(RectTransform rect)
		{
			return LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.flexibleWidth, 0f);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F3D6 File Offset: 0x0000D5D6
		public static float GetMinHeight(RectTransform rect)
		{
			return LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.minHeight, 0f);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F404 File Offset: 0x0000D604
		public static float GetPreferredHeight(RectTransform rect)
		{
			return Mathf.Max(LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.minHeight, 0f), LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.preferredHeight, 0f));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000F46A File Offset: 0x0000D66A
		public static float GetFlexibleHeight(RectTransform rect)
		{
			return LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.flexibleHeight, 0f);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F498 File Offset: 0x0000D698
		public static float GetLayoutProperty(RectTransform rect, Func<ILayoutElement, float> property, float defaultValue)
		{
			ILayoutElement layoutElement;
			return LayoutUtility.GetLayoutProperty(rect, property, defaultValue, out layoutElement);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000F4B0 File Offset: 0x0000D6B0
		public static float GetLayoutProperty(RectTransform rect, Func<ILayoutElement, float> property, float defaultValue, out ILayoutElement source)
		{
			source = null;
			if (rect == null)
			{
				return 0f;
			}
			float num = defaultValue;
			int num2 = int.MinValue;
			List<Component> list = CollectionPool<List<Component>, Component>.Get();
			rect.GetComponents(typeof(ILayoutElement), list);
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				ILayoutElement layoutElement = list[i] as ILayoutElement;
				if (!(layoutElement is Behaviour) || ((Behaviour)layoutElement).isActiveAndEnabled)
				{
					int layoutPriority = layoutElement.layoutPriority;
					if (layoutPriority >= num2)
					{
						float num3 = property(layoutElement);
						if (num3 >= 0f)
						{
							if (layoutPriority > num2)
							{
								num = num3;
								num2 = layoutPriority;
								source = layoutElement;
							}
							else if (num3 > num)
							{
								num = num3;
								source = layoutElement;
							}
						}
					}
				}
			}
			CollectionPool<List<Component>, Component>.Release(list);
			return num;
		}
	}
}
