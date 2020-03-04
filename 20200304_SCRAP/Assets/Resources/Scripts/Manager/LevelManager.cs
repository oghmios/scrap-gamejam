/*************************************************************************
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  Description: Level Manager. Controls the level
  
 -------------------------------------------------------------------------
  Created by:
  - Pedro Fuentes

*************************************************************************/

using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    #region Fields
	
	private string levelName;
	
    #endregion

    #region Propierties
	
    #endregion

    #region Functions

	/*
	void Awake(){
		 scaleheight = windowaspect / targetaspect;
	}*/
	
	public string getLevelName()
     {
            return levelName;
     }

    public void setLevelName(string levelAux)
    {
        levelName = levelAux;
    }

    #endregion
}
