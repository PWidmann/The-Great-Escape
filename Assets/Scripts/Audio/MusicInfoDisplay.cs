using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicInfoDisplay : MonoBehaviour
{
    [SerializeField] GameObject audioPanel;
    [SerializeField] Toggle randomMusicPlayToogle; // the same as in UIManager, just to make sure to have the reference to the toggle between all scenes. 

    public List<Music> musicPlaylist;

    public Text authorName;
    public Text titleName;
    public Text urlLink;

    int counter = 0;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ActiveScene: " + SceneManager.GetActiveScene().name);
        if (!SoundManager.instance.backGroundMusicSource.isPlaying)
            UpdateMusicPanel();
        if (SceneManager.GetActiveScene().name.Equals("Pre Game"))
            UpdateMusicPanel();
        if (SceneManager.GetActiveScene().name.Equals("The Great Escape"))
            UpdateMusicPanel();
    }

    void UpdateMusicPanel()
    {
        Debug.Log("Toggle Enabled? " + randomMusicPlayToogle.isOn);
        Debug.Log("Counter: " + counter);
        if (counter == musicPlaylist.Count && !randomMusicPlayToogle.isOn)
            counter = 0;

        if (randomMusicPlayToogle.isOn)
            counter = Random.Range(0, musicPlaylist.Count);

        if (!SoundManager.instance.backGroundMusicSource.isPlaying)
        {
            SoundManager.instance.backGroundMusicSource.clip = musicPlaylist[counter].audioFile;
            SoundManager.instance.backGroundMusicSource.Play();
        }

        authorName.text = musicPlaylist[counter].authorName;
        titleName.text = musicPlaylist[counter].titleName;
        urlLink.text = musicPlaylist[counter].webUrl;

        timer = SoundManager.instance.backGroundMusicSource.clip.length;
        counter++;

        StartCoroutine(EnableAudioPanel());
        StartCoroutine(DisableAudioPanel());

    }

    IEnumerator EnableAudioPanel()
    {
        yield return new WaitForSeconds(timer);
        audioPanel.SetActive(true);
        UpdateMusicPanel();
    }

    IEnumerator DisableAudioPanel()
    {
        yield return new WaitForSeconds(5f);
        audioPanel.SetActive(false);
    }
}
