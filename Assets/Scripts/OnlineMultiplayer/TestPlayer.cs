using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class TestPlayer : MonoBehaviour
{
    private Camera _main;
    private PhotonView _view;
    private void Awake()
    {
        _main = Camera.main;
        _view = GetComponent<PhotonView>();
    }

    public void Update()
    {
        if (_view.IsMine)
        {
            if (Pointer.current.IsPressed(1))
            {
                Vector2 position = _main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
                transform.position = position;
            }
        }
        
    }

}
