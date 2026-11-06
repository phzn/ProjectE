using System;
using System.Collections.Generic;
using Features.Localization;
using Features.Missions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x0200040B RID: 1035
	public class TrophyManager : MonoBehaviour
	{
		// Token: 0x06001A53 RID: 6739 RVA: 0x000A69D3 File Offset: 0x000A4BD3
		private void OnEnable()
		{
			MissionService.Changed += this.Rebuild;
			MissionService.Completed += this.HandleCompleted;
			this.Rebuild();
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x000A69FD File Offset: 0x000A4BFD
		private void OnDisable()
		{
			MissionService.Changed -= this.Rebuild;
			MissionService.Completed -= this.HandleCompleted;
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000A6A21 File Offset: 0x000A4C21
		private void HandleCompleted(MissionCompletion completion)
		{
			if (completion.Mission != null && completion.Mission.scope == MissionScope.Run)
			{
				this.Rebuild();
			}
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000A6A40 File Offset: 0x000A4C40
		private void Rebuild()
		{
			for (int i = 0; i < this._rows.Count; i++)
			{
				if (this._rows[i] != null)
				{
					Object.Destroy(this._rows[i]);
				}
			}
			this._rows.Clear();
			IReadOnlyList<MissionCompletion> completed = MissionService.CompletedRunMissions;
			int totalXp = 0;
			for (int j = 0; j < completed.Count; j++)
			{
				MissionCompletion missionCompletion = completed[j];
				totalXp += Mathf.RoundToInt(missionCompletion.XpOutcome.xpGained);
				this.AddMissionRow(missionCompletion, j);
			}
			if (this.recentTrophyText != null)
			{
				LocalizedGameText.BindComputed(this.recentTrophyText, delegate
				{
					if (completed.Count != 0)
					{
						return GameLocalization.FormatSource("phone_story", (completed.Count == 1) ? "{0} mission  |  +{1} XP" : "{0} missions  |  +{1} XP", new object[] { completed.Count, totalXp });
					}
					return GameLocalization.GetSource("phone_story", "No missions completed yet");
				});
			}
			if (this.recentTrophyImage != null && this.trophySprites != null && this.trophySprites.Length != 0)
			{
				this.recentTrophyImage.sprite = this.trophySprites[Mathf.Min(1, this.trophySprites.Length - 1)];
			}
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000A6B5C File Offset: 0x000A4D5C
		private void AddMissionRow(MissionCompletion entry, int index)
		{
			if (this.trophyPrefab == null || this.trophyParent == null || entry.Mission == null)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.trophyPrefab, this.trophyParent, false);
			gameObject.name = "Completed Mission " + (index + 1).ToString();
			this._rows.Add(gameObject);
			TrophyManager.BindComputed(gameObject.transform, "Title", () => GameLocalization.FormatSource("phone_story", "Mission {0:00}", new object[] { index + 1 }));
			TrophyManager.BindComputed(gameObject.transform, "Description", () => MissionLocalization.Full(entry.Mission));
			TrophyManager.BindComputed(gameObject.transform, "Status", () => GameLocalization.FormatSource("phone_story", "+{0} XP", new object[] { Mathf.RoundToInt(entry.XpOutcome.xpGained) }));
			TrophyManager.SetText(gameObject.transform, "ProgressPercentage", "Complete");
			Transform transform = gameObject.transform.Find("ProgressImage");
			if (transform != null)
			{
				Image component = transform.GetComponent<Image>();
				if (component != null)
				{
					component.fillAmount = 1f;
				}
			}
			Transform transform2 = gameObject.transform.Find("TrophyImage");
			if (transform2 != null && this.trophySprites != null && this.trophySprites.Length != 0)
			{
				Image component2 = transform2.GetComponent<Image>();
				if (component2 != null)
				{
					component2.sprite = this.trophySprites[index % this.trophySprites.Length];
				}
			}
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x000A6CE4 File Offset: 0x000A4EE4
		private static void SetText(Transform root, string childName, string value)
		{
			Transform transform = root.Find(childName);
			if (transform == null)
			{
				return;
			}
			TMP_Text component = transform.GetComponent<TMP_Text>();
			if (component != null)
			{
				LocalizedGameText.BindSource(component, "phone_story", value ?? string.Empty, null);
			}
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x000A6D2C File Offset: 0x000A4F2C
		private static void BindComputed(Transform root, string childName, Func<string> resolver)
		{
			Transform transform = root.Find(childName);
			if (transform != null)
			{
				LocalizedGameText.BindComputed(transform.GetComponent<TMP_Text>(), resolver);
			}
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x000A6D57 File Offset: 0x000A4F57
		public void SetTrophyClaimed(string title, string status, float progress, int sprite)
		{
			this.Rebuild();
		}

		// Token: 0x04001B64 RID: 7012
		public GameObject trophyPrefab;

		// Token: 0x04001B65 RID: 7013
		public Transform trophyParent;

		// Token: 0x04001B66 RID: 7014
		public Sprite[] trophySprites;

		// Token: 0x04001B67 RID: 7015
		public List<TrophyManager.TrophyData> trophyDataList;

		// Token: 0x04001B68 RID: 7016
		public Image recentTrophyImage;

		// Token: 0x04001B69 RID: 7017
		public TMP_Text recentTrophyText;

		// Token: 0x04001B6A RID: 7018
		private readonly List<GameObject> _rows = new List<GameObject>();

		// Token: 0x0200040C RID: 1036
		[Serializable]
		public class TrophyData
		{
			// Token: 0x04001B6B RID: 7019
			public string Title;

			// Token: 0x04001B6C RID: 7020
			public string Description;

			// Token: 0x04001B6D RID: 7021
			public string Status;

			// Token: 0x04001B6E RID: 7022
			public int SpriteIndex;

			// Token: 0x04001B6F RID: 7023
			public float ProgressPercentage;
		}
	}
}
