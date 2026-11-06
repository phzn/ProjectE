using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000248 RID: 584
	public class FocusController
	{
		// Token: 0x0600109D RID: 4253 RVA: 0x0003C173 File Offset: 0x0003A373
		public FocusController(IFocusRing focusRing)
		{
			this.focusRing = focusRing;
			this.imguiKeyboardControl = 0;
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x0003C19E File Offset: 0x0003A39E
		private IFocusRing focusRing { get; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x0003C1A6 File Offset: 0x0003A3A6
		// (set) Token: 0x060010A0 RID: 4256 RVA: 0x0003C1B0 File Offset: 0x0003A3B0
		internal TextElement selectedTextElement
		{
			get
			{
				return this.m_SelectedTextElement;
			}
			set
			{
				bool flag = this.m_SelectedTextElement == value;
				if (!flag)
				{
					TextElement selectedTextElement = this.m_SelectedTextElement;
					if (selectedTextElement != null)
					{
						selectedTextElement.selection.SelectNone();
					}
					this.m_SelectedTextElement = value;
				}
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x0003C1EC File Offset: 0x0003A3EC
		public Focusable focusedElement
		{
			get
			{
				Focusable retargetedFocusedElement = this.GetRetargetedFocusedElement(null);
				return this.IsLocalElement(retargetedFocusedElement) ? retargetedFocusedElement : null;
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0003C214 File Offset: 0x0003A414
		internal bool IsFocused(Focusable f)
		{
			bool flag = !this.IsLocalElement(f);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				foreach (FocusController.FocusedElement focusedElement in this.m_FocusedElements)
				{
					bool flag3 = focusedElement.m_FocusedElement == f;
					if (flag3)
					{
						return true;
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0003C290 File Offset: 0x0003A490
		internal Focusable GetRetargetedFocusedElement(VisualElement retargetAgainst)
		{
			VisualElement visualElement = ((retargetAgainst != null) ? retargetAgainst.hierarchy.parent : null);
			bool flag = visualElement == null;
			if (flag)
			{
				bool flag2 = this.m_FocusedElements.Count > 0;
				if (flag2)
				{
					return this.m_FocusedElements[this.m_FocusedElements.Count - 1].m_FocusedElement;
				}
			}
			else
			{
				while (!visualElement.isCompositeRoot && visualElement.hierarchy.parent != null)
				{
					visualElement = visualElement.hierarchy.parent;
				}
				foreach (FocusController.FocusedElement focusedElement in this.m_FocusedElements)
				{
					bool flag3 = focusedElement.m_SubTreeRoot == visualElement;
					if (flag3)
					{
						return focusedElement.m_FocusedElement;
					}
				}
			}
			return null;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0003C398 File Offset: 0x0003A598
		internal Focusable GetLeafFocusedElement()
		{
			bool flag = this.m_FocusedElements.Count > 0;
			Focusable focusable;
			if (flag)
			{
				Focusable focusedElement = this.m_FocusedElements[0].m_FocusedElement;
				focusable = (this.IsLocalElement(focusedElement) ? focusedElement : null);
			}
			else
			{
				focusable = null;
			}
			return focusable;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0003C3E0 File Offset: 0x0003A5E0
		private bool IsLocalElement(Focusable f)
		{
			return ((f != null) ? f.focusController : null) == this;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0003C401 File Offset: 0x0003A601
		internal void ClearPendingFocusEvents()
		{
			this.m_PendingFocusCount = 0;
			this.m_LastPendingFocusedElement = null;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0003C414 File Offset: 0x0003A614
		internal bool IsPendingFocus(Focusable f)
		{
			for (VisualElement visualElement = this.m_LastPendingFocusedElement as VisualElement; visualElement != null; visualElement = visualElement.hierarchy.parent)
			{
				bool flag = f == visualElement;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0003C45C File Offset: 0x0003A65C
		internal void SetFocusToLastFocusedElement()
		{
			bool flag = this.m_LastFocusedElement != null && !(this.m_LastFocusedElement is IMGUIContainer);
			if (flag)
			{
				this.m_LastFocusedElement.Focus();
			}
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0003C498 File Offset: 0x0003A698
		internal void BlurLastFocusedElement()
		{
			this.selectedTextElement = null;
			bool flag = this.m_LastFocusedElement != null && !(this.m_LastFocusedElement is IMGUIContainer);
			if (flag)
			{
				Focusable lastFocusedElement = this.m_LastFocusedElement;
				this.m_LastFocusedElement.Blur();
				this.m_LastFocusedElement = lastFocusedElement;
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0003C4EC File Offset: 0x0003A6EC
		internal void DoFocusChange(Focusable f)
		{
			this.m_FocusedElements.Clear();
			for (VisualElement visualElement = f as VisualElement; visualElement != null; visualElement = visualElement.hierarchy.parent)
			{
				bool flag = visualElement.hierarchy.parent == null || visualElement.isCompositeRoot;
				if (flag)
				{
					this.m_FocusedElements.Add(new FocusController.FocusedElement
					{
						m_SubTreeRoot = visualElement,
						m_FocusedElement = f
					});
					f = visualElement;
				}
			}
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0003C574 File Offset: 0x0003A774
		internal void ProcessPendingFocusChange(Focusable f)
		{
			this.m_PendingFocusCount--;
			bool flag = this.m_PendingFocusCount == 0;
			if (flag)
			{
				this.m_LastPendingFocusedElement = null;
			}
			this.DoFocusChange(f);
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0003C5AC File Offset: 0x0003A7AC
		internal Focusable FocusNextInDirection(FocusChangeDirection direction)
		{
			Focusable nextFocusable = this.focusRing.GetNextFocusable(this.GetLeafFocusedElement(), direction);
			direction.ApplyTo(this, nextFocusable);
			return nextFocusable;
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0003C5DC File Offset: 0x0003A7DC
		private void AboutToReleaseFocus(Focusable focusable, Focusable willGiveFocusTo, FocusChangeDirection direction, DispatchMode dispatchMode)
		{
			using (FocusOutEvent pooled = FocusEventBase<FocusOutEvent>.GetPooled(focusable, willGiveFocusTo, direction, this, false))
			{
				focusable.SendEvent(pooled, dispatchMode);
			}
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0003C620 File Offset: 0x0003A820
		private void ReleaseFocus(Focusable focusable, Focusable willGiveFocusTo, FocusChangeDirection direction, DispatchMode dispatchMode)
		{
			using (BlurEvent pooled = FocusEventBase<BlurEvent>.GetPooled(focusable, willGiveFocusTo, direction, this, false))
			{
				focusable.SendEvent(pooled, dispatchMode);
			}
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0003C664 File Offset: 0x0003A864
		private void AboutToGrabFocus(Focusable focusable, Focusable willTakeFocusFrom, FocusChangeDirection direction, DispatchMode dispatchMode)
		{
			using (FocusInEvent pooled = FocusEventBase<FocusInEvent>.GetPooled(focusable, willTakeFocusFrom, direction, this, false))
			{
				focusable.SendEvent(pooled, dispatchMode);
			}
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0003C6A8 File Offset: 0x0003A8A8
		private void GrabFocus(Focusable focusable, Focusable willTakeFocusFrom, FocusChangeDirection direction, bool bIsFocusDelegated, DispatchMode dispatchMode)
		{
			using (FocusEvent pooled = FocusEventBase<FocusEvent>.GetPooled(focusable, willTakeFocusFrom, direction, this, bIsFocusDelegated))
			{
				focusable.SendEvent(pooled, dispatchMode);
			}
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0003C6EC File Offset: 0x0003A8EC
		internal void Blur(Focusable focusable, bool bIsFocusDelegated = false, DispatchMode dispatchMode = DispatchMode.Default)
		{
			bool flag = ((this.m_PendingFocusCount > 0) ? this.IsPendingFocus(focusable) : this.IsFocused(focusable));
			bool flag2 = flag;
			if (flag2)
			{
				this.SwitchFocus(null, bIsFocusDelegated, dispatchMode);
			}
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x0003C725 File Offset: 0x0003A925
		internal void SwitchFocus(Focusable newFocusedElement, bool bIsFocusDelegated = false, DispatchMode dispatchMode = DispatchMode.Default)
		{
			this.SwitchFocus(newFocusedElement, FocusChangeDirection.unspecified, bIsFocusDelegated, dispatchMode);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0003C738 File Offset: 0x0003A938
		internal void SwitchFocus(Focusable newFocusedElement, FocusChangeDirection direction, bool bIsFocusDelegated = false, DispatchMode dispatchMode = DispatchMode.Default)
		{
			this.m_LastFocusedElement = newFocusedElement;
			Focusable focusable = ((this.m_PendingFocusCount > 0) ? this.m_LastPendingFocusedElement : this.GetLeafFocusedElement());
			bool flag = focusable == newFocusedElement;
			if (!flag)
			{
				bool flag2 = newFocusedElement == null || !newFocusedElement.canGrabFocus;
				if (flag2)
				{
					bool flag3 = focusable != null;
					if (flag3)
					{
						this.m_LastPendingFocusedElement = null;
						this.m_PendingFocusCount++;
						this.AboutToReleaseFocus(focusable, null, direction, dispatchMode);
						this.ReleaseFocus(focusable, null, direction, dispatchMode);
					}
				}
				else
				{
					bool flag4 = newFocusedElement != focusable;
					if (flag4)
					{
						VisualElement visualElement = newFocusedElement as VisualElement;
						Focusable focusable2 = ((visualElement != null) ? visualElement.RetargetElement(focusable as VisualElement) : null) ?? newFocusedElement;
						VisualElement visualElement2 = focusable as VisualElement;
						Focusable focusable3 = ((visualElement2 != null) ? visualElement2.RetargetElement(newFocusedElement as VisualElement) : null) ?? focusable;
						this.m_LastPendingFocusedElement = newFocusedElement;
						this.m_PendingFocusCount++;
						bool flag5 = focusable != null;
						if (flag5)
						{
							this.AboutToReleaseFocus(focusable, focusable2, direction, dispatchMode);
						}
						this.AboutToGrabFocus(newFocusedElement, focusable3, direction, dispatchMode);
						bool flag6 = focusable != null;
						if (flag6)
						{
							this.ReleaseFocus(focusable, focusable2, direction, dispatchMode);
						}
						this.GrabFocus(newFocusedElement, focusable3, direction, bIsFocusDelegated, dispatchMode);
					}
				}
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0003C878 File Offset: 0x0003AA78
		internal void SwitchFocusOnEvent(EventBase e)
		{
			bool processedByFocusController = e.processedByFocusController;
			if (!processedByFocusController)
			{
				using (FocusChangeDirection focusChangeDirection = this.focusRing.GetFocusChangeDirection(this.GetLeafFocusedElement(), e))
				{
					bool flag = focusChangeDirection != FocusChangeDirection.none;
					if (flag)
					{
						this.FocusNextInDirection(focusChangeDirection);
						e.processedByFocusController = true;
					}
				}
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0003C8E8 File Offset: 0x0003AAE8
		internal void ReevaluateFocus()
		{
			VisualElement visualElement = this.focusedElement as VisualElement;
			bool flag = visualElement != null;
			if (flag)
			{
				bool flag2 = !visualElement.isHierarchyDisplayed || !visualElement.visible;
				if (flag2)
				{
					visualElement.Blur();
				}
			}
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0003C92C File Offset: 0x0003AB2C
		internal bool GetFocusableParentForPointerEvent(Focusable target, out Focusable effectiveTarget)
		{
			bool flag = target == null || !target.focusable;
			bool flag2;
			if (flag)
			{
				effectiveTarget = target;
				flag2 = target != null;
			}
			else
			{
				effectiveTarget = target;
				for (;;)
				{
					VisualElement visualElement = effectiveTarget as VisualElement;
					bool flag3 = visualElement != null && (!visualElement.enabledInHierarchy || !visualElement.focusable) && visualElement.hierarchy.parent != null;
					if (!flag3)
					{
						break;
					}
					effectiveTarget = visualElement.hierarchy.parent;
				}
				flag2 = !this.IsFocused(effectiveTarget);
			}
			return flag2;
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0003C9B3 File Offset: 0x0003ABB3
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x0003C9BB File Offset: 0x0003ABBB
		internal int imguiKeyboardControl { get; set; }

		// Token: 0x060010B9 RID: 4281 RVA: 0x0003C9C4 File Offset: 0x0003ABC4
		internal void SyncIMGUIFocus(int imguiKeyboardControlID, Focusable imguiContainerHavingKeyboardControl, bool forceSwitch)
		{
			this.imguiKeyboardControl = imguiKeyboardControlID;
			bool flag = forceSwitch || this.imguiKeyboardControl != 0;
			if (flag)
			{
				this.SwitchFocus(imguiContainerHavingKeyboardControl, FocusChangeDirection.unspecified, false, DispatchMode.Default);
			}
			else
			{
				this.SwitchFocus(null, FocusChangeDirection.unspecified, false, DispatchMode.Default);
			}
		}

		// Token: 0x0400074D RID: 1869
		private TextElement m_SelectedTextElement;

		// Token: 0x0400074E RID: 1870
		private List<FocusController.FocusedElement> m_FocusedElements = new List<FocusController.FocusedElement>();

		// Token: 0x0400074F RID: 1871
		private Focusable m_LastFocusedElement;

		// Token: 0x04000750 RID: 1872
		internal Focusable m_LastPendingFocusedElement;

		// Token: 0x04000751 RID: 1873
		private int m_PendingFocusCount = 0;

		// Token: 0x02000249 RID: 585
		private struct FocusedElement
		{
			// Token: 0x04000753 RID: 1875
			public VisualElement m_SubTreeRoot;

			// Token: 0x04000754 RID: 1876
			public Focusable m_FocusedElement;
		}
	}
}
