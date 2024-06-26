﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistributedECommerce.Warehouse.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
    }
}
