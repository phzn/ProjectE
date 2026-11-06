using System;
using System.Collections.Generic;
using Features.DumpsterHide;
using Features.HideFlash;
using Features.LockerHide;
using Features.TrashcanLoot;
using Features.WantedPoster;
using ProjectExhibitionist.BenchSeating;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.Subway;
using SojaExiles;
using UnityEngine;

// Token: 0x02000099 RID: 153
[DisallowMultipleComponent]
public sealed class PoiResidencyStateBindings : MonoBehaviour
{
	// Token: 0x06000668 RID: 1640 RVA: 0x0002DC18 File Offset: 0x0002BE18
	internal void ValidateAndIndex(string expectedFamily)
	{
		if (!string.Equals(this.family, expectedFamily, StringComparison.Ordinal))
		{
			throw new InvalidOperationException("POI state family does not match its placement.");
		}
		Dictionary<Component, string> dictionary = new Dictionary<Component, string>();
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		PoiResidencyStateBindings.Record[] array = this.records;
		if (array == null)
		{
			throw new InvalidOperationException("Missing POI state records.");
		}
		foreach (PoiResidencyStateBindings.Record record in array)
		{
			if (record == null || record.target == null || string.IsNullOrEmpty(record.stableId) || !record.target.transform.IsChildOf(base.transform) || dictionary.ContainsKey(record.target) || !hashSet.Add(record.stableId))
			{
				throw new InvalidOperationException("Invalid or duplicate POI state identity.");
			}
			dictionary.Add(record.target, record.stableId);
		}
		this.ids = dictionary;
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x0002DCFD File Offset: 0x0002BEFD
	internal bool TryGetId(Component target, out string id)
	{
		id = null;
		return this.ids != null && this.ids.TryGetValue(target, out id);
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0002DD1C File Offset: 0x0002BF1C
	internal void ReplaceWorldProp(GameObject original, GameObject replacement)
	{
		List<KeyValuePair<PoiResidencyStateBindings.Record, Component>> list = new List<KeyValuePair<PoiResidencyStateBindings.Record, Component>>();
		foreach (PoiResidencyStateBindings.Record record in this.records)
		{
			if (!(record.target == null) && record.target.transform.IsChildOf(original.transform))
			{
				string text = PoiResidencyStateBindings.SemanticComponentId(original.transform, record.target);
				Component component = null;
				foreach (Component component2 in replacement.GetComponentsInChildren(record.target.GetType(), true))
				{
					if (PoiResidencyStateBindings.SemanticComponentId(replacement.transform, component2) == text)
					{
						component = component2;
						break;
					}
				}
				if (component == null)
				{
					throw new InvalidOperationException("Replacement tier lacks authored interaction: " + text);
				}
				list.Add(new KeyValuePair<PoiResidencyStateBindings.Record, Component>(record, component));
			}
		}
		foreach (KeyValuePair<PoiResidencyStateBindings.Record, Component> keyValuePair in list)
		{
			this.ids.Remove(keyValuePair.Key.target);
			keyValuePair.Key.target = keyValuePair.Value;
			this.ids.Add(keyValuePair.Value, keyValuePair.Key.stableId);
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0002DE8C File Offset: 0x0002C08C
	public static string WorldPropOwnerId(Transform root, Transform target)
	{
		return PoiResidencyStateBindings.SemanticPath(root, target) + "|world-prop";
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0002DEA0 File Offset: 0x0002C0A0
	public static bool IsGameplayStateTarget(Component component)
	{
		return component is StreamedWorldPropSlot || component is IInteractable || component is WantedPoster || component is Door || component is opencloseDoor || component is opencloseDoor1 || component is TrashcanLootInteractable || component is WasteCanHideInteractable || component is DumpsterHideInteractable || component is LockerHideInteractable || component is SubwayRideDirector || component is BenchSeat;
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0002DF10 File Offset: 0x0002C110
	public static string ProjectViewStateId(string slotId, string localStateId)
	{
		if (string.IsNullOrEmpty(slotId) || !slotId.EndsWith("|world-prop", StringComparison.Ordinal) || string.IsNullOrEmpty(localStateId))
		{
			throw new InvalidOperationException("Invalid canonical view state identity.");
		}
		string text = slotId.Substring(0, slotId.Length - "|world-prop".Length);
		return text + ((text.Length > 0 && localStateId[0] != '|') ? "/" : "") + localStateId;
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0002DF84 File Offset: 0x0002C184
	public static string SemanticStateId(Transform root, Component target)
	{
		if (target is StreamedWorldPropSlot)
		{
			return PoiResidencyStateBindings.WorldPropOwnerId(root, target.transform);
		}
		TrashcanLootInteractable componentInParent = target.GetComponentInParent<TrashcanLootInteractable>(true);
		if (target is TrashcanLootInteractable || target is WasteCanHideInteractable || target is HideFlashPickup)
		{
			Transform transform = ((componentInParent != null) ? componentInParent.transform : target.transform);
			return PoiResidencyStateBindings.WorldPropOwnerId(root, transform) + "/view/" + PoiResidencyStateBindings.SemanticComponentId(transform, target);
		}
		return PoiResidencyStateBindings.SemanticComponentId(root, target);
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0002E000 File Offset: 0x0002C200
	public static string SemanticPath(Transform root, Transform target)
	{
		if (root == null || target == null || !target.IsChildOf(root))
		{
			throw new ArgumentException("State identity target must belong to its authored root.");
		}
		List<string> list = new List<string>();
		Transform transform = target;
		while (transform != root)
		{
			int num = 0;
			for (int i = 0; i < transform.GetSiblingIndex(); i++)
			{
				if (transform.parent.GetChild(i).name == transform.name)
				{
					num++;
				}
			}
			list.Add(Uri.EscapeDataString(transform.name) + "#" + num.ToString());
			transform = transform.parent;
		}
		list.Reverse();
		return string.Join("/", list);
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0002E0B8 File Offset: 0x0002C2B8
	public static string SemanticComponentId(Transform root, Component component)
	{
		Component[] components = component.GetComponents(component.GetType());
		return string.Concat(new string[]
		{
			PoiResidencyStateBindings.SemanticPath(root, component.transform),
			"|",
			component.GetType().FullName,
			"#",
			Array.IndexOf<Component>(components, component).ToString()
		});
	}

	// Token: 0x040004F7 RID: 1271
	public string family;

	// Token: 0x040004F8 RID: 1272
	public PoiResidencyStateBindings.Record[] records = Array.Empty<PoiResidencyStateBindings.Record>();

	// Token: 0x040004F9 RID: 1273
	private Dictionary<Component, string> ids;

	// Token: 0x0200009A RID: 154
	[Serializable]
	public sealed class Record
	{
		// Token: 0x040004FA RID: 1274
		public Component target;

		// Token: 0x040004FB RID: 1275
		public string stableId;
	}
}
