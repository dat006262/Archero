




namespace TableTool
{
	[System.Serializable]
	public class Character_Char : LocalBean
	{
		public int CharID
	;

		public int TypeID
	;
		// public enum EntityType
		// {
		// 	Invalid  = 0,
		// 	Hero     = 1,
		// 	Soldier  = 11,
		// 	Elite    = 12,
		// 	Boss     = 21,
		// 	Baby     = 0x1F,
		// 	PartBody = 0x20,
		// 	Tower    = 33,
		// 	Ornament = 51
		// }

		public string ModelID
	;
		//VD::Game/Models/3081_01
		//	return Utils.FormatString("Game/Models/{0}", value);

		public float BodyScale
	;
		//Bosss to hon
		public string TextureID
	;
	//Model tường là model không màu, phải gắn thêm texture nữa mới có màu
	// Cau truc: Utils.FormatString("Game/ModelsTexture/{0}", textureid);
		public int WeaponID
	;
		//Mot vai AI co su dung vũ khí

		public int Attackrangetype
	;
	// Su dụng khi quái chịu thêm hoặc bớt sát thương tầm gần hoặc xa: 1 là gần /2 là xa/0 là ko có
		public int Speed
	;
		//Speed

		public int HP
	;
		//HP

		public int RotateSpeed
	;
		//Tốc độ quay thường là 1000/hoặc 1200
		// Số ít có tốc độ xoay thấp VD:Quái ong vs ID là 3033 co RotationSpeed= 120
		

		public int BodyAttack
	;

		//Satthuong
		public int Divide
	;
	//Đa số Divide =0 
	//trừ mot vai quai VD:CharID 3008 có Divide = 1

		public int[] Skills
	;
//Thường không có hặc có 1 skill là 1000007 
//Một vài có thêm skil 1000003,1000004,1000005,1000006
//Các Skill này giống skill người choi 1 vài quái có the so huu 
//1000001: bắn 2 đường đạn về phía trước
//1000003: Đạn nảy tường
//1000004: Bắn thêm 1 viên phía sau
//1000005: Ban 2 đường đạn sang 2 ben
//1000006: yếu máu tăng dame
//1000008: 2 viên đan liên tiếp
//1000007: chưa kiểm tra
		public float BackRatio
	;
		//BackRatio sẽ chứa giá trị từ 0-1
		//LÀ hệ số đẩy lùi khi trúng đạn. 0 sẽ ko bị đẩy lùi
		public float[] ActionSpeed
	;
		//10 giá trị thời gian hành động
		//0 : Idle
		//1: Run
		//2: Hitted
		//3: AttackPrev
		//4: AttackEnd
		//5: dead
		//6: call
		//7:skill
		//8: Continuous
		//9: Dizzy

		public int HittedEffectID
	;
		//Id am thanh Hit

		public int DeadSoundID
	;
		//Id am thanh Dead

		public int Cache
	;
		//Đa số là 0 . Tuc là Sẽ ko lưu trữ Entity khi chế mà trực tiếp Destroy
		// Một vài là 10. Lưu trữ tối đa 10 go trong Pool

		protected override bool ReadImpl()
		{
			CharID = readInt();
			TypeID = readInt();
			ModelID = readLocalString();
			BodyScale = readFloat();
			TextureID = readLocalString();
			WeaponID = readInt();
			Attackrangetype = readInt();
			Speed = readInt();
			HP = readInt();
			RotateSpeed = readInt();
			BodyAttack = readInt();
			Divide = readInt();
			Skills = readArrayint();
			BackRatio = readFloat();
			ActionSpeed = readArrayfloat();
			HittedEffectID = readInt();
			DeadSoundID = readInt();
			Cache = readInt();
			return true;
		}

		public Character_Char Copy()
		{
			Character_Char character_Char = new Character_Char();
			character_Char.CharID = CharID;
			character_Char.TypeID = TypeID;
			character_Char.ModelID = ModelID;
			character_Char.BodyScale = BodyScale;
			character_Char.TextureID = TextureID;
			character_Char.WeaponID = WeaponID;
			character_Char.Attackrangetype = Attackrangetype;
			character_Char.Speed = Speed;
			character_Char.HP = HP;
			character_Char.RotateSpeed = RotateSpeed;
			character_Char.BodyAttack = BodyAttack;
			character_Char.Divide = Divide;
			character_Char.Skills = Skills;
			character_Char.BackRatio = BackRatio;
			character_Char.ActionSpeed = ActionSpeed;
			character_Char.HittedEffectID = HittedEffectID;
			character_Char.DeadSoundID = DeadSoundID;
			character_Char.Cache = Cache;
			return character_Char;
		}
	}
}
