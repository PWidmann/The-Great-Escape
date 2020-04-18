using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicInfoDisplay : MonoBehaviour
{
    [SerializeField] GameObject audioPanel;

    public List<Music> musicPlaylist;

    public Text authorName;
    public Text titleName;
    public Text urlLink;

    int counter = 0;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        if (!SoundManager.instance.backGroundMusicSource.isPlaying)
            UpdateMusicPanel();
    }

    void UpdateMusicPanel()
    {
        Debug.Log("Toggle Enabled? " + UIManagement.instance.randomMusicPlayToggle.isOn);
        if (counter == musicPlaylist.Count && !UIManagement.instance.randomMusicPlayToggle.isOn)
            counter = 0;

        if (UIManagement.instance.randomMusicPlayToggle.isOn)
            counter = Random.Range(0, musicPlaylist.Count);

        SoundManager.instance.backGroundMusicSource.clip = musicPlaylist[counter].audioFile;
        SoundManager.instance.backGroundMusicSource.Play();

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
