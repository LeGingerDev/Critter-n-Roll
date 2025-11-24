using Core.Singleton;
using System.Collections.Generic;
using System.Linq;

public class CustomisationManager : MonoSingleton<CustomisationManager>
{
    public List<Customisation> allCustomisations = new List<Customisation>();

    private bool _customisationFlag = false;

    public Customisation GetDefault() => allCustomisations.First();

    public Customisation GetCustomisationById(string id)
    {
        return allCustomisations.Find(c => c.id == id);
    }

    public Customisation GetCustomisationByDisplayName(string displayName)
    {
        return allCustomisations.Find(c => c.displayName == displayName);
    }

    public bool GetCustomisationFlag() => _customisationFlag;
    public bool SetCustomisationFlag(bool value) => _customisationFlag = value;

}