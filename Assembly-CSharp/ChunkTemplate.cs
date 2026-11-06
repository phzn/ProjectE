using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005C RID: 92
[CreateAssetMenu(fileName = "ChunkTemplate", menuName = "MapGeneration/ChunkTemplate", order = 2)]
public class ChunkTemplate : ScriptableObject
{
	// Token: 0x060004B3 RID: 1203 RVA: 0x0002565B File Offset: 0x0002385B
	private void OnValidate()
	{
		this.ValidateZoneSlots();
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00025664 File Offset: 0x00023864
	private void ValidateZoneSlots()
	{
		if (this.zoneSlots.Count == 0)
		{
			return;
		}
		if (this.zoneSlots.Count > 1)
		{
			ChunkSize parentChunkSize = this.zoneSlots[0].parentChunkSize;
			using (List<ZoneSlot>.Enumerator enumerator = this.zoneSlots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.parentChunkSize != parentChunkSize)
					{
						Debug.LogWarning("[ChunkTemplate] '" + base.name + "': Slot con parentChunkSize diversi! Tutte le slot dovrebbero avere lo stesso parentChunkSize.");
					}
				}
			}
		}
		HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
		foreach (ZoneSlot zoneSlot in this.zoneSlots)
		{
			if (hashSet.Contains(zoneSlot.gridPosition))
			{
				Debug.LogWarning(string.Format("[ChunkTemplate] '{0}': Sovrapposizione rilevata alla posizione {1}!", base.name, zoneSlot.gridPosition));
			}
			else
			{
				hashSet.Add(zoneSlot.gridPosition);
			}
		}
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00025780 File Offset: 0x00023980
	[ContextMenu("Generate Slots From Prefab")]
	public void GenerateSlotsFromPrefab()
	{
		if (this.chunkPrefab == null)
		{
			Debug.LogError("[ChunkTemplate] Assegna prima il chunkPrefab!");
			return;
		}
		this.zoneSlots.Clear();
		foreach (Transform transform in this.chunkPrefab.GetComponentsInChildren<Transform>(true))
		{
			if (transform.name.Contains("Base") && transform != this.chunkPrefab.transform)
			{
				ZoneOccupancy zoneOccupancy = ZoneOccupancy.Full;
				if (transform.name.Contains("Quarter") || transform.name.Contains("Q"))
				{
					zoneOccupancy = ZoneOccupancy.Quarter;
				}
				else if (transform.name.Contains("Half") || transform.name.Contains("H"))
				{
					zoneOccupancy = ZoneOccupancy.Half;
				}
				Vector2Int vector2Int = new Vector2Int(this.zoneSlots.Count, 0);
				ChunkSize chunkSize = ChunkSize.Size1x1;
				ZoneSlot zoneSlot = new ZoneSlot(vector2Int, chunkSize, zoneOccupancy, 0);
				zoneSlot.spawnTransform = transform;
				this.zoneSlots.Add(zoneSlot);
				Debug.Log(string.Format("[ChunkTemplate] Slot creato da '{0}': pos={1}, chunk={2}, occupancy={3}", new object[] { transform.name, vector2Int, chunkSize, zoneOccupancy }));
			}
		}
		Debug.Log(string.Format("[ChunkTemplate] Generati {0} slot dal prefab '{1}'", this.zoneSlots.Count, this.chunkPrefab.name));
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x000258F0 File Offset: 0x00023AF0
	[ContextMenu("Generate Default Slots (Legacy)")]
	public void GenerateDefaultSlots()
	{
		this.zoneSlots.Clear();
		switch (this.shape)
		{
		case ChunkShape.XShape:
			this.zoneSlots.Add(new ZoneSlot(new Vector2Int(0, 0), ChunkSize.Size1x1, ZoneOccupancy.Quarter, 0));
			this.zoneSlots.Add(new ZoneSlot(new Vector2Int(1, 0), ChunkSize.Size1x1, ZoneOccupancy.Quarter, 0));
			this.zoneSlots.Add(new ZoneSlot(new Vector2Int(0, 1), ChunkSize.Size1x1, ZoneOccupancy.Quarter, 0));
			this.zoneSlots.Add(new ZoneSlot(new Vector2Int(1, 1), ChunkSize.Size1x1, ZoneOccupancy.Quarter, 0));
			break;
		case ChunkShape.TShape:
			this.zoneSlots.Add(new ZoneSlot(new Vector2Int(0, 0), ChunkSize.Size1x1, ZoneOccupancy.Quarter, 0));
			this.zoneSlots.Add(new ZoneSlot(new Vector2Int(1, 0), ChunkSize.Size1x1, ZoneOccupancy.Quarter, 0));
			this.zoneSlots.Add(new ZoneSlot(new Vector2Int(0, 1), ChunkSize.Size1x1, ZoneOccupancy.Half, 0));
			break;
		case ChunkShape.MonoShape:
			this.zoneSlots.Add(new ZoneSlot(new Vector2Int(0, 0), ChunkSize.Size1x1, ZoneOccupancy.Full, 0));
			break;
		}
		Debug.Log(string.Format("[ChunkTemplate] Generati {0} slot per forma {1}", this.zoneSlots.Count, this.shape));
	}

	// Token: 0x04000393 RID: 915
	[Header("Chunk Configuration")]
	public ChunkShape shape;

	// Token: 0x04000394 RID: 916
	[Tooltip("Prefab del chunk che contiene i GameObject Base come riferimenti per le zone")]
	public GameObject chunkPrefab;

	// Token: 0x04000395 RID: 917
	[Header("Zone Slots")]
	[Tooltip("Definisce le posizioni e dimensioni delle zone nel chunk")]
	public List<ZoneSlot> zoneSlots = new List<ZoneSlot>();

	// Token: 0x04000396 RID: 918
	[Header("Snap Configuration")]
	[Tooltip("Snap points del chunk per connettersi ad altri chunk")]
	public List<SnapPoint> chunkSnapPoints = new List<SnapPoint>();
}
