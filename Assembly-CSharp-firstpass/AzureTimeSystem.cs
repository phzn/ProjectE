using System;
using System.Collections.Generic;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000020 RID: 32
	[Serializable]
	public sealed class AzureTimeSystem
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00004DE3 File Offset: 0x00002FE3
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00004DEB File Offset: 0x00002FEB
		public Transform sunTransform
		{
			get
			{
				return this.m_sunTransform;
			}
			set
			{
				this.m_sunTransform = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00004DF4 File Offset: 0x00002FF4
		public float sunElevation
		{
			get
			{
				return this.m_sunElevation;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00004DFC File Offset: 0x00002FFC
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00004E04 File Offset: 0x00003004
		public Transform moonTransform
		{
			get
			{
				return this.m_moonTransform;
			}
			set
			{
				this.m_moonTransform = value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00004E0D File Offset: 0x0000300D
		public float moonElevation
		{
			get
			{
				return this.m_moonElevation;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00004E15 File Offset: 0x00003015
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00004E1D File Offset: 0x0000301D
		public Transform starfieldTransform
		{
			get
			{
				return this.m_starfieldTransform;
			}
			set
			{
				this.m_starfieldTransform = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00004E26 File Offset: 0x00003026
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00004E2E File Offset: 0x0000302E
		public Transform directionalLight
		{
			get
			{
				return this.m_directionalLight;
			}
			set
			{
				this.m_directionalLight = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00004E37 File Offset: 0x00003037
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00004E3F File Offset: 0x0000303F
		public AzureTimeMode timeMode
		{
			get
			{
				return this.m_timeMode;
			}
			set
			{
				this.m_timeMode = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00004E48 File Offset: 0x00003048
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00004E50 File Offset: 0x00003050
		public AzureTimeDirection timeDirection
		{
			get
			{
				return this.m_timeDirection;
			}
			set
			{
				this.m_timeDirection = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00004E59 File Offset: 0x00003059
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00004E61 File Offset: 0x00003061
		public AzureTimeLoop timeLoop
		{
			get
			{
				return this.m_timeLoop;
			}
			set
			{
				this.m_timeLoop = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00004E6A File Offset: 0x0000306A
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00004E72 File Offset: 0x00003072
		public float timeline
		{
			get
			{
				return this.m_timeline;
			}
			set
			{
				this.m_timeline = value;
				this.OnTimelineChanged();
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00004E81 File Offset: 0x00003081
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00004E89 File Offset: 0x00003089
		public float latitude
		{
			get
			{
				return this.m_latitude;
			}
			set
			{
				this.m_latitude = value;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00004E92 File Offset: 0x00003092
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00004E9A File Offset: 0x0000309A
		public float longitude
		{
			get
			{
				return this.m_longitude;
			}
			set
			{
				this.m_longitude = value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00004EA3 File Offset: 0x000030A3
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00004EAB File Offset: 0x000030AB
		public float utc
		{
			get
			{
				return this.m_utc;
			}
			set
			{
				this.m_utc = value;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00004EB4 File Offset: 0x000030B4
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00004EBC File Offset: 0x000030BC
		public int day
		{
			get
			{
				return this.m_day;
			}
			set
			{
				this.m_day = value;
				this.UpdateCalendar();
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00004ECB File Offset: 0x000030CB
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00004ED3 File Offset: 0x000030D3
		public int month
		{
			get
			{
				return this.m_month;
			}
			set
			{
				this.m_month = value;
				this.UpdateCalendar();
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00004EE2 File Offset: 0x000030E2
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00004EEA File Offset: 0x000030EA
		public int year
		{
			get
			{
				return this.m_year;
			}
			set
			{
				this.m_year = value;
				this.UpdateCalendar();
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00004EF9 File Offset: 0x000030F9
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00004F01 File Offset: 0x00003101
		public float startTime
		{
			get
			{
				return this.m_startTime;
			}
			set
			{
				this.m_startTime = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00004F0A File Offset: 0x0000310A
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00004F12 File Offset: 0x00003112
		public float dayLength
		{
			get
			{
				return this.m_dayLength;
			}
			set
			{
				this.m_dayLength = value;
				this.ComputeTimeProgressionStep();
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00004F21 File Offset: 0x00003121
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00004F29 File Offset: 0x00003129
		public float minLightAltitude
		{
			get
			{
				return this.m_minLightAltitude;
			}
			set
			{
				this.m_minLightAltitude = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00004F32 File Offset: 0x00003132
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00004F3A File Offset: 0x0000313A
		public float dawnTime
		{
			get
			{
				return this.m_dawnTime;
			}
			set
			{
				this.m_dawnTime = value;
				this.UpdateTimeLengthCurve();
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00004F49 File Offset: 0x00003149
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00004F51 File Offset: 0x00003151
		public float duskTime
		{
			get
			{
				return this.m_duskTime;
			}
			set
			{
				this.m_duskTime = value;
				this.UpdateTimeLengthCurve();
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00004F60 File Offset: 0x00003160
		public float timeOfDay
		{
			get
			{
				return this.m_timeOfDay;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00004F68 File Offset: 0x00003168
		public int hour
		{
			get
			{
				return this.m_hour;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00004F70 File Offset: 0x00003170
		public int minute
		{
			get
			{
				return this.m_minute;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00004F78 File Offset: 0x00003178
		public float evaluationTime
		{
			get
			{
				return this.m_evaluationTime;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00004F80 File Offset: 0x00003180
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00004F88 File Offset: 0x00003188
		public AnimationCurve timeLengthCurve
		{
			get
			{
				return this.m_timeLengthCurve;
			}
			set
			{
				this.m_timeLengthCurve = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00004F91 File Offset: 0x00003191
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00004F99 File Offset: 0x00003199
		public List<AzureCelestialBody> celestialBodiesList
		{
			get
			{
				return this.m_celestialBodiesList;
			}
			set
			{
				this.m_celestialBodiesList = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004FA2 File Offset: 0x000031A2
		public string[] weekNameList
		{
			get
			{
				return this.m_weekNameList;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00004FAA File Offset: 0x000031AA
		public string[] monthNameList
		{
			get
			{
				return this.m_monthNameList;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00004FB2 File Offset: 0x000031B2
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00004FBA File Offset: 0x000031BA
		public string[] DayNumberList
		{
			get
			{
				return this.m_dayNumberList;
			}
			set
			{
				this.m_dayNumberList = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00004FC3 File Offset: 0x000031C3
		public int selectedCalendarDay
		{
			get
			{
				return this.m_selectedCalendarDay;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00004FCB File Offset: 0x000031CB
		public int dayOfWeek
		{
			get
			{
				return this.m_dayOfWeek;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00004FD3 File Offset: 0x000031D3
		public int daysInMonth
		{
			get
			{
				return this.m_daysInMonth;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00004FDB File Offset: 0x000031DB
		public bool isPlayingTimelineTransition
		{
			get
			{
				return this.m_isPlayingTimelineTransition;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00004FE4 File Offset: 0x000031E4
		public void Start()
		{
			if (Application.isPlaying)
			{
				this.m_timeline = this.m_startTime;
			}
			this.m_timeOfDay = this.m_timeLengthCurve.Evaluate(this.m_timeline);
			this.m_hour = (int)Mathf.Floor(this.m_timeOfDay);
			this.m_minute = (int)Mathf.Floor(this.m_timeOfDay * 60f % 60f);
			this.m_previousMinute = this.m_minute;
			this.m_previousHour = this.m_hour;
			this.ComputeEvaluationTime();
			this.UpdateTimeLengthCurve();
			this.ComputeTimeProgressionStep();
			this.UpdateCalendar();
			this.UpdateCelestialBodies();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00005084 File Offset: 0x00003284
		public void UpdateTimeOfDay()
		{
			if (!this.m_isPlayingTimelineTransition)
			{
				if (this.m_dayLength > 0f)
				{
					if (this.m_timeDirection == AzureTimeDirection.Forward)
					{
						this.m_timeline += this.m_timeProgressionStep * Time.deltaTime;
						if (this.m_timeline > 24f)
						{
							this.IncreaseDay();
							this.m_timeline = 0f;
						}
					}
					else
					{
						this.m_timeline -= this.m_timeProgressionStep * Time.deltaTime;
						if (this.m_timeline < 0f)
						{
							this.DecreaseDay();
							this.m_timeline = 24f;
						}
					}
					this.OnTimelineChanged();
					return;
				}
			}
			else
			{
				this.ApplyTimelineTransition();
				this.OnTimelineChanged();
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00005137 File Offset: 0x00003337
		private void ComputeTimeProgressionStep()
		{
			if (this.m_dayLength > 0f)
			{
				this.m_timeProgressionStep = 0.4f / this.m_dayLength;
				return;
			}
			this.m_timeProgressionStep = 0f;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00005164 File Offset: 0x00003364
		public void UpdateCelestialBodies()
		{
			if (this.m_timeMode == AzureTimeMode.Simple)
			{
				this.m_sunTransform.rotation = Quaternion.Euler(0f, this.m_longitude, -this.m_latitude) * Quaternion.Euler((this.m_timeOfDay - this.m_utc) * 360f / 24f - 90f, 180f, 0f);
				this.m_moonTransform.rotation = this.m_sunTransform.rotation * Quaternion.Euler(0f, -180f, 0f);
				this.m_starfieldTransform.rotation = this.m_sunTransform.rotation;
			}
			else
			{
				float num = this.m_timeOfDay - this.m_utc;
				float num2 = 0.017453292f;
				float num3 = 57.29578f;
				float num4 = this.m_latitude * num2;
				float num5 = (float)(367 * this.m_year - 7 * (this.m_year + (this.m_month + 9) / 12) / 4 + 275 * this.m_month / 9 + this.m_day - 730530);
				num5 += num / 24f;
				float num6 = 23.4393f - 3.563E-07f * num5;
				num6 *= num2;
				float num7 = 282.9404f + 4.70935E-05f * num5;
				float num8 = 0.016709f - 1.151E-09f * num5;
				float num9 = 356.047f + 0.98560023f * num5;
				num9 *= num2;
				float num10 = num9 + num8 * Mathf.Sin(num9) * (1f + num8 * Mathf.Cos(num9));
				float num11 = Mathf.Cos(num10) - num8;
				float num12 = Mathf.Sqrt(1f - num8 * num8) * Mathf.Sin(num10);
				float num13 = Mathf.Atan2(num12, num11) * num3;
				float num14 = Mathf.Sqrt(num11 * num11 + num12 * num12);
				float num15 = (num13 + num7) * num2;
				float num16 = num14 * Mathf.Cos(num15);
				float num17 = num14 * Mathf.Sin(num15);
				float num18 = num16;
				float num19 = num17 * Mathf.Cos(num6);
				float num20 = num17 * Mathf.Sin(num6);
				float num21 = Mathf.Atan2(num19, num18);
				float num22 = Mathf.Atan2(num20, Mathf.Sqrt(num18 * num18 + num19 * num19));
				float num23 = (num13 + num7 + 180f + num * 15f + this.m_longitude) * num2;
				float num24 = num23 - num21;
				float num25 = Mathf.Cos(num24) * Mathf.Cos(num22);
				float num26 = Mathf.Sin(num24) * Mathf.Cos(num22);
				float num27 = Mathf.Sin(num22);
				float num28 = num25 * Mathf.Sin(num4) - num27 * Mathf.Cos(num4);
				float num29 = num26;
				float num30 = num25 * Mathf.Cos(num4) + num27 * Mathf.Sin(num4);
				float num31 = Mathf.Atan2(num29, num28);
				float num32 = Mathf.Asin(num30);
				Vector3 vector;
				vector.x = num32 * num3;
				vector.y = num31 * num3;
				vector.z = 0f;
				this.m_starfieldTransform.rotation = Quaternion.Euler(90f - this.m_latitude, 0f, 0f) * Quaternion.Euler(0f, this.m_longitude, 0f) * Quaternion.Euler(0f, num23 * num3, 0f);
				this.m_sunTransform.position = Quaternion.Euler(vector) * new Vector3(0f, 0f, 1f);
				this.m_sunTransform.rotation = Quaternion.LookRotation(this.m_sunTransform.position, this.m_starfieldTransform.up);
				float num33 = 125.1228f - 0.05295381f * num5;
				float num34 = 5.1454f;
				num7 = 318.0634f + 0.16435732f * num5;
				float num35 = 60.2666f;
				num8 = 0.0549f;
				num9 = 115.3654f + 13.064993f * num5;
				num9 *= num2;
				num10 = num9 + num8 * Mathf.Sin(num9) * (1f + num8 * Mathf.Cos(num9));
				num11 = num35 * (Mathf.Cos(num10) - num8);
				num12 = num35 * (Mathf.Sqrt(1f - num8 * num8) * Mathf.Sin(num10));
				num13 = Mathf.Atan2(num12, num11) * num3;
				float num36 = Mathf.Sqrt(num11 * num11 + num12 * num12);
				num15 = (num13 + num7) * num2;
				float num37 = Mathf.Sin(num15);
				float num38 = Mathf.Cos(num15);
				num33 *= num2;
				num34 *= num2;
				float num39 = num36 * (Mathf.Cos(num33) * num38 - Mathf.Sin(num33) * num37 * Mathf.Cos(num34));
				float num40 = num36 * (Mathf.Sin(num33) * num38 + Mathf.Cos(num33) * num37 * Mathf.Cos(num34));
				float num41 = num36 * (num37 * Mathf.Sin(num34));
				num18 = num39;
				num19 = num40 * Mathf.Cos(num6) - num41 * Mathf.Sin(num6);
				float num42 = num40 * Mathf.Sin(num6) + num41 * Mathf.Cos(num6);
				num21 = Mathf.Atan2(num19, num18);
				num22 = Mathf.Atan2(num42, Mathf.Sqrt(num18 * num18 + num19 * num19));
				float num43 = num23 - num21;
				num25 = Mathf.Cos(num43) * Mathf.Cos(num22);
				num26 = Mathf.Sin(num43) * Mathf.Cos(num22);
				num27 = Mathf.Sin(num22);
				num28 = num25 * Mathf.Sin(num4) - num27 * Mathf.Cos(num4);
				num29 = num26;
				float num44 = num25 * Mathf.Cos(num4) + num27 * Mathf.Sin(num4);
				num31 = Mathf.Atan2(num29, num28);
				num32 = Mathf.Asin(num44);
				vector.x = num32 * num3;
				vector.y = num31 * num3;
				vector.z = 0f;
				this.m_moonTransform.position = Quaternion.Euler(vector) * new Vector3(0f, 0f, 1f);
				this.m_moonTransform.rotation = Quaternion.LookRotation(this.m_moonTransform.position, this.m_starfieldTransform.up);
				if (this.m_celestialBodiesList.Count > 0)
				{
					for (int i = 0; i < this.m_celestialBodiesList.Count; i++)
					{
						AzureCelestialBody azureCelestialBody = this.m_celestialBodiesList[i];
						if (azureCelestialBody.transform)
						{
							switch (azureCelestialBody.type)
							{
							case AzureCelestialBodyType.Mercury:
								num33 = 48.3313f + 3.24587E-05f * num5;
								num34 = 7.0047f + 5E-08f * num5;
								num7 = 29.1241f + 1.01444E-05f * num5;
								num35 = 0.387098f;
								num8 = 0.205635f + 5.59E-10f * num5;
								num9 = 168.6562f + 4.0923343f * num5;
								break;
							case AzureCelestialBodyType.Venus:
								num33 = 76.6799f + 2.4659E-05f * num5;
								num34 = 3.3946f + 2.75E-08f * num5;
								num7 = 54.891f + 1.38374E-05f * num5;
								num35 = 0.72333f;
								num8 = 0.006773f - 1.302E-09f * num5;
								num9 = 48.0052f + 1.6021302f * num5;
								break;
							case AzureCelestialBodyType.Mars:
								num33 = 49.5574f + 2.11081E-05f * num5;
								num34 = 1.8497f - 1.78E-08f * num5;
								num7 = 286.5016f + 2.92961E-05f * num5;
								num35 = 1.523688f;
								num8 = 0.093405f + 2.516E-09f * num5;
								num9 = 18.6021f + 0.5240208f * num5;
								break;
							case AzureCelestialBodyType.Jupiter:
								num33 = 100.4542f + 2.76854E-05f * num5;
								num34 = 1.303f - 1.557E-07f * num5;
								num7 = 273.8777f + 1.64505E-05f * num5;
								num35 = 5.20256f;
								num8 = 0.048498f + 4.469E-09f * num5;
								num9 = 19.895f + 0.0830853f * num5;
								break;
							case AzureCelestialBodyType.Saturn:
								num33 = 113.6634f + 2.3898E-05f * num5;
								num34 = 2.4886f - 1.081E-07f * num5;
								num7 = 339.3939f + 2.97661E-05f * num5;
								num35 = 9.55475f;
								num8 = 0.055546f - 9.499E-09f * num5;
								num9 = 316.967f + 0.03344423f * num5;
								break;
							case AzureCelestialBodyType.Uranus:
								num33 = 74.0005f + 1.3978E-05f * num5;
								num34 = 0.7733f + 1.9E-08f * num5;
								num7 = 96.6612f + 3.0565E-05f * num5;
								num35 = 19.18171f - 1.55E-08f * num5;
								num8 = 0.047318f + 7.45E-09f * num5;
								num9 = 142.5905f + 0.011725806f * num5;
								break;
							case AzureCelestialBodyType.Neptune:
								num33 = 131.7806f + 3.0173E-05f * num5;
								num34 = 1.77f - 2.55E-07f * num5;
								num7 = 272.8461f - 6.027E-06f * num5;
								num35 = 30.05826f + 3.313E-08f * num5;
								num8 = 0.008606f + 2.15E-09f * num5;
								num9 = 260.2471f + 0.005995147f * num5;
								break;
							case AzureCelestialBodyType.Pluto:
							{
								float num45 = 50.03f + 0.033459652f * num5;
								float num46 = 238.95f + 0.003968789f * num5;
								num45 *= num2;
								num46 *= num2;
								float num47 = 238.9508f + 0.00400703f * num5 - 19.799f * Mathf.Sin(num46) + 19.848f * Mathf.Cos(num46) + 0.897f * Mathf.Sin(2f * num46) - 4.956f * Mathf.Cos(2f * num46) + 0.61f * Mathf.Sin(3f * num46) + 1.211f * Mathf.Cos(3f * num46) - 0.341f * Mathf.Sin(4f * num46) - 0.19f * Mathf.Cos(4f * num46) + 0.128f * Mathf.Sin(5f * num46) - 0.034f * Mathf.Cos(5f * num46) - 0.038f * Mathf.Sin(6f * num46) + 0.031f * Mathf.Cos(6f * num46) + 0.02f * Mathf.Sin(num45 - num46) - 0.01f * Mathf.Cos(num45 - num46);
								float num48 = -3.9082f - 5.453f * Mathf.Sin(num46) - 14.975f * Mathf.Cos(num46) + 3.527f * Mathf.Sin(2f * num46) + 1.673f * Mathf.Cos(2f * num46) - 1.051f * Mathf.Sin(3f * num46) + 0.328f * Mathf.Cos(3f * num46) + 0.179f * Mathf.Sin(4f * num46) - 0.292f * Mathf.Cos(4f * num46) + 0.019f * Mathf.Sin(5f * num46) + 0.1f * Mathf.Cos(5f * num46) - 0.031f * Mathf.Sin(6f * num46) - 0.026f * Mathf.Cos(6f * num46) + 0.011f * Mathf.Cos(num45 - num46);
								float num49 = 40.72f + 6.68f * Mathf.Sin(num46) + 6.9f * Mathf.Cos(num46) - 1.18f * Mathf.Sin(2f * num46) - 0.03f * Mathf.Cos(2f * num46) + 0.15f * Mathf.Sin(3f * num46) - 0.14f * Mathf.Cos(3f * num46);
								num47 *= num2;
								num48 *= num2;
								float num50 = Mathf.Cos(num48);
								num39 = num49 * Mathf.Cos(num47) * num50;
								num40 = num49 * Mathf.Sin(num47) * num50;
								num41 = num49 * Mathf.Sin(num48);
								float num51 = num39 + num16;
								float num52 = num40 + num17;
								float num53 = num41;
								num18 = num51;
								num19 = num52 * Mathf.Cos(num6) - num53 * Mathf.Sin(num6);
								float num54 = num52 * Mathf.Sin(num6) + num53 * Mathf.Cos(num6);
								num21 = Mathf.Atan2(num19, num18);
								num22 = Mathf.Atan2(num54, Mathf.Sqrt(num18 * num18 + num19 * num19));
								float num55 = num23 - num21;
								num25 = Mathf.Cos(num55) * Mathf.Cos(num22);
								num26 = Mathf.Sin(num55) * Mathf.Cos(num22);
								num27 = Mathf.Sin(num22);
								num28 = num25 * Mathf.Sin(num4) - num27 * Mathf.Cos(num4);
								num29 = num26;
								float num56 = num25 * Mathf.Cos(num4) + num27 * Mathf.Sin(num4);
								num31 = Mathf.Atan2(num29, num28);
								num32 = Mathf.Asin(num56);
								vector.x = num32 * num3;
								vector.y = num31 * num3;
								vector.z = 0f;
								azureCelestialBody.transform.position = Quaternion.Euler(vector) * new Vector3(0f, 0f, 1f);
								azureCelestialBody.transform.rotation = Quaternion.LookRotation(azureCelestialBody.transform.position, this.m_starfieldTransform.up);
								goto IL_0F95;
							}
							}
							num9 *= num2;
							num10 = num9 + num8 * Mathf.Sin(num9) * (1f + num8 * Mathf.Cos(num9));
							num11 = num35 * (Mathf.Cos(num10) - num8);
							num12 = num35 * (Mathf.Sqrt(1f - num8 * num8) * Mathf.Sin(num10));
							num13 = Mathf.Atan2(num12, num11) * num3;
							float num57 = Mathf.Sqrt(num11 * num11 + num12 * num12);
							num15 = (num13 + num7) * num2;
							num38 = Mathf.Cos(num15);
							num37 = Mathf.Sin(num15);
							num33 *= num2;
							num34 *= num2;
							num39 = num57 * (Mathf.Cos(num33) * num38 - Mathf.Sin(num33) * num37 * Mathf.Cos(num34));
							num40 = num57 * (Mathf.Sin(num33) * num38 + Mathf.Cos(num33) * num37 * Mathf.Cos(num34));
							num41 = num57 * (num37 * Mathf.Sin(num34));
							float num58 = Mathf.Atan2(num40, num39);
							float num59 = Mathf.Atan2(num41, Mathf.Sqrt(num39 * num39 + num40 * num40));
							float num60 = Mathf.Cos(num59);
							num39 = num57 * Mathf.Cos(num58) * num60;
							num40 = num57 * Mathf.Sin(num58) * num60;
							num41 = num57 * Mathf.Sin(num59);
							float num61 = num39 + num16;
							float num62 = num40 + num17;
							float num63 = num41;
							num18 = num61;
							num19 = num62 * Mathf.Cos(num6) - num63 * Mathf.Sin(num6);
							float num64 = num62 * Mathf.Sin(num6) + num63 * Mathf.Cos(num6);
							num21 = Mathf.Atan2(num19, num18);
							num22 = Mathf.Atan2(num64, Mathf.Sqrt(num18 * num18 + num19 * num19));
							float num65 = num23 - num21;
							num25 = Mathf.Cos(num65) * Mathf.Cos(num22);
							num26 = Mathf.Sin(num65) * Mathf.Cos(num22);
							num27 = Mathf.Sin(num22);
							num28 = num25 * Mathf.Sin(num4) - num27 * Mathf.Cos(num4);
							num29 = num26;
							float num66 = num25 * Mathf.Cos(num4) + num27 * Mathf.Sin(num4);
							num31 = Mathf.Atan2(num29, num28);
							num32 = Mathf.Asin(num66);
							vector.x = num32 * num3;
							vector.y = num31 * num3;
							vector.z = 0f;
							azureCelestialBody.transform.position = Quaternion.Euler(vector) * new Vector3(0f, 0f, 1f);
							azureCelestialBody.transform.rotation = Quaternion.LookRotation(azureCelestialBody.transform.position, this.m_starfieldTransform.up);
						}
						IL_0F95:;
					}
				}
			}
			this.m_sunElevation = Vector3.Dot(-this.m_sunTransform.forward, Vector3.up);
			this.m_moonElevation = Vector3.Dot(-this.m_moonTransform.forward, Vector3.up);
			this.m_directionalLight.localRotation = Quaternion.LookRotation((this.m_sunElevation >= 0f) ? this.m_sunTransform.forward : this.m_moonTransform.forward);
			this.m_directionalLightDirection = this.m_directionalLight.localEulerAngles;
			if (this.m_directionalLightDirection.x <= this.m_minLightAltitude)
			{
				this.m_directionalLightDirection.x = this.m_minLightAltitude;
			}
			this.m_directionalLight.localEulerAngles = this.m_directionalLightDirection;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000061DC File Offset: 0x000043DC
		public void IncreaseDay()
		{
			if (this.m_timeLoop != AzureTimeLoop.Dayly)
			{
				this.m_day++;
				if (this.m_day > this.m_daysInMonth)
				{
					this.m_day = 1;
					this.IncreaseMonth();
					AzureNotificationCenter.Invoke.OnDayChangedCallback(this);
					return;
				}
				AzureNotificationCenter.Invoke.OnDayChangedCallback(this);
			}
			this.UpdateCalendar();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006230 File Offset: 0x00004430
		public void DecreaseDay()
		{
			if (this.m_timeLoop != AzureTimeLoop.Dayly)
			{
				this.m_day--;
				this.m_previousMonth = ((this.m_timeLoop == AzureTimeLoop.Monthly) ? this.m_month : (this.m_month - 1));
				if (this.m_previousMonth < 1)
				{
					this.m_previousMonth = 12;
				}
				this.m_daysInPreviousMonth = DateTime.DaysInMonth(this.m_year, this.m_previousMonth);
				if (this.m_day < 1)
				{
					this.m_day = this.m_daysInPreviousMonth;
					this.DecreaseMonth();
					AzureNotificationCenter.Invoke.OnDayChangedCallback(this);
					return;
				}
				AzureNotificationCenter.Invoke.OnDayChangedCallback(this);
			}
			this.UpdateCalendar();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000062CC File Offset: 0x000044CC
		public void IncreaseMonth()
		{
			if (this.m_timeLoop != AzureTimeLoop.Monthly)
			{
				this.m_month++;
				if (this.m_month > 12)
				{
					this.m_month = 1;
					this.IncreaseYear();
					AzureNotificationCenter.Invoke.OnMonthChangedCallback(this);
					return;
				}
				AzureNotificationCenter.Invoke.OnMonthChangedCallback(this);
			}
			this.UpdateCalendar();
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000631C File Offset: 0x0000451C
		public void DecreaseMonth()
		{
			if (this.m_timeLoop != AzureTimeLoop.Monthly)
			{
				this.m_month--;
				if (this.m_month < 1)
				{
					this.m_month = 12;
					this.DecreaseYear();
					AzureNotificationCenter.Invoke.OnMonthChangedCallback(this);
					return;
				}
				AzureNotificationCenter.Invoke.OnMonthChangedCallback(this);
			}
			this.UpdateCalendar();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000636A File Offset: 0x0000456A
		public void IncreaseYear()
		{
			if (this.m_timeLoop != AzureTimeLoop.Yearly)
			{
				this.m_year++;
				if (this.m_year > 9999)
				{
					this.m_year = 0;
				}
				AzureNotificationCenter.Invoke.OnYearChangedCallback(this);
			}
			this.UpdateCalendar();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000063A3 File Offset: 0x000045A3
		public void DecreaseYear()
		{
			if (this.m_timeLoop != AzureTimeLoop.Yearly)
			{
				this.m_year--;
				if (this.m_year < 0)
				{
					this.m_year = 9999;
				}
				AzureNotificationCenter.Invoke.OnYearChangedCallback(this);
			}
			this.UpdateCalendar();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000063DC File Offset: 0x000045DC
		public void SetDate(int day, int month, int year)
		{
			this.m_year = year;
			this.m_month = month;
			this.m_day = day;
			this.UpdateCalendar();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000063F9 File Offset: 0x000045F9
		public Vector2Int GetTimeOfDayVector()
		{
			return new Vector2Int(this.m_hour, this.m_minute);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000640C File Offset: 0x0000460C
		public string GetTimeOfDayString()
		{
			return this.hour.ToString("00") + ":" + this.minute.ToString("00");
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006449 File Offset: 0x00004649
		public Vector3Int GetDateVector()
		{
			return new Vector3Int(this.m_year, this.m_month, this.m_day);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006464 File Offset: 0x00004664
		public string GetDateString()
		{
			return string.Concat(new string[]
			{
				this.m_monthNameList[this.m_month - 1],
				" ",
				this.m_day.ToString("00"),
				", ",
				this.m_year.ToString("0000")
			});
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000064C4 File Offset: 0x000046C4
		public string GetDateString(string format)
		{
			this.m_dateTime = new DateTime(this.m_year, this.m_month, this.m_day);
			return this.m_dateTime.ToString(format);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000064EF File Offset: 0x000046EF
		public string GetDayOfWeekString()
		{
			this.m_dateTime = new DateTime(this.m_year, this.m_month, this.m_day);
			return this.m_weekNameList[(int)this.m_dateTime.DayOfWeek];
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006520 File Offset: 0x00004720
		public int GetDayOfWeekInteger()
		{
			DateTime dateTime = new DateTime(this.m_year, this.m_month, this.m_day);
			return (int)dateTime.DayOfWeek;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006550 File Offset: 0x00004750
		public void UpdateCalendar()
		{
			this.m_year = Mathf.Clamp(this.year, 0, 9999);
			this.m_month = Mathf.Clamp(this.month, 1, 12);
			this.m_daysInMonth = DateTime.DaysInMonth(this.m_year, this.m_month);
			this.m_day = Mathf.Clamp(this.day, 1, this.m_daysInMonth);
			this.m_dateTime = new DateTime(this.m_year, this.m_month, 1);
			this.m_dayOfWeek = (int)this.m_dateTime.DayOfWeek;
			this.m_selectedCalendarDay = this.m_day - 1 + this.m_dayOfWeek;
			for (int i = 0; i < this.m_dayNumberList.Length; i++)
			{
				if (i < this.m_dayOfWeek || i >= this.m_dayOfWeek + this.m_daysInMonth)
				{
					this.m_dayNumberList[i] = "";
				}
				else
				{
					this.m_dateTime = new DateTime(this.m_year, this.m_month, i - this.m_dayOfWeek + 1);
					this.m_dayNumberList[i] = this.m_dateTime.Day.ToString();
				}
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000666C File Offset: 0x0000486C
		public void StartTimelineTransition(float targetTime, float transitionTime)
		{
			Mathf.Clamp(targetTime, 0f, 24f);
			this.m_timelineTransitionStartTime = Time.time;
			this.m_timelineTransitionStartPoint = this.m_timeline;
			this.m_timelineTransitionDuration = transitionTime;
			if (this.m_timeDirection == AzureTimeDirection.Forward)
			{
				if (targetTime < this.m_timeline)
				{
					this.m_timelineTransitionEndPoint = this.m_timeline + (targetTime - this.m_timeline + 24f);
				}
				else
				{
					this.m_timelineTransitionEndPoint = targetTime;
				}
			}
			else if (targetTime > this.m_timeline)
			{
				this.m_timelineTransitionEndPoint = this.m_timeline - (24f - targetTime + this.m_timeline);
			}
			else
			{
				this.m_timelineTransitionEndPoint = targetTime;
			}
			this.m_isPlayingTimelineTransition = true;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006714 File Offset: 0x00004914
		public void UpdateTimeLengthCurve()
		{
			this.m_timeLengthCurve = AnimationCurve.Linear(0f, 0f, 24f, 24f);
			this.m_timeLengthCurve.AddKey(this.m_dawnTime, 6f);
			this.m_timeLengthCurve.AddKey(this.m_duskTime, 18f);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006770 File Offset: 0x00004970
		public void UpdateTimeLengthCurve(float dawn, float dusk)
		{
			this.m_timeLengthCurve = AnimationCurve.Linear(0f, 0f, 24f, 24f);
			this.m_timeLengthCurve.AddKey(dawn, 6f);
			this.m_timeLengthCurve.AddKey(dusk, 18f);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000067C0 File Offset: 0x000049C0
		private void ComputeEvaluationTime()
		{
			if (this.m_timeMode == AzureTimeMode.Simple)
			{
				this.m_evaluationTime = this.m_timeOfDay;
				return;
			}
			this.m_sunElevationTime = Mathf.InverseLerp(-1f, 1f, this.m_sunElevation);
			this.m_evaluationTime = Mathf.Lerp(0f, 12f, this.m_sunElevationTime);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006818 File Offset: 0x00004A18
		private void ApplyTimelineTransition()
		{
			this.m_timelineTransitionStep = (Time.time - this.m_timelineTransitionStartTime) / this.m_timelineTransitionDuration;
			this.m_temporaryTimeline = Mathf.SmoothStep(this.m_timelineTransitionStartPoint, this.m_timelineTransitionEndPoint, this.m_timelineTransitionStep);
			this.m_timeline = this.m_temporaryTimeline;
			if (this.m_timeDirection == AzureTimeDirection.Forward)
			{
				if (this.m_timeline > 24f)
				{
					this.IncreaseDay();
					this.m_timeline = 0f;
					this.m_timelineTransitionStartPoint -= 24f;
					this.m_timelineTransitionEndPoint -= 24f;
				}
				if (this.m_timeline >= this.m_timelineTransitionEndPoint)
				{
					this.m_isPlayingTimelineTransition = false;
					return;
				}
			}
			else
			{
				if (this.m_timeline < 0f)
				{
					this.DecreaseDay();
					this.m_timeline = 24f;
					this.m_timelineTransitionStartPoint += 24f;
					this.m_timelineTransitionEndPoint += 24f;
				}
				if (this.m_timeline <= this.m_timelineTransitionEndPoint)
				{
					this.m_isPlayingTimelineTransition = false;
				}
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006920 File Offset: 0x00004B20
		private void OnTimelineChanged()
		{
			this.m_timeOfDay = this.m_timeLengthCurve.Evaluate(this.m_timeline);
			this.m_hour = (int)Mathf.Floor(this.m_timeOfDay);
			this.m_minute = (int)Mathf.Floor(this.m_timeOfDay * 60f % 60f);
			this.ComputeEvaluationTime();
			if (this.m_previousMinute != this.m_minute)
			{
				this.m_previousMinute = this.m_minute;
				AzureNotificationCenter.Invoke.OnMinuteChangedCallback(this);
			}
			if (this.m_previousHour != this.m_hour)
			{
				this.m_previousHour = this.m_hour;
				AzureNotificationCenter.Invoke.OnHourChangedCallback(this);
			}
			AzureNotificationCenter.Invoke.OnTimelineChangedCallback(this);
		}

		// Token: 0x0400011D RID: 285
		[SerializeField]
		private Transform m_sunTransform;

		// Token: 0x0400011E RID: 286
		private float m_sunElevation;

		// Token: 0x0400011F RID: 287
		private float m_sunElevationTime;

		// Token: 0x04000120 RID: 288
		[SerializeField]
		private Transform m_moonTransform;

		// Token: 0x04000121 RID: 289
		private float m_moonElevation;

		// Token: 0x04000122 RID: 290
		[SerializeField]
		private Transform m_starfieldTransform;

		// Token: 0x04000123 RID: 291
		[SerializeField]
		private Transform m_directionalLight;

		// Token: 0x04000124 RID: 292
		private Vector3 m_directionalLightDirection = Vector3.zero;

		// Token: 0x04000125 RID: 293
		[SerializeField]
		private AzureTimeMode m_timeMode;

		// Token: 0x04000126 RID: 294
		[SerializeField]
		private AzureTimeDirection m_timeDirection;

		// Token: 0x04000127 RID: 295
		[SerializeField]
		private AzureTimeLoop m_timeLoop;

		// Token: 0x04000128 RID: 296
		[SerializeField]
		private float m_timeline = 6.5f;

		// Token: 0x04000129 RID: 297
		[SerializeField]
		private float m_latitude;

		// Token: 0x0400012A RID: 298
		[SerializeField]
		private float m_longitude;

		// Token: 0x0400012B RID: 299
		[SerializeField]
		private float m_utc;

		// Token: 0x0400012C RID: 300
		[SerializeField]
		private int m_day = 1;

		// Token: 0x0400012D RID: 301
		[SerializeField]
		private int m_month = 1;

		// Token: 0x0400012E RID: 302
		[SerializeField]
		private int m_year = 2024;

		// Token: 0x0400012F RID: 303
		[SerializeField]
		private float m_startTime = 6.5f;

		// Token: 0x04000130 RID: 304
		[SerializeField]
		private float m_dayLength = 24f;

		// Token: 0x04000131 RID: 305
		[SerializeField]
		private float m_minLightAltitude;

		// Token: 0x04000132 RID: 306
		[SerializeField]
		private float m_dawnTime = 6f;

		// Token: 0x04000133 RID: 307
		[SerializeField]
		private float m_duskTime = 18f;

		// Token: 0x04000134 RID: 308
		[SerializeField]
		private float m_timeOfDay = 6.5f;

		// Token: 0x04000135 RID: 309
		[SerializeField]
		private int m_hour = 6;

		// Token: 0x04000136 RID: 310
		[SerializeField]
		private int m_minute;

		// Token: 0x04000137 RID: 311
		[SerializeField]
		private float m_evaluationTime = 6.5f;

		// Token: 0x04000138 RID: 312
		[SerializeField]
		private AnimationCurve m_timeLengthCurve = AnimationCurve.Linear(0f, 0f, 24f, 24f);

		// Token: 0x04000139 RID: 313
		[SerializeField]
		private List<AzureCelestialBody> m_celestialBodiesList = new List<AzureCelestialBody>();

		// Token: 0x0400013A RID: 314
		private readonly string[] m_weekNameList = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

		// Token: 0x0400013B RID: 315
		private readonly string[] m_monthNameList = new string[]
		{
			"January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
			"November", "December"
		};

		// Token: 0x0400013C RID: 316
		[SerializeField]
		private string[] m_dayNumberList = new string[]
		{
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41"
		};

		// Token: 0x0400013D RID: 317
		[SerializeField]
		private int m_selectedCalendarDay = 1;

		// Token: 0x0400013E RID: 318
		private int m_dayOfWeek;

		// Token: 0x0400013F RID: 319
		private int m_daysInMonth = 30;

		// Token: 0x04000140 RID: 320
		private DateTime m_dateTime;

		// Token: 0x04000141 RID: 321
		private int m_daysInPreviousMonth = 30;

		// Token: 0x04000142 RID: 322
		private int m_previousMonth = 1;

		// Token: 0x04000143 RID: 323
		private float m_timeProgressionStep;

		// Token: 0x04000144 RID: 324
		private int m_previousMinute;

		// Token: 0x04000145 RID: 325
		private int m_previousHour = 6;

		// Token: 0x04000146 RID: 326
		private float m_timelineTransitionStartTime;

		// Token: 0x04000147 RID: 327
		private float m_timelineTransitionStartPoint;

		// Token: 0x04000148 RID: 328
		private float m_timelineTransitionEndPoint;

		// Token: 0x04000149 RID: 329
		private float m_timelineTransitionDuration;

		// Token: 0x0400014A RID: 330
		private float m_timelineTransitionStep;

		// Token: 0x0400014B RID: 331
		private float m_temporaryTimeline;

		// Token: 0x0400014C RID: 332
		private bool m_isPlayingTimelineTransition;
	}
}
