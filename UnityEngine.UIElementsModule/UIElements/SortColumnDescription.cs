using System;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x02000103 RID: 259
	[Serializable]
	public class SortColumnDescription
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00022F85 File Offset: 0x00021185
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x00022F90 File Offset: 0x00021190
		public string columnName
		{
			get
			{
				return this.m_ColumnName;
			}
			set
			{
				bool flag = this.m_ColumnName == value;
				if (!flag)
				{
					this.m_ColumnName = value;
					Action<SortColumnDescription> action = this.changed;
					if (action != null)
					{
						action(this);
					}
				}
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00022FCA File Offset: 0x000211CA
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x00022FD4 File Offset: 0x000211D4
		public int columnIndex
		{
			get
			{
				return this.m_ColumnIndex;
			}
			set
			{
				bool flag = this.m_ColumnIndex == value;
				if (!flag)
				{
					this.m_ColumnIndex = value;
					Action<SortColumnDescription> action = this.changed;
					if (action != null)
					{
						action(this);
					}
				}
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0002300B File Offset: 0x0002120B
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x00023013 File Offset: 0x00021213
		public Column column { get; internal set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0002301C File Offset: 0x0002121C
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x00023024 File Offset: 0x00021224
		public SortDirection direction
		{
			get
			{
				return this.m_SortDirection;
			}
			set
			{
				bool flag = this.m_SortDirection == value;
				if (!flag)
				{
					this.m_SortDirection = value;
					Action<SortColumnDescription> action = this.changed;
					if (action != null)
					{
						action(this);
					}
				}
			}
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x060008F8 RID: 2296 RVA: 0x0002305C File Offset: 0x0002125C
		// (remove) Token: 0x060008F9 RID: 2297 RVA: 0x00023094 File Offset: 0x00021294
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<SortColumnDescription> changed;

		// Token: 0x060008FA RID: 2298 RVA: 0x000230C9 File Offset: 0x000212C9
		public SortColumnDescription()
		{
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x000230DA File Offset: 0x000212DA
		public SortColumnDescription(int columnIndex, SortDirection direction)
		{
			this.columnIndex = columnIndex;
			this.direction = direction;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x000230FB File Offset: 0x000212FB
		public SortColumnDescription(string columnName, SortDirection direction)
		{
			this.columnName = columnName;
			this.direction = direction;
		}

		// Token: 0x0400040A RID: 1034
		[SerializeField]
		private int m_ColumnIndex = -1;

		// Token: 0x0400040B RID: 1035
		[SerializeField]
		private string m_ColumnName;

		// Token: 0x0400040C RID: 1036
		[SerializeField]
		private SortDirection m_SortDirection;

		// Token: 0x02000104 RID: 260
		internal class UxmlObjectFactory<T> : UxmlObjectFactory<T, SortColumnDescription.UxmlObjectTraits<T>> where T : SortColumnDescription, new()
		{
		}

		// Token: 0x02000105 RID: 261
		internal class UxmlObjectTraits<T> : UnityEngine.UIElements.UxmlObjectTraits<T> where T : SortColumnDescription
		{
			// Token: 0x060008FE RID: 2302 RVA: 0x00023128 File Offset: 0x00021328
			public override void Init(ref T obj, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ref obj, bag, cc);
				obj.columnName = this.m_ColumnName.GetValueFromBag(bag, cc);
				obj.columnIndex = this.m_ColumnIndex.GetValueFromBag(bag, cc);
				obj.direction = this.m_SortDescription.GetValueFromBag(bag, cc);
			}

			// Token: 0x0400040F RID: 1039
			private readonly UxmlStringAttributeDescription m_ColumnName = new UxmlStringAttributeDescription
			{
				name = "column-name"
			};

			// Token: 0x04000410 RID: 1040
			private readonly UxmlIntAttributeDescription m_ColumnIndex = new UxmlIntAttributeDescription
			{
				name = "column-index",
				defaultValue = -1
			};

			// Token: 0x04000411 RID: 1041
			private readonly UxmlEnumAttributeDescription<SortDirection> m_SortDescription = new UxmlEnumAttributeDescription<SortDirection>
			{
				name = "direction",
				defaultValue = SortDirection.Ascending
			};
		}
	}
}
