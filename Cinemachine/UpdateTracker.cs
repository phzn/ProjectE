using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200004E RID: 78
	[DocumentationSorting(DocumentationSortingAttribute.Level.Undoc)]
	internal class UpdateTracker
	{
		// Token: 0x0600035C RID: 860 RVA: 0x000150DE File Offset: 0x000132DE
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeModule()
		{
			UpdateTracker.mUpdateStatus.Clear();
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000150EC File Offset: 0x000132EC
		private static void UpdateTargets(UpdateTracker.UpdateClock currentClock)
		{
			int frameCount = Time.frameCount;
			foreach (KeyValuePair<Transform, UpdateTracker.UpdateStatus> keyValuePair in UpdateTracker.mUpdateStatus)
			{
				if (keyValuePair.Key == null)
				{
					UpdateTracker.sToDelete.Add(keyValuePair.Key);
				}
				else
				{
					keyValuePair.Value.OnUpdate(frameCount, currentClock, keyValuePair.Key.localToWorldMatrix);
				}
			}
			for (int i = UpdateTracker.sToDelete.Count - 1; i >= 0; i--)
			{
				UpdateTracker.mUpdateStatus.Remove(UpdateTracker.sToDelete[i]);
			}
			UpdateTracker.sToDelete.Clear();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00015190 File Offset: 0x00013390
		public static UpdateTracker.UpdateClock GetPreferredUpdate(Transform target)
		{
			if (Application.isPlaying && target != null)
			{
				UpdateTracker.UpdateStatus updateStatus;
				if (UpdateTracker.mUpdateStatus.TryGetValue(target, out updateStatus))
				{
					return updateStatus.PreferredUpdate;
				}
				updateStatus = new UpdateTracker.UpdateStatus(Time.frameCount, target.localToWorldMatrix);
				UpdateTracker.mUpdateStatus.Add(target, updateStatus);
			}
			return UpdateTracker.UpdateClock.Late;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000151E4 File Offset: 0x000133E4
		public static void OnUpdate(UpdateTracker.UpdateClock currentClock)
		{
			float currentTime = CinemachineCore.CurrentTime;
			if (currentTime != UpdateTracker.mLastUpdateTime)
			{
				UpdateTracker.mLastUpdateTime = currentTime;
				UpdateTracker.UpdateTargets(currentClock);
			}
		}

		// Token: 0x04000239 RID: 569
		private static Dictionary<Transform, UpdateTracker.UpdateStatus> mUpdateStatus = new Dictionary<Transform, UpdateTracker.UpdateStatus>();

		// Token: 0x0400023A RID: 570
		private static List<Transform> sToDelete = new List<Transform>();

		// Token: 0x0400023B RID: 571
		private static float mLastUpdateTime;

		// Token: 0x020000C0 RID: 192
		public enum UpdateClock
		{
			// Token: 0x040003CD RID: 973
			Fixed,
			// Token: 0x040003CE RID: 974
			Late
		}

		// Token: 0x020000C1 RID: 193
		private class UpdateStatus
		{
			// Token: 0x170000EA RID: 234
			// (get) Token: 0x0600047E RID: 1150 RVA: 0x00019F20 File Offset: 0x00018120
			// (set) Token: 0x0600047F RID: 1151 RVA: 0x00019F28 File Offset: 0x00018128
			public UpdateTracker.UpdateClock PreferredUpdate { get; private set; }

			// Token: 0x06000480 RID: 1152 RVA: 0x00019F31 File Offset: 0x00018131
			public UpdateStatus(int currentFrame, Matrix4x4 pos)
			{
				this.windowStart = currentFrame;
				this.lastFrameUpdated = Time.frameCount;
				this.PreferredUpdate = UpdateTracker.UpdateClock.Late;
				this.lastPos = pos;
			}

			// Token: 0x06000481 RID: 1153 RVA: 0x00019F5C File Offset: 0x0001815C
			public void OnUpdate(int currentFrame, UpdateTracker.UpdateClock currentClock, Matrix4x4 pos)
			{
				if (this.lastPos == pos)
				{
					return;
				}
				if (currentClock == UpdateTracker.UpdateClock.Late)
				{
					this.numWindowLateUpdateMoves++;
				}
				else if (this.lastFrameUpdated != currentFrame)
				{
					this.numWindowFixedUpdateMoves++;
				}
				this.lastPos = pos;
				UpdateTracker.UpdateClock updateClock;
				if (this.numWindowFixedUpdateMoves > 3 && this.numWindowLateUpdateMoves < this.numWindowFixedUpdateMoves / 3)
				{
					updateClock = UpdateTracker.UpdateClock.Fixed;
				}
				else
				{
					updateClock = UpdateTracker.UpdateClock.Late;
				}
				if (this.numWindows == 0)
				{
					this.PreferredUpdate = updateClock;
				}
				if (this.windowStart + 30 <= currentFrame)
				{
					this.PreferredUpdate = updateClock;
					this.numWindows++;
					this.windowStart = currentFrame;
					this.numWindowLateUpdateMoves = ((this.PreferredUpdate == UpdateTracker.UpdateClock.Late) ? 1 : 0);
					this.numWindowFixedUpdateMoves = ((this.PreferredUpdate == UpdateTracker.UpdateClock.Fixed) ? 1 : 0);
				}
			}

			// Token: 0x040003CF RID: 975
			private const int kWindowSize = 30;

			// Token: 0x040003D0 RID: 976
			private int windowStart;

			// Token: 0x040003D1 RID: 977
			private int numWindowLateUpdateMoves;

			// Token: 0x040003D2 RID: 978
			private int numWindowFixedUpdateMoves;

			// Token: 0x040003D3 RID: 979
			private int numWindows;

			// Token: 0x040003D4 RID: 980
			private int lastFrameUpdated;

			// Token: 0x040003D5 RID: 981
			private Matrix4x4 lastPos;
		}
	}
}
