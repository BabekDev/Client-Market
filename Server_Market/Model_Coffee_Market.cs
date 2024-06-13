namespace Server_Market
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Model_Coffee_Market : DbContext
    {
        public Model_Coffee_Market()
            : base("name=Model_Coffee_Market")
        {}
        public virtual DbSet<Users> UsersList { get; set; }
        public virtual DbSet<Product> ProductList { get; set; }
        public virtual DbSet<SellProduct> SellList { get; set; }
    }

    public class Users
    {
        public int Id { get; set; }
        public string Login { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }

    public class SellProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Client { get; set; }
    }
}