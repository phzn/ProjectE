using System;
using System.Collections.Generic;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic.Debugging
{
	// Token: 0x02000BB4 RID: 2996
	[DisallowMultipleComponent]
	public class ToggleableChunkProvider : MonoBehaviour, IChunkProvider
	{
		// Token: 0x14000091 RID: 145
		// (add) Token: 0x060055EF RID: 21999 RVA: 0x00216244 File Offset: 0x00214444
		// (remove) Token: 0x060055F0 RID: 22000 RVA: 0x0021627C File Offset: 0x0021447C
		public event Action<ChunkInfo> OnChunkLoaded;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x060055F1 RID: 22001 RVA: 0x002162B4 File Offset: 0x002144B4
		// (remove) Token: 0x060055F2 RID: 22002 RVA: 0x002162EC File Offset: 0x002144EC
		public event Action<ChunkInfo> OnChunkUnloaded;

		// Token: 0x060055F3 RID: 22003 RVA: 0x00216324 File Offset: 0x00214524
		private void Awake()
		{
			foreach (ToggleableChunkProvider.ChunkEntry chunkEntry in this.chunks)
			{
				if (chunkEntry != null && !(chunkEntry.root == null))
				{
					chunkEntry.loaded = chunkEntry.startLoaded;
					chunkEntry.label = chunkEntry.root.name;
				}
			}
		}

		// Token: 0x060055F4 RID: 22004 RVA: 0x002163A0 File Offset: 0x002145A0
		public IEnumerable<ChunkInfo> GetActiveChunks()
		{
			foreach (ToggleableChunkProvider.ChunkEntry chunkEntry in this.chunks)
			{
				ChunkInfo chunkInfo;
				if (chunkEntry != null && !(chunkEntry.root == null) && chunkEntry.loaded && ToggleableChunkProvider.TryBuildInfo(chunkEntry.root, out chunkInfo))
				{
					yield return chunkInfo;
				}
			}
			List<ToggleableChunkProvider.ChunkEntry>.Enumerator enumerator = default(List<ToggleableChunkProvider.ChunkEntry>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060055F5 RID: 22005 RVA: 0x002163B0 File Offset: 0x002145B0
		public void LoadChunk(GameObject root)
		{
			ToggleableChunkProvider.ChunkEntry chunkEntry = this.Find(root);
			if (chunkEntry == null || chunkEntry.loaded)
			{
				return;
			}
			chunkEntry.loaded = true;
			ChunkInfo chunkInfo;
			if (ToggleableChunkProvider.TryBuildInfo(root, out chunkInfo))
			{
				Action<ChunkInfo> onChunkLoaded = this.OnChunkLoaded;
				if (onChunkLoaded == null)
				{
					return;
				}
				onChunkLoaded(chunkInfo);
			}
		}

		// Token: 0x060055F6 RID: 22006 RVA: 0x002163F4 File Offset: 0x002145F4
		public void UnloadChunk(GameObject root)
		{
			ToggleableChunkProvider.ChunkEntry chunkEntry = this.Find(root);
			if (chunkEntry == null || !chunkEntry.loaded)
			{
				return;
			}
			chunkEntry.loaded = false;
			Action<ChunkInfo> onChunkUnloaded = this.OnChunkUnloaded;
			if (onChunkUnloaded == null)
			{
				return;
			}
			onChunkUnloaded(new ChunkInfo
			{
				id = root.GetInstanceID().ToString(),
				chunkObject = root,
				bounds = new Bounds(root.transform.position, Vector3.one * 60f)
			});
		}

		// Token: 0x060055F7 RID: 22007 RVA: 0x00216478 File Offset: 0x00214678
		public void ToggleChunk(GameObject root)
		{
			ToggleableChunkProvider.ChunkEntry chunkEntry = this.Find(root);
			if (chunkEntry == null)
			{
				return;
			}
			if (chunkEntry.loaded)
			{
				this.UnloadChunk(root);
				return;
			}
			this.LoadChunk(root);
		}

		// Token: 0x060055F8 RID: 22008 RVA: 0x002164A8 File Offset: 0x002146A8
		private ToggleableChunkProvider.ChunkEntry Find(GameObject root)
		{
			foreach (ToggleableChunkProvider.ChunkEntry chunkEntry in this.chunks)
			{
				if (chunkEntry != null && chunkEntry.root == root)
				{
					return chunkEntry;
				}
			}
			return null;
		}

		// Token: 0x060055F9 RID: 22009 RVA: 0x0021650C File Offset: 0x0021470C
		private static bool TryBuildInfo(GameObject root, out ChunkInfo info)
		{
			info = default(ChunkInfo);
			if (root.GetComponent<global::VehicleSpawnPlugin.ChunkData>() == null)
			{
				return false;
			}
			Bounds bounds = new Bounds(root.transform.position, Vector3.one * 60f);
			foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(renderer.bounds);
			}
			info = new ChunkInfo
			{
				id = root.GetInstanceID().ToString(),
				chunkObject = root,
				bounds = bounds
			};
			return true;
		}

		// Token: 0x060055FA RID: 22010 RVA: 0x002165B0 File Offset: 0x002147B0
		private void OnGUI()
		{
			if (!this.showPanel)
			{
				return;
			}
			if (!GameSettingsService.ShowGameplayUI && SceneManager.GetActiveScene().name != "MainMenu")
			{
				return;
			}
			GUILayout.BeginArea(new Rect(10f, 10f, 240f, (float)(20 + this.chunks.Count * 24 + 30)), GUI.skin.box);
			GUILayout.Label("<b>CHUNK TOGGLE (grafo veicoli)</b>", Array.Empty<GUILayoutOption>());
			foreach (ToggleableChunkProvider.ChunkEntry chunkEntry in this.chunks)
			{
				if (chunkEntry != null && !(chunkEntry.root == null))
				{
					GUI.color = (chunkEntry.loaded ? Color.green : Color.gray);
					string text = (chunkEntry.loaded ? "ON " : "off");
					if (GUILayout.Button("[" + text + "] " + chunkEntry.label, Array.Empty<GUILayoutOption>()))
					{
						this.ToggleChunk(chunkEntry.root);
					}
				}
			}
			GUI.color = Color.white;
			GUILayout.EndArea();
		}

		// Token: 0x04005668 RID: 22120
		[Tooltip("Chunk gestiti dal lab. Ognuno DEVE avere un VehicleSpawnPlugin.ChunkData sul root. startLoaded=true → il chunk parte gia' nel grafo (snapshot iniziale).")]
		public List<ToggleableChunkProvider.ChunkEntry> chunks = new List<ToggleableChunkProvider.ChunkEntry>();

		// Token: 0x04005669 RID: 22121
		[Tooltip("Mostra il pannello OnGUI per accendere/spegnere i chunk col mouse durante il Play.")]
		public bool showPanel = true;

		// Token: 0x02000BB5 RID: 2997
		[Serializable]
		public class ChunkEntry
		{
			// Token: 0x0400566C RID: 22124
			public GameObject root;

			// Token: 0x0400566D RID: 22125
			public bool startLoaded = true;

			// Token: 0x0400566E RID: 22126
			[HideInInspector]
			public bool loaded;

			// Token: 0x0400566F RID: 22127
			[HideInInspector]
			public string label;
		}
	}
}
