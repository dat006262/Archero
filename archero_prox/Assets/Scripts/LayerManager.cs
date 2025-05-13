




using UnityEngine;

public class LayerManager
{
	public enum BulletLayer
	{
		eAll,
		eOnlyOut,
		eNone
	}

	public static int UI;

	public static int Player;

	public static int Map;

	public static int Goods;

	public static int Fly;

	public static int MapOutWall;

	public static int Bullet;

	public static int Bullet2Map;

	public static int PlayerAbsorb;

	public static int PlayerAbsorbImme;

	public static int Entity2MapOutWall;

	public static int Entity2Stone;

	public static int Entity2Water;

	public static int Stone;

	public static int Waters;

	public static int BattleHits;

	public static int BulletResist;

	public static int Hide;

	public static int[] BulletTriggers;

	public static int HitEntity;

	public static int MapAllInt;

	public static int Move_Fly;

	public static int Move_Ground;

	public const int RenderQueue_Fly = 3000;

	public const int RenderQueue_Default = 2000;

	static LayerManager()
	{
		UI = LayerMask.NameToLayer("UI");
		Player = LayerMask.NameToLayer("Player");
		Map = LayerMask.NameToLayer("Map");
		Goods = LayerMask.NameToLayer("Goods");
		Fly = LayerMask.NameToLayer("Fly");
		MapOutWall = LayerMask.NameToLayer("MapOutWall");
		Bullet = LayerMask.NameToLayer("Bullet");
		Bullet2Map = LayerMask.NameToLayer("Bullet2Map");
		PlayerAbsorb = LayerMask.NameToLayer("PlayerAbsorb");
		PlayerAbsorbImme = LayerMask.NameToLayer("PlayerAbsorbImme");
		Entity2MapOutWall = LayerMask.NameToLayer("Entity2MapOutWall");
		Entity2Stone = LayerMask.NameToLayer("Entity2Stone");
		Entity2Water = LayerMask.NameToLayer("Entity2Water");
		Stone = LayerMask.NameToLayer("Stone");
		Waters = LayerMask.NameToLayer("Waters");
		Hide = LayerMask.NameToLayer("Hide");
		BattleHits = LayerMask.NameToLayer("BattleHits");
		BulletResist = LayerMask.NameToLayer("BulletResist");
		BulletTriggers = new int[3];
		BulletTriggers[0] = ((1 << Player) | (1 << MapOutWall) | (1 << Bullet2Map) | (1 << BulletResist));
		BulletTriggers[1] = ((1 << Player) | (1 << MapOutWall) | (1 << BulletResist));
		BulletTriggers[2] = ((1 << Player) | (1 << BulletResist));
		HitEntity = 1 << Player;
		MapAllInt = ((1 << Stone) | (1 << Waters) | (1 << MapOutWall));
		Move_Fly = 0;
		Move_Ground = ((1 << Stone) | (1 << Waters));
	}

	public static bool IsCollisionMap(int layer)
	{
		return layer == Stone || layer == Waters || layer == MapOutWall;
	}

	public static int GetBullet(BulletLayer type)
	{
		return BulletTriggers[(int)type];
	}
}
