﻿namespace Nop.Web.Areas.Admin.Helpers;

public partial interface ITinyMceHelper
{
    Task<string> GetTinyMceLanguageAsync();
}