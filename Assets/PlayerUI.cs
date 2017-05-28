using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private RectTransform _thrusterFuelFill;

    private PlayerController _controller;

    void Update()
    {
        SetFuelAmount(_controller.GetThrusterFuelAmount());
    }

    public void SetController(PlayerController controller)
    {
        _controller = controller;
    }

    private void SetFuelAmount(float amount)
    {
        _thrusterFuelFill.localScale = new Vector3(1, amount, 1);
    }
}
