﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FISH.ViewModels
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }
    }
}
