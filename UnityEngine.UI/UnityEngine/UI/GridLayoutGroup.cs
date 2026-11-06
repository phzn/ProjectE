using System;

namespace UnityEngine.UI
{
	// Token: 0x0200001D RID: 29
	[AddComponentMenu("Layout/Grid Layout Group", 152)]
	public class GridLayoutGroup : LayoutGroup
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000D953 File Offset: 0x0000BB53
		// (set) Token: 0x06000245 RID: 581 RVA: 0x0000D95B File Offset: 0x0000BB5B
		public GridLayoutGroup.Corner startCorner
		{
			get
			{
				return this.m_StartCorner;
			}
			set
			{
				base.SetProperty<GridLayoutGroup.Corner>(ref this.m_StartCorner, value);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000D96A File Offset: 0x0000BB6A
		// (set) Token: 0x06000247 RID: 583 RVA: 0x0000D972 File Offset: 0x0000BB72
		public GridLayoutGroup.Axis startAxis
		{
			get
			{
				return this.m_StartAxis;
			}
			set
			{
				base.SetProperty<GridLayoutGroup.Axis>(ref this.m_StartAxis, value);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000D981 File Offset: 0x0000BB81
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000D989 File Offset: 0x0000BB89
		public Vector2 cellSize
		{
			get
			{
				return this.m_CellSize;
			}
			set
			{
				base.SetProperty<Vector2>(ref this.m_CellSize, value);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000D998 File Offset: 0x0000BB98
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000D9A0 File Offset: 0x0000BBA0
		public Vector2 spacing
		{
			get
			{
				return this.m_Spacing;
			}
			set
			{
				base.SetProperty<Vector2>(ref this.m_Spacing, value);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000D9AF File Offset: 0x0000BBAF
		// (set) Token: 0x0600024D RID: 589 RVA: 0x0000D9B7 File Offset: 0x0000BBB7
		public GridLayoutGroup.Constraint constraint
		{
			get
			{
				return this.m_Constraint;
			}
			set
			{
				base.SetProperty<GridLayoutGroup.Constraint>(ref this.m_Constraint, value);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000D9C6 File Offset: 0x0000BBC6
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0000D9CE File Offset: 0x0000BBCE
		public int constraintCount
		{
			get
			{
				return this.m_ConstraintCount;
			}
			set
			{
				base.SetProperty<int>(ref this.m_ConstraintCount, Mathf.Max(1, value));
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000D9E3 File Offset: 0x0000BBE3
		protected GridLayoutGroup()
		{
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000DA14 File Offset: 0x0000BC14
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			int num2;
			int num;
			if (this.m_Constraint == GridLayoutGroup.Constraint.FixedColumnCount)
			{
				num = (num2 = this.m_ConstraintCount);
			}
			else if (this.m_Constraint == GridLayoutGroup.Constraint.FixedRowCount)
			{
				num = (num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)this.m_ConstraintCount - 0.001f));
			}
			else
			{
				num2 = 1;
				num = Mathf.CeilToInt(Mathf.Sqrt((float)base.rectChildren.Count));
			}
			base.SetLayoutInputForAxis((float)base.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float)num2 - this.spacing.x, (float)base.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float)num - this.spacing.x, -1f, 0);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
		public override void CalculateLayoutInputVertical()
		{
			int num;
			if (this.m_Constraint == GridLayoutGroup.Constraint.FixedColumnCount)
			{
				num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)this.m_ConstraintCount - 0.001f);
			}
			else if (this.m_Constraint == GridLayoutGroup.Constraint.FixedRowCount)
			{
				num = this.m_ConstraintCount;
			}
			else
			{
				float width = base.rectTransform.rect.width;
				int num2 = Mathf.Max(1, Mathf.FloorToInt((width - (float)base.padding.horizontal + this.spacing.x + 0.001f) / (this.cellSize.x + this.spacing.x)));
				num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num2);
			}
			float num3 = (float)base.padding.vertical + (this.cellSize.y + this.spacing.y) * (float)num - this.spacing.y;
			base.SetLayoutInputForAxis(num3, num3, -1f, 1);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000DBF7 File Offset: 0x0000BDF7
		public override void SetLayoutHorizontal()
		{
			this.SetCellsAlongAxis(0);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000DC00 File Offset: 0x0000BE00
		public override void SetLayoutVertical()
		{
			this.SetCellsAlongAxis(1);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000DC0C File Offset: 0x0000BE0C
		private void SetCellsAlongAxis(int axis)
		{
			int count = base.rectChildren.Count;
			if (axis == 0)
			{
				for (int i = 0; i < count; i++)
				{
					RectTransform rectTransform = base.rectChildren[i];
					this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
					rectTransform.anchorMin = Vector2.up;
					rectTransform.anchorMax = Vector2.up;
					rectTransform.sizeDelta = this.cellSize;
				}
				return;
			}
			float x = base.rectTransform.rect.size.x;
			float y = base.rectTransform.rect.size.y;
			int num = 1;
			int num2 = 1;
			if (this.m_Constraint == GridLayoutGroup.Constraint.FixedColumnCount)
			{
				num = this.m_ConstraintCount;
				if (count > num)
				{
					num2 = count / num + ((count % num > 0) ? 1 : 0);
				}
			}
			else if (this.m_Constraint == GridLayoutGroup.Constraint.FixedRowCount)
			{
				num2 = this.m_ConstraintCount;
				if (count > num2)
				{
					num = count / num2 + ((count % num2 > 0) ? 1 : 0);
				}
			}
			else
			{
				if (this.cellSize.x + this.spacing.x <= 0f)
				{
					num = int.MaxValue;
				}
				else
				{
					num = Mathf.Max(1, Mathf.FloorToInt((x - (float)base.padding.horizontal + this.spacing.x + 0.001f) / (this.cellSize.x + this.spacing.x)));
				}
				if (this.cellSize.y + this.spacing.y <= 0f)
				{
					num2 = int.MaxValue;
				}
				else
				{
					num2 = Mathf.Max(1, Mathf.FloorToInt((y - (float)base.padding.vertical + this.spacing.y + 0.001f) / (this.cellSize.y + this.spacing.y)));
				}
			}
			int num3 = (int)(this.startCorner % GridLayoutGroup.Corner.LowerLeft);
			int num4 = (int)(this.startCorner / GridLayoutGroup.Corner.LowerLeft);
			int num5;
			int num6;
			int num7;
			if (this.startAxis == GridLayoutGroup.Axis.Horizontal)
			{
				num5 = num;
				num6 = Mathf.Clamp(num, 1, count);
				if (this.m_Constraint == GridLayoutGroup.Constraint.FixedRowCount)
				{
					num7 = Mathf.Min(num2, count);
				}
				else
				{
					num7 = Mathf.Clamp(num2, 1, Mathf.CeilToInt((float)count / (float)num5));
				}
			}
			else
			{
				num5 = num2;
				num7 = Mathf.Clamp(num2, 1, count);
				if (this.m_Constraint == GridLayoutGroup.Constraint.FixedColumnCount)
				{
					num6 = Mathf.Min(num, count);
				}
				else
				{
					num6 = Mathf.Clamp(num, 1, Mathf.CeilToInt((float)count / (float)num5));
				}
			}
			Vector2 vector = new Vector2((float)num6 * this.cellSize.x + (float)(num6 - 1) * this.spacing.x, (float)num7 * this.cellSize.y + (float)(num7 - 1) * this.spacing.y);
			Vector2 vector2 = new Vector2(base.GetStartOffset(0, vector.x), base.GetStartOffset(1, vector.y));
			int num8 = 0;
			if (count > this.m_ConstraintCount && Mathf.CeilToInt((float)count / (float)num5) < this.m_ConstraintCount)
			{
				num8 = this.m_ConstraintCount - Mathf.CeilToInt((float)count / (float)num5);
				num8 += Mathf.FloorToInt((float)num8 / ((float)num5 - 1f));
				if (count % num5 == 1)
				{
					num8++;
				}
			}
			for (int j = 0; j < count; j++)
			{
				int num9;
				int num10;
				if (this.startAxis == GridLayoutGroup.Axis.Horizontal)
				{
					if (this.m_Constraint == GridLayoutGroup.Constraint.FixedRowCount && count - j <= num8)
					{
						num9 = 0;
						num10 = this.m_ConstraintCount - (count - j);
					}
					else
					{
						num9 = j % num5;
						num10 = j / num5;
					}
				}
				else if (this.m_Constraint == GridLayoutGroup.Constraint.FixedColumnCount && count - j <= num8)
				{
					num9 = this.m_ConstraintCount - (count - j);
					num10 = 0;
				}
				else
				{
					num9 = j / num5;
					num10 = j % num5;
				}
				if (num3 == 1)
				{
					num9 = num6 - 1 - num9;
				}
				if (num4 == 1)
				{
					num10 = num7 - 1 - num10;
				}
				base.SetChildAlongAxis(base.rectChildren[j], 0, vector2.x + (this.cellSize[0] + this.spacing[0]) * (float)num9, this.cellSize[0]);
				base.SetChildAlongAxis(base.rectChildren[j], 1, vector2.y + (this.cellSize[1] + this.spacing[1]) * (float)num10, this.cellSize[1]);
			}
		}

		// Token: 0x040000D5 RID: 213
		[SerializeField]
		protected GridLayoutGroup.Corner m_StartCorner;

		// Token: 0x040000D6 RID: 214
		[SerializeField]
		protected GridLayoutGroup.Axis m_StartAxis;

		// Token: 0x040000D7 RID: 215
		[SerializeField]
		protected Vector2 m_CellSize = new Vector2(100f, 100f);

		// Token: 0x040000D8 RID: 216
		[SerializeField]
		protected Vector2 m_Spacing = Vector2.zero;

		// Token: 0x040000D9 RID: 217
		[SerializeField]
		protected GridLayoutGroup.Constraint m_Constraint;

		// Token: 0x040000DA RID: 218
		[SerializeField]
		protected int m_ConstraintCount = 2;

		// Token: 0x0200009D RID: 157
		public enum Corner
		{
			// Token: 0x040002CB RID: 715
			UpperLeft,
			// Token: 0x040002CC RID: 716
			UpperRight,
			// Token: 0x040002CD RID: 717
			LowerLeft,
			// Token: 0x040002CE RID: 718
			LowerRight
		}

		// Token: 0x0200009E RID: 158
		public enum Axis
		{
			// Token: 0x040002D0 RID: 720
			Horizontal,
			// Token: 0x040002D1 RID: 721
			Vertical
		}

		// Token: 0x0200009F RID: 159
		public enum Constraint
		{
			// Token: 0x040002D3 RID: 723
			Flexible,
			// Token: 0x040002D4 RID: 724
			FixedColumnCount,
			// Token: 0x040002D5 RID: 725
			FixedRowCount
		}
	}
}
