using System;
using UnityEngine;

// Token: 0x020000B6 RID: 182
[CreateAssetMenu(fileName = "NPCComponentConfig", menuName = "NPC/Component Config")]
public class NPCComponentConfig : ScriptableObject
{
	// Token: 0x04000605 RID: 1541
	[Header("Animator")]
	[Tooltip("Controller animator da assegnare agli NPC")]
	public RuntimeAnimatorController animatorController;

	// Token: 0x04000606 RID: 1542
	[Tooltip("Avatar da usare per l'animator (opzionale, usa quello esistente se null)")]
	public Avatar animatorAvatar;

	// Token: 0x04000607 RID: 1543
	[Header("Movement - A* Pathfinding")]
	[Tooltip("Velocità di movimento NPC")]
	public float moveSpeed = 3.5f;

	// Token: 0x04000608 RID: 1544
	[Tooltip("Velocità di rotazione NPC (gradi/secondo)")]
	public float rotationSpeed = 360f;

	// Token: 0x04000609 RID: 1545
	[Tooltip("Raggio collisione NPC")]
	public float radius = 0.5f;

	// Token: 0x0400060A RID: 1546
	[Tooltip("Altezza NPC")]
	public float height = 2f;

	// Token: 0x0400060B RID: 1547
	[Tooltip("Abilita local avoidance (RVO)")]
	public bool enableLocalAvoidance = true;

	// Token: 0x0400060C RID: 1548
	[Header("NPC Senses")]
	[Tooltip("Abilita il componente NPCSenses")]
	public bool enableSenses = true;

	// Token: 0x0400060D RID: 1549
	[Tooltip("Raggio di vista")]
	public float sightRange = 15f;

	// Token: 0x0400060E RID: 1550
	[Tooltip("Angolo del campo visivo")]
	public float fieldOfView = 120f;

	// Token: 0x0400060F RID: 1551
	[Tooltip("Raggio di udito")]
	public float hearingRange = 10f;

	// Token: 0x04000610 RID: 1552
	[Header("NPC Action Bridge")]
	[Tooltip("Abilita il componente NPCActionBridge")]
	public bool enableActionBridge = true;

	// Token: 0x04000611 RID: 1553
	[Header("Attention UI")]
	[Tooltip("Abilita il componente AttentionUIController")]
	public bool enableAttentionUI = true;

	// Token: 0x04000612 RID: 1554
	[Tooltip("Prefab per l'UI di attenzione (opzionale)")]
	public GameObject attentionUIPrefab;

	// Token: 0x04000613 RID: 1555
	[Header("Dialogue")]
	[Tooltip("Abilita il componente NPCDialogueHolder")]
	public bool enableDialogue = true;

	// Token: 0x04000614 RID: 1556
	[Header("Equipment")]
	[Tooltip("Abilita il componente NPCEquipmentManager")]
	public bool enableEquipment = true;

	// Token: 0x04000615 RID: 1557
	[Header("Mesh Optimization")]
	[Tooltip("Abilita ottimizzazione mesh (merge)")]
	public bool enableMeshOptimization = true;

	// Token: 0x04000616 RID: 1558
	[Tooltip("Delay prima dell'ottimizzazione mesh")]
	public float meshOptimizationDelay = 0.5f;
}
