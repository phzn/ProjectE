using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x020000F6 RID: 246
	public class Columns : ICollection<Column>, IEnumerable<Column>, IEnumerable
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x000210CB File Offset: 0x0001F2CB
		internal IList<Column> columns
		{
			get
			{
				return this.m_Columns;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x000210D3 File Offset: 0x0001F2D3
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x000210DC File Offset: 0x0001F2DC
		public string primaryColumnName
		{
			get
			{
				return this.m_PrimaryColumnName;
			}
			set
			{
				bool flag = this.m_PrimaryColumnName == value;
				if (!flag)
				{
					this.m_PrimaryColumnName = value;
					this.NotifyChange(ColumnsDataType.PrimaryColumn);
				}
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0002110B File Offset: 0x0001F30B
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x00021114 File Offset: 0x0001F314
		public bool reorderable
		{
			get
			{
				return this.m_Reorderable;
			}
			set
			{
				bool flag = this.m_Reorderable == value;
				if (!flag)
				{
					this.m_Reorderable = value;
					this.NotifyChange(ColumnsDataType.Reorderable);
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x00021140 File Offset: 0x0001F340
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x00021148 File Offset: 0x0001F348
		public bool resizable
		{
			get
			{
				return this.m_Resizable;
			}
			set
			{
				bool flag = this.m_Resizable == value;
				if (!flag)
				{
					this.m_Resizable = value;
					this.NotifyChange(ColumnsDataType.Resizable);
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00021174 File Offset: 0x0001F374
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x0002117C File Offset: 0x0001F37C
		public bool resizePreview
		{
			get
			{
				return this.m_ResizePreview;
			}
			set
			{
				bool flag = this.m_ResizePreview == value;
				if (!flag)
				{
					this.m_ResizePreview = value;
					this.NotifyChange(ColumnsDataType.ResizePreview);
				}
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x000211A8 File Offset: 0x0001F3A8
		internal IEnumerable<Column> displayList
		{
			get
			{
				this.InitOrderColumns();
				return this.m_DisplayColumns;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x000211C8 File Offset: 0x0001F3C8
		internal IEnumerable<Column> visibleList
		{
			get
			{
				this.UpdateVisibleColumns();
				return this.m_VisibleColumns;
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000874 RID: 2164 RVA: 0x000211E8 File Offset: 0x0001F3E8
		// (remove) Token: 0x06000875 RID: 2165 RVA: 0x00021220 File Offset: 0x0001F420
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<ColumnsDataType> changed;

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x00021255 File Offset: 0x0001F455
		// (set) Token: 0x06000877 RID: 2167 RVA: 0x00021260 File Offset: 0x0001F460
		public Columns.StretchMode stretchMode
		{
			get
			{
				return this.m_StretchMode;
			}
			set
			{
				bool flag = this.m_StretchMode == value;
				if (!flag)
				{
					this.m_StretchMode = value;
					this.NotifyChange(ColumnsDataType.StretchMode);
				}
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000878 RID: 2168 RVA: 0x0002128C File Offset: 0x0001F48C
		// (remove) Token: 0x06000879 RID: 2169 RVA: 0x000212C4 File Offset: 0x0001F4C4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<Column, int> columnAdded;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x0600087A RID: 2170 RVA: 0x000212FC File Offset: 0x0001F4FC
		// (remove) Token: 0x0600087B RID: 2171 RVA: 0x00021334 File Offset: 0x0001F534
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<Column> columnRemoved;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x0600087C RID: 2172 RVA: 0x0002136C File Offset: 0x0001F56C
		// (remove) Token: 0x0600087D RID: 2173 RVA: 0x000213A4 File Offset: 0x0001F5A4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<Column, ColumnDataType> columnChanged;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x0600087E RID: 2174 RVA: 0x000213DC File Offset: 0x0001F5DC
		// (remove) Token: 0x0600087F RID: 2175 RVA: 0x00021414 File Offset: 0x0001F614
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<Column> columnResized;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000880 RID: 2176 RVA: 0x0002144C File Offset: 0x0001F64C
		// (remove) Token: 0x06000881 RID: 2177 RVA: 0x00021484 File Offset: 0x0001F684
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<Column, int, int> columnReordered;

		// Token: 0x06000882 RID: 2178 RVA: 0x000214BC File Offset: 0x0001F6BC
		public bool IsPrimary(Column column)
		{
			return this.primaryColumnName == column.name || (string.IsNullOrEmpty(this.primaryColumnName) && column.visibleIndex == 0);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00021500 File Offset: 0x0001F700
		public IEnumerator<Column> GetEnumerator()
		{
			return this.m_Columns.GetEnumerator();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00021520 File Offset: 0x0001F720
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00021538 File Offset: 0x0001F738
		public void Add(Column item)
		{
			this.Insert(this.m_Columns.Count, item);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00021550 File Offset: 0x0001F750
		public void Clear()
		{
			while (this.m_Columns.Count > 0)
			{
				this.Remove(this.m_Columns[this.m_Columns.Count - 1]);
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00021594 File Offset: 0x0001F794
		public bool Contains(Column item)
		{
			return this.m_Columns.Contains(item);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000215B4 File Offset: 0x0001F7B4
		public bool Contains(string name)
		{
			foreach (Column column in this.m_Columns)
			{
				bool flag = column.name == name;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00021618 File Offset: 0x0001F818
		public void CopyTo(Column[] array, int arrayIndex)
		{
			this.m_Columns.CopyTo(array, arrayIndex);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0002162C File Offset: 0x0001F82C
		public bool Remove(Column column)
		{
			bool flag = column == null;
			if (flag)
			{
				throw new ArgumentException("Cannot remove null column");
			}
			bool flag2 = this.m_Columns.Remove(column);
			bool flag3;
			if (flag2)
			{
				List<Column> displayColumns = this.m_DisplayColumns;
				if (displayColumns != null)
				{
					displayColumns.Remove(column);
				}
				List<Column> visibleColumns = this.m_VisibleColumns;
				if (visibleColumns != null)
				{
					visibleColumns.Remove(column);
				}
				column.collection = null;
				column.changed -= this.OnColumnChanged;
				column.resized -= this.OnColumnResized;
				Action<Column> action = this.columnRemoved;
				if (action != null)
				{
					action(column);
				}
				flag3 = true;
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x000216D0 File Offset: 0x0001F8D0
		private void OnColumnChanged(Column column, ColumnDataType type)
		{
			bool flag = type == ColumnDataType.Visibility;
			if (flag)
			{
				this.DirtyVisibleColumns();
			}
			Action<Column, ColumnDataType> action = this.columnChanged;
			if (action != null)
			{
				action(column, type);
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00021701 File Offset: 0x0001F901
		private void OnColumnResized(Column column)
		{
			Action<Column> action = this.columnResized;
			if (action != null)
			{
				action(column);
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x00021717 File Offset: 0x0001F917
		public int Count
		{
			get
			{
				return this.m_Columns.Count;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00021724 File Offset: 0x0001F924
		public bool IsReadOnly
		{
			get
			{
				return this.m_Columns.IsReadOnly;
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00021734 File Offset: 0x0001F934
		public int IndexOf(Column column)
		{
			return this.m_Columns.IndexOf(column);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00021754 File Offset: 0x0001F954
		public void Insert(int index, Column column)
		{
			bool flag = column == null;
			if (flag)
			{
				throw new ArgumentException("Cannot insert null column");
			}
			bool flag2 = column.collection == this;
			if (flag2)
			{
				throw new ArgumentException("Already contains this column");
			}
			bool flag3 = column.collection != null;
			if (flag3)
			{
				column.collection.Remove(column);
			}
			this.m_Columns.Insert(index, column);
			bool flag4 = this.m_DisplayColumns != null;
			if (flag4)
			{
				this.m_DisplayColumns.Insert(index, column);
				this.DirtyVisibleColumns();
			}
			column.collection = this;
			column.changed += this.OnColumnChanged;
			column.resized += this.OnColumnResized;
			Action<Column, int> action = this.columnAdded;
			if (action != null)
			{
				action(column, index);
			}
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0002181B File Offset: 0x0001FA1B
		public void RemoveAt(int index)
		{
			this.Remove(this.m_Columns[index]);
		}

		// Token: 0x1700019F RID: 415
		public Column this[int index]
		{
			get
			{
				return this.m_Columns[index];
			}
		}

		// Token: 0x170001A0 RID: 416
		public Column this[string name]
		{
			get
			{
				foreach (Column column in this.m_Columns)
				{
					bool flag = column.name == name;
					if (flag)
					{
						return column;
					}
				}
				return null;
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x000218B8 File Offset: 0x0001FAB8
		public void ReorderDisplay(int from, int to)
		{
			this.InitOrderColumns();
			Column column = this.m_DisplayColumns[from];
			this.m_DisplayColumns.RemoveAt(from);
			this.m_DisplayColumns.Insert(to, column);
			this.DirtyVisibleColumns();
			Action<Column, int, int> action = this.columnReordered;
			if (action != null)
			{
				action(column, from, to);
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00021914 File Offset: 0x0001FB14
		private void InitOrderColumns()
		{
			bool flag = this.m_DisplayColumns == null;
			if (flag)
			{
				this.m_DisplayColumns = new List<Column>(this);
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00021940 File Offset: 0x0001FB40
		private void DirtyVisibleColumns()
		{
			this.m_VisibleColumnsDirty = true;
			bool flag = this.m_VisibleColumns != null;
			if (flag)
			{
				this.m_VisibleColumns.Clear();
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00021970 File Offset: 0x0001FB70
		private void UpdateVisibleColumns()
		{
			bool flag = !this.m_VisibleColumnsDirty;
			if (!flag)
			{
				this.InitOrderColumns();
				bool flag2 = this.m_VisibleColumns == null;
				if (flag2)
				{
					this.m_VisibleColumns = new List<Column>(this.m_Columns.Count);
				}
				this.m_VisibleColumns.AddRange(this.m_DisplayColumns.FindAll((Column c) => c.visible));
				this.m_VisibleColumnsDirty = false;
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x000219F6 File Offset: 0x0001FBF6
		private void NotifyChange(ColumnsDataType type)
		{
			Action<ColumnsDataType> action = this.changed;
			if (action != null)
			{
				action(type);
			}
		}

		// Token: 0x040003CD RID: 973
		private IList<Column> m_Columns = new List<Column>();

		// Token: 0x040003CE RID: 974
		private List<Column> m_DisplayColumns;

		// Token: 0x040003CF RID: 975
		private List<Column> m_VisibleColumns;

		// Token: 0x040003D0 RID: 976
		private bool m_VisibleColumnsDirty = true;

		// Token: 0x040003D1 RID: 977
		private Columns.StretchMode m_StretchMode = Columns.StretchMode.GrowAndFill;

		// Token: 0x040003D2 RID: 978
		private bool m_Reorderable = true;

		// Token: 0x040003D3 RID: 979
		private bool m_Resizable = true;

		// Token: 0x040003D4 RID: 980
		private bool m_ResizePreview;

		// Token: 0x040003D5 RID: 981
		private string m_PrimaryColumnName;

		// Token: 0x020000F7 RID: 247
		public enum StretchMode
		{
			// Token: 0x040003DD RID: 989
			Grow,
			// Token: 0x040003DE RID: 990
			GrowAndFill
		}

		// Token: 0x020000F8 RID: 248
		internal class UxmlObjectFactory<T> : UxmlObjectFactory<T, Columns.UxmlObjectTraits<T>> where T : Columns, new()
		{
		}

		// Token: 0x020000F9 RID: 249
		internal class UxmlObjectTraits<T> : UnityEngine.UIElements.UxmlObjectTraits<T> where T : Columns
		{
			// Token: 0x0600089B RID: 2203 RVA: 0x00021A48 File Offset: 0x0001FC48
			public override void Init(ref T obj, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ref obj, bag, cc);
				obj.primaryColumnName = this.m_PrimaryColumnName.GetValueFromBag(bag, cc);
				obj.stretchMode = this.m_StretchMode.GetValueFromBag(bag, cc);
				obj.reorderable = this.m_Reorderable.GetValueFromBag(bag, cc);
				obj.resizable = this.m_Resizable.GetValueFromBag(bag, cc);
				obj.resizePreview = this.m_ResizePreview.GetValueFromBag(bag, cc);
				List<Column> valueFromBag = this.m_Columns.GetValueFromBag(bag, cc);
				bool flag = valueFromBag != null;
				if (flag)
				{
					foreach (Column column in valueFromBag)
					{
						obj.Add(column);
					}
				}
			}

			// Token: 0x040003DF RID: 991
			private readonly UxmlStringAttributeDescription m_PrimaryColumnName = new UxmlStringAttributeDescription
			{
				name = "primary-column-name"
			};

			// Token: 0x040003E0 RID: 992
			private readonly UxmlEnumAttributeDescription<Columns.StretchMode> m_StretchMode = new UxmlEnumAttributeDescription<Columns.StretchMode>
			{
				name = "stretch-mode",
				defaultValue = Columns.StretchMode.GrowAndFill
			};

			// Token: 0x040003E1 RID: 993
			private readonly UxmlBoolAttributeDescription m_Reorderable = new UxmlBoolAttributeDescription
			{
				name = "reorderable",
				defaultValue = true
			};

			// Token: 0x040003E2 RID: 994
			private readonly UxmlBoolAttributeDescription m_Resizable = new UxmlBoolAttributeDescription
			{
				name = "resizable",
				defaultValue = true
			};

			// Token: 0x040003E3 RID: 995
			private readonly UxmlBoolAttributeDescription m_ResizePreview = new UxmlBoolAttributeDescription
			{
				name = "resize-preview"
			};

			// Token: 0x040003E4 RID: 996
			private readonly UxmlObjectListAttributeDescription<Column> m_Columns = new UxmlObjectListAttributeDescription<Column>();
		}
	}
}
