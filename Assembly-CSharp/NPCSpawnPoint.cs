using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class NPCSpawnPoint : MonoBehaviour
{
	// Token: 0x06000816 RID: 2070 RVA: 0x0003A896 File Offset: 0x00038A96
	private void OnDrawGizmos()
	{
		Gizmos.color = this.gizmoColor;
		Gizmos.DrawSphere(base.transform.position, this.gizmoRadius);
	}

	// Token: 0x04000729 RID: 1833
	[Header("Indoor / Stacked Floors")]
	[Tooltip("Optional maximum vertical distance from the player for this marker. Use a value smaller than the floor spacing in stacked interiors; 0 disables the filter.")]
	[Min(0f)]
	public float maxVerticalDistanceFromPlayer;

	// Token: 0x0400072A RID: 1834
	[Tooltip("When enabled, this point is rejected while it is visible to the gameplay camera. Use in interiors where distance alone cannot hide NPC pop-in.")]
	public bool requireCameraOcclusion;

	// Token: 0x0400072B RID: 1835
	[Header("Debug")]
	[Tooltip("Colore del gizmo disegnato in Scene View.")]
	[SerializeField]
	private Color gizmoColor = new Color(0.2f, 0.8f, 1f, 0.8f);

	// Token: 0x0400072C RID: 1836
	[Tooltip("Raggio del gizmo in Scene View.")]
	[SerializeField]
	private float gizmoRadius = 0.3f;
}
