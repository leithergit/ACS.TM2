using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.IO;
using UmsServer.Mq;

//using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Xml;

namespace MagoACSService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
