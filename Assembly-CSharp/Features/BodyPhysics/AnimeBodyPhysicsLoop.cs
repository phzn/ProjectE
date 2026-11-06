using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Features.BodyPhysics
{
	// Token: 0x020014FF RID: 5375
	public static class AnimeBodyPhysicsLoop
	{
		// Token: 0x0600A617 RID: 42519 RVA: 0x00456B00 File Offset: 0x00454D00
		public static void Add(Action restore, Action write)
		{
			AnimeBodyPhysicsLoop.Install();
			if (restore != null)
			{
				AnimeBodyPhysicsLoop.Restores.Add(restore);
			}
			if (write != null)
			{
				AnimeBodyPhysicsLoop.Writes.Add(write);
			}
		}

		// Token: 0x0600A618 RID: 42520 RVA: 0x00456B23 File Offset: 0x00454D23
		public static void Remove(Action restore, Action write)
		{
			if (restore != null)
			{
				AnimeBodyPhysicsLoop.Restores.Remove(restore);
			}
			if (write != null)
			{
				AnimeBodyPhysicsLoop.Writes.Remove(write);
			}
		}

		// Token: 0x0600A619 RID: 42521 RVA: 0x00456B41 File Offset: 0x00454D41
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetState()
		{
			AnimeBodyPhysicsLoop.Restores.Clear();
			AnimeBodyPhysicsLoop.Writes.Clear();
		}

		// Token: 0x0600A61A RID: 42522 RVA: 0x00456B58 File Offset: 0x00454D58
		private static void Install()
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			if (currentPlayerLoop.subSystemList == null)
			{
				return;
			}
			if (AnimeBodyPhysicsLoop.InstallFirst(ref currentPlayerLoop, typeof(EarlyUpdate), typeof(AnimeBodyPhysicsLoop.AnimeBodyPhysicsRestore), new PlayerLoopSystem.UpdateFunction(AnimeBodyPhysicsLoop.Restores.Run)) | AnimeBodyPhysicsLoop.InstallFirst(ref currentPlayerLoop, typeof(PostLateUpdate), typeof(AnimeBodyPhysicsLoop.AnimeBodyPhysicsWrite), new PlayerLoopSystem.UpdateFunction(AnimeBodyPhysicsLoop.Writes.Run)))
			{
				PlayerLoop.SetPlayerLoop(currentPlayerLoop);
			}
		}

		// Token: 0x0600A61B RID: 42523 RVA: 0x00456BD4 File Offset: 0x00454DD4
		private static bool InstallFirst(ref PlayerLoopSystem loop, Type categoryType, Type entryType, PlayerLoopSystem.UpdateFunction update)
		{
			for (int i = 0; i < loop.subSystemList.Length; i++)
			{
				if (!(loop.subSystemList[i].type != categoryType))
				{
					PlayerLoopSystem playerLoopSystem = loop.subSystemList[i];
					PlayerLoopSystem[] array = playerLoopSystem.subSystemList ?? Array.Empty<PlayerLoopSystem>();
					PlayerLoopSystem[] array2 = array;
					for (int j = 0; j < array2.Length; j++)
					{
						if (array2[j].type == entryType)
						{
							return false;
						}
					}
					PlayerLoopSystem[] array3 = new PlayerLoopSystem[array.Length + 1];
					array3[0] = new PlayerLoopSystem
					{
						type = entryType,
						updateDelegate = update
					};
					Array.Copy(array, 0, array3, 1, array.Length);
					playerLoopSystem.subSystemList = array3;
					loop.subSystemList[i] = playerLoopSystem;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400AAEC RID: 43756
		private static readonly AnimeBodyPhysicsLoop.Pass Restores = new AnimeBodyPhysicsLoop.Pass();

		// Token: 0x0400AAED RID: 43757
		private static readonly AnimeBodyPhysicsLoop.Pass Writes = new AnimeBodyPhysicsLoop.Pass();

		// Token: 0x02001500 RID: 5376
		private struct AnimeBodyPhysicsRestore
		{
		}

		// Token: 0x02001501 RID: 5377
		private struct AnimeBodyPhysicsWrite
		{
		}

		// Token: 0x02001502 RID: 5378
		private sealed class Pass
		{
			// Token: 0x0600A61D RID: 42525 RVA: 0x00456CC7 File Offset: 0x00454EC7
			public void Add(Action action)
			{
				if (this.Running)
				{
					if (!this.Pending.Contains(action))
					{
						this.Pending.Add(action);
						return;
					}
				}
				else if (!this.Actions.Contains(action))
				{
					this.Actions.Add(action);
				}
			}

			// Token: 0x0600A61E RID: 42526 RVA: 0x00456D06 File Offset: 0x00454F06
			public void Remove(Action action)
			{
				this.Actions.Remove(action);
				this.Pending.Remove(action);
			}

			// Token: 0x0600A61F RID: 42527 RVA: 0x00456D22 File Offset: 0x00454F22
			public void Clear()
			{
				this.Actions.Clear();
				this.Pending.Clear();
				this.Running = false;
			}

			// Token: 0x0600A620 RID: 42528 RVA: 0x00456D44 File Offset: 0x00454F44
			public void Run()
			{
				if (!Application.isPlaying || this.Actions.Count == 0)
				{
					return;
				}
				this.Running = true;
				try
				{
					for (int i = 0; i < this.Actions.Count; i++)
					{
						try
						{
							this.Actions[i]();
						}
						catch (Exception ex)
						{
							Debug.LogException(ex);
						}
					}
				}
				finally
				{
					this.Running = false;
					foreach (Action action in this.Pending)
					{
						if (!this.Actions.Contains(action))
						{
							this.Actions.Add(action);
						}
					}
					this.Pending.Clear();
				}
			}

			// Token: 0x0400AAEE RID: 43758
			public readonly List<Action> Actions = new List<Action>();

			// Token: 0x0400AAEF RID: 43759
			public readonly List<Action> Pending = new List<Action>();

			// Token: 0x0400AAF0 RID: 43760
			public bool Running;
		}
	}
}
