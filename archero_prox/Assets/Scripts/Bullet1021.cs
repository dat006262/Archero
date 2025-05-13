




public class Bullet1021 : BulletBase
{
	private ConditionBase mCondition1;

	private ConditionBase mCondition2;

	protected int DelayTimeEnable = 1300;

	protected override void OnInit()
	{
		base.OnInit();
		BoxEnable(enable: false);
		mCondition1 = AIMoveBase.GetConditionTime(DelayTimeEnable);
		mCondition2 = AIMoveBase.GetConditionTime(DelayTimeEnable + 100);
	}

	protected override void OnUpdate()
	{
		if (mCondition1 != null && mCondition1.IsEnd())
		{
			BoxEnable(enable: true);
			mCondition1 = null;
		}
		if (mCondition2 != null && mCondition2.IsEnd())
		{
			BoxEnable(enable: false);
			mCondition2 = null;
		}
	}
}
