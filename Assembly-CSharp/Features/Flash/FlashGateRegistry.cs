using System;
using System.Collections.Generic;
using Features.ExposeLingerie;
using Features.HideFlash;
using Features.PoolDive;
using Features.UniversityUniform;
using ProjectExhibitionist.BenchSeating;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.Flash
{
	// Token: 0x0200120C RID: 4620
	public static class FlashGateRegistry
	{
		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x06008EAA RID: 36522 RVA: 0x003A54CB File Offset: 0x003A36CB
		public static bool AnyEngaged
		{
			get
			{
				return FlashGateRegistry.AnyEngagedIn<ArmsFlashController>(FlashGateRegistry.ArmsGates) || FlashGateRegistry.AnyEngagedIn<ClothFullFlashController>(FlashGateRegistry.FullBodyGates);
			}
		}

		// Token: 0x06008EAB RID: 36523 RVA: 0x003A54E8 File Offset: 0x003A36E8
		public static bool IsFlashEngaged(IFlashGate gate)
		{
			MonoBehaviour monoBehaviour = gate as MonoBehaviour;
			if (monoBehaviour == null || monoBehaviour == null || !monoBehaviour.isActiveAndEnabled)
			{
				return false;
			}
			ArmsFlashController armsFlashController = gate as ArmsFlashController;
			if (armsFlashController != null)
			{
				return armsFlashController.Config != null && armsFlashController.Config.gateAsFlash && armsFlashController.IsEngaged;
			}
			ClothFullFlashController clothFullFlashController = gate as ClothFullFlashController;
			if (clothFullFlashController != null)
			{
				return clothFullFlashController.IsEngaged;
			}
			HideFlashController hideFlashController = gate as HideFlashController;
			return hideFlashController != null && hideFlashController.IsExposureActive;
		}

		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x06008EAC RID: 36524 RVA: 0x003A5560 File Offset: 0x003A3760
		public static bool AnyFlashEngaged
		{
			get
			{
				for (int i = FlashGateRegistry.ArmsGates.Count - 1; i >= 0; i--)
				{
					if (FlashGateRegistry.IsFlashEngaged(FlashGateRegistry.ArmsGates[i]))
					{
						return true;
					}
				}
				return FlashGateRegistry.AnyEngagedIn<ClothFullFlashController>(FlashGateRegistry.FullBodyGates);
			}
		}

		// Token: 0x06008EAD RID: 36525 RVA: 0x003A55A4 File Offset: 0x003A37A4
		public static bool TryAcquireActivity(Object owner)
		{
			if (!PlayerActivityGate.Allows(PlayerActivity.Flash) || HideFlashController.HasHeldCoverItem)
			{
				return false;
			}
			ArmsFlashController armsFlashController = owner as ArmsFlashController;
			PlayerContextResource playerContextResource = ((armsFlashController != null && FlashGateRegistry.HasSettledSeat(armsFlashController.PlayerAnimator)) ? PlayerContextResource.HeldItem : (PlayerContextResource.WorldInteraction | PlayerContextResource.FullBodyAnimation));
			if (FlashGateRegistry._flashActivity != null && FlashGateRegistry._flashActivity.IsActive)
			{
				return PlayerActivityGate.TryUpdate(FlashGateRegistry._flashActivity, PlayerActivity.Flash, playerContextResource, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Interact | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.ArmsPose);
			}
			return PlayerActivityGate.TryAcquire("GarmentFlash", owner, PlayerActivity.Flash, playerContextResource, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Interact | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.ArmsPose, out FlashGateRegistry._flashActivity, false, null);
		}

		// Token: 0x06008EAE RID: 36526 RVA: 0x003A5620 File Offset: 0x003A3820
		internal static bool HasSettledSeat(Animator actor)
		{
			if (actor == null)
			{
				return false;
			}
			BenchPlayerSeating component = actor.GetComponent<BenchPlayerSeating>();
			return component != null && component.IsSeated && PlayerActivityGate.GetResourceOwner<BenchPlayerSeating>(PlayerContextResource.FullBodyAnimation) == component;
		}

		// Token: 0x06008EAF RID: 36527 RVA: 0x003A5660 File Offset: 0x003A3860
		public static bool IsSeatedClothingActionActiveFor(Animator actor)
		{
			if (!FlashGateRegistry.HasSettledSeat(actor))
			{
				return false;
			}
			for (int i = FlashGateRegistry.ArmsGates.Count - 1; i >= 0; i--)
			{
				ArmsFlashController armsFlashController = FlashGateRegistry.ArmsGates[i];
				if (armsFlashController != null && armsFlashController.isActiveAndEnabled && armsFlashController.PlayerAnimator == actor && armsFlashController.HasActiveArmsAnimation)
				{
					return true;
				}
			}
			ExposeLingerieController resourceOwner = PlayerActivityGate.GetResourceOwner<ExposeLingerieController>(PlayerContextResource.HeldItem);
			return resourceOwner != null && resourceOwner.isActiveAndEnabled && resourceOwner.PlayerAnimator == actor;
		}

		// Token: 0x06008EB0 RID: 36528 RVA: 0x003A56EA File Offset: 0x003A38EA
		internal static void ReleaseActivityIfIdle()
		{
			if (FlashGateRegistry.AnyFlashEngaged)
			{
				return;
			}
			PlayerActivityLease flashActivity = FlashGateRegistry._flashActivity;
			if (flashActivity != null)
			{
				flashActivity.Dispose();
			}
			FlashGateRegistry._flashActivity = null;
		}

		// Token: 0x06008EB1 RID: 36529 RVA: 0x003A570C File Offset: 0x003A390C
		public static void InterruptForEncounter(Transform player)
		{
			FlashGateRegistry.InterruptGarmentsForBodyHandoff(player);
			if (player == null)
			{
				return;
			}
			HideFlashController[] componentsInChildren = player.GetComponentsInChildren<HideFlashController>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].InterruptForDetection();
			}
		}

		// Token: 0x06008EB2 RID: 36530 RVA: 0x003A5748 File Offset: 0x003A3948
		public static void InterruptGarmentsForBodyHandoff(Transform player)
		{
			if (player == null)
			{
				return;
			}
			ArmsFlashController[] componentsInChildren = player.GetComponentsInChildren<ArmsFlashController>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].InterruptForEncounter();
			}
			ClothFullFlashController[] componentsInChildren2 = player.GetComponentsInChildren<ClothFullFlashController>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].InterruptForEncounter();
			}
			ExposeLingerieController[] componentsInChildren3 = player.GetComponentsInChildren<ExposeLingerieController>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].InterruptForEncounter();
			}
			SwimsuitFlashController[] componentsInChildren4 = player.GetComponentsInChildren<SwimsuitFlashController>(true);
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].InterruptForEncounter();
			}
			UniversityUniformSkirtFlashCoordinator[] componentsInChildren5 = player.GetComponentsInChildren<UniversityUniformSkirtFlashCoordinator>(true);
			for (int i = 0; i < componentsInChildren5.Length; i++)
			{
				componentsInChildren5[i].InterruptForEncounter();
			}
			FlashGateRegistry.ReleaseActivityIfIdle();
		}

		// Token: 0x06008EB3 RID: 36531 RVA: 0x003A5800 File Offset: 0x003A3A00
		internal static void Register(ArmsFlashController gate)
		{
			FlashGateRegistry.RegisterUnique<ArmsFlashController>(FlashGateRegistry.ArmsGates, gate);
		}

		// Token: 0x06008EB4 RID: 36532 RVA: 0x003A580D File Offset: 0x003A3A0D
		internal static void Unregister(ArmsFlashController gate)
		{
			FlashGateRegistry.ArmsGates.Remove(gate);
			FlashGateRegistry.ReleaseActivityIfIdle();
		}

		// Token: 0x06008EB5 RID: 36533 RVA: 0x003A5820 File Offset: 0x003A3A20
		internal static void Register(ClothFullFlashController gate)
		{
			FlashGateRegistry.RegisterUnique<ClothFullFlashController>(FlashGateRegistry.FullBodyGates, gate);
		}

		// Token: 0x06008EB6 RID: 36534 RVA: 0x003A582D File Offset: 0x003A3A2D
		internal static void Unregister(ClothFullFlashController gate)
		{
			FlashGateRegistry.FullBodyGates.Remove(gate);
			FlashGateRegistry.ReleaseActivityIfIdle();
		}

		// Token: 0x06008EB7 RID: 36535 RVA: 0x003A5840 File Offset: 0x003A3A40
		private static void RegisterUnique<T>(List<T> gates, T gate) where T : MonoBehaviour, IFlashGate
		{
			if (gate != null && !gates.Contains(gate))
			{
				gates.Add(gate);
			}
		}

		// Token: 0x06008EB8 RID: 36536 RVA: 0x003A5860 File Offset: 0x003A3A60
		private static bool AnyEngagedIn<T>(List<T> gates) where T : MonoBehaviour, IFlashGate
		{
			for (int i = gates.Count - 1; i >= 0; i--)
			{
				T t = gates[i];
				if (t == null)
				{
					gates.RemoveAt(i);
				}
				else if (t.isActiveAndEnabled && t.IsEngaged)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008EB9 RID: 36537 RVA: 0x003A58BC File Offset: 0x003A3ABC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			PlayerActivityLease flashActivity = FlashGateRegistry._flashActivity;
			if (flashActivity != null)
			{
				flashActivity.Dispose();
			}
			FlashGateRegistry._flashActivity = null;
			FlashGateRegistry.ArmsGates.Clear();
			FlashGateRegistry.FullBodyGates.Clear();
		}

		// Token: 0x0400922E RID: 37422
		private static readonly List<ArmsFlashController> ArmsGates = new List<ArmsFlashController>(8);

		// Token: 0x0400922F RID: 37423
		private static readonly List<ClothFullFlashController> FullBodyGates = new List<ClothFullFlashController>(2);

		// Token: 0x04009230 RID: 37424
		private static PlayerActivityLease _flashActivity;

		// Token: 0x04009231 RID: 37425
		private const PlayerActivity FlashBlockedActivities = PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Interact | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.ArmsPose;
	}
}
