using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace UnityEngine.UIElements
{
	// Token: 0x02000046 RID: 70
	internal class DynamicHeightVirtualizationController<T> : VerticalVirtualizationController<T> where T : ReusableCollectionItem, new()
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00009484 File Offset: 0x00007684
		internal IReadOnlyDictionary<int, float> itemHeightCache
		{
			get
			{
				return this.m_ItemHeightCache;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000948C File Offset: 0x0000768C
		private float defaultExpectedHeight
		{
			get
			{
				bool flag = this.m_MinimumItemHeight > 0f;
				float num;
				if (flag)
				{
					num = this.m_MinimumItemHeight;
				}
				else
				{
					bool flag2 = this.m_CollectionView.m_ItemHeightIsInline && this.m_CollectionView.fixedItemHeight > 0f;
					if (flag2)
					{
						num = this.m_CollectionView.fixedItemHeight;
					}
					else
					{
						num = (float)BaseVerticalCollectionView.s_DefaultItemHeight;
					}
				}
				return num;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x000094F2 File Offset: 0x000076F2
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x000094FF File Offset: 0x000076FF
		private float contentPadding
		{
			get
			{
				return base.serializedData.contentPadding;
			}
			set
			{
				this.m_CollectionView.scrollView.contentContainer.style.paddingTop = value;
				base.serializedData.contentPadding = value;
				this.m_CollectionView.SaveViewData();
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000953B File Offset: 0x0000773B
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x00009548 File Offset: 0x00007748
		private float contentHeight
		{
			get
			{
				return base.serializedData.contentHeight;
			}
			set
			{
				this.m_CollectionView.scrollView.contentContainer.style.height = value;
				base.serializedData.contentHeight = value;
				this.m_CollectionView.SaveViewData();
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00009584 File Offset: 0x00007784
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00009591 File Offset: 0x00007791
		private int anchoredIndex
		{
			get
			{
				return base.serializedData.anchoredItemIndex;
			}
			set
			{
				base.serializedData.anchoredItemIndex = value;
				this.m_CollectionView.SaveViewData();
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x000095AC File Offset: 0x000077AC
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x000095B9 File Offset: 0x000077B9
		private float anchorOffset
		{
			get
			{
				return base.serializedData.anchorOffset;
			}
			set
			{
				base.serializedData.anchorOffset = value;
				this.m_CollectionView.SaveViewData();
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002FA RID: 762 RVA: 0x000095D4 File Offset: 0x000077D4
		private float viewportMaxOffset
		{
			get
			{
				return base.serializedData.scrollOffset.y + this.m_ScrollView.contentViewport.layout.height;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000960A File Offset: 0x0000780A
		protected override bool alwaysRebindOnRefresh
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00009610 File Offset: 0x00007810
		public DynamicHeightVirtualizationController(BaseVerticalCollectionView collectionView)
			: base(collectionView)
		{
			this.m_FillCallback = new Action(this.Fill);
			this.m_ScrollCallback = new Action(this.OnScrollUpdate);
			this.m_GeometryChangedCallback = new Action<ReusableCollectionItem>(this.OnRecycledItemGeometryChanged);
			this.m_IndexOutOfBoundsPredicate = new Predicate<int>(this.IsIndexOutOfBounds);
			this.m_ScrollResetCallback = new Action(this.ResetScroll);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000096D4 File Offset: 0x000078D4
		public override void Refresh(bool rebuild)
		{
			this.CleanItemHeightCache();
			int count = this.m_ActiveItems.Count;
			bool flag = false;
			if (rebuild)
			{
				this.m_WaitingCache.Clear();
			}
			else
			{
				flag |= this.m_WaitingCache.RemoveWhere(this.m_IndexOutOfBoundsPredicate) > 0;
			}
			base.Refresh(rebuild);
			this.m_ScrollDirection = DynamicHeightVirtualizationController<T>.ScrollDirection.Idle;
			this.m_LastChange = DynamicHeightVirtualizationController<T>.VirtualizationChange.None;
			bool flag2 = this.m_CollectionView.HasValidDataAndBindings();
			if (flag2)
			{
				bool flag3 = flag || count != this.m_ActiveItems.Count;
				if (flag3)
				{
					this.contentHeight = this.GetExpectedContentHeight();
					float num = Mathf.Max(0f, this.contentHeight - this.m_ScrollView.contentViewport.layout.height);
					this.m_ScrollView.verticalScroller.slider.SetHighValueWithoutNotify(num);
					this.m_ScrollView.verticalScroller.value = base.serializedData.scrollOffset.y;
					base.serializedData.scrollOffset.y = this.m_ScrollView.verticalScroller.value;
				}
				this.ScheduleFill();
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000980C File Offset: 0x00007A0C
		public override void ScrollToItem(int index)
		{
			bool flag = index < -1;
			if (!flag)
			{
				float height = this.m_ScrollView.contentContainer.layout.height;
				float height2 = this.m_ScrollView.contentViewport.layout.height;
				bool flag2 = index == -1;
				if (flag2)
				{
					this.m_ForcedLastVisibleItem = base.itemsCount - 1;
					this.m_ForcedFirstVisibleItem = -1;
					this.m_StickToBottom = true;
					this.m_ScrollView.scrollOffset = new Vector2(0f, (height2 >= height) ? 0f : height);
				}
				else
				{
					bool flag3 = this.firstVisibleIndex >= index;
					if (flag3)
					{
						Vector2 vector = new Vector2(0f, this.GetContentHeightForIndex(index - 1));
						bool flag4 = vector == this.m_ScrollView.scrollOffset;
						if (!flag4)
						{
							this.m_ForcedFirstVisibleItem = index;
							this.m_ForcedLastVisibleItem = -1;
							this.m_ScrollView.scrollOffset = vector;
						}
					}
					else
					{
						float contentHeightForIndex = this.GetContentHeightForIndex(index);
						bool flag5 = contentHeightForIndex < this.contentPadding + height2;
						if (!flag5)
						{
							float num = contentHeightForIndex - height2 + (float)BaseVerticalCollectionView.s_DefaultItemHeight;
							this.m_ForcedLastVisibleItem = index;
							this.m_ForcedFirstVisibleItem = -1;
							this.m_ScrollView.scrollOffset = new Vector2(0f, num);
						}
					}
				}
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000995C File Offset: 0x00007B5C
		public override void Resize(Vector2 size)
		{
			float expectedContentHeight = this.GetExpectedContentHeight();
			this.contentHeight = Mathf.Max(expectedContentHeight, this.contentHeight);
			float height = this.m_ScrollView.contentViewport.layout.height;
			float num = Mathf.Max(0f, this.contentHeight - height);
			float num2 = Mathf.Min(base.serializedData.scrollOffset.y, num);
			this.m_ScrollView.verticalScroller.slider.SetHighValueWithoutNotify(num);
			this.m_ScrollView.verticalScroller.slider.SetValueWithoutNotify(num2);
			base.serializedData.scrollOffset.y = this.m_ScrollView.verticalScroller.value;
			float num3 = this.m_CollectionView.ResolveItemHeight(size.y);
			int num4 = Mathf.CeilToInt(num3 / this.defaultExpectedHeight);
			int num5 = num4;
			bool flag = num5 <= 0;
			if (!flag)
			{
				num5 += 2;
				int num6 = Mathf.Min(num5, base.itemsCount);
				bool flag2 = this.m_ActiveItems.Count != num6;
				if (flag2)
				{
					int count = this.m_ActiveItems.Count;
					bool flag3 = count > num6;
					if (flag3)
					{
						int num7 = count - num6;
						for (int i = 0; i < num7; i++)
						{
							int num8 = this.m_ActiveItems.Count - 1;
							this.ReleaseItem(num8);
						}
					}
					else
					{
						int num9 = num6 - this.m_ActiveItems.Count;
						int num10 = ((this.firstVisibleIndex < 0) ? 0 : this.firstVisibleIndex);
						for (int j = 0; j < num9; j++)
						{
							int num11 = j + num10 + count;
							T orMakeItemAtIndex = this.GetOrMakeItemAtIndex(-1, -1);
							bool flag4 = this.IsIndexOutOfBounds(num11);
							if (flag4)
							{
								this.HideItem(this.m_ActiveItems.Count - 1);
							}
							else
							{
								base.Setup(orMakeItemAtIndex, num11);
								this.MarkWaitingForLayout(orMakeItemAtIndex);
							}
						}
					}
				}
				this.ScheduleFill();
				this.ScheduleScrollDirectionReset();
				this.m_LastChange = DynamicHeightVirtualizationController<T>.VirtualizationChange.Resize;
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00009B7C File Offset: 0x00007D7C
		public override void OnScroll(Vector2 scrollOffset)
		{
			bool flag = this.m_DelayedScrollOffset == scrollOffset;
			if (!flag)
			{
				this.m_DelayedScrollOffset = scrollOffset;
				bool flag2 = this.m_ForcedFirstVisibleItem != -1 || this.m_ForcedLastVisibleItem != -1;
				if (flag2)
				{
					this.OnScrollUpdate();
					this.m_LastChange = DynamicHeightVirtualizationController<T>.VirtualizationChange.ForcedScroll;
				}
				else
				{
					bool flag3 = this.m_ScheduledItem == null;
					if (flag3)
					{
						DynamicHeightVirtualizationController<T>.VirtualizationChange lastChange = this.m_LastChange;
						bool flag4 = lastChange == DynamicHeightVirtualizationController<T>.VirtualizationChange.Resize || lastChange == DynamicHeightVirtualizationController<T>.VirtualizationChange.ForcedScroll;
						if (flag4)
						{
							this.m_ScheduledItem = this.m_CollectionView.schedule.Execute(this.m_FillCallback);
							float height = this.m_ScrollView.contentViewport.layout.height;
							float num = Mathf.Max(0f, this.contentHeight - height);
							float num2 = Mathf.Min(base.serializedData.scrollOffset.y, num);
							this.m_ScrollView.verticalScroller.slider.SetHighValueWithoutNotify(num);
							this.m_ScrollView.verticalScroller.slider.SetValueWithoutNotify(num2);
							base.serializedData.scrollOffset.y = this.m_ScrollView.verticalScroller.value;
							return;
						}
					}
					this.ScheduleScroll();
				}
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00009CC8 File Offset: 0x00007EC8
		private void OnScrollUpdate()
		{
			Vector2 vector = (float.IsNegativeInfinity(this.m_DelayedScrollOffset.y) ? base.serializedData.scrollOffset : this.m_DelayedScrollOffset);
			bool flag = float.IsNaN(this.m_ScrollView.contentViewport.layout.height) || float.IsNaN(vector.y);
			if (!flag)
			{
				this.m_LastChange = DynamicHeightVirtualizationController<T>.VirtualizationChange.Scroll;
				float expectedContentHeight = this.GetExpectedContentHeight();
				this.contentHeight = Mathf.Max(expectedContentHeight, this.contentHeight);
				this.m_ScrollDirection = ((vector.y < base.serializedData.scrollOffset.y) ? DynamicHeightVirtualizationController<T>.ScrollDirection.Up : DynamicHeightVirtualizationController<T>.ScrollDirection.Down);
				float num = Mathf.Max(0f, this.contentHeight - this.m_ScrollView.contentViewport.layout.height);
				bool flag2 = vector.y <= 0f;
				if (flag2)
				{
					this.m_ForcedFirstVisibleItem = 0;
				}
				this.m_StickToBottom = num > 0f && Math.Abs(vector.y - this.m_ScrollView.verticalScroller.highValue) < float.Epsilon;
				base.serializedData.scrollOffset = vector;
				this.m_CollectionView.SaveViewData();
				int num2 = ((this.m_ForcedFirstVisibleItem != -1) ? this.m_ForcedFirstVisibleItem : this.GetFirstVisibleItem(base.serializedData.scrollOffset.y));
				float contentHeightForIndex = this.GetContentHeightForIndex(num2 - 1);
				this.contentPadding = contentHeightForIndex;
				this.m_ForcedFirstVisibleItem = -1;
				bool flag3 = num2 != this.firstVisibleIndex;
				if (flag3)
				{
					this.CycleItems(num2);
				}
				else
				{
					this.Fill();
				}
				this.ScheduleScrollDirectionReset();
				this.m_DelayedScrollOffset = Vector2.negativeInfinity;
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00009E8C File Offset: 0x0000808C
		private void CycleItems(int firstIndex)
		{
			bool flag = firstIndex == this.firstVisibleIndex;
			if (!flag)
			{
				T firstVisibleItem = base.firstVisibleItem;
				this.contentPadding = this.GetContentHeightForIndex(firstIndex - 1);
				this.firstVisibleIndex = firstIndex;
				bool flag2 = this.m_ActiveItems.Count > 0;
				if (flag2)
				{
					bool flag3 = firstVisibleItem == null || this.m_ActiveItems.Count <= Mathf.Abs(this.firstVisibleIndex - firstVisibleItem.index);
					if (!flag3)
					{
						bool flag4 = this.firstVisibleIndex < firstVisibleItem.index;
						if (flag4)
						{
							int num = firstVisibleItem.index - this.firstVisibleIndex;
							List<T> scrollInsertionList = this.m_ScrollInsertionList;
							for (int i = 0; i < num; i++)
							{
								List<T> activeItems = this.m_ActiveItems;
								T t = activeItems[activeItems.Count - 1];
								scrollInsertionList.Insert(0, t);
								this.m_ActiveItems.RemoveAt(this.m_ActiveItems.Count - 1);
								t.rootElement.SendToBack();
							}
							this.m_ActiveItems.InsertRange(0, scrollInsertionList);
							this.m_ScrollInsertionList.Clear();
						}
						else
						{
							List<T> scrollInsertionList2 = this.m_ScrollInsertionList;
							int num2 = 0;
							while (this.firstVisibleIndex > this.m_ActiveItems[num2].index)
							{
								T t2 = this.m_ActiveItems[num2];
								scrollInsertionList2.Add(t2);
								num2++;
								t2.rootElement.BringToFront();
							}
							this.m_ActiveItems.RemoveRange(0, num2);
							this.m_ActiveItems.AddRange(scrollInsertionList2);
							this.m_ScrollInsertionList.Clear();
						}
					}
					float num3 = this.contentPadding;
					for (int j = 0; j < this.m_ActiveItems.Count; j++)
					{
						T t3 = this.m_ActiveItems[j];
						int num4 = this.firstVisibleIndex + j;
						int index = t3.index;
						bool flag5 = t3.rootElement.style.display == DisplayStyle.Flex;
						this.m_WaitingCache.Remove(index);
						bool flag6 = this.IsIndexOutOfBounds(num4);
						if (flag6)
						{
							this.HideItem(j);
						}
						else
						{
							base.Setup(t3, num4);
							bool flag7 = num3 > this.viewportMaxOffset;
							bool flag8 = flag7;
							if (flag8)
							{
								this.HideItem(j);
							}
							else
							{
								bool flag9 = num4 != index || !flag5;
								if (flag9)
								{
									this.MarkWaitingForLayout(t3);
								}
							}
							num3 += this.GetExpectedItemHeight(num4);
						}
					}
				}
				bool flag10 = this.m_LastChange != DynamicHeightVirtualizationController<T>.VirtualizationChange.Resize;
				if (flag10)
				{
					this.UpdateAnchor();
				}
				this.ScheduleFill();
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000A188 File Offset: 0x00008388
		private bool NeedsFill()
		{
			bool flag = this.m_LastChange != DynamicHeightVirtualizationController<T>.VirtualizationChange.None || this.anchoredIndex < 0;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				T t = base.lastVisibleItem;
				int num = ((t != null) ? t.index : (-1));
				float num2 = this.contentPadding;
				bool flag3 = num2 > base.serializedData.scrollOffset.y;
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					for (int i = this.firstVisibleIndex; i < base.itemsCount; i++)
					{
						bool flag4 = num2 > this.viewportMaxOffset || (num2 == this.viewportMaxOffset && !this.m_StickToBottom);
						if (flag4)
						{
							break;
						}
						num2 += this.GetExpectedItemHeight(i);
						bool flag5 = i > num;
						if (flag5)
						{
							return true;
						}
					}
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000A25C File Offset: 0x0000845C
		private void Fill()
		{
			bool flag = !this.m_CollectionView.HasValidDataAndBindings();
			if (!flag)
			{
				bool flag2 = this.m_ActiveItems.Count == 0;
				if (flag2)
				{
					this.contentHeight = 0f;
					this.contentPadding = 0f;
				}
				else
				{
					bool flag3 = this.anchoredIndex < 0;
					if (!flag3)
					{
						bool flag4 = this.contentPadding > this.contentHeight;
						if (flag4)
						{
							this.OnScrollUpdate();
						}
						else
						{
							float num = this.contentPadding;
							float num2 = this.contentPadding;
							int num3 = 0;
							for (int i = this.firstVisibleIndex; i < base.itemsCount; i++)
							{
								bool flag5 = num2 > this.viewportMaxOffset || (num2 == this.viewportMaxOffset && !this.m_StickToBottom);
								if (flag5)
								{
									break;
								}
								num2 += this.GetExpectedItemHeight(i);
								T t = this.m_ActiveItems[num3++];
								bool flag6 = t.index != i || t.rootElement.style.display == DisplayStyle.None;
								if (flag6)
								{
									base.Setup(t, i);
									this.MarkWaitingForLayout(t);
								}
								bool flag7 = num3 >= this.m_ActiveItems.Count;
								if (flag7)
								{
									break;
								}
							}
							bool flag8 = this.firstVisibleIndex > 0 && this.contentPadding > base.serializedData.scrollOffset.y;
							if (flag8)
							{
								List<T> scrollInsertionList = this.m_ScrollInsertionList;
								for (int j = this.m_ActiveItems.Count - 1; j >= num3; j--)
								{
									bool flag9 = this.firstVisibleIndex == 0;
									if (flag9)
									{
										break;
									}
									T t2 = this.m_ActiveItems[j];
									scrollInsertionList.Insert(0, t2);
									this.m_ActiveItems.RemoveAt(this.m_ActiveItems.Count - 1);
									t2.rootElement.SendToBack();
									int num4 = this.firstVisibleIndex - 1;
									this.firstVisibleIndex = num4;
									int num5 = num4;
									base.Setup(t2, num5);
									this.MarkWaitingForLayout(t2);
									num -= this.GetExpectedItemHeight(num5);
									bool flag10 = num < base.serializedData.scrollOffset.y;
									if (flag10)
									{
										break;
									}
								}
								this.m_ActiveItems.InsertRange(0, scrollInsertionList);
								this.m_ScrollInsertionList.Clear();
							}
							this.contentPadding = num;
							this.contentHeight = this.GetExpectedContentHeight();
							bool flag11 = this.m_LastChange != DynamicHeightVirtualizationController<T>.VirtualizationChange.Resize;
							if (flag11)
							{
								this.UpdateAnchor();
							}
							bool flag12 = this.m_WaitingCache.Count == 0;
							if (flag12)
							{
								this.ResetScroll();
								this.ApplyScrollViewUpdate(true);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000A554 File Offset: 0x00008754
		private void UpdateScrollViewContainer(float previousHeight, float newHeight)
		{
			bool stickToBottom = this.m_StickToBottom;
			if (!stickToBottom)
			{
				bool flag = this.m_ForcedLastVisibleItem >= 0;
				if (flag)
				{
					float contentHeightForIndex = this.GetContentHeightForIndex(this.m_ForcedLastVisibleItem);
					base.serializedData.scrollOffset.y = contentHeightForIndex + (float)BaseVerticalCollectionView.s_DefaultItemHeight - this.m_ScrollView.contentViewport.layout.height;
				}
				else
				{
					bool flag2 = this.m_ScrollDirection == DynamicHeightVirtualizationController<T>.ScrollDirection.Up;
					if (flag2)
					{
						SerializedVirtualizationData serializedData = base.serializedData;
						serializedData.scrollOffset.y = serializedData.scrollOffset.y + (newHeight - previousHeight);
					}
				}
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000A5EC File Offset: 0x000087EC
		private void ApplyScrollViewUpdate(bool dimensionsOnly = false)
		{
			float contentPadding = this.contentPadding;
			float y = base.serializedData.scrollOffset.y;
			float num = y - contentPadding;
			bool flag = this.anchoredIndex >= 0;
			if (flag)
			{
				bool flag2 = this.firstVisibleIndex != this.anchoredIndex;
				if (flag2)
				{
					this.CycleItems(this.anchoredIndex);
					this.ScheduleFill();
				}
				this.firstVisibleIndex = this.anchoredIndex;
				num = this.anchorOffset;
			}
			float expectedContentHeight = this.GetExpectedContentHeight();
			this.contentHeight = expectedContentHeight;
			this.contentPadding = this.GetContentHeightForIndex(this.firstVisibleIndex - 1);
			float num2 = Mathf.Max(0f, this.m_ScrollView.RoundToPanelPixelSize(expectedContentHeight - this.m_ScrollView.contentViewport.layout.height));
			float num3 = Mathf.Min(this.contentPadding + num, num2);
			bool flag3 = this.m_StickToBottom && num2 > 0f;
			if (flag3)
			{
				num3 = num2;
			}
			else
			{
				bool flag4 = this.m_ForcedLastVisibleItem != -1;
				if (flag4)
				{
					float contentHeightForIndex = this.GetContentHeightForIndex(this.m_ForcedLastVisibleItem);
					float num4 = contentHeightForIndex + (float)BaseVerticalCollectionView.s_DefaultItemHeight - this.m_ScrollView.contentViewport.layout.height;
					num3 = Mathf.Clamp(num4, 0f, num2);
				}
			}
			this.m_ScrollView.verticalScroller.slider.SetHighValueWithoutNotify(num2);
			this.m_ScrollView.verticalScroller.slider.SetValueWithoutNotify(num3);
			base.serializedData.scrollOffset.y = this.m_ScrollView.verticalScroller.slider.value;
			this.m_ForcedLastVisibleItem = -1;
			bool flag5 = dimensionsOnly || this.m_LastChange == DynamicHeightVirtualizationController<T>.VirtualizationChange.Resize;
			if (flag5)
			{
				this.ScheduleScrollDirectionReset();
			}
			else
			{
				bool flag6 = this.NeedsFill();
				if (flag6)
				{
					this.Fill();
				}
				else
				{
					float num5 = this.contentPadding;
					int firstVisibleIndex = this.firstVisibleIndex;
					List<T> scrollInsertionList = this.m_ScrollInsertionList;
					int num6 = 0;
					for (int i = 0; i < this.m_ActiveItems.Count; i++)
					{
						T t = this.m_ActiveItems[i];
						int index = t.index;
						bool flag7 = index < 0;
						if (flag7)
						{
							break;
						}
						float expectedItemHeight = this.GetExpectedItemHeight(index);
						bool flag8 = this.m_ActiveItems[i].rootElement.style.display == DisplayStyle.Flex;
						if (flag8)
						{
							bool flag9 = num5 + expectedItemHeight < base.serializedData.scrollOffset.y;
							if (flag9)
							{
								t.rootElement.BringToFront();
								this.HideItem(i);
								scrollInsertionList.Add(t);
								num6++;
								int firstVisibleIndex2 = this.firstVisibleIndex;
								this.firstVisibleIndex = firstVisibleIndex2 + 1;
							}
							else
							{
								bool flag10 = num5 > this.viewportMaxOffset;
								if (flag10)
								{
									this.HideItem(i);
								}
							}
						}
						num5 += this.GetExpectedItemHeight(index);
					}
					this.m_ActiveItems.RemoveRange(0, num6);
					this.m_ActiveItems.AddRange(scrollInsertionList);
					this.m_ScrollInsertionList.Clear();
					bool flag11 = this.firstVisibleIndex != firstVisibleIndex;
					if (flag11)
					{
						this.contentPadding = this.GetContentHeightForIndex(this.firstVisibleIndex - 1);
						this.UpdateAnchor();
					}
					this.ScheduleScrollDirectionReset();
					this.m_CollectionView.SaveViewData();
				}
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000A987 File Offset: 0x00008B87
		private void UpdateAnchor()
		{
			this.anchoredIndex = this.firstVisibleIndex;
			this.anchorOffset = base.serializedData.scrollOffset.y - this.contentPadding;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000A9B8 File Offset: 0x00008BB8
		private void ScheduleFill()
		{
			bool flag = this.m_ScheduledItem == null;
			if (flag)
			{
				this.m_ScheduledItem = this.m_CollectionView.schedule.Execute(this.m_FillCallback);
			}
			else
			{
				this.m_ScheduledItem.Pause();
				this.m_ScheduledItem.Resume();
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000AA0C File Offset: 0x00008C0C
		private void ScheduleScroll()
		{
			bool flag = this.m_ScrollScheduledItem == null;
			if (flag)
			{
				this.m_ScrollScheduledItem = this.m_CollectionView.schedule.Execute(this.m_ScrollCallback);
			}
			else
			{
				this.m_ScrollScheduledItem.Pause();
				this.m_ScrollScheduledItem.Resume();
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000AA60 File Offset: 0x00008C60
		private void ScheduleScrollDirectionReset()
		{
			bool flag = this.m_ScrollResetScheduledItem == null;
			if (flag)
			{
				this.m_ScrollResetScheduledItem = this.m_CollectionView.schedule.Execute(this.m_ScrollResetCallback);
			}
			else
			{
				this.m_ScrollResetScheduledItem.Pause();
				this.m_ScrollResetScheduledItem.Resume();
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000AAB2 File Offset: 0x00008CB2
		private void ResetScroll()
		{
			this.m_ScrollDirection = DynamicHeightVirtualizationController<T>.ScrollDirection.Idle;
			this.m_LastChange = DynamicHeightVirtualizationController<T>.VirtualizationChange.None;
			this.m_ScrollView.UpdateContentViewTransform();
			this.UpdateAnchor();
			this.m_CollectionView.SaveViewData();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000AAE4 File Offset: 0x00008CE4
		public override int GetIndexFromPosition(Vector2 position)
		{
			int num = 0;
			for (float num2 = 0f; num2 < position.y; num2 += this.GetExpectedItemHeight(num++))
			{
			}
			return num - 1;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000AB20 File Offset: 0x00008D20
		public override float GetExpectedItemHeight(int index)
		{
			int draggedIndex = base.GetDraggedIndex();
			bool flag = draggedIndex >= 0 && index == draggedIndex;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				float num2;
				num = (this.m_ItemHeightCache.TryGetValue(index, out num2) ? num2 : this.defaultExpectedHeight);
			}
			return num;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000AB6C File Offset: 0x00008D6C
		private int GetFirstVisibleItem(float offset)
		{
			bool flag = offset <= 0f;
			int num;
			if (flag)
			{
				num = 0;
			}
			else
			{
				int num2 = -1;
				while (offset > 0f)
				{
					num2++;
					float expectedItemHeight = this.GetExpectedItemHeight(num2);
					offset -= expectedItemHeight;
				}
				num = num2;
			}
			return num;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000ABB8 File Offset: 0x00008DB8
		public override float GetExpectedContentHeight()
		{
			return this.m_AccumulatedHeight + (float)(base.itemsCount - this.m_ItemHeightCache.Count) * this.defaultExpectedHeight;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000ABEC File Offset: 0x00008DEC
		private float GetContentHeightForIndex(int lastIndex)
		{
			bool flag = lastIndex < 0;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo contentHeightCacheInfo;
				bool flag2 = this.m_ContentHeightCache.TryGetValue(lastIndex, out contentHeightCacheInfo);
				if (flag2)
				{
					int draggedIndex = base.GetDraggedIndex();
					bool flag3 = draggedIndex >= 0 && lastIndex >= draggedIndex;
					if (flag3)
					{
						num = contentHeightCacheInfo.sum + (float)(lastIndex - contentHeightCacheInfo.count + 1) * this.defaultExpectedHeight - this.m_DraggedItem.rootElement.layout.height;
					}
					else
					{
						num = contentHeightCacheInfo.sum + (float)(lastIndex - contentHeightCacheInfo.count + 1) * this.defaultExpectedHeight;
					}
				}
				else
				{
					num = this.GetContentHeightForIndex(lastIndex - 1) + this.GetExpectedItemHeight(lastIndex);
				}
			}
			return num;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000ACAC File Offset: 0x00008EAC
		private DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo GetCachedContentHeight(int index)
		{
			while (index >= 0)
			{
				DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo contentHeightCacheInfo;
				bool flag = this.m_ContentHeightCache.TryGetValue(index, out contentHeightCacheInfo);
				if (flag)
				{
					return contentHeightCacheInfo;
				}
				index--;
			}
			return default(DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		private void RegisterItemHeight(int index, float height)
		{
			bool flag = height <= 0f;
			if (!flag)
			{
				float num = this.m_CollectionView.ResolveItemHeight(height);
				float num2;
				bool flag2 = this.m_ItemHeightCache.TryGetValue(index, out num2);
				if (flag2)
				{
					this.m_AccumulatedHeight -= num2;
				}
				this.m_AccumulatedHeight += num;
				this.m_ItemHeightCache[index] = num;
				bool flag3 = index > this.m_HighestCachedIndex;
				if (flag3)
				{
					this.m_HighestCachedIndex = index;
				}
				bool flag4 = num2 == 0f;
				DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo cachedContentHeight = this.GetCachedContentHeight(index - 1);
				this.m_ContentHeightCache[index] = new DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo(cachedContentHeight.sum + num, cachedContentHeight.count + 1);
				foreach (KeyValuePair<int, float> keyValuePair in this.m_ItemHeightCache)
				{
					bool flag5 = keyValuePair.Key > index;
					if (flag5)
					{
						DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo contentHeightCacheInfo = this.m_ContentHeightCache[keyValuePair.Key];
						this.m_ContentHeightCache[keyValuePair.Key] = new DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo(contentHeightCacheInfo.sum - num2 + num, flag4 ? (contentHeightCacheInfo.count + 1) : contentHeightCacheInfo.count);
					}
				}
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000AE54 File Offset: 0x00009054
		private void UnregisterItemHeight(int index)
		{
			float num;
			bool flag = !this.m_ItemHeightCache.TryGetValue(index, out num);
			if (!flag)
			{
				this.m_AccumulatedHeight -= num;
				this.m_ItemHeightCache.Remove(index);
				this.m_ContentHeightCache.Remove(index);
				int num2 = -1;
				foreach (KeyValuePair<int, float> keyValuePair in this.m_ItemHeightCache)
				{
					bool flag2 = keyValuePair.Key > index;
					if (flag2)
					{
						DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo contentHeightCacheInfo = this.m_ContentHeightCache[keyValuePair.Key];
						this.m_ContentHeightCache[keyValuePair.Key] = new DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo(contentHeightCacheInfo.sum - num, contentHeightCacheInfo.count - 1);
					}
					bool flag3 = keyValuePair.Key > num2;
					if (flag3)
					{
						num2 = keyValuePair.Key;
					}
				}
				this.m_HighestCachedIndex = num2;
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000AF5C File Offset: 0x0000915C
		private void CleanItemHeightCache()
		{
			bool flag = !this.IsIndexOutOfBounds(this.m_HighestCachedIndex);
			if (!flag)
			{
				List<int> list = CollectionPool<List<int>, int>.Get();
				try
				{
					foreach (int num in this.m_ItemHeightCache.Keys)
					{
						bool flag2 = this.IsIndexOutOfBounds(num);
						if (flag2)
						{
							list.Add(num);
						}
					}
					foreach (int num2 in list)
					{
						this.UnregisterItemHeight(num2);
					}
				}
				finally
				{
					CollectionPool<List<int>, int>.Release(list);
				}
				this.m_MinimumItemHeight = -1f;
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000B054 File Offset: 0x00009254
		private void OnRecycledItemGeometryChanged(ReusableCollectionItem item)
		{
			bool flag = item.index == -1 || item.isDragGhost || float.IsNaN(item.rootElement.layout.height) || item.rootElement.layout.height == 0f;
			if (!flag)
			{
				bool flag2 = this.UpdateRegisteredHeight(item);
				if (flag2)
				{
					this.ApplyScrollViewUpdate(false);
				}
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000B0C8 File Offset: 0x000092C8
		private bool UpdateRegisteredHeight(ReusableCollectionItem item)
		{
			bool flag = item.index == -1 || item.isDragGhost || float.IsNaN(item.rootElement.layout.height) || item.rootElement.layout.height == 0f;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = item.rootElement.layout.height < this.defaultExpectedHeight;
				if (flag3)
				{
					this.m_MinimumItemHeight = item.rootElement.layout.height;
					this.Resize(this.m_ScrollView.layout.size);
				}
				float num = item.rootElement.layout.height - item.rootElement.resolvedStyle.paddingTop;
				float num2;
				bool flag4 = this.m_ItemHeightCache.TryGetValue(item.index, out num2);
				float num3 = (flag4 ? this.GetExpectedItemHeight(item.index) : this.defaultExpectedHeight);
				bool flag5 = this.m_WaitingCache.Count == 0;
				if (flag5)
				{
					bool flag6 = num > num3;
					if (flag6)
					{
						this.m_StickToBottom = false;
					}
					else
					{
						float num4 = num - num3;
						float num5 = Mathf.Max(0f, this.contentHeight - this.m_ScrollView.contentViewport.layout.height);
						this.m_StickToBottom = num5 > 0f && base.serializedData.scrollOffset.y >= this.m_ScrollView.verticalScroller.highValue + num4;
					}
				}
				bool flag7 = !flag4 || !Mathf.Approximately(num, num2);
				if (flag7)
				{
					this.RegisterItemHeight(item.index, num);
					this.UpdateScrollViewContainer(num3, num);
					bool flag8 = this.m_WaitingCache.Count == 0;
					if (flag8)
					{
						return true;
					}
				}
				flag2 = this.m_WaitingCache.Remove(item.index) && this.m_WaitingCache.Count == 0;
			}
			return flag2;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000B2E8 File Offset: 0x000094E8
		internal override T GetOrMakeItemAtIndex(int activeItemIndex = -1, int scrollViewIndex = -1)
		{
			T orMakeItemAtIndex = base.GetOrMakeItemAtIndex(activeItemIndex, scrollViewIndex);
			orMakeItemAtIndex.onGeometryChanged += this.m_GeometryChangedCallback;
			return orMakeItemAtIndex;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000B318 File Offset: 0x00009518
		internal override void ReleaseItem(int activeItemsIndex)
		{
			T t = this.m_ActiveItems[activeItemsIndex];
			t.onGeometryChanged -= this.m_GeometryChangedCallback;
			int index = t.index;
			this.UnregisterItemHeight(index);
			base.ReleaseItem(activeItemsIndex);
			this.m_WaitingCache.Remove(index);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000B36E File Offset: 0x0000956E
		internal override void StartDragItem(ReusableCollectionItem item)
		{
			this.m_WaitingCache.Remove(item.index);
			base.StartDragItem(item);
			this.m_DraggedItem.onGeometryChanged -= this.m_GeometryChangedCallback;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000B3A4 File Offset: 0x000095A4
		internal override void EndDrag(int dropIndex)
		{
			bool flag = this.m_DraggedItem.index < dropIndex;
			int index = this.m_DraggedItem.index;
			int num = (flag ? 1 : (-1));
			float expectedItemHeight = this.GetExpectedItemHeight(index);
			for (int num2 = index; num2 != dropIndex; num2 += num)
			{
				float expectedItemHeight2 = this.GetExpectedItemHeight(num2);
				float expectedItemHeight3 = this.GetExpectedItemHeight(num2 + num);
				bool flag2 = Mathf.Approximately(expectedItemHeight2, expectedItemHeight3);
				if (!flag2)
				{
					this.RegisterItemHeight(num2, expectedItemHeight3);
				}
			}
			this.RegisterItemHeight(flag ? (dropIndex - 1) : dropIndex, expectedItemHeight);
			bool flag3 = this.firstVisibleIndex > this.m_DraggedItem.index;
			if (flag3)
			{
				this.firstVisibleIndex = this.GetFirstVisibleItem(base.serializedData.scrollOffset.y);
				this.UpdateAnchor();
			}
			this.m_DraggedItem.onGeometryChanged += this.m_GeometryChangedCallback;
			base.EndDrag(dropIndex);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000B4A8 File Offset: 0x000096A8
		private void HideItem(int activeItemsIndex)
		{
			T t = this.m_ActiveItems[activeItemsIndex];
			t.rootElement.style.display = DisplayStyle.None;
			this.m_WaitingCache.Remove(t.index);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000B4F8 File Offset: 0x000096F8
		private void MarkWaitingForLayout(T item)
		{
			bool isDragGhost = item.isDragGhost;
			if (!isDragGhost)
			{
				this.m_WaitingCache.Add(item.index);
				item.rootElement.lastLayout.size = Vector2.zero;
				item.rootElement.MarkDirtyRepaint();
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000B55C File Offset: 0x0000975C
		private bool IsIndexOutOfBounds(int i)
		{
			return this.m_CollectionView.itemsSource == null || i >= base.itemsCount;
		}

		// Token: 0x040000CF RID: 207
		private int m_HighestCachedIndex = -1;

		// Token: 0x040000D0 RID: 208
		private readonly Dictionary<int, float> m_ItemHeightCache = new Dictionary<int, float>(32);

		// Token: 0x040000D1 RID: 209
		private readonly Dictionary<int, DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo> m_ContentHeightCache = new Dictionary<int, DynamicHeightVirtualizationController<T>.ContentHeightCacheInfo>(32);

		// Token: 0x040000D2 RID: 210
		private readonly HashSet<int> m_WaitingCache = new HashSet<int>(32);

		// Token: 0x040000D3 RID: 211
		private int m_ForcedFirstVisibleItem = -1;

		// Token: 0x040000D4 RID: 212
		private int m_ForcedLastVisibleItem = -1;

		// Token: 0x040000D5 RID: 213
		private bool m_StickToBottom;

		// Token: 0x040000D6 RID: 214
		private DynamicHeightVirtualizationController<T>.VirtualizationChange m_LastChange;

		// Token: 0x040000D7 RID: 215
		private DynamicHeightVirtualizationController<T>.ScrollDirection m_ScrollDirection;

		// Token: 0x040000D8 RID: 216
		private Vector2 m_DelayedScrollOffset = Vector2.negativeInfinity;

		// Token: 0x040000D9 RID: 217
		private float m_AccumulatedHeight;

		// Token: 0x040000DA RID: 218
		private float m_MinimumItemHeight = -1f;

		// Token: 0x040000DB RID: 219
		private Action m_FillCallback;

		// Token: 0x040000DC RID: 220
		private Action m_ScrollCallback;

		// Token: 0x040000DD RID: 221
		private Action m_ScrollResetCallback;

		// Token: 0x040000DE RID: 222
		private Action<ReusableCollectionItem> m_GeometryChangedCallback;

		// Token: 0x040000DF RID: 223
		private IVisualElementScheduledItem m_ScheduledItem;

		// Token: 0x040000E0 RID: 224
		private IVisualElementScheduledItem m_ScrollScheduledItem;

		// Token: 0x040000E1 RID: 225
		private IVisualElementScheduledItem m_ScrollResetScheduledItem;

		// Token: 0x040000E2 RID: 226
		private Predicate<int> m_IndexOutOfBoundsPredicate;

		// Token: 0x02000047 RID: 71
		private readonly struct ContentHeightCacheInfo
		{
			// Token: 0x0600031E RID: 798 RVA: 0x0000B58A File Offset: 0x0000978A
			public ContentHeightCacheInfo(float sum, int count)
			{
				this.sum = sum;
				this.count = count;
			}

			// Token: 0x040000E3 RID: 227
			public readonly float sum;

			// Token: 0x040000E4 RID: 228
			public readonly int count;
		}

		// Token: 0x02000048 RID: 72
		private enum VirtualizationChange
		{
			// Token: 0x040000E6 RID: 230
			None,
			// Token: 0x040000E7 RID: 231
			Resize,
			// Token: 0x040000E8 RID: 232
			Scroll,
			// Token: 0x040000E9 RID: 233
			ForcedScroll
		}

		// Token: 0x02000049 RID: 73
		private enum ScrollDirection
		{
			// Token: 0x040000EB RID: 235
			Idle,
			// Token: 0x040000EC RID: 236
			Up,
			// Token: 0x040000ED RID: 237
			Down
		}
	}
}
