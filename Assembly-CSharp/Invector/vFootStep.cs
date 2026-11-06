using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000459 RID: 1113
	public class vFootStep : vFootStepBase
	{
		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x000AE852 File Offset: 0x000ACA52
		// (set) Token: 0x06001BBD RID: 7101 RVA: 0x000AE85A File Offset: 0x000ACA5A
		public float Volume
		{
			get
			{
				return this._volume;
			}
			set
			{
				this._volume = value;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x000AE863 File Offset: 0x000ACA63
		// (set) Token: 0x06001BBF RID: 7103 RVA: 0x000AE86B File Offset: 0x000ACA6B
		public bool SpawnParticle
		{
			get
			{
				return this._spawnParticle;
			}
			set
			{
				this._spawnParticle = value;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x000AE874 File Offset: 0x000ACA74
		// (set) Token: 0x06001BC1 RID: 7105 RVA: 0x000AE87C File Offset: 0x000ACA7C
		public bool SpawnStepMark
		{
			get
			{
				return this._spawnStepMark;
			}
			set
			{
				this._spawnStepMark = value;
			}
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000AE885 File Offset: 0x000ACA85
		protected virtual void Start()
		{
			this.InitFootStep();
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000AE890 File Offset: 0x000ACA90
		public virtual void InitFootStep()
		{
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			if (this.animationType != AnimationType.Humanoid)
			{
				foreach (Collider collider in componentsInChildren)
				{
					for (int j = 0; j < this.footStepTriggers.Count; j++)
					{
						vFootStepTrigger vFootStepTrigger = this.footStepTriggers[j];
						vFootStepTrigger.trigger.isTrigger = true;
						if (collider.enabled && collider.gameObject != vFootStepTrigger.gameObject)
						{
							Physics.IgnoreCollision(vFootStepTrigger.trigger, collider);
						}
					}
				}
				return;
			}
			if (this.leftFootTrigger == null && this.rightFootTrigger == null)
			{
				Debug.Log("Missing FootStep Sphere Trigger, please unfold the FootStep Component to create the triggers.");
				return;
			}
			this.leftFootTrigger.trigger.isTrigger = true;
			this.rightFootTrigger.trigger.isTrigger = true;
			Physics.IgnoreCollision(this.leftFootTrigger.trigger, this.rightFootTrigger.trigger);
			foreach (Collider collider2 in componentsInChildren)
			{
				if (collider2.enabled && collider2.gameObject != this.leftFootTrigger.gameObject)
				{
					Physics.IgnoreCollision(this.leftFootTrigger.trigger, collider2);
				}
				if (collider2.enabled && collider2.gameObject != this.rightFootTrigger.gameObject)
				{
					Physics.IgnoreCollision(this.rightFootTrigger.trigger, collider2);
				}
			}
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000AEA00 File Offset: 0x000ACC00
		protected virtual void UpdateTerrainInfo(Terrain newTerrain)
		{
			if (this.terrain == null || this.terrain != newTerrain)
			{
				this.terrain = newTerrain;
				if (this.terrain != null)
				{
					this.terrainData = this.terrain.terrainData;
					this.terrainPos = this.terrain.transform.position;
					this.terrainCollider = this.terrain.GetComponent<TerrainCollider>();
				}
			}
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000AEA78 File Offset: 0x000ACC78
		protected virtual float[] GetTextureMix(FootStepObject footStepObj)
		{
			this.UpdateTerrainInfo(footStepObj.terrain);
			Vector3 position = footStepObj.sender.position;
			int num = (int)((position.x - this.terrainPos.x) / this.terrainData.size.x * (float)this.terrainData.alphamapWidth);
			int num2 = (int)((position.z - this.terrainPos.z) / this.terrainData.size.z * (float)this.terrainData.alphamapHeight);
			if (!this.terrainCollider.bounds.Contains(position))
			{
				return new float[0];
			}
			float[,,] alphamaps = this.terrainData.GetAlphamaps(num, num2, 1, 1);
			float[] array = new float[alphamaps.GetUpperBound(2) + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = alphamaps[0, 0, i];
			}
			return array;
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000AEB64 File Offset: 0x000ACD64
		protected virtual int GetMainTexture(FootStepObject footStepObj)
		{
			float[] textureMix = this.GetTextureMix(footStepObj);
			if (textureMix == null)
			{
				return -1;
			}
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < textureMix.Length; i++)
			{
				if (textureMix[i] > num)
				{
					num2 = i;
					num = textureMix[i];
				}
			}
			return num2;
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000AEBA4 File Offset: 0x000ACDA4
		protected virtual void OnDestroy()
		{
			if (this.leftFootTrigger != null)
			{
				Object.Destroy(this.leftFootTrigger.gameObject);
			}
			if (this.rightFootTrigger != null)
			{
				Object.Destroy(this.rightFootTrigger.gameObject);
			}
			if (this.footStepTriggers != null && this.footStepTriggers.Count > 0)
			{
				foreach (vFootStepTrigger vFootStepTrigger in this.footStepTriggers)
				{
					Object.Destroy(vFootStepTrigger.gameObject);
				}
			}
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000AEC4C File Offset: 0x000ACE4C
		public override void StepOnTerrain(FootStepObject footStepObject)
		{
			if (this.currentStep != null && this.currentStep == footStepObject.sender && this._useTriggerEnter)
			{
				return;
			}
			this.currentStep = footStepObject.sender;
			this.surfaceIndex = this.GetMainTexture(footStepObject);
			if (this.surfaceIndex != -1)
			{
				string text = ((this.terrainData != null && this.terrainData.terrainLayers.Length != 0) ? this.terrainData.terrainLayers[this.surfaceIndex].diffuseTexture.name : "");
				footStepObject.name = text;
				this.currentFootStep = footStepObject;
				if (this._useTriggerEnter)
				{
					this.PlayFootStepEffect();
					if (this.debugTextureName)
					{
						Debug.Log(this.terrain.name + " " + text);
					}
				}
			}
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000AED24 File Offset: 0x000ACF24
		public override void StepOnMesh(FootStepObject footStepObject)
		{
			if (this.currentStep != null && this.currentStep == footStepObject.sender && this._useTriggerEnter)
			{
				return;
			}
			this.currentStep = footStepObject.sender;
			this.currentFootStep = footStepObject;
			if (this._useTriggerEnter)
			{
				this.PlayFootStepEffect();
				if (this.debugTextureName)
				{
					Debug.Log(footStepObject.name);
				}
			}
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000AED90 File Offset: 0x000ACF90
		public override void PlayFootStepEffect()
		{
			if (this.currentFootStep != null)
			{
				this.currentFootStep.volume = this.Volume;
				this.currentFootStep.spawnParticleEffect = this.SpawnParticle;
				this.currentFootStep.spawnStepMarkEffect = this.SpawnStepMark;
				this.SpawnSurfaceEffect(this.currentFootStep);
			}
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000AEDE4 File Offset: 0x000ACFE4
		public override void PlayFootStep(AnimationEvent evt)
		{
			if ((double)evt.animatorClipInfo.weight > 0.5)
			{
				this.PlayFootStepEffect();
			}
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000AEE14 File Offset: 0x000AD014
		public override void PlayFootStepLeft(AnimationEvent evt)
		{
			if ((double)evt.animatorClipInfo.weight > 0.5)
			{
				this.currentFootStep.sender = this.leftFootTrigger.transform;
				this.PlayFootStepEffect();
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000AEE58 File Offset: 0x000AD058
		public override void PlayFootStepRight(AnimationEvent evt)
		{
			if ((double)evt.animatorClipInfo.weight > 0.15)
			{
				this.currentFootStep.sender = this.rightFootTrigger.transform;
				this.PlayFootStepEffect();
			}
		}

		// Token: 0x04001CF9 RID: 7417
		public AnimationType animationType;

		// Token: 0x04001CFA RID: 7418
		public bool debugTextureName;

		// Token: 0x04001CFB RID: 7419
		[SerializeField]
		[Range(0f, 1f)]
		protected float _volume = 1f;

		// Token: 0x04001CFC RID: 7420
		[vHelpBox("Enable or disable spawn particle when foot step is triggered", vHelpBoxAttribute.MessageType.None)]
		[SerializeField]
		protected bool _spawnParticle = true;

		// Token: 0x04001CFD RID: 7421
		[vHelpBox("Enable or disable spawn step mark when foot step is triggered", vHelpBoxAttribute.MessageType.None)]
		[SerializeField]
		protected bool _spawnStepMark = true;

		// Token: 0x04001CFE RID: 7422
		[vHelpBox("The step effect is spawned from on trigger enter event of the Foot Step Triggers. If you need to play step sound only by external events you need to disable this variable.<b>\n*Disable this to play step sound using animation events</b>", vHelpBoxAttribute.MessageType.None)]
		[SerializeField]
		protected bool _useTriggerEnter = true;

		// Token: 0x04001CFF RID: 7423
		protected int surfaceIndex;

		// Token: 0x04001D00 RID: 7424
		protected Terrain terrain;

		// Token: 0x04001D01 RID: 7425
		protected TerrainCollider terrainCollider;

		// Token: 0x04001D02 RID: 7426
		protected TerrainData terrainData;

		// Token: 0x04001D03 RID: 7427
		protected Vector3 terrainPos;

		// Token: 0x04001D04 RID: 7428
		public vFootStepTrigger leftFootTrigger;

		// Token: 0x04001D05 RID: 7429
		public vFootStepTrigger rightFootTrigger;

		// Token: 0x04001D06 RID: 7430
		public Transform currentStep;

		// Token: 0x04001D07 RID: 7431
		public List<vFootStepTrigger> footStepTriggers;

		// Token: 0x04001D08 RID: 7432
		protected FootStepObject currentFootStep;
	}
}
