using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FactWeb.BusinessLayer.Tests
{
    [TestClass]
    public class K2ManagerTests
    {
        [TestMethod]
        public void ComplianceApplicationApprovalTest()
        {
            var processName = "FACT Web\\Compliance Application Facility Approval v3";
            var orgName = "Fox Chase-Temple University Hospital Bone Marrow Transplant Program";
            var applicationId = 11;
            var complianceApplicationId = "BD979421-7A7C-4072-A5DB-F16A02A2A898";

            var manager = new K2NotificationManager();

            try
            {
                manager.SetDataFieldsStartProcess(null, complianceApplicationId, orgName, processName, "");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void RunInspectorsTest()
        {
            var manager = new K2NotificationManager();

            try
            {
                manager.RunInspectors();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
