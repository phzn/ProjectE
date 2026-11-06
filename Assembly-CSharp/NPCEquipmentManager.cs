using System;
using System.Collections.Generic;
using Features.NPC;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class NPCEquipmentManager : MonoBehaviour
{
	// Token: 0x0600076B RID: 1899 RVA: 0x00034493 File Offset: 0x00032693
	internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
	{
		if (field == UniversityGameplayField.NPCEquipmentManager_actionRegistry)
		{
			return this.actionRegistry;
		}
		if (field != UniversityGameplayField.NPCEquipmentManager_sharedConfig)
		{
			throw new InvalidOperationException("Unsupported NPCEquipmentManager POI binding: " + field.ToString());
		}
		return this.sharedConfig;
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x000344CC File Offset: 0x000326CC
	internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
	{
		if (field == UniversityGameplayField.NPCEquipmentManager_actionRegistry)
		{
			this.actionRegistry = (NPCActionRegistry)asset;
			return;
		}
		if (field != UniversityGameplayField.NPCEquipmentManager_sharedConfig)
		{
			throw new InvalidOperationException("Unsupported NPCEquipmentManager POI binding: " + field.ToString());
		}
		this.sharedConfig = (NPCEquipmentConfig)asset;
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x0003451B File Offset: 0x0003271B
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x00034524 File Offset: 0x00032724
	public void Initialize()
	{
		if (this._isInitialized)
		{
			return;
		}
		if (this.sharedConfig != null)
		{
			this.LoadFromConfig(this.sharedConfig);
		}
		if (this.autoFindProps)
		{
			this.AutoFindProps();
		}
		this.BuildCache();
		this.DeactivateAllProps();
		this._isInitialized = true;
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x00034578 File Offset: 0x00032778
	public void ReInitializeAfterMerge()
	{
		bool flag = this.enableDebugLogs || NPCDebug.Verbose;
		if (flag)
		{
			Debug.Log("[NPCEquipmentManager] " + base.gameObject.name + ": ReInitializeAfterMerge called - re-finding props after mesh optimization");
		}
		if (flag)
		{
			Debug.Log(string.Format("[NPCEquipmentManager] {0}: equipmentSlots.Count = {1}, autoFindProps = {2}", base.gameObject.name, this.equipmentSlots.Count, this.autoFindProps));
		}
		foreach (NPCEquipmentManager.EquipmentSlot equipmentSlot in this.equipmentSlots)
		{
			if (flag)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[NPCEquipmentManager] ",
					base.gameObject.name,
					": Clearing slot '",
					equipmentSlot.propName,
					"', was: ",
					(equipmentSlot.propObject != null) ? equipmentSlot.propObject.name : "null"
				}));
			}
			equipmentSlot.propObject = null;
		}
		if (this.autoFindProps)
		{
			this.AutoFindProps();
		}
		this.BuildCache();
		if (this._propsLocked)
		{
			foreach (NPCEquipmentManager.EquipmentSlot equipmentSlot2 in this.equipmentSlots)
			{
				if (equipmentSlot2.propObject != null)
				{
					equipmentSlot2.propObject.SetActive(false);
				}
			}
			this._activeProps.Clear();
		}
		else
		{
			this.UpdateActiveProps();
		}
		int num = 0;
		foreach (NPCEquipmentManager.EquipmentSlot equipmentSlot3 in this.equipmentSlots)
		{
			if (equipmentSlot3.propObject != null)
			{
				num++;
				if (flag)
				{
					string[] array = new string[6];
					array[0] = "[NPCEquipmentManager] ";
					array[1] = base.gameObject.name;
					array[2] = ": Found prop '";
					array[3] = equipmentSlot3.propName;
					array[4] = "' at ";
					int num2 = 5;
					Transform parent = equipmentSlot3.propObject.transform.parent;
					array[num2] = ((parent != null) ? parent.name : null) ?? "root";
					Debug.Log(string.Concat(array));
				}
			}
			else
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[NPCEquipmentManager] ",
					base.gameObject.name,
					": Prop '",
					equipmentSlot3.propName,
					"' NOT FOUND after merge!"
				}));
			}
		}
		if (flag)
		{
			Debug.Log(string.Format("[NPCEquipmentManager] {0}: ReInitializeAfterMerge complete: {1}/{2} props found, cache size: {3}", new object[]
			{
				base.gameObject.name,
				num,
				this.equipmentSlots.Count,
				this._slotsByName.Count
			}));
		}
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x00034880 File Offset: 0x00032A80
	private void LoadFromConfig(NPCEquipmentConfig config)
	{
		this.equipmentSlots.Clear();
		foreach (NPCEquipmentConfig.EquipmentEntry equipmentEntry in config.equipmentEntries)
		{
			this.equipmentSlots.Add(new NPCEquipmentManager.EquipmentSlot
			{
				propName = equipmentEntry.propName,
				propObject = null,
				activeForActionIndices = new List<int>(equipmentEntry.activeForActionIndices),
				activeForLocomotion = equipmentEntry.activeForLocomotion
			});
		}
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x00034918 File Offset: 0x00032B18
	private void AutoFindProps()
	{
		foreach (NPCEquipmentManager.EquipmentSlot equipmentSlot in this.equipmentSlots)
		{
			if (equipmentSlot.propObject == null && !string.IsNullOrEmpty(equipmentSlot.propName))
			{
				Transform transform = this.FindDeepChild(base.transform, equipmentSlot.propName);
				if (transform != null)
				{
					equipmentSlot.propObject = transform.gameObject;
				}
			}
		}
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x000349A8 File Offset: 0x00032BA8
	private void BuildCache()
	{
		this._slotsByName.Clear();
		this._slotsByActionIndex.Clear();
		foreach (NPCEquipmentManager.EquipmentSlot equipmentSlot in this.equipmentSlots)
		{
			if (!string.IsNullOrEmpty(equipmentSlot.propName))
			{
				this._slotsByName[equipmentSlot.propName] = equipmentSlot;
				foreach (NPCActionDefinition npcactionDefinition in equipmentSlot.activeForActions)
				{
					if (!(npcactionDefinition == null))
					{
						int animatorIndex = npcactionDefinition.animatorIndex;
						if (!this._slotsByActionIndex.ContainsKey(animatorIndex))
						{
							this._slotsByActionIndex[animatorIndex] = new List<NPCEquipmentManager.EquipmentSlot>();
						}
						if (!this._slotsByActionIndex[animatorIndex].Contains(equipmentSlot))
						{
							this._slotsByActionIndex[animatorIndex].Add(equipmentSlot);
						}
					}
				}
				foreach (int num in equipmentSlot.activeForActionIndices)
				{
					if (!this._slotsByActionIndex.ContainsKey(num))
					{
						this._slotsByActionIndex[num] = new List<NPCEquipmentManager.EquipmentSlot>();
					}
					if (!this._slotsByActionIndex[num].Contains(equipmentSlot))
					{
						this._slotsByActionIndex[num].Add(equipmentSlot);
					}
				}
			}
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x00034B74 File Offset: 0x00032D74
	public void SetAction(int actionIndex)
	{
		this.Log(string.Format("SetAction({0}): changing from {1}, propsLocked={2}", actionIndex, this._currentActionIndex, this._propsLocked));
		this._currentActionIndex = actionIndex;
		if (!this._propsLocked)
		{
			this.UpdateActiveProps();
		}
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x00034BC2 File Offset: 0x00032DC2
	public void SetAction(NPCInterestPoint.ActionType action)
	{
		this.SetAction((int)action);
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x00034BCB File Offset: 0x00032DCB
	public void SetAction(NPCActionDefinition action)
	{
		if (action == null)
		{
			this.SetAction(0);
			return;
		}
		this.SetAction(action.animatorIndex);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00034BEC File Offset: 0x00032DEC
	public void SetMoving(bool isMoving)
	{
		if (this._isMoving == isMoving)
		{
			return;
		}
		this.Log(string.Format("SetMoving({0}), propsLocked={1}", isMoving, this._propsLocked));
		this._isMoving = isMoving;
		if (!this._propsLocked && this._currentActionIndex > 0)
		{
			this.UpdateActiveProps();
		}
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00034C42 File Offset: 0x00032E42
	public float GetCurrentAttentionLevel()
	{
		if (this.actionRegistry == null)
		{
			return 0.5f;
		}
		return this.actionRegistry.GetAttentionLevel(this._currentActionIndex);
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00034C6C File Offset: 0x00032E6C
	public void ActivateAndLockProp(string propName)
	{
		this.Log(string.Format("ActivateAndLockProp('{0}') called. CurrentAction={1}, Slots: {2}, Cache: {3}, AlreadyLocked: {4}", new object[]
		{
			propName,
			this._currentActionIndex,
			this.equipmentSlots.Count,
			this._slotsByName.Count,
			this._manuallyLockedProps.Contains(propName)
		}));
		GameObject gameObject = null;
		NPCEquipmentManager.EquipmentSlot equipmentSlot = null;
		this._slotsByName.TryGetValue(propName, out equipmentSlot);
		if (equipmentSlot != null && equipmentSlot.propObject != null)
		{
			if (this.IsValidPropReference(equipmentSlot.propObject))
			{
				gameObject = equipmentSlot.propObject;
				this.Log("Using cached prop '" + propName + "'");
			}
			else
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[NPCEquipmentManager] ",
					base.gameObject.name,
					": Cached prop '",
					propName,
					"' is stale/invalid, re-searching..."
				}));
				equipmentSlot.propObject = null;
				gameObject = null;
			}
		}
		if (gameObject == null)
		{
			gameObject = this.SearchAndCacheProp(propName, equipmentSlot);
		}
		if (gameObject != null)
		{
			if (!this.TryActivateProp(gameObject, propName))
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[NPCEquipmentManager] ",
					base.gameObject.name,
					": Cached prop '",
					propName,
					"' is zombie, re-searching..."
				}));
				if (equipmentSlot != null)
				{
					equipmentSlot.propObject = null;
				}
				this._slotsByName.Remove(propName);
				gameObject = this.SearchAndCacheProp(propName, equipmentSlot);
				if (gameObject != null)
				{
					this.TryActivateProp(gameObject, propName);
					return;
				}
			}
		}
		else
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"[NPCEquipmentManager] ",
				base.gameObject.name,
				": FAILED - Prop '",
				propName,
				"' not found in hierarchy!"
			}));
		}
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00034E40 File Offset: 0x00033040
	private GameObject SearchAndCacheProp(string propName, NPCEquipmentManager.EquipmentSlot existingSlot)
	{
		this.Log("Searching hierarchy for prop '" + propName + "'...");
		Transform transform = this.FindDeepChild(base.transform, propName);
		if (transform != null)
		{
			GameObject gameObject = transform.gameObject;
			string[] array = new string[5];
			array[0] = "Found '";
			array[1] = propName;
			array[2] = "' at '";
			int num = 3;
			Transform parent = transform.parent;
			array[num] = ((parent != null) ? parent.name : null);
			array[4] = "'";
			this.Log(string.Concat(array));
			if (existingSlot != null)
			{
				existingSlot.propObject = gameObject;
			}
			else
			{
				NPCEquipmentManager.EquipmentSlot equipmentSlot = new NPCEquipmentManager.EquipmentSlot
				{
					propName = propName,
					propObject = gameObject
				};
				this.equipmentSlots.Add(equipmentSlot);
				this._slotsByName[propName] = equipmentSlot;
			}
			return gameObject;
		}
		return null;
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00034F04 File Offset: 0x00033104
	private bool TryActivateProp(GameObject propObject, string propName)
	{
		this._manuallyLockedProps.Add(propName);
		this.EnsureParentHierarchyActive(propObject.transform);
		propObject.SetActive(true);
		if (!propObject.activeSelf)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"[NPCEquipmentManager] ",
				base.gameObject.name,
				": SetActive(true) had no effect on '",
				propName,
				"' - object is zombie!"
			}));
			return false;
		}
		if (!this._activeProps.Contains(propObject))
		{
			this._activeProps.Add(propObject);
		}
		bool activeInHierarchy = propObject.activeInHierarchy;
		string text = "Activated prop '{0}' (GO: {1}, activeSelf: {2}, activeInHierarchy: {3}, parent: {4})";
		object[] array = new object[5];
		array[0] = propName;
		array[1] = propObject.name;
		array[2] = propObject.activeSelf;
		array[3] = activeInHierarchy;
		int num = 4;
		Transform parent = propObject.transform.parent;
		array[num] = ((parent != null) ? parent.name : null);
		this.Log(string.Format(text, array));
		if (!activeInHierarchy)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"[NPCEquipmentManager] ",
				base.gameObject.name,
				": Prop '",
				propName,
				"' activeSelf=true but activeInHierarchy=false"
			}));
			this.LogParentHierarchy(propObject.transform);
		}
		return true;
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00035034 File Offset: 0x00033234
	private bool IsValidPropReference(GameObject propObject)
	{
		if (propObject == null)
		{
			return false;
		}
		Transform transform = propObject.transform;
		while (transform != null)
		{
			if (transform == base.transform)
			{
				return true;
			}
			transform = transform.parent;
		}
		return false;
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00035078 File Offset: 0x00033278
	private void EnsureParentHierarchyActive(Transform propTransform)
	{
		Transform transform = propTransform.parent;
		while (transform != null && transform != base.transform)
		{
			if (!transform.gameObject.activeSelf)
			{
				this.Log("Activating inactive parent '" + transform.name + "' for prop visibility");
				transform.gameObject.SetActive(true);
			}
			transform = transform.parent;
		}
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x000350E0 File Offset: 0x000332E0
	private void LogParentHierarchy(Transform propTransform)
	{
		if (!this.enableDebugLogs && !NPCDebug.Verbose)
		{
			return;
		}
		Transform transform = propTransform.parent;
		int num = 0;
		while (transform != null && num < 10)
		{
			Debug.Log(string.Format("[NPCEquipmentManager] Parent[{0}]: '{1}' activeSelf={2}", num, transform.name, transform.gameObject.activeSelf));
			transform = transform.parent;
			num++;
		}
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0003514C File Offset: 0x0003334C
	public void UnlockProp(string propName)
	{
		if (this._manuallyLockedProps.Remove(propName))
		{
			this.Log("Unlocked prop: " + propName);
			this.UpdateActiveProps();
		}
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00035174 File Offset: 0x00033374
	public void UnlockAllProps()
	{
		if (this._propsLocked)
		{
			this.Log("UnlockAllProps() skipped - props are locked by FSM state");
			return;
		}
		if (this._manuallyLockedProps.Count > 0)
		{
			this.Log(string.Format("UnlockAllProps() - clearing {0} locked props: [{1}]", this._manuallyLockedProps.Count, string.Join(", ", this._manuallyLockedProps)));
		}
		this._manuallyLockedProps.Clear();
		if (this._currentActionIndex > 0)
		{
			this.UpdateActiveProps();
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x000351ED File Offset: 0x000333ED
	public void ActivateProp(string propName)
	{
		this.ActivateAndLockProp(propName);
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x000351F8 File Offset: 0x000333F8
	public void DeactivateProp(string propName)
	{
		this.UnlockProp(propName);
		NPCEquipmentManager.EquipmentSlot equipmentSlot;
		if (this._slotsByName.TryGetValue(propName, out equipmentSlot) && equipmentSlot.propObject != null)
		{
			equipmentSlot.propObject.SetActive(false);
			this._activeProps.Remove(equipmentSlot.propObject);
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00035248 File Offset: 0x00033448
	public void ActivateProps(List<string> propNames)
	{
		if (propNames == null)
		{
			return;
		}
		foreach (string text in propNames)
		{
			this.ActivateAndLockProp(text);
		}
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0003529C File Offset: 0x0003349C
	public void DeactivateAllProps()
	{
		if (this._manuallyLockedProps.Count > 0 || this._activeProps.Count > 0 || this._propsLocked)
		{
			this.Log(string.Format("DeactivateAllProps() - clearing {0} locked, {1} active, propsLocked was {2}", this._manuallyLockedProps.Count, this._activeProps.Count, this._propsLocked));
		}
		this._manuallyLockedProps.Clear();
		this._propsLocked = false;
		foreach (NPCEquipmentManager.EquipmentSlot equipmentSlot in this.equipmentSlots)
		{
			if (equipmentSlot.propObject != null)
			{
				equipmentSlot.propObject.SetActive(false);
			}
		}
		this._activeProps.Clear();
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00035380 File Offset: 0x00033580
	public void LockPropsDeactivated()
	{
		this.DeactivateAllProps();
		this._propsLocked = true;
		this.Log("Props locked (deactivated)");
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x0003539A File Offset: 0x0003359A
	public void UnlockProps()
	{
		this._propsLocked = false;
		this.Log("Props unlocked");
		if (this._currentActionIndex > 0)
		{
			this.UpdateActiveProps();
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000786 RID: 1926 RVA: 0x000353BD File Offset: 0x000335BD
	public bool ArePropsLocked
	{
		get
		{
			return this._propsLocked;
		}
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x000353C8 File Offset: 0x000335C8
	private void UpdateActiveProps()
	{
		foreach (NPCEquipmentManager.EquipmentSlot equipmentSlot in this.equipmentSlots)
		{
			if (equipmentSlot.propObject != null && !this._manuallyLockedProps.Contains(equipmentSlot.propName))
			{
				equipmentSlot.propObject.SetActive(false);
				this._activeProps.Remove(equipmentSlot.propObject);
			}
		}
		List<NPCEquipmentManager.EquipmentSlot> list;
		if (this._slotsByActionIndex.TryGetValue(this._currentActionIndex, out list))
		{
			this.Log(string.Format("UpdateActiveProps: Found {0} slots for action {1}, isMoving={2}", list.Count, this._currentActionIndex, this._isMoving));
			using (List<NPCEquipmentManager.EquipmentSlot>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NPCEquipmentManager.EquipmentSlot equipmentSlot2 = enumerator.Current;
					if (equipmentSlot2.propObject == null)
					{
						this.Log("  WARNING: Slot '" + equipmentSlot2.propName + "' has null propObject! Attempting re-find...");
						Transform transform = this.FindDeepChild(base.transform, equipmentSlot2.propName);
						if (!(transform != null))
						{
							this.Log("  FAILED to re-find prop '" + equipmentSlot2.propName + "'");
							continue;
						}
						equipmentSlot2.propObject = transform.gameObject;
						string text = "  Re-found prop '";
						string propName = equipmentSlot2.propName;
						string text2 = "' at ";
						Transform parent = transform.parent;
						this.Log(text + propName + text2 + ((parent != null) ? parent.name : null));
					}
					if (!this._isMoving || equipmentSlot2.activeForLocomotion)
					{
						equipmentSlot2.propObject.SetActive(true);
						if (!this._activeProps.Contains(equipmentSlot2.propObject))
						{
							this._activeProps.Add(equipmentSlot2.propObject);
						}
						this.Log(string.Format("  Activated prop: {0} (locomotion={1})", equipmentSlot2.propName, equipmentSlot2.activeForLocomotion));
					}
					else
					{
						this.Log("  Skipped prop: " + equipmentSlot2.propName + " (locomotion disabled while moving)");
					}
				}
				goto IL_022A;
			}
		}
		this.Log(string.Format("UpdateActiveProps: No slots configured for action {0}", this._currentActionIndex));
		IL_022A:
		if (this._manuallyLockedProps.Count > 0)
		{
			this.Log(string.Format("UpdateActiveProps: Re-activating {0} locked props: [{1}]", this._manuallyLockedProps.Count, string.Join(", ", this._manuallyLockedProps)));
		}
		foreach (string text3 in this._manuallyLockedProps)
		{
			NPCEquipmentManager.EquipmentSlot equipmentSlot3;
			if (this._slotsByName.TryGetValue(text3, out equipmentSlot3) && equipmentSlot3.propObject != null && !equipmentSlot3.propObject.activeSelf)
			{
				equipmentSlot3.propObject.SetActive(true);
				if (!this._activeProps.Contains(equipmentSlot3.propObject))
				{
					this._activeProps.Add(equipmentSlot3.propObject);
				}
				this.Log("  Re-activated locked prop: " + text3);
			}
		}
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00035730 File Offset: 0x00033930
	private Transform FindDeepChild(Transform parent, string name)
	{
		foreach (object obj in parent)
		{
			Transform transform = (Transform)obj;
			if (transform.name.Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				return transform;
			}
			Transform transform2 = this.FindDeepChild(transform, name);
			if (transform2 != null)
			{
				return transform2;
			}
		}
		return null;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x000357AC File Offset: 0x000339AC
	private void OnDisable()
	{
		this.DeactivateAllProps();
		this._currentActionIndex = 0;
		this._isMoving = false;
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x000357C2 File Offset: 0x000339C2
	private void Log(string message)
	{
		if (this.enableDebugLogs || NPCDebug.Verbose)
		{
			Debug.Log("[NPCEquipmentManager] " + base.gameObject.name + ": " + message);
		}
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x000357F4 File Offset: 0x000339F4
	public string[] GetActivePropsNames()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, NPCEquipmentManager.EquipmentSlot> keyValuePair in this._slotsByName)
		{
			if (keyValuePair.Value.propObject != null && keyValuePair.Value.propObject.activeSelf)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list.ToArray();
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x0600078C RID: 1932 RVA: 0x00035880 File Offset: 0x00033A80
	public int CurrentActionIndex
	{
		get
		{
			return this._currentActionIndex;
		}
	}

	// Token: 0x0400065D RID: 1629
	[Header("Action Registry")]
	[Tooltip("Registry centralizzato delle azioni (per lookup e validazione)")]
	public NPCActionRegistry actionRegistry;

	// Token: 0x0400065E RID: 1630
	[Header("Equipment Configuration")]
	[Tooltip("Configurazione condivisa (opzionale). Se assegnata, sovrascrive equipmentSlots locale.")]
	public NPCEquipmentConfig sharedConfig;

	// Token: 0x0400065F RID: 1631
	[Header("Equipment Slots (Local)")]
	[Tooltip("Lista di tutti i prop equipaggiabili dall'NPC. Ignorata se sharedConfig è assegnato.")]
	public List<NPCEquipmentManager.EquipmentSlot> equipmentSlots = new List<NPCEquipmentManager.EquipmentSlot>();

	// Token: 0x04000660 RID: 1632
	[Header("Auto-Setup")]
	[Tooltip("Se true, cerca automaticamente i prop per nome nei children")]
	public bool autoFindProps = true;

	// Token: 0x04000661 RID: 1633
	[Header("Debug")]
	public bool enableDebugLogs;

	// Token: 0x04000662 RID: 1634
	private Dictionary<string, NPCEquipmentManager.EquipmentSlot> _slotsByName = new Dictionary<string, NPCEquipmentManager.EquipmentSlot>();

	// Token: 0x04000663 RID: 1635
	private Dictionary<int, List<NPCEquipmentManager.EquipmentSlot>> _slotsByActionIndex = new Dictionary<int, List<NPCEquipmentManager.EquipmentSlot>>();

	// Token: 0x04000664 RID: 1636
	private int _currentActionIndex;

	// Token: 0x04000665 RID: 1637
	private bool _isMoving;

	// Token: 0x04000666 RID: 1638
	private List<GameObject> _activeProps = new List<GameObject>();

	// Token: 0x04000667 RID: 1639
	private HashSet<string> _manuallyLockedProps = new HashSet<string>();

	// Token: 0x04000668 RID: 1640
	private bool _isInitialized;

	// Token: 0x04000669 RID: 1641
	private bool _propsLocked;

	// Token: 0x020000BC RID: 188
	[Serializable]
	public class EquipmentSlot
	{
		// Token: 0x0400066A RID: 1642
		[Tooltip("Nome identificativo del prop (usato per lookup e per cercare nei children)")]
		public string propName;

		// Token: 0x0400066B RID: 1643
		[Tooltip("GameObject del prop (se null, verrà cercato per nome nei children)")]
		public GameObject propObject;

		// Token: 0x0400066C RID: 1644
		[Tooltip("Azioni che richiedono questo prop attivo (usa NPCActionDefinition per leggibilità)")]
		public List<NPCActionDefinition> activeForActions = new List<NPCActionDefinition>();

		// Token: 0x0400066D RID: 1645
		[Tooltip("(Legacy) Indici delle azioni - usa activeForActions per nuovi setup")]
		public List<int> activeForActionIndices = new List<int>();

		// Token: 0x0400066E RID: 1646
		[Tooltip("Se true, questo prop viene attivato anche durante la locomozione")]
		public bool activeForLocomotion = true;
	}
}
