﻿using TMPro;
using UnityEngine;
using UniView.Binding;

namespace UniView.Views
{
    public class TestView : View<TestViewModel>
    {
        protected override void Setup(ISetup<TestViewModel> setup)
        {
            setup.Content("First Name", x => x.FirstName);
            setup.Content("Last Name", x => x.LastName);
            setup.Content("Age", x => x.Age);
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
    }

    public class TestViewModel
    {
        public string FirstName;
        public string LastName;
        public int Age;
    }
}