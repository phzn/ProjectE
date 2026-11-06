using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace UnityEngine.Rendering
{
	// Token: 0x02000068 RID: 104
	public class DebugUI
	{
		// Token: 0x0200016D RID: 365
		public class Container : DebugUI.Widget, DebugUI.IContainer
		{
			// Token: 0x1700014E RID: 334
			// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0002D01E File Offset: 0x0002B21E
			internal bool hideDisplayName
			{
				get
				{
					return string.IsNullOrEmpty(base.displayName) || base.displayName.StartsWith("#");
				}
			}

			// Token: 0x1700014F RID: 335
			// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0002D03F File Offset: 0x0002B23F
			// (set) Token: 0x06000A0E RID: 2574 RVA: 0x0002D047 File Offset: 0x0002B247
			public ObservableList<DebugUI.Widget> children { get; private set; }

			// Token: 0x17000150 RID: 336
			// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0002D050 File Offset: 0x0002B250
			// (set) Token: 0x06000A10 RID: 2576 RVA: 0x0002D058 File Offset: 0x0002B258
			public override DebugUI.Panel panel
			{
				get
				{
					return this.m_Panel;
				}
				internal set
				{
					if (value != null && value.flags.HasFlag(DebugUI.Flags.FrequentlyUsed))
					{
						return;
					}
					this.m_Panel = value;
					int count = this.children.Count;
					for (int i = 0; i < count; i++)
					{
						this.children[i].panel = value;
					}
				}
			}

			// Token: 0x06000A11 RID: 2577 RVA: 0x0002D0B3 File Offset: 0x0002B2B3
			public Container()
				: this(string.Empty, new ObservableList<DebugUI.Widget>())
			{
			}

			// Token: 0x06000A12 RID: 2578 RVA: 0x0002D0C5 File Offset: 0x0002B2C5
			public Container(string id)
				: this("#" + id, new ObservableList<DebugUI.Widget>())
			{
			}

			// Token: 0x06000A13 RID: 2579 RVA: 0x0002D0E0 File Offset: 0x0002B2E0
			public Container(string displayName, ObservableList<DebugUI.Widget> children)
			{
				base.displayName = displayName;
				this.children = children;
				children.ItemAdded += this.OnItemAdded;
				children.ItemRemoved += this.OnItemRemoved;
				for (int i = 0; i < this.children.Count; i++)
				{
					this.OnItemAdded(this.children, new ListChangedEventArgs<DebugUI.Widget>(i, this.children[i]));
				}
			}

			// Token: 0x06000A14 RID: 2580 RVA: 0x0002D15C File Offset: 0x0002B35C
			internal override void GenerateQueryPath()
			{
				base.GenerateQueryPath();
				int count = this.children.Count;
				for (int i = 0; i < count; i++)
				{
					this.children[i].GenerateQueryPath();
				}
			}

			// Token: 0x06000A15 RID: 2581 RVA: 0x0002D198 File Offset: 0x0002B398
			protected virtual void OnItemAdded(ObservableList<DebugUI.Widget> sender, ListChangedEventArgs<DebugUI.Widget> e)
			{
				if (e.item != null)
				{
					e.item.panel = this.m_Panel;
					e.item.parent = this;
				}
				if (this.m_Panel != null)
				{
					this.m_Panel.SetDirty();
				}
			}

			// Token: 0x06000A16 RID: 2582 RVA: 0x0002D1D2 File Offset: 0x0002B3D2
			protected virtual void OnItemRemoved(ObservableList<DebugUI.Widget> sender, ListChangedEventArgs<DebugUI.Widget> e)
			{
				if (e.item != null)
				{
					e.item.panel = null;
					e.item.parent = null;
				}
				if (this.m_Panel != null)
				{
					this.m_Panel.SetDirty();
				}
			}

			// Token: 0x06000A17 RID: 2583 RVA: 0x0002D208 File Offset: 0x0002B408
			public override int GetHashCode()
			{
				int num = 17;
				num = num * 23 + base.queryPath.GetHashCode();
				num = num * 23 + base.isHidden.GetHashCode();
				int count = this.children.Count;
				for (int i = 0; i < count; i++)
				{
					num = num * 23 + this.children[i].GetHashCode();
				}
				return num;
			}

			// Token: 0x04000618 RID: 1560
			private const string k_IDToken = "#";
		}

		// Token: 0x0200016E RID: 366
		public class Foldout : DebugUI.Container, DebugUI.IValueField
		{
			// Token: 0x17000151 RID: 337
			// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0002D26D File Offset: 0x0002B46D
			public bool isReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000152 RID: 338
			// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0002D270 File Offset: 0x0002B470
			// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0002D278 File Offset: 0x0002B478
			public string[] columnLabels { get; set; }

			// Token: 0x17000153 RID: 339
			// (get) Token: 0x06000A1B RID: 2587 RVA: 0x0002D281 File Offset: 0x0002B481
			// (set) Token: 0x06000A1C RID: 2588 RVA: 0x0002D289 File Offset: 0x0002B489
			public string[] columnTooltips { get; set; }

			// Token: 0x06000A1D RID: 2589 RVA: 0x0002D292 File Offset: 0x0002B492
			public Foldout()
			{
			}

			// Token: 0x06000A1E RID: 2590 RVA: 0x0002D29A File Offset: 0x0002B49A
			public Foldout(string displayName, ObservableList<DebugUI.Widget> children, string[] columnLabels = null, string[] columnTooltips = null)
				: base(displayName, children)
			{
				this.columnLabels = columnLabels;
				this.columnTooltips = columnTooltips;
			}

			// Token: 0x06000A1F RID: 2591 RVA: 0x0002D2B3 File Offset: 0x0002B4B3
			public bool GetValue()
			{
				return this.opened;
			}

			// Token: 0x06000A20 RID: 2592 RVA: 0x0002D2BB File Offset: 0x0002B4BB
			object DebugUI.IValueField.GetValue()
			{
				return this.GetValue();
			}

			// Token: 0x06000A21 RID: 2593 RVA: 0x0002D2C8 File Offset: 0x0002B4C8
			public void SetValue(object value)
			{
				this.SetValue((bool)value);
			}

			// Token: 0x06000A22 RID: 2594 RVA: 0x0002D2D6 File Offset: 0x0002B4D6
			public object ValidateValue(object value)
			{
				return value;
			}

			// Token: 0x06000A23 RID: 2595 RVA: 0x0002D2D9 File Offset: 0x0002B4D9
			public void SetValue(bool value)
			{
				this.opened = value;
			}

			// Token: 0x0400061A RID: 1562
			public bool opened;

			// Token: 0x0400061B RID: 1563
			public bool isHeader;

			// Token: 0x0400061C RID: 1564
			public List<DebugUI.Foldout.ContextMenuItem> contextMenuItems;

			// Token: 0x020001F3 RID: 499
			public struct ContextMenuItem
			{
				// Token: 0x040007D4 RID: 2004
				public string displayName;

				// Token: 0x040007D5 RID: 2005
				public Action action;
			}
		}

		// Token: 0x0200016F RID: 367
		public class HBox : DebugUI.Container
		{
			// Token: 0x06000A24 RID: 2596 RVA: 0x0002D2E2 File Offset: 0x0002B4E2
			public HBox()
			{
				base.displayName = "HBox";
			}
		}

		// Token: 0x02000170 RID: 368
		public class VBox : DebugUI.Container
		{
			// Token: 0x06000A25 RID: 2597 RVA: 0x0002D2F5 File Offset: 0x0002B4F5
			public VBox()
			{
				base.displayName = "VBox";
			}
		}

		// Token: 0x02000171 RID: 369
		public class Table : DebugUI.Container
		{
			// Token: 0x06000A26 RID: 2598 RVA: 0x0002D308 File Offset: 0x0002B508
			public Table()
			{
				base.displayName = "Array";
			}

			// Token: 0x06000A27 RID: 2599 RVA: 0x0002D31C File Offset: 0x0002B51C
			public void SetColumnVisibility(int index, bool visible)
			{
				bool[] visibleColumns = this.VisibleColumns;
				if (index < 0 || index > visibleColumns.Length)
				{
					return;
				}
				visibleColumns[index] = visible;
			}

			// Token: 0x06000A28 RID: 2600 RVA: 0x0002D340 File Offset: 0x0002B540
			public bool GetColumnVisibility(int index)
			{
				bool[] visibleColumns = this.VisibleColumns;
				return index >= 0 && index <= visibleColumns.Length && visibleColumns[index];
			}

			// Token: 0x17000154 RID: 340
			// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0002D364 File Offset: 0x0002B564
			public bool[] VisibleColumns
			{
				get
				{
					if (this.m_Header != null)
					{
						return this.m_Header;
					}
					int num = 0;
					if (base.children.Count != 0)
					{
						num = ((DebugUI.Container)base.children[0]).children.Count;
						for (int i = 1; i < base.children.Count; i++)
						{
							if (((DebugUI.Container)base.children[i]).children.Count != num)
							{
								Debug.LogError("All rows must have the same number of children.");
								return null;
							}
						}
					}
					this.m_Header = new bool[num];
					for (int j = 0; j < num; j++)
					{
						this.m_Header[j] = true;
					}
					return this.m_Header;
				}
			}

			// Token: 0x06000A2A RID: 2602 RVA: 0x0002D412 File Offset: 0x0002B612
			protected override void OnItemAdded(ObservableList<DebugUI.Widget> sender, ListChangedEventArgs<DebugUI.Widget> e)
			{
				base.OnItemAdded(sender, e);
				this.m_Header = null;
			}

			// Token: 0x06000A2B RID: 2603 RVA: 0x0002D423 File Offset: 0x0002B623
			protected override void OnItemRemoved(ObservableList<DebugUI.Widget> sender, ListChangedEventArgs<DebugUI.Widget> e)
			{
				base.OnItemRemoved(sender, e);
				this.m_Header = null;
			}

			// Token: 0x0400061F RID: 1567
			public bool isReadOnly;

			// Token: 0x04000620 RID: 1568
			private bool[] m_Header;

			// Token: 0x020001F4 RID: 500
			public class Row : DebugUI.Foldout
			{
				// Token: 0x06000BAD RID: 2989 RVA: 0x00030720 File Offset: 0x0002E920
				public Row()
				{
					base.displayName = "Row";
				}
			}
		}

		// Token: 0x02000172 RID: 370
		[Flags]
		public enum Flags
		{
			// Token: 0x04000622 RID: 1570
			None = 0,
			// Token: 0x04000623 RID: 1571
			EditorOnly = 2,
			// Token: 0x04000624 RID: 1572
			RuntimeOnly = 4,
			// Token: 0x04000625 RID: 1573
			EditorForceUpdate = 8,
			// Token: 0x04000626 RID: 1574
			FrequentlyUsed = 16
		}

		// Token: 0x02000173 RID: 371
		public abstract class Widget
		{
			// Token: 0x17000155 RID: 341
			// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0002D434 File Offset: 0x0002B634
			// (set) Token: 0x06000A2D RID: 2605 RVA: 0x0002D43C File Offset: 0x0002B63C
			public virtual DebugUI.Panel panel
			{
				get
				{
					return this.m_Panel;
				}
				internal set
				{
					this.m_Panel = value;
				}
			}

			// Token: 0x17000156 RID: 342
			// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0002D445 File Offset: 0x0002B645
			// (set) Token: 0x06000A2F RID: 2607 RVA: 0x0002D44D File Offset: 0x0002B64D
			public virtual DebugUI.IContainer parent
			{
				get
				{
					return this.m_Parent;
				}
				internal set
				{
					this.m_Parent = value;
				}
			}

			// Token: 0x17000157 RID: 343
			// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0002D456 File Offset: 0x0002B656
			// (set) Token: 0x06000A31 RID: 2609 RVA: 0x0002D45E File Offset: 0x0002B65E
			public DebugUI.Flags flags { get; set; }

			// Token: 0x17000158 RID: 344
			// (get) Token: 0x06000A32 RID: 2610 RVA: 0x0002D467 File Offset: 0x0002B667
			// (set) Token: 0x06000A33 RID: 2611 RVA: 0x0002D46F File Offset: 0x0002B66F
			public string displayName { get; set; }

			// Token: 0x17000159 RID: 345
			// (get) Token: 0x06000A34 RID: 2612 RVA: 0x0002D478 File Offset: 0x0002B678
			// (set) Token: 0x06000A35 RID: 2613 RVA: 0x0002D480 File Offset: 0x0002B680
			public string tooltip { get; set; }

			// Token: 0x1700015A RID: 346
			// (get) Token: 0x06000A36 RID: 2614 RVA: 0x0002D489 File Offset: 0x0002B689
			// (set) Token: 0x06000A37 RID: 2615 RVA: 0x0002D491 File Offset: 0x0002B691
			public string queryPath { get; private set; }

			// Token: 0x1700015B RID: 347
			// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0002D49A File Offset: 0x0002B69A
			public bool isEditorOnly
			{
				get
				{
					return this.flags.HasFlag(DebugUI.Flags.EditorOnly);
				}
			}

			// Token: 0x1700015C RID: 348
			// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0002D4B2 File Offset: 0x0002B6B2
			public bool isRuntimeOnly
			{
				get
				{
					return this.flags.HasFlag(DebugUI.Flags.RuntimeOnly);
				}
			}

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0002D4CA File Offset: 0x0002B6CA
			public bool isInactiveInEditor
			{
				get
				{
					return this.isRuntimeOnly && !Application.isPlaying;
				}
			}

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0002D4DE File Offset: 0x0002B6DE
			public bool isHidden
			{
				get
				{
					Func<bool> func = this.isHiddenCallback;
					return func != null && func();
				}
			}

			// Token: 0x06000A3C RID: 2620 RVA: 0x0002D4F1 File Offset: 0x0002B6F1
			internal virtual void GenerateQueryPath()
			{
				this.queryPath = this.displayName.Trim();
				if (this.m_Parent != null)
				{
					this.queryPath = this.m_Parent.queryPath + " -> " + this.queryPath;
				}
			}

			// Token: 0x06000A3D RID: 2621 RVA: 0x0002D530 File Offset: 0x0002B730
			public override int GetHashCode()
			{
				return this.queryPath.GetHashCode() ^ this.isHidden.GetHashCode();
			}

			// Token: 0x1700015F RID: 351
			// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0002D557 File Offset: 0x0002B757
			public DebugUI.Widget.NameAndTooltip nameAndTooltip
			{
				set
				{
					this.displayName = value.name;
					this.tooltip = value.tooltip;
				}
			}

			// Token: 0x04000627 RID: 1575
			protected DebugUI.Panel m_Panel;

			// Token: 0x04000628 RID: 1576
			protected DebugUI.IContainer m_Parent;

			// Token: 0x0400062D RID: 1581
			public Func<bool> isHiddenCallback;

			// Token: 0x020001F5 RID: 501
			public struct NameAndTooltip
			{
				// Token: 0x040007D6 RID: 2006
				public string name;

				// Token: 0x040007D7 RID: 2007
				public string tooltip;
			}
		}

		// Token: 0x02000174 RID: 372
		public interface IContainer
		{
			// Token: 0x17000160 RID: 352
			// (get) Token: 0x06000A40 RID: 2624
			ObservableList<DebugUI.Widget> children { get; }

			// Token: 0x17000161 RID: 353
			// (get) Token: 0x06000A41 RID: 2625
			// (set) Token: 0x06000A42 RID: 2626
			string displayName { get; set; }

			// Token: 0x17000162 RID: 354
			// (get) Token: 0x06000A43 RID: 2627
			string queryPath { get; }
		}

		// Token: 0x02000175 RID: 373
		public interface IValueField
		{
			// Token: 0x06000A44 RID: 2628
			object GetValue();

			// Token: 0x06000A45 RID: 2629
			void SetValue(object value);

			// Token: 0x06000A46 RID: 2630
			object ValidateValue(object value);
		}

		// Token: 0x02000176 RID: 374
		public class Button : DebugUI.Widget
		{
			// Token: 0x17000163 RID: 355
			// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0002D579 File Offset: 0x0002B779
			// (set) Token: 0x06000A48 RID: 2632 RVA: 0x0002D581 File Offset: 0x0002B781
			public Action action { get; set; }
		}

		// Token: 0x02000177 RID: 375
		public class Value : DebugUI.Widget
		{
			// Token: 0x17000164 RID: 356
			// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0002D592 File Offset: 0x0002B792
			// (set) Token: 0x06000A4B RID: 2635 RVA: 0x0002D59A File Offset: 0x0002B79A
			public Func<object> getter { get; set; }

			// Token: 0x06000A4C RID: 2636 RVA: 0x0002D5A3 File Offset: 0x0002B7A3
			public Value()
			{
				base.displayName = "";
			}

			// Token: 0x06000A4D RID: 2637 RVA: 0x0002D5C1 File Offset: 0x0002B7C1
			public virtual object GetValue()
			{
				return this.getter();
			}

			// Token: 0x06000A4E RID: 2638 RVA: 0x0002D5CE File Offset: 0x0002B7CE
			public virtual string FormatString(object value)
			{
				if (!string.IsNullOrEmpty(this.formatString))
				{
					return string.Format(this.formatString, value);
				}
				return string.Format("{0}", value);
			}

			// Token: 0x04000630 RID: 1584
			public float refreshRate = 0.1f;

			// Token: 0x04000631 RID: 1585
			public string formatString;
		}

		// Token: 0x02000178 RID: 376
		public class ProgressBarValue : DebugUI.Value
		{
			// Token: 0x06000A4F RID: 2639 RVA: 0x0002D5F8 File Offset: 0x0002B7F8
			public override string FormatString(object value)
			{
				float num = DebugUI.ProgressBarValue.<FormatString>g__Remap01|2_0(Mathf.Clamp((float)value, this.min, this.max), this.min, this.max);
				return string.Format("{0:P1}", num);
			}

			// Token: 0x06000A51 RID: 2641 RVA: 0x0002D651 File Offset: 0x0002B851
			[CompilerGenerated]
			internal static float <FormatString>g__Remap01|2_0(float v, float x0, float y0)
			{
				return (v - x0) / (y0 - x0);
			}

			// Token: 0x04000632 RID: 1586
			public float min;

			// Token: 0x04000633 RID: 1587
			public float max = 1f;
		}

		// Token: 0x02000179 RID: 377
		public class ValueTuple : DebugUI.Widget
		{
			// Token: 0x17000165 RID: 357
			// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0002D65A File Offset: 0x0002B85A
			public int numElements
			{
				get
				{
					return this.values.Length;
				}
			}

			// Token: 0x17000166 RID: 358
			// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0002D664 File Offset: 0x0002B864
			public float refreshRate
			{
				get
				{
					DebugUI.Value value = this.values.FirstOrDefault<DebugUI.Value>();
					if (value == null)
					{
						return 0.1f;
					}
					return value.refreshRate;
				}
			}

			// Token: 0x04000634 RID: 1588
			public DebugUI.Value[] values;

			// Token: 0x04000635 RID: 1589
			public int pinnedElementIndex = -1;
		}

		// Token: 0x0200017A RID: 378
		public abstract class Field<T> : DebugUI.Widget, DebugUI.IValueField
		{
			// Token: 0x17000167 RID: 359
			// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0002D68F File Offset: 0x0002B88F
			// (set) Token: 0x06000A56 RID: 2646 RVA: 0x0002D697 File Offset: 0x0002B897
			public Func<T> getter { get; set; }

			// Token: 0x17000168 RID: 360
			// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0002D6A0 File Offset: 0x0002B8A0
			// (set) Token: 0x06000A58 RID: 2648 RVA: 0x0002D6A8 File Offset: 0x0002B8A8
			public Action<T> setter { get; set; }

			// Token: 0x06000A59 RID: 2649 RVA: 0x0002D6B1 File Offset: 0x0002B8B1
			object DebugUI.IValueField.ValidateValue(object value)
			{
				return this.ValidateValue((T)((object)value));
			}

			// Token: 0x06000A5A RID: 2650 RVA: 0x0002D6C4 File Offset: 0x0002B8C4
			public virtual T ValidateValue(T value)
			{
				return value;
			}

			// Token: 0x06000A5B RID: 2651 RVA: 0x0002D6C7 File Offset: 0x0002B8C7
			object DebugUI.IValueField.GetValue()
			{
				return this.GetValue();
			}

			// Token: 0x06000A5C RID: 2652 RVA: 0x0002D6D4 File Offset: 0x0002B8D4
			public T GetValue()
			{
				return this.getter();
			}

			// Token: 0x06000A5D RID: 2653 RVA: 0x0002D6E1 File Offset: 0x0002B8E1
			public void SetValue(object value)
			{
				this.SetValue((T)((object)value));
			}

			// Token: 0x06000A5E RID: 2654 RVA: 0x0002D6F0 File Offset: 0x0002B8F0
			public virtual void SetValue(T value)
			{
				T t = this.ValidateValue(value);
				if (t == null || !t.Equals(this.getter()))
				{
					this.setter(t);
					Action<DebugUI.Field<T>, T> action = this.onValueChanged;
					if (action == null)
					{
						return;
					}
					action(this, t);
				}
			}

			// Token: 0x04000638 RID: 1592
			public Action<DebugUI.Field<T>, T> onValueChanged;
		}

		// Token: 0x0200017B RID: 379
		public class BoolField : DebugUI.Field<bool>
		{
		}

		// Token: 0x0200017C RID: 380
		public class HistoryBoolField : DebugUI.BoolField
		{
			// Token: 0x17000169 RID: 361
			// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0002D75A File Offset: 0x0002B95A
			// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0002D762 File Offset: 0x0002B962
			public Func<bool>[] historyGetter { get; set; }

			// Token: 0x1700016A RID: 362
			// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0002D76B File Offset: 0x0002B96B
			public int historyDepth
			{
				get
				{
					Func<bool>[] historyGetter = this.historyGetter;
					if (historyGetter == null)
					{
						return 0;
					}
					return historyGetter.Length;
				}
			}

			// Token: 0x06000A64 RID: 2660 RVA: 0x0002D77B File Offset: 0x0002B97B
			public bool GetHistoryValue(int historyIndex)
			{
				return this.historyGetter[historyIndex]();
			}
		}

		// Token: 0x0200017D RID: 381
		public class IntField : DebugUI.Field<int>
		{
			// Token: 0x06000A66 RID: 2662 RVA: 0x0002D792 File Offset: 0x0002B992
			public override int ValidateValue(int value)
			{
				if (this.min != null)
				{
					value = Mathf.Max(value, this.min());
				}
				if (this.max != null)
				{
					value = Mathf.Min(value, this.max());
				}
				return value;
			}

			// Token: 0x0400063A RID: 1594
			public Func<int> min;

			// Token: 0x0400063B RID: 1595
			public Func<int> max;

			// Token: 0x0400063C RID: 1596
			public int incStep = 1;

			// Token: 0x0400063D RID: 1597
			public int intStepMult = 10;
		}

		// Token: 0x0200017E RID: 382
		public class UIntField : DebugUI.Field<uint>
		{
			// Token: 0x06000A68 RID: 2664 RVA: 0x0002D7E2 File Offset: 0x0002B9E2
			public override uint ValidateValue(uint value)
			{
				if (this.min != null)
				{
					value = (uint)Mathf.Max((int)value, (int)this.min());
				}
				if (this.max != null)
				{
					value = (uint)Mathf.Min((int)value, (int)this.max());
				}
				return value;
			}

			// Token: 0x0400063E RID: 1598
			public Func<uint> min;

			// Token: 0x0400063F RID: 1599
			public Func<uint> max;

			// Token: 0x04000640 RID: 1600
			public uint incStep = 1U;

			// Token: 0x04000641 RID: 1601
			public uint intStepMult = 10U;
		}

		// Token: 0x0200017F RID: 383
		public class FloatField : DebugUI.Field<float>
		{
			// Token: 0x06000A6A RID: 2666 RVA: 0x0002D832 File Offset: 0x0002BA32
			public override float ValidateValue(float value)
			{
				if (this.min != null)
				{
					value = Mathf.Max(value, this.min());
				}
				if (this.max != null)
				{
					value = Mathf.Min(value, this.max());
				}
				return value;
			}

			// Token: 0x04000642 RID: 1602
			public Func<float> min;

			// Token: 0x04000643 RID: 1603
			public Func<float> max;

			// Token: 0x04000644 RID: 1604
			public float incStep = 0.1f;

			// Token: 0x04000645 RID: 1605
			public float incStepMult = 10f;

			// Token: 0x04000646 RID: 1606
			public int decimals = 3;
		}

		// Token: 0x02000180 RID: 384
		public abstract class EnumField<T> : DebugUI.Field<T>
		{
			// Token: 0x1700016B RID: 363
			// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0002D890 File Offset: 0x0002BA90
			// (set) Token: 0x06000A6D RID: 2669 RVA: 0x0002D898 File Offset: 0x0002BA98
			public int[] enumValues
			{
				get
				{
					return this.m_EnumValues;
				}
				set
				{
					int? num = ((value != null) ? new int?(value.Distinct<int>().Count<int>()) : null);
					int? num2 = ((value != null) ? new int?(value.Count<int>()) : null);
					if (!((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null))))
					{
						Debug.LogWarning(base.displayName + " - The values of the enum are duplicated, this might lead to a errors displaying the enum");
					}
					this.m_EnumValues = value;
				}
			}

			// Token: 0x06000A6E RID: 2670 RVA: 0x0002D920 File Offset: 0x0002BB20
			protected void AutoFillFromType(Type enumType)
			{
				if (enumType == null || !enumType.IsEnum)
				{
					throw new ArgumentException("enumType must not be null and it must be an Enum type");
				}
				List<GUIContent> list;
				using (ListPool<GUIContent>.Get(out list))
				{
					List<int> list2;
					using (ListPool<int>.Get(out list2))
					{
						foreach (FieldInfo fieldInfo2 in from fieldInfo in enumType.GetFields(BindingFlags.Static | BindingFlags.Public)
							where !fieldInfo.IsDefined(typeof(ObsoleteAttribute)) && !fieldInfo.IsDefined(typeof(HideInInspector))
							select fieldInfo)
						{
							InspectorNameAttribute customAttribute = fieldInfo2.GetCustomAttribute<InspectorNameAttribute>();
							GUIContent guicontent = new GUIContent((customAttribute == null) ? DebugUI.EnumField<T>.s_NicifyRegEx.Replace(fieldInfo2.Name, "$1 ") : customAttribute.displayName);
							list.Add(guicontent);
							list2.Add((int)Enum.Parse(enumType, fieldInfo2.Name));
						}
						this.enumNames = list.ToArray();
						this.enumValues = list2.ToArray();
					}
				}
			}

			// Token: 0x04000647 RID: 1607
			public GUIContent[] enumNames;

			// Token: 0x04000648 RID: 1608
			private int[] m_EnumValues;

			// Token: 0x04000649 RID: 1609
			private static Regex s_NicifyRegEx = new Regex("([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", RegexOptions.Compiled);
		}

		// Token: 0x02000181 RID: 385
		public class EnumField : DebugUI.EnumField<int>
		{
			// Token: 0x1700016C RID: 364
			// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0002DA7C File Offset: 0x0002BC7C
			internal int[] indexes
			{
				get
				{
					int[] array;
					if ((array = this.m_Indexes) == null)
					{
						int num = 0;
						GUIContent[] enumNames = this.enumNames;
						array = (this.m_Indexes = Enumerable.Range(num, (enumNames != null) ? enumNames.Length : 0).ToArray<int>());
					}
					return array;
				}
			}

			// Token: 0x1700016D RID: 365
			// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0002DAB6 File Offset: 0x0002BCB6
			// (set) Token: 0x06000A73 RID: 2675 RVA: 0x0002DABE File Offset: 0x0002BCBE
			public Func<int> getIndex { get; set; }

			// Token: 0x1700016E RID: 366
			// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0002DAC7 File Offset: 0x0002BCC7
			// (set) Token: 0x06000A75 RID: 2677 RVA: 0x0002DACF File Offset: 0x0002BCCF
			public Action<int> setIndex { get; set; }

			// Token: 0x1700016F RID: 367
			// (get) Token: 0x06000A76 RID: 2678 RVA: 0x0002DAD8 File Offset: 0x0002BCD8
			// (set) Token: 0x06000A77 RID: 2679 RVA: 0x0002DAE5 File Offset: 0x0002BCE5
			public int currentIndex
			{
				get
				{
					return this.getIndex();
				}
				set
				{
					this.setIndex(value);
				}
			}

			// Token: 0x17000170 RID: 368
			// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0002DAF3 File Offset: 0x0002BCF3
			public Type autoEnum
			{
				set
				{
					base.AutoFillFromType(value);
					this.InitQuickSeparators();
				}
			}

			// Token: 0x06000A79 RID: 2681 RVA: 0x0002DB04 File Offset: 0x0002BD04
			internal void InitQuickSeparators()
			{
				IEnumerable<string> enumerable = this.enumNames.Select<GUIContent, string>(delegate(GUIContent x)
				{
					string[] array = x.text.Split('/', StringSplitOptions.None);
					if (array.Length == 1)
					{
						return "";
					}
					return array[0];
				});
				this.quickSeparators = new int[enumerable.Distinct<string>().Count<string>()];
				string text = null;
				int i = 0;
				int num = 0;
				while (i < this.quickSeparators.Length)
				{
					string text2 = enumerable.ElementAt<string>(num);
					while (text == text2)
					{
						text2 = enumerable.ElementAt<string>(++num);
					}
					text = text2;
					this.quickSeparators[i] = num++;
					i++;
				}
			}

			// Token: 0x06000A7A RID: 2682 RVA: 0x0002DB9C File Offset: 0x0002BD9C
			public override void SetValue(int value)
			{
				int num = this.ValidateValue(value);
				int num2 = Array.IndexOf<int>(base.enumValues, num);
				if (this.currentIndex != num2 && !num.Equals(base.getter()))
				{
					base.setter(num);
					Action<DebugUI.Field<int>, int> onValueChanged = this.onValueChanged;
					if (onValueChanged != null)
					{
						onValueChanged(this, num);
					}
					if (num2 > -1)
					{
						this.currentIndex = num2;
					}
				}
			}

			// Token: 0x0400064A RID: 1610
			internal int[] quickSeparators;

			// Token: 0x0400064B RID: 1611
			private int[] m_Indexes;
		}

		// Token: 0x02000182 RID: 386
		public class ObjectPopupField : DebugUI.Field<Object>
		{
			// Token: 0x17000171 RID: 369
			// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0002DC0D File Offset: 0x0002BE0D
			// (set) Token: 0x06000A7D RID: 2685 RVA: 0x0002DC15 File Offset: 0x0002BE15
			public Func<IEnumerable<Object>> getObjects { get; set; }
		}

		// Token: 0x02000183 RID: 387
		public class HistoryEnumField : DebugUI.EnumField
		{
			// Token: 0x17000172 RID: 370
			// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0002DC26 File Offset: 0x0002BE26
			// (set) Token: 0x06000A80 RID: 2688 RVA: 0x0002DC2E File Offset: 0x0002BE2E
			public Func<int>[] historyIndexGetter { get; set; }

			// Token: 0x17000173 RID: 371
			// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0002DC37 File Offset: 0x0002BE37
			public int historyDepth
			{
				get
				{
					Func<int>[] historyIndexGetter = this.historyIndexGetter;
					if (historyIndexGetter == null)
					{
						return 0;
					}
					return historyIndexGetter.Length;
				}
			}

			// Token: 0x06000A82 RID: 2690 RVA: 0x0002DC47 File Offset: 0x0002BE47
			public int GetHistoryValue(int historyIndex)
			{
				return this.historyIndexGetter[historyIndex]();
			}
		}

		// Token: 0x02000184 RID: 388
		public class BitField : DebugUI.EnumField<Enum>
		{
			// Token: 0x17000174 RID: 372
			// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0002DC5E File Offset: 0x0002BE5E
			// (set) Token: 0x06000A85 RID: 2693 RVA: 0x0002DC66 File Offset: 0x0002BE66
			public Type enumType
			{
				get
				{
					return this.m_EnumType;
				}
				set
				{
					this.m_EnumType = value;
					base.AutoFillFromType(value);
				}
			}

			// Token: 0x04000650 RID: 1616
			private Type m_EnumType;
		}

		// Token: 0x02000185 RID: 389
		public class ColorField : DebugUI.Field<Color>
		{
			// Token: 0x06000A87 RID: 2695 RVA: 0x0002DC80 File Offset: 0x0002BE80
			public override Color ValidateValue(Color value)
			{
				if (!this.hdr)
				{
					value.r = Mathf.Clamp01(value.r);
					value.g = Mathf.Clamp01(value.g);
					value.b = Mathf.Clamp01(value.b);
					value.a = Mathf.Clamp01(value.a);
				}
				return value;
			}

			// Token: 0x04000651 RID: 1617
			public bool hdr;

			// Token: 0x04000652 RID: 1618
			public bool showAlpha = true;

			// Token: 0x04000653 RID: 1619
			public bool showPicker = true;

			// Token: 0x04000654 RID: 1620
			public float incStep = 0.025f;

			// Token: 0x04000655 RID: 1621
			public float incStepMult = 5f;

			// Token: 0x04000656 RID: 1622
			public int decimals = 3;
		}

		// Token: 0x02000186 RID: 390
		public class Vector2Field : DebugUI.Field<Vector2>
		{
			// Token: 0x04000657 RID: 1623
			public float incStep = 0.025f;

			// Token: 0x04000658 RID: 1624
			public float incStepMult = 10f;

			// Token: 0x04000659 RID: 1625
			public int decimals = 3;
		}

		// Token: 0x02000187 RID: 391
		public class Vector3Field : DebugUI.Field<Vector3>
		{
			// Token: 0x0400065A RID: 1626
			public float incStep = 0.025f;

			// Token: 0x0400065B RID: 1627
			public float incStepMult = 10f;

			// Token: 0x0400065C RID: 1628
			public int decimals = 3;
		}

		// Token: 0x02000188 RID: 392
		public class Vector4Field : DebugUI.Field<Vector4>
		{
			// Token: 0x0400065D RID: 1629
			public float incStep = 0.025f;

			// Token: 0x0400065E RID: 1630
			public float incStepMult = 10f;

			// Token: 0x0400065F RID: 1631
			public int decimals = 3;
		}

		// Token: 0x02000189 RID: 393
		public class ObjectField : DebugUI.Field<Object>
		{
			// Token: 0x04000660 RID: 1632
			public Type type = typeof(Object);
		}

		// Token: 0x0200018A RID: 394
		public class ObjectListField : DebugUI.Field<Object[]>
		{
			// Token: 0x04000661 RID: 1633
			public Type type = typeof(Object);
		}

		// Token: 0x0200018B RID: 395
		public class MessageBox : DebugUI.Widget
		{
			// Token: 0x04000662 RID: 1634
			public DebugUI.MessageBox.Style style;

			// Token: 0x020001F8 RID: 504
			public enum Style
			{
				// Token: 0x040007DD RID: 2013
				Info,
				// Token: 0x040007DE RID: 2014
				Warning,
				// Token: 0x040007DF RID: 2015
				Error
			}
		}

		// Token: 0x0200018C RID: 396
		public class Panel : DebugUI.IContainer, IComparable<DebugUI.Panel>
		{
			// Token: 0x17000175 RID: 373
			// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0002DDB8 File Offset: 0x0002BFB8
			// (set) Token: 0x06000A90 RID: 2704 RVA: 0x0002DDC0 File Offset: 0x0002BFC0
			public DebugUI.Flags flags { get; set; }

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0002DDC9 File Offset: 0x0002BFC9
			// (set) Token: 0x06000A92 RID: 2706 RVA: 0x0002DDD1 File Offset: 0x0002BFD1
			public string displayName { get; set; }

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x06000A93 RID: 2707 RVA: 0x0002DDDA File Offset: 0x0002BFDA
			// (set) Token: 0x06000A94 RID: 2708 RVA: 0x0002DDE2 File Offset: 0x0002BFE2
			public int groupIndex { get; set; }

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000A95 RID: 2709 RVA: 0x0002DDEB File Offset: 0x0002BFEB
			public string queryPath
			{
				get
				{
					return this.displayName;
				}
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002DDF3 File Offset: 0x0002BFF3
			public bool isEditorOnly
			{
				get
				{
					return (this.flags & DebugUI.Flags.EditorOnly) > DebugUI.Flags.None;
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x06000A97 RID: 2711 RVA: 0x0002DE00 File Offset: 0x0002C000
			public bool isRuntimeOnly
			{
				get
				{
					return (this.flags & DebugUI.Flags.RuntimeOnly) > DebugUI.Flags.None;
				}
			}

			// Token: 0x1700017B RID: 379
			// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0002DE0D File Offset: 0x0002C00D
			public bool isInactiveInEditor
			{
				get
				{
					return this.isRuntimeOnly && !Application.isPlaying;
				}
			}

			// Token: 0x1700017C RID: 380
			// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0002DE21 File Offset: 0x0002C021
			public bool editorForceUpdate
			{
				get
				{
					return (this.flags & DebugUI.Flags.EditorForceUpdate) > DebugUI.Flags.None;
				}
			}

			// Token: 0x1700017D RID: 381
			// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0002DE2E File Offset: 0x0002C02E
			// (set) Token: 0x06000A9B RID: 2715 RVA: 0x0002DE36 File Offset: 0x0002C036
			public ObservableList<DebugUI.Widget> children { get; private set; }

			// Token: 0x1400000B RID: 11
			// (add) Token: 0x06000A9C RID: 2716 RVA: 0x0002DE40 File Offset: 0x0002C040
			// (remove) Token: 0x06000A9D RID: 2717 RVA: 0x0002DE78 File Offset: 0x0002C078
			public event Action<DebugUI.Panel> onSetDirty = delegate
			{
			};

			// Token: 0x06000A9E RID: 2718 RVA: 0x0002DEB0 File Offset: 0x0002C0B0
			public Panel()
			{
				this.children = new ObservableList<DebugUI.Widget>();
				this.children.ItemAdded += this.OnItemAdded;
				this.children.ItemRemoved += this.OnItemRemoved;
			}

			// Token: 0x06000A9F RID: 2719 RVA: 0x0002DF23 File Offset: 0x0002C123
			protected virtual void OnItemAdded(ObservableList<DebugUI.Widget> sender, ListChangedEventArgs<DebugUI.Widget> e)
			{
				if (e.item != null)
				{
					e.item.panel = this;
					e.item.parent = this;
				}
				this.SetDirty();
			}

			// Token: 0x06000AA0 RID: 2720 RVA: 0x0002DF4B File Offset: 0x0002C14B
			protected virtual void OnItemRemoved(ObservableList<DebugUI.Widget> sender, ListChangedEventArgs<DebugUI.Widget> e)
			{
				if (e.item != null)
				{
					e.item.panel = null;
					e.item.parent = null;
				}
				this.SetDirty();
			}

			// Token: 0x06000AA1 RID: 2721 RVA: 0x0002DF74 File Offset: 0x0002C174
			public void SetDirty()
			{
				int count = this.children.Count;
				for (int i = 0; i < count; i++)
				{
					this.children[i].GenerateQueryPath();
				}
				this.onSetDirty(this);
			}

			// Token: 0x06000AA2 RID: 2722 RVA: 0x0002DFB8 File Offset: 0x0002C1B8
			public override int GetHashCode()
			{
				int num = 17;
				num = num * 23 + this.displayName.GetHashCode();
				int count = this.children.Count;
				for (int i = 0; i < count; i++)
				{
					num = num * 23 + this.children[i].GetHashCode();
				}
				return num;
			}

			// Token: 0x06000AA3 RID: 2723 RVA: 0x0002E00C File Offset: 0x0002C20C
			int IComparable<DebugUI.Panel>.CompareTo(DebugUI.Panel other)
			{
				if (other != null)
				{
					return this.groupIndex.CompareTo(other.groupIndex);
				}
				return 1;
			}
		}
	}
}
