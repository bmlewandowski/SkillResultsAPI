﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkillResultsAPI
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SkillResultsDBEntities : DbContext
    {
        public SkillResultsDBEntities()
            : base("name=SkillResultsDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AreaCategoriesCustom> AreaCategoriesCustoms { get; set; }
        public virtual DbSet<AreaCategoriesMaster> AreaCategoriesMasters { get; set; }
        public virtual DbSet<AreasCustom> AreasCustoms { get; set; }
        public virtual DbSet<AreasLocal> AreasLocals { get; set; }
        public virtual DbSet<AreasMaster> AreasMasters { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<CategoriesCustom> CategoriesCustoms { get; set; }
        public virtual DbSet<CategoriesLocal> CategoriesLocals { get; set; }
        public virtual DbSet<CategoriesMaster> CategoriesMasters { get; set; }
        public virtual DbSet<CategorySkillsCustom> CategorySkillsCustoms { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationUser> OrganizationUsers { get; set; }
        public virtual DbSet<SkillsCustom> SkillsCustoms { get; set; }
        public virtual DbSet<SkillsLocal> SkillsLocals { get; set; }
        public virtual DbSet<SkillsMaster> SkillsMasters { get; set; }
        public virtual DbSet<UserSkill> UserSkills { get; set; }
        public virtual DbSet<CategorySkillsMaster> CategorySkillsMasters { get; set; }
    
        public virtual ObjectResult<Nullable<int>> delete_areacategoriescustoms(Nullable<int> categoryid)
        {
            var categoryidParameter = categoryid.HasValue ?
                new ObjectParameter("categoryid", categoryid) :
                new ObjectParameter("categoryid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("delete_areacategoriescustoms", categoryidParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> delete_catagory(Nullable<int> catagoryid)
        {
            var catagoryidParameter = catagoryid.HasValue ?
                new ObjectParameter("catagoryid", catagoryid) :
                new ObjectParameter("catagoryid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("delete_catagory", catagoryidParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> delete_categoryskillcustoms(Nullable<int> skillid)
        {
            var skillidParameter = skillid.HasValue ?
                new ObjectParameter("skillid", skillid) :
                new ObjectParameter("skillid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("delete_categoryskillcustoms", skillidParameter);
        }
    
        public virtual ObjectResult<get_categoriesbyareacustoms_Result> get_categoriesbyareacustoms(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<get_categoriesbyareacustoms_Result>("get_categoriesbyareacustoms", idParameter);
        }
    
        public virtual ObjectResult<get_categoriesbyareamasters_Result> get_categoriesbyareamasters(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<get_categoriesbyareamasters_Result>("get_categoriesbyareamasters", idParameter);
        }
    
        public virtual ObjectResult<get_skillsbycategorycustoms_Result> get_skillsbycategorycustoms(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<get_skillsbycategorycustoms_Result>("get_skillsbycategorycustoms", idParameter);
        }
    
        public virtual ObjectResult<get_skillsbycategorymasters_Result> get_skillsbycategorymasters(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<get_skillsbycategorymasters_Result>("get_skillsbycategorymasters", idParameter);
        }
    
        public virtual ObjectResult<user_object_Result> user_object(string userId)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<user_object_Result>("user_object", userIdParameter);
        }
    }
}
