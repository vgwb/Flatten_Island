using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShowCheatsButton : MonoBehaviour
{
	public void OnClick()
	{
#if CHEAT_DEBUG
		CheatManager.instance.ShowCheatMenu();
#endif
	}
}

