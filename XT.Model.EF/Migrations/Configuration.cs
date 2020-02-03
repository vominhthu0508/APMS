namespace XT.Model.EF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<XT.Model.MyDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(XT.Model.MyDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            InitCreate(context);
        }

        private void InitCreate(XT.Model.MyDataContext context)
        {
            //add list user types
            var user_types = new System.Collections.Generic.List<User_Type>();
            foreach (UserTypeEnum type in Enum.GetValues(typeof(UserTypeEnum)))
            {
                user_types.Add(new User_Type { Id = (int)type, User_Type_Name = type.ToString(), Created_Date = DateTime.Now });
            }

            context.User_Types.AddOrUpdate(user_types.ToArray());

            //add list role types
            var role_types = new System.Collections.Generic.List<Role_Type>();
            foreach (RoleTypeEnum type in Enum.GetValues(typeof(RoleTypeEnum)))
            {
                role_types.Add(new Role_Type { Id = (int)type, Role_Type_Name = type.ToString(), Created_Date = DateTime.Now });
            }

            context.Role_Types.AddOrUpdate(role_types.ToArray());

            

            //context.SaveChanges();
            SaveChanges(context);
        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}
