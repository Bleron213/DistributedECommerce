using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Application.Orders.Commands;
using BoxCommerce.Orders.Common.Requests;
using BoxCommerce.Orders.Common.Response;
using BoxCommerce.Warehouse.ApiClient.Abstractions;
using BoxCommerce.Warehouse.ApiClient.Clients;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxCommerce.Orders.API.Controllers.V1
{
    public class ComponentController : ApiBaseController
    {
        private readonly ISender _mediator;
        private readonly IWarehouseApiClient _warehouseApiClient;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(
            ISender mediator,
            IWarehouseApiClient warehouseApiClient,
            ILogger<ComponentController> logger
            )
        {
            _mediator = mediator;
            _warehouseApiClient = warehouseApiClient;
            _logger = logger;
        }


        /// <summary>
        /// Hardcoded because not relevant
        /// 
        /// Imagine these products come from another Microservice and are cached here. 
        /// Every hour the list is refreshed 
        /// </summary>
        /// <returns></returns>
        [HttpGet("AvailableComponents")]
        public async Task<IActionResult> GetAvailableProducts()
        {
            return Ok(new List<ComponentResponse>
            {
                new ComponentResponse("Turbocharged Inline-Four 2.0L", "TURBO_INLINE_FOUR_2_0L", ComponentType.ENGINE),
                new ComponentResponse("Naturally Aspirated V6 3.5L", "NATURALLY_ASPIRATED_V6_3_5L", ComponentType.ENGINE),
                new ComponentResponse("Hybrid Synergy Drive(HSD) 1.8L", "HYBRID_SYNERGY_DRIVE_HSD", ComponentType.ENGINE),
                new ComponentResponse("Direct Injection Inline-Three 1.2L", "DIRECTION_INJECTION_INLINE_THREE_1_2L", ComponentType.ENGINE),
                new ComponentResponse("Supercharged V8 5.0L", "SUPERCHARGED_V8_5_0L", ComponentType.ENGINE),
                new ComponentResponse("Twin - turbocharged V6 3.0L", "TWIN_TURBO_V6_3_0L", ComponentType.ENGINE),
                new ComponentResponse("Diesel Compression - Ignition 2.2L", "DIESEL_COMPRESSION_IGNITION_2_2L", ComponentType.ENGINE),
                new ComponentResponse("Electric Motor 60 kWh", "ELECTRIC_MOTOR_60_KWH", ComponentType.ENGINE),
                new ComponentResponse("Mild Hybrid Inline-Four 1.6L", "MILD_HYBRID_INLINE_FOUR_1_6L", ComponentType.ENGINE),
                new ComponentResponse("Variable Compression Ratio Inline - Four 1.5L", "VARIABLE_COMPRESSION_RATIO_INLINE_FOUR_1_5L", ComponentType.ENGINE),
                new ComponentResponse("Body - on - Frame", "BODY_ON_FRAME", ComponentType.CHASSIS),
                new ComponentResponse("Unibody(Monocoque)", "UNIBODY", ComponentType.CHASSIS),
                new ComponentResponse("Backbone", "BACKBONE", ComponentType.CHASSIS),
                new ComponentResponse("Tubular Space Frame", "TUBULAR_SPACE_FRAME", ComponentType.CHASSIS),
                new ComponentResponse("Subframe", "SUBFRAME", ComponentType.CHASSIS),
                new ComponentResponse("Hybrid(Unibody with Reinforcement)", "HYBRID_UNIBODY_WITH_REINFORCEMENT", ComponentType.CHASSIS),
                new ComponentResponse("Rolling Chassis", "ROLLING_CHASIS", ComponentType.CHASSIS),
                new ComponentResponse("Semi - Monocoque", "SEMI_MONOCOQUE", ComponentType.CHASSIS),
                new ComponentResponse("Modular Chassis", "MODULAR_CHASSIS", ComponentType.CHASSIS),
                new ComponentResponse("Sport Performance Pack", "SPORT_PERFORMANCE_PACK", ComponentType.OPTION_PACK),
                new ComponentResponse("Luxury Comfort Pack", "LUXURY_COMFORT_PACK", ComponentType.OPTION_PACK),
                new ComponentResponse("Technology Package", "TECH_PACK", ComponentType.OPTION_PACK),
                new ComponentResponse("Off-Road Adventure Pack", "OFFROAD_ADVENTURE_PACK", ComponentType.OPTION_PACK),
                new ComponentResponse("City Urban Pack", "CITY_URBAN_PACK", ComponentType.OPTION_PACK),
                new ComponentResponse("Eco Efficiency Pack", "ECO_EFFICIENCY_PACK", ComponentType.OPTION_PACK),
                new ComponentResponse("Winter Weather Pack", "WINTER_WEATHER_PACK", ComponentType.OPTION_PACK),
                new ComponentResponse("Family Safety Pack", "FAMILY_SAFETY_PACK", ComponentType.OPTION_PACK),
                new ComponentResponse("Performance Track Pack", "PERFORMANCE_TRACK_PACK", ComponentType.OPTION_PACK),
                new ComponentResponse("Premium Sound Pack", "PREMIUM_SOUND_PACK", ComponentType.OPTION_PACK),
            });
        }


    }
}
