using System;
using UnityEngine;

namespace _Scripts.Core
{
public class Money : MonoBehaviour
{
    [ SerializeField ] private MoneyUi _ui;

    [ SerializeField ] private double _currency = 100;
    public double Currency { get => _currency; 
        set => _currency = value; }

    public static Money Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        
    }
}
}