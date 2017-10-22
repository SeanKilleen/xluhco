﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace xluhco.web.Controllers
{
    public class RedirectController : Controller
    {
        private readonly IShortLinkRepository _shortLinkRepo;
        private readonly Serilog.ILogger _logger;
        private readonly RedirectOptions _redirectOptions;
        private readonly GoogleAnalyticsOptions _gaOptions;

        public RedirectController(IShortLinkRepository shortLinkRepo, Serilog.ILogger logger, IOptions<RedirectOptions> redirectOptions, IOptions<GoogleAnalyticsOptions> gaOptions)
        {
            _shortLinkRepo = shortLinkRepo;
            _logger = logger;
            _redirectOptions = redirectOptions.Value;
            _gaOptions = gaOptions.Value;
        }

        [HttpGet]
        public IActionResult Index(string shortCode)
        {
            _logger.Debug("Entered the redirect for short code {shortCode}", shortCode);
            var redirectItem = _shortLinkRepo.GetByShortCode(shortCode);

            if (string.IsNullOrWhiteSpace(redirectItem?.URL))
            {
                _logger.Warning("No redirect found for requested short code {shortCode}", shortCode);
                return NotFound($"Short link not found for short code '{shortCode}'");
            }

            _logger.Information("Redirecteing {shortCode} to {redirectUrl}", shortCode, redirectItem);

            return View("Index", redirectItem);
        }
    }
}