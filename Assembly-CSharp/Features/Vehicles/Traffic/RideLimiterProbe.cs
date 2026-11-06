using System;
using System.IO;
using System.Text;
using Features.Suspicion;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000B9F RID: 2975
	public class RideLimiterProbe : MonoBehaviour
	{
		// Token: 0x06005576 RID: 21878 RVA: 0x00212428 File Offset: 0x00210628
		private void Start()
		{
			this._end = Time.time + this.duration;
			RideLimiterProbe.Append(string.Format("[{0:F1}s] PROBE START (every {1:F0}s for {2:F0}s)\n", Time.time, this.interval, this.duration));
		}

		// Token: 0x06005577 RID: 21879 RVA: 0x00212476 File Offset: 0x00210676
		private void Update()
		{
			if (Time.time >= this._next)
			{
				this._next = Time.time + this.interval;
				this.Sample();
			}
			if (Time.time >= this._end)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x002124B8 File Offset: 0x002106B8
		private void Sample()
		{
			StringBuilder stringBuilder = new StringBuilder();
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			SuspicionManager instance = SuspicionManager.Instance;
			stringBuilder.Append(string.Format("[{0:F1}s] probe player={1} ", Time.time, (gameObject != null) ? gameObject.transform.position.ToString("F1") : "?")).Append("suspicion=" + ((instance != null) ? string.Format("{0:F1} lv{1}", instance.CurrentScore, instance.CurrentLevel) : "n/a") + "\n");
			foreach (CarRideLimiter carRideLimiter in Object.FindObjectsOfType<CarRideLimiter>())
			{
				if (carRideLimiter.IsConfronting || carRideLimiter.IsAbandoned || !(carRideLimiter.DriverNpc == null))
				{
					VehicleAI vehicle = carRideLimiter.Vehicle;
					stringBuilder.Append(string.Format("    '{0}' pos={1:F1} confronting={2} abandoned={3} ", new object[]
					{
						carRideLimiter.name,
						carRideLimiter.transform.position,
						carRideLimiter.IsConfronting,
						carRideLimiter.IsAbandoned
					})).Append("driver=" + ((carRideLimiter.DriverNpc != null) ? (carRideLimiter.DriverNpc.name + "@" + carRideLimiter.DriverNpc.transform.position.ToString("F1")) : "none") + " ");
					if (vehicle != null)
					{
						stringBuilder.Append(string.Format("speed={0:F1} blocked={1} hasPassenger={2}", vehicle.CurrentSpeed, vehicle.IsBlocked, vehicle.HasPassenger));
					}
					stringBuilder.Append('\n');
				}
			}
			foreach (ScaredDriverFleeRunner scaredDriverFleeRunner in Object.FindObjectsOfType<ScaredDriverFleeRunner>())
			{
				stringBuilder.Append(string.Format("    FLEE '{0}' at {1:F1}\n", scaredDriverFleeRunner.name, scaredDriverFleeRunner.transform.position));
			}
			RideLimiterProbe.Append(stringBuilder.ToString());
		}

		// Token: 0x06005579 RID: 21881 RVA: 0x00212708 File Offset: 0x00210908
		private static void Append(string text)
		{
			try
			{
				File.AppendAllText(Path.Combine("Temp", "RideLimiterState.txt"), text);
			}
			catch
			{
			}
		}

		// Token: 0x04005580 RID: 21888
		public float interval = 2f;

		// Token: 0x04005581 RID: 21889
		public float duration = 45f;

		// Token: 0x04005582 RID: 21890
		private float _next;

		// Token: 0x04005583 RID: 21891
		private float _end;
	}
}
