using System.Globalization;
using Workspace_Management_System.Application.Contracts;

namespace Workspace_Management_System.Infrastructure.Services;

public class LocalizationService : ILocalizationService
{
    public string GetLocalizedValue(string valueEn, string valueAr)
    {
        var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        return language == "ar"
            ? valueAr
            : valueEn;
    }
}