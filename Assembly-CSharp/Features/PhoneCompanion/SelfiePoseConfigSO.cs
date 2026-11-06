using System;
using System.Collections.Generic;
using Features.Rank;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E69 RID: 3689
	[CreateAssetMenu(fileName = "SelfiePoseConfig", menuName = "Phone Companion/Selfie Pose Config")]
	public class SelfiePoseConfigSO : ScriptableObject
	{
		// Token: 0x06006E2A RID: 28202 RVA: 0x002C4D0C File Offset: 0x002C2F0C
		public static string PoseLabel(SelfiePoseConfigSO.PoseEntry e)
		{
			if (e == null)
			{
				return "";
			}
			if (!string.IsNullOrEmpty(e.displayName))
			{
				return e.displayName;
			}
			if (!(e.clip != null))
			{
				return "(empty)";
			}
			return e.clip.name;
		}

		// Token: 0x06006E2B RID: 28203 RVA: 0x002C4D4A File Offset: 0x002C2F4A
		public static string FaceLabel(SelfiePoseConfigSO.FaceEntry e)
		{
			if (e == null)
			{
				return "";
			}
			if (!string.IsNullOrEmpty(e.displayName))
			{
				return e.displayName;
			}
			return string.Format("Face {0}", e.faceExpressionIndex);
		}

		// Token: 0x040070D1 RID: 28881
		[Tooltip("Le pose disponibili nel menu selfie (in ordine). Ognuna porta la sua clip + offset telefono/camera.")]
		public List<SelfiePoseConfigSO.PoseEntry> poses = new List<SelfiePoseConfigSO.PoseEntry>();

		// Token: 0x040070D2 RID: 28882
		[Tooltip("Le facce disponibili nel menu selfie (in ordine). Ognuna mappa un indice del layer 'Face' dell'Animator (FaceExpressionIndex). Niente clip/mask qui: blending e mask li gestisce il layer Face dell'Animator.")]
		public List<SelfiePoseConfigSO.FaceEntry> faces = new List<SelfiePoseConfigSO.FaceEntry>();

		// Token: 0x02000E6A RID: 3690
		[Serializable]
		public class PoseBlendShapeBinding
		{
			// Token: 0x040070D3 RID: 28883
			public string rendererPath;

			// Token: 0x040070D4 RID: 28884
			public string blendShapeName;
		}

		// Token: 0x02000E6B RID: 3691
		[Serializable]
		public class PoseEntry
		{
			// Token: 0x06006E2E RID: 28206 RVA: 0x002C4D9C File Offset: 0x002C2F9C
			public bool IsAvailable(RankDefinitionSO rank)
			{
				return !this.requiresUnderwearRemovalRank || (rank != null && rank.allowFullUndress);
			}

			// Token: 0x040070D5 RID: 28885
			[Tooltip("Nome mostrato nel dropdown Pose. Se vuoto si usa il nome della clip.")]
			public string displayName;

			// Token: 0x040070D6 RID: 28886
			[Tooltip("Indice della posa nel layer 'SelfiePose' dell'Animator (parametro PoseIndex). 0 = nessuna posa (Empty), 1..4 = Pose1..4. Il picker fa SetInteger(PoseIndex, n): il blend tra pose lo gestisce Mecanim.")]
			public int poseIndex;

			// Token: 0x040070D7 RID: 28887
			[Tooltip("Clip della posa (full-body). Riferimento usato dal tool Setup Selfie Pose Layer per popolare gli stati; a runtime la posa la suona il layer Animator, non questa clip.")]
			public AnimationClip clip;

			// Token: 0x040070D8 RID: 28888
			[Tooltip("Blendshape curves authored in this pose, relative to the player Animator. Their original weights are restored when the pose stops, including abrupt phone closure.")]
			public List<SelfiePoseConfigSO.PoseBlendShapeBinding> animatedBlendShapes = new List<SelfiePoseConfigSO.PoseBlendShapeBinding>();

			// Token: 0x040070D9 RID: 28889
			[Tooltip("La posa NON espone il volto (es. personaggio di spalle): anche se il marker 'face' risulta inquadrato, la faccia non si vede. Se TRUE, le foto scattate in questa posa NON ricevono il flag _face -> niente censura del volto ne' rischio di leak/bug. Marcalo a mano sulle pose di schiena/profilo che nascondono il viso.")]
			public bool hidesFace;

			// Token: 0x040070DA RID: 28890
			[Tooltip("Requires a rank that permits removing underwear, including when taking photos at home.")]
			public bool requiresUnderwearRemovalRank;

			// Token: 0x040070DB RID: 28891
			[Header("Telefono in mano (offset locali rispetto al bone RightHand)")]
			public Vector3 phoneLocalPosition = new Vector3(0.0248f, 0.043f, -0.0264f);

			// Token: 0x040070DC RID: 28892
			public Vector3 phoneLocalEuler = new Vector3(33.111f, -179.861f, -67.906f);

			// Token: 0x040070DD RID: 28893
			public Vector3 phoneLocalScale = new Vector3(0.1f, 0.1f, 0.1f);

			// Token: 0x040070DE RID: 28894
			[Header("Camera selfie (offset locali rispetto al telefono)")]
			public Vector3 camLocalPosition = new Vector3(0.044f, 0.92f, 0.198f);

			// Token: 0x040070DF RID: 28895
			public Vector3 camLocalEuler = new Vector3(14.18f, 14.486f, 93.783f);
		}

		// Token: 0x02000E6C RID: 3692
		[Serializable]
		public class FaceEntry
		{
			// Token: 0x040070E0 RID: 28896
			[Tooltip("Nome mostrato nel dropdown Face.")]
			public string displayName;

			// Token: 0x040070E1 RID: 28897
			[Tooltip("Indice dell'espressione nel layer 'Face' dell'Animator (parametro FaceExpressionIndex). Le facce selfie sono gli stati 'Selfie 1..4' = indici 8..11. 0 = Base/neutra. Il picker fa animator.SetInteger(\"FaceExpressionIndex\", index): blending e mask li gestisce Mecanim.")]
			public int faceExpressionIndex;
		}
	}
}
