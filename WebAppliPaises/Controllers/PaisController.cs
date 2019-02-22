﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [Route("api/Pais")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaisController : Controller
    {

        private readonly AplicationDbContext  context;
        public PaisController(AplicationDbContext contex)
        {
            this.context = contex;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //claims del usuario
            var claims = User.Claims.ToList();

            var esAdmin = claims.Any(x => x.Type == "Admin" && x.Value == "Y");
            if (esAdmin)
            {
                return Ok(context.Paises.ToList());
            } else
            {
                var pais = claims.FirstOrDefault(x => x.Type == "Pais");
                if (pais == null)
                {
                    return Unauthorized();
                }
                return Ok(context.Paises.Where(x => x.Nombre == pais.Value));
            }
           
        }

        [HttpGet("{id}", Name = "paisCreado")]
        public IActionResult GetById(int id)
        {
            var pais = context.Paises.Include(x => x.Provincias).FirstOrDefault(x => x.Id == id);
            if (pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Pais pais)
        {
            if (ModelState.IsValid)
            {
                context.Paises.Add(pais);
                context.SaveChanges();
                return new CreatedAtRouteResult("paisCreado", new { id = pais.Id }, pais);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Pais pais , int id)
        {
            if (pais.Id != id)
            {
                return BadRequest();
            }

            context.Entry(pais).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pais = context.Paises.FirstOrDefault(x => x.Id == id);
            if (pais == null)
            {
                return NotFound();
            }

            context.Paises.Remove(pais);
            context.SaveChanges();
            return Ok();
        }
    }
}