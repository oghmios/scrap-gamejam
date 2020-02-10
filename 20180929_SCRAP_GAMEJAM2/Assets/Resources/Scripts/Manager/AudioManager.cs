/*************************************************************************
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  Description: Audio Manager. Stores the call functions and the sounds of the game
  
 -------------------------------------------------------------------------
  Created by:
  - Pedro Fuentes

*************************************************************************/

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    [Header("GAME EFFECTS")]
    public AudioClip buzzerScore;
    public AudioClip blocksMoving01;
    public AudioClip blocksMoving02;
    public AudioClip blocksMoving03;
    public AudioClip explosionStones;

    [Header("BAGS EFFECTS")]
    public AudioClip BagWrong;

    [Header("PLAYER ACTIONS")]
    public AudioClip playerDigImpossible;
    public AudioClip playerDig;
    public AudioClip playerDigDash;
    public AudioClip playerThrow;
    public AudioClip playerJump;
    public AudioClip playerGrounded;
    public AudioClip playerDie;

    [Header("PLAYER VOICES")]
    public AudioClip playerLaughtShort00;
    public AudioClip playerLaughtShort01;
    public AudioClip playerLaughtShort02;
    public AudioClip playerLaughtShort03;
    public AudioClip playerLaughtShort04;
    public AudioClip playerLaughtShort05;
    public AudioClip playerLaughtShort06;
    public AudioClip playerLaughtShort07;
    public AudioClip playerLaughtShort08;
    public AudioClip playerLaughtMedium01;
    public AudioClip playerLaughtMedium02;
    public AudioClip playerLaughtLong01;
    public AudioClip playerLaughtLong02;

    [Header("BAT")]
    public AudioClip batAppear;
    public AudioClip batCrow;
    public AudioClip batExplosion;
    public AudioClip batBreakLayer;
    public AudioClip batFail;
    public AudioClip batExplosionFail;

    [Header("WORM")]
    public AudioClip wormAppear;
    public AudioClip wormPutEgg;
    public AudioClip wormEggExplosion;
    public AudioClip wormDropEffect;

    [Header("JAIL")]
    public AudioClip jailOn;
    public AudioClip jailOff;
    public AudioClip jailIncrease;
    public AudioClip jailDecrease;
    public AudioClip jailHit;

    [Header("COMBO HITS")]
    public AudioClip comboHitx1;
    public AudioClip comboHitx2;
    public AudioClip comboHitx3;
    public AudioClip comboHitx4;
    public AudioClip comboHitx5;
    public AudioClip comboHitx6;
    public AudioClip comboHitx7;
    public AudioClip comboHitx8;
    public AudioClip comboHitxFINAL;

    [Header("COMBOS")]
    public AudioClip combox2;
    public AudioClip combox3;
    public AudioClip combox4;
    public AudioClip combox5;
    public AudioClip combox6;
    public AudioClip combox7;
    public AudioClip combox8;
    public AudioClip comboxFINAL;

    [Header("MUSIC")]
    public AudioClip music01;
    public AudioClip music02;
    public AudioClip music03;
    public AudioClip music04;
    public AudioClip music05;

    #region Functions

    /* MOVING SOUND  */

    // clip + transform
    public AudioSource Play(AudioClip clip, Transform emitter)
        {
            return Play(clip, emitter, 1f, 1f);
        }

        // clip + transform + volume
        public AudioSource Play(AudioClip clip, Transform emitter, float volume)
        {
            return Play(clip, emitter, volume, 1f);
        }

    /// <summary>
    /// Plays a sound by creating an empty game object with an AudioSource
    /// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="emitter"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch)
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            //Create an empty game object
            // GameObject go = new GameObject("Audio: " + clip.name);
            GameObject go = NewObjectPoolerAudio.current.GetPooledAudioSource();

            if (go == null) return null;

            go.transform.position = emitter.position;
            go.transform.parent = emitter;

            // Create the source
            // AudioSource source = go.AddComponent<AudioSource>();

            go.GetComponent<AudioSource>().clip = clip;
            go.GetComponent<AudioSource>().volume = volume;
            go.GetComponent<AudioSource>().pitch = pitch;
            go.SetActive(true);

            go.GetComponent<AudioSource>().Play();

            // Destroy(go, clip.length);
            go.GetComponent<AudioSourceDestroy>().DestroyWithSeconds(clip.length);

            // return source;
            return go.GetComponent<AudioSource>();
        }
        else
        {
            return null;
        }
    }
    /*
    public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch)
    {
        if(PlayerPrefs.GetInt("Sound")==1){
            //Create an empty game object
            GameObject go = new GameObject("Audio: " + clip.name);
            go.transform.position = emitter.position;
            go.transform.parent = emitter;

            //Create the source
            AudioSource source = go.AddComponent<AudioSource>();

            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();

            Destroy(go, clip.length);
            return source;
        }else {
            return null;
        }
}
*/

    /// <summary>
    /// Plays a sound by creating an empty game object with an AudioSource
    /// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="emitter"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch, bool loop, float loopTime)
        {
			if (PlayerPrefs.GetInt("Sound")==1) {
            // Create an empty game object
            // GameObject go = new GameObject("Audio: " + clip.name);
            GameObject go = NewObjectPoolerAudio.current.GetPooledAudioSource();

            if (go == null) return null;

            go.transform.position = emitter.position;
			go.transform.parent = emitter;

            //Create the source
            // AudioSource source = go.AddComponent<AudioSource>();

            go.GetComponent<AudioSource>().clip = clip;
            go.GetComponent<AudioSource>().volume = volume;
            go.GetComponent<AudioSource>().pitch = pitch;
            go.GetComponent<AudioSource>().loop = true;
            go.SetActive(true);

            go.GetComponent<AudioSource>().Play();

            // if (!loop)
            // Destroy(go, loopTime);

             if (!loop)
                go.GetComponent<AudioSourceDestroy>().DestroyWithSeconds(loopTime);

            // return source;
            return go.GetComponent<AudioSource>();

            } else {
				return null;
			}

        }

        /*
        public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch, bool loop, float loopTime)
        {
			if(PlayerPrefs.GetInt("Sound")==1){
				//Create an empty game object
				GameObject go = new GameObject("Audio: " + clip.name);
				go.transform.position = emitter.position;
				go.transform.parent = emitter;
				
				//Create the source
				AudioSource source = go.AddComponent<AudioSource>();

					source.clip = clip;
					source.volume = volume;
					source.pitch = pitch;
					source.loop = true;
					source.Play();

				if (!loop)
					Destroy(go, loopTime);
				return source;
			} else {
				return null;
			}
            
			// Create an empty game object
            // GameObject go = new GameObject("Audio: " + clip.name);
            // go.transform.position = emitter.position;
            // go.transform.parent = emitter;

            // Create the source
            // AudioSource source = go.AddComponent<AudioSource>();
			// if(PlayerPrefs.GetInt("Sound")==1){
			// source.clip = clip;
            // source.volume = volume;
            // source.pitch = pitch;
            // source.loop = true;
            // source.Play();
			// }
            // if (!loop)
            //     Destroy(go, loopTime);
            // return source;
            // 
        }*/

        /*****************************************************************************************/

        /* STATIC SOUND */

        // clip + vector
        public AudioSource Play(AudioClip clip, Vector3 point)
        {
            return Play(clip, point, 1f, 1f);
        }

        // clip + vector + volume
        public AudioSource Play(AudioClip clip, Vector3 point, float volume)
        {
            return Play(clip, point, volume, 1f);
        }

    /// <summary>
    /// Plays a sound at the given point in space by creating an empty game object with an AudioSource
    /// in that place and destroys it after it finished playing.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="point"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public AudioSource Play(AudioClip clip, Vector3 point, float volume, float pitch)
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            // Create an empty game object
            // GameObject go = new GameObject("Audio: " + clip.name);   
            GameObject go = NewObjectPoolerAudio.current.GetPooledAudioSource();

            if (go == null) return null;

            go.transform.position = point;

            //Create the source
            // AudioSource source = go.AddComponent<AudioSource>();

            go.GetComponent<AudioSource>().clip = clip;
            go.GetComponent<AudioSource>().volume = volume;
            go.GetComponent<AudioSource>().pitch = pitch;
            go.SetActive(true);

            go.GetComponent<AudioSource>().Play();

            // Destroy(go, clip.length);
            go.GetComponent<AudioSourceDestroy>().DestroyWithSeconds(clip.length);

            // return source;
            return go.GetComponent<AudioSource>();
        }
        else
        {
            return null;
        }

    }
    /*
    public AudioSource Play(AudioClip clip, Vector3 point, float volume, float pitch)
    {
        if(PlayerPrefs.GetInt("Sound")==1){
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);
        go.transform.position = point;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();

            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();

            Destroy(go, clip.length);
            return source;
        } else {
            return null;
        }

        //Create an empty game object
        // GameObject go = new GameObject("Audio: " + clip.name);
        // go.transform.position = point;

        //Create the source
        // AudioSource source = go.AddComponent<AudioSource>();
        // if(PlayerPrefs.GetInt("Sound")==1){
        // source.clip = clip;
        // source.volume = volume;
        // source.pitch = pitch;
        // source.Play();
        // }
        // Destroy(go, clip.length);
        // return source;
        // 
    }*/


    #endregion
}
