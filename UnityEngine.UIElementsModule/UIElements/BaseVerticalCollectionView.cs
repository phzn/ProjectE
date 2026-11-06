using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine.Pool;

namespace UnityEngine.UIElements
{
	// Token: 0x0200006B RID: 107
	public abstract class BaseVerticalCollectionView : BindableElement, ISerializationCallbackReceiver
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000489 RID: 1161 RVA: 0x0001196B File Offset: 0x0000FB6B
		// (remove) Token: 0x0600048A RID: 1162 RVA: 0x00011975 File Offset: 0x0000FB75
		[Obsolete("onItemsChosen is deprecated, use itemsChosen instead", false)]
		public event Action<IEnumerable<object>> onItemsChosen
		{
			add
			{
				this.itemsChosen += value;
			}
			remove
			{
				this.itemsChosen -= value;
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600048B RID: 1163 RVA: 0x00011980 File Offset: 0x0000FB80
		// (remove) Token: 0x0600048C RID: 1164 RVA: 0x000119B8 File Offset: 0x0000FBB8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IEnumerable<object>> itemsChosen;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600048D RID: 1165 RVA: 0x000119ED File Offset: 0x0000FBED
		// (remove) Token: 0x0600048E RID: 1166 RVA: 0x000119F7 File Offset: 0x0000FBF7
		[Obsolete("onSelectionChange is deprecated, use selectionChanged instead", false)]
		public event Action<IEnumerable<object>> onSelectionChange
		{
			add
			{
				this.selectionChanged += value;
			}
			remove
			{
				this.selectionChanged -= value;
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600048F RID: 1167 RVA: 0x00011A04 File Offset: 0x0000FC04
		// (remove) Token: 0x06000490 RID: 1168 RVA: 0x00011A3C File Offset: 0x0000FC3C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IEnumerable<object>> selectionChanged;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000491 RID: 1169 RVA: 0x00011A71 File Offset: 0x0000FC71
		// (remove) Token: 0x06000492 RID: 1170 RVA: 0x00011A7B File Offset: 0x0000FC7B
		[Obsolete("onSelectedIndicesChange is deprecated, use selectedIndicesChanged instead", false)]
		public event Action<IEnumerable<int>> onSelectedIndicesChange
		{
			add
			{
				this.selectedIndicesChanged += value;
			}
			remove
			{
				this.selectedIndicesChanged -= value;
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000493 RID: 1171 RVA: 0x00011A88 File Offset: 0x0000FC88
		// (remove) Token: 0x06000494 RID: 1172 RVA: 0x00011AC0 File Offset: 0x0000FCC0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IEnumerable<int>> selectedIndicesChanged;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000495 RID: 1173 RVA: 0x00011AF8 File Offset: 0x0000FCF8
		// (remove) Token: 0x06000496 RID: 1174 RVA: 0x00011B30 File Offset: 0x0000FD30
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int, int> itemIndexChanged;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000497 RID: 1175 RVA: 0x00011B68 File Offset: 0x0000FD68
		// (remove) Token: 0x06000498 RID: 1176 RVA: 0x00011BA0 File Offset: 0x0000FDA0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action itemsSourceChanged;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000499 RID: 1177 RVA: 0x00011BD8 File Offset: 0x0000FDD8
		// (remove) Token: 0x0600049A RID: 1178 RVA: 0x00011C10 File Offset: 0x0000FE10
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action selectionNotChanged;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x0600049B RID: 1179 RVA: 0x00011C48 File Offset: 0x0000FE48
		// (remove) Token: 0x0600049C RID: 1180 RVA: 0x00011C80 File Offset: 0x0000FE80
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Func<CanStartDragArgs, bool> canStartDrag;

		// Token: 0x0600049D RID: 1181 RVA: 0x00011CB5 File Offset: 0x0000FEB5
		internal bool HasCanStartDrag()
		{
			return this.canStartDrag != null;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00011CC0 File Offset: 0x0000FEC0
		internal bool RaiseCanStartDrag(ReusableCollectionItem item, IEnumerable<int> ids)
		{
			Func<CanStartDragArgs, bool> func = this.canStartDrag;
			return func == null || func(new CanStartDragArgs((item != null) ? item.rootElement : null, (item != null) ? item.id : (-1), ids));
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x0600049F RID: 1183 RVA: 0x00011D04 File Offset: 0x0000FF04
		// (remove) Token: 0x060004A0 RID: 1184 RVA: 0x00011D3C File Offset: 0x0000FF3C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Func<SetupDragAndDropArgs, StartDragArgs> setupDragAndDrop;

		// Token: 0x060004A1 RID: 1185 RVA: 0x00011D74 File Offset: 0x0000FF74
		internal StartDragArgs RaiseSetupDragAndDrop(ReusableCollectionItem item, IEnumerable<int> ids, StartDragArgs args)
		{
			Func<SetupDragAndDropArgs, StartDragArgs> func = this.setupDragAndDrop;
			return (func != null) ? func(new SetupDragAndDropArgs((item != null) ? item.rootElement : null, ids, args)) : args;
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060004A2 RID: 1186 RVA: 0x00011DAC File Offset: 0x0000FFAC
		// (remove) Token: 0x060004A3 RID: 1187 RVA: 0x00011DE4 File Offset: 0x0000FFE4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Func<HandleDragAndDropArgs, DragVisualMode> dragAndDropUpdate;

		// Token: 0x060004A4 RID: 1188 RVA: 0x00011E1C File Offset: 0x0001001C
		internal DragVisualMode RaiseHandleDragAndDrop(Vector2 pointerPosition, DragAndDropArgs dragAndDropArgs)
		{
			Func<HandleDragAndDropArgs, DragVisualMode> func = this.dragAndDropUpdate;
			return (func != null) ? func(new HandleDragAndDropArgs(pointerPosition, dragAndDropArgs)) : DragVisualMode.None;
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060004A5 RID: 1189 RVA: 0x00011E48 File Offset: 0x00010048
		// (remove) Token: 0x060004A6 RID: 1190 RVA: 0x00011E80 File Offset: 0x00010080
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Func<HandleDragAndDropArgs, DragVisualMode> handleDrop;

		// Token: 0x060004A7 RID: 1191 RVA: 0x00011EB8 File Offset: 0x000100B8
		internal DragVisualMode RaiseDrop(Vector2 pointerPosition, DragAndDropArgs dragAndDropArgs)
		{
			Func<HandleDragAndDropArgs, DragVisualMode> func = this.handleDrop;
			return (func != null) ? func(new HandleDragAndDropArgs(pointerPosition, dragAndDropArgs)) : DragVisualMode.None;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00011EE3 File Offset: 0x000100E3
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x00011033 File Offset: 0x0000F233
		public IList itemsSource
		{
			get
			{
				CollectionViewController viewController = this.viewController;
				return (viewController != null) ? viewController.itemsSource : null;
			}
			set
			{
				this.GetOrCreateViewController().itemsSource = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00011EF7 File Offset: 0x000100F7
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x00011EF7 File Offset: 0x000100F7
		[Obsolete("makeItem has been moved to ListView and TreeView. Use these ones instead.")]
		public Func<VisualElement> makeItem
		{
			get
			{
				throw new UnityException("makeItem has been moved to ListView and TreeView. Use these ones instead.");
			}
			set
			{
				throw new UnityException("makeItem has been moved to ListView and TreeView. Use these ones instead.");
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00011F03 File Offset: 0x00010103
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x00011F03 File Offset: 0x00010103
		[Obsolete("bindItem has been moved to ListView and TreeView. Use these ones instead.")]
		public Action<VisualElement, int> bindItem
		{
			get
			{
				throw new UnityException("bindItem has been moved to ListView and TreeView. Use these ones instead.");
			}
			set
			{
				throw new UnityException("bindItem has been moved to ListView and TreeView. Use these ones instead.");
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00011F0F File Offset: 0x0001010F
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x00011F0F File Offset: 0x0001010F
		[Obsolete("unbindItem has been moved to ListView and TreeView. Use these ones instead.")]
		public Action<VisualElement, int> unbindItem
		{
			get
			{
				throw new UnityException("unbindItem has been moved to ListView and TreeView. Use these ones instead.");
			}
			set
			{
				throw new UnityException("unbindItem has been moved to ListView and TreeView. Use these ones instead.");
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00011F1B File Offset: 0x0001011B
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x00011F1B File Offset: 0x0001011B
		[Obsolete("destroyItem has been moved to ListView and TreeView. Use these ones instead.")]
		public Action<VisualElement> destroyItem
		{
			get
			{
				throw new UnityException("destroyItem has been moved to ListView and TreeView. Use these ones instead.");
			}
			set
			{
				throw new UnityException("destroyItem has been moved to ListView and TreeView. Use these ones instead.");
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00011F27 File Offset: 0x00010127
		public override VisualElement contentContainer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00011F2C File Offset: 0x0001012C
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00011F44 File Offset: 0x00010144
		public SelectionType selectionType
		{
			get
			{
				return this.m_SelectionType;
			}
			set
			{
				this.m_SelectionType = value;
				bool flag = this.m_SelectionType == SelectionType.None;
				if (flag)
				{
					this.ClearSelection();
				}
				else
				{
					bool flag2 = this.m_SelectionType == SelectionType.Single;
					if (flag2)
					{
						bool flag3 = this.m_Selection.indexCount > 1;
						if (flag3)
						{
							this.SetSelection(this.m_Selection.FirstIndex());
						}
					}
				}
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00011FA6 File Offset: 0x000101A6
		public object selectedItem
		{
			get
			{
				return this.m_Selection.FirstObject();
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00011FB4 File Offset: 0x000101B4
		public IEnumerable<object> selectedItems
		{
			get
			{
				foreach (int index in this.m_Selection.indices)
				{
					object item;
					bool flag = this.m_Selection.items.TryGetValue(index, out item);
					if (flag)
					{
						yield return item;
					}
					else
					{
						yield return null;
					}
					item = null;
				}
				List<int>.Enumerator enumerator = default(List<int>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00011FD3 File Offset: 0x000101D3
		public IEnumerable<int> selectedIds
		{
			get
			{
				return this.m_Selection.selectedIds;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00011FE0 File Offset: 0x000101E0
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x0001200D File Offset: 0x0001020D
		public int selectedIndex
		{
			get
			{
				return (this.m_Selection.indexCount == 0) ? (-1) : this.m_Selection.FirstIndex();
			}
			set
			{
				this.SetSelection(value);
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x00012018 File Offset: 0x00010218
		public IEnumerable<int> selectedIndices
		{
			get
			{
				return this.m_Selection.indices;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00012025 File Offset: 0x00010225
		internal IEnumerable<ReusableCollectionItem> activeItems
		{
			get
			{
				CollectionVirtualizationController virtualizationController = this.m_VirtualizationController;
				return ((virtualizationController != null) ? virtualizationController.activeItems : null) ?? BaseVerticalCollectionView.k_EmptyItems;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00012042 File Offset: 0x00010242
		internal ScrollView scrollView
		{
			get
			{
				return this.m_ScrollView;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001204A File Offset: 0x0001024A
		internal ListViewDragger dragger
		{
			get
			{
				return this.m_Dragger;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00012052 File Offset: 0x00010252
		internal CollectionVirtualizationController virtualizationController
		{
			get
			{
				return this.GetOrCreateVirtualizationController();
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001205A File Offset: 0x0001025A
		public CollectionViewController viewController
		{
			get
			{
				return this.m_ViewController;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00012062 File Offset: 0x00010262
		[Obsolete("resolvedItemHeight is deprecated and will be removed from the API.", false)]
		public float resolvedItemHeight
		{
			get
			{
				return this.ResolveItemHeight(-1f);
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00012070 File Offset: 0x00010270
		internal float ResolveItemHeight(float height = -1f)
		{
			return (height < 0f) ? this.fixedItemHeight : height;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00012093 File Offset: 0x00010293
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x000120A5 File Offset: 0x000102A5
		public bool showBorder
		{
			get
			{
				return this.m_ScrollView.ClassListContains(BaseVerticalCollectionView.borderUssClassName);
			}
			set
			{
				this.m_ScrollView.EnableInClassList(BaseVerticalCollectionView.borderUssClassName, value);
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000120BC File Offset: 0x000102BC
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00012104 File Offset: 0x00010304
		public bool reorderable
		{
			get
			{
				ListViewDragger dragger = this.m_Dragger;
				bool? flag;
				if (dragger == null)
				{
					flag = null;
				}
				else
				{
					ICollectionDragAndDropController dragAndDropController = dragger.dragAndDropController;
					flag = ((dragAndDropController != null) ? new bool?(dragAndDropController.enableReordering) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
			set
			{
				ICollectionDragAndDropController dragAndDropController = this.m_Dragger.dragAndDropController;
				bool flag = dragAndDropController != null && dragAndDropController.enableReordering != value;
				if (flag)
				{
					dragAndDropController.enableReordering = value;
					this.Rebuild();
				}
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00012145 File Offset: 0x00010345
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00012150 File Offset: 0x00010350
		public bool horizontalScrollingEnabled
		{
			get
			{
				return this.m_HorizontalScrollingEnabled;
			}
			set
			{
				bool flag = this.m_HorizontalScrollingEnabled == value;
				if (!flag)
				{
					this.m_HorizontalScrollingEnabled = value;
					this.m_ScrollView.horizontalScrollerVisibility = (value ? ScrollerVisibility.Auto : ScrollerVisibility.Hidden);
					this.m_ScrollView.mode = (value ? ScrollViewMode.VerticalAndHorizontal : ScrollViewMode.Vertical);
				}
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0001219C File Offset: 0x0001039C
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x000121B4 File Offset: 0x000103B4
		public AlternatingRowBackground showAlternatingRowBackgrounds
		{
			get
			{
				return this.m_ShowAlternatingRowBackgrounds;
			}
			set
			{
				bool flag = this.m_ShowAlternatingRowBackgrounds == value;
				if (!flag)
				{
					this.m_ShowAlternatingRowBackgrounds = value;
					this.RefreshItems();
				}
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x000121DF File Offset: 0x000103DF
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x000121E8 File Offset: 0x000103E8
		public CollectionVirtualizationMethod virtualizationMethod
		{
			get
			{
				return this.m_VirtualizationMethod;
			}
			set
			{
				CollectionVirtualizationMethod virtualizationMethod = this.m_VirtualizationMethod;
				this.m_VirtualizationMethod = value;
				bool flag = virtualizationMethod != value;
				if (flag)
				{
					this.CreateVirtualizationController();
					this.Rebuild();
				}
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0001221F File Offset: 0x0001041F
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x00012228 File Offset: 0x00010428
		[Obsolete("itemHeight is deprecated, use fixedItemHeight instead.", false)]
		public int itemHeight
		{
			get
			{
				return (int)this.fixedItemHeight;
			}
			set
			{
				this.fixedItemHeight = (float)value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x00012233 File Offset: 0x00010433
		// (set) Token: 0x060004CF RID: 1231 RVA: 0x0001223C File Offset: 0x0001043C
		public float fixedItemHeight
		{
			get
			{
				return this.m_FixedItemHeight;
			}
			set
			{
				bool flag = value < 0f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("fixedItemHeight", "Value needs to be positive for virtualization.");
				}
				this.m_ItemHeightIsInline = true;
				bool flag2 = Math.Abs(this.m_FixedItemHeight - value) > float.Epsilon;
				if (flag2)
				{
					this.m_FixedItemHeight = value;
					this.RefreshItems();
				}
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00012295 File Offset: 0x00010495
		internal float lastHeight
		{
			get
			{
				return this.m_LastHeight;
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001229D File Offset: 0x0001049D
		private protected virtual void CreateVirtualizationController()
		{
			this.CreateVirtualizationController<ReusableCollectionItem>();
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x000122A8 File Offset: 0x000104A8
		internal CollectionVirtualizationController GetOrCreateVirtualizationController()
		{
			bool flag = this.m_VirtualizationController == null;
			if (flag)
			{
				this.CreateVirtualizationController();
			}
			return this.m_VirtualizationController;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000122D4 File Offset: 0x000104D4
		internal void CreateVirtualizationController<T>() where T : ReusableCollectionItem, new()
		{
			CollectionVirtualizationMethod virtualizationMethod = this.virtualizationMethod;
			CollectionVirtualizationMethod collectionVirtualizationMethod = virtualizationMethod;
			if (collectionVirtualizationMethod != CollectionVirtualizationMethod.FixedHeight)
			{
				if (collectionVirtualizationMethod != CollectionVirtualizationMethod.DynamicHeight)
				{
					throw new ArgumentOutOfRangeException("virtualizationMethod", this.virtualizationMethod, "Unsupported virtualizationMethod virtualization");
				}
				this.m_VirtualizationController = new DynamicHeightVirtualizationController<T>(this);
			}
			else
			{
				this.m_VirtualizationController = new FixedHeightVirtualizationController<T>(this);
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00012330 File Offset: 0x00010530
		internal CollectionViewController GetOrCreateViewController()
		{
			bool flag = this.m_ViewController == null;
			if (flag)
			{
				this.SetViewController(this.CreateViewController());
			}
			return this.m_ViewController;
		}

		// Token: 0x060004D5 RID: 1237
		protected abstract CollectionViewController CreateViewController();

		// Token: 0x060004D6 RID: 1238 RVA: 0x00012364 File Offset: 0x00010564
		public virtual void SetViewController(CollectionViewController controller)
		{
			bool flag = this.m_ViewController != null;
			if (flag)
			{
				this.m_ViewController.itemIndexChanged -= this.m_ItemIndexChangedCallback;
				this.m_ViewController.itemsSourceChanged -= this.m_ItemsSourceChangedCallback;
				this.m_ViewController.Dispose();
				this.m_ViewController = null;
			}
			this.m_ViewController = controller;
			bool flag2 = this.m_ViewController != null;
			if (flag2)
			{
				this.m_ViewController.SetView(this);
				this.m_ViewController.itemIndexChanged += this.m_ItemIndexChangedCallback;
				this.m_ViewController.itemsSourceChanged += this.m_ItemsSourceChangedCallback;
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00012400 File Offset: 0x00010600
		internal virtual ListViewDragger CreateDragger()
		{
			return new ListViewDragger(this);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00012418 File Offset: 0x00010618
		internal void InitializeDragAndDropController(bool enableReordering)
		{
			bool flag = this.m_Dragger != null;
			if (flag)
			{
				this.m_Dragger.UnregisterCallbacksFromTarget(true);
				this.m_Dragger.dragAndDropController = null;
				this.m_Dragger = null;
			}
			this.m_Dragger = this.CreateDragger();
			this.m_Dragger.dragAndDropController = this.CreateDragAndDropController();
			bool flag2 = this.m_Dragger.dragAndDropController == null;
			if (!flag2)
			{
				this.m_Dragger.dragAndDropController.enableReordering = enableReordering;
			}
		}

		// Token: 0x060004D9 RID: 1241
		internal abstract ICollectionDragAndDropController CreateDragAndDropController();

		// Token: 0x060004DA RID: 1242 RVA: 0x0001249A File Offset: 0x0001069A
		internal void SetDragAndDropController(ICollectionDragAndDropController dragAndDropController)
		{
			if (this.m_Dragger == null)
			{
				this.m_Dragger = this.CreateDragger();
			}
			this.m_Dragger.dragAndDropController = dragAndDropController;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x000124C0 File Offset: 0x000106C0
		public BaseVerticalCollectionView()
		{
			base.AddToClassList(BaseVerticalCollectionView.ussClassName);
			this.m_Selection = new BaseVerticalCollectionView.Selection
			{
				selectedIds = this.m_SelectedIds
			};
			this.selectionType = SelectionType.Single;
			this.m_ScrollView = new ScrollView();
			this.m_ScrollView.AddToClassList(BaseVerticalCollectionView.listScrollViewUssClassName);
			this.m_ScrollView.verticalScroller.valueChanged += delegate(float v)
			{
				this.OnScroll(new Vector2(0f, v));
			};
			this.m_ScrollView.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnSizeChanged), TrickleDown.NoTrickleDown);
			base.RegisterCallback<CustomStyleResolvedEvent>(new EventCallback<CustomStyleResolvedEvent>(this.OnCustomStyleResolved), TrickleDown.NoTrickleDown);
			this.m_ScrollView.contentContainer.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.NoTrickleDown);
			this.m_ScrollView.contentContainer.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnDetachFromPanel), TrickleDown.NoTrickleDown);
			base.hierarchy.Add(this.m_ScrollView);
			this.m_ScrollView.contentContainer.focusable = true;
			this.m_ScrollView.contentContainer.usageHints &= ~UsageHints.GroupTransform;
			this.m_ScrollView.viewDataKey = "unity-vertical-collection-scroll-view";
			this.m_ScrollView.verticalScroller.viewDataKey = null;
			this.m_ScrollView.horizontalScroller.viewDataKey = null;
			base.focusable = true;
			base.isCompositeRoot = true;
			base.delegatesFocus = true;
			this.m_ItemIndexChangedCallback = new Action<int, int>(this.OnItemIndexChanged);
			this.m_ItemsSourceChangedCallback = new Action(this.OnItemsSourceChanged);
			this.InitializeDragAndDropController(false);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00012688 File Offset: 0x00010888
		public BaseVerticalCollectionView(IList itemsSource, float itemHeight = -1f)
			: this()
		{
			bool flag = Math.Abs(itemHeight - -1f) > float.Epsilon;
			if (flag)
			{
				this.m_FixedItemHeight = itemHeight;
				this.m_ItemHeightIsInline = true;
			}
			bool flag2 = itemsSource != null;
			if (flag2)
			{
				this.itemsSource = itemsSource;
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000126D8 File Offset: 0x000108D8
		[Obsolete("makeItem and bindItem are now in ListView and TreeView directly, please use a constructor without these parameters.")]
		public BaseVerticalCollectionView(IList itemsSource, float itemHeight = -1f, Func<VisualElement> makeItem = null, Action<VisualElement, int> bindItem = null)
			: this()
		{
			bool flag = Math.Abs(itemHeight - -1f) > float.Epsilon;
			if (flag)
			{
				this.m_FixedItemHeight = itemHeight;
				this.m_ItemHeightIsInline = true;
			}
			this.itemsSource = itemsSource;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001271C File Offset: 0x0001091C
		public VisualElement GetRootElementForId(int id)
		{
			ReusableCollectionItem reusableCollectionItem = this.activeItems.FirstOrDefault<ReusableCollectionItem>((ReusableCollectionItem t) => t.id == id);
			return (reusableCollectionItem != null) ? reusableCollectionItem.rootElement : null;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00012760 File Offset: 0x00010960
		public VisualElement GetRootElementForIndex(int index)
		{
			return this.GetRootElementForId(this.viewController.GetIdForIndex(index));
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00012784 File Offset: 0x00010984
		internal virtual bool HasValidDataAndBindings()
		{
			return this.m_ViewController != null && this.itemsSource != null;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000127AA File Offset: 0x000109AA
		private void OnItemIndexChanged(int srcIndex, int dstIndex)
		{
			Action<int, int> action = this.itemIndexChanged;
			if (action != null)
			{
				action(srcIndex, dstIndex);
			}
			this.RefreshItems();
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x000127C8 File Offset: 0x000109C8
		private void OnItemsSourceChanged()
		{
			Action action = this.itemsSourceChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000127E0 File Offset: 0x000109E0
		public void RefreshItem(int index)
		{
			foreach (ReusableCollectionItem reusableCollectionItem in this.activeItems)
			{
				int index2 = reusableCollectionItem.index;
				bool flag = index2 == index;
				if (flag)
				{
					this.viewController.InvokeUnbindItem(reusableCollectionItem, index2);
					this.viewController.InvokeBindItem(reusableCollectionItem, index2);
					break;
				}
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001285C File Offset: 0x00010A5C
		public void RefreshItems()
		{
			using (BaseVerticalCollectionView.k_RefreshMarker.Auto())
			{
				bool flag = this.m_ViewController == null;
				if (!flag)
				{
					IVisualElementScheduledItem rebuildScheduled = this.m_RebuildScheduled;
					bool flag2 = rebuildScheduled != null && rebuildScheduled.isActive;
					if (flag2)
					{
						this.Rebuild();
					}
					else
					{
						this.RefreshSelection();
						this.virtualizationController.Refresh(false);
						this.PostRefresh();
					}
				}
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000128E8 File Offset: 0x00010AE8
		[Obsolete("Refresh() has been deprecated. Use Rebuild() instead. (UnityUpgradable) -> Rebuild()", false)]
		public void Refresh()
		{
			this.Rebuild();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000128F4 File Offset: 0x00010AF4
		public void Rebuild()
		{
			using (BaseVerticalCollectionView.k_RebuildMarker.Auto())
			{
				bool flag = this.m_ViewController == null;
				if (!flag)
				{
					this.RefreshSelection();
					this.virtualizationController.Refresh(true);
					this.PostRefresh();
					IVisualElementScheduledItem rebuildScheduled = this.m_RebuildScheduled;
					if (rebuildScheduled != null)
					{
						rebuildScheduled.Pause();
					}
				}
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00012970 File Offset: 0x00010B70
		internal void ScheduleRebuild()
		{
			bool flag = this.m_RebuildScheduled == null;
			if (flag)
			{
				this.m_RebuildScheduled = base.schedule.Execute(new Action(this.Rebuild));
			}
			else
			{
				bool flag2 = !this.m_RebuildScheduled.isActive;
				if (flag2)
				{
					this.m_RebuildScheduled.Resume();
				}
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000129C8 File Offset: 0x00010BC8
		private void RefreshSelection()
		{
			BaseVerticalCollectionView.<>c__DisplayClass172_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.selectedIndicesChanged = false;
			CS$<>8__locals1.previousSelectionCount = this.m_Selection.indexCount;
			this.m_Selection.items.Clear();
			CollectionViewController viewController = this.viewController;
			bool flag = ((viewController != null) ? viewController.itemsSource : null) == null;
			if (flag)
			{
				this.m_Selection.ClearIndices();
				this.<RefreshSelection>g__NotifyIfChanged|172_0(ref CS$<>8__locals1);
			}
			else
			{
				bool flag2 = this.m_Selection.idCount > 0;
				if (flag2)
				{
					List<int> list;
					using (CollectionPool<List<int>, int>.Get(out list))
					{
						foreach (int num in this.m_Selection.selectedIds)
						{
							int indexForId = this.viewController.GetIndexForId(num);
							bool flag3 = indexForId < 0;
							if (flag3)
							{
								CS$<>8__locals1.selectedIndicesChanged = true;
							}
							else
							{
								bool flag4 = !this.m_Selection.ContainsIndex(indexForId);
								if (flag4)
								{
									CS$<>8__locals1.selectedIndicesChanged = true;
								}
								list.Add(indexForId);
							}
						}
						this.m_Selection.ClearIndices();
						foreach (int num2 in list)
						{
							this.m_Selection.AddIndex(num2, this.viewController.GetItemForIndex(num2));
						}
					}
				}
				this.<RefreshSelection>g__NotifyIfChanged|172_0(ref CS$<>8__locals1);
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00012B84 File Offset: 0x00010D84
		private protected virtual void PostRefresh()
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				this.m_LastHeight = this.m_ScrollView.layout.height;
				bool flag2 = float.IsNaN(this.m_ScrollView.layout.height);
				if (!flag2)
				{
					this.Resize(this.m_ScrollView.layout.size);
				}
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00012BF2 File Offset: 0x00010DF2
		public void ScrollTo(VisualElement visualElement)
		{
			this.m_ScrollView.ScrollTo(visualElement);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00012C04 File Offset: 0x00010E04
		public void ScrollToItem(int index)
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				this.virtualizationController.ScrollToItem(index);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00012C2E File Offset: 0x00010E2E
		[Obsolete("ScrollToId() has been deprecated. Use ScrollToItemById() instead. (UnityUpgradable) -> ScrollToItemById(*)", false)]
		public void ScrollToId(int id)
		{
			this.ScrollToItemById(id);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00012C3C File Offset: 0x00010E3C
		public void ScrollToItemById(int id)
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				int indexForId = this.viewController.GetIndexForId(id);
				this.virtualizationController.ScrollToItem(indexForId);
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00012C74 File Offset: 0x00010E74
		private void OnScroll(Vector2 offset)
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				this.virtualizationController.OnScroll(offset);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00012C9E File Offset: 0x00010E9E
		private void Resize(Vector2 size)
		{
			this.virtualizationController.Resize(size);
			this.m_LastHeight = size.y;
			this.virtualizationController.UpdateBackground();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00012CC8 File Offset: 0x00010EC8
		private void OnAttachToPanel(AttachToPanelEvent evt)
		{
			bool flag = evt.destinationPanel == null;
			if (!flag)
			{
				this.m_ScrollView.contentContainer.AddManipulator(this.m_NavigationManipulator = new KeyboardNavigationManipulator(new Action<KeyboardNavigationOperation, EventBase>(this.Apply)));
				this.m_ScrollView.contentContainer.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
				this.m_ScrollView.contentContainer.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
				this.m_ScrollView.contentContainer.RegisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancel), TrickleDown.NoTrickleDown);
				this.m_ScrollView.contentContainer.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00012D8C File Offset: 0x00010F8C
		private void OnDetachFromPanel(DetachFromPanelEvent evt)
		{
			bool flag = evt.originPanel == null;
			if (!flag)
			{
				this.m_ScrollView.contentContainer.RemoveManipulator(this.m_NavigationManipulator);
				this.m_ScrollView.contentContainer.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
				this.m_ScrollView.contentContainer.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
				this.m_ScrollView.contentContainer.UnregisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancel), TrickleDown.NoTrickleDown);
				this.m_ScrollView.contentContainer.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00012E3B File Offset: 0x0001103B
		[Obsolete("OnKeyDown is obsolete and will be removed from ListView. Use the event system instead, i.e. SendEvent(EventBase e).", true)]
		public void OnKeyDown(KeyDownEvent evt)
		{
			this.m_NavigationManipulator.OnKeyDown(evt);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00012E4C File Offset: 0x0001104C
		private bool Apply(KeyboardNavigationOperation op, bool shiftKey, bool altKey)
		{
			BaseVerticalCollectionView.<>c__DisplayClass183_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.shiftKey = shiftKey;
			bool flag = this.selectionType == SelectionType.None || !this.HasValidDataAndBindings();
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				switch (op)
				{
				case KeyboardNavigationOperation.SelectAll:
					this.SelectAll();
					return true;
				case KeyboardNavigationOperation.Cancel:
					this.ClearSelection();
					return true;
				case KeyboardNavigationOperation.Submit:
				{
					Action<IEnumerable<object>> action = this.itemsChosen;
					if (action != null)
					{
						action(this.selectedItems);
					}
					this.ScrollToItem(this.selectedIndex);
					return true;
				}
				case KeyboardNavigationOperation.Previous:
				{
					bool flag3 = this.selectedIndex > 0;
					if (flag3)
					{
						this.<Apply>g__HandleSelectionAndScroll|183_0(this.selectedIndex - 1, ref CS$<>8__locals1);
						return true;
					}
					break;
				}
				case KeyboardNavigationOperation.Next:
				{
					bool flag4 = this.selectedIndex + 1 < this.m_ViewController.itemsSource.Count;
					if (flag4)
					{
						this.<Apply>g__HandleSelectionAndScroll|183_0(this.selectedIndex + 1, ref CS$<>8__locals1);
						return true;
					}
					break;
				}
				case KeyboardNavigationOperation.MoveRight:
				{
					bool flag5 = this.m_Selection.indexCount > 0;
					if (flag5)
					{
						return this.HandleItemNavigation(true, altKey);
					}
					break;
				}
				case KeyboardNavigationOperation.MoveLeft:
				{
					bool flag6 = this.m_Selection.indexCount > 0;
					if (flag6)
					{
						return this.HandleItemNavigation(false, altKey);
					}
					break;
				}
				case KeyboardNavigationOperation.PageUp:
				{
					bool flag7 = this.m_Selection.indexCount > 0;
					if (flag7)
					{
						int num = (this.m_IsRangeSelectionDirectionUp ? this.m_Selection.minIndex : this.m_Selection.maxIndex);
						this.<Apply>g__HandleSelectionAndScroll|183_0(Mathf.Max(0, num - (this.virtualizationController.visibleItemCount - 1)), ref CS$<>8__locals1);
					}
					return true;
				}
				case KeyboardNavigationOperation.PageDown:
				{
					bool flag8 = this.m_Selection.indexCount > 0;
					if (flag8)
					{
						int num2 = (this.m_IsRangeSelectionDirectionUp ? this.m_Selection.minIndex : this.m_Selection.maxIndex);
						this.<Apply>g__HandleSelectionAndScroll|183_0(Mathf.Min(this.viewController.itemsSource.Count - 1, num2 + (this.virtualizationController.visibleItemCount - 1)), ref CS$<>8__locals1);
					}
					return true;
				}
				case KeyboardNavigationOperation.Begin:
					this.<Apply>g__HandleSelectionAndScroll|183_0(0, ref CS$<>8__locals1);
					return true;
				case KeyboardNavigationOperation.End:
					this.<Apply>g__HandleSelectionAndScroll|183_0(this.m_ViewController.itemsSource.Count - 1, ref CS$<>8__locals1);
					return true;
				default:
					throw new ArgumentOutOfRangeException("op", op, null);
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000130D8 File Offset: 0x000112D8
		private void Apply(KeyboardNavigationOperation op, EventBase sourceEvent)
		{
			KeyDownEvent keyDownEvent = sourceEvent as KeyDownEvent;
			bool flag;
			if (keyDownEvent == null || !keyDownEvent.shiftKey)
			{
				INavigationEvent navigationEvent = sourceEvent as INavigationEvent;
				if (navigationEvent != null)
				{
					if (navigationEvent.shiftKey)
					{
						goto IL_0030;
					}
				}
				flag = false;
				goto IL_0038;
			}
			IL_0030:
			flag = true;
			IL_0038:
			bool flag2 = flag;
			keyDownEvent = sourceEvent as KeyDownEvent;
			bool flag3;
			if (keyDownEvent == null || !keyDownEvent.altKey)
			{
				INavigationEvent navigationEvent = sourceEvent as INavigationEvent;
				if (navigationEvent != null)
				{
					if (navigationEvent.altKey)
					{
						goto IL_006C;
					}
				}
				flag3 = false;
				goto IL_0072;
			}
			IL_006C:
			flag3 = true;
			IL_0072:
			bool flag4 = flag3;
			bool flag5 = this.Apply(op, flag2, flag4);
			if (flag5)
			{
				sourceEvent.StopPropagation();
				sourceEvent.PreventDefault();
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00013178 File Offset: 0x00011378
		private protected virtual bool HandleItemNavigation(bool moveIn, bool altKey)
		{
			return false;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001318C File Offset: 0x0001138C
		private void OnPointerMove(PointerMoveEvent evt)
		{
			bool flag = evt.button == 0;
			if (flag)
			{
				bool flag2 = (evt.pressedButtons & 1) == 0;
				if (flag2)
				{
					this.ProcessPointerUp(evt);
				}
				else
				{
					this.ProcessPointerDown(evt);
				}
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000131D0 File Offset: 0x000113D0
		private void OnPointerDown(PointerDownEvent evt)
		{
			bool flag = evt.pointerType != PointerType.mouse;
			if (flag)
			{
				this.ProcessPointerDown(evt);
				base.panel.PreventCompatibilityMouseEvents(evt.pointerId);
			}
			else
			{
				this.ProcessPointerDown(evt);
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001321C File Offset: 0x0001141C
		private void OnPointerCancel(PointerCancelEvent evt)
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				bool flag2 = !evt.isPrimary;
				if (!flag2)
				{
					this.ClearSelection();
				}
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00013250 File Offset: 0x00011450
		private void OnPointerUp(PointerUpEvent evt)
		{
			bool flag = evt.pointerType != PointerType.mouse;
			if (flag)
			{
				this.ProcessPointerUp(evt);
				base.panel.PreventCompatibilityMouseEvents(evt.pointerId);
			}
			else
			{
				this.ProcessPointerUp(evt);
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001329C File Offset: 0x0001149C
		private void ProcessPointerDown(IPointerEvent evt)
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				bool flag2 = !evt.isPrimary;
				if (!flag2)
				{
					bool flag3 = evt.button != 0;
					if (!flag3)
					{
						bool flag4 = evt.pointerType != PointerType.mouse;
						if (flag4)
						{
							this.m_TouchDownPosition = evt.position;
							PointerDownEvent pointerDownEvent = evt as PointerDownEvent;
							long num = ((pointerDownEvent != null) ? pointerDownEvent.timestamp : 0L);
							this.m_PointerDownCount = ((num - this.m_LastPointerDownTimeStamp < (long)Event.GetDoubleClickTime()) ? (this.m_PointerDownCount + 1) : 1);
							this.m_LastPointerDownTimeStamp = num;
						}
						else
						{
							this.m_PointerDownCount = evt.clickCount;
							this.DoSelect(evt.localPosition, this.m_PointerDownCount, evt.actionKey, evt.shiftKey);
						}
					}
				}
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00013374 File Offset: 0x00011574
		private void ProcessPointerUp(IPointerEvent evt)
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				bool flag2 = !evt.isPrimary;
				if (!flag2)
				{
					bool flag3 = evt.button != 0;
					if (!flag3)
					{
						bool flag4 = evt.pointerType != PointerType.mouse;
						if (flag4)
						{
							bool flag5 = (evt.position - this.m_TouchDownPosition).sqrMagnitude <= 100f;
							if (flag5)
							{
								this.DoSelect(evt.localPosition, this.m_PointerDownCount, evt.actionKey, evt.shiftKey);
							}
							PointerUpEvent pointerUpEvent = evt as PointerUpEvent;
							long num = ((pointerUpEvent != null) ? pointerUpEvent.timestamp : 0L);
							this.m_PointerDownCount = ((num - this.m_LastPointerDownTimeStamp < (long)Event.GetDoubleClickTime()) ? this.m_PointerDownCount : 0);
						}
						else
						{
							int indexFromPosition = this.virtualizationController.GetIndexFromPosition(evt.localPosition);
							bool flag6 = this.selectionType == SelectionType.Multiple && !evt.shiftKey && !evt.actionKey && this.m_Selection.indexCount > 1 && this.m_Selection.ContainsIndex(indexFromPosition);
							if (flag6)
							{
								this.ProcessSingleClick(indexFromPosition);
							}
						}
					}
				}
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x000134BC File Offset: 0x000116BC
		private void DoSelect(Vector2 localPosition, int clickCount, bool actionKey, bool shiftKey)
		{
			int indexFromPosition = this.virtualizationController.GetIndexFromPosition(localPosition);
			int num = ((this.m_Selection.indexCount > 0 && this.m_Selection.FirstIndex() != indexFromPosition) ? 1 : ((clickCount > 2) ? 2 : clickCount));
			bool flag = indexFromPosition > this.viewController.itemsSource.Count - 1;
			if (!flag)
			{
				bool flag2 = this.selectionType == SelectionType.None;
				if (!flag2)
				{
					int idForIndex = this.viewController.GetIdForIndex(indexFromPosition);
					int num2 = num;
					int num3 = num2;
					if (num3 != 1)
					{
						if (num3 == 2)
						{
							bool flag3 = this.itemsChosen == null;
							if (!flag3)
							{
								bool flag4 = false;
								foreach (int num4 in this.selectedIndices)
								{
									bool flag5 = indexFromPosition == num4;
									if (flag5)
									{
										flag4 = true;
										break;
									}
								}
								this.ProcessSingleClick(indexFromPosition);
								bool flag6 = !flag4;
								if (!flag6)
								{
									Action<IEnumerable<object>> action = this.itemsChosen;
									if (action != null)
									{
										action(this.selectedItems);
									}
								}
							}
						}
					}
					else
					{
						bool flag7 = this.selectionType == SelectionType.Multiple && actionKey;
						if (flag7)
						{
							bool flag8 = this.m_Selection.ContainsId(idForIndex);
							if (flag8)
							{
								this.RemoveFromSelection(indexFromPosition);
							}
							else
							{
								this.AddToSelection(indexFromPosition);
							}
						}
						else
						{
							bool flag9 = this.selectionType == SelectionType.Multiple && shiftKey;
							if (flag9)
							{
								bool flag10 = this.m_Selection.indexCount == 0;
								if (flag10)
								{
									this.SetSelection(indexFromPosition);
								}
								else
								{
									this.DoRangeSelection(indexFromPosition);
								}
							}
							else
							{
								bool flag11 = this.selectionType == SelectionType.Multiple && this.m_Selection.ContainsIndex(indexFromPosition);
								if (flag11)
								{
									Action action2 = this.selectionNotChanged;
									if (action2 != null)
									{
										action2();
									}
								}
								else
								{
									bool flag12 = this.selectionType == SelectionType.Single && this.m_Selection.ContainsIndex(indexFromPosition);
									if (flag12)
									{
										Action action3 = this.selectionNotChanged;
										if (action3 != null)
										{
											action3();
										}
									}
									this.SetSelection(indexFromPosition);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x000136E4 File Offset: 0x000118E4
		internal void DoRangeSelection(int rangeSelectionFinalIndex)
		{
			int num = (this.m_IsRangeSelectionDirectionUp ? this.m_Selection.maxIndex : this.m_Selection.minIndex);
			this.ClearSelectionWithoutValidation();
			List<int> list = new List<int>();
			this.m_IsRangeSelectionDirectionUp = rangeSelectionFinalIndex < num;
			bool isRangeSelectionDirectionUp = this.m_IsRangeSelectionDirectionUp;
			if (isRangeSelectionDirectionUp)
			{
				for (int i = rangeSelectionFinalIndex; i <= num; i++)
				{
					list.Add(i);
				}
			}
			else
			{
				for (int j = rangeSelectionFinalIndex; j >= num; j--)
				{
					list.Add(j);
				}
			}
			this.AddToSelection(list);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001200D File Offset: 0x0001020D
		private void ProcessSingleClick(int clickedIndex)
		{
			this.SetSelection(clickedIndex);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00013784 File Offset: 0x00011984
		internal void SelectAll()
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				bool flag2 = this.selectionType != SelectionType.Multiple;
				if (!flag2)
				{
					for (int i = 0; i < this.m_ViewController.itemsSource.Count; i++)
					{
						int idForIndex = this.viewController.GetIdForIndex(i);
						object itemForIndex = this.viewController.GetItemForIndex(i);
						foreach (ReusableCollectionItem reusableCollectionItem in this.activeItems)
						{
							bool flag3 = reusableCollectionItem.id == idForIndex;
							if (flag3)
							{
								reusableCollectionItem.SetSelected(true);
							}
						}
						bool flag4 = !this.m_Selection.ContainsId(idForIndex);
						if (flag4)
						{
							this.m_Selection.AddId(idForIndex);
							this.m_Selection.AddIndex(i, itemForIndex);
						}
					}
					this.NotifyOfSelectionChange();
					base.SaveViewData();
				}
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001389C File Offset: 0x00011A9C
		public void AddToSelection(int index)
		{
			this.AddToSelection(new int[] { index });
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000138B0 File Offset: 0x00011AB0
		internal void AddToSelection(IList<int> indexes)
		{
			bool flag = !this.HasValidDataAndBindings() || indexes == null || indexes.Count == 0;
			if (!flag)
			{
				foreach (int num in indexes)
				{
					this.AddToSelectionWithoutValidation(num);
				}
				this.NotifyOfSelectionChange();
				base.SaveViewData();
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001392C File Offset: 0x00011B2C
		private void AddToSelectionWithoutValidation(int index)
		{
			bool flag = this.m_Selection.ContainsIndex(index);
			if (!flag)
			{
				int idForIndex = this.viewController.GetIdForIndex(index);
				object itemForIndex = this.viewController.GetItemForIndex(index);
				foreach (ReusableCollectionItem reusableCollectionItem in this.activeItems)
				{
					bool flag2 = reusableCollectionItem.id == idForIndex;
					if (flag2)
					{
						reusableCollectionItem.SetSelected(true);
					}
				}
				this.m_Selection.AddId(idForIndex);
				this.m_Selection.AddIndex(index, itemForIndex);
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000139D8 File Offset: 0x00011BD8
		public void RemoveFromSelection(int index)
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				this.RemoveFromSelectionWithoutValidation(index);
				this.NotifyOfSelectionChange();
				base.SaveViewData();
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00013A0C File Offset: 0x00011C0C
		private void RemoveFromSelectionWithoutValidation(int index)
		{
			bool flag = !this.m_Selection.TryRemove(index);
			if (!flag)
			{
				int idForIndex = this.viewController.GetIdForIndex(index);
				foreach (ReusableCollectionItem reusableCollectionItem in this.activeItems)
				{
					bool flag2 = reusableCollectionItem.id == idForIndex;
					if (flag2)
					{
						reusableCollectionItem.SetSelected(false);
					}
				}
				this.m_Selection.RemoveId(idForIndex);
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00013A9C File Offset: 0x00011C9C
		public void SetSelection(int index)
		{
			bool flag = index < 0;
			if (flag)
			{
				this.ClearSelection();
			}
			else
			{
				this.SetSelection(new int[] { index });
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00013ACD File Offset: 0x00011CCD
		public void SetSelection(IEnumerable<int> indices)
		{
			this.SetSelectionInternal(indices, true);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00013AD9 File Offset: 0x00011CD9
		public void SetSelectionWithoutNotify(IEnumerable<int> indices)
		{
			this.SetSelectionInternal(indices, false);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00013AE8 File Offset: 0x00011CE8
		internal void SetSelectionInternal(IEnumerable<int> indices, bool sendNotification)
		{
			bool flag = !this.HasValidDataAndBindings() || indices == null;
			if (!flag)
			{
				bool flag2 = this.MatchesExistingSelection(indices);
				if (!flag2)
				{
					this.ClearSelectionWithoutValidation();
					ICollection collection = indices as ICollection;
					bool flag3 = collection != null && this.m_Selection.capacity < collection.Count;
					if (flag3)
					{
						this.m_Selection.capacity = collection.Count;
					}
					foreach (int num in indices)
					{
						this.AddToSelectionWithoutValidation(num);
					}
					if (sendNotification)
					{
						this.NotifyOfSelectionChange();
					}
					base.SaveViewData();
				}
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00013BB8 File Offset: 0x00011DB8
		private bool MatchesExistingSelection(IEnumerable<int> indices)
		{
			IList<int> list = indices as IList<int>;
			List<int> list2 = null;
			bool flag3;
			try
			{
				bool flag = list == null;
				if (flag)
				{
					list2 = CollectionPool<List<int>, int>.Get();
					list2.AddRange(indices);
					list = list2;
				}
				bool flag2 = list.Count != this.m_Selection.indexCount;
				if (flag2)
				{
					flag3 = false;
				}
				else
				{
					for (int i = 0; i < list.Count; i++)
					{
						bool flag4 = list[i] != this.m_Selection.indices[i];
						if (flag4)
						{
							return false;
						}
					}
					flag3 = true;
				}
			}
			finally
			{
				bool flag5 = list2 != null;
				if (flag5)
				{
					CollectionPool<List<int>, int>.Release(list2);
				}
			}
			return flag3;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00013C7C File Offset: 0x00011E7C
		private void NotifyOfSelectionChange()
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				Action<IEnumerable<object>> action = this.selectionChanged;
				if (action != null)
				{
					action(this.selectedItems);
				}
				Action<IEnumerable<int>> action2 = this.selectedIndicesChanged;
				if (action2 != null)
				{
					action2(this.m_Selection.indices);
				}
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00013CD0 File Offset: 0x00011ED0
		public void ClearSelection()
		{
			bool flag = !this.HasValidDataAndBindings() || this.m_Selection.idCount == 0;
			if (!flag)
			{
				this.ClearSelectionWithoutValidation();
				this.NotifyOfSelectionChange();
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00013D0C File Offset: 0x00011F0C
		private void ClearSelectionWithoutValidation()
		{
			foreach (ReusableCollectionItem reusableCollectionItem in this.activeItems)
			{
				reusableCollectionItem.SetSelected(false);
			}
			this.m_Selection.Clear();
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00013D6C File Offset: 0x00011F6C
		internal override void OnViewDataReady()
		{
			base.OnViewDataReady();
			string fullHierarchicalViewDataKey = base.GetFullHierarchicalViewDataKey();
			base.OverwriteFromViewData(this, fullHierarchicalViewDataKey);
			this.m_ScrollView.UpdateContentViewTransform();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00013DA0 File Offset: 0x00011FA0
		[EventInterest(new Type[]
		{
			typeof(PointerUpEvent),
			typeof(FocusEvent),
			typeof(NavigationSubmitEvent),
			typeof(BlurEvent)
		})]
		protected override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			bool flag = evt.eventTypeId == EventBase<PointerUpEvent>.TypeId();
			if (flag)
			{
				ListViewDragger dragger = this.m_Dragger;
				if (dragger != null)
				{
					dragger.OnPointerUpEvent((PointerUpEvent)evt);
				}
			}
			else
			{
				bool flag2 = evt.eventTypeId == EventBase<FocusEvent>.TypeId();
				if (flag2)
				{
					CollectionVirtualizationController virtualizationController = this.m_VirtualizationController;
					if (virtualizationController != null)
					{
						virtualizationController.OnFocus(evt.leafTarget as VisualElement);
					}
				}
				else
				{
					bool flag3 = evt.eventTypeId == EventBase<BlurEvent>.TypeId();
					if (flag3)
					{
						BlurEvent blurEvent = evt as BlurEvent;
						CollectionVirtualizationController virtualizationController2 = this.m_VirtualizationController;
						if (virtualizationController2 != null)
						{
							virtualizationController2.OnBlur(((blurEvent != null) ? blurEvent.relatedTarget : null) as VisualElement);
						}
					}
					else
					{
						bool flag4 = evt.eventTypeId == EventBase<NavigationSubmitEvent>.TypeId();
						if (flag4)
						{
							bool flag5 = evt.target == this;
							if (flag5)
							{
								this.m_ScrollView.contentContainer.Focus();
							}
						}
					}
				}
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00013E90 File Offset: 0x00012090
		private void OnSizeChanged(GeometryChangedEvent evt)
		{
			bool flag = !this.HasValidDataAndBindings();
			if (!flag)
			{
				bool flag2 = Mathf.Approximately(evt.newRect.width, evt.oldRect.width) && Mathf.Approximately(evt.newRect.height, evt.oldRect.height);
				if (!flag2)
				{
					this.Resize(evt.newRect.size);
				}
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00013F10 File Offset: 0x00012110
		private void OnCustomStyleResolved(CustomStyleResolvedEvent e)
		{
			int num;
			bool flag = !this.m_ItemHeightIsInline && e.customStyle.TryGetValue(BaseVerticalCollectionView.s_ItemHeightProperty, out num);
			if (flag)
			{
				bool flag2 = Math.Abs(this.m_FixedItemHeight - (float)num) > float.Epsilon;
				if (flag2)
				{
					this.m_FixedItemHeight = (float)num;
					this.RefreshItems();
				}
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00003CD2 File Offset: 0x00001ED2
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00013F6B File Offset: 0x0001216B
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.m_Selection.selectedIds = this.m_SelectedIds;
			this.RefreshItems();
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000140A8 File Offset: 0x000122A8
		[CompilerGenerated]
		private void <RefreshSelection>g__NotifyIfChanged|172_0(ref BaseVerticalCollectionView.<>c__DisplayClass172_0 A_1)
		{
			bool flag = A_1.selectedIndicesChanged || this.m_Selection.indexCount != A_1.previousSelectionCount;
			if (flag)
			{
				this.NotifyOfSelectionChange();
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000140E4 File Offset: 0x000122E4
		[CompilerGenerated]
		private void <Apply>g__HandleSelectionAndScroll|183_0(int index, ref BaseVerticalCollectionView.<>c__DisplayClass183_0 A_2)
		{
			bool flag = index < 0 || index >= this.m_ViewController.itemsSource.Count;
			if (!flag)
			{
				bool flag2 = ((this.selectionType == SelectionType.Multiple) & A_2.shiftKey) && this.m_Selection.indexCount != 0;
				if (flag2)
				{
					this.DoRangeSelection(index);
				}
				else
				{
					this.selectedIndex = index;
				}
				this.ScrollToItem(index);
			}
		}

		// Token: 0x040001B4 RID: 436
		private static readonly ProfilerMarker k_RefreshMarker = new ProfilerMarker("BaseVerticalCollectionView.RefreshItems");

		// Token: 0x040001B5 RID: 437
		private static readonly ProfilerMarker k_RebuildMarker = new ProfilerMarker("BaseVerticalCollectionView.Rebuild");

		// Token: 0x040001B6 RID: 438
		internal const string internalBindingKey = "__unity-collection-view-internal-binding";

		// Token: 0x040001C1 RID: 449
		private SelectionType m_SelectionType;

		// Token: 0x040001C2 RID: 450
		private static readonly List<ReusableCollectionItem> k_EmptyItems = new List<ReusableCollectionItem>();

		// Token: 0x040001C3 RID: 451
		private bool m_HorizontalScrollingEnabled;

		// Token: 0x040001C4 RID: 452
		[SerializeField]
		private AlternatingRowBackground m_ShowAlternatingRowBackgrounds = AlternatingRowBackground.None;

		// Token: 0x040001C5 RID: 453
		internal static readonly int s_DefaultItemHeight = 22;

		// Token: 0x040001C6 RID: 454
		internal float m_FixedItemHeight = (float)BaseVerticalCollectionView.s_DefaultItemHeight;

		// Token: 0x040001C7 RID: 455
		internal bool m_ItemHeightIsInline;

		// Token: 0x040001C8 RID: 456
		private CollectionVirtualizationMethod m_VirtualizationMethod;

		// Token: 0x040001C9 RID: 457
		private readonly ScrollView m_ScrollView;

		// Token: 0x040001CA RID: 458
		private CollectionViewController m_ViewController;

		// Token: 0x040001CB RID: 459
		private CollectionVirtualizationController m_VirtualizationController;

		// Token: 0x040001CC RID: 460
		private KeyboardNavigationManipulator m_NavigationManipulator;

		// Token: 0x040001CD RID: 461
		[SerializeField]
		internal SerializedVirtualizationData serializedVirtualizationData = new SerializedVirtualizationData();

		// Token: 0x040001CE RID: 462
		[SerializeField]
		private readonly List<int> m_SelectedIds = new List<int>();

		// Token: 0x040001CF RID: 463
		private readonly BaseVerticalCollectionView.Selection m_Selection;

		// Token: 0x040001D0 RID: 464
		private float m_LastHeight;

		// Token: 0x040001D1 RID: 465
		private bool m_IsRangeSelectionDirectionUp;

		// Token: 0x040001D2 RID: 466
		private ListViewDragger m_Dragger;

		// Token: 0x040001D3 RID: 467
		internal const float ItemHeightUnset = -1f;

		// Token: 0x040001D4 RID: 468
		internal static CustomStyleProperty<int> s_ItemHeightProperty = new CustomStyleProperty<int>("--unity-item-height");

		// Token: 0x040001D5 RID: 469
		private Action<int, int> m_ItemIndexChangedCallback;

		// Token: 0x040001D6 RID: 470
		private Action m_ItemsSourceChangedCallback;

		// Token: 0x040001D7 RID: 471
		internal IVisualElementScheduledItem m_RebuildScheduled;

		// Token: 0x040001D8 RID: 472
		public static readonly string ussClassName = "unity-collection-view";

		// Token: 0x040001D9 RID: 473
		public static readonly string borderUssClassName = BaseVerticalCollectionView.ussClassName + "--with-border";

		// Token: 0x040001DA RID: 474
		public static readonly string itemUssClassName = BaseVerticalCollectionView.ussClassName + "__item";

		// Token: 0x040001DB RID: 475
		public static readonly string dragHoverBarUssClassName = BaseVerticalCollectionView.ussClassName + "__drag-hover-bar";

		// Token: 0x040001DC RID: 476
		public static readonly string dragHoverMarkerUssClassName = BaseVerticalCollectionView.ussClassName + "__drag-hover-marker";

		// Token: 0x040001DD RID: 477
		public static readonly string itemDragHoverUssClassName = BaseVerticalCollectionView.itemUssClassName + "--drag-hover";

		// Token: 0x040001DE RID: 478
		public static readonly string itemSelectedVariantUssClassName = BaseVerticalCollectionView.itemUssClassName + "--selected";

		// Token: 0x040001DF RID: 479
		public static readonly string itemAlternativeBackgroundUssClassName = BaseVerticalCollectionView.itemUssClassName + "--alternative-background";

		// Token: 0x040001E0 RID: 480
		public static readonly string listScrollViewUssClassName = BaseVerticalCollectionView.ussClassName + "__scroll-view";

		// Token: 0x040001E1 RID: 481
		internal static readonly string backgroundFillUssClassName = BaseVerticalCollectionView.ussClassName + "__background-fill";

		// Token: 0x040001E2 RID: 482
		private Vector3 m_TouchDownPosition;

		// Token: 0x040001E3 RID: 483
		private long m_LastPointerDownTimeStamp;

		// Token: 0x040001E4 RID: 484
		private int m_PointerDownCount;

		// Token: 0x0200006C RID: 108
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			// Token: 0x170000ED RID: 237
			// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001415C File Offset: 0x0001235C
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			// Token: 0x06000518 RID: 1304 RVA: 0x0001417C File Offset: 0x0001237C
			public UxmlTraits()
			{
				base.focusable.defaultValue = true;
			}

			// Token: 0x06000519 RID: 1305 RVA: 0x00014294 File Offset: 0x00012494
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				int num = 0;
				BaseVerticalCollectionView baseVerticalCollectionView = (BaseVerticalCollectionView)ve;
				baseVerticalCollectionView.reorderable = this.m_Reorderable.GetValueFromBag(bag, cc);
				bool flag = this.m_FixedItemHeight.TryGetValueFromBag(bag, cc, ref num);
				if (flag)
				{
					baseVerticalCollectionView.fixedItemHeight = (float)num;
				}
				baseVerticalCollectionView.virtualizationMethod = this.m_VirtualizationMethod.GetValueFromBag(bag, cc);
				baseVerticalCollectionView.showBorder = this.m_ShowBorder.GetValueFromBag(bag, cc);
				baseVerticalCollectionView.selectionType = this.m_SelectionType.GetValueFromBag(bag, cc);
				baseVerticalCollectionView.showAlternatingRowBackgrounds = this.m_ShowAlternatingRowBackgrounds.GetValueFromBag(bag, cc);
				baseVerticalCollectionView.horizontalScrollingEnabled = this.m_HorizontalScrollingEnabled.GetValueFromBag(bag, cc);
			}

			// Token: 0x040001E5 RID: 485
			private readonly UxmlIntAttributeDescription m_FixedItemHeight = new UxmlIntAttributeDescription
			{
				name = "fixed-item-height",
				obsoleteNames = new string[] { "itemHeight, item-height" },
				defaultValue = BaseVerticalCollectionView.s_DefaultItemHeight
			};

			// Token: 0x040001E6 RID: 486
			private readonly UxmlEnumAttributeDescription<CollectionVirtualizationMethod> m_VirtualizationMethod = new UxmlEnumAttributeDescription<CollectionVirtualizationMethod>
			{
				name = "virtualization-method",
				defaultValue = CollectionVirtualizationMethod.FixedHeight
			};

			// Token: 0x040001E7 RID: 487
			private readonly UxmlBoolAttributeDescription m_ShowBorder = new UxmlBoolAttributeDescription
			{
				name = "show-border",
				defaultValue = false
			};

			// Token: 0x040001E8 RID: 488
			private readonly UxmlEnumAttributeDescription<SelectionType> m_SelectionType = new UxmlEnumAttributeDescription<SelectionType>
			{
				name = "selection-type",
				defaultValue = SelectionType.Single
			};

			// Token: 0x040001E9 RID: 489
			private readonly UxmlEnumAttributeDescription<AlternatingRowBackground> m_ShowAlternatingRowBackgrounds = new UxmlEnumAttributeDescription<AlternatingRowBackground>
			{
				name = "show-alternating-row-backgrounds",
				defaultValue = AlternatingRowBackground.None
			};

			// Token: 0x040001EA RID: 490
			private readonly UxmlBoolAttributeDescription m_Reorderable = new UxmlBoolAttributeDescription
			{
				name = "reorderable",
				defaultValue = false
			};

			// Token: 0x040001EB RID: 491
			private readonly UxmlBoolAttributeDescription m_HorizontalScrollingEnabled = new UxmlBoolAttributeDescription
			{
				name = "horizontal-scrolling",
				defaultValue = false
			};
		}

		// Token: 0x0200006E RID: 110
		private class Selection
		{
			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x06000522 RID: 1314 RVA: 0x000143E3 File Offset: 0x000125E3
			// (set) Token: 0x06000523 RID: 1315 RVA: 0x000143EB File Offset: 0x000125EB
			public List<int> selectedIds { get; set; }

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x06000524 RID: 1316 RVA: 0x000143F4 File Offset: 0x000125F4
			public int indexCount
			{
				get
				{
					return this.indices.Count;
				}
			}

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x06000525 RID: 1317 RVA: 0x00014401 File Offset: 0x00012601
			public int idCount
			{
				get
				{
					return this.selectedIds.Count;
				}
			}

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x06000526 RID: 1318 RVA: 0x00014410 File Offset: 0x00012610
			public int minIndex
			{
				get
				{
					bool flag = this.m_MinIndex == -1;
					if (flag)
					{
						this.m_MinIndex = this.indices.Min();
					}
					return this.m_MinIndex;
				}
			}

			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x06000527 RID: 1319 RVA: 0x00014448 File Offset: 0x00012648
			public int maxIndex
			{
				get
				{
					bool flag = this.m_MaxIndex == -1;
					if (flag)
					{
						this.m_MaxIndex = this.indices.Max();
					}
					return this.m_MaxIndex;
				}
			}

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x06000528 RID: 1320 RVA: 0x0001447E File Offset: 0x0001267E
			// (set) Token: 0x06000529 RID: 1321 RVA: 0x0001448C File Offset: 0x0001268C
			public int capacity
			{
				get
				{
					return this.indices.Capacity;
				}
				set
				{
					this.indices.Capacity = value;
					bool flag = this.selectedIds.Capacity < value;
					if (flag)
					{
						this.selectedIds.Capacity = value;
					}
				}
			}

			// Token: 0x0600052A RID: 1322 RVA: 0x000144C6 File Offset: 0x000126C6
			public int FirstIndex()
			{
				return (this.indices.Count > 0) ? this.indices[0] : (-1);
			}

			// Token: 0x0600052B RID: 1323 RVA: 0x000144E8 File Offset: 0x000126E8
			public object FirstObject()
			{
				object obj;
				return this.items.TryGetValue(this.FirstIndex(), out obj) ? obj : null;
			}

			// Token: 0x0600052C RID: 1324 RVA: 0x0001450E File Offset: 0x0001270E
			public bool ContainsIndex(int index)
			{
				return this.m_IndexLookup.Contains(index);
			}

			// Token: 0x0600052D RID: 1325 RVA: 0x0001451C File Offset: 0x0001271C
			public bool ContainsId(int id)
			{
				return this.m_IdLookup.Contains(id);
			}

			// Token: 0x0600052E RID: 1326 RVA: 0x0001452A File Offset: 0x0001272A
			public void AddId(int id)
			{
				this.selectedIds.Add(id);
				this.m_IdLookup.Add(id);
			}

			// Token: 0x0600052F RID: 1327 RVA: 0x00014548 File Offset: 0x00012748
			public void AddIndex(int index, object obj)
			{
				this.m_IndexLookup.Add(index);
				this.indices.Add(index);
				this.items[index] = obj;
				bool flag = index < this.m_MinIndex;
				if (flag)
				{
					this.m_MinIndex = index;
				}
				bool flag2 = index > this.m_MaxIndex;
				if (flag2)
				{
					this.m_MaxIndex = index;
				}
			}

			// Token: 0x06000530 RID: 1328 RVA: 0x000145A8 File Offset: 0x000127A8
			public bool TryRemove(int index)
			{
				bool flag = !this.m_IndexLookup.Remove(index);
				bool flag2;
				if (flag)
				{
					flag2 = false;
				}
				else
				{
					int num = this.indices.IndexOf(index);
					bool flag3 = num >= 0;
					if (flag3)
					{
						this.indices.RemoveAt(num);
						this.items.Remove(index);
						bool flag4 = index == this.m_MinIndex;
						if (flag4)
						{
							this.m_MinIndex = -1;
						}
						bool flag5 = index == this.m_MaxIndex;
						if (flag5)
						{
							this.m_MaxIndex = -1;
						}
					}
					flag2 = true;
				}
				return flag2;
			}

			// Token: 0x06000531 RID: 1329 RVA: 0x00014632 File Offset: 0x00012832
			public void RemoveId(int id)
			{
				this.selectedIds.Remove(id);
				this.m_IdLookup.Remove(id);
			}

			// Token: 0x06000532 RID: 1330 RVA: 0x0001464F File Offset: 0x0001284F
			public void ClearItems()
			{
				this.items.Clear();
			}

			// Token: 0x06000533 RID: 1331 RVA: 0x0001465E File Offset: 0x0001285E
			public void ClearIds()
			{
				this.m_IdLookup.Clear();
				this.selectedIds.Clear();
			}

			// Token: 0x06000534 RID: 1332 RVA: 0x00014679 File Offset: 0x00012879
			public void ClearIndices()
			{
				this.m_IndexLookup.Clear();
				this.indices.Clear();
				this.m_MinIndex = -1;
				this.m_MaxIndex = -1;
			}

			// Token: 0x06000535 RID: 1333 RVA: 0x000146A2 File Offset: 0x000128A2
			public void Clear()
			{
				this.ClearItems();
				this.ClearIds();
				this.ClearIndices();
			}

			// Token: 0x040001F0 RID: 496
			private readonly HashSet<int> m_IndexLookup = new HashSet<int>();

			// Token: 0x040001F1 RID: 497
			private readonly HashSet<int> m_IdLookup = new HashSet<int>();

			// Token: 0x040001F2 RID: 498
			private int m_MinIndex = -1;

			// Token: 0x040001F3 RID: 499
			private int m_MaxIndex = -1;

			// Token: 0x040001F5 RID: 501
			public readonly List<int> indices = new List<int>();

			// Token: 0x040001F6 RID: 502
			public readonly Dictionary<int, object> items = new Dictionary<int, object>();
		}
	}
}
