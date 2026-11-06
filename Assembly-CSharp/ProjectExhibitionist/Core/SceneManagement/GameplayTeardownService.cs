using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.Core.SceneManagement
{
	// Token: 0x02000A78 RID: 2680
	public static class GameplayTeardownService
	{
		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06004C3E RID: 19518 RVA: 0x001DA7D9 File Offset: 0x001D89D9
		// (set) Token: 0x06004C3F RID: 19519 RVA: 0x001DA7E0 File Offset: 0x001D89E0
		public static bool IsRunning { get; private set; }

		// Token: 0x06004C40 RID: 19520 RVA: 0x001DA7E8 File Offset: 0x001D89E8
		public static IEnumerator FastTeardownRoutine()
		{
			return GameplayTeardownService.RunWithTeardownOwnership(GameplayTeardownService.FastTeardownBody());
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x001DA7F4 File Offset: 0x001D89F4
		private static IEnumerator RunWithTeardownOwnership(IEnumerator work)
		{
			if (GameplayTeardownService.IsRunning)
			{
				yield break;
			}
			GameplayTeardownService.IsRunning = true;
			try
			{
				while (work.MoveNext())
				{
					object obj = work.Current;
					yield return obj;
				}
			}
			finally
			{
				try
				{
					IDisposable disposable = work as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				finally
				{
					GameplayTeardownService.IsRunning = false;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x001DA803 File Offset: 0x001D8A03
		private static IEnumerator FastTeardownBody()
		{
			List<GameObject> roots = new List<GameObject>(512);
			if (NPCZoneManager.Instance != null)
			{
				NPCZoneManager.Instance.BeginFastTeardown(roots);
			}
			NPCRuntimeSpawner npcruntimeSpawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
			if (npcruntimeSpawner != null)
			{
				npcruntimeSpawner.BeginFastTeardown(roots);
			}
			if (ChunkManager.Instance != null)
			{
				ChunkManager.Instance.BeginFastTeardown(roots);
			}
			if (roots.Count == 0)
			{
				yield break;
			}
			float start = Time.realtimeSinceStartup;
			int destroyedThisFrame = 0;
			int num;
			for (int i = 0; i < roots.Count; i = num + 1)
			{
				GameObject gameObject = roots[i];
				if (!(gameObject == null))
				{
					Object.Destroy(gameObject);
					num = destroyedThisFrame;
					destroyedThisFrame = num + 1;
					if (destroyedThisFrame >= 12)
					{
						destroyedThisFrame = 0;
						yield return null;
					}
				}
				num = i;
			}
			yield return null;
			Debug.Log(string.Format("[GameplayTeardownService] Fast teardown completato: {0} root distrutti ", roots.Count) + string.Format("in {0:F0} ms (a lotti di {1}/frame).", (Time.realtimeSinceStartup - start) * 1000f, 12));
			yield break;
		}

		// Token: 0x04004DC5 RID: 19909
		private const int DestroysPerFrame = 12;
	}
}
