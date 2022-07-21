using UnityEngine;
using Wosylus.UniView;
using Wosylus.UniView.Binding;
using Wosylus.UniView.Binding.Signals;

public class TestView : View<TestViewModel>
{
    private bool _isHovered;
    private ISignalSender<string> _clickedOutput;

    protected override void Setup(ISetup<TestViewModel> setup)
    {
        setup.Content("<<Is Hovered>>", _ => _isHovered).Continuously();
        setup.Content("First Name", x => x.FirstName);
        setup.Content("Last Name", x => x.LastName);
        setup.Content("Age", x => x.Age).Continuously();
        
        setup.Input<string>("Clicked string", OnClickedString);
        setup.Output("Clicked string", out _clickedOutput);
    }

    private void OnClickedString(string clickedName)
    {
        Debug.Log(clickedName);
    }

    private void Start()
    {
        DisplayCharles();
    }

    [ContextMenu("Display Charles")]
    private void DisplayCharles()
    {
        var content = new TestViewModel
        {
            FirstName = "Charles",
            LastName = "Runner",
            Age = 56
        };

        Display(content);
    }
        
    [ContextMenu("Display Jenny")]
    private void DisplayJenny()
    {
        var content = new TestViewModel
        {
            FirstName = "Jenny",
            LastName = "McGuire",
            Age = 14
        };

        Display(content);
    }

    [ContextMenu("Clear")]
    private void DoClear()
    {
        Clear();
    }

    private void Update()
    {
        if (DisplayedContent != null)
            DisplayedContent.Age += 1;

        if (Input.GetKeyDown(KeyCode.Space))
            _isHovered = !_isHovered;
        
        if (Input.GetKeyDown(KeyCode.Return))
            _clickedOutput.Send("Some signal");
    }
}

public class TestViewModel
{
    public string FirstName;
    public string LastName;
    public int Age;
}