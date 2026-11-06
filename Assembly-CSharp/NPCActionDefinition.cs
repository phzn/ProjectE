using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
[CreateAssetMenu(fileName = "NPCAction_", menuName = "NPC/Action Definition")]
public class NPCActionDefinition : ScriptableObject
{
	// Token: 0x0600072C RID: 1836 RVA: 0x00032A82 File Offset: 0x00030C82
	public float GetRandomDuration()
	{
		if (this.maxDuration <= 0f)
		{
			return 0f;
		}
		return Random.Range(this.minDuration, this.maxDuration);
	}

	// Token: 0x040005EF RID: 1519
	[Header("Identificazione")]
	[Tooltip("Nome leggibile dell'azione (es. 'Scrivere al Telefono')")]
	public string displayName;

	// Token: 0x040005F0 RID: 1520
	[Tooltip("Indice corrispondente nel parametro UpperBodyAction dell'Animator")]
	public int animatorIndex;

	// Token: 0x040005F1 RID: 1521
	[Tooltip("Descrizione dell'azione per debug/documentazione")]
	[TextArea(2, 4)]
	public string description;

	// Token: 0x040005F2 RID: 1522
	[Header("Attenzione")]
	[Tooltip("Livello di attenzione dell'NPC mentre esegue questa azione (0 = distratto, 1 = massima attenzione)")]
	[Range(0f, 1f)]
	public float attentionLevel = 0.5f;

	// Token: 0x040005F3 RID: 1523
	[Tooltip("Raggio visivo dell'NPC durante questa azione (moltiplicatore del raggio base)")]
	[Range(0f, 2f)]
	public float visionRadiusMultiplier = 1f;

	// Token: 0x040005F4 RID: 1524
	[Tooltip("Angolo del campo visivo durante questa azione (moltiplicatore)")]
	[Range(0f, 2f)]
	public float visionAngleMultiplier = 1f;

	// Token: 0x040005F5 RID: 1525
	[Tooltip("Se true, l'NPC può eseguire LookAt durante questa azione")]
	public bool allowLookAt = true;

	// Token: 0x040005F6 RID: 1526
	[Header("Locomozione")]
	[Tooltip("Se true, questa azione può essere eseguita mentre l'NPC cammina")]
	public bool canPerformWhileMoving;

	// Token: 0x040005F7 RID: 1527
	[Tooltip("Modifica della velocità di movimento durante l'azione (1 = normale)")]
	[Range(0.1f, 1.5f)]
	public float movementSpeedMultiplier = 1f;

	// Token: 0x040005F8 RID: 1528
	[Tooltip("Peso relativo per la selezione random durante la locomozione (0 = usa default del registry, >0 = peso specifico)")]
	[Range(0f, 10f)]
	public float locomotionSpawnWeight;

	// Token: 0x040005F9 RID: 1529
	[Header("Equipment")]
	[Tooltip("Props richiesti per questa azione (es. 'Smartphone')")]
	public string[] requiredProps;

	// Token: 0x040005FA RID: 1530
	[Header("Timing")]
	[Tooltip("Durata minima dell'azione in secondi (0 = istantanea)")]
	public float minDuration;

	// Token: 0x040005FB RID: 1531
	[Tooltip("Durata massima dell'azione in secondi")]
	public float maxDuration;
}
