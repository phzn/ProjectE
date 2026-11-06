using System;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class PreSpawnedChunk : MonoBehaviour
{
	// Token: 0x0600068B RID: 1675 RVA: 0x0002E4D0 File Offset: 0x0002C6D0
	private void OnDrawGizmos()
	{
		Gizmos.color = this.gizmoColor;
		float num = 60f;
		Vector3 vector = new Vector3((float)this.gridSize.x * num, 10f, (float)this.gridSize.y * num);
		Gizmos.DrawWireCube(base.transform.position + new Vector3(vector.x * 0.5f - num * 0.5f, 5f, vector.z * 0.5f - num * 0.5f), vector);
	}

	// Token: 0x0400050A RID: 1290
	[Header("Chunk Configuration")]
	[Tooltip("ChunkData associato a questo chunk (opzionale, per chunk con zone).")]
	public ChunkData chunkData;

	// Token: 0x0400050B RID: 1291
	[Tooltip("Dimensione del chunk in celle (default 1x1).")]
	public Vector2Int gridSize = new Vector2Int(1, 1);

	// Token: 0x0400050C RID: 1292
	[Header("Debug")]
	[Tooltip("Colore del gizmo per questo chunk.")]
	public Color gizmoColor = new Color(0f, 1f, 0f, 0.3f);
}
