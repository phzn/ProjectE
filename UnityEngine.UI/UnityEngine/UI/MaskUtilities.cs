using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace UnityEngine.UI
{
	// Token: 0x0200002C RID: 44
	public class MaskUtilities
	{
		// Token: 0x060002FE RID: 766 RVA: 0x0000FD2C File Offset: 0x0000DF2C
		public static void Notify2DMaskStateChanged(Component mask)
		{
			List<Component> list = CollectionPool<List<Component>, Component>.Get();
			mask.GetComponentsInChildren<Component>(list);
			for (int i = 0; i < list.Count; i++)
			{
				if (!(list[i] == null) && !(list[i].gameObject == mask.gameObject))
				{
					IClippable clippable = list[i] as IClippable;
					if (clippable != null)
					{
						clippable.RecalculateClipping();
					}
				}
			}
			CollectionPool<List<Component>, Component>.Release(list);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000FD9C File Offset: 0x0000DF9C
		public static void NotifyStencilStateChanged(Component mask)
		{
			List<Component> list = CollectionPool<List<Component>, Component>.Get();
			mask.GetComponentsInChildren<Component>(list);
			for (int i = 0; i < list.Count; i++)
			{
				if (!(list[i] == null) && !(list[i].gameObject == mask.gameObject))
				{
					IMaskable maskable = list[i] as IMaskable;
					if (maskable != null)
					{
						maskable.RecalculateMasking();
					}
				}
			}
			CollectionPool<List<Component>, Component>.Release(list);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000FE0C File Offset: 0x0000E00C
		public static Transform FindRootSortOverrideCanvas(Transform start)
		{
			List<Canvas> list = CollectionPool<List<Canvas>, Canvas>.Get();
			start.GetComponentsInParent<Canvas>(false, list);
			Canvas canvas = null;
			for (int i = 0; i < list.Count; i++)
			{
				canvas = list[i];
				if (canvas.overrideSorting)
				{
					break;
				}
			}
			CollectionPool<List<Canvas>, Canvas>.Release(list);
			if (!(canvas != null))
			{
				return null;
			}
			return canvas.transform;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000FE64 File Offset: 0x0000E064
		public static int GetStencilDepth(Transform transform, Transform stopAfter)
		{
			int num = 0;
			if (transform == stopAfter)
			{
				return num;
			}
			Transform transform2 = transform.parent;
			List<Mask> list = CollectionPool<List<Mask>, Mask>.Get();
			while (transform2 != null)
			{
				transform2.GetComponents<Mask>(list);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != null && list[i].MaskEnabled() && list[i].graphic.IsActive())
					{
						num++;
						break;
					}
				}
				if (transform2 == stopAfter)
				{
					break;
				}
				transform2 = transform2.parent;
			}
			CollectionPool<List<Mask>, Mask>.Release(list);
			return num;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000FEFC File Offset: 0x0000E0FC
		public static bool IsDescendantOrSelf(Transform father, Transform child)
		{
			if (father == null || child == null)
			{
				return false;
			}
			if (father == child)
			{
				return true;
			}
			while (child.parent != null)
			{
				if (child.parent == father)
				{
					return true;
				}
				child = child.parent;
			}
			return false;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000FF50 File Offset: 0x0000E150
		public static RectMask2D GetRectMaskForClippable(IClippable clippable)
		{
			List<RectMask2D> list = CollectionPool<List<RectMask2D>, RectMask2D>.Get();
			List<Canvas> list2 = CollectionPool<List<Canvas>, Canvas>.Get();
			RectMask2D rectMask2D = null;
			clippable.gameObject.GetComponentsInParent<RectMask2D>(false, list);
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					rectMask2D = list[i];
					if (rectMask2D.gameObject == clippable.gameObject)
					{
						rectMask2D = null;
					}
					else
					{
						if (rectMask2D.isActiveAndEnabled)
						{
							clippable.gameObject.GetComponentsInParent<Canvas>(false, list2);
							for (int j = list2.Count - 1; j >= 0; j--)
							{
								if (!MaskUtilities.IsDescendantOrSelf(list2[j].transform, rectMask2D.transform) && list2[j].overrideSorting)
								{
									rectMask2D = null;
									break;
								}
							}
							break;
						}
						rectMask2D = null;
					}
				}
			}
			CollectionPool<List<RectMask2D>, RectMask2D>.Release(list);
			CollectionPool<List<Canvas>, Canvas>.Release(list2);
			return rectMask2D;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00010028 File Offset: 0x0000E228
		public static void GetRectMasksForClip(RectMask2D clipper, List<RectMask2D> masks)
		{
			masks.Clear();
			List<Canvas> list = CollectionPool<List<Canvas>, Canvas>.Get();
			List<RectMask2D> list2 = CollectionPool<List<RectMask2D>, RectMask2D>.Get();
			clipper.transform.GetComponentsInParent<RectMask2D>(false, list2);
			if (list2.Count > 0)
			{
				clipper.transform.GetComponentsInParent<Canvas>(false, list);
				for (int i = list2.Count - 1; i >= 0; i--)
				{
					if (list2[i].IsActive())
					{
						bool flag = true;
						for (int j = list.Count - 1; j >= 0; j--)
						{
							if (!MaskUtilities.IsDescendantOrSelf(list[j].transform, list2[i].transform) && list[j].overrideSorting)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							masks.Add(list2[i]);
						}
					}
				}
			}
			CollectionPool<List<RectMask2D>, RectMask2D>.Release(list2);
			CollectionPool<List<Canvas>, Canvas>.Release(list);
		}
	}
}
