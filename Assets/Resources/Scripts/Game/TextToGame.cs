using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TextToGame : MonoBehaviour{
	
	public Vector2 gridSize;
	public List<Word> across = new List<Word>();
	public List<Word> down = new List<Word>();
	public Transform puzzleGUI;
	public Transform acrossGUI;
	public Transform downGUI;
	
    void Start (){
		
        //FileInfo theSourceFile = new FileInfo (Application.dataPath+"/Resources/LevelData/"+Application.loadedLevelName+".txt");
        TextAsset file = (TextAsset)Resources.Load("LevelData/"+Application.loadedLevelName, typeof(TextAsset));
		StringReader reader = new StringReader(file.text);
		
		// get size of grid and word counts
		gridSize = new Vector2(int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine()));
		SendMessage("GridSize",gridSize);
		
		//position main camera
		//Camera.mainCamera.transform.position = new Vector3(gridSize.x/2f-.5f, -gridSize.y/2+.5f, -50f);
		
		//read puzzles words into Word objects
		Object wordResource = Resources.Load("Objects/Word");
        while(reader.Peek()!=-1){
			int wordIndex = int.Parse(reader.ReadLine());
			Vector3 wordPosition = new Vector3((int.Parse(reader.ReadLine())+31f-gridSize.x)*24f, (-int.Parse(reader.ReadLine())+gridSize.y-8f)*24f,0f);
			string wordDirection = reader.ReadLine();
            string wordAnswer = reader.ReadLine();
			string wordClue = wordIndex + ". " + reader.ReadLine();
			
			GameObject wordPrefab = Instantiate(wordResource,puzzleGUI.position+wordPosition,Quaternion.identity) as GameObject;
			wordPrefab.transform.parent = puzzleGUI;
			
			Word word = wordPrefab.GetComponent<Word>();
			word.Init(wordIndex,wordPosition,wordDirection,wordAnswer,wordClue);
			
			//sort clues
			if(wordDirection.CompareTo("across")==0)
			{
				across.Add(word);
				for(int i = across.Count-1; i > 0; i --)
				{
					if(across[i].index< across[i-1].index)
					{
						Word temp = across[i];
						across[i] = across[i-1];
						across[i-1] = temp;
					}
				}
			}
			else
			{
				down.Add(word);
				for(int i = down.Count-1; i > 0; i --)
				{
					if(down[i].index< down[i-1].index)
					{
						Word temp = down[i];
						down[i] = down[i-1];
						down[i-1] = temp;
					}
				}
			}
		}
		reader.Close();
		
		//generate clues
		float cluePosition = 0;
		Object clueResource = Resources.Load("Objects/Clue");
		Clue clue;
		clue = (Instantiate(clueResource,acrossGUI.position+cluePosition*Vector3.up, Quaternion.identity) as GameObject).GetComponent<Clue>();
		clue.transform.parent = puzzleGUI;
		cluePosition -= clue.Init("Across",null);
		for(int i = 0; i < across.Count; i ++)
		{
			clue = (Instantiate(clueResource,acrossGUI.position+cluePosition*Vector3.up, Quaternion.identity) as GameObject).GetComponent<Clue>();
			float length = clue.Init(across[i].clue,across[i]);
			cluePosition -= length;
			BoxCollider collider = clue.gameObject.AddComponent<BoxCollider>();
			collider.center = new Vector3(3130f,-length*10f+20f,0f)*0.002604167f;
			collider.size = new Vector3(6260f,length*20f-40f,1f)*0.002604167f;
			clue.transform.parent = across[i].transform;
		}
		cluePosition = 0;
		clue = (Instantiate(clueResource,downGUI.position+cluePosition*Vector3.up, Quaternion.identity) as GameObject).GetComponent<Clue>();
		clue.transform.parent = puzzleGUI;
		cluePosition -= clue.Init("Down",null);
		for(int i = 0; i < down.Count; i ++)
		{
			clue = (Instantiate(clueResource,downGUI.position+cluePosition*Vector3.up, Quaternion.identity) as GameObject).GetComponent<Clue>();
			float length = clue.Init(down[i].clue,down[i]);
			cluePosition -= length;
			BoxCollider collider = clue.gameObject.AddComponent<BoxCollider>();
			collider.center = new Vector3(3130f,-length*10f+20f,0f)*0.002604167f;
			collider.size = new Vector3(6260f,length*20f-40f,1f)*0.002604167f;
			clue.transform.parent = down[i].transform;
		}
		
    }
}