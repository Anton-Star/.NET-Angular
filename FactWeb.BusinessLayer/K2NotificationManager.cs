using FactWeb.Infrastructure;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Workflow.Client;
using System;
using System.Configuration;

namespace FactWeb.BusinessLayer
{
    public class K2NotificationManager
    {

        private readonly string serverName;
        //private readonly string userName = "factapp01-dev\\k2localadmin2";
        //private readonly string domain = "factapp01-dev";

        private readonly SCConnectionStringBuilder connectionString;

        public K2NotificationManager()
        {
            this.serverName = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.K2Server];
            var port = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.K2Port];

            this.connectionString = new SCConnectionStringBuilder
            {
                Authenticate = true,
                Host = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.K2Host],
                Integrated = false,
                IsPrimaryLogin = true,
                Port = !string.IsNullOrWhiteSpace(port) ? Convert.ToUInt32(ConfigurationManager.AppSettings[Constants.ConfigurationConstants.K2Port]) : 5252,
                UserID = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.K2UserName],
                //WindowsDomain = this.domain,
                Password = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.K2Password],
                //SecurityLabelName = "K2"
            };
        }
        
        public void SetDataFieldsStartProcess(int? applicationId, string complianceApplicationId, string orgName, string processName, string userName)
        {
            //the default label  
            var connection = new Connection();
            try
            {
                //open connection to K2 server   
                connection.Open(this.serverName, this.connectionString.ToString());
                //create process instance  
                var processInstance = connection.CreateProcessInstance(processName);
                //populate data fields  
                //processInstance.DataFields["ApplicationID"].Value = applicationId;
                processInstance.DataFields["ComplianceApplicationID"].Value = complianceApplicationId;
                processInstance.DataFields["Submitter"].Value = userName;
                //set process folio  
                processInstance.Folio = complianceApplicationId;
                //start the process  
                connection.StartProcessInstance(processInstance, false);
            }
            catch
            {
                throw;
            }
            finally
            {
                // close the connection  
                connection.Close();
            }
        }

        public void SetApproval(string serialNumber)
        {
            using (var wfConn = new Connection())
            {
                wfConn.Open(this.serverName, this.connectionString.ToString());

                var serverItem = wfConn.OpenServerItem(serialNumber);
                serverItem.Finish();
            }
        }

        public void SetDecline(string serialNumber)
        {
            using (var wfConn = new Connection())
            {
                wfConn.Open(this.serverName, this.connectionString.ToString());
                var wli = wfConn.OpenWorklistItem(serialNumber);
                wli.Actions["Decline"].Execute();
            }
        }

        public void RunInspectors()
        {
            var _processFolio = "Compliance Application Populate Inspectors";

            //the default label  
            var connection = new Connection();
            try
            {
                //open connection to K2 server   
                connection.Open(this.serverName, this.connectionString.ToString());
                //create process instance  
                var processInstance = connection.CreateProcessInstance(_processFolio);
                //set process folio  
                processInstance.Folio = _processFolio + DateTime.Now;
                //start the process  
                connection.StartProcessInstance(processInstance, false);
            }
            catch
            {
                throw;
            }
            finally
            {
                // close the connection  
                connection.Close();
            }
        }
    }
}
