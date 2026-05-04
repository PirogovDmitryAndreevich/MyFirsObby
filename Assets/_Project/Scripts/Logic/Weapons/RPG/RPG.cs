using System;
using UnityEngine;

public class RPG : WeaponBase
{
    [SerializeField] private Transform _rocketModel;

    public override void Shoot(Vector3 direction)
    {
        if (!CanShoot)
            return;

        ReloadFinished += OnReloadFinished;
        _rocketModel.gameObject.SetActive(false);

        CreateBullets(direction);
    }

    private void OnReloadFinished()
    {
        ReloadFinished -= OnReloadFinished;
        _rocketModel.gameObject.SetActive(true);
    }
}
