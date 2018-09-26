﻿using Microsoft.EntityFrameworkCore;

namespace OmEnergo.Models
{
    public class OmEnergoContext : DbContext
    {
        public DbSet<Section> Sections { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductModel> ProductModels { get; set; }

        public DbSet<Stabilizer> Stabilizers { get; set; }

        public DbSet<StabilizerModel> StabilizerModels { get; set; }

        public DbSet<Inverter> Inverters { get; set; }

        public DbSet<InverterModel> InverterModels { get; set; }

        public DbSet<Autotransformer> Autotransformers { get; set; }

        public DbSet<AutotransformerModel> AutotransformerModels { get; set; }

        public OmEnergoContext() { }

        public OmEnergoContext(DbContextOptions<OmEnergoContext> options) : base(options) { }
    }
}
