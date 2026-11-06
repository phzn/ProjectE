using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ECD RID: 3789
	[CreateAssetMenu(menuName = "NPC/Ambient Life Settings")]
	public sealed class NPCAmbientLifeSettings : ScriptableObject
	{
		// Token: 0x0600717C RID: 29052 RVA: 0x002E1C12 File Offset: 0x002DFE12
		public float GestureDuration(string gesture)
		{
			if (!(gesture == "Stretch"))
			{
				return this.lookAroundDuration;
			}
			return this.stretchDuration;
		}

		// Token: 0x0600717D RID: 29053 RVA: 0x002E1C2E File Offset: 0x002DFE2E
		private void OnEnable()
		{
			this.gripLookup = null;
		}

		// Token: 0x0600717E RID: 29054 RVA: 0x002E1C2E File Offset: 0x002DFE2E
		private void OnValidate()
		{
			this.gripLookup = null;
		}

		// Token: 0x0600717F RID: 29055 RVA: 0x002E1C2E File Offset: 0x002DFE2E
		public void ResetGripLookup()
		{
			this.gripLookup = null;
		}

		// Token: 0x06007180 RID: 29056 RVA: 0x002E1C38 File Offset: 0x002DFE38
		private NPCAmbientLifeSettings.DrinkGrip Grip(Mesh body)
		{
			if (body == null)
			{
				return null;
			}
			if (this.gripLookup == null)
			{
				this.gripLookup = new Dictionary<string, NPCAmbientLifeSettings.DrinkGrip>(StringComparer.Ordinal);
				foreach (NPCAmbientLifeSettings.DrinkGrip drinkGrip in this.drinkGrips)
				{
					if (drinkGrip != null && !string.IsNullOrEmpty(drinkGrip.bodyName))
					{
						this.gripLookup[drinkGrip.bodyName] = drinkGrip;
					}
				}
			}
			NPCAmbientLifeSettings.DrinkGrip drinkGrip2;
			if (!this.gripLookup.TryGetValue(body.name, out drinkGrip2))
			{
				return null;
			}
			return drinkGrip2;
		}

		// Token: 0x06007181 RID: 29057 RVA: 0x002E1CE4 File Offset: 0x002DFEE4
		public Vector3 DrinkPosition(float stance, Mesh body = null)
		{
			NPCAmbientLifeSettings.DrinkGrip drinkGrip = this.Grip(body);
			if (drinkGrip == null)
			{
				return Vector3.Lerp(this.maleDrinkPosition, this.drinkPosition, Mathf.Clamp01(stance));
			}
			return drinkGrip.position;
		}

		// Token: 0x06007182 RID: 29058 RVA: 0x002E1D10 File Offset: 0x002DFF10
		public Vector3 DrinkScale(float stance, Mesh body = null)
		{
			NPCAmbientLifeSettings.DrinkGrip drinkGrip = this.Grip(body);
			float num = ((drinkGrip != null) ? drinkGrip.width : Mathf.Lerp(this.maleDrinkWidth, 1f, Mathf.Clamp01(stance)));
			return new Vector3(num, 1f, num);
		}

		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06007183 RID: 29059 RVA: 0x002E1D53 File Offset: 0x002DFF53
		public static NPCAmbientLifeSettings Current
		{
			get
			{
				if (!NPCAmbientLifeSettings.loaded)
				{
					NPCAmbientLifeSettings.cached = Resources.Load<NPCAmbientLifeSettings>("NPCAmbientLifeSettings");
					NPCAmbientLifeSettings.loaded = true;
				}
				return NPCAmbientLifeSettings.cached;
			}
		}

		// Token: 0x06007184 RID: 29060 RVA: 0x002E1D76 File Offset: 0x002DFF76
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetCache()
		{
			NPCAmbientLifeSettings.cached = null;
			NPCAmbientLifeSettings.loaded = false;
		}

		// Token: 0x0400745B RID: 29787
		public bool enableAmbientLife = true;

		// Token: 0x0400745C RID: 29788
		[Range(0f, 1f)]
		public float shoppingChance = 0.18f;

		// Token: 0x0400745D RID: 29789
		[Range(0f, 1f)]
		public float drinkChance = 0.55f;

		// Token: 0x0400745E RID: 29790
		[Tooltip("Share of optional phone/idle stops that can choose a new ambient activity.")]
		[Range(0f, 1f)]
		public float ambientStopChance = 0.8f;

		// Token: 0x0400745F RID: 29791
		[Tooltip("Prefer joining a nearby group that is already forming.")]
		[Range(0f, 1f)]
		public float joinGroupChance = 0.85f;

		// Token: 0x04007460 RID: 29792
		[Range(0f, 1f)]
		public float startGroupChance = 0.35f;

		// Token: 0x04007461 RID: 29793
		[Range(0f, 1f)]
		public float nearbyStopChance = 0.8f;

		// Token: 0x04007462 RID: 29794
		[Min(5f)]
		public float nearbyStopRadius = 40f;

		// Token: 0x04007463 RID: 29795
		public Vector2 socialWait = new Vector2(40f, 65f);

		// Token: 0x04007464 RID: 29796
		public Vector2 socialCooldown = new Vector2(45f, 75f);

		// Token: 0x04007465 RID: 29797
		public Vector2 soloWait = new Vector2(9f, 17f);

		// Token: 0x04007466 RID: 29798
		public Vector2 speakingTurn = new Vector2(3.5f, 6.5f);

		// Token: 0x04007467 RID: 29799
		public float conversationDistance = 3.5f;

		// Token: 0x04007468 RID: 29800
		[HideInInspector]
		public float stretchDuration = 4f;

		// Token: 0x04007469 RID: 29801
		[HideInInspector]
		public float lookAroundDuration = 4f;

		// Token: 0x0400746A RID: 29802
		[HideInInspector]
		public float greetingDuration = 2f;

		// Token: 0x0400746B RID: 29803
		public GameObject drinkPrefab;

		// Token: 0x0400746C RID: 29804
		public GameObject shoppingBagPrefab;

		// Token: 0x0400746D RID: 29805
		public GameObject workBagPrefab;

		// Token: 0x0400746E RID: 29806
		[Range(0f, 1f)]
		public float workBagShare = 0.4f;

		// Token: 0x0400746F RID: 29807
		public HumanBodyBones drinkHand = HumanBodyBones.RightHand;

		// Token: 0x04007470 RID: 29808
		public HumanBodyBones bagHand = HumanBodyBones.RightHand;

		// Token: 0x04007471 RID: 29809
		public Vector3 drinkPosition;

		// Token: 0x04007472 RID: 29810
		public Vector3 drinkRotation;

		// Token: 0x04007473 RID: 29811
		public Vector3 maleDrinkPosition = new Vector3(0.11256f, -0.03864f, 0.04f);

		// Token: 0x04007474 RID: 29812
		public float maleDrinkWidth = 0.8f;

		// Token: 0x04007475 RID: 29813
		public Vector3 bagPosition;

		// Token: 0x04007476 RID: 29814
		public Vector3 bagRotation;

		// Token: 0x04007477 RID: 29815
		public List<NPCAmbientLifeSettings.DrinkGrip> drinkGrips = new List<NPCAmbientLifeSettings.DrinkGrip>();

		// Token: 0x04007478 RID: 29816
		private Dictionary<string, NPCAmbientLifeSettings.DrinkGrip> gripLookup;

		// Token: 0x04007479 RID: 29817
		private static NPCAmbientLifeSettings cached;

		// Token: 0x0400747A RID: 29818
		private static bool loaded;

		// Token: 0x02000ECE RID: 3790
		[Serializable]
		public sealed class DrinkGrip
		{
			// Token: 0x0400747B RID: 29819
			public string bodyName;

			// Token: 0x0400747C RID: 29820
			public Vector3 position;

			// Token: 0x0400747D RID: 29821
			public float width = 1f;
		}
	}
}
