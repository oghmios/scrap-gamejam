/*************************************************************************
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  Description: Screen Manager. Controls GUI of the game states
  
 -------------------------------------------------------------------------
  Created by:
  - Pedro Fuentes

*************************************************************************/

using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    #region Fields
	
	// set the desired aspect ratio (the values in this example are
    // hard-coded for 16:9, but you could make them into public
    // variables instead so you can set them at design time)
	private float targetaspect = 640.0f / 960.0f;
		
	// determine the game window's current aspect ratio
    private float windowaspect = (float)Screen.width / (float)Screen.height;

   // current viewport height should be scaled by this amount
    private float scaleheight; 

    // define and create our GUI delegate
    // se le llama a un método o función de menú a visualizar
   
	// private delegate void GUIMethod();
    //  private GUIMethod currentGUIMethod;

	// private float currentTime;
	
    #endregion

    #region Propierties
	
    #endregion

    #region Functions

	
	void Awake(){
		 scaleheight = windowaspect / targetaspect;
	}
	
	public float getTargetAspect()
     {
            return targetaspect;
     } 
	
	public float getWindowAspect()
     {
            return windowaspect;
     }
	
	public float getScaleHeight()
     {		
            return scaleheight;
     }
	
  /* public void SetStateGUI(string newGUI)
    {
        switch (newGUI)
        {
            case "MainMenu":
                this.currentGUIMethod = MainMenu;
				currentTime = 1.0f;
                break;
            case "DifficultyMenu":
                this.currentGUIMethod = DifficultyMenu;
                break;
            case "OptionsMenu":
                this.currentGUIMethod = OptionsMenu;
                break;
            case "WinMenu":
                this.currentGUIMethod = WinMenu;
                break;
            case "GameoverMenu":
                this.currentGUIMethod = GameoverMenu;
                break;
            case "ClearMenu":
                this.currentGUIMethod = ClearMenu;
                break;
            case "PauseMenu":
                this.currentGUIMethod = PauseMenu;
                break;
			case "GamePlay":
                this.currentGUIMethod = GamePlay;
                break;
        }
        
    }

    // LIMPIA EL MENU
    private void ClearMenu()
    {
    }

    // MENU PRINCIPAL DEL JUEGO
    public void MainMenu()
    {
		currentTime -= Time.deltaTime;
		
		if (currentTime < 0.0f)
		{
            if ((Application.GetStreamProgressForLevel("MainMenu") == 1))
            {
                
            }
		}
    }

    // MENU DE OPCIONES
    private void OptionsMenu()
    {


    }

    // MENU DE DIFICULTAD
    private void DifficultyMenu()
    {

    }

    // MENU DE VICTORIA
    public void WinMenu()
    {

    }

    // MENU DE GAMEOVER
    public void GameoverMenu()
    {

    }

    // MENU DE PAUSA
    public void PauseMenu()
    {

    }
	
	// JUEGO
    public void GamePlay(){

    }

    public void OnGUI()
    {
        this.currentGUIMethod();
    } */
	
    #endregion
}
