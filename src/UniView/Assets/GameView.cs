using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wosylus.UniView;
using Wosylus.UniView.Binding;

public class GameView : View<IGamePhase>
{
    protected override void Setup(ISetup<IGamePhase> setup)
    {
        setup.Content("Yes Phase", x => x as YesPhase);
        setup.Content("No Phase", x => x as NoPhase);
    }

    private void Start()
    {
        Display(new ThirdPhase());
    }

    [ContextMenu("Show Null")]
    public void ShowNull()
    {
        Display(null);
    }
    
    [ContextMenu("Show Yes")]
    public void ShowYes()
    {
        Display(new YesPhase());
    }

    [ContextMenu("Show No")]
    public void ShowNo()
    {
        Display(new NoPhase());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("SampleScene");
    }
}

public class ThirdPhase : IGamePhase
{
    
}