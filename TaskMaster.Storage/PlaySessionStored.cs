//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskMaster.Storage
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlaySessionStored
    {
        public System.Guid Id { get; set; }
        public System.Guid TaskItemId { get; set; }
        public int PlayedTime_Hours { get; set; }
        public System.DateTime Date { get; set; }
        public int PlayedTime_Minutes { get; set; }
        public int PlayedTime_Seconds { get; set; }
        public int PlayedTime_Days { get; set; }
    }
}
