﻿using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Security;
using Nop.Services.Security;

namespace Nop.Web.Framework.Security.Captcha;

/// <summary>
/// Represents the HTTP client to request reCAPTCHA service
/// </summary>
public partial class CaptchaHttpClient
{
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly HttpClient _httpClient;
    protected readonly IWebHelper _webHelper;

    #endregion

    #region Ctor

    public CaptchaHttpClient(CaptchaSettings captchaSettings,
        HttpClient client,
        IWebHelper webHelper)
    {
        _captchaSettings = captchaSettings;
        _httpClient = client;
        _webHelper = webHelper;

        //configure client
        client.BaseAddress = new Uri(captchaSettings.ReCaptchaApiUrl);
        client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, $"nopCommerce-{NopVersion.CURRENT_VERSION}");

        if (captchaSettings.ReCaptchaRequestTimeout is int timeout && timeout > 0)
            client.Timeout = TimeSpan.FromSeconds(timeout);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Validate reCAPTCHA
    /// </summary>
    /// <param name="responseValue">Response value</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the asynchronous task whose result contains response from the reCAPTCHA service
    /// </returns>
    public virtual async Task<CaptchaResponse> ValidateCaptchaAsync(string responseValue)
    {
        //prepare URL to request
        var url = string.Format(NopSecurityDefaults.RecaptchaValidationPath,
            _captchaSettings.ReCaptchaPrivateKey,
            responseValue,
            _webHelper.GetCurrentIpAddress());

        //get response
        var response = await _httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<CaptchaResponse>(response);

    }

    #endregion
}