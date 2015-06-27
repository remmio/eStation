﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EStationCore.Model.Fuel.Entity;

namespace EStationCore.Model.Sale.Entity
{
    public class Journal
    {
        [Key]
        public Guid JournalGuid { get; set; }





        public List<FuelPrelevement> Prelevements { get; set; }

    }
}
