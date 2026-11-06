using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic.Debugging
{
	// Token: 0x02000BB7 RID: 2999
	[DisallowMultipleComponent]
	public class TrafficDeepDiagnostics : MonoBehaviour
	{
		// Token: 0x06005606 RID: 22022 RVA: 0x002168BC File Offset: 0x00214ABC
		private void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			foreach (VehicleAI vehicleAI in Object.FindObjectsByType<VehicleAI>(FindObjectsSortMode.None))
			{
				if (!(vehicleAI == null) && vehicleAI.isActiveAndEnabled)
				{
					TrafficDeepDiagnostics.Track track;
					if (!this._tracks.TryGetValue(vehicleAI, out track))
					{
						track = new TrafficDeepDiagnostics.Track
						{
							lastPos = vehicleAI.transform.position,
							lastRot = vehicleAI.transform.rotation,
							hasLast = true
						};
						this._tracks[vehicleAI] = track;
					}
					else
					{
						Vector3 position = vehicleAI.transform.position;
						Quaternion rotation = vehicleAI.transform.rotation;
						float num = (position - track.lastPos).magnitude / fixedDeltaTime;
						float num2 = Quaternion.Angle(track.lastRot, rotation) / fixedDeltaTime;
						track.lastPos = position;
						track.lastRot = rotation;
						string text = ((vehicleAI.CurrentTrack != null) ? vehicleAI.CurrentTrack.name : "null");
						if (num < 0.1f && num2 > 40f)
						{
							track.spinAccumDeg += num2 * fixedDeltaTime;
							if (track.spinAccumDeg > 60f)
							{
								this._spinEvents++;
								this._spinEx = string.Format("'{0}' spin {1:F0}° fermo su '{2}' (angVel={3:F0}°/s)", new object[] { vehicleAI.name, track.spinAccumDeg, text, num2 });
								track.spinAccumDeg = 0f;
							}
						}
						else
						{
							track.spinAccumDeg = 0f;
						}
						if (vehicleAI.CurrentSpeed < this.stillSpeed)
						{
							track.stillTime += fixedDeltaTime;
							if (track.stillTime > this.stuckSeconds && !track.stuckReported)
							{
								track.stuckReported = true;
								string text2 = string.Format("blocked={0} prio={1} ", vehicleAI.IsBlocked, vehicleAI.hasIntersectionPriority) + string.Format("t={0:F2} group='{1}'", vehicleAI.PathT, (vehicleAI.CurrentTrack != null) ? vehicleAI.CurrentTrack.intersectionGroupId : "");
								if (text.StartsWith("Arc_") || text.StartsWith("Int_") || text.StartsWith("Corner"))
								{
									this._curveStallEvents++;
									this._curveEx = string.Format("'{0}' fermo {1:F1}s IN CURVA '{2}' speed={3:F2} {4}", new object[] { vehicleAI.name, track.stillTime, text, vehicleAI.CurrentSpeed, text2 });
								}
								else
								{
									this._stuckEvents++;
									this._stuckEx = string.Format("'{0}' fermo {1:F1}s su '{2}' speed={3:F2} {4}", new object[] { vehicleAI.name, track.stillTime, text, vehicleAI.CurrentSpeed, text2 });
								}
							}
						}
						else
						{
							track.stillTime = 0f;
							track.stuckReported = false;
						}
					}
				}
			}
			VehicleAI[] array;
			for (int j = 0; j < array.Length; j++)
			{
				if (!(array[j] == null) && array[j].isActiveAndEnabled)
				{
					for (int k = j + 1; k < array.Length; k++)
					{
						if (!(array[k] == null) && array[k].isActiveAndEnabled)
						{
							float num3 = Vector3.Distance(array[j].transform.position, array[k].transform.position);
							if (num3 < this.overlapDist)
							{
								this._nearMissEvents++;
								if (num3 < this.overlapHardDist)
								{
									this._hardOverlapEvents++;
								}
								string text3 = ((array[j].CurrentTrack != null) ? array[j].CurrentTrack.name : "?");
								string text4 = ((array[k].CurrentTrack != null) ? array[k].CurrentTrack.name : "?");
								string text5 = TrafficDeepDiagnostics.StripName(text3);
								string text6 = TrafficDeepDiagnostics.StripName(text4);
								string text7 = ((string.CompareOrdinal(text5, text6) <= 0) ? (text5 + " ↔ " + text6) : (text6 + " ↔ " + text5));
								int num4;
								this._nearMissByPair.TryGetValue(text7, out num4);
								this._nearMissByPair[text7] = num4 + 1;
							}
						}
					}
				}
			}
			this._reportTimer -= fixedDeltaTime;
			if (this._reportTimer <= 0f)
			{
				this._reportTimer = this.reportInterval;
				this.Report(array.Length);
			}
		}

		// Token: 0x06005607 RID: 22023 RVA: 0x00216D98 File Offset: 0x00214F98
		private static string StripName(string n)
		{
			int num = n.IndexOf("_pooled");
			if (num <= 0)
			{
				return n;
			}
			return n.Substring(0, num);
		}

		// Token: 0x06005608 RID: 22024 RVA: 0x00216DC0 File Offset: 0x00214FC0
		private void Report(int vehicleCount)
		{
			Debug.Log(string.Format("[DeepDiag] veicoli={0} | SPIN={1} STUCK={2} ", vehicleCount, this._spinEvents, this._stuckEvents) + string.Format("CURVE-STALL={0} NEARMISS={1} (di cui COMPENETRAZIONE<{2}m={3})", new object[] { this._curveStallEvents, this._nearMissEvents, this.overlapHardDist, this._hardOverlapEvents }));
			if (this._spinEvents > 0)
			{
				Debug.LogWarning("[DeepDiag][SPIN] " + this._spinEx);
			}
			if (this._stuckEvents > 0)
			{
				Debug.LogWarning("[DeepDiag][STUCK] " + this._stuckEx);
			}
			if (this._curveStallEvents > 0)
			{
				Debug.LogWarning("[DeepDiag][CURVE-STALL] " + this._curveEx);
			}
			if (this._nearMissByPair.Count > 0)
			{
				List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(this._nearMissByPair);
				list.Sort((KeyValuePair<string, int> x, KeyValuePair<string, int> y) => y.Value.CompareTo(x.Value));
				StringBuilder stringBuilder = new StringBuilder(256);
				stringBuilder.Append("[DeepDiag][NEARMISS by track] ");
				int num = 0;
				while (num < list.Count && num < 5)
				{
					stringBuilder.Append(string.Format("{0}={1}  ", list[num].Key, list[num].Value));
					num++;
				}
				Debug.LogWarning(stringBuilder.ToString());
			}
			this._spinEvents = (this._stuckEvents = (this._curveStallEvents = (this._nearMissEvents = (this._hardOverlapEvents = 0))));
			this._nearMissByPair.Clear();
		}

		// Token: 0x04005675 RID: 22133
		[Tooltip("Ogni quanti secondi stampare il report aggregato.")]
		public float reportInterval = 3f;

		// Token: 0x04005676 RID: 22134
		[Tooltip("Distanza sotto la quale due veicoli sono un near-miss/overlap (m).")]
		public float overlapDist = 3.5f;

		// Token: 0x04005677 RID: 22135
		[Tooltip("Velocità sotto la quale un veicolo è 'fermo' ai fini stuck/curve (m/s).")]
		public float stillSpeed = 0.4f;

		// Token: 0x04005678 RID: 22136
		[Tooltip("Secondi di immobilità (non-bloccato) oltre i quali logga uno STUCK.")]
		public float stuckSeconds = 2.5f;

		// Token: 0x04005679 RID: 22137
		private readonly Dictionary<VehicleAI, TrafficDeepDiagnostics.Track> _tracks = new Dictionary<VehicleAI, TrafficDeepDiagnostics.Track>();

		// Token: 0x0400567A RID: 22138
		private float _reportTimer;

		// Token: 0x0400567B RID: 22139
		[Tooltip("Distanza sotto la quale è COMPENETRAZIONE grave (auto una dentro l'altra), non coda.")]
		public float overlapHardDist = 1.8f;

		// Token: 0x0400567C RID: 22140
		private int _spinEvents;

		// Token: 0x0400567D RID: 22141
		private int _stuckEvents;

		// Token: 0x0400567E RID: 22142
		private int _curveStallEvents;

		// Token: 0x0400567F RID: 22143
		private int _nearMissEvents;

		// Token: 0x04005680 RID: 22144
		private int _hardOverlapEvents;

		// Token: 0x04005681 RID: 22145
		private string _spinEx = "";

		// Token: 0x04005682 RID: 22146
		private string _stuckEx = "";

		// Token: 0x04005683 RID: 22147
		private string _curveEx = "";

		// Token: 0x04005684 RID: 22148
		private readonly Dictionary<string, int> _nearMissByPair = new Dictionary<string, int>();

		// Token: 0x02000BB8 RID: 3000
		private class Track
		{
			// Token: 0x04005685 RID: 22149
			public float stillTime;

			// Token: 0x04005686 RID: 22150
			public float spinAccumDeg;

			// Token: 0x04005687 RID: 22151
			public Vector3 lastPos;

			// Token: 0x04005688 RID: 22152
			public Quaternion lastRot;

			// Token: 0x04005689 RID: 22153
			public bool hasLast;

			// Token: 0x0400568A RID: 22154
			public bool stuckReported;
		}
	}
}
