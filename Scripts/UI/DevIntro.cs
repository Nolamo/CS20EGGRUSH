using UnityEngine;
using System.IO;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DevIntro : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;
    public string videoName;
    public float waitTime;

    void Start()
    {
        GameObject cam = GameObject.FindWithTag("MainCamera");
        if(!videoPlayer)
        {
            videoPlayer = cam.AddComponent<UnityEngine.Video.VideoPlayer>();
        }

        // Obtain the location of the video clip.
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, videoName);
        print("Video URL found " + videoPlayer.url);
        videoPlayer.Prepare();
        videoPlayer.Play();
    }

    // Checks for Pause Button being pressed, if so it skips the intro
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if ((videoPlayer.frame) > 0 && (videoPlayer.isPlaying == false))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}