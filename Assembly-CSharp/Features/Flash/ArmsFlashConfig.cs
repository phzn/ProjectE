using System;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.Flash
{
	// Token: 0x02001202 RID: 4610
	[Serializable]
	public class ArmsFlashConfig
	{
		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x06008E3B RID: 36411 RVA: 0x003A26DC File Offset: 0x003A08DC
		public string DetailClosedStateName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.detailClosedStateName))
				{
					return this.detailClosedStateName;
				}
				string text = this.endTrigger;
				if (text == "SwimsuitFlashEnd")
				{
					return "PullUp";
				}
				if (text == "BikiniBraFlashEnd")
				{
					return "EPBra_Lower";
				}
				if (text == "BikiniPantiesFlashEnd")
				{
					return "EPPanties_Raise";
				}
				if (text == "RainCoatFlashEnd")
				{
					return "Flash_Close";
				}
				if (text == "RainCoatHoodEnd")
				{
					return "Hood_Lower";
				}
				if (!(text == "UniversitySkirtFlashEnd"))
				{
					return this.emptyStateName;
				}
				return "UniversitySkirt_LowerShape";
			}
		}

		// Token: 0x040091AF RID: 37295
		[Header("Worn garment")]
		[Tooltip("Mesh root name of the worn garment (the SMR-owning object the blendshape clips bind to). Matched by exact name or StartsWith.")]
		public string wornObjectName = "School_Swimsuit";

		// Token: 0x040091B0 RID: 37296
		[Tooltip("Garment-only blendshape controller (e.g. SwimsuitFlash_SwimsuitOnly). The garment is dressed at runtime, so this is added to its Animator on the fly if needed.")]
		public RuntimeAnimatorController detailOnlyController;

		// Token: 0x040091B1 RID: 37297
		[Tooltip("HYBRID breast-size strategy for the detail (garment blendshape) animator.\nfalse (default) = CORRECTIVE: one base blendshape clip + BoobsSizeCorrectiveShapeDriver drives the '{Base}_{Size}_Corr' shapes per size (cheap, one clip fits all sizes).\ntrue = DEDICATED CLIPS: the detail controller has per-size BlendTrees on 'BoobsType'; we DON'T add the corrective driver and instead propagate the player's BoobsType float to the detail animator. Use for garments whose extreme sizes (Flat/Balloon) the correctives can't hold (e.g. the Swimsuit).")]
		public bool useDedicatedSizeClips;

		// Token: 0x040091B2 RID: 37298
		[Header("Animator (player) — shared flash layer")]
		[Tooltip("Optional player Animator layer used by this garment. Empty keeps the common ClothFlashArms layer. Use a dedicated masked layer when a gesture needs more than the arms (for example ClothFlashUpperBody).")]
		public string sharedLayerName = "";

		// Token: 0x040091B3 RID: 37299
		[Tooltip("Variante di questo capo sul layer condiviso ClothFlashArms (ClothFlashVariant int): Swimsuit=1, BikiniBra=2, BikiniPanties=3. Il controller seleziona la variante PRIMA di lanciare il trigger condiviso, così il layer entra nel gesto giusto.")]
		public int flashVariant = 1;

		// Token: 0x040091B4 RID: 37300
		[Tooltip("LEGACY — nome del vecchio layer per-capo. Non più usato per pilotare l'Animator (ora si usa il layer condiviso ClothFlashArms); resta solo per identificare il controller (es. il coordinatore BikiniBraActionsUI lo confronta per distinguere bra/panties).")]
		public string flashLayerName = "SwimsuitFlash";

		// Token: 0x040091B5 RID: 37301
		[Tooltip("LEGACY — vecchio trigger per-capo sul player. Il player ora usa ClothFlashStart. Questo nome resta usato SOLO sul detail animator (clip blendshape del capo).")]
		public string startTrigger = "SwimsuitFlashStart";

		// Token: 0x040091B6 RID: 37302
		[Tooltip("LEGACY — vecchio trigger per-capo sul player. Il player ora usa ClothFlashEnd. Questo nome resta usato SOLO sul detail animator (clip blendshape del capo).")]
		public string endTrigger = "SwimsuitFlashEnd";

		// Token: 0x040091B7 RID: 37303
		[Tooltip("Optional authored detail closing state to sample immediately when caught. Empty uses the legacy garment's known closing state.")]
		public string detailClosedStateName = "";

		// Token: 0x040091B8 RID: 37304
		[Tooltip("Name of the empty/rest state on the flash layer, used to detect when the gesture has returned to rest so the layer can fade back to 0.")]
		public string emptyStateName = "Empty";

		// Token: 0x040091B9 RID: 37305
		[Tooltip("OPZIONALE — nome dello stato di HOLD sul layer condiviso (es. 'RainCoat_Hold'). Vuoto (default) = gesto transiente classico: il layer torna a Empty e sfuma a 0 dopo ogni press. Se impostato, il capo resta APERTO in una posa braccia persistente (layer a 1, OnlyArms: il base layer continua a camminare/correre) finché non arriva il press di chiusura; il token del ClothActionGate viene rilasciato appena lo stato di hold è raggiunto, così il secondo press può chiudere subito.")]
		public string holdStateName = "";

		// Token: 0x040091BA RID: 37306
		[Tooltip("Indice del layer sul DETAIL animator (blendshape del capo) pilotato da questo controller. 0 per il flash classico; un capo con PIÙ azioni indipendenti sullo stesso detail controller (es. raincoat: flash su layer 0, cappuccio su layer 1) usa un layer per azione, così il reset/monitor di un'azione non azzera la posa dell'altra.")]
		public int detailLayerIndex;

		// Token: 0x040091BB RID: 37307
		[Tooltip("Se true (default) l'APERTURA è gatata dal rank come attività Flash (bloccata sotto HORNY). False per azioni innocue sullo stesso sistema (es. alzare/abbassare il cappuccio) che non devono mostrare il feedback di blocco.")]
		public bool gateAsFlash = true;

		// Token: 0x040091BC RID: 37308
		[Tooltip("Se true (default) il reset a neutro riporta il layer del detail sullo stato Empty. False per i layer che NON devono mai tornare in Empty dopo aver giocato (quirk Unity: su animator multi-layer WD=false, un layer parcheggiato in Empty con storia blocca le scritture blendshape dei layer sotto — es. layer Hood del raincoat).")]
		public bool resetDetailToEmpty = true;

		// Token: 0x040091BD RID: 37309
		[Header("Layer weight")]
		public float fadeInDuration = 0.15f;

		// Token: 0x040091BE RID: 37310
		public float fadeOutDuration = 0.25f;

		// Token: 0x040091BF RID: 37311
		[Tooltip("When true (recommended), the OnlyArms layer fades IN from 0 to 1 over fadeInDuration so the arm blends in smoothly (the Bikini behaviour). When false it snaps to 1 instantly on press, then still fades out (the original Swimsuit behaviour).")]
		public bool fadeInFromZero = true;

		// Token: 0x040091C0 RID: 37312
		[Header("Input")]
		public GenericInput toggleInput = new GenericInput("B", "Y", "B");

		// Token: 0x040091C1 RID: 37313
		[Tooltip("Seconds to lock further toggles after triggering an action.")]
		public float toggleCooldown = 0.6f;

		// Token: 0x040091C2 RID: 37314
		[Tooltip("When false, this controller does NOT read its own key — an external coordinator (e.g. BikiniBraActionsUI) drives Toggle() instead. Single-piece garments leave this true.")]
		public bool listenForOwnInput = true;
	}
}
