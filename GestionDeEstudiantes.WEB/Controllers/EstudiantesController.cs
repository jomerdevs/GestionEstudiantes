using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionDeEstudiantes.WEB.Data;
using GestionDeEstudiantes.WEB.Entities;

namespace GestionDeEstudiantes.WEB.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly GestionDeEstudiantesContext _context;

        public EstudiantesController(GestionDeEstudiantesContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var gestionDeEstudiantesContext = _context.Estudiantes.Include(e => e.IdColegioNavigation);
            return View(await gestionDeEstudiantesContext.ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Estudiantes == null)
            {
                return NotFound();
            }

            var estudiantes = await _context.Estudiantes
                .Include(e => e.IdColegioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiantes == null)
            {
                return NotFound();
            }

            return View(estudiantes);
        }
        
        public IActionResult Create()
        {
            var rnd = new Random();
            ViewData["Code"] = $"EST-{rnd.Next(111111, 999999)}";
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdColegio,Codigo,Nombre,Apellido1,Apellido2,FechaNacimiento,LugarNacimiento,Foto,Activo,FechaCreacion")] Estudiantes estudiantes)
        {
            if (ModelState.IsValid)
            {
                estudiantes.IdColegio = 1;
                estudiantes.Activo = true;
                estudiantes.FechaCreacion = DateTime.Now;

                _context.Add(estudiantes);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            var rnd = new Random();
            ViewData["Code"] = $"EST-{rnd.Next(111111, 999999)}";

            return View(estudiantes);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Estudiantes == null)
            {
                return NotFound();
            }

            var estudiantes = await _context.Estudiantes.FindAsync(id);
            if (estudiantes == null)
            {
                return NotFound();
            }
            ViewData["IdColegio"] = new SelectList(_context.Colegios, "Id", "Nombre", estudiantes.IdColegio);
            return View(estudiantes);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdColegio,Codigo,Nombre,Apellido1,Apellido2,FechaNacimiento,LugarNacimiento,Foto,Activo,FechaCreacion")] Estudiantes estudiantes)
        {
            if (id != estudiantes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiantes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudiantesExists(estudiantes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdColegio"] = new SelectList(_context.Colegios, "Id", "Nombre", estudiantes.IdColegio);
            return View(estudiantes);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Estudiantes == null)
            {
                return NotFound();
            }

            var estudiantes = await _context.Estudiantes
                .Include(e => e.IdColegioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiantes == null)
            {
                return NotFound();
            }

            return View(estudiantes);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Estudiantes == null)
            {
                return Problem("Entity set 'GestionDeEstudiantesContext.Estudiantes'  is null.");
            }
            var estudiantes = await _context.Estudiantes.FindAsync(id);
            if (estudiantes != null)
            {
                _context.Estudiantes.Remove(estudiantes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudiantesExists(int id)
        {
          return _context.Estudiantes.Any(e => e.Id == id);
        }
    }
}
