using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Domain.Enums;
using BoxCommerce.Orders.Infrastructure.Data;
using System.Reflection;
using System.Threading;

namespace BoxCommerce.Orders.API.Services
{
    public class OrderSeeder
    {
        private readonly OrderDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public OrderSeeder(OrderDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task SeedDefaultData()
        {
            if (!_context.Components.Any(x => x.ComponentCode == "TURBO_INLINE_FOUR_2_0L" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Turbocharged Inline-Four 2.0L", "TURBO_INLINE_FOUR_2_0L", ComponentType.ENGINE));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "NATURALLY_ASPIRATED_V6_3_5L" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Naturally Aspirated V6 3.5L", "NATURALLY_ASPIRATED_V6_3_5L", ComponentType.ENGINE));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "HYBRID_SYNERGY_DRIVE_HSD" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Hybrid Synergy Drive(HSD) 1.8L", "HYBRID_SYNERGY_DRIVE_HSD", ComponentType.ENGINE));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "DIRECTION_INJECTION_INLINE_THREE_1_2L" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Direct Injection Inline-Three 1.2L", "DIRECTION_INJECTION_INLINE_THREE_1_2L", ComponentType.ENGINE));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "SUPERCHARGED_V8_5_0L" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Supercharged V8 5.0L", "SUPERCHARGED_V8_5_0L", ComponentType.ENGINE));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "TWIN_TURBO_V6_3_0L" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Twin - turbocharged V6 3.0L", "TWIN_TURBO_V6_3_0L", ComponentType.ENGINE));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "DIESEL_COMPRESSION_IGNITION_2_2L" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Diesel Compression - Ignition 2.2L", "DIESEL_COMPRESSION_IGNITION_2_2L", ComponentType.ENGINE));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "ELECTRIC_MOTOR_60_KWH" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Electric Motor 60 kWh", "ELECTRIC_MOTOR_60_KWH", ComponentType.ENGINE));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "MILD_HYBRID_INLINE_FOUR_1_6L" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Mild Hybrid Inline-Four 1.6L", "MILD_HYBRID_INLINE_FOUR_1_6L", ComponentType.ENGINE));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "VARIABLE_COMPRESSION_RATIO_INLINE_FOUR_1_5L" && x.ComponentType == ComponentType.ENGINE))
            {
                _context.Components.Add(new Domain.Entities.Component("Variable Compression Ratio Inline - Four 1.5L", "VARIABLE_COMPRESSION_RATIO_INLINE_FOUR_1_5L", ComponentType.ENGINE));
            }
            _context.SaveChanges();


            if (!_context.Components.Any(x => x.ComponentCode == "BODY_ON_FRAME" && x.ComponentType == ComponentType.CHASSIS))
            {
                _context.Components.Add(new Domain.Entities.Component("Body - on - Frame", "BODY_ON_FRAME", ComponentType.CHASSIS));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "UNIBODY" && x.ComponentType == ComponentType.CHASSIS))
            {
                _context.Components.Add(new Domain.Entities.Component("Unibody(Monocoque)", "UNIBODY", ComponentType.CHASSIS));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "BACKBONE" && x.ComponentType == ComponentType.CHASSIS))
            {
                _context.Components.Add(new Domain.Entities.Component("Backbone", "BACKBONE", ComponentType.CHASSIS));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "TUBULAR_SPACE_FRAME" && x.ComponentType == ComponentType.CHASSIS))
            {
                _context.Components.Add(new Domain.Entities.Component("Tubular Space Frame", "TUBULAR_SPACE_FRAME", ComponentType.CHASSIS));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "SUBFRAME" && x.ComponentType == ComponentType.CHASSIS))
            {
                _context.Components.Add(new Domain.Entities.Component("Subframe", "SUBFRAME", ComponentType.CHASSIS));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "HYBRID_UNIBODY_WITH_REINFORCEMENT" && x.ComponentType == ComponentType.CHASSIS))
            {
                _context.Components.Add(new Domain.Entities.Component("Hybrid(Unibody with Reinforcement)", "HYBRID_UNIBODY_WITH_REINFORCEMENT", ComponentType.CHASSIS));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "ROLLING_CHASIS" && x.ComponentType == ComponentType.CHASSIS))
            {
                _context.Components.Add(new Domain.Entities.Component("Rolling Chassis", "ROLLING_CHASIS", ComponentType.CHASSIS));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "SEMI_MONOCOQUE" && x.ComponentType == ComponentType.CHASSIS))
            {
                _context.Components.Add(new Domain.Entities.Component("Semi - Monocoque", "SEMI_MONOCOQUE", ComponentType.CHASSIS));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "MODULAR_CHASSIS" && x.ComponentType == ComponentType.CHASSIS))
            {
                _context.Components.Add(new Domain.Entities.Component("Modular Chassis", "MODULAR_CHASSIS", ComponentType.CHASSIS));
            }
            _context.SaveChanges();

            if (!_context.Components.Any(x => x.ComponentCode == "SPORT_PERFORMANCE_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("Sport Performance Pack", "SPORT_PERFORMANCE_PACK", ComponentType.OPTION_PACK));
            }

            if (!_context.Components.Any(x => x.ComponentCode == "LUXURY_COMFORT_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("Luxury Comfort Pack", "LUXURY_COMFORT_PACK", ComponentType.OPTION_PACK));
            }

            if (!_context.Components.Any(x => x.ComponentCode == "TECH_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("Technology Package", "TECH_PACK", ComponentType.OPTION_PACK));
            }

            if (!_context.Components.Any(x => x.ComponentCode == "OFFROAD_ADVENTURE_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("Off-Road Adventure Pack", "OFFROAD_ADVENTURE_PACK", ComponentType.OPTION_PACK));
            }

            if (!_context.Components.Any(x => x.ComponentCode == "CITY_URBAN_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("City Urban Pack", "CITY_URBAN_PACK", ComponentType.OPTION_PACK));
            }

            if (!_context.Components.Any(x => x.ComponentCode == "ECO_EFFICIENCY_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("Eco Efficiency Pack", "ECO_EFFICIENCY_PACK", ComponentType.OPTION_PACK));
            }

            if (!_context.Components.Any(x => x.ComponentCode == "WINTER_WEATHER_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("Winter Weather Pack", "WINTER_WEATHER_PACK", ComponentType.OPTION_PACK));
            }

            if (!_context.Components.Any(x => x.ComponentCode == "FAMILY_SAFETY_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("Family Safety Pack", "FAMILY_SAFETY_PACK", ComponentType.OPTION_PACK));
            }

            if (!_context.Components.Any(x => x.ComponentCode == "PERFORMANCE_TRACK_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("Performance Track Pack", "PERFORMANCE_TRACK_PACK", ComponentType.OPTION_PACK));
            }
            if (!_context.Components.Any(x => x.ComponentCode == "PREMIUM_SOUND_PACK" && x.ComponentType == ComponentType.OPTION_PACK))
            {
                _context.Components.Add(new Domain.Entities.Component("Premium Sound Pack", "PREMIUM_SOUND_PACK", ComponentType.OPTION_PACK));
            }
            _context.SaveChanges();

            // Adding cars to the context with amalgamated codes
            if(!_context.Products.Any(x => x.Code == "VOLKSWAGEN_GOLF_GTI"))
            {
                _context.Products.Add(new Domain.Entities.Product("Volkswagen Golf GTI", "VOLKSWAGEN_GOLF_GTI"));
            }

            if(!_context.Products.Any(x => x.Code == "BMW_M3"))
            {
                _context.Products.Add(new Domain.Entities.Product("BMW M3", "BMW_M3"));
            }

            if (!_context.Products.Any(x => x.Code == "AUDI_RS3"))
            {
                _context.Products.Add(new Domain.Entities.Product("Audi RS3", "AUDI_RS3"));
            }

            if (!_context.Products.Any(x => x.Code == "MERCEDES_BENZ_AMG_C63"))
            {
                _context.Products.Add(new Domain.Entities.Product("Mercedes-Benz AMG C63", "MERCEDES_BENZ_AMG_C63"));

            }

            if (!_context.Products.Any(x => x.Code == "PORSCHE_911_CARRERA"))
            {
                _context.Products.Add(new Domain.Entities.Product("Porsche 911 Carrera", "PORSCHE_911_CARRERA"));

            }

            if (!_context.Products.Any(x => x.Code == "VOLKSWAGEN_PASSAT_R_LINE"))
            {
                _context.Products.Add(new Domain.Entities.Product("Volkswagen Passat R-Line", "VOLKSWAGEN_PASSAT_R_LINE"));

            }

            if (!_context.Products.Any(x => x.Code == "BMW_M5"))
            {
                _context.Products.Add(new Domain.Entities.Product("BMW M5", "BMW_M5"));

            }

            if (!_context.Products.Any(x => x.Code == "AUDI_RS6_AVANT"))
            {
                _context.Products.Add(new Domain.Entities.Product("Audi RS6 Avant", "AUDI_RS6_AVANT"));

            }

            if (!_context.Products.Any(x => x.Code == "MERCEDES_BENZ_AMG_E63"))
            {
                _context.Products.Add(new Domain.Entities.Product("Mercedes-Benz AMG E63", "MERCEDES_BENZ_AMG_E63"));

            }
            if (!_context.Products.Any(x => x.Code == "PORSCHE_CAYENNE_TURBO_S"))
            {
                _context.Products.Add(new Domain.Entities.Product("Porsche Cayenne Turbo S", "PORSCHE_CAYENNE_TURBO_S"));

            }
            _context.SaveChanges();
        }
    }
}
