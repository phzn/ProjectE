using System;
using System.Globalization;
using Features.Localization;
using Features.MatchRecap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x020003EA RID: 1002
	public class HealthManager : MonoBehaviour
	{
		// Token: 0x06001989 RID: 6537 RVA: 0x000A1530 File Offset: 0x0009F730
		private void Start()
		{
			if (this.heartRateText == null)
			{
				return;
			}
			this._font = this.heartRateText.font;
			Transform transform = this.heartRateText.transform;
			while (transform != null && transform.name != "mHealth")
			{
				transform = transform.parent;
			}
			this._healthPage = transform;
			Transform transform2 = ((transform != null) ? transform.Find("Health Category") : null);
			if (transform2 == null)
			{
				return;
			}
			this._category = transform2;
			Transform transform3 = transform2.Find("ImagesPanel");
			if (transform3 != null)
			{
				transform3.gameObject.SetActive(false);
			}
			Transform transform4 = transform2.Find("Health Category");
			this._title = ((transform4 != null) ? transform4.GetComponent<TMP_Text>() : null);
			if (this._title != null)
			{
				this._title.text = HealthManager.T("CIEL  /  HEALTH", "CIEL  /  SALUTE");
			}
			this.Build(transform2);
			PlayerStatsService.Changed += this.ReadSaved;
			GameLocalization.LocaleChanged += this.RefreshLocale;
			this.ReadSaved();
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x000A1652 File Offset: 0x0009F852
		private void OnDestroy()
		{
			PlayerStatsService.Changed -= this.ReadSaved;
			GameLocalization.LocaleChanged -= this.RefreshLocale;
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x000A1676 File Offset: 0x0009F876
		private void ReadSaved()
		{
			this._saved = PlayerStatsService.GetSnapshot();
			this._lastSteps = -1;
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x000A168A File Offset: 0x0009F88A
		private void RefreshLocale()
		{
			if (this._title != null)
			{
				this._title.text = HealthManager.T("CIEL  /  HEALTH", "CIEL  /  SALUTE");
			}
			this._lastSteps = -1;
			this._nextRefresh = 0f;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x000A16C6 File Offset: 0x0009F8C6
		private static string T(string en, string it)
		{
			if (!(GameLocalization.CurrentCode == "it"))
			{
				return en;
			}
			return it;
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x000A16DC File Offset: 0x0009F8DC
		private void Update()
		{
			if (!(this._healthPage != null) || !this._healthPage.gameObject.activeInHierarchy || !(this._category != null) || !this._category.gameObject.activeSelf)
			{
				this._wasVisible = false;
				return;
			}
			if (!this._wasVisible)
			{
				this._wasVisible = true;
				this._nextRefresh = 0f;
				this._lastSteps = -1;
			}
			if (Time.unscaledTime < this._nextRefresh)
			{
				return;
			}
			this._nextRefresh = Time.unscaledTime + 0.5f;
			this.Refresh();
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x000A177C File Offset: 0x0009F97C
		private void Refresh()
		{
			if (this._saved == null || this._steps == null)
			{
				return;
			}
			DateTime today = DateTime.Today;
			MatchStatsService instance = MatchStatsService.Instance;
			MatchStatsSnapshot matchStatsSnapshot = ((instance != null && instance.IsRunTrackingActive && !instance.IsMocked) ? instance.GetSnapshot() : default(MatchStatsSnapshot));
			int num = 0;
			string text = today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			for (int i = 0; i < this._saved.healthDays.Count; i++)
			{
				if (this._saved.healthDays[i].day == text)
				{
					num = this._saved.healthDays[i].steps;
					break;
				}
			}
			int num2 = (int)Math.Min(2147483647L, (long)num + (long)matchStatsSnapshot.StepsOnDay(today));
			if (num2 != this._lastSteps || today != this._shownDay)
			{
				this._lastSteps = num2;
				this._shownDay = today;
				this._steps.text = num2.ToString("N0");
				this._goal.text = string.Format("{0:N0} / {1:N0} {2}", num2, this._saved.dailyStepGoal, HealthManager.T("STEPS TODAY", "PASSI OGGI"));
				((RectTransform)this._progress.transform).sizeDelta = new Vector2(296f * Mathf.Clamp01((float)num2 / (float)this._saved.dailyStepGoal), 5f);
				this.UpdateChart(today, matchStatsSnapshot);
			}
			this._pulse.text = ((instance != null && instance.LiveBpm > 0) ? string.Format("{0}  BPM", instance.LiveBpm) : "—  BPM");
			this._hint.text = ((instance != null && instance.LiveBpm > 0) ? HealthManager.T("LIVE READING", "IN TEMPO REALE") : HealthManager.T("NO RECENT DATA", "NESSUN DATO RECENTE"));
			this._distance.text = string.Format("{0} {1:0.00} km     {2} {3:0.00} km", new object[]
			{
				HealthManager.T("WALK", "PASSO"),
				(this._saved.totalWalkingMeters + matchStatsSnapshot.WalkingMeters) / 1000f,
				HealthManager.T("RUN", "CORSA"),
				(this._saved.totalRunningMeters + matchStatsSnapshot.RunningMeters) / 1000f
			});
			this._activity.text = string.Concat(new string[]
			{
				HealthManager.T("WALK", "PASSO"),
				"  ",
				HealthManager.Duration(this._saved.totalWalkingSeconds + matchStatsSnapshot.WalkingSeconds),
				"  /  ",
				HealthManager.T("RUN", "CORSA"),
				"  ",
				HealthManager.Duration(this._saved.totalRunningSeconds + matchStatsSnapshot.RunningSeconds),
				"\n",
				HealthManager.T("TRAVEL", "VIAGGIO"),
				"  ",
				HealthManager.Duration(this._saved.totalVehicleSeconds + matchStatsSnapshot.VehicleSeconds)
			});
			this._lifetime.text = string.Format("{0}  {1:N0} {2}", HealthManager.T("TOTAL", "TOTALE"), (long)this._saved.totalSteps + (long)matchStatsSnapshot.Steps, HealthManager.T("STEPS", "PASSI"));
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x000A1B24 File Offset: 0x0009FD24
		private static string Duration(float seconds)
		{
			int num = Mathf.Max(0, Mathf.FloorToInt(seconds));
			if (num >= 3600)
			{
				return string.Format("{0:0}h {1:00}m {2:00}s", num / 3600, num / 60 % 60, num % 60);
			}
			return string.Format("{0:0}m {1:00}s", num / 60, num % 60);
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x000A1B90 File Offset: 0x0009FD90
		private void UpdateChart(DateTime today, MatchStatsSnapshot live)
		{
			int num = Mathf.Max(1, this._saved.dailyStepGoal);
			for (int i = 0; i < 7; i++)
			{
				string text = today.AddDays((double)(i - 6)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
				this._chartValues[i] = 0;
				for (int j = 0; j < this._saved.healthDays.Count; j++)
				{
					if (this._saved.healthDays[j].day == text)
					{
						this._chartValues[i] = this._saved.healthDays[j].steps;
						break;
					}
				}
				this._chartValues[i] = (int)Math.Min(2147483647L, (long)this._chartValues[i] + (long)live.StepsOnDay(today.AddDays((double)(i - 6))));
				num = Mathf.Max(num, this._chartValues[i]);
			}
			for (int k = 0; k < 7; k++)
			{
				((RectTransform)this._bars[k].transform).sizeDelta = new Vector2(22f, Mathf.Max(2f, 75f * (float)this._chartValues[k] / (float)num));
				this._bars[k].color = ((k == 6) ? HealthManager.Pink : new Color(0.67f, 0.38f, 0.56f));
				DateTime dateTime = today.AddDays((double)(k - 6));
				this._days[k].text = HealthManager.T(dateTime.ToString("ddd", CultureInfo.InvariantCulture).ToUpperInvariant(), HealthManager.ItalianDays[(int)dateTime.DayOfWeek]);
			}
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x000A1D50 File Offset: 0x0009FF50
		private void Build(Transform category)
		{
			RectTransform rectTransform = HealthManager.Panel(category, "CielHealthDashboard", 0f, 0f, 346f, 588f, new Color(0.055f, 0.052f, 0.075f));
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(0f, -0.48f);
			rectTransform.localScale = Vector3.one * 0.00713892f;
			RectTransform rectTransform4 = HealthManager.Panel(rectTransform, "Today", 8f, 8f, 330f, 132f, HealthManager.Card);
			this.StaticLabel(rectTransform4, "TODAY  /  ACTIVITY", "OGGI  /  ATTIVITÀ", 14f, 10f, 296f, 22f, 14f, HealthManager.Muted);
			this._steps = this.Label(rectTransform4, "0", 14f, 34f, 224f, 52f, 42f, HealthManager.Pale);
			this.StaticLabel(rectTransform4, "STEPS", "PASSI", 229f, 59f, 78f, 24f, 15f, HealthManager.Pink);
			this._goal = this.Label(rectTransform4, "0 / 3,000 STEPS TODAY", 14f, 90f, 240f, 19f, 12f, HealthManager.Muted);
			RectTransform rectTransform5 = HealthManager.Panel(rectTransform4, "GoalTrack", 14f, 116f, 296f, 5f, new Color(0.29f, 0.22f, 0.29f));
			this._progress = HealthManager.Panel(rectTransform5, "GoalFill", 0f, 0f, 296f, 5f, HealthManager.Pink).GetComponent<Image>();
			this._progress.type = Image.Type.Simple;
			((RectTransform)this._progress.transform).sizeDelta = new Vector2(0f, 5f);
			this.GoalButton(rectTransform4, 264f, "−", -1);
			this.GoalButton(rectTransform4, 291f, "+", 1);
			RectTransform rectTransform6 = HealthManager.Panel(rectTransform, "SevenDayHistory", 8f, 148f, 330f, 142f, HealthManager.Card);
			this.StaticLabel(rectTransform6, "LAST 7 DAYS", "ULTIMI 7 GIORNI", 14f, 10f, 190f, 21f, 15f, HealthManager.Pale);
			this.StaticLabel(rectTransform6, "STEPS", "PASSI", 253f, 11f, 60f, 18f, 12f, HealthManager.Muted);
			for (int i = 0; i < 7; i++)
			{
				RectTransform rectTransform7 = HealthManager.Panel(rectTransform6, "DayBar" + i.ToString(), (float)(30 + i * 43), 115f, 22f, 2f, HealthManager.Pink);
				rectTransform7.pivot = new Vector2(0f, 0f);
				this._bars[i] = rectTransform7.GetComponent<Image>();
				this._days[i] = this.Label(rectTransform6, "---", (float)(13 + i * 43), 117f, 55f, 20f, 11f, HealthManager.Muted);
				this._days[i].alignment = TextAlignmentOptions.Center;
			}
			RectTransform rectTransform8 = HealthManager.Panel(rectTransform, "Pulse", 8f, 298f, 330f, 78f, HealthManager.Card);
			this.StaticLabel(rectTransform8, "♥  HEART RATE", "♥  FREQUENZA CARDIACA", 14f, 9f, 296f, 21f, 14f, HealthManager.Pink);
			this._pulse = this.Label(rectTransform8, "—  BPM", 14f, 29f, 170f, 38f, 29f, HealthManager.Pale);
			this._hint = this.Label(rectTransform8, HealthManager.T("NO RECENT DATA", "NESSUN DATO RECENTE"), 165f, 37f, 151f, 26f, 11f, HealthManager.Muted);
			RectTransform rectTransform9 = HealthManager.Panel(rectTransform, "Movement", 8f, 384f, 330f, 126f, HealthManager.Card);
			this.StaticLabel(rectTransform9, "ACTIVITY TOTALS", "ATTIVITÀ TOTALE", 14f, 9f, 260f, 22f, 14f, HealthManager.Pale);
			this._distance = this.Label(rectTransform9, "", 14f, 32f, 307f, 21f, 12f, HealthManager.Pink);
			this._activity = this.Label(rectTransform9, "", 14f, 58f, 307f, 55f, 12f, HealthManager.Pale);
			this._lifetime = this.Label(rectTransform, "", 13f, 520f, 325f, 22f, 11f, HealthManager.Muted);
			this.StaticLabel(rectTransform, "Activity recorded automatically.", "Attività registrata automaticamente.", 13f, 547f, 325f, 23f, 10f, HealthManager.Muted);
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x000A22AC File Offset: 0x000A04AC
		private void GoalButton(RectTransform parent, float x, string title, int direction)
		{
			RectTransform rectTransform = HealthManager.Panel(parent, "Goal" + title, x, 86f, 25f, 25f, new Color(0.28f, 0.18f, 0.27f));
			rectTransform.gameObject.AddComponent<Button>().onClick.AddListener(delegate
			{
				int num = 1;
				for (int i = 0; i < HealthManager.Goals.Length; i++)
				{
					if (this._saved.dailyStepGoal == HealthManager.Goals[i])
					{
						num = i;
						break;
					}
				}
				PlayerStatsService.SetDailyStepGoal(HealthManager.Goals[Mathf.Clamp(num + direction, 0, HealthManager.Goals.Length - 1)]);
				this._lastSteps = -1;
				this.Refresh();
			});
			this.Label(rectTransform, title, 0f, 0f, 25f, 25f, 19f, HealthManager.Pale).alignment = TextAlignmentOptions.Center;
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x000A2358 File Offset: 0x000A0558
		private static RectTransform Panel(Transform parent, string name, float x, float y, float width, float height, Color color)
		{
			RectTransform component = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			}).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			Vector2 vector = new Vector2(0f, 1f);
			component.anchorMax = vector;
			component.anchorMin = vector;
			component.pivot = new Vector2(0f, 1f);
			component.anchoredPosition = new Vector2(x, -y);
			component.sizeDelta = new Vector2(width, height);
			component.GetComponent<Image>().color = color;
			return component;
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000A23F8 File Offset: 0x000A05F8
		private TMP_Text Label(Transform parent, string value, float x, float y, float width, float height, float size, Color color)
		{
			RectTransform component = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			}).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			Vector2 vector = new Vector2(0f, 1f);
			component.anchorMax = vector;
			component.anchorMin = vector;
			component.pivot = new Vector2(0f, 1f);
			component.anchoredPosition = new Vector2(x, -y);
			component.sizeDelta = new Vector2(width, height);
			TMP_Text component2 = component.GetComponent<TMP_Text>();
			component2.font = this._font;
			component2.fontSize = size;
			component2.color = color;
			component2.text = value;
			component2.enableWordWrapping = false;
			component2.overflowMode = TextOverflowModes.Overflow;
			component2.raycastTarget = false;
			return component2;
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x000A24CC File Offset: 0x000A06CC
		private TMP_Text StaticLabel(Transform parent, string en, string it, float x, float y, float width, float height, float size, Color color)
		{
			TMP_Text tmp_Text = this.Label(parent, HealthManager.T(en, it), x, y, width, height, size, color);
			LocalizedGameText.BindComputed(tmp_Text, () => HealthManager.T(en, it));
			return tmp_Text;
		}

		// Token: 0x04001AA5 RID: 6821
		public TMP_Text beforeGlucoseText;

		// Token: 0x04001AA6 RID: 6822
		public TMP_Text afterGlucoseText;

		// Token: 0x04001AA7 RID: 6823
		public TMP_Text oxygenText;

		// Token: 0x04001AA8 RID: 6824
		public TMP_Text systolicText;

		// Token: 0x04001AA9 RID: 6825
		public TMP_Text diastolicText;

		// Token: 0x04001AAA RID: 6826
		public TMP_Text heartRateText;

		// Token: 0x04001AAB RID: 6827
		public TMP_Text activeMoveText;

		// Token: 0x04001AAC RID: 6828
		public TMP_Text activeEnergyText;

		// Token: 0x04001AAD RID: 6829
		public TMP_Text restingEnergyText;

		// Token: 0x04001AAE RID: 6830
		public TMP_Text stepsText;

		// Token: 0x04001AAF RID: 6831
		public TMP_Text walkingDistanceText;

		// Token: 0x04001AB0 RID: 6832
		public TMP_Text floorsClimbedText;

		// Token: 0x04001AB1 RID: 6833
		public TMP_Text runningDistanceText;

		// Token: 0x04001AB2 RID: 6834
		private static readonly Color Card = new Color(0.115f, 0.1f, 0.145f);

		// Token: 0x04001AB3 RID: 6835
		private static readonly Color Pink = new Color(1f, 0.36f, 0.65f);

		// Token: 0x04001AB4 RID: 6836
		private static readonly Color Pale = new Color(1f, 0.82f, 0.9f);

		// Token: 0x04001AB5 RID: 6837
		private static readonly Color Muted = new Color(0.69f, 0.65f, 0.72f);

		// Token: 0x04001AB6 RID: 6838
		private static readonly int[] Goals = new int[] { 1000, 3000, 5000, 10000, 20000 };

		// Token: 0x04001AB7 RID: 6839
		private static readonly string[] ItalianDays = new string[] { "DOM", "LUN", "MAR", "MER", "GIO", "VEN", "SAB" };

		// Token: 0x04001AB8 RID: 6840
		private Transform _healthPage;

		// Token: 0x04001AB9 RID: 6841
		private Transform _category;

		// Token: 0x04001ABA RID: 6842
		private TMP_FontAsset _font;

		// Token: 0x04001ABB RID: 6843
		private TMP_Text _title;

		// Token: 0x04001ABC RID: 6844
		private TMP_Text _steps;

		// Token: 0x04001ABD RID: 6845
		private TMP_Text _goal;

		// Token: 0x04001ABE RID: 6846
		private TMP_Text _pulse;

		// Token: 0x04001ABF RID: 6847
		private TMP_Text _distance;

		// Token: 0x04001AC0 RID: 6848
		private TMP_Text _activity;

		// Token: 0x04001AC1 RID: 6849
		private TMP_Text _lifetime;

		// Token: 0x04001AC2 RID: 6850
		private TMP_Text _hint;

		// Token: 0x04001AC3 RID: 6851
		private Image _progress;

		// Token: 0x04001AC4 RID: 6852
		private readonly Image[] _bars = new Image[7];

		// Token: 0x04001AC5 RID: 6853
		private readonly TMP_Text[] _days = new TMP_Text[7];

		// Token: 0x04001AC6 RID: 6854
		private readonly int[] _chartValues = new int[7];

		// Token: 0x04001AC7 RID: 6855
		private PlayerStatsData _saved;

		// Token: 0x04001AC8 RID: 6856
		private float _nextRefresh;

		// Token: 0x04001AC9 RID: 6857
		private int _lastSteps = -1;

		// Token: 0x04001ACA RID: 6858
		private DateTime _shownDay;

		// Token: 0x04001ACB RID: 6859
		private bool _wasVisible;
	}
}
