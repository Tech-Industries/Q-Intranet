﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orizon.Web.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OrizonDevEntities : DbContext
    {
        public OrizonDevEntities()
            : base("name=OrizonDevEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Bug> Bugs { get; set; }
        public virtual DbSet<BugTag> BugTags { get; set; }
        public virtual DbSet<ProjectAssignee> ProjectAssignees { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectTask> ProjectTasks { get; set; }
    }
}
