using consumirAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;

namespace consumirAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _clientFactory;
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("APIClient");
            List<Produto> produtos = null;

            HttpResponseMessage response = await client.GetAsync("api/produtos"); // Substitua pela rota correta da sua API

            if (response.IsSuccessStatusCode)
            {
                produtos = await response.Content.ReadFromJsonAsync<List<Produto>>();
            }

            return View(produtos);
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

        // GET: Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("APIClient");
                HttpResponseMessage response = await client.PostAsJsonAsync("api/produtos", produto);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle error
                    ModelState.AddModelError(string.Empty, "Não foi possível criar o produto.");
                }
            }

            return View(produto);
        }

    }

    
}
