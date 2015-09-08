using System;
using Elasticsearch.Net.ConnectionPool;
using Nest;
using Elasticsearch.Net.Connection;
using Elasticsearch.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net.Serialization;
using System.IO;

namespace Tests.Framework
{
	public class SealedVirtualCluster
	{
		private readonly VirtualCluster _cluster;
		private readonly IConnectionPool _connectionPool;
		private readonly IConnection _connection;
		private readonly TestableDateTimeProvider _dateTimeProvider;

		public SealedVirtualCluster(VirtualCluster cluster, IConnectionPool pool, TestableDateTimeProvider dateTimeProvider)
		{
			this._cluster = cluster;
			this._connectionPool = pool;
			this._connection = new VirtualClusterConnection(cluster);
			this._dateTimeProvider = dateTimeProvider;
		}

		private ConnectionSettings CreateSettings() =>
			new ConnectionSettings(this._connectionPool, this._connection);

		public VirtualizedCluster AllDefaults() =>
			new VirtualizedCluster(this._cluster, this._connectionPool, this._dateTimeProvider, CreateSettings());

		public VirtualizedCluster Settings(Func<ConnectionSettings, ConnectionSettings> selector) =>
			new VirtualizedCluster(this._cluster, this._connectionPool, this._dateTimeProvider, selector(CreateSettings()));
	}
}