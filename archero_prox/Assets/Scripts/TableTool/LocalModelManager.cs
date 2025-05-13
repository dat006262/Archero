




namespace TableTool
{
	public class LocalModelManager
	{
		private static volatile LocalModelManager _Instance;

		private Goods_foodModel _Goods_foodModel;

		private Equip_equipModel _Equip_equipModel;

		private Skill_slotoutcostModel _Skill_slotoutcostModel;

		private Config_configModel _Config_configModel;

		private Box_SilverNormalBoxModel _Box_SilverNormalBoxModel;

		private Skill_slotinModel _Skill_slotinModel;

		private Room_eventgameturnModel _Room_eventgameturnModel;

		private Operation_moveModel _Operation_moveModel;

		private Character_BabyModel _Character_BabyModel;

		private Shop_itemModel _Shop_itemModel;

		private Character_CharModel _Character_CharModel;

		private Language_lauguageModel _Language_lauguageModel;

		private Skill_dropinModel _Skill_dropinModel;

		private Drop_FakeDropModel _Drop_FakeDropModel;

		private Room_eventangelskillModel _Room_eventangelskillModel;

		private Box_ChapterBoxModel _Box_ChapterBoxModel;

		private Equip_UpgradeModel _Equip_UpgradeModel;

		private Stage_Level_activityModel _Stage_Level_activityModel;

		private Character_LevelModel _Character_LevelModel;

		private Room_eventdemontext2loseModel _Room_eventdemontext2loseModel;

		private Achieve_AchieveModel _Achieve_AchieveModel;

		private Sound_soundModel _Sound_soundModel;

		private Box_ActivityModel _Box_ActivityModel;

		private Fx_fxModel _Fx_fxModel;

		private Shop_MysticShopModel _Shop_MysticShopModel;

		private Curve_curveModel _Curve_curveModel;

		private Equip2_equip2Model _Equip2_equip2Model;

		private Stage_Level_chapter12Model _Stage_Level_chapter12Model;

		private Stage_Level_chapter13Model _Stage_Level_chapter13Model;

		private Stage_Level_chapter10Model _Stage_Level_chapter10Model;

		private Stage_Level_chapter11Model _Stage_Level_chapter11Model;

		private Buff_aloneModel _Buff_aloneModel;

		private Goods_goodsModel _Goods_goodsModel;

		private Box_SilverBoxModel _Box_SilverBoxModel;

		private Room_soldierupModel _Room_soldierupModel;

		private Stage_Level_chapter9Model _Stage_Level_chapter9Model;

		private Test_AttrValueModel _Test_AttrValueModel;

		private Stage_Level_chapter7Model _Stage_Level_chapter7Model;

		private Stage_Level_chapter8Model _Stage_Level_chapter8Model;

		private Stage_Level_chapter5Model _Stage_Level_chapter5Model;

		private Stage_Level_chapter6Model _Stage_Level_chapter6Model;

		private Stage_Level_chapter3Model _Stage_Level_chapter3Model;

		private Room_roomModel _Room_roomModel;

		private Room_eventdemontext2skillModel _Room_eventdemontext2skillModel;

		private Stage_Level_chapter4Model _Stage_Level_chapter4Model;

		private Stage_Level_chapter1Model _Stage_Level_chapter1Model;

		private Room_colorstyleModel _Room_colorstyleModel;

		private Stage_Level_chapter2Model _Stage_Level_chapter2Model;

		private Stage_Level_stagechapterModel _Stage_Level_stagechapterModel;

		private Shop_MysticShopShowModel _Shop_MysticShopShowModel;

		private Drop_harvestModel _Drop_harvestModel;

		private Box_TimeBoxModel _Box_TimeBoxModel;

		private Preload_loadModel _Preload_loadModel;

		private Room_levelModel _Room_levelModel;

		private Shop_ReadyShopModel _Shop_ReadyShopModel;

		private Skill_superModel _Skill_superModel;

		private Shop_ShopModel _Shop_ShopModel;

		private Soldier_standardModel _Soldier_standardModel;

		private Skill_aloneModel _Skill_aloneModel;

		private Goods_waterModel _Goods_waterModel;

		private Soldier_soldierModel _Soldier_soldierModel;

		private Beat_beatModel _Beat_beatModel;

		private Weapon_weaponModel _Weapon_weaponModel;

		private Skill_skillModel _Skill_skillModel;

		private Skill_greedyskillModel _Skill_greedyskillModel;

		private Shop_GoldModel _Shop_GoldModel;

		private Skill_slotfirstModel _Skill_slotfirstModel;

		private Exp_expModel _Exp_expModel;

		private Drop_GoldModel _Drop_GoldModel;

		private Drop_DropModel _Drop_DropModel;

		private Stage_Level_activitylevelModel _Stage_Level_activitylevelModel;

		private Skill_slotoutModel _Skill_slotoutModel;

		public static LocalModelManager Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new LocalModelManager();
				}
				return _Instance;
			}
		}

		public Goods_foodModel Goods_food
		{
			get
			{
				if (_Goods_foodModel == null)
				{
					_Goods_foodModel = new Goods_foodModel();
				}
				return _Goods_foodModel;
			}
		}

		public Equip_equipModel Equip_equip
		{
			get
			{
				if (_Equip_equipModel == null)
				{
					_Equip_equipModel = new Equip_equipModel();
				}
				return _Equip_equipModel;
			}
		}

		public Skill_slotoutcostModel Skill_slotoutcost
		{
			get
			{
				if (_Skill_slotoutcostModel == null)
				{
					_Skill_slotoutcostModel = new Skill_slotoutcostModel();
				}
				return _Skill_slotoutcostModel;
			}
		}

		public Config_configModel Config_config
		{
			get
			{
				if (_Config_configModel == null)
				{
					_Config_configModel = new Config_configModel();
				}
				return _Config_configModel;
			}
		}

		public Box_SilverNormalBoxModel Box_SilverNormalBox
		{
			get
			{
				if (_Box_SilverNormalBoxModel == null)
				{
					_Box_SilverNormalBoxModel = new Box_SilverNormalBoxModel();
				}
				return _Box_SilverNormalBoxModel;
			}
		}

		public Skill_slotinModel Skill_slotin
		{
			get
			{
				if (_Skill_slotinModel == null)
				{
					_Skill_slotinModel = new Skill_slotinModel();
				}
				return _Skill_slotinModel;
			}
		}

		public Room_eventgameturnModel Room_eventgameturn
		{
			get
			{
				if (_Room_eventgameturnModel == null)
				{
					_Room_eventgameturnModel = new Room_eventgameturnModel();
				}
				return _Room_eventgameturnModel;
			}
		}

		public Operation_moveModel Operation_move
		{
			get
			{
				if (_Operation_moveModel == null)
				{
					_Operation_moveModel = new Operation_moveModel();
				}
				return _Operation_moveModel;
			}
		}

		public Character_BabyModel Character_Baby
		{
			get
			{
				if (_Character_BabyModel == null)
				{
					_Character_BabyModel = new Character_BabyModel();
				}
				return _Character_BabyModel;
			}
		}

		public Shop_itemModel Shop_item
		{
			get
			{
				if (_Shop_itemModel == null)
				{
					_Shop_itemModel = new Shop_itemModel();
				}
				return _Shop_itemModel;
			}
		}

		public Character_CharModel Character_Char
		{
			get
			{
				if (_Character_CharModel == null)
				{
					_Character_CharModel = new Character_CharModel();
				}
				return _Character_CharModel;
			}
		}

		public Language_lauguageModel Language_lauguage
		{
			get
			{
				if (_Language_lauguageModel == null)
				{
					_Language_lauguageModel = new Language_lauguageModel();
				}
				return _Language_lauguageModel;
			}
		}

		public Skill_dropinModel Skill_dropin
		{
			get
			{
				if (_Skill_dropinModel == null)
				{
					_Skill_dropinModel = new Skill_dropinModel();
				}
				return _Skill_dropinModel;
			}
		}

		public Drop_FakeDropModel Drop_FakeDrop
		{
			get
			{
				if (_Drop_FakeDropModel == null)
				{
					_Drop_FakeDropModel = new Drop_FakeDropModel();
				}
				return _Drop_FakeDropModel;
			}
		}

		public Room_eventangelskillModel Room_eventangelskill
		{
			get
			{
				if (_Room_eventangelskillModel == null)
				{
					_Room_eventangelskillModel = new Room_eventangelskillModel();
				}
				return _Room_eventangelskillModel;
			}
		}

		public Box_ChapterBoxModel Box_ChapterBox
		{
			get
			{
				if (_Box_ChapterBoxModel == null)
				{
					_Box_ChapterBoxModel = new Box_ChapterBoxModel();
				}
				return _Box_ChapterBoxModel;
			}
		}

		public Equip_UpgradeModel Equip_Upgrade
		{
			get
			{
				if (_Equip_UpgradeModel == null)
				{
					_Equip_UpgradeModel = new Equip_UpgradeModel();
				}
				return _Equip_UpgradeModel;
			}
		}

		public Stage_Level_activityModel Stage_Level_activity
		{
			get
			{
				if (_Stage_Level_activityModel == null)
				{
					_Stage_Level_activityModel = new Stage_Level_activityModel();
				}
				return _Stage_Level_activityModel;
			}
		}

		public Character_LevelModel Character_Level
		{
			get
			{
				if (_Character_LevelModel == null)
				{
					_Character_LevelModel = new Character_LevelModel();
				}
				return _Character_LevelModel;
			}
		}

		public Room_eventdemontext2loseModel Room_eventdemontext2lose
		{
			get
			{
				if (_Room_eventdemontext2loseModel == null)
				{
					_Room_eventdemontext2loseModel = new Room_eventdemontext2loseModel();
				}
				return _Room_eventdemontext2loseModel;
			}
		}

		public Achieve_AchieveModel Achieve_Achieve
		{
			get
			{
				if (_Achieve_AchieveModel == null)
				{
					_Achieve_AchieveModel = new Achieve_AchieveModel();
				}
				return _Achieve_AchieveModel;
			}
		}

		public Sound_soundModel Sound_sound
		{
			get
			{
				if (_Sound_soundModel == null)
				{
					_Sound_soundModel = new Sound_soundModel();
				}
				return _Sound_soundModel;
			}
		}

		public Box_ActivityModel Box_Activity
		{
			get
			{
				if (_Box_ActivityModel == null)
				{
					_Box_ActivityModel = new Box_ActivityModel();
				}
				return _Box_ActivityModel;
			}
		}

		public Fx_fxModel Fx_fx
		{
			get
			{
				if (_Fx_fxModel == null)
				{
					_Fx_fxModel = new Fx_fxModel();
				}
				return _Fx_fxModel;
			}
		}

		public Shop_MysticShopModel Shop_MysticShop
		{
			get
			{
				if (_Shop_MysticShopModel == null)
				{
					_Shop_MysticShopModel = new Shop_MysticShopModel();
				}
				return _Shop_MysticShopModel;
			}
		}

		public Curve_curveModel Curve_curve
		{
			get
			{
				if (_Curve_curveModel == null)
				{
					_Curve_curveModel = new Curve_curveModel();
				}
				return _Curve_curveModel;
			}
		}

		public Equip2_equip2Model Equip2_equip2
		{
			get
			{
				if (_Equip2_equip2Model == null)
				{
					_Equip2_equip2Model = new Equip2_equip2Model();
				}
				return _Equip2_equip2Model;
			}
		}

		public Stage_Level_chapter12Model Stage_Level_chapter12
		{
			get
			{
				if (_Stage_Level_chapter12Model == null)
				{
					_Stage_Level_chapter12Model = new Stage_Level_chapter12Model();
				}
				return _Stage_Level_chapter12Model;
			}
		}

		public Stage_Level_chapter13Model Stage_Level_chapter13
		{
			get
			{
				if (_Stage_Level_chapter13Model == null)
				{
					_Stage_Level_chapter13Model = new Stage_Level_chapter13Model();
				}
				return _Stage_Level_chapter13Model;
			}
		}

		public Stage_Level_chapter10Model Stage_Level_chapter10
		{
			get
			{
				if (_Stage_Level_chapter10Model == null)
				{
					_Stage_Level_chapter10Model = new Stage_Level_chapter10Model();
				}
				return _Stage_Level_chapter10Model;
			}
		}

		public Stage_Level_chapter11Model Stage_Level_chapter11
		{
			get
			{
				if (_Stage_Level_chapter11Model == null)
				{
					_Stage_Level_chapter11Model = new Stage_Level_chapter11Model();
				}
				return _Stage_Level_chapter11Model;
			}
		}

		public Buff_aloneModel Buff_alone
		{
			get
			{
				if (_Buff_aloneModel == null)
				{
					_Buff_aloneModel = new Buff_aloneModel();
				}
				return _Buff_aloneModel;
			}
		}

		public Goods_goodsModel Goods_goods
		{
			get
			{
				if (_Goods_goodsModel == null)
				{
					_Goods_goodsModel = new Goods_goodsModel();
				}
				return _Goods_goodsModel;
			}
		}

		public Box_SilverBoxModel Box_SilverBox
		{
			get
			{
				if (_Box_SilverBoxModel == null)
				{
					_Box_SilverBoxModel = new Box_SilverBoxModel();
				}
				return _Box_SilverBoxModel;
			}
		}

		public Room_soldierupModel Room_soldierup
		{
			get
			{
				if (_Room_soldierupModel == null)
				{
					_Room_soldierupModel = new Room_soldierupModel();
				}
				return _Room_soldierupModel;
			}
		}

		public Stage_Level_chapter9Model Stage_Level_chapter9
		{
			get
			{
				if (_Stage_Level_chapter9Model == null)
				{
					_Stage_Level_chapter9Model = new Stage_Level_chapter9Model();
				}
				return _Stage_Level_chapter9Model;
			}
		}

		public Test_AttrValueModel Test_AttrValue
		{
			get
			{
				if (_Test_AttrValueModel == null)
				{
					_Test_AttrValueModel = new Test_AttrValueModel();
				}
				return _Test_AttrValueModel;
			}
		}

		public Stage_Level_chapter7Model Stage_Level_chapter7
		{
			get
			{
				if (_Stage_Level_chapter7Model == null)
				{
					_Stage_Level_chapter7Model = new Stage_Level_chapter7Model();
				}
				return _Stage_Level_chapter7Model;
			}
		}

		public Stage_Level_chapter8Model Stage_Level_chapter8
		{
			get
			{
				if (_Stage_Level_chapter8Model == null)
				{
					_Stage_Level_chapter8Model = new Stage_Level_chapter8Model();
				}
				return _Stage_Level_chapter8Model;
			}
		}

		public Stage_Level_chapter5Model Stage_Level_chapter5
		{
			get
			{
				if (_Stage_Level_chapter5Model == null)
				{
					_Stage_Level_chapter5Model = new Stage_Level_chapter5Model();
				}
				return _Stage_Level_chapter5Model;
			}
		}

		public Stage_Level_chapter6Model Stage_Level_chapter6
		{
			get
			{
				if (_Stage_Level_chapter6Model == null)
				{
					_Stage_Level_chapter6Model = new Stage_Level_chapter6Model();
				}
				return _Stage_Level_chapter6Model;
			}
		}

		public Stage_Level_chapter3Model Stage_Level_chapter3
		{
			get
			{
				if (_Stage_Level_chapter3Model == null)
				{
					_Stage_Level_chapter3Model = new Stage_Level_chapter3Model();
				}
				return _Stage_Level_chapter3Model;
			}
		}

		public Room_roomModel Room_room
		{
			get
			{
				if (_Room_roomModel == null)
				{
					_Room_roomModel = new Room_roomModel();
				}
				return _Room_roomModel;
			}
		}

		public Room_eventdemontext2skillModel Room_eventdemontext2skill
		{
			get
			{
				if (_Room_eventdemontext2skillModel == null)
				{
					_Room_eventdemontext2skillModel = new Room_eventdemontext2skillModel();
				}
				return _Room_eventdemontext2skillModel;
			}
		}

		public Stage_Level_chapter4Model Stage_Level_chapter4
		{
			get
			{
				if (_Stage_Level_chapter4Model == null)
				{
					_Stage_Level_chapter4Model = new Stage_Level_chapter4Model();
				}
				return _Stage_Level_chapter4Model;
			}
		}

		public Stage_Level_chapter1Model Stage_Level_chapter1
		{
			get
			{
				if (_Stage_Level_chapter1Model == null)
				{
					_Stage_Level_chapter1Model = new Stage_Level_chapter1Model();
				}
				return _Stage_Level_chapter1Model;
			}
		}

		public Room_colorstyleModel Room_colorstyle
		{
			get
			{
				if (_Room_colorstyleModel == null)
				{
					_Room_colorstyleModel = new Room_colorstyleModel();
				}
				return _Room_colorstyleModel;
			}
		}

		public Stage_Level_chapter2Model Stage_Level_chapter2
		{
			get
			{
				if (_Stage_Level_chapter2Model == null)
				{
					_Stage_Level_chapter2Model = new Stage_Level_chapter2Model();
				}
				return _Stage_Level_chapter2Model;
			}
		}

		public Stage_Level_stagechapterModel Stage_Level_stagechapter
		{
			get
			{
				if (_Stage_Level_stagechapterModel == null)
				{
					_Stage_Level_stagechapterModel = new Stage_Level_stagechapterModel();
				}
				return _Stage_Level_stagechapterModel;
			}
		}

		public Shop_MysticShopShowModel Shop_MysticShopShow
		{
			get
			{
				if (_Shop_MysticShopShowModel == null)
				{
					_Shop_MysticShopShowModel = new Shop_MysticShopShowModel();
				}
				return _Shop_MysticShopShowModel;
			}
		}

		public Drop_harvestModel Drop_harvest
		{
			get
			{
				if (_Drop_harvestModel == null)
				{
					_Drop_harvestModel = new Drop_harvestModel();
				}
				return _Drop_harvestModel;
			}
		}

		public Box_TimeBoxModel Box_TimeBox
		{
			get
			{
				if (_Box_TimeBoxModel == null)
				{
					_Box_TimeBoxModel = new Box_TimeBoxModel();
				}
				return _Box_TimeBoxModel;
			}
		}

		public Preload_loadModel Preload_load
		{
			get
			{
				if (_Preload_loadModel == null)
				{
					_Preload_loadModel = new Preload_loadModel();
				}
				return _Preload_loadModel;
			}
		}

		public Room_levelModel Room_level
		{
			get
			{
				if (_Room_levelModel == null)
				{
					_Room_levelModel = new Room_levelModel();
				}
				return _Room_levelModel;
			}
		}

		public Shop_ReadyShopModel Shop_ReadyShop
		{
			get
			{
				if (_Shop_ReadyShopModel == null)
				{
					_Shop_ReadyShopModel = new Shop_ReadyShopModel();
				}
				return _Shop_ReadyShopModel;
			}
		}

		public Skill_superModel Skill_super
		{
			get
			{
				if (_Skill_superModel == null)
				{
					_Skill_superModel = new Skill_superModel();
				}
				return _Skill_superModel;
			}
		}

		public Shop_ShopModel Shop_Shop
		{
			get
			{
				if (_Shop_ShopModel == null)
				{
					_Shop_ShopModel = new Shop_ShopModel();
				}
				return _Shop_ShopModel;
			}
		}

		public Soldier_standardModel Soldier_standard
		{
			get
			{
				if (_Soldier_standardModel == null)
				{
					_Soldier_standardModel = new Soldier_standardModel();
				}
				return _Soldier_standardModel;
			}
		}

		public Skill_aloneModel Skill_alone
		{
			get
			{
				if (_Skill_aloneModel == null)
				{
					_Skill_aloneModel = new Skill_aloneModel();
				}
				return _Skill_aloneModel;
			}
		}

		public Goods_waterModel Goods_water
		{
			get
			{
				if (_Goods_waterModel == null)
				{
					_Goods_waterModel = new Goods_waterModel();
				}
				return _Goods_waterModel;
			}
		}

		public Soldier_soldierModel Soldier_soldier
		{
			get
			{
				if (_Soldier_soldierModel == null)
				{
					_Soldier_soldierModel = new Soldier_soldierModel();
				}
				return _Soldier_soldierModel;
			}
		}

		public Beat_beatModel Beat_beat
		{
			get
			{
				if (_Beat_beatModel == null)
				{
					_Beat_beatModel = new Beat_beatModel();
				}
				return _Beat_beatModel;
			}
		}

		public Weapon_weaponModel Weapon_weapon
		{
			get
			{
				if (_Weapon_weaponModel == null)
				{
					_Weapon_weaponModel = new Weapon_weaponModel();
				}
				return _Weapon_weaponModel;
			}
		}

		public Skill_skillModel Skill_skill
		{
			get
			{
				if (_Skill_skillModel == null)
				{
					_Skill_skillModel = new Skill_skillModel();
				}
				return _Skill_skillModel;
			}
		}

		public Skill_greedyskillModel Skill_greedyskill
		{
			get
			{
				if (_Skill_greedyskillModel == null)
				{
					_Skill_greedyskillModel = new Skill_greedyskillModel();
				}
				return _Skill_greedyskillModel;
			}
		}

		public Shop_GoldModel Shop_Gold
		{
			get
			{
				if (_Shop_GoldModel == null)
				{
					_Shop_GoldModel = new Shop_GoldModel();
				}
				return _Shop_GoldModel;
			}
		}

		public Skill_slotfirstModel Skill_slotfirst
		{
			get
			{
				if (_Skill_slotfirstModel == null)
				{
					_Skill_slotfirstModel = new Skill_slotfirstModel();
				}
				return _Skill_slotfirstModel;
			}
		}

		public Exp_expModel Exp_exp
		{
			get
			{
				if (_Exp_expModel == null)
				{
					_Exp_expModel = new Exp_expModel();
				}
				return _Exp_expModel;
			}
		}

		public Drop_GoldModel Drop_Gold
		{
			get
			{
				if (_Drop_GoldModel == null)
				{
					_Drop_GoldModel = new Drop_GoldModel();
				}
				return _Drop_GoldModel;
			}
		}

		public Drop_DropModel Drop_Drop
		{
			get
			{
				if (_Drop_DropModel == null)
				{
					_Drop_DropModel = new Drop_DropModel();
				}
				return _Drop_DropModel;
			}
		}

		public Stage_Level_activitylevelModel Stage_Level_activitylevel
		{
			get
			{
				if (_Stage_Level_activitylevelModel == null)
				{
					_Stage_Level_activitylevelModel = new Stage_Level_activitylevelModel();
				}
				return _Stage_Level_activitylevelModel;
			}
		}

		public Skill_slotoutModel Skill_slotout
		{
			get
			{
				if (_Skill_slotoutModel == null)
				{
					_Skill_slotoutModel = new Skill_slotoutModel();
				}
				return _Skill_slotoutModel;
			}
		}

		private LocalModelManager()
		{
		}

		public void InitializeAll()
		{
			Goods_foodModel goods_food = Goods_food;
			Equip_equipModel equip_equip = Equip_equip;
			Skill_slotoutcostModel skill_slotoutcost = Skill_slotoutcost;
			Config_configModel config_config = Config_config;
			Box_SilverNormalBoxModel box_SilverNormalBox = Box_SilverNormalBox;
			Skill_slotinModel skill_slotin = Skill_slotin;
			Room_eventgameturnModel room_eventgameturn = Room_eventgameturn;
			Operation_moveModel operation_move = Operation_move;
			Character_BabyModel character_Baby = Character_Baby;
			Shop_itemModel shop_item = Shop_item;
			Character_CharModel character_Char = Character_Char;
			Language_lauguageModel language_lauguage = Language_lauguage;
			Skill_dropinModel skill_dropin = Skill_dropin;
			Drop_FakeDropModel drop_FakeDrop = Drop_FakeDrop;
			Room_eventangelskillModel room_eventangelskill = Room_eventangelskill;
			Box_ChapterBoxModel box_ChapterBox = Box_ChapterBox;
			Equip_UpgradeModel equip_Upgrade = Equip_Upgrade;
			Stage_Level_activityModel stage_Level_activity = Stage_Level_activity;
			Character_LevelModel character_Level = Character_Level;
			Room_eventdemontext2loseModel room_eventdemontext2lose = Room_eventdemontext2lose;
			Achieve_AchieveModel achieve_Achieve = Achieve_Achieve;
			Sound_soundModel sound_sound = Sound_sound;
			Box_ActivityModel box_Activity = Box_Activity;
			Fx_fxModel fx_fx = Fx_fx;
			Shop_MysticShopModel shop_MysticShop = Shop_MysticShop;
			Curve_curveModel curve_curve = Curve_curve;
			Equip2_equip2Model equip2_equip = Equip2_equip2;
			Stage_Level_chapter12Model stage_Level_chapter = Stage_Level_chapter12;
			Stage_Level_chapter13Model stage_Level_chapter2 = Stage_Level_chapter13;
			Stage_Level_chapter10Model stage_Level_chapter3 = Stage_Level_chapter10;
			Stage_Level_chapter11Model stage_Level_chapter4 = Stage_Level_chapter11;
			Buff_aloneModel buff_alone = Buff_alone;
			Goods_goodsModel goods_goods = Goods_goods;
			Box_SilverBoxModel box_SilverBox = Box_SilverBox;
			Room_soldierupModel room_soldierup = Room_soldierup;
			Stage_Level_chapter9Model stage_Level_chapter5 = Stage_Level_chapter9;
			Test_AttrValueModel test_AttrValue = Test_AttrValue;
			Stage_Level_chapter7Model stage_Level_chapter6 = Stage_Level_chapter7;
			Stage_Level_chapter8Model stage_Level_chapter7 = Stage_Level_chapter8;
			Stage_Level_chapter5Model stage_Level_chapter8 = Stage_Level_chapter5;
			Stage_Level_chapter6Model stage_Level_chapter9 = Stage_Level_chapter6;
			Stage_Level_chapter3Model stage_Level_chapter10 = Stage_Level_chapter3;
			Room_roomModel room_room = Room_room;
			Room_eventdemontext2skillModel room_eventdemontext2skill = Room_eventdemontext2skill;
			Stage_Level_chapter4Model stage_Level_chapter11 = Stage_Level_chapter4;
			Stage_Level_chapter1Model stage_Level_chapter12 = Stage_Level_chapter1;
			Room_colorstyleModel room_colorstyle = Room_colorstyle;
			Stage_Level_chapter2Model stage_Level_chapter13 = Stage_Level_chapter2;
			Stage_Level_stagechapterModel stage_Level_stagechapter = Stage_Level_stagechapter;
			Shop_MysticShopShowModel shop_MysticShopShow = Shop_MysticShopShow;
			Drop_harvestModel drop_harvest = Drop_harvest;
			Box_TimeBoxModel box_TimeBox = Box_TimeBox;
			Preload_loadModel preload_load = Preload_load;
			Room_levelModel room_level = Room_level;
			Shop_ReadyShopModel shop_ReadyShop = Shop_ReadyShop;
			Skill_superModel skill_super = Skill_super;
			Shop_ShopModel shop_Shop = Shop_Shop;
			Soldier_standardModel soldier_standard = Soldier_standard;
			Skill_aloneModel skill_alone = Skill_alone;
			Goods_waterModel goods_water = Goods_water;
			Soldier_soldierModel soldier_soldier = Soldier_soldier;
			Beat_beatModel beat_beat = Beat_beat;
			Weapon_weaponModel weapon_weapon = Weapon_weapon;
			Skill_skillModel skill_skill = Skill_skill;
			Skill_greedyskillModel skill_greedyskill = Skill_greedyskill;
			Shop_GoldModel shop_Gold = Shop_Gold;
			Skill_slotfirstModel skill_slotfirst = Skill_slotfirst;
			Exp_expModel exp_exp = Exp_exp;
			Drop_GoldModel drop_Gold = Drop_Gold;
			Drop_DropModel drop_Drop = Drop_Drop;
			Stage_Level_activitylevelModel stage_Level_activitylevel = Stage_Level_activitylevel;
			Skill_slotoutModel skill_slotout = Skill_slotout;
		}
	}
}
