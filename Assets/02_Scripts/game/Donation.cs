using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donation : MonoBehaviour
{
    public void CallForDonation()
    {
        
        Application.OpenURL("https://vgwb.org/contact/");
    }

   
}
