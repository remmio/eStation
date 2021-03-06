﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using eLib;
using eStationCore.IManagers;
using eStationCore.Model;
using eStationCore.Model.Fuel.Entity;
using eStationCore.Model.Fuel.Views;

namespace eStationCore.Store.SqlServer
{
    public class CiternesManager : ICiternesManager
    {
        private readonly StationContext _db;

        public CiternesManager(StationContext stationContext)
        {
            _db = stationContext;
        }

        #region CRUD

        public async Task<bool> Post(Citerne myCiternes)
        {
            using (var db = new StationContext())
            {                
                if (myCiternes.CiterneGuid == Guid.Empty) myCiternes.CiterneGuid = Guid.NewGuid();

                myCiternes.DateAdded = DateTime.Now;
                myCiternes.LastEditDate = DateTime.Now;

                db.Citernes.Add(myCiternes);
                return await db.SaveChangesAsync() > 0;
            }
        }       

        public async Task<bool> Put(Citerne myCiternes)
        {
            using (var db = new StationContext())
            {
                myCiternes.LastEditDate = DateTime.Now;

                db.Citernes.Attach(myCiternes);
                db.Entry(myCiternes).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> Delete(Guid citerneGuid)
        {
            using (var db = new StationContext())
            {
                var myObject = await db.Citernes.FindAsync(citerneGuid);

                if (myObject == null) throw new InvalidOperationException("CITERNE_NOT_FOUND");

                myObject.LastEditDate = DateTime.Now;
                myObject.IsDeleted = true;

                db.Citernes.Attach(myObject);
                db.Entry(myObject).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<Citerne> Get(Guid citerneGuid)
        {
            using (var db = new StationContext())
                return await db.Citernes.FindAsync(citerneGuid);
        }

        public async Task<FuelDelivery> GetStock(Guid stockGuid)
        {
            using (var db = new StationContext())
                return await db.FuelDeliverys.FindAsync(stockGuid);
        }

        public async Task<bool> Post(FuelDelivery fuelDelivery)
        {
            using (var db = new StationContext())
            {
                if (fuelDelivery.FuelDeliveryGuid == Guid.Empty) fuelDelivery.FuelDeliveryGuid = Guid.NewGuid();

                fuelDelivery.DateAdded = DateTime.Now;
                fuelDelivery.LastEditDate = DateTime.Now;

                db.FuelDeliverys.Add(fuelDelivery);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> Put(FuelDelivery myDelivery)
        {
            using (var db = new StationContext())
            {
                myDelivery.LastEditDate = DateTime.Now;

                db.FuelDeliverys.Attach(myDelivery);
                db.Entry(myDelivery).State = EntityState.Modified;
                return await db.SaveChangesAsync() > 0;
            }
        }



        #endregion




        #region Views



        public async Task<List<FuelCard>> GetFuelCards()
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Fuels.Where(f=> !f.IsDeleted).ToList().OrderByDescending(c => c.DateAdded).Select(c => new FuelCard(c)).ToList();
            });
        }


        public async Task<double> GetCiterneFuelBalance(Guid citerneGuid) => await StaticGetCiterneFuelBalance(citerneGuid);


        public async Task<List<string>> GetSuppliers()
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
            {
                var deps = db.FuelDeliverys.OrderByDescending(f => f.DateAdded)
                        .Where(s => !string.IsNullOrEmpty(s.Supplier))
                        .Select(s => s.Supplier)
                        .ToList();
                    deps.AddRange(db.OilDeliveries.OrderByDescending(f => f.DateAdded)
                        .Where(s => !string.IsNullOrEmpty(s.Supplier))
                        .Select(s => s.Supplier)
                        .ToList());

                return !deps.Any()
                    ? new List<string>{ "Winxo" }
                    : deps.Distinct().ToList();
            }
            });
        }


        public async Task<List<CiterneCard>> GetCiternesCards()
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Citernes.Where(c=>!c.IsDeleted).ToList().OrderByDescending(c=> c.DateAdded).Select(c => new CiterneCard(c)).ToList();
           });
        }


        public async Task<List<Citerne>> GetCiternes()
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Citernes.Where(c=> !c.IsDeleted).OrderByDescending(c=> c.DateAdded).ToList();
            });
        }


        public async Task<List<FuelDeliveryCard>> GetCiterneStocks(Guid citerneGuid)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Citernes.Find(citerneGuid)?.Deliveries.Where(d=> !d.IsDeleted).OrderByDescending(s=> s.DateAdded).ToList().Select(s => new FuelDeliveryCard(s)).ToList();
             });
        }



        #endregion



        #region Internal Static


        


        internal async static Task<double> StaticGetCiterneFuelBalance(Guid citerneGuid)
        {
            return await Task.Run(() => {
                using (var db = new StationContext()){
                try
                {
                    var stocks = db.Citernes.Find(citerneGuid)?.Deliveries.Sum(s => s.QuantityDelivered) ?? 0;

                    var prelevs = db.Citernes.Find(citerneGuid)?.Prelevements.Sum(p => p.Result) ?? 0;

                    return stocks - prelevs;
                }
                catch (Exception exception)
                {
                    DebugHelper.WriteException(exception);
                    return 0;
                }
            }
           });  
        }


        internal async static Task<double> GetCiterneStock(Guid citerneGuid)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
            {
                var stocks = db.Citernes.Find(citerneGuid)?.Deliveries.Sum(s => s.QuantityDelivered);
                if (stocks == null)
                    return 0;
                return (double)stocks;
            }
            });
        }


        internal async static Task<double> GetCiternePrelevement(Guid citerneGuid)
        {
            return await Task.Run(() => {
                using (var db = new StationContext())
                return db.Citernes.Find(citerneGuid)?
                         .Pompes.Select(p => p.Prelevements.Select(v => v.Result))
                         .Select(s => s.Sum())
                         .Sum() ?? 0;
           });
        }


        #endregion

        
    }
}
