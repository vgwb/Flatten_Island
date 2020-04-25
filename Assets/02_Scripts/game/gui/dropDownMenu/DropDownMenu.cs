using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class DropDownMenu : MonoBehaviour
{
	public Button showMenuButton;
	public Sprite firstButtonBackground;
	public Sprite middleButtonBackground;
	public Sprite lastButtonBackground;
	public GridLayoutGroup gridLayoutGroup;

	private void Awake()
	{
		OnAwake();
	}

	protected virtual void OnAwake()
	{

	}

	private void Start()
	{
		OnStart();
	}

	protected virtual void OnStart()
	{

	}

	private void OnDestroy()
	{
		GameObjectUtils.instance.DestroyAllChildren(gridLayoutGroup.transform);
	}
}