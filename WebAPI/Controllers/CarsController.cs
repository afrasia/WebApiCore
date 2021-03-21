using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCore.Data;
using EFCore.Models;
using Microsoft.AspNetCore.Http;

namespace EFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly CarDbContext _context;

        public CarsController(CarDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        /// <summary>
        /// Get collection of Cars.
        /// </summary>
        /// <returns>A colection of Cars</returns>
        /// <response code="200">Returns a collection of Cars</response>
        /// <response code="500">Internal error</response>      
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Car>>> Get()
        {
            return Ok(await _context.Cars.ToListAsync());
        }

        // GET: Cars/d2cab0c9-7e94-409e-2b9a-08d88428ae4a
        /// <summary>
        /// Get a Car.
        /// </summary>
        /// <param id="id"></param>
        /// <returns>A Car</returns>
        /// <response code="201">Returns a collection of Cars</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="404">If the Car is null</response>      
        /// <response code="500">Internal error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Car>> GetById(Guid id)
        {
            Car car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        // POST: Cars
        /// <summary>
        /// Creates a Car.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Cars
        ///     {
        ///        "make": "make",
        ///        "price": 0
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created Car</returns>
        /// <response code="201">Returns the newly created Car</response>
        /// <response code="400">If the Car is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Car>> CreateAsync([Bind("Make,Price")] CarBase carBase)
        {
            Car car = new Car
            {
                Make = carBase.Make,
                Price = carBase.Price
            };

            _context.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = car.ID }, car);
        }

        // PUT: Cars/5
        /// <summary>
        /// Upserts a Car.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Cars
        ///     {
        ///        "make": "make",
        ///        "price": 0
        ///     }
        ///
        /// </remarks>
        /// <param id="id"></param>
        /// <returns>An upserted Car</returns>
        /// <response code="200">Returns the updated Car</response>
        /// <response code="201">Returns the newly created Car</response>
        /// <response code="400">If the Car or id is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Car>> Upsert(Guid id, [Bind("Make,Price")] CarBase carBase)
        {
            Car car = new Car
            {
                Make = carBase.Make,
                Price = carBase.Price
            };

            if (!CarExists(id))
            {
                car.ID = id;
                _context.Add(car);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = car.ID }, car);
            }

            Car dbCar = await _context.Cars.FindAsync(id);
            dbCar.Price = car.Price;
            dbCar.Make = car.Make;

            _context.Update(dbCar);
            await _context.SaveChangesAsync();

            return Ok(dbCar);
        }

        // DELETE: Cars/5
        /// <summary>
        /// Deletes a Car.
        /// </summary>
        /// <param id="id"></param>
        /// <response code="202">Car is deleted</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return Accepted();
        }

        private bool CarExists(Guid id)
        {
            return _context.Cars.Any(e => e.ID == id);
        }
    }
}
