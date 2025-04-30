﻿using System.Text.Json.Serialization;

namespace Domain.Models;

public class CookieConsent
{
    [JsonPropertyName("essential")]
    public bool Essential { get; set; }

    [JsonPropertyName("functional")]
    public bool Functional { get; set; }
}
