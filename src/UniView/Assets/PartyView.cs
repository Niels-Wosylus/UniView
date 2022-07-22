using Wosylus.UniView;
using Wosylus.UniView.Binding;
using Wosylus.UniView.Binding.Content.Processors;

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
        var model = new PartyViewModel
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
        
        
        Display(model);
    }
}

public class PartyViewModel
{
    public string PartyName;
    public TestViewModel PersonA;
    public TestViewModel PersonB;
    public TestViewModel PersonC;
}