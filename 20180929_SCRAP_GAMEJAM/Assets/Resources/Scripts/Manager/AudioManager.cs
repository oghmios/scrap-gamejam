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

    public AudioClip Shoot;
	public AudioClip impactBoss;
	public AudioClip impactShot;
	public AudioClip bossExplosion;
	public AudioClip bossIdle;
	public AudioClip jumpPlayer;
	public AudioClip damagePlayer;
	public AudioClip destroyPlayer;
	public AudioClip teleportPlayer;
	public AudioClip catchPieceBoss;
	public AudioClip shotBulletBoss;
	public AudioClip enemyExplosion;
    public AudioClip playerThrow;

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
            /*
			//Create an empty game object
            GameObject go = new GameObject("Audio: " + clip.name);
            go.transform.position = emitter.position;
            go.transform.parent = emitter;

            //Create the source
            AudioSource source = go.AddComponent<AudioSource>();
			if(PlayerPrefs.GetInt("Sound")==1){
			source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = true;
            source.Play();
			}
            if (!loop)
                Destroy(go, loopTime);
            return source;
            */
        }

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
			/*
            //Create an empty game object
            GameObject go = new GameObject("Audio: " + clip.name);
            go.transform.position = point;

            //Create the source
            AudioSource source = go.AddComponent<AudioSource>();
			if(PlayerPrefs.GetInt("Sound")==1){
			source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
			}
            Destroy(go, clip.length);
            return source;
            */
        }
        /*******************************************************************************************/

    #endregion
}
