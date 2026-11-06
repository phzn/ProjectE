using System;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x020008F2 RID: 2290
	[CreateAssetMenu(fileName = "NewSkinDecal", menuName = "ProjectExhibitionist/Skin Decal")]
	public class SkinDecal : ScriptableObject
	{
		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06003E11 RID: 15889 RVA: 0x001711A7 File Offset: 0x0016F3A7
		public bool HasBodyTexture
		{
			get
			{
				return this.bodyTexture != null;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06003E12 RID: 15890 RVA: 0x001711B5 File Offset: 0x0016F3B5
		public bool HasHeadTexture
		{
			get
			{
				return this.headTexture != null;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x001711C3 File Offset: 0x0016F3C3
		public bool IsWashableByRain
		{
			get
			{
				return this.washableByRain || this.decalType == SkinDecalType.BodyPaint;
			}
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x001711D8 File Offset: 0x0016F3D8
		public int GetSortOrder()
		{
			return (int)(this.decalType * (SkinDecalType)100);
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x001711E3 File Offset: 0x0016F3E3
		private void OnValidate()
		{
			if (string.IsNullOrEmpty(this.displayName))
			{
				this.displayName = base.name;
			}
		}

		// Token: 0x04003E3E RID: 15934
		[Header("Info")]
		[Tooltip("Nome visualizzato")]
		public string displayName;

		// Token: 0x04003E3F RID: 15935
		[Tooltip("Applied through an item during a run; excluded from customization and appearance presets.")]
		public bool gameplayOnly;

		// Token: 0x04003E40 RID: 15936
		[Tooltip("Character-rendered preview used by the customization card.")]
		public Sprite previewSprite;

		// Token: 0x04003E41 RID: 15937
		[Header("Preview Camera")]
		[Tooltip("Override the automatic detail framing when regenerating this decal's preview.")]
		public bool overridePreviewCamera;

		// Token: 0x04003E42 RID: 15938
		[Tooltip("Preview focus offset from Ciel's head bone, in the capture pose.")]
		public Vector3 previewFocusOffset = new Vector3(0f, -0.3f, 0f);

		// Token: 0x04003E43 RID: 15939
		[Tooltip("Visible height of the square preview in world units. Smaller values zoom in.")]
		[Min(0.05f)]
		public float previewFrameHeight = 0.7f;

		// Token: 0x04003E44 RID: 15940
		[Tooltip("Camera yaw around Ciel: 0 is front, 180 is back.")]
		public float previewYaw;

		// Token: 0x04003E45 RID: 15941
		[Tooltip("Camera elevation in degrees.")]
		[Range(-60f, 60f)]
		public float previewPitch;

		// Token: 0x04003E46 RID: 15942
		[Header("Tipo")]
		[Tooltip("Categoria del decal (influenza l'ordine di rendering)")]
		public SkinDecalType decalType = SkinDecalType.Tattoo;

		// Token: 0x04003E47 RID: 15943
		[Tooltip("Permanente (tatuaggio) o temporaneo (sporco, sangue)")]
		public bool isPermanent = true;

		// Token: 0x04003E48 RID: 15944
		[Header("Water Response")]
		[Tooltip("Se attivo, la pioggia puo' sciogliere e rimuovere questo decal. I decal della categoria BodyPaint sono sempre considerati lavabili.")]
		public bool washableByRain;

		// Token: 0x04003E49 RID: 15945
		[Header("Detection Coverage")]
		[Tooltip("Contributo alla copertura per la detection, sulla stessa scala dei vestiti. Positivo rallenta l'attenzione, negativo la accelera; 0 e' neutro. Il contributo segue l'opacita' visibile e il lavaggio della pioggia.")]
		public float detectionCoverage;

		// Token: 0x04003E4A RID: 15946
		[Tooltip("Relative increase to NPC attention, weighted by visible opacity (0.15 = +15%).")]
		[Min(0f)]
		public float detectionAttentionBonus;

		// Token: 0x04003E4B RID: 15947
		[Header("Texture Body")]
		[Tooltip("Texture per il corpo (mappata sulle UV del Body)")]
		public Texture2D bodyTexture;

		// Token: 0x04003E4C RID: 15948
		[Tooltip("Padding in texel generato ai bordi delle isole UV del corpo. Usare solo per texture che mostrano cuciture; 0 disabilita la correzione.")]
		[Range(0f, 16f)]
		public int bodyUvSeamPaddingPixels;

		// Token: 0x04003E4D RID: 15949
		[Tooltip("Only pad outside UV islands; preserve every authored interior pixel. Use for sparse handprints and lettering whose transparent gaps are intentional.")]
		public bool bodyUvSeamGutterOnly;

		// Token: 0x04003E4E RID: 15950
		[Header("Texture Head")]
		[Tooltip("Texture per la testa (mappata sulle UV dell'Head)")]
		public Texture2D headTexture;

		// Token: 0x04003E4F RID: 15951
		[Tooltip("Padding in texel generato ai bordi delle isole UV della testa. Usare solo per texture che mostrano cuciture; 0 disabilita la correzione.")]
		[Range(0f, 16f)]
		public int headUvSeamPaddingPixels;

		// Token: 0x04003E50 RID: 15952
		[Tooltip("Ciel's untanned face atlas. When assigned, transfers the tan colour change to the separate enlarged lip UV island, preserving its detail. Leave empty for tattoos and other face paint.")]
		public Texture2D headLipToneReference;

		// Token: 0x04003E51 RID: 15953
		[Header("Skin Tone Matching")]
		[Tooltip("If true, the decal tint is automatically driven by the current Skin color (preset or custom). Used for sun tan, blush, etc.")]
		public bool matchSkinTone;

		// Token: 0x04003E52 RID: 15954
		[Header("Default Settings")]
		[Tooltip("Colore di tinta di default (ignored when matchSkinTone is true)")]
		public Color defaultTint = Color.white;

		// Token: 0x04003E53 RID: 15955
		[Tooltip("Minimum lilToon lighting for this decal, preserving its texture, alpha and wet reflections. Use for water highlights that become dirty grey indoors. 0 inherits the base material.")]
		[Range(0f, 1f)]
		public float minimumLight;

		// Token: 0x04003E54 RID: 15956
		[Tooltip("Opacità di default")]
		[Range(0f, 1f)]
		public float defaultOpacity = 1f;
	}
}
