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
    public class CalificacionesController : Controller
    {
        private readonly GestionDeEstudiantesContext _context;

        public CalificacionesController(GestionDeEstudiantesContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var gestionDeEstudiantesContext = _context.Calificaciones.Include(c => c.IdMateriaNavigation);
            return View(await gestionDeEstudiantesContext.ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Calificaciones == null)
            {
                return NotFound();
            }

            var calificacion = await _context.Calificaciones.Include(x=> x.IdMateriaNavigation).FirstOrDefaultAsync(x=> x.Id== id);

            if (calificacion == null)
            {
                return NotFound();
            }

            ViewData["Materia"] = calificacion.IdMateriaNavigation.Nombre.ToUpper();
            ViewData["AñoEscolar"] = calificacion.AnoEscolar;
            ViewData["CalificacionId"] = id;

            var estudiantes = await _context.Estudiantes.Where(x => x.Activo).ToListAsync();

            return View(estudiantes);
        }
        
        public IActionResult Create()
        {
            ViewData["IdMateria"] = new SelectList(_context.Materias, "Id", "Nombre");
            var rnd = new Random();
            
            ViewData["Code"] = $"EST-{rnd.Next(111111, 999999)}";            

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,IdMateria,AnoEscolar,FechaCreacion")] Calificaciones calificaciones)
        {
            if (ModelState.IsValid)
            {
                calificaciones.FechaCreacion = DateTime.Now;

                _context.Add(calificaciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdMateria"] = new SelectList(_context.Materias, "Id", "Nombre", calificaciones.IdMateria);
            var rnd = new Random();
            ViewData["Code"] = $"EST-{rnd.Next(111111, 999999)}";
            

            return View(calificaciones);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Calificaciones == null)
            {
                return NotFound();
            }

            var calificaciones = await _context.Calificaciones.FindAsync(id);
            if (calificaciones == null)
            {
                return NotFound();
            }
            ViewData["IdMateria"] = new SelectList(_context.Materias, "Id", "Nombre", calificaciones.IdMateria);
            return View(calificaciones);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,IdMateria,AnoEscolar,FechaCreacion")] Calificaciones calificaciones)
        {
            if (id != calificaciones.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calificaciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalificacionesExists(calificaciones.Id))
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
            ViewData["IdMateria"] = new SelectList(_context.Materias, "Id", "Nombre", calificaciones.IdMateria);
            return View(calificaciones);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Calificaciones == null)
            {
                return NotFound();
            }

            var calificaciones = await _context.Calificaciones
                .Include(c => c.IdMateriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calificaciones == null)
            {
                return NotFound();
            }

            return View(calificaciones);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Calificaciones == null)
            {
                return Problem("Entity set 'GestionDeEstudiantesContext.Calificaciones'  is null.");
            }
            var calificaciones = await _context.Calificaciones.FindAsync(id);
            if (calificaciones != null)
            {
                _context.Calificaciones.Remove(calificaciones);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalificacionesExists(int id)
        {
          return _context.Calificaciones.Any(e => e.Id == id);
        }

        [HttpPost]
        public JsonResult Calificar(int CalificacionId, int IdEstudiante, int Nota, string Calificacion)
        {
            var cf = new DetalleCalificaciones { IdCalificacion = CalificacionId, IdEstudiante = IdEstudiante, Calificacion = Nota, CalificacionLiteral = Calificacion, FechaCreacion = DateTime.Now };
            _context.DetalleCalificaciones.Add(cf);
            var result = _context.SaveChanges() > 0;

            return Json(result);
        }

        [HttpPost]
        public JsonResult ActualizarCalificacion(int CalificacionId)
        {
            var result = false;
            var calificacion = _context.Calificaciones.FirstOrDefault(x => x.Id == CalificacionId);
            if(calificacion != null)
            {
                calificacion.Calificada = true;
                calificacion.FechaCalificacion = DateTime.Now;
                result = _context.SaveChanges() > 0;
            }

            return Json(result);
        }
    }
}
