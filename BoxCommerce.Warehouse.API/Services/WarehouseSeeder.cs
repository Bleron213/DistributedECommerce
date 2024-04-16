using BoxCommerce.Warehouse.Application.Common.Infrastructure;
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
            _context.Stocks.Add(new Stock("VOLKSWAGEN_GOLF_GTI", StockType.Vehicle));
            _context.Stocks.Add(new Stock("BMW_M3", StockType.Vehicle));
            _context.Stocks.Add(new Stock("AUDI_RS3", StockType.Vehicle));
            _context.Stocks.Add(new Stock("MERCEDES_BENZ_AMG_C63", StockType.Vehicle));
            _context.Stocks.Add(new Stock("PORSCHE_911_CARRERA", StockType.Vehicle));
            _context.Stocks.Add(new Stock("VOLKSWAGEN_PASSAT_R_LINE", StockType.Vehicle));
            _context.Stocks.Add(new Stock("BMW_M5", StockType.Vehicle));
            _context.Stocks.Add(new Stock("AUDI_RS6_AVANT", StockType.Vehicle));
            _context.Stocks.Add(new Stock("MERCEDES_BENZ_AMG_E63", StockType.Vehicle));

            _context.Stocks.Add(new Stock("TURBO_INLINE_FOUR_2_0L", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("NATURALLY_ASPIRATED_V6_3_5L", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("HYBRID_SYNERGY_DRIVE_HSD", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("DIRECTION_INJECTION_INLINE_THREE_1_2L", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("SUPERCHARGED_V8_5_0L", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("TWIN_TURBO_V6_3_0L", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("DIESEL_COMPRESSION_IGNITION_2_2L", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("ELECTRIC_MOTOR_60_KWH", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("MILD_HYBRID_INLINE_FOUR_1_6L", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("VARIABLE_COMPRESSION_RATIO_INLINE_FOUR_1_5L", StockType.COMPONENT_ENGINE));
            _context.Stocks.Add(new Stock("BODY_ON_FRAME", StockType.COMPONENT_CHASSIS));
            _context.Stocks.Add(new Stock("UNIBODY", StockType.COMPONENT_CHASSIS));
            _context.Stocks.Add(new Stock("BACKBONE", StockType.COMPONENT_CHASSIS));
            _context.Stocks.Add(new Stock("TUBULAR_SPACE_FRAME", StockType.COMPONENT_CHASSIS));
            _context.Stocks.Add(new Stock("SUBFRAME", StockType.COMPONENT_CHASSIS));
            _context.Stocks.Add(new Stock("HYBRID_UNIBODY_WITH_REINFORCEMENT", StockType.COMPONENT_CHASSIS));
            _context.Stocks.Add(new Stock("ROLLING_CHASIS", StockType.COMPONENT_CHASSIS));
            _context.Stocks.Add(new Stock("SEMI_MONOCOQUE", StockType.COMPONENT_CHASSIS));
            _context.Stocks.Add(new Stock("MODULAR_CHASSIS", StockType.COMPONENT_CHASSIS));
            _context.Stocks.Add(new Stock("SPORT_PERFORMANCE_PACK", StockType.COMPONENT_OPTION_PACK));
            _context.Stocks.Add(new Stock("LUXURY_COMFORT_PACK", StockType.COMPONENT_OPTION_PACK));
            _context.Stocks.Add(new Stock("TECH_PACK", StockType.COMPONENT_OPTION_PACK));
            _context.Stocks.Add(new Stock("OFFROAD_ADVENTURE_PACK", StockType.COMPONENT_OPTION_PACK));
            _context.Stocks.Add(new Stock("CITY_URBAN_PACK", StockType.COMPONENT_OPTION_PACK));
            _context.Stocks.Add(new Stock("ECO_EFFICIENCY_PACK", StockType.COMPONENT_OPTION_PACK));
            _context.Stocks.Add(new Stock("WINTER_WEATHER_PACK", StockType.COMPONENT_OPTION_PACK));
            _context.Stocks.Add(new Stock("FAMILY_SAFETY_PACK", StockType.COMPONENT_OPTION_PACK));
            _context.Stocks.Add(new Stock("PERFORMANCE_TRACK_PACK", StockType.COMPONENT_OPTION_PACK));
            _context.Stocks.Add(new Stock("PREMIUM_SOUND_PACK", StockType.COMPONENT_OPTION_PACK));

            _context.SaveChanges();
        }
    }
}
