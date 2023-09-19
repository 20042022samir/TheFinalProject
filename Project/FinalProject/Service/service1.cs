using FinalProject.Contexts;
using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;

namespace FinalProject.Service
{
    public class service1
    {
        private readonly FinalProjectDatbase _context;
        public service1( FinalProjectDatbase context)
        {
            _context = context;
        }
        public async Task<List<Country>> Getall()
        {
            List<Country> countries=_context.Countries.Where(x=>!x.IsDeleted).ToList();
            return countries; 
        }

        public async Task<List<City>> GetAllCities()
        {
            List<City> cities = await _context.Cities.Where(x => !x.IsDeleted).ToListAsync();
            return cities;
        }

        public async Task<List<Typee>> GetAllHouseTypes()
        {
            List<Typee> typees = await _context.Typees.Where(x => !x.IsDeleted)
                .ToListAsync();
            return typees;
        }
      
        public async Task<PageAbout> GetPageInfo()
        {
            PageAbout about = await _context.PageAbouts.Where(x => !x.IsDeleted && x.Id == 1).FirstOrDefaultAsync();
            return about;
        }
    }
}
