using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Data;
using WebApiCore.Models;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarDbContext _context;

        public CarsController(CarDbContext context)
        {
            _context = context;
        }

        // GET: api/Cars
        /// <summary>
        /// Get collection of Cars.
        /// </summary>
        /// <returns>A colection of Cars</returns>
        /// <response code="200">Returns a collection of Cars</response>
        /// <response code="500">Internal error</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        // GET: api/Cars/5
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
        public async Task<ActionResult<Car>> GetCar(Guid id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        // PUT: api/Cars/5
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutCar(Guid id, [Bind("Make,Price")] Car car)
        {
            Car? storedCar = null;
            try
            {
                storedCar = _context.Cars.Single(c => c.ID == id);
                storedCar.Price = car.Price;
                storedCar.Make = car.Make;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(storedCar);
        }

        // POST: api/Cars
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Car>> PostCar([Bind("Make,Price")] Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.ID }, car);
        }

        // DELETE: api/Cars/5
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
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

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
