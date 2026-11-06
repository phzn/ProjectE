using System;
using System.Threading.Tasks;
using Features.Detection;
using Features.MainMenu;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000E1 RID: 225
public class TroupeSpawnTest : MonoBehaviour
{
	// Token: 0x06000901 RID: 2305 RVA: 0x00041A5B File Offset: 0x0003FC5B
	private void Update()
	{
		if (GameplayInput.GetKeyDown(this.spawnKey))
		{
			this.SpawnAsync();
		}
		if (GameplayInput.GetKeyDown(this.despawnKey))
		{
			this.DespawnLast();
		}
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x00041A84 File Offset: 0x0003FC84
	private async Task SpawnAsync()
	{
		ReporterTroupeManager instance = ReporterTroupeManager.Instance;
		if (instance == null)
		{
			Debug.LogWarning("[TroupeSpawnTest] ReporterTroupeManager.Instance è null. Aggiungi il componente alla scena.");
		}
		else
		{
			Transform transform = ((this.spawnAroundTarget != null) ? this.spawnAroundTarget : ((Camera.main != null) ? Camera.main.transform : base.transform));
			Vector3 vector = Quaternion.Euler(0f, this.spawnAngleOffset, 0f) * transform.forward;
			Vector3 vector2 = transform.position + vector.normalized * this.spawnDistance;
			Quaternion quaternion = Quaternion.LookRotation(-vector, Vector3.up);
			Debug.Log(string.Format("[TroupeSpawnTest] Spawn troupe at {0} (active={1}/{2})", vector2, instance.ActiveTroupeCount, instance.maxActiveTroupes));
			ReporterTroupe reporterTroupe = await instance.SpawnTroupe(vector2, quaternion);
			this._lastSpawned = reporterTroupe;
			if (this._lastSpawned != null)
			{
				Debug.Log("[TroupeSpawnTest] Troupe '" + this._lastSpawned.name + "' creata.");
			}
			else
			{
				Debug.LogWarning("[TroupeSpawnTest] Spawn fallito (limite raggiunto, config invalida o spawner null).");
			}
		}
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00041AC8 File Offset: 0x0003FCC8
	private void DespawnLast()
	{
		ReporterTroupeManager instance = ReporterTroupeManager.Instance;
		if (instance == null || this._lastSpawned == null)
		{
			return;
		}
		instance.DespawnTroupe(this._lastSpawned, false);
		this._lastSpawned = null;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00041B08 File Offset: 0x0003FD08
	private void OnGUI()
	{
		if (!this.showHUD)
		{
			return;
		}
		if (!GameSettingsService.ShowGameplayUI && SceneManager.GetActiveScene().name != "MainMenu")
		{
			return;
		}
		ReporterTroupeManager instance = ReporterTroupeManager.Instance;
		int num = ((instance != null) ? instance.ActiveTroupeCount : (-1));
		int num2 = ((instance != null) ? instance.maxActiveTroupes : (-1));
		GUI.Label(new Rect(10f, 30f, 600f, 20f), string.Format("[{0}] Spawn troupe   [{1}] Despawn last   Active: {2}/{3}", new object[] { this.spawnKey, this.despawnKey, num, num2 }));
	}

	// Token: 0x04000818 RID: 2072
	[Header("Spawn Settings")]
	[Tooltip("Player o riferimento attorno a cui spawnare la troupe. Se null, prova Camera.main.transform.")]
	[SerializeField]
	private Transform spawnAroundTarget;

	// Token: 0x04000819 RID: 2073
	[Tooltip("Distanza in metri davanti al target dove spawnare il centro della troupe.")]
	[SerializeField]
	private float spawnDistance = 8f;

	// Token: 0x0400081A RID: 2074
	[Tooltip("Direzione orizzontale aggiuntiva (gradi) rispetto al forward del target.")]
	[SerializeField]
	private float spawnAngleOffset;

	// Token: 0x0400081B RID: 2075
	[Header("Keybinds")]
	[SerializeField]
	private KeyCode spawnKey = KeyCode.T;

	// Token: 0x0400081C RID: 2076
	[SerializeField]
	private KeyCode despawnKey = KeyCode.Y;

	// Token: 0x0400081D RID: 2077
	[Header("Debug")]
	[SerializeField]
	private bool showHUD = true;

	// Token: 0x0400081E RID: 2078
	private ReporterTroupe _lastSpawned;
}
