using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShellsV3.Factories;
using SeeShellsV3.Repositories;
using SeeShellsV3.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SeeShellsV3.Data;
using Unity;

namespace SeeShellsV3Tests.Services.Tests
{
    [TestClass()]
    public class TimezoneManagerTests
    {
        [TestMethod()]
        public void LoadTimezonesTest()
        {
            TimezoneManager timeMan = new TimezoneManager(null, null, null, new TimezoneCollection());

            ITimezone utc = new Timezone("UTC", displayName: "Coordinated Universal Time");
            ITimezone est = new Timezone("Eastern Standard Time");

            Assert.IsTrue(timeMan.SupportedTimezones.Contains(utc));
            Assert.IsTrue(timeMan.SupportedTimezones.Contains(est));
        }

        [TestMethod()]
        public void ChangeTimezoneTest()
        {
            IShellEvent shellEvent = new ShellEvent();
            shellEvent.TimeStamp = DateTime.MinValue;

            IShellEventCollection shellEventCollection = new ShellEventCollection
            {
                shellEvent
            };

            TimezoneManager timeMan = new TimezoneManager(shellEventCollection, new ShellItemCollection(), new Selected(), new TimezoneCollection());

            Timezone est = new Timezone("Eastern Standard Time");

            timeMan.TimezoneChangeHandler(est);

            Assert.IsTrue(timeMan.CurrentTimezone == est);
            Assert.IsTrue(shellEventCollection.First().TimeStamp == TimeZoneInfo.ConvertTimeFromUtc(DateTime.MinValue, TimeZoneInfo.Local));
        }
    }
}