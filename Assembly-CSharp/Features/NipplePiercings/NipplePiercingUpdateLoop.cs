using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Features.NipplePiercings
{
	// Token: 0x02000F6E RID: 3950
	internal static class NipplePiercingUpdateLoop
	{
		// Token: 0x060076EA RID: 30442 RVA: 0x00305A3C File Offset: 0x00303C3C
		internal static void Add(NipplePiercingAttachment attachment)
		{
			if (!NipplePiercingUpdateLoop.Attachments.Contains(attachment))
			{
				NipplePiercingUpdateLoop.Attachments.Add(attachment);
			}
			if (!Application.isPlaying)
			{
				return;
			}
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			if (NipplePiercingUpdateLoop.Install(ref currentPlayerLoop))
			{
				PlayerLoop.SetPlayerLoop(currentPlayerLoop);
			}
		}

		// Token: 0x060076EB RID: 30443 RVA: 0x00305A7E File Offset: 0x00303C7E
		internal static void Remove(NipplePiercingAttachment attachment)
		{
			NipplePiercingUpdateLoop.Attachments.Remove(attachment);
		}

		// Token: 0x060076EC RID: 30444 RVA: 0x00305A8C File Offset: 0x00303C8C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetState()
		{
			NipplePiercingUpdateLoop.Attachments.Clear();
			NipplePiercingUpdateLoop.FrameAttachments.Clear();
		}

		// Token: 0x060076ED RID: 30445 RVA: 0x00305AA4 File Offset: 0x00303CA4
		private static bool Install(ref PlayerLoopSystem loop)
		{
			if (loop.subSystemList == null)
			{
				return false;
			}
			for (int i = 0; i < loop.subSystemList.Length; i++)
			{
				if (!(loop.subSystemList[i].type != typeof(PostLateUpdate)))
				{
					PlayerLoopSystem playerLoopSystem = loop.subSystemList[i];
					PlayerLoopSystem[] subSystemList = playerLoopSystem.subSystemList;
					if (subSystemList == null)
					{
						return false;
					}
					PlayerLoopSystem[] array = subSystemList;
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].type == typeof(NipplePiercingUpdateLoop.FitBeforeRendering))
						{
							return false;
						}
					}
					for (int k = 0; k < subSystemList.Length; k++)
					{
						if (!(subSystemList[k].type != typeof(PostLateUpdate.UpdateAllRenderers)))
						{
							PlayerLoopSystem[] array2 = new PlayerLoopSystem[subSystemList.Length + 1];
							Array.Copy(subSystemList, 0, array2, 0, k);
							array2[k] = new PlayerLoopSystem
							{
								type = typeof(NipplePiercingUpdateLoop.FitBeforeRendering),
								updateDelegate = new PlayerLoopSystem.UpdateFunction(NipplePiercingUpdateLoop.Fit)
							};
							Array.Copy(subSystemList, k, array2, k + 1, subSystemList.Length - k);
							playerLoopSystem.subSystemList = array2;
							loop.subSystemList[i] = playerLoopSystem;
							return true;
						}
					}
				}
			}
			Debug.LogError("[NipplePiercing] Could not locate the final renderer update in the player loop.");
			return false;
		}

		// Token: 0x060076EE RID: 30446 RVA: 0x00305C04 File Offset: 0x00303E04
		private static void Fit()
		{
			if (!Application.isPlaying || NipplePiercingUpdateLoop.Attachments.Count == 0)
			{
				return;
			}
			NipplePiercingUpdateLoop.FrameAttachments.Clear();
			NipplePiercingUpdateLoop.FrameAttachments.AddRange(NipplePiercingUpdateLoop.Attachments);
			foreach (NipplePiercingAttachment nipplePiercingAttachment in NipplePiercingUpdateLoop.FrameAttachments)
			{
				if (!(nipplePiercingAttachment == null) && nipplePiercingAttachment.isActiveAndEnabled)
				{
					try
					{
						nipplePiercingAttachment.RefreshAfterDeformation();
					}
					catch (Exception ex)
					{
						Debug.LogException(ex, nipplePiercingAttachment);
					}
				}
			}
			NipplePiercingUpdateLoop.FrameAttachments.Clear();
		}

		// Token: 0x04007A3C RID: 31292
		private static readonly List<NipplePiercingAttachment> Attachments = new List<NipplePiercingAttachment>();

		// Token: 0x04007A3D RID: 31293
		private static readonly List<NipplePiercingAttachment> FrameAttachments = new List<NipplePiercingAttachment>();

		// Token: 0x02000F6F RID: 3951
		private struct FitBeforeRendering
		{
		}
	}
}
