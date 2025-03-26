using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGuarderias.Application.DTOs;
using SistemaGuarderias.Web.Models;
using System.Text;

namespace SistemaGuarderias.Web.Controllers
{
    public class NinosController : Controller
    {
        private readonly HttpClient _httpClient;

        public NinosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7271/api");
        }

        // Listar 
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Ninos");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var ninos = JsonConvert.DeserializeObject<IEnumerable<NinosViewModel>>(content);
                return View("Index", ninos);
            }

            return View(new List<NinosViewModel>());
        }

        // Crear 
        public async Task<IActionResult> Create()
        {
            ViewBag.GuarderiaList = await GetGuarderias();
            ViewBag.TutorList = await GetTutores();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NinoDTO nino)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(nino);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Ninos", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear niño.");
                }
            }

            ViewBag.GuarderiaList = await GetGuarderias();
            ViewBag.TutorList = await GetTutores();
            return View(nino);
        }

        // Editar 
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Ninos/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var nino = JsonConvert.DeserializeObject<NinosViewModel>(content);

                ViewBag.GuarderiaList = await GetGuarderias();
                ViewBag.TutorList = await GetTutores();

                return View(nino);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NinoDTO nino)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(nino);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/Ninos/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar niño.");
                }
            }

            ViewBag.GuarderiaList = await GetGuarderias();
            ViewBag.TutorList = await GetTutores();
            return View(nino);
        }

        // Mostrar detalles de un niño
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Ninos/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            var nino = JsonConvert.DeserializeObject<NinosViewModel>(content);

            if (nino == null)
            {
                return NotFound();
            }

            if (nino.GuarderiaId > 0)
            {
                var responseGuarderia = await _httpClient.GetAsync($"/api/Guarderias/{nino.GuarderiaId}");
                if (responseGuarderia.IsSuccessStatusCode)
                {
                    var guarderiaContent = await responseGuarderia.Content.ReadAsStringAsync();
                    nino.Guarderia = JsonConvert.DeserializeObject<GuarderiasViewModel>(guarderiaContent) ?? new GuarderiasViewModel();
                }
            }

            if (nino.TutorId > 0)
            {
                var responseTutor = await _httpClient.GetAsync($"/api/Tutores/{nino.TutorId}");
                if (responseTutor.IsSuccessStatusCode)
                {
                    var tutorContent = await responseTutor.Content.ReadAsStringAsync();
                    nino.Tutor = JsonConvert.DeserializeObject<TutoresViewModel>(tutorContent) ?? new TutoresViewModel();
                }
            }

            return View(nino);
        }

        // Eliminar 
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Ninos/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el niño.";
                return RedirectToAction("Index");
            }
        }

        private async Task<IEnumerable<GuarderiasViewModel>> GetGuarderias()
        {
            var response = await _httpClient.GetAsync("/api/Guarderias");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<GuarderiasViewModel>>(content) ?? new List<GuarderiasViewModel>();
            }
            return new List<GuarderiasViewModel>();
        }

        private async Task<IEnumerable<TutoresViewModel>> GetTutores()
        {
            var response = await _httpClient.GetAsync("/api/Tutores");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TutoresViewModel>>(content) ?? new List<TutoresViewModel>();
            }
            return new List<TutoresViewModel>();
        }
    }
}
