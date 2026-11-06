using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic.Diagnostics
{
	// Token: 0x02000BB2 RID: 2994
	public class TrafficDiagProbe : MonoBehaviour
	{
		// Token: 0x060055E4 RID: 21988 RVA: 0x002153E0 File Offset: 0x002135E0
		private void Awake()
		{
			this._nextReport = Time.time + 15f;
			Debug.Log("[TrafficProbe] started.");
		}

		// Token: 0x060055E5 RID: 21989 RVA: 0x00215400 File Offset: 0x00213600
		private void Update()
		{
			if (Time.time < this._nextSample)
			{
				return;
			}
			this._nextSample = Time.time + 0.25f;
			if (this._player == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this._player = gameObject.transform;
				}
			}
			if (this._cam == null)
			{
				this._cam = Camera.main;
			}
			if (this._cam != null)
			{
				GeometryUtility.CalculateFrustumPlanes(this._cam, this._planes);
			}
			VehicleAI[] array = Object.FindObjectsOfType<VehicleAI>();
			foreach (KeyValuePair<int, TrafficDiagProbe.VehState> keyValuePair in this._tracked)
			{
				keyValuePair.Value.seenThisPass = false;
			}
			foreach (VehicleAI vehicleAI in array)
			{
				if (!(vehicleAI == null) && vehicleAI.isActiveAndEnabled)
				{
					int instanceID = vehicleAI.GetInstanceID();
					TrafficDiagProbe.VehState vehState;
					if (!this._tracked.TryGetValue(instanceID, out vehState))
					{
						vehState = this.NewState(vehicleAI);
						this._tracked[instanceID] = vehState;
						this.LogEvent(string.Format("[SPAWN] '{0}' at {1} dist={2:0.0}m inView={3} speed={4:0.0}", new object[]
						{
							vehState.name,
							TrafficDiagProbe.Fmt(vehicleAI.transform.position),
							vehState.spawnDist,
							vehState.spawnInView,
							vehicleAI.CurrentSpeed
						}));
					}
					else
					{
						vehState.seenThisPass = true;
						this.SampleVehicle(vehState, vehicleAI);
					}
				}
			}
			List<int> list = null;
			foreach (KeyValuePair<int, TrafficDiagProbe.VehState> keyValuePair2 in this._tracked)
			{
				TrafficDiagProbe.VehState value = keyValuePair2.Value;
				if (!value.seenThisPass && Time.time - value.firstSeen >= 0.5f)
				{
					List<int> list2;
					if ((list2 = list) == null)
					{
						list2 = (list = new List<int>());
					}
					list2.Add(keyValuePair2.Key);
					float num = ((this._player != null) ? Vector3.Distance(value.lastPos, this._player.position) : (-1f));
					bool flag = this.IsInView(value.lastPos);
					if (num >= 0f && (num < 35f || flag))
					{
						this.LogEvent(string.Format("[DESPAWN] '{0}' at {1} dist={2:0.0}m inView={3} — near/visible despawn!", new object[]
						{
							value.name,
							TrafficDiagProbe.Fmt(value.lastPos),
							num,
							flag
						}));
					}
					this._dead.Add(value);
				}
			}
			if (list != null)
			{
				foreach (int num2 in list)
				{
					this._tracked.Remove(num2);
				}
			}
			this.DetectOverlaps(array);
			if (Time.time >= this._nextReport)
			{
				this._nextReport = Time.time + 15f;
				this.DumpReport();
			}
		}

		// Token: 0x060055E6 RID: 21990 RVA: 0x00215790 File Offset: 0x00213990
		private TrafficDiagProbe.VehState NewState(VehicleAI v)
		{
			return new TrafficDiagProbe.VehState
			{
				ai = v,
				name = v.name,
				firstSeen = Time.time,
				lastSampleTime = Time.time,
				lastPos = v.transform.position,
				lastSpeed = v.CurrentSpeed,
				lastYaw = v.transform.eulerAngles.y,
				spawnDist = ((this._player != null) ? Vector3.Distance(v.transform.position, this._player.position) : (-1f)),
				spawnInView = this.IsInView(v.transform.position)
			};
		}

		// Token: 0x060055E7 RID: 21991 RVA: 0x0021584C File Offset: 0x00213A4C
		private void SampleVehicle(TrafficDiagProbe.VehState s, VehicleAI v)
		{
			float time = Time.time;
			float num = Mathf.Max(0.01f, time - s.lastSampleTime);
			float currentSpeed = v.CurrentSpeed;
			float y = v.transform.eulerAngles.y;
			float num2 = Mathf.Abs(Mathf.DeltaAngle(s.lastYaw, y));
			if (time - s.firstSeen <= 1.5f && currentSpeed < 1f)
			{
				s.rotWhileSlow += num2;
				if (!s.rotSpawnReported && s.rotWhileSlow > 25f)
				{
					s.rotSpawnReported = true;
					this.LogEvent(string.Format("[ROTSPAWN] '{0}' pirouetted {1:0}° at spawn {2} (speed {3:0.0})", new object[]
					{
						s.name,
						s.rotWhileSlow,
						TrafficDiagProbe.Fmt(v.transform.position),
						currentSpeed
					}));
				}
			}
			if (currentSpeed > 2f)
			{
				s.totalTurn += num2;
			}
			if (currentSpeed < 0.05f)
			{
				if (s.stoppedSince < 0f)
				{
					s.stoppedSince = time;
				}
				else if (!s.stuckReported && time - s.stoppedSince > 5f)
				{
					s.stuckReported = true;
					this.LogEvent(string.Format("[STUCK] '{0}' stopped {1:0.0}s at {2} hasPassenger={3}", new object[]
					{
						s.name,
						time - s.stoppedSince,
						TrafficDiagProbe.Fmt(v.transform.position),
						v.HasPassenger
					}));
				}
			}
			else
			{
				if (s.stuckReported)
				{
					this.LogEvent(string.Format("[STUCK-END] '{0}' moving again after {1:0.0}s", s.name, time - s.stoppedSince));
				}
				s.stoppedSince = -1f;
				s.stuckReported = false;
			}
			float num3 = (currentSpeed - s.lastSpeed) / num;
			if (num3 > s.maxAccel)
			{
				s.maxAccel = num3;
			}
			if (num3 < s.maxDecel)
			{
				s.maxDecel = num3;
			}
			if (Mathf.Abs(num3) > 8f)
			{
				s.jerkEvents++;
			}
			s.lastSampleTime = time;
			s.lastPos = v.transform.position;
			s.lastSpeed = currentSpeed;
			s.lastYaw = y;
		}

		// Token: 0x060055E8 RID: 21992 RVA: 0x00215A88 File Offset: 0x00213C88
		private void DetectOverlaps(VehicleAI[] vehicles)
		{
			for (int i = 0; i < vehicles.Length; i++)
			{
				VehicleAI vehicleAI = vehicles[i];
				if (!(vehicleAI == null) && vehicleAI.isActiveAndEnabled)
				{
					for (int j = i + 1; j < vehicles.Length; j++)
					{
						VehicleAI vehicleAI2 = vehicles[j];
						if (!(vehicleAI2 == null) && vehicleAI2.isActiveAndEnabled && (vehicleAI.transform.position - vehicleAI2.transform.position).sqrMagnitude <= 6.25f)
						{
							int instanceID = vehicleAI.GetInstanceID();
							int instanceID2 = vehicleAI2.GetInstanceID();
							long num = ((instanceID < instanceID2) ? (((long)instanceID << 32) | (long)((ulong)instanceID2)) : (((long)instanceID2 << 32) | (long)((ulong)instanceID)));
							if (this._overlapPairs.Add(num))
							{
								this.LogEvent(string.Format("[OVERLAP] '{0}' + '{1}' {2:0.00}m apart at {3} speeds=({4:0.0},{5:0.0})", new object[]
								{
									vehicleAI.name,
									vehicleAI2.name,
									Vector3.Distance(vehicleAI.transform.position, vehicleAI2.transform.position),
									TrafficDiagProbe.Fmt(vehicleAI.transform.position),
									vehicleAI.CurrentSpeed,
									vehicleAI2.CurrentSpeed
								}));
							}
						}
					}
				}
			}
		}

		// Token: 0x060055E9 RID: 21993 RVA: 0x00215BDA File Offset: 0x00213DDA
		private bool IsInView(Vector3 pos)
		{
			return !(this._cam == null) && GeometryUtility.TestPlanesAABB(this._planes, new Bounds(pos, Vector3.one * 2f));
		}

		// Token: 0x060055EA RID: 21994 RVA: 0x00215C0C File Offset: 0x00213E0C
		private void LogEvent(string msg)
		{
			this._events.Add(string.Format("t={0:0.0} {1}", Time.time, msg));
			Debug.Log("[TrafficProbe] " + msg);
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x00215C3E File Offset: 0x00213E3E
		private static string Fmt(Vector3 p)
		{
			return string.Format("({0:0.0}, {1:0.0}, {2:0.0})", p.x, p.y, p.z);
		}

		// Token: 0x060055EC RID: 21996 RVA: 0x00215C6C File Offset: 0x00213E6C
		public void DumpReport()
		{
			int count = this._tracked.Count;
			int num = count + this._dead.Count;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			float num6 = 0f;
			float num7 = 0f;
			List<TrafficDiagProbe.VehState> list = new List<TrafficDiagProbe.VehState>(this._dead);
			foreach (KeyValuePair<int, TrafficDiagProbe.VehState> keyValuePair in this._tracked)
			{
				list.Add(keyValuePair.Value);
			}
			foreach (TrafficDiagProbe.VehState vehState in list)
			{
				if (vehState.totalTurn > 40f)
				{
					num2++;
				}
				else
				{
					num3++;
				}
				if (vehState.jerkEvents > 0)
				{
					num4++;
				}
				if (vehState.rotSpawnReported)
				{
					num5++;
				}
				if (vehState.maxAccel > num6)
				{
					num6 = vehState.maxAccel;
				}
				if (vehState.maxDecel < num7)
				{
					num7 = vehState.maxDecel;
				}
			}
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			foreach (string text in this._events)
			{
				if (text.Contains("[OVERLAP]"))
				{
					num8++;
				}
				else if (text.Contains("[SPAWN]") && text.Contains("inView=True"))
				{
					num9++;
				}
				else if (text.Contains("[DESPAWN]"))
				{
					num11++;
				}
				else if (text.Contains("[STUCK]") && !text.Contains("STUCK-END"))
				{
					num12++;
				}
			}
			foreach (string text2 in this._events)
			{
				if (text2.Contains("[SPAWN]"))
				{
					int num13 = text2.IndexOf("dist=");
					float num14;
					if (num13 >= 0 && float.TryParse(text2.Substring(num13 + 5, text2.IndexOf('m', num13) - num13 - 5), NumberStyles.Float, CultureInfo.InvariantCulture, out num14) && num14 < 35f)
					{
						num10++;
					}
				}
			}
			Debug.Log(string.Format("[TrafficProbe] ===== REPORT t={0:0} ===== vehicles alive={1} totalSeen={2} | ", Time.time, count, num) + string.Format("overlapPairs={0} spawnsInView={1} spawnsNear(<{2:0}m)={3} ", new object[] { num8, num9, 35f, num10 }) + string.Format("nearDespawns={0} rotSpawns={1} stuckEvents={2} | ", num11, num5, num12) + string.Format("turned={0} straightOnly={1} | worstAccel={2:0.0} worstDecel={3:0.0} jerkyCars={4}", new object[] { num2, num3, num6, num7, num4 }));
			StringBuilder stringBuilder = new StringBuilder(16384);
			stringBuilder.AppendLine(string.Format("TrafficProbe report — t={0:0.0}s, vehicles alive={1}, totalSeen={2}", Time.time, count, num));
			stringBuilder.AppendLine(string.Format("turned={0} straightOnly={1} rotSpawns={2} jerkyCars={3} worstAccel={4:0.00} worstDecel={5:0.00}", new object[] { num2, num3, num5, num4, num6, num7 }));
			stringBuilder.AppendLine("--- per-vehicle (alive) ---");
			foreach (KeyValuePair<int, TrafficDiagProbe.VehState> keyValuePair2 in this._tracked)
			{
				TrafficDiagProbe.VehState value = keyValuePair2.Value;
				stringBuilder.AppendLine(string.Format("{0}: age={1:0}s pos={2} speed={3:0.0} totalTurn={4:0}° maxAcc={5:0.0} maxDec={6:0.0} jerks={7}", new object[]
				{
					value.name,
					Time.time - value.firstSeen,
					TrafficDiagProbe.Fmt(value.lastPos),
					value.lastSpeed,
					value.totalTurn,
					value.maxAccel,
					value.maxDecel,
					value.jerkEvents
				}));
			}
			stringBuilder.AppendLine(string.Format("--- events ({0}) ---", this._events.Count));
			foreach (string text3 in this._events)
			{
				stringBuilder.AppendLine(text3);
			}
			string text4 = Path.Combine(Application.dataPath, "../Temp/TrafficProbeReport.txt");
			try
			{
				File.WriteAllText(text4, stringBuilder.ToString());
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[TrafficProbe] report write failed: " + ex.Message);
			}
		}

		// Token: 0x04005644 RID: 22084
		private const float SampleInterval = 0.25f;

		// Token: 0x04005645 RID: 22085
		private const float ReportInterval = 15f;

		// Token: 0x04005646 RID: 22086
		private const float OverlapDistance = 2.5f;

		// Token: 0x04005647 RID: 22087
		private const float NearPlayerDistance = 35f;

		// Token: 0x04005648 RID: 22088
		private const float StuckSeconds = 5f;

		// Token: 0x04005649 RID: 22089
		private const float RotSpawnWindow = 1.5f;

		// Token: 0x0400564A RID: 22090
		private const float RotSpawnDegrees = 25f;

		// Token: 0x0400564B RID: 22091
		private const float TurnedThresholdDeg = 40f;

		// Token: 0x0400564C RID: 22092
		private const float JerkThreshold = 8f;

		// Token: 0x0400564D RID: 22093
		private readonly Dictionary<int, TrafficDiagProbe.VehState> _tracked = new Dictionary<int, TrafficDiagProbe.VehState>();

		// Token: 0x0400564E RID: 22094
		private readonly List<string> _events = new List<string>(256);

		// Token: 0x0400564F RID: 22095
		private readonly HashSet<long> _overlapPairs = new HashSet<long>();

		// Token: 0x04005650 RID: 22096
		private readonly List<TrafficDiagProbe.VehState> _dead = new List<TrafficDiagProbe.VehState>();

		// Token: 0x04005651 RID: 22097
		private float _nextSample;

		// Token: 0x04005652 RID: 22098
		private float _nextReport;

		// Token: 0x04005653 RID: 22099
		private Transform _player;

		// Token: 0x04005654 RID: 22100
		private Camera _cam;

		// Token: 0x04005655 RID: 22101
		private readonly Plane[] _planes = new Plane[6];

		// Token: 0x02000BB3 RID: 2995
		private class VehState
		{
			// Token: 0x04005656 RID: 22102
			public VehicleAI ai;

			// Token: 0x04005657 RID: 22103
			public string name;

			// Token: 0x04005658 RID: 22104
			public float firstSeen;

			// Token: 0x04005659 RID: 22105
			public float lastSampleTime;

			// Token: 0x0400565A RID: 22106
			public Vector3 lastPos;

			// Token: 0x0400565B RID: 22107
			public float lastSpeed;

			// Token: 0x0400565C RID: 22108
			public float lastYaw;

			// Token: 0x0400565D RID: 22109
			public float rotWhileSlow;

			// Token: 0x0400565E RID: 22110
			public bool rotSpawnReported;

			// Token: 0x0400565F RID: 22111
			public float totalTurn;

			// Token: 0x04005660 RID: 22112
			public float stoppedSince = -1f;

			// Token: 0x04005661 RID: 22113
			public bool stuckReported;

			// Token: 0x04005662 RID: 22114
			public float maxAccel;

			// Token: 0x04005663 RID: 22115
			public float maxDecel;

			// Token: 0x04005664 RID: 22116
			public int jerkEvents;

			// Token: 0x04005665 RID: 22117
			public float spawnDist;

			// Token: 0x04005666 RID: 22118
			public bool spawnInView;

			// Token: 0x04005667 RID: 22119
			public bool seenThisPass;
		}
	}
}
