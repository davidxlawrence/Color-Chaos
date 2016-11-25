using UnityEngine;
using System.Collections;

public class MoveNodeManager : MonoBehaviour {

	public PlayerCharacter player;
	public AudioClip selectSfx;

	void Start () 
	{
		m_moveNodes = GetComponentsInChildren<CharacterMoveNode>();
		gameState.GameStarted += OnGameStarted;
	}

	void Update()
	{
		if (!gameState.IsPlayingGame && !gameState.IsPreGame) return;

#if UNITY_EDITOR
		if(Input.GetMouseButton(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) 
			{
				CharacterMoveNode c = hit.collider.gameObject.GetComponent<CharacterMoveNode>();
				if(c != null && c != m_currentSelectedNode)
				{
					if (gameState.IsPreGame)
					{
						gameState.StartGame();
					}

					AudioManager.GetInstance().PlayOneShot(selectSfx);
					m_currentSelectedNode = c;
					player.MovementTarget = c;

					SetMoveNodeColors();
				}
			}
		}
#else
		if(Input.touchCount == 1)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			if (Physics.Raycast(ray, out hit)) 
			{
				CharacterMoveNode c = hit.collider.gameObject.GetComponent<CharacterMoveNode>();
				if(c != null && c != m_currentSelectedNode)
				{
					if (gameState.IsPreGame)
					{
						gameState.StartGame();
					}

					AudioManager.GetInstance().PlayOneShot(selectSfx);
					m_currentSelectedNode = c;
					player.MovementTarget = c;

					SetMoveNodeColors();
				}
			}
		}
#endif
	}

	private void SetMoveNodeColors()
	{
		foreach(CharacterMoveNode moveNode in m_moveNodes)
		{
			if(moveNode != m_currentSelectedNode)
			{
				moveNode.Background.color = Color.black;
			}
			else
			{
				AudioManager.GetInstance().PlayOneShot(selectSfx);
				moveNode.Background.color = Color.white;
			}
		}
	}

	private void OnGameStarted()
	{
		m_currentSelectedNode = null;
		SetMoveNodeColors();
	}

	private GameStateManager gameState
	{
		get {
			if(m_gameState == null)
			{
				GameObject g = GameObject.FindGameObjectWithTag("GameController");
				if(g != null)
				{
					m_gameState = g.GetComponent<GameStateManager>();
				}
			}
			
			return m_gameState;
		}
	}
	
	private GameStateManager m_gameState;
	private CharacterMoveNode m_currentSelectedNode;
	private CharacterMoveNode[] m_moveNodes;
}
