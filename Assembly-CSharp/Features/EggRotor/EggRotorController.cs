using System;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using Features.BodyPhysics;
using Features.Detection;
using Features.InsertableToys;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.MainMenu;
using Features.NippleRotor;
using Features.NPC;
using Features.NPC.StateMachine;
using Features.PlayerEmotes;
using MagicaCloth2;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Futanari;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.EggRotor
{
	// Token: 0x02001226 RID: 4646
	[DefaultExecutionOrder(22100)]
	[DisallowMultipleComponent]
	public sealed class EggRotorController : MonoBehaviour
	{
		// Token: 0x140000FC RID: 252
		// (add) Token: 0x06008FDC RID: 36828 RVA: 0x003ADC0C File Offset: 0x003ABE0C
		// (remove) Token: 0x06008FDD RID: 36829 RVA: 0x003ADC40 File Offset: 0x003ABE40
		public static event Action<int> LevelChanged;

		// Token: 0x140000FD RID: 253
		// (add) Token: 0x06008FDE RID: 36830 RVA: 0x003ADC74 File Offset: 0x003ABE74
		// (remove) Token: 0x06008FDF RID: 36831 RVA: 0x003ADCA8 File Offset: 0x003ABEA8
		public static event Action<EggRotorPlacement> PlacementChanged;

		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x06008FE0 RID: 36832 RVA: 0x003ADCDC File Offset: 0x003ABEDC
		public static EggRotorController Current
		{
			get
			{
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				if (instance == null)
				{
					return null;
				}
				Transform transform = ((instance.PlayerAnimator != null) ? instance.PlayerAnimator.transform : null);
				for (int i = EggRotorController.EnabledControllers.Count - 1; i >= 0; i--)
				{
					EggRotorController eggRotorController = EggRotorController.EnabledControllers[i];
					if (eggRotorController == null)
					{
						EggRotorController.EnabledControllers.RemoveAt(i);
					}
					else if (eggRotorController._inventory == instance || (transform != null && eggRotorController._playerRoot == transform))
					{
						return eggRotorController;
					}
				}
				return null;
			}
		}

		// Token: 0x170016FE RID: 5886
		// (get) Token: 0x06008FE1 RID: 36833 RVA: 0x003ADD78 File Offset: 0x003ABF78
		public EggRotorConfigSO Config
		{
			get
			{
				return this._config;
			}
		}

		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x06008FE2 RID: 36834 RVA: 0x003ADD80 File Offset: 0x003ABF80
		public Transform PlayerRoot
		{
			get
			{
				return this._playerRoot;
			}
		}

		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x06008FE3 RID: 36835 RVA: 0x003ADD88 File Offset: 0x003ABF88
		public Animator PlayerAnimator
		{
			get
			{
				return this._animator;
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x06008FE4 RID: 36836 RVA: 0x003ADD90 File Offset: 0x003ABF90
		public bool IsWorn
		{
			get
			{
				return this._worn;
			}
		}

		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x06008FE5 RID: 36837 RVA: 0x003ADD98 File Offset: 0x003ABF98
		public int Level
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x06008FE6 RID: 36838 RVA: 0x003ADDA0 File Offset: 0x003ABFA0
		public EggRotorPlacement Placement
		{
			get
			{
				return this._placement;
			}
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x06008FE7 RID: 36839 RVA: 0x003ADDA8 File Offset: 0x003ABFA8
		public EggRotorPlacement PhysicalPlacement
		{
			get
			{
				if (!this._worn || !this._rigExpected)
				{
					return this._placement;
				}
				switch (this._phase)
				{
				case EggRotorController.EggPhase.Sliding:
					if (this._insertion < 0.5f)
					{
						return EggRotorPlacement.Outside;
					}
					return EggRotorPlacement.Inside;
				case EggRotorController.EggPhase.Swing:
					return EggRotorPlacement.Swing;
				case EggRotorController.EggPhase.MovingToGlans:
				case EggRotorController.EggPhase.Glans:
					return EggRotorPlacement.Glans;
				default:
					return EggRotorPlacement.Outside;
				}
			}
		}

		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x06008FE8 RID: 36840 RVA: 0x003ADE04 File Offset: 0x003AC004
		public bool IsTransitioning
		{
			get
			{
				if (!this._worn || !this._rigExpected)
				{
					return false;
				}
				switch (this._phase)
				{
				case EggRotorController.EggPhase.Outside:
					return this._placement != EggRotorPlacement.Outside;
				case EggRotorController.EggPhase.MovingToOutside:
				case EggRotorController.EggPhase.MovingToEntry:
				case EggRotorController.EggPhase.MovingToGlans:
					return true;
				case EggRotorController.EggPhase.Sliding:
					return this._placement != EggRotorPlacement.Inside || this._insertion < 1f;
				case EggRotorController.EggPhase.Glans:
					return this._placement != EggRotorPlacement.Glans;
				}
				return this._placement != EggRotorPlacement.Swing;
			}
		}

		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x06008FE9 RID: 36841 RVA: 0x003ADE8E File Offset: 0x003AC08E
		public bool IsEggInserted
		{
			get
			{
				return this._worn && this._rigExpected && this._phase == EggRotorController.EggPhase.Sliding && this._insertion >= 1f;
			}
		}

		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x06008FEA RID: 36842 RVA: 0x003ADEBB File Offset: 0x003AC0BB
		public float InsertionProgress
		{
			get
			{
				if (!this._worn || this._phase != EggRotorController.EggPhase.Sliding)
				{
					return 0f;
				}
				return this._insertion;
			}
		}

		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x06008FEB RID: 36843 RVA: 0x003ADEDA File Offset: 0x003AC0DA
		public bool IsCovered
		{
			get
			{
				return this._covered;
			}
		}

		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x06008FEC RID: 36844 RVA: 0x003ADEE2 File Offset: 0x003AC0E2
		public bool IsCrotchOccupied
		{
			get
			{
				return this._toy != null && this._toy.IsInserted && this._toy.ActiveInsertionSite == InsertionSite.Vaginal;
			}
		}

		// Token: 0x1700170A RID: 5898
		// (get) Token: 0x06008FED RID: 36845 RVA: 0x003ADF10 File Offset: 0x003AC110
		public bool IsNippleRotorWorn
		{
			get
			{
				NippleRotorController nippleRotorController = NippleRotorController.Current;
				return (nippleRotorController != null && nippleRotorController.IsWorn) || (this._wear != null && this._wear.GetWorn("nipple_rotor", false) != null);
			}
		}

		// Token: 0x1700170B RID: 5899
		// (get) Token: 0x06008FEE RID: 36846 RVA: 0x003ADF5D File Offset: 0x003AC15D
		public bool IsSuspended
		{
			get
			{
				return Time.timeScale <= 0f || this.IsInterrupted;
			}
		}

		// Token: 0x1700170C RID: 5900
		// (get) Token: 0x06008FEF RID: 36847 RVA: 0x003ADF74 File Offset: 0x003AC174
		public bool IsInterrupted
		{
			get
			{
				if (DetectionInteractionGate.IsGameOver)
				{
					return true;
				}
				ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
				return (instance != null && instance.isActiveAndEnabled && instance.IsEventActive) || GameplayInputGate.IsLoading;
			}
		}

		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x06008FF0 RID: 36848 RVA: 0x003ADFB0 File Offset: 0x003AC1B0
		public bool IsVibrating
		{
			get
			{
				return this._worn && this._level > 0 && !this.IsSuspended;
			}
		}

		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x06008FF1 RID: 36849 RVA: 0x003ADFCE File Offset: 0x003AC1CE
		public bool IsStimulating
		{
			get
			{
				return this.IsVibrating && EggRotorIds.Stimulates(this.PhysicalPlacement);
			}
		}

		// Token: 0x1700170F RID: 5903
		// (get) Token: 0x06008FF2 RID: 36850 RVA: 0x003ADFE5 File Offset: 0x003AC1E5
		public bool IsFutanariMode
		{
			get
			{
				return this._futaMode;
			}
		}

		// Token: 0x17001710 RID: 5904
		// (get) Token: 0x06008FF3 RID: 36851 RVA: 0x003ADFED File Offset: 0x003AC1ED
		public bool IsOnGlans
		{
			get
			{
				return this._worn && this._rigExpected && this.IsGlansPhase;
			}
		}

		// Token: 0x17001711 RID: 5905
		// (get) Token: 0x06008FF4 RID: 36852 RVA: 0x003AE007 File Offset: 0x003AC207
		private bool IsGlansPhase
		{
			get
			{
				return this._phase == EggRotorController.EggPhase.Glans || this._phase == EggRotorController.EggPhase.MovingToGlans;
			}
		}

		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x06008FF5 RID: 36853 RVA: 0x003AE020 File Offset: 0x003AC220
		public bool IsPenisVisible
		{
			get
			{
				return this._futa != null && this._futa.IsEffectivelyVisible && this._penis != null && this._penis.enabled && this._penis.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x06008FF6 RID: 36854 RVA: 0x003AE075 File Offset: 0x003AC275
		public SkinnedMeshRenderer PenisRenderer
		{
			get
			{
				return this._penis;
			}
		}

		// Token: 0x17001714 RID: 5908
		// (get) Token: 0x06008FF7 RID: 36855 RVA: 0x003AE07D File Offset: 0x003AC27D
		public Transform PenisTip
		{
			get
			{
				return this._penisTip;
			}
		}

		// Token: 0x17001715 RID: 5909
		// (get) Token: 0x06008FF8 RID: 36856 RVA: 0x003AE085 File Offset: 0x003AC285
		public Transform GlansFrame
		{
			get
			{
				return this._glansMount;
			}
		}

		// Token: 0x06008FF9 RID: 36857 RVA: 0x003AE090 File Offset: 0x003AC290
		public bool TryGetPenisTip(out Vector3 point, out Vector3 axis)
		{
			point = this._glans.TipPoint;
			axis = this._glans.TipAxis;
			return this._glansTipFrame >= 0 && Time.frameCount - this._glansTipFrame <= 1 && this._glans.HasTip;
		}

		// Token: 0x17001716 RID: 5910
		// (get) Token: 0x06008FFA RID: 36858 RVA: 0x003AE0E4 File Offset: 0x003AC2E4
		public bool IsRequestingErection
		{
			get
			{
				return this._erectionRequested;
			}
		}

		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x06008FFB RID: 36859 RVA: 0x003AE0EC File Offset: 0x003AC2EC
		// (set) Token: 0x06008FFC RID: 36860 RVA: 0x003AE0F4 File Offset: 0x003AC2F4
		public bool HoldsClimaxFace { get; set; }

		// Token: 0x17001718 RID: 5912
		// (get) Token: 0x06008FFD RID: 36861 RVA: 0x003AE0FD File Offset: 0x003AC2FD
		public float BodyScale
		{
			get
			{
				return this._bodyScale;
			}
		}

		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x06008FFE RID: 36862 RVA: 0x003AE105 File Offset: 0x003AC305
		public GameObject Egg
		{
			get
			{
				return this._egg;
			}
		}

		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x06008FFF RID: 36863 RVA: 0x003AE10D File Offset: 0x003AC30D
		public GameObject Remote
		{
			get
			{
				return this._remote;
			}
		}

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x06009000 RID: 36864 RVA: 0x003AE115 File Offset: 0x003AC315
		public NippleRotorCord Cord
		{
			get
			{
				return this._cord;
			}
		}

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x06009001 RID: 36865 RVA: 0x003AE11D File Offset: 0x003AC31D
		public Transform Anchor
		{
			get
			{
				return this._anchor;
			}
		}

		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x06009002 RID: 36866 RVA: 0x003AE128 File Offset: 0x003AC328
		private EggRotorRigSettings Rig
		{
			get
			{
				if (this._config != null && this._config.rig != null)
				{
					return this._config.rig;
				}
				EggRotorRigSettings eggRotorRigSettings;
				if ((eggRotorRigSettings = this._fallbackRig) == null)
				{
					eggRotorRigSettings = (this._fallbackRig = new EggRotorRigSettings());
				}
				return eggRotorRigSettings;
			}
		}

		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x06009003 RID: 36867 RVA: 0x003AE174 File Offset: 0x003AC374
		private string BlendShapeSourceId
		{
			get
			{
				string text;
				if ((text = this._blendShapeSourceId) == null)
				{
					text = (this._blendShapeSourceId = "EggRotor." + base.GetInstanceID().ToString());
				}
				return text;
			}
		}

		// Token: 0x06009004 RID: 36868 RVA: 0x003AE1AC File Offset: 0x003AC3AC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			EggRotorController.EnabledControllers.Clear();
			EggRotorController.LevelChanged = null;
			EggRotorController.PlacementChanged = null;
		}

		// Token: 0x06009005 RID: 36869 RVA: 0x003AE1C4 File Offset: 0x003AC3C4
		public void Configure(EggRotorConfigSO config, PlayerInventoryManager inventory)
		{
			Animator animator = ((inventory != null) ? inventory.PlayerAnimator : null);
			if (config == this._config && inventory == this._inventory && animator == this._animator && this._playerRoot != null)
			{
				return;
			}
			this.SetWorn(false);
			this.BindWearController(null);
			this.ClearNoise();
			this.ReleaseBlendShape();
			this.ReleaseErection();
			this._config = config;
			this._inventory = inventory;
			this._animator = animator;
			this._playerRoot = ((animator != null) ? animator.transform : null);
			this._actor = null;
			this._clothing = null;
			this._toy = null;
			this._body = null;
			this.ForgetFuta();
			this._faceConfiguredFor = null;
			this._warnedMissingAssets = false;
			this._warnedAnchor = false;
			if (!this._placementInitialized && config != null)
			{
				this._placement = EggRotorIds.ClampPlacement((int)config.defaultPlacement);
				this._placementInitialized = true;
			}
			this.ResolveActor();
			this.ResolveFace();
			this.ResolveClothing();
			this.ResolveToy();
			this.ResolveFuta();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.BindWearController(this.ResolveWearController());
			this.ReconcileWorn();
		}

		// Token: 0x06009006 RID: 36870 RVA: 0x003AE303 File Offset: 0x003AC503
		public void SetLevel(int level, bool playSwitch = true)
		{
			this.SetLevelInternal(level, playSwitch);
		}

		// Token: 0x06009007 RID: 36871 RVA: 0x003AE30D File Offset: 0x003AC50D
		public void CycleLevel()
		{
			if (!this._worn)
			{
				return;
			}
			this.SetLevel(EggRotorIds.NextLevel(this._level), true);
		}

		// Token: 0x06009008 RID: 36872 RVA: 0x003AE32C File Offset: 0x003AC52C
		public bool SetPlacement(EggRotorPlacement placement, bool instant = false)
		{
			placement = EggRotorIds.ForMode(placement, this._futaMode, this._classicCrotch);
			if (EggRotorIds.UsesCrotch(placement) && this.IsCrotchOccupied)
			{
				placement = EggRotorPlacement.Swing;
			}
			this._placementInitialized = true;
			bool flag = this.SetPlacementInternal(placement);
			if (instant && this._worn && this._rigExpected)
			{
				this.SnapToPlacement();
			}
			return flag;
		}

		// Token: 0x06009009 RID: 36873 RVA: 0x003AE387 File Offset: 0x003AC587
		public void CyclePlacement()
		{
			if (!this._worn)
			{
				return;
			}
			this.SetPlacement(EggRotorIds.NextPlacement(this._placement, !this.IsCrotchOccupied, this._futaMode), false);
		}

		// Token: 0x0600900A RID: 36874 RVA: 0x003AE3B4 File Offset: 0x003AC5B4
		private void Awake()
		{
			this.ResolveFace();
		}

		// Token: 0x0600900B RID: 36875 RVA: 0x003AE3BC File Offset: 0x003AC5BC
		private void OnEnable()
		{
			if (!EggRotorController.EnabledControllers.Contains(this))
			{
				EggRotorController.EnabledControllers.Add(this);
			}
			this._housekeepingTimer = 0f;
			this.HookPostSimulation(true);
			if (this._config == null || this._playerRoot == null)
			{
				return;
			}
			this.BindWearController(this.ResolveWearController());
			this.ReconcileWorn();
		}

		// Token: 0x0600900C RID: 36876 RVA: 0x003AE424 File Offset: 0x003AC624
		private void OnDisable()
		{
			EggRotorController.EnabledControllers.Remove(this);
			this.HookPostSimulation(false);
			this.BindWearController(null);
			this.SetWorn(false);
			this.ClearNoise();
			this.ReleaseBlendShape();
			this.ReleaseErection();
			if (this._face != null)
			{
				this._face.SetWanted(false);
			}
		}

		// Token: 0x0600900D RID: 36877 RVA: 0x003AE47E File Offset: 0x003AC67E
		private void OnDestroy()
		{
			EggRotorController.EnabledControllers.Remove(this);
			GameplayEmoteController.SetMasturbationEmotesBlocked(this, false, null);
			this.HookPostSimulation(false);
			this.BindWearController(null);
			this.ClearNoise();
			this.ReleaseBlendShape();
			this.ReleaseErection();
			this.DespawnRig();
		}

		// Token: 0x0600900E RID: 36878 RVA: 0x003AE4BC File Offset: 0x003AC6BC
		private void Update()
		{
			if (this._config == null || this._playerRoot == null)
			{
				return;
			}
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			this._housekeepingTimer -= unscaledDeltaTime;
			if (this._housekeepingTimer <= 0f)
			{
				this._housekeepingTimer = 0.5f;
				this.Housekeeping();
			}
			if (this._level != 0 && DetectionInteractionGate.IsGameOver)
			{
				this.SetLevelInternal(0, false);
			}
			this._covered = this._worn && this.ReadBottomCovered();
			this.RefreshFutaMode();
			if (this._worn && this._rigExpected)
			{
				this.EnforceFreeCrotch();
				this.TickPlacement(Time.deltaTime);
				this.ApplyRigPoses();
				this.UpdateBlendShape(Time.deltaTime);
				this.UpdateTapeShape();
				this.ApplyVisibility();
			}
			this.UpdateErection();
			bool isVibrating = this.IsVibrating;
			this.UpdateNoise(isVibrating);
			this.UpdateNpcTargets(isVibrating);
			this.UpdateFace(this._worn && (this._level > 0 || EggRotorController.NippleRotorWantsFace() || this.HoldsClimaxFace) && !this.IsInterrupted);
			this.UpdateBuzz(isVibrating);
			this.UpdateAudio(isVibrating, unscaledDeltaTime);
		}

		// Token: 0x0600900F RID: 36879 RVA: 0x003AE5E8 File Offset: 0x003AC7E8
		private void LateUpdate()
		{
			if (this._config == null || !this._worn || !this._rigExpected)
			{
				return;
			}
			if (this._phase == EggRotorController.EggPhase.Swing)
			{
				this.ApplySwingEggPose();
				if (this._cord == null || !this._cord.IsConfigured)
				{
					this.PlaceSwingFallback();
					return;
				}
			}
			else
			{
				if (this.IsGlansPhase && this._glansPostSimulationFrame < Time.frameCount - 1)
				{
					this.EvaluateGlansMount();
				}
				this.UpdateCableEnd();
			}
		}

		// Token: 0x06009010 RID: 36880 RVA: 0x003AE66C File Offset: 0x003AC86C
		private void HandlePostSimulation()
		{
			if (this._config == null || !this._worn || !this._rigExpected || !this.IsGlansPhase)
			{
				return;
			}
			if (!this.EvaluateGlansMount())
			{
				return;
			}
			this._glansPostSimulationFrame = Time.frameCount;
			this.UpdateCableEnd();
		}

		// Token: 0x06009011 RID: 36881 RVA: 0x003AE6BC File Offset: 0x003AC8BC
		private void HookPostSimulation(bool hook)
		{
			if (hook == this._postSimulationHooked)
			{
				return;
			}
			if (this._postSimulationHandler == null)
			{
				this._postSimulationHandler = new Action(this.HandlePostSimulation);
			}
			if (hook)
			{
				MagicaManager.OnPostSimulation = (Action)Delegate.Combine(MagicaManager.OnPostSimulation, this._postSimulationHandler);
			}
			else
			{
				MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, this._postSimulationHandler);
			}
			this._postSimulationHooked = hook;
		}

		// Token: 0x06009012 RID: 36882 RVA: 0x003AE72D File Offset: 0x003AC92D
		private WearableUseController ResolveWearController()
		{
			if (!(this._playerRoot != null))
			{
				return null;
			}
			return this._playerRoot.GetComponentInChildren<WearableUseController>(true);
		}

		// Token: 0x06009013 RID: 36883 RVA: 0x003AE74C File Offset: 0x003AC94C
		private void BindWearController(WearableUseController wear)
		{
			if (this._wear != null)
			{
				this._wear.OnItemWorn -= this.HandleItemWorn;
				this._wear.OnItemUnworn -= this.HandleItemUnworn;
			}
			this._wear = wear;
			if (this._wear == null || !base.isActiveAndEnabled)
			{
				return;
			}
			this._wear.OnItemWorn += this.HandleItemWorn;
			this._wear.OnItemUnworn += this.HandleItemUnworn;
		}

		// Token: 0x06009014 RID: 36884 RVA: 0x003AE7E1 File Offset: 0x003AC9E1
		private static bool IsRotorItem(WearableItemSO item)
		{
			return item != null && string.Equals(item.itemId, "egg_rotor", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06009015 RID: 36885 RVA: 0x003AE7FF File Offset: 0x003AC9FF
		private bool IsRotorWorn()
		{
			return this._wear != null && this._wear.GetWorn("egg_rotor", false) != null;
		}

		// Token: 0x06009016 RID: 36886 RVA: 0x003AE828 File Offset: 0x003ACA28
		private void HandleItemWorn(WearableItemSO item)
		{
			if (EggRotorController.IsRotorItem(item))
			{
				this.SetWorn(true);
			}
		}

		// Token: 0x06009017 RID: 36887 RVA: 0x003AE839 File Offset: 0x003ACA39
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (EggRotorController.IsRotorItem(item))
			{
				this.SetWorn(this.IsRotorWorn());
			}
		}

		// Token: 0x06009018 RID: 36888 RVA: 0x003AE84F File Offset: 0x003ACA4F
		private void ReconcileWorn()
		{
			this.SetWorn(this._config != null && this.IsRotorWorn());
		}

		// Token: 0x06009019 RID: 36889 RVA: 0x003AE870 File Offset: 0x003ACA70
		private void SetWorn(bool worn)
		{
			if (this._worn == worn)
			{
				return;
			}
			this._worn = worn;
			this.UpdateMasturbationEmoteBlock();
			if (worn)
			{
				this.RefreshFutaMode();
				if (this.IsCrotchOccupied && EggRotorIds.UsesCrotch(this._placement))
				{
					this.SetPlacementInternal(EggRotorPlacement.Swing);
				}
				this._covered = this.ReadBottomCovered();
				this.SpawnRig(true);
				return;
			}
			this.SetLevelInternal(0, false);
			this.ReleaseErection();
			this.DespawnRig();
			this.ClearNoise();
			this._covered = false;
			if (this._face != null)
			{
				this._face.SetWanted(false);
			}
			this.StopAudioImmediate();
		}

		// Token: 0x0600901A RID: 36890 RVA: 0x003AE910 File Offset: 0x003ACB10
		private void UpdateMasturbationEmoteBlock()
		{
			string text = ((this._config != null && !string.IsNullOrEmpty(this._config.masturbationEmotesBlockedMessage)) ? this._config.masturbationEmotesBlockedMessage : "Remove the Egg Rotor first");
			GameplayEmoteController.SetMasturbationEmotesBlocked(this, this._worn, text);
		}

		// Token: 0x0600901B RID: 36891 RVA: 0x003AE960 File Offset: 0x003ACB60
		private void SetLevelInternal(int level, bool playSwitch)
		{
			level = EggRotorIds.ClampLevel(level);
			if (!this._worn || DetectionInteractionGate.IsGameOver)
			{
				level = 0;
			}
			if (level == this._level)
			{
				return;
			}
			int level2 = this._level;
			this._level = level;
			if (playSwitch)
			{
				this.PlaySwitch();
			}
			if (level > level2 && !this.IsSuspended)
			{
				this.Shake();
			}
			Action<int> levelChanged = EggRotorController.LevelChanged;
			if (levelChanged == null)
			{
				return;
			}
			levelChanged(level);
		}

		// Token: 0x0600901C RID: 36892 RVA: 0x003AE9CA File Offset: 0x003ACBCA
		private bool SetPlacementInternal(EggRotorPlacement placement)
		{
			if (EggRotorIds.UsesCrotch(placement))
			{
				this._classicCrotch = placement;
			}
			if (placement == this._placement)
			{
				return false;
			}
			this._placement = placement;
			Action<EggRotorPlacement> placementChanged = EggRotorController.PlacementChanged;
			if (placementChanged != null)
			{
				placementChanged(placement);
			}
			return true;
		}

		// Token: 0x0600901D RID: 36893 RVA: 0x003AEA00 File Offset: 0x003ACC00
		private void Housekeeping()
		{
			if (this._wear == null)
			{
				this.BindWearController(this.ResolveWearController());
			}
			if (this._actor == null)
			{
				this.ResolveActor();
			}
			if (this._face == null)
			{
				this.ResolveFace();
			}
			if (this._clothing == null)
			{
				this.ResolveClothing();
			}
			if (this._toy == null)
			{
				this.ResolveToy();
			}
			if (this._futa == null || this._penis == null || this._erection == null)
			{
				this.ResolveFuta();
			}
			if (this._climax == null)
			{
				this._climax = base.GetComponent<EggRotorClimax>();
			}
			if (this._body == null)
			{
				this._body = this.FindBodyRenderer();
			}
			this._bodyScale = this.ResolveBodyScale();
			this.ReconcileWorn();
			if (!this._worn)
			{
				return;
			}
			if (this._rigExpected && this.IsRigLost())
			{
				this.SpawnRig(false);
			}
			if (this._cordObject != null)
			{
				EggRotorController.SetLayerRecursive(this._cordObject.transform, this._rigLayer);
			}
			if (this._blendShapeActive && !this.IsBlendShapeHeld())
			{
				this.DropBlendShapeOverride();
			}
		}

		// Token: 0x0600901E RID: 36894 RVA: 0x003AEB48 File Offset: 0x003ACD48
		private bool IsRigLost()
		{
			return this._anchor == null || this._thigh == null || this._cableEnd == null || this._swingEnd == null || this._glansMount == null || (this._eggExpected && this._egg == null) || (this._remoteExpected && this._remote == null);
		}

		// Token: 0x0600901F RID: 36895 RVA: 0x003AEBCC File Offset: 0x003ACDCC
		private void SpawnRig(bool animateInsertion)
		{
			this.DespawnRig();
			if (this._config == null || this._playerRoot == null)
			{
				return;
			}
			if (this._body == null)
			{
				this._body = this.FindBodyRenderer();
			}
			this._bodyScale = this.ResolveBodyScale();
			this._rigLayer = this.ResolveRigLayer();
			this._anchor = this.ResolveAnchor();
			this._thigh = this.ResolveThigh();
			this._rigExpected = true;
			this._cableEnd = this.CreateMarker("EggRotorCableEnd", this._anchor);
			this._swingEnd = this.CreateMarker("EggRotorSwingEnd", base.transform);
			this._glansMount = this.CreateMarker("EggRotorGlansMount", base.transform);
			this._glansPostSimulationFrame = -1;
			if (this._config.remotePrefab != null)
			{
				this.SpawnRemote();
			}
			if (this._config.eggPrefab != null)
			{
				this.SpawnEgg();
			}
			if (this._remoteSocket != null)
			{
				this.SpawnCord();
			}
			else if (this._remote != null)
			{
				Debug.LogWarning("[EggRotor] The remote has no \"CordSocket\" child; the cable is skipped.", this);
			}
			this.ApplyRigPoses();
			this.EnterPlacement(animateInsertion);
			this._appliedVisibility = -1;
			this.ApplyVisibility();
			this.RefreshPoiProbeLighting();
			if ((this._config.eggPrefab == null || this._config.remotePrefab == null) && !this._warnedMissingAssets)
			{
				this._warnedMissingAssets = true;
				Debug.LogWarning("[EggRotor] The config is missing the egg or remote prefab; run Tools/Egg Rotor/Build Assets.", this._config);
			}
		}

		// Token: 0x06009020 RID: 36896 RVA: 0x003AED60 File Offset: 0x003ACF60
		private void RefreshPoiProbeLighting()
		{
			Transform transform = ((PlayerProximityProvider.Instance != null) ? PlayerProximityProvider.Instance.Player : null);
			if (transform == null)
			{
				return;
			}
			Scene activeScene = SceneManager.GetActiveScene();
			foreach (PoiCharacterProbeLighting poiCharacterProbeLighting in Object.FindObjectsByType<PoiCharacterProbeLighting>(FindObjectsSortMode.None))
			{
				if (poiCharacterProbeLighting.isActiveAndEnabled && poiCharacterProbeLighting.gameObject.scene == activeScene)
				{
					poiCharacterProbeLighting.ApplyToPlayer(transform);
				}
			}
		}

		// Token: 0x06009021 RID: 36897 RVA: 0x003AEDD7 File Offset: 0x003ACFD7
		private Transform CreateMarker(string objectName, Transform parent)
		{
			GameObject gameObject = new GameObject(objectName);
			gameObject.layer = this._rigLayer;
			gameObject.transform.SetParent(parent, false);
			return gameObject.transform;
		}

		// Token: 0x06009022 RID: 36898 RVA: 0x003AEE00 File Offset: 0x003AD000
		private void SpawnRemote()
		{
			this._remote = Object.Instantiate<GameObject>(this._config.remotePrefab, this._thigh, false);
			this._remote.name = this._config.remotePrefab.name;
			this._remoteExpected = true;
			Transform transform = this._remote.transform;
			this._remoteBase = new Pose(transform.localPosition, transform.localRotation);
			this._remoteBaseScale = transform.localScale;
			EggRotorController.MakeNonPhysical(this._remote);
			FirstPersonVisibleProp.Mark(this._remote);
			EggRotorController.SetLayerRecursive(this._remote.transform, this._rigLayer);
			this._remoteBuzz = EggRotorController.FindBuzz(this._remote.transform);
			this._remoteSocket = EggRotorController.FindDeep(this._remote.transform, "CordSocket");
			this._remote.GetComponentsInChildren<Renderer>(true, this._remoteRenderers);
		}

		// Token: 0x06009023 RID: 36899 RVA: 0x003AEEEC File Offset: 0x003AD0EC
		private void SpawnEgg()
		{
			this._egg = Object.Instantiate<GameObject>(this._config.eggPrefab, this._anchor, false);
			this._egg.name = this._config.eggPrefab.name;
			this._eggExpected = true;
			float num = Mathf.Abs(this._egg.transform.localScale.x);
			this._eggBaseScale = ((num > 1E-06f && !float.IsInfinity(num)) ? num : 1f);
			EggRotorController.MakeNonPhysical(this._egg);
			FirstPersonVisibleProp.Mark(this._egg);
			EggRotorController.SetLayerRecursive(this._egg.transform, this._rigLayer);
			this._eggBuzz = EggRotorController.FindBuzz(this._egg.transform);
			this._eggSocket = EggRotorController.FindDeep(this._egg.transform, "CordSocket");
			Transform transform = this._egg.transform;
			this._hasSocketPose = this._eggSocket != null;
			if (this._hasSocketPose)
			{
				this._socketInEgg = transform.InverseTransformPoint(this._eggSocket.position);
				this._socketRotationInEgg = Quaternion.Inverse(transform.rotation) * this._eggSocket.rotation;
			}
			this._egg.GetComponentsInChildren<Renderer>(true, this._eggRenderers);
			MeshFilter meshFilter = ((EggRotorController.FindDeep(transform, "Egg") != null) ? EggRotorController.FindDeep(transform, "Egg").GetComponent<MeshFilter>() : null);
			if (meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.bounds.extents.x > 0.0001f)
			{
				this._eggRadiusLocal = meshFilter.sharedMesh.bounds.extents.x;
			}
			Transform transform2 = EggRotorController.FindDeep(transform, "Tape");
			if (transform2 == null)
			{
				return;
			}
			transform2.GetComponentsInChildren<Renderer>(true, this._tapeRenderers);
			for (int i = 0; i < this._tapeRenderers.Count; i++)
			{
				this._eggRenderers.Remove(this._tapeRenderers[i]);
			}
			transform2.GetComponentsInChildren<MeshFilter>(true, this._tapeFilters);
			for (int j = 0; j < this._tapeFilters.Count; j++)
			{
				Mesh mesh = ((this._tapeFilters[j] != null) ? this._tapeFilters[j].sharedMesh : null);
				this._tapeSourceMeshes.Add(mesh);
				this._tapeWrappedMeshes.Add(null);
				bool flag = mesh != null && mesh.isReadable;
				Vector3[] array = (flag ? mesh.vertices : null);
				Vector3[] array2 = (flag ? mesh.normals : null);
				if (array2 != null && array != null && array2.Length != array.Length)
				{
					array2 = null;
				}
				this._tapeSourceVertices.Add(array);
				this._tapeSourceNormals.Add(array2);
				this._tapeWrapVertices.Add((array != null) ? new Vector3[array.Length] : null);
				this._tapeWrapNormals.Add((array2 != null) ? new Vector3[array2.Length] : null);
			}
		}

		// Token: 0x06009024 RID: 36900 RVA: 0x003AF21C File Offset: 0x003AD41C
		private void SpawnCord()
		{
			this._cordObject = new GameObject("EggRotorCord");
			this._cordObject.layer = this._rigLayer;
			this._cordObject.transform.SetParent(base.transform, false);
			this._cord = this._cordObject.AddComponent<NippleRotorCord>();
			EggRotorController.SetLayerRecursive(this._cordObject.transform, this._rigLayer);
		}

		// Token: 0x06009025 RID: 36901 RVA: 0x003AF288 File Offset: 0x003AD488
		private void ConfigureCord(bool swing)
		{
			if (this._cord == null || this._remoteSocket == null || this._config == null)
			{
				return;
			}
			NippleRotorCordSettings nippleRotorCordSettings = (swing ? (this._config.swingCord ?? EggRotorConfigSO.CreateSwingCord()) : (this._config.attachedCord ?? EggRotorConfigSO.CreateAttachedCord()));
			if (!swing)
			{
				this.UpdateCableEnd();
			}
			Transform transform = (swing ? this._swingEnd : null);
			this._cord.FreeEndFollower = transform;
			this._cord.ConfigureSingle(nippleRotorCordSettings, this._config.cordMaterial, this._remoteSocket, swing ? null : this._cableEnd, this._playerRoot, this._animator);
			this._cord.FreeEndFollower = transform;
			EggRotorController.SetLayerRecursive(this._cordObject.transform, this._rigLayer);
			this._cord.SetHidden(this._covered);
		}

		// Token: 0x06009026 RID: 36902 RVA: 0x003AF378 File Offset: 0x003AD578
		private void DespawnRig()
		{
			this.ReleaseBlendShape();
			this.RestoreTape();
			for (int i = 0; i < this._tapeWrappedMeshes.Count; i++)
			{
				if (this._tapeWrappedMeshes[i] != null)
				{
					EggRotorController.DestroyRuntime(this._tapeWrappedMeshes[i]);
				}
			}
			if (this._cordObject != null)
			{
				EggRotorController.DestroyRuntime(this._cordObject);
			}
			if (this._egg != null)
			{
				EggRotorController.DestroyRuntime(this._egg);
			}
			if (this._remote != null)
			{
				EggRotorController.DestroyRuntime(this._remote);
			}
			if (this._cableEnd != null)
			{
				EggRotorController.DestroyRuntime(this._cableEnd.gameObject);
			}
			if (this._swingEnd != null)
			{
				EggRotorController.DestroyRuntime(this._swingEnd.gameObject);
			}
			if (this._glansMount != null)
			{
				EggRotorController.DestroyRuntime(this._glansMount.gameObject);
			}
			this._cordObject = null;
			this._cord = null;
			this._egg = null;
			this._remote = null;
			this._eggSocket = (this._remoteSocket = null);
			this._cableEnd = (this._swingEnd = null);
			this._glansMount = null;
			this._glansPostSimulationFrame = -1;
			this._tapeFilters.Clear();
			this._tapeSourceMeshes.Clear();
			this._tapeWrappedMeshes.Clear();
			this._tapeSourceVertices.Clear();
			this._tapeSourceNormals.Clear();
			this._tapeWrapVertices.Clear();
			this._tapeWrapNormals.Clear();
			this._glansTipFrame = -1;
			this._tapeWrapped = false;
			this._tapeWrapRadius = -1f;
			this._eggBuzz = (this._remoteBuzz = null);
			this._anchor = (this._thigh = null);
			this._hasSocketPose = false;
			this._remoteBase = Pose.identity;
			this._remoteBaseScale = Vector3.one;
			this._eggBaseScale = 1f;
			this._eggRenderers.Clear();
			this._tapeRenderers.Clear();
			this._remoteRenderers.Clear();
			this._appliedVisibility = -1;
			this._rigExpected = (this._eggExpected = (this._remoteExpected = false));
			this._buzzApplied = false;
			this._phase = EggRotorController.EggPhase.Outside;
			this._insertion = 0f;
			this._blend = 0f;
			this._snapOpening = false;
			this._openingCovered = false;
		}

		// Token: 0x06009027 RID: 36903 RVA: 0x003AF5DC File Offset: 0x003AD7DC
		private Transform ResolveAnchor()
		{
			Transform transform = EggRotorController.FindDeep(this._playerRoot, "PussyAnchor");
			if (transform != null)
			{
				return transform;
			}
			if (!this._warnedAnchor)
			{
				this._warnedAnchor = true;
				Debug.LogWarning("[EggRotor] PussyAnchor not found under the player; the egg uses the hips and its calibration will be off.", this);
			}
			Transform transform2 = ((this._animator != null && this._animator.isHuman) ? this._animator.GetBoneTransform(HumanBodyBones.Hips) : null);
			if (!(transform2 != null))
			{
				return this._playerRoot;
			}
			return transform2;
		}

		// Token: 0x06009028 RID: 36904 RVA: 0x003AF65C File Offset: 0x003AD85C
		private Transform ResolveThigh()
		{
			bool flag = this._animator != null && this._animator.isHuman;
			Transform transform = (flag ? this._animator.GetBoneTransform(HumanBodyBones.Hips) : null);
			Transform transform2 = ((transform != null) ? EggRotorController.FindDeep(transform, "thigh_stretch.r") : null);
			if (transform2 == null)
			{
				transform2 = EggRotorController.FindDeep(this._playerRoot, "thigh_stretch.r");
			}
			if (transform2 == null && flag)
			{
				transform2 = this._animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
			}
			if (transform2 != null)
			{
				return transform2;
			}
			if (!(transform != null))
			{
				return this._playerRoot;
			}
			return transform;
		}

		// Token: 0x06009029 RID: 36905 RVA: 0x003AF700 File Offset: 0x003AD900
		private int ResolveRigLayer()
		{
			if (this._body != null)
			{
				return this._body.gameObject.layer;
			}
			int num = LayerMask.NameToLayer("PlayerClothing");
			if (num >= 0)
			{
				return num;
			}
			if (!(this._playerRoot != null))
			{
				return base.gameObject.layer;
			}
			return this._playerRoot.gameObject.layer;
		}

		// Token: 0x0600902A RID: 36906 RVA: 0x003AF768 File Offset: 0x003AD968
		private float ResolveBodyScale()
		{
			float num = ((this._body != null) ? Mathf.Abs(this._body.transform.lossyScale.x) : 0f);
			if (num <= 1E-06f && this._playerRoot != null)
			{
				num = Mathf.Abs(this._playerRoot.lossyScale.x);
			}
			if (num <= 1E-06f || float.IsInfinity(num))
			{
				return 1f;
			}
			return num;
		}

		// Token: 0x0600902B RID: 36907 RVA: 0x003AF7E8 File Offset: 0x003AD9E8
		private SkinnedMeshRenderer FindBodyRenderer()
		{
			if (this._playerRoot == null)
			{
				return null;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = null;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in this._playerRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer2 == null) && !(skinnedMeshRenderer2.name != "Body_sotai_NSFW"))
				{
					if (skinnedMeshRenderer2.gameObject.activeInHierarchy)
					{
						return skinnedMeshRenderer2;
					}
					if (skinnedMeshRenderer == null)
					{
						skinnedMeshRenderer = skinnedMeshRenderer2;
					}
				}
			}
			return skinnedMeshRenderer;
		}

		// Token: 0x0600902C RID: 36908 RVA: 0x003AF85C File Offset: 0x003ADA5C
		private static NippleRotorBuzz FindBuzz(Transform root)
		{
			if (root == null)
			{
				return null;
			}
			Transform transform = EggRotorController.FindDeep(root, "Buzz");
			NippleRotorBuzz nippleRotorBuzz = ((transform != null) ? transform.GetComponent<NippleRotorBuzz>() : null);
			if (nippleRotorBuzz == null)
			{
				nippleRotorBuzz = root.GetComponentInChildren<NippleRotorBuzz>(true);
			}
			if (!(nippleRotorBuzz != null) || !(nippleRotorBuzz.transform != root))
			{
				return null;
			}
			return nippleRotorBuzz;
		}

		// Token: 0x0600902D RID: 36909 RVA: 0x003AF8C0 File Offset: 0x003ADAC0
		private static void MakeNonPhysical(GameObject prop)
		{
			foreach (Collider collider in prop.GetComponentsInChildren<Collider>(true))
			{
				if (collider != null)
				{
					collider.enabled = false;
				}
			}
			foreach (Rigidbody rigidbody in prop.GetComponentsInChildren<Rigidbody>(true))
			{
				if (!(rigidbody == null))
				{
					rigidbody.isKinematic = true;
					rigidbody.useGravity = false;
					rigidbody.detectCollisions = false;
				}
			}
		}

		// Token: 0x0600902E RID: 36910 RVA: 0x003AF934 File Offset: 0x003ADB34
		private static Transform FindDeep(Transform root, string objectName)
		{
			if (root == null)
			{
				return null;
			}
			if (root.name == objectName)
			{
				return root;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform transform = EggRotorController.FindDeep(root.GetChild(i), objectName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x0600902F RID: 36911 RVA: 0x003AF988 File Offset: 0x003ADB88
		private static void SetLayerRecursive(Transform root, int layer)
		{
			if (root == null)
			{
				return;
			}
			root.gameObject.layer = layer;
			for (int i = 0; i < root.childCount; i++)
			{
				EggRotorController.SetLayerRecursive(root.GetChild(i), layer);
			}
		}

		// Token: 0x06009030 RID: 36912 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyRuntime(Object target)
		{
			if (target == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x06009031 RID: 36913 RVA: 0x003AF9CC File Offset: 0x003ADBCC
		private void EnterPlacement(bool animateInsertion)
		{
			this._blend = 0f;
			switch (this._placement)
			{
			case EggRotorPlacement.Inside:
				this._phase = EggRotorController.EggPhase.Sliding;
				this._insertion = (animateInsertion ? 0f : 1f);
				this._snapOpening = !animateInsertion;
				goto IL_0079;
			case EggRotorPlacement.Swing:
				this.HandoffToSwing();
				return;
			case EggRotorPlacement.Glans:
				this.AttachToGlansNow();
				this.ConfigureCord(false);
				return;
			}
			this._phase = EggRotorController.EggPhase.Outside;
			this._insertion = 0f;
			IL_0079:
			this._pose = this.ComputeAttachedPose();
			this.ApplyEggPose();
			this.ConfigureCord(false);
		}

		// Token: 0x06009032 RID: 36914 RVA: 0x003AFA6C File Offset: 0x003ADC6C
		private void SnapToPlacement()
		{
			if (this._placement == EggRotorPlacement.Swing)
			{
				if (this._phase != EggRotorController.EggPhase.Swing)
				{
					this.HandoffToSwing();
				}
				return;
			}
			if (this._placement == EggRotorPlacement.Glans)
			{
				bool flag = this._phase == EggRotorController.EggPhase.Swing;
				this.AttachToGlansNow();
				if (flag)
				{
					this.ConfigureCord(false);
				}
				return;
			}
			bool flag2 = this._phase == EggRotorController.EggPhase.Swing;
			if ((flag2 || this.IsGlansPhase) && this._egg != null && this._anchor != null)
			{
				this._egg.transform.SetParent(this._anchor, false);
			}
			bool flag3 = this._placement == EggRotorPlacement.Inside;
			this._phase = (flag3 ? EggRotorController.EggPhase.Sliding : EggRotorController.EggPhase.Outside);
			this._insertion = (flag3 ? 1f : 0f);
			this._blend = 0f;
			this._snapOpening = true;
			this._pose = this.ComputeAttachedPose();
			this.ApplyEggPose();
			if (flag2)
			{
				this.ConfigureCord(false);
			}
		}

		// Token: 0x06009033 RID: 36915 RVA: 0x003AFB54 File Offset: 0x003ADD54
		private void EnforceFreeCrotch()
		{
			if (!this.IsCrotchOccupied)
			{
				return;
			}
			bool flag = this._phase == EggRotorController.EggPhase.Outside || this._phase == EggRotorController.EggPhase.MovingToOutside || this._phase == EggRotorController.EggPhase.MovingToEntry || this._phase == EggRotorController.EggPhase.Sliding;
			if (this._placement == EggRotorPlacement.Glans)
			{
				if (flag)
				{
					this.BeginAttachToGlans();
				}
				return;
			}
			if (!EggRotorIds.UsesCrotch(this._placement) && !flag)
			{
				return;
			}
			this.SetPlacementInternal(EggRotorPlacement.Swing);
			if (this._phase != EggRotorController.EggPhase.Swing)
			{
				this.HandoffToSwing();
			}
		}

		// Token: 0x06009034 RID: 36916 RVA: 0x003AFBCC File Offset: 0x003ADDCC
		private void TickPlacement(float deltaTime)
		{
			switch (this._phase)
			{
			case EggRotorController.EggPhase.Outside:
				if (this._placement == EggRotorPlacement.Inside)
				{
					this.BeginBlend(EggRotorController.EggPhase.MovingToEntry);
				}
				else if (this._placement == EggRotorPlacement.Swing)
				{
					this.HandoffToSwing();
				}
				else if (this._placement == EggRotorPlacement.Glans)
				{
					this.BeginAttachToGlans();
				}
				break;
			case EggRotorController.EggPhase.MovingToOutside:
				if (this._placement == EggRotorPlacement.Inside)
				{
					this.BeginBlend(EggRotorController.EggPhase.MovingToEntry);
				}
				else if (this._placement == EggRotorPlacement.Swing)
				{
					this.HandoffToSwing();
				}
				else if (this._placement == EggRotorPlacement.Glans)
				{
					this.BeginAttachToGlans();
				}
				else
				{
					this._blend = EggRotorPlacementMath.Advance(this._blend, deltaTime, this._config.placeSeconds);
					if (this._blend >= 1f)
					{
						this._phase = EggRotorController.EggPhase.Outside;
					}
				}
				break;
			case EggRotorController.EggPhase.MovingToEntry:
				if (this._placement == EggRotorPlacement.Outside)
				{
					this.BeginBlend(EggRotorController.EggPhase.MovingToOutside);
				}
				else if (this._placement == EggRotorPlacement.Swing)
				{
					this.HandoffToSwing();
				}
				else if (this._placement == EggRotorPlacement.Glans)
				{
					this.BeginAttachToGlans();
				}
				else
				{
					this._blend = EggRotorPlacementMath.Advance(this._blend, deltaTime, this._config.ApproachSeconds);
					if (this._blend >= 1f)
					{
						this._phase = EggRotorController.EggPhase.Sliding;
						this._insertion = 0f;
					}
				}
				break;
			case EggRotorController.EggPhase.Sliding:
				if (this._placement == EggRotorPlacement.Inside)
				{
					this._insertion = EggRotorPlacementMath.Advance(this._insertion, deltaTime, this._config.SlideInSeconds);
				}
				else
				{
					this._insertion = EggRotorPlacementMath.Retreat(this._insertion, deltaTime, this._config.removeSeconds);
					if (this._insertion <= 0f)
					{
						if (this._placement == EggRotorPlacement.Outside)
						{
							this._pose = this.ComputeAttachedPose();
							this.BeginBlend(EggRotorController.EggPhase.MovingToOutside);
						}
						else if (this._placement == EggRotorPlacement.Glans)
						{
							this._pose = this.ComputeAttachedPose();
							this.ApplyEggPose();
							this.BeginAttachToGlans();
						}
						else
						{
							this.HandoffToSwing();
						}
					}
				}
				break;
			case EggRotorController.EggPhase.Swing:
				if (this._placement != EggRotorPlacement.Swing)
				{
					this.BeginAttachFromSwing();
				}
				break;
			case EggRotorController.EggPhase.MovingToGlans:
				if (this._placement == EggRotorPlacement.Swing)
				{
					this.HandoffToSwing();
				}
				else if (this._placement != EggRotorPlacement.Glans)
				{
					this.BeginAttachFromGlans();
				}
				else
				{
					this._blend = EggRotorPlacementMath.Advance(this._blend, deltaTime, this._config.glansSeconds);
					if (this._blend >= 1f)
					{
						this._phase = EggRotorController.EggPhase.Glans;
					}
				}
				break;
			case EggRotorController.EggPhase.Glans:
				if (this._placement == EggRotorPlacement.Swing)
				{
					this.HandoffToSwing();
				}
				else if (this._placement != EggRotorPlacement.Glans)
				{
					this.BeginAttachFromGlans();
				}
				break;
			}
			if (this._phase != EggRotorController.EggPhase.Swing)
			{
				this._pose = this.ComputeAttachedPose();
			}
		}

		// Token: 0x06009035 RID: 36917 RVA: 0x003AFE80 File Offset: 0x003AE080
		private void BeginBlend(EggRotorController.EggPhase phase)
		{
			this._from = this._pose;
			this._blend = 0f;
			this._phase = phase;
		}

		// Token: 0x06009036 RID: 36918 RVA: 0x003AFEA0 File Offset: 0x003AE0A0
		private Pose ComputeAttachedPose()
		{
			EggRotorRigSettings rig = this.Rig;
			switch (this._phase)
			{
			case EggRotorController.EggPhase.MovingToOutside:
				return EggRotorPlacementMath.Blend(this._from, rig.OutsidePose, EggRotorPlacementMath.Ease(this._blend));
			case EggRotorController.EggPhase.MovingToEntry:
				return EggRotorPlacementMath.Blend(this._from, rig.EntryPose, EggRotorPlacementMath.Ease(this._blend));
			case EggRotorController.EggPhase.Sliding:
				return EggRotorPlacementMath.Blend(rig.EntryPose, rig.InsidePose, EggRotorPlacementMath.Ease(this._insertion));
			case EggRotorController.EggPhase.MovingToGlans:
				return EggRotorPlacementMath.Blend(this._from, rig.GlansPose, EggRotorPlacementMath.Ease(this._blend));
			case EggRotorController.EggPhase.Glans:
				return rig.GlansPose;
			}
			return rig.OutsidePose;
		}

		// Token: 0x06009037 RID: 36919 RVA: 0x003AFF60 File Offset: 0x003AE160
		private void BeginAttachFromSwing()
		{
			if (this._placement == EggRotorPlacement.Glans)
			{
				this.BeginAttachToGlans();
				this.ConfigureCord(false);
				return;
			}
			Pose pose = this.CaptureEggPose(this._anchor, this.Rig.OutsidePose);
			if (this._egg != null && this._anchor != null)
			{
				this._egg.transform.SetParent(this._anchor, false);
			}
			this._pose = pose;
			this.BeginBlend((this._placement == EggRotorPlacement.Inside) ? EggRotorController.EggPhase.MovingToEntry : EggRotorController.EggPhase.MovingToOutside);
			this.ApplyEggPose();
			this.ConfigureCord(false);
		}

		// Token: 0x06009038 RID: 36920 RVA: 0x003AFFF8 File Offset: 0x003AE1F8
		private Pose CaptureEggPose(Transform parent, Pose fallback)
		{
			if (this._egg == null || parent == null)
			{
				return fallback;
			}
			Transform transform = this._egg.transform;
			Vector3 vector = EggRotorPlacementMath.WorldToBodyPosition(parent, transform.position, this._bodyScale);
			Quaternion quaternion = Quaternion.Inverse(parent.rotation) * transform.rotation;
			if (EggRotorPlacementMath.IsFinite(vector) && EggRotorPlacementMath.IsFinite(quaternion) && vector.sqrMagnitude <= 0.36f)
			{
				return new Pose(vector, quaternion);
			}
			return fallback;
		}

		// Token: 0x06009039 RID: 36921 RVA: 0x003B007C File Offset: 0x003AE27C
		private void BeginAttachToGlans()
		{
			this.ReleaseBlendShape();
			this.EvaluateGlansMount();
			Pose pose = this.CaptureEggPose(this._glansMount, this.Rig.GlansPose);
			if (this._egg != null && this._glansMount != null)
			{
				this._egg.transform.SetParent(this._glansMount, false);
			}
			this._insertion = 0f;
			this._pose = pose;
			this.BeginBlend(EggRotorController.EggPhase.MovingToGlans);
			this.ApplyEggPose();
		}

		// Token: 0x0600903A RID: 36922 RVA: 0x003B0100 File Offset: 0x003AE300
		private void BeginAttachFromGlans()
		{
			Pose pose = this.CaptureEggPose(this._anchor, this.Rig.OutsidePose);
			if (this._egg != null && this._anchor != null)
			{
				this._egg.transform.SetParent(this._anchor, false);
			}
			this._pose = pose;
			this.BeginBlend((this._placement == EggRotorPlacement.Inside) ? EggRotorController.EggPhase.MovingToEntry : EggRotorController.EggPhase.MovingToOutside);
			this.ApplyEggPose();
		}

		// Token: 0x0600903B RID: 36923 RVA: 0x003B0178 File Offset: 0x003AE378
		private void AttachToGlansNow()
		{
			this.ReleaseBlendShape();
			this._phase = EggRotorController.EggPhase.Glans;
			this._insertion = 0f;
			this._blend = 0f;
			if (this._egg != null && this._glansMount != null)
			{
				this._egg.transform.SetParent(this._glansMount, false);
			}
			this.EvaluateGlansMount();
			this._pose = this.Rig.GlansPose;
			this.ApplyEggPose();
		}

		// Token: 0x0600903C RID: 36924 RVA: 0x003B01FC File Offset: 0x003AE3FC
		private void HandoffToSwing()
		{
			this.ReleaseBlendShape();
			this._phase = EggRotorController.EggPhase.Swing;
			this._insertion = 0f;
			this._blend = 0f;
			this.PlaceSwingFallback();
			if (this._egg != null && this._swingEnd != null)
			{
				this._egg.transform.SetParent(this._swingEnd, false);
				this.ApplySwingEggPose();
			}
			this.ConfigureCord(true);
		}

		// Token: 0x0600903D RID: 36925 RVA: 0x003B0274 File Offset: 0x003AE474
		private void ApplyRigPoses()
		{
			EggRotorRigSettings rig = this.Rig;
			if (this._remote != null && this._thigh != null && this._remote.transform.parent == this._thigh)
			{
				Transform transform = this._remote.transform;
				transform.localPosition = this._remoteBase.position + rig.remotePosition;
				transform.localRotation = Quaternion.Euler(rig.remoteEuler) * this._remoteBase.rotation;
				transform.localScale = this._remoteBaseScale * Mathf.Max(0.0001f, rig.remoteScale);
			}
			if (this._phase != EggRotorController.EggPhase.Swing)
			{
				this.ApplyEggPose();
			}
		}

		// Token: 0x0600903E RID: 36926 RVA: 0x003B0340 File Offset: 0x003AE540
		private void ApplyEggPose()
		{
			Transform transform = (this.IsGlansPhase ? this._glansMount : this._anchor);
			if (this._egg == null || transform == null || this._egg.transform.parent != transform)
			{
				return;
			}
			EggRotorPlacementMath.ApplyBodyPose(this._egg.transform, transform, this._pose, this._bodyScale, this.EggScale);
		}

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x0600903F RID: 36927 RVA: 0x003B03B7 File Offset: 0x003AE5B7
		private float EggScale
		{
			get
			{
				return Mathf.Max(0.0001f, this.Rig.eggScale) * this._eggBaseScale;
			}
		}

		// Token: 0x06009040 RID: 36928 RVA: 0x003B03D8 File Offset: 0x003AE5D8
		private void ApplySwingEggPose()
		{
			if (this._egg == null || this._swingEnd == null || this._egg.transform.parent != this._swingEnd)
			{
				return;
			}
			EggRotorRigSettings rig = this.Rig;
			float num = EggRotorPlacementMath.LocalScaleFor(this._swingEnd, this._bodyScale, this.EggScale);
			Pose pose = (this._hasSocketPose ? EggRotorPlacementMath.SwingRootPose(this._socketInEgg, this._socketRotationInEgg, num, rig.SwingEndRotation) : new Pose(Vector3.zero, rig.SwingEndRotation));
			Transform transform = this._egg.transform;
			transform.localPosition = pose.position;
			transform.localRotation = pose.rotation;
			transform.localScale = Vector3.one * num;
		}

		// Token: 0x06009041 RID: 36929 RVA: 0x003B04A4 File Offset: 0x003AE6A4
		private void PlaceSwingFallback()
		{
			if (this._swingEnd == null)
			{
				return;
			}
			Transform transform = ((this._remoteSocket != null) ? this._remoteSocket : ((this._remote != null) ? this._remote.transform : this._anchor));
			if (transform == null)
			{
				return;
			}
			Vector3 gravity = Physics.gravity;
			Vector3 vector = ((gravity.sqrMagnitude > 1E-08f) ? gravity.normalized : Vector3.down);
			NippleRotorCordSettings nippleRotorCordSettings = ((this._config != null) ? this._config.swingCord : null);
			float num = Mathf.Max(0.01f, (nippleRotorCordSettings != null) ? nippleRotorCordSettings.mainLength : 0.16f) * this._bodyScale;
			Vector3 vector2 = ((this._playerRoot != null) ? this._playerRoot.forward : Vector3.forward);
			if (Mathf.Abs(Vector3.Dot(vector2, vector)) > 0.99f)
			{
				vector2 = Vector3.right;
			}
			Vector3 vector3 = transform.position + vector * num;
			if (!EggRotorPlacementMath.IsFinite(vector3))
			{
				return;
			}
			this._swingEnd.SetPositionAndRotation(vector3, Quaternion.LookRotation(vector, vector2));
		}

		// Token: 0x06009042 RID: 36930 RVA: 0x003B05D8 File Offset: 0x003AE7D8
		private void UpdateCableEnd()
		{
			if (this._cableEnd == null)
			{
				return;
			}
			Transform transform = ((this._eggSocket != null) ? this._eggSocket : ((this._egg != null) ? this._egg.transform : null));
			if (transform == null)
			{
				return;
			}
			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			if (!EggRotorPlacementMath.IsFinite(position) || !EggRotorPlacementMath.IsFinite(rotation))
			{
				return;
			}
			this._cableEnd.SetPositionAndRotation(position, rotation);
		}

		// Token: 0x06009043 RID: 36931 RVA: 0x003B0660 File Offset: 0x003AE860
		private void ResolveClothing()
		{
			if (this._playerRoot == null)
			{
				return;
			}
			ClothingToggleController clothingToggleController = this._playerRoot.GetComponentInChildren<ClothingToggleController>(true);
			if (clothingToggleController == null)
			{
				clothingToggleController = this._playerRoot.GetComponentInParent<ClothingToggleController>();
			}
			if (clothingToggleController == null)
			{
				PlayerEmotionController instance = PlayerEmotionController.Instance;
				if (instance != null && instance.PlayerAnimator == this._animator)
				{
					clothingToggleController = instance.ClothingController;
				}
			}
			this._clothing = clothingToggleController;
		}

		// Token: 0x06009044 RID: 36932 RVA: 0x003B06D8 File Offset: 0x003AE8D8
		private bool ReadBottomCovered()
		{
			if (this._clothing == null)
			{
				return false;
			}
			bool flag;
			bool flag2;
			this._clothing.GetCurrentPhysicalCoverage(out flag, out flag2);
			return flag2;
		}

		// Token: 0x06009045 RID: 36933 RVA: 0x003B0708 File Offset: 0x003AE908
		private void ResolveToy()
		{
			this._toy = ((this._inventory != null) ? this._inventory.GetComponent<InsertableToyController>() : null);
			if (this._toy == null && this._animator != null)
			{
				this._toy = this._animator.GetComponentInParent<InsertableToyController>(true);
			}
		}

		// Token: 0x06009046 RID: 36934 RVA: 0x003B0768 File Offset: 0x003AE968
		private void ApplyVisibility()
		{
			bool flag = this.IsGlansPhase && !this.IsPenisVisible;
			bool flag2 = this._covered || flag;
			bool flag3 = flag2 || (this._phase != EggRotorController.EggPhase.Outside && this._phase != EggRotorController.EggPhase.Glans);
			bool covered = this._covered;
			if (this._cord != null)
			{
				this._cord.SetHidden(this._covered || flag);
			}
			int num = (flag2 ? 1 : 0) | (flag3 ? 2 : 0) | (covered ? 4 : 0);
			if (num == this._appliedVisibility)
			{
				return;
			}
			this._appliedVisibility = num;
			EggRotorController.SetRenderingOff(this._eggRenderers, flag2);
			EggRotorController.SetRenderingOff(this._tapeRenderers, flag3);
			EggRotorController.SetRenderingOff(this._remoteRenderers, covered);
		}

		// Token: 0x06009047 RID: 36935 RVA: 0x003B082C File Offset: 0x003AEA2C
		private static void SetRenderingOff(List<Renderer> renderers, bool off)
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				if (renderers[i] != null)
				{
					renderers[i].forceRenderingOff = off;
				}
			}
		}

		// Token: 0x06009048 RID: 36936 RVA: 0x003B0868 File Offset: 0x003AEA68
		private void UpdateBlendShape(float deltaTime)
		{
			string insertionBlendShape = this._config.insertionBlendShape;
			float insertionBlendShapeWeight = this._config.insertionBlendShapeWeight;
			if (this._openingCovered && !this._covered)
			{
				this._snapOpening = true;
			}
			this._openingCovered = this._covered;
			float num = Mathf.Max(insertionBlendShapeWeight, this._config.insertionPeakBlendShapeWeight);
			float num2 = 0f;
			bool flag = this._phase == EggRotorController.EggPhase.Sliding && !this._covered && !this.IsCrotchOccupied && !string.IsNullOrEmpty(insertionBlendShape);
			if (flag)
			{
				EggRotorRigSettings rig = this.Rig;
				float num3 = EggRotorPlacementMath.WidestPassesOpening(rig.EntryPose, rig.InsidePose, 0.0259f * this.EggScale, EggRotorRigSettings.CanalOpening, EggRotorRigSettings.CanalAxis);
				num2 = EggRotorPlacementMath.InsideOpening(EggRotorPlacementMath.Ease(this._insertion), insertionBlendShapeWeight, num, num3);
			}
			if (this._blendShapeActive && insertionBlendShape != this._blendShapeName)
			{
				this.ReleaseBlendShape();
			}
			this._openWeight = ((this._snapOpening || flag) ? num2 : EggRotorPlacementMath.FadeOpening(this._openWeight, num2, deltaTime, this._config.insertionBlendShapeFadeSeconds, num));
			this._snapOpening = false;
			if (this._openWeight <= 0f)
			{
				this.ReleaseBlendShape();
				return;
			}
			if (this._blendShapes == null)
			{
				BlendShapeManager blendShapeManager = this.ResolveBlendShapeManager();
				if (blendShapeManager == null)
				{
					return;
				}
				if (!blendShapeManager.HasBlendShape(insertionBlendShape))
				{
					this.WarnMissingBlendShape(insertionBlendShape);
					return;
				}
				this._blendShapes = blendShapeManager;
				this._blendShapeActive = false;
			}
			float num4 = this._openWeight;
			float num5;
			if (this._blendShapes.TryGetEffectiveOverride(insertionBlendShape, out num5, this.BlendShapeSourceId))
			{
				num4 = Mathf.Max(num4, num5);
			}
			if (this._blendShapeActive && Mathf.Abs(num4 - this._appliedOpenWeight) <= 0.01f)
			{
				return;
			}
			this._blendShapes.SetOverride(insertionBlendShape, num4, this.BlendShapeSourceId, BlendShapePriority.Animation, false);
			this._blendShapeActive = true;
			this._blendShapeName = insertionBlendShape;
			this._appliedOpenWeight = num4;
		}

		// Token: 0x06009049 RID: 36937 RVA: 0x003B0A55 File Offset: 0x003AEC55
		private BlendShapeManager ResolveBlendShapeManager()
		{
			return BlendShapeManager.ResolveForRig((this._animator != null) ? this._animator : this);
		}

		// Token: 0x0600904A RID: 36938 RVA: 0x003B0A74 File Offset: 0x003AEC74
		private bool IsBlendShapeHeld()
		{
			float num;
			return !(this._blendShapes == null) && !(this._blendShapes != this.ResolveBlendShapeManager()) && this._blendShapes.TryGetEffectiveOverride(this._blendShapeName, out num, null) && num >= this._appliedOpenWeight - 0.01f;
		}

		// Token: 0x0600904B RID: 36939 RVA: 0x003B0ACE File Offset: 0x003AECCE
		private void WarnMissingBlendShape(string shape)
		{
			if (shape == this._warnedBlendShape)
			{
				return;
			}
			this._warnedBlendShape = shape;
			Debug.LogWarning("[EggRotor] Ciel's body has no blend shape \"" + shape + "\"; the opening around the Inside egg is skipped. The vaginal dildo actions open Kupa_V_LR.", this._config);
		}

		// Token: 0x0600904C RID: 36940 RVA: 0x003B0B01 File Offset: 0x003AED01
		private void ReleaseBlendShape()
		{
			this.DropBlendShapeOverride();
			this._openWeight = 0f;
		}

		// Token: 0x0600904D RID: 36941 RVA: 0x003B0B14 File Offset: 0x003AED14
		private void DropBlendShapeOverride()
		{
			if (this._blendShapeActive && this._blendShapes != null)
			{
				if (string.IsNullOrEmpty(this._blendShapeName))
				{
					this._blendShapes.RemoveAllOverridesFromSource(this.BlendShapeSourceId);
				}
				else
				{
					this._blendShapes.RemoveOverride(this._blendShapeName, this.BlendShapeSourceId, true, true, 0f);
				}
			}
			this._blendShapeActive = false;
			this._blendShapes = null;
		}

		// Token: 0x0600904E RID: 36942 RVA: 0x003B0B84 File Offset: 0x003AED84
		private void ResolveFuta()
		{
			if (this._playerRoot == null)
			{
				return;
			}
			if (this._futa == null)
			{
				if (Time.unscaledTime < this._nextFutaSearch)
				{
					return;
				}
				this._nextFutaSearch = Time.unscaledTime + 5f;
				Transform transform = this._playerRoot;
				while (transform != null && this._futa == null)
				{
					FutanariModeController componentInChildren = transform.GetComponentInChildren<FutanariModeController>(true);
					if (!(componentInChildren == null))
					{
						this._futa = componentInChildren;
						this._futaSearchRoot = transform;
					}
					transform = transform.parent;
				}
				if (this._futa == null)
				{
					return;
				}
				this._futaModeKnown = false;
			}
			if (this._erection == null)
			{
				this._erection = this._futa.GetComponent<PenisErectionController>();
			}
			if (this._penis == null && this._futaSearchRoot != null && Time.unscaledTime >= this._nextPenisSearch)
			{
				this._nextPenisSearch = Time.unscaledTime + 5f;
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._futaSearchRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true))
				{
					if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null) && !(skinnedMeshRenderer.sharedMesh.name != "sao"))
					{
						this._penis = skinnedMeshRenderer;
						break;
					}
				}
				this._glans.Unbind();
			}
			if (this._penis == null)
			{
				return;
			}
			if (this._penisTip == null)
			{
				this._penisTip = Array.Find<Transform>(this._penis.bones, (Transform bone) => bone != null && bone.name == "Bone.019");
			}
			if (this._penisCloth == null && this._penis.transform.parent != null)
			{
				foreach (MagicaCloth magicaCloth in this._penis.transform.parent.GetComponentsInChildren<MagicaCloth>(true))
				{
					if (!(magicaCloth == null) && !(magicaCloth.name != "Sao_Physics"))
					{
						this._penisCloth = magicaCloth;
						return;
					}
				}
			}
		}

		// Token: 0x0600904F RID: 36943 RVA: 0x003B0DBC File Offset: 0x003AEFBC
		private void ForgetFuta()
		{
			this.ReleaseErection();
			this._futa = null;
			this._futaSearchRoot = null;
			this._nextFutaSearch = (this._nextPenisSearch = 0f);
			this._erection = null;
			this._penis = null;
			this._penisTip = null;
			this._penisCloth = null;
			this._futaModeKnown = false;
			this._glans.Unbind();
		}

		// Token: 0x06009050 RID: 36944 RVA: 0x003B0E20 File Offset: 0x003AF020
		private void RefreshFutaMode()
		{
			bool flag = this._futa != null && this._futa.isActiveAndEnabled && this._futa.FutanariMode;
			if (this._futaModeKnown && flag == this._futaMode)
			{
				return;
			}
			this._futaModeKnown = true;
			this._futaMode = flag;
			EggRotorPlacement eggRotorPlacement = EggRotorIds.ForMode(this._placement, flag, this._classicCrotch);
			if (EggRotorIds.UsesCrotch(eggRotorPlacement) && this.IsCrotchOccupied)
			{
				eggRotorPlacement = EggRotorPlacement.Swing;
			}
			this.SetPlacementInternal(eggRotorPlacement);
		}

		// Token: 0x06009051 RID: 36945 RVA: 0x003B0EA4 File Offset: 0x003AF0A4
		private bool EnsureGlansSurface()
		{
			if (this._penis == null)
			{
				this.ResolveFuta();
			}
			if (this._penis == null)
			{
				return false;
			}
			EggRotorRigSettings rig = this.Rig;
			if (this._glans.IsBoundTo(this._penis, rig.glansStation, rig.glansStationSpan))
			{
				return true;
			}
			if (Time.unscaledTime < this._nextGlansBind)
			{
				return false;
			}
			if (this._glans.Bind(this._penis, "Bone.019", rig.glansStation, rig.glansStationSpan))
			{
				return true;
			}
			this._nextGlansBind = Time.unscaledTime + 2f;
			if (!this._warnedGlans)
			{
				this._warnedGlans = true;
				Debug.LogWarning("[EggRotor] Could not bind the glans frame on \"" + this._penis.name + "\" (readable mesh, Bone.019 and vertices at the stations); the egg stays put.", this);
			}
			return false;
		}

		// Token: 0x06009052 RID: 36946 RVA: 0x003B0F74 File Offset: 0x003AF174
		private bool EvaluateGlansMount()
		{
			if (this._glansMount == null || !this.EnsureGlansSurface())
			{
				return false;
			}
			Pose pose;
			bool flag = this._glans.TryEvaluate(out pose);
			if (this._glans.HasTip)
			{
				this._glansTipFrame = Time.frameCount;
			}
			if (!flag)
			{
				return false;
			}
			this._glansMount.SetPositionAndRotation(pose.position, pose.rotation);
			float num = this._glans.WorldRadius / Mathf.Max(0.0001f, this._bodyScale);
			if (num > 0f && !float.IsInfinity(num))
			{
				this._glansRadiusBody = num;
			}
			return true;
		}

		// Token: 0x06009053 RID: 36947 RVA: 0x003B1010 File Offset: 0x003AF210
		private void UpdateTapeShape()
		{
			if (this._phase != EggRotorController.EggPhase.Glans || !this.Rig.wrapTapeOnGlans || this._glansRadiusBody <= 0f)
			{
				this.RestoreTape();
				return;
			}
			float num = this._glansRadiusBody / Mathf.Max(0.0001f, this.EggScale);
			if (this._tapeWrapped && Mathf.Abs(num - this._tapeWrapRadius) < 0.0004f)
			{
				return;
			}
			for (int i = 0; i < this._tapeFilters.Count; i++)
			{
				MeshFilter meshFilter = this._tapeFilters[i];
				Mesh mesh = ((i < this._tapeSourceMeshes.Count) ? this._tapeSourceMeshes[i] : null);
				Vector3[] array = ((i < this._tapeSourceVertices.Count) ? this._tapeSourceVertices[i] : null);
				if (!(meshFilter == null) && !(mesh == null) && array != null)
				{
					Mesh mesh2 = this._tapeWrappedMeshes[i];
					if (mesh2 == null)
					{
						mesh2 = Object.Instantiate<Mesh>(mesh);
						mesh2.name = mesh.name + " (glans)";
						mesh2.hideFlags = HideFlags.DontSave;
						this._tapeWrappedMeshes[i] = mesh2;
					}
					Vector3[] array2 = this._tapeSourceNormals[i];
					Vector3[] array3 = this._tapeWrapVertices[i];
					Vector3[] array4 = this._tapeWrapNormals[i];
					EggRotorPlacementMath.WrapTape(array, array2, array3, array4, num, this._eggRadiusLocal * 0.75f, this._eggRadiusLocal * 0.75f);
					mesh2.SetVertices(array3);
					if (array4 != null)
					{
						mesh2.SetNormals(array4);
					}
					mesh2.RecalculateBounds();
					meshFilter.sharedMesh = mesh2;
				}
			}
			this._tapeWrapRadius = num;
			this._tapeWrapped = true;
		}

		// Token: 0x06009054 RID: 36948 RVA: 0x003B11D8 File Offset: 0x003AF3D8
		private void RestoreTape()
		{
			if (!this._tapeWrapped)
			{
				return;
			}
			int num = 0;
			while (num < this._tapeFilters.Count && num < this._tapeSourceMeshes.Count)
			{
				if (this._tapeFilters[num] != null && this._tapeSourceMeshes[num] != null)
				{
					this._tapeFilters[num].sharedMesh = this._tapeSourceMeshes[num];
				}
				num++;
			}
			this._tapeWrapped = false;
			this._tapeWrapRadius = -1f;
		}

		// Token: 0x06009055 RID: 36949 RVA: 0x003B126C File Offset: 0x003AF46C
		private void UpdateErection()
		{
			if (this._climax == null)
			{
				this._climax = base.GetComponent<EggRotorClimax>();
			}
			EggRotorClimaxSettings eggRotorClimaxSettings = ((this._config != null) ? this._config.climax : null);
			bool flag = this._climax != null && this._climax.IsRefractory && !this._climax.IsClimaxRunning;
			bool flag2 = this._worn && this._futaMode && this._erection != null && eggRotorClimaxSettings != null && eggRotorClimaxSettings.erectWhileVibrating && this._level > 0 && this.PhysicalPlacement == EggRotorPlacement.Glans && !flag && !DetectionInteractionGate.IsGameOver;
			if (this._erectionRequestedOn != null && this._erectionRequestedOn != this._erection)
			{
				this.ReleaseErection();
			}
			if (flag2 == this._erectionRequested && (!flag2 || this._erection.IsExternalErectionRequestedBy(this)))
			{
				return;
			}
			if (!flag2)
			{
				this.ReleaseErection();
				return;
			}
			this._erection.SetExternalErectionRequested(this, true);
			this._erectionRequested = true;
			this._erectionRequestedOn = this._erection;
		}

		// Token: 0x06009056 RID: 36950 RVA: 0x003B1393 File Offset: 0x003AF593
		private void ReleaseErection()
		{
			if (this._erectionRequestedOn != null)
			{
				this._erectionRequestedOn.SetExternalErectionRequested(this, false);
			}
			this._erectionRequested = false;
			this._erectionRequestedOn = null;
		}

		// Token: 0x06009057 RID: 36951 RVA: 0x003B13C0 File Offset: 0x003AF5C0
		public bool TryPulsePenis(float velocity)
		{
			if (velocity <= 0f || float.IsInfinity(velocity) || !this.IsOnGlans || !this.IsPenisVisible)
			{
				return false;
			}
			MagicaCloth penisCloth = this._penisCloth;
			if (penisCloth == null || !penisCloth.isActiveAndEnabled)
			{
				return false;
			}
			Vector3 vector = ((this._glansMount != null) ? (-this._glansMount.forward) : ((this._playerRoot != null) ? this._playerRoot.up : Vector3.up));
			penisCloth.AddForce(vector, velocity, ClothForceMode.VelocityAdd);
			return true;
		}

		// Token: 0x06009058 RID: 36952 RVA: 0x003B1454 File Offset: 0x003AF654
		private void UpdateBuzz(bool vibrating)
		{
			NippleRotorLevel nippleRotorLevel = (vibrating ? this._config.GetLevel(this._level) : default(NippleRotorLevel));
			float num = (vibrating ? Mathf.Max(0f, nippleRotorLevel.jitterAmplitude) : 0f);
			float num2 = (vibrating ? nippleRotorLevel.jitterHz : 0f);
			float num3 = (vibrating ? nippleRotorLevel.wobbleDegrees : 0f);
			if (this._buzzApplied && num == this._appliedAmplitude && num2 == this._appliedHz && num3 == this._appliedWobble)
			{
				return;
			}
			this._buzzApplied = true;
			this._appliedAmplitude = num;
			this._appliedHz = num2;
			this._appliedWobble = num3;
			if (this._eggBuzz != null)
			{
				this._eggBuzz.SetIntensity(num, num2, num3);
			}
			float num4 = Mathf.Max(0f, this._config.remoteBuzzScale);
			if (this._remoteBuzz != null)
			{
				this._remoteBuzz.SetIntensity(num * num4, num2, num3 * num4);
			}
		}

		// Token: 0x06009059 RID: 36953 RVA: 0x003B1554 File Offset: 0x003AF754
		private void Shake()
		{
			if (this._config == null || this._config.levelChangeShake <= 0f || this._playerRoot == null)
			{
				return;
			}
			AnimeBodyPhysics.TryShake(this._playerRoot, Vector3.up, false, this._config.levelChangeShake);
		}

		// Token: 0x0600905A RID: 36954 RVA: 0x003B15B0 File Offset: 0x003AF7B0
		private void ResolveFace()
		{
			NippleRotorFaceDriver component = base.GetComponent<NippleRotorFaceDriver>();
			if (component != this._face)
			{
				this._faceConfiguredFor = null;
			}
			this._face = component;
		}

		// Token: 0x0600905B RID: 36955 RVA: 0x003B15E0 File Offset: 0x003AF7E0
		private static bool NippleRotorWantsFace()
		{
			NippleRotorController nippleRotorController = NippleRotorController.Current;
			return nippleRotorController != null && nippleRotorController.IsWorn && nippleRotorController.Level > 0;
		}

		// Token: 0x0600905C RID: 36956 RVA: 0x003B1610 File Offset: 0x003AF810
		private void UpdateFace(bool wanted)
		{
			if (this._face == null)
			{
				return;
			}
			if (this._animator != null && this._faceConfiguredFor != this._animator)
			{
				this._face.Configure(this._animator);
				this._faceConfiguredFor = this._animator;
			}
			this._face.SetWanted(wanted);
		}

		// Token: 0x0600905D RID: 36957 RVA: 0x003B1678 File Offset: 0x003AF878
		private void ResolveActor()
		{
			StealthActor stealthActor = ((this._playerRoot != null) ? this._playerRoot.GetComponentInChildren<StealthActor>(true) : null);
			if (stealthActor == null && SensoryManager.Instance != null)
			{
				stealthActor = SensoryManager.Instance.GetPlayer();
			}
			if (stealthActor == this._actor)
			{
				return;
			}
			if (this._actor != null)
			{
				this._actor.SetExternalNoise(this, 0f);
			}
			this._actor = stealthActor;
		}

		// Token: 0x0600905E RID: 36958 RVA: 0x003B16F9 File Offset: 0x003AF8F9
		private void UpdateNoise(bool vibrating)
		{
			if (this._actor == null)
			{
				return;
			}
			this._actor.SetExternalNoise(this, vibrating ? EggRotorConfigSO.NoiseRadiusFor(this._config, this._level, this.PhysicalPlacement) : 0f);
		}

		// Token: 0x0600905F RID: 36959 RVA: 0x003B1737 File Offset: 0x003AF937
		private void ClearNoise()
		{
			if (this._actor != null)
			{
				this._actor.SetExternalNoise(this, 0f);
			}
		}

		// Token: 0x06009060 RID: 36960 RVA: 0x003B1758 File Offset: 0x003AF958
		private void UpdateNpcTargets(bool vibrating)
		{
			if (!vibrating || this._config.npcRefreshHz <= 0f)
			{
				this._npcRefreshTimer = 0f;
				return;
			}
			this._npcRefreshTimer -= Time.deltaTime;
			if (this._npcRefreshTimer > 0f)
			{
				return;
			}
			this._npcRefreshTimer = 1f / this._config.npcRefreshHz;
			this.RefreshNpcTargets();
		}

		// Token: 0x06009061 RID: 36961 RVA: 0x003B17C4 File Offset: 0x003AF9C4
		private void RefreshNpcTargets()
		{
			SensoryManager instance = SensoryManager.Instance;
			if (instance == null || this._actor == null || instance.GetPlayer() != this._actor)
			{
				return;
			}
			if (DetectionInteractionGate.IsGameOver || DetectionTransitionGate.IsBlocked || instance.IsPaused || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f || !instance.IsRunModeDetectionEnabled)
			{
				return;
			}
			IReadOnlyList<NPCSenses> registeredNPCs = instance.GetRegisteredNPCs();
			if (registeredNPCs == null)
			{
				return;
			}
			Vector3 position = this._actor.Position;
			Vector3 position2 = this._actor.transform.position;
			for (int i = 0; i < registeredNPCs.Count; i++)
			{
				NPCSenses npcsenses = registeredNPCs[i];
				if (!(npcsenses == null) && !npcsenses.IsInCooldown && npcsenses.HasAudioContact)
				{
					npcsenses.LastKnownPlayerPosition = position;
					NPCStateMachine npcstateMachine;
					if (npcsenses.IsSearching && npcsenses.TryGetComponent<NPCStateMachine>(out npcstateMachine))
					{
						NPCFSMContext context = npcstateMachine.Context;
						if (context != null)
						{
							context.LastSeenPlayerPosition = position2;
						}
					}
				}
			}
		}

		// Token: 0x06009062 RID: 36962 RVA: 0x003B18C8 File Offset: 0x003AFAC8
		private void UpdateAudio(bool vibrating, float unscaledDeltaTime)
		{
			bool flag = EggRotorPlacementMath.UsesMuffledLoops(this.PhysicalPlacement == EggRotorPlacement.Inside, this._covered);
			AudioClip audioClip = (vibrating ? this.ResolveLoopClip(this._level, flag) : null);
			if (audioClip == null && !this._audioActive)
			{
				return;
			}
			this.EnsureAudio();
			for (int i = 0; i < this._voices.Length; i++)
			{
				if (this._voices[i].source == null)
				{
					return;
				}
			}
			if (audioClip != null)
			{
				if (this._voices[this._activeVoice].source.clip != audioClip)
				{
					this.StartVoice(audioClip);
				}
				else
				{
					this.ResumeVoice(this._activeVoice);
				}
				NippleRotorLevel level = this._config.GetLevel(this._level);
				float num = (flag ? Mathf.Clamp01(this._config.insideLoopsVolumeScale) : 1f);
				this._voices[this._activeVoice].volume = Mathf.Clamp01(level.volume) * num;
				this._voices[this._activeVoice].pitch = ((level.pitch > 0f) ? level.pitch : 1f);
			}
			float audioFadeSeconds = this._config.audioFadeSeconds;
			float num2 = ((audioFadeSeconds > 0.0001f) ? (unscaledDeltaTime / audioFadeSeconds) : 1f);
			bool flag2 = this._worn && this._level > 0 && this.IsSuspended;
			bool flag3 = false;
			for (int j = 0; j < this._voices.Length; j++)
			{
				AudioSource source = this._voices[j].source;
				float num3 = ((audioClip != null && j == this._activeVoice) ? 1f : 0f);
				this._voices[j].gain = Mathf.MoveTowards(this._voices[j].gain, num3, num2);
				source.volume = this._voices[j].gain * this._voices[j].volume;
				source.pitch = this._voices[j].pitch;
				if (this._voices[j].gain > 0f || num3 > 0f)
				{
					flag3 = true;
				}
				else if (flag2 && j == this._activeVoice)
				{
					if (source.isPlaying)
					{
						source.Pause();
						this._voices[j].paused = true;
					}
					flag3 |= this._voices[j].paused;
				}
				else if (source.isPlaying || this._voices[j].paused)
				{
					source.Stop();
					this._voices[j].paused = false;
				}
			}
			this._audioActive = flag3;
			this.PlaceAudio();
		}

		// Token: 0x06009063 RID: 36963 RVA: 0x003B1BCC File Offset: 0x003AFDCC
		private AudioClip ResolveLoopClip(int level, bool muffled)
		{
			if (level <= 0 || this._config == null)
			{
				return null;
			}
			int num = level - 1;
			AudioClip audioClip = EggRotorController.PickClip(muffled ? this._config.insideLoops : this._config.outsideLoops, num);
			if (!(audioClip != null))
			{
				return EggRotorController.PickClip(muffled ? this._config.outsideLoops : this._config.insideLoops, num);
			}
			return audioClip;
		}

		// Token: 0x06009064 RID: 36964 RVA: 0x002FC2C3 File Offset: 0x002FA4C3
		private static AudioClip PickClip(AudioClip[] clips, int index)
		{
			if (clips == null || index < 0 || index >= clips.Length)
			{
				return null;
			}
			return clips[index];
		}

		// Token: 0x06009065 RID: 36965 RVA: 0x003B1C40 File Offset: 0x003AFE40
		private void StartVoice(AudioClip clip)
		{
			int num = -1;
			int num2 = 0;
			while (num2 < this._voices.Length && num < 0)
			{
				if (num2 != this._activeVoice && this._voices[num2].source.clip == clip && (this._voices[num2].source.isPlaying || this._voices[num2].paused))
				{
					num = num2;
				}
				num2++;
			}
			bool flag = num >= 0;
			if (!flag)
			{
				float num3 = float.MaxValue;
				for (int i = 0; i < this._voices.Length; i++)
				{
					if (i != this._activeVoice && this._voices[i].gain < num3)
					{
						num3 = this._voices[i].gain;
						num = i;
					}
				}
			}
			ref EggRotorController.LoopVoice ptr = ref this._voices[this._activeVoice];
			ref EggRotorController.LoopVoice ptr2 = ref this._voices[num];
			if (flag)
			{
				if (ptr2.paused)
				{
					ptr2.source.UnPause();
				}
				ptr2.paused = false;
			}
			else
			{
				AudioSource source = ptr.source;
				float num4 = ((source.clip != null && (source.isPlaying || ptr.paused)) ? source.time : 0f);
				ptr2.source.Stop();
				ptr2.paused = false;
				ptr2.gain = 0f;
				ptr2.source.clip = clip;
				ptr2.source.loop = true;
				this.ApplySpatial(ptr2.source);
				if (clip.length > 0f && clip.loadState == AudioDataLoadState.Loaded)
				{
					ptr2.source.time = Mathf.Repeat(num4, clip.length);
				}
				GameAudioRouter.Route(ptr2.source, GameAudioRouter.Channel.Sfx);
				ptr2.source.Play();
			}
			this._activeVoice = num;
		}

		// Token: 0x06009066 RID: 36966 RVA: 0x003B1E2C File Offset: 0x003B002C
		private void ResumeVoice(int index)
		{
			AudioSource source = this._voices[index].source;
			if (this._voices[index].paused)
			{
				source.UnPause();
				this._voices[index].paused = false;
			}
			if (!source.isPlaying)
			{
				this.ApplySpatial(source);
				source.Play();
			}
		}

		// Token: 0x06009067 RID: 36967 RVA: 0x003B1E8C File Offset: 0x003B008C
		private void StopAudioImmediate()
		{
			for (int i = 0; i < this._voices.Length; i++)
			{
				AudioSource source = this._voices[i].source;
				if (source != null)
				{
					source.Stop();
					source.volume = 0f;
				}
				this._voices[i].gain = 0f;
				this._voices[i].paused = false;
			}
			this._audioActive = false;
		}

		// Token: 0x06009068 RID: 36968 RVA: 0x003B1F08 File Offset: 0x003B0108
		private void PlaySwitch()
		{
			if (this._config == null || this._config.switchClip == null || !base.isActiveAndEnabled)
			{
				return;
			}
			this.EnsureAudio();
			if (this._switchSource == null)
			{
				return;
			}
			this.ApplySpatial(this._switchSource);
			this.PlaceAudio();
			this._switchSource.PlayOneShot(this._config.switchClip, this._config.switchVolume);
		}

		// Token: 0x06009069 RID: 36969 RVA: 0x003B1F88 File Offset: 0x003B0188
		private void EnsureAudio()
		{
			if (this._voices[0].source == null)
			{
				this._voices[0] = new EggRotorController.LoopVoice
				{
					source = this.CreateSource("EggRotorBuzzAudio", true),
					pitch = 1f
				};
			}
			if (this._voices[1].source == null)
			{
				this._voices[1] = new EggRotorController.LoopVoice
				{
					source = this.CreateSource("EggRotorBuzzAudio2", true),
					pitch = 1f
				};
			}
			if (this._voices[2].source == null)
			{
				this._voices[2] = new EggRotorController.LoopVoice
				{
					source = this.CreateSource("EggRotorBuzzAudio3", true),
					pitch = 1f
				};
			}
			if (this._switchSource == null)
			{
				this._switchSource = this.CreateSource("EggRotorSwitchAudio", false);
			}
		}

		// Token: 0x0600906A RID: 36970 RVA: 0x003B209C File Offset: 0x003B029C
		private AudioSource CreateSource(string objectName, bool loop)
		{
			Transform transform = base.transform.Find(objectName);
			GameObject gameObject = ((transform != null) ? transform.gameObject : new GameObject(objectName));
			if (transform == null)
			{
				gameObject.transform.SetParent(base.transform, false);
			}
			AudioSource audioSource = gameObject.GetComponent<AudioSource>();
			if (audioSource == null)
			{
				audioSource = gameObject.AddComponent<AudioSource>();
			}
			audioSource.playOnAwake = false;
			audioSource.loop = loop;
			audioSource.dopplerLevel = 0f;
			audioSource.volume = (loop ? 0f : 1f);
			this.ApplySpatial(audioSource);
			GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Sfx);
			return audioSource;
		}

		// Token: 0x0600906B RID: 36971 RVA: 0x003B2140 File Offset: 0x003B0340
		private void ApplySpatial(AudioSource source)
		{
			if (source == null || this._config == null)
			{
				return;
			}
			float num = Mathf.Max(0.01f, this._config.audioMinDistance);
			source.spatialBlend = Mathf.Clamp01(this._config.audioSpatialBlend);
			source.rolloffMode = AudioRolloffMode.Linear;
			source.minDistance = num;
			source.maxDistance = Mathf.Max(num + 0.01f, this._config.audioMaxDistance);
			source.dopplerLevel = 0f;
		}

		// Token: 0x0600906C RID: 36972 RVA: 0x003B21C8 File Offset: 0x003B03C8
		private void PlaceAudio()
		{
			Vector3 vector;
			if (this._egg != null)
			{
				vector = this._egg.transform.position;
			}
			else if (this._anchor != null)
			{
				vector = this._anchor.position;
			}
			else
			{
				vector = base.transform.position;
			}
			for (int i = 0; i < this._voices.Length; i++)
			{
				if (this._voices[i].source != null)
				{
					this._voices[i].source.transform.position = vector;
				}
			}
			if (this._switchSource != null)
			{
				this._switchSource.transform.position = ((this._remote != null) ? this._remote.transform.position : vector);
			}
		}

		// Token: 0x040093EC RID: 37868
		public const string BuzzAudioObjectName = "EggRotorBuzzAudio";

		// Token: 0x040093ED RID: 37869
		public const string BuzzCrossfadeAudioObjectName = "EggRotorBuzzAudio2";

		// Token: 0x040093EE RID: 37870
		public const string BuzzThirdAudioObjectName = "EggRotorBuzzAudio3";

		// Token: 0x040093EF RID: 37871
		public const string SwitchAudioObjectName = "EggRotorSwitchAudio";

		// Token: 0x040093F0 RID: 37872
		public const string CordObjectName = "EggRotorCord";

		// Token: 0x040093F1 RID: 37873
		public const string CableEndObjectName = "EggRotorCableEnd";

		// Token: 0x040093F2 RID: 37874
		public const string SwingEndObjectName = "EggRotorSwingEnd";

		// Token: 0x040093F3 RID: 37875
		public const string GlansMountObjectName = "EggRotorGlansMount";

		// Token: 0x040093F4 RID: 37876
		private const string BodyRendererName = "Body_sotai_NSFW";

		// Token: 0x040093F5 RID: 37877
		private const string ClothingLayerName = "PlayerClothing";

		// Token: 0x040093F6 RID: 37878
		private const float HousekeepingInterval = 0.5f;

		// Token: 0x040093F7 RID: 37879
		private const float InsideFromProgress = 0.5f;

		// Token: 0x040093F8 RID: 37880
		private const float MaxCapturedDistance = 0.6f;

		// Token: 0x040093F9 RID: 37881
		private const float OpeningWriteEpsilon = 0.01f;

		// Token: 0x040093FA RID: 37882
		private const float GlansBindRetrySeconds = 2f;

		// Token: 0x040093FB RID: 37883
		private const float FutaSearchRetrySeconds = 5f;

		// Token: 0x040093FC RID: 37884
		private const float TapeRadiusEpsilon = 0.0004f;

		// Token: 0x040093FD RID: 37885
		private static readonly List<EggRotorController> EnabledControllers = new List<EggRotorController>(2);

		// Token: 0x04009400 RID: 37888
		private EggRotorConfigSO _config;

		// Token: 0x04009401 RID: 37889
		private PlayerInventoryManager _inventory;

		// Token: 0x04009402 RID: 37890
		private Transform _playerRoot;

		// Token: 0x04009403 RID: 37891
		private Animator _animator;

		// Token: 0x04009404 RID: 37892
		private WearableUseController _wear;

		// Token: 0x04009405 RID: 37893
		private StealthActor _actor;

		// Token: 0x04009406 RID: 37894
		private NippleRotorFaceDriver _face;

		// Token: 0x04009407 RID: 37895
		private Animator _faceConfiguredFor;

		// Token: 0x04009408 RID: 37896
		private ClothingToggleController _clothing;

		// Token: 0x04009409 RID: 37897
		private InsertableToyController _toy;

		// Token: 0x0400940A RID: 37898
		private SkinnedMeshRenderer _body;

		// Token: 0x0400940B RID: 37899
		private EggRotorRigSettings _fallbackRig;

		// Token: 0x0400940C RID: 37900
		private FutanariModeController _futa;

		// Token: 0x0400940D RID: 37901
		private Transform _futaSearchRoot;

		// Token: 0x0400940E RID: 37902
		private float _nextFutaSearch;

		// Token: 0x0400940F RID: 37903
		private float _nextPenisSearch;

		// Token: 0x04009410 RID: 37904
		private PenisErectionController _erection;

		// Token: 0x04009411 RID: 37905
		private SkinnedMeshRenderer _penis;

		// Token: 0x04009412 RID: 37906
		private Transform _penisTip;

		// Token: 0x04009413 RID: 37907
		private MagicaCloth _penisCloth;

		// Token: 0x04009414 RID: 37908
		private EggRotorClimax _climax;

		// Token: 0x04009415 RID: 37909
		private bool _futaMode;

		// Token: 0x04009416 RID: 37910
		private bool _futaModeKnown;

		// Token: 0x04009417 RID: 37911
		private EggRotorPlacement _classicCrotch = EggRotorPlacement.Outside;

		// Token: 0x04009418 RID: 37912
		private bool _erectionRequested;

		// Token: 0x04009419 RID: 37913
		private PenisErectionController _erectionRequestedOn;

		// Token: 0x0400941A RID: 37914
		private bool _worn;

		// Token: 0x0400941B RID: 37915
		private int _level;

		// Token: 0x0400941C RID: 37916
		private EggRotorPlacement _placement = EggRotorPlacement.Outside;

		// Token: 0x0400941D RID: 37917
		private bool _placementInitialized;

		// Token: 0x0400941E RID: 37918
		private bool _covered;

		// Token: 0x0400941F RID: 37919
		private Transform _anchor;

		// Token: 0x04009420 RID: 37920
		private Transform _thigh;

		// Token: 0x04009421 RID: 37921
		private GameObject _egg;

		// Token: 0x04009422 RID: 37922
		private GameObject _remote;

		// Token: 0x04009423 RID: 37923
		private Transform _eggSocket;

		// Token: 0x04009424 RID: 37924
		private Transform _remoteSocket;

		// Token: 0x04009425 RID: 37925
		private Transform _cableEnd;

		// Token: 0x04009426 RID: 37926
		private Transform _swingEnd;

		// Token: 0x04009427 RID: 37927
		private Transform _glansMount;

		// Token: 0x04009428 RID: 37928
		private readonly EggRotorGlansSurface _glans = new EggRotorGlansSurface();

		// Token: 0x04009429 RID: 37929
		private float _nextGlansBind;

		// Token: 0x0400942A RID: 37930
		private bool _warnedGlans;

		// Token: 0x0400942B RID: 37931
		private float _glansRadiusBody;

		// Token: 0x0400942C RID: 37932
		private int _glansPostSimulationFrame = -1;

		// Token: 0x0400942D RID: 37933
		private int _glansTipFrame = -1;

		// Token: 0x0400942E RID: 37934
		private Action _postSimulationHandler;

		// Token: 0x0400942F RID: 37935
		private bool _postSimulationHooked;

		// Token: 0x04009430 RID: 37936
		private readonly List<MeshFilter> _tapeFilters = new List<MeshFilter>(1);

		// Token: 0x04009431 RID: 37937
		private readonly List<Mesh> _tapeSourceMeshes = new List<Mesh>(1);

		// Token: 0x04009432 RID: 37938
		private readonly List<Mesh> _tapeWrappedMeshes = new List<Mesh>(1);

		// Token: 0x04009433 RID: 37939
		private readonly List<Vector3[]> _tapeSourceVertices = new List<Vector3[]>(1);

		// Token: 0x04009434 RID: 37940
		private readonly List<Vector3[]> _tapeSourceNormals = new List<Vector3[]>(1);

		// Token: 0x04009435 RID: 37941
		private readonly List<Vector3[]> _tapeWrapVertices = new List<Vector3[]>(1);

		// Token: 0x04009436 RID: 37942
		private readonly List<Vector3[]> _tapeWrapNormals = new List<Vector3[]>(1);

		// Token: 0x04009437 RID: 37943
		private float _tapeWrapRadius = -1f;

		// Token: 0x04009438 RID: 37944
		private bool _tapeWrapped;

		// Token: 0x04009439 RID: 37945
		private float _eggRadiusLocal = 0.006f;

		// Token: 0x0400943A RID: 37946
		private NippleRotorBuzz _eggBuzz;

		// Token: 0x0400943B RID: 37947
		private NippleRotorBuzz _remoteBuzz;

		// Token: 0x0400943C RID: 37948
		private Vector3 _socketInEgg;

		// Token: 0x0400943D RID: 37949
		private Quaternion _socketRotationInEgg = Quaternion.identity;

		// Token: 0x0400943E RID: 37950
		private bool _hasSocketPose;

		// Token: 0x0400943F RID: 37951
		private Pose _remoteBase = Pose.identity;

		// Token: 0x04009440 RID: 37952
		private Vector3 _remoteBaseScale = Vector3.one;

		// Token: 0x04009441 RID: 37953
		private float _eggBaseScale = 1f;

		// Token: 0x04009442 RID: 37954
		private GameObject _cordObject;

		// Token: 0x04009443 RID: 37955
		private NippleRotorCord _cord;

		// Token: 0x04009444 RID: 37956
		private readonly List<Renderer> _eggRenderers = new List<Renderer>(4);

		// Token: 0x04009445 RID: 37957
		private readonly List<Renderer> _tapeRenderers = new List<Renderer>(2);

		// Token: 0x04009446 RID: 37958
		private readonly List<Renderer> _remoteRenderers = new List<Renderer>(4);

		// Token: 0x04009447 RID: 37959
		private int _appliedVisibility = -1;

		// Token: 0x04009448 RID: 37960
		private int _rigLayer;

		// Token: 0x04009449 RID: 37961
		private float _bodyScale = 1f;

		// Token: 0x0400944A RID: 37962
		private bool _rigExpected;

		// Token: 0x0400944B RID: 37963
		private bool _eggExpected;

		// Token: 0x0400944C RID: 37964
		private bool _remoteExpected;

		// Token: 0x0400944D RID: 37965
		private bool _warnedMissingAssets;

		// Token: 0x0400944E RID: 37966
		private bool _warnedAnchor;

		// Token: 0x0400944F RID: 37967
		private EggRotorController.EggPhase _phase;

		// Token: 0x04009450 RID: 37968
		private float _insertion;

		// Token: 0x04009451 RID: 37969
		private float _blend;

		// Token: 0x04009452 RID: 37970
		private Pose _from = Pose.identity;

		// Token: 0x04009453 RID: 37971
		private Pose _pose = Pose.identity;

		// Token: 0x04009454 RID: 37972
		private BlendShapeManager _blendShapes;

		// Token: 0x04009455 RID: 37973
		private bool _blendShapeActive;

		// Token: 0x04009456 RID: 37974
		private string _blendShapeSourceId;

		// Token: 0x04009457 RID: 37975
		private string _blendShapeName;

		// Token: 0x04009458 RID: 37976
		private float _openWeight;

		// Token: 0x04009459 RID: 37977
		private float _appliedOpenWeight;

		// Token: 0x0400945A RID: 37978
		private bool _snapOpening;

		// Token: 0x0400945B RID: 37979
		private bool _openingCovered;

		// Token: 0x0400945C RID: 37980
		private string _warnedBlendShape;

		// Token: 0x0400945D RID: 37981
		private bool _buzzApplied;

		// Token: 0x0400945E RID: 37982
		private float _appliedAmplitude;

		// Token: 0x0400945F RID: 37983
		private float _appliedHz;

		// Token: 0x04009460 RID: 37984
		private float _appliedWobble;

		// Token: 0x04009461 RID: 37985
		private readonly EggRotorController.LoopVoice[] _voices = new EggRotorController.LoopVoice[3];

		// Token: 0x04009462 RID: 37986
		private int _activeVoice;

		// Token: 0x04009463 RID: 37987
		private bool _audioActive;

		// Token: 0x04009464 RID: 37988
		private AudioSource _switchSource;

		// Token: 0x04009465 RID: 37989
		private float _housekeepingTimer;

		// Token: 0x04009466 RID: 37990
		private float _npcRefreshTimer;

		// Token: 0x02001227 RID: 4647
		private enum EggPhase
		{
			// Token: 0x04009469 RID: 37993
			Outside,
			// Token: 0x0400946A RID: 37994
			MovingToOutside,
			// Token: 0x0400946B RID: 37995
			MovingToEntry,
			// Token: 0x0400946C RID: 37996
			Sliding,
			// Token: 0x0400946D RID: 37997
			Swing,
			// Token: 0x0400946E RID: 37998
			MovingToGlans,
			// Token: 0x0400946F RID: 37999
			Glans
		}

		// Token: 0x02001228 RID: 4648
		private struct LoopVoice
		{
			// Token: 0x04009470 RID: 38000
			public AudioSource source;

			// Token: 0x04009471 RID: 38001
			public float gain;

			// Token: 0x04009472 RID: 38002
			public float volume;

			// Token: 0x04009473 RID: 38003
			public float pitch;

			// Token: 0x04009474 RID: 38004
			public bool paused;
		}
	}
}
