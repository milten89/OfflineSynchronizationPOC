using Dotmim.Sync;
using Dotmim.Sync.Sqlite;
using Dotmim.Sync.Web.Client;
using OfflineSynchronizationPOC.Client;

if (File.Exists("sqlite.db"))
    File.Delete("sqlite.db");

var serverOrchestrator = new WebRemoteOrchestrator("https://localhost:7293/Sync");

var clientProvider = new SqliteSyncProvider(SqliteDatabaseContext.ConnectionString);

var agent = new SyncAgent(clientProvider, serverOrchestrator);

var progress = new SynchronousProgress<ProgressArgs>(pa => Console.WriteLine($"{pa.ProgressPercentage:p}\t {pa.Message}"));

var parameters = new SyncParameters(("FacilityId", "2a0706e7-9201-4a57-80f2-dc8fc230b169"));

do
{
    // Launch the sync process
    var s1 = await agent.SynchronizeAsync(parameters, progress);
    // Write results
    Console.WriteLine(s1);

} while (Console.ReadKey().Key != ConsoleKey.Escape);

Console.WriteLine("End");
