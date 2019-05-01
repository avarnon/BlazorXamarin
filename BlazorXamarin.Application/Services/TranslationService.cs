﻿using BlazorXamarin.Application.Contracts;
using System.Globalization;

namespace BlazorXamarin.Application.Services
{
    public class TranslationService : ITranslationService
    {
        public string this[string index] => Resources.Resources.ResourceManager.GetString(index, CultureInfo.DefaultThreadCurrentUICulture);
    }
}
