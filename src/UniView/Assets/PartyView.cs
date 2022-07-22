using UnityEngine;
using Wosylus.UniView;
using Wosylus.UniView.Binding;

public class PartyView : View<PartyViewModel>
{
    protected override void Setup(ISetup<PartyViewModel> setup)
    {
        setup.Content("Party Name", x => x.PartyName);
        setup.Content("Person A", x => x.PersonA);
        setup.Content("Person B", x => x.PersonB);
        setup.Content("Person C", x => x.PersonC);
    }

    private void Start()
    {
        var model = BuildModel();
        Display(model);
    }

    [ContextMenu("Clear B")]
    public void ClearB()
    {
        var model = BuildModel();
        model.PersonB = null;
        Display(model);
    }
    
    [ContextMenu("Show All")]
    public void ShowAll()
    {
        var model = BuildModel();
        Display(model);
    }

    [ContextMenu("Show Null")]
    public void ShowNull()
    {
        Display(null);
    }
    
    [ContextMenu("Clear All")]
    public void ClearAll()
    {
        Clear();
    }

    private PartyViewModel BuildModel()
    {
        return new PartyViewModel
        {
            PartyName = "Happy Party",
            PersonA = new TestViewModel
            {
                FirstName = "Jenny",
                LastName = "McGuire",
                Age = 14
            },
            PersonB = new TestViewModel
            {
                FirstName = "Harvard",
                LastName = "Travinsky",
                Age = 71
            },
            PersonC = new TestViewModel
            {
                FirstName = "Roger",
                LastName = "Charles",
                Age = 32
            }
        };
    }
}

public class PartyViewModel
{
    public string PartyName;
    public TestViewModel PersonA;
    public TestViewModel PersonB;
    public TestViewModel PersonC;
}