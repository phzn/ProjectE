using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public class AstarColor : ISerializationCallbackReceiver
	{
		// Token: 0x06000248 RID: 584 RVA: 0x0000ACE4 File Offset: 0x00008EE4
		public static int ColorHash()
		{
			int num = AstarColor.SolidColor.GetHashCode() ^ AstarColor.UnwalkableNode.GetHashCode() ^ AstarColor.BoundsHandles.GetHashCode() ^ AstarColor.ConnectionLowLerp.GetHashCode() ^ AstarColor.ConnectionHighLerp.GetHashCode() ^ AstarColor.MeshEdgeColor.GetHashCode();
			for (int i = 0; i < AstarColor.AreaColors.Length; i++)
			{
				num = (7 * num) ^ AstarColor.AreaColors[i].GetHashCode();
			}
			return num;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000AD85 File Offset: 0x00008F85
		public static Color GetAreaColor(uint area)
		{
			if ((ulong)area >= (ulong)((long)AstarColor.AreaColors.Length))
			{
				return AstarMath.IntToColor((int)area, 1f);
			}
			return AstarColor.AreaColors[(int)area];
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000AD85 File Offset: 0x00008F85
		public static Color GetTagColor(uint tag)
		{
			if ((ulong)tag >= (ulong)((long)AstarColor.AreaColors.Length))
			{
				return AstarMath.IntToColor((int)tag, 1f);
			}
			return AstarColor.AreaColors[(int)tag];
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000ADAC File Offset: 0x00008FAC
		public void PushToStatic()
		{
			this._AreaColors = this._AreaColors ?? new Color[0];
			AstarColor.SolidColor = this._SolidColor;
			AstarColor.UnwalkableNode = this._UnwalkableNode;
			AstarColor.BoundsHandles = this._BoundsHandles;
			AstarColor.ConnectionLowLerp = this._ConnectionLowLerp;
			AstarColor.ConnectionHighLerp = this._ConnectionHighLerp;
			AstarColor.MeshEdgeColor = this._MeshEdgeColor;
			AstarColor.AreaColors = this._AreaColors;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00003788 File Offset: 0x00001988
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000AE1C File Offset: 0x0000901C
		public void OnAfterDeserialize()
		{
			if (this._AreaColors != null && this._AreaColors.Length == 1 && this._AreaColors[0] == default(Color))
			{
				this._AreaColors = new Color[0];
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000AE64 File Offset: 0x00009064
		public AstarColor()
		{
			this._SolidColor = new Color(0.11764706f, 0.4f, 0.7882353f, 0.9f);
			this._UnwalkableNode = new Color(1f, 0f, 0f, 0.5f);
			this._BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);
			this._ConnectionLowLerp = new Color(0f, 1f, 0f, 0.5f);
			this._ConnectionHighLerp = new Color(1f, 0f, 0f, 0.5f);
			this._MeshEdgeColor = new Color(0f, 0f, 0f, 0.5f);
		}

		// Token: 0x0400013D RID: 317
		public Color _SolidColor;

		// Token: 0x0400013E RID: 318
		public Color _UnwalkableNode;

		// Token: 0x0400013F RID: 319
		public Color _BoundsHandles;

		// Token: 0x04000140 RID: 320
		public Color _ConnectionLowLerp;

		// Token: 0x04000141 RID: 321
		public Color _ConnectionHighLerp;

		// Token: 0x04000142 RID: 322
		public Color _MeshEdgeColor;

		// Token: 0x04000143 RID: 323
		public Color[] _AreaColors;

		// Token: 0x04000144 RID: 324
		public static Color SolidColor = new Color(0.11764706f, 0.4f, 0.7882353f, 0.9f);

		// Token: 0x04000145 RID: 325
		public static Color UnwalkableNode = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x04000146 RID: 326
		public static Color BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);

		// Token: 0x04000147 RID: 327
		public static Color ConnectionLowLerp = new Color(0f, 1f, 0f, 0.5f);

		// Token: 0x04000148 RID: 328
		public static Color ConnectionHighLerp = new Color(1f, 0f, 0f, 0.5f);

		// Token: 0x04000149 RID: 329
		public static Color MeshEdgeColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x0400014A RID: 330
		private static Color[] AreaColors = new Color[1];
	}
}
