namespace NHS111.Data.Sqlite.Itk.Dispatcher.Repositories {
    using System;
    using System.Data;
    using System.Data.SQLite;
    using System.Threading;
    using System.Threading.Tasks;
    using Data.Itk.Dispatcher.Repositories;
    using log4net;

    public class ItkDispatchRequestRepository
        : IItkDispatchRequestRepository {

        public ItkDispatchRequestRepository(string connectionString, ILog log)
            : this(new SQLiteConnection(connectionString)) {
            _log = log;
        }

        private ItkDispatchRequestRepository(SQLiteConnection databaseConnection) {
            _connection = databaseConnection;
        }

        public async Task InsertAsync(string id, string request, CancellationToken cancellationToken) {
            using (var cmd = _connection.CreateCommand()) {
                try {
                    await _connection.OpenAsync(cancellationToken);
                    cmd.CommandText = "INSERT INTO Requests (JourneyId, Request) VALUES (@journeyId, @request); "; // +
                    //"SELECT last_insert_rowid();";
                    cmd.Parameters.Add("@journeyId", DbType.String).Value = id;
                    cmd.Parameters.Add("@request", DbType.String).Value = request;

                    await cmd.ExecuteNonQueryAsync(cancellationToken);
                }
                catch (Exception e) {
                    _log.Fatal(e);
                    throw;
                }
                finally {
                    _connection.Close();
                }
            }
        }

        public void InitialiseDatabase() {
            try {
                _connection.Open();

                using (var transaction = _connection.BeginTransaction()) {
                    using (var command = _connection.CreateCommand()) {
                        command.Transaction = transaction;
                        command.CommandText = script;
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
            finally {
                _connection.Close();
            }
        }

        private static string script = @"CREATE TABLE IF NOT EXISTS [Requests]
                                        (
                                            [Id] INTEGER PRIMARY KEY NOT NULL,
                                            [JourneyId] INTEGER NOT NULL,
                                            [Request] TEXT NOT NULL
                                        );";


        private readonly ILog _log;
        private readonly SQLiteConnection _connection;
    }
}