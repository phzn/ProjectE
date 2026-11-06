using System;
using Features.RenderingPrecision;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class PlayerProximityProvider : MonoBehaviour
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060000D0 RID: 208 RVA: 0x000079CC File Offset: 0x00005BCC
	// (set) Token: 0x060000D1 RID: 209 RVA: 0x000079D3 File Offset: 0x00005BD3
	public static PlayerProximityProvider Instance { get; private set; }

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060000D2 RID: 210 RVA: 0x000079DC File Offset: 0x00005BDC
	// (remove) Token: 0x060000D3 RID: 211 RVA: 0x00007A14 File Offset: 0x00005C14
	public event Action<Vector2Int> OnPlayerCellChanged;

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060000D4 RID: 212 RVA: 0x00007A49 File Offset: 0x00005C49
	public Transform Player
	{
		get
		{
			if (!(this.player != null))
			{
				return null;
			}
			return this.player;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060000D5 RID: 213 RVA: 0x00007A61 File Offset: 0x00005C61
	public Vector3 Position
	{
		get
		{
			return this._cachedPosition;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000D6 RID: 214 RVA: 0x00007A69 File Offset: 0x00005C69
	public Vector3 Forward
	{
		get
		{
			return this._cachedForward;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060000D7 RID: 215 RVA: 0x00007A71 File Offset: 0x00005C71
	public Vector2Int CurrentCell
	{
		get
		{
			return this._cachedCell;
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00007A7C File Offset: 0x00005C7C
	private void Awake()
	{
		if (PlayerProximityProvider.Instance != null && PlayerProximityProvider.Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		PlayerProximityProvider.Instance = this;
		WorldOrigin.Shifted += this.OnWorldOriginShifted;
		PlayerRegistry.PlayerChanged += this.BindPlayer;
		if (PlayerRegistry.Player != null)
		{
			this.player = PlayerRegistry.Player;
		}
		if (this.player == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				this.player = gameObject.transform;
			}
		}
		this.BindPlayer(this.player);
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00007B29 File Offset: 0x00005D29
	private void OnDestroy()
	{
		WorldOrigin.Shifted -= this.OnWorldOriginShifted;
		PlayerRegistry.PlayerChanged -= this.BindPlayer;
		if (PlayerProximityProvider.Instance == this)
		{
			PlayerProximityProvider.Instance = null;
		}
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00007B60 File Offset: 0x00005D60
	private void Update()
	{
		if (this.player == null)
		{
			return;
		}
		this.UpdateCache();
		if (this._cachedCell != this._lastCell)
		{
			this._lastCell = this._cachedCell;
			Action<Vector2Int> onPlayerCellChanged = this.OnPlayerCellChanged;
			if (onPlayerCellChanged == null)
			{
				return;
			}
			onPlayerCellChanged(this._cachedCell);
		}
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00007BB8 File Offset: 0x00005DB8
	private void UpdateCache()
	{
		if (this.player == null)
		{
			return;
		}
		if (this._lastUpdateFrame == Time.frameCount)
		{
			return;
		}
		this._lastUpdateFrame = Time.frameCount;
		this._cachedPosition = this.player.position;
		this._cachedForward = this.player.forward;
		this._cachedCell = this.WorldToCell(this._cachedPosition);
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00007C24 File Offset: 0x00005E24
	public void BindPlayer(Transform currentPlayer)
	{
		this.player = ((currentPlayer != null) ? currentPlayer : null);
		this._lastUpdateFrame = -1;
		this._cachedPosition = Vector3.zero;
		this._cachedForward = Vector3.forward;
		this._cachedCell = default(Vector2Int);
		if (this.player != null)
		{
			this.UpdateCache();
		}
		this._lastCell = this._cachedCell;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00007C8D File Offset: 0x00005E8D
	private void OnWorldOriginShifted(Vector3 delta)
	{
		this._cachedPosition += delta;
		this._lastUpdateFrame = -1;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00007CA8 File Offset: 0x00005EA8
	public void SetCellSize(float size)
	{
		this._cellSize = size;
		this._lastUpdateFrame = -1;
		if (this.player != null)
		{
			this.UpdateCache();
		}
		this._lastCell = this._cachedCell;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00007CD8 File Offset: 0x00005ED8
	public float GetDistance(Vector3 point)
	{
		if (this.player == null)
		{
			return float.PositiveInfinity;
		}
		this.UpdateCache();
		return Vector3.Distance(this._cachedPosition, point);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00007D00 File Offset: 0x00005F00
	public float GetDistance2D(Vector3 point)
	{
		if (this.player == null)
		{
			return float.PositiveInfinity;
		}
		this.UpdateCache();
		float num = this._cachedPosition.x - point.x;
		float num2 = this._cachedPosition.z - point.z;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00007D57 File Offset: 0x00005F57
	public bool IsWithinRadius(Vector3 point, float radius)
	{
		return this.GetDistance(point) <= radius;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00007D68 File Offset: 0x00005F68
	public bool IsInFOV(Vector3 point, float fovAngle = -1f, float maxDistance = -1f)
	{
		if (this.player == null)
		{
			return false;
		}
		if (fovAngle < 0f)
		{
			fovAngle = this.defaultFOVAngle;
		}
		if (maxDistance < 0f)
		{
			maxDistance = this.defaultFOVDistance;
		}
		this.UpdateCache();
		Vector3 normalized = (point - this._cachedPosition).normalized;
		normalized.y = 0f;
		Vector3 cameraForward = this.GetCameraForward();
		cameraForward.y = 0f;
		cameraForward.Normalize();
		float num = Vector3.Angle(cameraForward, normalized);
		float distance2D = this.GetDistance2D(point);
		return num <= fovAngle * 0.5f && distance2D <= maxDistance;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00007E08 File Offset: 0x00006008
	public Vector3 GetCameraForward()
	{
		Camera camera = null;
		if (GameSystemsBootstrapper.Instance != null)
		{
			camera = GameSystemsBootstrapper.Instance.MainCamera;
		}
		if (camera == null)
		{
			camera = Camera.main;
		}
		if (!(camera != null))
		{
			return this._cachedForward;
		}
		return camera.transform.forward;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00007E59 File Offset: 0x00006059
	public bool IsVisible(Vector3 point, float minRadius, float fovAngle, float fovDistance)
	{
		return this.GetDistance(point) <= minRadius || this.IsInFOV(point, fovAngle, fovDistance);
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00007E71 File Offset: 0x00006071
	public Vector2Int WorldToCell(Vector3 worldPos)
	{
		worldPos = WorldOrigin.ToLogical(worldPos);
		return new Vector2Int(Mathf.RoundToInt(worldPos.x / this._cellSize), Mathf.RoundToInt(worldPos.z / this._cellSize));
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00007EA4 File Offset: 0x000060A4
	public Vector3 CellToWorld(Vector2Int cell)
	{
		return WorldOrigin.ToUnity(new Vector3((float)cell.x * this._cellSize + this._cellSize * 0.5f, 0f, (float)cell.y * this._cellSize + this._cellSize * 0.5f));
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00007EF8 File Offset: 0x000060F8
	public float GetCellDistance(Vector2Int cell)
	{
		if (this.player == null)
		{
			return float.PositiveInfinity;
		}
		this.UpdateCache();
		return Vector2Int.Distance(this._cachedCell, cell);
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00007F20 File Offset: 0x00006120
	public bool IsCellWithinRadius(Vector2Int cell, int radiusCells)
	{
		return this.GetCellDistance(cell) <= (float)radiusCells;
	}

	// Token: 0x040000A5 RID: 165
	[Header("Player Reference")]
	[SerializeField]
	private Transform player;

	// Token: 0x040000A6 RID: 166
	[Header("Default Settings")]
	[SerializeField]
	private float defaultFOVAngle = 120f;

	// Token: 0x040000A7 RID: 167
	[SerializeField]
	private float defaultFOVDistance = 100f;

	// Token: 0x040000A8 RID: 168
	private Vector3 _cachedPosition;

	// Token: 0x040000A9 RID: 169
	private Vector3 _cachedForward;

	// Token: 0x040000AA RID: 170
	private Vector2Int _cachedCell;

	// Token: 0x040000AB RID: 171
	private float _cellSize = 60f;

	// Token: 0x040000AC RID: 172
	private int _lastUpdateFrame = -1;

	// Token: 0x040000AE RID: 174
	private Vector2Int _lastCell;
}
