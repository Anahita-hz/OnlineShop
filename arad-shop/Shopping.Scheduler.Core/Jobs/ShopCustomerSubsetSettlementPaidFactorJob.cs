﻿using System;
using System.Web.Hosting;
using FluentScheduler;
using NLog;
using Shopping.Scheduler.Core.AppStart;
using Shopping.Scheduler.Core.Interfaces;

namespace Shopping.Scheduler.Core.Jobs
{
    public class ShopCustomerSubsetSettlementPaidFactorJob: IJob, IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;
        private readonly IShopCustomerSubsetSettlementService _shopCustomerSubsetSettlementService;
        private readonly Logger _logger;
        public ShopCustomerSubsetSettlementPaidFactorJob()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _shopCustomerSubsetSettlementService = Bootstrapper.Container.Resolve<IShopCustomerSubsetSettlementService>();
            HostingEnvironment.RegisterObject(this);
        }
        public void Execute()
        {
            try
            {
                lock (_lock)
                {
                    if (_shuttingDown)
                        return;
                    _logger.Info($"ShopCustomerSubsetSettlementJob execute at ### {DateTime.Now} ###");

                    _shopCustomerSubsetSettlementService.SettlementPaidFactor();

                    _logger.Info($"ShopCustomerSubsetSettlementJob end at ### {DateTime.Now} ###");
                }
            }
            finally
            {
                lock (_lock)
                {
                    Bootstrapper.Container.Release(_shopCustomerSubsetSettlementService);
                }
                HostingEnvironment.UnregisterObject(this);
            }
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }
    }
}