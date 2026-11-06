using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Pool;

namespace UnityEngine.UIElements
{
	// Token: 0x02000036 RID: 54
	public abstract class BaseListViewController : CollectionViewController
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000212 RID: 530 RVA: 0x00006870 File Offset: 0x00004A70
		// (remove) Token: 0x06000213 RID: 531 RVA: 0x000068A8 File Offset: 0x00004AA8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action itemsSourceSizeChanged;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000214 RID: 532 RVA: 0x000068E0 File Offset: 0x00004AE0
		// (remove) Token: 0x06000215 RID: 533 RVA: 0x00006918 File Offset: 0x00004B18
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IEnumerable<int>> itemsAdded;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000216 RID: 534 RVA: 0x00006950 File Offset: 0x00004B50
		// (remove) Token: 0x06000217 RID: 535 RVA: 0x00006988 File Offset: 0x00004B88
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IEnumerable<int>> itemsRemoved;

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000218 RID: 536 RVA: 0x000069BD File Offset: 0x00004BBD
		protected BaseListView baseListView
		{
			get
			{
				return base.view as BaseListView;
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000069CC File Offset: 0x00004BCC
		internal override void InvokeMakeItem(ReusableCollectionItem reusableItem)
		{
			ReusableListViewItem reusableListViewItem = reusableItem as ReusableListViewItem;
			bool flag = reusableListViewItem != null;
			if (flag)
			{
				reusableListViewItem.Init(this.MakeItem(), this.baseListView.reorderable && this.baseListView.reorderMode == ListViewReorderMode.Animated);
				this.PostInitRegistration(reusableListViewItem);
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00006A20 File Offset: 0x00004C20
		internal void PostInitRegistration(ReusableListViewItem listItem)
		{
			listItem.bindableElement.style.position = Position.Relative;
			listItem.bindableElement.style.flexBasis = StyleKeyword.Initial;
			listItem.bindableElement.style.marginTop = 0f;
			listItem.bindableElement.style.marginBottom = 0f;
			listItem.bindableElement.style.paddingTop = 0f;
			listItem.bindableElement.style.flexGrow = 0f;
			listItem.bindableElement.style.flexShrink = 0f;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00006AE4 File Offset: 0x00004CE4
		internal override void InvokeBindItem(ReusableCollectionItem reusableItem, int index)
		{
			ReusableListViewItem reusableListViewItem = reusableItem as ReusableListViewItem;
			bool flag = reusableListViewItem != null;
			if (flag)
			{
				bool flag2 = this.baseListView.reorderable && this.baseListView.reorderMode == ListViewReorderMode.Animated;
				reusableListViewItem.UpdateDragHandle(flag2 && this.NeedsDragHandle(index));
			}
			base.InvokeBindItem(reusableItem, index);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00006B40 File Offset: 0x00004D40
		public virtual bool NeedsDragHandle(int index)
		{
			return true;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00006B54 File Offset: 0x00004D54
		public virtual void AddItems(int itemCount)
		{
			bool flag = itemCount <= 0;
			if (!flag)
			{
				this.EnsureItemSourceCanBeResized();
				int count = this.itemsSource.Count;
				List<int> list = CollectionPool<List<int>, int>.Get();
				try
				{
					bool isFixedSize = this.itemsSource.IsFixedSize;
					if (isFixedSize)
					{
						this.itemsSource = BaseListViewController.AddToArray((Array)this.itemsSource, itemCount);
						for (int i = 0; i < itemCount; i++)
						{
							list.Add(count + i);
						}
					}
					else
					{
						Type type = this.itemsSource.GetType();
						Type type2 = type.GetInterfaces().FirstOrDefault<Type>(new Func<Type, bool>(BaseListViewController.<AddItems>g__IsGenericList|15_0));
						bool flag2 = type2 != null && type2.GetGenericArguments()[0].IsValueType;
						if (flag2)
						{
							Type type3 = type2.GetGenericArguments()[0];
							for (int j = 0; j < itemCount; j++)
							{
								list.Add(count + j);
								this.itemsSource.Add(Activator.CreateInstance(type3));
							}
						}
						else
						{
							for (int k = 0; k < itemCount; k++)
							{
								list.Add(count + k);
								this.itemsSource.Add(null);
							}
						}
					}
					this.RaiseItemsAdded(list);
				}
				finally
				{
					CollectionPool<List<int>, int>.Release(list);
				}
				this.RaiseOnSizeChanged();
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00006CD8 File Offset: 0x00004ED8
		public virtual void Move(int index, int newIndex)
		{
			bool flag = this.itemsSource == null;
			if (!flag)
			{
				bool flag2 = index == newIndex;
				if (!flag2)
				{
					int num = Mathf.Min(index, newIndex);
					int num2 = Mathf.Max(index, newIndex);
					bool flag3 = num < 0 || num2 >= this.itemsSource.Count;
					if (!flag3)
					{
						int num3 = newIndex;
						int num4 = ((newIndex < index) ? 1 : (-1));
						while (Mathf.Min(index, newIndex) < Mathf.Max(index, newIndex))
						{
							this.Swap(index, newIndex);
							newIndex += num4;
						}
						base.RaiseItemIndexChanged(index, num3);
					}
				}
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00006D70 File Offset: 0x00004F70
		public virtual void RemoveItem(int index)
		{
			List<int> list;
			using (CollectionPool<List<int>, int>.Get(out list))
			{
				list.Add(index);
				this.RemoveItems(list);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00006DBC File Offset: 0x00004FBC
		public virtual void RemoveItems(List<int> indices)
		{
			this.EnsureItemSourceCanBeResized();
			bool flag = indices == null;
			if (!flag)
			{
				indices.Sort();
				this.RaiseItemsRemoved(indices);
				bool isFixedSize = this.itemsSource.IsFixedSize;
				if (isFixedSize)
				{
					this.itemsSource = BaseListViewController.RemoveFromArray((Array)this.itemsSource, indices);
				}
				else
				{
					for (int i = indices.Count - 1; i >= 0; i--)
					{
						this.itemsSource.RemoveAt(indices[i]);
					}
				}
				this.RaiseOnSizeChanged();
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00006E50 File Offset: 0x00005050
		internal virtual void RemoveItems(int itemCount)
		{
			bool flag = itemCount <= 0;
			if (!flag)
			{
				int itemsCount = this.GetItemsCount();
				List<int> list = CollectionPool<List<int>, int>.Get();
				try
				{
					int num = itemsCount - itemCount;
					for (int i = num; i < itemsCount; i++)
					{
						list.Add(i);
					}
					this.RemoveItems(list);
				}
				finally
				{
					CollectionPool<List<int>, int>.Release(list);
				}
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00006EC4 File Offset: 0x000050C4
		public virtual void ClearItems()
		{
			bool flag = this.itemsSource == null;
			if (!flag)
			{
				this.EnsureItemSourceCanBeResized();
				IEnumerable<int> enumerable = Enumerable.Range(0, this.itemsSource.Count - 1);
				this.itemsSource.Clear();
				this.RaiseItemsRemoved(enumerable);
				this.RaiseOnSizeChanged();
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00006F17 File Offset: 0x00005117
		protected void RaiseOnSizeChanged()
		{
			Action action = this.itemsSourceSizeChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00006F2C File Offset: 0x0000512C
		protected void RaiseItemsAdded(IEnumerable<int> indices)
		{
			Action<IEnumerable<int>> action = this.itemsAdded;
			if (action != null)
			{
				action(indices);
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00006F42 File Offset: 0x00005142
		protected void RaiseItemsRemoved(IEnumerable<int> indices)
		{
			Action<IEnumerable<int>> action = this.itemsRemoved;
			if (action != null)
			{
				action(indices);
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00006F58 File Offset: 0x00005158
		private static Array AddToArray(Array source, int itemCount)
		{
			Type elementType = source.GetType().GetElementType();
			bool flag = elementType == null;
			if (flag)
			{
				throw new InvalidOperationException("Cannot resize source, because its size is fixed.");
			}
			Array array = Array.CreateInstance(elementType, source.Length + itemCount);
			Array.Copy(source, array, source.Length);
			return array;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00006FAC File Offset: 0x000051AC
		private static Array RemoveFromArray(Array source, List<int> indicesToRemove)
		{
			int length = source.Length;
			int num = length - indicesToRemove.Count;
			bool flag = num < 0;
			if (flag)
			{
				throw new InvalidOperationException("Cannot remove more items than the current count from source.");
			}
			Type elementType = source.GetType().GetElementType();
			bool flag2 = num == 0;
			Array array;
			if (flag2)
			{
				array = Array.CreateInstance(elementType, 0);
			}
			else
			{
				Array array2 = Array.CreateInstance(elementType, num);
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < source.Length; i++)
				{
					bool flag3 = num3 < indicesToRemove.Count && indicesToRemove[num3] == i;
					if (flag3)
					{
						num3++;
					}
					else
					{
						array2.SetValue(source.GetValue(i), num2);
						num2++;
					}
				}
				array = array2;
			}
			return array;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00007074 File Offset: 0x00005274
		private void Swap(int lhs, int rhs)
		{
			IList itemsSource = this.itemsSource;
			IList itemsSource2 = this.itemsSource;
			object obj = this.itemsSource[rhs];
			object obj2 = this.itemsSource[lhs];
			itemsSource[lhs] = obj;
			itemsSource2[rhs] = obj2;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000070C8 File Offset: 0x000052C8
		private void EnsureItemSourceCanBeResized()
		{
			IList itemsSource = this.itemsSource;
			Type type = ((itemsSource != null) ? itemsSource.GetType() : null);
			bool flag = type != null && type.IsArray;
			bool flag2 = this.itemsSource == null || (this.itemsSource.IsFixedSize && !flag);
			if (flag2)
			{
				throw new InvalidOperationException("Cannot add or remove items from source, because it is null or its size is fixed.");
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000712D File Offset: 0x0000532D
		[CompilerGenerated]
		internal static bool <AddItems>g__IsGenericList|15_0(Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>);
		}
	}
}
