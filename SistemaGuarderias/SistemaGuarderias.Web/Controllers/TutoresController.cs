using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGuarderias.Application.DTOs;
using SistemaGuarderias.Web.Models;
using System.Text;

namespace SistemaGuarderias.Web.Controllers
{
    public class TutoresController : Controller
    {
        private readonly HttpClient _httpClient;

        public TutoresController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7271/api");
        }

        // Listar
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Tutores");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tutores = JsonConvert.DeserializeObject<IEnumerable<TutoresViewModel>>(content);
                return View("Index", tutores);
            }

            return View(new List<TutoresViewModel>());
        }

        // Crear
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TutoresViewModel tutor)
        {
            if (ModelState.IsValid)
            {
                var tutorDTO = new TutorDTO
                {
                    Nombre = tutor.Nombre,
                    Apellido = tutor.Apellido,
                    Telefono = tutor.Telefono,
                    Cedula = tutor.Cedula,
                    CorreoElectronico = tutor.CorreoElectronico
                };

                var json = JsonConvert.SerializeObject(tutorDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Tutores", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al añadir tutor");
                }
            }
            return View(tutor);
        }

        // Editar
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Tutores/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tutor = JsonConvert.DeserializeObject<TutoresViewModel>(content);

                return View(tutor);
            }
            else
            {
                TempData["Error"] = "Error al cargar el tutor para editar.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TutoresViewModel tutor)
        {
            if (ModelState.IsValid)
            {
                var tutorDTO = new TutorDTO
                {
                    Nombre = tutor.Nombre,
                    Apellido = tutor.Apellido,
                    Telefono = tutor.Telefono,
                    Cedula = tutor.Cedula,
                    CorreoElectronico = tutor.CorreoElectronico
                };

                var json = JsonConvert.SerializeObject(tutorDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/Tutores/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar tutor.");
                }
            }

            return View(tutor);
        }

        // Mostrar específicamente
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Tutores/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            TutoresViewModel? tutor = null;

            try
            {
                tutor = JsonConvert.DeserializeObject<TutoresViewModel>(content);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Error al deserializar el tutor: {ex.Message}");
                return BadRequest("Error en la deserialización del tutor.");
            }

            if (tutor == null)
            {
                return NotFound();
            }

            return View(tutor);
        }

        // Eliminar
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Tutores/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el tutor.";
                return RedirectToAction("Index");
            }
        }
    }
}
