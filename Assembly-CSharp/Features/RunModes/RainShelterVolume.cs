using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D4F RID: 3407
	[DisallowMultipleComponent]
	public sealed class RainShelterVolume : MonoBehaviour
	{
		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06006426 RID: 25638 RVA: 0x00282370 File Offset: 0x00280570
		// (remove) Token: 0x06006427 RID: 25639 RVA: 0x002823A4 File Offset: 0x002805A4
		public static event Action Changed;

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06006428 RID: 25640 RVA: 0x002823D7 File Offset: 0x002805D7
		internal static IReadOnlyList<RainShelterVolume> ActiveVolumes
		{
			get
			{
				return RainShelterVolume.Volumes;
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06006429 RID: 25641 RVA: 0x002823DE File Offset: 0x002805DE
		public bool FullyEnclosedPoi
		{
			get
			{
				return this.fullyEnclosedPoi;
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x0600642A RID: 25642 RVA: 0x002823E6 File Offset: 0x002805E6
		public Texture2D RoofHeightMap
		{
			get
			{
				return this.roofHeightMap;
			}
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x0600642B RID: 25643 RVA: 0x002823EE File Offset: 0x002805EE
		public static bool HasClosedPoiVolumes
		{
			get
			{
				return RainShelterVolume.ClosedPoiVolumes.Count > 0;
			}
		}

		// Token: 0x0600642C RID: 25644 RVA: 0x002823FD File Offset: 0x002805FD
		public void ConfigureRoofHeightMap(Texture2D map)
		{
			this.roofHeightMap = map;
			Action changed = RainShelterVolume.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x0600642D RID: 25645 RVA: 0x00282418 File Offset: 0x00280618
		public void SetFullyEnclosedPoi(bool value)
		{
			this.fullyEnclosedPoi = value;
			if (value && RainShelterVolume.Volumes.Contains(this))
			{
				if (!RainShelterVolume.ClosedPoiVolumes.Contains(this))
				{
					RainShelterVolume.ClosedPoiVolumes.Add(this);
				}
			}
			else
			{
				RainShelterVolume.ClosedPoiVolumes.Remove(this);
			}
			Action changed = RainShelterVolume.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x0600642E RID: 25646 RVA: 0x00282474 File Offset: 0x00280674
		public static bool ContainsClosedPoi(Vector3 worldPosition)
		{
			for (int i = 0; i < RainShelterVolume.ClosedPoiVolumes.Count; i++)
			{
				if (RainShelterVolume.ClosedPoiVolumes[i] != null && RainShelterVolume.ClosedPoiVolumes[i].Contains(worldPosition))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600642F RID: 25647 RVA: 0x002824C0 File Offset: 0x002806C0
		public void Configure(Vector3 localCenter, Vector3 localSize)
		{
			this.center = localCenter;
			this.size = new Vector3(Mathf.Max(0.01f, localSize.x), Mathf.Max(0.01f, localSize.y), Mathf.Max(0.01f, localSize.z));
			Action changed = RainShelterVolume.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06006430 RID: 25648 RVA: 0x00282520 File Offset: 0x00280720
		public bool Contains(Vector3 worldPosition)
		{
			Vector3 vector = base.transform.InverseTransformPoint(worldPosition);
			if (!new Bounds(this.center, this.size).Contains(vector))
			{
				return false;
			}
			if (this.roofHeightMap == null)
			{
				return true;
			}
			Vector3 vector2 = new Vector3((vector.x - this.center.x) / this.size.x + 0.5f, (vector.y - this.center.y) / this.size.y + 0.5f, (vector.z - this.center.z) / this.size.z + 0.5f);
			int num = Mathf.Clamp(Mathf.FloorToInt(vector2.x * (float)this.roofHeightMap.width), 0, this.roofHeightMap.width - 1);
			int num2 = Mathf.Clamp(Mathf.FloorToInt(vector2.z * (float)this.roofHeightMap.height), 0, this.roofHeightMap.height - 1);
			Color pixel = this.roofHeightMap.GetPixel(num, num2);
			return pixel.g > 0.5f && vector2.y <= pixel.r;
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06006431 RID: 25649 RVA: 0x00282668 File Offset: 0x00280868
		public Matrix4x4 WorldToUnitBox
		{
			get
			{
				return Matrix4x4.TRS(this.center, Quaternion.identity, this.size).inverse * base.transform.worldToLocalMatrix;
			}
		}

		// Token: 0x06006432 RID: 25650 RVA: 0x002826A4 File Offset: 0x002808A4
		public float SqrDistance(Vector3 worldPosition)
		{
			Vector3 vector = base.transform.InverseTransformPoint(worldPosition);
			Vector3 vector2 = new Bounds(this.center, this.size).ClosestPoint(vector);
			return (base.transform.TransformPoint(vector2) - worldPosition).sqrMagnitude;
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x002826F4 File Offset: 0x002808F4
		public static bool ContainsAny(Vector3 worldPosition)
		{
			for (int i = 0; i < RainShelterVolume.Volumes.Count; i++)
			{
				if (RainShelterVolume.Volumes[i] != null && RainShelterVolume.Volumes[i].Contains(worldPosition))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006434 RID: 25652 RVA: 0x00282740 File Offset: 0x00280940
		private void OnEnable()
		{
			if (!RainShelterVolume.Volumes.Contains(this))
			{
				RainShelterVolume.Volumes.Add(this);
			}
			if (this.fullyEnclosedPoi && !RainShelterVolume.ClosedPoiVolumes.Contains(this))
			{
				RainShelterVolume.ClosedPoiVolumes.Add(this);
			}
			Action changed = RainShelterVolume.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06006435 RID: 25653 RVA: 0x00282794 File Offset: 0x00280994
		private void OnDisable()
		{
			RainShelterVolume.Volumes.Remove(this);
			RainShelterVolume.ClosedPoiVolumes.Remove(this);
			Action changed = RainShelterVolume.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06006436 RID: 25654 RVA: 0x002827BD File Offset: 0x002809BD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRegistry()
		{
			RainShelterVolume.Volumes.Clear();
			RainShelterVolume.ClosedPoiVolumes.Clear();
			RainShelterVolume.Changed = null;
		}

		// Token: 0x06006437 RID: 25655 RVA: 0x002827DC File Offset: 0x002809DC
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0.2f, 0.65f, 1f, 0.35f);
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(this.center, this.size);
		}

		// Token: 0x04006726 RID: 26406
		[SerializeField]
		private Vector3 center;

		// Token: 0x04006727 RID: 26407
		[SerializeField]
		private Vector3 size = Vector3.one;

		// Token: 0x04006728 RID: 26408
		[Tooltip("Only explicit inline POI interiors (Kombini/Subway). Stops weather while the player is inside; never installs a city shader mask.")]
		[SerializeField]
		private bool fullyEnclosedPoi;

		// Token: 0x04006729 RID: 26409
		[Tooltip("Optional baked Stadium roof footprint: linear readable point texture, R=normalized ceiling height, G=coverage.")]
		[SerializeField]
		private Texture2D roofHeightMap;

		// Token: 0x0400672A RID: 26410
		private static readonly List<RainShelterVolume> Volumes = new List<RainShelterVolume>();

		// Token: 0x0400672B RID: 26411
		private static readonly List<RainShelterVolume> ClosedPoiVolumes = new List<RainShelterVolume>();
	}
}
