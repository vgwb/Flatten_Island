using System;
using UnityEngine;

[Serializable]
public class UIAnimation
{
	public Animation animation = null;
	public string animationName = null;
	public float speed = 1.0f;
	public float delay = 0.0f;
	public bool ignoreTimeScale = false;
	public WrapMode wrapMode = WrapMode.Once;
	
	public bool isPlaying { get; private set; }
	
	private float progressionTime = 0.0f;
	private float timeAtLastFrame = 0.0f;
	private float deltaTime = 0.0f;
	private float delayTimer = 0.0f;
	
	private AnimationState currentAnimationState;
	
	public void Update()
	{
		if (isPlaying)
		{
			//Debug.Log("UIAnimation - " + animationName + " - deltaTime:" + deltaTime + " - progressionTime:" + progressionTime + " - currentAnimationState.length:" + currentAnimationState.length);
			
			if (ignoreTimeScale)
			{
				deltaTime = Time.realtimeSinceStartup - timeAtLastFrame;
				timeAtLastFrame = Time.realtimeSinceStartup;
			}
			else
			{
				deltaTime = Time.deltaTime;
			}
			
			if (delayTimer <= 0.0f)
			{
				progressionTime += (deltaTime * speed);
				
				currentAnimationState.enabled = true;
				currentAnimationState.time = progressionTime / currentAnimationState.length;
				animation.Sample();

				//Need to disable after a Sample() otherwise it will not work correctly
				currentAnimationState.enabled = false; 

				if (speed > 0.0f)
				{
					//Playing forward
					if (progressionTime >= currentAnimationState.length)
					{
						if(currentAnimationState.wrapMode != WrapMode.Loop)
						{
							isPlaying = false;
						}
						else
						{
							progressionTime = 0.0f;
						}
					}
				}
				else
				{
					//Playing backward
					if (progressionTime <= 0)
					{
						if(currentAnimationState.wrapMode != WrapMode.Loop)
						{
							isPlaying = false;
						}
						else
						{
							progressionTime = currentAnimationState.length;
						}
					}
				}			
			}else
			{
				delayTimer -= deltaTime;
			}
		}
	}
	
	public void Play()
	{
		if (animation != null)
		{
			//Debug.Log("UIAnimation - Starting animation:" + animation.ToString() + " ( " + animationName + " ) ");
			isPlaying = true;
			
			currentAnimationState = animation[animationName];
			timeAtLastFrame = 0.0f;
			deltaTime = 0.0f;
			delayTimer = delay;
			
			if (speed < 0.0f)
			{
				progressionTime = currentAnimationState.length;
			}
			else
			{
				progressionTime = 0.0f;
			}
			
			animation.Play(animationName);

			currentAnimationState.wrapMode = wrapMode;
            currentAnimationState.enabled = true;
			currentAnimationState.time = progressionTime / currentAnimationState.length;
			animation.Sample();
			currentAnimationState.enabled = false;
			
			timeAtLastFrame = Time.realtimeSinceStartup;
		}
		else
		{
			//Debug.Log("UIAnimation:" + this.animationName + " - m_Animation is null!");
		}
	}

	public void Stop()
	{
		if (animation != null)
		{
			isPlaying = false;
			animation.Stop(animationName);
		}
	}
	public void Finish()
	{
		if (animation != null)
		{
			isPlaying = false;
			currentAnimationState.enabled = true;
			currentAnimationState.time = 1.0f;
			animation.Sample();
			currentAnimationState.enabled = false;
			animation.Stop(animationName);
		}
	}
}
