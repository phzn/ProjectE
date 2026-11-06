using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invector;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class vShooterScore : MonoBehaviour
{
	// Token: 0x06000A3F RID: 2623 RVA: 0x00046864 File Offset: 0x00044A64
	public void AddScore(vShooterScore.ScorePoint score)
	{
		this.scoreDisplay.currentScore += score.value;
		this.scoreDisplay.ShowValue();
		if (this.hitCounters.Length != 0 && score.id < this.hitCounters.Length)
		{
			vShooterScore.TargetPointCounter targetPointCounter = this.hitCounters[score.id];
			if (targetPointCounter != null)
			{
				targetPointCounter.currentScore += 1f;
				targetPointCounter.ShowValue();
			}
		}
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x000468D8 File Offset: 0x00044AD8
	internal void StartScore()
	{
		this.scoreDisplay.currentScore = 0f;
		vShooterScore.TargetPointCounter[] array = this.hitCounters;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].currentScore = 0f;
		}
		this.scoreDATAList = this.LoadData("ShooterScore");
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x00046928 File Offset: 0x00044B28
	internal void FinishScore()
	{
		if (this.scoreDATAList != null)
		{
			vShooterScore.ScoreDATA scoreDATA = new vShooterScore.ScoreDATA();
			scoreDATA.score = this.scoreDisplay.currentScore;
			scoreDATA.hits = new List<float>();
			foreach (vShooterScore.TargetPointCounter targetPointCounter in this.hitCounters)
			{
				scoreDATA.hits.Add(targetPointCounter.currentScore);
			}
			if (this.scoreDATAList.datas.Count < this.dataDisplays.Length)
			{
				this.scoreDATAList.datas.Add(scoreDATA);
				this.scoreDATAList.datas = this.scoreDATAList.datas.OrderBy<vShooterScore.ScoreDATA, float>((vShooterScore.ScoreDATA d) => d.score).Reverse<vShooterScore.ScoreDATA>().ToList<vShooterScore.ScoreDATA>();
			}
			else
			{
				this.scoreDATAList.datas.Add(scoreDATA);
				this.scoreDATAList.datas = this.scoreDATAList.datas.OrderBy<vShooterScore.ScoreDATA, float>((vShooterScore.ScoreDATA d) => d.score).Reverse<vShooterScore.ScoreDATA>().ToList<vShooterScore.ScoreDATA>();
				int num = this.scoreDATAList.datas.Count - this.dataDisplays.Length;
				for (int j = 0; j < num; j++)
				{
					if (this.scoreDATAList.datas[this.scoreDATAList.datas.Count - 1].Equals(scoreDATA))
					{
					}
					this.scoreDATAList.datas.RemoveAt(this.scoreDATAList.datas.Count - 1);
				}
			}
			this.SaveData("ShooterScore");
			this.ShowData();
		}
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x00046AE7 File Offset: 0x00044CE7
	public void ClearData()
	{
		this.scoreDATAList = new vShooterScore.ScoreDATAList();
		this.SaveData("ShooterScore");
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x00046B00 File Offset: 0x00044D00
	public void ShowData()
	{
		if (this.scoreDATAList == null || !Application.isPlaying)
		{
			this.scoreDATAList = this.LoadData("ShooterScore");
		}
		for (int i = 0; i < this.dataDisplays.Length; i++)
		{
			if (this.scoreDATAList.datas.Count > 0 && i < this.scoreDATAList.datas.Count)
			{
				vShooterScore.ScoreDATA scoreDATA = this.scoreDATAList.datas[i];
				this.dataDisplays[i].Show(i + 1, new float?(scoreDATA.score), scoreDATA.hits);
			}
			else
			{
				this.dataDisplays[i].Show(i + 1, null, null);
			}
		}
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00046BB8 File Offset: 0x00044DB8
	public void SaveData(string dataName)
	{
		if (this.scoreDATAList == null)
		{
			this.scoreDATAList = new vShooterScore.ScoreDATAList();
		}
		string text = JsonUtility.ToJson(this.scoreDATAList);
		File.WriteAllText(Application.dataPath + "/" + dataName + ".json", text);
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00046C00 File Offset: 0x00044E00
	public vShooterScore.ScoreDATAList LoadData(string dataName)
	{
		string text = Application.dataPath + "/" + dataName + ".json";
		if (!File.Exists(text))
		{
			this.SaveData("ShooterScore");
		}
		else
		{
			string text2 = File.ReadAllText(text);
			this.scoreDATAList = JsonUtility.FromJson<vShooterScore.ScoreDATAList>(text2);
		}
		return this.scoreDATAList;
	}

	// Token: 0x04000976 RID: 2422
	[vButton("ShowData", "ShowData", typeof(vShooterScore), false)]
	[vButton("ClearData", "ClearData", typeof(vShooterScore), false)]
	public vShooterScore.TargetPointCounter scoreDisplay;

	// Token: 0x04000977 RID: 2423
	public vShooterScore.TargetPointCounter[] hitCounters;

	// Token: 0x04000978 RID: 2424
	public vScoreDataDisplay[] dataDisplays;

	// Token: 0x04000979 RID: 2425
	private vShooterScore.ScoreDATAList scoreDATAList;

	// Token: 0x0200013C RID: 316
	[Serializable]
	public class TargetPointCounter
	{
		// Token: 0x06000A47 RID: 2631 RVA: 0x00046C51 File Offset: 0x00044E51
		public void ShowValue()
		{
			this.display.ShowValue(this.currentScore);
		}

		// Token: 0x0400097A RID: 2426
		[vReadOnly(false)]
		public float currentScore;

		// Token: 0x0400097B RID: 2427
		public vScorePointDisplay display;
	}

	// Token: 0x0200013D RID: 317
	public class ScorePoint
	{
		// Token: 0x06000A49 RID: 2633 RVA: 0x00046C64 File Offset: 0x00044E64
		public ScorePoint(float value)
		{
			this.value = value;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00046C73 File Offset: 0x00044E73
		public ScorePoint(int id, float value)
		{
			this.id = id;
			this.value = value;
		}

		// Token: 0x0400097C RID: 2428
		public int id;

		// Token: 0x0400097D RID: 2429
		public float value;
	}

	// Token: 0x0200013E RID: 318
	[Serializable]
	public class ScoreDATAList
	{
		// Token: 0x0400097E RID: 2430
		public List<vShooterScore.ScoreDATA> datas = new List<vShooterScore.ScoreDATA>();
	}

	// Token: 0x0200013F RID: 319
	[Serializable]
	public class ScoreDATA
	{
		// Token: 0x0400097F RID: 2431
		public float score;

		// Token: 0x04000980 RID: 2432
		public List<float> hits = new List<float>();
	}
}
