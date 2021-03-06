﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eLib.Database.Interfaces;
using eLib.Database.Model;
using eStationCore.Model.Common.Enums;

namespace eStationCore.Model.Sale.Entity
{
    public class Company : Tracable 
    {

        [Key]
        public Guid CompanyGuid { get; set; }

        public Guid AddressGuid { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public CustomerStatus CustomerStatus { get; set; }

        public string Comment { get; set; }



        [ForeignKey("AddressGuid")]
        public virtual Address Address { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();

       
    }
}
