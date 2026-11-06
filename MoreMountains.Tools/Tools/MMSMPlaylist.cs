using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000C6 RID: 198
	[CreateAssetMenu(menuName = "MoreMountains/Audio/MMSM Playlist")]
	[Serializable]
	public class MMSMPlaylist : ScriptableObject
	{
		// Token: 0x0600040D RID: 1037 RVA: 0x00013204 File Offset: 0x00011404
		public virtual void Initialization()
		{
			this.PlayCount = 0;
			this._randomUniqueCandidates = new List<int>();
			foreach (MMSMPlaylistSong mmsmplaylistSong in this.Songs)
			{
				mmsmplaylistSong.Initialization();
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00013268 File Offset: 0x00011468
		public virtual int PickNextIndex(int direction, int currentSongIndex, ref int queuedSongIndex, bool bypassLoop)
		{
			int num = currentSongIndex;
			if (this.Songs.Count == 0)
			{
				return -1;
			}
			if (queuedSongIndex != -1)
			{
				int num2 = queuedSongIndex;
				queuedSongIndex = -1;
				return num2;
			}
			if (this.PlayCount >= this.Songs.Count && this.PlayMode == MMSMPlaylist.PlayModes.PlayOnce)
			{
				return -2;
			}
			if (this.PlayMode == MMSMPlaylist.PlayModes.PlayXTimes && this.PlayCount >= this.MaxAmountOfPlays)
			{
				return -2;
			}
			if (currentSongIndex >= 0 && currentSongIndex < this.Songs.Count && this.Songs[currentSongIndex].Options.Loop && !bypassLoop)
			{
				return currentSongIndex;
			}
			switch (this.PlayOrder)
			{
			case MMSMPlaylist.PlayOrders.ReverseOrder:
				direction = -1;
				break;
			case MMSMPlaylist.PlayOrders.Random:
				while (num == currentSongIndex)
				{
					num = Random.Range(0, this.Songs.Count);
				}
				return num;
			case MMSMPlaylist.PlayOrders.RandomUnique:
			{
				bool flag = true;
				int num3 = int.MaxValue;
				this._randomUniqueCandidates.Clear();
				for (int i = 0; i < this.Songs.Count; i++)
				{
					if (this.Songs[i].PlayCount <= num3 && i != currentSongIndex)
					{
						flag = false;
						num3 = this.Songs[i].PlayCount;
						this._randomUniqueCandidates.Add(i);
					}
				}
				if (flag)
				{
					while (num == currentSongIndex)
					{
						num = Random.Range(0, this.Songs.Count);
					}
				}
				else
				{
					int num4 = Random.Range(0, this._randomUniqueCandidates.Count);
					num = this._randomUniqueCandidates[num4];
				}
				return num;
			}
			}
			if (direction > 0)
			{
				num = (currentSongIndex + 1) % this.Songs.Count;
			}
			else
			{
				num = currentSongIndex - 1;
				if (num < 0)
				{
					num = this.Songs.Count - 1;
				}
			}
			return num;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00013410 File Offset: 0x00011610
		public virtual void ResetPlayCount()
		{
			this.PlayCount = 0;
			foreach (MMSMPlaylistSong mmsmplaylistSong in this.Songs)
			{
				mmsmplaylistSong.PlayCount = 0;
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00013468 File Offset: 0x00011668
		protected virtual void OnValidate()
		{
			foreach (MMSMPlaylistSong mmsmplaylistSong in this.Songs)
			{
				if (!mmsmplaylistSong.Options.Initialized)
				{
					mmsmplaylistSong.Options = MMSoundManagerPlayOptions.Default;
				}
			}
		}

		// Token: 0x04000557 RID: 1367
		[Header("Play Modes")]
		[Tooltip("the sound manager track on which to play this playlist's songs")]
		public MMSoundManager.MMSoundManagerTracks Track = MMSoundManager.MMSoundManagerTracks.Music;

		// Token: 0x04000558 RID: 1368
		[Tooltip("the order in which to play songs (top to bottom, bottom to top, random, or random while trying to maintain playcount across songs")]
		public MMSMPlaylist.PlayOrders PlayOrder;

		// Token: 0x04000559 RID: 1369
		[Tooltip("if this is true, random seed will be randomized by the system clock")]
		[MMEnumCondition("PlayOrder", new int[] { 2, 3 })]
		public bool RandomizeOrderSeed = true;

		// Token: 0x0400055A RID: 1370
		[Tooltip("whether to play this playlist forever, only once, or play songs until total playcount reaches MaxAmountOfPlays")]
		public MMSMPlaylist.PlayModes PlayMode;

		// Token: 0x0400055B RID: 1371
		[Tooltip("when in PlayXTimes mode, the max amount of plays before this playlist ends")]
		[MMEnumCondition("PlayMode", new int[] { 2 })]
		public int MaxAmountOfPlays = 10;

		// Token: 0x0400055C RID: 1372
		[Tooltip("a playlist to switch to when reaching the end of this playlist")]
		[MMEnumCondition("PlayMode", new int[] { 1, 2 })]
		public MMSMPlaylist NextPlaylist;

		// Token: 0x0400055D RID: 1373
		[Tooltip("the list of songs to play on this playlist")]
		public List<MMSMPlaylistSong> Songs;

		// Token: 0x0400055E RID: 1374
		[Header("Debug")]
		[Tooltip("the total number of times songs in this playlist have been played ")]
		[MMReadOnly]
		public int PlayCount;

		// Token: 0x0400055F RID: 1375
		protected List<int> _randomUniqueCandidates;

		// Token: 0x0200040C RID: 1036
		public enum PlayModes
		{
			// Token: 0x04001ED4 RID: 7892
			PlayForever,
			// Token: 0x04001ED5 RID: 7893
			PlayOnce,
			// Token: 0x04001ED6 RID: 7894
			PlayXTimes
		}

		// Token: 0x0200040D RID: 1037
		public enum PlayOrders
		{
			// Token: 0x04001ED8 RID: 7896
			Normal,
			// Token: 0x04001ED9 RID: 7897
			ReverseOrder,
			// Token: 0x04001EDA RID: 7898
			Random,
			// Token: 0x04001EDB RID: 7899
			RandomUnique
		}
	}
}
