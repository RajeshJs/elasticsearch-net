namespace Elasticsearch.Net.Virtual
{
	public class FixedPipelineFactory : IRequestPipelineFactory
	{
		public FixedPipelineFactory(IConnectionConfigurationValues connectionSettings, IDateTimeProvider dateTimeProvider)
		{
			DateTimeProvider = dateTimeProvider;
			MemoryStreamFactory = new MemoryStreamFactory();

			Settings = connectionSettings;
			Pipeline = Create(Settings, DateTimeProvider, MemoryStreamFactory, new SearchRequestParameters());
		}

		public ElasticLowLevelClient Client => new ElasticLowLevelClient(Transport);

		public IRequestPipeline Pipeline { get; }

		private IDateTimeProvider DateTimeProvider { get; }
		private MemoryStreamFactory MemoryStreamFactory { get; }
		private IConnectionConfigurationValues Settings { get; }

		private Transport<IConnectionConfigurationValues> Transport =>
			new Transport<IConnectionConfigurationValues>(Settings, this, DateTimeProvider, MemoryStreamFactory);

		public IRequestPipeline Create(IConnectionConfigurationValues configurationValues, IDateTimeProvider dateTimeProvider,
			IMemoryStreamFactory memoryStreamFactory, IRequestParameters requestParameters
		) =>
			new RequestPipeline(Settings, DateTimeProvider, MemoryStreamFactory, requestParameters ?? new SearchRequestParameters());
	}
}
