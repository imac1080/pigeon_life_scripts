using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cerrarPestaña : MonoBehaviour, IPointerDownHandler
{
    public GameStates stateManager = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("panelCerrarPestaña");
        stateManager.TitleGame();
    }
}
