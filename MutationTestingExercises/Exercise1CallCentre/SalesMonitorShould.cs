﻿using System;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace MutationTestingExercises.Exercise1CallCentre
{
    [TestFixture]
    public class SalesMonitorShould
    {
        private SalesMonitor _salesMonitor;
        private Mock<Alert> _alert;

        [SetUp]
        public void SetUp()
        {
            _salesMonitor = new SalesMonitor();
            _alert = new Mock<Alert>();

            _salesMonitor.AddAlert(_alert.Object);
        }

        [TestCase(101)]
        [TestCase(100)]
        [TestCase(1000)]
        public void Raise_an_alert_when_a_high_value_sale_is_made(int saleAmount)
        {
            _salesMonitor.ProcessSale(saleAmount);

            _alert.Verify(a => a.Raise());
        }

        [TestCase(3)]
        [TestCase(99)]
        public void Not_raise_an_alert_when_a_low_value_sale_is_made(int saleAmount)
        {
            _salesMonitor.ProcessSale(saleAmount);

            _alert.Verify(a => a.Raise(), Times.Never);
        }

        [Test]
        public void Raise_as_many_alerts_as_listeners()
        {
            var salesMonitor = new SalesMonitor();
            int alertCount = new Random().Next(1, 9999);
            var alertListeners = new List<Mock<Alert>>();
            for (int i = 0; i < alertCount; i++)
            {
                var alertMock = new Mock<Alert>();
                alertListeners.Add(alertMock);
                salesMonitor.AddAlert(alertMock.Object);
            }

            int higherThanThreshHoldValue = 999;
            salesMonitor.ProcessSale(higherThanThreshHoldValue);

            foreach(var alertMock in alertListeners)
                alertMock.Verify(a => a.Raise());
        }
    }
}
