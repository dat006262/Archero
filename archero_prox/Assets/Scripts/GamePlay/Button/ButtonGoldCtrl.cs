




public class ButtonGoldCtrl : ButtonCtrl
{
	public GoldTextCtrl mGoldCtrl;

	private bool bRed = true;

	public override void SetEnable(bool value)
	{
		base.SetEnable(value);
		if ((bool)mGoldCtrl && bRed)
		{
			mGoldCtrl.SetButtonEnable(value);
		}
	}

	public void SetCanRed(bool value)
	{
		bRed = value;
	}

	public void SetGold(int gold)
	{
		if ((bool)mGoldCtrl)
		{
			mGoldCtrl.SetValue(gold);
			if (bRed)
			{
				mGoldCtrl.SetButtonEnable(bEnable);
			}
		}
	}

	public void SetCurrency(int type)
	{
		SetCurrency((CurrencyType)type);
	}

	public void SetCurrency(CurrencyType type)
	{
		if ((bool)mGoldCtrl)
		{
			mGoldCtrl.SetCurrencyType(type);
		}
	}
}
