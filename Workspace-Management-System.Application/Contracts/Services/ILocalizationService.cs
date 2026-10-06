namespace Workspace_Management_System.Application.Contracts;

public interface ILocalizationService
{
    string GetLocalizedValue(string valueEn, string valueAr);
}