using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	public struct FRect
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00003C71 File Offset: 0x00001E71
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00003C79 File Offset: 0x00001E79
		public Vector2 position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00003C82 File Offset: 0x00001E82
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00003C8A File Offset: 0x00001E8A
		public Vector2 size
		{
			get
			{
				return this._size;
			}
			set
			{
				this._size = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00003C93 File Offset: 0x00001E93
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00003C9B File Offset: 0x00001E9B
		public float angle
		{
			get
			{
				return this._angle;
			}
			set
			{
				this._angle = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00003CA4 File Offset: 0x00001EA4
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00003CAC File Offset: 0x00001EAC
		public float absoluteAngle
		{
			get
			{
				return this._absoluteAngle;
			}
			set
			{
				this._absoluteAngle = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00003CB5 File Offset: 0x00001EB5
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00003CBD File Offset: 0x00001EBD
		public RectOffsetCustom padding
		{
			get
			{
				return this._padding;
			}
			set
			{
				this._padding = value;
			}
		}

		// Token: 0x04000340 RID: 832
		[SerializeField]
		private Vector2 _position;

		// Token: 0x04000341 RID: 833
		[SerializeField]
		private Vector2 _size;

		// Token: 0x04000342 RID: 834
		[SerializeField]
		private RectOffsetCustom _padding;

		// Token: 0x04000343 RID: 835
		[SerializeField]
		private float _angle;

		// Token: 0x04000344 RID: 836
		[SerializeField]
		private float _absoluteAngle;
	}
}
