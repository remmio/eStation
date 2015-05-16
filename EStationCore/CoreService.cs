﻿using EStationCore.Managers;


namespace EStationCore
{
    public class CoreService
    {

        /// <summary>
        /// System de Gestion Des Citernes
        /// </summary>
        public CiternesManager Citernes = new CiternesManager();


        /// <summary>
        /// System de Gestion Des Carburants
        /// </summary>
        public FuelManager Fuels = new FuelManager();


        /// <summary>
        /// System de Gestion Des Pompes
        /// </summary>
        public PompesManager Pompes = new PompesManager();


        /// <summary>
        /// System de Gestion Des Clients
        /// </summary>
        public CustomersManager Customers = new CustomersManager();


        /// <summary>
        /// System de Gestion Des Staffs
        /// </summary>
        public HrManager HumanResource = new HrManager();


        /// <summary>
        /// System de Gestion de Security 
        /// </summary>
        public SecurityManager Authentication = new SecurityManager();


        /// <summary>
        /// Gestion des Ressources Financieres
        /// </summary>
        public EconomatManager Economat = new EconomatManager();


        /// <summary>
        /// Gestion des Documents Etudiants et Staffs
        /// </summary>
        public DocumentsManager Documents = new DocumentsManager();


        ///// <summary>
        ///// Messagerie
        ///// </summary>
        //public CommManager Comm = new CommManager();


        ///// <summary>
        ///// Analytics
        ///// </summary>
        //public AnalyticsManager Analytics = new AnalyticsManager();



    }
}