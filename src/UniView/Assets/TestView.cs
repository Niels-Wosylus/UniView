using UnityEngine;
using UniView.Binding;

namespace UniView.Views
{
    public class TestView : View<TestViewModel>
    {
        private bool _isHovered;
        
        protected override void Setup(ISetup<TestViewModel> setup)
        {
            setup.Content("[Displayed Content]", x => x);
            setup.Content("[Is Hovered]", _ => _isHovered).Continuously();
            setup.Content("First Name", x => x.FirstName);
            setup.Content("Last Name", x => x.LastName);
            setup.Content("Age", x => x.Age).Continuously();
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

        [ContextMenu("Refresh")]
        private void DoRefresh()
        {
            Refresh();
        }

        private void Update()
        {
            if (DisplayedContent != null)
                DisplayedContent.Age += 1;

            if (Input.GetKeyDown(KeyCode.Space))
                _isHovered = !_isHovered;
        }
    }

    public class TestViewModel
    {
        public string FirstName;
        public string LastName;
        public int Age;
    }
}
