using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x02000912 RID: 2322
	public static class InteractionCandidateRegistry
	{
		// Token: 0x06003ED4 RID: 16084 RVA: 0x00175514 File Offset: 0x00173714
		public static string ActiveBinding(GenericInput input)
		{
			if (input == null || !input.useInput)
			{
				return string.Empty;
			}
			if (!ControllerInput.IsActive)
			{
				return input.buttonName ?? string.Empty;
			}
			string text = input.joystick;
			KeyCode keyCode;
			if (!input.preserveJoystickBinding && ControllerGameplayBridge.TryKey(input.keyboard, out keyCode) && ControllerBindings.HasKey(keyCode))
			{
				text = ControllerInput.ForKey(keyCode);
			}
			return (text ?? string.Empty).Replace("D-Pad ", "DPad");
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x00175590 File Offset: 0x00173790
		public static bool IsInventoryInputReserved(GenericInput input)
		{
			string text = InteractionCandidateRegistry.ActiveBinding(input);
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			if (InteractionCandidateRegistry._consumedInputFrame == Time.frameCount && InteractionCandidateRegistry._consumedInventoryBindings.Contains(text))
			{
				return true;
			}
			if (!InteractionCandidateRegistry.CanStartWorldInteraction)
			{
				return false;
			}
			for (int i = 0; i < InteractionCandidateRegistry._all.Count; i++)
			{
				Object @object = InteractionCandidateRegistry._all[i] as Object;
				if (@object == null || !(@object == null))
				{
					IInteractionInputPriority interactionInputPriority = InteractionCandidateRegistry._all[i] as IInteractionInputPriority;
					if (interactionInputPriority != null && interactionInputPriority.ReservesInventoryBinding(text))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x00175624 File Offset: 0x00173824
		public static bool IsContextInputReserved(GenericInput input)
		{
			if (InteractionCandidateRegistry.IsInventoryInputReserved(input))
			{
				return true;
			}
			string text = InteractionCandidateRegistry.ActiveBinding(input);
			if (string.IsNullOrEmpty(text) || !InteractionCandidateRegistry.CanStartWorldInteraction)
			{
				return false;
			}
			for (int i = 0; i < InteractionCandidateRegistry._all.Count; i++)
			{
				Object @object = InteractionCandidateRegistry._all[i] as Object;
				if (@object == null || !(@object == null))
				{
					IInteractionInputCandidate interactionInputCandidate = InteractionCandidateRegistry._all[i] as IInteractionInputCandidate;
					if (interactionInputCandidate != null && interactionInputCandidate.IsCandidateEligible && string.Equals(text, interactionInputCandidate.CandidateInputBinding, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x001756B4 File Offset: 0x001738B4
		public static void MarkContextInputConsumed(GenericInput input)
		{
			string text = InteractionCandidateRegistry.ActiveBinding(input);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (InteractionCandidateRegistry._consumedInputFrame != Time.frameCount)
			{
				InteractionCandidateRegistry._consumedInputFrame = Time.frameCount;
				InteractionCandidateRegistry._consumedInventoryBindings.Clear();
			}
			InteractionCandidateRegistry._consumedInventoryBindings.Add(text);
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x00175700 File Offset: 0x00173900
		public static bool TryConsumeContextInput(GenericInput input)
		{
			string text = InteractionCandidateRegistry.ActiveBinding(input);
			if (string.IsNullOrEmpty(text) || (InteractionCandidateRegistry._consumedInputFrame == Time.frameCount && InteractionCandidateRegistry._consumedInventoryBindings.Contains(text)))
			{
				return false;
			}
			InteractionCandidateRegistry.MarkContextInputConsumed(input);
			return true;
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003ED9 RID: 16089 RVA: 0x0017573E File Offset: 0x0017393E
		// (set) Token: 0x06003EDA RID: 16090 RVA: 0x00175745 File Offset: 0x00173945
		public static float LastInteractTime { get; private set; } = -999f;

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06003EDB RID: 16091 RVA: 0x0017574D File Offset: 0x0017394D
		public static bool HasInteractedThisFrame
		{
			get
			{
				return InteractionCandidateRegistry._lastInteractFrame == Time.frameCount;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06003EDC RID: 16092 RVA: 0x0017575B File Offset: 0x0017395B
		public static bool CanStartWorldInteraction
		{
			get
			{
				return PlayerActivityGate.CanStartWorldInteraction;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06003EDD RID: 16093 RVA: 0x00175764 File Offset: 0x00173964
		public static bool HasEligibleCandidate
		{
			get
			{
				bool canStartWorldInteraction = InteractionCandidateRegistry.CanStartWorldInteraction;
				for (int i = 0; i < InteractionCandidateRegistry._all.Count; i++)
				{
					if (InteractionCandidateRegistry.CanParticipate(InteractionCandidateRegistry._all[i], canStartWorldInteraction))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x001757A2 File Offset: 0x001739A2
		public static void MarkInteract()
		{
			InteractionCandidateRegistry.LastInteractTime = Time.time;
			InteractionCandidateRegistry._lastInteractFrame = Time.frameCount;
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x001757B8 File Offset: 0x001739B8
		public static bool TryCommitWorldInteraction()
		{
			if (!InteractionCandidateRegistry.CanStartWorldInteraction || InteractionCandidateRegistry.HasInteractedThisFrame)
			{
				return false;
			}
			InteractionCandidateRegistry.MarkInteract();
			return true;
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x001757D0 File Offset: 0x001739D0
		public static void Register(IInteractionCandidate candidate)
		{
			if (candidate != null && !InteractionCandidateRegistry._all.Contains(candidate))
			{
				InteractionCandidateRegistry._all.Add(candidate);
			}
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x001757ED File Offset: 0x001739ED
		public static void Unregister(IInteractionCandidate candidate)
		{
			InteractionCandidateRegistry._all.Remove(candidate);
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x001757FB File Offset: 0x001739FB
		public static bool IsWinner(IInteractionCandidate candidate)
		{
			return InteractionCandidateRegistry.IsWinner(candidate, true);
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00175804 File Offset: 0x00173A04
		public static bool IsPromptWinner(IInteractionCandidate candidate)
		{
			return InteractionCandidateRegistry.IsWinner(candidate, false);
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x00175810 File Offset: 0x00173A10
		private static bool IsWinner(IInteractionCandidate candidate, bool separateInputBindings)
		{
			bool canStartWorldInteraction = InteractionCandidateRegistry.CanStartWorldInteraction;
			if (InteractionCandidateRegistry.HasInteractedThisFrame || !InteractionCandidateRegistry.CanParticipate(candidate, canStartWorldInteraction))
			{
				return false;
			}
			bool flag = candidate is IInteractionFallbackCandidate;
			string text;
			if (separateInputBindings)
			{
				IInteractionInputCandidate interactionInputCandidate = candidate as IInteractionInputCandidate;
				if (interactionInputCandidate != null)
				{
					text = interactionInputCandidate.CandidateInputBinding;
					goto IL_003C;
				}
			}
			text = string.Empty;
			IL_003C:
			string text2 = text;
			float candidateSqrDistance = candidate.CandidateSqrDistance;
			for (int i = 0; i < InteractionCandidateRegistry._all.Count; i++)
			{
				IInteractionCandidate interactionCandidate = InteractionCandidateRegistry._all[i];
				if (interactionCandidate != null && interactionCandidate != candidate && InteractionCandidateRegistry.CanParticipate(interactionCandidate, canStartWorldInteraction))
				{
					bool flag2 = interactionCandidate is IInteractionFallbackCandidate;
					if (flag && !flag2)
					{
						return false;
					}
					if (flag || !flag2)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							IInteractionInputCandidate interactionInputCandidate2 = interactionCandidate as IInteractionInputCandidate;
							if (interactionInputCandidate2 != null)
							{
								string candidateInputBinding = interactionInputCandidate2.CandidateInputBinding;
								if (!string.IsNullOrEmpty(candidateInputBinding) && !string.Equals(text2, candidateInputBinding, StringComparison.OrdinalIgnoreCase))
								{
									goto IL_0131;
								}
							}
						}
						IInteractionPickupCandidate interactionPickupCandidate = candidate as IInteractionPickupCandidate;
						bool flag3 = interactionPickupCandidate != null && interactionPickupCandidate.CanPrioritizePickup;
						IInteractionPickupCandidate interactionPickupCandidate2 = interactionCandidate as IInteractionPickupCandidate;
						bool flag4 = interactionPickupCandidate2 != null && interactionPickupCandidate2.CanPrioritizePickup;
						if (flag3 != flag4)
						{
							if (flag4)
							{
								return false;
							}
						}
						else
						{
							if (interactionCandidate.CandidateSqrDistance < candidateSqrDistance)
							{
								return false;
							}
							if (Mathf.Approximately(interactionCandidate.CandidateSqrDistance, candidateSqrDistance) && interactionCandidate.CandidateInstanceId < candidate.CandidateInstanceId)
							{
								return false;
							}
						}
					}
				}
				IL_0131:;
			}
			return true;
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x00175968 File Offset: 0x00173B68
		private static bool CanParticipate(IInteractionCandidate candidate, bool worldInteractionOpen)
		{
			if (candidate != null)
			{
				Object @object = candidate as Object;
				if ((@object == null || !(@object == null)) && candidate.IsCandidateEligible)
				{
					if (!worldInteractionOpen)
					{
						IInteractionHandoffCandidate interactionHandoffCandidate = candidate as IInteractionHandoffCandidate;
						return interactionHandoffCandidate != null && interactionHandoffCandidate.CanPrepareInteraction;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x001759AC File Offset: 0x00173BAC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			InteractionCandidateRegistry._all.Clear();
			InteractionCandidateRegistry.LastInteractTime = -999f;
			InteractionCandidateRegistry._lastInteractFrame = -1;
			InteractionCandidateRegistry._consumedInventoryBindings.Clear();
			InteractionCandidateRegistry._consumedInputFrame = -1;
		}

		// Token: 0x04003ED7 RID: 16087
		private static readonly List<IInteractionCandidate> _all = new List<IInteractionCandidate>(32);

		// Token: 0x04003ED8 RID: 16088
		private static readonly HashSet<string> _consumedInventoryBindings = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04003ED9 RID: 16089
		private static int _consumedInputFrame = -1;

		// Token: 0x04003EDB RID: 16091
		private static int _lastInteractFrame = -1;
	}
}
