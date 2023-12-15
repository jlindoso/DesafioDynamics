using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DioDynamics1
{
    public class Conexao
    {
        public static CrmServiceClient CrmServiceClient;
        public string ConnectionString { get; set; }

        public Conexao()
        {
            ConnectionString = @"AuthType=Office365; 
                                Username =  jorgelindoso@jlindoso.onmicrosoft.com; 
                                Password = Komodor5!#; 
                                SkipDiscovery=  True; 
                                AppId = c9299480-c13a-49db-a7ae-*****;
                                Url = https://org18e2d1ba.crm2.dynamics.com/main.aspx;";
        }

        public CrmServiceClient ObterConexao()
        {
            if (CrmServiceClient == null)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                CrmServiceClient = new CrmServiceClient(ConnectionString);
            }

            return CrmServiceClient;
        }


    }
}
