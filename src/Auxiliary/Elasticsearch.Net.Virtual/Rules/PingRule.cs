using System;

namespace Elasticsearch.Net.Virtual.Rules
{
	public class PingRule : RuleBase<PingRule>, IRule
	{
		private IRule Self => this;

		public PingRule Fails(RuleOption<TimesHelper.AllTimes, int> times, RuleOption<Exception, int> errorState = null)
		{
			Self.Times = times;
			Self.Succeeds = false;
			Self.Return = errorState;
			return this;
		}

		public PingRule Succeeds(RuleOption<TimesHelper.AllTimes, int> times, int? validResponseCode = 200)
		{
			Self.Times = times;
			Self.Succeeds = true;
			Self.Return = validResponseCode;
			return this;
		}

		public PingRule SucceedAlways(int? validResponseCode = 200) => Succeeds(TimesHelper.Always, validResponseCode);

		public PingRule FailAlways(RuleOption<Exception, int> errorState = null) => Fails(TimesHelper.Always, errorState);
	}
}
