




namespace TableTool
{
	public class Weapon_weapon : LocalBean
	{
		private bool bInitSpecial;

		private bool bCachep;

		private bool bThroughWallp;

		private bool bThroughEntityp;

		private bool bThroughInsideWallp;

		private bool bMoreHitp;

		private LayerManager.BulletLayer mLayer;

		public int WeaponID
		{
			get;
			private set;
		}

		public int SpecialAttribute
		{
			get;
			private set;
		}

		public string ModelID
		{
			get;
			private set;
		}

		public float ModelScale
		{
			get;
			private set;
		}

		public string[] Attributes
		{
			get;
			private set;
		}

		public int DebuffID
		{
			get;
			private set;
		}

		public int LookCamera
		{
			get;
			private set;
		}

		public int Attack
		{
			get;
			private set;
		}

		public float Distance
		{
			get;
			private set;
		}

		public float Speed
		{
			get;
			private set;
		}

		public float AttackSpeed
		{
			get;
			private set;
		}

		public float RandomAngle
		{
			get;
			private set;
		}

		public int WeaponNode
		{
			get;
			private set;
		}

		public int CreateNode
		{
			get;
			private set;
		}

		public float RotateSpeed
		{
			get;
			private set;
		}

		public string AttackPrevString
		{
			get;
			private set;
		}

		public string AttackEndString
		{
			get;
			private set;
		}

		public int Ballistic
		{
			get;
			private set;
		}

		public float BackRatio
		{
			get;
			private set;
		}

		public string CreatePath
		{
			get;
			private set;
		}

		public int CreateSoundID
		{
			get;
			private set;
		}

		public int DeadSoundID
		{
			get;
			private set;
		}

		public int HitWallSoundID
		{
			get;
			private set;
		}

		public int HittedEffectID
		{
			get;
			private set;
		}

		public int AliveTime
		{
			get;
			private set;
		}

		public int DeadDelay
		{
			get;
			private set;
		}

		public int DeadEffectID
		{
			get;
			private set;
		}

		public int DeadNode
		{
			get;
			private set;
		}

		public string[] Args
		{
			get;
			private set;
		}

		public bool bCache
		{
			get
			{
				InitSpecial();
				return bCachep;
			}
		}

		public bool bThroughWall
		{
			get
			{
				InitSpecial();
				return bThroughWallp;
			}
		}

		public bool bThroughEntity
		{
			get
			{
				InitSpecial();
				return bThroughEntityp || bCachep;
			}
		}

		public bool bThroughInsideWall
		{
			get
			{
				InitSpecial();
				return bThroughInsideWallp;
			}
		}

		public bool bMoreHit
		{
			get
			{
				InitSpecial();
				return bMoreHitp;
			}
		}

		protected override bool ReadImpl()
		{
			WeaponID = readInt();
			SpecialAttribute = readInt();
			ModelID = readLocalString();
			ModelScale = readFloat();
			Attributes = readArraystring();
			DebuffID = readInt();
			LookCamera = readInt();
			Attack = readInt();
			Distance = readFloat();
			Speed = readFloat();
			AttackSpeed = readFloat();
			RandomAngle = readFloat();
			WeaponNode = readInt();
			CreateNode = readInt();
			RotateSpeed = readFloat();
			AttackPrevString = readLocalString();
			AttackEndString = readLocalString();
			Ballistic = readInt();
			BackRatio = readFloat();
			CreatePath = readLocalString();
			CreateSoundID = readInt();
			DeadSoundID = readInt();
			HitWallSoundID = readInt();
			HittedEffectID = readInt();
			AliveTime = readInt();
			DeadDelay = readInt();
			DeadEffectID = readInt();
			DeadNode = readInt();
			Args = readArraystring();
			return true;
		}

		public Weapon_weapon Copy()
		{
			Weapon_weapon weapon_weapon = new Weapon_weapon();
			weapon_weapon.WeaponID = WeaponID;
			weapon_weapon.SpecialAttribute = SpecialAttribute;
			weapon_weapon.ModelID = ModelID;
			weapon_weapon.ModelScale = ModelScale;
			weapon_weapon.Attributes = Attributes;
			weapon_weapon.DebuffID = DebuffID;
			weapon_weapon.LookCamera = LookCamera;
			weapon_weapon.Attack = Attack;
			weapon_weapon.Distance = Distance;
			weapon_weapon.Speed = Speed;
			weapon_weapon.AttackSpeed = AttackSpeed;
			weapon_weapon.RandomAngle = RandomAngle;
			weapon_weapon.WeaponNode = WeaponNode;
			weapon_weapon.CreateNode = CreateNode;
			weapon_weapon.RotateSpeed = RotateSpeed;
			weapon_weapon.AttackPrevString = AttackPrevString;
			weapon_weapon.AttackEndString = AttackEndString;
			weapon_weapon.Ballistic = Ballistic;
			weapon_weapon.BackRatio = BackRatio;
			weapon_weapon.CreatePath = CreatePath;
			weapon_weapon.CreateSoundID = CreateSoundID;
			weapon_weapon.DeadSoundID = DeadSoundID;
			weapon_weapon.HitWallSoundID = HitWallSoundID;
			weapon_weapon.HittedEffectID = HittedEffectID;
			weapon_weapon.AliveTime = AliveTime;
			weapon_weapon.DeadDelay = DeadDelay;
			weapon_weapon.DeadEffectID = DeadEffectID;
			weapon_weapon.DeadNode = DeadNode;
			weapon_weapon.Args = Args;
			return weapon_weapon;
		}

		public int GetLayer()
		{
			if (bThroughWall)
			{
				mLayer = LayerManager.BulletLayer.eNone;
			}
			else if (bThroughInsideWall)
			{
				mLayer = LayerManager.BulletLayer.eOnlyOut;
			}
			else
			{
				mLayer = LayerManager.BulletLayer.eAll;
			}
			return LayerManager.GetBullet(mLayer);
		}

		private void InitSpecial()
		{
			if (!bInitSpecial)
			{
				bInitSpecial = true;
				bCachep = false;
				bThroughWallp = false;
				bThroughEntityp = false;
				bThroughInsideWallp = false;
				bMoreHitp = false;
				int num = SpecialAttribute;
				if (num >= 16)
				{
					bMoreHitp = true;
					num -= 16;
				}
				if (num >= 8)
				{
					bThroughInsideWallp = true;
					num -= 8;
				}
				if (num >= 4)
				{
					bThroughEntityp = true;
					num -= 4;
				}
				if (num >= 2)
				{
					bThroughWallp = true;
					num -= 2;
				}
				if (num >= 1)
				{
					bCachep = true;
					num--;
				}
			}
		}
	}
}
