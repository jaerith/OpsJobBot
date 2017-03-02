using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsJobRepository
{
    public class OpsRepository
    {
        private static object InstanceLock = new object();
        private static OpsRepository Instance = null;

        private OpsRepository()
        { }

        static public OpsRepository GetInstance()
        {
            lock (InstanceLock)
            {
                Instance = new OpsRepository();
            }

            return Instance;
        }

        public HashSet<OpsJob> GetJobsList()
        {
            HashSet<OpsJob> AllJobs = new HashSet<OpsJob>();

            AllJobs.Add(new OpsJob() { JobName = "job123", Description = "something", ActiveCd = "Y", LastReturnCd = "0" });
            AllJobs.Add(new OpsJob() { JobName = "job456", Description = "blah", ActiveCd = "Y", LastReturnCd = "0" });
            AllJobs.Add(new OpsJob() { JobName = "job789", Description = "etc", ActiveCd = "Y", LastReturnCd = "0" });

            AllJobs.Add(new OpsJob() { JobName = "copyJob.bat", Description = "Copies...Duh", ActiveCd = "Y", LastReturnCd = "0" });

            return AllJobs;
        }
    }
}