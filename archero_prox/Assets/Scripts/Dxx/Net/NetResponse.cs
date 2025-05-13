




using GameProtocol;

namespace Dxx.Net
{
	public class NetResponse
	{
		public IProtocol data;

		public CCommonRespMsg error;

		public bool IsSuccess
		{
			get
			{
				if (data != null && data is CRespItemPacket)
				{
					CRespItemPacket cRespItemPacket = data as CRespItemPacket;
					if (cRespItemPacket != null && cRespItemPacket.m_commMsg != null)
					{
						error = cRespItemPacket.m_commMsg;
						if (error.m_nStatusCode == 0 || error.m_nStatusCode == 1)
						{
							return true;
						}
						data = null;
						return false;
					}
				}
				return data != null || (error != null && (error.m_nStatusCode == 0 || error.m_nStatusCode == 1));
			}
		}

		public bool IsTimeOut => data == null && error == null;
	}
}
