using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vdp
{
    [TestClass]
    public class FundsTransferTest
    {
        private string pushFundsRequest;
        private VisaAPIClient visaAPIClient;

        public FundsTransferTest()
        {
            visaAPIClient = new VisaAPIClient();
            string strDate = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss");


            //required
            // acquirerCountryCode

            //pushFundsRequest =
            //"{"
            //        + "\"systemsTraceAuditNumber\":350420,"
            //        + "\"retrievalReferenceNumber\":\"401010350420\","
            //        + "\"localTransactionDateTime\":\"" + strDate + "\","
            //        + "\"acquiringBin\":409999,\"acquirerCountryCode\":\"840\","
            //        + "\"senderAccountNumber\":\"1234567890123456\","
            //        + "\"senderCountryCode\":\"USA\","
            //        + "\"transactionCurrencyCode\":\"USD\","
            //        + "\"senderName\":\"John Smith\","
            //        + "\"senderAddress\":\"44 Market St.\","
            //        + "\"senderCity\":\"San Francisco\","
            //        + "\"senderStateCode\":\"CA\","
            //        + "\"recipientName\":\"Adam Smith\","
            //        + "\"recipientPrimaryAccountNumber\":\"4957030420210454\","
            //        + "\"amount\":\"112.00\","
            //        + "\"businessApplicationId\":\"AA\","
            //        + "\"transactionIdentifier\":234234322342343,"
            //        + "\"merchantCategoryCode\":6012,"
            //        + "\"sourceOfFundsCode\":\"03\","
            //        + "\"cardAcceptor\":{"
            //                            + "\"name\":\"John Smith\","
            //                            + "\"terminalId\":\"13655392\","
            //                            + "\"idCode\":\"VMT200911026070\","
            //                            + "\"address\":{"
            //                                            + "\"state\":\"CA\","
            //                                            + "\"county\":\"081\","
            //                                            + "\"country\":\"USA\","
            //                                            + "\"zipCode\":\"94105\""
            //                                + "}"
            //                            + "},"
            //        + "\"feeProgramIndicator\":\"123\""
            //    + "}";

            var requestFile = "PushRequest.json";

            if (File.Exists(requestFile))
            {
                pushFundsRequest = File.ReadAllText(requestFile);
            }
        }

        [TestMethod]
        public void TestPushFundsTransactions()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string baseUri = "visadirect/";
            string resourcePath = "fundstransfer/v1/pushfundstransactions/";
            string status = visaAPIClient.DoMutualAuthCall(baseUri + resourcePath, "POST", "Push Funds Transaction Test", pushFundsRequest);
            Assert.AreEqual(status, "OK");
        }


        //https://sandbox.api.visa.com/visadirect/fundstransfer/v1/pullfundstransactions

        [TestMethod]
        public void TestPullFundsTransactions()
        {
            var requestFile = "PullRequest.json";
            string request = "";

            if (File.Exists(requestFile))
            {
                request = File.ReadAllText(requestFile);
            }

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string baseUri = "visadirect/";
            string resourcePath = "fundstransfer/v1/pullfundstransactions/";
            string status = visaAPIClient.DoMutualAuthCall(baseUri + resourcePath, "POST", "Pull Funds Transaction Test", request);
            Assert.AreEqual(status, "OK");
        }

    }
}
