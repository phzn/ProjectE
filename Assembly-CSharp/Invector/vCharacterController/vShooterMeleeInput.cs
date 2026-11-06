using System;
using Invector.IK;
using Invector.PlayerController;
using Invector.vCamera;
using Invector.vShooter;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController
{
	// Token: 0x020006C0 RID: 1728
	[vClassHeader("SHOOTER/MELEE INPUT", true, "icon_v2", false, "", iconName = "inputIcon")]
	public class vShooterMeleeInput : vMeleeCombatInput, vIShooterIKController, vILockCamera
	{
		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06002AAF RID: 10927 RVA: 0x000EF2BB File Offset: 0x000ED4BB
		// (set) Token: 0x06002AB0 RID: 10928 RVA: 0x000EF2C3 File Offset: 0x000ED4C3
		public virtual bool isAimingByInput { get; set; }

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06002AB1 RID: 10929 RVA: 0x000EF2CC File Offset: 0x000ED4CC
		// (set) Token: 0x06002AB2 RID: 10930 RVA: 0x000EF2D4 File Offset: 0x000ED4D4
		public virtual bool isReloading { get; set; }

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x000EF2DD File Offset: 0x000ED4DD
		// (set) Token: 0x06002AB4 RID: 10932 RVA: 0x000EF2E5 File Offset: 0x000ED4E5
		public virtual bool ignoreIK { get; set; }

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000EF2EE File Offset: 0x000ED4EE
		// (set) Token: 0x06002AB6 RID: 10934 RVA: 0x000EF2F6 File Offset: 0x000ED4F6
		public virtual bool aimConditions { get; protected set; }

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06002AB7 RID: 10935 RVA: 0x000EF2FF File Offset: 0x000ED4FF
		// (set) Token: 0x06002AB8 RID: 10936 RVA: 0x000EF307 File Offset: 0x000ED507
		public virtual bool allowAttack { get; set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06002AB9 RID: 10937 RVA: 0x000EF310 File Offset: 0x000ED510
		// (set) Token: 0x06002ABA RID: 10938 RVA: 0x000EF318 File Offset: 0x000ED518
		public virtual bool isCameraRightSwitched { get; set; }

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06002ABB RID: 10939 RVA: 0x000EF321 File Offset: 0x000ED521
		// (set) Token: 0x06002ABC RID: 10940 RVA: 0x000EF329 File Offset: 0x000ED529
		protected virtual bool wasAiming { get; set; }

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x000EF332 File Offset: 0x000ED532
		// (set) Token: 0x06002ABE RID: 10942 RVA: 0x000EF33A File Offset: 0x000ED53A
		protected virtual bool wasAimingWithScope { get; set; }

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x000EF343 File Offset: 0x000ED543
		// (set) Token: 0x06002AC0 RID: 10944 RVA: 0x000EF34B File Offset: 0x000ED54B
		protected virtual bool lockShooterInput { get; set; }

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x000EF354 File Offset: 0x000ED554
		// (set) Token: 0x06002AC2 RID: 10946 RVA: 0x000EF35C File Offset: 0x000ED55C
		protected virtual bool checkCanAimInit { get; set; }

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x000EF365 File Offset: 0x000ED565
		// (set) Token: 0x06002AC4 RID: 10948 RVA: 0x000EF36D File Offset: 0x000ED56D
		protected virtual bool _ignoreIKFromAnimator { get; set; }

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x000EF376 File Offset: 0x000ED576
		// (set) Token: 0x06002AC6 RID: 10950 RVA: 0x000EF37E File Offset: 0x000ED57E
		protected virtual bool _walkingByDefaultWasChanged { get; set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002AC7 RID: 10951 RVA: 0x000EF387 File Offset: 0x000ED587
		// (set) Token: 0x06002AC8 RID: 10952 RVA: 0x000EF38F File Offset: 0x000ED58F
		protected virtual bool isUsingScopeView
		{
			get
			{
				return this._isUsingScopeView;
			}
			set
			{
				this._isUsingScopeView = value;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x000EF398 File Offset: 0x000ED598
		// (set) Token: 0x06002ACA RID: 10954 RVA: 0x000EF3A0 File Offset: 0x000ED5A0
		public virtual int onlyArmsLayer { get; set; }

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002ACB RID: 10955 RVA: 0x000EF3A9 File Offset: 0x000ED5A9
		// (set) Token: 0x06002ACC RID: 10956 RVA: 0x000EF3B1 File Offset: 0x000ED5B1
		public virtual int shotLayer { get; set; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002ACD RID: 10957 RVA: 0x000EF3BA File Offset: 0x000ED5BA
		// (set) Token: 0x06002ACE RID: 10958 RVA: 0x000EF3C2 File Offset: 0x000ED5C2
		public virtual int shootCountA { get; set; }

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002ACF RID: 10959 RVA: 0x000EF3CB File Offset: 0x000ED5CB
		// (set) Token: 0x06002AD0 RID: 10960 RVA: 0x000EF3D3 File Offset: 0x000ED5D3
		public virtual float onlyArmsLayerWeight { get; set; }

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000EF3DC File Offset: 0x000ED5DC
		// (set) Token: 0x06002AD2 RID: 10962 RVA: 0x000EF3E4 File Offset: 0x000ED5E4
		public virtual float supportIKWeight { get; set; }

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002AD3 RID: 10963 RVA: 0x000EF3ED File Offset: 0x000ED5ED
		// (set) Token: 0x06002AD4 RID: 10964 RVA: 0x000EF3F5 File Offset: 0x000ED5F5
		public virtual float weaponIKWeight { get; set; }

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x000EF3FE File Offset: 0x000ED5FE
		// (set) Token: 0x06002AD6 RID: 10966 RVA: 0x000EF406 File Offset: 0x000ED606
		public virtual float armAlignmentWeight { get; protected set; }

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x000EF40F File Offset: 0x000ED60F
		// (set) Token: 0x06002AD8 RID: 10968 RVA: 0x000EF417 File Offset: 0x000ED617
		protected virtual float _aimTiming { get; set; }

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x000EF420 File Offset: 0x000ED620
		// (set) Token: 0x06002ADA RID: 10970 RVA: 0x000EF428 File Offset: 0x000ED628
		protected virtual float checkCanAimOffsetStartX { get; set; }

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002ADB RID: 10971 RVA: 0x000EF431 File Offset: 0x000ED631
		// (set) Token: 0x06002ADC RID: 10972 RVA: 0x000EF439 File Offset: 0x000ED639
		protected virtual float checkCanAimOffsetStartY { get; set; }

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002ADD RID: 10973 RVA: 0x000EF442 File Offset: 0x000ED642
		// (set) Token: 0x06002ADE RID: 10974 RVA: 0x000EF44A File Offset: 0x000ED64A
		protected virtual float checkCanAimOffsetEndX { get; set; }

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002ADF RID: 10975 RVA: 0x000EF453 File Offset: 0x000ED653
		// (set) Token: 0x06002AE0 RID: 10976 RVA: 0x000EF45B File Offset: 0x000ED65B
		protected virtual float checkCanAimOffsetEndY { get; set; }

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x000EF464 File Offset: 0x000ED664
		// (set) Token: 0x06002AE2 RID: 10978 RVA: 0x000EF46C File Offset: 0x000ED66C
		protected virtual float checkCanAimHeight { get; set; }

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x000EF475 File Offset: 0x000ED675
		// (set) Token: 0x06002AE4 RID: 10980 RVA: 0x000EF47D File Offset: 0x000ED67D
		protected virtual float scopeDirectionWeight { get; set; }

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x000EF486 File Offset: 0x000ED686
		// (set) Token: 0x06002AE6 RID: 10982 RVA: 0x000EF48E File Offset: 0x000ED68E
		protected virtual GameObject aimAngleReference { get; set; }

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x000EF497 File Offset: 0x000ED697
		// (set) Token: 0x06002AE8 RID: 10984 RVA: 0x000EF49F File Offset: 0x000ED69F
		protected virtual GameObject lastShooterWeapon { get; set; }

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x000EF4A8 File Offset: 0x000ED6A8
		// (set) Token: 0x06002AEA RID: 10986 RVA: 0x000EF4B0 File Offset: 0x000ED6B0
		protected virtual Vector3 localAimPosition { get; set; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002AEB RID: 10987 RVA: 0x000EF4B9 File Offset: 0x000ED6B9
		// (set) Token: 0x06002AEC RID: 10988 RVA: 0x000EF4C1 File Offset: 0x000ED6C1
		protected virtual Vector3 aimHitPoint { get; set; }

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002AED RID: 10989 RVA: 0x000EF4CA File Offset: 0x000ED6CA
		// (set) Token: 0x06002AEE RID: 10990 RVA: 0x000EF4D2 File Offset: 0x000ED6D2
		protected virtual Vector3 upperArmPosition { get; set; }

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002AEF RID: 10991 RVA: 0x000EF4DB File Offset: 0x000ED6DB
		// (set) Token: 0x06002AF0 RID: 10992 RVA: 0x000EF4E3 File Offset: 0x000ED6E3
		protected virtual Vector3 muzzlePosition { get; set; }

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x000EF4EC File Offset: 0x000ED6EC
		// (set) Token: 0x06002AF2 RID: 10994 RVA: 0x000EF4F4 File Offset: 0x000ED6F4
		protected virtual Vector3 muzzleForward { get; set; }

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x000EF4FD File Offset: 0x000ED6FD
		// (set) Token: 0x06002AF4 RID: 10996 RVA: 0x000EF505 File Offset: 0x000ED705
		protected virtual Vector3 ikRotationOffset { get; set; }

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x000EF50E File Offset: 0x000ED70E
		// (set) Token: 0x06002AF6 RID: 10998 RVA: 0x000EF516 File Offset: 0x000ED716
		protected virtual Vector3 ikPositionOffset { get; set; }

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x000EF51F File Offset: 0x000ED71F
		// (set) Token: 0x06002AF8 RID: 11000 RVA: 0x000EF527 File Offset: 0x000ED727
		protected virtual Vector3 scopeLocalPosition { get; set; }

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002AF9 RID: 11001 RVA: 0x000EF530 File Offset: 0x000ED730
		// (set) Token: 0x06002AFA RID: 11002 RVA: 0x000EF538 File Offset: 0x000ED738
		protected virtual Vector3 scopeCameraLocalPosition { get; set; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002AFB RID: 11003 RVA: 0x000EF541 File Offset: 0x000ED741
		// (set) Token: 0x06002AFC RID: 11004 RVA: 0x000EF549 File Offset: 0x000ED749
		protected virtual Vector3 scopeLocalForward { get; set; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002AFD RID: 11005 RVA: 0x000EF552 File Offset: 0x000ED752
		// (set) Token: 0x06002AFE RID: 11006 RVA: 0x000EF55A File Offset: 0x000ED75A
		protected virtual Vector3 scopeCameraLocalForward { get; set; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002AFF RID: 11007 RVA: 0x000EF563 File Offset: 0x000ED763
		// (set) Token: 0x06002B00 RID: 11008 RVA: 0x000EF56B File Offset: 0x000ED76B
		protected virtual Vector3 lastTargetCameraEuler { get; set; }

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x000EF574 File Offset: 0x000ED774
		// (set) Token: 0x06002B02 RID: 11010 RVA: 0x000EF57C File Offset: 0x000ED77C
		protected virtual Quaternion handRotation { get; set; }

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002B03 RID: 11011 RVA: 0x000EF585 File Offset: 0x000ED785
		// (set) Token: 0x06002B04 RID: 11012 RVA: 0x000EF58D File Offset: 0x000ED78D
		protected virtual Quaternion upperArmRotation { get; set; }

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002B05 RID: 11013 RVA: 0x000EF596 File Offset: 0x000ED796
		// (set) Token: 0x06002B06 RID: 11014 RVA: 0x000EF59E File Offset: 0x000ED79E
		protected virtual Quaternion upperArmRotationAlignment { get; set; }

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06002B07 RID: 11015 RVA: 0x000EF5A7 File Offset: 0x000ED7A7
		// (set) Token: 0x06002B08 RID: 11016 RVA: 0x000EF5AF File Offset: 0x000ED7AF
		protected virtual Quaternion handRotationAlignment { get; set; }

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06002B09 RID: 11017 RVA: 0x000EF5B8 File Offset: 0x000ED7B8
		// (set) Token: 0x06002B0A RID: 11018 RVA: 0x000EF5C0 File Offset: 0x000ED7C0
		public virtual vIKSolver LeftIK { get; set; }

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06002B0B RID: 11019 RVA: 0x000EF5C9 File Offset: 0x000ED7C9
		// (set) Token: 0x06002B0C RID: 11020 RVA: 0x000EF5D1 File Offset: 0x000ED7D1
		public virtual vIKSolver RightIK { get; set; }

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06002B0D RID: 11021 RVA: 0x000EF5DA File Offset: 0x000ED7DA
		// (set) Token: 0x06002B0E RID: 11022 RVA: 0x000EF5F6 File Offset: 0x000ED7F6
		public virtual vWeaponIKAdjustList WeaponIKAdjustList
		{
			get
			{
				if (this.shooterManager)
				{
					return this.shooterManager.weaponIKAdjustList;
				}
				return null;
			}
			set
			{
				if (this.shooterManager)
				{
					this.shooterManager.weaponIKAdjustList = value;
				}
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06002B0F RID: 11023 RVA: 0x000EF611 File Offset: 0x000ED811
		public virtual vWeaponIKAdjust CurrentWeaponIK
		{
			get
			{
				if (this.shooterManager)
				{
					return this.shooterManager.CurrentWeaponIK;
				}
				return null;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002B10 RID: 11024 RVA: 0x000EF630 File Offset: 0x000ED830
		public virtual IKAdjust CurrentIKAdjust
		{
			get
			{
				if (this.CurrentWeaponIK == null)
				{
					return null;
				}
				if (this.CurrentIKAdjustStateWithTag != this.IKWeaponTag + this.TargetIKAdjustState || this._currentIKAdjust == null)
				{
					this.CurrentIKAdjustStateWithTag = this.IKWeaponTag + this.TargetIKAdjustState;
					this.CurrentIKAdjustState = this.TargetIKAdjustState;
					this._currentIKAdjust = this.CurrentWeaponIK.GetIKAdjust(this.CurrentIKAdjustState, this.CurrentActiveWeapon.isLeftWeapon);
				}
				return this._currentIKAdjust;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06002B11 RID: 11025 RVA: 0x000EF6BE File Offset: 0x000ED8BE
		// (set) Token: 0x06002B12 RID: 11026 RVA: 0x000EF6C6 File Offset: 0x000ED8C6
		public virtual bool EditingIKGlobalOffset { get; set; }

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06002B13 RID: 11027 RVA: 0x000EF6CF File Offset: 0x000ED8CF
		public virtual string DefaultIKAdjustState
		{
			get
			{
				if (!this.CurrentWeaponIK)
				{
					return string.Empty;
				}
				return this.CurrentWeaponIK.GetDefaultStateName(this);
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002B14 RID: 11028 RVA: 0x000EF6F0 File Offset: 0x000ED8F0
		protected virtual string TargetIKAdjustState
		{
			get
			{
				if (this.IsUsingCustomIKAdjust)
				{
					return this.CustomIKAdjustState;
				}
				return this.DefaultIKAdjustState;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x000EF707 File Offset: 0x000ED907
		protected virtual string IKWeaponTag
		{
			get
			{
				if (!this.CurrentActiveWeapon)
				{
					return "";
				}
				return this.CurrentActiveWeapon.weaponCategory + "@";
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06002B16 RID: 11030 RVA: 0x000EF731 File Offset: 0x000ED931
		// (set) Token: 0x06002B17 RID: 11031 RVA: 0x000EF739 File Offset: 0x000ED939
		public virtual string CurrentIKAdjustStateWithTag { get; set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002B18 RID: 11032 RVA: 0x000EF742 File Offset: 0x000ED942
		// (set) Token: 0x06002B19 RID: 11033 RVA: 0x000EF74A File Offset: 0x000ED94A
		public virtual string CurrentIKAdjustState { get; protected set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x000EF753 File Offset: 0x000ED953
		public virtual bool IsUsingCustomIKAdjust
		{
			get
			{
				return !string.IsNullOrEmpty(this.CustomIKAdjustState);
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x000EF763 File Offset: 0x000ED963
		// (set) Token: 0x06002B1C RID: 11036 RVA: 0x000EF76B File Offset: 0x000ED96B
		public string CustomIKAdjustState { get; protected set; }

		// Token: 0x06002B1D RID: 11037 RVA: 0x000EF774 File Offset: 0x000ED974
		public virtual void SetCustomIKAdjustState(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				this.CustomIKAdjustState = value;
			}
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000EF785 File Offset: 0x000ED985
		public virtual void ResetCustomIKAdjustState()
		{
			if (!string.IsNullOrEmpty(this.CustomIKAdjustState))
			{
				this.CustomIKAdjustState = string.Empty;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002B1F RID: 11039 RVA: 0x000EF79F File Offset: 0x000ED99F
		public virtual bool IsIgnoreIK
		{
			get
			{
				return this.ignoreIK || this._ignoreIKFromAnimator;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002B20 RID: 11040 RVA: 0x000EF7B1 File Offset: 0x000ED9B1
		public virtual bool IsIgnoreSupportHandIK
		{
			get
			{
				return this.cc.IsAnimatorTag("IgnoreSupportHandIK");
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002B21 RID: 11041 RVA: 0x000EF7C3 File Offset: 0x000ED9C3
		// (set) Token: 0x06002B22 RID: 11042 RVA: 0x000EF7CB File Offset: 0x000ED9CB
		public virtual bool IsSupportHandIKEnabled { get; protected set; }

		// Token: 0x06002B23 RID: 11043 RVA: 0x000EF7D4 File Offset: 0x000ED9D4
		public virtual void UpdateWeaponIK()
		{
			if (this.shooterManager)
			{
				this.shooterManager.UpdateWeaponIK();
				if (this.CurrentWeaponIK == null)
				{
					return;
				}
				this._currentIKAdjust = this.CurrentWeaponIK.GetIKAdjust(this.CurrentIKAdjustState, this.CurrentActiveWeapon.isLeftWeapon);
			}
		}

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06002B24 RID: 11044 RVA: 0x000EF82C File Offset: 0x000EDA2C
		// (remove) Token: 0x06002B25 RID: 11045 RVA: 0x000EF864 File Offset: 0x000EDA64
		public event IKUpdateEvent onStartUpdateIK;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06002B26 RID: 11046 RVA: 0x000EF89C File Offset: 0x000EDA9C
		// (remove) Token: 0x06002B27 RID: 11047 RVA: 0x000EF8D4 File Offset: 0x000EDAD4
		public event IKUpdateEvent onFinishUpdateIK;

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x000EF909 File Offset: 0x000EDB09
		// (set) Token: 0x06002B29 RID: 11049 RVA: 0x000EF911 File Offset: 0x000EDB11
		public virtual Vector3 DesiredAimPosition { get; protected set; }

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x000EF91A File Offset: 0x000EDB1A
		// (set) Token: 0x06002B2B RID: 11051 RVA: 0x000EF922 File Offset: 0x000EDB22
		public virtual Vector3 AimPosition { get; protected set; }

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002B2C RID: 11052 RVA: 0x000EF92B File Offset: 0x000EDB2B
		// (set) Token: 0x06002B2D RID: 11053 RVA: 0x000EF947 File Offset: 0x000EDB47
		public virtual bool LockAiming
		{
			get
			{
				return this.shooterManager && this.shooterManager.alwaysAiming;
			}
			set
			{
				this.shooterManager.alwaysAiming = value;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002B2E RID: 11054 RVA: 0x000EF955 File Offset: 0x000EDB55
		// (set) Token: 0x06002B2F RID: 11055 RVA: 0x000EF95D File Offset: 0x000EDB5D
		public virtual bool LockHipFireAiming { get; set; }

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002B30 RID: 11056 RVA: 0x000EF966 File Offset: 0x000EDB66
		// (set) Token: 0x06002B31 RID: 11057 RVA: 0x000EF973 File Offset: 0x000EDB73
		public virtual bool IsCrouching
		{
			get
			{
				return this.cc.isCrouching;
			}
			set
			{
				this.cc.isCrouching = value;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06002B32 RID: 11058 RVA: 0x000EF981 File Offset: 0x000EDB81
		public virtual bool IsLeftWeapon
		{
			get
			{
				return this.shooterManager && this.shooterManager.IsLeftWeapon;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06002B33 RID: 11059 RVA: 0x000EF99D File Offset: 0x000EDB9D
		// (set) Token: 0x06002B34 RID: 11060 RVA: 0x000EF9B9 File Offset: 0x000EDBB9
		public virtual bool LockCamera
		{
			get
			{
				return this.tpCamera && this.tpCamera.LockCamera;
			}
			set
			{
				if (this.tpCamera)
				{
					this.tpCamera.LockCamera = value;
				}
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002B35 RID: 11061 RVA: 0x000EF9D4 File Offset: 0x000EDBD4
		public virtual bool IsAiming
		{
			get
			{
				return !this.lockShooterInput && !this.cc.isRolling && (this.isAimingByInput || this.isAimingByHipFire);
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002B36 RID: 11062 RVA: 0x000EFA00 File Offset: 0x000EDC00
		public virtual bool isAimingByHipFire
		{
			get
			{
				if ((!this.shooterManager.hipfireShot && this._aimTiming > 0f) || (this.isReloading && !this.shooterManager.keepAimingWhenReload) || this.isEquipping || this.lockShooterInput || this.cc.customAction)
				{
					this._aimTiming = 0f;
					return false;
				}
				return this.shooterManager.hipfireShot && (this._aimTiming > 0f || (this.shotInput.GetButton() && this.shooterManager.CurrentWeapon != null) || (!this.isAimingByInput && this.shootCountA > 0));
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002B37 RID: 11063 RVA: 0x000EFAB7 File Offset: 0x000EDCB7
		public virtual vControlAimCanvas controlAimCanvas
		{
			get
			{
				if (!this._controlAimCanvas)
				{
					this._controlAimCanvas = Object.FindObjectOfType<vControlAimCanvas>();
					if (this._controlAimCanvas)
					{
						this._controlAimCanvas.Init(this.cc);
					}
				}
				return this._controlAimCanvas;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002B38 RID: 11064 RVA: 0x000EFAF5 File Offset: 0x000EDCF5
		public override bool lockInventory
		{
			get
			{
				return base.lockInventory || this.isReloading || this.cc.customAction || this.cc.isRolling;
			}
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x000EFB24 File Offset: 0x000EDD24
		protected override void Start()
		{
			this.shooterManager = base.GetComponent<vShooterManager>();
			base.Start();
			this.checkCanAimHeight = this.cc._capsuleCollider.height;
			this.leftHand = base.animator.GetBoneTransform(HumanBodyBones.LeftHand);
			this.rightHand = base.animator.GetBoneTransform(HumanBodyBones.RightHand);
			this.leftLowerArm = base.animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
			this.rightLowerArm = base.animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
			this.leftUpperArm = base.animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
			this.rightUpperArm = base.animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
			this.onlyArmsLayer = base.animator.GetLayerIndex("OnlyArms");
			this.shotLayer = base.animator.GetLayerIndex("Shot");
			this.aimAngleReference = new GameObject("aimAngleReference");
			this.aimAngleReference.tag = "Ignore Ragdoll";
			this.aimAngleReference.transform.rotation = base.transform.rotation;
			Transform boneTransform = base.animator.GetBoneTransform(HumanBodyBones.Head);
			this.aimAngleReference.transform.SetParent(boneTransform);
			this.aimAngleReference.transform.localPosition = Vector3.zero;
			this.headTrack = base.GetComponent<vHeadTrack>();
			if (this.headTrack)
			{
				this.headTrack.onInitUpdate.AddListener(new UnityAction(this.UpdateAimAngleReference));
			}
			if (!this.controlAimCanvas)
			{
				Debug.LogWarning("Missing the AimCanvas, drag and drop the prefab to this scene in order to Aim", base.gameObject);
			}
			this.muzzlePosition = Vector3.forward * this.cc._capsuleCollider.radius * 2f;
			this.muzzleForward = Vector3.forward;
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000EFCEE File Offset: 0x000EDEEE
		protected override void LateUpdate()
		{
			if (!this.updateIK && base.animator.updateMode == AnimatorUpdateMode.AnimatePhysics)
			{
				return;
			}
			base.LateUpdate();
			this.UpdateAimBehaviour();
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x000EFD13 File Offset: 0x000EDF13
		protected virtual void Reset()
		{
			this.weakAttackInput = new GenericInput("Mouse2", "RB", "RB");
			this.strafeInput.useInput = false;
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000EFD3C File Offset: 0x000EDF3C
		public virtual void SetLockShooterInput(bool value)
		{
			this.lockShooterInput = value;
			if (value)
			{
				this.isBlocking = false;
				this.isAimingByInput = false;
				this._aimTiming = 0f;
				if (this.controlAimCanvas)
				{
					this.controlAimCanvas.SetActiveAim(false);
					this.controlAimCanvas.SetActiveScopeCamera(false, false);
				}
			}
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000EFD92 File Offset: 0x000EDF92
		public override void SetLockAllInput(bool value)
		{
			base.SetLockAllInput(value);
			this.SetLockShooterInput(value);
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000EF947 File Offset: 0x000EDB47
		public virtual void SetAlwaysAim(bool value)
		{
			this.shooterManager.alwaysAiming = value;
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002B3F RID: 11071 RVA: 0x000EFDA2 File Offset: 0x000EDFA2
		public virtual vShooterWeapon CurrentActiveWeapon
		{
			get
			{
				return this.shooterManager.CurrentActiveWeapon;
			}
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000EFDB0 File Offset: 0x000EDFB0
		public override void InputHandle()
		{
			if (this.cc == null || this.cc.isDead)
			{
				return;
			}
			if (!this.cc.ragdolled && !this.lockInput)
			{
				this.MoveInput();
				this.SprintInput();
				this.CrouchInput();
				this.StrafeInput();
				this.JumpInput();
				this.RollInput();
			}
			if (this.MeleeAttackConditions() && !this.IsAiming && !this.isReloading && !this.lockMeleeInput && !this.CurrentActiveWeapon)
			{
				if (this.shooterManager.canUseMeleeWeakAttack_H || this.shooterManager.CurrentWeapon == null)
				{
					this.MeleeWeakAttackInput();
				}
				if (this.shooterManager.canUseMeleeStrongAttack_H || this.shooterManager.CurrentWeapon == null)
				{
					this.MeleeStrongAttackInput();
				}
				if (this.shooterManager.canUseMeleeBlock_H || this.shooterManager.CurrentWeapon == null)
				{
					this.BlockingInput();
				}
				else
				{
					this.isBlocking = false;
				}
			}
			if (this.lockShooterInput)
			{
				this.isAimingByInput = false;
				this._aimTiming = 0f;
				if (this.controlAimCanvas != null && (this.controlAimCanvas.isAimActive || this.controlAimCanvas.isScopeCameraActive))
				{
					this.isUsingScopeView = false;
					this.controlAimCanvas.DisableAim();
					return;
				}
			}
			else if (this.shooterManager.CurrentWeapon)
			{
				if (this.MeleeAttackConditions() && (!this.IsAiming || this.shooterManager.canUseMeleeAiming))
				{
					if (this.shooterManager.canUseMeleeWeakAttack_E)
					{
						this.MeleeWeakAttackInput();
					}
					if (this.shooterManager.canUseMeleeStrongAttack_E)
					{
						this.MeleeStrongAttackInput();
					}
					if (this.shooterManager.canUseMeleeBlock_E)
					{
						this.BlockingInput();
					}
					else
					{
						this.isBlocking = false;
					}
				}
				else
				{
					this.isBlocking = false;
				}
				if (!(this.shooterManager == null) && !(this.CurrentActiveWeapon == null) && !this.isEquipping)
				{
					this.AimInput();
					this.ShotInput();
					this.ReloadInput();
					this.SwitchCameraSideInput();
					this.ScopeViewInput();
					return;
				}
				if (this._walkingByDefaultWasChanged)
				{
					this._walkingByDefaultWasChanged = false;
					this.ResetWalkByDefault();
				}
				if (this.IsAiming)
				{
					this.isAimingByInput = false;
					this._aimTiming = 0f;
					if (!this.cc.lockInStrafe && this.cc.isStrafing)
					{
						this.cc.Strafe();
					}
					if (this.controlAimCanvas != null && (this.controlAimCanvas.isAimActive || this.controlAimCanvas.isScopeCameraActive))
					{
						this.isUsingScopeView = false;
						this.controlAimCanvas.DisableAim();
					}
					if (this.shooterManager && this.shooterManager.CurrentWeapon && this.shooterManager.CurrentWeapon.chargeWeapon && this.shooterManager.CurrentWeapon.powerCharge != 0f)
					{
						this.CurrentActiveWeapon.powerCharge = 0f;
					}
					this.shootCountA = 0;
					return;
				}
			}
			else
			{
				this.isAimingByInput = false;
				this._aimTiming = 0f;
				if (!this.cc.lockInStrafe && this.cc.isStrafing && this.cc.locomotionType != vThirdPersonMotor.LocomotionType.OnlyStrafe)
				{
					this.cc.Strafe();
				}
				if (this._walkingByDefaultWasChanged && !this.IsAiming)
				{
					this._walkingByDefaultWasChanged = false;
					this.ResetWalkByDefault();
				}
				if (this.controlAimCanvas != null && (this.controlAimCanvas.isAimActive || this.controlAimCanvas.isScopeCameraActive))
				{
					this.isUsingScopeView = false;
					this.controlAimCanvas.DisableAim();
				}
			}
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x000F0171 File Offset: 0x000EE371
		public override void TriggerStrongAttack()
		{
			this.shooterManager.CancelReload();
			base.TriggerStrongAttack();
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x000F0184 File Offset: 0x000EE384
		public virtual void AimInput()
		{
			this.cc.strafeSpeed.rotateWithCamera = this.IsAiming || this.cc.strafeSpeed.defaultRotateWithCamera;
			if (this._walkingByDefaultWasChanged && !this.IsAiming)
			{
				this._walkingByDefaultWasChanged = false;
				this.ResetWalkByDefault();
			}
			if (!this.shooterManager || this.isEquipping || this.isAttacking || (this.isReloading && (this.isUsingScopeView || !this.shooterManager.keepAimingWhenReload)))
			{
				if (!this.isReloading || (this.isReloading && !this.shooterManager.keepAimingWhenReload))
				{
					this.isAimingByInput = false;
					this.ResetWalkByDefault();
					this._walkingByDefaultWasChanged = false;
					if (this.cc.isStrafing && this.cc.locomotionType != vThirdPersonMotor.LocomotionType.OnlyStrafe)
					{
						this.cc.Strafe();
					}
				}
				if (this.controlAimCanvas && (this.controlAimCanvas.isAimActive || this.controlAimCanvas.isScopeCameraActive))
				{
					this.isUsingScopeView = false;
					this.controlAimCanvas.DisableAim();
				}
				return;
			}
			if (this.LockHipFireAiming)
			{
				this._aimTiming = 1f;
			}
			if (this.shooterManager.onlyWalkWhenAiming && (!this.isReloading || this.shooterManager.keepAimingWhenReload))
			{
				if (this.isAimingByInput)
				{
					this.SetWalkByDefault(true);
					this._walkingByDefaultWasChanged = true;
				}
				else
				{
					this.ResetWalkByDefault();
					this._walkingByDefaultWasChanged = false;
				}
			}
			if (this.cc.locomotionType == vThirdPersonMotor.LocomotionType.OnlyFree)
			{
				Debug.LogWarning("Shooter behaviour needs to be OnlyStrafe or Free with Strafe. \n Please change the Locomotion Type.");
				return;
			}
			if (this.shooterManager.hipfireShot && !this.LockHipFireAiming)
			{
				if (this._aimTiming > 0f && this.CanRotateAimArm())
				{
					this._aimTiming -= Time.deltaTime;
				}
				if (this.sprintInput.GetButtonDown() && this._aimTiming > 0f)
				{
					this._aimTiming = 0f;
				}
			}
			if (!this.shooterManager || !this.CurrentActiveWeapon)
			{
				if (this.controlAimCanvas && (this.controlAimCanvas.isAimActive || this.controlAimCanvas.isScopeCameraActive))
				{
					this.isUsingScopeView = false;
					this.controlAimCanvas.DisableAim();
				}
				this.isAimingByInput = false;
				if (this.cc.isStrafing)
				{
					this.cc.Strafe();
				}
				return;
			}
			if (!this.cc.isRolling)
			{
				this.isAimingByInput = ((!this.isReloading || this.shooterManager.keepAimingWhenReload) && (this.aimInput.GetButton() || (this.shooterManager.alwaysAiming && this.CurrentActiveWeapon)) && !this.cc.ragdolled && !this.cc.customAction) || (this.cc.customAction && this.cc.isJumping);
			}
			if (this.aimInput.GetButtonUp() && !this.shotInput.GetButton())
			{
				this._aimTiming = 0f;
			}
			if (this.headTrack)
			{
				this.headTrack.alwaysFollowCamera = this.isAimingByInput;
			}
			if (this.cc.locomotionType == vThirdPersonMotor.LocomotionType.FreeWithStrafe && !this.cc.lockInStrafe)
			{
				if (this.IsAiming && !this.cc.isStrafing)
				{
					this.cc.Strafe();
				}
				else if (!this.IsAiming && this.cc.isStrafing)
				{
					this.cc.Strafe();
				}
			}
			if (this.IsAiming && this.shooterManager.onlyWalkWhenAiming && this.cc.isSprinting)
			{
				this.cc.isSprinting = false;
			}
			if (this.controlAimCanvas && !this.isUsingScopeView)
			{
				if (this.IsAiming && !this.controlAimCanvas.isAimActive)
				{
					this.controlAimCanvas.SetActiveAim(true);
				}
				if (!this.IsAiming && this.controlAimCanvas.isAimActive)
				{
					this.controlAimCanvas.SetActiveAim(false);
				}
			}
			if (this.shooterManager.rWeapon)
			{
				this.shooterManager.rWeapon.SetActiveAim(this.IsAiming && this.aimConditions);
				this.shooterManager.rWeapon.SetActiveScope(this.IsAiming && this.isUsingScopeView);
				return;
			}
			if (this.shooterManager.lWeapon)
			{
				this.shooterManager.lWeapon.SetActiveAim(this.IsAiming && this.aimConditions);
				this.shooterManager.lWeapon.SetActiveScope(this.IsAiming && this.isUsingScopeView);
			}
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000F0648 File Offset: 0x000EE848
		public virtual void ShotInput()
		{
			if (!this.shooterManager || this.CurrentActiveWeapon == null || this.cc.isDead || this.isReloading || this.isAttacking || this.isEquipping)
			{
				if (this.shooterManager && this.shooterManager.CurrentWeapon.chargeWeapon && this.shooterManager.CurrentWeapon.powerCharge != 0f)
				{
					this.CurrentActiveWeapon.powerCharge = 0f;
				}
				this.shootCountA = 0;
				return;
			}
			if (this.IsAiming && !this.shooterManager.isShooting && this.aimConditions)
			{
				if (this.CurrentActiveWeapon || (this.shooterManager.CurrentWeapon && this.shooterManager.hipfireShot))
				{
					this.HandleShotCount(this.shooterManager.CurrentWeapon, this.shotInput.GetButton());
					return;
				}
			}
			else if (!this.IsAiming)
			{
				if (this.shooterManager.CurrentWeapon.chargeWeapon && this.shooterManager.CurrentWeapon.powerCharge != 0f)
				{
					this.CurrentActiveWeapon.powerCharge = 0f;
				}
				this.shootCountA = 0;
			}
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000F0794 File Offset: 0x000EE994
		public virtual void HandleShotCount(vShooterWeapon weapon, bool weaponInput = true)
		{
			if (weapon.chargeWeapon)
			{
				if (this.shooterManager.WeaponHasLoadedAmmo() && weapon.powerCharge < 1f && weaponInput)
				{
					if (this.shooterManager.hipfireShot)
					{
						this._aimTiming = this.shooterManager.HipfireAimTime;
					}
					weapon.powerCharge += Time.fixedDeltaTime * weapon.chargeSpeed;
				}
				else if ((weapon.powerCharge >= 1f && weapon.autoShotOnFinishCharge && weaponInput) || (!weaponInput && this.IsAiming && weapon.powerCharge > 0f))
				{
					if (this.shooterManager.hipfireShot)
					{
						this._aimTiming = this.shooterManager.HipfireAimTime;
					}
					this.shootCountA = 1;
				}
				base.animator.SetFloat(vAnimatorParameters.PowerCharger, weapon.powerCharge);
				return;
			}
			if (weapon.automaticWeapon && weaponInput)
			{
				if (this.shooterManager.hipfireShot && !this.isAimingByInput)
				{
					this._aimTiming = this.shooterManager.HipfireAimTime;
				}
				this.shootCountA = 1;
				return;
			}
			if (weaponInput)
			{
				if (this.shooterManager.hipfireShot && !this.isAimingByInput)
				{
					this._aimTiming = this.shooterManager.HipfireAimTime;
				}
				if (!this.allowAttack)
				{
					this.shootCountA = 1;
					this.allowAttack = true;
					return;
				}
			}
			else
			{
				this.allowAttack = false;
			}
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x000F08F3 File Offset: 0x000EEAF3
		public virtual void DoShots()
		{
			if (this.CanDoShots())
			{
				this.TriggerShot();
			}
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000F0904 File Offset: 0x000EEB04
		public virtual void TriggerShot()
		{
			base.animator.SetFloat(vAnimatorParameters.Shot_ID, (float)this.shooterManager.GetShotID());
			this.shooterManager.Shoot(this.AimPosition, !this.isAimingByInput, false);
			if (this.CurrentActiveWeapon.chargeWeapon)
			{
				this.CurrentActiveWeapon.powerCharge = 0f;
			}
			int shootCountA = this.shootCountA;
			this.shootCountA = shootCountA - 1;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x000F0978 File Offset: 0x000EEB78
		public virtual void ReloadInput()
		{
			if (!this.shooterManager || this.CurrentActiveWeapon == null || this.isReloading || this.cc.customAction || this.shooterManager.isShooting || this.cc.ragdolled)
			{
				return;
			}
			if (this.reloadInput.GetButtonDown())
			{
				this.shootCountA = 0;
				this._aimTiming = 0f;
				this.shooterManager.ReloadWeapon();
				return;
			}
			if (this.CurrentActiveWeapon.autoReload && !this.shooterManager.WeaponHasLoadedAmmo() && this.shooterManager.WeaponHasUnloadedAmmo())
			{
				switch (this.CurrentActiveWeapon.autoReloadStyle)
				{
				case vShooterWeapon.AutoReloadStyle.WhenAiming:
					if (this.IsAiming)
					{
						this.shooterManager.ReloadWeapon();
						return;
					}
					break;
				case vShooterWeapon.AutoReloadStyle.WhenShot:
					if (this.shotInput.GetButtonDown())
					{
						this.shooterManager.ReloadWeapon();
						return;
					}
					break;
				case vShooterWeapon.AutoReloadStyle.WhenAmmoAvailable:
					this.shooterManager.ReloadWeapon();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x000F0A79 File Offset: 0x000EEC79
		public virtual void SwitchCameraSideInput()
		{
			if (this.tpCamera == null)
			{
				return;
			}
			if (this.switchCameraSideInput.GetButtonDown())
			{
				this.SwitchCameraSide();
			}
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x000F0A9D File Offset: 0x000EEC9D
		public virtual void SwitchCameraSide()
		{
			if (this.tpCamera == null)
			{
				return;
			}
			this.isCameraRightSwitched = !this.isCameraRightSwitched;
			this.tpCamera.SwitchRight(this.isCameraRightSwitched);
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x000F0ACE File Offset: 0x000EECCE
		public virtual void CancelAiming()
		{
			this.isAimingByInput = false;
			this._aimTiming = 0f;
			if (this.controlAimCanvas)
			{
				this.controlAimCanvas.SetActiveAim(false);
				this.controlAimCanvas.SetActiveScopeCamera(false, false);
			}
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x000F0B08 File Offset: 0x000EED08
		public virtual void ScopeViewInput()
		{
			if (!this.shooterManager || this.CurrentActiveWeapon == null)
			{
				return;
			}
			if (this.isAimingByInput && this.aimConditions && (this.scopeViewInput.GetButtonDown() || this.CurrentActiveWeapon.onlyUseScopeUIView))
			{
				if (this.controlAimCanvas && this.CurrentActiveWeapon.scopeTarget)
				{
					if (!this.isUsingScopeView && this.CurrentActiveWeapon.onlyUseScopeUIView)
					{
						this.EnableScopeView();
						return;
					}
					if (this.isUsingScopeView && !this.CurrentActiveWeapon.onlyUseScopeUIView)
					{
						this.DisableScopeView();
						return;
					}
					if (!this.isUsingScopeView)
					{
						this.EnableScopeView();
						return;
					}
				}
			}
			else if (this.isUsingScopeView && ((this.controlAimCanvas && !this.isAimingByInput) || (this.controlAimCanvas && !this.aimConditions) || this.cc.isRolling))
			{
				this.DisableScopeView();
			}
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x000F0C14 File Offset: 0x000EEE14
		public virtual void EnableScopeView()
		{
			if (!this.isAimingByInput || !this.controlAimCanvas.scopeBackgroundCamera || this.isReloading || this.isEquipping)
			{
				return;
			}
			this.isUsingScopeView = true;
			this.controlAimCanvas.SetActiveScopeCamera(true, this.CurrentActiveWeapon.useUI);
			this.controlAimCanvas.SetActiveAim(false);
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x000F0C78 File Offset: 0x000EEE78
		public virtual void DisableScopeView()
		{
			if (!this.controlAimCanvas.scopeBackgroundCamera)
			{
				return;
			}
			this.isUsingScopeView = false;
			bool useSmooth = this.tpCamera.useSmooth;
			this.tpCamera.useSmooth = false;
			this.tpCamera.mouseX = this.lastTargetCameraEuler.y;
			this.tpCamera.mouseY = this.lastTargetCameraEuler.x;
			this.tpCamera.CameraMovement(false);
			this.tpCamera.useSmooth = useSmooth;
			this.controlAimCanvas.SetActiveScopeCamera(false, false);
			this.controlAimCanvas.SetActiveAim(this.IsAiming);
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x000F0D1C File Offset: 0x000EEF1C
		protected override void UpdateMeleeAnimations()
		{
			if (!base.animator)
			{
				return;
			}
			this.isEquipping = this.cc.IsAnimatorTag("IsEquipping");
			this._ignoreIKFromAnimator = this.cc.IsAnimatorTag("IgnoreIK");
			if (this.cc.customAction)
			{
				this.ResetMeleeAnimations();
				this.ResetShooterAnimations();
				this.UpdateCameraStates();
				this.CancelAiming();
				return;
			}
			if ((this.shooterManager == null || !this.CurrentActiveWeapon) && this.meleeManager)
			{
				base.UpdateMeleeAnimations();
				this.onlyArmsLayerWeight = Mathf.Lerp(this.onlyArmsLayerWeight, 0f, 6f * vTime.fixedDeltaTime);
				base.animator.SetLayerWeight(this.onlyArmsLayer, this.onlyArmsLayerWeight);
				base.animator.SetBool(vAnimatorParameters.IsAiming, false);
				this.isReloading = false;
				return;
			}
			if (this.shooterManager && this.CurrentActiveWeapon)
			{
				this.UpdateShooterAnimations();
				return;
			}
			this.ResetMoveSet();
			this.ResetMeleeAnimations();
			this.ResetShooterAnimations();
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x000F0E3B File Offset: 0x000EF03B
		public virtual void ResetMoveSet()
		{
			this.cc.animator.SetFloat(vAnimatorParameters.MoveSet_ID, (float)this.defaultMoveSetID, 0.2f, Time.fixedDeltaTime);
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000F0E64 File Offset: 0x000EF064
		public virtual void ResetShooterAnimations()
		{
			if (this.shooterManager == null || !base.animator)
			{
				return;
			}
			base.animator.SetFloat(vAnimatorParameters.UpperBody_ID, 0f, 0.2f, vTime.fixedDeltaTime);
			base.animator.SetBool(vAnimatorParameters.CanAim, false);
			base.animator.SetBool(vAnimatorParameters.IsAiming, false);
			this.onlyArmsLayerWeight = Mathf.Lerp(this.onlyArmsLayerWeight, 0f, 6f * vTime.fixedDeltaTime);
			base.animator.SetLayerWeight(this.onlyArmsLayer, this.onlyArmsLayerWeight);
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x000F0F08 File Offset: 0x000EF108
		protected virtual void UpdateShooterAnimations()
		{
			if (this.shooterManager == null)
			{
				return;
			}
			this.onlyArmsLayerWeight = Mathf.Lerp(this.onlyArmsLayerWeight, (this.CurrentActiveWeapon || this.isEquipping) ? 1f : 0f, this.shooterManager.onlyArmsSpeed * vTime.fixedDeltaTime);
			base.animator.SetLayerWeight(this.onlyArmsLayer, this.onlyArmsLayerWeight);
			if (this.CurrentActiveWeapon && this.IsAiming)
			{
				base.animator.SetLayerWeight(this.shotLayer, this.isUsingScopeView ? this.CurrentActiveWeapon.scopeShootAnimationWeight : 1f);
			}
			if ((this.CurrentActiveWeapon && !this.shooterManager.useDefaultMovesetWhenNotAiming) || this.IsAiming)
			{
				base.animator.SetFloat(vAnimatorParameters.MoveSet_ID, (float)this.shooterMoveSetID, 0.1f, vTime.fixedDeltaTime);
			}
			else if ((!this.CurrentActiveWeapon && !this.shooterManager.useDefaultMovesetWhenNotAiming) || this.shooterManager.useDefaultMovesetWhenNotAiming)
			{
				base.animator.SetFloat(vAnimatorParameters.MoveSet_ID, (float)this.defaultMoveSetID, 0.1f, vTime.fixedDeltaTime);
			}
			base.animator.SetInteger(vAnimatorParameters.DefenseID, this.DefenseID);
			base.animator.SetBool(vAnimatorParameters.IsBlocking, this.isBlocking);
			base.animator.SetFloat(vAnimatorParameters.UpperBody_ID, (float)this.shooterManager.GetUpperBodyID());
			base.animator.SetBool(vAnimatorParameters.CanAim, this.aimConditions);
			base.animator.SetBool(vAnimatorParameters.IsAiming, this.IsAiming);
			this.isReloading = this.cc.IsAnimatorTag("IsReloading") || this.shooterManager.isReloadingWeapon;
			new vAnimatorParameter(base.animator, "IsReloading");
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x000F10F8 File Offset: 0x000EF2F8
		public virtual int shooterMoveSetID
		{
			get
			{
				int num = this.shooterManager.GetMoveSetID();
				if (num == 0 || this.overrideWeaponMoveSetID)
				{
					num = this.defaultMoveSetID;
				}
				return num;
			}
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000F1124 File Offset: 0x000EF324
		public override void UpdateCameraStates()
		{
			if (this.ignoreTpCamera)
			{
				return;
			}
			if (this.tpCamera == null)
			{
				this.tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
				if (this.tpCamera == null)
				{
					return;
				}
				if (this.tpCamera)
				{
					this.tpCamera.SetMainTarget(base.transform);
					this.tpCamera.Init();
				}
			}
			if (this.changeCameraState)
			{
				this.tpCamera.ChangeState(this.customCameraState, this.customlookAtPoint, true);
				return;
			}
			if (this.cc.isCrouching && !this.isAimingByInput)
			{
				this.tpCamera.ChangeState("Crouch", true);
				return;
			}
			if (this.cc.isStrafing && !this.isAimingByInput)
			{
				this.tpCamera.ChangeState("Strafing", true);
				return;
			}
			if (!this.isAimingByInput || !this.CurrentActiveWeapon)
			{
				this.tpCamera.ChangeState("Default", true);
				return;
			}
			if (this.isUsingScopeView)
			{
				if (string.IsNullOrEmpty(this.CurrentActiveWeapon.customScopeCameraState))
				{
					this.tpCamera.ChangeState(this.cc.isCrouching ? "CrouchingAiming" : "Aiming", true);
					return;
				}
				this.tpCamera.ChangeState(this.CurrentActiveWeapon.customScopeCameraState, true);
				return;
			}
			else
			{
				if (string.IsNullOrEmpty(this.CurrentActiveWeapon.customAimCameraState))
				{
					this.tpCamera.ChangeState(this.cc.isCrouching ? "CrouchingAiming" : "Aiming", true);
					return;
				}
				this.tpCamera.ChangeState(this.CurrentActiveWeapon.customAimCameraState, true);
				return;
			}
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000F12D0 File Offset: 0x000EF4D0
		protected virtual void UpdateDesiredAimPosition()
		{
			if (!this.shooterManager)
			{
				return;
			}
			if (this.CurrentActiveWeapon == null)
			{
				return;
			}
			if (this.isUsingScopeView)
			{
				this.DesiredAimPosition = this.cameraMain.transform.TransformPoint(this.localAimPosition);
				return;
			}
			Transform transform = this.cameraMain.transform;
			Vector3 position = transform.position;
			Vector3 vector = transform.position + transform.forward * this.cameraMain.farClipPlane;
			this.ray.origin = position;
			this.ray.direction = transform.forward;
			RaycastHit raycastHit;
			if (this.shooterManager.raycastAimTarget && this.CurrentActiveWeapon.raycastAimTarget && Physics.Raycast(this.ray, out raycastHit, this.cameraMain.farClipPlane, this.shooterManager.damageLayer))
			{
				bool flag = false;
				float num = this.cameraMain.farClipPlane;
				if (raycastHit.collider.transform.IsChildOf(base.transform))
				{
					Collider collider = raycastHit.collider;
					raycastHit = default(RaycastHit);
					RaycastHit[] array = Physics.RaycastAll(this.ray, this.cameraMain.farClipPlane, this.shooterManager.damageLayer);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].distance < num && array[i].collider.gameObject != collider.gameObject && !array[i].collider.transform.IsChildOf(base.transform))
						{
							num = array[i].distance;
							raycastHit = array[i];
							flag = true;
						}
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					vector = raycastHit.point;
				}
			}
			Vector3 vector2 = this.aimAngleReference.transform.InverseTransformPoint(vector);
			if (!this.isUsingScopeView)
			{
				if (!this.shooterManager.ignoreBackAimPoint)
				{
					vector2.z = Mathf.Max(vector2.z, this.muzzlePosition.z + this.shooterManager.backAimPointOffset + this.shooterManager.minAimHitPointDistance);
				}
				vector = this.aimAngleReference.transform.TransformPoint(vector2);
			}
			this.DesiredAimPosition = vector;
			this.localAimPosition = this.cameraMain.transform.InverseTransformPoint(this.DesiredAimPosition);
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000F154C File Offset: 0x000EF74C
		protected virtual void UpdateAimPosition()
		{
			if (!this.shooterManager)
			{
				return;
			}
			if (this.CurrentActiveWeapon == null)
			{
				return;
			}
			Vector3 vector = this.DesiredAimPosition - this.CurrentActiveWeapon.aimReference.position;
			this.ray.origin = this.CurrentActiveWeapon.aimReference.position;
			this.ray.direction = vector;
			Vector3 vector2 = this.DesiredAimPosition;
			LayerMask blockAimLayer = this.shooterManager.blockAimLayer;
			float magnitude = vector.magnitude;
			Ray ray = this.ray;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, magnitude, blockAimLayer))
			{
				bool flag = false;
				float num = this.cameraMain.farClipPlane;
				if (raycastHit.collider.transform.IsChildOf(base.transform))
				{
					Collider collider = raycastHit.collider;
					raycastHit = default(RaycastHit);
					RaycastHit[] array = Physics.RaycastAll(ray, magnitude, blockAimLayer);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].distance < num && array[i].collider.gameObject != collider.gameObject && !array[i].collider.transform.IsChildOf(base.transform))
						{
							num = array[i].distance;
							raycastHit = array[i];
							flag = true;
						}
					}
				}
				else
				{
					flag = true;
				}
				if (raycastHit.collider && flag)
				{
					vector2 = raycastHit.point;
				}
			}
			this.AimPosition = vector2;
			if (this.isUsingScopeView)
			{
				this.lastTargetCameraEuler = Quaternion.LookRotation(this.AimPosition - this.cameraMain.transform.position, this.cameraMain.transform.up).eulerAngles.NormalizeAngle();
			}
			if (this.isAimingByInput)
			{
				this.shooterManager.CameraSway();
			}
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000F1748 File Offset: 0x000EF948
		public override void ControlRotation()
		{
			base.ControlRotation();
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000F1750 File Offset: 0x000EF950
		public override void CameraInput()
		{
			base.CameraInput();
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000F1758 File Offset: 0x000EF958
		protected virtual void UpdateAimBehaviour()
		{
			if (this.cc.isDead || this.cc.ragdolled)
			{
				return;
			}
			this.UpdateDesiredAimPosition();
			this.UpdateHeadTrack();
			this.OnStartUpdateIK();
			this.CheckAimConditions();
			if (this.shooterManager && this.CurrentActiveWeapon)
			{
				this.UpdateIKAdjust(this.shooterManager.IsLeftWeapon);
				this.AlignArmToAimPosition(this.shooterManager.IsLeftWeapon);
				this.UpdateArmsIK(this.shooterManager.IsLeftWeapon);
			}
			this.UpdateAimPosition();
			this.OnFinishUpdateIK();
			this.UpdateAimHud();
			this.DoShots();
			this.CheckAimEvents();
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000F1804 File Offset: 0x000EFA04
		protected virtual void CheckAimEvents()
		{
			if (this.IsAiming != this.wasAiming)
			{
				this.wasAiming = this.IsAiming;
				if (this.IsAiming)
				{
					this.shooterManager.onEnableAim.Invoke(this.shooterManager.CurrentWeapon);
				}
				else
				{
					this.shooterManager.onDisableAim.Invoke(this.shooterManager.CurrentWeapon);
				}
			}
			if (this.isUsingScopeView != this.wasAimingWithScope)
			{
				this.wasAimingWithScope = this.isUsingScopeView;
				if (this.IsAiming)
				{
					this.shooterManager.onEnableScopeView.Invoke(this.shooterManager.CurrentWeapon);
					return;
				}
				this.shooterManager.onDisableScopeView.Invoke(this.shooterManager.CurrentWeapon);
			}
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000F18C4 File Offset: 0x000EFAC4
		protected virtual void UpdateAimAngleReference()
		{
			this.aimAngleReference.transform.rotation = base.transform.rotation;
			this.UpdateCheckAimHelpers(this.shooterManager.IsLeftWeapon);
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x000F18F4 File Offset: 0x000EFAF4
		protected void UpdateCheckAimHelpers(bool isUsingLeftHand)
		{
			if (!this.CurrentActiveWeapon)
			{
				return;
			}
			float num = (float)Math.Round((double)this.armAlignmentWeight, 1);
			Transform transform = (isUsingLeftHand ? this.leftUpperArm : this.rightUpperArm);
			this.upperArmPosition = Vector3.Lerp(this.upperArmPosition, this.aimAngleReference.transform.InverseTransformPoint(transform.position), num);
			this.muzzlePosition = Vector3.Lerp(this.muzzlePosition, this.aimAngleReference.transform.InverseTransformPoint(this.CurrentActiveWeapon.muzzle.position), num);
			this.muzzleForward = Vector3.Lerp(this.muzzleForward, this.aimAngleReference.transform.InverseTransformDirection(this.CurrentActiveWeapon.muzzle.forward), num);
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000F19BC File Offset: 0x000EFBBC
		public virtual void UpdateCheckAimPoints(ref Vector3 start, ref Vector3 end)
		{
			if (this.CurrentActiveWeapon)
			{
				float checkAimOffsetSmooth = this.shooterManager.checkAimOffsetSmooth;
				this.checkCanAimOffsetStartX = Mathf.Lerp(this.checkCanAimOffsetStartX, this.IsCrouching ? this.shooterManager.checkAimCrouchedOffsetStartX : this.shooterManager.checkAimStandingOffsetStartX, checkAimOffsetSmooth * Time.fixedDeltaTime);
				this.checkCanAimOffsetStartY = Mathf.Lerp(this.checkCanAimOffsetStartY, this.IsCrouching ? this.shooterManager.checkAimCrouchedOffsetStartY : this.shooterManager.checkAimStandingOffsetStartY, checkAimOffsetSmooth * Time.fixedDeltaTime);
				this.checkCanAimOffsetEndX = Mathf.Lerp(this.checkCanAimOffsetEndX, this.IsCrouching ? this.shooterManager.checkAimCrouchedOffsetEndX : this.shooterManager.checkAimStandingOffsetEndX, checkAimOffsetSmooth * Time.fixedDeltaTime);
				this.checkCanAimOffsetEndY = Mathf.Lerp(this.checkCanAimOffsetEndY, this.IsCrouching ? this.shooterManager.checkAimCrouchedOffsetEndY : this.shooterManager.checkAimStandingOffsetEndY, checkAimOffsetSmooth * Time.fixedDeltaTime);
				float checkCanAimOffsetStartX = this.checkCanAimOffsetStartX;
				float checkCanAimOffsetStartY = this.checkCanAimOffsetStartY;
				float checkCanAimOffsetEndX = this.checkCanAimOffsetEndX;
				float checkCanAimOffsetEndY = this.checkCanAimOffsetEndY;
				vShooterMeleeInput.UpdateCheckAimPointsEvent updateCheckAimPointsEvent = this.onUpdateCheckAimPointsEvent;
				if (updateCheckAimPointsEvent != null)
				{
					updateCheckAimPointsEvent(ref checkCanAimOffsetStartX, ref checkCanAimOffsetEndX, ref checkCanAimOffsetStartY, ref checkCanAimOffsetEndY);
				}
				Vector3 vector = this.aimAngleReference.transform.TransformPoint(this.upperArmPosition);
				float num = Vector3.Distance(vector, this.aimAngleReference.transform.TransformPoint(this.muzzlePosition));
				Vector3 vector2 = vector + (this.DesiredAimPosition - vector).normalized * num;
				Vector3 normalized = (this.DesiredAimPosition - vector).normalized;
				Vector3 vector3 = vector + this.cameraMain.transform.right * (checkCanAimOffsetStartX * (float)((this.tpCamera.switchRight > 0f) ? 1 : (-1))) + this.cameraMain.transform.up * checkCanAimOffsetStartY;
				Vector3 vector4 = vector2 + this.cameraMain.transform.right * (checkCanAimOffsetEndX * (float)((this.tpCamera.switchRight > 0f) ? 1 : (-1))) + this.cameraMain.transform.up * checkCanAimOffsetEndY + normalized * this.shooterManager.checkAimOffsetZ;
				start = vector3;
				end = vector4;
			}
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000F1C38 File Offset: 0x000EFE38
		protected virtual void OnFinishUpdateIK()
		{
			IKUpdateEvent ikupdateEvent = this.onFinishUpdateIK;
			if (ikupdateEvent == null)
			{
				return;
			}
			ikupdateEvent();
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x000F1C4A File Offset: 0x000EFE4A
		protected virtual void OnStartUpdateIK()
		{
			IKUpdateEvent ikupdateEvent = this.onStartUpdateIK;
			if (ikupdateEvent == null)
			{
				return;
			}
			ikupdateEvent();
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000F1C5C File Offset: 0x000EFE5C
		protected virtual void UpdateIKAdjust(bool isUsingLeftHand)
		{
			if (this.LeftIK == null || !this.LeftIK.isValidBones)
			{
				this.LeftIK = new vIKSolver(base.animator, AvatarIKGoal.LeftHand);
				this.LeftIK.UpdateIK();
			}
			if (this.RightIK == null || !this.RightIK.isValidBones)
			{
				this.RightIK = new vIKSolver(base.animator, AvatarIKGoal.RightHand);
				this.RightIK.UpdateIK();
			}
			if (this.WeaponIKAdjustList == null)
			{
				return;
			}
			this.CurrentActiveWeapon.handIKTargetOffset.localPosition = (isUsingLeftHand ? this.WeaponIKAdjustList.ikTargetPositionOffsetL : this.WeaponIKAdjustList.ikTargetPositionOffsetR);
			this.CurrentActiveWeapon.handIKTargetOffset.localEulerAngles = (isUsingLeftHand ? this.WeaponIKAdjustList.ikTargetRotationOffsetL : this.WeaponIKAdjustList.ikTargetRotationOffsetR);
			if (!this.CurrentWeaponIK)
			{
				this.LeftIK.UpdateIK();
				this.RightIK.UpdateIK();
				this.RightIK.SetIKWeight(0f);
				this.LeftIK.SetIKWeight(0f);
				this.weaponIKWeight = 0f;
				return;
			}
			bool flag = !this.IsIgnoreIK && !this.cc.customAction && !this.isReloading && !this.isEquipping && this.CurrentWeaponIK != null && this.CurrentIKAdjust != null;
			this.weaponIKWeight = Mathf.Lerp(this.weaponIKWeight, (float)(flag ? 1 : 0), this.shooterManager.ikAdjustSmooth * Time.fixedDeltaTime);
			if (this.weaponIKWeight <= 0f)
			{
				return;
			}
			if (isUsingLeftHand)
			{
				this.ApplyOffsets(this.LeftIK, this.RightIK, flag);
				return;
			}
			this.ApplyOffsets(this.RightIK, this.LeftIK, flag);
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000F1E24 File Offset: 0x000F0024
		protected virtual void ApplyOffsets(vIKSolver weaponHand, vIKSolver supportHand, bool isValidIK = true)
		{
			if (!weaponHand.isValidBones || !supportHand.isValidBones)
			{
				return;
			}
			weaponHand.SetIKWeight(this.shooterManager.ikAdjustWeightCurve.Evaluate(this.weaponIKWeight));
			this.ApplyOffsetToTargetBone(isValidIK ? this.CurrentIKAdjust.weaponHandOffset : null, weaponHand.endBoneOffset, isValidIK);
			this.ApplyOffsetToTargetBone(isValidIK ? this.CurrentIKAdjust.weaponHintOffset : null, weaponHand.middleBoneOffset, isValidIK);
			weaponHand.AnimationToIK();
			this.ApplyOffsetToTargetBone(isValidIK ? this.CurrentIKAdjust.supportHandOffset : null, supportHand.endBoneOffset, !this.EditingIKGlobalOffset && isValidIK);
			this.ApplyOffsetToTargetBone(isValidIK ? this.CurrentIKAdjust.supportHintOffset : null, supportHand.middleBoneOffset, !this.EditingIKGlobalOffset && isValidIK);
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000F1EF0 File Offset: 0x000F00F0
		protected virtual void ApplyOffsetToTargetBone(IKOffsetTransform iKOffset, Transform target, bool isValidIK)
		{
			try
			{
				target.localPosition = Vector3.Lerp(target.localPosition, isValidIK ? iKOffset.position : Vector3.zero, this.shooterManager.ikAdjustSmooth * vTime.fixedDeltaTime);
				target.localRotation = Quaternion.Lerp(target.localRotation, isValidIK ? Quaternion.Euler(iKOffset.eulerAngles) : Quaternion.Euler(Vector3.zero), this.shooterManager.ikAdjustSmooth * vTime.fixedDeltaTime);
			}
			catch
			{
				Debug.LogWarning("Can't Get IK Adjust");
			}
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000F1F8C File Offset: 0x000F018C
		protected virtual void UpdateArmsIK(bool isUsingLeftHand = false)
		{
			if (this.LeftIK == null || !this.LeftIK.isValidBones)
			{
				this.LeftIK = new vIKSolver(base.animator, AvatarIKGoal.LeftHand);
			}
			if (this.RightIK == null || !this.RightIK.isValidBones)
			{
				this.RightIK = new vIKSolver(base.animator, AvatarIKGoal.RightHand);
			}
			vIKSolver vIKSolver;
			if (isUsingLeftHand)
			{
				vIKSolver = this.RightIK;
			}
			else
			{
				vIKSolver = this.LeftIK;
			}
			bool flag = (isUsingLeftHand ? this.shooterManager.useLeftIK : this.shooterManager.useRightIK);
			if (!this.shooterManager || !this.CurrentActiveWeapon || this.IsIgnoreIK || this.isEquipping || (this.cc.IsAnimatorTag("Shot Fire") && this.CurrentActiveWeapon.disableIkOnShot))
			{
				flag = false;
			}
			bool flag2 = false;
			double num = Math.Round((double)this.cc.inputMagnitude, 1);
			if (!flag)
			{
				flag2 = false;
			}
			else if (!this.IsAiming && !this.isAttacking)
			{
				vShooterWeapon.IKLocomotionOptions iklocomotionOptions = (this.cc.isStrafing ? this.CurrentActiveWeapon.strafeIKOptions : this.CurrentActiveWeapon.freeIKOptions);
				if (iklocomotionOptions.use)
				{
					if (num <= 0.10000000149011612)
					{
						flag2 = iklocomotionOptions.useOnIdle;
					}
					else if (num <= 0.5)
					{
						flag2 = iklocomotionOptions.useOnWalk;
					}
					else if (num <= 1.0)
					{
						flag2 = iklocomotionOptions.useOnRun;
					}
					else if (num <= 1.5)
					{
						flag2 = iklocomotionOptions.useOnSprint;
					}
				}
				else
				{
					flag2 = false;
				}
			}
			else if (this.IsAiming && !this.isAttacking)
			{
				flag2 = (this.shooterManager.isShooting ? (!this.CurrentActiveWeapon.disableIkOnShot) : this.CurrentActiveWeapon.useIKOnAiming);
			}
			else if (this.isAttacking)
			{
				flag2 = this.CurrentActiveWeapon.useIkAttacking;
			}
			this.IsSupportHandIKEnabled = flag2 && !this.IsIgnoreSupportHandIK;
			if (vIKSolver != null)
			{
				if (this.shooterManager.weaponIKAdjustList)
				{
					if (isUsingLeftHand)
					{
						this.ikRotationOffset = this.shooterManager.weaponIKAdjustList.ikTargetRotationOffsetR;
						this.ikPositionOffset = this.shooterManager.weaponIKAdjustList.ikTargetPositionOffsetR;
					}
					else
					{
						this.ikRotationOffset = this.shooterManager.weaponIKAdjustList.ikTargetRotationOffsetL;
						this.ikPositionOffset = this.shooterManager.weaponIKAdjustList.ikTargetPositionOffsetL;
					}
				}
				if (this.CurrentActiveWeapon && this.CurrentActiveWeapon.handIKTargetOffset && !this.isReloading && !this.cc.customAction && (this.cc.isGrounded || this.IsAiming) && this.IsSupportHandIKEnabled)
				{
					this.supportIKWeight = Mathf.Lerp(this.supportIKWeight, 1f, this.shooterManager.armIKSmoothIn * vTime.fixedDeltaTime);
				}
				else
				{
					this.supportIKWeight = Mathf.Lerp(this.supportIKWeight, 0f, this.shooterManager.armIKSmoothOut * vTime.fixedDeltaTime);
				}
				if (this.supportIKWeight <= 0f)
				{
					if (!this.IsSupportHandIKEnabled && this.shooterManager.CurrentWeaponIK)
					{
						vIKSolver.SetIKWeight(this.shooterManager.armIKCurve.Evaluate(this.supportIKWeight));
						vIKSolver.AnimationToIK();
					}
					return;
				}
				vIKSolver.SetIKWeight(this.shooterManager.armIKCurve.Evaluate(this.supportIKWeight));
				if (this.shooterManager && this.CurrentActiveWeapon && this.CurrentActiveWeapon.handIKTargetOffset)
				{
					vIKSolver.SetIKPosition(this.CurrentActiveWeapon.handIKTargetOffset.position);
					vIKSolver.SetIKRotation(this.CurrentActiveWeapon.handIKTargetOffset.rotation);
					if (this.shooterManager.CurrentWeaponIK)
					{
						vIKSolver.AnimationToIK();
					}
				}
			}
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000F2377 File Offset: 0x000F0577
		protected virtual bool CanRotateAimArm()
		{
			return this.cc.IsAnimatorTag("Upperbody Pose") && this.cc.animatorStateInfos.GetCurrentNormalizedTime(this.cc.upperBodyLayer) > 0.5f;
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000F23AF File Offset: 0x000F05AF
		protected virtual bool CanDoShots()
		{
			return this.armAlignmentWeight >= 0.5f && this.cc.IsAnimatorTag("Upperbody Pose") && this.shootCountA > 0 && !this.isReloading;
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x000F23E4 File Offset: 0x000F05E4
		protected virtual void AlignArmToAimPosition(bool isUsingLeftHand = false)
		{
			if (!this.shooterManager)
			{
				return;
			}
			if (this.leftArmAim == null)
			{
				this.leftArmAim = new vArmAimAlign(this.leftUpperArm, this.leftLowerArm, this.leftHand);
			}
			if (this.rightArmAim == null)
			{
				this.rightArmAim = new vArmAimAlign(this.rightUpperArm, this.rightLowerArm, this.rightHand);
			}
			vArmAimAlign vArmAimAlign = (isUsingLeftHand ? this.leftArmAim : this.rightArmAim);
			this.armAlignmentWeight = ((this.IsAiming && this.aimConditions && this.CanRotateAimArm()) ? Mathf.Lerp(this.armAlignmentWeight, Mathf.Clamp(this.cc.upperBodyInfo.normalizedTime, 0f, 1f), this.shooterManager.smoothArmWeight * (0.001f + Time.fixedDeltaTime)) : 0f);
			if (this.CurrentActiveWeapon)
			{
				if (!this.shooterManager.isShooting)
				{
					vArmAimAlign.UpdateDefaultAlignment();
				}
				else
				{
					vArmAimAlign.RestoreToLastAlignment();
				}
				vArmAimAlign.smoothIKAlignmentPoint = this.shooterManager.smoothIKAlignmentPoint;
				vArmAimAlign.aimReference = this.CurrentActiveWeapon.aimReference;
				vArmAimAlign.smooth = this.shooterManager.smoothArmIKRotation;
				vArmAimAlign.maxVerticalAligmentAngle = this.shooterManager.maxVerticalAimAngle;
				vArmAimAlign.maxHorizontalAligmentAngle = this.shooterManager.maxHorizontalAimAngle;
				if (this.shooterManager.showCheckAimGizmos)
				{
					vArmAimAlign.DrawBones(Color.blue);
				}
				vArmAimAlign.AlignToArmToPosition(this.targetArmAlignmentPosition, this.armAlignmentWeight, this.CurrentActiveWeapon.alignRightUpperArmToAim, this.CurrentActiveWeapon.alignRightHandToAim);
				if (this.shooterManager.showCheckAimGizmos)
				{
					vArmAimAlign.DrawHelpers(Color.green);
				}
			}
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x000F259C File Offset: 0x000F079C
		protected virtual void CheckAimConditions()
		{
			if (!this.shooterManager)
			{
				return;
			}
			float switchRight = this.tpCamera.switchRight;
			if (this.CurrentActiveWeapon == null)
			{
				this.aimConditions = false;
				return;
			}
			if (!this.shooterManager.useCheckAim)
			{
				this.aimConditions = true;
				return;
			}
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Ray ray = new Ray(zero, (zero2 - zero).normalized);
			bool flag = this.aimConditions;
			if (base.animator.IsInTransition(0))
			{
				if (Physics.SphereCast(ray, this.shooterManager.checkAimRadius, out this.checkCanAimHit, (zero2 - zero).magnitude, this.shooterManager.blockAimLayer))
				{
					this.aimConditions = false;
				}
				flag = this.aimConditions;
				vShooterMeleeInput.CheckAimConditionsEvent checkAimConditionsEvent = this.checkAimConditionOverride;
				if (checkAimConditionsEvent != null)
				{
					checkAimConditionsEvent(zero, zero2, ref flag);
				}
				this.aimConditions = flag;
				return;
			}
			this.UpdateCheckAimPoints(ref zero, ref zero2);
			ray = new Ray(zero, (zero2 - zero).normalized);
			if (Vector3.Distance(zero, this.AimPosition) < Vector3.Distance(zero, zero2))
			{
				this.aimConditions = false;
			}
			if (Physics.SphereCast(ray, this.shooterManager.checkAimRadius, out this.checkCanAimHit, (zero2 - zero).magnitude, this.shooterManager.blockAimLayer))
			{
				this.aimConditions = false;
			}
			else
			{
				this.aimConditions = true;
			}
			flag = this.aimConditions;
			vShooterMeleeInput.CheckAimConditionsEvent checkAimConditionsEvent2 = this.checkAimConditionOverride;
			if (checkAimConditionsEvent2 != null)
			{
				checkAimConditionsEvent2(zero, zero2, ref flag);
			}
			this.aimConditions = flag;
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x000F273C File Offset: 0x000F093C
		protected virtual Vector3 targetArmAlignmentPosition
		{
			get
			{
				if (!this.shooterManager.alignArmToHitPoint && !this.isUsingScopeView)
				{
					return this.cameraMain.transform.position + this.cameraMain.transform.forward * 100f;
				}
				return this.DesiredAimPosition;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002B68 RID: 11112 RVA: 0x000F2794 File Offset: 0x000F0994
		protected virtual Transform targetCamera
		{
			get
			{
				if (!this.controlAimCanvas || !this.controlAimCanvas.isScopeCameraActive || !this.controlAimCanvas.scopeBackgroundCamera)
				{
					return this.cameraMain.transform;
				}
				return this.controlAimCanvas.scopeBackgroundCamera.transform;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06002B69 RID: 11113 RVA: 0x000F27E9 File Offset: 0x000F09E9
		protected virtual Vector3 targetArmAlignmentDirection
		{
			get
			{
				return this.targetCamera.forward;
			}
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000F27F8 File Offset: 0x000F09F8
		protected virtual void UpdateHeadTrack()
		{
			if (this.headTrack)
			{
				this.UpdateHeadTrackLookPoint();
			}
			if (!this.shooterManager || !this.headTrack)
			{
				if (this.headTrack)
				{
					this.headTrack.offsetSpine = Vector2.Lerp(this.headTrack.offsetSpine, Vector2.zero, this.weaponIKWeight);
					this.headTrack.offsetHead = Vector2.Lerp(this.headTrack.offsetHead, Vector2.zero, this.weaponIKWeight);
				}
				return;
			}
			if (!this.CurrentActiveWeapon || !this.headTrack || !this.CurrentWeaponIK || this.CurrentIKAdjust == null)
			{
				if (this.headTrack)
				{
					this.headTrack.offsetSpine = Vector2.Lerp(this.headTrack.offsetSpine, Vector2.zero, this.weaponIKWeight);
					this.headTrack.offsetHead = Vector2.Lerp(this.headTrack.offsetHead, Vector2.zero, this.weaponIKWeight);
				}
				return;
			}
			if (this.IsAiming)
			{
				IKAdjust currentIKAdjust = this.CurrentIKAdjust;
				Vector2 spine = currentIKAdjust.spineOffset.spine;
				Vector2 head = currentIKAdjust.spineOffset.head;
				this.headTrack.offsetSpine = Vector2.Lerp(this.headTrack.offsetSpine, spine, this.weaponIKWeight);
				this.headTrack.offsetHead = Vector2.Lerp(this.headTrack.offsetHead, head, this.weaponIKWeight);
				return;
			}
			IKAdjust currentIKAdjust2 = this.CurrentIKAdjust;
			Vector2 spine2 = currentIKAdjust2.spineOffset.spine;
			Vector2 head2 = currentIKAdjust2.spineOffset.head;
			this.headTrack.offsetSpine = Vector2.Lerp(this.headTrack.offsetSpine, spine2, this.headTrack.Smooth);
			this.headTrack.offsetHead = Vector2.Lerp(this.headTrack.offsetHead, head2, this.headTrack.Smooth);
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x000F29EA File Offset: 0x000F0BEA
		protected virtual void UpdateHeadTrackLookPoint()
		{
			if (this.IsAiming && this.CurrentActiveWeapon && this.aimConditions)
			{
				this.headTrack.SetTemporaryLookPoint(this.targetArmAlignmentPosition, 1f);
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x000F2A20 File Offset: 0x000F0C20
		protected virtual void UpdateAimHud()
		{
			if (!this.shooterManager || !this.controlAimCanvas)
			{
				return;
			}
			if (this.CurrentActiveWeapon == null)
			{
				return;
			}
			Vector3 aimPosition = this.AimPosition;
			this.controlAimCanvas.SetAimCanvasID(this.CurrentActiveWeapon.scopeID);
			if (this.controlAimCanvas.scopeBackgroundCamera && this.controlAimCanvas.scopeBackgroundCamera.gameObject.activeSelf)
			{
				this.controlAimCanvas.SetAimToCenter(true);
			}
			else if (this.IsAiming && this.aimConditions)
			{
				float num = Vector3.Distance(this.aimAngleReference.transform.position, aimPosition);
				bool flag = (num > this.shooterManager.maxAimHitPointIndicator || Vector3.Distance(this.AimPosition, this.DesiredAimPosition) < 0.1f) && this.aimConditions;
				if (num > this.shooterManager.maxAimHitPointIndicator || Vector3.Distance(this.AimPosition, this.DesiredAimPosition) < 0.1f)
				{
					this.controlAimCanvas.SetAimToCenter(true);
				}
				else
				{
					this.controlAimCanvas.SetWordPosition(aimPosition, flag);
				}
			}
			else
			{
				this.controlAimCanvas.SetAimToCenter(this.aimConditions);
			}
			if (this.controlAimCanvas.scopeBackgroundCamera && this.CurrentActiveWeapon.scopeTarget)
			{
				if (this.isUsingScopeView || this.controlAimCanvas.isScopeCameraActive)
				{
					vArmAimAlign vArmAimAlign = (this.shooterManager.IsLeftWeapon ? this.leftArmAim : this.rightArmAim);
					if (!this.shooterManager.isShooting)
					{
						this.scopeLocalForward = vArmAimAlign.aimReferenceHelper.InverseTransformDirection(this.CurrentActiveWeapon.scopeTarget.forward);
						this.scopeLocalPosition = vArmAimAlign.aimReferenceHelper.InverseTransformPoint(this.CurrentActiveWeapon.scopeTarget.position);
						this.scopeCameraLocalForward = this.aimAngleReference.transform.InverseTransformDirection(this.controlAimCanvas.scopeBackgroundCamera.transform.forward);
						this.scopeCameraLocalPosition = this.aimAngleReference.transform.InverseTransformPoint(this.controlAimCanvas.scopeBackgroundCamera.transform.position);
					}
					float num2 = (this.shooterManager.isShooting ? (1f - this.CurrentActiveWeapon.scopeShootAnimationWeight) : 1f);
					Vector3 vector = this.aimAngleReference.transform.TransformPoint(this.scopeCameraLocalPosition);
					Vector3 vector2 = Vector3.Lerp(this.aimAngleReference.transform.TransformDirection(this.scopeCameraLocalForward), vArmAimAlign.aimReferenceHelper.TransformDirection(this.scopeLocalForward), this.scopeDirectionWeight = Mathf.Lerp(this.scopeDirectionWeight, 1.001f, 10f / this.CurrentActiveWeapon.shootFrequency * Time.fixedDeltaTime));
					Vector3 vector3 = Vector3.Lerp(vector, vArmAimAlign.aimReferenceHelper.TransformPoint(this.scopeLocalPosition), num2);
					this.controlAimCanvas.UpdateScopeCamera(vector3, vector2, this.aimAngleReference.transform.up, this.CurrentActiveWeapon.backGroundScopeZoom, this.IsAiming);
					return;
				}
				this.scopeDirectionWeight = 1f;
			}
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x000B0592 File Offset: 0x000AE792
		GameObject vIShooterIKController.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x04002A91 RID: 10897
		[vEditorToolbar("Inputs", false, "", false, false)]
		[Header("Shooter Inputs")]
		public GenericInput aimInput = new GenericInput("Mouse1", false, "LT", true, "LT", false);

		// Token: 0x04002A92 RID: 10898
		public GenericInput shotInput = new GenericInput("Mouse0", false, "RT", true, "RT", false);

		// Token: 0x04002A93 RID: 10899
		public GenericInput reloadInput = new GenericInput("R", "LB", "LB");

		// Token: 0x04002A94 RID: 10900
		public GenericInput switchCameraSideInput = new GenericInput("Tab", "RightStickClick", "RightStickClick");

		// Token: 0x04002A95 RID: 10901
		public GenericInput scopeViewInput = new GenericInput("Z", "RB", "RB");

		// Token: 0x04002A96 RID: 10902
		[HideInInspector]
		public vShooterManager shooterManager;

		// Token: 0x04002A97 RID: 10903
		public vArmAimAlign leftArmAim;

		// Token: 0x04002A98 RID: 10904
		public vArmAimAlign rightArmAim;

		// Token: 0x04002AA5 RID: 10917
		protected bool _isUsingScopeView;

		// Token: 0x04002AC6 RID: 10950
		protected vHeadTrack headTrack;

		// Token: 0x04002AC7 RID: 10951
		protected vControlAimCanvas _controlAimCanvas;

		// Token: 0x04002AC8 RID: 10952
		protected IKAdjust _currentIKAdjust;

		// Token: 0x04002AC9 RID: 10953
		protected RaycastHit checkCanAimHit;

		// Token: 0x04002ACA RID: 10954
		protected Transform leftHand;

		// Token: 0x04002ACB RID: 10955
		protected Transform rightHand;

		// Token: 0x04002ACC RID: 10956
		protected Transform rightLowerArm;

		// Token: 0x04002ACD RID: 10957
		protected Transform leftLowerArm;

		// Token: 0x04002ACE RID: 10958
		protected Transform rightUpperArm;

		// Token: 0x04002ACF RID: 10959
		protected Transform leftUpperArm;

		// Token: 0x04002ADC RID: 10972
		protected Ray ray;

		// Token: 0x04002ADD RID: 10973
		public vShooterMeleeInput.UpdateCheckAimPointsEvent onUpdateCheckAimPointsEvent;

		// Token: 0x04002ADE RID: 10974
		public vShooterMeleeInput.CheckAimConditionsEvent checkAimConditionOverride;

		// Token: 0x020006C1 RID: 1729
		// (Invoke) Token: 0x06002B70 RID: 11120
		public delegate void UpdateCheckAimPointsEvent(ref float startX, ref float endX, ref float startY, ref float endY);

		// Token: 0x020006C2 RID: 1730
		// (Invoke) Token: 0x06002B74 RID: 11124
		public delegate void CheckAimConditionsEvent(Vector3 startPoint, Vector3 endPoint, ref bool aimConditions);
	}
}
