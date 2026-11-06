using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009A4 RID: 2468
	internal static class MiniMapPoiDiscoveryPolicy
	{
		// Token: 0x060044DC RID: 17628 RVA: 0x0019C595 File Offset: 0x0019A795
		internal static bool RequiresInteriorScene(MiniMapPoiKind kind)
		{
			return kind == MiniMapPoiKind.University || kind == MiniMapPoiKind.Stadium || kind == MiniMapPoiKind.Mall || kind == MiniMapPoiKind.Gallery || kind == MiniMapPoiKind.Pool;
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x0019C5AD File Offset: 0x0019A7AD
		internal static bool CanDiscoverInWorld(MiniMapPoiKind kind)
		{
			return kind == MiniMapPoiKind.Park || kind == MiniMapPoiKind.Metro || kind == MiniMapPoiKind.Kombini;
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x0019C5BE File Offset: 0x0019A7BE
		internal static MiniMapPoiDiscoveryMode ResolveMode(MiniMapPoiKind kind, BoxCollider[] volumes)
		{
			if (MiniMapPoiDiscoveryPolicy.RequiresInteriorScene(kind))
			{
				return MiniMapPoiDiscoveryMode.InteriorScene;
			}
			if (kind == MiniMapPoiKind.Park)
			{
				return MiniMapPoiDiscoveryMode.WorldFootprint;
			}
			if (kind != MiniMapPoiKind.Metro && kind != MiniMapPoiKind.Kombini)
			{
				return MiniMapPoiDiscoveryMode.None;
			}
			if (volumes == null || volumes.Length == 0)
			{
				return MiniMapPoiDiscoveryMode.None;
			}
			return MiniMapPoiDiscoveryMode.NamedInteriorVolume;
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x0019C5E8 File Offset: 0x0019A7E8
		internal static BoxCollider[] CaptureInteriorVolumes(GameObject source, MiniMapPoiKind kind)
		{
			if (source == null || (kind != MiniMapPoiKind.Metro && kind != MiniMapPoiKind.Kombini))
			{
				return Array.Empty<BoxCollider>();
			}
			string text = ((kind == MiniMapPoiKind.Metro) ? "SubwayInteriorZone" : "Kombini Interior Volume");
			BoxCollider[] componentsInChildren = source.GetComponentsInChildren<BoxCollider>(true);
			List<BoxCollider> list = null;
			foreach (BoxCollider boxCollider in componentsInChildren)
			{
				if (!(boxCollider == null) && boxCollider.name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					if (list == null)
					{
						list = new List<BoxCollider>(2);
					}
					list.Add(boxCollider);
				}
			}
			if (list == null)
			{
				return Array.Empty<BoxCollider>();
			}
			return list.ToArray();
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x0019C678 File Offset: 0x0019A878
		internal static bool IsInsideWorldVisitArea(MiniMapPoiVisual poi, Vector3 position)
		{
			if (poi.discoveryMode == MiniMapPoiDiscoveryMode.NamedInteriorVolume)
			{
				int num = 0;
				while (poi.interiorVolumes != null && num < poi.interiorVolumes.Length)
				{
					BoxCollider boxCollider = poi.interiorVolumes[num];
					if (boxCollider != null && boxCollider.enabled && boxCollider.gameObject.activeInHierarchy && MiniMapPoiDiscoveryPolicy.ContainsBox(boxCollider, position))
					{
						return true;
					}
					num++;
				}
				return false;
			}
			return poi.discoveryMode == MiniMapPoiDiscoveryMode.WorldFootprint && MiniMapPoiDiscoveryPolicy.ContainsFootprint(poi.visitCenter, poi.footprintSize, poi.footprintYaw, position, 2f);
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x0019C708 File Offset: 0x0019A908
		internal static bool ContainsBox(BoxCollider box, Vector3 position)
		{
			if (box == null)
			{
				return false;
			}
			Vector3 vector = box.transform.InverseTransformPoint(position) - box.center;
			Vector3 vector2 = box.size * 0.5f;
			return Mathf.Abs(vector.x) <= vector2.x + 0.05f && Mathf.Abs(vector.y) <= vector2.y + 0.05f && Mathf.Abs(vector.z) <= vector2.z + 0.05f;
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x0019C79C File Offset: 0x0019A99C
		internal static bool ContainsFootprint(Vector3 center, Vector2 size, float yaw, Vector3 position, float inset = 2f)
		{
			if (size.x <= 0f || size.y <= 0f)
			{
				return false;
			}
			Vector3 vector = Quaternion.Euler(0f, -yaw, 0f) * (position - center);
			float num = Mathf.Max(0.25f, size.x * 0.5f - Mathf.Max(0f, inset));
			float num2 = Mathf.Max(0.25f, size.y * 0.5f - Mathf.Max(0f, inset));
			return Mathf.Abs(vector.x) <= num + 0.05f && Mathf.Abs(vector.z) <= num2 + 0.05f;
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x0019C858 File Offset: 0x0019AA58
		internal static bool TryResolveInteriorSceneKind(string sceneName, out MiniMapPoiKind kind)
		{
			if (string.Equals(sceneName, "POI - Art Gallery", StringComparison.OrdinalIgnoreCase))
			{
				kind = MiniMapPoiKind.Gallery;
			}
			else if (string.Equals(sceneName, "POI - Pool", StringComparison.OrdinalIgnoreCase))
			{
				kind = MiniMapPoiKind.Pool;
			}
			else if (string.Equals(sceneName, "POI - University", StringComparison.OrdinalIgnoreCase))
			{
				kind = MiniMapPoiKind.University;
			}
			else if (string.Equals(sceneName, "POI - Mall", StringComparison.OrdinalIgnoreCase))
			{
				kind = MiniMapPoiKind.Mall;
			}
			else
			{
				if (!string.Equals(sceneName, "POI - Stadium", StringComparison.OrdinalIgnoreCase))
				{
					kind = MiniMapPoiKind.None;
					return false;
				}
				kind = MiniMapPoiKind.Stadium;
			}
			return true;
		}

		// Token: 0x04004482 RID: 17538
		private const float Tolerance = 0.05f;

		// Token: 0x04004483 RID: 17539
		private const float FootprintInset = 2f;
	}
}
