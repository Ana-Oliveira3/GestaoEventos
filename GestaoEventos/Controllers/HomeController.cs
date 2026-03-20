using System.Diagnostics;
using GestaoEventos.Data;
using GestaoEventos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoEventos.Controllers
{
    public class HomeController : Controller
    {
       private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var hoje = DateTime.Now.Date;

            var eventos = await _context.Eventos
                .Include(e => e.Categoria)
                .Include(e => e.Local)
                .Where(e => e.Data >= hoje)
                .OrderBy(e => e.Data)
                .ToListAsync();

            return View(eventos);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
