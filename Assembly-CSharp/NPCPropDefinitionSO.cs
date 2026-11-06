using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
[CreateAssetMenu(fileName = "PropDef_", menuName = "NPC/Props/Prop Definition")]
public class NPCPropDefinitionSO : ScriptableObject
{
	// Token: 0x040007CA RID: 1994
	[Header("Identità")]
	[Tooltip("ID univoco usato per lookup e debug")]
	public string propId;

	// Token: 0x040007CB RID: 1995
	[Header("Prefab")]
	[Tooltip("Prefab del prop da istanziare a runtime")]
	public GameObject prefab;

	// Token: 0x040007CC RID: 1996
	[Header("Attacco al bone")]
	[Tooltip("Bone humanoid a cui agganciare il prop (mano primaria)")]
	public HumanBodyBones attachBone = HumanBodyBones.RightHand;

	// Token: 0x040007CD RID: 1997
	[Tooltip("Posizione locale relativa al bone")]
	public Vector3 localPosition;

	// Token: 0x040007CE RID: 1998
	[Tooltip("Rotazione locale relativa al bone (gradi Euler)")]
	public Vector3 localEulerAngles;

	// Token: 0x040007CF RID: 1999
	[Tooltip("Scala locale del prop")]
	public Vector3 localScale = Vector3.one;

	// Token: 0x040007D0 RID: 2000
	[Header("IK Mano Primaria (opzionale)")]
	[Tooltip("Se true, abilita IK sulla mano primaria per correggere rotazione/posizione del grip")]
	public bool usePrimaryHandIK;

	// Token: 0x040007D1 RID: 2001
	[Tooltip("Nome (o path tipo 'Mesh/GripL') del GO ANCHOR nel prop prefab che il target deve seguire.\nLascia vuoto per usare il prop transform stesso come riferimento.")]
	public string primaryHandAnchorName;

	// Token: 0x040007D2 RID: 2002
	[Tooltip("Offset locale RELATIVO ALL'ANCHOR (o al prop se anchor vuoto) per fine-tuning del target")]
	public Vector3 primaryHandTargetOffset;

	// Token: 0x040007D3 RID: 2003
	[Tooltip("Rotazione locale RELATIVA ALL'ANCHOR per fine-tuning del target (gradi Euler)")]
	public Vector3 primaryHandTargetRotation;

	// Token: 0x040007D4 RID: 2004
	[Tooltip("Nome del GO anchor nel prop prefab per l'hint del gomito primario.\nLascia vuoto per usare il prop transform stesso.")]
	public string primaryElbowHintAnchorName;

	// Token: 0x040007D5 RID: 2005
	[Tooltip("Offset locale relativo all'anchor (o al prop) per il hint del gomito primario")]
	public Vector3 primaryElbowHintOffset;

	// Token: 0x040007D6 RID: 2006
	[Header("IK Mano Secondaria (opzionale)")]
	[Tooltip("Se true, abilita IK sulla mano secondaria per impugnatura a due mani.\nIl bone chain è pre-configurato nel constraint sul prefab.")]
	public bool useSecondaryHandIK;

	// Token: 0x040007D7 RID: 2007
	[Tooltip("Nome del GO ANCHOR nel prop prefab che il target secondario deve seguire.\nLascia vuoto per usare il prop transform stesso.")]
	public string secondaryHandAnchorName;

	// Token: 0x040007D8 RID: 2008
	[Tooltip("Offset locale RELATIVO ALL'ANCHOR per fine-tuning del target secondario")]
	public Vector3 secondaryHandTargetOffset;

	// Token: 0x040007D9 RID: 2009
	[Tooltip("Rotazione locale RELATIVA ALL'ANCHOR per fine-tuning del target secondario (gradi Euler)")]
	public Vector3 secondaryHandTargetRotation;

	// Token: 0x040007DA RID: 2010
	[Tooltip("Nome del GO anchor nel prop prefab per l'hint del gomito secondario.\nLascia vuoto per usare il prop transform stesso.")]
	public string secondaryElbowHintAnchorName;

	// Token: 0x040007DB RID: 2011
	[Tooltip("Offset locale relativo all'anchor per il hint del gomito secondario")]
	public Vector3 secondaryElbowHintOffset;

	// Token: 0x040007DC RID: 2012
	[Header("Animazione")]
	[Tooltip("Valore di UpperBodyAction da impostare quando il prop è equipaggiato (0 = nessuna azione)")]
	public int upperBodyActionIndex;

	// Token: 0x040007DD RID: 2013
	[Tooltip("Se true, il prop rimane visibile anche durante la locomozione")]
	public bool activeWhileMoving = true;
}
