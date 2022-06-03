using UnityEngine;
using TMPro;
using Photon.Pun;

public class UsernameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;

    public void SaveUsername()
    {
        PlayerPrefs.SetString("Username", _input.text);
        PhotonNetwork.NickName = _input.text;
    }

    private void Start()
    {
        LoadUsername();
    }

    private void LoadUsername()
    {
        string username = PlayerPrefs.GetString("Username");
        if (username == null)
            return;
        _input.text = username;
        PhotonNetwork.NickName = username;
    }
}
