using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro
{
	// Token: 0x0200000C RID: 12
	[RequireComponent(typeof(RectTransform))]
	public class TextContainer : UIBehaviour
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000027B6 File Offset: 0x000009B6
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000027BE File Offset: 0x000009BE
		public bool hasChanged
		{
			get
			{
				return this.m_hasChanged;
			}
			set
			{
				this.m_hasChanged = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000027C7 File Offset: 0x000009C7
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000027CF File Offset: 0x000009CF
		public Vector2 pivot
		{
			get
			{
				return this.m_pivot;
			}
			set
			{
				if (this.m_pivot != value)
				{
					this.m_pivot = value;
					this.m_anchorPosition = this.GetAnchorPosition(this.m_pivot);
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002805 File Offset: 0x00000A05
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000280D File Offset: 0x00000A0D
		public TextContainerAnchors anchorPosition
		{
			get
			{
				return this.m_anchorPosition;
			}
			set
			{
				if (this.m_anchorPosition != value)
				{
					this.m_anchorPosition = value;
					this.m_pivot = this.GetPivot(this.m_anchorPosition);
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000283E File Offset: 0x00000A3E
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002846 File Offset: 0x00000A46
		public Rect rect
		{
			get
			{
				return this.m_rect;
			}
			set
			{
				if (this.m_rect != value)
				{
					this.m_rect = value;
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000036 RID: 54 RVA: 0x0000286A File Offset: 0x00000A6A
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002888 File Offset: 0x00000A88
		public Vector2 size
		{
			get
			{
				return new Vector2(this.m_rect.width, this.m_rect.height);
			}
			set
			{
				if (new Vector2(this.m_rect.width, this.m_rect.height) != value)
				{
					this.SetRect(value);
					this.m_hasChanged = true;
					this.m_isDefaultWidth = false;
					this.m_isDefaultHeight = false;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000028DA File Offset: 0x00000ADA
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000028E7 File Offset: 0x00000AE7
		public float width
		{
			get
			{
				return this.m_rect.width;
			}
			set
			{
				this.SetRect(new Vector2(value, this.m_rect.height));
				this.m_hasChanged = true;
				this.m_isDefaultWidth = false;
				this.OnContainerChanged();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002914 File Offset: 0x00000B14
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002921 File Offset: 0x00000B21
		public float height
		{
			get
			{
				return this.m_rect.height;
			}
			set
			{
				this.SetRect(new Vector2(this.m_rect.width, value));
				this.m_hasChanged = true;
				this.m_isDefaultHeight = false;
				this.OnContainerChanged();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000294E File Offset: 0x00000B4E
		public bool isDefaultWidth
		{
			get
			{
				return this.m_isDefaultWidth;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002956 File Offset: 0x00000B56
		public bool isDefaultHeight
		{
			get
			{
				return this.m_isDefaultHeight;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000295E File Offset: 0x00000B5E
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002966 File Offset: 0x00000B66
		public bool isAutoFitting
		{
			get
			{
				return this.m_isAutoFitting;
			}
			set
			{
				this.m_isAutoFitting = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000040 RID: 64 RVA: 0x0000296F File Offset: 0x00000B6F
		public Vector3[] corners
		{
			get
			{
				return this.m_corners;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002977 File Offset: 0x00000B77
		public Vector3[] worldCorners
		{
			get
			{
				return this.m_worldCorners;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000297F File Offset: 0x00000B7F
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002987 File Offset: 0x00000B87
		public Vector4 margins
		{
			get
			{
				return this.m_margins;
			}
			set
			{
				if (this.m_margins != value)
				{
					this.m_margins = value;
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000029AB File Offset: 0x00000BAB
		public RectTransform rectTransform
		{
			get
			{
				if (this.m_rectTransform == null)
				{
					this.m_rectTransform = base.GetComponent<RectTransform>();
				}
				return this.m_rectTransform;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000029CD File Offset: 0x00000BCD
		public TextMeshPro textMeshPro
		{
			get
			{
				if (this.m_textMeshPro == null)
				{
					this.m_textMeshPro = base.GetComponent<TextMeshPro>();
				}
				return this.m_textMeshPro;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000029EF File Offset: 0x00000BEF
		protected override void Awake()
		{
			Debug.LogWarning("The Text Container component is now Obsolete and can safely be removed from [" + base.gameObject.name + "].", this);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002A11 File Offset: 0x00000C11
		protected override void OnEnable()
		{
			this.OnContainerChanged();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002A19 File Offset: 0x00000C19
		protected override void OnDisable()
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002A1C File Offset: 0x00000C1C
		private void OnContainerChanged()
		{
			this.UpdateCorners();
			if (this.m_rectTransform != null)
			{
				this.m_rectTransform.sizeDelta = this.size;
				this.m_rectTransform.hasChanged = true;
			}
			if (this.textMeshPro != null)
			{
				this.m_textMeshPro.SetVerticesDirty();
				this.m_textMeshPro.margin = this.m_margins;
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002A84 File Offset: 0x00000C84
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.rectTransform == null)
			{
				this.m_rectTransform = base.gameObject.AddComponent<RectTransform>();
			}
			if (this.m_rectTransform.sizeDelta != TextContainer.k_defaultSize)
			{
				this.size = this.m_rectTransform.sizeDelta;
			}
			this.pivot = this.m_rectTransform.pivot;
			this.m_hasChanged = true;
			this.OnContainerChanged();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002AF6 File Offset: 0x00000CF6
		private void SetRect(Vector2 size)
		{
			this.m_rect = new Rect(this.m_rect.x, this.m_rect.y, size.x, size.y);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002B28 File Offset: 0x00000D28
		private void UpdateCorners()
		{
			this.m_corners[0] = new Vector3(-this.m_pivot.x * this.m_rect.width, -this.m_pivot.y * this.m_rect.height);
			this.m_corners[1] = new Vector3(-this.m_pivot.x * this.m_rect.width, (1f - this.m_pivot.y) * this.m_rect.height);
			this.m_corners[2] = new Vector3((1f - this.m_pivot.x) * this.m_rect.width, (1f - this.m_pivot.y) * this.m_rect.height);
			this.m_corners[3] = new Vector3((1f - this.m_pivot.x) * this.m_rect.width, -this.m_pivot.y * this.m_rect.height);
			if (this.m_rectTransform != null)
			{
				this.m_rectTransform.pivot = this.m_pivot;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002C6C File Offset: 0x00000E6C
		private Vector2 GetPivot(TextContainerAnchors anchor)
		{
			Vector2 zero = Vector2.zero;
			switch (anchor)
			{
			case TextContainerAnchors.TopLeft:
				zero = new Vector2(0f, 1f);
				break;
			case TextContainerAnchors.Top:
				zero = new Vector2(0.5f, 1f);
				break;
			case TextContainerAnchors.TopRight:
				zero = new Vector2(1f, 1f);
				break;
			case TextContainerAnchors.Left:
				zero = new Vector2(0f, 0.5f);
				break;
			case TextContainerAnchors.Middle:
				zero = new Vector2(0.5f, 0.5f);
				break;
			case TextContainerAnchors.Right:
				zero = new Vector2(1f, 0.5f);
				break;
			case TextContainerAnchors.BottomLeft:
				zero = new Vector2(0f, 0f);
				break;
			case TextContainerAnchors.Bottom:
				zero = new Vector2(0.5f, 0f);
				break;
			case TextContainerAnchors.BottomRight:
				zero = new Vector2(1f, 0f);
				break;
			}
			return zero;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002D60 File Offset: 0x00000F60
		private TextContainerAnchors GetAnchorPosition(Vector2 pivot)
		{
			if (pivot == new Vector2(0f, 1f))
			{
				return TextContainerAnchors.TopLeft;
			}
			if (pivot == new Vector2(0.5f, 1f))
			{
				return TextContainerAnchors.Top;
			}
			if (pivot == new Vector2(1f, 1f))
			{
				return TextContainerAnchors.TopRight;
			}
			if (pivot == new Vector2(0f, 0.5f))
			{
				return TextContainerAnchors.Left;
			}
			if (pivot == new Vector2(0.5f, 0.5f))
			{
				return TextContainerAnchors.Middle;
			}
			if (pivot == new Vector2(1f, 0.5f))
			{
				return TextContainerAnchors.Right;
			}
			if (pivot == new Vector2(0f, 0f))
			{
				return TextContainerAnchors.BottomLeft;
			}
			if (pivot == new Vector2(0.5f, 0f))
			{
				return TextContainerAnchors.Bottom;
			}
			if (pivot == new Vector2(1f, 0f))
			{
				return TextContainerAnchors.BottomRight;
			}
			return TextContainerAnchors.Custom;
		}

		// Token: 0x04000024 RID: 36
		private bool m_hasChanged;

		// Token: 0x04000025 RID: 37
		[SerializeField]
		private Vector2 m_pivot;

		// Token: 0x04000026 RID: 38
		[SerializeField]
		private TextContainerAnchors m_anchorPosition = TextContainerAnchors.Middle;

		// Token: 0x04000027 RID: 39
		[SerializeField]
		private Rect m_rect;

		// Token: 0x04000028 RID: 40
		private bool m_isDefaultWidth;

		// Token: 0x04000029 RID: 41
		private bool m_isDefaultHeight;

		// Token: 0x0400002A RID: 42
		private bool m_isAutoFitting;

		// Token: 0x0400002B RID: 43
		private Vector3[] m_corners = new Vector3[4];

		// Token: 0x0400002C RID: 44
		private Vector3[] m_worldCorners = new Vector3[4];

		// Token: 0x0400002D RID: 45
		[SerializeField]
		private Vector4 m_margins;

		// Token: 0x0400002E RID: 46
		private RectTransform m_rectTransform;

		// Token: 0x0400002F RID: 47
		private static Vector2 k_defaultSize = new Vector2(100f, 100f);

		// Token: 0x04000030 RID: 48
		private TextMeshPro m_textMeshPro;
	}
}
