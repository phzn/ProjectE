using System;
using System.Collections;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Invector
{
	// Token: 0x02000452 RID: 1106
	[vClassHeader("Simple GameController Example", true, "icon_v2", false, "", openClose = false)]
	public class vGameController : vMonoBehaviour
	{
		// Token: 0x06001B92 RID: 7058 RVA: 0x000AD998 File Offset: 0x000ABB98
		protected virtual void Start()
		{
			if (vGameController.instance == null)
			{
				vGameController.instance = this;
				if (this.dontDestroyOnLoad)
				{
					Object.DontDestroyOnLoad(base.gameObject);
				}
				base.gameObject.name = base.gameObject.name + " Instance";
				SceneManager.sceneLoaded += this.OnLevelFinishedLoading;
				if (this.displayInfoInFadeText && vHUDController.instance)
				{
					vHUDController.instance.ShowText("Init Scene");
				}
				this.FindPlayer();
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000ADA34 File Offset: 0x000ABC34
		public virtual void ShowCursor(bool value)
		{
			Cursor.visible = value;
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x000ADA3C File Offset: 0x000ABC3C
		public virtual void LockCursor(bool value)
		{
			if (value)
			{
				Cursor.lockState = CursorLockMode.Locked;
				return;
			}
			Cursor.lockState = CursorLockMode.None;
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x000ADA50 File Offset: 0x000ABC50
		protected virtual void OnCharacterDead(GameObject _gameObject)
		{
			this.oldPlayer = _gameObject;
			if (this.playerPrefab != null)
			{
				base.StartCoroutine(this.RespawnRoutine());
				return;
			}
			if (this.displayInfoInFadeText && vHUDController.instance)
			{
				vHUDController.instance.ShowText("Restarting Scene...");
			}
			base.Invoke("ResetScene", this.respawnTimer);
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x000ADAB4 File Offset: 0x000ABCB4
		protected virtual IEnumerator RespawnRoutine()
		{
			yield return new WaitForSeconds(this.respawnTimer);
			if (this.playerPrefab != null && this.spawnPoint != null)
			{
				if (this.oldPlayer != null && this.destroyBodyAfterDead)
				{
					if (this.displayInfoInFadeText && vHUDController.instance)
					{
						vHUDController.instance.ShowText("Player destroyed: " + this.oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
					}
					Object.Destroy(this.oldPlayer);
				}
				else
				{
					if (this.displayInfoInFadeText && vHUDController.instance)
					{
						vHUDController.instance.ShowText("Remove Player Components: " + this.oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
					}
					this.DestroyPlayerComponents(this.oldPlayer);
				}
				yield return new WaitForEndOfFrame();
				this.currentPlayer = Object.Instantiate<GameObject>(this.playerPrefab, this.spawnPoint.position, this.spawnPoint.rotation);
				this.currentController = this.currentPlayer.GetComponent<vThirdPersonController>();
				this.currentController.onDead.AddListener(new UnityAction<GameObject>(this.OnCharacterDead));
				if (this.displayInfoInFadeText && vHUDController.instance)
				{
					vHUDController.instance.ShowText("Respawn player: " + this.currentPlayer.name.Replace("(Clone)", ""));
				}
				this.OnReloadGame.Invoke();
				this.onSpawn.Invoke();
			}
			yield break;
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000ADAC4 File Offset: 0x000ABCC4
		protected virtual void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
		{
			this.FindPlayer();
			if (this.currentController == null)
			{
				return;
			}
			if (this.currentController.currentHealth > 0f)
			{
				if (this.displayInfoInFadeText && vHUDController.instance)
				{
					vHUDController.instance.ShowText("Load Scene: " + scene.name);
				}
				return;
			}
			if (this.displayInfoInFadeText && vHUDController.instance)
			{
				vHUDController.instance.ShowText("Reload Scene");
			}
			this.OnReloadGame.Invoke();
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000ADB58 File Offset: 0x000ABD58
		protected virtual void FindPlayer()
		{
			vThirdPersonController vThirdPersonController = Object.FindObjectOfType<vThirdPersonController>();
			if (vThirdPersonController)
			{
				this.currentPlayer = vThirdPersonController.gameObject;
				this.currentController = vThirdPersonController;
				vThirdPersonController.onDead.AddListener(new UnityAction<GameObject>(this.OnCharacterDead));
				if (this.displayInfoInFadeText && vHUDController.instance)
				{
					vHUDController.instance.ShowText("Found player: " + this.currentPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
					return;
				}
			}
			else if (this.currentPlayer == null && this.playerPrefab != null && this.spawnPoint != null)
			{
				this.SpawnAtPoint(this.spawnPoint);
			}
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000ADC2C File Offset: 0x000ABE2C
		protected virtual void DestroyPlayerComponents(GameObject target)
		{
			if (!target)
			{
				return;
			}
			MonoBehaviour[] componentsInChildren = target.GetComponentsInChildren<MonoBehaviour>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.Destroy(componentsInChildren[i]);
			}
			Collider component = target.GetComponent<Collider>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			Rigidbody component2 = target.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				Object.Destroy(component2);
			}
			Animator component3 = target.GetComponent<Animator>();
			if (component3 != null)
			{
				Object.Destroy(component3);
			}
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000ADCA6 File Offset: 0x000ABEA6
		public virtual void SetSpawnPoint(Transform newSpawnPoint)
		{
			this.spawnPoint = newSpawnPoint;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000ADCAF File Offset: 0x000ABEAF
		public void SetPlayerPrefab(GameObject prefab)
		{
			this.playerPrefab = prefab;
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000ADCB8 File Offset: 0x000ABEB8
		public virtual void SpawnAtPoint(Transform targetPoint)
		{
			if (this.playerPrefab != null)
			{
				if (this.oldPlayer != null && this.destroyBodyAfterDead)
				{
					if (this.displayInfoInFadeText && vHUDController.instance)
					{
						vHUDController.instance.ShowText("Player destroyed: " + this.oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
					}
					Object.Destroy(this.oldPlayer);
				}
				else if (this.oldPlayer != null)
				{
					if (this.displayInfoInFadeText && vHUDController.instance)
					{
						vHUDController.instance.ShowText("Remove Player Components: " + this.oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
					}
					this.DestroyPlayerComponents(this.oldPlayer);
				}
				this.currentPlayer = Object.Instantiate<GameObject>(this.playerPrefab, targetPoint.position, targetPoint.rotation);
				this.currentController = this.currentPlayer.GetComponent<vThirdPersonController>();
				this.currentController.onDead.AddListener(new UnityAction<GameObject>(this.OnCharacterDead));
				this.OnReloadGame.Invoke();
				if (this.displayInfoInFadeText && vHUDController.instance)
				{
					vHUDController.instance.ShowText("Spawn player: " + this.currentPlayer.name.Replace("(Clone)", ""));
				}
			}
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000ADE50 File Offset: 0x000AC050
		public virtual void SpawnPlayer(GameObject prefab)
		{
			if (prefab != null && this.spawnPoint != null)
			{
				Transform transform = this.spawnPoint;
				if (this.oldPlayer != null && this.destroyBodyAfterDead)
				{
					if (this.displayInfoInFadeText && vHUDController.instance)
					{
						vHUDController.instance.ShowText("Player destroyed: " + this.oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
					}
					Object.Destroy(this.oldPlayer);
				}
				else if (this.oldPlayer != null)
				{
					if (this.displayInfoInFadeText && vHUDController.instance)
					{
						vHUDController.instance.ShowText("Remove Player Components: " + this.oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
					}
					this.DestroyPlayerComponents(this.oldPlayer);
				}
				this.currentPlayer = Object.Instantiate<GameObject>(prefab, transform.position, transform.rotation);
				this.currentController = this.currentPlayer.GetComponent<vThirdPersonController>();
				this.currentController.onDead.AddListener(new UnityAction<GameObject>(this.OnCharacterDead));
				this.OnReloadGame.Invoke();
				if (this.displayInfoInFadeText && vHUDController.instance)
				{
					vHUDController.instance.ShowText("Spawn player: " + this.currentPlayer.name.Replace("(Clone)", ""));
				}
			}
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000ADFF4 File Offset: 0x000AC1F4
		public virtual void ResetScene()
		{
			if (this.oldPlayer)
			{
				this.DestroyPlayerComponents(this.oldPlayer);
			}
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			if (this.oldPlayer && this.destroyBodyAfterDead)
			{
				Object.Destroy(this.oldPlayer);
			}
		}

		// Token: 0x04001CD9 RID: 7385
		[vHelpBox("Assign your Character Prefab to be instantiate at the SpawnPoint, leave it unassigned to Restart the Scene instead", vHelpBoxAttribute.MessageType.None)]
		public GameObject playerPrefab;

		// Token: 0x04001CDA RID: 7386
		[vHelpBox("Assign a empty transform to spawn the Player to a specific location", vHelpBoxAttribute.MessageType.None)]
		public Transform spawnPoint;

		// Token: 0x04001CDB RID: 7387
		[vHelpBox("Time to wait until the scene restart or the player will be spawned again", vHelpBoxAttribute.MessageType.None)]
		public float respawnTimer = 4f;

		// Token: 0x04001CDC RID: 7388
		[vHelpBox("Check this if you want to destroy the dead body after the respawn", vHelpBoxAttribute.MessageType.None)]
		public bool destroyBodyAfterDead;

		// Token: 0x04001CDD RID: 7389
		[vHelpBox("Display a message using the FadeText UI", vHelpBoxAttribute.MessageType.None)]
		public bool displayInfoInFadeText = true;

		// Token: 0x04001CDE RID: 7390
		[HideInInspector]
		public vGameController.OnRealoadGame OnReloadGame = new vGameController.OnRealoadGame();

		// Token: 0x04001CDF RID: 7391
		[HideInInspector]
		public GameObject currentPlayer;

		// Token: 0x04001CE0 RID: 7392
		private vThirdPersonController currentController;

		// Token: 0x04001CE1 RID: 7393
		public static vGameController instance;

		// Token: 0x04001CE2 RID: 7394
		private GameObject oldPlayer;

		// Token: 0x04001CE3 RID: 7395
		public UnityEvent onSpawn;

		// Token: 0x04001CE4 RID: 7396
		public bool dontDestroyOnLoad = true;

		// Token: 0x02000453 RID: 1107
		[Serializable]
		public class OnRealoadGame : UnityEvent
		{
		}
	}
}
