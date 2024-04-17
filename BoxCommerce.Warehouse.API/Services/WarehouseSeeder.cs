using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Application.Services;
using BoxCommerce.Warehouse.Domain.Entities;
using BoxCommerce.Warehouse.Domain.Enums;
using BoxCommerce.Warehouse.Infrastructure.Data;

namespace BoxCommerce.Warehouse.API.Services
{
    public class WarehouseSeeder
    {
        private readonly WarehouseDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public WarehouseSeeder(
            WarehouseDbContext context, 
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task SeedDefaultData()
        {

            Random rnd = new Random();
            _context.Components.Add(new Component("TURBO_INLINE_FOUR_2_0L", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("NATURALLY_ASPIRATED_V6_3_5L", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("HYBRID_SYNERGY_DRIVE_HSD", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("DIRECTION_INJECTION_INLINE_THREE_1_2L", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("SUPERCHARGED_V8_5_0L", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("TWIN_TURBO_V6_3_0L", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("DIESEL_COMPRESSION_IGNITION_2_2L", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("ELECTRIC_MOTOR_60_KWH", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("MILD_HYBRID_INLINE_FOUR_1_6L", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("VARIABLE_COMPRESSION_RATIO_INLINE_FOUR_1_5L", rnd.Next(1, 100_000).ToString(), ComponentType.ENGINE));
            _context.Components.Add(new Component("BODY_ON_FRAME", rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS));
            _context.Components.Add(new Component("UNIBODY", rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS));
            _context.Components.Add(new Component("BACKBONE", rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS));
            _context.Components.Add(new Component("TUBULAR_SPACE_FRAME", rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS));
            _context.Components.Add(new Component("SUBFRAME", rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS));
            _context.Components.Add(new Component("HYBRID_UNIBODY_WITH_REINFORCEMENT", rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS));
            _context.Components.Add(new Component("ROLLING_CHASIS", rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS));
            _context.Components.Add(new Component("SEMI_MONOCOQUE", rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS));
            _context.Components.Add(new Component("MODULAR_CHASSIS", rnd.Next(1, 100_000).ToString(), ComponentType.CHASSIS));
            _context.Components.Add(new Component("SPORT_PERFORMANCE_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));
            _context.Components.Add(new Component("LUXURY_COMFORT_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));
            _context.Components.Add(new Component("TECH_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));
            _context.Components.Add(new Component("OFFROAD_ADVENTURE_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));
            _context.Components.Add(new Component("CITY_URBAN_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));
            _context.Components.Add(new Component("ECO_EFFICIENCY_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));
            _context.Components.Add(new Component("WINTER_WEATHER_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));
            _context.Components.Add(new Component("FAMILY_SAFETY_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));
            _context.Components.Add(new Component("PERFORMANCE_TRACK_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));
            _context.Components.Add(new Component("PREMIUM_SOUND_PACK", rnd.Next(1, 100_000).ToString(), ComponentType.OPTION_PACK));

            _context.SaveChanges();

            rnd = new Random();

            var componentHashingService = new ComponentHashingService();

            _context.Products.Add(new Product("VOLKSWAGEN_GOLF_GTI", rnd.Next(1, 100_000).ToString()));
            _context.Products.Add(new Product("BMW_M3", rnd.Next(1, 100_000).ToString(), componentHashingService.HashComponentCodes("BMW_M3", ["TURBO_INLINE_FOUR_2_0L", "BODY_ON_FRAME", "SPORT_PERFORMANCE_PACK"])));
            _context.Products.Add(new Product("AUDI_RS3", rnd.Next(1, 100_000).ToString(), componentHashingService.HashComponentCodes("AUDI_RS3",["DIESEL_COMPRESSION_IGNITION_2_2L", "HYBRID_UNIBODY_WITH_REINFORCEMENT", "OFFROAD_ADVENTURE_PACK"])));
            _context.Products.Add(new Product("MERCEDES_BENZ_AMG_C63", rnd.Next(1, 100_000).ToString()));
            _context.Products.Add(new Product("PORSCHE_911_CARRERA",rnd.Next(1, 100_000).ToString()));
            _context.Products.Add(new Product("VOLKSWAGEN_PASSAT_R_LINE",rnd.Next(1, 100_000).ToString()));
            _context.Products.Add(new Product("BMW_M5",rnd.Next(1, 100_000).ToString()));
            _context.Products.Add(new Product("AUDI_RS6_AVANT",rnd.Next(1, 100_000).ToString()));
            _context.Products.Add(new Product("MERCEDES_BENZ_AMG_E63",rnd.Next(1, 100_000).ToString()));
            _context.Products.Add(new Product("PORSCHE_CAYENNE_TURBO_S",rnd.Next(1, 100_000).ToString()));


            _context.SaveChanges();


        }
    }
}
