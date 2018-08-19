﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrtgAPI.Tests.UnitTests.Support.TestItems;
using PrtgAPI.Tests.UnitTests.Support.TestResponses;

namespace PrtgAPI.Tests.UnitTests.ObjectTests
{
    [TestClass]
    public class LogTests : StreamableObjectTests<Log, MessageItem, MessageResponse>
    {
        [TestMethod]
        public void Log_CanDeserialize() => Object_CanDeserialize();

        [TestMethod]
        public async Task Log_CanDeserializeAsync() => await Object_CanDeserializeAsync();

        [TestMethod]
        public void Log_AllFields_HaveValues() => Object_AllFields_HaveValues();

        protected override List<Log> GetObjects(PrtgClient client) => client.GetLogs();

        protected override Task<List<Log>> GetObjectsAsync(PrtgClient client) => client.GetLogsAsync();

        protected override IEnumerable<Log> Stream(PrtgClient client) => client.StreamLogs();

        public override MessageItem GetItem() => new MessageItem();

        protected override MessageResponse GetResponse(MessageItem[] items) => new MessageResponse(items);

        [TestMethod]
        public void Log_ExecutesAllOverloads()
        {
            var client = Initialize_Client_WithItems(GetItem());

            var logsDate = client.GetLogs(null, DateTime.Now);
            var logsDateAsync = client.GetLogsAsync(null, DateTime.Now).Result;
            var logsDateStream = client.StreamLogs(null, DateTime.Now).ToList();

            var logsTimeSpan = client.GetLogs();
            var logsTimeSpanAsync = client.GetLogsAsync().Result;
            var logsTimeSpanStream = client.StreamLogs().ToList();
        [TestMethod]
        public void Log_Stream_WithCorrectPageSize()
        {
            var urls = new[]
            {
                TestHelpers.RequestLog("count=500&start=1&filter_drel=7days", UrlFlag.Columns),
                TestHelpers.RequestLog("count=500&start=501&filter_drel=7days", UrlFlag.Columns),
                TestHelpers.RequestLog("count=500&start=1001&filter_drel=7days", UrlFlag.Columns),
                TestHelpers.RequestLog("count=100&start=1501&filter_drel=7days", UrlFlag.Columns)
            };

            var client = Initialize_Client(new AddressValidatorResponse(urls)
            {
                CountOverride = new Dictionary<Content, int>
                {
                    [Content.Logs] = 1600
                }
            });

            var items = client.StreamLogs(serial: true).ToList();

            Assert.AreEqual(1600, items.Count);
        }

        [TestMethod]
        public void Log_SanitizesSystemMessage()
        {
            var client = Initialize_Client(new MessageResponse(new MessageItem(messageRaw: "#O18", message: "<div class=\"logmessage\">Timeout (code: PE018)<div class=\"moreicon\"></div></div>")));

            var log = client.GetLogs().First();

            Assert.AreEqual(log.Message, "Timeout (code: PE018)");
        }
    }
}
