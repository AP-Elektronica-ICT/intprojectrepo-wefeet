﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    public void PlayDiscoGame()
    {
        NavBuffer.BufferSceneToLoad = "Disco";
        
        //SceneManager.LoadScene("Disco",LoadSceneMode.Single); // Will be loaded after choosing song        
    }

    public void PlayCinemaGame()
    {
        NavBuffer.BufferSceneToLoad = "Assets/Scenes/Dance_Cinema/Dance_Cinema.unity";
        //SceneManager.LoadScene("Assets/Scenes/Dance_Cinema/Dance_Cinema.unity", LoadSceneMode.Single); // Will be loaded after choosing song
    }

    public void PlaySong1()
    {
        NavBuffer.SongToLoad = "chacha";
        MatInputController.CloseTheConnection();
        SceneManager.LoadScene(NavBuffer.BufferSceneToLoad, LoadSceneMode.Single);
    }

    public void PlaySong2()
    {
        NavBuffer.SongToLoad = "danceMonkey";
        MatInputController.CloseTheConnection();
        SceneManager.LoadScene(NavBuffer.BufferSceneToLoad, LoadSceneMode.Single);
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!");

        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game

        MatInputController.CloseTheConnection();
        Application.Quit();
        EditorApplication.isPlaying = false;

    }
}
