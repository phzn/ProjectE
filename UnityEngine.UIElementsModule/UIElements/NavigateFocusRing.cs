using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200024B RID: 587
	internal class NavigateFocusRing : IFocusRing
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x0003CAA6 File Offset: 0x0003ACA6
		private FocusController focusController
		{
			get
			{
				return this.m_Root.focusController;
			}
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0003CAB3 File Offset: 0x0003ACB3
		public NavigateFocusRing(VisualElement root)
		{
			this.m_Root = root;
			this.m_Ring = new VisualElementFocusRing(root, VisualElementFocusRing.DefaultFocusOrder.ChildOrder);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0003CAD4 File Offset: 0x0003ACD4
		public FocusChangeDirection GetFocusChangeDirection(Focusable currentFocusable, EventBase e)
		{
			bool flag = e.eventTypeId == EventBase<PointerDownEvent>.TypeId();
			if (flag)
			{
				Focusable focusable;
				bool focusableParentForPointerEvent = this.focusController.GetFocusableParentForPointerEvent(e.target as Focusable, out focusable);
				if (focusableParentForPointerEvent)
				{
					return VisualElementFocusChangeTarget.GetPooled(focusable);
				}
			}
			bool flag2 = e.eventTypeId == EventBase<NavigationMoveEvent>.TypeId();
			if (flag2)
			{
				switch (((NavigationMoveEvent)e).direction)
				{
				case NavigationMoveEvent.Direction.Left:
					return NavigateFocusRing.Left;
				case NavigationMoveEvent.Direction.Up:
					return NavigateFocusRing.Up;
				case NavigationMoveEvent.Direction.Right:
					return NavigateFocusRing.Right;
				case NavigationMoveEvent.Direction.Down:
					return NavigateFocusRing.Down;
				case NavigationMoveEvent.Direction.Next:
					return NavigateFocusRing.Next;
				case NavigationMoveEvent.Direction.Previous:
					return NavigateFocusRing.Previous;
				}
			}
			return FocusChangeDirection.none;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0003CBA0 File Offset: 0x0003ADA0
		public virtual Focusable GetNextFocusable(Focusable currentFocusable, FocusChangeDirection direction)
		{
			bool flag = direction == NavigateFocusRing.Up || direction == NavigateFocusRing.Down || direction == NavigateFocusRing.Right || direction == NavigateFocusRing.Left;
			Focusable focusable;
			if (flag)
			{
				focusable = this.GetNextFocusable2D(currentFocusable, (NavigateFocusRing.ChangeDirection)direction);
			}
			else
			{
				focusable = this.m_Ring.GetNextFocusable(currentFocusable, direction);
			}
			return focusable;
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0003CBF8 File Offset: 0x0003ADF8
		private Focusable GetNextFocusable2D(Focusable currentFocusable, NavigateFocusRing.ChangeDirection direction)
		{
			VisualElement visualElement = currentFocusable as VisualElement;
			bool flag = visualElement == null;
			if (flag)
			{
				visualElement = this.m_Root;
			}
			Rect worldBoundingBox = this.m_Root.worldBoundingBox;
			Rect rect = new Rect(worldBoundingBox.position - Vector2.one, worldBoundingBox.size + Vector2.one * 2f);
			Rect worldBound = visualElement.worldBound;
			Rect rect2 = new Rect(worldBound.position - Vector2.one, worldBound.size + Vector2.one * 2f);
			bool flag2 = direction == NavigateFocusRing.Up;
			if (flag2)
			{
				rect2.yMin = rect.yMin;
			}
			else
			{
				bool flag3 = direction == NavigateFocusRing.Down;
				if (flag3)
				{
					rect2.yMax = rect.yMax;
				}
				else
				{
					bool flag4 = direction == NavigateFocusRing.Left;
					if (flag4)
					{
						rect2.xMin = rect.xMin;
					}
					else
					{
						bool flag5 = direction == NavigateFocusRing.Right;
						if (flag5)
						{
							rect2.xMax = rect.xMax;
						}
					}
				}
			}
			NavigateFocusRing.FocusableHierarchyTraversal focusableHierarchyTraversal = default(NavigateFocusRing.FocusableHierarchyTraversal);
			focusableHierarchyTraversal.currentFocusable = visualElement;
			focusableHierarchyTraversal.direction = direction;
			focusableHierarchyTraversal.validRect = rect2;
			focusableHierarchyTraversal.firstPass = true;
			Focusable focusable = focusableHierarchyTraversal.GetBestOverall(this.m_Root, null);
			bool flag6 = focusable != null;
			Focusable focusable2;
			if (flag6)
			{
				focusable2 = focusable;
			}
			else
			{
				rect2 = new Rect(worldBound.position - Vector2.one, worldBound.size + Vector2.one * 2f);
				bool flag7 = direction == NavigateFocusRing.Down;
				if (flag7)
				{
					rect2.yMin = rect.yMin;
				}
				else
				{
					bool flag8 = direction == NavigateFocusRing.Up;
					if (flag8)
					{
						rect2.yMax = rect.yMax;
					}
					else
					{
						bool flag9 = direction == NavigateFocusRing.Right;
						if (flag9)
						{
							rect2.xMin = rect.xMin;
						}
						else
						{
							bool flag10 = direction == NavigateFocusRing.Left;
							if (flag10)
							{
								rect2.xMax = rect.xMax;
							}
						}
					}
				}
				focusableHierarchyTraversal = default(NavigateFocusRing.FocusableHierarchyTraversal);
				focusableHierarchyTraversal.currentFocusable = visualElement;
				focusableHierarchyTraversal.direction = direction;
				focusableHierarchyTraversal.validRect = rect2;
				focusableHierarchyTraversal.firstPass = false;
				focusable = focusableHierarchyTraversal.GetBestOverall(this.m_Root, null);
				bool flag11 = focusable != null;
				if (flag11)
				{
					focusable2 = focusable;
				}
				else
				{
					focusable2 = currentFocusable;
				}
			}
			return focusable2;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0003CE60 File Offset: 0x0003B060
		private static bool IsActive(VisualElement v)
		{
			return v.resolvedStyle.display != DisplayStyle.None && v.enabledInHierarchy;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0003CE8C File Offset: 0x0003B08C
		private static bool IsNavigable(Focusable focusable)
		{
			return focusable.canGrabFocus && focusable.tabIndex >= 0 && !focusable.delegatesFocus && !focusable.excludeFromFocusRing;
		}

		// Token: 0x0400075A RID: 1882
		public static readonly NavigateFocusRing.ChangeDirection Left = new NavigateFocusRing.ChangeDirection(1);

		// Token: 0x0400075B RID: 1883
		public static readonly NavigateFocusRing.ChangeDirection Right = new NavigateFocusRing.ChangeDirection(2);

		// Token: 0x0400075C RID: 1884
		public static readonly NavigateFocusRing.ChangeDirection Up = new NavigateFocusRing.ChangeDirection(3);

		// Token: 0x0400075D RID: 1885
		public static readonly NavigateFocusRing.ChangeDirection Down = new NavigateFocusRing.ChangeDirection(4);

		// Token: 0x0400075E RID: 1886
		public static readonly FocusChangeDirection Next = VisualElementFocusChangeDirection.right;

		// Token: 0x0400075F RID: 1887
		public static readonly FocusChangeDirection Previous = VisualElementFocusChangeDirection.left;

		// Token: 0x04000760 RID: 1888
		private readonly VisualElement m_Root;

		// Token: 0x04000761 RID: 1889
		private readonly VisualElementFocusRing m_Ring;

		// Token: 0x0200024C RID: 588
		public class ChangeDirection : FocusChangeDirection
		{
			// Token: 0x060010CF RID: 4303 RVA: 0x0003CF11 File Offset: 0x0003B111
			public ChangeDirection(int i)
				: base(i)
			{
			}
		}

		// Token: 0x0200024D RID: 589
		private struct FocusableHierarchyTraversal
		{
			// Token: 0x060010D0 RID: 4304 RVA: 0x0003CF1C File Offset: 0x0003B11C
			private bool ValidateHierarchyTraversal(VisualElement v)
			{
				return NavigateFocusRing.IsActive(v) && v.worldBoundingBox.Overlaps(this.validRect);
			}

			// Token: 0x060010D1 RID: 4305 RVA: 0x0003CF50 File Offset: 0x0003B150
			private bool ValidateElement(VisualElement v)
			{
				return NavigateFocusRing.IsNavigable(v) && v.worldBound.Overlaps(this.validRect);
			}

			// Token: 0x060010D2 RID: 4306 RVA: 0x0003CF84 File Offset: 0x0003B184
			private int Order(VisualElement a, VisualElement b)
			{
				Rect worldBound = a.worldBound;
				Rect worldBound2 = b.worldBound;
				int num = this.StrictOrder(worldBound, worldBound2);
				return (num != 0) ? num : this.TieBreaker(worldBound, worldBound2);
			}

			// Token: 0x060010D3 RID: 4307 RVA: 0x0003CFBC File Offset: 0x0003B1BC
			private int StrictOrder(VisualElement a, VisualElement b)
			{
				return this.StrictOrder(a.worldBound, b.worldBound);
			}

			// Token: 0x060010D4 RID: 4308 RVA: 0x0003CFE0 File Offset: 0x0003B1E0
			private int StrictOrder(Rect ra, Rect rb)
			{
				float num = 0f;
				bool flag = this.direction == NavigateFocusRing.Up;
				if (flag)
				{
					num = rb.yMax - ra.yMax;
				}
				else
				{
					bool flag2 = this.direction == NavigateFocusRing.Down;
					if (flag2)
					{
						num = ra.yMin - rb.yMin;
					}
					else
					{
						bool flag3 = this.direction == NavigateFocusRing.Left;
						if (flag3)
						{
							num = rb.xMax - ra.xMax;
						}
						else
						{
							bool flag4 = this.direction == NavigateFocusRing.Right;
							if (flag4)
							{
								num = ra.xMin - rb.xMin;
							}
						}
					}
				}
				bool flag5 = !Mathf.Approximately(num, 0f);
				int num2;
				if (flag5)
				{
					num2 = ((num > 0f) ? 1 : (-1));
				}
				else
				{
					num2 = 0;
				}
				return num2;
			}

			// Token: 0x060010D5 RID: 4309 RVA: 0x0003D0AC File Offset: 0x0003B2AC
			private int TieBreaker(Rect ra, Rect rb)
			{
				Rect worldBound = this.currentFocusable.worldBound;
				float num = (ra.min - worldBound.min).sqrMagnitude - (rb.min - worldBound.min).sqrMagnitude;
				bool flag = !Mathf.Approximately(num, 0f);
				int num2;
				if (flag)
				{
					num2 = ((num > 0f) ? 1 : (-1));
				}
				else
				{
					num2 = 0;
				}
				return num2;
			}

			// Token: 0x060010D6 RID: 4310 RVA: 0x0003D128 File Offset: 0x0003B328
			public VisualElement GetBestOverall(VisualElement candidate, VisualElement bestSoFar = null)
			{
				bool flag = !this.ValidateHierarchyTraversal(candidate);
				VisualElement visualElement;
				if (flag)
				{
					visualElement = bestSoFar;
				}
				else
				{
					bool flag2 = this.ValidateElement(candidate);
					if (flag2)
					{
						bool flag3 = (!this.firstPass || this.StrictOrder(candidate, this.currentFocusable) > 0) && (bestSoFar == null || this.Order(bestSoFar, candidate) > 0);
						if (flag3)
						{
							bestSoFar = candidate;
						}
						visualElement = bestSoFar;
					}
					else
					{
						int childCount = candidate.hierarchy.childCount;
						for (int i = 0; i < childCount; i++)
						{
							VisualElement visualElement2 = candidate.hierarchy[i];
							bestSoFar = this.GetBestOverall(visualElement2, bestSoFar);
						}
						visualElement = bestSoFar;
					}
				}
				return visualElement;
			}

			// Token: 0x04000762 RID: 1890
			public VisualElement currentFocusable;

			// Token: 0x04000763 RID: 1891
			public Rect validRect;

			// Token: 0x04000764 RID: 1892
			public bool firstPass;

			// Token: 0x04000765 RID: 1893
			public NavigateFocusRing.ChangeDirection direction;
		}
	}
}
