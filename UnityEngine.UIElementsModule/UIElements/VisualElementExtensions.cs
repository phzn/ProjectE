using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003F7 RID: 1015
	public static class VisualElementExtensions
	{
		// Token: 0x060020D0 RID: 8400 RVA: 0x0007C2A8 File Offset: 0x0007A4A8
		public static void StretchToParentSize(this VisualElement elem)
		{
			bool flag = elem == null;
			if (flag)
			{
				throw new ArgumentNullException("elem");
			}
			IStyle style = elem.style;
			style.position = Position.Absolute;
			style.left = 0f;
			style.top = 0f;
			style.right = 0f;
			style.bottom = 0f;
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x0007C324 File Offset: 0x0007A524
		public static void StretchToParentWidth(this VisualElement elem)
		{
			bool flag = elem == null;
			if (flag)
			{
				throw new ArgumentNullException("elem");
			}
			IStyle style = elem.style;
			style.position = Position.Absolute;
			style.left = 0f;
			style.right = 0f;
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x0007C37C File Offset: 0x0007A57C
		public static void AddManipulator(this VisualElement ele, IManipulator manipulator)
		{
			bool flag = manipulator != null;
			if (flag)
			{
				manipulator.target = ele;
			}
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x0007C39C File Offset: 0x0007A59C
		public static void RemoveManipulator(this VisualElement ele, IManipulator manipulator)
		{
			bool flag = manipulator != null;
			if (flag)
			{
				manipulator.target = null;
			}
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x0007C3BC File Offset: 0x0007A5BC
		public static Vector2 WorldToLocal(this VisualElement ele, Vector2 p)
		{
			bool flag = ele == null;
			if (flag)
			{
				throw new ArgumentNullException("ele");
			}
			return VisualElement.MultiplyMatrix44Point2(ele.worldTransformInverse, p);
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x0007C3F0 File Offset: 0x0007A5F0
		public static Vector2 LocalToWorld(this VisualElement ele, Vector2 p)
		{
			bool flag = ele == null;
			if (flag)
			{
				throw new ArgumentNullException("ele");
			}
			return VisualElement.MultiplyMatrix44Point2(ele.worldTransformRef, p);
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x0007C424 File Offset: 0x0007A624
		public static Rect WorldToLocal(this VisualElement ele, Rect r)
		{
			bool flag = ele == null;
			if (flag)
			{
				throw new ArgumentNullException("ele");
			}
			return VisualElement.CalculateConservativeRect(ele.worldTransformInverse, r);
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x0007C458 File Offset: 0x0007A658
		public static Rect LocalToWorld(this VisualElement ele, Rect r)
		{
			bool flag = ele == null;
			if (flag)
			{
				throw new ArgumentNullException("ele");
			}
			return VisualElement.CalculateConservativeRect(ele.worldTransformRef, r);
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x0007C48C File Offset: 0x0007A68C
		public static Vector2 ChangeCoordinatesTo(this VisualElement src, VisualElement dest, Vector2 point)
		{
			return dest.WorldToLocal(src.LocalToWorld(point));
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x0007C4AC File Offset: 0x0007A6AC
		public static Rect ChangeCoordinatesTo(this VisualElement src, VisualElement dest, Rect rect)
		{
			return dest.WorldToLocal(src.LocalToWorld(rect));
		}
	}
}
