﻿namespace BiomasaEUPT.Modelos
{
    using BiomasaEUPT.Modelos.Tablas;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Linq;

    public class BiomasaEUPTContext : DbContext
    {
        // El contexto se ha configurado para usar una cadena de conexión 'BiomasaEUPTContext' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'BiomasaEUPT.Modelos.BiomasaEUPTContext' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'BiomasaEUPTContext'  en el archivo de configuración de la aplicación.
        public BiomasaEUPTContext()
            : base("name=BiomasaEUPTContext")
        //: base("metadata=res://*/Modelo.csdl|res://*/Modelo.ssdl|res://*/Modelo.msl;provider=System.Data.SqlClient;provider connection string='data source=155.210.68.124,49170;initial catalog=BiomasaEUPT;persist security info=True;user id=usuario;password=usuario;MultipleActiveResultSets=True;App=EntityFramework'")
        //: base("metadata=res://*/Modelo.csdl|res://*/;provider=System.Data.SqlClient;provider connection string='data source=155.210.68.124,49170;initial catalog=BiomasaEUPT;persist security info=True;user id=usuario;password=usuario;MultipleActiveResultSets=True;App=EntityFramework'")
        //: base(nameOrConnectionString: ConnectionString())       
        {
            Database.Connection.ConnectionString = Database.Connection.ConnectionString.Replace("********", "usuario");

            // CUIDADO -> Borra la tabla y la vuelve a crear
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BiomasaEUPTContext>());
        }

        public int GuardarCambios<TEntity>() where TEntity : class
        {
            // http://stackoverflow.com/questions/33403838/how-can-i-tell-entity-framework-to-save-changes-only-for-a-specific-dbset
            var original = this.ChangeTracker.Entries()
                        .Where(x => !typeof(TEntity).IsAssignableFrom(x.Entity.GetType()) && x.State != EntityState.Unchanged)
                        .GroupBy(x => x.State)
                        .ToList();

            foreach (var entry in this.ChangeTracker.Entries().Where(x => !typeof(TEntity).IsAssignableFrom(x.Entity.GetType())))
            {
                entry.State = EntityState.Unchanged;
            }

            var rows = base.SaveChanges();

            foreach (var state in original)
            {
                foreach (var entry in state)
                {
                    entry.State = state.Key;
                }
            }

            return rows;
        }

        public bool HayCambios<TEntity>(bool detectarEstadoInicial = true) where TEntity : class
        {
            return this.ChangeTracker.Entries().Where(
                x => typeof(TEntity).IsAssignableFrom(x.Entity.GetType()) &&
                x.State != EntityState.Unchanged &&
                (detectarEstadoInicial && x.State == EntityState.Modified ? (x.OriginalValues.PropertyNames.Where(y => x.OriginalValues[y] != null && x.CurrentValues[y] != null && x.OriginalValues[y].ToString() != x.CurrentValues[y].ToString()).ToList().Count > 0) : true)
                ).ToList().Count > 0;
        }

        private static string ConnectionString()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = "155.210.68.124,49170";
            sqlBuilder.InitialCatalog = "BiomasaEUPT";
            sqlBuilder.PersistSecurityInfo = true;
            sqlBuilder.IntegratedSecurity = true;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.UserID = "usuario";
            sqlBuilder.Password = "usuario";
            sqlBuilder.ApplicationName = "EntityFramework";

            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            //entityBuilder.Metadata = "res://*/Modelo.csdl|res://*/Modelo.ssdl|res://*/Modelo.msl";
            entityBuilder.Provider = "System.Data.SqlClient";

            //Console.WriteLine(entityBuilder.ToString());
            return entityBuilder.ToString();
        }

        /*protected override DbEntityValidationResult ValidateEntity(
      System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
            if (entityEntry.Entity is Usuario && entityEntry.State == EntityState.Added)
            {
                Usuario post = entityEntry.Entity as Usuario;
                if (Usuarios.Where(p => p.Nombre == post.Nombre).Count() > 0)
                {
                    result.ValidationErrors.Add(
                            new DbValidationError("nombre", "El nombre debe ser único."));
                }
            }

            if (result.ValidationErrors.Count > 0)
            {
                return result;
            }
            else
            {
                return base.ValidateEntity(entityEntry, items);
            }
        }*/

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //var convention = new AttributeToColumnAnnotationConvention<DefaultValueAttribute, string>("SqlDefaultValue", (p, attributes) => attributes.SingleOrDefault().Value.ToString());
            //modelBuilder.Conventions.Add(convention);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoUsuario> TiposUsuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<TipoCliente> TiposClientes { get; set; }
        public DbSet<GrupoCliente> GruposClientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<TipoProveedor> TiposProveedores { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<MateriaPrima> MateriasPrimas { get; set; }
        public DbSet<TipoMateriaPrima> TiposMateriasPrimas { get; set; }
        public DbSet<GrupoMateriaPrima> GruposMateriasPrimas { get; set; }
        public DbSet<Recepcion> Recepciones { get; set; }
        public DbSet<EstadoRecepcion> EstadosRecepciones { get; set; }
        public DbSet<SitioRecepcion> SitiosRecepciones { get; set; }
        public DbSet<HuecoRecepcion> HuecosRecepciones { get; set; }
        public DbSet<ProductoTerminado> ProductosTerminados { get; set; }
        public DbSet<TipoProductoTerminado> TiposProductosTerminados { get; set; }
        public DbSet<GrupoProductoTerminado> GruposProductosTerminados { get; set; }
        public DbSet<Salida> Salidas { get; set; }
        public DbSet<EstadoSalida> EstadosSalidas { get; set; }
        public DbSet<SitioRecogida> SitiosRecogidas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Comunidad> Comunidades { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
    }

}