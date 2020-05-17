using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FlattenIslandAnalyticsEvent : AnalyticsEvent
{
	public FlattenIslandAnalyticsEvent(FlattenIslandAnalyticsEventName flattenIslandAnalyticsEventName) : base(flattenIslandAnalyticsEventName)
	{
		AddParam(new FlattenIslandAnalyticsEventParameter(FlattenIslandAnalyticsEventParameter.PlayerId), UnityEngine.Analytics.AnalyticsSessionInfo.userId);
	}
}