using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum viewState { view2D, view3D }
    viewState state;

    public GameObject Grid2d;
    public GameObject Grid3d;

    void Start()
    {
        state = viewState.view2D;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeView()
    {
        if(state == viewState.view2D) 
        {
            Grid2d.SetActive(false);
            Grid3d.SetActive(true);
            state = viewState.view3D;
        }
        else
        {
            Grid3d.SetActive(false);
            Grid2d.SetActive(true);
            state = viewState.view2D;
        }
    }
}
