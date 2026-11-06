using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001283 RID: 4739
	[Serializable]
	public class EmotePropBinding
	{
		// Token: 0x040097B6 RID: 38838
		[Tooltip("Descriptive label for this prop/effect (editor only)")]
		public string Label;

		// Token: 0x040097B7 RID: 38839
		[Tooltip("Prefab to instantiate at runtime (e.g. Elixir_Normal for pee effect). Instantiated once under AttachBone, kept disabled until activated.")]
		public GameObject EffectPrefab;

		// Token: 0x040097B8 RID: 38840
		[Tooltip("Se true, ignora AttachBone (HumanBodyBones) e usa CustomBoneName / CustomBonePath per agganciare il prop a un osso che NON e' nello standard humanoid (es. il bone extra del Futanari). Se false, usa AttachBone come al solito.")]
		public bool UseCustomBone;

		// Token: 0x040097B9 RID: 38841
		[Tooltip("Nome del transform custom da cercare ricorsivamente sotto l'animator root. Cerca il primo child con questo nome (case-sensitive). Pi&#249; comodo del path completo ma ambiguo se ce ne sono piu' con lo stesso nome — in quel caso usa CustomBonePath.")]
		public string CustomBoneName;

		// Token: 0x040097BA RID: 38842
		[Tooltip("Path completo del transform custom relativo all'animator root (es. 'J_Bip_C_Hips/Futanari/Penis_Tip'). Vince su CustomBoneName se entrambi sono settati. Usato solo se UseCustomBone = true.")]
		public string CustomBonePath;

		// Token: 0x040097BB RID: 38843
		[Tooltip("HumanBodyBones bone to parent the effect under (e.g. Hips for pee). Usato solo se UseCustomBone = false.")]
		public HumanBodyBones AttachBone;

		// Token: 0x040097BC RID: 38844
		[Tooltip("Local position offset from the attach bone")]
		public Vector3 LocalPositionOffset;

		// Token: 0x040097BD RID: 38845
		[Tooltip("Local rotation offset from the attach bone")]
		public Vector3 LocalRotationOffset;

		// Token: 0x040097BE RID: 38846
		[Tooltip("Optional absolute local scale after parenting. Zero preserves the effect prefab's authored scale.")]
		public Vector3 LocalScaleOffset = Vector3.zero;

		// Token: 0x040097BF RID: 38847
		[Tooltip("If > 0, the effect activates automatically this many seconds after emote starts. If 0, use Animation Event 'AE_EmoteProp' with string parameter = Label to activate.")]
		public float AutoActivateDelay;

		// Token: 0x040097C0 RID: 38848
		[Tooltip("If > 0, the effect deactivates automatically this many seconds after activation. If 0, deactivates when emote ends.")]
		public float AutoDeactivateAfter;

		// Token: 0x040097C1 RID: 38849
		[Tooltip("If true, the instantiated effect is destroyed when emote ends. If false, just deactivated (pooled).")]
		public bool DestroyOnEnd;
	}
}
