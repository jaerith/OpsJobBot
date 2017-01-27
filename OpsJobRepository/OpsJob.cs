using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsJobRepository
{
    public class OpsJob
    {
        public OpsJob()
        { }

        public string JobName { get; set; }

        public string Description { get; set; }

        public string ActiveCd { get; set; }

        public string LastReturnCd { get; set; }
    }
}
