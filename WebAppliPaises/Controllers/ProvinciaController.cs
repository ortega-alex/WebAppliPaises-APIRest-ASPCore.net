using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppliPaises.Models;

namespace WebAppliPaises.Controllers
{
    [Produces("application/json")]
    [Route("api/Pais/{PaisId}/Provincia")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProvinciaController : Controller
    {
        private readonly AplicationDbContext context;
        public ProvinciaController(AplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Provincia> GetAll(int PaisId)
        {
            return context.Provincias.Where(x => x.PaisId == PaisId).ToList();
        }

        [HttpGet("{id}" , Name = "provinciaById")]
        public IActionResult GetById(int id)
        {
            var provincia = context.Provincias.FirstOrDefault(x => x.Id == id);

            if ( provincia == null )
            {
                return NotFound();
            }

            return new ObjectResult(provincia);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Provincia provincia , int idPais)
        {
            provincia.PaisId = idPais;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Provincias.Add(provincia);
            context.SaveChanges();

            return new CreatedAtRouteResult("provinciaById", new { id = provincia.Id }, provincia);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] Provincia provincia , int id)
        {
            if(provincia.Id != id)
            {
                return BadRequest();
            }

            context.Entry(provincia).State = EntityState.Modified;
            context.SaveChanges();


            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var provincia = context.Provincias.FirstOrDefault(x => x.Id == id);
            if (provincia == null)
            {
                return BadRequest();
            }

            context.Provincias.Remove(provincia);
            context.SaveChanges();

            return Ok();
        }

    }
}