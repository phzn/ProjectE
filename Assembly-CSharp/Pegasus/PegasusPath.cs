using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001B9 RID: 441
	[Serializable]
	public class PegasusPath : ScriptableObject
	{
		// Token: 0x06000D18 RID: 3352 RVA: 0x0005A6DC File Offset: 0x000588DC
		public static PegasusPath CreatePegasusPath()
		{
			return ScriptableObject.CreateInstance<PegasusPath>();
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0005A6E4 File Offset: 0x000588E4
		public void AddPoint(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			Vector3 position = go.transform.position;
			Vector3 eulerAngles = go.transform.eulerAngles;
			if (this.m_path.Count > 0)
			{
				PegasusPath.PegasusPoint pegasusPoint = this.m_path[this.m_path.Count - 1];
				if (pegasusPoint.m_location == position && pegasusPoint.m_rotationEuler != eulerAngles)
				{
					return;
				}
			}
			this.m_path.Add(new PegasusPath.PegasusPoint(position, eulerAngles));
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0005A76B File Offset: 0x0005896B
		public void ClearPath()
		{
			this.m_path.Clear();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0005A778 File Offset: 0x00058978
		public void CreatePegasusFromPath()
		{
			if (this.m_path.Count == 0)
			{
				return;
			}
			PegasusManager pegasusManager = new GameObject("Pegasus Manager").AddComponent<PegasusManager>();
			pegasusManager.SetDefaults();
			pegasusManager.m_heightCheckType = PegasusConstants.HeightCheckType.None;
			pegasusManager.m_minHeightAboveTerrain = 0.1f;
			pegasusManager.m_flythroughType = PegasusConstants.FlythroughType.SingleShot;
			for (int i = 0; i < this.m_path.Count; i++)
			{
				PegasusPath.PegasusPoint pegasusPoint = this.m_path[i];
				pegasusManager.AddPOI(pegasusPoint.m_location, pegasusPoint.m_location + Quaternion.Euler(pegasusPoint.m_rotationEuler) * (Vector3.forward * 2f));
			}
			pegasusManager.SetSpeed(this.m_defaultSpeed);
		}

		// Token: 0x04000D72 RID: 3442
		public float m_defaultSpeed = 8f;

		// Token: 0x04000D73 RID: 3443
		public List<PegasusPath.PegasusPoint> m_path = new List<PegasusPath.PegasusPoint>();

		// Token: 0x020001BA RID: 442
		[Serializable]
		public class PegasusPoint
		{
			// Token: 0x06000D1D RID: 3357 RVA: 0x0005A847 File Offset: 0x00058A47
			public PegasusPoint(Vector3 location, Vector3 rotationEuler)
			{
				this.m_location = location;
				this.m_rotationEuler = rotationEuler;
			}

			// Token: 0x04000D74 RID: 3444
			public Vector3 m_location;

			// Token: 0x04000D75 RID: 3445
			public Vector3 m_rotationEuler;

			// Token: 0x04000D76 RID: 3446
			public Vector3 m_dofDistance;
		}
	}
}
