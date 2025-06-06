﻿using Newtonsoft.Json;

namespace Nop.Web.Framework.Security.Captcha;

/// <summary>
/// Google reCAPTCHA response
/// </summary>
public partial class CaptchaResponse
{
    #region Ctor

    public CaptchaResponse()
    {
        Errors = new List<string>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the action name for this request (important to verify)
    /// </summary>
    [JsonProperty(PropertyName = "action")]

    public string Action { get; set; }

    [JsonProperty(PropertyName = "score")]
    public decimal Score { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether validation is success
    /// </summary>
    [JsonProperty(PropertyName = "success")]
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets a date and time of the challenge load
    /// </summary>
    [JsonProperty(PropertyName = "challenge_ts")]
    public DateTime? ChallengeDateTime { get; set; }

    /// <summary>
    /// Gets or sets the hostname of the site where the reCAPTCHA was solved
    /// </summary>
    [JsonProperty(PropertyName = "hostname")]
    public string Hostname { get; set; }

    /// <summary>
    /// Gets or sets errors
    /// </summary>
    [JsonProperty(PropertyName = "error-codes")]
    public List<string> Errors { get; set; }

    #endregion
}