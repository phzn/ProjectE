using System;
using System.Collections.Generic;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic.Debugging
{
	// Token: 0x02000BBA RID: 3002
	[DisallowMultipleComponent]
	public class TrafficGraphDebugOverlay : MonoBehaviour
	{
		// Token: 0x0600560E RID: 22030 RVA: 0x0021703F File Offset: 0x0021523F
		private void Awake()
		{
			this._provider = this.chunkProviderObject as IChunkProvider;
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x0600560F RID: 22031 RVA: 0x00217052 File Offset: 0x00215252
		private float Threshold
		{
			get
			{
				if (!(this.profile != null))
				{
					return 1.5f;
				}
				return this.profile.connectionThreshold;
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x06005610 RID: 22032 RVA: 0x00217073 File Offset: 0x00215273
		private float MinDot
		{
			get
			{
				if (!(this.profile != null))
				{
					return -0.2f;
				}
				return this.profile.connectionMinDot;
			}
		}

		// Token: 0x06005611 RID: 22033 RVA: 0x00217094 File Offset: 0x00215294
		private void CollectActiveTracks()
		{
			this._tracks.Clear();
			if (this._provider == null)
			{
				return;
			}
			foreach (ChunkInfo chunkInfo in this._provider.GetActiveChunks())
			{
				if (!(chunkInfo.chunkObject == null) && chunkInfo.chunkObject.activeInHierarchy)
				{
					global::VehicleSpawnPlugin.ChunkData component = chunkInfo.chunkObject.GetComponent<global::VehicleSpawnPlugin.ChunkData>();
					if (!(component == null))
					{
						foreach (TrackData trackData in component.tracks)
						{
							if (trackData != null && trackData.HasValidPath)
							{
								this._tracks.Add(trackData);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005612 RID: 22034 RVA: 0x00217188 File Offset: 0x00215388
		private bool HasConnection(TrackData track)
		{
			if (track.isLoop)
			{
				return true;
			}
			Vector3 worldPosition = track.GetWorldPosition(1f);
			Vector3 normalized = track.GetWorldTangent(1f).normalized;
			for (int i = 0; i < this._tracks.Count; i++)
			{
				TrackData trackData = this._tracks[i];
				if (!(trackData == track))
				{
					Vector3 worldPosition2 = trackData.GetWorldPosition(0f);
					if (Vector3.Distance(worldPosition, worldPosition2) <= this.Threshold && Vector3.Dot(normalized, trackData.GetWorldTangent(0f).normalized) >= this.MinDot)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x00217230 File Offset: 0x00215430
		private void OnGUI()
		{
			if (!GameSettingsService.ShowGameplayUI && SceneManager.GetActiveScene().name != "MainMenu")
			{
				return;
			}
			GUIStyle guistyle = new GUIStyle(GUI.skin.box)
			{
				alignment = TextAnchor.UpperLeft,
				fontSize = 12
			};
			string text = string.Concat(new string[]
			{
				"TRAFFIC GRAPH\n",
				string.Format("tracks attivi: {0}\n", this._tracks.Count),
				string.Format("dead-end (rosso): {0}\n", this._deadEndCount),
				string.Format("connessioni (giallo): {0}\n", this._linkCount),
				string.Format("overlap veicoli: {0}", this._overlapCount)
			});
			GUI.color = ((this._deadEndCount > 0) ? new Color(1f, 0.7f, 0.7f) : Color.white);
			GUI.Box(new Rect((float)(Screen.width - 210), 10f, 200f, 92f), text, guistyle);
			GUI.color = Color.white;
		}

		// Token: 0x0400568D RID: 22157
		[Tooltip("SpawnProfile da cui leggere connectionThreshold / connectionMinDot. Se vuoto usa i default (1.5 / -0.2).")]
		public SpawnProfile profile;

		// Token: 0x0400568E RID: 22158
		[Tooltip("Provider che elenca i chunk (per sapere quali sono attivi nel grafo).")]
		public MonoBehaviour chunkProviderObject;

		// Token: 0x0400568F RID: 22159
		[Header("Colori")]
		public Color okColor = new Color(0.2f, 1f, 0.3f, 0.9f);

		// Token: 0x04005690 RID: 22160
		public Color deadEndColor = new Color(1f, 0.15f, 0.15f, 1f);

		// Token: 0x04005691 RID: 22161
		public Color linkColor = new Color(1f, 0.9f, 0.2f, 0.9f);

		// Token: 0x04005692 RID: 22162
		[Header("Overlap veicoli")]
		[Tooltip("Distanza sotto la quale due veicoli attivi sono considerati compenetrati (m).")]
		public float overlapDistance = 3f;

		// Token: 0x04005693 RID: 22163
		private IChunkProvider _provider;

		// Token: 0x04005694 RID: 22164
		private int _deadEndCount;

		// Token: 0x04005695 RID: 22165
		private int _linkCount;

		// Token: 0x04005696 RID: 22166
		private int _overlapCount;

		// Token: 0x04005697 RID: 22167
		private readonly List<TrackData> _tracks = new List<TrackData>();
	}
}
