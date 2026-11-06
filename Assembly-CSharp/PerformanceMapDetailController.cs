using System;
using ProjectExhibitionist.Core.SceneManagement;
using Unity.Profiling;
using UnityEngine;

// Token: 0x0200008E RID: 142
[DisallowMultipleComponent]
public sealed class PerformanceMapDetailController : MonoBehaviour
{
	// Token: 0x1700016D RID: 365
	// (get) Token: 0x0600061A RID: 1562 RVA: 0x0002C256 File Offset: 0x0002A456
	// (set) Token: 0x0600061B RID: 1563 RVA: 0x0002C25E File Offset: 0x0002A45E
	public int HighDetailGroups { get; private set; }

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x0600061C RID: 1564 RVA: 0x0002C267 File Offset: 0x0002A467
	// (set) Token: 0x0600061D RID: 1565 RVA: 0x0002C26F File Offset: 0x0002A46F
	public int MediumDetailGroups { get; private set; }

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x0600061E RID: 1566 RVA: 0x0002C278 File Offset: 0x0002A478
	// (set) Token: 0x0600061F RID: 1567 RVA: 0x0002C280 File Offset: 0x0002A480
	public int LowDetailGroups { get; private set; }

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06000620 RID: 1568 RVA: 0x0002C289 File Offset: 0x0002A489
	// (set) Token: 0x06000621 RID: 1569 RVA: 0x0002C291 File Offset: 0x0002A491
	public int LevelChangesLastFrame { get; private set; }

	// Token: 0x06000622 RID: 1570 RVA: 0x0002C29C File Offset: 0x0002A49C
	private void OnEnable()
	{
		this.viewCamera = null;
		for (int i = 0; i < this.groups.Length; i++)
		{
			this.groups[i].selectedLevel = -1;
		}
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002C2D4 File Offset: 0x0002A4D4
	private void OnDisable()
	{
		for (int i = 0; i < this.groups.Length; i++)
		{
			LODGroup lodGroup = this.groups[i].lodGroup;
			if (lodGroup != null && lodGroup.enabled && lodGroup.gameObject.activeInHierarchy)
			{
				lodGroup.ForceLOD(-1);
			}
			this.groups[i].selectedLevel = -1;
		}
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002C338 File Offset: 0x0002A538
	private void LateUpdate()
	{
		Transform player = PlayerRegistry.Player;
		if (player == null)
		{
			return;
		}
		if (this.viewCamera == null || !this.viewCamera.isActiveAndEnabled)
		{
			this.viewCamera = Camera.main;
		}
		if (this.viewCamera == null)
		{
			return;
		}
		this.UpdateDetail(player.position, this.viewCamera.transform.position);
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002C3A8 File Offset: 0x0002A5A8
	public void UpdateDetail(Vector3 playerPosition, Vector3 cameraPosition)
	{
		using (PerformanceMapDetailController.UpdateMarker.Auto())
		{
			this.HighDetailGroups = (this.MediumDetailGroups = (this.LowDetailGroups = (this.LevelChangesLastFrame = 0)));
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			for (int i = 0; i < this.groups.Length; i++)
			{
				PerformanceMapDetailController.Group group = this.groups[i];
				if (!(group.lodGroup == null) && group.lodGroup.enabled && group.lodGroup.gameObject.activeInHierarchy)
				{
					Bounds bounds = PerformanceMapDetailController.TransformBounds(group.localBounds, localToWorldMatrix);
					int num = PerformanceMapDetailController.SelectLevel(Mathf.Sqrt(Mathf.Min(bounds.SqrDistance(playerPosition), bounds.SqrDistance(cameraPosition))), group.selectedLevel, group.levelCount, this.highDetailDistance, this.lowDetailDistance, this.distanceHysteresis);
					if (num != group.selectedLevel)
					{
						group.lodGroup.ForceLOD(num);
						group.selectedLevel = num;
						int num2 = this.LevelChangesLastFrame;
						this.LevelChangesLastFrame = num2 + 1;
					}
					if (num == 0)
					{
						int num2 = this.HighDetailGroups;
						this.HighDetailGroups = num2 + 1;
					}
					else if (num == 1)
					{
						int num2 = this.MediumDetailGroups;
						this.MediumDetailGroups = num2 + 1;
					}
					else
					{
						int num2 = this.LowDetailGroups;
						this.LowDetailGroups = num2 + 1;
					}
				}
			}
		}
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0002C544 File Offset: 0x0002A744
	public static int SelectLevel(float distance, int previous, int count, float near, float far, float hysteresis)
	{
		if (count <= 1 || distance <= near || (previous == 0 && distance <= near + hysteresis))
		{
			return 0;
		}
		if (count == 2 || distance <= far || (previous == 1 && distance <= far + hysteresis))
		{
			return 1;
		}
		return count - 1;
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0002C574 File Offset: 0x0002A774
	public static Bounds TransformBounds(Bounds local, Matrix4x4 matrix)
	{
		Vector3 vector = matrix.MultiplyVector(new Vector3(local.extents.x, 0f, 0f));
		Vector3 vector2 = matrix.MultiplyVector(new Vector3(0f, local.extents.y, 0f));
		Vector3 vector3 = matrix.MultiplyVector(new Vector3(0f, 0f, local.extents.z));
		Vector3 vector4 = new Vector3(Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x), Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y), Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z));
		return new Bounds(matrix.MultiplyPoint3x4(local.center), vector4 * 2f);
	}

	// Token: 0x040004C1 RID: 1217
	[SerializeField]
	[Min(1f)]
	private float highDetailDistance = 20f;

	// Token: 0x040004C2 RID: 1218
	[SerializeField]
	[Min(2f)]
	private float lowDetailDistance = 40f;

	// Token: 0x040004C3 RID: 1219
	[SerializeField]
	[Min(0f)]
	private float distanceHysteresis = 3f;

	// Token: 0x040004C4 RID: 1220
	[SerializeField]
	private PerformanceMapDetailController.Group[] groups = Array.Empty<PerformanceMapDetailController.Group>();

	// Token: 0x040004C5 RID: 1221
	private static readonly ProfilerMarker UpdateMarker = new ProfilerMarker("PerformanceMapDetail.Update");

	// Token: 0x040004C6 RID: 1222
	private Camera viewCamera;

	// Token: 0x0200008F RID: 143
	[Serializable]
	public sealed class Group
	{
		// Token: 0x040004CB RID: 1227
		public LODGroup lodGroup;

		// Token: 0x040004CC RID: 1228
		public Bounds localBounds;

		// Token: 0x040004CD RID: 1229
		public int levelCount;

		// Token: 0x040004CE RID: 1230
		[NonSerialized]
		public int selectedLevel = -1;
	}
}
